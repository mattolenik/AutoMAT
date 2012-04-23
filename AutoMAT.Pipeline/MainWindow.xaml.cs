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
            var pipeline = new Pipeline();
            pipeline.Start();
            pipeline.AddAndStart(
                new PipelineMapping
                {
                    InputDirectory = new DirectoryInfo(@"C:\Users\Matt\Desktop\MAT\input"),
                    OutputDirectory = new DirectoryInfo(@"C:\Users\Matt\Desktop\MAT"),
                    Options = ConversionOptions.Default
                });
        }
    }
}
