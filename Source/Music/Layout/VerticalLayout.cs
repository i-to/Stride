using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class VerticalLayout
    {
        readonly StavesMetrics Metrics;

        public VerticalLayout(StavesMetrics metrics)
        {
            Metrics = metrics;
        }

        public double StaffPositionToYOffset(StaffPosition position)
        {
            var middleLinePosition = 4 * Metrics.HalfSpace;
            var offset = position.VerticalOffset * Metrics.HalfSpace;
            var cleffOffset = position.Clef == Clef.Bass ? Metrics.GrandStaffOffset : 0.0;
            return middleLinePosition + cleffOffset - offset;
        }
    }
}