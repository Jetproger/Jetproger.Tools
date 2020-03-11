using System;
using System.Text;

namespace Jetproger.Tools.Convert.Converts
{
    public static class TxtExtensions
    {
        public static string Of(this Jc.ITxtExpander exp, object value)
        {
            return Jc.Txt<Jc.BaseTxt>.Of(value);
        }

        public static TOut To<TOut>(this Jc.ITxtExpander exp, string txt)
        {
            return Jc.Txt<Jc.BaseTxt>.To<TOut>(txt);
        }

        public static object To(this Jc.ITxtExpander exp, string txt, Type type)
        {
            return Jc.Txt<Jc.BaseTxt>.To(txt, type);
        }

        public static string Repeat(this Jc.ITxtExpander exp, string s, int count)
        {
            if (s == null) return null;
            if (s == string.Empty || count <= 0) return s;
            if (count == 1) return s;
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++) sb.Append(s);
            return sb.ToString();
        }

        public static string Reverse(this Jc.ITxtExpander exp, string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            var sb = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--) sb.Append(s[i]);
            return sb.ToString();
        }

        public static string Left(this Jc.ITxtExpander exp, string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > s.Length) return s;
            return s.Substring(0, length);
        }

        public static string Right(this Jc.ITxtExpander exp, string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > s.Length) return s;
            return s.Substring(s.Length - length);
        }

        public static string Replace(this Jc.ITxtExpander exp, string s, string substringOld, string substringNew)
        {
            if (substringOld == substringNew) return s;
            if (substringOld == null) return s;
            if (s == null) return null;
            if (substringOld.Length > s.Length) return s;
            substringNew = substringNew ?? "";
            return s.Replace(substringOld, substringNew);
        }

        public static string NewLength(this Jc.ITxtExpander exp, string s, int len)
        {
            s = s ?? string.Empty;
            var oldLen = s.Length;
            var newLen = len > 0 ? len : oldLen;
            if (newLen == oldLen) return s;
            var sb = new StringBuilder();
            for (int i = 0; i < newLen; i++) sb.Append(i < s.Length ? s[i] : ' ');
            return sb.ToString();
        }
    }
}