using System;
using System.Diagnostics;
using Stride.Music;
using Stride.Utility;

namespace Stride.Model
{
    public class DrillPresenter
    {
        readonly Random Random;
        readonly Stopwatch Timer;
        int WrongAnswerCount;

        public DrillPresenter()
        {
            Random = new Random();
            Timer = new Stopwatch();
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
            WrongAnswerCount = 0;
            Timer.Restart();
        }

        public void SetPlayedPitch(Pitch pitch)
        {
            if (pitch == null)
            {
                var index = Session.Pitches.FindIndex(Staff.TestPitch);
                if (Staff.TestPitch == Staff.PlayedPitch)
                {
                    Timer.Stop();
                    var wrongAnswerPenalty = 3 * WrongAnswerCount.LimitFromTop(3);
                    var timePenalty = (Timer.Elapsed.Seconds - 1).Limit(0, 5);
                    var penalty = wrongAnswerPenalty + timePenalty;
                    if (penalty == 0)
                    {
                        if (Session.PitchWeights[index] > 1)
                            Session.PitchWeights[index] -= 1;
                    }
                    else
                    {
                        Session.PitchWeights[index] += penalty;
                    }
                    SwitchToNextQuestion();
                }
                else
                {
                    ++WrongAnswerCount;
                }
            }
            Staff = Staff.WithPlayedPitch(pitch);
        }
    }
}
