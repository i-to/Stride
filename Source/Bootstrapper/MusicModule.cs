using Stride.Music.Layout;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Bootstrapper
{
    public class MusicModule
    {
        public readonly ScoreLayoutAlgorithm Layout;
        public readonly ScoreBuilder ScoreBuilder;

        public MusicModule()
        {
            var metrics = new StavesMetrics(halfSpace: 8.0, staffLineThickness: 1.5);
            //var layout = CreateTestLayoutEngine(metrics);
            Layout = CreateLayoutEngine(metrics);
            ScoreBuilder = CreateScoreBuilder();
        }

        ScoreLayoutAlgorithm CreateLayoutEngine(StavesMetrics metrics)
        {
            var staffLinesGeometryBuilder = new StaffLinesLayoutAlgorithm();
            var ledgerLinesComputation = new LedgerLinesComputation();
            var beatGroupLayoutAlgorithm = new BeatGroupLayoutAlgorithm();
            var beatGroupSpanComputation = new BeatGroupSpanComputation(metrics);
            var horizontalLayout = new HorizontalLayoutAlgorithm(metrics);
            var verticalLayout = new VerticalLayoutAlgorithm(metrics);
            var stemLayout = new StemsLayoutAlgorithm(metrics, verticalLayout);
            return new ScoreLayoutAlgorithm(
                metrics,
                staffLinesGeometryBuilder,
                ledgerLinesComputation,
                beatGroupLayoutAlgorithm,
                beatGroupSpanComputation,
                horizontalLayout,
                verticalLayout,
                stemLayout);
        }

        TestLayout CreateTestLayout(StavesMetrics metrics)
        {
            return new TestLayout(metrics);
        }

        ScoreBuilder CreateScoreBuilder()
        {
            var staffPositionComputation = new StaffPositionComputation();
            var barsComputation = new BarsComputation();
            var lowestTreebleStaffPitch = Pitch.C4;
            return new ScoreBuilder(staffPositionComputation, barsComputation, lowestTreebleStaffPitch);
        }
    }
}