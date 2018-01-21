namespace Stride.Gui.Wpf.Model
{
    public class AnswerPerformance
    {
        public readonly int FullSecondsElapsed;
        public readonly int WrongAnswersCount;

        public AnswerPerformance(int fullSecondsElapsed, int wrongAnswersCount)
        {
            FullSecondsElapsed = fullSecondsElapsed;
            WrongAnswersCount = wrongAnswersCount;
        }
    }
}