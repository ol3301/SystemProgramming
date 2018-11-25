using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinderFiles
{
    public class SpecificWord
    {
        public string Name { get; set; }
        public int Repetitions { get; set; }

        public SpecificWord(string Name, int Repetitions=0)
        {
            this.Name = Name;
            this.Repetitions = Repetitions;
        }

        public string GenChars()
        {
            string s = "";
            foreach (var i in Name)
                s += '*';
            return s;
        }
    }
}
