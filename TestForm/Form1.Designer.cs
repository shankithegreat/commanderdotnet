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
            this.driveToolBar1 = new TestForm.DriveToolBar();
            this.fileListView1 = new TestForm.FileListView();
            this.commandBox1 = new TestForm.CommandBox();
            this.SuspendLayout();
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
            this.driveToolBar1.Size = new System.Drawing.Size(636, 31);
            this.driveToolBar1.TabIndex = 1;
            this.driveToolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            // 
            // fileListView1
            // 
            this.fileListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileListView1.BackColor = System.Drawing.Color.LightGray;
            this.fileListView1.Location = new System.Drawing.Point(0, 37);
            this.fileListView1.Name = "fileListView1";
            this.fileListView1.SelectedNode = null;
            this.fileListView1.Size = new System.Drawing.Size(636, 442);
            this.fileListView1.TabIndex = 0;
            this.fileListView1.UseCompatibleStateImageBehavior = false;
            this.fileListView1.View = System.Windows.Forms.View.Details;
            // 
            // commandBox1
            // 
            this.commandBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.commandBox1.BackColor = System.Drawing.Color.Silver;
            this.commandBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.commandBox1.FormattingEnabled = true;
            this.commandBox1.Lines = "";
            this.commandBox1.Location = new System.Drawing.Point(198, 485);
            this.commandBox1.Name = "commandBox1";
            this.commandBox1.Size = new System.Drawing.Size(426, 21);
            this.commandBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 518);
            this.Controls.Add(this.commandBox1);
            this.Controls.Add(this.driveToolBar1);
            this.Controls.Add(this.fileListView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private FileListView fileListView1;
        private DriveToolBar driveToolBar1;
        private CommandBox commandBox1;







    }
}

