using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using System.Collections.Generic;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces
{
    public interface IPatternLayoutGenerator
    {
        Report GeneratePatternLayoutsFromOrders(
            IEnumerable<OrderInfo> orders,
            int maxWidth,
            double minWaste, double maxWaste, int groupSize);
    }
}
