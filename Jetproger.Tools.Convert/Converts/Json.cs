using System;

namespace Jetproger.Tools.Convert.Converts
{
    public static class JsonExtensions
    {
        public static string Of(this Jc.IJsonExpander exp, object value)
        {
            return Jc.Json<Jc.BaseJson>.Of(value);
        }

        public static TOut To<TOut>(this Jc.IJsonExpander exp, string txt)
        {
            return Jc.Json<Jc.BaseJson>.To<TOut>(txt);
        }

        public static object To(this Jc.IJsonExpander exp, string txt, Type type)
        {
            return Jc.Json<Jc.BaseJson>.To(txt, type);
        }
    }
}