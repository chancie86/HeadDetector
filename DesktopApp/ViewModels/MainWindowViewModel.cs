using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DesktopApp;
using DesktopApp.ViewModels;
using FaceDetector.Commands;
using Microsoft.Win32;
using Image = System.Windows.Controls.Image;

namespace FaceDetector.ViewModels
{
    public class MainWindowViewModel
        : BaseViewModel
    {
        private readonly AppConfig _config;

        public MainWindowViewModel(AppConfig config)
        {
            _config = config;
            LoadCommand = new SimpleAsyncCommand(LoadCommandExecute, HandleError);
            AttributesCommand = new SimpleCommand(AttributesCommandExecute, HandleError);
            ProcessCommand = new SimpleAsyncCommand(ProcessCommandExecute, HandleError);
        }

        private string _filePath;
        private ImageViewModel _image;
        private HeadViewModel _head;
        private GaussianViewModel _gaussian;
        private FrequentialMaskViewModel _frequentialMask;

        public ImageViewModel Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AttributesCommand { get; }
        public ICommand ProcessCommand { get; }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public HeadViewModel Head
        {
            get => _head;
            set
            {
                _head = value;
                OnPropertyChanged();
            }
        }

        public GaussianViewModel Gaussian
        {
            get => _gaussian;
            set
            {
                _gaussian = value;
                OnPropertyChanged();
            }
        }

        public FrequentialMaskViewModel FrequentialMask
        {
            get => _frequentialMask;
            set
            {
                _frequentialMask = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadCommandExecute(object parameter)
        {
            Image = null;
            Head = null;
            Gaussian = null;
            FrequentialMask = null;

            LoadImage();
        }

        public void AttributesCommandExecute(object parameter)
        {
            ShowMessage(Head?.ImageAttributes);
        }

        public async Task ProcessCommandExecute(object parameter)
        {
            await RenderHead();
            await RenderGaussian();
            await RenderFrequentialMask();
        }

        private async Task RenderHead()
        {
            Head = new HeadViewModel(_config, FilePath);
            await Head.Detect(Image.Image);
        }

        private async Task RenderGaussian()
        {
            Gaussian = new GaussianViewModel();
            await Gaussian.Run();
        }

        public async Task RenderFrequentialMask()
        {
            var mask = new FrequentialMaskViewModel((BitmapImage) Image.Image.Source);
            await mask.Run();
            FrequentialMask = mask;
        }

        private void LoadImage()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _config.InitialFolder;
            openFileDialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileUri = new Uri(openFileDialog.FileName);
                Image = new ImageViewModel
                {
                    Image = new Image
                    {
                        Source = new BitmapImage(fileUri)
                    }
                };
                FilePath = openFileDialog.FileName;
            }
        }

        private void HandleError(Exception ex)
        {
            ShowMessage(ex.ToString());
        }
    }
}
