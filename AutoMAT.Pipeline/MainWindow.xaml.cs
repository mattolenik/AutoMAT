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
        Pipeline pipeline;

        public MainWindow()
        {
            PreferencesManager.Load();
            InitializeComponent();
            pipeline = new Pipeline();
            pipeline.Start();
            pipeline.AddAndStart(
                new PipelineMapping
                {
                    InputDirectory = @"C:\Users\Matt\Desktop\MAT\input",
                    OutputDirectory = @"C:\Users\Matt\Desktop\MAT",
                    Options = ConversionOptions.Default
                });
            mappingList.ItemsSource = PreferencesManager.Current.Mappings;
        }

        void Save_Click(object sender, RoutedEventArgs e)
        {
            PreferencesManager.Save();
            pipeline.Reset();
            pipeline.AddAndStart(PreferencesManager.Current.Mappings.ToArray());
        }

        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PreferencesManager.Current.Mappings.Add(new PipelineMapping { Options = ConversionOptions.Default });
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selected in mappingList.SelectedItems.OfType<PipelineMapping>().ToList())
            {
                PreferencesManager.Current.Mappings.Remove(selected);
            }
        }
    }
}
