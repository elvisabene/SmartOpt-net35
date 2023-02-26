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
    private string workbookFilepath;

    private WidthRange availableWidth;

    public MainWindowViewModel()
    {
        availableWidth = new WidthRange(minWaste, maxWaste, maxWidth, rightLimit, leftLimit);
        BusyIndicatorManager = BusyIndicatorManager.Instance;
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

    public int MaxWidth
    {
        get => maxWidth;
        set
        {
            maxWidth = value;
            OnPropertyChanged(nameof(MaxWidth));
            UpdateWidthRangeForWidth();
        }
    }

    public double MinWaste
    {
        get => minWaste;
        set
        {
            minWaste = value;
            OnPropertyChanged(nameof(MinWaste));
            UpdateWidthRangeForWaste();
        }
    }

    public double MaxWaste
    {
        get => maxWaste;
        set
        {
            maxWaste = value;
            OnPropertyChanged(nameof(MaxWaste));
            UpdateWidthRangeForWaste();
        }
    }

    public double LeftLimit
    {
        get => leftLimit;
        set
        {
            leftLimit = value;
            OnPropertyChanged(nameof(LeftLimit));
            UpdateWidthRangeForLimit();
        }
    }

    public double RightLimit
    {
        get => rightLimit;
        set
        {
            rightLimit = value;
            OnPropertyChanged(nameof(RightLimit));
            UpdateWidthRangeForLimit();
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

    private void UpdateWidthRangeForWaste()
    {
        availableWidth.SetNewRangeForWaste(ref leftLimit, ref rightLimit, minWaste, maxWaste, maxWidth);
    }

    private void UpdateWidthRangeForLimit()
    {
        availableWidth.SetNewRangeForLimit(ref minWaste, ref maxWaste, leftLimit, rightLimit, maxWidth);
    }

    private void UpdateWidthRangeForWidth()
    {
        availableWidth.SetNewRangeForWidth(maxWidth, ref minWaste, ref maxWaste, ref leftLimit, ref rightLimit);
    }

    public ICommand GeneratePatternLayouts { get; set; } = null!;
    public ICommand IncrementGroupSize { get; set; } = null!;
    public ICommand DecrementGroupSize { get; set; } = null!;
    public ICommand SelectWorkbookFilepath { get; set; } = null!;

    public BusyIndicatorManager BusyIndicatorManager { get; }

    public Effect Effect { get; set; }
}
