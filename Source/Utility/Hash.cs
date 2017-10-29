namespace Stride.Utility
{
    public static class Hash
    {
        public static int Compute(int first, int second)
        {
            unchecked
            {
                int hash = 17;
                hash = hash + 23 * first;
                hash = hash + 23 * second;
                return hash;
            }
        }

        public static int Compute<T, S>(T first, S second) 
            where T : class
            where S : class => 
            Compute(first.GetHashCode(), second.GetHashCode());
    }
}