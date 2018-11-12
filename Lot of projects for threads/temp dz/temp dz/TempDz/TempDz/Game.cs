using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TempDz
{
    class Game
    {
        public void Start()
        {
            Thread thr = new Thread(()=>
            {
                Random rnd = new Random();
                Thread.Sleep(rnd.Next(5000));

                Stopwatch sp = new Stopwatch();

                Console.WriteLine("ТЫЦ!");
                sp.Start();
                Console.ReadKey();
                sp.Stop();

                Console.WriteLine("\nПрошло времени: " + sp.Elapsed.TotalMilliseconds + "   mls");
            });
            thr.Start();
            thr.Join();
        }
    }
}
