using System.Configuration.Install;
using System.ServiceProcess;
using Jetproger.Tools.Process.Bases;

namespace Jetproger.Tools.Process.Services
{
    public abstract class ServiceMounter : Installer
    {
        protected ServiceMounter()
        {
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };
            var baseInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = Methods.ConfigAsString("ServiceName", "Jetproger.Tools.Service"),
                DisplayName = Methods.ConfigAsString("ServiceName", "Jetproger.Tools.Service"),
                Description = Methods.ConfigAsString("ServiceName", "Jetproger.Tools.Service"),
            };
            Installers.Add(baseInstaller);
            Installers.Add(processInstaller);
        }
    }
}