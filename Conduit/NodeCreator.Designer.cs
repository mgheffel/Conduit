namespace Conduit
{
    partial class NodeCreator
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
            this.label1 = new System.Windows.Forms.Label();
            this.cb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.inputValue = new System.Windows.Forms.TextBox();
            this.outputValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(834, 386);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Create Fields";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 393);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "How Many Parameter Fields?";
            // 
            // cb
            // 
            this.cb.FormattingEnabled = true;
            this.cb.Location = new System.Drawing.Point(153, 388);
            this.cb.Name = "cb";
            this.cb.Size = new System.Drawing.Size(45, 21);
            this.cb.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 391);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "How many input snaps?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(387, 393);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "How many output snaps?";
            // 
            // inputValue
            // 
            this.inputValue.Location = new System.Drawing.Point(330, 388);
            this.inputValue.Name = "inputValue";
            this.inputValue.Size = new System.Drawing.Size(40, 20);
            this.inputValue.TabIndex = 6;
            // 
            // outputValue
            // 
            this.outputValue.Location = new System.Drawing.Point(520, 388);
            this.outputValue.Name = "outputValue";
            this.outputValue.Size = new System.Drawing.Size(37, 20);
            this.outputValue.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(563, 393);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Node Name?";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(639, 386);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(149, 20);
            this.name.TabIndex = 9;
            // 
            // NodeCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 444);
            this.Controls.Add(this.name);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.outputValue);
            this.Controls.Add(this.inputValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "NodeCreator";
            this.Text = "Node Creation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inputValue;
        private System.Windows.Forms.TextBox outputValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox name;
    }
}