using System.Collections.Generic;
using System.Linq;
using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class StaffPositionComputation
    {
        public StaffPosition ComputeStaffPosition(Pitch lowestTreebleStaffPitch, Pitch pitch) =>
            pitch >= lowestTreebleStaffPitch
                ? StaffPosition.InTreebleClef(-pitch.DiatonicDistanceTo(Pitch.B4))
                : StaffPosition.InBassClef(-pitch.DiatonicDistanceTo(Pitch.D3));

        public IEnumerable<StaffPosition> ComputeStaffPositions(
            Pitch lowestTreebleStaffPitch,
            IEnumerable<Pitch> pitches)
            => pitches.Select(pitch => ComputeStaffPosition(lowestTreebleStaffPitch, pitch));
    }
}
