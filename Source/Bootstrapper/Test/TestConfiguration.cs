using System.Windows.Media;

namespace Stride.Bootstrapper.Test
{
    public class TestConfiguration
    {
        public readonly double BitmapDpi;
        public readonly PixelFormat PixelFormat;
        public readonly double DiffRatioThreshold;
        public readonly string GoldenImagePath;
        public readonly string OutputImagePath;
        public readonly bool GoldenDataGenerationMode;

        public TestConfiguration(
            double bitmapDpi,
            PixelFormat pixelFormat,
            double diffRatioThreshold,
            string goldenImagePath,
            string outputImagePath,
            bool goldenDataGenerationMode)
        {
            BitmapDpi = bitmapDpi;
            DiffRatioThreshold = diffRatioThreshold;
            GoldenImagePath = goldenImagePath;
            OutputImagePath = outputImagePath;
            GoldenDataGenerationMode = goldenDataGenerationMode;
            PixelFormat = pixelFormat;
        }
    }
}