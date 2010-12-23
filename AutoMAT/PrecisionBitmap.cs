using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace AutoMAT
{
	public class PrecisionBitmap
	{
		private double[] rgbValues;
		public int Width { get; private set; }
		public int Height { get; private set; }

		public PrecisionBitmap(Bitmap bitmap)
		{
			Width = bitmap.Width;
			Height = bitmap.Height;
			var bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb);

			IntPtr ptr = bitmapData.Scan0;
			int stride = bitmapData.Stride;
			int numBytes = bitmap.Width * bitmap.Height * 3;
			byte[] rgbBytes = new byte[numBytes];
			Marshal.Copy(ptr, rgbBytes, 0, numBytes);

			rgbValues = new double[rgbBytes.Length];
			for (int i = 0; i < rgbBytes.Length; i++)
			{
				rgbValues[i] = rgbBytes[i];
			}

			bitmap.UnlockBits(bitmapData);
		}

		public double[] GetAllPixels()
		{
			return rgbValues;
		}

		public void SetAllPixels(double[] pixels)
		{
			rgbValues = pixels;
		}

		public PrecisionColor GetPixel(int x, int y)
		{
			double blue = rgbValues[(y * Width + x) * 3];
			double green = rgbValues[(y * Width + x) * 3 + 1];
			double red = rgbValues[(y * Width + x) * 3 + 2];

			return new PrecisionColor(red, green, blue);
		}

		public void SetPixel(int x, int y, PrecisionColor cIn)
		{
			rgbValues[(y * Width + x) * 3] = cIn.B;
			rgbValues[(y * Width + x) * 3 + 1] = cIn.G;
			rgbValues[(y * Width + x) * 3 + 2] = cIn.R;
		}

		public Bitmap ToBitmap()
		{
			var bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			var bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height),
				ImageLockMode.WriteOnly,
				PixelFormat.Format24bppRgb);

			byte[] rgbBytes = new byte[rgbValues.Length];
			for (int i = 0; i < rgbBytes.Length; i++)
			{
				var c = Math.Round(rgbValues[i], MidpointRounding.AwayFromZero);
				c = c > 255 ? 255 : c;
				rgbBytes[i] = (byte)c;
			}
			Marshal.Copy(rgbBytes, 0, bitmapData.Scan0, bitmap.Width * bitmap.Height * 3);
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}
	}
}