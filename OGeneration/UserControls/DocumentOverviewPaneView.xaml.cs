using System.Windows.Controls;
using WPF.Common.ViewModels;

namespace OGeneration.UserControls
{
    public partial class DocumentOverviewPaneView : UserControl
    {
        public DocumentOverviewPaneView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        private DocumentOverviewPaneViewModel ViewModel
        {
            get
            {
                return (DocumentOverviewPaneViewModel)this.DataContext;
            }
        }
    }
}
