using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Stride.Music.Score;

namespace Stride.Bootstrapper.Test
{
    public class ScoreRendererTest
    {
        public readonly TestUtility Utility;
        public readonly MusicModule MusicModule;
        public readonly DrawingModule DrawingModule;

        public ScoreRendererTest(
            TestUtility utility,
            MusicModule musicModule,
            DrawingModule drawingModule)
        {
            Utility = utility;
            MusicModule = musicModule;
            DrawingModule = drawingModule;
        }

        public bool Run(string name, IReadOnlyDictionary<Beat, BeatGroup> score, bool goldenDataGenerationMode)
        {
            var rendered = RenderToBitmapSource(score);
            if (goldenDataGenerationMode)
            {
                Utility.ReplaceGolden(name, rendered);
                return false;
            }
            Utility.SaveRendered(name, rendered);
            var golden = Utility.LoadGolden(name);
            Utility.SaveLoadedGolden(name, golden);
            var (pass, blend) = Utility.CompareAndHighlightDiff(rendered, golden);
            Utility.SaveBlend(name, blend);
            return pass;
        }

        BitmapSource RenderToBitmapSource(IReadOnlyDictionary<Beat, BeatGroup> score)
        {
            var layout = MusicModule.Layout.CreateLayout(score);
            var drawing = DrawingModule.MusicDrawingBuilder.BuildDrawing(layout);
            return Utility.RenderToBitmap(drawing);
        }
    }
}