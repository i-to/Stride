using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MoreLinq;
using Stride.Music.Theory;
using Stride.Utility;
using Stride.Music.Score;
using Stride.Utility.Fluent;

namespace Stride.Music.Layout
{
    public class ScoreLayoutAlgorithm
    {
        public readonly StavesMetrics Metrics;
        public readonly StaffLinesLayoutAlgorithm StaffLinesLayout;
        public readonly BeatGroupLayoutAlgorithm BeatGroupLayoutAlgorithm;
        public readonly LedgerLinesComputation LedgerLinesComputation;
        public readonly BeatGroupSpanComputation BeatGroupSpanComputation;
        public readonly HorizontalLayoutAlgorithm HorizontalLayout;
        public readonly VerticalLayoutAlgorithm VerticalLayout;
        public readonly StemsLayoutAlgorithm StemsLayout;

        public ScoreLayoutAlgorithm(
            StavesMetrics metrics,
            StaffLinesLayoutAlgorithm staffLinesLayout,
            LedgerLinesComputation ledgerLinesComputation,
            BeatGroupLayoutAlgorithm beatGroupLayoutAlgorithm,
            BeatGroupSpanComputation beatGroupSpanComputation,
            HorizontalLayoutAlgorithm horizontalLayout,
            VerticalLayoutAlgorithm verticalLayout,
            StemsLayoutAlgorithm stemsLayout)
        {
            Metrics = metrics;
            StaffLinesLayout = staffLinesLayout;
            LedgerLinesComputation = ledgerLinesComputation;
            VerticalLayout = verticalLayout;
            StemsLayout = stemsLayout;
            BeatGroupLayoutAlgorithm = beatGroupLayoutAlgorithm;
            BeatGroupSpanComputation = beatGroupSpanComputation;
            HorizontalLayout = horizontalLayout;
        }

        Point TreebleClefOrigin => new Point(0, 4.0 * Metrics.HalfSpace);
        Point BassClefOrigin => new Point(0, 2.0 * Metrics.HalfSpace + Metrics.GrandStaffOffset);
        double FirstNoteXBias => 10.0 * Metrics.HalfSpace;

        SymbolObject PlaceTreebleClef() => new SymbolObject(TreebleClefOrigin, Symbol.TreebleClef, Metrics.GlyphSize, false);
        SymbolObject PlaceBassClef() => new SymbolObject(BassClefOrigin, Symbol.BassClef, Metrics.GlyphSize, false);
        
        SymbolObject CreateNoteSymbol(StaffPosition notePosition, Duration duration, double tickOffset, bool highlighted = false)
        {
            //var secondOffset = notePosition.HorisontalOffset ? Metrics.SecondNoteOffset : 0.0;
            var secondOffset = 0.0;
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

        IEnumerable<SymbolObject> CreateNoteSymbols(BeatGroup beatGroup, double beatOffset)
            => beatGroup.ScoreNotes.Select(
                note => CreateNoteSymbol(note.StaffPosition, note.Duration, beatOffset));

        IEnumerable<SymbolObject> CreateNoteSymbols(
            IReadOnlyDictionary<Beat, BeatGroup> beatGroups,
            IReadOnlyDictionary<Beat, double> beatOffsets)
            => beatGroups.SelectMany(
                kv => CreateNoteSymbols(kv.Value, beatOffsets[kv.Key]));

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

        IReadOnlyDictionary<Beat, (GrandStaffLedgerLines, bool isWholeNote)> ComputeLedgerLinesForBeats(
            IReadOnlyDictionary<Beat, BeatGroup> beatGroups)
            => beatGroups.MapValues(ComputeLedgerLinesForBeat);

        bool HasWholeNote(IEnumerable<ScoreNote> group) 
            => group.Aggregate(false, (hasWholeNote, note) => hasWholeNote || note.Duration.IsWhole());

        (GrandStaffLedgerLines, bool isWholeNote) ComputeLedgerLinesForBeat(BeatGroup beatGroup) 
            => (
                LedgerLinesComputation.ComputeLedgerLines(beatGroup.ScoreNotes),
                HasWholeNote(beatGroup.ScoreNotes)
                );

        IReadOnlyList<IReadOnlyList<Beat>> GroupByBars(IEnumerable<Beat> beats)
            => beats
               .GroupBy(beat => beat.Bar)
               .Select(g => g.ToReadOnlyList())
               .ToReadOnlyList();

        IReadOnlyDictionary<Beat, BeatGroupSpan> ComputeGroupSpans(
            IReadOnlyDictionary<Beat, BeatGroup> beatGroups)
            => beatGroups.MapValues(
                    beatGroup => BeatGroupSpanComputation.ComputeGroupSpan(beatGroup.ScoreNotes));


        public IEnumerable<LayoutObject> CreateLayout(IReadOnlyDictionary<Beat, BeatGroup> score)
        {
            var barsActiveBeats = GroupByBars(score.Keys);
            var groupSpans = ComputeGroupSpans(score);
            var (beatPositions, barlinePositions) = HorizontalLayout.ComputeHorizontalLayout(
                FirstNoteXBias,
                barsActiveBeats,
                score,
                groupSpans);
            var ledgerLinesByBeats = ComputeLedgerLinesForBeats(score);

            var ledgerLines = StaffLinesLayout.CreateLedgerLines(Metrics, ledgerLinesByBeats, beatPositions);
            var staffLines = StaffLinesLayout.CreateGrandStaffLines(Metrics);
            var stems = StemsLayout.Create(barsActiveBeats, score, beatPositions);
            var barlines = CreateBarLines(barlinePositions);

            var noteSymbols = CreateNoteSymbols(score, beatPositions);

            return (new[] {PlaceBassClef(), PlaceTreebleClef()} as IEnumerable<LayoutObject>)
                .Concat(staffLines)
                .Concat(ledgerLines)
                .Concat(stems)
                .Concat(barlines)
                .Concat(noteSymbols);
        }
    }
}
