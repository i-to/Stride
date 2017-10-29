namespace Stride.Music.Presentation
{
    /// <summary>
    /// Ledger lines at a given position of the grand staff.
    /// </summary>
    public class GrandStaffLedgerLines
    {
        public readonly LedgerLines TreebleClef, BassClef;

        public GrandStaffLedgerLines(LedgerLines treebleClef, LedgerLines bassClef)
        {
            TreebleClef = treebleClef;
            BassClef = bassClef;
        }

        public static readonly GrandStaffLedgerLines Absent =
            new GrandStaffLedgerLines(LedgerLines.Absent, LedgerLines.Absent);

        public static GrandStaffLedgerLines CreateTreeble(LedgerLines lines) =>
            new GrandStaffLedgerLines(lines, LedgerLines.Absent);

        public static GrandStaffLedgerLines CreateBass(LedgerLines lines) =>
            new GrandStaffLedgerLines(LedgerLines.Absent, lines);

        public static GrandStaffLedgerLines CreateSingle(LedgerLines lines, bool treeble) =>
            treeble ? CreateTreeble(lines) : CreateBass(lines);

        public GrandStaffLedgerLines Combine(GrandStaffLedgerLines other)
        {
            var treeble = TreebleClef.Combine(other.TreebleClef);
            var bass = BassClef.Combine(other.BassClef);
            return new GrandStaffLedgerLines(treeble, bass);
        }
    }
}