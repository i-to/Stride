﻿using System;
using System.Collections.Generic;

namespace Stride.Utility
{
    public static class EnumerableExtensions
    {
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