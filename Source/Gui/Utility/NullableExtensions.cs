using System;

namespace Stride.Gui.Utility
{
    public static class NullableExtensions
    {
        public static void ForValue<T>(this T? nullable, Action<T> action) where T: struct
        {
            if (nullable.HasValue)
                action.Invoke(nullable.Value);
        }
    }
}
