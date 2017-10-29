using System;
using System.Collections.Generic;
using System.Linq;
using Stride.Gui.Input;
using Stride.Gui.Model;
using Stride.Gui.MusicDrawing;
using Stride.Gui.Persistence;
using Stride.Music.Layout;

namespace Stride.Gui
{
    public class Root
    {
        readonly NoteInputMode NoteInputMode;
        readonly DrillId CurrentDrill = DrillId.Testing;

        public readonly App Application;
        public readonly MainWindow MainWindow;
        public readonly DrillControl DrillControl;
        public readonly DrillViewModel DrillViewModel;
        public readonly DrillPresenter DrillPresenter;
        public readonly Database Database;
        public readonly Drills Drills;

        public Root()
        {
            NoteInputMode = Properties.Settings.Default.NoteInputMode;
            Application = new App();
            var glyphRunBuilder = new GlyphRunBuilder();
            var staffLinesGeometryBuilder = new StaffGeometryBuilder();
            var typefaceProvider = new MusicTypefaceProvider();
            var musicSymbolToFontText = new MusicSymbolToFontText();
            var drawingContainer = new DrillMusicDrawingContainer();
            var drawingMetrics = new StavesMetrics(baseSize: 8);
            var musicDrawingBuilder = new MusicDrawingBuilder(
                drawingMetrics,
                glyphRunBuilder,
                musicSymbolToFontText,
                staffLinesGeometryBuilder,
                typefaceProvider,
                drawingContainer);
            DrillPresenter = new DrillPresenter(new AnswerTracker(), new PerformanceFeedback());
            DrillViewModel = new DrillViewModel(musicDrawingBuilder, DrillPresenter);
            var keyboardPitchMapping = new KeyboardPitchMapping();
            var midiPitchMapping = new MidiPitchMapping();
            var noteInputConverter = new NoteInputConverter(
                keyboardPitchMapping,
                midiPitchMapping,
                DrillViewModel,
                NoteInputMode);
            DrillControl = new DrillControl(DrillViewModel);
            var midiSink = NoteInputMode == NoteInputMode.Midi ? noteInputConverter : null;
            MainWindow = new MainWindow(noteInputConverter, midiSink) {Content = DrillControl};
            Database = new Database();
            Drills = new Drills();
        }
        
        DrillSession CreateDrillSession(DrillId drillId, IReadOnlyList<int> lastSessionWeights)
        {
            var drill = Drills[drillId];
            var pitchWeights = lastSessionWeights ?? Enumerable.Range(0, drill.Pitches.Count).Select(_ => 10);
            return new DrillSession(drill, pitchWeights);
        }

        public void Run()
        {
            var database = Database.Load(CurrentDrill);
            var lastSessionWeights = database.LastOrDefault()?.Weights;
            var session = CreateDrillSession(CurrentDrill, lastSessionWeights);
            DrillPresenter.Start(session);
            DrillViewModel.InitializeDrillDrawing();
            Application.Run(MainWindow);
            var sessionResult = new SessionRecord(DateTime.Now, session.PitchWeights);
            database.Add(sessionResult);
            Database.Save(CurrentDrill, database);
            MainWindow.Dispose();
        }
    }
}