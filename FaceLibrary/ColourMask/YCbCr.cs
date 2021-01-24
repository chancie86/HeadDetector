namespace FaceLibrary.ColourMask
{
    public struct YCbCr
    {
        public YCbCr(float y, float cb, float cr)
        {
            Y = y;
            Cb = cb;
            Cr = cr;
        }

        /// <summary>
        /// Luma
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Blue difference chroma
        /// </summary>
        public float Cb { get; }

        /// <summary>
        /// Red difference chroma
        /// </summary>
        public float Cr { get; }

        public static YCbCr operator +(YCbCr a, YCbCr b)
        {
            return new YCbCr(a.Y + b.Y, a.Cb + b.Cb, a.Cr + b.Cr);
        }

        public static YCbCr operator -(YCbCr a, YCbCr b)
        {
            return new YCbCr(a.Y - b.Y, a.Cb - b.Cb, a.Cr - b.Cr);
        }
    }
}
