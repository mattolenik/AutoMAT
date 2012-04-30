using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AutoMAT.Pipeline
{
    public class PreferencesManager
    {
        const string FileName = "preferences.json";

        public static Preferences Current { get; set; }

        static Preferences snapshot;

        public PreferencesManager()
        {
        }

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
            snapshot = Current.Clone() as Preferences;
        }
        
        public static void Save()
        {
            snapshot = Current.Clone() as Preferences;

            Directory.CreateDirectory(AppData.ApplicationPath);
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

        public static Task SaveAsync()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    Save();
                });
        }

        public static void RevertToSnapshot()
        {
            Current.Mappings.Clear();
            foreach (var mapping in snapshot.Mappings)
            {
                Current.Mappings.Add(mapping.Clone() as PipelineMapping);
            }
            Current.EnableSync = snapshot.EnableSync;
        }
    }
}
