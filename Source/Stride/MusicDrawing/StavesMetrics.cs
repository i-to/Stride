using System.Windows;

namespace Stride.MusicDrawing
{
    public class StavesMetrics
    {
        // Half distance between two staff lines.
        public readonly double BaseSize;

        public StavesMetrics(double baseSize)
        {
            BaseSize = baseSize;
        }

        public int StaffLinesCount => 5;
        public Point Origin => new Point(0, 0);
        public double StaffLinesDistance => 2.0 * BaseSize;
        public double GlyphSize => 8.0 * BaseSize;
        public double StaffLinesThickness => 1;
        public double GrandStaffOffset => 2.0 * StaffLinesDistance * StaffLinesCount;
    }
}