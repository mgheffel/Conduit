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
using FluentFTP;
using System.Net;
using System.IO;

namespace Conduit
{
    public partial class FTP : Form
    {
        private MainWindow m;
        public FTP(MainWindow v)
        { 
            InitializeComponent();
            m = v;
        }

        public void Send(string fileName, string path)
        {
            /*using (FtpClient conn = new FtpClient())
            {
                conn.Host = "headnode.beocat.ksu.edu";
                conn.Credentials = new NetworkCredential(m.user, m.password);
                conn.Connect();

               //conn.RetryAttempts = 3;
                //conn.UploadFile(@path, "/homes/kbowers/", FtpExists.Overwrite, false, FtpVerify.Retry); // /homes/kbowers
                conn.CreateDirectory("/homes/kbowers/SeniorDesign");
            }*/

        }
        private void button1_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            string path =  Path.GetDirectoryName(fd.FileName);
            path += fd.FileName;
            MessageBox.Show(path);
            Send(fd.FileName, path);     */
        }
       
    }

}
