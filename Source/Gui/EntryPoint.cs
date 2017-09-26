using System;
using System.Windows;

namespace Stride.Gui
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            var window = new Window();
            var glyphRunBuilder = new GlyphRunBuilder();
            var staffLinesGeometryBuilder = new StaffLinesGeometryBuilder();
            var typefaceProvider = new TypefaceProvider();
            var drawingMetrics = new DrawingMetrics(baseSize: 8);
            var musicSymbolToFontText = new MusicSymbolToFontText();
            var drawingContainer = new DrillMusicDrawingContainer();
            var musicDrawingBuilder = new MusicDrawingBuilder(
                drawingMetrics, glyphRunBuilder, musicSymbolToFontText, staffLinesGeometryBuilder,
                typefaceProvider, drawingContainer);
            var drillViewModel = new DrillViewModel(musicDrawingBuilder);
            var drillControl = new DrillControl(drillViewModel);
            window.Content = drillControl;
            drillViewModel.SetupDrawing();
            app.Run(window);
        }
    }
}