using NAudio.Midi;

namespace Stride.Input
{
    public static class MidiEventExtensions
    {
        public static bool IsNoteOn(this MidiEvent midiEvent) => 
            MidiEvent.IsNoteOn(midiEvent);

        public static bool IsNoteOff(this MidiEvent midiEvent) =>
            MidiEvent.IsNoteOff(midiEvent);
    }
}
