using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoMAT.Common
{
    public static class Converter
    {
        static readonly ushort[] fiveBitSpace = new ushort[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9, 10, 10, 10, 10, 10, 10, 10, 10, 11, 11, 11, 11, 11, 11, 11, 11, 12, 12, 12, 12, 12, 12, 12, 12, 13, 13, 13, 13, 13, 13, 13, 13, 13, 14, 14, 14, 14, 14, 14, 14, 14, 15, 15, 15, 15, 15, 15, 15, 15, 16, 16, 16, 16, 16, 16, 16, 16, 17, 17, 17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 19, 19, 19, 19, 19, 19, 19, 19, 20, 20, 20, 20, 20, 20, 20, 20, 21, 21, 21, 21, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22, 22, 22, 22, 23, 23, 23, 23, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24, 25, 25, 25, 25, 25, 25, 25, 25, 26, 26, 26, 26, 26, 26, 26, 26, 27, 27, 27, 27, 27, 27, 27, 27, 27, 28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29, 30, 30, 30, 30, 30, 30, 30, 30, 31, 31, 31, 31, 31 };
        static readonly ushort[] sixBitSpace = new ushort[] { 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10, 10, 11, 11, 11, 11, 12, 12, 12, 12, 13, 13, 13, 13, 14, 14, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 17, 17, 17, 17, 18, 18, 18, 18, 19, 19, 19, 19, 20, 20, 20, 20, 21, 21, 21, 21, 21, 22, 22, 22, 22, 23, 23, 23, 23, 24, 24, 24, 24, 25, 25, 25, 25, 26, 26, 26, 26, 27, 27, 27, 27, 28, 28, 28, 28, 29, 29, 29, 29, 30, 30, 30, 30, 31, 31, 31, 31, 32, 32, 32, 32, 33, 33, 33, 33, 34, 34, 34, 34, 35, 35, 35, 35, 36, 36, 36, 36, 37, 37, 37, 37, 38, 38, 38, 38, 39, 39, 39, 39, 40, 40, 40, 40, 41, 41, 41, 41, 42, 42, 42, 42, 42, 43, 43, 43, 43, 44, 44, 44, 44, 45, 45, 45, 45, 46, 46, 46, 46, 47, 47, 47, 47, 48, 48, 48, 48, 49, 49, 49, 49, 50, 50, 50, 50, 51, 51, 51, 51, 52, 52, 52, 52, 53, 53, 53, 53, 54, 54, 54, 54, 55, 55, 55, 55, 56, 56, 56, 56, 57, 57, 57, 57, 58, 58, 58, 58, 59, 59, 59, 59, 60, 60, 60, 60, 61, 61, 61, 61, 62, 62, 62, 62, 63, 63, 63 };

        public static Task ConvertAsync(ConversionOptions options, FileInfo outputFile, params FileInfo[] inputFiles)
        {
            return Task.Factory.StartNew(() => Convert(options, outputFile, inputFiles));
        }

        public static void Convert(ConversionOptions options, FileInfo outputFile, params FileInfo[] inputFiles)
        {
            IEnumerable<Bitmap> sources = null;
            IEnumerable<Bitmap> dithered = null;
            try
            {
                sources = inputFiles.Select(f => DevIL.DevIL.LoadBitmap(f.FullName));

                int numMipmaps;
                PixelFormat format;
                DitherMethod ditherMethod;

                if (options.Transparency)
                {
                    numMipmaps = 1;
                    format = PixelFormat.Format16bppArgb1555;
                    ditherMethod = DitherMethod.None;
                }
                else
                {
                    bool large = sources.Any(b => b.Width > 256 || b.Height > 256);

                    // Only generate mipmaps if the image isn't larger than 256 or if LargeMipmaps
                    // option is enabled.
                    //
                    // Add 1 because mipmaps includes original image
                    numMipmaps = !large || options.ForceMipmaps ? options.NumMipmaps + 1 : 1;
                    format = PixelFormat.Format16bppRgb565;
                    ditherMethod = options.Dither ? DitherMethod.Burkes : DitherMethod.None;
                }

                dithered = sources.Select(b => Filters.Dither(b, format, ditherMethod));
                byte[] result = Converter.ToMat16(format, numMipmaps, dithered.ToArray());

                File.WriteAllBytes(outputFile.FullName, result);
            }
            finally
            {
                if (sources != null)
                {
                    foreach (Bitmap bitmap in sources)
                    {
                        if (bitmap != null)
                        {
                            bitmap.Dispose();
                        }
                    }
                }
                if (dithered != null)
                {
                    foreach (Bitmap bitmap in dithered)
                    {
                        if (bitmap != null)
                        {
                            bitmap.Dispose();
                        }
                    }
                }
            }
        }

        public static byte[] ToMat16(PixelFormat format, int numMipmaps, params Bitmap[] sources)
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
                MatRecordCount = sources.Length,
                TextureCount = sources.Length,
                Transparency = isAlpha ? 1 : 0,
                Bitdepth = 16,
                BlueBits = 5,
                GreenBits = isAlpha ? 5 : 6,
                RedBits = 5,
                RedShl = 0,
                GreenShl = 0,
                BlueShl = 0,
                RedShr = 0,
                GreenShr = 0,
                BlueShr = 0
            };

            using (var stream = new MemoryStream())
            {
                // MAT header
                stream.WriteBytes(RawSerializer.Serialize(header));

                foreach (Bitmap source in sources)
                {
                    var recordHeader = new MatRecordHeader
                    {
                        RecordType = 0x8,
                        Unknown4 = 1065353216,
                        Unknown6 = 4,
                        Unknown7 = 4
                    };
                    // MAT record header
                    stream.WriteBytes(RawSerializer.Serialize(recordHeader));
                }

                foreach (Bitmap source in sources)
                {
                    var dataHeader = new TextureDataHeader
                    {
                        SizeX = source.Width,
                        SizeY = source.Height,
                        MipmapCount = numMipmaps
                    };

                    // Texture data header
                    stream.WriteBytes(RawSerializer.Serialize(dataHeader));

                    // Texture data
                    for (int i = 0; i < numMipmaps; i++)
                    {
                        int height = source.Height >> i;
                        int width = source.Width >> i;
                        stream.WriteBytes(GetBitmapData(Filters.Dither(Filters.Resize(source, width, height), format), format));
                    }
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
            byte[] rgb = new byte[numBytes];
            Marshal.Copy(ptr, rgb, 0, numBytes);
            source.UnlockBits(bitmapData);

            using (var stream = new MemoryStream(source.Width * source.Height))
            {
                if (format == PixelFormat.Format16bppRgb565)
                {
                    for (int i = 0; i < rgb.Length; i += 3)
                    {
                        ushort pixel = (ushort)((fiveBitSpace[rgb[i + 2]] << 11) | (sixBitSpace[rgb[i + 1]] << 5) | fiveBitSpace[rgb[i]]);
                        stream.Write(BitConverter.GetBytes(pixel), 0, 2);
                    }
                }
                else if (format == PixelFormat.Format16bppArgb1555)
                {
                    for (int i = 0; i < rgb.Length; i += 3)
                    {
                        ushort pixel;
                        // Transparent pixel case
                        if (rgb[i + 2] == 0 && rgb[i + 1] == 0 && rgb[i] == 0)
                        {
                            pixel = 0;
                        }
                        else
                        {
                            // OR with 0x8000 to set the alpha bit
                            pixel = (ushort)(0x8000 | (fiveBitSpace[rgb[i + 2]] << 10) | (fiveBitSpace[rgb[i + 1]] << 5) | fiveBitSpace[rgb[i]]);
                        }
                        stream.Write(BitConverter.GetBytes(pixel), 0, 2);
                    }
                }
                else
                {
                    throw new ArgumentException("format", "Pixel format must be RGB565 or ARGB1555");
                }
                return stream.ToArray();
            }
        }
    }
}