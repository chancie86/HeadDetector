using System;
using System.Drawing;
using FaceLibrary.Extensions;

namespace FaceLibrary.Hair
{
    /// <summary>
    /// https://hal.archives-ouvertes.fr/hal-00322719/document
    /// </summary>
    public class FrequentialMask
    {
        private readonly Bitmap _image;
        private int _kernelSize;

        public FrequentialMask(Bitmap image, int kernelSize)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            _kernelSize = kernelSize;
        }

        public int Width => _image.Width;

        public int Height => _image.Height;

        public FrequentialMaskResult Run()
        {
            var kernel = new FrequentialMap(_kernelSize).Run();

            var imageConvolver = new ImageConvolver(_image, kernel);
            var convolvedImage = imageConvolver.Run();

            var mask = new bool[Width, Height];

            var threshold = convolvedImage.Mean - convolvedImage.StandardDeviation;
            convolvedImage.Data.ForEach((x, y, value) => { mask[x, y] = value <= threshold; });

            return new FrequentialMaskResult(mask);
        }
    }
}
