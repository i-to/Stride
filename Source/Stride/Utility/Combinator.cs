namespace Stride.Utility
{
    public static class Combinator
    {
        public static T Identity<T>(T t) => t;

        public static double SumTwoDoubles(double a, double b) => a + b;
    }
}
