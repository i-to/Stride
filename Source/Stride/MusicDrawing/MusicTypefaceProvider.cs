using System;
using System.Windows;
using System.Windows.Media;

namespace Stride.MusicDrawing
{
    public class MusicTypefaceProvider
    {
        public MusicTypefaceProvider()
        {
            var fontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Bravura");
            Typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        public Typeface Typeface { get; }
    }
}