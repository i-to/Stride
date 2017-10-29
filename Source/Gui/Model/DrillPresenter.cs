using System;
using System.Collections.Generic;
using Stride.Music;
using Stride.Utility;

namespace Stride.Gui.Model
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
            PlayingPitches = new List<Pitch>();
        }

        DrillSession Session;
        readonly List<Pitch> PlayingPitches;
        public Pitch TestPitch { get; private set; }
        public IReadOnlyList<Pitch> SoundingPitches => PlayingPitches;
        public Pitch LowestTreebleStaffPitch => Session.Drill.LowestTreebleSaffPitch;

        public void Start(DrillSession session)
        {
            Session = session;
            SwitchToNextQuestion();
        }

        Pitch ComputeNextPitch()
        {
            var random = Random.NextDouble();
            var index = WeighedDistribution.BucketIndexOfValue(Session.PitchWeights, random);
            return Session.Drill.Pitches[index];
        }

        public void SwitchToNextQuestion()
        {
            TestPitch = ComputeNextPitch();
            AnswerTracker.Reset();
        }

        void CheckAnswer(Pitch testPitch, Pitch playedPitch)
        {
            if (PlayingPitches.Count == 1 && testPitch == playedPitch)
            {
                var index = Session.Drill.Pitches.FindIndex(testPitch);
                var performance = AnswerTracker.AnswerPerformance;
                PerformanceFeedback.UpdateWeight(ref Session.PitchWeights[index], performance);
                SwitchToNextQuestion();
            }
            else
            {
                AnswerTracker.OnWrongAnswer();
            }
        }

        public void PitchOn(Pitch pitch)
        {
            PlayingPitches.Add(pitch);
        }

        public void PitchOff(Pitch pitch)
        {
            CheckAnswer(TestPitch, pitch);
            PlayingPitches.Remove(pitch);
        }
    }
}
