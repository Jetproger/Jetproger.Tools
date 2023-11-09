using System;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{ 
    public abstract class DomainCommand : DomainCommand<object, object, object, object, object, object, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T> : DomainCommand<T, T, object, object, object, object, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_> : DomainCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0> : DomainCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1> : DomainCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2> : DomainCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2, T3> : DomainCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2, T3, T4> : DomainCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2, T3, T4, T5> : DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : DomainCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { protected DomainCommand(string commandType) : base(commandType) { } }
    public abstract class DomainCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ShareCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private static readonly _DomainControllers Controllers = new _DomainControllers();
        protected DomainCommand(string commandType) : base(commandType) { }
        private _DomainController _controller;
        
        protected override ICommand GetExecuter()
        {
            _controller = _controller ?? Controllers.Get(this);
            return new _DomainCommand(_controller.Controller);
        }

        private class _DomainCommand : BaseCommand<CommandResponse, CommandRequest>, ICommand
        {
            public _DomainCommand(DomainController controller) { _controller = controller; }
            private readonly DomainController _controller;

            public void Execute()
            {
                try
                { 
                    if (State != ECommandState.None) return;
                    State = ECommandState.Running;
                    Result = _controller.Execute(Value);
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }

        #region inner types

        private class _DomainControllers
        {
            private readonly List<_DomainController> _list = new List<_DomainController>();
            private _DomainController Get(int index) { lock (_list) { return index < _list.Count ? _list[index] : null; } }
            private void Add(_DomainController item) { lock (_list) { _list.Add(item); } }
            private void Remove(int index) { lock (_list) { _list.RemoveAt(index); } }
            private int Count() { lock (_list) { return _list.Count; } }

            public _DomainController Get(ICommand command)
            {
                var controller = (_DomainController)null;
                for (int i = 0; i < Count();)
                {
                    if (i >= Count()) break;
                    var currentController = Get(i);
                    if (currentController == null) break;
                    var state = GetState(currentController);
                    if (state == ECommandState.Completed)
                    {
                        Remove(i);
                    }
                    else if (controller != null)
                    {
                        if (currentController.Command.State == ECommandState.Completed)
                        {
                            Remove(i);
                            t<AppDomain>.few(currentController.Domain);
                        }
                    }
                    else
                    {
                        controller = state == ECommandState.None ? currentController : null;
                        i++;
                    }
                } 
                controller = controller ?? Add(command);
                controller.Command = command;
                return controller;
            }

            private ECommandState GetState(_DomainController controller)
            {
                var domainError = GetDomainError(controller);
                if (!string.IsNullOrWhiteSpace(domainError))
                {
                    AppDomain.Unload(controller.Domain);
                    f.log(new CommandMessage { Message = domainError, Category = ECommandMessage.Error.ToString() });
                    return ECommandState.Completed;
                }
                return controller.Command.State != ECommandState.Running ? ECommandState.None : ECommandState.Running;
            }

            private string GetDomainError(_DomainController controller)
            {
                try
                {
                    return controller.Controller.GetState();
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }

            private _DomainController Add(ICommand command)
            {
                var domain = t<AppDomain>.few(CreateDomain);
                var controller = (DomainController)domain.CreateInstanceAndUnwrap("Jetproger.Tools.Convert", "Jetproger.Tools.Convert.Commanders.DomainController");
                controller.SetOwner(AppDomain.CurrentDomain);
                var item = new _DomainController { Controller = controller, Domain = domain, Command = command };
                Add(item);
                return item;
            }

            private AppDomain CreateDomain()
            {
                var setup = new AppDomainSetup
                {
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    LoaderOptimization = LoaderOptimization.SingleDomain,
                    ShadowCopyFiles = "true",
                };
                var name = $"{f.cmd.SYS_CMDDOMAIN}{Guid.NewGuid().ToString().Replace("-", "")}".ToUpper();
                return AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            }
        }

        private class _DomainController
        {
            public DomainController Controller;
            public AppDomain Domain;
            public ICommand Command;
        }
        #endregion
    }
}