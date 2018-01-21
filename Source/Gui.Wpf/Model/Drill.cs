using System.Collections.Generic;
using Stride.Music.Theory;

namespace Stride.Gui.Wpf.Model
{
    public class Drill
    {
        public readonly IReadOnlyList<Note> Notes;
        public readonly Pitch LowestTreebleSaffPitch;

        public Drill(IReadOnlyList<Note> notes, Pitch lowestTreebleSaffPitch)
        {
            Notes = notes;
            LowestTreebleSaffPitch = lowestTreebleSaffPitch;
        }
    }
}