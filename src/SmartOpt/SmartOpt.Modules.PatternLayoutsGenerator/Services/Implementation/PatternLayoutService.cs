using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using System.Collections.Generic;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Implementation
{
    public class PatternLayoutService : IPatternLayoutService
    {
        private readonly IPatternLayoutGenerator patternLayoutGenerator;
        private readonly IOrderInfoParser orderInfoParser;

        public PatternLayoutService(
            IPatternLayoutGenerator patternLayoutGenerator,
            IOrderInfoParser orderInfoParser)
        {
            this.patternLayoutGenerator = patternLayoutGenerator;
            this.orderInfoParser = orderInfoParser;
        }

        public Report GeneratePatternLayoutsFromActiveExcelWorksheet(int maxWidth, double minWaste, double maxWaste, int groupSize, double coefficient)
        {
            var orders = orderInfoParser.ParseOrdersFromActiveExcelWorksheet(coefficient);

            return GeneratePatternLayoutsFromExcelWorksheetInternal(orders, maxWidth, minWaste, maxWaste, groupSize);
        }

        public Report GeneratePatternLayoutsFromExcelWorksheet(string workbookFilepath, int maxWidth, double minWaste, double maxWaste, int groupSize, double coefficient)
        {
            var orders = orderInfoParser.ParseOrdersFromExcelWorksheet(workbookFilepath, coefficient);

            return GeneratePatternLayoutsFromExcelWorksheetInternal(orders, maxWidth, minWaste, maxWaste, groupSize);
        }

        private Report GeneratePatternLayoutsFromExcelWorksheetInternal(
            IEnumerable<OrderInfo> orders, int maxWidth, double minWaste, double maxWaste, int groupSize)
        {
            var report = patternLayoutGenerator.GeneratePatternLayoutsFromOrders(
                orders, maxWidth, minWaste, maxWaste, groupSize);

            return report;
        }
    }
}
