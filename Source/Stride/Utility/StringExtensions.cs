﻿using System.Collections.Generic;

namespace Stride.Utility
{
    public static class StringExtensions
    {
        public static string ConcatSpaceSeparated(this IEnumerable<string> strings) =>
            string.Join(" ", strings);
    }
}
