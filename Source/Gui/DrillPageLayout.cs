using System.Collections.Generic;
using System.Linq;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Gui
{
    public class DrillPageLayout
    {
        public readonly StaffPositionComputation StaffPositionComputation;

        public DrillPageLayout(StaffPositionComputation staffPositionComputation)
        {
            StaffPositionComputation = staffPositionComputation;
        }

        public Page CreatePage(
            Pitch lowestTreebleStaffPitch,
            IEnumerable<Note> testPhrase,
            IEnumerable<Pitch> soundingPitches,
            int drillQuizCurrentPosition)
        {
            var pageNotes =
                StaffPositionComputation.ComputeStaffPositions(
                    lowestTreebleStaffPitch, testPhrase.Select(n => n.Pitch))
                .Select((staffPosition, i) =>
                    new NoteOnPage(
                        Duration.Whole,
                        staffPosition,
                        new Tick(i, 0)))
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
    }
}