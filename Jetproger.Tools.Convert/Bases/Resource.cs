using System;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Ex
    {
        private static ISettingFactory _resourceSettingFactory = new DefaultSettingFactory();

        public static void RegisterResourceSettingFactory(ISettingFactory settingFactory)
        {
            _resourceSettingFactory = settingFactory;
        }

        public static class Rs<T> where T : ExSetting
        {
            private static readonly T[] Holder = { null };

            public static bool IsValid => One.IsValid;

            public static bool IsDeclare => One.IsDeclare;

            public static string Code => One.Code;

            public static string Name => One.Name;

            public static string Description => One.Description;

            public static string SpecName => One.SpecName;

            public static string Shortcut => One.Shortcut;

            public static byte[] Picture => One.Picture;

            private static T One
            {
                get
                {
                    if (Holder[0] == null)
                    {
                        lock (Holder)
                        {
                            if (Holder[0] == null) Holder[0] = (T)_resourceSettingFactory.CreateSetting(typeof(T));
                        }
                    }
                    return Holder[0];
                }
            }
        }
    }
}