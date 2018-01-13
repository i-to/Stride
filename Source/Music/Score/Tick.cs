using System;
using Rationals;
using Stride.Utility;

namespace Stride.Music.Score
{
    public class Tick : IEquatable<Tick>, IComparable<Tick>
    {
        public readonly int Bar;
        public readonly Rational Offset;

        public Tick(int bar, Rational offset)
        {
            if (offset < 0 || offset >= 1)
                throw new InvalidOperationException($"{nameof(offset)} value outside of the bar.");
            Bar = bar;
            Offset = offset;
        }

        public static bool operator ==(Tick left, Tick right) =>
            left == null ? right == null : left.Equals(right);

        public static bool operator !=(Tick left, Tick right) =>
            !(left == right);

        public static bool operator <(Tick left, Tick right) =>
            left.CompareTo(right) < 0;

        public static bool operator >(Tick left, Tick right) =>
            left.CompareTo(right) > 0;

        public static bool operator <=(Tick left, Tick right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >=(Tick left, Tick right) =>
            left.CompareTo(right) >= 0;

        public int CompareTo(Tick other)
        {
            int barDiff = Bar - other.Bar;
            if (barDiff != 0)
                return barDiff;
            return (Offset - other.Offset).Sign;
        }

        public bool Equals(Tick other) => 
            !(other is null)
            && Bar == other.Bar
            && Offset == other.Offset;

        public override bool Equals(object other) => 
            Equals(other as Tick);

        public override int GetHashCode() => 
            Hash.Compute(Bar, Offset.GetHashCode());
    }
}
