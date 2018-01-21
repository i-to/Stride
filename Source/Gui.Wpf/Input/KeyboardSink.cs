using System.Windows.Input;

namespace Stride.Gui.Wpf.Input
{
    public interface KeyboardSink
    {
        void KeyDown(KeyEventArgs args);
        void KeyUp(KeyEventArgs args);
    }
}