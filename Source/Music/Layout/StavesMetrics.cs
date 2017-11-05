using System.Windows;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StavesMetrics
    {
        /// <summary>
        /// Half distance between two staff lines.
        /// </summary>
        public readonly double BaseSize;

        public StavesMetrics(double baseSize)
        {
            BaseSize = baseSize;
        }

        public Point Origin => new Point(0, 0);
        public double StaffLinesDistance => 2.0 * BaseSize;
        public double GlyphSize => 8.0 * BaseSize;
        public double LineThickness => 1.0;
        public double GrandStaffOffset => 2.0 * StaffLinesDistance * Const.LinesInStaff;
        public double LedgerLineLength => 3.0 * BaseSize;
        public double SecondNoteOffset => 3.0 * BaseSize;
    }
}