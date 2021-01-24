using System.Drawing;
using FaceLibrary.ColourMask;

namespace FaceLibrary.Extensions
{
    internal static class BitmapExtensions
    {
        public static double GetGreyPixel(this Bitmap self, int x, int y)
        {
            var color = self.GetPixel(x, y);
            return 0.3 * color.R + 0.59 * color.G + 0.11 * color.B;
        }

        public static YCbCr GetYCbCr(this Bitmap self, int x, int y)
        {
            var colour = self.GetPixel(x, y);

            float fr = (float)colour.R / 255;
            float fg = (float)colour.G / 255;
            float fb = (float)colour.B / 255;

            float yLuma = (float)(0.2989 * fr + 0.5866 * fg + 0.1145 * fb);
            float cb = (float)(-0.1687 * fr - 0.3313 * fg + 0.5000 * fb);
            float cr = (float)(0.5000 * fr - 0.4184 * fg - 0.0816 * fb);

            return new YCbCr(yLuma, cb, cr);
        }
    }
}
