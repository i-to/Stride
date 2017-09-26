using System;
using System.Windows.Media;

namespace Stride.Gui
{
    public class DrillViewModel
    {
        readonly MusicDrawingBuilder MusicDrawingBuilder;

        public DrillViewModel(MusicDrawingBuilder musicDrawingBuilder)
        {
            MusicDrawingBuilder = musicDrawingBuilder;
        }

        public Drawing MusicDrawing => MusicDrawingBuilder.Drawing;

        public void SetupDrawing()
        {
            var testNoteStaffPosition = ComputeStaffPosition(Pitch.B4);
            var playedNoteStaffPosition = ComputeStaffPosition(Pitch.F5);
            MusicDrawingBuilder.UpdateDrawing(testNoteStaffPosition, playedNoteStaffPosition);
        }

        int ComputeStaffPosition(Pitch pitch)
        {
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
