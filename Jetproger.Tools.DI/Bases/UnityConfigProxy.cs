using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Security.Principal;

namespace Jetproger.Tools.Plugin.DI
{
    public class UnityConfigProxy : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public string GetUnityConfigXml()
        {
            return UnityConfigXml.Xml;
        }

        public void CreateChannel()
        {
            var type = typeof(UnityConfigProxy);
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