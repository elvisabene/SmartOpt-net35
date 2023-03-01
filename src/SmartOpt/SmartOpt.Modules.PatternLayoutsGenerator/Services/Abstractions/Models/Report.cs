using System.Collections.Generic;
using System.Linq;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models
{
    public class Report
    {
        private readonly List<PatternLayout> patternLayouts;
        private readonly List<OrderInfo> ungroupedOrders;

        public Report(IEnumerable<PatternLayout> patternLayouts, IEnumerable<OrderInfo> ungroupedOrders)
        {
            this.patternLayouts = patternLayouts.ToList();
            this.ungroupedOrders = ungroupedOrders.ToList();
        }

        public Report()
        {
            patternLayouts = new List<PatternLayout>();
            ungroupedOrders = new List<OrderInfo>();
        }

        public IEnumerable<PatternLayout> GetPatternLayouts() => patternLayouts;
        public IEnumerable<OrderInfo> GetUngroupedOrders() => ungroupedOrders;

        public void AddPatternLayout(PatternLayout patternLayout)
        {
            patternLayouts.Add(patternLayout);
        }

        public void AddUngroupedOrders(IEnumerable<OrderInfo> ungroupedOrders)
        {
            this.ungroupedOrders.AddRange(ungroupedOrders);
        }
    }
}
