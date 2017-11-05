using System.Windows;
using Stride.Music.Presentation;

namespace Stride.Music.Layout
{
    public class SymbolObject : LayoutObject
    {
        public readonly Symbol Symbol;

        public SymbolObject(Point origin, Symbol symbol)
            : base(origin)
        {
            Symbol = symbol;
        }
    }
}