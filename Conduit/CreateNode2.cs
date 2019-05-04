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
    public partial class CreateNode2 : Form
    {
        private MainWindow v;
        private int inValue;
        private int outValue;
        private int inputs;
        public CreateNode2(MainWindow a)
        {
            InitializeComponent();
            v = a;
            string[] range = new string[] { "0", "1" };
            inputSnaps.Items.AddRange(range);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inputSnaps.SelectedItem == null)
            {
                MessageBox.Show("Must Select number of Input Snaps");
            }
            else if (outputSnaps == null || !int.TryParse(outputSnaps.Text, out outValue))
            {
                MessageBox.Show("Output Snaps must be an integer value");
            }
            else if (nodeName == null)
            {
                MessageBox.Show("Node must have a name");
            }
           
            else
            {
                int.TryParse(inputSnaps.Text, out inValue);
                int.TryParse(outputSnaps.Text, out outValue);
                int.TryParse(inputSnaps.SelectedItem.ToString(), out inputs);

                var vm = v.DataContext as MainViewModel;

                var strings = Controls.OfType<TextBox>()
                          .Select(c => c.Text)
                          .ToList();
                string[] yep = new string[5];
                yep[0] = strings[0];
                yep[1] = "Location";
                yep[2] = strings[1];
                yep[3] = inputs.ToString();
                yep[4] = strings[2];
                Node2 node = vm.CreateNewNode2(1, yep); 
                v.updateNodes();
                Close();
            }
        }
    }
}
