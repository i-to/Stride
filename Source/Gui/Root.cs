using Stride.Gui.Input;
using Stride.Gui.Model;

namespace Stride.Gui
{
    public class Root
    {
        public readonly App Application;
        public readonly MainWindow MainWindow;
        public readonly DrillControl DrillControl;
        public readonly DrillViewModel DrillViewModel;

        public Root()
        {
            Application = new App();
            var glyphRunBuilder = new GlyphRunBuilder();
            var staffLinesGeometryBuilder = new StaffLinesGeometryBuilder();
            var typefaceProvider = new TypefaceProvider();
            var musicSymbolToFontText = new MusicSymbolToFontText();
            var drawingContainer = new DrillMusicDrawingContainer();
            var drawingMetrics = new DrawingMetrics(baseSize: 8);
            var musicDrawingBuilder = new MusicDrawingBuilder(
                drawingMetrics, glyphRunBuilder, musicSymbolToFontText, staffLinesGeometryBuilder,
                typefaceProvider, drawingContainer);
            DrillViewModel = new DrillViewModel(musicDrawingBuilder, new Drill());
            var keyboardPitchMapping = new KeyboardPitchMapping();
            var noteInput = new NoteInput(DrillViewModel);
            var inputDispatcher = new Dispatcher(keyboardPitchMapping, noteInput);
            DrillControl = new DrillControl(DrillViewModel);
            MainWindow = new MainWindow(inputDispatcher) {Content = DrillControl};
        }

        public void Run()
        {
            DrillViewModel.InitializeDrill();
            Application.Run(MainWindow);
        }
    }
}