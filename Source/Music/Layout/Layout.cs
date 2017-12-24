using System.Collections.Generic;

namespace Stride.Music.Layout
{
    public class Layout
    {
        public readonly IReadOnlyList<LineObject> StaffLines, LedgerLines;
        public readonly IReadOnlyList<BarLineObject> BarLines;
        public readonly IReadOnlyList<SymbolObject> Clefs;
        public readonly IReadOnlyList<SymbolObject> TestPhrase, SoundingNotes;

        public readonly double StaffLineThickness;
        public readonly double BarLineThickness;
        public readonly double GlyphSize;

        public Layout(
            IReadOnlyList<LineObject> staffLines,
            IReadOnlyList<LineObject> ledgerLines,
            IReadOnlyList<BarLineObject> barLines,
            IReadOnlyList<SymbolObject> clefs,
            IReadOnlyList<SymbolObject> testPhrase,
            IReadOnlyList<SymbolObject> soundingNotes,
            double staffLineThickness,
            double barLineThickness,
            double glyphSize)
        {
            StaffLines = staffLines;
            LedgerLines = ledgerLines;
            BarLines = barLines;
            Clefs = clefs;
            TestPhrase = testPhrase;
            SoundingNotes = soundingNotes;
            StaffLineThickness = staffLineThickness;
            BarLineThickness = barLineThickness;
            GlyphSize = glyphSize;
        }
    }
}