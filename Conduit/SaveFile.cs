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
    public partial class SaveFile: Form
    {
        private MainWindow x;
        public SaveFile(MainWindow y)
        {
            InitializeComponent();
            x = y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
         if(x.Nodes.Count > 0)
            {
                string three = "";
                foreach (Connector c in x.Connectors)
                {
                    Node n = c.StartNode;
                    string a = n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                        + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString();
                    n = c.EndNode;
                    string b = n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                        + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString(); ;
                    three = three + a + "&" + b;
                    if(c != x.Connectors.Last())
                    {
                        three = three + "+";
                    }
                }
                string one = "";
                foreach (Node n in x.Nodes)
                { 
                    
                    one = one + n.Location.Value.X.ToString() +"," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                        + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString();
                    if (n != x.Nodes.Last())
                    {
                        one= one + "+";
                    }

                }
                string four = "";
                foreach (Node2 n in x.Nodes2)
                {

                    one = one + n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + ","
                        + n.T1.ToString();
                    if (n != x.Nodes2.Last())
                    {
                        one = one + "+";
                    }

                }

                string two = "\r\n";
              

                string save = one + two + three + four;

                saveFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.AddExtension = true;

           
                saveFileDialog1.ShowDialog();
                if(saveFileDialog1.FileName != null)
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
                            MessageBox.Show(nodeString[0]);
                            MessageBox.Show(nodeString[1]);
                            MessageBox.Show(nodeString[2]);
                            MessageBox.Show(strings[0]);
                            MessageBox.Show(strings[1]);
                            MessageBox.Show(strings[2]);
                            var vm = x.DataContext as MainViewModel;
                            Node n = vm.CreateNewNode(Convert.ToInt32(nodeString[2]), strings);
                            n.Location.Value = new System.Windows.Point(Convert.ToInt32(nodeString[0]), Convert.ToInt32(nodeString[1]));
                            x.updateNodes();

                        }

                        break;
                    case 2:
                        connectors = filelines[1].Split('+');
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
                            Node n = vm.CreateNewNode(Convert.ToInt32(nodeString[2]), strings);
                            n.Location.Value = new System.Windows.Point(Convert.ToInt32(nodeString[0]), Convert.ToInt32(nodeString[1]));
                            x.updateNodes();
                        }



                            for (int i = 0; i < connectors.Length; i++)
                        {
                            string[] connectorString = connectors[i].Split('&');
                            int a = 0;
                            int b = 0;
                            var vm = x.DataContext as MainViewModel;
                            string[] nodeString = connectorString[0].Split(',');
                            string[] strings = new string[(nodeString.Length - 3)];
                            for (int j = 0; j < strings.Length; j++)
                            {
                                strings[j] = nodeString[(j + 3)];
                            }
                            for (int d = 0; d < vm.Nodes.Count; d++)
                            {
                                if (vm.Nodes[d].Name.ToString() == strings[0].ToString())
                                {
                                    a = d;
                                    break;
                                }
                            }

                            nodeString = connectorString[1].Split(',');
                            strings = new string[(nodeString.Length - 3)];
                            for (int j = 0; j < strings.Length; j++)
                            {
                                strings[j] = nodeString[(j + 3)];
                            }
                            for (int d = 0; d < vm.Nodes.Count; d++)
                            {
                                if (vm.Nodes[d].Name.ToString() == strings[0].ToString())
                                {
                                    b = d;
                                    break;
                                }
                            }

                            //vm.customConnector(vm.Nodes[a], vm.Nodes[b]);
                            x.updateNodes();
                            
                        }
                        break;
                }
                
            }
            Close();

        }
       
    }
}
