using System;
using System.Windows;
using System.Windows.Media;
using Stride.Gui;
using Stride.Gui.Input;
using Stride.Gui.Model;
using Stride.Gui.MusicDrawing;
using Stride.Music.Layout;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Resources;

namespace Stride.Bootstrapper
{
    public class Root
    {
        readonly NoteInputMode NoteInputMode;

        public readonly CopiedResourcesPath CopiedResourcesPath;
        public readonly Application Application;
        public readonly MainWindow MainWindow;
        public readonly DrillControl DrillControl;
        public readonly DrillViewModel DrillViewModel;
        public readonly DrillQuiz DrillQuiz;

        public Root()
        {
            //Properties.Settings.Default.NoteInputMode = NoteInputMode.Midi;
            //Properties.Settings.Default.Save();
            NoteInputMode = Properties.Settings.Default.NoteInputMode;
            CopiedResourcesPath = new CopiedResourcesPath();
            Application = new Application();
            var bravuraTypeface = LoadBravuraTypeface();
            var glyphRunBuilder = new GlyphRunBuilder(bravuraTypeface);
            var musicSymbolToFontText = new FontSymbolMapping();
            var metrics = new StavesMetrics(halfSpace: 8.0, staffLineThickness: 1.5);
            var layoutEngine = CreateLayoutEngine(metrics);
            //var layout = CreateTestLayoutEngine(metrics);
            var musicDrawingBuilder = new MusicDrawingBuilder(glyphRunBuilder, musicSymbolToFontText);
            DrillQuiz = new DrillQuiz();
            var drillPageLayout = new DrillPageLayout(new StaffPositionComputation(), new BarsComputation());
            DrillViewModel = new DrillViewModel(musicDrawingBuilder, DrillQuiz, layoutEngine, drillPageLayout);
            var keyboardPitchMappings = new KeyboardPitchMappings();
            var midiPitchMapping = new MidiPitchMapping();
            var noteInputConverter = new NoteInputConverter(
                keyboardPitchMappings,
                midiPitchMapping,
                DrillViewModel,
                NoteInputMode);
            DrillControl = new DrillControl(DrillViewModel);
            var midiSink = NoteInputMode == NoteInputMode.Midi ? noteInputConverter : null;
            MainWindow = new MainWindow(noteInputConverter, midiSink) {Content = DrillControl};
        }

        GlyphTypeface LoadBravuraTypeface()
        {
            var uri = CopiedResourcesPath.GetResourceUri("Fonts/Bravura.otf");
            return new GlyphTypeface(uri);
        }

        Layout CreateLayoutEngine(StavesMetrics metrics)
        {
            var verticalLayout = new VerticalLayout(metrics);
            var staffLinesGeometryBuilder = new StaffLinesLayout();
            var ledgerLinesComputation = new LedgerLinesComputation();
            var stemLayout = new StemLayout(metrics, verticalLayout);
            return new PageLayout(metrics, staffLinesGeometryBuilder, ledgerLinesComputation, verticalLayout, stemLayout);
        }

        Layout CreateTestLayoutEngine(StavesMetrics metrics)
        {
            return new TestLayout(metrics);
        }

        public void Run()
        {
            var testPhrase = new[]
            {   
                Note.Whole(Pitch.C6),
                Note.Half(Pitch.D6), Note.Half(Pitch.E6),
                Note.Quarter(Pitch.C4), Note.Quarter(Pitch.D4), Note.Quarter(Pitch.F4),Note.Quarter(Pitch.C4),
                Note.Whole(Pitch.B5)
            };
            var drill = new Drill(testPhrase, Pitch.C4);
            DrillQuiz.Start(drill);
            DrillViewModel.InitializeDrillDrawing();
            Application.Run(MainWindow);
            MainWindow.Dispose();
        }
    }
}