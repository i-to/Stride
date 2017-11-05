using System.Collections.Generic;

namespace Stride.Music.Layout
{
    public class Layout
    {
        public readonly IReadOnlyList<LineObject> StaffLines; // For now, also includes ledger lines.
        public readonly SymbolObject BassClef, TreebleClef;
        public readonly IReadOnlyList<SymbolObject> Notes;

        public readonly double LineThickness;
        public readonly double GlyphSize;

        public Layout(
            IReadOnlyList<LineObject> staffLines,
            SymbolObject bassClef,
            SymbolObject treebleClef,
            IReadOnlyList<SymbolObject> notes,
            double lineThickness,
            double glyphSize)
        {
            StaffLines = staffLines;
            BassClef = bassClef;
            TreebleClef = treebleClef;
            Notes = notes;
            LineThickness = lineThickness;
            GlyphSize = glyphSize;
        }
    }
}