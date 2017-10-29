using System;
using System.Linq;
using System.Windows.Media;
using Stride.Input;
using Stride.Model;
using Stride.Music;
using Stride.MusicDrawing;

namespace Stride
{
    public class DrillViewModel : NoteSink
    {
        readonly MusicDrawingBuilder MusicDrawingBuilder;
        readonly DrillPresenter DrillPresenter;

        public DrillViewModel(MusicDrawingBuilder musicDrawingBuilder, DrillPresenter drillPresenter)
        {
            MusicDrawingBuilder = musicDrawingBuilder;
            DrillPresenter = drillPresenter;
        }

        public Drawing MusicDrawing => MusicDrawingBuilder.Drawing;
        public event EventHandler MusicDrawingChanged;
        void RaiseMusicDrawingChanged() => MusicDrawingChanged?.Invoke(this, EventArgs.Empty);

        public void InitializeDrillDrawing() => Update();

        void Update()
        {
            var testNoteStaffPosition = ComputeStaffPosition(DrillPresenter.TestPitch);
            var soundingNotesStaffPositions = DrillPresenter.SoundingPitches.Select(ComputeStaffPosition);
            MusicDrawingBuilder.UpdateDrawing(testNoteStaffPosition, soundingNotesStaffPositions);
            RaiseMusicDrawingChanged();
        }

        StaffPosition ComputeStaffPosition(Pitch pitch)
        {
            if (pitch is null)
                return null;
            return pitch >= DrillPresenter.LowestTreebleStaffPitch
                ? StaffPosition.InTreebleClef(-pitch.DiatonicDistanceTo(Pitch.B4))
                : StaffPosition.InBassClef(-pitch.DiatonicDistanceTo(Pitch.D3));
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
