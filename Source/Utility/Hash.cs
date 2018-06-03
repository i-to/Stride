namespace Stride.Utility
{
    public static class Hash
    {
        public static int Compute(int[] ints)
        {
            unchecked
            {
                int hash = 17;
                foreach (var val in ints)
                    hash = hash + 23 * val;
                return hash;
            }
        }

        public static int Compute(int first, int second)
            => Compute(new[] {first, second});

        public static int Compute(int first, int second, int third)
            => Compute(new[] {first, second, third});

        public static int Compute(int first, int second, int third, int fourths)
            => Compute(new[] {first, second, third, fourths});

        public static int Compute<T, S>(T first, S second) 
            where T : class
            where S : class => 
            Compute(first.GetHashCode(), second.GetHashCode());
    }
}