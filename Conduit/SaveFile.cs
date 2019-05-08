using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Conduit
{
    public partial class SaveFile : Form
    {
        private MainWindow x;
        public SaveFile(MainWindow y)
        {
            InitializeComponent();
            x = y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (x.Nodes.Count > 0 || x.Nodes2.Count > 0)
            {
                string three = "";
                foreach (Connector c in x.Connectors)
                {
                    if (c.StartNode == null)
                    {
                        string conString = "N2,";
                        conString += c.StartNode2.Name+',';
                        conString += "0,";
                        conString += c.EndNode.Name + ',';
                        conString += c.End.Name;
                        three +=conString;
                    }
                    else
                    {
                        string conString = "N1,";
                        conString += c.StartNode.Name + ',';
                        conString += c.Start.Name + ',';
                        conString += c.EndNode2.Name + ',';
                        conString += "0";
                        three +=conString;
                    }
                    if (c != x.Connectors.Last())
                    {
                        three = three + "+";
                    }
                }
                string one = "";
                foreach (Node n in x.Nodes)
                {

                    one = one + n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                        + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString();

                    foreach (var item in n.InSnaps)
                    {
                        one = one + "," + item.Value.Name.ToString();
                    }
                    foreach (var item in n.OutSnaps)
                    {
                        one = one + "," + item.Value.Name.ToString();
                    }
                    if (n != x.Nodes.Last())
                    {
                        one = one + "+";
                    }

                }

                string four = "";
                foreach (Node2 n in x.Nodes2)
                {

                    four = four + n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + ","
                        + n.T1.ToString();
                    if (n != x.Nodes2.Last())
                    {
                        four = four + "+";
                    }

                }

                string two = "\r\n";
                string save = "";
                if (four == "")
                    save = one;
                else if (three == "")
                    save = one + two + four;
                else
                    save = one + two + four + two + three;


                saveFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.AddExtension = true;


                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != null)
                {
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, save);
                }
            }
            else
            {
                MessageBox.Show("Need at least one node to save layout");
            }
            Close(); ;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string[] filelines = File.ReadAllLines(filename);
                string[] connectors;
                string[] nodes;
                string[] nodes2;

                switch (filelines.Count())
                {
                    case 1:
                        nodes = filelines[0].Split('+');
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            string[] nodeString = nodes[i].Split(',');
                            string[] strings = new string[(nodeString.Length - 3)];
                            for (int j = 0; j < strings.Length; j++)
                            {
                                strings[j] = nodeString[(j + 3)];
                            }

                            var vm = x.DataContext as MainViewModel;
                            Node n = vm.CreateNewNode(Convert.ToInt32(nodeString[2]), 10, strings);
                            n.Location.Value = new System.Windows.Point(Convert.ToInt32(nodeString[0]), Convert.ToInt32(nodeString[1]));
                            vm.viewNodes(n);
                            x.updateNodes();

                        }

                        break;
                    case 2:
                        nodes = filelines[0].Split('+');
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            string[] nodeString = nodes[i].Split(',');
                            string[] strings = new string[(nodeString.Length - 3)];
                            for (int j = 0; j < strings.Length; j++)
                            {
                                strings[j] = nodeString[(j + 3)];
                            }

                            var vm = x.DataContext as MainViewModel;
                            Node n = vm.CreateNewNode(Convert.ToInt32(nodeString[2]), 10, strings);
                            n.Location.Value = new System.Windows.Point(Convert.ToInt32(nodeString[0]), Convert.ToInt32(nodeString[1]));
                            vm.viewNodes(n);
                            x.updateNodes();
                        }
                        nodes2 = filelines[1].Split('+');
                        for (int i = 0; i < nodes2.Length; i++)
                        {
                            string[] node2String = nodes2[i].Split(',');
                            string[] strings2 = new string[(node2String.Length - 3)];
                            for (int j = 0; j < strings2.Length; j++)
                            {
                                strings2[j] = node2String[(j + 3)];
                            }

                            var vm = x.DataContext as MainViewModel;
                            Node2 n2 = vm.CreateNewNode2(Convert.ToInt32(node2String[2]), strings2);
                            n2.Location.Value = new System.Windows.Point(Convert.ToInt32(node2String[0]), Convert.ToInt32(node2String[1]));
                            x.updateNodes();
                        }
                        break;

                    case 3:
                        connectors = filelines[2].Split('+');
                        nodes = filelines[0].Split('+');
                        nodes2 = filelines[1].Split('+');


                        for (int i = 0; i < nodes.Length; i++)
                        {
                            string[] nodeString = nodes[i].Split(',');
                            string[] strings = new string[(nodeString.Length - 3)];
                            for (int j = 0; j < strings.Length; j++)
                            {
                                strings[j] = nodeString[(j + 3)];
                            }

                            var vm = x.DataContext as MainViewModel;
                            Node n = vm.CreateNewNode(Convert.ToInt32(nodeString[2]), 10, strings);
                            n.Location.Value = new System.Windows.Point(Convert.ToInt32(nodeString[0]), Convert.ToInt32(nodeString[1]));
                            vm.viewNodes(n);
                            x.updateNodes();
                        }
                        for (int i = 0; i < nodes2.Length; i++)
                        {
                            string[] node2String = nodes2[i].Split(',');
                            string[] strings2 = new string[(node2String.Length - 3)];
                            for (int j = 0; j < strings2.Length; j++)
                            {
                                strings2[j] = node2String[(j + 3)];
                            }

                            var vm = x.DataContext as MainViewModel;
                            Node2 n2 = vm.CreateNewNode2(Convert.ToInt32(node2String[2]), strings2);
                            n2.Location.Value = new System.Windows.Point(Convert.ToInt32(node2String[0]), Convert.ToInt32(node2String[1]));
                            x.updateNodes();
                        }


                        for (int i = 0; i < connectors.Length; i++)
                        {
                            var vm = x.DataContext as MainViewModel;
                            string[] conSplit = connectors[i].Split(',');
                            if (conSplit[0] == "N2")
                            {
                                Node2 n2=vm.Nodes2[0];
                                for (int n = 0; n < vm.Nodes2.Count; n++)
                                {
                                    if (vm.Nodes2[n].Name == conSplit[1])
                                    {
                                        n2 = vm.Nodes2[n];
                                        break;
                                    }
                                }
                                SnapSpot snapA = n2.Snaps[0];
                                Node n1 = vm.Nodes[0];
                                for (int n = 0; n < vm.Nodes.Count; n++)
                                {
                                    if (vm.Nodes[n].Name == conSplit[3])
                                    {
                                        n1 = vm.Nodes[n];
                                        break;
                                    }
                                }
                                SnapSpot snapB = n1.InSnaps[conSplit[4]];
                                vm.customConnectorFromData(snapA, snapB);
                            }
                            else
                            {
                                
                                Node n1 = vm.Nodes[0];
                                for (int n = 0; n < vm.Nodes.Count; n++)
                                {
                                    if (vm.Nodes[n].Name == conSplit[1])
                                    {
                                        n1 = vm.Nodes[n];
                                        break;
                                    }
                                }
                                SnapSpot snapA = n1.OutSnaps[conSplit[2]];
                                Node2 n2 = vm.Nodes2[0];
                                for (int n = 0; n < vm.Nodes2.Count; n++)
                                {
                                    if (vm.Nodes2[n].Name == conSplit[3])
                                    {
                                        n2 = vm.Nodes2[n];
                                        break;
                                    }
                                }
                                SnapSpot snapB = n2.Snaps[0];
                                for (int s = n2.InputSnaps; s < n2.Snaps.Count; s++)
                                {
                                    if (n2.Snaps[s].IsConnected == false)
                                    {
                                        snapB = n2.Snaps[s];
                                    }
                                }
                                
                                vm.customConnectorToData(snapA, snapB);
                            }
                            x.updateNodes();

                        }
                        break;
                }

            }
            Close();

        }

    }
}