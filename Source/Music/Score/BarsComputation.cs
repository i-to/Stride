using System;
using System.Collections.Generic;
using Rationals;
using Stride.Music.Theory;

namespace Stride.Music.Score
{
    public class BarsComputation
    {
        public IEnumerable<Beat> SplitToBars(IEnumerable<Note> notes)
        {
            int bar = 0;
            var spaceLeft = Rational.One;
            foreach (var note in notes)
            {
                yield return new Beat(bar, 1 - spaceLeft);
                spaceLeft = spaceLeft - note.Duration.Fraction();
                if (spaceLeft.IsZero)
                {
                    ++bar;
                    spaceLeft = Rational.One;
                }
                else if (spaceLeft < 0)
                {
                    throw new InvalidOperationException("Bar overflow");
                }
            }
        }
    }
}