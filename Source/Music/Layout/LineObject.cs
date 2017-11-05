using System.Windows;

namespace Stride.Music.Layout
{
    public class LineObject : LayoutObject
    {
        public readonly Point End;

        public LineObject(Point origin, Point end)
            : base(origin)
        {
            End = end;
        }
    }
}