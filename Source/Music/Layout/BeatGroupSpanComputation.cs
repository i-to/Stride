using System.Collections.Generic;
using System.Linq;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Utility;
using Stride.Utility.Fluent;

namespace Stride.Music.Layout
{
    public class BeatGroupSpanComputation
    {
        public readonly StavesMetrics Metrics;

        public BeatGroupSpanComputation(StavesMetrics metrics)
        {
            Metrics = metrics;
        }

        public BeatGroupSpan ComputeGroupSpan(IReadOnlyList<ScoreNote> beatNotes)
        {
            var (minDuration, maxDuration) = beatNotes.Select(note => note.Duration).MinMax();
            return new BeatGroupSpan(
                leftMargin: GetNoteMargin(maxDuration),
                // todo: account for space to accomodate second offsets here
                span: GetNoteSpan(maxDuration),
                rightMargin: GetNoteMargin(minDuration)
            );
        }

        double GetNoteSpan(Duration duration) =>
            duration.IsWhole()
                ? Metrics.WholeNoteheadWidth
                : Metrics.OtherNoteheadWidth;

        double GetNoteMargin(Duration duration)
        {
            switch (duration)
            {
                case Duration.Whole:
                    return Metrics.WholeNoteMargin;
                case Duration.Half:
                    return Metrics.HalfNoteMargin;
                case Duration.Quarter:
                    return Metrics.QuarterNoteMargin;
                default:
                    return Metrics.OtherNoteMargin;
            }
        }
    }
}