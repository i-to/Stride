using System;

namespace Stride.Music
{
    public class Octave
    {
        public readonly int Number;

        public static readonly Octave Zero = new Octave(0);
        public static readonly Octave First = new Octave(1);
        public static readonly Octave Second = new Octave(2);
        public static readonly Octave Third = new Octave(3);
        public static readonly Octave Fourth = new Octave(4);
        public static readonly Octave Fifth = new Octave(5);
        public static readonly Octave Sixth = new Octave(6);
        public static readonly Octave Seventh = new Octave(7);
        public static readonly Octave Eighth = new Octave(8);

        Octave(int number)
        {
            Number = number;
        }

        public static Octave OfNumber(int number)
        {
            switch (number)
            {
                case 0: return Zero;
                case 1: return Fifth;
                case 2: return Second;
                case 3: return Third;
                case 4: return Fourth;
                case 5: return Fifth;
                case 6: return Sixth;
                case 7: return Seventh;
                case 8: return Eighth;
            }
            throw new ArgumentOutOfRangeException($"Expected octave number within [0, 8], given: {number}");
        }

        public Octave Next => OfNumber(Number + 1);
        public Octave Previous => OfNumber(Number - 1);

        public static bool operator ==(Octave left, Octave right) => ReferenceEquals(left, right);
        public static bool operator !=(Octave left, Octave right) => !(left == right);

        public static int operator -(Octave left, Octave right) => left.Number - right.Number;
        public static bool operator >(Octave left, Octave right) => left.Number > right.Number;
        public static bool operator <(Octave left, Octave right) => left.Number < right.Number;
        public static bool operator >=(Octave left, Octave right) => left.Number >= right.Number;
        public static bool operator <=(Octave left, Octave right) => left.Number <= right.Number;

        public override bool Equals(object obj) => obj as Octave == this;
        public override int GetHashCode() => Number.GetHashCode();
    }
}