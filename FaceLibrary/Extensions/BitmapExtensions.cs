using System.Drawing;

namespace FaceLibrary.Extensions
{
    internal static class BitmapExtensions
    {
        public static double GetGreyPixel(this Bitmap self, int x, int y)
        {
            var color = self.GetPixel(x, y);
            return 0.3 * color.R + 0.59 * color.G + 0.11 * color.B;
        }
    }
}
