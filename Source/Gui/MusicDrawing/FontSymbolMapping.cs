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
                {Symbol.FlagEighthUp, (char)0xE240},
                {Symbol.FlagEightsDown, (char)0xE241},
                {Symbol.DoubleSharp, (char)0xE263},
                {Symbol.Sharp, (char)0xE262},
                {Symbol.Natural, (char)0xE261},
                {Symbol.Flat, (char)0xE260},
                {Symbol.DoubleFlat, (char)0xE264},
            };
    }
}