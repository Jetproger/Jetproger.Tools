using System;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        public static IOneExpander One => null;

        public interface IOneExpander { }
    }

    public static class OneExtensions
    {
        public static T Get<T>(this Je.IOneExpander e, T[] holder, Func<T> factory) where T : class
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return holder[0];
        }

        public static T Get<T>(this Je.IOneExpander e, T?[] holder, Func<T> factory) where T : struct
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return (T) holder[0];
        }

        public static T Get<T>(this Je.IOneExpander e, T[] holder) where T : class
        {
            lock (holder)
            {
                return holder[0];
            }
        }

        public static T Get<T>(this Je.IOneExpander e, T?[] holder) where T : struct
        {
            lock (holder)
            {
                return holder[0] ?? default(T);
            }
        }

        public static void Set<T>(this Je.IOneExpander e, T[] holder, T value) where T : class
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }

        public static void Set<T>(this Je.IOneExpander e, T?[] holder, T value) where T : struct
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }
    }
}