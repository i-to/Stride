using System;
using System.Windows;
using System.Windows.Media;

namespace Stride.MusicDrawing
{
    public class MusicDrawingBuilder
    {
        readonly DrawingMetrics DrawingMetrics;
        readonly GlyphRunBuilder GlyphRunBuilder;
        readonly MusicSymbolToFontText MusicSymbolToFontText;
        readonly StaffLinesGeometryBuilder StaffLinesGeometryBuilder;
        readonly MusicTypefaceProvider MusicTypefaceProvider;
        readonly DrillMusicDrawingContainer DrawingContainer;

        public MusicDrawingBuilder(
            DrawingMetrics drawingMetrics,
            GlyphRunBuilder glyphRunBuilder,
            MusicSymbolToFontText musicSymbolToFontText,
            StaffLinesGeometryBuilder staffLinesGeometryBuilder,
            MusicTypefaceProvider musicTypefaceProvider,
            DrillMusicDrawingContainer drawingContainer)
        {
            GlyphRunBuilder = glyphRunBuilder;
            StaffLinesGeometryBuilder = staffLinesGeometryBuilder;
            MusicTypefaceProvider = musicTypefaceProvider;
            DrawingContainer = drawingContainer;
            MusicSymbolToFontText = musicSymbolToFontText;
            DrawingMetrics = drawingMetrics;
        }

        public Drawing Drawing => DrawingContainer.Drawing;

        public double StaffLinesLength => 20.0 * DrawingMetrics.BaseSize;
        public Point GClefOrigin => new Point(0, 4.0 * DrawingMetrics.BaseSize);
        public double NoteX => 10.0 * DrawingMetrics.BaseSize;

        string ClefText => MusicSymbolToFontText.TreebleClef.ToString();
        string NoteText => MusicSymbolToFontText.WholeNote.ToString();

        double? StaffPositionToYOffset(int staffPosition)
        {
            if (staffPosition < -1 || staffPosition > 10)
                throw new ArgumentException($"The expected range for {nameof(staffPosition)} is [-1, 10]");
            if (staffPosition == -1)
                return null;
            return (9 - staffPosition) * DrawingMetrics.BaseSize;
        }

        // Staff position parameter indicates a number of semi-tones above the
        // D under the first staff line. Thus, numbers in the range [0, 10] represent
        // all notes drawable without ledger lines (considering only G clef for now).
        // A special value of -1 indicates that the note is not to be shown.
        public void UpdateDrawing(int testNoteStaffPosition, int playedNoteStaffPosition)
        {
            DrawingContainer.ClefDrawing.GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                MusicTypefaceProvider.Typeface, ClefText, GClefOrigin, DrawingMetrics.GlyphSize);
            DrawingContainer.ClefDrawing.ForegroundBrush = Brushes.Black;

            SetupNoteDrawing(DrawingContainer.TestNoteDrawing, testNoteStaffPosition, Brushes.Black);
            SetupNoteDrawing(DrawingContainer.PlayedNoteDrawing, playedNoteStaffPosition, Brushes.Red);

            DrawingContainer.StaffLinesDrawing.Geometry = StaffLinesGeometryBuilder.CreateStaffLinesGeometry(
                DrawingMetrics.StaffLinesOrigin, StaffLinesLength, DrawingMetrics.StaffLinesDistance);
            DrawingContainer.StaffLinesDrawing.Pen =
                new Pen { Brush = Brushes.Black, Thickness = DrawingMetrics.StaffLinesThickness };
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
            drawing.GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                MusicTypefaceProvider.Typeface, NoteText, noteOrigin, DrawingMetrics.GlyphSize);
            drawing.ForegroundBrush = brush;
        }
    }
}
