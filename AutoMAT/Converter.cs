using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoMAT
{
    public static class Converter
    {
        public static byte[] ToMat16(Bitmap source, PixelFormat format, int numMipmaps)
        {
            if (format != PixelFormat.Format16bppRgb565 &&
                format != PixelFormat.Format16bppArgb1555)
            {
                throw new ArgumentOutOfRangeException("format", "Only supported formats are RGB565 and ARGB1555.");
            }

            bool isAlpha = format == PixelFormat.Format16bppArgb1555;

            var header = new MatHeader
            {
                Magic = new byte[] { (byte)'M', (byte)'A', (byte)'T', (byte)' ' },
                Version = 0x32,
                Type = 2,
                MatRecordCount = 1,
                TextureCount = 1,
                Transparency = isAlpha ? 1 : 0,
                Bitdepth = 16,
                // Leave these all 0, the spec isn't solid and this is what everything else tends to do.
                // The pixel format should be inferred as RGB565 or ARGB1555 from the transparency byte anyway.
                BlueBits = 0,
                GreenBits = 0,
                RedBits = 0,
                RedShl = 0,
                GreenShl = 0,
                BlueShl = 0,
                RedShr = 0,
                GreenShr = 0,
                BlueShr = 0
            };

            var recordHeader = new MatRecordHeader
            {
                RecordType = 0x8,
                Unknown4 = 1065353216,
                Unknown6 = 4,
                Unknown7 = 4
            };

            var dataHeader = new TextureDataHeader
            {
                SizeX = source.Width,
                SizeY = source.Height,
                MipmapCount = numMipmaps
            };

            using (var stream = new MemoryStream())
            {
                // MAT header
                stream.WriteBytes(RawSerializer.Serialize(header));

                // MAT record header
                stream.WriteBytes(RawSerializer.Serialize(recordHeader));

                // Texture data header
                stream.WriteBytes(RawSerializer.Serialize(dataHeader));

                // Texture data
                for (int i = 0; i < numMipmaps; i++)
                {
                    int height = source.Height >> i;
                    int width = source.Width >> i;
                    stream.WriteBytes(GetBitmapData(Filters.Dither(Filters.Resize(source, width, height), format), format));
                }

                return stream.ToArray();
            }
        }

        static byte[] GetBitmapData(Bitmap source, PixelFormat format)
        {
            var bitmapData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            IntPtr ptr = bitmapData.Scan0;
            int stride = bitmapData.Stride;
            int numBytes = source.Width * source.Height * 3;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(ptr, rgbData, 0, numBytes);
            source.UnlockBits(bitmapData);

            using (var stream = new MemoryStream(source.Width * source.Height))
            {
                for (int i = 0; i < rgbData.Length; i += 3)
                {
                    ushort pixel = ConvertPixel(rgbData[i + 2], rgbData[i + 1], rgbData[i], format);
                    stream.Write(BitConverter.GetBytes(pixel), 0, 2);
                }
                return stream.ToArray();
            }
        }
        
        static ushort ConvertPixel(byte r, byte g, byte b, PixelFormat format)
        {
            if (format == PixelFormat.Format16bppRgb565)
            {
                return
                    (ushort)(
                    ((ushort)(r / 255.0 * 31.0 + 0.5) << 11) |
                    ((ushort)(g / 255.0 * 63.0 + 0.5) << 5) |
                    ((ushort)(b / 255.0 * 31.0 + 0.5))
                    );
            }
            else if (format == PixelFormat.Format16bppArgb1555)
            {
                // Return transparent bit
                if (r == 0 && g == 0 && b == 0)
                {
                    return 0;
                }
                return
                    (ushort)(
                    ((ushort)0x8000) |
                    ((ushort)(r / 255.0 * 31.0 + 0.5) << 10) |
                    ((ushort)(g / 255.0 * 31.0 + 0.5) << 5) |
                    ((ushort)(b / 255.0 * 31.0 + 0.5))
                    );
            }
            else
            {
                throw new ArgumentException("format", "Pixel format must be RGB565 or ARGB1555");
            }
        }
    }
}