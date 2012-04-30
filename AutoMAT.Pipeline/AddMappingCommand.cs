using System;
using System.Windows.Input;
using AutoMAT.Common;

namespace AutoMAT.Pipeline
{
    class AddMappingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            PreferencesManager.Current.Mappings.Add(new PipelineMapping { Options = ConversionOptions.Default });
        }
    }
}