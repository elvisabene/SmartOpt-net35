using System.Windows.Data;
using SmartOpt.Core.Infrastructure.Models;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.Services;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels.Interfaces;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private bool isInteractionAllowed = true;
    private int maxWidth = 6000;
    private double maxWaste = 1.4;
    private double minWaste = 0.8;
    private double leftLimit = 5916;
    private double rightLimit = 5952;
    private int groupSize = 5;
    private double coefficient = 0.17;
    private string workbookFilepath;

    private WidthRange availableWidth;

    public MainWindowViewModel()
    {
        availableWidth = new WidthRange(minWaste, maxWaste, maxWidth, rightLimit, leftLimit, coefficient);
        BusyIndicatorManager = BusyIndicatorManager.Instance;
        
        availableWidth.OnWasteChange += () =>
        {
            OnPropertyChanged(nameof(AvailableRange));
        };
        
        availableWidth.OnLimitChange += () =>
        {
            OnPropertyChanged(nameof(AvailableRange));
        };

        availableWidth.OnWidthChange += () =>
        {
            OnPropertyChanged(nameof(AvailableRange));
        };
    }

    public WidthRange AvailableRange => availableWidth;

    public bool IsInteractionAllowed
    {
        get => isInteractionAllowed;
        set
        {
            isInteractionAllowed = value;
            OnPropertyChanged(nameof(IsInteractionAllowed));
        }
    }

    public int GroupSize
    {
        get => groupSize;
        set
        {
            groupSize = value;
            OnPropertyChanged(nameof(GroupSize));
        }
    }

    public string WorkbookFilename
    {
        get => workbookFilepath ?? "Не указано";
        set
        {
            workbookFilepath = value;
            OnPropertyChanged(nameof(WorkbookFilename));
        }
    }

    public ICommand GeneratePatternLayouts { get; set; } = null!;
    public ICommand IncrementGroupSize { get; set; } = null!;
    public ICommand DecrementGroupSize { get; set; } = null!;
    public ICommand SelectWorkbookFilepath { get; set; } = null!;

    public BusyIndicatorManager BusyIndicatorManager { get; }

    public Effect Effect { get; set; }
}
