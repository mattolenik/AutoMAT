using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace AutoMAT.Pipeline
{
    static class PreferencesManager
    {
        const string FileName = "preferences.json";

        public static Preferences Current { get; set; }

        public static void Load()
        {
            var preferencesFile = new FileInfo(Path.Combine(AppData.ApplicationPath, FileName));
            if (preferencesFile.Exists)
            {
                try
                {
                    Current = JsonConvert.DeserializeObject<Preferences>(File.ReadAllText(preferencesFile.FullName));
                }
                catch(Exception e)
                {
                    preferencesFile.Delete();
                    Current = new Preferences();
                    Logger.WriteLine("An exception occurred while trying to read preferences from file {0}:", preferencesFile);
                    Logger.WriteLine(e);
                    Logger.WriteLine();
                }
            }
            else
            {
                Current = new Preferences();
            }
        }
        
        public static void Save()
        {
            var preferencesFile = new FileInfo(Path.Combine(AppData.ApplicationPath, FileName));
            try
            {
                File.WriteAllText(preferencesFile.FullName, JsonConvert.SerializeObject(Current, Formatting.Indented));
            }
            catch (Exception e)
            {
                preferencesFile.Delete();
                Current = new Preferences();
                Logger.WriteLine("An exception occurred while trying to write preferences to file {0}:", preferencesFile);
                Logger.WriteLine(e);
                Logger.WriteLine();
            }
        }
    }
}
