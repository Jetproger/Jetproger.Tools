using System; 
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    [Serializable]
    public class ContainerController : MarshalByRefObject
    {
        public CommandResponse ExecuteCommand(CommandRequest request)
        {
            return Je.cmd.ServerExecute(request);
        }
    }
}