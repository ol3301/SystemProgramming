using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace TempDz
{
    class Temp
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}     {Value}";
        }
    }
    public class Bank
    {        
        private object locker = new object();
        private const string filename = "data.txt";
        private void Saver(object _temp)
        {
            if(_temp is Temp data)
            {
                lock (locker)
                {
                    using (StreamWriter writer = new StreamWriter(filename,true))
                    {
                        writer.WriteLine(data.ToString());
                    }
                }
            }
        }

        private double _money;
        public double Money
        {
            get => _money;

            set
            {
                _money = value;
                ThreadPool.QueueUserWorkItem(Saver,new Temp {Name=nameof(Money), Value=Money.ToString() });
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                ThreadPool.QueueUserWorkItem(Saver, new Temp { Name = nameof(Name), Value = Name });
            }
        }

        private int _percent;
        public int Percent
        {
            get => _percent;
            set
            {
                _percent = value;
                ThreadPool.QueueUserWorkItem(Saver, new Temp { Name = nameof(Percent), Value = Percent.ToString() });
            }
        }
    }
}
