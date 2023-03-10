using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading; 
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public class SafeCommandController : MarshalByRefObject
    {
        private static readonly ConcurrentDictionary<string, _CommandController> BusyControllers = new ConcurrentDictionary<string, _CommandController>();
        private static readonly ConcurrentBag<_CommandController> FreeControllers = new ConcurrentBag<_CommandController>();
        public CommandResponse Execute(CommandRequest request) { return _commandController.Execute(request); }
        public int GetCommandCount() { return f.cmd.ServerCommander.CommandCount; }
        private readonly CommandController _commandController = new CommandController();
        public string GetState() { return _state; }
        private static Thread _threadCheck;
        private static string _state;

        public SafeCommandController()
        {
            AppDomain.CurrentDomain.UnhandledException -= DomainUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;
        }

        public static string GetContainerId(CommandRequest request)
        {
            TryStartCheckControllers();
            _CommandController domainController;
            domainController = FreeControllers.TryTake(out domainController) ? domainController : CreateController();
            BusyAdd(domainController);
            return domainController.Domain.FriendlyName;
        }

        public static CommandResponse Execute(string containerId, CommandRequest request)
        {
            var response = new CommandResponse { Session = request.Session };
            try
            {
                _CommandController domainController;
                if (!BusyControllers.TryGetValue(containerId, out domainController)) throw new ContainerNotFoundException(containerId);
                return Execute(domainController, request, response);
            }
            catch (Exception e)
            {
                f.log(e);
                response.Result = string.Empty;
                response.Report = e.As<CommandMessage[]>();
                return response;
            }

        }

        private static CommandResponse Execute(_CommandController domainController, CommandRequest request, CommandResponse response)
        {
            try
            {
                return domainController.Controller.Execute(request);
            }
            catch
            {
                CloseContainer(domainController);
                response.Result = null;
                response.Report = (new CommandMessage(nameof(ContainerNotFoundException), ECommandMessage.Warn).As<CommandMessage[]>());
                return response;
            }
        }

        private static _CommandController CreateController()
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true",
            };
            var name = string.Format("CONTAINERCOMMAND{0}", Guid.NewGuid().ToString().Replace("-", "")).ToUpper();
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var controller = domain.CreateInstanceAndUnwrap("Jetproger.Tools.Convert", "Jetproger.Tools.Convert.Commanders.SafeCommandController") as SafeCommandController;
            return new _CommandController(controller, domain);
        }

        private static void TryStartCheckControllers()
        {
            if (!AppDomain.CurrentDomain.FriendlyName.StartsWith("CONTAINERCOMMAND")) _threadCheck = _threadCheck ?? f.sys.threadof(TryCheckControllers, 5555);
        }

        private static void TryCheckControllers()
        {
            try { CheckControllers(); }
            catch (Exception e) { f.log(e); }
        }

        private static void CheckControllers()
        {
            var reverts = new List<_CommandController>();
            var unloads = new List<_CommandController>();
            foreach (_CommandController item in BusyControllers.Values)
            {
                var state = GetDomainState(item.Controller);
                if (!string.IsNullOrWhiteSpace(state)) unloads.Add(item);
                var commandCount = GetDomainCommandCount(item.Controller);
                if (commandCount == -1) unloads.Add(item);
                if (commandCount == 0) reverts.Add(item);
            }
            foreach (_CommandController item in unloads) CloseContainer(item);
            foreach (_CommandController item in reverts) RevertContainer(item);
        }

        private static void CloseContainer(_CommandController domainController)
        {
            try
            {
                BusyRemove(domainController);
                AppDomain.Unload(domainController.Domain);
            }
            catch (Exception e)
            {
                f.log(e);
            }
        }

        private static void RevertContainer(_CommandController domainController)
        {
            try
            {
                BusyRemove(domainController);
                FreeControllers.Add(domainController);
            }
            catch (Exception e)
            {
                f.log(e);
            }
        }

        private static string GetDomainState(SafeCommandController controller)
        {
            try
            {
                return controller.GetState();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private static int GetDomainCommandCount(SafeCommandController controller)
        {
            try
            {
                return controller.GetCommandCount();
            }
            catch
            {
                return -1;
            }
        }

        private static void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _state = e.ExceptionObject != null ? e.ExceptionObject.ToString() : null;
        }

        private static void BusyAdd(_CommandController domainController)
        {
            if (!BusyControllers.ContainsKey(domainController.Domain.FriendlyName)) BusyControllers.TryAdd(domainController.Domain.FriendlyName, domainController);
        }

        private static void BusyRemove(_CommandController domainController)
        {
            if (BusyControllers.ContainsKey(domainController.Domain.FriendlyName)) BusyControllers.TryRemove(domainController.Domain.FriendlyName, out domainController);
        }

        private class _CommandController
        {
            public _CommandController(SafeCommandController controller, AppDomain domain) { Controller = controller; Domain = domain; }
            public readonly SafeCommandController Controller;
            public readonly AppDomain Domain;
        }
    }

    public class CommandController
    {
        public CommandResponse Execute(CommandRequest request)
        {
            try
            {
                return f.cmd.ServerEnqueue(request);
            }
            catch (Exception e)
            {
                f.log(e);
                return new CommandResponse { Session = request.Session, Result = string.Empty, Report = e.As<CommandMessage[]>() };
            }
        }
    }
}