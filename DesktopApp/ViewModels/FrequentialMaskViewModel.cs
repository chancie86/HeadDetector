using System.Windows.Media.Imaging;
using FaceLibrary.FrequentialMask;

namespace DesktopApp.ViewModels
{
    public class FrequentialMaskViewModel
        : BaseMaskViewModel
    {
        public FrequentialMaskViewModel(BitmapImage bitmap)
        {
            Mask = new FrequentialMask(BitmapImageToBitmap(bitmap), 9);
        }
    }
}
