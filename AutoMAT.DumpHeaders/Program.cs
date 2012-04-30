using System;
using System.IO;
using AutoMAT.Common;
using CommandLine;

namespace AutoMAT.DumpHeaders
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Out));
            if (!parser.ParseArguments(args, options))
            {
                Environment.Exit(1);
            }

            if (options.MatFiles.Count == 0)
            {
                Console.WriteLine(options.GetUsage());
                Console.WriteLine();
                Console.WriteLine("You must specify at least one input file.");
            }
            foreach (var file in options.MatFiles)
            {
                DumpHeaders(file);
            }
        }

        static void DumpHeaders(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                Console.WriteLine("MAT header data for file {0}:".FormatInvariant(file));
                Console.WriteLine("MAT Header section");
                
                var header = RawSerializer.Deserialize<MatHeader>(stream);

                if (header.Type == MatHeader.MatType.Texture)
                {
                    Console.WriteLine(header);
                    for (int i = 0; i < header.MatRecordCount; i++)
                    {
                        Console.WriteLine();
                        Console.WriteLine("MAT record section");
                        Console.WriteLine(RawSerializer.Deserialize<MatRecordHeader>(stream));
                    }

                    for (int i = 0; i < header.MatRecordCount; i++)
                    {
                        var textureHeader = RawSerializer.Deserialize<TextureDataHeader>(stream);
                        Console.WriteLine();
                        Console.WriteLine("MAT record section");
                        Console.WriteLine(textureHeader);

                        // Skip over texture data
                        long dataSize = textureHeader.SizeX * textureHeader.SizeY * header.Bitdepth;
                        stream.Seek(dataSize, SeekOrigin.Current);
                    }
                }
                else
                {
                    Console.WriteLine("Unsupported or unrecognized MAT type, only texture MATs are supported");
                    Environment.Exit(1);
                }
            }
        }
    }
}