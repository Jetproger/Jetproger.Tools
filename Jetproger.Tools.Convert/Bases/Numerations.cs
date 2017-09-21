namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        public static long AsHex(this string value)
        {
            return System.Convert.ToInt64(value, 16);
        }

        public static string AsHex(this byte value)
        {
            return System.Convert.ToString(value, 16);
        }

        public static string AsHex(this sbyte value)
        {
            return System.Convert.ToString(value, 16);
        }

        public static string AsHex(this char value)
        {
            return System.Convert.ToString(value, 16);
        }

        public static string AsHex(this short value)
        {
            return System.Convert.ToString(value, 16);
        }

        public static string AsHex(this int value)
        {
            return System.Convert.ToString(value, 16);
        }

        public static string AsHex(this long value)
        {
            return System.Convert.ToString(value, 16);
        }

        public static long AsOct(this string value)
        {
            return System.Convert.ToInt64(value, 8);
        }

        public static string AsOct(this byte value)
        {
            return System.Convert.ToString(value, 8);
        }

        public static string AsOct(this sbyte value)
        {
            return System.Convert.ToString(value, 8);
        }

        public static string AsOct(this char value)
        {
            return System.Convert.ToString(value, 8);
        }

        public static string AsOct(this short value)
        {
            return System.Convert.ToString(value, 8);
        }

        public static string AsOct(this int value)
        {
            return System.Convert.ToString(value, 8);
        }

        public static string AsOct(this long value)
        {
            return System.Convert.ToString(value, 8);
        }

        public static long AsBin(this string value)
        {
            return System.Convert.ToInt64(value, 2);
        }

        public static string AsBin(this byte value)
        {
            return System.Convert.ToString(value, 2);
        }

        public static string AsBin(this sbyte value)
        {
            return System.Convert.ToString(value, 2);
        }

        public static string AsBin(this char value)
        {
            return System.Convert.ToString(value, 2);
        }

        public static string AsBin(this short value)
        {
            return System.Convert.ToString(value, 2);
        }

        public static string AsBin(this int value)
        {
            return System.Convert.ToString(value, 2);
        }

        public static string AsBin(this long value)
        {
            return System.Convert.ToString(value, 2);
        }
    }
}