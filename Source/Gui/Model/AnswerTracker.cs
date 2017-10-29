using System.Diagnostics;

namespace Stride.Gui.Model
{
    public class AnswerTracker
    {
        readonly Stopwatch Timer;
        int WrongAnswerCount;

        public AnswerTracker()
        {
            Timer = new Stopwatch();
        }

        public void OnWrongAnswer() =>
            ++WrongAnswerCount;

        public AnswerPerformance AnswerPerformance =>
            new AnswerPerformance(
                fullSecondsElapsed: (int)Timer.Elapsed.TotalSeconds,
                wrongAnswersCount: WrongAnswerCount);

        public void Reset()
        {
            Timer.Reset();
            WrongAnswerCount = 0;
        }
    }
}