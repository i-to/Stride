using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class ScoreNote
    {
        public readonly Duration Duration;
        public readonly StaffPosition StaffPosition;
        public readonly Accidental Accidental;

        public ScoreNote(Duration duration, StaffPosition staffPosition, Accidental accidental)
        {
            Duration = duration;
            StaffPosition = staffPosition;
            Accidental = accidental;
        }
    }
}
