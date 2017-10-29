using System.Windows.Input;
using Stride.Music;

namespace Stride.Gui.Input
{
    public class KeyboardPitchMapping
    {
        public Pitch Map(Key key)
        {
            switch (key)
            {
                case Key.A: return Pitch.A4;
                case Key.B: return Pitch.B4;
                case Key.C: return Pitch.C5;
                case Key.D: return Pitch.D5;
                case Key.E: return Pitch.E5;
                case Key.F: return Pitch.F5;
                case Key.G: return Pitch.G5;
            }
            return null;
        }
    }
}