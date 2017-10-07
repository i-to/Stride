using System;

namespace Stride.Utility
{
    public static class IntExtensions
    {
        public static int Abs(this int value) =>
            Math.Abs(value);

        /// <summary>
        /// Verifies if the value belongs to [min, max] interval.
        /// </summary>
        public static bool IsInRangeInclusive(this int value, int min, int max) =>
            value >= min && value <= max;

        public static int LimitFromBottom(this int value, int min) =>
            Math.Max(value, min);
    }
}
