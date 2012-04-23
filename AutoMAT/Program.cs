using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMAT.Common;
using CommandLine;

namespace AutoMAT
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
            {
                Environment.Exit(1);
            }
            if (options.InputPaths.Count == 0)
            {
                Console.WriteLine(options.GetUsage());
                Console.WriteLine();
                Console.WriteLine("You must specify at least one input file.");
            }

            Parallel.ForEach(options.InputPaths,
            (path) =>
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        var directory = new DirectoryInfo(path);
                        FileInfo[] inputFiles = directory.GetFiles().OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase).ToArray();
                        var outputFile = new FileInfo(Path.Combine(directory.Parent.FullName, directory.Name + ".mat"));
                        Converter.Convert(
                            new ConversionOptions
                            {
                                Dither = options.Dither,
                                ForceMipmaps = options.AlwaysMipmap,
                                NumMipmaps = options.NumMipmaps,
                                Transparency = options.Transparency
                            },
                            outputFile,
                            inputFiles);
                    }
                    else
                    {
                        var file = new FileInfo(path);
                        if (file.Exists)
                        {
                            Converter.Convert(
                                new ConversionOptions
                                {
                                    Dither = options.Dither,
                                    ForceMipmaps = options.AlwaysMipmap,
                                    NumMipmaps = options.NumMipmaps,
                                    Transparency = options.Transparency
                                },
                                new FileInfo(file.BareName() + ".mat"),
                                file);
                        }
                        else
                        {
                            Console.WriteLine("File not found: " + file.FullName);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}