﻿using System;

namespace Stride.Utility.Fluent
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
