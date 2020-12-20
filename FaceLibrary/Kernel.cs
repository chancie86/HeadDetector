using System;

namespace FaceLibrary
{
    public class Kernel
    {
        private readonly double[,] _data;

        public Kernel(double[,] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));

            if (data.GetLength(0) % 2 == 0
                || data.GetLength(0) != data.GetLength(1))
            {
                throw new ArgumentException("Kernel must be square and have an odd length/width");
            }
        }

        public double this[int index1, int index2] => _data[index1, index2];

        public int Width => _data.GetLength(0);

        public int Height => _data.GetLength(1);
    }
}
