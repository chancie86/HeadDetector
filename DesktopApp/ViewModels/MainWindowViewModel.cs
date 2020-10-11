using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using DesktopApp;
using FaceDetector.Commands;
using FaceLibrary;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Win32;

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
        }

        private Image _image;
        private string _filePath;
        private string _imageAttributes;

        public Image Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public string ImageAttributes
        {
            get => _imageAttributes;
            set
            {
                _imageAttributes = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadCommandExecute(object parameter)
        {
            var creds = new FaceServiceCredentials(
                _config.SubscriptionKey,
                _config.EndpointUrl);
            var faceService = new FaceService(creds);

            LoadImage();
            await Detect(faceService);
        }

        private void LoadImage()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _config.InitialFolder;
            openFileDialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileUri = new Uri(openFileDialog.FileName);
                Image = new Image
                {
                    Source = new BitmapImage(fileUri)
                };
                FilePath = openFileDialog.FileName;
            }
        }

        private async Task Detect(FaceService faceService)
        {
            faceService.Authenticate();
            var detector = faceService.GetFaceDetector();

            var attributeTypes = new List<FaceAttributeType?>
            {
                FaceAttributeType.Accessories, FaceAttributeType.Age,
                FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure,
                FaceAttributeType.FacialHair,
                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile
            };

            await using var fileStream = File.OpenRead(FilePath);
            var faces = await detector.Detect(fileStream, attributeTypes);

            var result = new StringBuilder();
            foreach (var face in faces)
            {
                result.Append(face.GetAttributeText());
            }

            DrawFaces((BitmapImage)Image.Source, faces);

            ImageAttributes = result.ToString();
        }

        private void DrawFaces(BitmapImage image, IList<DetectedFace> faces)
        {
            var visual = new DrawingVisual();

            using var drawingContext = visual.RenderOpen();
            drawingContext.DrawImage(image, new Rect(0, 0, image.Width, image.Height));

            foreach (var face in faces)
            {
                var hRatio = image.Width / image.PixelWidth;
                var vRatio = image.Height / image.PixelHeight;

                var faceRect = new Rect(
                    new Point(face.FaceRectangle.Left * hRatio, face.FaceRectangle.Top * vRatio),
                    new Size(face.FaceRectangle.Width * hRatio, face.FaceRectangle.Height * vRatio));
                var headRect = ResizeRectFromCenter(faceRect, 5.0 / 3, 2);

                DrawRectangle(drawingContext, faceRect, Brushes.Red);
                DrawRectangle(drawingContext, headRect, Brushes.Green);
            }

            drawingContext.Close();

            var target = new RenderTargetBitmap(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, PixelFormats.Pbgra32);
            target.Render(visual);
            Image = new Image
            {
                Source = target
            };
        }

        private void DrawRectangle(DrawingContext context, Rect rectangle, Brush brush)
        {
            context.DrawRectangle(null, new Pen(brush, 2.0), rectangle);
        }

        private Rect ResizeRectFromCenter(Rect rect, double scaleX, double scaleY)
        {
            var centerX = (rect.Width / 2) + rect.Left;
            var centerY = (rect.Height / 2) + rect.Top;
            var newWidth = rect.Width * scaleX;
            var newHeight = rect.Height * scaleY;
            var newLeft = centerX - (newWidth / 2);
            var newTop = centerY - (newHeight / 2);

            return new Rect(
                new Point(newLeft, newTop),
                new Size(newWidth, newHeight));
        }

        private void HandleError(Exception ex)
        {
            ShowMessage(ex.ToString());
        }
    }
}
