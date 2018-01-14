using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MoreLinq;
using Stride.Music.Theory;
using Stride.Utility;
using Stride.Music.Score;

namespace Stride.Music.Layout
{
    public class PageLayout : Layout
    {
        readonly StavesMetrics Metrics;
        readonly StaffLinesLayout StaffLinesLayout;
        readonly LedgerLinesComputation LedgerLinesComputation;
        readonly VerticalLayout VerticalLayout;
        readonly DurationsLayout DurationsLayout;

        public PageLayout(
            StavesMetrics metrics,
            StaffLinesLayout staffLinesLayout,
            LedgerLinesComputation ledgerLinesComputation,
            VerticalLayout verticalLayout,
            DurationsLayout durationsLayout)
        {
            Metrics = metrics;
            StaffLinesLayout = staffLinesLayout;
            LedgerLinesComputation = ledgerLinesComputation;
            VerticalLayout = verticalLayout;
            DurationsLayout = durationsLayout;
        }

        Point TreebleClefOrigin => new Point(0, 4.0 * Metrics.HalfSpace);
        Point BassClefOrigin => new Point(0, 2.0 * Metrics.HalfSpace + Metrics.GrandStaffOffset);
        double FirstNoteXBias => 10.0 * Metrics.HalfSpace;

        SymbolObject PlaceTreebleClef() => new SymbolObject(TreebleClefOrigin, Symbol.TreebleClef, Metrics.GlyphSize, false);
        SymbolObject PlaceBassClef() => new SymbolObject(BassClefOrigin, Symbol.BassClef, Metrics.GlyphSize, false);


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
                    var (leftMargin, span, rightMargin) = GetTickDimensions(tickGroup);
                    currentOffset += leftMargin;
                    tickPositions.Add(tickGroup.Key, currentOffset);
                    currentOffset += span;
                    currentOffset += rightMargin;
                }
                barlinePositions.Add(currentOffset);
                currentOffset += Metrics.BarLineThickness;
            }
            return (tickPositions, barlinePositions);
        }

        (double leftMargin, double span, double rightMargin) GetTickDimensions(IEnumerable<NoteOnPage> tickNotes)
        {
            var (minDuration, maxDuration) = tickNotes.Select(note => note.Duration).MinMax();
            return (
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

        SymbolObject CreateNoteSymbol(StaffPosition notePosition, Duration duration, double tickOffset, bool highlighted)
        {
            var secondOffset = notePosition.HorisontalOffset ? Metrics.SecondNoteOffset : 0.0;
            var y = VerticalLayout.StaffPositionToYOffset(notePosition);
            var xOffset = tickOffset + secondOffset;
            var noteOrigin = new Point(xOffset, y);
            var symbol = GetNoteheadSymbol(duration);
            return new SymbolObject(noteOrigin, symbol, Metrics.GlyphSize, highlighted);
        }

        Symbol GetNoteheadSymbol(Duration duration)
        {
            switch (duration)
            {
                case Duration.Whole:
                    return Symbol.NoteheadWhole;
                case Duration.Half:
                    return Symbol.NoteheadHalf;
                case Duration.Quarter:
                case Duration.Eighth:
                case Duration.Sixteenth:
                    return Symbol.NoteheadBlack;
            }
            throw new InvalidOperationException();
        }

        IEnumerable<SymbolObject> CreateNoteSymbols(
            IReadOnlyDictionary<Tick, double> tickOffsets,
            IReadOnlyList<NoteOnPage> notes,
            bool highlighted) =>
            notes.Select(note => CreateNoteSymbol(note.StaffPosition, note.Duration, tickOffsets[note.Tick], highlighted));

        IEnumerable<LineObject> CreateBarLines(IEnumerable<double> barlinePositions)
        {
            var yOrigin = 0;
            var yEnd = 8 * Metrics.HalfSpace + Metrics.GrandStaffOffset;
            return barlinePositions
                .Select(x => CreateBarline(x + 0, yOrigin, yEnd))
                .ToReadOnlyList();
        }

        LineObject CreateBarline(double x, double yOrigin, double yEnd)
        {
            var origin = new Point(x, yOrigin);
            var end = new Point(x, yEnd);
            return new LineObject(origin, end, Metrics.BarLineThickness);
        }

        IEnumerable<(Tick, (GrandStaffLedgerLines, bool isWholeNote))> ComputeLedgerLinesForTicks(
            IEnumerable<IGrouping<Tick, NoteOnPage>> notesByTicks) => 
            notesByTicks.Select(ComputeLedgerLinesForTick);

        bool HasWholeNote(IEnumerable<NoteOnPage> notes) =>
            notes.Aggregate(false, (hasWholeNote, note) => hasWholeNote || note.Duration.IsWhole());

        (Tick, (GrandStaffLedgerLines, bool isWholeNote)) ComputeLedgerLinesForTick(IGrouping<Tick, NoteOnPage> group) => 
            (group.Key, (LedgerLinesComputation.ComputeLedgerLines(group), HasWholeNote(group)));

        public IEnumerable<LayoutObject> CreateLayout(Page page)
        {
            var allNotes = page.PageNotes.Concat(page.OverlayNotes);
            var notesByTicks = allNotes.GroupBy(note => note.Tick);
            var notesByBars = allNotes.GroupBy(note => note.Tick.Bar);

            var (tickPositions, barlinePositions) = ComputeHorizontalLayout(FirstNoteXBias, notesByBars);

            var ledgerLinesByTicks = ComputeLedgerLinesForTicks(notesByTicks);

            var ledgerLines = StaffLinesLayout.CreateLedgerLines(Metrics, ledgerLinesByTicks, tickPositions);
            var staffLines = StaffLinesLayout.CreateGrandStaffLines(Metrics);
            var stems = DurationsLayout.Create(page.PageNotes, tickPositions);
            var barlines = CreateBarLines(barlinePositions);

            var notes = CreateNoteSymbols(tickPositions, page.PageNotes, false);
            var playedNotes = CreateNoteSymbols(tickPositions, page.OverlayNotes, true);

            return (new[] {PlaceBassClef(), PlaceTreebleClef()} as IEnumerable<LayoutObject>)
                .Concat(staffLines)
                .Concat(ledgerLines)
                .Concat(stems)
                .Concat(barlines)
                .Concat(notes)
                .Concat(playedNotes);
        }
    }
}
