using System.Windows.Controls;
using FaceDetector.ViewModels;

namespace DesktopApp.ViewModels
{
    public class ImageViewModel
        : BaseViewModel
    {
        private Image _image;

        public Image Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
    }
}
