using Stride.Music;

namespace Stride.Model
{
    public class DrillStaff
    {
        public readonly Pitch TestPitch;
        public readonly Pitch? PlayedPitch;

        public DrillStaff(Pitch testPitch, Pitch? playedPitch = null)
        {
            TestPitch = testPitch;
            PlayedPitch = playedPitch;
        }

        public DrillStaff WithPlayedPitch(Pitch? pitch) => new DrillStaff(TestPitch, pitch);
        public DrillStaff WithNoPlayedPitch() => WithPlayedPitch(null);
    }
}