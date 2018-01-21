namespace Stride.Gui.Wpf.Input
{
    public enum NoteInputMode { KeyboardNamed, KeyboardLinearDiatonic, Midi }

    public static class NoteInputModeExtensions
    {
        public static bool IsMidi(this NoteInputMode mode) =>
            mode == NoteInputMode.Midi;

        public static bool IsKeyboardMode(this NoteInputMode mode) =>
            !mode.IsMidi();
    }
}