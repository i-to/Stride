using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MoreLinq;
using Stride.Music.Layout;
using Stride.Utility;

namespace Stride.Gui.MusicDrawing
{
    public class MusicDrawingBuilder
    {
        readonly GlyphRunBuilder GlyphRunBuilder;
        readonly FontSymbolMapping FontSymbolMapping;
        readonly MusicTypefaceProvider MusicTypefaceProvider;
        readonly DrawingCollection DrawingChildren;

        public MusicDrawingBuilder(
            GlyphRunBuilder glyphRunBuilder,
            FontSymbolMapping fontSymbolMapping,
            MusicTypefaceProvider musicTypefaceProvider)
        {
            GlyphRunBuilder = glyphRunBuilder;
            MusicTypefaceProvider = musicTypefaceProvider;
            FontSymbolMapping = fontSymbolMapping;
            var drawing = new DrawingGroup();
            DrawingChildren = drawing.Children;
            Drawing = drawing;
        }

        public Drawing Drawing { get; }

        public void BuildDrawing(Layout layout)
        {
            DrawingChildren.Clear();
            var soundingNoteDrawings = BuildGlyphDrawings(layout.SoundingNotes, Brushes.Red, layout.GlyphSize);
            var clefDrawings = BuildGlyphDrawings(layout.Clefs, Brushes.Black, layout.GlyphSize);
            var lines = layout.StaffLines.Concat(layout.LedgerLines).ToReadOnlyList();
            var staffDrawing = BuildLinesDrawing(lines, CreateSolidBlackLinePen(layout.StaffLineThickness));
            var barlinesDrawing = BuildLinesDrawing(layout.BarLines, CreateSolidBlackLinePen(layout.BarLineThickness));
            var testPhraseDrawing = BuildGlyphDrawings(layout.TestPhrase, Brushes.Black, layout.GlyphSize);
            new Drawing[] { staffDrawing, barlinesDrawing}
                .Concat(clefDrawings)
                .Concat(testPhraseDrawing)
                .Concat(soundingNoteDrawings)
                .ForEach(DrawingChildren.Add);
        }

        Pen CreateSolidBlackLinePen(double thickness) =>
            new Pen {Brush = Brushes.Black, Thickness = thickness};

        LineGeometry BuildLineGeometry(LineObject line) =>
            new LineGeometry
            {
                StartPoint = line.Origin,
                EndPoint = line.End
            };

        Geometry BuildLinesGeometry(IEnumerable<LineObject> lines)
        {
            var geometry = new GeometryGroup();
            foreach (var line in lines)
                geometry.Children.Add(BuildLineGeometry(line));
            return geometry;
        }

        GeometryDrawing BuildLinesDrawing(IReadOnlyList<LineObject> lines, Pen pen) => 
            new GeometryDrawing
            {
                Geometry = BuildLinesGeometry(lines),
                Pen = pen
            };

        GlyphRunDrawing BuildGlyphDrawing(SymbolObject symbolObject, Brush brush, double glyphSize) => 
            new GlyphRunDrawing
            {
                GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                    MusicTypefaceProvider.Typeface,
                    FontSymbolMapping[symbolObject.Symbol].ToString(),
                    symbolObject.Origin,
                    glyphSize),
                ForegroundBrush = brush
            };

        IEnumerable<GlyphRunDrawing> BuildGlyphDrawings(IEnumerable<SymbolObject> symbolObjects, Brush brush, double glyphSize) =>
            symbolObjects.Select(symbol => BuildGlyphDrawing(symbol, brush, glyphSize));
    }
}
