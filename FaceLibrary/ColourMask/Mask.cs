using System;
using System.Drawing;
using FaceLibrary.Extensions;

namespace FaceLibrary.ColourMask
{
    public class Mask
    {
        private readonly Bitmap _image;
        private readonly MaskResult _frequentialMaskResult;
        private readonly Rectangle _window;

        public Mask(Bitmap image, Rectangle window, MaskResult frequentialMaskResult)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            _frequentialMaskResult = frequentialMaskResult ?? throw new ArgumentNullException(nameof(frequentialMaskResult));
            _window = window;
        }

        public int Width => _image.Width;

        public int Height => _image.Height;

        public MaskResult Run()
        {
            return null;
        }
    }
}
