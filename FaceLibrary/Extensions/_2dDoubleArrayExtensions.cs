using System;

namespace FaceLibrary.Extensions
{
    public static class _2dDoubleArrayExtensions
    {
        public static void ForEach(this double[,] self, Action<int, int, double> action)
        {
            for (var x = 0; x < self.GetLength(0); x++)
            {
                for (var y = 0; y < self.GetLength(1); y++)
                {
                    action(x, y, self[x, y]);
                }
            }
        }

        public static double Accumulate(this double[,] self, Func<double, double, double> accumulator, double previousResult = 0)
        {
            var result = previousResult;
            self.ForEach((x, y, value) => result = accumulator(self[x, y], result));
            return result;
        }
    }
}
