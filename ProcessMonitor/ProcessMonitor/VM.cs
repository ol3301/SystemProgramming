using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;

namespace ProcessMonitor
{
    class VM : BindableBase
    {
        private Timer timer = new Timer();

        private DelegateCommand _startCommand;
        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand
        {
            get => _saveCommand ?? (_saveCommand = new DelegateCommand(()=>
            {
                SaveFileDialog dlg = new SaveFileDialog();
                if(dlg.ShowDialog() == true)
                {
                    using (StreamWriter fs = new StreamWriter(dlg.FileName))
                    {
                        foreach (var i in Processes)
                        {
                            fs.WriteLine($"name: {i.ProcessName}      PID: {i.Id}      Modules: {i.HandleCount}|");
                        }
                    }
                }
            }));
        }
        public DelegateCommand StartCommand
        {
            get => _startCommand ?? (_startCommand = new DelegateCommand(() =>
            {
                if (timer.Enabled)
                    return;

                timer.Elapsed += new ElapsedEventHandler(Tick);
                timer.Interval = 1000;
                timer.Start();
            }));
        }


        public ObservableCollection<Process> Processes { get; set; }



        private void Tick(object sender,ElapsedEventArgs arg)
        {
            Application.Current.Dispatcher.Invoke(new System.Action(()=>
            {
                Processes.Clear();
                foreach (var i in Process.GetProcesses())
                    Processes.Add(i);
            }));
        }

        public VM()
        {
            Processes = new ObservableCollection<Process>();
        }
    }
}
