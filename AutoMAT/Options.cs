using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace AutoMAT
{
	internal sealed class Options
	{
		[ValueList(typeof(List<String>))]
		public IList<String> InputFiles = new List<String>();

		[Option("d", "dither",
			Required = false,
			HelpText = "Toggle dithering on MAT output, not used by default.")]
		public bool Dither = false;

		[Option(null, "alwaysmipmap",
			Required = false,
			HelpText = "Always create mipmaps, even for images larger than 256, not used by default.")]
		public bool AlwaysMipmap = false;

		[Option("m", "mipmaps",
			Required = false,
			HelpText = "Number of mipmaps to generate, default 3.")]
		public int NumMipmaps = 3;

		[HelpOption(HelpText = "Display this help screen.")]
		public String GetUsage()
		{
			var help = new HelpText(new HeadingInfo("AutoMAT", "0.1"));
			help.Copyright = new CopyrightInfo("Matt Olenik", 2009);
			help.AddPreOptionsLine("This is freeware, you may distribute it as such.");
			help.AddPreOptionsLine(String.Empty);
			help.AddPreOptionsLine("Options in [brackets] are optional. Specify one or more input images.");
			help.AddPreOptionsLine("Supported formats formats: bmp, jpg, gif, png, tiff");
			help.AddPreOptionsLine(String.Empty);
			help.AddPreOptionsLine("Usage: AutoMAT [-d] [-m 3] [--alwaysMipmap] image1.bmp [image2.bmp image3.bmp...]");
			help.AddOptions(this);
			return help;
		}
	}
}
