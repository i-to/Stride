using System.Collections.Generic;
using System.Linq;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Music.Score
{
    public class StaffPositionComputation
    {
        public StaffPosition ComputeStaffPosition(Pitch lowestTreebleStaffPitch, Pitch pitch) =>
            pitch >= lowestTreebleStaffPitch
                ? StaffPosition.InTreebleClef(-pitch.DiatonicDistanceTo(Pitch.B4))
                : StaffPosition.InBassClef(-pitch.DiatonicDistanceTo(Pitch.D3));

        public IReadOnlyList<StaffPosition> ComputeStaffPositions(
            Pitch lowestTreebleStaffPitch,
            IEnumerable<Pitch> pitches) =>
            pitches.Select(p => ComputeStaffPosition(lowestTreebleStaffPitch, p)).ToReadOnlyList();

        public IReadOnlyList<StaffPosition> ComputeStaffPositionsHarmonic(
            Pitch lowestTreebleStaffPitch,
            IEnumerable<Pitch> pitches)
        {
            var descendingPitches = pitches.OrderDescending().ToReadOnlyList();
            var count = descendingPitches.Count;
            var result = new StaffPosition[count];
            for (int i = 0; i != count; ++i)
            {
                var pitch = descendingPitches[i];
                var position = ComputeStaffPosition(lowestTreebleStaffPitch, pitch);
                if (i > 0)
                {
                    var previousPosition = result[i - 1];
                    if (!previousPosition.HorisontalOffset
                        && previousPosition.Clef == position.Clef
                        && previousPosition.VerticalOffset - position.VerticalOffset == 1)
                    {
                        position = position.WithHorizontalOffset(true);
                    }
                }
                result[i] = position;
            }
            return result;
        }
    }
}
