using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Security.Principal;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        private static AppDomain _defaultAppDomain = AppDomain.CurrentDomain.IsDefaultAppDomain() ? AppDomain.CurrentDomain : null;

        public static AppDomain DefaultAppDomain
        {
            get { return _defaultAppDomain; }
            set { _defaultAppDomain = _defaultAppDomain ?? value; }
        }
    }

    public static class Je<T> where T : class
    {
        private static readonly ConcurrentBag<T> Pool = new ConcurrentBag<T>();
        private static readonly T[] Holder = { null };

        public static T New(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }

        public static T One()
        {
            return !NeedIpc() ? Je.One.Get(Holder) : (T)IpcClient.One.GetOne(typeof(T));
        }

        public static T One(Func<T> factory)
        {
            return !NeedIpc() ? Je.One.Get(Holder, factory) : (T)IpcClient.One.GetOne(typeof(T));
        }

        public static void One(T value)
        {
            Je.One.Set(Holder, value);
        }

        public static T Get()
        {
            if (NeedIpc()) return (T)IpcClient.One.Pull(typeof(T));
            T value; return Pool.TryTake(out value) ? value : New();
        }

        public static T Get(Func<T> factory)
        {
            T value; return Pool.TryTake(out value) ? value : factory();
        }

        public static void Put(T value)
        {
            if (NeedIpc()) IpcClient.One.Push(typeof(T), value); else Pool.Add(value);
        }

        private static bool NeedIpc()
        {
            return !AppDomain.CurrentDomain.IsDefaultAppDomain() && Je.DefaultAppDomain != null && typeof(T).IsMarshalByRef;
        }

        private static T GetOne()
        {
            return Je.One.Get(Holder, () => New());
        }

        private static T Pull()
        {
            return Get();
        }

        private static void Push(T value)
        {
            Put(value);
        }
    }

    public static class IpcClient
    {
        public static IpcOne One => Je.One.Get(Holder, GetProxy);
        private static readonly IpcOne[] Holder = { null };

        private static IpcOne GetProxy()
        {
            CreateChannel();
            var type = typeof(IpcOne);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = GetProxy(type, uri);
            if (proxy != null) return proxy;
            CreateProxy();
            return (IpcOne)Activator.GetObject(type, uri);
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

        private static IpcOne GetProxy(Type type, string uri)
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

        private static IpcOne TryGetProxy(Type type, string uri)
        {
            var proxy = (IpcOne)Activator.GetObject(type, uri);
            proxy.CreateChannel();
            return proxy;
        }

        private static void CreateProxy()
        {
            var type = typeof(IpcOne);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = Je.DefaultAppDomain.CreateInstanceAndUnwrap(assemblyName, typeName) as IpcOne;
            instance?.CreateChannel();
        }
    }

    public static class IpcServer
    {
        private static readonly ConcurrentDictionary<Type, MethodHolder> Methods = new ConcurrentDictionary<Type, MethodHolder>();

        public static MethodInfo GetGetOneMethod(Type type)
        {
            return Methods.GetOrAdd(type, FindMethods).GetOne;
        }

        public static MethodInfo GetPushMethod(Type type)
        {
            return Methods.GetOrAdd(type, FindMethods).Push;
        }

        public static MethodInfo GetPullMethod(Type type)
        {
            return Methods.GetOrAdd(type, FindMethods).Pull;
        }

        private static MethodHolder FindMethods(Type type)
        {
            var jeType = typeof(Je<>);
            var genericType = jeType.MakeGenericType(type);
            var getOne = (MethodInfo)null;
            var push = (MethodInfo)null;
            var pull = (MethodInfo)null;
            foreach (MethodInfo m in genericType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (m.Name == "GetOne") getOne = m;
                if (m.Name == "Push") push = m;
                if (m.Name == "Pull") pull = m;
            }
            return new MethodHolder(getOne, push, pull);
        }

        private class MethodHolder
        {
            public readonly MethodInfo GetOne;
            public readonly MethodInfo Push;
            public readonly MethodInfo Pull;
            public MethodHolder(MethodInfo getOne, MethodInfo push, MethodInfo pull)
            {
                GetOne = getOne;
                Push = push;
                Pull = pull;
            }
        }
    }

    public class IpcOne : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public object GetOne(Type type)
        {
            return IpcServer.GetGetOneMethod(type)?.Invoke(null, new object[0]);
        }

        public object Pull(Type type)
        {
            return IpcServer.GetPullMethod(type)?.Invoke(null, new object[0]);
        }

        public void Push(Type type, object value)
        {
            IpcServer.GetPushMethod(type)?.Invoke(null, new [] { value });
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