using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MoreLinq;
using Stride.Music.Layout;
using Stride.Music.Presentation;

namespace Stride.Gui.MusicDrawing
{
    public class MusicDrawingBuilder
    {
        readonly GlyphRunBuilder GlyphRunBuilder;
        readonly FontSymbolMapping FontSymbolMapping;
        readonly MusicTypefaceProvider MusicTypefaceProvider;
        readonly LayoutEngine LayoutEngine;
        readonly DrawingCollection DrawingChildren;

        public MusicDrawingBuilder(
            GlyphRunBuilder glyphRunBuilder,
            FontSymbolMapping fontSymbolMapping,
            MusicTypefaceProvider musicTypefaceProvider,
            LayoutEngine layoutEngine)
        {
            GlyphRunBuilder = glyphRunBuilder;
            MusicTypefaceProvider = musicTypefaceProvider;
            LayoutEngine = layoutEngine;
            FontSymbolMapping = fontSymbolMapping;
            var drawing = new DrawingGroup();
            DrawingChildren = drawing.Children;
            Drawing = drawing;
        }

        public Drawing Drawing { get; }

        public void BuildDrawing(StaffPosition testNotePosition, IEnumerable<StaffPosition> soundingNotePositions)
        {
            DrawingChildren.Clear();
            var layout = LayoutEngine.CreateLayout(testNotePosition.Concat(soundingNotePositions));
            var soundingNoteDrawings = BuildGlyphDrawings(layout.Notes.Skip(1), Brushes.Red, layout.GlyphSize);
            var treebleClefDrawing = BuildGlyphDrawing(layout.TreebleClef, Brushes.Black, layout.GlyphSize);
            var bassClefDrawing = BuildGlyphDrawing(layout.BassClef, Brushes.Black, layout.GlyphSize);
            var staffDrawing = BuildLinesDrawing(layout.StaffLines, layout.LineThickness);
            var testNoteDrawing = BuildGlyphDrawing(layout.Notes.First(), Brushes.Black, layout.GlyphSize);
            DrawingChildren.Add(testNoteDrawing);
            new Drawing[] {treebleClefDrawing, bassClefDrawing, staffDrawing}
                .Concat(soundingNoteDrawings)
                .ForEach(DrawingChildren.Add);
        }

        Geometry BuildLinesGeometry(IEnumerable<LineObject> lines)
        {
            var geometry = new GeometryGroup();
            foreach (var line in lines)
                geometry.Children.Add(
                    new LineGeometry
                    {
                        StartPoint = line.Origin,
                        EndPoint = line.End
                    });
            return geometry;
        }

        GeometryDrawing BuildLinesDrawing(IReadOnlyList<LineObject> lines, double thickness) => 
            new GeometryDrawing
            {
                Geometry = BuildLinesGeometry(lines),
                Pen = new Pen {Brush = Brushes.Black, Thickness = thickness}
            };

        GlyphRunDrawing BuildGlyphDrawing(SymbolObject symbolObject, Brush brush, double glyphSize)
        {
            return new GlyphRunDrawing
            {
                GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                    MusicTypefaceProvider.Typeface,
                    FontSymbolMapping[symbolObject.Symbol].ToString(),
                    symbolObject.Origin,
                    glyphSize),
                ForegroundBrush = brush
            };
        }

        IEnumerable<GlyphRunDrawing> BuildGlyphDrawings(IEnumerable<SymbolObject> symbolObjects, Brush brush, double glyphSize) =>
            symbolObjects.Select(symbol => BuildGlyphDrawing(symbol, brush, glyphSize));
    }
}
