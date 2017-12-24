using System.Windows.Input;
using NAudio.Midi;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Gui.Input
{
    public class NoteInputConverter : KeyboardSink, MidiSink
    {
        readonly KeyboardPitchMappings KeyboardPitchMappings;
        readonly MidiPitchMapping MidiPitchMapping;
        readonly NoteSink NoteSink;

        public NoteInputConverter(
            KeyboardPitchMappings keyboardPitchMappings,
            MidiPitchMapping midiPitchMapping,
            NoteSink noteSink,
            NoteInputMode noteInputMode)
        {
            KeyboardPitchMappings = keyboardPitchMappings;
            NoteSink = noteSink;
            NoteInputMode = noteInputMode;
            MidiPitchMapping = midiPitchMapping;
        }

        public NoteInputMode NoteInputMode { get; }

        Pitch TryMapKey(Key key) =>
            KeyboardPitchMappings[NoteInputMode]
            .TryGetValue(key, out Pitch pitch)
            ? pitch : null;

        public void KeyDown(KeyEventArgs args)
        {
            if (!NoteInputMode.IsKeyboardMode())
                return;
            TryMapKey(args.Key).IfNotNull(NoteSink.NoteOn);
        }

        public void KeyUp(KeyEventArgs args)
        {
            if (!NoteInputMode.IsKeyboardMode())
                return;
            TryMapKey(args.Key).IfNotNull(NoteSink.NoteOff);
        }

        public void MidiEvent(MidiEvent midiEvent)
        {
            if (!NoteInputMode.IsMidi())
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