using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using Jetproger.Tools.Process.Bases;
using Jetproger.Tools.Process.Services;

namespace Jetproger.Tools.Process.Commands
{
    [ServiceErrorBehavior, ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CommandService : ICommandService
    {
        #region Unexecute

        public IAsyncResult BeginInvokeUnexecute(CommandRequest request, AsyncCallback asyncCallback, object state)
        {
            try
            {
                return (new Func<CommandRequest, CommandResponse>(Unexecute)).BeginInvoke(request, asyncCallback, state);
            }
            catch (Exception e)
            {
                return Methods.ReturnInvalidResult<IAsyncResult>(e);
            }
        }

        public CommandResponse EndInvokeUnexecute(IAsyncResult asyncResult)
        {
            try
            {
                return ((Func<CommandRequest, CommandResponse>)((AsyncResult)asyncResult).AsyncDelegate).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                return new CommandResponse(Methods.ReturnInvalidResult<Exception>(e));
            }
        }

        private CommandResponse Unexecute(CommandRequest request)
        {
            try
            {
                return Command.UnexecuteService(request);
            }
            catch (Exception e)
            {
                return new CommandResponse(Methods.ReturnInvalidResult<Exception>(e));
            }
        }

        #endregion

        #region Execute

        public IAsyncResult BeginInvokeExecute(CommandRequest request, AsyncCallback asyncCallback, object state)
        {
            try
            {
                return (new Func<CommandRequest, CommandResponse>(Execute)).BeginInvoke(request, asyncCallback, state);
            }
            catch (Exception e)
            {
                return Methods.ReturnInvalidResult<IAsyncResult>(e);
            }
        }

        public CommandResponse EndInvokeExecute(IAsyncResult asyncResult)
        {
            try
            {
                return ((Func<CommandRequest, CommandResponse>)((AsyncResult)asyncResult).AsyncDelegate).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                return new CommandResponse(Methods.ReturnInvalidResult<Exception>(e));
            }
        }

        private CommandResponse Execute(CommandRequest request)
        {
            try
            {
                return Command.ExecuteService(request);
            }
            catch (Exception e)
            {
                return new CommandResponse(Methods.ReturnInvalidResult<Exception>(e));
            }
        }

        #endregion

        #region Enabled

        public IAsyncResult BeginInvokeEnabled(CommandRequest request, AsyncCallback asyncCallback, object state)
        {
            try
            {
                return (new Func<CommandRequest, CommandResponse>(Enabled)).BeginInvoke(request, asyncCallback, state);
            }
            catch (Exception e)
            {
                return Methods.ReturnInvalidResult<IAsyncResult>(e);
            }
        }

        public CommandResponse EndInvokeEnabled(IAsyncResult asyncResult)
        {
            try
            {
                return ((Func<CommandRequest, CommandResponse>)((AsyncResult)asyncResult).AsyncDelegate).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                return new CommandResponse(Methods.ReturnInvalidResult<Exception>(e));
            }
        }

        private CommandResponse Enabled(CommandRequest request)
        {
            try
            {
                return Command.EnabledService(request);
            }
            catch (Exception e)
            {
                return new CommandResponse(Methods.ReturnInvalidResult<Exception>(e));
            }
        }

        #endregion
    }
}