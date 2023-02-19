using System.Collections.Generic;
using System.Linq;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models
{
    public class PatternLayout
    {
        public PatternLayout(IEnumerable<OrderInfo> orders, double rollsCount, double waste)
        {
            Waste = waste;
            Orders = orders;
            RollsCount = rollsCount;
        }

        public IEnumerable<OrderInfo> Orders { get; }

        public double RollsCount { get; }

        public double Waste { get; }

        public override string ToString()
        {
            return $"Waste: {Waste}; RollsCount: {RollsCount}";
        }
    }
}
