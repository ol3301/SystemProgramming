using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TreeProcesses
{
    class Model
    {
        private List<Process> processes;
        //сюда добавляемый дочерние процессы, что бы предотвратить повторное включение в корневой узел
        private List<Process> childs;

        //Возвращает дерево процессов
        public ObservableCollection<TreeViewItem> GetProcessesTree()
        {
            ObservableCollection<TreeViewItem> coll = new ObservableCollection<TreeViewItem>();
            processes = Process.GetProcesses().ToList();
            childs = new List<Process>();

            foreach (var process in processes)
            {
                if (childs.Contains(process))
                    continue;

                var item = GenerateTreeViewItem(process);
                AddChildItems(item,process);

                coll.Add(item);
            }

            return coll;
        }       

        public void AddChildItems(TreeViewItem item,Process proc)
        {
            foreach (var child in proc.GetChildProcesses())
            {
                childs.Add(child);
                var child_item = GenerateTreeViewItem(child);
                AddChildItems(child_item,child);

                item.Items.Add(GenerateTreeViewItem(child));
            }
        }

        public TreeViewItem GenerateTreeViewItem(Process proc)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = proc.ProcessName;
            item.Tag = proc;
            return item;
        }
    }
}
