using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FinderFiles
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected Mutex mutex = new Mutex(false,"MY_PROGRAMM");
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!mutex.WaitOne(500))
                Environment.Exit(0);

            MainWindow window = new MainWindow();
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            mutex.ReleaseMutex();
        }
    }
}
