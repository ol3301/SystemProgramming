using System.Collections.Generic;
using System.IO;

namespace FinderFiles
{
    public class DetectedFile
    {
        public FileInfo File { get; set; }
        public List<SpecificWord> ForbiddenWords {get;set;}
        public int Count
        {
            get
            {
                int count=0;
                foreach (var i in ForbiddenWords)
                    count += i.Repetitions;
                return count;
            }
        }

        public DetectedFile(string path)
        {
            this.File = new FileInfo(path);
            ForbiddenWords = new List<SpecificWord>();
        }

        public void AddWord(SpecificWord word)
        {
            var finded = ForbiddenWords.Find(x=>x.Name == word.Name);

            if (finded == null)            
                ForbiddenWords.Add(word);
            else            
                ++finded.Repetitions;                    
        }
    }
}
