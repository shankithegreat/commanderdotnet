namespace Commander
{
    partial class TestForm
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
            this.toolStripDropDown1 = new System.Windows.Forms.ToolStripDropDown();
            this.editableLabel1 = new Commander.EditableLabel();
            this.SuspendLayout();
            // 
            // toolStripDropDown1
            // 
            this.toolStripDropDown1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripDropDown1.Name = "toolStripDropDown1";
            this.toolStripDropDown1.Size = new System.Drawing.Size(2, 4);
            // 
            // editableLabel1
            // 
            this.editableLabel1.AutoSize = true;
            this.editableLabel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.editableLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editableLabel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.editableLabel1.Location = new System.Drawing.Point(68, 67);
            this.editableLabel1.Name = "editableLabel1";
            this.editableLabel1.Size = new System.Drawing.Size(174, 13);
            this.editableLabel1.TabIndex = 1;
            this.editableLabel1.Text = "1234";
            this.editableLabel1.TextBoxBackColor = System.Drawing.Color.Silver;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.editableLabel1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripDropDown toolStripDropDown1;
        private EditableLabel editableLabel1;
    }
}