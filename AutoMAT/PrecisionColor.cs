using System;
using System.Drawing;

namespace AutoMAT
{
	public struct PrecisionColor
	{
		public double R { get; set; }
		public double G { get; set; }
		public double B { get; set; }

		public PrecisionColor(double r, double g, double b)
			: this()
		{
			R = r;
			G = g;
			B = b;
		}

		public static implicit operator PrecisionColor(Color color)
		{
			return new PrecisionColor(color.R, color.G, color.B);
		}

		public static implicit operator Color(PrecisionColor color)
		{
			byte r = (byte)Math.Round(color.R, MidpointRounding.AwayFromZero);
			byte g = (byte)Math.Round(color.G, MidpointRounding.AwayFromZero);
			byte b = (byte)Math.Round(color.B, MidpointRounding.AwayFromZero);
			return Color.FromArgb(r, g, b);
		}

		public static PrecisionColor operator +(PrecisionColor c1, PrecisionColor c2)
		{
			double r = c1.R + c2.R;
			double g = c1.G + c2.G;
			double b = c1.B + c2.B;
			return new PrecisionColor(r, g, b);
		}

		public static PrecisionColor operator -(PrecisionColor c1, PrecisionColor c2)
		{
			double r = c1.R - c2.R;
			double g = c1.G - c2.G;
			double b = c1.B - c2.B;
			r = r < 0 ? 0 : r;
			g = g < 0 ? 0 : g;
			b = b < 0 ? 0 : b;
			return new PrecisionColor(r, g, b);
		}

		public static PrecisionColor operator *(PrecisionColor c, double scalar)
		{
			return new PrecisionColor(c.R * scalar, c.G * scalar, c.B * scalar);
		}
	}
}