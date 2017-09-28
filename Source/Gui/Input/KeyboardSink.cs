using System.Windows.Input;

namespace Stride.Gui.Input
{
    public interface KeyboardSink
    {
        void KeyDown(KeyEventArgs args);
        void KeyUp(KeyEventArgs args);
    }
}