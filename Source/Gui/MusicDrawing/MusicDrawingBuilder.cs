using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MoreLinq;
using Stride.Music.Layout;
using Stride.Music.Presentation;
using Stride.Music.Theory;

namespace Stride.Gui.MusicDrawing
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
            var offset = position.VerticalOffset * Metrics.BaseSize;
            var cleffOffset = position.Clef == Clef.Bass ? Metrics.GrandStaffOffset : 0.0;
            return middleLinePosition + cleffOffset - offset;
        }

        public void BuildDrawing(StaffPosition testNotePosition, IEnumerable<StaffPosition> soundingNotePositions)
        {
            BuildCleffDrawing(MusicSymbolToFontText.TreebleClef, DrawingContainer.TreebleClef, TreebleClefOrigin);
            BuildCleffDrawing(MusicSymbolToFontText.BassClef, DrawingContainer.BassClef, BassClefOrigin);
            BuildNoteDrawing(DrawingContainer.TestNote, testNotePosition, Brushes.Black);
            BuildSoundingNotesDrawing(DrawingContainer.SoundingNotes, soundingNotePositions, Brushes.Red);
            var ledgerLines = LedgerLinesComputation.ComputeLedgerLines(testNotePosition.Concat(soundingNotePositions));
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

        void BuildSoundingNotesDrawing(
            DrawingGroup drawingGroup,
            IEnumerable<StaffPosition> notePositions,
            SolidColorBrush brush)
        {
            var drawings = drawingGroup.Children;
            drawings.Clear();
            foreach (var notePosition in notePositions)
            {
                var drawing = new GlyphRunDrawing();
                BuildNoteDrawing(drawing, notePosition, brush);
                drawings.Add(drawing);
            }
        }

        void BuildNoteDrawing(GlyphRunDrawing drawing, StaffPosition notePosition, Brush brush)
        {
            var testNoteY = StaffPositionToYOffset(notePosition);
            if (!testNoteY.HasValue)
            {
                drawing.GlyphRun = null;
                return;
            }
            var xOffset = notePosition.HorisontalOffset ? Metrics.SecondNoteOffset : 0.0;
            var noteOrigin = new Point(NoteX + xOffset, testNoteY.Value);
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
