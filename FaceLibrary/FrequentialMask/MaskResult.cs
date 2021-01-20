using System;

namespace FaceLibrary.FrequentialMask
{
    public class MaskResult
    {
        private bool[,] _data;

        public MaskResult(bool[,] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public int Width => _data.GetLength(0);

        public int Height => _data.GetLength(1);

        public bool this[int index1, int index2] => _data[index1, index2];
    }
}
