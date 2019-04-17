using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using OGeneration.ViewModel;
using SampleApp.ViewModels;
using WPF.Common.AvalonDockMVVM;
using WPF.Common.ViewModels;


namespace OGeneration.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IDialogProvider
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
            this.DataContext = new MainWindowViewModel(this);
        }

        /// <summary>
        /// This method allows the user to select a file to open 
        /// (so the view-model can implement 'Open File' functionality).
        /// </summary>
        public bool UserSelectsFileToOpen(out string filePath)
        {
            var openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                filePath = openFileDialog.FileName;
                return true;
            }
            else
            {
                filePath = null;
                return false;
            }
        }

        /// <summary>
        /// This method allows the user to select a new filename for an existing file 
        /// (so the view-model can implement 'Save As' functionality).
        /// </summary>
        public bool UserSelectsNewFilePath(string oldFilePath, out string newFilePath)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = this.ViewModel.ActiveDocument.FilePath;

            var result = saveFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                newFilePath = saveFileDialog.FileName;
                return true;
            }
            else
            {
                newFilePath = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Display an error message dialog box.
        /// This allows the view-model to display error messages.
        /// </summary>
        public void ErrorMessage(string msg)
        {
            MessageBox.Show(this, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Allow the user to confirm whether they want to close a modified document.
        /// </summary>
        public bool QueryCloseModifiedDocument(TextFileDocumentViewModel document)
        {
            string msg = document.FileName + " has been modified but not saved.\n" +
                         "Do you really want to close it?";
            var result = MessageBox.Show(this, msg, "File modified but not saved", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Allow the user to confirm whether they want to close the application 
        /// when 1 or more documents are modified.
        /// </summary>
        public bool QueryCloseApplicationWhenDocumentsModified()
        {
            string msg = "1 or more open files have been modified but not saved.\n" +
                         "Do you really want to exit?";
            var result = MessageBox.Show(this, msg, "File(s) modified but not saved", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        private MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel) this.DataContext; }
        }

        /// <summary>
        /// Event raised when the 'NewFile' command is executed.
        /// </summary>
        private void NewFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.NewFile();
        }

        /// <summary>
        /// Event raised when the 'OpenFile' command is executed.
        /// </summary>
        private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.OpenFile();
        }

        /// <summary>
        /// Event raised when the 'SaveFile' command is executed.
        /// </summary>
        private void SaveFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.SaveFile();
        }

        /// <summary>
        /// Event raised when the 'SaveFileAs' command is executed.
        /// </summary>
        private void SaveFileAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.SaveFileAs();
        }

        /// <summary>
        /// Event raised when the 'SaveAllFiles' command is executed.
        /// </summary>
        private void SaveAllFiles_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.SaveAllFiles();
        }

        /// <summary>
        /// Event raised when the 'CloseFile' command is executed.
        /// </summary>
        private void CloseFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.CloseFile();
        }

        /// <summary>
        /// Event raised when the 'CloseAllFiles' command is executed.
        /// </summary>
        private void CloseAllFiles_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.CloseAllFiles();
        }

        /// <summary>
        /// Event raised when the 'ShowAllPanes' command is executed.
        /// </summary>
        private void ShowAllPanes_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.ShowAllPanes();
        }

        /// <summary>
        /// Event raised when the 'HideAllPanes' command is executed.
        /// </summary>
        private void HideAllPanes_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.HideAllPanes();
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event raised when AvalonDock has loaded.
        /// </summary>
        private void avalonDockHost_AvalonDockLoaded(object sender, EventArgs e)
        {
            //if (System.IO.File.Exists(LayoutFileName))
            //{
            //    //
            //    // If there is already a saved layout file, restore AvalonDock layout from it.
            //    //
            //    avalonDockHost.DockingManager.RestoreLayout(LayoutFileName);
            //}
            //else
            //{
            //    //
            //    // This line of code can be uncommented to get a list of resources.
            //    //
            //    //string[] names = this.GetType().Assembly.GetManifestResourceNames();

            //    //
            //    // Load the default AvalonDock layout from an embedded resource.
            //    //
            //    var assembly = Assembly.GetExecutingAssembly();
            //    using (var stream = assembly.GetManifestResourceStream(DefaultLayoutResourceName))
            //    {
            //        avalonDockHost.DockingManager.RestoreLayout(stream);
            //    }
            //}
        }

        /// <summary>
        /// Event raised when a document is being closed by clicking the 'X' button in AvalonDock.
        /// </summary>
        private void avalonDockHost_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            var document = (TextFileDocumentViewModel) e.Document;
            if (!this.ViewModel.QueryCanCloseFile(document))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Event raised when the window is about to close.
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //
            // Notify the view-model that the application is closing,
            // allows the view-model the chance to cancel application exit.
            //
            if (!this.ViewModel.OnApplicationClosing())
            {
                //
                // The view-model has cancelled application exit.
                // This will happen when the 1 or more documents have been modified but not saved
                // and the user has selected 'No' when asked to confirm application exit.
                //
                e.Cancel = true;
                return;
            }

            //
            // When the window is closing, save AvalonDock layout to a file.
            //
            avalonDockHost.DockingManager.SaveLayout(LayoutFileName);
        }
    }
}
