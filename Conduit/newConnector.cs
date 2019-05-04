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
                outputBox2.Items.Add(a[i].Name);
            }

            List<Node2> b = v.Nodes2.ToList();
            int m = b.Count;
            for(int j = 0; j<m; j++)
            {
                inputBox2.Items.Add(b[j].Name);
                outputBox.Items.Add(b[j].Name);
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
                Node2 a = v.Nodes2[inputBox.SelectedIndex];
                Node b = v.Nodes[outputBox.SelectedIndex];
                var vm = v.DataContext as MainViewModel;
               
                vm.customConnectorFromData(a,b);
            }
            v.updateNodes();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (inputBox2.SelectedItem == null || outputBox2.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
                Node a = v.Nodes[inputBox2.SelectedIndex];
                Node2 b = v.Nodes2[outputBox2.SelectedIndex];
                var vm = v.DataContext as MainViewModel;

                vm.customConnectorToData(a, b);
            }
            v.updateNodes();
            Close();
        }
    }
}
