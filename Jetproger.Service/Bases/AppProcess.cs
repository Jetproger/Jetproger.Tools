using System;
using System.IO;
using System.Reflection;

namespace Jetproger.Service.Bases
{
    public static class AppProcess
    {
        private static MethodInfo MainMethod => GetOne(_mainHolder, () => Provider.GetType().GetMethod("MainExecute", BindingFlags.Instance | BindingFlags.Public));
        private static readonly MethodInfo[] _mainHolder = { null };

        private static MethodInfo BaseMethod => GetOne(_baseHolder, () => Provider.GetType().GetMethod("BaseExecute", BindingFlags.Instance | BindingFlags.Public));
        private static readonly MethodInfo[] _baseHolder = { null };

        private static MethodInfo HttpMethod => GetOne(_httpHolder, () => Provider.GetType().GetMethod("HttpExecute", BindingFlags.Instance | BindingFlags.Public));
        private static readonly MethodInfo[] _httpHolder = { null };

        private static MethodInfo PingMethod => GetOne(_pingHolder, () => Provider.GetType().GetMethod("PingExecute", BindingFlags.Instance | BindingFlags.Public));
        private static readonly MethodInfo[] _pingHolder = { null };

        private static object Provider => GetOne(_providerHolder, CreateProvider);
        private static readonly object[] _providerHolder = { null };

        public static void StartCommands() { _stop = false; }
        public static void StopCommands() { _stop = true; }
        private static AppDomain _domain;
        private static bool _stop;

        public static bool MainExecute(string[] args)
        {
            if (_stop) return false;
            try
            {
                return (bool)MainMethod.Invoke(Provider, new object[] { f.exe.name, args });
            }
            catch (Exception e)
            {
                ErrorProcessing(e);
                return true;
            }
        }

        public static void BaseExecute()
        {
            if (_stop) return;
            try
            {
                BaseMethod.Invoke(Provider, new object[] { f.exe.name });
            }
            catch (Exception e)
            {
                ErrorProcessing(e);
            }
        }

        public static string HttpExecute(string request)
        {
            if (_stop) return null;
            try
            {
                return (string)HttpMethod.Invoke(Provider, new object[] { request });
            }
            catch (Exception e)
            {
                ErrorProcessing(e);
                return string.Empty;
            }
        }

        private static bool PingExecute()
        {
            try
            {
                return (bool)PingMethod.Invoke(Provider, new object[0]);
            }
            catch
            {
                return false;
            }
        }

        public static void Reset()
        {
            lock (_providerHolder)
            {
                TryUnloadDomain();
                _providerHolder[0] = null;
                _baseHolder[0] = null;
                _httpHolder[0] = null;
                _pingHolder[0] = null;
                _mainHolder[0] = null;
            }
        }

        private static void TryUnloadDomain()
        {
            try
            {
                if (_domain != null) AppDomain.Unload(_domain);
                _domain = null;
            }
            catch
            {
                _domain = null;
            }
        }

        private static void ErrorProcessing(Exception e)
        {
            f.log.Error(e);
            if (!PingExecute()) Reset();
        }

        private static object CreateProvider()
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true",
            };
            var name = $"APPDOMAIN{Guid.NewGuid().ToString().Replace("-", "")}".ToUpper();
            _domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            return _domain.CreateInstanceFromAndUnwrap(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Jetproger.Tools.Convert.dll"), "Jetproger.Tools.Convert.Commanders.CommandProvider");
        }

        private static T GetOne<T>(T[] holder, Func<T> factory) where T : class
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return holder[0];
        }
    }
}