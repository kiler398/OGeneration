using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; 


namespace OGeneration.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window 
    {
        /// <summary>
        /// Name of the file used to save/restore AvalonDock layout.
        /// </summary>
        private static readonly string LayoutFileName = "MyLayoutFile.xml";

        /// <summary>
        /// Name of the embedded resource that contains the default AvalonDock layout.
        /// </summary>
        private static readonly string DefaultLayoutResourceName = "OGeneration.Resources.DefaultLayoutFile.xml";

        private string startupPath;
        private List<string> startupArgs;


        public MainWindow(string startupPath, params string[] args)
        {
            this.startupPath = startupPath;
            this.startupArgs = new List<string>(args);

            InitializeComponent();

            //
            // Create the main window's view-model and assign to the DataContext.
            //
            //this.DataContext = new MainWindowViewModel(this);
        }
         
    }
}
