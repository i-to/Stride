using System;
using Stride.Utility;

namespace Stride.Music
{
    public class Note : IEquatable<Note>
    {
        public readonly int Number;

        Note(int number)
        {
            if (!number.IsInRangeInclusive(0, 6))
                throw new ArgumentException($"Expected note number in [0, 6], given: {number}");
            Number = number;
        }

        public static Note C = new Note(0);
        public static Note D = new Note(1);
        public static Note E = new Note(2);
        public static Note F = new Note(3);
        public static Note G = new Note(4);
        public static Note A = new Note(5);
        public static Note B = new Note(6);

        static Note OfNumber(int number)
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

        public Note Next => OfNumber((Number + 1) % Const.NotesInOctave);
        public Note Previous => OfNumber((Number - 1 + Const.NotesInOctave) % Const.NotesInOctave);

        public bool IsC => this == C;
        public bool IsD => this == D;
        public bool IsE => this == E;
        public bool IsF => this == F;
        public bool IsG => this == G;
        public bool IsA => this == A;
        public bool IsB => this == B;

        public static bool operator ==(Note left, Note right) => ReferenceEquals(left, right);
        public static bool operator !=(Note left, Note right) => !(left == right);

        public bool Equals(Note other) => this == other;
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