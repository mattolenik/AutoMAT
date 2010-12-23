using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CommandLine;

namespace AutoMAT
{
	class Program
	{
		static void Main(String[] args)
		{
			//var stream = File.OpenRead("06column01.mat");
			//var header = RawSerializer.Deserialize<MatHeader>(stream);
			//var recordHeader = RawSerializer.Deserialize<MatRecordHeader>(stream);
			//var dataHeader = RawSerializer.Deserialize<TextureDataHeader>(stream);

			var options = new Options();
			var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
			if (!parser.ParseArguments(args, options))
			{
				Environment.Exit(1);
			}
			if(options.InputFiles.Count == 0)
			{
				Console.Error.WriteLine(options.GetUsage());
				Console.Error.WriteLine();
				Console.Error.WriteLine("You must specify at least one input file.");
			}
			foreach (var file in options.InputFiles)
			{
				try
				{
					var inputFile = new FileInfo(file);
					var bitmap = Bitmap.FromStream(inputFile.OpenRead()) as Bitmap;

					var large = bitmap.Width > 256 || bitmap.Height > 256;

					// Only generate mipmaps if the image isn't larger than 256 or if LargeMipmaps
					// option is enabled.
					int numMipmaps = !large || options.AlwaysMipmap ? options.NumMipmaps + 1 : 1;

					bitmap = Filters.Dither(bitmap, PixelFormat.Format16bppRgb565,
						options.Dither ? DitherMethod.Burkes : DitherMethod.None);

					var result = Converter.ToMat16(bitmap, PixelFormat.Format16bppRgb565, numMipmaps);

					var outputPath = Path.Combine(inputFile.DirectoryName, String.Format("{0}.mat", inputFile.BareName()));
					File.WriteAllBytes(outputPath, result);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine(e.Message);
				}
			}
		}
	}
}