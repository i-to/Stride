using System;
using Stride.Utility;

namespace Stride.Music.Theory
{
    public partial class Pitch : IEquatable<Pitch>, IComparable<Pitch>
    {
        public readonly Octave Octave;
        public readonly PitchClass PitchClass;

        public Pitch(Octave octave, PitchClass pitchClass)
        {
            Octave = octave;
            PitchClass = pitchClass;
        }

        public static Pitch Create(Octave octave, PitchClass pitchClass) => new Pitch(octave, pitchClass);

        public Pitch NextDiatonic => Create(PitchClass == PitchClass.B ? Octave.Next : Octave, PitchClass.Next);

        public int DiatonicDistanceTo(Pitch pitch)
        {
            var octaveDistance = pitch.Octave - Octave;
            var pitchDistance = pitch.PitchClass.Number - PitchClass.Number;
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
                ? left.PitchClass.Number > right.PitchClass.Number
                : left.Octave > right.Octave;

        public static bool operator <(Pitch left, Pitch right) =>
            left.Octave == right.Octave
                ? left.PitchClass.Number < right.PitchClass.Number
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
    }
}