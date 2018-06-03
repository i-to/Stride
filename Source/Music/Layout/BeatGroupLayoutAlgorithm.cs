using System.Collections.Generic;
using System.Linq;
using Stride.Music.Score;
using Stride.Utility;

namespace Stride.Music.Layout
{
    public class BeatGroupLayoutAlgorithm
    {
        public BeatGroupLayout ComputeBeatGroupLayout(BeatGroup group)
        {
            var offsetNotes = ComputeOffsetNotes(group.ScoreNotes).ToReadOnlyList();
            return new BeatGroupLayout(offsetNotes);
        }

        IEnumerable<int> ComputeOffsetNotes(IReadOnlyList<ScoreNote> notes)
        {
            var previousOffset = false;
            for (int i = 1; i != notes.Count; ++i)
            {
                var prev = notes[i - 1].StaffPosition;
                var curr = notes[i].StaffPosition;

                if (prev.Clef != curr.Clef
                 || curr.VerticalOffset - prev.VerticalOffset != 1)
                {
                    previousOffset = false;
                    continue;
                }

                if (previousOffset)
                {
                    previousOffset = false;
                    continue;
                }

                previousOffset = true;
                yield return i;
            }
        }
    }
}