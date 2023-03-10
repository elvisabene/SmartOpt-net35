using System;
using Ninject.Syntax;

namespace SmartOpt.Modules
{
    public interface IModule
    {
        /// <summary>
        /// Configure services for the current modlet
        /// </summary>
        void ConfigureServices(IBindingRoot services);

        /// <summary>
        /// Initialize module in the current modlet
        /// </summary>
        /// <param name="provider"></param>
        void InitializeModule(IServiceProvider provider);
    }
}
