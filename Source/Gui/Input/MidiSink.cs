using NAudio.Midi;

namespace Stride.Gui.Input
{
    public interface MidiSink
    {
        void MidiEvent(MidiEvent midiEvent);
    }
}