﻿namespace Commander
{
    partial class FileView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileView));
            this.titlePanel = new System.Windows.Forms.Panel();
            this.titleLabel = new Commander.EditableLabel();
            this.linkButton = new System.Windows.Forms.Button();
            this.listImageList = new System.Windows.Forms.ImageList(this.components);
            this.historyButton = new System.Windows.Forms.Button();
            this.hintPanel = new System.Windows.Forms.Panel();
            this.hintLabel = new System.Windows.Forms.Label();
            this.rootButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.listView = new Commander.ShellListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.largeImageList = new System.Windows.Forms.ImageList(this.components);
            this.titlePanel.SuspendLayout();
            this.hintPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.AutoSize = true;
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Controls.Add(this.linkButton);
            this.titlePanel.Controls.Add(this.historyButton);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 20);
            this.titlePanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(343, 20);
            this.titlePanel.TabIndex = 5;
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.titleLabel.Location = new System.Drawing.Point(3, 3);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(296, 13);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "c:\\*.*";
            this.titleLabel.TextBoxBackColor = System.Drawing.Color.Silver;
            this.titleLabel.BeforeEdit += new Commander.BeforeEditEventHandler(this.titleLabel_BeforeEdit);
            this.titleLabel.AfterEdit += new Commander.AfterEditEventHandler(this.titleLabel_AfterEdit);
            this.titleLabel.Click += new System.EventHandler(this.titleLabel_Click);
            this.titleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseUp);
            // 
            // linkButton
            // 
            this.linkButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.linkButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.linkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkButton.ImageKey = "star";
            this.linkButton.ImageList = this.listImageList;
            this.linkButton.Location = new System.Drawing.Point(301, 0);
            this.linkButton.Margin = new System.Windows.Forms.Padding(0);
            this.linkButton.Name = "linkButton";
            this.linkButton.Size = new System.Drawing.Size(21, 20);
            this.linkButton.TabIndex = 2;
            this.linkButton.UseVisualStyleBackColor = true;
            // 
            // listImageList
            // 
            this.listImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listImageList.ImageStream")));
            this.listImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.listImageList.Images.SetKeyName(0, "star");
            this.listImageList.Images.SetKeyName(1, "downList");
            this.listImageList.Images.SetKeyName(2, "back.png");
            // 
            // historyButton
            // 
            this.historyButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.historyButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.historyButton.ImageKey = "downList";
            this.historyButton.ImageList = this.listImageList;
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
            // listView
            // 
            this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView.BackColor = System.Drawing.Color.Silver;
            this.listView.ColumnHeaderContextMenu = null;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView.FullRowSelect = true;
            this.listView.Location = new System.Drawing.Point(0, 40);
            this.listView.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.Size = new System.Drawing.Size(343, 200);
            this.listView.SuspendHeaderContextMenu = false;
            this.listView.TabIndex = 3;
            this.listView.TileSize = new System.Drawing.Size(188, 32);
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ItemActivate += new System.EventHandler(this.listView_ItemActivate);
            this.listView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_MouseUp);
            this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 10;
            // 
            // largeImageList
            // 
            this.largeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.largeImageList.ImageSize = new System.Drawing.Size(108, 108);
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FileView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.hintPanel);
            this.Name = "FileView";
            this.Size = new System.Drawing.Size(343, 240);
            this.Leave += new System.EventHandler(this.FileView_Leave);
            this.Enter += new System.EventHandler(this.FileView_Enter);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.hintPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Commander.ShellListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.Button historyButton;
        private System.Windows.Forms.Panel hintPanel;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.Button rootButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.ImageList listImageList;
        private EditableLabel titleLabel;
        private System.Windows.Forms.ImageList largeImageList;
    }
}
