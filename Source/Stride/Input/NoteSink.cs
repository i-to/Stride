using Stride.Music;

namespace Stride.Input
{
    public interface NoteSink
    {
        void NoteOn(Pitch pitch);
        void NoteOff(Pitch pitch);
    }
}