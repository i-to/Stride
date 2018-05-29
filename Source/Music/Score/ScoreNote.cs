using System.Collections.Generic;
using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class BeatGroup
    {
        public readonly Beat Beat;

        public readonly IReadOnlyList<ScoreNote> ScoreNotes;

        public BeatGroup(Beat beat, IReadOnlyList<ScoreNote> scoreNotes)
        {
            Beat = beat;
            ScoreNotes = scoreNotes;
        }
    }

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
