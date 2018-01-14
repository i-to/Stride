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
            IEnumerable<NoteOnPage> pageNotes,
            IReadOnlyDictionary<Tick, double> tickPositions) =>
            CreateStemsAndFlags(pageNotes, tickPositions);

        IEnumerable<LayoutObject> CreateStemsAndFlags(
            IEnumerable<NoteOnPage> pageNotes,
            IReadOnlyDictionary<Tick, double> tickPositions) => 
            pageNotes.SelectMany(note => CreateStemAndFlag(tickPositions, note));

        IEnumerable<LayoutObject> CreateStemAndFlag(IReadOnlyDictionary<Tick, double> tickPositions, NoteOnPage note)
        {
            if (note.Duration.IsWhole())
                yield break;

            var stemLength = Metrics.RegularStemLength;
            var position = note.StaffPosition;
            bool stemUp = position.VerticalOffset < 0;
            var (xOffset, yOffset, length) = stemUp
                ? (Metrics.OtherNoteheadWidth - Metrics.StemLineThickness, 0, -stemLength)
                : (0, 2, stemLength);
            var x = xOffset + tickPositions[note.Tick];
            var y = yOffset + VerticalLayout.StaffPositionToYOffset(position);
            var origin = new Point(x, y);
            var end = new Point(x, y + length);
            yield return new LineObject(origin, end, Metrics.StemLineThickness);

            if (note.Duration.IsHalf() || note.Duration.IsQuarter())
                yield break;
            var (flagY, symbol) = stemUp
                ? (y - stemLength, Symbol.FlagEighthUp)
                : (y + stemLength, Symbol.FlagEightsDown);
            var flagOrigin = new Point(x, flagY);
            yield return new SymbolObject(flagOrigin, symbol, Metrics.GlyphSize, false);
        }
    }
}