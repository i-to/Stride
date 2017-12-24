using System;
using Stride.Utility;

namespace Stride.Music.Score
{
    public class Tick : IEquatable<Tick>, IComparable<Tick>
    {
        public readonly int Bar;
        public readonly double Offset;

        public Tick(int bar, double offset)
        {
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
            return Math.Sign(Offset - other.Offset);
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
