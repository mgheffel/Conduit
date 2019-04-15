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

        
        private MainWindow v;
        public NodeCreator(int k, MainWindow a)
        {
            InitializeComponent();
            n = k;
            v = a;
            createFields();
            this.AutoSize = true;
        }

        private void createFields()
        {
            Label nodeName = new Label();
            nodeName.Text = String.Format("Node {0}", n);
            nodeName.Left = 220;
            nodeName.Top = 25;
            this.Controls.Add(nodeName);


            for (int i = 1; i <= n; i++)
            {
                //Create label
                Label label = new Label();
                label.Text = String.Format("Parameter X{0}", i);
                //Position label on screen
                label.Left = 10;
                label.Top = (i + 1) * 25;
                //Create textbox
                TextBox TextBox = new TextBox();
                //Position Textbox on screen
                TextBox.Left = 120;
                TextBox.Top = (i + 1) * 25;
                //Add controls to form
                this.Controls.Add(label);
                this.Controls.Add(TextBox);
            }

            for (int i = 1; i <= n; i++)
            {
                //Create label
                Label label = new Label();
                label.Text = String.Format("Parameter Y{0}", i);
                //Position label on screen
                label.Left = 260;
                label.Top = (i + 1) * 25;
                //Create textbox
                TextBox textBox = new TextBox();
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
            button.Top = (n + 2) * 25;
            this.Controls.Add(button);
            button.Width = 100;
            button.Enabled = true;
            button.Click += new EventHandler(this.button_Click);
           
        }
        void button_Click(object sender, EventArgs e)
        {
            var vm = v.DataContext as MainViewModel;
            vm.CreateNewNode();
            v.updateNodes();
            Close();

        }

       
    }
}
