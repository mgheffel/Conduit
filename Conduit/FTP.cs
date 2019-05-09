using System;
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
    public partial class FTP : Form
    {
        private MainWindow mainWindow;
        public FTP(MainWindow v)
        { 
            InitializeComponent();
            mainWindow = v;
        }
    }
}
