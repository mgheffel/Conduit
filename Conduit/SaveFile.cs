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
    //Form to open/save a file
    public partial class SaveFile : Form
    {
        private MainWindow x;
        public SaveFile(MainWindow y)
        {
            InitializeComponent();
            x = y;
        }
        /*Button Click method to save a file
         * It first writes all the relevant information to a string needed to recreated the connectors upon loading the file
         * Then it writes all the relevant information to a string for the Software Nodes
         * Next it writes all the relevant information to a string for the Data Nodes
         * Then it joins these strings separated by new line characters into the string that will be written to the file
         * Lastly it uses a save file dialog to write the given string 
         */
        private void button1_Click(object sender, EventArgs e)
        {

            if (x.Nodes.Count > 0 || x.Nodes2.Count > 0)
            {
                //string that will hold all of the connectors and will be added to the large string third
                string three = "";
                //writing the relevant connector information
                foreach (Connector c in x.Connectors)
                {
                    if (c.StartNode == null)
                    {
                        string conString = "N2,";
                        conString += c.StartNode2.Name + ',';
                        conString += "0,";
                        conString += c.EndNode.Name + ',';
                        conString += c.End.Name;
                        three += conString;
                    }
                    else
                    {
                        string conString = "N1,";
                        conString += c.StartNode.Name + ',';
                        conString += c.Start.Name + ',';
                        conString += c.EndNode2.Name + ',';
                        conString += "0";
                        three += conString;
                    }
                    if (c != x.Connectors.Last())
                    {
                        three = three + "+";
                    }
                }
                //string that will hold all of the Software Nodes and will be added to the large string first
                string one = "";
                //writing relevant software node information
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
                //string that will hold all of the data nodes and will be added to the large string second
                string two = "";

                //writing relevant data node information
                foreach (Node2 n in x.Nodes2)
                {

                    two = two + n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + "1" + "," + "1" + "," + n.V1.ToString() + ","
                        + n.T1.ToString();
                    if (n != x.Nodes2.Last())
                    {
                        two = two + "+";
                    }

                }
                //Creating the joint string that will be saved based on what elements exist
                string newline = "\r\n";
                string save = "";
                if (two == "")
                    save = one;
                else if (three == "")
                    save = one + newline + two;
                else
                    save = one + newline + two + newline + three;

                //Save File Dialog to get the name and location of the file to write the string to
                saveFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != null)
                {
                    //Write the string
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, save);
                }
            }
            //Need at least one Software Node or Data Node to save
            else
            {
                MessageBox.Show("Need at least one node to save layout");
            }
            Close();

        }

        /*Button Click method to open a saved file
         * It first uses an Open File Dialog to get the file. It then reads all the lines and creates string[] for connectors, software nodes, and data nodes.
         * Next it uses a switch case based on the number of file lines to  determine which elements exist in the save file
         * Lastly, based on the case, it reads in the relevant information for each data structure and adds them to the screen.
         */
        private void button2_Click(object sender, EventArgs e)
        {
            //clear screen before loading
            x.clearScreen();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //create variables based on the file read in.
                string filename = openFileDialog1.FileName;
                string[] filelines = File.ReadAllLines(filename);
                string[] connectors;
                string[] nodes;
                string[] nodes2;
                //Switch case that is used to determine which elements need to be created and added
                switch (filelines.Count())
                {
                    //Just Software Nodes
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
                    //Software and Data Nodes
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
                    //Software Nodes, Data Nodes, and Connectors
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
                                Node2 n2 = vm.Nodes2[0];
                                for (int n = 0; n < vm.Nodes2.Count; n++)
                                {
                                    if (vm.Nodes2[n].Name == conSplit[1])
                                    {
                                        n2 = vm.Nodes2[n];
                                        break;
                                    }
                                }
                                SnapSpot snapA = n2.Snaps[1];
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