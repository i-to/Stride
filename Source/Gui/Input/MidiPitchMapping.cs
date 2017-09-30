using NAudio.Midi;
using Stride.Gui.Music;

namespace Stride.Gui.Input
{
    public class MidiPitchMapping
    {
        public Pitch? Map(NoteEvent noteEvent)
        {
            switch (noteEvent.NoteNumber)
            {
                case 62: return Pitch.D4;
                case 64: return Pitch.E4;
                case 65: return Pitch.F4;
                case 67: return Pitch.G4;
                case 69: return Pitch.A4;
                case 71: return Pitch.B4;
                case 72: return Pitch.C5;
                case 74: return Pitch.D5;
                case 76: return Pitch.E5;
                case 77: return Pitch.F5;
                case 79: return Pitch.G5;
            }
            return null;
        }
    }
}