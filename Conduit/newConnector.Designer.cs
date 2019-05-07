namespace Conduit
{
    partial class newConnector
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.ListBox();
            this.inputBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.outputBox2 = new System.Windows.Forms.ListBox();
            this.inputBox2 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.ToSnaps = new System.Windows.Forms.ListBox();
            this.FromSnaps = new System.Windows.Forms.ListBox();
            this.createToConnector = new System.Windows.Forms.Button();
            this.createFromConnector = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Input";
            // 
            // outputBox
            // 
            this.outputBox.FormattingEnabled = true;
            this.outputBox.Location = new System.Drawing.Point(12, 60);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(120, 95);
            this.outputBox.TabIndex = 2;
            // 
            // inputBox
            // 
            this.inputBox.FormattingEnabled = true;
            this.inputBox.Location = new System.Drawing.Point(12, 170);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(120, 95);
            this.inputBox.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Set Nodes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Connector To Data Node";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Connector From Data Node";
            // 
            // outputBox2
            // 
            this.outputBox2.FormattingEnabled = true;
            this.outputBox2.Location = new System.Drawing.Point(244, 60);
            this.outputBox2.Name = "outputBox2";
            this.outputBox2.Size = new System.Drawing.Size(120, 95);
            this.outputBox2.TabIndex = 7;
            // 
            // inputBox2
            // 
            this.inputBox2.FormattingEnabled = true;
            this.inputBox2.Location = new System.Drawing.Point(244, 170);
            this.inputBox2.Name = "inputBox2";
            this.inputBox2.Size = new System.Drawing.Size(120, 95);
            this.inputBox2.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Output";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(385, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Input";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(244, 281);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Set Nodes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ToSnaps
            // 
            this.ToSnaps.FormattingEnabled = true;
            this.ToSnaps.Location = new System.Drawing.Point(12, 331);
            this.ToSnaps.Name = "ToSnaps";
            this.ToSnaps.Size = new System.Drawing.Size(120, 95);
            this.ToSnaps.TabIndex = 12;
            // 
            // FromSnaps
            // 
            this.FromSnaps.FormattingEnabled = true;
            this.FromSnaps.Location = new System.Drawing.Point(244, 331);
            this.FromSnaps.Name = "FromSnaps";
            this.FromSnaps.Size = new System.Drawing.Size(120, 95);
            this.FromSnaps.TabIndex = 13;
            // 
            // createToConnector
            // 
            this.createToConnector.Location = new System.Drawing.Point(12, 432);
            this.createToConnector.Name = "createToConnector";
            this.createToConnector.Size = new System.Drawing.Size(120, 23);
            this.createToConnector.TabIndex = 14;
            this.createToConnector.Text = "Create Connector";
            this.createToConnector.UseVisualStyleBackColor = true;
            this.createToConnector.Click += new System.EventHandler(this.createToConnector_Click);
            // 
            // createFromConnector
            // 
            this.createFromConnector.Location = new System.Drawing.Point(244, 432);
            this.createFromConnector.Name = "createFromConnector";
            this.createFromConnector.Size = new System.Drawing.Size(120, 23);
            this.createFromConnector.TabIndex = 15;
            this.createFromConnector.Text = "Create Connector";
            this.createFromConnector.UseVisualStyleBackColor = true;
            this.createFromConnector.Click += new System.EventHandler(this.createFromConnector_Click);
            // 
            // newConnector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 486);
            this.Controls.Add(this.createFromConnector);
            this.Controls.Add(this.createToConnector);
            this.Controls.Add(this.FromSnaps);
            this.Controls.Add(this.ToSnaps);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.inputBox2);
            this.Controls.Add(this.outputBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "newConnector";
            this.Text = "Create New Connector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox outputBox;
        private System.Windows.Forms.ListBox inputBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox outputBox2;
        private System.Windows.Forms.ListBox inputBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox ToSnaps;
        private System.Windows.Forms.ListBox FromSnaps;
        private System.Windows.Forms.Button createToConnector;
        private System.Windows.Forms.Button createFromConnector;
    }
}