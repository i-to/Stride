using System;

namespace Stride
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main() => new Root().Run();
    }
}