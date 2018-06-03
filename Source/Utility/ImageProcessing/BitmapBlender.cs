using System;
using Stride.Utility.Fluent;

namespace Stride.Utility.ImageProcessing
{
    public class BitmapBlender
    {
        public PixelColor[,] Blend(
            PixelColor[,] first,
            PixelColor[,] second,
            Func<PixelColor?, PixelColor?, PixelColor> blendFunction)
        {
            var width = Math.Max(first.Width(), second.Width());
            var height = Math.Max(first.Height(), second.Height());
            var blend = new PixelColor[width, height];

            for (int col = 0; col != width; ++col)
            for (int row = 0; row != height; ++row)
            {
                var color = BlendPixel(first, second, blendFunction, col, row);
                blend[col, row] = color;
            }

            return blend;
        }

        public PixelColor BlendPixel(
            PixelColor[,] first,
            PixelColor[,] second,
            Func<PixelColor?, PixelColor?, PixelColor> blendFunction,
            int col, int row)
        {
            var colorFirst = 
                col >= first.Width() || row >= first.Height()
                ? (PixelColor?) null
                : first[col, row];
            var colorSecond =
                col >= second.Width() || row >= second.Height()
                ? (PixelColor?) null
                : second[col, row];
            return blendFunction(colorFirst, colorSecond);
        }

        public PixelColor AbsoluteDistanceBlender(PixelColor? first, PixelColor? second)
        {
            var maxDistance = 4.0 * 255.0.Squared();
            double distance;
            if (first is null || second is null)
            {
                distance = maxDistance;
            }
            else
            {
                var firstColor = first.Value;
                var secondColor = second.Value;
                var squared =
                    ((double) firstColor.Blue - secondColor.Blue).Squared()
                  + ((double) firstColor.Green - secondColor.Green).Squared()
                  + ((double) firstColor.Red - secondColor.Red).Squared()
                  + ((double) firstColor.Alpha - secondColor.Alpha).Squared();
                distance = squared.Sqrt();
            }
            var intencity = (byte) (255.0 * maxDistance / distance);
            return new PixelColor(intencity, intencity, intencity, 255);
        }

        public double EvaluateDiff(PixelColor[,] diff)
        {
            double total = 0.0;
            for (int col = 0; col != diff.Width(); ++col)
            for (int row = 0; row != diff.Height(); ++row)
            {
                var color = diff[col, row];
                total += color.Red + color.Green + color.Blue;
            }

            double max = 3.0 * 255.0 * diff.Length;
            return total / max;
        }
    }
}