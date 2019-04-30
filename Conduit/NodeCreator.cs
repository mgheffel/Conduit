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
        private int n;
        private int inValue;
        private int outValue;
        private int numFields;

        

        private MainWindow v;
        public NodeCreator(int k, MainWindow a)
        {
            InitializeComponent();
            n = k;
            v = a;
            string[] range = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            cb.Items.AddRange(range);
           // createFields();
            this.AutoSize = true;
        }

        private void createFields(int m)
        {
            Label nodeName = new Label();
            nodeName.Text = String.Format("Node {0}", n);
            nodeName.Left = 220;
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

            Button button = new Button();
            button.Text = String.Format("Create Node");
            button.Left = 185;
            button.Top = (m + 2) * 25;
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
            Node node = vm.CreateNewNode(inValue,outValue,numFields);
            int fields = numFields;
            switch (fields)
            {
                case 1:
                    node.V1 = strings[2];
                    node.T1 = strings[3];
                    break;
                case 2:
                    node.V1 = strings[2];
                    node.T1 = strings[4];
                    node.V2 = strings[3];
                    node.T2 = strings[5];
                    break;
                case 3:
                    node.V1 = strings[2];
                    node.T1 = strings[5];
                    node.V2 = strings[3];
                    node.T2 = strings[6];
                    node.V3 = strings[4];
                    node.T3 = strings[7];
                    break;
                case 4:
                    node.V1 = strings[2];
                    node.T1 = strings[6];
                    node.V2 = strings[3];
                    node.T2 = strings[7];
                    node.V4 = strings[4];
                    node.T4 = strings[8];
                    node.V5 = strings[5];
                    node.T5 = strings[9];
                    break;
                case 5:
                    node.V1 = strings[2];
                    node.T1 = strings[7];
                    node.V2 = strings[3];
                    node.T2 = strings[8];
                    node.V3 = strings[4];
                    node.T3 = strings[9];
                    node.V4 = strings[5];
                    node.T4 = strings[10];
                    node.V5 = strings[6];
                    node.T5 = strings[11];
                    break;
                case 6:
                    node.V1 = strings[2];
                    node.T1 = strings[8];
                    node.V2 = strings[3];
                    node.T2 = strings[9];
                    node.V4 = strings[4];
                    node.T4 = strings[10];
                    node.V5 = strings[5];
                    node.T5 = strings[11];
                    node.V6= strings[6];
                    node.T6 = strings[12];
                    node.V7 = strings[7];
                    node.T7 = strings[13];
                    break;
                case 7:
                    node.V1 = strings[2];
                    node.T1 = strings[9];
                    node.V2 = strings[3];
                    node.T2 = strings[10];
                    node.V4 = strings[4];
                    node.T4 = strings[11];
                    node.V5 = strings[5];
                    node.T5 = strings[12];
                    node.V6 = strings[6];
                    node.T6 = strings[13];
                    node.V7 = strings[7];
                    node.T7 = strings[14];
                    node.V9 = strings[8];
                    node.T9 = strings[15];
                    break;
                case 8:
                    node.V1 = strings[2];
                    node.T1 = strings[10];
                    node.V2 = strings[3];
                    node.T2 = strings[11];
                    node.V3 = strings[4];
                    node.T3 = strings[12];
                    node.V4 = strings[5];
                    node.T4 = strings[13];
                    node.V5 = strings[6];
                    node.T5 = strings[14];
                    node.V6 = strings[7];
                    node.T6 = strings[15];
                    node.V7 = strings[8];
                    node.T7 = strings[16];
                    node.V8 = strings[9];
                    node.T8 = strings[17];
                    break;
                case 9:
                    node.V1 = strings[2];
                    node.T1 = strings[11];
                    node.V2 = strings[3];
                    node.T2 = strings[12];
                    node.V3 = strings[4];
                    node.T3 = strings[13];
                    node.V4 = strings[5];
                    node.T4 = strings[14];
                    node.V5 = strings[6];
                    node.T5 = strings[15];
                    node.V6 = strings[7];
                    node.T6 = strings[16];
                    node.V7 = strings[8];
                    node.T7 = strings[17];
                    node.V8 = strings[9];
                    node.T8 = strings[18];
                    node.V9 = strings[10];
                    node.T9 = strings[19];
                    break;
                default:
                    node.V1 = strings[2];
                    node.T1 = strings[12];
                    node.V2 = strings[3];
                    node.T2 = strings[13];
                    node.V3 = strings[4];
                    node.T3 = strings[14];
                    node.V4 = strings[5];
                    node.T4 = strings[15];
                    node.V5 = strings[6];
                    node.T5 = strings[16];
                    node.V6 = strings[7];
                    node.T6 = strings[17];
                    node.V7 = strings[8];
                    node.T7 = strings[18];
                    node.V8 = strings[9];
                    node.T8 = strings[19];
                    node.V9 = strings[10];
                    node.T9 = strings[20];
                    node.V10 = strings[11];
                    node.T10 = strings[21];
                    break;
            }
            v.updateNodes();
            Close();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if(cb.SelectedItem == null)
            {
                MessageBox.Show("Must Select Number of Fields");
            }
            else if(inputValue == null || !int.TryParse(inputValue.Text, out inValue))
            {
                MessageBox.Show("Input Snaps must be an integer value");
            }
            else if (outputValue == null || !int.TryParse(outputValue.Text, out outValue))
            {
                MessageBox.Show("Output Snaps must be an integer value");
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
