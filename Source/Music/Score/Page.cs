using System.Collections.Generic;
using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class NoteOnPage
    {
        public readonly Duration Duration;
        public readonly StaffPosition StaffPosition;
        public readonly Tick Tick;

        public NoteOnPage(Duration duration, StaffPosition staffPosition, Tick tick)
        {
            Duration = duration;
            StaffPosition = staffPosition;
            Tick = tick;
        }
    }

    public class Page
    {
        public readonly IReadOnlyList<NoteOnPage> PageNotes, OverlayNotes;

        public Page(
            IReadOnlyList<NoteOnPage> pageNotes,
            IReadOnlyList<NoteOnPage> overlayNotes)
        {
            PageNotes = pageNotes;
            OverlayNotes = overlayNotes;
        }
    }
}
