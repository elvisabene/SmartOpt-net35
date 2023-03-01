using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using System.Collections.Generic;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces
{
    public interface IOrderInfoParser
    {
        IEnumerable<OrderInfo> ParseOrdersFromActiveExcelWorksheet(double coefficient);

        // todo add worksheetId parameter. by default this method uses 0
        IEnumerable<OrderInfo> ParseOrdersFromExcelWorksheet(string workbookFilepath, double coefficient);
    }
}
