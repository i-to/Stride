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
            KeyboardPitchMapping.Map(args.Key).ForValue(NoteInput.NoteOn);
        }

        public void KeyUp(KeyEventArgs args)
        {
            if (NoteInputMode != NoteInputMode.Keyboard)
                return;
            KeyboardPitchMapping.Map(args.Key).ForValue(NoteInput.NoteOff);
        }

        public void MidiEvent(MidiEvent midiEvent)
        {
            if (NoteInputMode != NoteInputMode.Midi)
                return;

            if (midiEvent.IsNoteOn())
                MidiPitchMapping
                    .Map(midiEvent.Cast<NoteEvent>())
                    .ForValue(NoteInput.NoteOn);
            else if (midiEvent.IsNoteOff())
                MidiPitchMapping
                    .Map(midiEvent.Cast<NoteEvent>())
                    .ForValue(NoteInput.NoteOff);
        }
    }
}