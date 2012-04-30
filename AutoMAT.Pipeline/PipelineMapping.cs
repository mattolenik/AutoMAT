using System;
using System.ComponentModel;
using AutoMAT.Common;

namespace AutoMAT.Pipeline
{
    public class PipelineMapping : INotifyPropertyChanged, IEquatable<PipelineMapping>, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string inputDirectory;

        public string InputDirectory
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

        string outputDirectory;

        public string OutputDirectory
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

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as PipelineMapping;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public bool Equals(PipelineMapping other)
        {
            if (other == null)
            {
                return false;
            }
            return outputDirectory == other.outputDirectory && inputDirectory == other.inputDirectory;
        }

        public override int GetHashCode()
        {
            return (inputDirectory + outputDirectory).GetHashCode();
        }

        public static bool operator ==(PipelineMapping x, PipelineMapping y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return false;
            }
            return x.Equals(y);
        }

        public static bool operator !=(PipelineMapping x, PipelineMapping y)
        {
            return !(x == y);
        }

        public object Clone()
        {
            return new PipelineMapping
            {
                InputDirectory = this.InputDirectory,
                OutputDirectory = this.OutputDirectory,
                Options = this.Options.Clone() as ConversionOptions
            };
        }
    }
}