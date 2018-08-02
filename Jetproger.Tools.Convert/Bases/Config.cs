using System;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Ex
    {
        private static ISettingFactory _configSettingFactory = new DefaultSettingFactory();

        public static void RegisterConfigSettingFactory(ISettingFactory settingFactory)
        {
            _configSettingFactory = settingFactory;
        }

        public static class Fi<T> where T : ExSetting
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
                                Holder[0] = (T)_configSettingFactory.CreateSetting(typeof(T));
                            }
                        }
                    }
                    return Holder[0];
                }
            }
        }
    }
}