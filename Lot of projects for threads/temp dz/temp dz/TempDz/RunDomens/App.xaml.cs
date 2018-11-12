using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RunDomens
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static object locker = new object();

        static AppDomain Drawer;
        static AppDomain Initiator;

        static Assembly DrawerAsm;
        static Assembly InitiatorAsm;

        static Window DrawerWindow;
        static Window InitiatorWindow;

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            Drawer = AppDomain.CreateDomain("Drawer");
            DrawerAsm = Drawer.Load(AssemblyName.GetAssemblyName("Drawer.exe"));

            Initiator = AppDomain.CreateDomain("Initiator");
            InitiatorAsm = Initiator.Load(AssemblyName.GetAssemblyName("Initiator.exe"));

            DrawerWindow = Activator.CreateInstance(DrawerAsm.GetType("Drawer.MainWindow")) as Window;
            InitiatorWindow = Activator.CreateInstance(InitiatorAsm.GetType("Initiator.MainWindow"), new object[]
{DrawerAsm.GetModule("Drawer.exe"), DrawerWindow }) as Window;



            InitiatorWindow.ShowDialog();

            DrawerWindow.ShowDialog();

            AppDomain.Unload(Drawer);

            //Thread td = new Thread(RunDrawer);
            //td.SetApartmentState(ApartmentState.STA);
            //td.Start();
            //
            //Thread ti = new Thread(RunInitiator);
            //ti.SetApartmentState(ApartmentState.STA);
            //ti.Start();
        }


        static void RunDrawer()
        {
            lock (locker)
            {
                
            }
        }


        static void RunInitiator()
        {
            lock (locker)
            {
            }
        }
    }
}
