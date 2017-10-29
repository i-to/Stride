using System;

namespace Stride.Gui
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main() => new Root().Run();
    }
}