using System;
using System.Collections.Generic;

namespace Stride.Music.Theory
{
    public class Pitches
    {
        public static IEnumerable<Pitch> DiatonicRange(Pitch first, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException($"Expected positive count, given: {count}");
            while (count > 0)
            {
                yield return first;
                --count;
                first = first.NextDiatonic;
            }
        }
    }
}