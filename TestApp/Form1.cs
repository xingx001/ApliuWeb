using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //ClassA.pub.Add("fff", new ClassA("fff"));
            //MessageBox.Show(ClassA.pub["fff"].value);

            //ClassA.pub["a"] = new ClassA("ddd");
            //MessageBox.Show(ClassA.pub["a"].value);
            MessageBox.Show(ClassA.pub["a"].value);
            ClassA.set();
            MessageBox.Show(ClassA.pub["b"].value);

        }
    }
}
