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

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            //MessageBox.Show(filename);
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
                }
                sftpClient.Dispose();
            }
            MessageBox.Show("File successfully added");
        }

        public static ConnectionInfo getSftpConnection(string host, string username, int port, string password)
        {
            return new PasswordConnectionInfo(host, port, username, password);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// Downloads a file in the desktop synchronously
            /// </summary>


            string username = m.user;
            string password = m.password;
            string locationToWrite = "";

            // Path to file on SFTP server
            string pathRemoteFile = fileToDownload.Text;//"homes/kbowers/Beocat.txt";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TXT Files (*.txt)|*.txt";
            sfd.DefaultExt = "txt";
            sfd.AddExtension = true;


            sfd.ShowDialog();
            if (sfd.FileName != null)
            {
                locationToWrite = sfd.FileName;
            }
            // Path where the file should be saved once downloaded (locally)
            string pathLocalFile = locationToWrite;//Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Download_Beocat.txt" );
            fileToDownload.Text = "";
            using (SftpClient sftp = new SftpClient("headnode.beocat.ksu.edu", username, password))
            {
                try
                {
                    sftp.Connect();

                    MessageBox.Show("Downloading {0}", pathRemoteFile);

                    using (Stream fileStream = File.OpenWrite(pathLocalFile))
                    {
                        sftp.DownloadFile(pathRemoteFile, fileStream);
                    }

                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                    MessageBox.Show("An exception has been caught " + er.ToString());
                }
            }
            MessageBox.Show("File Successfully Downloaded");
        }
    }

}
