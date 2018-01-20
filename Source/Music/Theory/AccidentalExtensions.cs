using System;

namespace Stride.Music.Theory
{
    public static class AccidentalExtensions
    {
        public static string ToStringSymbol(this Accidental accidental)
        {
            switch (accidental)
            {
                case Accidental.DoubleFlat: return "bb";
                case Accidental.Flat: return "b";
                case Accidental.Natural: return "";
                case Accidental.Sharp: return "#";
                case Accidental.DoubleSharp: return "x";
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}