using System;
using System.Linq;
using System.Windows.Input;

namespace AutoMAT.Pipeline
{
    class SaveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            PreferencesManager.Save();
            PipelineManager.Current.Reset();
            PipelineManager.Current.AddAndStart(PreferencesManager.Current.Mappings.ToArray());
        }
    }
}