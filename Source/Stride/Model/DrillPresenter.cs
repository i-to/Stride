using System;
using Stride.Music;
using Stride.Utility;

namespace Stride.Model
{
    public class DrillPresenter
    {
        readonly Random Random;
        readonly AnswerTracker AnswerTracker;
        readonly PerformanceFeedback PerformanceFeedback;

        public DrillPresenter(AnswerTracker answerTracker, PerformanceFeedback performanceFeedback)
        {
            Random = new Random();
            AnswerTracker = answerTracker;
            PerformanceFeedback = performanceFeedback;
        }

        DrillSession Session;
        public Pitch TestPitch { get; private set; }
        public Pitch PlayedPitch { get; private set; }

        public void Start(DrillSession session)
        {
            Session = session;
            SwitchToNextQuestion();
        }

        Pitch ComputeNextPitch()
        {
            var random = Random.NextDouble();
            var index = WeighedDistribution.BucketIndexOfValue(Session.PitchWeights, random);
            return Session.Pitches[index];
        }

        public void SwitchToNextQuestion()
        {
            TestPitch = ComputeNextPitch();
            AnswerTracker.Reset();
        }

        public void SetPlayedPitch(Pitch pitch)
        {
            if (pitch == null)
            {
                if (TestPitch == PlayedPitch)
                {
                    var index = Session.Pitches.FindIndex(TestPitch);
                    var performance = AnswerTracker.AnswerPerformance;
                    PerformanceFeedback.UpdateWeight(ref Session.PitchWeights[index], performance);
                    SwitchToNextQuestion();
                }
                else
                {
                    AnswerTracker.OnWrongAnswer();
                }
            }
            PlayedPitch = pitch;
        }
    }
}
