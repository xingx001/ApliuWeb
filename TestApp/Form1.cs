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
        private readonly static string RequestJson = @"{
                    ""ext"": ""{0}"",
                    ""extend"": ""{1}"",
                    ""msg"": ""{2}"",
                    ""sig"": ""{3}"",
                    ""tel"": {
                        ""mobile"": ""{4}"",
                        ""nationcode"": ""{5}""
                    },
                    ""time"": {6},
                    ""type"": {7}
                }";

        private readonly static string a1 = @"{{
                    ""ext"": ""{0}"",
                    ""extend"": ""{1}"",
                    ""msg"": ""{2}"",
                    ""sig"": ""{3}"",
                    ""tel"": {{
                        ""mobile"": ""{4}"",
                        ""nationcode"": ""{5}""
                    }},
                    ""time"": {6},
                    ""type"": {7}
                }}";
        private void button1_Click(object sender, EventArgs e)
        {
            string json = string.Format(RequestJson, "1", "2", "3", "4", "5", "6", "7", "8");

            MessageBox.Show(json);
        }
    }
}
