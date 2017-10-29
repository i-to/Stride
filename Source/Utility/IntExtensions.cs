using System;

namespace Stride.Utility
{
    public static class IntExtensions
    {
        public static int Abs(this int value) =>
            Math.Abs(value);

        public static bool IsInRangeInclusive(this int value, int min, int max) =>
            value >= min && value <= max;

        public static int LimitFromBottom(this int value, int min) =>
            Math.Max(value, min);

        public static int LimitFromTop(this int value, int max) =>
            Math.Min(value, max);

        public static int Limit(this int value, int min, int max) =>
            value.LimitFromBottom(min).LimitFromTop(max);
    }
}
