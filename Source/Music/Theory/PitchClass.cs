using System;

namespace Stride.Music.Theory
{
    public class PitchClass : IEquatable<PitchClass>
    {
        public readonly NoteName NoteName;
        public readonly Accidental Accidental;

        public PitchClass(NoteName noteName, Accidental accidental)
        {
            NoteName = noteName;
            Accidental = accidental;
        }

        public static PitchClass Create(NoteName noteName, Accidental accidental = Accidental.Natural) =>
            new PitchClass(noteName, accidental);

        public PitchClass DoubleFlat => Create(NoteName, Accidental.DoubleFlat);
        public PitchClass Flat => Create(NoteName, Accidental.Flat);
        public PitchClass Natural => Create(NoteName);
        public PitchClass Sharp => Create(NoteName, Accidental.Sharp);
        public PitchClass DoubleSharp => Create(NoteName, Accidental.DoubleSharp);

        public static bool operator ==(PitchClass left, PitchClass right) => ReferenceEquals(left, right);
        public static bool operator !=(PitchClass left, PitchClass right) => !(left == right);

        public bool Equals(PitchClass other) => this == other;
        public override bool Equals(object obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => NoteName.GetHashCode();

        public override string ToString() => $"{NoteName}{Accidental.ToStringSymbol()}";

        public static PitchClass C = Create(NoteName.C);
        public static PitchClass D = Create(NoteName.D);
        public static PitchClass E = Create(NoteName.E);
        public static PitchClass F = Create(NoteName.F);
        public static PitchClass G = Create(NoteName.G);
        public static PitchClass A = Create(NoteName.A);
        public static PitchClass B = Create(NoteName.B);
    }
}