using System.Windows;
using System.Windows.Media;

namespace Stride.Drawing.Wpf
{
    public class GlyphRunBuilder
    {
        public readonly GlyphTypeface Typeface;

        public GlyphRunBuilder(GlyphTypeface typeface)
        {
            Typeface = typeface;
        }

        public GlyphRun CreateGlyphRun(string text, Point origin, double size)
        {
            var glyphIndexes = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            double totalWidth = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = Typeface.CharacterToGlyphMap[text[n]];
                glyphIndexes[n] = glyphIndex;

                double width = Typeface.AdvanceWidths[glyphIndex] * size;
                advanceWidths[n] = width;

                totalWidth += width;
            }

            return new GlyphRun(Typeface, 0, false, size,
                glyphIndexes, origin, advanceWidths, null, null, null, null,
                null, null);
        }
    }
}