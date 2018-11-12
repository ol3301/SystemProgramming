using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace TreeProcesses
{
    public static class ProcessExtensions
    {
        public static int ParentId(this Process process)
        {
            int id = 0;

            using (ManagementObject obj = new ManagementObject("win32_process.handle="+process.Id))
            {
                try
                {
                    obj.Get();

                    id = int.Parse(obj["ParentProcessId"].ToString());
                }
                catch { }
            }

            return id;
        }

        public static List<Process> GetChildProcesses(this Process parent)
        {
            var processes = Process.GetProcesses();

            List<Process> res = new List<Process>();

            foreach (var proc in processes)
                if (parent.Id == proc.ParentId())
                    res.Add(proc);

            return res;
        }
    }
}
