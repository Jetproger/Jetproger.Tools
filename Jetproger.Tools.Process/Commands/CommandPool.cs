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

namespace Jetproger.Tools.Process.Commands
{
    public class CommandPool : MarshalByRefObject
    {
        private readonly ConcurrentQueue<CommandWorker> _instances;
        private readonly string _assemblyName;
        private readonly string _typeName;
        private readonly int _size;
        public CommandPool() : this(0) { }

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

        private void BeginGenerateDomains()
        {
            while (true)
            {
                if (_size == int.MaxValue)
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
            instance?.Initialize();
            return instance;
        }
    }

    public class CommandPoolProxy : MarshalByRefObject
    {
        private static readonly CommandPool[] PoolHolder = { null };

        public CommandPool GetPool(int size)
        {
            if (PoolHolder[0] == null)
            {
                lock (PoolHolder)
                {
                    if (PoolHolder[0] == null) PoolHolder[0] = new CommandPool(size);
                }
            }
            return PoolHolder[0];
        }

        public void CreateChannel()
        {
            var type = typeof(CommandPoolProxy);
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{System.Diagnostics.Process.GetCurrentProcess().Id}";
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
    }
}