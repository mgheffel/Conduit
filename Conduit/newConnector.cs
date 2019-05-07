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
                outputBox.Items.Add(a[i].Name);
                inputBox2.Items.Add(a[i].Name);
            }

            List<Node2> b = v.Nodes2.ToList();
            int m = b.Count;
            for(int j = 0; j<m; j++)
            {
                outputBox2.Items.Add(b[j].Name);
                inputBox.Items.Add(b[j].Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (outputBox.SelectedItem ==null || inputBox.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
                Node a = v.Nodes[outputBox.SelectedIndex];
                Node2 b = v.Nodes2[inputBox.SelectedIndex];

                int n = a.Snaps.Count;
                int l = a.InputSnaps;
                int k = a.OutputSnaps;
                for (int i = k; i < n; i++)
                {
                    if (a.Snaps[i].IsConnected == false)
                    {
                        ToSnaps.Items.Add(a.Snaps[i].Name);
                    }
                    
                }
                
                /*var vm = v.DataContext as MainViewModel;
                vm.customConnectorToData(a,b);*/
            }
            //v.updateNodes();
            //Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (outputBox2.SelectedItem == null || inputBox2.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
                Node2 a = v.Nodes2[outputBox2.SelectedIndex];
                Node b = v.Nodes[inputBox2.SelectedIndex];
                int n = b.Snaps.Count;
                int k = b.InputSnaps;
                for (int i = 0; i < k; i++)
                {
                    if(b.Snaps[i].IsConnected == false)
                    {
                        FromSnaps.Items.Add(b.Snaps[i].Name);
                    }
                    

                }
                /*var vm = v.DataContext as MainViewModel;

                vm.customConnectorFromData(a,b);*/
            }
           // v.updateNodes();
            //Close();
        }

        private void createToConnector_Click(object sender, EventArgs e)
        {
            if (ToSnaps.SelectedItem == null)
            {
                MessageBox.Show("Must Select a Snap");
            }
            else
            {
                Node a = v.Nodes[outputBox.SelectedIndex];
                Node2 b = v.Nodes2[inputBox.SelectedIndex];
                int k = a.OutputSnaps;
                SnapSpot x;
                var vm = v.DataContext as MainViewModel;
                x = a.Snaps[ToSnaps.SelectedIndex];
                int pass = 0;
                for (int i = k; i < a.Snaps.Count; i++)
                {
                    if (x == a.Snaps[i])
                    {
                        pass = i;
                        break;
                    }
                }
                MessageBox.Show(pass.ToString());
                vm.customConnectorToData(a, b, pass);
            }
        v.updateNodes();
        Close();

    }

    private void createFromConnector_Click(object sender, EventArgs e)
        {
            if (FromSnaps.SelectedItem == null)
            {
                MessageBox.Show("Must Select a Snap");
            }
            else
            {
                Node2 a = v.Nodes2[outputBox2.SelectedIndex];
                Node b = v.Nodes[inputBox2.SelectedIndex];
                SnapSpot x;
                int k = b.InputSnaps;
                var vm = v.DataContext as MainViewModel;
                x = b.Snaps[FromSnaps.SelectedIndex];
                int pass = 0;
                int add = 0;
                
                //BROKEN HERE
                for (int i = 0; i <k; i++)
                {
                    if (x.Name == b.Snaps[i].Name)
                    {
                        pass = i;
                        break;
                    }
                    else if (b.Snaps[i].IsConnected)
                    {
                        add += 1;
                    }
                }
                pass += add;
                

                MessageBox.Show(pass.ToString());
                vm.customConnectorFromData(a, b, pass);
            }
            v.updateNodes();
            Close();

        }
    }
}
