using System;

namespace Stride.Bootstrapper
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main() => new Root().Run();
    }
}