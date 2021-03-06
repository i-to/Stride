﻿using Stride.Music.Theory;

namespace Stride.Music.Score
{
    /// <summary>
    /// Identifies position of the note at a particular beat within the staff.
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

        public StaffPosition(Clef clef, int verticalOffset)
        {
            Clef = clef;
            VerticalOffset = verticalOffset;
        }

        public StaffPosition WithHorizontalOffset(bool offset)
            => new StaffPosition(Clef, VerticalOffset);

        public static StaffPosition InTreebleClef(int offset)
            => new StaffPosition(Clef.Treeble, offset);

        public static StaffPosition InBassClef(int offset) 
            => new StaffPosition(Clef.Bass, offset);

        public override string ToString()
            => $"{Clef}:{VerticalOffset}";
    }
}