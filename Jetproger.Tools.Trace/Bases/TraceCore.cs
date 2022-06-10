using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;
using NLog;

namespace Jetproger.Tools.Trace.Bases
{
    public static class TraceCore
    {
        private static readonly Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();
        private static NlogConfig Config { get { return Je.one.Get(ConfigHolder, ReadConfig); } }
        private static readonly NlogConfig[] ConfigHolder = { null };
        private static readonly string[] AppUserHolder = { null };

        public static string AppUser
        {
            get { return Je.one.Get(AppUserHolder); }
        }

        public static void SetAppUser(string appUser)
        {
            if (string.IsNullOrWhiteSpace(appUser))
            {
                return;
            }
            lock (AppUserHolder)
            {
                if (string.IsNullOrWhiteSpace(AppUserHolder[0])) AppUserHolder[0] = appUser;
            }
        }

        public static void RegisterLogger(string loggerName)
        {
            if (Config.IsSaved())
            {
                return;
            }
            loggerName = NlogConfig.ParseName(loggerName);
            lock (Loggers)
            {
                if (!Loggers.ContainsKey(loggerName))
                {
                    if (Config.RegisterRule(loggerName)) Loggers.Add(loggerName, null);
                }
            }
        }

        public static void Write(string loggerName, Jc.Ticket ticket)
        {
            var message = !string.IsNullOrWhiteSpace(ticket.Description) ? ticket.Description : ticket.Text;
            if (string.IsNullOrWhiteSpace(message)) return;
            var logger = GetLogger(loggerName);
            if (logger == null) return;
            lock (logger)
            {
                if (ticket.IsException)
                {
                    logger.Error(message);
                }
                else
                {
                    logger.Trace(message);
                }
            }
        }

        private static Logger GetLogger(string loggerName)
        {
            loggerName = NlogConfig.ParseName(loggerName);
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

        private static NlogConfig ReadConfig()
        {
            var config = new NlogConfig();
            config.OfXml();
            return config;
        }
    }
}