using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Stride.Drawing.Wpf;
using Stride.Utility.ImageProcessing;

namespace Stride.Bootstrapper.Test
{
    public class TestUtility
    {
        public readonly TestConfiguration Config;
        public readonly BitmapUtility BitmapUtility;
        public readonly BitmapConverter BitmapConverter;
        public readonly BitmapBlender BitmapBlender;

        public TestUtility(
            TestConfiguration config,
            BitmapUtility bitmapUtility,
            BitmapConverter bitmapConverter,
            BitmapBlender bitmapBlender)
        {
            Config = config;
            BitmapUtility = bitmapUtility;
            BitmapConverter = bitmapConverter;
            BitmapBlender = bitmapBlender;
        }

        public void AssureOutputFolderExists()
            => Directory.CreateDirectory(Config.OutputImagePath);

        public bool IsOutputFolderEmpty()
            => !Directory.EnumerateFileSystemEntries(Config.OutputImagePath).Any();

        public BitmapSource LoadGolden(string name)
        {
            var fullName = Path.Combine(Config.GoldenImagePath, $"{name}-Golden.png");
            return BitmapUtility.LoadFromPng(fullName);
        }

        public void SaveRendered(string name, BitmapSource bitmap) => SavePng(name, "Rendered", bitmap);
        public void SaveLoadedGolden(string name, BitmapSource bitmap) => SavePng(name, "Golden", bitmap);
        public void SaveBlend(string name, BitmapSource bitmap) => SavePng(name, "Blend", bitmap);

        void SavePng(string name, string suffix, BitmapSource bitmap)
        {
            var fullName = Path.Combine(Config.OutputImagePath, $"{name}-{suffix}.png");
            BitmapUtility.SaveAsPng(fullName, bitmap);
        }

        public void ReplaceGolden(string name, BitmapSource bitmap)
        {
            var fullName = Path.Combine(Config.GoldenImagePath, $"{name}-Golden.png");
            BitmapUtility.SaveAsPng(fullName, bitmap);
        }

        public BitmapSource RenderToBitmap(System.Windows.Media.Drawing drawing)
            => BitmapUtility.RenderToBitmap(
                drawing,
                Config.BitmapDpi);

        public (bool same, BitmapSource blend) CompareAndHighlightDiff(
            BitmapSource first,
            BitmapSource second)
        {
            var firstPixels = BitmapConverter.ToPixelsArray(first);
            var secondPixels = BitmapConverter.ToPixelsArray(second);
            var blendPixels = BitmapBlender.Blend(
                firstPixels,
                secondPixels,
                BitmapBlender.AbsoluteDistanceBlender);
            var diffRatio = BitmapBlender.EvaluateDiff(blendPixels);
            var same = diffRatio < Config.DiffRatioThreshold;
            var dpi = (Config.BitmapDpi, Config.BitmapDpi);
            var blend = BitmapConverter.ToBitmap(blendPixels, dpi);
            return (same, blend);
        }
    }
}