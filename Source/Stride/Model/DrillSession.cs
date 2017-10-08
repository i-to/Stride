using System.Collections.Generic;
using System.Linq;

namespace Stride.Model
{
    public class DrillSession
    {
        public readonly Drill Drill;
        public readonly int[] PitchWeights;
        
        public DrillSession(Drill drill, IEnumerable<int> initialPitchWeights)
        {
            Drill = drill;
            PitchWeights = initialPitchWeights.ToArray();
        }
    }
}