using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using AutoMAT.Common;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AutoMAT.Pipeline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PreferencesManager.Load();
            InitializeComponent();
            mappingList.ItemsSource = PreferencesManager.Current.Mappings;
            PipelineManager.Current = new Pipeline();
            PipelineManager.Current.Start();
            PipelineManager.Current.AddAndStart(PreferencesManager.Current.Mappings.ToArray());
        }
    }
}
