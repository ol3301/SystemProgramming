using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TreeProcesses
{
    class VM : BindableBase
    {
        private Model model = new Model();

        public ObservableCollection<TreeViewItem> TreeItems { get; set; }

        public VM()
        {
                TreeItems = model.GetProcessesTree();
        }
    }
}
