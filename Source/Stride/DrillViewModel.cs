using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Stride.Input;
using Stride.Model;
using Stride.Music;
using Stride.MusicDrawing;
using Stride.Utility;

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
            var soundingPitches = DrillPresenter.SoundingPitches.OrderDescending().ToReadOnlyList();
            var soundingNotesStaffPositions = ComputeStaffPositions(soundingPitches);
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

        IReadOnlyList<StaffPosition> ComputeStaffPositions(IReadOnlyList<Pitch> pitches)
        {
            var count = pitches.Count;
            var result = new StaffPosition[count];
            for (int i = 0; i != count; ++i)
            {
                var pitch = pitches[i];
                var position = ComputeStaffPosition(pitch);
                if (i > 0)
                {
                    var previousPosition = result[i - 1];
                    if (!previousPosition.HorisontalOffset
                        && previousPosition.Clef == position.Clef
                        && previousPosition.VerticalOffset - position.VerticalOffset == 1)
                    {
                        position = position.WithHorizontalOffset(true);
                    }
                }
                result[i] = position;
            }
            return result;
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
