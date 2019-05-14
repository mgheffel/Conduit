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
    public partial class CreateDataNode : Form
    {
        private MainWindow v;
        private int inValue;
        private int outValue;
        private int inputs;
        public CreateDataNode(MainWindow a)
        {
            InitializeComponent();
            v = a;
        }

        /*Button click to create Data Node.
         * Checks to ensure it has a name 
         * Next it reads the strings from the form to create the string [] necessary to make the data node
         * It then checks to make sure a node doesn't already exist with the same name
         * Finally, it calls the constructor to make the data node
         */
        private void button1_Click(object sender, EventArgs e)
        {
            //Node must have a name
            if (nodeName == null)
            {
                MessageBox.Show("Node must have a name");
            }
           
            else
            {
                var vm = v.DataContext as MainViewModel;
                //Create string necessary to make Data node
                var strings = Controls.OfType<TextBox>()
                          .Select(c => c.Text)
                          .ToList();
                string[] node2string = new string[5];
                node2string[4] = strings[0];
                node2string[3] = "Location";
                node2string[1] = "1";
                node2string[2] = "1";
                node2string[0] = strings[1];

                bool createNewNode2 = true;
                foreach (Node2 n2 in vm.Nodes2)
                {
                    //checks for name duplication
                    if (node2string[0] == n2.Name)
                    {
                        createNewNode2 = false;
                        MessageBox.Show("Cannot have data nodes with the same name");
                    }
                }
                //if the node is ready to be made, it calls the make node constructor
                if (createNewNode2)
                {
                    vm.CreateNewNode2(1, node2string);
                }
                v.updateNodes();
                Close();
            }
        }
    }
}

