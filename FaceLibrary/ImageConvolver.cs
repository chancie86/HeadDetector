using System;
using System.Drawing;
using FaceLibrary.Extensions;

namespace FaceLibrary
{
    public class ImageConvolver
    {
        private readonly Bitmap _bitmap;
        private readonly Kernel _kernel;

        public ImageConvolver(Bitmap bitmap, Kernel kernel)
        {
            _bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        public ImageConvolverResult Run()
        {
            var workingData = new double[_bitmap.Width, _bitmap.Height];

            var minX = _kernel.Width / 2;
            var maxX = _bitmap.Width - (_kernel.Width / 2);
            var minY = _kernel.Height / 2;
            var maxY = _bitmap.Height - (_kernel.Height / 2);

            for (var x = minX; x < maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    workingData[x, y] = Convolve(x, y);
                }
            }

            return new ImageConvolverResult(workingData);
        }

        private double Convolve(int centerX, int centerY)
        {
            var halfKernelLength = _kernel.Width / 2;
            var sum = 0.0;

            for (var kernelX = -halfKernelLength; kernelX < _kernel.Width - halfKernelLength; kernelX++)
            {
                for (var kernelY = -halfKernelLength; kernelY < _kernel.Height - halfKernelLength; kernelY++)
                {
                    var imageX = centerX + kernelX;
                    var imageY = centerY + kernelY;
                    var pixelValue = _bitmap.GetGreyPixel(imageX, imageY);
                    sum +=
                        pixelValue * _kernel[kernelX + halfKernelLength, kernelY + halfKernelLength];
                }
            }

            return sum;
        }
    }
}
