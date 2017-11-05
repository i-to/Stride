using System.Collections.Generic;

namespace Stride.Music.Layout
{
    public class Layout
    {
        public readonly IReadOnlyList<LineObject> StaffLines; // For now, also includes ledger lines.
        public readonly SymbolObject BassClef, TreebleClef;
        public readonly SymbolObject TestNote;
        public readonly IReadOnlyList<SymbolObject> SoundingNotes;

        public readonly double LineThickness;
        public readonly double GlyphSize;

        public Layout(
            IReadOnlyList<LineObject> staffLines,
            SymbolObject bassClef,
            SymbolObject treebleClef,
            SymbolObject testNote,
            IReadOnlyList<SymbolObject> soundingNotes,
            double lineThickness,
            double glyphSize)
        {
            StaffLines = staffLines;
            BassClef = bassClef;
            TreebleClef = treebleClef;
            SoundingNotes = soundingNotes;
            LineThickness = lineThickness;
            GlyphSize = glyphSize;
            TestNote = testNote;
        }
    }
}