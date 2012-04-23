using System.ComponentModel;
using System.IO;
using AutoMAT.Common;

namespace AutoMAT.Pipeline
{
    public class PipelineMapping : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        DirectoryInfo inputDirectory;
        public DirectoryInfo InputDirectory
        {
            get { return inputDirectory; }
            set
            {
                if (value != inputDirectory)
                {
                    inputDirectory = value;
                    NotifyPropertyChanged("InputDirectory");
                }
            }
        }

        DirectoryInfo outputDirectory;
        public DirectoryInfo OutputDirectory
        {
            get { return outputDirectory; }
            set
            {
                if (value != outputDirectory)
                {
                    outputDirectory = value;
                    NotifyPropertyChanged("OutputDirectory");
                }
            }
        }

        ConversionOptions options;
        public ConversionOptions Options
        {
            get { return options; }
            set
            {
                options = value;
                NotifyPropertyChanged("Options");
            }
        }

        void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}