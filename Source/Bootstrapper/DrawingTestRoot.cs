using System;
using Stride.Bootstrapper.Test;
using Stride.Drawing.Wpf;
using Stride.Resources;
using Stride.Utility.Fluent;
using Stride.Utility.ImageProcessing;

namespace Stride.Bootstrapper
{
    public class DrawingTestRoot
    {
        public readonly TestScores TestScores;

        public readonly MusicModule MusicModule;
        public readonly DrawingModule DrawingModule;

        public DrawingTestRoot()
        {
            MusicModule = new MusicModule();
            TestScores = new TestScores(MusicModule.ScoreBuilder);
            var resourceLoader = ResourceLoader.OfCopiedResources();
            DrawingModule = new DrawingModule(resourceLoader);
        }

        public void Run()
        {
            var testSession = CreateTestSession();
            testSession.Prepare();
            testSession.Run(TestScores.Scores);
        }

        TestConfiguration CreateConfig(bool goldenDataGenerationMode, string outputSubfolder)
            => new TestConfiguration(
                bitmapDpi: 96,
                pixelFormat: BitmapConverter.PixelFormat,
                diffRatioThreshold: 1e-12,
                goldenImagePath: "../../Data/Test",
                outputImagePath: $"../../Output/Test/{outputSubfolder}",
                goldenDataGenerationMode: goldenDataGenerationMode);

        TestSession CreateTestSession()
        {
            var outputSubfolder = DateTime.Now.ToStringForFileName();
            var config = CreateConfig(false, outputSubfolder);
            var testUtility = new TestUtility(
                config,
                new BitmapUtility(),
                new BitmapConverter(),
                new BitmapBlender());
            var scoreRendererTest = new ScoreRendererTest(testUtility, MusicModule, DrawingModule);
            return new TestSession(config, testUtility, scoreRendererTest);
        }
    }
}