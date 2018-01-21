using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Score;

namespace Stride.Music.Layout
{
    public class TestLayout
    {
        readonly StavesMetrics Metrics;

        public TestLayout(StavesMetrics metrics)
        {
            Metrics = metrics;
        }

        public IEnumerable<LayoutObject> CreateLayout(IEnumerable<NoteOnPage> notes)
        {
            var x = 100;
            var y = 100;
            var origin = new Point(x, y);
            //var symbol = new SymbolObject(origin, Symbol.NoteheadWhole, Metrics.GlyphSize, true);
            var symbol = new SymbolObject(origin, Symbol.NoteheadHalf, Metrics.GlyphSize, true);
            var ly = CreateLine(x, 0, x, 2 * y);
            var lx = CreateLine(0, y, 2 * x, y);
            var width = Metrics.HalfSpace * 2.45;
            var lw = CreateLine(x + width, 0, x + width, 2 * y);
            return new LayoutObject[]
            {
                ly, lx, lw,
                symbol,
            };
        }

        LineObject CreateLine(double x1, double y1, double x2, double y2)
        {
            var origin = new Point(x1, y1);
            var end = new Point(x2, y2);
            return new LineObject(origin, end, 1.0);
        }
    }
}