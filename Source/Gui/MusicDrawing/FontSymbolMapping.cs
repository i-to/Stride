using System.Collections.Generic;
using Stride.Music.Layout;

namespace Stride.Gui.MusicDrawing
{
    public class FontSymbolMapping
    {
        public readonly IReadOnlyDictionary<Symbol, char> Map =
            new Dictionary<Symbol, char>
            {
                {Symbol.TreebleClef, (char)0xE050},
                {Symbol.BassClef, (char)0xE062},
                {Symbol.NoteheadWhole, (char)0xE0A2},
                {Symbol.NoteheadHalf, (char)0xE0A3},
                {Symbol.NoteheadBlack, (char)0xE0A4},
            };
    }
}