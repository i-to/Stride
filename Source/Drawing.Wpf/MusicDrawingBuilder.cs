using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MoreLinq;
using Stride.Music.Layout;

namespace Stride.Drawing.Wpf
{
    public class MusicDrawingBuilder
    {
        readonly GlyphRunBuilder GlyphRunBuilder;
        readonly FontSymbolMapping FontSymbolMapping;

        public MusicDrawingBuilder(
            GlyphRunBuilder glyphRunBuilder,
            FontSymbolMapping fontSymbolMapping)
        {
            GlyphRunBuilder = glyphRunBuilder;
            FontSymbolMapping = fontSymbolMapping;
        }

        public System.Windows.Media.Drawing BuildDrawing(IEnumerable<LayoutObject> layout)
        {
            var drawing = new DrawingGroup();
            var symbols = BuildGlyphDrawings(layout.OfType<SymbolObject>());
            var lineDrawings = CreateLineDrawings(layout.OfType<LineObject>());
            lineDrawings.Concat(symbols).ForEach(drawing.Children.Add);
            return drawing;
        }

        IEnumerable<System.Windows.Media.Drawing> CreateLineDrawings(IEnumerable<LineObject> lines) => 
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
                    FontSymbolMapping.Map[symbol.Symbol].ToString(),
                    symbol.Origin,
                    symbol.Size),
                ForegroundBrush = symbol.IsHighlighted ? Brushes.Red : Brushes.Black
            };

        IEnumerable<GlyphRunDrawing> BuildGlyphDrawings(IEnumerable<SymbolObject> symbolObjects) => 
            symbolObjects.Select(BuildGlyphDrawing);
    }
}
