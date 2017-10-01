using System;
using System.Windows;
using System.Windows.Media;

namespace Stride.MusicDrawing
{
    public class GlyphRunBuilder
    {
        public GlyphRun CreateGlyphRun(Typeface typeface, string text, Point origin, double size)
        {
            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                throw new InvalidOperationException("No glyph typeface found");

            var glyphIndexes = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            double totalWidth = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
                glyphIndexes[n] = glyphIndex;

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
                advanceWidths[n] = width;

                totalWidth += width;
            }

            return new GlyphRun(glyphTypeface, 0, false, size,
                glyphIndexes, origin, advanceWidths, null, null, null, null,
                null, null);
        }
    }
}