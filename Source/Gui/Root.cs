using System.Linq;
using Stride.Gui.Input;
using Stride.Gui.Model;
using Stride.Gui.MusicDrawing;
using Stride.Music.Layout;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Gui
{
    public class Root
    {
        readonly NoteInputMode NoteInputMode;

        public readonly App Application;
        public readonly MainWindow MainWindow;
        public readonly DrillControl DrillControl;
        public readonly DrillViewModel DrillViewModel;
        public readonly DrillQuiz DrillQuiz;

        public Root()
        {
            //Properties.Settings.Default.NoteInputMode = NoteInputMode.Midi;
            //Properties.Settings.Default.Save();
            NoteInputMode = Properties.Settings.Default.NoteInputMode;
            Application = new App();
            var glyphRunBuilder = new GlyphRunBuilder();
            var typefaceProvider = new MusicTypefaceProvider();
            var musicSymbolToFontText = new FontSymbolMapping();
            var layoutEngine = CreateLayoutEngine();
            var musicDrawingBuilder = new MusicDrawingBuilder(
                glyphRunBuilder,
                musicSymbolToFontText,
                typefaceProvider);
            DrillQuiz = new DrillQuiz();
            var drillPageLayout = new DrillPageLayout(new StaffPositionComputation());
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

        LayoutEngine CreateLayoutEngine()
        {
            var staffLinesGeometryBuilder = new StaffLinesLayout();
            var drawingMetrics = new StavesMetrics(baseSize: 8);
            var ledgerLinesComputation = new LedgerLinesComputation();
            return new LayoutEngine(drawingMetrics, staffLinesGeometryBuilder, ledgerLinesComputation);
        }

        public void Run()
        {
            var testPhrase = 
                new[] {Pitch.C6, Pitch.D6, Pitch.E6, Pitch.B5}
                .Select(Note.Whole)
                .ToReadOnlyList();
            var drill = new Drill(testPhrase, Pitch.C4);
            DrillQuiz.Start(drill);
            DrillViewModel.InitializeDrillDrawing();
            Application.Run(MainWindow);
            MainWindow.Dispose();
        }
    }
}