using System.Web.Http;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandController : ApiController
    {
        [HttpPost]
        public CommandResponse ExecuteCommand(CommandRequest request)
        {
            return f.cmd.ServerExecute(request);
        }
    }
}