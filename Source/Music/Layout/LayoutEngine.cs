using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Theory;
using Stride.Utility;
using Stride.Music.Score;

namespace Stride.Music.Layout
{
    public class LayoutEngine
    {
        readonly StavesMetrics Metrics;
        readonly StaffLinesLayout StaffLinesLayout;
        readonly LedgerLinesComputation LedgerLinesComputation;

        public LayoutEngine(
            StavesMetrics metrics,
            StaffLinesLayout staffLinesLayout,
            LedgerLinesComputation ledgerLinesComputation)
        {
            Metrics = metrics;
            StaffLinesLayout = staffLinesLayout;
            LedgerLinesComputation = ledgerLinesComputation;
        }

        Point TreebleClefOrigin => new Point(0, 4.0 * Metrics.BaseSize);
        Point BassClefOrigin => new Point(0, 2.0 * Metrics.BaseSize + Metrics.GrandStaffOffset);
        double FirstNoteXBias => 10.0 * Metrics.BaseSize;
        double StaffLinesLength => 50.0 * Metrics.BaseSize;
        double TickWidth => Metrics.BaseSize * 4.0;
        double BarlineWidth => Metrics.BaseSize * 2.0;

        SymbolObject PlaceTreebleClef() => new SymbolObject(TreebleClefOrigin, Symbol.TreebleClef);
        SymbolObject PlaceBassClef() => new SymbolObject(BassClefOrigin, Symbol.BassClef);
        IReadOnlyList<SymbolObject> PlaceClefs() => new[] {PlaceTreebleClef(), PlaceBassClef()};

        double StaffPositionToYOffset(StaffPosition position)
        {
            var middleLinePosition = 4 * Metrics.BaseSize;
            var offset = position.VerticalOffset * Metrics.BaseSize;
            var cleffOffset = position.Clef == Clef.Bass ? Metrics.GrandStaffOffset : 0.0;
            return middleLinePosition + cleffOffset - offset;
        }

        (IReadOnlyDictionary<Tick, double>, IReadOnlyList<double>) ComputeHorizontalLayout(
            double baseOffset,
            IEnumerable<IGrouping<int, NoteOnPage>> notesByBars)
        {
            var tickPositions = new Dictionary<Tick, double>();
            var barlinePositions = new List<double>();
            double currentOffset = baseOffset;
            foreach (var bar in notesByBars)
            {
                var ticksGroups = bar.GroupBy(note => note.Tick).OrderBy(group => group.Key);
                foreach (var tickGroup in ticksGroups)
                {
                    // todo: account for space to accomodate second offsets here
                    tickPositions.Add(tickGroup.Key, currentOffset);
                    currentOffset += TickWidth;
                }
                barlinePositions.Add(currentOffset);
                currentOffset += BarlineWidth;
            }
            return (tickPositions, barlinePositions);
        }

        SymbolObject CreateNoteSymbol(StaffPosition notePosition, double tickOffset)
        {
            var y = StaffPositionToYOffset(notePosition);
            var secondOffset = notePosition.HorisontalOffset ? Metrics.SecondNoteOffset : 0.0;
            var xOffset = tickOffset + secondOffset;
            var noteOrigin = new Point(xOffset, y);
            return new SymbolObject(noteOrigin, Symbol.WholeNote);
        }

        IReadOnlyList<SymbolObject> CreateNoteSymbols(
            IReadOnlyDictionary<Tick, double> tickOffsets,
            IReadOnlyList<NoteOnPage> notes) => 
            notes
                .Select(note => CreateNoteSymbol(note.StaffPosition, tickOffsets[note.Tick]))
                .ToReadOnlyList();

        IReadOnlyList<BarLineObject> CreateBarLines(IReadOnlyList<double> barlinePositions)
        {
            var middleOffset = 0.5 * BarlineWidth - Metrics.BarLineThickness;
            var yOrigin = 0;
            var yEnd = 8 * Metrics.BaseSize + Metrics.GrandStaffOffset;
            return barlinePositions
                .Select(x => CreateBarline(x + middleOffset, yOrigin, yEnd))
                .ToReadOnlyList();
        }

        BarLineObject CreateBarline(double x, double yOrigin, double yEnd)
        {
            var origin = new Point(x, yOrigin);
            var end = new Point(x, yEnd);
            return new BarLineObject(origin, end, BarLineStyle.Single);
        }

        public Layout CreateLayout(Page page)
        {
            var clefs = PlaceClefs();

            var allNotes = page.PageNotes.Concat(page.OverlayNotes);
            var notesByTicks = allNotes.GroupBy(note => note.Tick);
            var notesByBars = allNotes.GroupBy(note => note.Tick.Bar);

            var (tickPositions, barlinePositions) = ComputeHorizontalLayout(FirstNoteXBias, notesByBars);

            var ledgerLinesByTicks = notesByTicks
                .Select(group => (group.Key, LedgerLinesComputation.ComputeLedgerLines(group)));

            var ledgerLines = StaffLinesLayout.CreateLedgerLines(Metrics, ledgerLinesByTicks, tickPositions);

            var staffLines = StaffLinesLayout.CreateGrandStaffLines(Metrics, StaffLinesLength);
            var barLines = CreateBarLines(barlinePositions);

            var testPhraseSymbols = CreateNoteSymbols(tickPositions, page.PageNotes);
            var soundingNoteSymbols = CreateNoteSymbols(tickPositions, page.OverlayNotes);

            return new Layout(
                staffLines,
                ledgerLines,
                barLines,
                clefs,
                testPhraseSymbols,
                soundingNoteSymbols,
                Metrics.StaffLineThickness,
                Metrics.BarLineThickness,
                Metrics.GlyphSize);
        }
    }
}
