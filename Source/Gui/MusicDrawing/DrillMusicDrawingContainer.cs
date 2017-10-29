using System.Windows.Media;

namespace Stride.Gui.MusicDrawing
{
    public class DrillMusicDrawingContainer
    {
        public readonly GeometryDrawing StaffLines;
        public readonly GlyphRunDrawing TreebleClef, BassClef, TestNote;
        public readonly DrawingGroup SoundingNotes;

        public DrillMusicDrawingContainer()
        {
            var drawing = new DrawingGroup();
            StaffLines = new GeometryDrawing();
            TreebleClef = new GlyphRunDrawing();
            BassClef = new GlyphRunDrawing();
            TestNote = new GlyphRunDrawing();
            SoundingNotes = new DrawingGroup();
            drawing.Children.Add(StaffLines);
            drawing.Children.Add(TreebleClef);
            drawing.Children.Add(BassClef);
            drawing.Children.Add(TestNote);
            drawing.Children.Add(SoundingNotes);
            Drawing = drawing;
        }

        public Drawing Drawing { get; }
    }
}