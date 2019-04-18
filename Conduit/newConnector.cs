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
    public partial class newConnector : Form
    {
        private MainWindow v;
        public newConnector(MainWindow a)
        {
            InitializeComponent();
            v = a;
            populateFields();
        }

        public void populateFields()
        {
            List<Node> a = v.Nodes.ToList();
            int n = a.Count;
            for (int i = 0; i < n; i++)
            {
                inputBox.Items.Add(a[i].Name);
                outputBox.Items.Add(a[i].Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inputBox.SelectedItem ==null || outputBox.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
                Node a = v.Nodes[inputBox.SelectedIndex];
                Node b = v.Nodes[outputBox.SelectedIndex];
                var vm = v.DataContext as MainViewModel;
                vm.customConnector(a,b);
            }
            v.updateNodes();
            Close();
        }
    }
}
