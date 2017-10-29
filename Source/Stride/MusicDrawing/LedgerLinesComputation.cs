using System.Collections.Generic;
using System.Linq;
using Stride.Music;
using Stride.Utility;

namespace Stride.MusicDrawing
{
    public static class LedgerLinesComputation
    {
        public static GrandStaffLedgerLines ComputeLedgerLines(StaffPosition notePosition)
        {
            if (notePosition == null)
                return GrandStaffLedgerLines.Absent;
            var offset = notePosition.Offset;
            var lines = (offset.Abs() / 2 - 2).LimitFromBottom(0);
            var ledgerLines = LedgerLines.CreateSingle(lines, top: offset > 0);
            return GrandStaffLedgerLines.CreateSingle(ledgerLines, treeble: notePosition.Clef == Clef.Treeble);
        }

        static GrandStaffLedgerLines Combine(GrandStaffLedgerLines ledgerLines, StaffPosition position)
        {
            var otherLedgerLines = ComputeLedgerLines(position);
            return ledgerLines.Combine(otherLedgerLines);
        }

        public static GrandStaffLedgerLines ComputeLedgerLines(StaffPosition testNotePosition, StaffPosition playedNotePosition)
        {
            var testLedgerLines = ComputeLedgerLines(testNotePosition);
            return Combine(testLedgerLines, playedNotePosition);
        }

        public static GrandStaffLedgerLines ComputeLedgerLines(IEnumerable<StaffPosition> notePositions) => 
            notePositions.Aggregate(GrandStaffLedgerLines.Absent, Combine);
    }
}