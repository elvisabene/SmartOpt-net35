using Ninject.Syntax;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels;
using SmartOpt.Modules.PatternLayoutsGenerator.UI.ViewModels.Interfaces;

namespace SmartOpt.Modules.PatternLayoutsGenerator.UI;

public class PatternLayoutGeneratorUiModule : ModuleBase
{
    public override void ConfigureServices(IBindingRoot services)
    {
        services.Bind<IMainWindowViewModel>().To<MainWindowViewModel>().InTransientScope();
    }
}
