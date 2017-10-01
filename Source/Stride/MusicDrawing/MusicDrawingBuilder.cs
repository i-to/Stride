using System;
using System.Windows;
using System.Windows.Media;

namespace Stride.MusicDrawing
{
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

        double? StaffPositionToYOffset(int staffPosition)
        {
            if (staffPosition < -1 || staffPosition > 10)
                throw new ArgumentException($"The expected range for {nameof(staffPosition)} is [-1, 10]");
            if (staffPosition == -1)
                return null;
            return (9 - staffPosition) * Metrics.BaseSize;
        }

        // Staff position parameter indicates a number of semi-tones above the
        // D under the first staff line. Thus, numbers in the range [0, 10] represent
        // all notes drawable without ledger lines (considering only G clef for now).
        // A special value of -1 indicates that the note is not to be shown.
        public void UpdateDrawing(int testNoteStaffPosition, int playedNoteStaffPosition)
        {
            SetupCleffDrawing(MusicSymbolToFontText.TreebleClef, DrawingContainer.TreebleClef, TreebleClefOrigin);
            SetupCleffDrawing(MusicSymbolToFontText.BassClef, DrawingContainer.BassClef, BassClefOrigin);
            SetupNoteDrawing(DrawingContainer.TestNote, testNoteStaffPosition, Brushes.Black);
            SetupNoteDrawing(DrawingContainer.PlayedNote, playedNoteStaffPosition, Brushes.Red);
            SetupStavesDrawing(DrawingContainer.StaffLines);
        }

        void SetupCleffDrawing(char symbol, GlyphRunDrawing drawing, Point origin)
        {
            drawing.GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                MusicTypefaceProvider.Typeface,
                symbol.ToString(),
                origin,
                Metrics.GlyphSize);
            drawing.ForegroundBrush = Brushes.Black;
        }

        void SetupNoteDrawing(GlyphRunDrawing drawing, int staffPosition, Brush brush)
        {
            var testNoteY = StaffPositionToYOffset(staffPosition);
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

        void SetupStavesDrawing(GeometryDrawing drawing)
        {
            drawing.Geometry = StaffGeometryBuilder.CreateGrandStaffGeometry(Metrics, StaffLinesLength);
            drawing.Pen = new Pen { Brush = Brushes.Black, Thickness = Metrics.StaffLinesThickness };
        }

    }
}
