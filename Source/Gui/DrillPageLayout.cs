using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Gui
{
    public class DrillPageLayout
    {
        public readonly StaffPositionComputation StaffPositionComputation;
        public readonly BarsComputation BarsComputation;

        public DrillPageLayout(StaffPositionComputation staffPositionComputation, BarsComputation barsComputation)
        {
            StaffPositionComputation = staffPositionComputation;
            BarsComputation = barsComputation;
        }

        public Page CreatePage(
            Pitch lowestTreebleStaffPitch,
            IEnumerable<Note> testPhrase,
            IEnumerable<Pitch> soundingPitches,
            int drillQuizCurrentPosition)
        {
            var staffPositions = ComputeStaffPositions(lowestTreebleStaffPitch, testPhrase);
            var ticks = BarsComputation.SplitToBars(testPhrase);
            var pageNotes = testPhrase
                .EquiZip(staffPositions, ticks,
                    (note, staffPosition, tick) => new NoteOnPage(note.Duration, staffPosition, tick))
                .ToReadOnlyList();

            var overlayNotes =
                StaffPositionComputation.ComputeStaffPositionsHarmonic(
                    lowestTreebleStaffPitch, soundingPitches)
                .Select(staffPosition =>
                    new NoteOnPage(
                        Duration.Whole,
                        staffPosition,
                        new Tick(drillQuizCurrentPosition, 0)))
                .ToReadOnlyList();
            return new Page(pageNotes,overlayNotes);
        }

        IEnumerable<StaffPosition> ComputeStaffPositions(Pitch lowestTreebleStaffPitch, IEnumerable<Note> notes) => 
            StaffPositionComputation.ComputeStaffPositions(
                lowestTreebleStaffPitch, notes.Select(Note.GetPitch));
    }
}