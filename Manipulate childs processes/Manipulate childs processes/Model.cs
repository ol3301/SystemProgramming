using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manipulate_childs_processes
{
    class Model
    {
        public void FullCloseProcess(Process pr)
        {
            pr.CloseMainWindow();
            pr.Close();
        }
    }
}
