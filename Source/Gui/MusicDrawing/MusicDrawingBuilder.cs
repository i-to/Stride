using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MoreLinq;
using Stride.Music.Layout;

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

        public void BuildDrawing(IEnumerable<LayoutObject> layout)
        {
            DrawingChildren.Clear();
            var symbols = BuildGlyphDrawings(layout.OfType<SymbolObject>());
            var lineDrawings = CreateLineDrawings(layout.OfType<LineObject>());
            lineDrawings.Concat(symbols).ForEach(DrawingChildren.Add);
        }

        IEnumerable<Drawing> CreateLineDrawings(IEnumerable<LineObject> lines) => 
            lines
            .GroupBy(LineObject.GetThickness)
            .Select(group =>
                {
                    var pen = CreateSolidBlackLinePen(group.Key);
                    return BuildLinesDrawing(group, pen);
                });

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

        GeometryDrawing BuildLinesDrawing(IEnumerable<LineObject> lines, Pen pen) => 
            new GeometryDrawing
            {
                Geometry = BuildLinesGeometry(lines),
                Pen = pen
            };

        GlyphRunDrawing BuildGlyphDrawing(SymbolObject symbol) => 
            new GlyphRunDrawing
            {
                GlyphRun = GlyphRunBuilder.CreateGlyphRun(
                    MusicTypefaceProvider.Typeface,
                    FontSymbolMapping.Map[symbol.Symbol].ToString(),
                    symbol.Origin,
                    symbol.Size),
                ForegroundBrush = symbol.IsHighlighted ? Brushes.Red : Brushes.Black
            };

        IEnumerable<GlyphRunDrawing> BuildGlyphDrawings(IEnumerable<SymbolObject> symbolObjects) => 
            symbolObjects.Select(BuildGlyphDrawing);
    }
}
