namespace Commander
{
    partial class EditableLabel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.Location = new System.Drawing.Point(0, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(174, 13);
            this.label.TabIndex = 0;
            this.label.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(174, 13);
            this.textBox.TabIndex = 1;
            this.textBox.Visible = false;
            this.textBox.VisibleChanged += new System.EventHandler(this.textBox_VisibleChanged);
            this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.textBox.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // EditableLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label);
            this.Name = "EditableLabel";
            this.Size = new System.Drawing.Size(174, 13);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox;
    }
}
