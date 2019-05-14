using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Conduit
{
    public partial class CheckProgress : Form
    {
        private MainWindow m = new MainWindow();
        public CheckProgress(MainWindow v)
        {
            InitializeComponent();
            m = v;
            runCommand();
        }
        //Connects to Beocat through ssh and then runs a kstat --me command and shows the results in the texbox
        public void runCommand()
        {
            SshClient ssh = new SshClient("headnode.beocat.ksu.edu", m.user, m.password);

            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    var command = ssh.CreateCommand("kstat --me");
                    var results = command.Execute();
                    ssh.Disconnect();
                    kstatProgress.Text = results;
                }
            }
        }
    }
}
