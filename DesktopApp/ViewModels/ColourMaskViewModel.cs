using System.Drawing;
using System.Windows.Media.Imaging;
using FaceLibrary;
using FaceLibrary.ColourMask;
using FaceLibrary.Face;

namespace DesktopApp.ViewModels
{
    public class ColourMaskViewModel
        : BaseMaskViewModel
    {
        private readonly Rectangle _sampleWindow;

        public ColourMaskViewModel(BitmapImage bitmap, Head head, MaskResult frequentialMask)

        {
            _sampleWindow = new Rectangle(
                head.FaceArea.Left,
                head.HeadArea.Top,
                head.FaceArea.Width,
                head.FaceArea.Top - head.HeadArea.Top
            );

            Mask = new ColourMask(BitmapImageToBitmap(bitmap), _sampleWindow, frequentialMask);
        }

        protected override void DrawImage(WriteableBitmap bitmap, MaskResult mask)
        {
            base.DrawImage(bitmap, mask);

            _sampleWindow
        }
    }
}
