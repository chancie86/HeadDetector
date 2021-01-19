using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DesktopApp.Extensions;
using FaceDetector.ViewModels;
using FaceLibrary.Hair;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;

namespace DesktopApp.ViewModels
{
    public class FrequentialMaskViewModel
        : ImageViewModel
    {
        private FrequentialMask _mask;

        public FrequentialMaskViewModel(BitmapImage bitmap)
        {
            _mask = new FrequentialMask(BitmapImageToBitmap(bitmap), 7);
        }

        public async Task Run()
        {
            FrequentialMaskResult result = null;

            await Task.Run(() =>
            {
                result = _mask.Run();
            });

            Image = new Image
            {
                Source = DrawMask(result)
            };
        }

        private WriteableBitmap DrawMask(FrequentialMaskResult mask)
        {
            var bitmap = new WriteableBitmap(mask.Width, mask.Height, 96, 96, PixelFormats.Bgr32, null);

            bitmap.WritePixels((x, y) => mask[x, y] ? Color.White.ToArgb() : Color.Black.ToArgb());

            return bitmap;
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using var outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            var bitmap = new Bitmap(outStream);

            return bitmap;
        }

    }
}
