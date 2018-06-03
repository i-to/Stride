using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Stride.Utility.Fluent;

namespace Stride.Drawing.Wpf
{
    public class BitmapUtility
    {
        public BitmapSource RenderToBitmap(
            System.Windows.Media.Drawing drawing,
            double dpi)
        {
            var visual = DrawOnVisual(drawing);
            var size = AlignVisual(visual, drawing);
            return Render(dpi, size, visual);
        }

        BitmapSource Render(double dpi, (int, int) size, DrawingVisual visual)
        {
            var target = new RenderTargetBitmap(size.Item1, size.Item2, dpi, dpi, PixelFormats.Pbgra32);
            target.Render(visual);
            return target;
        }

        (int, int) AlignVisual(DrawingVisual visual, System.Windows.Media.Drawing drawing)
        {
            var bounds = drawing.Bounds;
            double left = bounds.Left;
            double top = bounds.Top;
            var transform = new TranslateTransform(-left, -top);
            visual.Transform = transform;
            return (bounds.Width.Ceiling(), bounds.Height.Ceiling());
        }

        DrawingVisual DrawOnVisual(System.Windows.Media.Drawing drawing)
        {
            var visual = new DrawingVisual();
            var context = visual.RenderOpen();
            try
            {
                context.DrawDrawing(drawing);
            }
            finally
            {
                context?.Close();
            }
            return visual;
        }

        public BitmapSource LoadFromPng(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var uri = new Uri(fullPath);
            return new BitmapImage(uri);
        }

        public void SaveAsPng(string path, BitmapSource bitmap)
        {
            var outputFrame = BitmapFrame.Create(bitmap);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(outputFrame);
            using (var file = File.OpenWrite(path))
                encoder.Save(file);
        }
    }
}