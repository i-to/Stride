using System;
using Stride.Utility;

namespace Stride.Music
{
    public partial class Pitch : IEquatable<Pitch>
    {
        public readonly Octave Octave;
        public readonly Note Note;

        public Pitch(Octave octave, Note note)
        {
            Octave = octave;
            Note = note;
        }

        public static Pitch Create(Octave octave, Note note) => new Pitch(octave, note);

        public static bool operator ==(Pitch left, Pitch right)
        {
            if (left is null)
                return right is null;
            if (right is null)
                return false;
            return left.Note == right.Note && left.Octave == right.Octave;
        }

        public static bool operator !=(Pitch left, Pitch right) => !(left == right);
        public bool Equals(Pitch other) => other == this;
        public override bool Equals(object obj) => obj as Pitch == this;
        public override int GetHashCode() => Hash.Compute((int) Octave, (int) Note);
    }
}