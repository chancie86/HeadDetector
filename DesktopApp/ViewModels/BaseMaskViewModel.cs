using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DesktopApp.Extensions;
using FaceLibrary;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;

namespace DesktopApp.ViewModels
{
    public abstract class BaseMaskViewModel
        : ImageViewModel
    {
        protected IMask Mask { get; set; }
        public MaskResult Result { get; private set; }

        public async Task Run()
        {
            await Task.Run(() =>
            {
                Result = Mask.Run();
            });

            Image = new Image
            {
                Source = DrawMask(Result)
            };
        }

        private WriteableBitmap DrawMask(MaskResult mask)
        {
            var bitmap = new WriteableBitmap(mask.Width, mask.Height, 96, 96, PixelFormats.Bgr32, null);
            DrawImage(bitmap, mask);
            return bitmap;
        }

        protected virtual void DrawImage(WriteableBitmap bitmap, MaskResult mask)
        {
            bitmap.WritePixels((x, y) => mask[x, y] ? Color.White.ToArgb() : Color.Black.ToArgb());
        }

        protected Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using var outStream = new MemoryStream();

            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(outStream);
            var bitmap = new Bitmap(outStream);

            return bitmap;
        }

        protected static void DrawRectangle(WriteableBitmap bitmap, Rect rectangle)
        {
            var visual = new DrawingVisual();

            using var drawingContext = visual.RenderOpen();
            drawingContext.DrawRectangle(null, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 2.0), rectangle);
            drawingContext.Close();

            bitmap
        }
    }
}