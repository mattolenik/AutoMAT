using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using System.Reflection;

namespace AutoMAT
{
    sealed class Options
    {
        [ValueList(typeof(List<String>))]
        public List<String> InputPaths = new List<String>();

        [Option("d", "dither",
            Required = false,
            HelpText = "Toggle dithering on MAT output, not used by default.")]
        public bool Dither = false;

        [Option("t", "transparency",
            Required = false,
            HelpText = "Whether or not to create a transparent MAT. Uses RGB(0,0,0) as the transparent color. Disables mipmaps and dither.")]
        public bool Transparency = false;

        [Option(null, "alwaysMipmap",
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
            var help = new HelpText(new HeadingInfo("AutoMAT", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            help.Copyright = new CopyrightInfo("Matt Olenik", 2012);
            help.AddPreOptionsLine("This is freeware, you may distribute it as such.");
            help.AddPreOptionsLine(string.Empty);
            help.AddPreOptionsLine("Supports all file formats supported by the DevIL image library. See: http://openil.sourceforge.net/features.php");
            help.AddPreOptionsLine("Common formats are: bmp, jpg, png, tga, gif, psp, psd");
            help.AddPreOptionsLine(string.Empty);
            help.AddPreOptionsLine("Options in [brackets] are optional. Specify one or more input images.");
            help.AddPreOptionsLine(string.Empty);
            help.AddPreOptionsLine("Usage: AutoMAT [-d] [-m 3] [--alwaysMipmap] image1.bmp [image2.png image3.jpg...]");
            help.AddPreOptionsLine(string.Empty);
            help.AddPostOptionsLine("Typical usage is just this: ");
            help.AddPostOptionsLine("AutoMAT image1.bmp [image2.png image3.jpg...]");
            help.AddPostOptionsLine(string.Empty);
            help.AddPostOptionsLine("For animated MATs, pass in folders containing the frames. Frame order will by done by alphanumeric sorting of the file names.");
            help.AddPostOptionsLine("AutoMAT -a folderForMat1 [folderForMat2 folderForMat3...]");
            help.AddPostOptionsLine(string.Empty);
            help.AddPostOptionsLine("With no options specified, AutoMAT will use no dithering and create 3 mipmaps, unless the source image is larger than 256 in width or height, then no mipmaps will be created by default.");
            help.AddOptions(this);
            return help;
        }
    }
}