using System.Windows;
using System.Windows.Controls;
using WPF.Common.ViewModels;

namespace OGeneration.UserControls
{
    /// <summary>
    /// View class for the view of open documents.
    /// </summary>
    public partial class OpenDocumentsPaneView : UserControl
    {
        public OpenDocumentsPaneView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        private OpenDocumentsPaneViewModel ViewModel
        {
            get
            {
                return (OpenDocumentsPaneViewModel)this.DataContext;
            }
        }

        /// <summary>
        /// Event raised the user clicks the 'Close Selected' button.
        /// </summary>
        private void CloseSelected_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.CloseSelected();
        }
    }
}
