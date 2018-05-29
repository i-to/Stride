using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class DurationsLayout
    {
        readonly StavesMetrics Metrics;
        readonly VerticalLayout VerticalLayout;

        public DurationsLayout(StavesMetrics metrics, VerticalLayout verticalLayout)
        {
            Metrics = metrics;
            VerticalLayout = verticalLayout;
        }

        public IEnumerable<LayoutObject> Create(
            IEnumerable<BeatGroup> beatGroups,
            IReadOnlyDictionary<Beat, double> tickPositions) =>
            CreateStemsAndFlags(beatGroups, tickPositions);

        IEnumerable<LayoutObject> CreateStemsAndFlags(
            IEnumerable<BeatGroup> beatGroups,
            IReadOnlyDictionary<Beat, double> tickPositions) =>
            beatGroups.SelectMany(note => CreateStemAndFlag(tickPositions, note));

        IEnumerable<LayoutObject> CreateStemAndFlag(IReadOnlyDictionary<Beat, double> tickPositions, BeatGroup beatGroup)
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
            var x = xOffset + tickPositions[beatGroup.Beat];
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