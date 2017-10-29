using System.Collections.Generic;
using System.Linq;
using Stride.Music;
using Stride.Utility;

namespace Stride.Model
{
    public class Drills
    {
        public readonly IReadOnlyList<Drill> Instances = 
            new[]
            {
                new Drill(
                    DrillId.DiatonicOneOctaveC3,
                    CreatePitches(Pitch.C3, 8),
                    Pitch.D4
                    ),
                new Drill(
                    DrillId.DiatonicOneOctaveC4,
                    CreatePitches(Pitch.C4, 8),
                    Pitch.C4),
                new Drill(
                    DrillId.DiatonicTwoOctavesC3,
                    CreatePitches(Pitch.C3, 15),
                    Pitch.C4), 
                new Drill(
                    DrillId.DiatonicTwoOctavesC4,
                    CreatePitches(Pitch.C4, 15),
                    Pitch.C4),
                new Drill(
                    DrillId.Testing,
                    CreatePitches(Pitch.C4, 7),
                    Pitch.C4), 
            };

        static IReadOnlyList<Pitch> CreatePitches(Pitch first, int count) =>
            Pitches.DiatonicRange(first, count).ToReadOnlyList();

        public Drill this[DrillId id] => Instances.Single(drill => drill.Id == id);
    }
}
