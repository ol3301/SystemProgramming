using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Semaphore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        //Созданные потоки в ожидающие
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Мега хак
            ((sender as ListBox).SelectedItem as TaskVm).Start();
        }

        private void ListBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            //Мега хак
            (DataContext as Vm).ForWorkToWait(((sender as ListBox).SelectedItem as TaskVm));
        }
    }
}
