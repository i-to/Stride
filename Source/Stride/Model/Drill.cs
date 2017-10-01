using System;
using System.Collections.Generic;
using System.Linq;
using Stride.Music;
using Stride.Utility;

namespace Stride.Model
{
    public class Drill
    {
        readonly IReadOnlyList<Pitch> Pitches = new[]
        {
            Pitch.D4, Pitch.E4, Pitch.F4, Pitch.G4, Pitch.A4, Pitch.B4,
            Pitch.C5, Pitch.D5, Pitch.E5, Pitch.F5, Pitch.G5
        };

        readonly double[] PitchWeights;
        readonly Random Random;

        public Drill()
        {
            Random = new Random();
            PitchWeights = Enumerable.Range(0, Pitches.Count).Select(_ => 3.0).ToArray();
            SwitchToNextQuestion();
        }

        public DrillStaff Staff { get; private set; }

        Pitch ComputeNextPitch()
        {
            var random = Random.NextDouble();
            var index = WeighedDistribution.BucketIndexOfValue(PitchWeights, random);
            return Pitches[index];
        }

        public void SwitchToNextQuestion()
        {
            var nextPitch = ComputeNextPitch();
            Staff = new DrillStaff(nextPitch);
        }

        public void SetPlayedPitch(Pitch? pitch)
        {
            if (pitch == null)
            {
                var index = Pitches.FindIndex(Staff.TestPitch);
                if (Staff.TestPitch == Staff.PlayedPitch)
                {
                    if (PitchWeights[index] > 1.0)
                        PitchWeights[index] -= 1.0;
                    SwitchToNextQuestion();
                }
                else
                {
                    PitchWeights[index] += 3.0;
                }
            }
            Staff = Staff.WithPlayedPitch(pitch);
        }
    }
}
