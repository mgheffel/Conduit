﻿using System;
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
        private MainWindow m;
        public Beocat(MainWindow v)
        {
            InitializeComponent();
            m = v;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (userName.Text == string.Empty || password.Text == string.Empty)
                MessageBox.Show("Login Failed. Must enter username and password.");
            else { 
                setClient(userName.Text, password.Text);
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

        private void button2_Click(object sender, EventArgs e)
        {
            setClient(m.user, m.password);
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

        public void setClient(string username, string pword)
        {
            
            
                ssh = new SshClient("headnode.beocat.ksu.edu", username, pword);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setClient(m.user, m.password);
            //MessageBox.Show(ssh.ToString());
            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    var command = ssh.CreateCommand("ls");
                    var results = command.Execute();
                    //command.Execute();
                    CommandOutput.Text = (results);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            setClient(m.user,m.password);
            //MessageBox.Show(ssh.ToString());
            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    var command = ssh.CreateCommand("cd /bulk ; ls");
                    var results = command.Execute();
                    CommandOutput.Text = (results);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CommandOutput.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            setClient(m.user, m.password);
            
            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    var command = ssh.CreateCommand("kstat --me");
                    var results = command.Execute();
                    
                    CommandOutput.Text = (results);
                }
            }
        }
    }
}
