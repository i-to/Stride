using System;
using System.Windows.Media;
using Stride.Model;
using Stride.Music;
using Stride.MusicDrawing;

namespace Stride
{
    public class DrillViewModel
    {
        readonly MusicDrawingBuilder MusicDrawingBuilder;
        readonly Drill Drill;

        public DrillViewModel(MusicDrawingBuilder musicDrawingBuilder, Drill drill)
        {
            MusicDrawingBuilder = musicDrawingBuilder;
            Drill = drill;
        }

        public Drawing MusicDrawing => MusicDrawingBuilder.Drawing;
        public event EventHandler MusicDrawingChanged;
        void RaiseMusicDrawingChanged() => MusicDrawingChanged?.Invoke(this, EventArgs.Empty);

        public void InitializeDrill() => UpdatePlayedPitch(null);

        public void UpdatePlayedPitch(Pitch pitch)
        {
            Drill.SetPlayedPitch(pitch);
            var testNoteStaffPosition = ComputeStaffPosition(Drill.Staff.TestPitch);
            var playedNoteStaffPosition = ComputeStaffPosition(Drill.Staff.PlayedPitch);
            MusicDrawingBuilder.UpdateDrawing(testNoteStaffPosition, playedNoteStaffPosition);
            RaiseMusicDrawingChanged();
        }

        StaffPosition ComputeStaffPosition(Pitch pitch)
        {
            if (pitch is null)
                return null;
            return pitch >= Pitch.C4
                ? StaffPosition.InTreebleClef(-pitch.DiatonicDistanceTo(Pitch.B4))
                : StaffPosition.InBassClef(-pitch.DiatonicDistanceTo(Pitch.D3));
        }
    }
}
