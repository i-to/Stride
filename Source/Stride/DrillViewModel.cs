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

        public void UpdatePlayedPitch(Pitch? pitch)
        {
            Drill.SetPlayedPitch(pitch);
            var testNoteStaffPosition = ComputeStaffPosition(Drill.Staff.TestPitch);
            var playedNoteStaffPosition = ComputeStaffPosition(Drill.Staff.PlayedPitch);
            MusicDrawingBuilder.UpdateDrawing(testNoteStaffPosition, playedNoteStaffPosition);
            RaiseMusicDrawingChanged();
        }

        int ComputeStaffPosition(Pitch? pitchObj)
        {
            if (!pitchObj.HasValue) return -1;
            var pitch = pitchObj.Value;
            if (pitch == Pitch.D4) return 0;
            if (pitch == Pitch.E4) return 1;
            if (pitch == Pitch.F4) return 2;
            if (pitch == Pitch.G4) return 3;
            if (pitch == Pitch.A4) return 4;
            if (pitch == Pitch.B4) return 5;
            if (pitch == Pitch.C5) return 6;
            if (pitch == Pitch.D5) return 7;
            if (pitch == Pitch.E5) return 8;
            if (pitch == Pitch.F5) return 9;
            if (pitch == Pitch.G5) return 10;
            throw new ArgumentException($"Not supported Pitch: {pitch}");
        }
    }
}
