using System;
using Stride.Utility;

namespace Stride.Music.Theory
{
    public class Pitch : IEquatable<Pitch>, IComparable<Pitch>
    {
        public readonly Octave Octave;
        public readonly PitchClass PitchClass;

        public Pitch(Octave octave, PitchClass pitchClass)
        {
            Octave = octave;
            PitchClass = pitchClass;
        }

        public static Pitch Create(Octave octave, PitchClass pitchClass) => new Pitch(octave, pitchClass);
        public static Accidental GetAccidental(Pitch pitch) => pitch.PitchClass.Accidental;

        public Pitch DoubleFlat => Create(Octave, PitchClass.DoubleFlat);
        public Pitch Flat => Create(Octave, PitchClass.Flat);
        public Pitch Natural => Create(Octave, PitchClass.Natural);
        public Pitch Sharp => Create(Octave, PitchClass.Sharp);
        public Pitch DoubleSharp => Create(Octave, PitchClass.DoubleSharp);

        public int DiatonicDistanceTo(Pitch pitch)
        {
            var octaveDistance = pitch.Octave - Octave;
            var pitchDistance = pitch.PitchClass.NoteName - PitchClass.NoteName;
            return Const.NotesInOctave * octaveDistance + pitchDistance;
        }

        public bool IsInRangeInclusive(Pitch low, Pitch high) => this >= low && this <= high;

        public static bool operator ==(Pitch left, Pitch right)
        {
            if (left is null)
                return right is null;
            if (right is null)
                return false;
            return left.PitchClass == right.PitchClass && left.Octave == right.Octave;
        }

        public static bool operator >(Pitch left, Pitch right) => 
            left.Octave == right.Octave
                ? left.PitchClass.NoteName > right.PitchClass.NoteName
                : left.Octave > right.Octave;

        public static bool operator <(Pitch left, Pitch right) =>
            left.Octave == right.Octave
                ? left.PitchClass.NoteName < right.PitchClass.NoteName
                : left.Octave < right.Octave;

        public static bool operator >=(Pitch left, Pitch right) => !(left < right);
        public static bool operator <=(Pitch left, Pitch right) => !(left > right);

        public int CompareTo(Pitch other)
        {
            if (this < other)
                return -1;
            if (this > other)
                return 1;
            return 0;
        }

        public static bool operator !=(Pitch left, Pitch right) => !(left == right);
        public bool Equals(Pitch other) => other == this;
        public override bool Equals(object obj) => obj as Pitch == this;
        public override int GetHashCode() => Hash.Compute(Octave.GetHashCode(), PitchClass.GetHashCode());

        public override string ToString() => $"{PitchClass}{Octave.Number}";

        public static Pitch C3 = Create(Octave.Third, PitchClass.C);
        public static Pitch D3 = Create(Octave.Third, PitchClass.D);
        public static Pitch E3 = Create(Octave.Third, PitchClass.E);
        public static Pitch F3 = Create(Octave.Third, PitchClass.F);
        public static Pitch G3 = Create(Octave.Third, PitchClass.G);
        public static Pitch A3 = Create(Octave.Third, PitchClass.A);
        public static Pitch B3 = Create(Octave.Third, PitchClass.B);

        public static Pitch C4 = Create(Octave.Fourth, PitchClass.C);
        public static Pitch D4 = Create(Octave.Fourth, PitchClass.D);
        public static Pitch E4 = Create(Octave.Fourth, PitchClass.E);
        public static Pitch F4 = Create(Octave.Fourth, PitchClass.F);
        public static Pitch G4 = Create(Octave.Fourth, PitchClass.G);
        public static Pitch A4 = Create(Octave.Fourth, PitchClass.A);
        public static Pitch B4 = Create(Octave.Fourth, PitchClass.B);

        public static Pitch C5 = Create(Octave.Fifth, PitchClass.C);
        public static Pitch D5 = Create(Octave.Fifth, PitchClass.D);
        public static Pitch E5 = Create(Octave.Fifth, PitchClass.E);
        public static Pitch F5 = Create(Octave.Fifth, PitchClass.F);
        public static Pitch G5 = Create(Octave.Fifth, PitchClass.G);
        public static Pitch A5 = Create(Octave.Fifth, PitchClass.A);
        public static Pitch B5 = Create(Octave.Fifth, PitchClass.B);

        public static Pitch C6 = Create(Octave.Sixth, PitchClass.C);
        public static Pitch D6 = Create(Octave.Sixth, PitchClass.D);
        public static Pitch E6 = Create(Octave.Sixth, PitchClass.E);
        public static Pitch F6 = Create(Octave.Sixth, PitchClass.F);
        public static Pitch G6 = Create(Octave.Sixth, PitchClass.G);
        public static Pitch A6 = Create(Octave.Sixth, PitchClass.A);
        public static Pitch B6 = Create(Octave.Sixth, PitchClass.B);
    }
}