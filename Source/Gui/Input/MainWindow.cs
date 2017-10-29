using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NAudio.Midi;

namespace Stride.Gui.Input
{
    public class MainWindow : Window, IDisposable
    {
        readonly KeyboardSink KeyboardSink;
        readonly MidiSink MidiSink;

        MidiIn MidiIn;

        public MainWindow(KeyboardSink keyboardSink, MidiSink midiSink)
        {
            KeyboardSink = keyboardSink;
            MidiSink = midiSink;
            InitMidi();
        }

        void OnMidiMessageAsync(object sender, MidiInMessageEventArgs args)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new DispatcherOperationCallback(_ => DispatchMidiEvent(args)),
                null);
        }

        object DispatchMidiEvent(MidiInMessageEventArgs args)
        {
            MidiSink.MidiEvent(args.MidiEvent);
            return null;
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

        public bool Disposed => MidiIn == null;

        void InitMidi()
        {
            MidiIn = new MidiIn(0);
            MidiIn.Start();
            MidiIn.MessageReceived += OnMidiMessageAsync;
        }

        public void Dispose()
        {
            if (Disposed)
                return;
            MidiIn.Stop();
            MidiIn.Dispose();
            MidiIn = null;
        }
    }
}