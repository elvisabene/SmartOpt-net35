using System;
using System.Collections;
using SmartOpt.Core.Extensions;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Implementation
{
    public class PatternLayoutGenerator : IPatternLayoutGenerator
    {
        public Report GeneratePatternLayoutsFromOrders(IEnumerable<OrderInfo> orderInfos,
            int maxWidth, double minWaste, double maxWaste, int groupSize)
        {
            orderInfos = AggregateOrdersWithIdenticalWidth(orderInfos);

            var orders = orderInfos as OrderInfo[] ?? orderInfos.ToArray();

            if (orders.Count() < groupSize)
            {
                groupSize = orders.Count();
            }

            var report = new Report();

            var allOrders = orders.ToList();

            var elementsForGroupingExist = true;
            
            while (elementsForGroupingExist)
            {
                allOrders.Sort((prev, next) => next.RollsCount.CompareTo(prev.RollsCount));

                var ordersGroup = allOrders.GetRange(0, groupSize);
                var ungroupedOrders = allOrders.GetRange(groupSize, allOrders.Count - groupSize);

                if (TryCreatePatternLayout(
                        allOrders, 
                        ungroupedOrders,
                        ordersGroup,
                        minWaste, maxWaste, maxWidth,
                        out var patternLayout,
                        out elementsForGroupingExist))
                {
                    allOrders = MergeSplitOrders(allOrders);
                    AddPatternLayoutToReport(report, patternLayout);
                }
            }

            allOrders = MergeSplitOrders(allOrders);
            AddRemainingOrdersToReport(report, allOrders);
            return report;
        }

        private static void AddPatternLayoutToReport(Report report, PatternLayout patternLayout)
        {
            report.AddPatternLayout(patternLayout);
        }

        private static void AddRemainingOrdersToReport(Report report, List<OrderInfo> remainingOrders)
        {
            // Implementation details. CreatePatternLayout method is "subtracting rolls count from the remaining orders"
            var patternLayout = CreatePatternLayout(remainingOrders, 100.0);
            report.AddUngroupedOrders(patternLayout.Orders);
        }

        private static bool TryCreatePatternLayout(
            List<OrderInfo> orders,
            List<OrderInfo> unprocessedOrders,
            List<OrderInfo> ordersGroup,
            double minWaste, double maxWaste, int maxWidth,
            out PatternLayout patternLayout, out bool elementsForGroupingExist)
        {
            patternLayout = null!;
            elementsForGroupingExist = true;

            var groupWaste = CalculateWasteForOrdersGroup(ordersGroup, maxWidth);
            
            var lastWasteHistory = new List<double> { groupWaste, };

            while (groupWaste > maxWaste || groupWaste < minWaste)
            {
                RemoveOldWaste(lastWasteHistory);
                
                if (IsWasteCycle(lastWasteHistory))
                {
                    elementsForGroupingExist = false;
                    
                    return false;
                }
                
                if (TryFindSuitableOrderIndexForReplacing(
                        ordersGroup,
                        unprocessedOrders,
                        groupWaste, maxWidth,
                        minWaste, maxWaste,
                        out var suitableOrderForReplacingIndex))
                {
                    (ordersGroup[0], unprocessedOrders[suitableOrderForReplacingIndex]) = (unprocessedOrders[suitableOrderForReplacingIndex], ordersGroup[0]);

                    groupWaste = CalculateWasteForOrdersGroup(ordersGroup, maxWidth);
                    lastWasteHistory.Add(groupWaste);
                }
                else
                {
                    if (TryFindSuitableOrderIndexForSplitting(orders, out int suitableOrderForSplittingIndex))
                    {
                        SplitOrderIntoTwo(orders, suitableOrderForSplittingIndex);
                    }
                    else
                    {
                        elementsForGroupingExist = false;
                    }

                    return false;
                }
            }

            patternLayout = CreatePatternLayout(ordersGroup, groupWaste);
            return true;
        }

        private static PatternLayout CreatePatternLayout(List<OrderInfo> orders, double waste)
        {
            var minGroupRollsCount = orders.Min(x => x.RollsCount);
            var ordersGroup = new List<OrderInfo>();
            orders.ForEach(order =>
            {
                ordersGroup.Add(order.Clone());
                order.RollsCount -= minGroupRollsCount;
            });

            return new PatternLayout(ordersGroup, minGroupRollsCount, waste);
        }

        private static List<OrderInfo> MergeSplitOrders(IEnumerable<OrderInfo> splitOrders)
        {
            return splitOrders
                .GroupBy(c => c.Width)
                .Select(group => new OrderInfo(group.First().Name, group.Key, group.Sum(ci => ci.RollsCount)))
                .ToList();
        }

        private static double CalculateWasteForOrdersGroup(IEnumerable<OrderInfo> ordersGroup, int maxWidth)
        {
            return (1 - (double)ordersGroup.Sum(order => order.Width) / maxWidth) * 100;
        }

        private static void SplitOrderIntoTwo(List<OrderInfo> orders, int orderForSplittingIndex)
        {
            var suitableOrder = orders[orderForSplittingIndex];

            suitableOrder.RollsCount /= 2;
            orders.Add(suitableOrder.Clone());
        }

        private static bool TryFindSuitableOrderIndexForSplitting(List<OrderInfo> orders, out int index)
        {
            index = 0;

            index = orders.FindIndex(x =>
                x.RollsCount >= 2 &&
                orders.Max(c => c.RollsCount).Equals7DigitPrecision(x.RollsCount));

            return index >= 0;
        }

        private static bool TryFindSuitableOrderIndexForReplacing(
            List<OrderInfo> ordersGroup,
            List<OrderInfo> unprocessedOrders,
            double groupWaste, int maxWidth,
            double minWaste, double maxWaste,
            out int index)
        {
            index = 0;

            if (groupWaste >= maxWaste)
            {
                ordersGroup.Sort((prev, next) =>
                    prev.Width.CompareTo(next.Width));

                index = unprocessedOrders.FindIndex(item =>
                    ordersGroup[0].Width < item.Width &&
                    item.Width - ordersGroup[0].Width < maxWidth * groupWaste / 100 &&
                    item.RollsCount >= 1);
            }
            else if (groupWaste < minWaste)
            {
                ordersGroup.Sort((prev, next) =>
                    next.Width.CompareTo(prev.Width));

                index = unprocessedOrders.FindIndex(item =>
                    ordersGroup[0].Width > item.Width &&
                    ordersGroup[0].Width - item.Width > maxWidth * groupWaste / 100 &&
                    item.RollsCount >= 1);
            }

            return index >= 0;
        }

        private static IEnumerable<OrderInfo> AggregateOrdersWithIdenticalWidth(IEnumerable<OrderInfo> orders)
        {
            var ordersArray = orders as OrderInfo[] ?? orders.ToArray();
            
            var elementWidths = ordersArray
                .Select(x => x.Width)
                .Distinct();

            var a = elementWidths
                .Select(currentWidth => ordersArray.Where(x => x.Width == currentWidth));
            
            var b = a.Select(tmp => tmp.Aggregate((prev, next) =>
                {
                    prev.Name += ", " + next.Name;
                    prev.RollsCount += next.RollsCount;
                    
                    return prev;
                }));

            return b;
        }

        private static void RemoveOldWaste(IList wasteHistory)
        {
            if (wasteHistory.Count == 4)
            {
                wasteHistory.RemoveAt(0);
            }
        }

        private static bool IsWasteCycle(IList<double> wasteHistory)
        {
            return wasteHistory.Count > 2 && Math.Abs(wasteHistory[0] - wasteHistory[2]) < 0.00005;
        }
    }
}
