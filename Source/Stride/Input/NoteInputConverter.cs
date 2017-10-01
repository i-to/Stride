using System.Windows.Input;
using NAudio.Midi;
using Stride.Utility;

namespace Stride.Input
{
    public class NoteInputConverter : KeyboardSink, MidiSink
    {
        readonly KeyboardPitchMapping KeyboardPitchMapping;
        readonly MidiPitchMapping MidiPitchMapping;
        readonly NoteInput NoteInput;

        public NoteInputConverter(
            KeyboardPitchMapping keyboardPitchMapping,
            MidiPitchMapping midiPitchMapping,
            NoteInput noteInput,
            NoteInputMode noteInputMode)
        {
            KeyboardPitchMapping = keyboardPitchMapping;
            NoteInput = noteInput;
            NoteInputMode = noteInputMode;
            MidiPitchMapping = midiPitchMapping;
        }

        public NoteInputMode NoteInputMode { get; }

        public void KeyDown(KeyEventArgs args)
        {
            if (NoteInputMode != NoteInputMode.Keyboard)
                return;
            var pitch = KeyboardPitchMapping.Map(args.Key);
            NoteInput.NoteOn(pitch);
        }

        public void KeyUp(KeyEventArgs args)
        {
            if (NoteInputMode != NoteInputMode.Keyboard)
                return;
            var pitch = KeyboardPitchMapping.Map(args.Key);
            NoteInput.NoteOff(pitch);
        }

        public void MidiEvent(MidiEvent midiEvent)
        {
            if (NoteInputMode != NoteInputMode.Midi)
                return;
            if (midiEvent.IsNoteOn())
            {
                var pitch = MidiPitchMapping.Map(midiEvent.Cast<NoteEvent>());
                NoteInput.NoteOn(pitch);
            }
            else if (midiEvent.IsNoteOff())
            {
                var pitch = MidiPitchMapping.Map(midiEvent.Cast<NoteEvent>());
                NoteInput.NoteOff(pitch);
            }
        }
    }
}