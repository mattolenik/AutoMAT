using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace AutoMAT.Pipeline
{
    public class Preferences
    {
        public ObservableCollection<PipelineMapping> Mappings { get; set; }
    }
}