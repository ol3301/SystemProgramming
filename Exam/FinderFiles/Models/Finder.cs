using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FinderFiles
{
    public class Finder : BindableBase
    {
        
        private const string defmask = "*.txt";

        private bool isRun;
        private Thread Worker;
        private string _scanpath;
        /// <summary>
        /// Служит для "удобного" добавления событий в список Log.
        /// Не в коем случае не служит как строка
        /// </summary>
        private string log
        {
            set
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Log.Add(value);
                }));
            }
        }

        public ObservableCollection<DriveInfo> Drives { get; set; }
        public ObservableCollection<SpecificWord> Words { get; set; }
        public ObservableCollection<DetectedFile> DetectedFiles { get; set; }
        public ObservableCollection<string> Log { get; set; }
        public string NotCensorshipPath;
        public string CensorshipPath;
        public string ScanPath
        {
            get => _scanpath;
            set
            {
                _scanpath = value;
                RaisePropertyChanged();
            }
        }


        public Finder()
        {
            Worker = new Thread(WorkerMethod);
            Words = new ObservableCollection<SpecificWord>();

        }
        public void AddWord(string name)
        {
            Words.Add(new SpecificWord(name));
        }
        public bool CreateDirectories()
        {
            if (!CreateDir(NotCensorshipPath))
                return false;

            if (!CreateDir(CensorshipPath))
                return false;
            return true;
        }
        public void Start()
        {
            if (isRun)
                return;

            Worker.Start();
        }
        public void Pause()
        {
            log = "Сканирование приостановленно";
            Worker.Suspend();
        }
        public void Resume()
        {
            log = "Сканирование возобновлено";
            Worker.Resume();
        }
        public void Stop()
        {
            isRun = false;
            Log.Clear();
            Worker.Abort();

            log = "Прервано";
            ScanPath = "";
        }


        private bool CreateDir(string path)
        {
            var dirinfo = Directory.CreateDirectory(path);
            if (!dirinfo.Exists)
            {
                dirinfo.Create();
                return true;
            }
            return false;
        }
        private void GetDrives()
        {
            foreach(var drive in DriveInfo.GetDrives())
                if (drive.IsReady)
                    Drives.Add(drive);          
        }
        private DetectedFile ScanFile(DetectedFile file)
        {
            string path = file.File.DirectoryName + file.File.Name;
            string buf = GetFileLines(path);

            ScanPath = path;

            var words = GetForbiddenWords(buf);

            if (words.Count == 0)
                return null;

            foreach (var word in words)
            {
                buf=buf.Replace(word.Name,word.GenChars());
                ++word.Repetitions;
                file.AddWord(new SpecificWord(word.Name,1));
            }

            CreateFile(CensorshipPath+"\\" + file.File.Name,buf);
            File.Copy(path,NotCensorshipPath+"\\"+file.File.Name,true);

            return file;
        }
        private string GetFileLines(string path)
        {
            string buf = "";

            try
            {
                using (StreamReader reader = new StreamReader(path, Encoding.Default))
                {
                    while (reader.Peek() >= 0)
                    {
                        buf += reader.ReadLine();
                    }
                }
            }
            catch { }

            return buf;
        }
        private void WorkerMethod()
        {
            Drives = new ObservableCollection<DriveInfo>();
            Words = new ObservableCollection<SpecificWord>();
            DetectedFiles = new ObservableCollection<DetectedFile>();
            Log = new ObservableCollection<string>();
            GetDrives();

            DetectedFiles.Clear();

            log = "Сканирование запущено";

            foreach (var drive in Drives)
            {
                if (drive.Name == @"C:\")
                    continue;

                log =  $"Собираем файлы по маске с диска {drive.Name}";
                Stopwatch s = new Stopwatch();

                s.Start();
                var allfiles = GetFiles(drive.Name);
                s.Stop();

                log = $"Файлы собраны за {s.Elapsed.TotalMilliseconds} ms.";

                Thread.Sleep(1000);

                log = $"Необходимо просканировать: {allfiles.Count} файлов";
                log = "Проверка на запрещенные слова начата!";

                ScanFiles(allfiles);
            }
        }
        private void ScanFiles(List<DetectedFile> files)
        {
            foreach(var file in files)
            {
                if (file.File.Exists==false)
                    continue;

                var detected = ScanFile(file);

                if (detected == null)
                    continue;

                DetectedFiles.Add(file);
            }
        }
        private List<DetectedFile> GetFiles(string path)
        {            
            List<DetectedFile> files = new List<DetectedFile>();

            try
            {
                foreach (var fl in Directory.GetFiles(path, defmask, SearchOption.TopDirectoryOnly))
                    files.Add(new DetectedFile(fl));
            }
            catch { }

            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                    files.AddRange(GetFiles(dir));
            }
            catch { }


            return files;
        }
        private List<SpecificWord> GetForbiddenWords(string buf)
        {
            List<SpecificWord> res = new List<SpecificWord>();

            foreach (var word in Words)
            {
                Regex regex = new Regex($@"\w*{word.Name}\w*",RegexOptions.Compiled);

                foreach (Match match in regex.Matches(buf))
                    res.Add(word);
            }           

            return res;
        }
        private void CreateFile(string path,string buf)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create,FileAccess.Write))
            {
                var bb = Encoding.Default.GetBytes(buf);
                fs.Write(bb,0,bb.Length);
            }
        }
    }
}
