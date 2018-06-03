namespace Stride.Music.Layout
{
    public class BeatGroupSpan
    {
        public readonly double LeftMargin;
        public readonly double Span;
        public readonly double RightMargin;

        public BeatGroupSpan(double leftMargin, double span, double rightMargin)
        {
            LeftMargin = leftMargin;
            Span = span;
            RightMargin = rightMargin;
        }
    }
}