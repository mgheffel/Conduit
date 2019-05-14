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
    //Form used to sign into Beocat
    public partial class Beocat : Form
    {
        private SshClient ssh;
        private MainWindow m;
        public Beocat(MainWindow v)
        {
            InitializeComponent();
            m = v;

        }

        /*Button Click for login
         * Checks to see if a username and password has been entered
         * It then attempts to SSH into Beocat and throws an error if unsuccessful
         * Username and Password are then saved as private variables in the MainWindow form to be used for FTP
        */
        private void button1_Click(object sender, EventArgs e)
        {
            if (userName.Text == string.Empty || password.Text == string.Empty)
                MessageBox.Show("Login Failed. Must enter username and password.");
            else
            {
                ssh = new SshClient("headnode.beocat.ksu.edu", userName.Text, password.Text);
                if (ssh != null)
                {
                    using (ssh)
                    {

                        try
                        {
                            ssh.Connect();
                            var result = ssh.RunCommand("df -h");
                            MessageBox.Show("Successful Login");
                            m.user = userName.Text;
                            m.password = password.Text;
                            userName.Text = "";
                            password.Text = "";
                            Close();
                        }
                        catch (Renci.SshNet.Common.SshAuthenticationException)
                        {
                            MessageBox.Show("Wrong Password");
                        }

                        ssh.Disconnect();
                    }
                }

            }

        }
    }
}
