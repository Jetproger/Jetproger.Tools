using System;
using System.ServiceModel;
using Jetproger.Tools.Process.Commands;

namespace Jetproger.Tools.Process.Services
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