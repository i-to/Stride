using System;
using Stride.Utility;

namespace Stride.Music.Theory
{
    public class PitchClass : IEquatable<PitchClass>
    {
        public readonly int Number;

        PitchClass(int number)
        {
            if (!number.IsInRangeInclusive(0, 6))
                throw new ArgumentException($"Expected pitchClass number in [0, 6], given: {number}");
            Number = number;
        }

        public static PitchClass C = new PitchClass(0);
        public static PitchClass D = new PitchClass(1);
        public static PitchClass E = new PitchClass(2);
        public static PitchClass F = new PitchClass(3);
        public static PitchClass G = new PitchClass(4);
        public static PitchClass A = new PitchClass(5);
        public static PitchClass B = new PitchClass(6);

        static PitchClass OfNumber(int number)
        {
            switch (number)
            {
                case 0: return C;
                case 1: return D;
                case 2: return E;
                case 3: return F;
                case 4: return G;
                case 5: return A;
                case 6: return B;
            }
            throw new ArgumentOutOfRangeException($"Expected number within [0, 6], given: {number}.");
        }

        public PitchClass Next => OfNumber((Number + 1) % Const.NotesInOctave);
        public PitchClass Previous => OfNumber((Number - 1 + Const.NotesInOctave) % Const.NotesInOctave);

        public bool IsC => this == C;
        public bool IsD => this == D;
        public bool IsE => this == E;
        public bool IsF => this == F;
        public bool IsG => this == G;
        public bool IsA => this == A;
        public bool IsB => this == B;

        public static bool operator ==(PitchClass left, PitchClass right) => ReferenceEquals(left, right);
        public static bool operator !=(PitchClass left, PitchClass right) => !(left == right);

        public bool Equals(PitchClass other) => this == other;
        public override bool Equals(object obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => Number.GetHashCode();

        public override string ToString()
        {
            if (IsC) return "C";
            if (IsD) return "D";
            if (IsE) return "E";
            if (IsF) return "F";
            if (IsG) return "G";
            if (IsA) return "A";
            if (IsB) return "B";
            throw new InvalidOperationException();
        }
    }
}