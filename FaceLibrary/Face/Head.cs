using System.Drawing;

namespace FaceLibrary.Face
{
    public class Head
    {
        public Head(RectangleF faceArea, string attributes)
        {
            FaceArea = faceArea;
            HeadArea = ResizeRectFromCenter(faceArea, 5.0f / 3, 2);
            Attributes = attributes;
        }

        public RectangleF FaceArea { get; }

        public RectangleF HeadArea { get; }

        public string Attributes { get; }

        private RectangleF ResizeRectFromCenter(RectangleF rect, float scaleX, float scaleY)
        {
            var centerX = (rect.Width / 2) + rect.Left;
            var centerY = (rect.Height / 2) + rect.Top;
            var newWidth = rect.Width * scaleX;
            var newHeight = rect.Height * scaleY;
            var newLeft = centerX - (newWidth / 2);
            var newTop = centerY - (newHeight / 2);

            return new RectangleF(
                newLeft, newTop,
                newWidth, newHeight);
        }
    }
}
