using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MoreLinq;
using Stride.Drawing.Wpf;
using Stride.Visualizer.Wpf;
using Stride.Music.Layout;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Resources;
using Duration = Stride.Music.Theory.Duration;

namespace Stride.Bootstrapper
{
    public class Root
    {
        public readonly CopiedResourcesPath CopiedResourcesPath;
        public readonly Application Application;
        public readonly Window MainWindow;
        public readonly MusicDrawingBuilder MusicDrawingBuilder;
        public readonly ScoreLayoutAlgorithm Layout;
        public readonly ScoreBuilder ScoreBuilder;
        public readonly DrawingControl DrawingControl;

        public Root()
        {
            CopiedResourcesPath = new CopiedResourcesPath();
            Application = new Application();
            var bravuraTypeface = LoadBravuraTypeface();
            var glyphRunBuilder = new GlyphRunBuilder(bravuraTypeface);
            var musicSymbolToFontText = new FontSymbolMapping();
            var metrics = new StavesMetrics(halfSpace: 8.0, staffLineThickness: 1.5);
            Layout = CreateLayoutEngine(metrics);
            ScoreBuilder = new ScoreBuilder(new StaffPositionComputation(), new BarsComputation(), Pitch.C4);
            //var layout = CreateTestLayoutEngine(metrics);
            MusicDrawingBuilder = new MusicDrawingBuilder(glyphRunBuilder, musicSymbolToFontText);
            DrawingControl = new DrawingControl();
            MainWindow = new Window
            {
                Content = DrawingControl,
                Width = 1200, Height = 900,
                Left = 100, Top = 50
            };
        }

        GlyphTypeface LoadBravuraTypeface()
        {
            var uri = CopiedResourcesPath.GetResourceUri("Fonts/Bravura.otf");
            return new GlyphTypeface(uri);
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

        public IEnumerable<Note> TestPhrase =>
            new[]
            {
                Note.Half(Pitch.D6), Note.Quarter(Pitch.E6), Note.Eighth(Pitch.C5), Note.Eighth(Pitch.B4), 
                Note.Quarter(Pitch.C4), Note.Quarter(Pitch.D4), Note.Quarter(Pitch.F4), Note.Quarter(Pitch.C4),
                Note.Whole(Pitch.B5)
            };

        public IEnumerable<Note> EighthNotePhrase =>
            new[]
            {
                Pitch.C4, Pitch.E4, Pitch.B4, Pitch.G4.Sharp, Pitch.A4, Pitch.B4, Pitch.C5, Pitch.D5,
                Pitch.E5, Pitch.C5, Pitch.B4, Pitch.A4, Pitch.G4.Sharp, Pitch.A4, Pitch.E4, Pitch.C4
            }
            .Select(Note.Eighth);

        public IEnumerable<IEnumerable<Pitch>> ChordsPhrase => new[]
            {
                new [] {Pitch.C4, Pitch.E4, Pitch.G4, Pitch.B4},
                new [] {Pitch.D4, Pitch.F4, Pitch.A4, Pitch.C5},
                new [] {Pitch.E4, Pitch.G4, Pitch.B4, Pitch.D5},
                new [] {Pitch.F4, Pitch.A4, Pitch.C5, Pitch.E5},
                new [] {Pitch.C4, Pitch.F4, Pitch.G4, Pitch.B4},
                new [] {Pitch.D4, Pitch.F4, Pitch.A4, Pitch.C5},
                new [] {Pitch.E4, Pitch.G4, Pitch.B4, Pitch.D5},
                new [] {Pitch.F4, Pitch.A4, Pitch.C5, Pitch.E5},
            };

        public void AddPhraseDrawing(IEnumerable<Note> phrase)
        {
            var score = ScoreBuilder.FromMelodicPhrase(phrase);
            AddScoreDrawing(score);
        }

        public void AddScoreDrawing(IReadOnlyDictionary<Beat, BeatGroup> score)
        {
            var layout = Layout.CreateLayout(score);
            var drawing = MusicDrawingBuilder.BuildDrawing(layout);
            DrawingControl.AddDrawing(drawing);
        }

        public void AddChordPhraseDrawing()
        {
            var chordPhrase = ChordsPhrase.Select(chords => (chords, Duration.Quarter));
            var score = ScoreBuilder.FromChordsPhrase(chordPhrase);
            AddScoreDrawing(score);
        }

        public void Run()
        {
            AddPhraseDrawing(TestPhrase);
            AddPhraseDrawing(EighthNotePhrase);
            AddChordPhraseDrawing();
            Application.Run(MainWindow);
        }
    }
}