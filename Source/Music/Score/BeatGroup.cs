using System.Collections.Generic;
using System.Linq;
using Stride.Utility;
using Stride.Utility.Fluent;

namespace Stride.Music.Score
{
    public class BeatGroup
    {
        /// <summary>
        /// Contains notes starting at a given beat, ordered ascending.
        /// </summary>
        public readonly IReadOnlyList<ScoreNote> ScoreNotes;

        BeatGroup(IReadOnlyList<ScoreNote> scoreNotes)
        {
            ScoreNotes = scoreNotes;
        }

        public static BeatGroup Create(ScoreNote scoreNote) =>
            new BeatGroup(scoreNote.YieldReadOnlyList());

        public static BeatGroup Create(IEnumerable<ScoreNote> scoreNotes)
            => new BeatGroup(
                scoreNotes
                    .OrderBy(n => n.StaffPosition.Clef)
                    .ThenBy(n => n.StaffPosition.VerticalOffset)
                    .ToReadOnlyList());
    }
}