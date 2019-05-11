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
    public partial class NodeCreator : Form
    {
        //private int n;
        private int inValue;
        private int outValue;
        private int numFields;
        private MainWindow v;

        public NodeCreator(MainWindow a)
        {
            InitializeComponent();
           // n = k;
            v = a;
            string[] range = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            cb.Items.AddRange(range);
           // createFields();
            this.AutoSize = true;
        }

        private void createFields(int m)
        {
            var strings = Controls.OfType<TextBox>()
                      .Select(c => c.Text)
                      .ToList();
            Label nodeName = new Label();
            nodeName.Text = String.Format(strings[0]);
            nodeName.Left = 480;
            nodeName.Top = 25;
            this.Controls.Add(nodeName);


            for (int i = 1; i <= m; i++)
            {
                //Create label
                Label label = new Label();
                label.Text = String.Format("Parameter Name {0}", i);
                label.AutoSize = true;
                //Position label on screen
                label.Left = 10;
                label.Top = (i + 1) * 25;
                //Create textbox
                TextBox TextBox = new TextBox();
                TextBox.Name = String.Format("Name {0}", i);
                //Position Textbox on screen
                TextBox.Left = 120;
                TextBox.Top = (i + 1) * 25;
                //Add controls to form
                this.Controls.Add(label);
                this.Controls.Add(TextBox);
            }

            for (int i = 1; i <= m; i++)
            {
                //Create label
                Label label = new Label();
                label.Text = String.Format("Parameter Value {0}", i);
                //Position label on screen
                label.Left = 260;
                label.Top = (i + 1) * 25;
                label.AutoSize = true;
                //Create textbox
                TextBox textBox = new TextBox();
                textBox.Name = "Value{0}" + i;
                //Position textbox on screen
                textBox.Left = 370;
                textBox.Top = (i + 1) * 25;
                //Add controls to form
                this.Controls.Add(label);
                this.Controls.Add(textBox);
               
            }
            for (int i = 1; i <= inValue; i++)
            {
                //Create label
                Label label = new Label();
                label.Text = String.Format("Input Snap {0} Name", i);
                label.AutoSize = true;
                //Position label on screen
                label.Left = 520;
                label.Top = (i + 1) * 25;
                //Create textbox
                TextBox TextBox = new TextBox();
                TextBox.Name = String.Format("Name {0}", i);
                //Position Textbox on screen
                TextBox.Left = 630;
                TextBox.Top = (i + 1) * 25;
                //Add controls to form
                this.Controls.Add(label);
                this.Controls.Add(TextBox);
            }

            for (int i = 1; i <= outValue; i++)
            {
                //Create label
                Label label = new Label();
                label.Text = String.Format("Output Snap {0} Name", i);
                //Position label on screen
                label.Left = 770;
                label.Top = (i + 1) * 25;
                label.AutoSize = true;
                //Create textbox
                TextBox textBox = new TextBox();
                textBox.Name = "Name{0}" + i;
                //Position textbox on screen
                textBox.Left = 890;
                textBox.Top = (i + 1) * 25;
                //Add controls to form
                this.Controls.Add(label);
                this.Controls.Add(textBox);

            }

            Button button = new Button();
            button.Text = String.Format("Create Node");
            button.Left = 440;
            int max = Math.Max(m, Math.Max(inValue, outValue));
            button.Top = (max + 2) * 25;
            this.Controls.Add(button);
            button.Width = 100;
            button.Enabled = true;
            button.Click += new EventHandler(this.button_Click);
           
        }
        void button_Click(object sender, EventArgs e)
        {
            var vm = v.DataContext as MainViewModel;

            var strings = Controls.OfType<TextBox>()
                      .Select(c => c.Text)
                      .ToList();
            string[] yep = new string[strings.Count];
            for (int i = 0; i < strings.Count; i++)
            {
                yep[i] = strings[i];
            }

            
            bool addToDictionary = true;
            foreach (var item in vm.NodeSkeletons)
            {
                if (item.Key== yep[0])
                {
                    addToDictionary = false;
                    MessageBox.Show("Cannot create a node with the same name as a previously created node");
                    break;
                }
            }
            if (addToDictionary)
            {
                Node node = vm.CreateNewNode(numFields, numFields, yep);
                vm.NodeSkeletons.Add(node.Name, v.writeNode(node).Split(','));
            }
            v.updateNodes();
            Close();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (cb.SelectedItem == null)
            {
                MessageBox.Show("Must Select Number of Fields");
            }
            else if (inputValue == null || !int.TryParse(inputValue.Text, out inValue))
            {
                MessageBox.Show("Input Snaps must be an integer value");
            }
            else if (outputValue == null || !int.TryParse(outputValue.Text, out outValue))
            {
                MessageBox.Show("Output Snaps must be an integer value");
            }
            else if (name == null || name.Text.Contains('-') == true || name.Text.Contains(' ')) 
            {
                MessageBox.Show("Node must have a name without hypens or spaces");
            }
            else
            {
                int.TryParse(inputValue.Text, out inValue);
                int.TryParse(outputValue.Text, out outValue);
                int.TryParse(cb.SelectedItem.ToString(), out numFields);
                createFields(numFields);
            }
        }
    }
}
