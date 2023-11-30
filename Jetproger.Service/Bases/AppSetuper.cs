using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace Jetproger.Service.Bases
{
    [RunInstaller(true)]
    public class AppSetuper : Installer
    {
        public AppSetuper()
        {
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem, Password = null, Username = null
            };
            var name = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).ToLower().Replace(".", "-");
            var baseInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = name,
                DisplayName = name,
                Description = name,
            };
            Installers.Add(baseInstaller);
            Installers.Add(processInstaller);
        }
    }
}