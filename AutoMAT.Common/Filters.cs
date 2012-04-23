using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AutoMAT.Common
{
    public enum DitherMethod
    {
        None, FilterLite, Sierra2, Burkes
    }

    public class Filters
    {
        public static Bitmap Dither(Bitmap source, PixelFormat format)
        {
            return Dither(source, format, DitherMethod.None);
        }

        public static Bitmap Dither(Bitmap source, PixelFormat format, DitherMethod method)
        {
            switch (method)
            {
                case DitherMethod.FilterLite:
                    return FilterLite(source, format);
                case DitherMethod.Sierra2:
                    return Sierra2(source, format);
                case DitherMethod.Burkes:
                    return Burkes(source, format);
                default:
                    return source;
            }
        }

        public static Bitmap Resize(Bitmap source, int width, int height)
        {
            if (source.Height == height && source.Width == width)
            {
                return source;
            }

            var result = new Bitmap(width, height);
            result.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphic = Graphics.FromImage(result))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var attributes = new ImageAttributes();
                attributes.SetWrapMode(WrapMode.Tile);
                graphic.DrawImage(source, new Rectangle(0, 0, width, height),
                    0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
            }

            return result;
        }

        static Bitmap Burkes(Bitmap source, PixelFormat format)
        {
            var result = new PrecisionBitmap(TileBuffer(source));

            double weight2 = 2 / 32d;
            double weight4 = 4 / 32d;
            double weight8 = 8 / 32d;

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    PrecisionColor oldPixel = result.GetPixel(x, y);
                    PrecisionColor newPixel = ApproximatePixel(oldPixel, format);
                    PrecisionColor error = oldPixel - newPixel;
                    result.SetPixel(x, y, newPixel);

                    int xP1 = GetActualPosition(x + 1, result.Width);
                    int xP2 = GetActualPosition(x + 2, result.Width);
                    int xM1 = GetActualPosition(x - 1, result.Width);
                    int xM2 = GetActualPosition(x - 2, result.Width);
                    int yP1 = GetActualPosition(y + 1, result.Height);

                    result.SetPixel(xP1, y, result.GetPixel(xP1, y) + error * weight8);
                    result.SetPixel(xP2, y, result.GetPixel(xP2, y) + error * weight4);

                    result.SetPixel(xM2, yP1, result.GetPixel(xM2, yP1) + error * weight2);
                    result.SetPixel(xM1, yP1, result.GetPixel(xM1, yP1) + error * weight4);
                    result.SetPixel(x, yP1, result.GetPixel(x, yP1) + error * weight8);
                    result.SetPixel(xP1, yP1, result.GetPixel(xP1, yP1) + error * weight4);
                    result.SetPixel(xP2, yP1, result.GetPixel(xP2, yP1) + error * weight2);
                }
            }

            return result.ToBitmap();
        }

        static Bitmap Sierra3(Bitmap source, PixelFormat format)
        {
            var result = new PrecisionBitmap(TileBuffer(source));

            double weight2 = 2 / 32d;
            double weight3 = 3 / 32d;
            double weight4 = 4 / 32d;
            double weight5 = 5 / 32d;

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    PrecisionColor oldPixel = result.GetPixel(x, y);
                    PrecisionColor newPixel = ApproximatePixel(oldPixel, format);
                    PrecisionColor error = oldPixel - newPixel;
                    result.SetPixel(x, y, newPixel);

                    int xP1 = GetActualPosition(x + 1, result.Width);
                    int xP2 = GetActualPosition(x + 2, result.Width);
                    int xM1 = GetActualPosition(x - 1, result.Width);
                    int xM2 = GetActualPosition(x - 2, result.Width);
                    int yP1 = GetActualPosition(y + 1, result.Height);
                    int yP2 = GetActualPosition(y + 2, result.Height);

                    result.SetPixel(xP1, y, result.GetPixel(xP1, y) + error * weight5);
                    result.SetPixel(xP2, y, result.GetPixel(xP2, y) + error * weight3);

                    result.SetPixel(xM2, yP1, result.GetPixel(xM2, yP1) + error * weight2);
                    result.SetPixel(xM1, yP1, result.GetPixel(xM1, yP1) + error * weight4);
                    result.SetPixel(x, yP1, result.GetPixel(x, yP1) + error * weight5);
                    result.SetPixel(xP1, yP1, result.GetPixel(xP1, yP1) + error * weight4);
                    result.SetPixel(xP2, yP1, result.GetPixel(xP2, yP1) + error * weight2);

                    result.SetPixel(xM1, yP2, result.GetPixel(xM1, yP2) + error * weight2);
                    result.SetPixel(x, yP2, result.GetPixel(x, yP2) + error * weight3);
                    result.SetPixel(xP1, yP2, result.GetPixel(xP1, yP2) + error * weight2);
                }
            }

            return result.ToBitmap();
        }

        static Bitmap Sierra2(Bitmap source, PixelFormat format)
        {
            var result = new PrecisionBitmap(TileBuffer(source));

            double weight1 = 1 / 16d;
            double weight2 = 2 / 16d;
            double weight3 = 3 / 16d;
            double weight4 = 4 / 16d;

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    PrecisionColor oldPixel = result.GetPixel(x, y);
                    PrecisionColor newPixel = ApproximatePixel(oldPixel, format);
                    PrecisionColor error = oldPixel - newPixel;
                    result.SetPixel(x, y, newPixel);

                    int xP1 = GetActualPosition(x + 1, result.Width);
                    int xP2 = GetActualPosition(x + 2, result.Width);
                    int xM1 = GetActualPosition(x - 1, result.Width);
                    int xM2 = GetActualPosition(x - 2, result.Width);
                    int yP1 = GetActualPosition(y + 1, result.Height);

                    result.SetPixel(xP1, y, result.GetPixel(xP1, y) + error * weight4);
                    result.SetPixel(xP2, y, result.GetPixel(xP2, y) + error * weight3);

                    result.SetPixel(xM2, yP1, result.GetPixel(xM2, yP1) + error * weight1);
                    result.SetPixel(xM1, yP1, result.GetPixel(xM1, yP1) + error * weight2);
                    result.SetPixel(x, yP1, result.GetPixel(x, yP1) + error * weight3);
                    result.SetPixel(xP1, yP1, result.GetPixel(xP1, yP1) + error * weight2);
                    result.SetPixel(xP2, yP1, result.GetPixel(xP2, yP1) + error * weight1);
                }
            }

            return result.ToBitmap();
        }

        static Bitmap FilterLite(Bitmap source, PixelFormat format)
        {
            var result = new PrecisionBitmap(TileBuffer(source));

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    PrecisionColor oldPixel = result.GetPixel(x, y);
                    PrecisionColor newPixel = ApproximatePixel(oldPixel, format);
                    PrecisionColor error = oldPixel - newPixel;
                    result.SetPixel(x, y, newPixel);

                    int xP1 = GetActualPosition(x + 1, result.Width);
                    int xM1 = GetActualPosition(x - 1, result.Width);
                    int yP1 = GetActualPosition(y + 1, result.Height);

                    result.SetPixel(xP1, y, result.GetPixel(xP1, y) + error * 0.5);
                    result.SetPixel(xM1, yP1, result.GetPixel(xM1, yP1) + error * 0.25);
                    result.SetPixel(x, yP1, result.GetPixel(x, yP1) + error * 0.25);
                }
            }

            return result.ToBitmap();
        }

        static int GetActualPosition(int pos, int max)
        {
            if (pos < 0)
            {
                return max + pos;
            }
            if (pos >= max)
            {
                return pos - max;
            }
            else
            {
                return pos;
            }
        }

        static PrecisionColor ApproximatePixel(PrecisionColor color, PixelFormat format)
        {
            int rM;
            int gM;
            int bM;

            if (format == PixelFormat.Format16bppRgb565)
            {
                rM = bM = 8;
                gM = 4;
            }
            else if (format == PixelFormat.Format16bppRgb555 ||
                format == PixelFormat.Format16bppArgb1555)
            {
                rM = gM = bM = 8;
            }
            else
            {
                throw new ArgumentOutOfRangeException("format", "Only supported formats are RGB555, RGB565 and ARGB1555.");
            }

            var r = (int)(color.R / rM) * rM;
            var g = (int)(color.G / gM) * gM;
            var b = (int)(color.B / bM) * bM;
            return new PrecisionColor(r, g, b);
        }

        static Bitmap TileBuffer(Bitmap source)
        {
            var result = new Bitmap(source.Width + 10, source.Height + 10);
            result.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphic = Graphics.FromImage(result))
            {
                var attributes = new ImageAttributes();
                attributes.SetWrapMode(WrapMode.Tile);
                graphic.DrawImage(source, new Rectangle(0, 0, result.Width, result.Height),
                    -5, -5, result.Width, result.Height, GraphicsUnit.Pixel, attributes);
            }

            return result;
        }

        static Bitmap TileDebuffer(Bitmap source)
        {
            var result = new Bitmap(source.Width - 10, source.Height - 10);
            result.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphic = Graphics.FromImage(result))
            {
                var attributes = new ImageAttributes();
                attributes.SetWrapMode(WrapMode.Tile);
                graphic.DrawImage(source, new Rectangle(0, 0, result.Width, result.Height),
                    5, 5, result.Width, result.Height, GraphicsUnit.Pixel, attributes);
            }

            return result;
        }
    }
}
