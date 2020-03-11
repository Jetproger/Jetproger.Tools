using System.Linq;

namespace Jetproger.Tools.Convert.Converts
{
    public static class ExtExtensions
    {
        public static bool ArrContains<T>(this Jc.IExtExpander exp, T[] parents, T item)
        {
            return parents.Any(x => ReferenceEquals(x, item));
        }

        public static T[] ArrAdd<T>(this Jc.IExtExpander exp, T[] items, T item)
        {
            var newItems = new T[items.Length + 1];
            for (int i = 0; i < items.Length; i++) newItems[i] = items[i];
            newItems[newItems.Length - 1] = item;
            return newItems;
        }
    }
}