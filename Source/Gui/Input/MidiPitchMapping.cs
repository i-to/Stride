using System;
using NAudio.Midi;
using Stride.Music.Theory;

namespace Stride.Gui.Input
{
    public class MidiPitchMapping
    {
        Pitch MakeNatural(PitchClass pitchClass, Octave octave)
        {
            return new Pitch(octave, pitchClass);
        }

        // todo: add support for flats/sharps
        Pitch MakeSharp(PitchClass pitchClass, Octave octave)
        {
            throw new NotImplementedException();
        }

        public Pitch Map(NoteEvent noteEvent)
        {
            var octave = Octave.OfNumber(noteEvent.NoteNumber / Const.SemitonesInOctave - 1);
            switch (noteEvent.NoteNumber % Const.SemitonesInOctave)
            {

                case 0 : return MakeNatural(PitchClass.C, octave);
                case 1 : return MakeSharp  (PitchClass.C, octave);
                case 2 : return MakeNatural(PitchClass.D, octave);
                case 3 : return MakeSharp  (PitchClass.D, octave);
                case 4 : return MakeNatural(PitchClass.E, octave);
                case 5 : return MakeNatural(PitchClass.F, octave);
                case 6 : return MakeSharp  (PitchClass.F, octave);
                case 7 : return MakeNatural(PitchClass.G, octave);
                case 8 : return MakeSharp  (PitchClass.G, octave);
                case 9 : return MakeNatural(PitchClass.A, octave);
                case 10: return MakeSharp  (PitchClass.A, octave);
                case 11: return MakeNatural(PitchClass.B, octave);
            }
            throw new InvalidOperationException();
        }
    }
}