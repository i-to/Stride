using Rationals;

namespace Stride.Music.Theory
{
    public enum Duration
    {
        Whole = 1,
        Half = 2,
        Quarter = 4,
    }

    public static class DurationExtensions
    {
        public static Rational Fraction(this Duration duration) => 
            Rational.One / (int)duration;

        public static bool IsWhole(this Duration duration) =>
            duration == Duration.Whole;

        public static bool IsHalf(this Duration duration) =>
            duration == Duration.Half;

        public static bool IsQuarter(this Duration duration) =>
            duration == Duration.Quarter;
    }
}
