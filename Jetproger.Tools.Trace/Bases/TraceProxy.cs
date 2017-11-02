using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Security.Principal;
using NLog;
using Log = Tools.Trace;

namespace Jetproger.Tools.Trace.Bases
{
    public class TraceProxy : MarshalByRefObject
    {
        private static readonly Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();
        private static readonly NlogConfig[] NlogConfigHolder = { null };

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void SetAppUser(string appUser)
        {
            Log.AppUser = appUser;
        }

        public void RegisterLogger(string loggerName)
        {
            if (Config.IsSaved()) return;
            loggerName = NlogConfig.GetTraceName(loggerName);
            if (!Loggers.ContainsKey(loggerName))
            {
                lock (Loggers)
                {
                    if (!Loggers.ContainsKey(loggerName))
                    {
                        if (Config.RegisterRule(loggerName)) Loggers.Add(loggerName, null);
                    }
                }
            }
        }

        public void Trace(string message, string loggerName)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var logger = GetLogger(loggerName);
            if (logger == null) return;
            lock (logger)
            {
                logger.Trace(message);
            }
        }

        public void Error(string message, string loggerName)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var logger = GetLogger(loggerName);
            if (logger == null) return;
            lock (logger)
            {
                logger.Error(message);
            }
        }

        private static Logger GetLogger(string loggerName)
        {
            loggerName = NlogConfig.GetTraceName(loggerName);
            lock (Loggers)
            {
                if (!Loggers.ContainsKey(loggerName)) return null;
                if (!Config.IsSaved()) Config.ToXml();
                var logger = Loggers[loggerName];
                if (logger == null)
                {
                    logger = LogManager.GetLogger(loggerName);
                    Loggers[loggerName] = logger;
                }
                return Loggers[loggerName];
            }
        }

        public void CreateChannel()
        {
            var type = typeof(TraceProxy);
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

        private static NlogConfig Config
        {
            get
            {
                if (NlogConfigHolder[0] == null)
                {
                    lock (NlogConfigHolder)
                    {
                        if (NlogConfigHolder[0] == null)
                        {
                            NlogConfigHolder[0] = new NlogConfig();
                            NlogConfigHolder[0].OfXml();
                        }
                    }
                }
                return NlogConfigHolder[0];
            }
        }
    }
}