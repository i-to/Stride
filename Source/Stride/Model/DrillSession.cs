using System.Collections.Generic;
using System.Linq;
using Stride.Music;

namespace Stride.Model
{
    public class DrillSession
    {
        public readonly IReadOnlyList<Pitch> Pitches;
        public readonly int[] PitchWeights;
        
        public DrillSession(IReadOnlyList<Pitch> pitches, IEnumerable<int> initialPitchWeights)
        {
            Pitches = pitches;
            PitchWeights = initialPitchWeights.ToArray();
        }
    }
}