using System.Collections.Generic;
using System.Linq;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Music.Score
{
    public class LedgerLinesComputation
    {
        public GrandStaffLedgerLines ComputeLedgerLines(StaffPosition notePosition)
        {
            if (notePosition == null)
                return GrandStaffLedgerLines.Absent;
            var offset = notePosition.VerticalOffset;
            var lines = (offset.Abs() / 2 - 2).LimitFromBottom(0);
            var ledgerLines = LedgerLines.CreateSingle(lines, top: offset > 0);
            return GrandStaffLedgerLines.CreateSingle(ledgerLines, treeble: notePosition.Clef == Clef.Treeble);
        }

        GrandStaffLedgerLines Combine(GrandStaffLedgerLines ledgerLines, StaffPosition position)
        {
            var otherLedgerLines = ComputeLedgerLines(position);
            return ledgerLines.Combine(otherLedgerLines);
        }

        public GrandStaffLedgerLines ComputeLedgerLines(StaffPosition testNotePosition, StaffPosition playedNotePosition)
        {
            var testLedgerLines = ComputeLedgerLines(testNotePosition);
            return Combine(testLedgerLines, playedNotePosition);
        }

        public GrandStaffLedgerLines ComputeLedgerLines(IEnumerable<ScoreNote> notes) =>
            ComputeLedgerLines(notes.Select(note => note.StaffPosition));

        public GrandStaffLedgerLines ComputeLedgerLines(IEnumerable<StaffPosition> notePositions) => 
            notePositions.Aggregate(GrandStaffLedgerLines.Absent, Combine);
    }
}