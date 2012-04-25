using System.ComponentModel;

namespace AutoMAT.Pipeline
{
    public class Preferences
    {
        public BindingList<PipelineMapping> Mappings { get; set; }

        public Preferences()
        {
            Mappings = new BindingList<PipelineMapping>();
        }
    }
}