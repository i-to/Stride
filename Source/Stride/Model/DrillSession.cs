using System.Collections.Generic;
using Stride.Music;

namespace Stride.Model
{
    public class DrillSession
    {
        public readonly IReadOnlyList<Pitch> Pitches;
        public readonly double[] PitchWeights;
        
        public DrillSession(IReadOnlyList<Pitch> pitches, double[] pitchWeights)
        {
            Pitches = pitches;
            PitchWeights = pitchWeights;
        }
    }
}