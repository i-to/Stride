using System.Windows.Input;
using Stride.Gui.Utility;

namespace Stride.Gui.Input
{
    public class Dispatcher : KeyboardSink
    {
        readonly KeyboardPitchMapping KeyboardPitchMapping;
        readonly NoteInput NoteInput;

        public Dispatcher(KeyboardPitchMapping keyboardPitchMapping, NoteInput noteInput)
        {
            KeyboardPitchMapping = keyboardPitchMapping;
            NoteInput = noteInput;
        }

        public void KeyDown(KeyEventArgs args) => 
            KeyboardPitchMapping
                .Map(args.Key)
                .ForValue(NoteInput.NoteOn);

        public void KeyUp(KeyEventArgs args) =>
            KeyboardPitchMapping
                .Map(args.Key)
                .ForValue(NoteInput.NoteOff);
    }
}