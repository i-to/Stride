using System;
using Stride.Bootstrapper.Test;

namespace Stride.Bootstrapper
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main() => new DrawingVisualizeRoot().Run();
        
        //public static void Main() => new DrawingTestRoot().Run();
    }
}