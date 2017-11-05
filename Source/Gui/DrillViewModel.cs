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
        readonly DrillPresenter DrillPresenter;
        readonly LayoutEngine LayoutEngine;

        public DrillViewModel(
            MusicDrawingBuilder musicDrawingBuilder,
            DrillPresenter drillPresenter,
            LayoutEngine layoutEngine)
        {
            MusicDrawingBuilder = musicDrawingBuilder;
            DrillPresenter = drillPresenter;
            LayoutEngine = layoutEngine;
        }

        public Drawing MusicDrawing => MusicDrawingBuilder.Drawing;
        public event EventHandler MusicDrawingChanged;
        void RaiseMusicDrawingChanged() => MusicDrawingChanged?.Invoke(this, EventArgs.Empty);

        public void InitializeDrillDrawing() => Update();

        void Update()
        {
            var layout = LayoutEngine.CreateLayout(
                DrillPresenter.LowestTreebleStaffPitch,
                DrillPresenter.TestPitch,
                DrillPresenter.SoundingPitches);
            MusicDrawingBuilder.BuildDrawing(layout);
            RaiseMusicDrawingChanged();
        }

        public void NoteOn(Pitch pitch)
        {
            DrillPresenter.PitchOn(pitch);
            Update();
        }

        public void NoteOff(Pitch pitch)
        {
            DrillPresenter.PitchOff(pitch);
            Update();
        }
    }
}
