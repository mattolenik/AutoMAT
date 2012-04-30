using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoMAT.Pipeline
{
    class DeleteMappingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var list = parameter as ListView;
            if (list == null)
            {
                throw new ArgumentNullException("parameter", "parameter is either null or not a ListView control");
            }
            foreach (var selected in list.SelectedItems.OfType<PipelineMapping>().ToList())
            {
                PreferencesManager.Current.Mappings.Remove(selected);
            }
        }
    }
}