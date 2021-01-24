using System.Drawing;

namespace FaceLibrary.Face
{
    public class Head
    {
        public Head(Rectangle faceArea, string attributes)
        {
            FaceArea = faceArea;
            HeadArea = ResizeRectFromCenter(faceArea, 5.0f / 3, 2);
            Attributes = attributes;
        }

        public Rectangle FaceArea { get; }

        public Rectangle HeadArea { get; }

        public string Attributes { get; }

        private Rectangle ResizeRectFromCenter(Rectangle rect, float scaleX, float scaleY)
        {
            var centerX = (rect.Width / 2) + rect.Left;
            var centerY = (rect.Height / 2) + rect.Top;
            var newWidth = rect.Width * scaleX;
            var newHeight = rect.Height * scaleY;
            var newLeft = centerX - (newWidth / 2);
            var newTop = centerY - (newHeight / 2);

            return new Rectangle(
                (int)newLeft, (int)newTop,
                (int)newWidth, (int)newHeight);
        }
    }
}
