using System.Windows.Input;
using SmartOpt.Core.Infrastructure.Models;

namespace SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels.Interfaces;

public interface IMainWindowViewModel
{
    int GroupSize { get; set; }
    string WorkbookFilename { get; set; }
    
    WidthRange AvailableRange { get; }

    ICommand GeneratePatternLayouts { get; set; }
    ICommand IncrementGroupSize { get; set; }
    ICommand DecrementGroupSize { get; set; }
    ICommand SelectWorkbookFilepath { get; set; }
}
