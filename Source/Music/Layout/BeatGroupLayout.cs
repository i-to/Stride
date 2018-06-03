using System.Collections.Generic;

namespace Stride.Music.Layout
{
    public class BeatGroupLayout
    {
        /// <summary>
        /// Indices of notes that are offset to the right due to a second interval.
        /// </summary>
        public readonly IReadOnlyList<int> OffsetNotes;

        public BeatGroupLayout(IReadOnlyList<int> offsetNotes)
        {
            OffsetNotes = offsetNotes;
        }
    }
}