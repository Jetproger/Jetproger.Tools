using System;
using System.ServiceModel;
using Jetproger.Tools.Plugin.Commands;

namespace Jetproger.Tools.Plugin.Services
{
    [ServiceContract]
    public interface ICommandService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginInvokeUnexecute(CommandRequest request, AsyncCallback asyncCallback, object state);
        CommandResponse EndInvokeUnexecute(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginInvokeExecute(CommandRequest request, AsyncCallback asyncCallback, object state);
        CommandResponse EndInvokeExecute(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginInvokeEnabled(CommandRequest request, AsyncCallback asyncCallback, object state);
        CommandResponse EndInvokeEnabled(IAsyncResult asyncResult);
    }
}