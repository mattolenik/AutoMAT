using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AutoMAT.Common;

namespace AutoMAT.Pipeline
{
    static class Logger
    {
        public const string FileName = "log.txt";

        public static void WriteLine(object o)
        {
            string path = Path.Combine(AppData.ApplicationPath, FileName);
            using (var writer = new StreamWriter(File.Open(path, FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine(o);
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(format.FormatInvariant(args));
        }

        public static void WriteLine()
        {
            WriteLine(string.Empty);
        }
    }
}
