using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace T9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        string text => textb.Text;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var splited = text.Split(' ');

            if (e.Key == Key.Tab)
            {
                var v = text.LastWord();
                if (v == "")
                    return;
                textb.Text = text.Replace(text.LastWord(), (DataContext as VM).List.First());
                return;
            }
            (DataContext as VM).NewText(splited.Last());
        }
    }
}
