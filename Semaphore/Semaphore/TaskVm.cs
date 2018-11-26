using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Semaphore
{
    public class TaskVm
    {
        private CancellationTokenSource sorc = new CancellationTokenSource();
        public CancellationToken token => sorc.Token;

        public Task Thread { get; set; }
        public int Id { get; set; } 
        public int Count { get; set; }

        public TaskVm()
        {
            Count = 0;
        }

        public void Start()
        {
            Thread.Start();
        }

        public void Stop()
        {
            sorc.Cancel();
        }

        public void NextStart()
        {
            sorc = new CancellationTokenSource();
        }
    }
}
