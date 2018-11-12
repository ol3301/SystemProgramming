using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Initiator
{
    class BindProject
    {
        Module DrawerModule;
        object DrawerWindow;

        public BindProject(Module DrawerModule, object DrawerWindow)
        {
            this.DrawerModule = DrawerModule;
            this.DrawerWindow = DrawerWindow;
        }

        public void SendToPaint(double[] weathers, string[] days)
        {
            DrawerModule.GetType("Drawer.MainWindow").GetMethod("AddData").Invoke(DrawerWindow,new object[] { weathers,days});
        }
    }
}
