using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_visualizer
{
    public partial class Form1 : Form
    {
        Module DrawerModule;
        object Drawer;
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Module dm, object dm1)
        {
            DrawerModule = dm;
            Drawer = dm1;

            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            DrawerModule.GetType("Drawer.Form1").GetMethod("SetText").Invoke(Drawer,new object[] { s });
        }
    }
}
