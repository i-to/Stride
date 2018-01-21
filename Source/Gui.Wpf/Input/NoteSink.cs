using Stride.Music.Theory;

namespace Stride.Gui.Wpf.Input
{
    public interface NoteSink
    {
        void NoteOn(Pitch pitch);
        void NoteOff(Pitch pitch);
    }
}