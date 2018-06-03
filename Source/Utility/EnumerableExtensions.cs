using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Stride.Utility
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrderAscending<T>(this IEnumerable<T> enumerable)
            => enumerable.OrderBy(Combinator.Identity);

        public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> enumerable)
            => enumerable.OrderByDescending(Combinator.Identity);

        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> enumerable)
            => enumerable.ToArray();

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

        public static (T min, T max) MinMax<T>(this IEnumerable<T> enumerable)
        {
            bool started = false;
            var comparer = Comparer<T>.Default;
            var min = default(T);
            var max = default(T);
            foreach (var element in enumerable)
                if (started)
                {
                    if (comparer.Compare(element, min) < 0)
                        min = element;
                    if (comparer.Compare(max, element) < 0)
                        max = element;
                }
                else
                {
                    min = max = element;
                    started = true;
                }
            if (!started)
                throw new InvalidOperationException("The operation is not valid on empty enumerable.");
            return (min, max);
        }

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> enumerable)
            => enumerable.SelectMany(Combinator.Identity);

        public static IReadOnlyList<T> YieldReadOnlyList<T>(this T element) => new[] { element };
        public static IEnumerable<T> Yield<T>(this T element) => element.YieldReadOnlyList();
    }
}
