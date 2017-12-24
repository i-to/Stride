using System.Windows;

namespace Stride.Music.Layout
{
    public enum BarLineStyle
    {
        Single
    }

    public class BarLineObject : LineObject
    {
        public readonly BarLineStyle Style;

        public BarLineObject(Point origin, Point end, BarLineStyle style)
            : base(origin, end)
        {
            Style = style;
        }
    }
}