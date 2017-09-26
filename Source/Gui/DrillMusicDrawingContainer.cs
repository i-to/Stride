using System.Windows.Media;

namespace Stride.Gui
{
    public class DrillMusicDrawingContainer
    {
        public readonly GeometryDrawing StaffLinesDrawing;
        public readonly GlyphRunDrawing ClefDrawing, TestNoteDrawing, PlayedNoteDrawing;

        public DrillMusicDrawingContainer()
        {
            var drawing = new DrawingGroup();
            StaffLinesDrawing = new GeometryDrawing();
            ClefDrawing = new GlyphRunDrawing();
            TestNoteDrawing = new GlyphRunDrawing();
            PlayedNoteDrawing = new GlyphRunDrawing();
            drawing.Children.Add(StaffLinesDrawing);
            drawing.Children.Add(ClefDrawing);
            drawing.Children.Add(TestNoteDrawing);
            drawing.Children.Add(PlayedNoteDrawing);
            Drawing = drawing;
        }

        public Drawing Drawing { get; }
    }
}