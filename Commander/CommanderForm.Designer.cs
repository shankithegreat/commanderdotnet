namespace Commander
{
    partial class CommanderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommanderForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.viewMenuItem = new System.Windows.Forms.MenuItem();
            this.editMenuItem = new System.Windows.Forms.MenuItem();
            this.copyMenuItem = new System.Windows.Forms.MenuItem();
            this.moveMenuItem = new System.Windows.Forms.MenuItem();
            this.createFolderMenuItem = new System.Windows.Forms.MenuItem();
            this.deleteMenuItem = new System.Windows.Forms.MenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmdComboBox = new System.Windows.Forms.ComboBox();
            this.cmdLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.exitButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.creteFolderButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.viewButton = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.topSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftDrivesToolBar = new System.Windows.Forms.ToolBar();
            this.driveToolBarImageList = new System.Windows.Forms.ImageList(this.components);
            this.rightDriveToolBar = new System.Windows.Forms.ToolBar();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.leftFileView = new Commander.FileView();
            this.rightFileView = new Commander.FileView();
            this.listImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.topSplitContainer.Panel1.SuspendLayout();
            this.topSplitContainer.Panel2.SuspendLayout();
            this.topSplitContainer.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem4});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.exitMenuItem});
            this.menuItem1.Text = "File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 0;
            this.exitMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.viewMenuItem,
            this.editMenuItem,
            this.copyMenuItem,
            this.moveMenuItem,
            this.createFolderMenuItem,
            this.deleteMenuItem});
            this.menuItem4.Text = "Commands";
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.Index = 0;
            this.viewMenuItem.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.viewMenuItem.Text = "View";
            this.viewMenuItem.Click += new System.EventHandler(this.viewMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Index = 1;
            this.editMenuItem.Shortcut = System.Windows.Forms.Shortcut.F4;
            this.editMenuItem.Text = "Edit";
            this.editMenuItem.Click += new System.EventHandler(this.editMenuItem_Click);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Index = 2;
            this.copyMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.copyMenuItem_Click);
            // 
            // moveMenuItem
            // 
            this.moveMenuItem.Index = 3;
            this.moveMenuItem.Shortcut = System.Windows.Forms.Shortcut.F6;
            this.moveMenuItem.Text = "Move";
            this.moveMenuItem.Click += new System.EventHandler(this.moveMenuItem_Click);
            // 
            // createFolderMenuItem
            // 
            this.createFolderMenuItem.Index = 4;
            this.createFolderMenuItem.Shortcut = System.Windows.Forms.Shortcut.F7;
            this.createFolderMenuItem.Text = "Create Folder";
            this.createFolderMenuItem.Click += new System.EventHandler(this.createFolderMenuItem_Click);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Index = 5;
            this.deleteMenuItem.Shortcut = System.Windows.Forms.Shortcut.F8;
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(670, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.tableLayoutPanel2);
            this.bottomPanel.Controls.Add(this.tableLayoutPanel1);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 363);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(670, 55);
            this.bottomPanel.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel2.Controls.Add(this.cmdComboBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmdLabel, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(670, 27);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // cmdComboBox
            // 
            this.cmdComboBox.BackColor = System.Drawing.Color.Silver;
            this.cmdComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdComboBox.FormattingEnabled = true;
            this.cmdComboBox.Location = new System.Drawing.Point(224, 3);
            this.cmdComboBox.Name = "cmdComboBox";
            this.cmdComboBox.Size = new System.Drawing.Size(443, 21);
            this.cmdComboBox.TabIndex = 1;
            this.cmdComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmdComboBox_KeyDown);
            // 
            // cmdLabel
            // 
            this.cmdLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdLabel.Location = new System.Drawing.Point(0, 0);
            this.cmdLabel.Margin = new System.Windows.Forms.Padding(0);
            this.cmdLabel.Name = "cmdLabel";
            this.cmdLabel.Size = new System.Drawing.Size(221, 27);
            this.cmdLabel.TabIndex = 2;
            this.cmdLabel.Text = ">";
            this.cmdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdLabel.UseMnemonic = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28204F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28633F));
            this.tableLayoutPanel1.Controls.Add(this.exitButton, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.deleteButton, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.creteFolderButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.moveButton, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.copyButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.editButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewButton, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(670, 27);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // exitButton
            // 
            this.exitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exitButton.Location = new System.Drawing.Point(573, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(94, 21);
            this.exitButton.TabIndex = 7;
            this.exitButton.Text = "Alt+F4 Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deleteButton.Location = new System.Drawing.Point(478, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(89, 21);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Text = "F8 Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // creteFolderButton
            // 
            this.creteFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.creteFolderButton.Location = new System.Drawing.Point(383, 3);
            this.creteFolderButton.Name = "creteFolderButton";
            this.creteFolderButton.Size = new System.Drawing.Size(89, 21);
            this.creteFolderButton.TabIndex = 5;
            this.creteFolderButton.Text = "F7 Folder";
            this.creteFolderButton.UseVisualStyleBackColor = true;
            this.creteFolderButton.Click += new System.EventHandler(this.createFolderMenuItem_Click);
            // 
            // moveButton
            // 
            this.moveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moveButton.Location = new System.Drawing.Point(288, 3);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(89, 21);
            this.moveButton.TabIndex = 4;
            this.moveButton.Text = "F6 Move";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.moveMenuItem_Click);
            // 
            // copyButton
            // 
            this.copyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.copyButton.Location = new System.Drawing.Point(193, 3);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(89, 21);
            this.copyButton.TabIndex = 3;
            this.copyButton.Text = "F5 Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyMenuItem_Click);
            // 
            // editButton
            // 
            this.editButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editButton.Location = new System.Drawing.Point(98, 3);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(89, 21);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "F4 Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editMenuItem_Click);
            // 
            // viewButton
            // 
            this.viewButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewButton.Location = new System.Drawing.Point(3, 3);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(89, 21);
            this.viewButton.TabIndex = 1;
            this.viewButton.Text = "F3 View";
            this.viewButton.UseVisualStyleBackColor = true;
            this.viewButton.Click += new System.EventHandler(this.viewMenuItem_Click);
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.topSplitContainer);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 25);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(670, 33);
            this.topPanel.TabIndex = 2;
            // 
            // topSplitContainer
            // 
            this.topSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topSplitContainer.IsSplitterFixed = true;
            this.topSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.topSplitContainer.Name = "topSplitContainer";
            // 
            // topSplitContainer.Panel1
            // 
            this.topSplitContainer.Panel1.Controls.Add(this.leftDrivesToolBar);
            // 
            // topSplitContainer.Panel2
            // 
            this.topSplitContainer.Panel2.Controls.Add(this.rightDriveToolBar);
            this.topSplitContainer.Size = new System.Drawing.Size(670, 33);
            this.topSplitContainer.SplitterDistance = 300;
            this.topSplitContainer.TabIndex = 0;
            // 
            // leftDrivesToolBar
            // 
            this.leftDrivesToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.leftDrivesToolBar.ButtonSize = new System.Drawing.Size(36, 21);
            this.leftDrivesToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftDrivesToolBar.DropDownArrows = true;
            this.leftDrivesToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftDrivesToolBar.ImageList = this.driveToolBarImageList;
            this.leftDrivesToolBar.Location = new System.Drawing.Point(0, 0);
            this.leftDrivesToolBar.Name = "leftDrivesToolBar";
            this.leftDrivesToolBar.ShowToolTips = true;
            this.leftDrivesToolBar.Size = new System.Drawing.Size(300, 27);
            this.leftDrivesToolBar.TabIndex = 0;
            this.leftDrivesToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.leftDrivesToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.drivesToolBar_ButtonClick);
            this.leftDrivesToolBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.leftDrivesToolBar_MouseUp);
            // 
            // driveToolBarImageList
            // 
            this.driveToolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("driveToolBarImageList.ImageStream")));
            this.driveToolBarImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.driveToolBarImageList.Images.SetKeyName(0, "flopydrive.png");
            this.driveToolBarImageList.Images.SetKeyName(1, "Drive.png");
            this.driveToolBarImageList.Images.SetKeyName(2, "cddrive.png");
            this.driveToolBarImageList.Images.SetKeyName(3, "removabledrive.png");
            this.driveToolBarImageList.Images.SetKeyName(4, "net.png");
            this.driveToolBarImageList.Images.SetKeyName(5, "star");
            this.driveToolBarImageList.Images.SetKeyName(6, "downList");
            // 
            // rightDriveToolBar
            // 
            this.rightDriveToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.rightDriveToolBar.ButtonSize = new System.Drawing.Size(36, 21);
            this.rightDriveToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightDriveToolBar.DropDownArrows = true;
            this.rightDriveToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightDriveToolBar.ImageList = this.driveToolBarImageList;
            this.rightDriveToolBar.Location = new System.Drawing.Point(0, 0);
            this.rightDriveToolBar.Name = "rightDriveToolBar";
            this.rightDriveToolBar.ShowToolTips = true;
            this.rightDriveToolBar.Size = new System.Drawing.Size(366, 27);
            this.rightDriveToolBar.TabIndex = 1;
            this.rightDriveToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.rightDriveToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.drivesToolBar_ButtonClick);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer.Location = new System.Drawing.Point(0, 58);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.leftFileView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rightFileView);
            this.splitContainer.Size = new System.Drawing.Size(670, 305);
            this.splitContainer.SplitterDistance = 332;
            this.splitContainer.TabIndex = 3;
            this.splitContainer.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitContainer_SplitterMoving);
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            this.splitContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseDown);
            this.splitContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseUp);
            // 
            // leftFileView
            // 
            this.leftFileView.CurrentDirectory = null;
            this.leftFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftFileView.Location = new System.Drawing.Point(0, 0);
            this.leftFileView.Name = "leftFileView";
            this.leftFileView.Size = new System.Drawing.Size(332, 305);
            this.leftFileView.TabIndex = 0;
            this.leftFileView.DirectorySelected += new Commander.DirectorySelectedEventHandler(this.fileView_DirectorySelected);
            this.leftFileView.Leave += new System.EventHandler(this.fileView_Leave);
            this.leftFileView.Enter += new System.EventHandler(this.fileView_Enter);
            // 
            // rightFileView
            // 
            this.rightFileView.CurrentDirectory = null;
            this.rightFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightFileView.Location = new System.Drawing.Point(0, 0);
            this.rightFileView.Name = "rightFileView";
            this.rightFileView.Size = new System.Drawing.Size(334, 305);
            this.rightFileView.TabIndex = 1;
            this.rightFileView.DirectorySelected += new Commander.DirectorySelectedEventHandler(this.fileView_DirectorySelected);
            this.rightFileView.Leave += new System.EventHandler(this.fileView_Leave);
            this.rightFileView.Enter += new System.EventHandler(this.fileView_Enter);
            // 
            // listImageList
            // 
            this.listImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listImageList.ImageStream")));
            this.listImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.listImageList.Images.SetKeyName(0, "star");
            this.listImageList.Images.SetKeyName(1, "downList");
            // 
            // splitToolTip
            // 
            this.splitToolTip.AutomaticDelay = 0;
            this.splitToolTip.UseAnimation = false;
            this.splitToolTip.UseFading = false;
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 418);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.toolStrip);
            this.Menu = this.mainMenu;
            this.Name = "CommanderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topSplitContainer.Panel1.ResumeLayout(false);
            this.topSplitContainer.Panel1.PerformLayout();
            this.topSplitContainer.Panel2.ResumeLayout(false);
            this.topSplitContainer.Panel2.PerformLayout();
            this.topSplitContainer.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.SplitContainer topSplitContainer;
        private System.Windows.Forms.ToolBar leftDrivesToolBar;
        private System.Windows.Forms.ToolBar rightDriveToolBar;
        private System.Windows.Forms.ImageList driveToolBarImageList;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ImageList listImageList;
        private FileView leftFileView;
        private FileView rightFileView;
        private System.Windows.Forms.ToolTip splitToolTip;
        private System.Windows.Forms.ComboBox cmdComboBox;
        private System.Windows.Forms.Label cmdLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button creteFolderButton;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem viewMenuItem;
        private System.Windows.Forms.MenuItem editMenuItem;
        private System.Windows.Forms.MenuItem copyMenuItem;
        private System.Windows.Forms.MenuItem moveMenuItem;
        private System.Windows.Forms.MenuItem createFolderMenuItem;
        private System.Windows.Forms.MenuItem deleteMenuItem;


    }
}

