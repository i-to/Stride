using System;
using System.Windows;
using System.Windows.Media;
using Stride.Music;
using Stride.Utility;

namespace Stride.MusicDrawing
{
    /// <summary>
    /// Ledger lines at a given position of the single staff.
    /// </summary>
    public class LedgerLines
    {
        /// <summary>
        /// Indices represent number of legder lines on each side (0 - for no lines).
        /// </summary>
        public readonly int Top, Bottom;

        public LedgerLines(int top, int bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public static readonly LedgerLines Absent = new LedgerLines(0, 0);

        public static LedgerLines CreateTop(int count) => 
            new LedgerLines(count, 0);

        public static LedgerLines CreateBottom(int count) => 
            new LedgerLines(0, count);

        public static LedgerLines CreateSingle(int count, bool top) =>
            top ? CreateTop(count) : CreateBottom(count);

        public LedgerLines Combine(LedgerLines other)
        {
            var top = Math.Max(Top, other.Top);
            var bottom = Math.Max(Bottom, other.Bottom);
            return new LedgerLines(top, bottom);
        }
    }

    /// <summary>
    /// Ledger lines at a given position of the grand staff.
    /// </summary>
    public class GrandStaffLedgerLines
    {
        public readonly LedgerLines TreebleClef, BassClef;

        public GrandStaffLedgerLines(LedgerLines treebleClef, LedgerLines bassClef)
        {
            TreebleClef = treebleClef;
            BassClef = bassClef;
        }

        public static readonly GrandStaffLedgerLines Absent =
            new GrandStaffLedgerLines(LedgerLines.Absent, LedgerLines.Absent);

        public static GrandStaffLedgerLines CreateTreeble(LedgerLines lines) =>
            new GrandStaffLedgerLines(lines, LedgerLines.Absent);

        public static GrandStaffLedgerLines CreateBass(LedgerLines lines) =>
            new GrandStaffLedgerLines(LedgerLines.Absent, lines);

        public static GrandStaffLedgerLines CreateSingle(LedgerLines lines, bool treeble) =>
            treeble ? CreateTreeble(lines) : CreateBass(lines);

        public GrandStaffLedgerLines Combine(GrandStaffLedgerLines other)
        {
            var treeble = TreebleClef.Combine(other.TreebleClef);
            var bass = BassClef.Combine(other.BassClef);
            return new GrandStaffLedgerLines(treeble, bass);
        }
    }

    public class MusicDrawingBuilder
    {
        readonly StavesMetrics Metrics;
        readonly GlyphRunBuilder GlyphRunBuilder;
        readonly MusicSymbolToFontText MusicSymbolToFontText;
        readonly StaffGeometryBuilder StaffGeometryBuilder;
        readonly MusicTypefaceProvider MusicTypefaceProvider;
        readonly DrillMusicDrawingContainer DrawingContainer;

        public MusicDrawingBuilder(
            StavesMetrics metrics,
            GlyphRunBuilder glyphRunBuilder,
            MusicSymbolToFontText musicSymbolToFontText,
            StaffGeometryBuilder staffGeometryBuilder,
            MusicTypefaceProvider musicTypefaceProvider,
            DrillMusicDrawingContainer drawingContainer)
        {
            GlyphRunBuilder = glyphRunBuilder;
            StaffGeometryBuilder = staffGeometryBuilder;
            MusicTypefaceProvider = musicTypefaceProvider;
            DrawingContainer = drawingContainer;
            MusicSymbolToFontText = musicSymbolToFontText;
            Metrics = metrics;
        }

        public Drawing Drawing => DrawingContainer.Drawing;

        public double StaffLinesLength => 20.0 * Metrics.BaseSize;
        public Point TreebleClefOrigin => new Point(0, 4.0 * Metrics.BaseSize);
        public Point BassClefOrigin => new Point(0, 2.0 * Metrics.BaseSize + Metrics.GrandStaffOffset);
        public double NoteX => 10.0 * Metrics.BaseSize;

        double? StaffPositionToYOffset(StaffPosition position)
        {
            if (position == null)
                return null;
            var middleLinePosition = 4 * Metrics.BaseSize;
            var offset = position.Offset * Metrics.BaseSize;
            var cleffOffset = position.Clef == Clef.Bass ? Metrics.GrandStaffOffset : 0.0;
            return middleLinePosition + cleffOffset - offset;
        }

        public void UpdateDrawing(StaffPosition testNotePosition, StaffPosition playedNotePosition)
        {
            BuildCleffDrawing(MusicSymbolToFontText.TreebleClef, DrawingContainer.TreebleClef, TreebleClefOrigin);
            BuildCleffDrawing(MusicSymbolToFontText.BassClef, DrawingContainer.BassClef, BassClefOrigin);
            BuildNoteDrawing(DrawingContainer.TestNote, testNotePosition, Brushes.Black);
            BuildNoteDrawing(DrawingContainer.PlayedNote, playedNotePosition, Brushes.Red);
            var ledgerLines = LedgerLinesComputation.ComputeLedgerLines(testNotePosition, playedNotePosition);
            BuildStaffDrawing(DrawingContainer.StaffLines, ledgerLines);
        }

        void BuildCleffDrawing(char symbol, GlyphRunDrawing drawing, Point origin)
        {
            drawing.GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                MusicTypefaceProvider.Typeface,
                symbol.ToString(),
                origin,
                Metrics.GlyphSize);
            drawing.ForegroundBrush = Brushes.Black;
        }

        void BuildNoteDrawing(GlyphRunDrawing drawing, StaffPosition notePosition, Brush brush)
        {
            var testNoteY = StaffPositionToYOffset(notePosition);
            if (!testNoteY.HasValue)
            {
                drawing.GlyphRun = null;
                return;
            }
            var noteOrigin = new Point(NoteX, testNoteY.Value);
            var noteText = MusicSymbolToFontText.WholeNote.ToString();
            drawing.GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                MusicTypefaceProvider.Typeface, noteText, noteOrigin, Metrics.GlyphSize);
            drawing.ForegroundBrush = brush;
        }

        void BuildStaffDrawing(GeometryDrawing drawing, GrandStaffLedgerLines ledgerLines)
        {
            drawing.Geometry = StaffGeometryBuilder.CreateGrandStaffGeometry(Metrics, StaffLinesLength, ledgerLines, NoteX);
            drawing.Pen = new Pen { Brush = Brushes.Black, Thickness = Metrics.StaffLinesThickness };
        }
    }
}
