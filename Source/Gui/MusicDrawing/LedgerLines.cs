using System;

namespace Stride.Gui.MusicDrawing
{
    /// <summary>
    /// Ledger lines at a given position of the single staff.
    /// </summary>
    public class LedgerLines
    {
        /// <summary>
        /// Indices represent number of legder lines on each side (0 - for no lines).
        /// </summary>
        public readonly int Top, Bottom;

        public LedgerLines(int top, int bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public static readonly LedgerLines Absent = new LedgerLines(0, 0);

        public static LedgerLines CreateTop(int count) => 
            new LedgerLines(count, 0);

        public static LedgerLines CreateBottom(int count) => 
            new LedgerLines(0, count);

        public static LedgerLines CreateSingle(int count, bool top) =>
            top ? CreateTop(count) : CreateBottom(count);

        public LedgerLines Combine(LedgerLines other)
        {
            var top = Math.Max(Top, other.Top);
            var bottom = Math.Max(Bottom, other.Bottom);
            return new LedgerLines(top, bottom);
        }
    }
}