using System;

namespace Stride.Utility.Fluent
{
    public static class DateTimeExtensions
    {
        public static string ToStringForFileName(this DateTime dateTime)
            => DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
    }
}
