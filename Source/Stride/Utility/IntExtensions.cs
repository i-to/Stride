using System;

namespace Stride.Utility
{
    public static class IntExtensions
    {
        public static int Abs(this int value) =>
            Math.Abs(value);

        public static int LimitFromBottom(this int value, int min) =>
            Math.Max(value, min);
    }
}
