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

        /*This method is used to populate the available fields for Software and Data Nodes
         * It ensures that the nodes present have available connectors.
         */
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

        //Button click for populating the available SnapSpot names for the selected software node outputs
        private void button1_Click(object sender, EventArgs e)
        {
            ToSnaps.Items.Clear();
            if (outputBox.SelectedItem == null || inputBox.SelectedItem == null)
            {
                MessageBox.Show("Must make an input and output selection for connector");
            }
            else
            {
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
                foreach (var item in n1.OutSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        ToSnaps.Items.Add(item.Value.Name);
                    }
                }
                n1.RecalculateSnaps();
            }
        }
        //Button click for populating the available SnapSpot names for the selected software node inputs
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
                foreach (var item in n1.InSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        FromSnaps.Items.Add(item.Value.Name);
                    }
                }
            }
        }
        //Button click to create a connector from the selected software node snap to the selected data node
        private void createToConnector_Click(object sender, EventArgs e)
        {
            if (ToSnaps.SelectedItem == null)
            {
                MessageBox.Show("Must Select a Snap");
            }
            else
            {
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
                
                //Constructor to make the connector
                vm.customConnectorToData(snapA, snapB);
            }
            v.updateNodes();
            Close();

        }
        //Button click to create a connector from the selected data node to the selected software node snap
        private void createFromConnector_Click(object sender, EventArgs e)
        {
            if (FromSnaps.SelectedItem == null)
            {
                MessageBox.Show("Must Select a Snap");
            }
            else
            {
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
                //Constructor to make the connector
                vm.customConnectorFromData(snapA, snapB);

            }
            v.updateNodes();
            Close();

        }
    }
}
