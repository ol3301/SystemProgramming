using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsingAppDomains
{
    static class Program
    {
        static AppDomain Drawer;
        static AppDomain TextWindow;

        static Assembly DrawerAsm;
        static Assembly TextWindowAsm;

        static Form DrawerWindow;
        static Form TextWindowWnd;


        static void RunDrawer()
        {
            DrawerWindow.ShowDialog();
            AppDomain.Unload(Drawer);
        }

        static void RunVisualizer()
        {
            TextWindowWnd.ShowDialog();
            Application.Exit();
        }

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main()
        {
            Application.EnableVisualStyles();

            Drawer = AppDomain.CreateDomain("Drawer");
            TextWindow = AppDomain.CreateDomain("TextWindow");

            DrawerAsm = Drawer.Load(AssemblyName.GetAssemblyName("Drawer.exe"));
            TextWindowAsm = TextWindow.Load(AssemblyName.GetAssemblyName("Text visualizer.exe"));

            DrawerWindow = Activator.CreateInstance(DrawerAsm.GetType("Drawer.Form1")) as Form;
            TextWindowWnd = Activator.CreateInstance(TextWindowAsm.GetType("Text_visualizer.Form1"),new object[] 
            { DrawerAsm.GetModule("Drawer.exe"), DrawerWindow}) as Form;



            (new Thread(new ThreadStart(RunVisualizer))).Start();
            (new Thread(new ThreadStart(RunDrawer))).Start();

        }
    }
}
