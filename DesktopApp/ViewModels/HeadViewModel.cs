using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FaceLibrary;
using FaceLibrary.Face;
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

            var filter = new Filter(new Config
            {
                EndpointUrl = _config.EndpointUrl,
                SubscriptionKey = _config.SubscriptionKey
            }, _filePath);

            var faces = await filter.Run();

            DrawFaces((BitmapImage)image.Source, faces);

            ImageAttributes = string.Join(',', faces.Select(x => x.Attributes));
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

        private void DrawFaces(BitmapImage image, IList<Head> heads)
        {
            var visual = new DrawingVisual();

            using var drawingContext = visual.RenderOpen();
            drawingContext.DrawImage(image, new Rect(0, 0, image.Width, image.Height));

            for (var i = 0; i < heads.Count; i++)
            {
                var head = heads[i];

                var hRatio = image.Width / image.PixelWidth;
                var vRatio = image.Height / image.PixelHeight;

                var faceRect = new Rect(
                    new Point(head.FaceArea.Left * hRatio, head.FaceArea.Top * vRatio),
                    new Size(head.FaceArea.Width * hRatio, head.FaceArea.Height * vRatio));
                var headRect = new Rect(
                    new Point(head.HeadArea.Left * hRatio, head.HeadArea.Top * vRatio),
                    new Size(head.HeadArea.Width * hRatio, head.HeadArea.Height * vRatio));

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
    }
}
