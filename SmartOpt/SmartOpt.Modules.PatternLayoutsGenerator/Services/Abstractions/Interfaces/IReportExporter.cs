using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces
{
    public interface IReportExporter
    {
        void ExportToNewExcelWorkbook(Report report);
    }
}
