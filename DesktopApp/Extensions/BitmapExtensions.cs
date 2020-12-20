using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DesktopApp.Extensions
{
    public static class BitmapExtensions
    {
        public static void WritePixels(this WriteableBitmap self, Func<int, int, int> writePixel)
        {
            try
            {
                // Reserve the back buffer for updates.
                self.Lock();

                unsafe
                {
                    for (var x = 0; x < self.Width; x++)
                    {
                        for (var y = 0; y < self.Height; y++)
                        {
                            // Get a pointer to the back buffer.
                            IntPtr pBackBuffer = self.BackBuffer;

                            // Find the address of the pixel to draw.
                            pBackBuffer += y * self.BackBufferStride;
                            pBackBuffer += x * 4;

                            // Assign the color data to the pixel.
                            *((int*)pBackBuffer) = writePixel(x, y);

                            // Specify the area of the bitmap that changed.
                            self.AddDirtyRect(new Int32Rect(x, y, 1, 1));
                        }
                    }
                }
            }
            finally
            {
                // Release the back buffer and make it available for display.
                self.Unlock();
            }
        }
    }
}
