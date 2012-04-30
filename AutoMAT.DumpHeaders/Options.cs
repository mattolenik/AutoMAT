using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using System.Reflection;

namespace AutoMAT.DumpHeaders
{
    sealed class Options
    {
        [ValueList(typeof(List<string>))]
        public List<string> MatFiles = new List<string>();

        [HelpOption(HelpText="Display this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText(new HeadingInfo("AutoMAT.DumpHeaders", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            help.Copyright = new CopyrightInfo("Matt Olenik", 2012);
            help.AddPreOptionsLine("This is freeware, you may distribute it as such.");
            help.AddPreOptionsLine(string.Empty);
            help.AddPreOptionsLine("Usage: AutoMAT.DumpHeaders file1.mat [file2.mat file3.mat...]");
            return help;
        }
    }
}
