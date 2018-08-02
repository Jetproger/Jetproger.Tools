using System;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Ex
    {
        private static ISettingFactory _consoleSettingFactory = new DefaultSettingFactory();

        public static void RegisterConsoleSettingFactory(ISettingFactory settingFactory)
        {
            _consoleSettingFactory = settingFactory;
        }

        public static class Ln<T> where T : ExSetting
        {
            private static readonly T[] Holder = { null };

            public static bool IsValid => One.IsValid;

            public static bool IsDeclare => One.IsDeclare;

            public static string Key => One.Code;

            public static string Text => One.Name;

            private static T One
            {
                get
                {
                    if (Holder[0] == null)
                    {
                        lock (Holder)
                        {
                            if (Holder[0] == null)
                            {
                                Holder[0] = (T)_consoleSettingFactory.CreateSetting(typeof(T));
                            }
                        }
                    }
                    return Holder[0];
                }
            }
        }
    }
}