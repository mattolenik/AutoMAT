using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoMAT
{
	public static class Converter
	{
		public static byte[] ToMat16(Bitmap source, PixelFormat format, int mipmaps)
		{
			if (format != PixelFormat.Format16bppRgb565 &&
				format != PixelFormat.Format16bppRgb555 &&
				format != PixelFormat.Format16bppArgb1555)
			{
				throw new ArgumentOutOfRangeException("format", "Only supported formats are RGB555, RGB565 and ARGB1555.");
			}

			MatHeader header = new MatHeader
			{
				Magic = new byte[] { (byte)'M', (byte)'A', (byte)'T', (byte)' ' },
				Version = 0x32,
				Type = 2,
				MatRecordCount = 1,
				TextureCount = 1,
				Transparency = 1,
				Bitdepth = 16,
				BlueBits = 5,
				GreenBits = 6,
				RedBits = 5,
				RedShl = 11,
				GreenShl = 5,
				BlueShl = 0,
				RedShr = 3,
				GreenShr = 2,
				BlueShr = 3
			};

			MatRecordHeader recordHeader = new MatRecordHeader
			{
				RecordType = 0x8,
				Unknown4 = 1065353216,
				Unknown6 = 4,
				Unknown7 = 4
			};

			TextureDataHeader dataHeader = new TextureDataHeader
			{
				SizeX = source.Width,
				SizeY = source.Height,
				MipmapCount = mipmaps
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
				for (int i = 0; i < mipmaps; i++)
				{
					int height = source.Height >> i;
					int width = source.Width >> i;
					stream.WriteBytes(GetBitmapData(Filters.Dither(Filters.Resize(source, width, height), format), format));
				}

				return stream.ToArray();
			}
		}

		private static byte[] GetBitmapData(Bitmap source, PixelFormat format)
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

			int greenShiftR, redShiftL;
			greenShiftR = format == PixelFormat.Format16bppRgb565 ? 2 : 3;
			redShiftL = format == PixelFormat.Format16bppRgb565 ? 11 : 10;

			var stream = new MemoryStream(source.Width * source.Height);
			for (int i = 0; i < rgbData.Length; i += 3)
			{
				var b = (byte)(rgbData[i] >> 3);
				var g = (byte)(rgbData[i + 1] >> greenShiftR);
				var r = (byte)(rgbData[i + 2] >> 3);
				UInt16 pixel = (UInt16)((r << redShiftL) | (g << 5) | b);
				stream.Write(BitConverter.GetBytes(pixel), 0, 2);
			}
			return stream.ToArray();
		}
	}
}