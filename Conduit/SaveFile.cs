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
                string two = "";
                /*foreach (Connector c in x.Connectors)
                {
                    string a = c.Start.Name.ToString();
                    string b = c.End.Name.ToString();
                    two = two + a + "," + b;
                    if(c != x.Connectors.Last())
                    {
                        two = two + "+";
                    }
                }*/
                //string one = x.Connectors.ToString();
                string one = "";
                foreach (Node n in x.Nodes)
                {
                    one = one + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                        + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString();
                    if (n != x.Nodes.Last())
                    {
                        one= one + "+";
                    }

                }
                /*foreach (Connector c in x.Connectors)
                {
                    one = one + "," + c.ToString();
                    //MessageBox.Show(k);
                }*/
                // string two = "\r\n";
                // string three =  x.Nodes.ToString();
                string three = "";
                

                string save = one + two + three;

                saveFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.AddExtension = true;

                saveFileDialog1.ShowDialog();
                System.IO.File.WriteAllText(saveFileDialog1.FileName, save);
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
                string[] nodes = filelines[0].Split('+');
                for (int i = 0; i < nodes.Length; i++)
                {
                    string[] nodeString = nodes[i].Split(',');
                    string[] strings = new string[(nodeString.Length - 1)];
                    for(int j =0; j< strings.Length; j++)
                    {
                        strings[j] = nodeString[(j + 1)];
                    }
                    MessageBox.Show(strings[1].ToString());
                    var vm = x.DataContext as MainViewModel;
                    vm.CreateNewNode(Convert.ToInt32(nodeString[0]), strings);
                    x.updateNodes();

                }
            }
            Close();   

        }
       
    }
}
