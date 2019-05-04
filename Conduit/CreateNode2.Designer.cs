namespace Conduit
{
    partial class CreateNode2
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
            this.nodeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.outputSnaps = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ParameterValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.inputSnaps = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // nodeName
            // 
            this.nodeName.Location = new System.Drawing.Point(23, 22);
            this.nodeName.Name = "nodeName";
            this.nodeName.Size = new System.Drawing.Size(129, 20);
            this.nodeName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Node Name";
            // 
            // outputSnaps
            // 
            this.outputSnaps.Location = new System.Drawing.Point(23, 74);
            this.outputSnaps.Name = "outputSnaps";
            this.outputSnaps.Size = new System.Drawing.Size(100, 20);
            this.outputSnaps.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Input Snaps";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output Snaps";
            // 
            // ParameterValue
            // 
            this.ParameterValue.Location = new System.Drawing.Point(23, 100);
            this.ParameterValue.Name = "ParameterValue";
            this.ParameterValue.Size = new System.Drawing.Size(100, 20);
            this.ParameterValue.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(176, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Parameter Value";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(77, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Create Node";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputSnaps
            // 
            this.inputSnaps.FormattingEnabled = true;
            this.inputSnaps.Location = new System.Drawing.Point(23, 48);
            this.inputSnaps.Name = "inputSnaps";
            this.inputSnaps.Size = new System.Drawing.Size(100, 21);
            this.inputSnaps.TabIndex = 11;
            // 
            // CreateNode2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.inputSnaps);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ParameterValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outputSnaps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nodeName);
            this.Name = "CreateNode2";
            this.Text = "CreateNode2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nodeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputSnaps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ParameterValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox inputSnaps;
    }
}