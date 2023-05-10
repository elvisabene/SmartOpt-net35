using System.Collections.Generic;
using System.Linq;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models
{
    public class PatternLayout
    {
        private PatternLayout(IEnumerable<OrderInfo> orders, double rollsCount, double waste)
        {
            Waste = waste;
            Orders = orders;
            RollsCount = rollsCount;
        }

        public IEnumerable<OrderInfo> Orders { get; }

        public double RollsCount { get; }

        public double Waste { get; }

        public static PatternLayout Create(List<OrderInfo> orders, double waste)
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

        public override string ToString()
        {
            return $"Waste: {Waste}; RollsCount: {RollsCount}";
        }
    }
}
