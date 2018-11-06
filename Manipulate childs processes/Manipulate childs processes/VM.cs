using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Manipulate_childs_processes
{
    class VM : BindableBase
    {
        //наша модель
        Model model = new Model();
        //синхронизируем потоки
        private object locker = new object();

        private DelegateCommand<string> _startCommand;
        private DelegateCommand<Process> _killCommand;
        private DelegateCommand<Process> _closeWindowCommand;
        private DelegateCommand<Process> _refreshCommand;
        private DelegateCommand _runCalcCommand;

        public ObservableCollection<Process> Processes { get; set; }
        public ObservableCollection<string> ProcessesSource { get; set; }

        public DelegateCommand<Process> RefreshCommand
        {
            get => _refreshCommand ?? (_refreshCommand = new DelegateCommand<Process>(prc=>
            {
                prc.Refresh();
            }));
        }
        public DelegateCommand<Process> CloseWindowCommand
        {
            get => _closeWindowCommand ?? (_closeWindowCommand = new DelegateCommand<Process>(prc=>
            {
                prc.CloseMainWindow();
            }));
        }
        public DelegateCommand<Process> KillCommand
        {
            get => _killCommand ?? (_killCommand = new DelegateCommand<Process>(prc=>
            {
                model.FullCloseProcess(prc);

                lock (locker)
                {
                    Processes.Remove(prc);
                }
            }));
        }
        public DelegateCommand<string> StartCommand
        {
            get => _startCommand ?? (_startCommand = new DelegateCommand<string>(str=>
            {
                Process p = new Process();
                p.StartInfo.FileName = str;
                p.Start();

                Processes.Add(p);

                Task.Run(()=>
                {
                    p.WaitForExit();
                    //невозможно изменять ObservableCollection не из того потока, где она не была создана
                    Application.Current.Dispatcher.Invoke(()=>
                    {
                        //синхронизация....(не точно)
                        lock (locker)
                        {
                            Processes.Remove(p);
                        }                   
                    });
                });
            }));
        }

        



        public VM()
        {
            Processes = new ObservableCollection<Process>();
            ProcessesSource = new ObservableCollection<string> { "EntityTran" };

            


        }
    }
}
