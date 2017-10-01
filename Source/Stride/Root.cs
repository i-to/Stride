using Stride.Input;
using Stride.Model;
using Stride.MusicDrawing;

namespace Stride
{
    public class Root
    {
        readonly NoteInputMode NoteInputMode = NoteInputMode.Midi;

        public readonly App Application;
        public readonly MainWindow MainWindow;
        public readonly DrillControl DrillControl;
        public readonly DrillViewModel DrillViewModel;

        public Root()
        {
            Application = new App();
            var glyphRunBuilder = new GlyphRunBuilder();
            var staffLinesGeometryBuilder = new StaffGeometryBuilder();
            var typefaceProvider = new MusicTypefaceProvider();
            var musicSymbolToFontText = new MusicSymbolToFontText();
            var drawingContainer = new DrillMusicDrawingContainer();
            var drawingMetrics = new StavesMetrics(baseSize: 8);
            var musicDrawingBuilder = new MusicDrawingBuilder(
                drawingMetrics, glyphRunBuilder, musicSymbolToFontText, staffLinesGeometryBuilder,
                typefaceProvider, drawingContainer);
            DrillViewModel = new DrillViewModel(musicDrawingBuilder, new Drill());
            var keyboardPitchMapping = new KeyboardPitchMapping();
            var noteInput = new NoteInput(DrillViewModel);
            var midiPitchMapping = new MidiPitchMapping();
            var noteInputConverter = new NoteInputConverter(keyboardPitchMapping, midiPitchMapping, noteInput,
                NoteInputMode);
            DrillControl = new DrillControl(DrillViewModel);
            MainWindow = new MainWindow(noteInputConverter, noteInputConverter) {Content = DrillControl};
        }

        public void Run()
        {
            DrillViewModel.InitializeDrill();
            Application.Run(MainWindow);
            MainWindow.Dispose();
        }
    }
}