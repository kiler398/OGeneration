﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using SampleApp.ViewModels;
using WPF.Common.ViewModels;

namespace OGeneration.ViewModel
{
    /// <summary>
    /// Class for the main window's view-model.
    /// </summary>
    public sealed class MainWindowViewModel : AbstractViewModel
    {
        /// <summary>
        /// The default title for the main window.
        /// </summary>
        private static readonly string defaultTitle = "My First Text Editor";

        /// <summary>
        /// View-model for the active document.
        /// </summary>
        private TextFileDocumentViewModel activeDocument = null;

        /// <summary>
        /// View-model for the active pane.
        /// </summary>
        private AbstractPaneViewModel activePane = null;

        public MainWindowViewModel(IDialogProvider dialogProvider)
        {
            this.DialogProvider = dialogProvider;

            //
            // Initialize the 'Document Overview' pane view-model.
            //
            this.DocumentOverviewPaneViewModel = new DocumentOverviewPaneViewModel(this);

            //
            // Initialize the 'Open Documents' pane view-model.
            //
            this.OpenDocumentsPaneViewModel = new OpenDocumentsPaneViewModel(this);

            this.OpenDocumentsPaneViewModel1 = new OpenDocumentsPaneViewModel(this);

            //
            // Add view-models for panes to the 'Panes' collection.
            //
            this.Panes = new ObservableCollection<AbstractPaneViewModel>();
            this.Panes.Add(this.DocumentOverviewPaneViewModel);
            this.Panes.Add(this.OpenDocumentsPaneViewModel);
            this.Panes.Add(this.OpenDocumentsPaneViewModel1);

            //
            // Add an example/test document view-model.
            //
            this.Documents = new ObservableCollection<TextFileDocumentViewModel>();
            this.Documents.Add(new TextFileDocumentViewModel(string.Empty, "test data!", true));
        }

        public OpenDocumentsPaneViewModel OpenDocumentsPaneViewModel1 { get; set; }

        /// <summary>
        /// The current title of the main window.
        /// </summary>
        public string Title
        {
            get
            {
                var title = new StringBuilder();
                title.Append(defaultTitle);

                if (this.ActiveDocument != null)
                {
                    title.Append(" - ");
                    title.Append(this.ActiveDocument.Title);
                }

                return title.ToString();
            }
        }

        /// <summary>
        /// View-models for panes.
        /// </summary>
        public ObservableCollection<AbstractPaneViewModel> Panes
        {
            get;
            private set;
        }

