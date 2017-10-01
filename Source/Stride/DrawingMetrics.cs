using System.Windows;

namespace Stride
{
    public class DrawingMetrics
    {
        // Half distance between two staff lines.
        public readonly double BaseSize;

        public DrawingMetrics(double baseSize)
        {
            BaseSize = baseSize;
        }

        public double StaffLinesDistance => 2.0 * BaseSize;
        public double GlyphSize => 8.0 * BaseSize;
        public Point StaffLinesOrigin => new Point(0, 0);
        public double StaffLinesThickness => 1;
    }
}