using Stride.Music;
using Stride.Utility;

namespace Stride.MusicDrawing
{
    public class LedgerLinesComputation
    {
        public static GrandStaffLedgerLines ComputeLedgerLines(StaffPosition testNotePosition, StaffPosition playedNotePosition)
        {
            var testLedgerLines = ComputeLedgerLines(testNotePosition);
            var playedLedgerLines = ComputeLedgerLines(playedNotePosition);
            return testLedgerLines.Combine(playedLedgerLines);
        }

        public static GrandStaffLedgerLines ComputeLedgerLines(StaffPosition notePosition)
        {
            if (notePosition == null)
                return GrandStaffLedgerLines.Absent;
            var offset = notePosition.Offset;
            var lines = (offset.Abs() / 2 - 2).LimitFromBottom(0);
            var ledgerLines = LedgerLines.CreateSingle(lines, top: offset > 0);
            return GrandStaffLedgerLines.CreateSingle(ledgerLines, treeble: notePosition.Clef == Clef.Treeble);
        }
    }
}