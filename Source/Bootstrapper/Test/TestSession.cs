using System;
using System.Collections.Generic;
using Stride.Music.Score;

namespace Stride.Bootstrapper.Test
{
    public class TestSession
    {
        public readonly TestConfiguration Config;
        public readonly TestUtility TestUtility;
        public readonly ScoreRendererTest ScoreRendererTest;

        public TestSession(TestConfiguration config, TestUtility testUtility, ScoreRendererTest scoreRendererTest)
        {
            Config = config;
            TestUtility = testUtility;
            ScoreRendererTest = scoreRendererTest;
        }

        public void Prepare()
        {
            if (Config.GoldenDataGenerationMode)
                return;

            TestUtility.AssureOutputFolderExists();
            if (!TestUtility.IsOutputFolderEmpty())
                throw new InvalidOperationException("Output folder is not empty.");
        }

        public void Run(IReadOnlyDictionary<string, IReadOnlyDictionary<Beat, BeatGroup>> testScores)
        {
            var success = true;

            foreach (var kvp in testScores)
                success &= ScoreRendererTest.Run(kvp.Key, kvp.Value, Config.GoldenDataGenerationMode);

            if (!Config.GoldenDataGenerationMode && !success)
                throw new InvalidOperationException("Test failed");
        }
    }
}