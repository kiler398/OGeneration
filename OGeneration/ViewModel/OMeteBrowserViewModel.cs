using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Common.ViewModels;

namespace OGeneration.ViewModel
{
    public class OMeteBrowserViewModel
    {
        public ObservableCollection<TreeNodeModel> TreeNodes { get; set; }
    }
}
