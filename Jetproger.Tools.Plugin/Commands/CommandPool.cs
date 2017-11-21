using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Threading;
using TP = Tools.Plugin;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandPool : MarshalByRefObject
    {
        private readonly ConcurrentQueue<CommandWorker> _instances;
        private readonly string _assemblyName;
        private readonly string _typeName;
        private readonly int _size;

        public CommandPool() : this(0)
        {
        }

        public CommandPool(int size)
        {
            _size = size > 0 ? size : 4;
            var type = typeof(CommandWorker);
            _typeName = type.ToString();
            _assemblyName = type.Assembly.GetName().Name;
            _instances = new ConcurrentQueue<CommandWorker>();
            var proc = new Action(BeginGenerateDomains);
            proc.BeginInvoke(EndGenerateDomains, proc);
        }

        public bool IsFull
        {
            get; private set;
        }

        public CommandWorker Get()
        {
            CommandWorker instance;
            return _instances.TryDequeue(out instance) ? instance : CreateInstance();
        }

        public void CreateChannel()
        {
            var type = typeof(CommandPool);
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var client = new BinaryClientFormatterSinkProvider();
            var server = new BinaryServerFormatterSinkProvider
            {
                TypeFilterLevel = TypeFilterLevel.Full
            };
            var config = new Hashtable
            {
                ["name"] = string.Empty,
                ["portName"] = portName,
                ["tokenImpersonationLevel"] = TokenImpersonationLevel.Impersonation,
                ["impersonate"] = true,
                ["useDefaultCredentials"] = true,
                ["secure"] = true,
                ["typeFilterLevel"] = TypeFilterLevel.Full
            };
            var ipcChannel = new IpcChannel(config, client, server);
            ChannelServices.RegisterChannel(ipcChannel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(type, objectUri, WellKnownObjectMode.SingleCall);
        }

        private void BeginGenerateDomains()
        {
            while (true)
            {
                if (!TP.IsHost)
                {
                    return;
                }
                if (_instances.Count >= _size)
                {
                    IsFull = true;
                    Thread.Sleep(333);
                    continue;
                }
                IsFull = false;
                _instances.Enqueue(CreateInstance());
            }
        }

        private void EndGenerateDomains(IAsyncResult asyncResult)
        {
            ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
        }

        private CommandWorker CreateInstance()
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var instance = domain.CreateInstanceAndUnwrap(_assemblyName, _typeName) as CommandWorker;
            //instance?.InitializeEnvironment(CoreMethods.Program.ExecutingAssemblyLocation);
            return instance;
        }
    }
}