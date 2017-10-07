using System;
using Stride.Music;
using Stride.Utility;

namespace Stride.Model
{
    public class DrillPresenter
    {
        readonly Random Random;

        public DrillPresenter()
        {
            Random = new Random();
        }

        DrillSession Session;

        public void Start(DrillSession session)
        {
            Session = session;
            SwitchToNextQuestion();
        }

        public DrillStaff Staff { get; private set; }

        Pitch ComputeNextPitch()
        {
            var random = Random.NextDouble();
            var index = WeighedDistribution.BucketIndexOfValue(Session.PitchWeights, random);
            return Session.Pitches[index];
        }

        public void SwitchToNextQuestion()
        {
            var nextPitch = ComputeNextPitch();
            Staff = new DrillStaff(nextPitch);
        }

        public void SetPlayedPitch(Pitch pitch)
        {
            if (pitch == null)
            {
                var index = Session.Pitches.FindIndex(Staff.TestPitch);
                if (Staff.TestPitch == Staff.PlayedPitch)
                {
                    if (Session.PitchWeights[index] > 1.0)
                        Session.PitchWeights[index] -= 1.0;
                    SwitchToNextQuestion();
                }
                else
                {
                    Session.PitchWeights[index] += 3.0;
                }
            }
            Staff = Staff.WithPlayedPitch(pitch);
        }
    }
}
