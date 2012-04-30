using System.ComponentModel;
using System;
using System.Linq;

namespace AutoMAT.Pipeline
{
    public class Preferences : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        BindingList<PipelineMapping> mappings;
        public BindingList<PipelineMapping> Mappings
        {
            get { return mappings; }
            set
            {
                mappings = value;
                NotifyPropertyChanged("Mappings");
            }
        }

        bool enableSync;
        public bool EnableSync
        {
            get { return enableSync; }
            set
            {
                if (enableSync != value)
                {
                    enableSync = value;
                    NotifyPropertyChanged("EnableSync");
                }
            }
        }

        void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public Preferences()
        {
            Mappings = new BindingList<PipelineMapping>();
            EnableSync = true;
        }
        
        public object Clone()
        {
            var result = new Preferences
            {
                EnableSync = this.enableSync
            };
            foreach (var mapping in mappings)
            {
                result.Mappings.Add(mapping.Clone() as PipelineMapping);
            }
            return result;
        }
    }
}