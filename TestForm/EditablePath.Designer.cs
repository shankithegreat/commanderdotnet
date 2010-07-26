namespace TestForm
{
    partial class EditablePath
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
            this.SuspendLayout();
            // 
            // EditablePath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "EditablePath";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditablePath_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
