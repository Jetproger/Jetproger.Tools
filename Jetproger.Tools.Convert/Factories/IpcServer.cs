using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Security.Principal;

namespace Jetproger.Tools.Convert.Factories
{
    public class IpcServer : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public object OfToOne(Type type, SimpleCreator creator)
        {
            return OneExtensions.GetSet(type, creator);
        }

        public object OfOne(Type type)
        {
            return OneExtensions.Get(type);
        }

        public void ToOne(Type type, object value)
        {
            OneExtensions.Set(type, value);
        }

        public object OfToPool(Type type, SimpleCreator creator)
        {
            return PoolExtensions.GetSet(type, creator);
        }

        public object OfPool(Type type)
        {
            return PoolExtensions.Get(type);
        }

        public void ToPool(Type type, object value)
        {
            PoolExtensions.Set(type, value);
        }

        public object OfToStore(Type keyType, Type valueType, object key, ParameterizedCreator creator)
        {
            return StoreExtensions.GetSet(keyType, valueType, key, creator);
        }

        public object OfStore(Type keyType, Type valueType, object key)
        {
            return StoreExtensions.Get(keyType, valueType, key);
        }

        public void ToStore(Type keyType, Type valueType, object key, object value)
        {
            StoreExtensions.Set(keyType, valueType, key, value);
        }

        public void CreateChannel()
        {
            var type = GetType();
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
    }
}