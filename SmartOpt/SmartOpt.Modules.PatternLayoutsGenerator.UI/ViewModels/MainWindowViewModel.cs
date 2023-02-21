using System.Windows.Input;
using System.Windows.Media.Effects;
using SmartOpt.Core.Infrastructure.Models;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.Services;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels.Interfaces;

namespace SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private bool isInteractionAllowed = true;
    private int maxWidth = 6000;
    private double maxWaste = 1.4;
    private double minWaste = 0.8;
    private int groupSize = 5;
    private string workbookFilepath;

    private WidthRange availableWidth;

    public MainWindowViewModel()
    {
        availableWidth = new WidthRange(maxWaste, minWaste, maxWidth);
        BusyIndicatorManager = BusyIndicatorManager.Instance;
    }

    public WidthRange AvailableRange
    {
        get => availableWidth;
    }

    public bool IsInteractionAllowed
    {
        get => isInteractionAllowed;
        set
        {
            isInteractionAllowed = value;
            OnPropertyChanged(nameof(IsInteractionAllowed));
        }
    }
    
    public int MaxWidth
    {
        get => maxWidth;
        set
        {
            maxWidth = value;
            OnPropertyChanged(nameof(MaxWidth));
        }
    }

    public double MinWaste
    {
        get => minWaste;
        set
        {
            minWaste = value;
            OnPropertyChanged(nameof(MinWaste));
        }
    }

    public double MaxWaste
    {
        get => maxWaste;
        set
        {
            maxWaste = value;
            OnPropertyChanged(nameof(MaxWaste));
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

    private void UpdateWidth()
    {
        ma
    }

    public ICommand GeneratePatternLayouts { get; set; } = null!;
    public ICommand IncrementGroupSize { get; set; } = null!;
    public ICommand DecrementGroupSize { get; set; } = null!;
    public ICommand SelectWorkbookFilepath { get; set; } = null!;

    public BusyIndicatorManager BusyIndicatorManager { get; }
    
    public Effect Effect { get; set; }
}
