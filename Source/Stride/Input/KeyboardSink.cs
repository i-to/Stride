using System.Windows.Input;

namespace Stride.Input
{
    public interface KeyboardSink
    {
        void KeyDown(KeyEventArgs args);
        void KeyUp(KeyEventArgs args);
    }
}