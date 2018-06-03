using Stride.Drawing.Wpf;
using Stride.Resources;

namespace Stride.Bootstrapper
{
    public class DrawingModule
    {
        public readonly MusicDrawingBuilder MusicDrawingBuilder;

        public DrawingModule(ResourceLoader resourceLoader)
        {
            var bravuraTypeface = resourceLoader.LoadTypeface("Fonts/Bravura.otf");
            var glyphRunBuilder = new GlyphRunBuilder(bravuraTypeface);
            var musicSymbolToFontText = new FontSymbolMapping();
            MusicDrawingBuilder = new MusicDrawingBuilder(glyphRunBuilder, musicSymbolToFontText);
        }
    }
}