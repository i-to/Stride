using System.Windows;
using System.Windows.Input;

namespace Stride.Gui.Input
{
    public class MainWindow : Window
    {
        readonly KeyboardSink KeyboardSink;

        public MainWindow(KeyboardSink keyboardSink)
        {
            KeyboardSink = keyboardSink;
        }

        protected override void OnKeyDown(KeyEventArgs args)
        {
            //Debug.WriteLine($"Pressed key: {args.Key}");
            base.OnKeyDown(args);
            KeyboardSink.KeyDown(args);
        }

        protected override void OnKeyUp(KeyEventArgs args)
        {
            //Debug.WriteLine($"Depressed key: {args.Key}");
            base.OnKeyUp(args);
            KeyboardSink.KeyUp(args);
        }
    }
}