using System;
using System.Collections.Generic;
using System.Drawing;
using FaceLibrary.Extensions;

namespace FaceLibrary.ColourMask
{
    public class ColourMask
        : IMask
    {
        private readonly Bitmap _image;
        private readonly MaskResult _frequentialMaskResult;
        private readonly Rectangle _sampleWindow;

        public ColourMask(Bitmap image, Rectangle sampleWindow, MaskResult frequentialMaskResult)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            _frequentialMaskResult = frequentialMaskResult ?? throw new ArgumentNullException(nameof(frequentialMaskResult));
            _sampleWindow = sampleWindow;
        }

        public int Width => _image.Width;

        public int Height => _image.Height;

        public MaskResult Run()
        {
            var meanSd = GetMeanAndSd();
            var average = meanSd.average;
            var standardDeviation = meanSd.standardDeviation;
            var min = average - standardDeviation;
            var max = average + standardDeviation;

            var mask = new bool[Width, Height];
            
            mask.ForEach((x, y, val) =>
            {
                var pixel = _image.GetYCbCr(x, y);
                mask[x, y] = Filter(pixel, min, max);
            });

            return new MaskResult(mask);
        }

        private (YCbCr average, YCbCr standardDeviation) GetMeanAndSd()
        {
            var count = 0;
            var luma = 0.0;
            var cb = 0.0;
            var cr = 0.0;

            var sdSumFuncs = new List<Func<YCbCr, (double Y, double Cb, double Cr)>>(_sampleWindow.Width * _sampleWindow.Height);

            _frequentialMaskResult.ForEach((x, y, val) =>
            {
                if (!val)
                {
                    return;
                }

                var pixel = _image.GetYCbCr(x, y);
                luma += pixel.Y;
                cb += pixel.Cb;
                cr = pixel.Cr;
                count++;

                sdSumFuncs.Add(average => (
                    Math.Pow(pixel.Y - average.Y, 2),
                    Math.Pow(pixel.Cb - average.Cb, 2),
                    Math.Pow(pixel.Cr - average.Cr, 2)
                ));
            },
            _sampleWindow);

            var average = new YCbCr(
                (float)luma / count,
                (float)cb / count,
                (float)cr / count);
            
            luma = cb = cr = 0.0;

            foreach (var func in sdSumFuncs)
            {
                var val = func(average);
                luma += val.Y;
                cb += val.Cb;
                cr += val.Cr;
            }

            var sd = new YCbCr(
                (float)Math.Sqrt(luma / sdSumFuncs.Count),
                (float)Math.Sqrt(cb / sdSumFuncs.Count),
                (float)Math.Sqrt(cr / sdSumFuncs.Count)
                );

            return (average, sd);
        }

        private bool Filter(YCbCr value, YCbCr min, YCbCr max)
        {
            return value.Y > min.Y
                    && value.Cb > min.Cb
                    && value.Cb > min.Cr
                    && value.Y < max.Y
                    && value.Cb < max.Cb
                    && value.Cr < max.Cr;
        }
    }
}
