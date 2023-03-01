using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;

namespace SmartOpt;

public partial class Application
{
    private void GeneratePatternLayoutsNoGui(
        IPatternLayoutService patternLayoutService,
        IReportExporter reportExporter,
        double coefficient)
    {
        Report report;
        if (_applicationState.ExcelBookFilepath != null)
        {
            report = patternLayoutService.GeneratePatternLayoutsFromExcelWorksheet(
                _applicationState.ExcelBookFilepath,
                _applicationState.MaxWidth!.Value,
                _applicationState.MinWaste!.Value,
                _applicationState.MaxWaste!.Value,
                _applicationState.GroupSize!.Value, 
                coefficient);
        }
        else
        {
            report = patternLayoutService.GeneratePatternLayoutsFromActiveExcelWorksheet(
                _applicationState.MaxWidth!.Value,
                _applicationState.MinWaste!.Value,
                _applicationState.MaxWaste!.Value,
                _applicationState.GroupSize!.Value,
                coefficient);
        }

        reportExporter.ExportToNewExcelWorkbook(report);
    }
}
