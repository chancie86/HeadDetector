using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DesktopApp.Extensions;
using FaceDetector.ViewModels;
using FaceLibrary;
using FaceLibrary.Hair;

namespace DesktopApp.ViewModels
{
    public class GaussianWindowViewModel
        : BaseViewModel
    {
        private const int KernelSize = 201;
        private Image _image;

        public GaussianWindowViewModel()
        {
            var kernel = new FrequentialMap(KernelSize).Run();
            kernel = Normalise(kernel);

            Image = new Image
            {
                Source = DrawKernel(kernel)
            };
        }

        public Image Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        private WriteableBitmap DrawKernel(Kernel kernel)
        {
            var bitmap = new WriteableBitmap(KernelSize, KernelSize, 96, 96, PixelFormats.Bgr32, null);

            bitmap.WritePixels((x, y) =>
            {
                var data = (int)kernel[x, y];
                var color_data = data << 16; // R
                color_data |= data << 8; // G
                color_data |= data << 0; // B

                return color_data;
            });

            return bitmap;
        }

        private Kernel Normalise(Kernel kernel)
        {
            double max = 0;

            for (var x = 0; x < kernel.Width; x++)
            {
                for (var y = 0; y < kernel.Height; y++)
                {
                    max = kernel[x, y] < max ? max : kernel[x, y];
                }
            }

            var normalisedKernelData = new double[kernel.Width, kernel.Height];

            for (var x = 0; x < kernel.Width; x++)
            {
                for (var y = 0; y < kernel.Height; y++)
                {
                    normalisedKernelData[x, y] = (kernel[x, y] / max) * 255;
                }
            }

            return new Kernel(normalisedKernelData);
        }
    }
}
