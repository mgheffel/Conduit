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
using Renci.SshNet.Sftp;
using System.IO.Compression;


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



        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string filename = "";
            string justName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            justName = Path.GetFileNameWithoutExtension(filename);
            //MessageBox.Show("Create client Object");
            using (SftpClient sftpClient = new SftpClient(getSftpConnection("headnode.beocat.ksu.edu", m.user, 22, m.password)))
            {
                //MessageBox.Show("Connect to server");
                sftpClient.Connect();
                //MessageBox.Show("Creating FileStream object to stream a file");
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    sftpClient.BufferSize = 1024;
                    sftpClient.UploadFile(fs, Path.GetFileName(filename));
                                                             /* string directory = sftpClient.WorkingDirectory;
                                                              directory += "/results.zip";
                                                              MessageBox.Show(directory);
                                                              ZipFile.ExtractToDirectory(directory, justName);*/
                }

                sftpClient.Dispose();
            }
            MessageBox.Show("File successfully added");

            SshClient ssh = new SshClient("headnode.beocat.ksu.edu", m.user, m.password);
            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    string file2upzip = "unzip " + justName + ".zip";
                    var command = ssh.CreateCommand("unzip " + justName +".zip");
                    command.Execute();
                }
            }
        }

        public static ConnectionInfo getSftpConnection(string host, string username, int port, string password)
        {
            return new PasswordConnectionInfo(host, port, username, password);
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            string startPath = @"c:\example\start";
            string zipPath = @"c:\example\result.zip";

            

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "ZIP Files (*.zip)|*.zip";
            sfd.DefaultExt = "zip";
            sfd.AddExtension = true;


            sfd.ShowDialog();
            if (sfd.FileName != null)
            {
               zipPath = sfd.FileName;
            }
            startPath = fileToZip.Text;

            ZipFile.CreateFromDirectory(startPath, zipPath);
        }
    }

}
