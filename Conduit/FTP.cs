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


        /*Button click to send a zipped file to Beocat
         * Once clicked an open file dialog appears to select the file to send
         * Then a sftp client is created and then it is used to connect to the Beocat server
         * Next a FileStream object is used to stream the file and upload it to Beocat.
         * A messagebox then shows the file was added successfully
         * Last the file is unzipped in Beocat using ssh and the unzip command
         */
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string filename = "";
            string justName = "";
            //Get file name to upload
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            justName = Path.GetFileNameWithoutExtension(filename);
            //Create client Object
            using (SftpClient sftpClient = new SftpClient(getSftpConnection("headnode.beocat.ksu.edu", m.user, 22, m.password)))
            {
                //Connect to server
                sftpClient.Connect();
                //Create FileStream object to stream a file
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    sftpClient.BufferSize = 1024;
                    sftpClient.UploadFile(fs, Path.GetFileName(filename));
                }

                sftpClient.Dispose();
            }
            MessageBox.Show("File successfully added");

            //sshClient to unzip the newly added zipfile
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
                ssh.Disconnect();
            }
            
        }

        //Gets the connection information to connect to Beocat
        public static ConnectionInfo getSftpConnection(string host, string username, int port, string password)
        {
            return new PasswordConnectionInfo(host, port, username, password);
        }

       
        /*Takes the path of the folder to zip from the form
         * Then it has a save file dialog to pick a location and name for the zipped file
         * Lastly it zips the folder entered to the save file dialog name and location
        */
        private void button4_Click(object sender, EventArgs e)
        {
            string startPath = @"c:\example\start";
            string zipPath = @"c:\example\result.zip";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "ZIP Files (*.zip)|*.zip";
            sfd.DefaultExt = "zip";
            sfd.AddExtension = true;

            //Gets the location and name for zipped file
            sfd.ShowDialog();
            if (sfd.FileName != null)
            {
               zipPath = sfd.FileName;
            }
            //gets the folder location of the folder to be zipped
            startPath = fileToZip.Text;
            //zips the folder to the new location
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }
    }

}
