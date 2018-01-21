using NAudio.Midi;

namespace Stride.Gui.Wpf.Input
{
    public interface MidiSink
    {
        void MidiEvent(MidiEvent midiEvent);
    }
}