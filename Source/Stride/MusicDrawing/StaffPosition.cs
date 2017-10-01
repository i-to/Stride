using Stride.Music;

namespace Stride.MusicDrawing
{
    /// <summary>
    /// Identifies vertical position of the note within the staff.
    /// </summary>
    public class StaffPosition
    {
        /// <summary>
        /// Cleff identifies one of the two staves in the grand staff.
        /// </summary>
        public readonly Clef Clef;

        /// <summary>
        /// Offset relative to the middle (the third) staff line.
        /// Positive direction is up, negative is down.
        /// One step is between the position on line and the next position
        /// between the two lines and vice versa.
        /// </summary>
        public readonly int Offset;

        public StaffPosition(Clef clef, int offset)
        {
            Clef = clef;
            Offset = offset;
        }

        public static StaffPosition InTreebleClef(int offset) =>
            new StaffPosition(Clef.Treeble, offset);

        public static StaffPosition InBassClef(int offset) =>
            new StaffPosition(Clef.Bass, offset);
    }
}