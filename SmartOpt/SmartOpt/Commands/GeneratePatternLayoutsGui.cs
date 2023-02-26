using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;
using Microsoft.Win32;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.Commands;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.Views;

namespace SmartOpt;

public partial class Application
{
    private void GeneratePatternLayoutsGui(
        IPatternLayoutService patternLayoutService,
        IReportExporter reportExporter)
    {
        var viewModel = new MainWindowViewModel
        {
            WorkbookFilename = Path.GetFileName(_applicationState.ExcelBookFilepath!) ?? "Активная книга"
        };
        var window = new MainWindow
        {
            DataContext = viewModel
        };

        // ReSharper disable once AsyncVoidLambda
        viewModel.GeneratePatternLayouts = new RelayCommand(_ =>
            {
                _applicationState.SetMaxWidth(viewModel, viewModel.AvailableRange.Width);
                _applicationState.SetMinWaste(viewModel, viewModel.AvailableRange.MinWastePercent);
                _applicationState.SetMaxWaste(viewModel, viewModel.AvailableRange.MaxWastePercent);
                _applicationState.SetGroupSize(viewModel, viewModel.GroupSize);

                viewModel.BusyIndicatorManager.Show(1, "Обрабатываем...");
                viewModel.Effect = new BlurEffect();
                
                try
                {
                    GeneratePatternLayoutsNoGui(patternLayoutService, reportExporter);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                        "An unexpected error was occured",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    viewModel.BusyIndicatorManager.Close(1);
                    viewModel.Effect = null;
                }
            },
            CanExecuteGeneratePattenLayoutsCommand(viewModel));
        
        viewModel.SelectWorkbookFilepath = new RelayCommand(_ =>
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != null && result.Value)
            {
                viewModel.WorkbookFilename = Path.GetFileName(dialog.FileName);
                _applicationState.SetExcelWorkbookFilepath(viewModel, dialog.FileName);
            }
        });
        viewModel.IncrementGroupSize = new RelayCommand(_ => viewModel.GroupSize++);
        viewModel.DecrementGroupSize = new RelayCommand(_ => viewModel.GroupSize--);

        window.ShowDialog();
    }

    private Func<object, bool> CanExecuteGeneratePattenLayoutsCommand(IMainWindowViewModel viewModel) =>
        _ => viewModel.AvailableRange.Width >= 0 && viewModel.AvailableRange.MinWastePercent > 0 && viewModel.GroupSize > 0;
}
