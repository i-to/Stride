using System;

namespace Stride.Utility
{
    public static class ObjectExtensions
    {
        public static T Cast<T>(this object obj) => (T) obj;
    }

    public static class ClassObjectExtensions
    { 
        public static void IfNotNull<T>(this T obj, Action<T> action) where T : class 
        {
            if (!(obj is null))
                action(obj);
        }
    }
}
