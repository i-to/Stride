using System;
using System.Windows.Media;
using Stride.Gui.Input;
using Stride.Gui.Model;
using Stride.Gui.MusicDrawing;
using Stride.Music.Layout;
using Stride.Music.Theory;

namespace Stride.Gui
{
    public class DrillViewModel : NoteSink
    {
        readonly MusicDrawingBuilder MusicDrawingBuilder;
        readonly DrillQuiz DrillQuiz;
        readonly LayoutEngine LayoutEngine;
        readonly DrillPageLayout DrillPageLayout;

        public DrillViewModel(
            MusicDrawingBuilder musicDrawingBuilder,
            DrillQuiz drillQuiz,
            LayoutEngine layoutEngine,
            DrillPageLayout drillPageLayout)
        {
            MusicDrawingBuilder = musicDrawingBuilder;
            DrillQuiz = drillQuiz;
            LayoutEngine = layoutEngine;
            DrillPageLayout = drillPageLayout;
        }

        public Drawing MusicDrawing => MusicDrawingBuilder.Drawing;
        public event EventHandler MusicDrawingChanged;
        void RaiseMusicDrawingChanged() => MusicDrawingChanged?.Invoke(this, EventArgs.Empty);

        public void InitializeDrillDrawing() => Update();

        void Update()
        {
            var page = DrillPageLayout.CreatePage(
                DrillQuiz.LowestTreebleStaffPitch,
                DrillQuiz.TestPhrase,
                DrillQuiz.SoundingPitches,
                DrillQuiz.CurrentPosition);
            var layout = LayoutEngine.CreateLayout(page);
            MusicDrawingBuilder.BuildDrawing(layout);
            RaiseMusicDrawingChanged();
        }

        public void NoteOn(Pitch pitch)
        {
            DrillQuiz.PitchOn(pitch);
            Update();
        }

        public void NoteOff(Pitch pitch)
        {
            DrillQuiz.PitchOff(pitch);
            Update();
        }
    }
}
