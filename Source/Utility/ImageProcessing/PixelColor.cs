namespace Stride.Utility.ImageProcessing
{
    /// <summary>
    /// Represents pixel color in <code>PixelFormats.Bgra32</code> format, suitable for array-based interop.
    /// The set of fields, their order and <code>struct</code> keyword should not change.
    /// </summary>
    public struct PixelColor
    {
        public readonly byte Blue;
        public readonly byte Green;
        public readonly byte Red;
        public readonly byte Alpha;

        public PixelColor(byte blue, byte green, byte red, byte alpha)
        {
            Blue = blue;
            Green = green;
            Red = red;
            Alpha = alpha;
        }

        public int Argb => Alpha << 12 + Red << 8 + Green << 4 + Blue;

        public static bool operator ==(PixelColor first, PixelColor second)
            => first.Blue == second.Blue
            && first.Green == second.Green
            && first.Red == second.Red
            && first.Alpha == second.Alpha;

        public static bool operator !=(PixelColor first, PixelColor second)
            => !(first == second);

        public override bool Equals(object other)
            => other is PixelColor color
            && color == this;

        public override int GetHashCode()
            => Hash.Compute(Blue, Green, Red, Alpha);
    }
}