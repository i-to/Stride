﻿using System;
using Stride.Utility;

namespace Stride.Music.Theory
{
    public partial class Pitch : IEquatable<Pitch>, IComparable<Pitch>
    {
        public readonly Octave Octave;
        public readonly Note Note;

        public Pitch(Octave octave, Note note)
        {
            Octave = octave;
            Note = note;
        }

        public static Pitch Create(Octave octave, Note note) => new Pitch(octave, note);

        public Pitch NextDiatonic => Create(Note == Note.B ? Octave.Next : Octave, Note.Next);

        public int DiatonicDistanceTo(Pitch pitch)
        {
            var octaveDistance = pitch.Octave - Octave;
            var pitchDistance = pitch.Note.Number - Note.Number;
            return Const.NotesInOctave * octaveDistance + pitchDistance;
        }

        public bool IsInRangeInclusive(Pitch low, Pitch high) => this >= low && this <= high;

        public static bool operator ==(Pitch left, Pitch right)
        {
            if (left is null)
                return right is null;
            if (right is null)
                return false;
            return left.Note == right.Note && left.Octave == right.Octave;
        }

        public static bool operator >(Pitch left, Pitch right) => 
            left.Octave == right.Octave
                ? left.Note.Number > right.Note.Number
                : left.Octave > right.Octave;

        public static bool operator <(Pitch left, Pitch right) =>
            left.Octave == right.Octave
                ? left.Note.Number < right.Note.Number
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
        public override int GetHashCode() => Hash.Compute(Octave.GetHashCode(), Note.GetHashCode());

        public override string ToString() => $"{Note}{Octave.Number}";
    }
}