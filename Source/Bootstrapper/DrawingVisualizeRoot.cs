using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Visualizer.Wpf;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Resources;
using Duration = Stride.Music.Theory.Duration;

namespace Stride.Bootstrapper
{
    public class DrawingVisualizeRoot
    {
        public readonly MusicModule MusicModule;
        public readonly DrawingModule DrawingModule;

        public readonly Application Application;
        public readonly Window MainWindow;
        public readonly DrawingControl DrawingControl;

        public DrawingVisualizeRoot()
        {
            MusicModule = new MusicModule();
            var resourceLoader = ResourceLoader.OfCopiedResources();
            DrawingModule = new DrawingModule(resourceLoader);

            Application = new Application();
            DrawingControl = new DrawingControl();
            MainWindow = new Window
            {
                Content = DrawingControl,
                Width = 1200, Height = 900,
                Left = 100, Top = 50
            };
        }

        public void AddPhraseDrawing(IEnumerable<Note> phrase)
        {
            var score = MusicModule.ScoreBuilder.FromMelodicPhrase(phrase);
            AddScoreDrawing(score);
        }

        public void AddScoreDrawing(IReadOnlyDictionary<Beat, BeatGroup> score)
        {
            var layout = MusicModule.Layout.CreateLayout(score);
            var drawing = DrawingModule.MusicDrawingBuilder.BuildDrawing(layout);
            DrawingControl.AddDrawing(drawing);
        }

        public void Run()
        {
            var testScores = new TestScores(MusicModule.ScoreBuilder);
            AddScoreDrawing(testScores.SimpleTestPhraseScore);
            AddScoreDrawing(testScores.EightNotePhraseScore);
            AddScoreDrawing(testScores.ChordPhraseScore);
            Application.Run(MainWindow);
        }
    }
}