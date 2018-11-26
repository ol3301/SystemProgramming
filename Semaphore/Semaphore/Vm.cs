using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Semaphore
{
    class Vm : BindableBase
    {
        private int _thCount;
        private System.Threading.Semaphore sem;
        private DelegateCommand _createThreadCommand;

        public ObservableCollection<TaskVm> CreatedThreads { get; set; }
        public ObservableCollection<TaskVm> WaitThreads { get; set; }
        public ObservableCollection<TaskVm> WorkThreads { get; set; }


        public DelegateCommand CreateThreadCommand
        {
            get => _createThreadCommand ?? (_createThreadCommand = new DelegateCommand(()=>
            {
                var task = new TaskVm();

                task.Thread = new Task(()=>
                {
                    ForCreatedToWait(task);

                    while (true)
                    {
                        if (sem.WaitOne())
                        {
                            ForWaitToWork(task);

                            while (!task.token.IsCancellationRequested)
                            {
                                Thread.Sleep(1000);
                                ++task.Count;
                            }
                        }
                    }

                });

                task.Id = task.Thread.Id;

                CreatedThreads.Add(task);
            }));
        }

        public int ThCount
        {
            get => _thCount;
            set
            {
                _thCount = value;
                sem = new System.Threading.Semaphore(value, value, "SemName");
            }
        }

        public Vm()
        {
            ThCount = 2;
            CreatedThreads = new ObservableCollection<TaskVm>();
            WaitThreads = new ObservableCollection<TaskVm>();
            WorkThreads = new ObservableCollection<TaskVm>();
        }

        /// <summary>
        /// переводим поток в список ожидающих
        /// </summary>
        /// <param name="tv">Выделенный поток</param>
        public void ForCreatedToWait(TaskVm task)
        {
            Application.Current.Dispatcher.Invoke(new Action(()=>
            {
                CreatedThreads.Remove(task);
                WaitThreads.Add(task);
            }));
        }

        public void ForWaitToWork(TaskVm task)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //разрешаем потоку работать следующем разом
                task.NextStart();

                WaitThreads.Remove(task);
                WorkThreads.Add(task);
            }));
        }

        public void ForWorkToWait(TaskVm task)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //останавливаем работу цыкла
                task.Stop();

                sem.Release();
                WorkThreads.Remove(task);
                WaitThreads.Add(task);
            }));
        }
    }
}
