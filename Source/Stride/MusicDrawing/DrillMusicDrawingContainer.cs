using System.Windows.Media;

namespace Stride.MusicDrawing
{
    public class DrillMusicDrawingContainer
    {
        public readonly GeometryDrawing StaffLines;
        public readonly GlyphRunDrawing TreebleClef, BassClef, TestNote, PlayedNote;

        public DrillMusicDrawingContainer()
        {
            var drawing = new DrawingGroup();
            StaffLines = new GeometryDrawing();
            TreebleClef = new GlyphRunDrawing();
            BassClef = new GlyphRunDrawing();
            TestNote = new GlyphRunDrawing();
            PlayedNote = new GlyphRunDrawing();
            drawing.Children.Add(StaffLines);
            drawing.Children.Add(TreebleClef);
            drawing.Children.Add(BassClef);
            drawing.Children.Add(TestNote);
            drawing.Children.Add(PlayedNote);
            Drawing = drawing;
        }

        public Drawing Drawing { get; }
    }
}