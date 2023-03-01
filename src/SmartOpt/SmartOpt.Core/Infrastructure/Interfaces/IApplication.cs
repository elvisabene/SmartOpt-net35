using System;

namespace SmartOpt.Core.Infrastructure.Interfaces
{
    public interface IApplication
    {
        void Start();
        
        IServiceProvider GetServicesProvider();
    }
}
