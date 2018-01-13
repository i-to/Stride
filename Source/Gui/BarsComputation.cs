using System;
using System.Collections.Generic;
using Rationals;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Gui
{
    public class BarsComputation
    {
        public IReadOnlyList<Tick> SplitToBars(IEnumerable<Note> notes)
        {
            var ticks = new List<Tick>();
            int bar = 0;
            var spaceLeft = Rational.One;
            foreach (var note in notes)
            {
                ticks.Add(new Tick(bar, 1 - spaceLeft));
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
            return ticks;
        }
    }
}