namespace Conduit
{
    partial class FTP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.fileToDownload = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.unZip = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(161, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Send File to Beocat";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(170, 140);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Download File From Beocat";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fileToDownload
            // 
            this.fileToDownload.Location = new System.Drawing.Point(24, 140);
            this.fileToDownload.Name = "fileToDownload";
            this.fileToDownload.Size = new System.Drawing.Size(121, 20);
            this.fileToDownload.TabIndex = 2;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(170, 187);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(161, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Unzip Directory";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // unZip
            // 
            this.unZip.Location = new System.Drawing.Point(24, 190);
            this.unZip.Name = "unZip";
            this.unZip.Size = new System.Drawing.Size(121, 20);
            this.unZip.TabIndex = 6;
            // 
            // FTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.unZip);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.fileToDownload);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "FTP";
            this.Text = "FTP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox fileToDownload;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox unZip;
    }
}