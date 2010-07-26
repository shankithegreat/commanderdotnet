namespace TestForm
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.fileViewPanel1 = new Commander.FileViewPanel();
            this.driveToolBar1 = new TestForm.DriveToolBar();
            this.SuspendLayout();
            // 
            // fileViewPanel1
            // 
            this.fileViewPanel1.Location = new System.Drawing.Point(423, 38);
            this.fileViewPanel1.Name = "fileViewPanel1";
            this.fileViewPanel1.Size = new System.Drawing.Size(343, 240);
            this.fileViewPanel1.TabIndex = 0;
            // 
            // driveToolBar1
            // 
            this.driveToolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.driveToolBar1.AutoSize = false;
            this.driveToolBar1.ButtonSize = new System.Drawing.Size(36, 21);
            this.driveToolBar1.DropDownArrows = true;
            this.driveToolBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.driveToolBar1.Location = new System.Drawing.Point(0, 0);
            this.driveToolBar1.Name = "driveToolBar1";
            this.driveToolBar1.ShowToolTips = true;
            this.driveToolBar1.Size = new System.Drawing.Size(996, 32);
            this.driveToolBar1.TabIndex = 1;
            this.driveToolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 290);
            this.Controls.Add(this.driveToolBar1);
            this.Controls.Add(this.fileViewPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Commander.FileViewPanel fileViewPanel1;
        private DriveToolBar driveToolBar1;





    }
}

