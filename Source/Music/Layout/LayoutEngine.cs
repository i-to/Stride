using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Presentation;
using Stride.Music.Theory;
using Stride.Utility;
using MoreLinq;

namespace Stride.Music.Layout
{
    public class LayoutEngine
    {
        readonly StavesMetrics Metrics;
        readonly StaffLinesBuilder StaffLinesBuilder;

        public LayoutEngine(StavesMetrics metrics, StaffLinesBuilder staffLinesBuilder)
        {
            Metrics = metrics;
            StaffLinesBuilder = staffLinesBuilder;
        }

        Point TreebleClefOrigin => new Point(0, 4.0 * Metrics.BaseSize);
        Point BassClefOrigin => new Point(0, 2.0 * Metrics.BaseSize + Metrics.GrandStaffOffset);
        double NoteX => 10.0 * Metrics.BaseSize;
        double StaffLinesLength => 20.0 * Metrics.BaseSize;

        SymbolObject PlaceTreebleClef() => new SymbolObject(TreebleClefOrigin, Symbol.TreebleClef);
        SymbolObject PlaceBassClef() => new SymbolObject(BassClefOrigin, Symbol.BassClef);

        double StaffPositionToYOffset(StaffPosition position)
        {
            var middleLinePosition = 4 * Metrics.BaseSize;
            var offset = position.VerticalOffset * Metrics.BaseSize;
            var cleffOffset = position.Clef == Clef.Bass ? Metrics.GrandStaffOffset : 0.0;
            return middleLinePosition + cleffOffset - offset;
        }

        SymbolObject BuildNoteSymbol(StaffPosition notePosition)
        {
            var y = StaffPositionToYOffset(notePosition);
            var xOffset = notePosition.HorisontalOffset ? Metrics.SecondNoteOffset : 0.0;
            var noteOrigin = new Point(NoteX + xOffset, y);
            return new SymbolObject(noteOrigin, Symbol.WholeNote);
        }

        Layout CreateLayout(StaffPosition testNote, IEnumerable<StaffPosition> soundingNotes)
        {
            var bassClef = PlaceBassClef();
            var treebleClef = PlaceTreebleClef();
            var ledgerLines = LedgerLinesComputation.ComputeLedgerLines(testNote.Concat(soundingNotes));
            var lines = StaffLinesBuilder.CreateGrandStaffGeometry(Metrics, StaffLinesLength, ledgerLines, NoteX);
            var testNoteSymbol = BuildNoteSymbol(testNote);
            var soundingNoteSymbols = soundingNotes.Select(BuildNoteSymbol).ToReadOnlyList();
            return new Layout(
                lines,
                bassClef,
                treebleClef,
                testNoteSymbol,
                soundingNoteSymbols,
                Metrics.LineThickness,
                Metrics.GlyphSize);
        }

        public Layout CreateLayout(
            Pitch lowestTreebleStaffPitch,
            Pitch testPitch,
            IEnumerable<Pitch> soundingPitches)
        {
            var testNoteStaffPosition = StaffPositionComputation.ComputeStaffPosition(
                lowestTreebleStaffPitch, testPitch);
            var soundingNotesStaffPositions = StaffPositionComputation.ComputeStaffPositions(
                lowestTreebleStaffPitch, soundingPitches);
            return CreateLayout(testNoteStaffPosition, soundingNotesStaffPositions);
        }
    }
}
