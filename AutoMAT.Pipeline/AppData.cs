using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AutoMAT.Pipeline
{
    static class AppData
    {
        public static string ApplicationPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName); }
        }

        const string DirectoryName = "AutoMAT.Pipeline";
    }
}
