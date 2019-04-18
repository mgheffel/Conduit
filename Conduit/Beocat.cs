using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;

namespace Conduit
{
    public partial class Beocat : Form
    {
        private SshClient ssh;
        public Beocat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setClient();
            if (ssh != null)
                
           
            {
                using (ssh)
                {

                    try
                    {
                        ssh.Connect();
                    }
                    catch (Renci.SshNet.Common.SshAuthenticationException)
                    {
                        MessageBox.Show("Wrong Password");
                    }
                    var result = ssh.RunCommand("df -h");
                    //ssh.Disconnect();
                }
                MessageBox.Show("Successful Login");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setClient();
            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Disconnect();
                }
                MessageBox.Show("You have logged out of Beocat.");
                Close();
            }

        }

        public void setClient()
        {
            if (userName.Text == string.Empty || password.Text == string.Empty)
                MessageBox.Show("Login Failed. Must enter username and password.");
            else
            {
                ssh = new SshClient("headnode.beocat.ksu.edu", userName.Text, password.Text);
            }
        }
    }
}
