namespace Commander
{
    partial class FileViewPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileViewPanel));
            this.titlePanel = new System.Windows.Forms.Panel();
            this.editablePath1 = new TestForm.EditablePath();
            this.linkButton = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.historyButton = new System.Windows.Forms.Button();
            this.hintPanel = new System.Windows.Forms.Panel();
            this.hintLabel = new System.Windows.Forms.Label();
            this.rootButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.view = new TestForm.FileListView();
            this.titlePanel.SuspendLayout();
            this.hintPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.AutoSize = true;
            this.titlePanel.Controls.Add(this.editablePath1);
            this.titlePanel.Controls.Add(this.linkButton);
            this.titlePanel.Controls.Add(this.historyButton);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 20);
            this.titlePanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(343, 20);
            this.titlePanel.TabIndex = 5;
            // 
            // editablePath1
            // 
            this.editablePath1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editablePath1.AutoSize = true;
            this.editablePath1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.editablePath1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editablePath1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.editablePath1.Location = new System.Drawing.Point(3, 3);
            this.editablePath1.Name = "editablePath1";
            this.editablePath1.Size = new System.Drawing.Size(295, 13);
            this.editablePath1.TabIndex = 3;
            this.editablePath1.Text = "c:\\*.*";
            //this.editablePath1.TextBoxBackColor = System.Drawing.Color.Silver;
            // 
            // linkButton
            // 
            this.linkButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.linkButton.Enabled = false;
            this.linkButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.linkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkButton.ImageKey = "star";
            this.linkButton.ImageList = this.imageList;
            this.linkButton.Location = new System.Drawing.Point(301, 0);
            this.linkButton.Margin = new System.Windows.Forms.Padding(0);
            this.linkButton.Name = "linkButton";
            this.linkButton.Size = new System.Drawing.Size(21, 20);
            this.linkButton.TabIndex = 2;
            this.linkButton.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "star");
            this.imageList.Images.SetKeyName(1, "downList");
            this.imageList.Images.SetKeyName(2, "back.png");
            // 
            // historyButton
            // 
            this.historyButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.historyButton.Enabled = false;
            this.historyButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.historyButton.ImageKey = "downList";
            this.historyButton.ImageList = this.imageList;
            this.historyButton.Location = new System.Drawing.Point(322, 0);
            this.historyButton.Margin = new System.Windows.Forms.Padding(0);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(21, 20);
            this.historyButton.TabIndex = 1;
            this.historyButton.UseVisualStyleBackColor = true;
            // 
            // hintPanel
            // 
            this.hintPanel.AutoSize = true;
            this.hintPanel.Controls.Add(this.hintLabel);
            this.hintPanel.Controls.Add(this.rootButton);
            this.hintPanel.Controls.Add(this.upButton);
            this.hintPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hintPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.hintPanel.Location = new System.Drawing.Point(0, 0);
            this.hintPanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.hintPanel.Name = "hintPanel";
            this.hintPanel.Size = new System.Drawing.Size(343, 20);
            this.hintPanel.TabIndex = 4;
            // 
            // hintLabel
            // 
            this.hintLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hintLabel.Location = new System.Drawing.Point(0, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(297, 20);
            this.hintLabel.TabIndex = 3;
            this.hintLabel.Text = "Title";
            this.hintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rootButton
            // 
            this.rootButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.rootButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rootButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rootButton.Location = new System.Drawing.Point(297, 0);
            this.rootButton.Margin = new System.Windows.Forms.Padding(0);
            this.rootButton.Name = "rootButton";
            this.rootButton.Size = new System.Drawing.Size(23, 20);
            this.rootButton.TabIndex = 2;
            this.rootButton.Text = "\\";
            this.rootButton.UseVisualStyleBackColor = true;
            this.rootButton.Click += new System.EventHandler(this.rootButton_Click);
            // 
            // upButton
            // 
            this.upButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.upButton.Location = new System.Drawing.Point(320, 0);
            this.upButton.Margin = new System.Windows.Forms.Padding(0);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(23, 20);
            this.upButton.TabIndex = 1;
            this.upButton.Text = "..";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // view
            // 
            this.view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.view.Location = new System.Drawing.Point(0, 40);
            this.view.Name = "view";
            this.view.SelectedNode = null;
            this.view.Size = new System.Drawing.Size(343, 200);
            this.view.TabIndex = 6;
            this.view.UseCompatibleStateImageBehavior = false;
            this.view.View = System.Windows.Forms.View.Details;
            // 
            // FileViewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.view);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.hintPanel);
            this.Name = "FileViewControl";
            this.Size = new System.Drawing.Size(343, 240);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.hintPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.Button historyButton;
        private System.Windows.Forms.Panel hintPanel;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.Button rootButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.ImageList imageList;
        private TestForm.FileListView view;
        private TestForm.EditablePath editablePath1;
    }
}
