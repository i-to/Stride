using System;

namespace Stride.Bootstrapper
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
            => new DrawingTestRoot().Run(goldenDataGenerationMode: false);
            //=> new DrawingVisualizeRoot().Run();
    }
}