using System;

namespace Stride.Utility.Fluent
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Verifies if the value belongs to [min, max] interval.
        /// </summary>
        public static bool IsInRangeInclusive(this double value, double min, double max) =>
            value >= min && value <= max;

        /// <summary>
        /// Verifies if the value belongs to [min, max) interval.
        /// </summary>
        public static bool IsInRangeInclusiveLower(this double value, double min, double max) =>
            value >= min && value < max;

        /// <summary>
        /// Verifies if the value belongs to (min, max] interval.
        /// </summary>
        public static bool IsInRangeInclusiveUpper(this double value, double min, double max) =>
            value > min && value <= max;

        /// <summary>
        /// Verifies if the value belongs to [min, max] interval.
        /// </summary>
        public static bool IsInRangeNonInclusive(this double value, double min, double max) =>
            value > min && value < max;

        public static int Ceiling(this double value) => (int) Math.Ceiling(value);
        public static int Floor(this double value) => (int) Math.Floor(value);

        public static double Squared(this double value) => value * value;
        public static double Sqrt(this double value) => Math.Sqrt(value);
    }
}
