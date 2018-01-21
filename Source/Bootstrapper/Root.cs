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

namespace Stride.Bootstrapper
{
    public class Root
    {
        public readonly CopiedResourcesPath CopiedResourcesPath;
        public readonly Application Application;
        public readonly Window MainWindow;
        public readonly MusicDrawingBuilder MusicDrawingBuilder;
        public readonly ScoreLayout Layout;
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
            ScoreBuilder = new ScoreBuilder(new StaffPositionComputation(), new BarsComputation());
            //var layout = CreateTestLayoutEngine(metrics);
            MusicDrawingBuilder = new MusicDrawingBuilder(glyphRunBuilder, musicSymbolToFontText);
            DrawingControl = new DrawingControl();
            MainWindow = new Window {Content = DrawingControl};
        }

        GlyphTypeface LoadBravuraTypeface()
        {
            var uri = CopiedResourcesPath.GetResourceUri("Fonts/Bravura.otf");
            return new GlyphTypeface(uri);
        }

        ScoreLayout CreateLayoutEngine(StavesMetrics metrics)
        {
            var verticalLayout = new VerticalLayout(metrics);
            var staffLinesGeometryBuilder = new StaffLinesLayout();
            var ledgerLinesComputation = new LedgerLinesComputation();
            var stemLayout = new DurationsLayout(metrics, verticalLayout);
            return new ScoreLayout(metrics, staffLinesGeometryBuilder, ledgerLinesComputation, verticalLayout, stemLayout);
        }

        TestLayout CreateTestLayout(StavesMetrics metrics)
        {
            return new TestLayout(metrics);
        }

        IEnumerable<Note> TestPhrase =>
            new[]
            {
                Note.Half(Pitch.D6), Note.Quarter(Pitch.E6), Note.Eighth(Pitch.C5), Note.Eighth(Pitch.B4), 
                Note.Quarter(Pitch.C4), Note.Quarter(Pitch.D4), Note.Quarter(Pitch.F4), Note.Quarter(Pitch.C4),
                Note.Whole(Pitch.B5)
            };

        IEnumerable<Note> EighthNotePhrase =>
            new[]
            {
                Pitch.C4, Pitch.E4, Pitch.B4, Pitch.G4.Sharp, Pitch.A4, Pitch.B4, Pitch.C5, Pitch.D5,
                Pitch.E5, Pitch.C5, Pitch.B4, Pitch.A4, Pitch.G4.Sharp, Pitch.A4, Pitch.E4, Pitch.C4
            }
            .Select(Note.Eighth);

        

        public void AddPhraseDrawing(IEnumerable<Note> phrase)
        {
            var lowestTreebleStaffPitch = Pitch.C4;
            var score = ScoreBuilder.CreateScore(lowestTreebleStaffPitch, phrase);
            var layout = Layout.CreateLayout(score);
            var drawing = MusicDrawingBuilder.BuildDrawing(layout);
            DrawingControl.AddDrawing(drawing);
        }

        public void Run()
        {
            AddPhraseDrawing(TestPhrase);
            AddPhraseDrawing(EighthNotePhrase);
            Application.Run(MainWindow);
        }
    }
}