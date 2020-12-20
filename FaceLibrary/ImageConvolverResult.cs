using System;
using FaceLibrary.Extensions;

namespace FaceLibrary
{
    public class ImageConvolverResult
    {
        private double[,] _data;

        public ImageConvolverResult(double[,] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            CalculateStats();
        }

        public double[,] Data => _data;

        public double this[int index1, int index2] => _data[index1, index2];

        public int Width => _data.GetLength(0);

        public int Height => _data.GetLength(1);

        public double Count => Width * Height;

        public double Mean { get; private set; }

        public double StandardDeviation { get; private set; }

        private void CalculateStats()
        {
            var sum = _data.Accumulate((next, accumulator) => accumulator + next);
            Mean = sum / Count;
            StandardDeviation = Math.Sqrt(_data.Accumulate((next, accumulator) => accumulator + Math.Pow(next - Mean, 2)) / Count);
        }
    }
}
