using System.Windows.Input;

namespace SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels.Interfaces;

public interface IMainWindowViewModel
{
    int MaxWidth { get; set; }
    double MaxWaste { get; set; }
    int GroupSize { get; set; }
    string WorkbookFilename { get; set; }

    ICommand GeneratePatternLayouts { get; set; }
    ICommand IncrementGroupSize { get; set; }
    ICommand DecrementGroupSize { get; set; }
    ICommand SelectWorkbookFilepath { get; set; }
}
