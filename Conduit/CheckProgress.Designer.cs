namespace Conduit
{
    partial class CheckProgress
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
            this.kstatProgress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // kstatProgress
            // 
            this.kstatProgress.AcceptsReturn = true;
            this.kstatProgress.AcceptsTab = true;
            this.kstatProgress.Location = new System.Drawing.Point(33, 84);
            this.kstatProgress.Multiline = true;
            this.kstatProgress.Name = "kstatProgress";
            this.kstatProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.kstatProgress.Size = new System.Drawing.Size(734, 283);
            this.kstatProgress.TabIndex = 8;
            // 
            // CheckProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.kstatProgress);
            this.Name = "CheckProgress";
            this.Text = "CheckProgress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox kstatProgress;
    }
}