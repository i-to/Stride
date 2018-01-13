using System.Windows;

namespace Stride.Music.Layout
{
    public class LineObject : LayoutObject
    {
        public readonly Point End;
        public readonly double Thickness;

        public LineObject(Point origin, Point end, double thickness)
            : base(origin)
        {
            End = end;
            Thickness = thickness;
        }

        public static double GetThickness(LineObject line) =>
            line.Thickness;
    }
}