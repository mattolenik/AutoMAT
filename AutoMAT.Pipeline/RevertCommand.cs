using System;
using System.Windows.Input;

namespace AutoMAT.Pipeline
{
    class RevertCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            PreferencesManager.RevertToSnapshot();
        }
    }
}