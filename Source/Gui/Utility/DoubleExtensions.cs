namespace Stride.Gui.Utility
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
    }
}
