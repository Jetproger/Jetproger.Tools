using System;

namespace Jetproger.Tools.Process.Commands
{
    public class CommandRequest
    {
        public Guid SessionId { get; set; }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Json { get; set; }
        public bool IsCancel { get; set; }
        public bool IsPostBack { get; set; }
        public bool IsIsolate { get; set; }
        public bool IsRemote { get; set; }
    }
}