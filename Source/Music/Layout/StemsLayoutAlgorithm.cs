using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StemsLayoutAlgorithm
    {
        readonly StavesMetrics Metrics;
        readonly VerticalLayoutAlgorithm VerticalLayout;

        public StemsLayoutAlgorithm(StavesMetrics metrics, VerticalLayoutAlgorithm verticalLayout)
        {
            Metrics = metrics;
            VerticalLayout = verticalLayout;
        }

        public IEnumerable<LayoutObject> Create(
            IReadOnlyDictionary<Beat, BeatGroup> beatGroups,
            IReadOnlyDictionary<Beat, double> tickPositions)
            => beatGroups.SelectMany(
                kv => CreateStemAndFlag(tickPositions[kv.Key], kv.Value));

        IEnumerable<LayoutObject> CreateStemAndFlag(
            double tickPosition,
            BeatGroup beatGroup)
        {
            // todo: handle groups correctly
            var scoreNote = beatGroup.ScoreNotes.First();
            if (scoreNote.Duration.IsWhole())
                yield break;

            var stemLength = Metrics.RegularStemLength;
            var position = scoreNote.StaffPosition;
            bool stemUp = position.VerticalOffset < 0;
            var (xOffset, yOffset, length) = stemUp
                ? (Metrics.OtherNoteheadWidth - Metrics.StemLineThickness, 0, -stemLength)
                : (0, 2, stemLength);
            var x = xOffset + tickPosition;
            var y = yOffset + VerticalLayout.StaffPositionToYOffset(position);
            var origin = new Point(x, y);
            var end = new Point(x, y + length);
            yield return new LineObject(origin, end, Metrics.StemLineThickness);

            if (scoreNote.Duration.IsHalf() || scoreNote.Duration.IsQuarter())
                yield break;
            var (flagY, symbol) = stemUp
                ? (y - stemLength, Symbol.FlagEighthUp)
                : (y + stemLength, Symbol.FlagEightsDown);
            var flagOrigin = new Point(x, flagY);
            yield return new SymbolObject(flagOrigin, symbol, Metrics.GlyphSize, false);
        }
    }
}