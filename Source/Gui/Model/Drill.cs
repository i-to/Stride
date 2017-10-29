using System.Collections.Generic;
using Stride.Music;

namespace Stride.Gui.Model
{
    public class Drill
    {
        public readonly DrillId Id;
        public readonly IReadOnlyList<Pitch> Pitches;
        public readonly Pitch LowestTreebleSaffPitch;

        public Drill(DrillId id, IReadOnlyList<Pitch> pitches, Pitch lowestTreebleSaffPitch)
        {
            Id = id;
            Pitches = pitches;
            LowestTreebleSaffPitch = lowestTreebleSaffPitch;
        }
    }
}