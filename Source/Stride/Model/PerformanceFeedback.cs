using Stride.Utility;

namespace Stride.Model
{
    public class PerformanceFeedback
    {
        public readonly int PenaltyMultiplierPerWrongAnswer = 3;
        public readonly int MaxPenaliziableWrongAnswers = 3;
        public readonly int MaxPenalizableSeconds = 5;
        public readonly int SecondsPenaltyMultiplier = 1;
        public readonly int WeightDecrementPerCorrectAnswer = 1;
        public readonly int MinimalWeightValue = 1;

        public void UpdateWeight(ref int weight, AnswerPerformance performance)
        {
            var wrongAnswerPenalty = PenaltyMultiplierPerWrongAnswer
                                     * performance.WrongAnswersCount.LimitFromTop(MaxPenaliziableWrongAnswers);
            var timePenalty = SecondsPenaltyMultiplier * 
                              performance.FullSecondsElapsed.LimitFromTop(MaxPenalizableSeconds);
            var penalty = wrongAnswerPenalty + timePenalty;
            if (penalty == 0)
            {
                if (weight - MinimalWeightValue >= WeightDecrementPerCorrectAnswer)
                    weight -= WeightDecrementPerCorrectAnswer;
            }
            else
            {
                weight += penalty;
            }
        }
    }
}