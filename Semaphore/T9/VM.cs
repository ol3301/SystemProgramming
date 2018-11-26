using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace T9
{
    class VM
    {
        private object locker = new object();
        private List<string> Dictionary;


        public ObservableCollection<string> List { get; set; }

        public string Text { get; set; }


        public VM()
        {
            List = new ObservableCollection<string>();

            Dictionary = new List<string>();


            Dictionary.AddRange(new List<string> {
                "Привет",
                "Пока",
                "Ноутбук",
                "Лол",
                "Программа",
                "C#",
                "Комната"
            });
        }

        public async void NewText(string word)
        {
            List.Clear();

            foreach (var i in await SelectionWords(word))
                List.Add(i);
        }


        private async Task<List<string>> SelectionWords(string word)
        {
            return await Task.Run(async ()=>
            {
                List<string> buf = new List<string>();

                foreach (var w in Dictionary)
                    if (await Help(word.ToLower(), w.ToLower()) == true)
                        buf.Add(w);
                return buf;
            });
        }

        private async Task<bool> Help(string word,string w_dictionary)
        {
            return await Task.Run(()=>
            {
                for (int i = 0; i < word.Length; ++i)
                {
                    if(w_dictionary.Length > i)
                    {
                        var x1 = word[i];
                        var x2 = w_dictionary[i];

                        if (x1 != x2)
                            return false;
                    }
                }
                return true;
            });
        }
    }
}
