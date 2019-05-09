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
                bool flag = false;
                for (int s = b[j].InputSnaps; s < b[j].Snaps.Count; s++)
                {
                    if (b[j].Snaps[s].IsConnected == false)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    outputBox2.Items.Add(b[j].Name);
                }
                
                if (b[j].InputSnaps==1 && b[j].Snaps[0].IsConnected == false)
                {
                    inputBox.Items.Add(b[j].Name);
                }
                
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
                Node a = v.Nodes[outputBox.SelectedIndex];
                Node2 b = v.Nodes2[inputBox.SelectedIndex];

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
                foreach (var item in a.OutSnaps)
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
                a.RecalculateSnaps();

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
                Node2 a = v.Nodes2[outputBox2.SelectedIndex];
                Node b = v.Nodes[inputBox2.SelectedIndex];
                /*int n = b.Snaps.Count;
                int k = b.InputSnaps;
                for (int i = 0; i < k; i++)
                {
                    if (b.Snaps[i].IsConnected == false)
                    {
                        FromSnaps.Items.Add(b.Snaps[i].Name);
                    }


                }*/
                foreach (var item in b.InSnaps)
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
                Node a = v.Nodes[outputBox.SelectedIndex];
                Node2 b = v.Nodes2[inputBox.SelectedIndex];
                var vm = v.DataContext as MainViewModel;
                SnapSpot snapA = a.OutSnaps[ToSnaps.SelectedItem.ToString()];
                SnapSpot snapB = b.Snaps[0];
                
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
                Node2 a = v.Nodes2[outputBox2.SelectedIndex];
                Node b = v.Nodes[inputBox2.SelectedIndex];
                SnapSpot snapA = a.Snaps[0];
                for (int s = a.InputSnaps; s < a.Snaps.Count; s++)
                {
                    if (a.Snaps[s].IsConnected == false)
                    {
                        snapA = a.Snaps[s];
                        break;
                    }
                }

                
                SnapSpot snapB = b.InSnaps[FromSnaps.SelectedItem.ToString()];
                var vm = v.DataContext as MainViewModel;

                vm.customConnectorFromData(snapA, snapB);

            }
            v.updateNodes();
            Close();

        }
    }
}
