using System.Windows;
using System.Windows.Media;
using Stride.Music;

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
            SetupCleffDrawing(MusicSymbolToFontText.TreebleClef, DrawingContainer.TreebleClef, TreebleClefOrigin);
            SetupCleffDrawing(MusicSymbolToFontText.BassClef, DrawingContainer.BassClef, BassClefOrigin);
            SetupNoteDrawing(DrawingContainer.TestNote, testNotePosition, Brushes.Black);
            SetupNoteDrawing(DrawingContainer.PlayedNote, playedNotePosition, Brushes.Red);
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

        void SetupNoteDrawing(GlyphRunDrawing drawing, StaffPosition notePosition, Brush brush)
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

        void SetupStavesDrawing(GeometryDrawing drawing)
        {
            drawing.Geometry = StaffGeometryBuilder.CreateGrandStaffGeometry(Metrics, StaffLinesLength);
            drawing.Pen = new Pen { Brush = Brushes.Black, Thickness = Metrics.StaffLinesThickness };
        }

    }
}
