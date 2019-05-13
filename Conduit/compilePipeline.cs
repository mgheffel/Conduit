using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conduit
{
    public partial class compilePipeline : Form
    {
        MainViewModel vm;
        public compilePipeline(MainViewModel a)
        {
            InitializeComponent();
            vm = a;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            vm.compilePipeline(textBox1.Text, textBox2.Text, checkBox1.Checked);
            Close();
        }
    }
}
