using System;
using System.Collections.Generic;
using Stride.Gui.Music;

namespace Stride.Gui.Model
{
    public class Drill
    {
        readonly IReadOnlyList<Pitch> Pitches = new[]
        {
            Pitch.D4, Pitch.E4, Pitch.F4, Pitch.G4, Pitch.A4, Pitch.B4,
            Pitch.C5, Pitch.D5, Pitch.E5, Pitch.F5, Pitch.G5
        };

        readonly Random Random;

        public Drill()
        {
            Random = new Random();
            Reset();
        }

        public DrillStaff Staff { get; private set; }

        Pitch RandomPitch => Pitches[Random.Next(0, Pitches.Count)];

        public void Reset() => Staff = new DrillStaff(RandomPitch);
        public void SetPlayedPitch(Pitch? pitch)
        {
            if (pitch == null && Staff.TestPitch == Staff.PlayedPitch)
                Reset();
            Staff = Staff.WithPlayedPitch(pitch);
        }
    }
}
