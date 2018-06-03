using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Stride.Utility.Fluent;
using Stride.Utility.ImageProcessing;

namespace Stride.Drawing.Wpf
{
    public class BitmapConverter
    {
        /// <summary>
        /// This format is implicitly encoded in <see cref="PixelColor"/> struct.
        /// </summary>
        public static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;

        public PixelColor[,] ToPixelsArray(BitmapSource source)
        {
            if (source.Format != PixelFormat)
                source = new FormatConvertedBitmap(source, PixelFormat, null, 0);

            int width = source.PixelWidth;
            int height = source.PixelHeight;

            var result = new PixelColor[width, height];
            var array = new byte[4 * width * height];
            source.CopyPixels(array, width * 4, 0);

            for (int y = 0; y != height; ++y)
            for (int x = 0; x != width; ++x)
            {
                var blue  = array[4 * (width * y + x) + 0];
                var green = array[4 * (width * y + x) + 1];
                var red   = array[4 * (width * y + x) + 2];
                var alpha = array[4 * (width * y + x) + 3];
                result[x, y] = new PixelColor(blue, green, red, alpha);
            }

            return result;
        }

        public BitmapSource ToBitmap(PixelColor[,] pixels, (double, double) dpi)
        {
            var width = pixels.Width();
            var height = pixels.Height();
            var bitmap = new WriteableBitmap(width, height, dpi.Item1, dpi.Item2, PixelFormat, null);
            var sourceRect = new Int32Rect(0, 0, width, height);
            var array = pixels.UnrollRows();
            bitmap.WritePixels(sourceRect, array, 4 * width, 0, 0);
            return bitmap;
        }
    }
}