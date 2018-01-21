using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class NoteOnPage
    {
        public readonly Duration Duration;
        public readonly StaffPosition StaffPosition;
        public readonly Beat Beat;

        public NoteOnPage(Duration duration, StaffPosition staffPosition, Beat beat)
        {
            Duration = duration;
            StaffPosition = staffPosition;
            Beat = beat;
        }
    }
}
