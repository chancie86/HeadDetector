using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FaceLibrary;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace DesktopApp.ViewModels
{
    public class HeadViewModel
        : ImageViewModel
    {
        private readonly AppConfig _config;
        private readonly string _filePath;
        private string _imageAttributes;

        public HeadViewModel(AppConfig config, string filePath)
        {
            _config = config;
            _filePath = filePath;
        }

        public async Task Detect(Image originalImage)
        {
            var image = new Image
            {
                Source = originalImage.Source.Clone()
            };

            var credentials = new FaceServiceCredentials(
                _config.SubscriptionKey,
                _config.EndpointUrl);
            var faceService = new FaceService(credentials);

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

            await using var fileStream = File.OpenRead(_filePath);
            var faces = await detector.Detect(fileStream, attributeTypes);

            var result = new StringBuilder();
            for (var i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                result.Append(face.GetAttributeText($"Face id: {i}", faces.Count == 1));
            }

            DrawFaces((BitmapImage)image.Source, faces);

            ImageAttributes = result.ToString();
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

        private void DrawFaces(BitmapImage image, IList<DetectedFace> faces)
        {
            var visual = new DrawingVisual();

            using var drawingContext = visual.RenderOpen();
            drawingContext.DrawImage(image, new Rect(0, 0, image.Width, image.Height));

            for (var i = 0; i < faces.Count; i++)
            {
                var face = faces[i];

                var hRatio = image.Width / image.PixelWidth;
                var vRatio = image.Height / image.PixelHeight;

                var faceRect = new Rect(
                    new Point(face.FaceRectangle.Left * hRatio, face.FaceRectangle.Top * vRatio),
                    new Size(face.FaceRectangle.Width * hRatio, face.FaceRectangle.Height * vRatio));
                var headRect = ResizeRectFromCenter(faceRect, 5.0 / 3, 2);

                DrawRectangle(drawingContext, faceRect, Brushes.Red);
                DrawRectangle(drawingContext, headRect, Brushes.Green);
                DrawText(drawingContext, headRect.TopLeft, image.DpiY / 96, i.ToString());
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

        private void DrawText(DrawingContext context, Point origin, double dip, string text)
        {
            context.DrawText(
                new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    32,
                    Brushes.LawnGreen,
                    dip),
                origin
            );
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
    }
}
