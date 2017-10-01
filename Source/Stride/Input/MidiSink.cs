using NAudio.Midi;

namespace Stride.Input
{
    public interface MidiSink
    {
        void MidiEvent(MidiEvent midiEvent);
    }
}