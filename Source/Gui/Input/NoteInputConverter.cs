using System.Windows.Input;
using NAudio.Midi;
using Stride.Utility;

namespace Stride.Gui.Input
{
    public class NoteInputConverter : KeyboardSink, MidiSink
    {
        readonly KeyboardPitchMapping KeyboardPitchMapping;
        readonly MidiPitchMapping MidiPitchMapping;
        readonly NoteSink NoteSink;

        public NoteInputConverter(
            KeyboardPitchMapping keyboardPitchMapping,
            MidiPitchMapping midiPitchMapping,
            NoteSink noteSink,
            NoteInputMode noteInputMode)
        {
            KeyboardPitchMapping = keyboardPitchMapping;
            NoteSink = noteSink;
            NoteInputMode = noteInputMode;
            MidiPitchMapping = midiPitchMapping;
        }

        public NoteInputMode NoteInputMode { get; }

        public void KeyDown(KeyEventArgs args)
        {
            if (NoteInputMode != NoteInputMode.Keyboard)
                return;
            var pitch = KeyboardPitchMapping.Map(args.Key);
            NoteSink.NoteOn(pitch);
        }

        public void KeyUp(KeyEventArgs args)
        {
            if (NoteInputMode != NoteInputMode.Keyboard)
                return;
            var pitch = KeyboardPitchMapping.Map(args.Key);
            NoteSink.NoteOff(pitch);
        }

        public void MidiEvent(MidiEvent midiEvent)
        {
            if (NoteInputMode != NoteInputMode.Midi)
                return;
            if (midiEvent.IsNoteOn())
            {
                var pitch = MidiPitchMapping.Map(midiEvent.Cast<NoteEvent>());
                NoteSink.NoteOn(pitch);
            }
            else if (midiEvent.IsNoteOff())
            {
                var pitch = MidiPitchMapping.Map(midiEvent.Cast<NoteEvent>());
                NoteSink.NoteOff(pitch);
            }
        }
    }
}