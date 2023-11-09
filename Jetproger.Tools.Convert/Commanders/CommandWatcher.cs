using System;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandWatcher
    {
        private CommandWatcher() { }

        private DateTime _timestamp;

        private TimeSpan _timespan;

        public void Start()
        {
            _timestamp = DateTime.UtcNow;
        }

        public bool Expired()
        {
            return (DateTime.UtcNow - _timestamp) >= _timespan;
        }

        public static CommandWatcher CreateCommandWatcherMilliseconds(long period, long min, long max)
        {
            return new CommandWatcher { _timespan = TimeSpan.FromMilliseconds(PeriodOf(period, min, max)) };
        }

        public static CommandWatcher CreateCommandWatcherSeconds(long period, long min, long max)
        {
            return new CommandWatcher { _timespan = TimeSpan.FromMilliseconds(1000 * PeriodOf(period, min, max)) };
        }

        public static CommandWatcher CreateCommandWatcherMinutes(long period, long min, long max)
        {
            return new CommandWatcher { _timespan = TimeSpan.FromMilliseconds(60000 * PeriodOf(period, min, max)) };
        }

        public static CommandWatcher CreateCommandWatcherHours(long period, long min, long max)
        {
            return new CommandWatcher { _timespan = TimeSpan.FromMilliseconds(3600000 * PeriodOf(period, min, max)) };
        }

        private static long PeriodOf(long period, long min, long max)
        {
            period = period > max ? max : period;
            period = period < min ? min : period;
            return period;
        }
    }
}