using System;
using Stride.Music.Presentation;

namespace Stride.Gui.MusicDrawing
{
    public class FontSymbolMapping
    {
        readonly char TreebleClef = (char)0xE050;
        readonly char BassClef = (char)0xE062;
        readonly char WholeNote = (char)0xE0A2;

        public char this[Symbol symbol]
        {
            get
            {
                switch (symbol)
                {
                    case Symbol.TreebleClef: return TreebleClef;
                    case Symbol.BassClef: return BassClef;
                    case Symbol.WholeNote: return WholeNote;
                }
                throw new ArgumentOutOfRangeException($"Unrecognized symbol: {symbol}.");
            }
        }
    }
}