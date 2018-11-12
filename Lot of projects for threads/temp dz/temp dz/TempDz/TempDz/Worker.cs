using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TempDz
{
    public static class Worker
    {
        public static void Task1()
        {
            (new Thread((obj) =>
            {
                var coll = (object[])obj;
                foreach (var i in coll)
                    i?.ToString();
            })).Start(new object[3]);
        }
    }
}
