using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Stride.Music.Layout;
using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class ScoreBuilder
    {
        readonly StaffPositionComputation StaffPositionComputation;
        readonly BarsComputation BarsComputation;

        public ScoreBuilder(StaffPositionComputation staffPositionComputation, BarsComputation barsComputation)
        {
            StaffPositionComputation = staffPositionComputation;
            BarsComputation = barsComputation;
        }

        public IEnumerable<NoteOnPage> CreateScore(Pitch lowestTreebleStaffPitch, IEnumerable<Note> phrase)
        {
            var pitches = phrase.Select(Note.GetPitch);
            var staffPositions = StaffPositionComputation.ComputeStaffPositions(lowestTreebleStaffPitch, pitches);
            var ticks = BarsComputation.SplitToBars(phrase);
            return phrase.EquiZip(staffPositions, ticks,
                (note, staffPosition, tick) => new NoteOnPage(note.Duration, staffPosition, tick));
        }
    }
}
