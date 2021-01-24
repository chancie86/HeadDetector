using System;
using System.Drawing;

namespace FaceLibrary.Extensions
{
    public static class _2dArrayExtensions
    {
        public static void ForEach<T>(this T[,] self, Action<int, int, T> action, Rectangle? sampleWindow = null)
            where T : notnull
        {
            int minX, maxX, minY, maxY;

            if (sampleWindow.HasValue)
            {
                var window = sampleWindow.Value;
                if (window.Left < 0
                    || window.Right > self.GetLength(0)
                    || window.Top < 0
                    || window.Bottom > self.GetLength(1))
                {
                    throw new ArgumentException($"{nameof(window)} values are out of bounds");
                }

                minX = window.Left;
                maxX = window.Right;
                minY = window.Top;
                maxY = window.Bottom;
            }
            else
            {
                minX = minY = 0;
                maxX = self.GetLength(0);
                maxY = self.GetLength(1);
            }

            for (var x = minX; x < maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    action(x, y, self[x, y]);
                }
            }
        }

        public static T Accumulate<T>(this T[,] self, Func<T, T, T> accumulator, T previousResult = default)
            where T : notnull
        {
            var result = previousResult;
            self.ForEach((x, y, value) => result = accumulator(self[x, y], result));
            return result;
        }
    }
}
