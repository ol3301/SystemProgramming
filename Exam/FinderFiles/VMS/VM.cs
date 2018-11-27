using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinderFiles
{
    public class VM : BindableBase
    {
        #region Модель
        private Finder finder = new Finder();

        private DelegateCommand _start;
        private DelegateCommand _stop;
        private DelegateCommand _pause;
        private DelegateCommand _resume;

        public ObservableCollection<DriveInfo> Drives           => finder.Drives;
        public ObservableCollection<SpecificWord> Words         => finder.Words;
        public ObservableCollection<DetectedFile> DetectedFiles => finder.DetectedFiles;
        public ObservableCollection<string> Log                 => finder.Log;
        public string ScanPath                                  => finder.ScanPath;
        public int Pos                                          => finder.Pos;
        public int Max                                          => finder.Max;


        public DelegateCommand Start
        {
            get => _start ?? (_start = new DelegateCommand(() =>
            {
                finder.Start();
            }));
        }
        public DelegateCommand Stop
        {
            get => _stop ?? (_stop = new DelegateCommand(()=>
            {
                finder.Stop();
            }));
        }
        public DelegateCommand Pause
        {
            get => _pause ?? (_pause = new DelegateCommand(()=>
            {
                finder.Pause();
            }));
        }
        public DelegateCommand Resume
        {
            get => _resume ?? (_resume = new DelegateCommand(()=>
            {
                finder.Resume();
            }));
        }
        #endregion

        #region Вьюшка
        private bool _isStart;
        private DelegateCommand _showMenu;
        private DelegateCommand _chooseCensorship;
        private DelegateCommand _chooseNotCensorship;
        private DelegateCommand _saveToFile;

        public DelegateCommand<string> AddWordCommand { get; set; }

        public DelegateCommand ChooseNotCensorship
        {
            get => _chooseNotCensorship ?? (_chooseNotCensorship = new DelegateCommand(()=>
            {
                using (FolderBrowserDialog sfd = new FolderBrowserDialog())
                    if (sfd.ShowDialog() == DialogResult.OK)
                        NotCensorshipPath = sfd.SelectedPath;
            }));
        }
        public DelegateCommand ChooseCensorship
        {
            get => _chooseCensorship ?? (_chooseCensorship = new DelegateCommand(()=>
            {
                using (FolderBrowserDialog sfd = new FolderBrowserDialog())
                    if (sfd.ShowDialog() == DialogResult.OK)
                        CensorshipPath = sfd.SelectedPath;
            }));
        }
        public DelegateCommand ShowMenu
        {
            get => _showMenu ?? (_showMenu = new DelegateCommand(()=>
            {               
                //если true то ставим false. И наоборот
                IsStart = IsStart ? false : true;
            }));
        }
        public DelegateCommand SaveToFile
        {
            get => _saveToFile ?? (_saveToFile = new DelegateCommand(() =>
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                    if (fbd.ShowDialog() == DialogResult.OK)
                        finder.SaveToFile(fbd.SelectedPath);
            }));
        }

        //анимируем меню слева(выдвигается)
        public bool IsStart
        {
            get => _isStart;
            set
            {
                _isStart = value;
                RaisePropertyChanged();
            }
        }

        public string CensorshipPath
        {
            get => finder.CensorshipPath;
            set
            {
                finder.CensorshipPath = value;
                RaisePropertyChanged();
            }
        }
        public string NotCensorshipPath
        {
            get => finder.NotCensorshipPath;
            set
            {
                finder.NotCensorshipPath = value;
                RaisePropertyChanged();
            }
        }

        #endregion  


        public VM()
        {
            //пробрасываем события модели
            finder.PropertyChanged += (s, e) => RaisePropertyChanged(e.PropertyName);
            AddWordCommand = new DelegateCommand<string>(AddWord);
        }


        private void AddWord(string name) => finder.AddWord(name);
    }
}
