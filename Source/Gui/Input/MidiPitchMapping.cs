using System;
using NAudio.Midi;
using Stride.Music;

namespace Stride.Gui.Input
{
    public class MidiPitchMapping
    {
        Pitch MakeNatural(Note note, Octave octave)
        {
            return new Pitch(octave, note);
        }

        // todo: add support for flats/sharps
        Pitch MakeSharp(Note note, Octave octave)
        {
            throw new NotImplementedException();
        }

        public Pitch Map(NoteEvent noteEvent)
        {
            var octave = Octave.OfNumber(noteEvent.NoteNumber / Const.SemitonesInOctave - 1);
            switch (noteEvent.NoteNumber % Const.SemitonesInOctave)
            {

                case 0 : return MakeNatural(Note.C, octave);
                case 1 : return MakeSharp  (Note.C, octave);
                case 2 : return MakeNatural(Note.D, octave);
                case 3 : return MakeSharp  (Note.D, octave);
                case 4 : return MakeNatural(Note.E, octave);
                case 5 : return MakeNatural(Note.F, octave);
                case 6 : return MakeSharp  (Note.F, octave);
                case 7 : return MakeNatural(Note.G, octave);
                case 8 : return MakeSharp  (Note.G, octave);
                case 9 : return MakeNatural(Note.A, octave);
                case 10: return MakeSharp  (Note.A, octave);
                case 11: return MakeNatural(Note.B, octave);
            }
            throw new InvalidOperationException();
        }
    }
}