        /// <summary>
        /// View-models for documents.
        /// </summary>
        public ObservableCollection<TextFileDocumentViewModel> Documents
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns 'true' if any of the open documents are modified.
        /// </summary>
        public bool AnyDocumentIsModified
        {
            get
            {
                foreach (var document in this.Documents)
                {
                    if (document.IsModified)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// View-model for the active document.
        /// </summary>
        public TextFileDocumentViewModel ActiveDocument
        {
            get
            {
                return activeDocument;
            }
            set
            {
                if (activeDocument == value)
                {
                    return;
                }

                if (activeDocument != null)
                {
                    activeDocument.IsModifiedChanged -= new EventHandler<EventArgs>(activeDocument_IsModifiedChanged);
                }

                activeDocument = value;

                if (activeDocument != null)
                {
                    activeDocument.IsModifiedChanged += new EventHandler<EventArgs>(activeDocument_IsModifiedChanged);
                }

                OnPropertyChanged("ActiveDocument");
                OnPropertyChanged("Title");

                if (ActiveDocumentChanged != null)
                {
                    ActiveDocumentChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Event raised when the ActiveDocument property has changed.
        /// </summary>
        public event EventHandler<EventArgs> ActiveDocumentChanged;

        /// <summary>
        /// View-model for the active pane.
        /// </summary>
        public AbstractPaneViewModel ActivePane
        {
            get
            {
                return activePane;
            }
            set
            {
                if (activePane == value)
                {
                    return;
                }

                activePane = value;

                OnPropertyChanged("ActivePane");
            }
        }

        /// <summary>
        /// View-model for the 'Document Overview' pane.
        /// </summary>
        public DocumentOverviewPaneViewModel DocumentOverviewPaneViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// View-model for the 'Open Documents' pane.
        /// </summary>
        public OpenDocumentsPaneViewModel OpenDocumentsPaneViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Create a new file and add it to the view-model.
        /// </summary>
        public void NewFile()
        {
            var newDocument = new TextFileDocumentViewModel(string.Empty, string.Empty, false);
            this.Documents.Add(newDocument);
        }

        /// <summary>
        /// Allow the user to select a particular file then open it.
        /// </summary>
        public void OpenFile()
        {
            string filePath = null;
            if (DialogProvider.UserSelectsFileToOpen(out filePath))
            {
                OpenFile(filePath);
            }
        }

        /// <summary>
        /// Open a file, create a view-model and it to the collection of document view-models.
        /// </summary>
        public void OpenFile(string filePath)
        {
            try
            {
                var fileContent = File.ReadAllText(filePath);
                var newDocumentViewModel = new TextFileDocumentViewModel(filePath, fileContent, false);
                this.Documents.Add(newDocumentViewModel);
            }
            catch (Exception ex)
            {
                DialogProvider.ErrorMessage("Failed to open document " + filePath + "\n" +
                                            "Exception occurred:\n" +
                                            ex.ToString());

            }
        }

        /// <summary>
        /// Save the active document, allowing the user to select a filename for new files.
        /// </summary>
        public void SaveFile()
        {
            SaveFile(this.ActiveDocument);
        }

        /// <summary>
        /// Save the specified document, allowing the user to select a filename for new files.
        /// </summary>
        public void SaveFile(TextFileDocumentViewModel document)
        {
            if (string.IsNullOrEmpty(document.FilePath))
            {
                SaveFileAs(document);
            }
            else
            {
                SaveFile(document, document.FilePath);
            }
        }

        /// <summary>
        /// Save the specified document to the specified filepath.
        /// </summary>
        public void SaveFile(TextFileDocumentViewModel document, string newFilePath)
        {
            try
            {
                File.WriteAllText(newFilePath, document.Text);

                document.FilePath = newFilePath;
                document.IsModified = false;
            }
            catch (Exception ex)
            {
                DialogProvider.ErrorMessage("Failed to save document " + newFilePath + "\n" +
                                            "Exception occurred: \n" +
                                            ex.ToString());
            }
        }

        /// <summary>
        /// Save the active document as a new file, allowing the user to specify the new filepath.
        /// </summary>
        public void SaveFileAs()
        {
            SaveFileAs(this.ActiveDocument);
        }

        /// <summary>
        /// Save the specified document as a new file, allowing the user to specify the new filepath.
        /// </summary>
        public void SaveFileAs(TextFileDocumentViewModel document)
        {
            string newFilePath = null;
            if (DialogProvider.UserSelectsNewFilePath(document.FilePath, out newFilePath))
            {
                SaveFile(document, newFilePath);
            }
        }

        /// <summary>
        /// Save all documents.
        /// </summary>
        public void SaveAllFiles()
        {
            foreach (var document in this.Documents)
            {
                SaveFile(document);
            }
        }

        /// <summary>
        /// Close the active document.
        /// </summary>
        public void CloseFile()
        {
            CloseFile(this.ActiveDocument);
        }

        /// <summary>
        /// Determine if the file can be closed.
        /// If the file is modified, but not saved, the user is asked
        /// to confirm that the document should be closed.
        /// </summary>
        public bool QueryCanCloseFile(TextFileDocumentViewModel document)
        {
            if (document.IsModified)
            {
                //
                // Ask the user to confirm closing a modified document.
                //
                if (!this.DialogProvider.QueryCloseModifiedDocument(document))
                {
                    // User doesn't want to close it.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Close the specified document.
        /// Returns 'true' if the user allowed the file to be closed,
        /// or 'false' if the user canceled closing of the file.
        /// </summary>
        public bool CloseFile(TextFileDocumentViewModel document)
        {
            if (!QueryCanCloseFile(document))
            {
                //
                // User has chosen not to close the file.
                //
                return false;
            }

            this.Documents.Remove(document);

            //
            // File has been closed.
            //
            return true;
        }

        /// <summary>
        /// Close all open documents.
        /// </summary>
        public void CloseAllFiles()
        {
            //
            // Copy the list to an array so that we can iterate
            // and remove items from the list.
            //
            var documents = this.Documents.ToArray();

            foreach (var document in documents)
            {
                CloseFile(document);
            }
        }

        /// <summary>
        /// Show all panes.
        /// </summary>
        public void ShowAllPanes()
        {
            foreach (var pane in this.Panes)
            {
                pane.IsVisible = true;
            }
        }

        /// <summary>
        /// Hide all panes.
        /// </summary>
        public void HideAllPanes()
        {
            foreach (var pane in this.Panes)
            {
                pane.IsVisible = false;
            }
        }

        /// <summary>
        /// Called when the application is closing.
        /// Return 'true' to allow application to exit.
        /// </summary>
        public bool OnApplicationClosing()
        {
            if (this.AnyDocumentIsModified)
            {
                if (!this.DialogProvider.QueryCloseApplicationWhenDocumentsModified())
                {
                    //
                    // User has cancelled application exit.
                    //
                    return false;
                }
            }

            //
            // Allow application exit to proceed.
            //
            return true;
        }

        /// <summary>
        /// An interface that provides dialog services, eg open file, save file as, error message boxes.
        /// </summary>
        private IDialogProvider DialogProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Event raised when the active document's IsModified property has changed.
        /// </summary>
        private void activeDocument_IsModifiedChanged(object sender, EventArgs e)
        {
            //
            // Update the main window's title when the active document has been modified.
            //
            OnPropertyChanged("Title");
        }
    }
}