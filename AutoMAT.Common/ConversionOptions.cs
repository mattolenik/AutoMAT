using System.ComponentModel;
using System;

namespace AutoMAT.Common
{
    public class ConversionOptions : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool transparency;
        public bool Transparency
        {
            get { return transparency; }
            set
            {
                if (value != transparency)
                {
                    transparency = value;
                    NotifyPropertyChanged("Transparency");
                }
            }
        }

        int numMipmaps;
        public int NumMipmaps
        {
            get { return numMipmaps; }
            set
            {
                if (value != numMipmaps)
                {
                    numMipmaps = value;
                    NotifyPropertyChanged("NumMipmaps");
                }
            }
        }

        bool forceMipmaps;
        public bool ForceMipmaps
        {
            get { return forceMipmaps; }
            set
            {
                if (value != forceMipmaps)
                {
                    forceMipmaps = value;
                    NotifyPropertyChanged("ForceMipmaps");
                }
            }
        }

        bool dither;
        public bool Dither
        {
            get { return dither; }
            set
            {
                if (value != dither)
                {
                    dither = value;
                    NotifyPropertyChanged("Dither");
                }
            }
        }

        public static ConversionOptions Default
        {
            get
            {
                return new ConversionOptions
                {
                    Transparency = false,
                    NumMipmaps = 3,
                    ForceMipmaps = false,
                    Dither = false
                };
            }
        }

        void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public object Clone()
        {
            return new ConversionOptions
            {
                Dither = this.Dither,
                ForceMipmaps = this.ForceMipmaps,
                NumMipmaps = this.NumMipmaps,
                Transparency = this.Transparency
            };
        }
    }
}