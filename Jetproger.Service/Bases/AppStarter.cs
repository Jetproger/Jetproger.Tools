using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Jetproger.Service.Bases
{
    internal class AppStarter
    {
        private static void Main(string[] args)
        {
            f.keysof(args);
            if (f.iskey("i", "install"))
            {
                Install(Assembly.GetExecutingAssembly().Location);
                Console.ReadKey();
                return;
            }
            if (f.iskey("u", "uninstall"))
            {
                Uninstall(Assembly.GetExecutingAssembly().Location);
                Console.ReadKey();
                return;
            }
            if (AppMethods.Instance.MainExecute(args))
            {
                return;
            }
            var service = new AppService();
            if (Environment.UserInteractive)
            {
                Console.CancelKeyPress += (x, y) => service.Stop();
                service.Start();
                Console.WriteLine("Running service, press a key to stop");
                Console.ReadKey();
                service.Stop();
                Console.WriteLine("Service stopped. Goodbye.");
                return;
            }
            f.log.Trace("ServiceBase.Run...");
            ServiceBase.Run(new ServiceBase[] { service });
        }

        private static void Install(string pathToService)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { pathToService });
                Console.WriteLine("Success to install service");
            }
            catch
            {
                Console.WriteLine("Failed to install service");
            }
        }

        private static void Uninstall(string pathToService)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/u", pathToService });
                Console.WriteLine("Success to uninstall service");
            }
            catch
            {
                Console.WriteLine("Failed to uninstall service");
            }
        }
    }
}