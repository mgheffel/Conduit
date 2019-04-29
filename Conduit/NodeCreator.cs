﻿using System;
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
            Node node = vm.CreateNewNode(inValue,outValue,numFields);
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
