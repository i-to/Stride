using System;
using System.Collections.Generic;
using System.Linq;
using Stride.Input;
using Stride.Model;
using Stride.Music;
using Stride.MusicDrawing;
using Stride.Persistence;
using Stride.Utility;

namespace Stride
{
    public class Root
    {
        readonly NoteInputMode NoteInputMode = NoteInputMode.Midi;

        public readonly App Application;
        public readonly MainWindow MainWindow;
        public readonly DrillControl DrillControl;
        public readonly DrillViewModel DrillViewModel;
        public readonly DrillPresenter DrillPresenter;
        public readonly Database Database;

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
            MainWindow = new MainWindow(noteInputConverter, noteInputConverter) {Content = DrillControl};
            Database = new Database();
        }
        
        DrillSession CreateDrillSession(IReadOnlyList<int> lastSessionWeights)
        {
            var pitches = Pitches.DiatonicRange(Pitch.C4, 15).ToArray().ToReadOnlyList();
            var pitchWeights = lastSessionWeights ?? Enumerable.Range(0, pitches.Count).Select(_ => 10);
            return new DrillSession(pitches, pitchWeights);
        }

        public void Run()
        {
            var database = Database.Load();
            var lastSessionWeights = database.LastOrDefault()?.Weights;
            var session = CreateDrillSession(lastSessionWeights);
            DrillPresenter.Start(session);
            DrillViewModel.InitializeDrillDrawing();
            Application.Run(MainWindow);
            var sessionResult = new SessionRecord(DateTime.Now, session.PitchWeights);
            database.Add(sessionResult);
            Database.Save(database);
            MainWindow.Dispose();
        }
    }
}