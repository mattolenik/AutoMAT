using System;
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
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
            {
                Environment.Exit(1);
            }
            if (options.InputFiles.Count == 0)
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
                    using (var bitmap = Bitmap.FromStream(inputFile.OpenRead()) as Bitmap)
                    {
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
                            bool large = bitmap.Width > 256 || bitmap.Height > 256;

                            // Only generate mipmaps if the image isn't larger than 256 or if LargeMipmaps
                            // option is enabled.
                            //
                            // Add 1 because mipmaps includes original image
                            numMipmaps = !large || options.AlwaysMipmap ? options.NumMipmaps + 1 : 1;
                            format = PixelFormat.Format16bppRgb565;
                            ditherMethod = options.Dither ? DitherMethod.Burkes : DitherMethod.None;
                        }

                        using (Bitmap dithered = Filters.Dither(bitmap, format, ditherMethod))
                        {
                            byte[] result = Converter.ToMat16(dithered, format, numMipmaps);

                            string outputPath = Path.Combine(inputFile.DirectoryName, String.Format("{0}.mat", inputFile.BareName()));
                            File.WriteAllBytes(outputPath, result);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }
    }
}