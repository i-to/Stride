using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Utility
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrderAscending<T>(this IEnumerable<T> enumerable) =>
            enumerable.OrderBy(Combinator.Identity);

        public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> enumerable) =>
            enumerable.OrderByDescending(Combinator.Identity);

        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> enumerable) =>
            enumerable.ToArray();

        public static int FindIndex<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (var element in enumerable)
            {
                if (predicate(element))
                    return index;
                ++index;
            }
            return -1;
        }

        public static int FindIndex<T>(
            this IEnumerable<T> enumerable,
            T value,
            IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;
            return enumerable.FindIndex(v => comparer.Equals(v, value));
        }
    }
}
