namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        public static class Rs<T> where T : class, ISetting
        {
            public static bool Is
            {
                get { return Je<T>.One(() => Je<T>.New()).IsDeclared(); }
            }

            public static string Sz
            {
                get { return Je<T>.One(() => Je<T>.New()).GetValue(); }
            }
        }
    }

    public interface ISetting
    {
        bool IsDeclared();

        string GetValue();
    }
}