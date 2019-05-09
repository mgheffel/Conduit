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
                bool flagOut = false;
                foreach (var item in a[i].OutSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        flagOut = true;
                        break;
                    }
                }
                if (flagOut)
                {
                    outputBox.Items.Add(a[i].Name);
                }
                bool flagIn = false;
                foreach (var item in a[i].InSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        flagIn = true;
                        break;
                    }
                }
                if (flagIn)
                {
                    inputBox2.Items.Add(a[i].Name);
                }
                
            }

            List<Node2> b = v.Nodes2.ToList();
            int m = b.Count;
            for (int j = 0; j < m; j++)
            {
                outputBox2.Items.Add(b[j].Name);
                
                inputBox.Items.Add(b[j].Name);
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToSnaps.Items.Clear();
            if (outputBox.SelectedItem == null || inputBox.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
                //Node a = v.Nodes[outputBox.SelectedIndex];
                //Node2 b = v.Nodes2[inputBox.SelectedIndex];
                string n1name = outputBox.SelectedItem.ToString();
                Node n1 = v.Nodes[0];
                for (int i = 0; i < v.Nodes.Count; i++)
                {
                    if (v.Nodes[i].Name == n1name)
                    {
                        n1 = v.Nodes[i];
                        break;
                    }
                }
                string n2name = inputBox.SelectedItem.ToString();
                Node2 n2 = v.Nodes2[0];
                for (int i=0; i < v.Nodes2.Count; i++)
                {
                    if (v.Nodes2[i].Name == n2name)
                    {
                        n2 = v.Nodes2[i];
                        break;
                    }
                }
                /*int n = a.Snaps.Count;
                int l = a.InputSnaps;
                int k = a.OutputSnaps;
                for (int i = l; i < n; i++)
                {
                    if (a.Snaps[i].IsConnected == false)
                    {
                        ToSnaps.Items.Add(a.Snaps[i].Name);
                    }

                }*/
                foreach (var item in n1.OutSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        ToSnaps.Items.Add(item.Value.Name);
                    }
                }
                /*foreach (var item in a.Snaps)
                {
                    item.Value.Recalculate();
                }*/
                n1.RecalculateSnaps();

                /*var vm = v.DataContext as MainViewModel;
                vm.customConnectorToData(a,b);*/
            }
            //v.updateNodes();
            //Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FromSnaps.Items.Clear();
            if (outputBox2.SelectedItem == null || inputBox2.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
                string n2name = outputBox2.SelectedItem.ToString();
                Node2 n2 = v.Nodes2[0];
                for (int i = 0; i < v.Nodes2.Count; i++)
                {
                    if (v.Nodes2[i].Name == n2name)
                    {
                        n2 = v.Nodes2[i];
                        break;
                    }
                }
                //Node b = v.Nodes[inputBox2.SelectedIndex];
                string n1name = inputBox2.SelectedItem.ToString();
                Node n1 = v.Nodes[0];
                for (int i = 0; i < v.Nodes.Count; i++)
                {
                    if (v.Nodes[i].Name == n1name)
                    {
                        n1 = v.Nodes[i];
                        break;
                    }
                }
                /*int n = b.Snaps.Count;
                int k = b.InputSnaps;
                for (int i = 0; i < k; i++)
                {
                    if (b.Snaps[i].IsConnected == false)
                    {
                        FromSnaps.Items.Add(b.Snaps[i].Name);
                    }


                }*/
                foreach (var item in n1.InSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        FromSnaps.Items.Add(item.Value.Name);
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
                //Node a = v.Nodes[outputBox.SelectedIndex];
                //Node2 b = v.Nodes2[inputBox.SelectedIndex];
                string n1name = outputBox.SelectedItem.ToString();
                Node n1 = v.Nodes[0];
                for (int i = 0; i < v.Nodes.Count; i++)
                {
                    if (v.Nodes[i].Name == n1name)
                    {
                        n1 = v.Nodes[i];
                        break;
                    }
                }
                string n2name = inputBox.SelectedItem.ToString();
                Node2 n2 = v.Nodes2[0];
                for (int i = 0; i < v.Nodes2.Count; i++)
                {
                    if (v.Nodes2[i].Name == n2name)
                    {
                        n2 = v.Nodes2[i];
                        break;
                    }
                }
                var vm = v.DataContext as MainViewModel;
                SnapSpot snapA = n1.OutSnaps[ToSnaps.SelectedItem.ToString()];
                SnapSpot snapB = n2.Snaps[0];
                
                vm.customConnectorToData(snapA, snapB);
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
                //Node2 a = v.Nodes2[outputBox2.SelectedIndex];
                //Node b = v.Nodes[inputBox2.SelectedIndex];
                string n1name = inputBox2.SelectedItem.ToString();
                Node n1 = v.Nodes[0];
                for (int i = 0; i < v.Nodes.Count; i++)
                {
                    if (v.Nodes[i].Name == n1name)
                    {
                        n1 = v.Nodes[i];
                        break;
                    }
                }
                string n2name = outputBox2.SelectedItem.ToString();
                Node2 n2 = v.Nodes2[0];
                for (int i = 0; i < v.Nodes2.Count; i++)
                {
                    if (v.Nodes2[i].Name == n2name)
                    {
                        n2 = v.Nodes2[i];
                        break;
                    }
                }
                SnapSpot snapA = n2.Snaps[1];
                

                
                SnapSpot snapB = n1.InSnaps[FromSnaps.SelectedItem.ToString()];
                var vm = v.DataContext as MainViewModel;

                vm.customConnectorFromData(snapA, snapB);

            }
            v.updateNodes();
            Close();

        }
    }
}
