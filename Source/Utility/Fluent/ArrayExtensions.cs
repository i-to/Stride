namespace Stride.Utility.Fluent
{
    public static class ArrayExtensions
    {
        public static int Width<T>(this T[,] array) => array.GetLength(0);
        public static int Height<T>(this T[,] array) => array.GetLength(1);

        public static T[] UnrollRows<T>(this T[,] array)
        {
            var width = array.Width();
            var height = array.Height();
            var result = new T[width * height];
            for (int col = 0; col != width; ++col)
            for (int row = 0; row != height; ++row)
                result[row * width + col] = array[col, row];
            return result;
        }
    }
}