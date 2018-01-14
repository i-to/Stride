using System.Windows;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StavesMetrics
    {
        /// <summary>
        /// Half distance between two staff lines.
        /// </summary>
        public readonly double HalfSpace;

        public readonly double StaffLineThickness;

        public StavesMetrics(double halfSpace, double staffLineThickness)
        {
            HalfSpace = halfSpace;
            StaffLineThickness = staffLineThickness;
        }

        /// <summary>
        /// Origin point of the fifth staff line of the treeble staff.
        /// </summary>
        public Point Origin => new Point(0, 0);
        public double StaffLinesDistance => 2.0 * HalfSpace;
        public double GlyphSize => 8.0 * HalfSpace;
        public double BarLineThickness => 1.2 * StaffLineThickness;

        public double StaffLinesLength => 500.0 * HalfSpace;

        /// <summary>
        /// Distance between equivalent lines in treeble and bass staves.
        /// </summary>
        public double GrandStaffOffset => 2.0 * StaffLinesDistance * Const.LinesInStaff;
        public double SecondNoteOffset => 3.0 * HalfSpace;
        public double StemLineThickness => 0.75 * StaffLineThickness;

        /// <summary>
        /// The distance to which ledger line sticks out of the note on each side.
        /// </summary>
        public double LedgerLineLip => 0.4 * HalfSpace;

        /// <summary>
        /// Widths of notehead symbols. So far, determined experimentally for current font.
        /// </summary>
        public double WholeNoteheadWidth => HalfSpace * 3.4;
        public double OtherNoteheadWidth => HalfSpace * 2.45;

        /// <summary>
        /// Margins that appear on each side of layout objects.
        /// </summary>
        public double BarlineMargin => 1.0 * HalfSpace;
        public double WholeNoteMargin => 4.0 * WholeNoteheadWidth;
        public double HalfNoteMargin => 2.0 * OtherNoteheadWidth;
        public double QuarterNoteMargin => 1.0 * OtherNoteheadWidth;
        public double OtherNoteMargin => 0.5 * OtherNoteheadWidth;

        public double RegularStemLength => 7.0 * HalfSpace;
    }
}