using Stride.Music;

namespace Stride.Gui.MusicDrawing
{
    /// <summary>
    /// Identifies position of the note at a particular tick within the staff.
    /// </summary>
    public class StaffPosition
    {
        /// <summary>
        /// Cleff identifies one of the two staves in the grand staff.
        /// </summary>
        public readonly Clef Clef;

        /// <summary>
        /// Vertical offset relative to the middle (the third) staff line.
        /// Positive direction is up, negative is down.
        /// One step is between the position on line and the next position
        /// between the two lines and vice versa.
        /// </summary>
        public readonly int VerticalOffset;

        /// <summary>
        /// Horizontal offset used when drawing harmonic seconds.
        /// </summary>
        public readonly bool HorisontalOffset;

        public StaffPosition(Clef clef, int verticalOffset, bool horisontalOffset)
        {
            Clef = clef;
            VerticalOffset = verticalOffset;
            HorisontalOffset = horisontalOffset;
        }

        public StaffPosition WithHorizontalOffset(bool offset) =>
            new StaffPosition(Clef, VerticalOffset, offset);

        public static StaffPosition InTreebleClef(int offset) =>
            new StaffPosition(Clef.Treeble, offset, false);

        public static StaffPosition InBassClef(int offset) =>
            new StaffPosition(Clef.Bass, offset, false);
    }
}