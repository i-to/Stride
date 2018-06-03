using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Stride.Utility
{
    public static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<K, V> ToReadOnlyDictionary<K, V>(this IEnumerable<(K, V)> enumerable)
            => enumerable.ToDictionary();

        public static IReadOnlyDictionary<K, VTarget> MapValues<K, VSource, VTarget>(
            this IReadOnlyDictionary<K, VSource> dictionary,
            Func<K, VSource, VTarget> mapping)
            => dictionary
                .Select(kv => (kv.Key, mapping(kv.Key, kv.Value)))
                .ToReadOnlyDictionary();

        public static IReadOnlyDictionary<K, VTarget> MapValues<K, VSource, VTarget>(
            this IReadOnlyDictionary<K, VSource> dictionary,
            Func<VSource, VTarget> mapping)
            => dictionary.MapValues((_, v) => mapping(v));
    }
}
