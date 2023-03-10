using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Factories
{
    public class IpcClient
    {
        private readonly ConcurrentDictionary<Type, bool> _localTypes = new ConcurrentDictionary<Type, bool>();
        private IpcServer ContainerServer => f.one.of(_containerServerHolder, GetProxy);
        private readonly IpcServer[] _containerServerHolder = { null };
        private IpcServer LocalServer => f.one.of(_localServerHolder, () => new IpcServer());
        private readonly IpcServer[] _localServerHolder = { null };
        private bool IsLocal => f.one.of(_isLocalHolder, () => UseLocalCalls());
        private readonly bool?[] _isLocalHolder = { null };

        public object OfToOne(Type type, Func<object> func)
        {
            var creator = new SimpleCreator(func);
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal)) return TryOfToOneLocal(type, creator);
            object value;
            isLocal = TryOfToOneCombain(type, creator, out value);
            _localTypes.GetOrAdd(type, isLocal);
            return value;
        }

        public object OfOne(Type type)
        {
            bool isLocal;
            if (IsLocal ||(_localTypes.TryGetValue(type, out isLocal) && isLocal)) return TryOfOneLocal(type);
            object value;
            isLocal = TryOfOneCombain(type, out value);
            _localTypes.GetOrAdd(type, isLocal);
            return value;
        }

        public void ToOne(Type type, object value)
        {  
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal))
            {
                TryToOneLocal(type, value);
                return;
            } 
            isLocal = TryToOneCombain(type, value);
            _localTypes.GetOrAdd(type, isLocal);
        }

        public object OfToPool(Type type, Func<object> func)
        {
            var creator = new SimpleCreator(func);
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal)) return TryOfToPoolLocal(type, creator);
            object value;
            isLocal = TryOfToPoolCombain(type, creator, out value);
            _localTypes.GetOrAdd(type, isLocal);
            return value;
        }

        public object OfPool(Type type)
        {
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal)) return TryOfPoolLocal(type);
            object value;
            isLocal = TryOfPoolCombain(type, out value);
            _localTypes.GetOrAdd(type, isLocal);
            return value;
        }

        public void ToPool(Type type, object value)
        {
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal))
            {
                TryToPoolLocal(type, value);
                return;
            }
            isLocal = TryToPoolCombain(type, value);
            _localTypes.GetOrAdd(type, isLocal);
        }

        public object OfToStore(Type keyType, Type valueType, object key, Func<object, object> func)
        {
            var creator = new ParameterizedCreator(func);
            var type = typeof(StoreExtensions<,>).MakeGenericType(keyType, valueType);
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal)) return TryOfToStoreLocal(keyType, valueType, key, creator);
            object value;
            isLocal = TryOfToStoreCombain(keyType, valueType, key, creator, out value);
            _localTypes.GetOrAdd(type, isLocal);
            return value;
        }

        public object OfStore(Type keyType, Type valueType, object key)
        {
            var type = typeof(StoreExtensions<,>).MakeGenericType(keyType, valueType);
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal)) return TryOfStoreLocal(keyType, valueType, key);
            object value;
            isLocal = TryOfStoreCombain(keyType, valueType, key, out value);
            _localTypes.GetOrAdd(type, isLocal);
            return value;
        }

        public void ToStore(Type keyType, Type valueType, object key, object value)
        {
            var type = typeof(StoreExtensions<,>).MakeGenericType(keyType, valueType);
            bool isLocal;
            if (IsLocal || (_localTypes.TryGetValue(type, out isLocal) && isLocal))
            {
                TryToStoreLocal(keyType, valueType, key, value);
                return;
            }
            isLocal = TryToStoreCombain(keyType, valueType, key, value);
            _localTypes.GetOrAdd(type, isLocal);
        }

        #region Combain calls

        private bool TryOfToOneCombain(Type type, SimpleCreator creator, out object value)
        {
            if (TryOfToOneContainer(type, creator, out value)) return false;
            value = TryOfToOneLocal(type, creator);
            return true;
        }

        private bool TryOfOneCombain(Type type, out object value)
        {
            if (TryOfOneContainer(type, out value)) return false;
            value = TryOfOneLocal(type);
            return true;
        }

        private bool TryToOneCombain(Type type, object value)
        {
            if (TryToOneContainer(type, value)) return false;
            TryToOneLocal(type, value);
            return true;
        }

        private bool TryOfToPoolCombain(Type type, SimpleCreator creator, out object value)
        {
            if (TryOfToPoolContainer(type, creator, out value)) return false;
            value = TryOfToPoolLocal(type, creator);
            return true;
        }

        private bool TryOfPoolCombain(Type type, out object value)
        {
            if (TryOfPoolContainer(type, out value)) return false;
            value = TryOfPoolLocal(type);
            return true;
        }

        private bool TryToPoolCombain(Type type, object value)
        {
            if (TryToPoolContainer(type, value)) return false;
            TryToPoolLocal(type, value);
            return true;
        }

        private bool TryOfToStoreCombain(Type keyType, Type valueType, object key, ParameterizedCreator creator, out object value)
        {
            if (TryOfToStoreContainer(keyType, valueType, key, creator, out value)) return false;
            value = TryOfToStoreLocal(keyType, valueType, key, creator);
            return true;
        }

        private bool TryOfStoreCombain(Type keyType, Type valueType, object key, out object value)
        {
            if (TryOfStoreContainer(keyType, valueType, key, out value)) return false;
            value = TryOfStoreLocal(keyType, valueType, key);
            return true;
        }

        private bool TryToStoreCombain(Type keyType, Type valueType, object key, object value)
        {
            if (TryToStoreContainer(keyType, valueType, key, value)) return false;
            TryToStoreLocal(keyType, valueType, key, value);
            return true;
        }

        #endregion

        #region Container calls 

        private bool TryOfToOneContainer(Type type, SimpleCreator creator, out object value)
        {
            try
            {
                value = ContainerServer.OfToOne(type, creator);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        private bool TryOfOneContainer(Type type, out object value)
        {
            try
            {
                value = ContainerServer.OfOne(type);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        private bool TryToOneContainer(Type type, object value)
        {
            try
            {
                ContainerServer.ToOne(type, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryOfToPoolContainer(Type type, SimpleCreator creator, out object value)
        {
            try
            {
                value = ContainerServer.OfToPool(type, creator);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        private bool TryOfPoolContainer(Type type, out object value)
        {
            try
            {
                value = ContainerServer.OfPool(type);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        private bool TryToPoolContainer(Type type, object value)
        {
            try
            {
                ContainerServer.ToPool(type, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryOfToStoreContainer(Type keyType, Type valueType, object key, ParameterizedCreator creator, out object value)
        {
            try
            {
                value = ContainerServer.OfToStore(keyType, valueType, key, creator);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        private bool TryOfStoreContainer(Type keyType, Type valueType, object key, out object value)
        {
            try
            {
                value = ContainerServer.OfStore(keyType, valueType, key);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        private bool TryToStoreContainer(Type keyType, Type valueType, object key, object value)
        {
            try
            {
                ContainerServer.ToStore(keyType, valueType, key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Local calls 

        private object TryOfToOneLocal(Type type, SimpleCreator creator)
        {
            return LocalServer.OfToOne(type, creator);
        }

        private object TryOfOneLocal(Type type)
        {
            return LocalServer.OfOne(type);
        }

        private void TryToOneLocal(Type type, object value)
        {
            LocalServer.ToOne(type, value);
        }

        private object TryOfToPoolLocal(Type type, SimpleCreator creator)
        {
            return LocalServer.OfToPool(type, creator);
        }

        private object TryOfPoolLocal(Type type)
        {
            return LocalServer.OfPool(type);
        }

        private void TryToPoolLocal(Type type, object value)
        {
            LocalServer.ToPool(type, value);
        }

        private object TryOfToStoreLocal(Type keyType, Type valueType, object key, ParameterizedCreator creator)
        {
            return LocalServer.OfToStore(keyType, valueType, key, creator);
        }

        private object TryOfStoreLocal(Type keyType, Type valueType, object key)
        {
            return LocalServer.OfStore(keyType, valueType, key);
        }

        private void TryToStoreLocal(Type keyType, Type valueType, object key, object value)
        {
            LocalServer.ToStore(keyType, valueType, key, value);
        }


        #endregion

        private static IpcServer GetProxy()
        {
            CreateChannel();
            var type = typeof(IpcServer);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = GetProxy(type, uri);
            if (proxy != null) return proxy;
            CreateProxy();
            return (IpcServer)Activator.GetObject(type, uri);
        }

        private static void CreateChannel()
        {
            var type = typeof(IpcClient);
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{AppDomain.CurrentDomain.FriendlyName.Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
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

        private static IpcServer GetProxy(Type type, string uri)
        {
            try
            {
                return TryGetProxy(type, uri);
            }
            catch
            {
                return null;
            }
        }

        private static IpcServer TryGetProxy(Type type, string uri)
        {
            var proxy = (IpcServer)Activator.GetObject(type, uri);
            proxy.CreateChannel();
            return proxy;
        }

        private static void CreateProxy()
        {
            var type = typeof(IpcServer);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = f.own.CreateInstanceAndUnwrap(assemblyName, typeName) as IpcServer;
            instance?.CreateChannel();
        }

        private static bool UseLocalCalls()
        {
            return AppDomain.CurrentDomain.IsDefaultAppDomain() || f.own == null;
        }
    }
}