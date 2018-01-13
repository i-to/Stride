using System.Windows;

namespace Stride.Music.Layout
{
    public class SymbolObject : LayoutObject
    {
        public readonly Symbol Symbol;
        public readonly double Size;
        public readonly bool IsHighlighted;

        public SymbolObject(Point origin, Symbol symbol, double size, bool isHighlighted)
            : base(origin)
        {
            Symbol = symbol;
            Size = size;
            IsHighlighted = isHighlighted;
        }
    }
}