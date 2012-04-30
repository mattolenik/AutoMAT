using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;

namespace AutoMAT.Pipeline
{
    class DirectoryBrowseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var textbox = parameter as TextBox;
            if (textbox == null)
            {
                throw new ArgumentNullException("parameter", "parameter is either null or not a TextBox");
            }

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textbox.Text = dialog.SelectedPath;
            }
        }
    }
}
