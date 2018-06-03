using System.Collections.Generic;
using Stride.Music.Score;

namespace Stride.Music.Layout
{
    public class HorizontalLayoutAlgorithm
    {
        readonly StavesMetrics Metrics;

        public HorizontalLayoutAlgorithm(StavesMetrics metrics)
        {
            Metrics = metrics;
        }

        public (IReadOnlyDictionary<Beat, double> BeatPositions, IReadOnlyList<double> BarlinePositions)
            ComputeHorizontalLayout(
                double baseOffset,
                IEnumerable<IEnumerable<Beat>> barsActiveBeats,
                IReadOnlyDictionary<Beat, BeatGroup> beatGroups,
                IReadOnlyDictionary<Beat, BeatGroupSpan> groupSpans
            )
        {
            var beatPositions = new Dictionary<Beat, double>();
            var barlinePositions = new List<double>();
            double currentOffset = baseOffset;

            foreach (var barActiveBeats in barsActiveBeats)
            {
                //var beatGroups = bar.GroupBy(note => note.Beat).OrderBy(group => group.Key);
                foreach (var beat in barActiveBeats)
                {
                    var groupSpan = groupSpans[beat];
                    currentOffset += groupSpan.LeftMargin;
                    beatPositions.Add(beat, currentOffset);
                    currentOffset += groupSpan.Span;
                    currentOffset += groupSpan.RightMargin;
                }
                barlinePositions.Add(currentOffset);
                currentOffset += Metrics.BarLineThickness;
            }
            return (beatPositions, barlinePositions);
        }
    }
}