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
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.testLabel = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.topSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftDrivesToolBar = new System.Windows.Forms.ToolBar();
            this.driveToolBarImageList = new System.Windows.Forms.ImageList(this.components);
            this.rightDriveToolBar = new System.Windows.Forms.ToolBar();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.leftListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.leftTitlePanel = new System.Windows.Forms.Panel();
            this.leftTitleLabel = new System.Windows.Forms.Label();
            this.leftLinkButton = new System.Windows.Forms.Button();
            this.listImageList = new System.Windows.Forms.ImageList(this.components);
            this.leftHistoryButton = new System.Windows.Forms.Button();
            this.leftHintListPanel = new System.Windows.Forms.Panel();
            this.leftHintLabel = new System.Windows.Forms.Label();
            this.leftRootButton = new System.Windows.Forms.Button();
            this.leftUpButton = new System.Windows.Forms.Button();
            this.rightListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.rightTitlePanel = new System.Windows.Forms.Panel();
            this.rightTitleLabel = new System.Windows.Forms.Label();
            this.rightLinkButton = new System.Windows.Forms.Button();
            this.rightHistoryButton = new System.Windows.Forms.Button();
            this.rightHintPanel = new System.Windows.Forms.Panel();
            this.rightHintLabel = new System.Windows.Forms.Label();
            this.rightRootButton = new System.Windows.Forms.Button();
            this.rightUpButton = new System.Windows.Forms.Button();
            this.toolStrip.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.topSplitContainer.Panel1.SuspendLayout();
            this.topSplitContainer.Panel2.SuspendLayout();
            this.topSplitContainer.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.leftTitlePanel.SuspendLayout();
            this.leftHintListPanel.SuspendLayout();
            this.rightTitlePanel.SuspendLayout();
            this.rightHintPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3});
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Item";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Exit";
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
            this.toolStrip.Size = new System.Drawing.Size(580, 25);
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
            this.bottomPanel.Controls.Add(this.testLabel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 312);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(580, 38);
            this.bottomPanel.TabIndex = 1;
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(4, 13);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(50, 13);
            this.testLabel.TabIndex = 0;
            this.testLabel.Text = "testLabel";
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.topSplitContainer);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 25);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(580, 33);
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
            this.topSplitContainer.Size = new System.Drawing.Size(580, 33);
            this.topSplitContainer.SplitterDistance = 274;
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
            this.leftDrivesToolBar.Size = new System.Drawing.Size(274, 27);
            this.leftDrivesToolBar.TabIndex = 0;
            this.leftDrivesToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.leftDrivesToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.drivesToolBar_ButtonClick);
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
            this.rightDriveToolBar.Size = new System.Drawing.Size(302, 27);
            this.rightDriveToolBar.TabIndex = 1;
            this.rightDriveToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.rightDriveToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.drivesToolBar_ButtonClick);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 58);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.leftListView);
            this.splitContainer.Panel1.Controls.Add(this.leftTitlePanel);
            this.splitContainer.Panel1.Controls.Add(this.leftHintListPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rightListView);
            this.splitContainer.Panel2.Controls.Add(this.rightTitlePanel);
            this.splitContainer.Panel2.Controls.Add(this.rightHintPanel);
            this.splitContainer.Size = new System.Drawing.Size(580, 254);
            this.splitContainer.SplitterDistance = 274;
            this.splitContainer.TabIndex = 3;
            // 
            // leftListView
            // 
            this.leftListView.BackColor = System.Drawing.Color.Silver;
            this.leftListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.leftListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftListView.FullRowSelect = true;
            this.leftListView.Location = new System.Drawing.Point(0, 40);
            this.leftListView.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.leftListView.Name = "leftListView";
            this.leftListView.Size = new System.Drawing.Size(274, 214);
            this.leftListView.TabIndex = 0;
            this.leftListView.TileSize = new System.Drawing.Size(188, 32);
            this.leftListView.UseCompatibleStateImageBehavior = false;
            this.leftListView.View = System.Windows.Forms.View.Details;
            this.leftListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
            this.leftListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 10;
            // 
            // leftTitlePanel
            // 
            this.leftTitlePanel.AutoSize = true;
            this.leftTitlePanel.Controls.Add(this.leftTitleLabel);
            this.leftTitlePanel.Controls.Add(this.leftLinkButton);
            this.leftTitlePanel.Controls.Add(this.leftHistoryButton);
            this.leftTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.leftTitlePanel.Location = new System.Drawing.Point(0, 20);
            this.leftTitlePanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.leftTitlePanel.Name = "leftTitlePanel";
            this.leftTitlePanel.Size = new System.Drawing.Size(274, 20);
            this.leftTitlePanel.TabIndex = 2;
            // 
            // leftTitleLabel
            // 
            this.leftTitleLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.leftTitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.leftTitleLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.leftTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftTitleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.leftTitleLabel.Location = new System.Drawing.Point(1, 1);
            this.leftTitleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.leftTitleLabel.MinimumSize = new System.Drawing.Size(0, 15);
            this.leftTitleLabel.Name = "leftTitleLabel";
            this.leftTitleLabel.Size = new System.Drawing.Size(232, 17);
            this.leftTitleLabel.TabIndex = 0;
            this.leftTitleLabel.Text = "c:\\*.*";
            this.leftTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // leftLinkButton
            // 
            this.leftLinkButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.leftLinkButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.leftLinkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftLinkButton.ImageKey = "star";
            this.leftLinkButton.ImageList = this.listImageList;
            this.leftLinkButton.Location = new System.Drawing.Point(232, 0);
            this.leftLinkButton.Margin = new System.Windows.Forms.Padding(0);
            this.leftLinkButton.Name = "leftLinkButton";
            this.leftLinkButton.Size = new System.Drawing.Size(21, 20);
            this.leftLinkButton.TabIndex = 2;
            this.leftLinkButton.UseVisualStyleBackColor = true;
            // 
            // listImageList
            // 
            this.listImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listImageList.ImageStream")));
            this.listImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.listImageList.Images.SetKeyName(0, "star");
            this.listImageList.Images.SetKeyName(1, "downList");
            // 
            // leftHistoryButton
            // 
            this.leftHistoryButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.leftHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.leftHistoryButton.ImageKey = "downList";
            this.leftHistoryButton.ImageList = this.listImageList;
            this.leftHistoryButton.Location = new System.Drawing.Point(253, 0);
            this.leftHistoryButton.Margin = new System.Windows.Forms.Padding(0);
            this.leftHistoryButton.Name = "leftHistoryButton";
            this.leftHistoryButton.Size = new System.Drawing.Size(21, 20);
            this.leftHistoryButton.TabIndex = 1;
            this.leftHistoryButton.UseVisualStyleBackColor = true;
            // 
            // leftHintListPanel
            // 
            this.leftHintListPanel.AutoSize = true;
            this.leftHintListPanel.Controls.Add(this.leftHintLabel);
            this.leftHintListPanel.Controls.Add(this.leftRootButton);
            this.leftHintListPanel.Controls.Add(this.leftUpButton);
            this.leftHintListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.leftHintListPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftHintListPanel.Location = new System.Drawing.Point(0, 0);
            this.leftHintListPanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.leftHintListPanel.Name = "leftHintListPanel";
            this.leftHintListPanel.Size = new System.Drawing.Size(274, 20);
            this.leftHintListPanel.TabIndex = 1;
            // 
            // leftHintLabel
            // 
            this.leftHintLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftHintLabel.Location = new System.Drawing.Point(0, 0);
            this.leftHintLabel.Name = "leftHintLabel";
            this.leftHintLabel.Size = new System.Drawing.Size(228, 20);
            this.leftHintLabel.TabIndex = 3;
            this.leftHintLabel.Text = "Title";
            this.leftHintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // leftRootButton
            // 
            this.leftRootButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.leftRootButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.leftRootButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftRootButton.Location = new System.Drawing.Point(228, 0);
            this.leftRootButton.Margin = new System.Windows.Forms.Padding(0);
            this.leftRootButton.Name = "leftRootButton";
            this.leftRootButton.Size = new System.Drawing.Size(23, 20);
            this.leftRootButton.TabIndex = 2;
            this.leftRootButton.Text = "\\";
            this.leftRootButton.UseVisualStyleBackColor = true;
            this.leftRootButton.Click += new System.EventHandler(this.rootButton_Click);
            // 
            // leftUpButton
            // 
            this.leftUpButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.leftUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.leftUpButton.Location = new System.Drawing.Point(251, 0);
            this.leftUpButton.Margin = new System.Windows.Forms.Padding(0);
            this.leftUpButton.Name = "leftUpButton";
            this.leftUpButton.Size = new System.Drawing.Size(23, 20);
            this.leftUpButton.TabIndex = 1;
            this.leftUpButton.Text = "..";
            this.leftUpButton.UseVisualStyleBackColor = true;
            this.leftUpButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // rightListView
            // 
            this.rightListView.BackColor = System.Drawing.Color.Silver;
            this.rightListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.rightListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightListView.Location = new System.Drawing.Point(0, 40);
            this.rightListView.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rightListView.Name = "rightListView";
            this.rightListView.Size = new System.Drawing.Size(302, 214);
            this.rightListView.TabIndex = 1;
            this.rightListView.UseCompatibleStateImageBehavior = false;
            this.rightListView.View = System.Windows.Forms.View.Details;
            this.rightListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
            this.rightListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_MouseUp);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 10;
            // 
            // rightTitlePanel
            // 
            this.rightTitlePanel.AutoSize = true;
            this.rightTitlePanel.Controls.Add(this.rightTitleLabel);
            this.rightTitlePanel.Controls.Add(this.rightLinkButton);
            this.rightTitlePanel.Controls.Add(this.rightHistoryButton);
            this.rightTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rightTitlePanel.Location = new System.Drawing.Point(0, 20);
            this.rightTitlePanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.rightTitlePanel.Name = "rightTitlePanel";
            this.rightTitlePanel.Size = new System.Drawing.Size(302, 20);
            this.rightTitlePanel.TabIndex = 3;
            // 
            // rightTitleLabel
            // 
            this.rightTitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rightTitleLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rightTitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rightTitleLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rightTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightTitleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rightTitleLabel.Location = new System.Drawing.Point(1, 1);
            this.rightTitleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.rightTitleLabel.MinimumSize = new System.Drawing.Size(0, 15);
            this.rightTitleLabel.Name = "rightTitleLabel";
            this.rightTitleLabel.Size = new System.Drawing.Size(259, 17);
            this.rightTitleLabel.TabIndex = 0;
            this.rightTitleLabel.Text = "c:\\*.*";
            this.rightTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rightLinkButton
            // 
            this.rightLinkButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightLinkButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rightLinkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightLinkButton.ImageKey = "star";
            this.rightLinkButton.ImageList = this.listImageList;
            this.rightLinkButton.Location = new System.Drawing.Point(260, 0);
            this.rightLinkButton.Margin = new System.Windows.Forms.Padding(0);
            this.rightLinkButton.Name = "rightLinkButton";
            this.rightLinkButton.Size = new System.Drawing.Size(21, 20);
            this.rightLinkButton.TabIndex = 2;
            this.rightLinkButton.UseVisualStyleBackColor = true;
            // 
            // rightHistoryButton
            // 
            this.rightHistoryButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rightHistoryButton.ImageKey = "downList";
            this.rightHistoryButton.ImageList = this.listImageList;
            this.rightHistoryButton.Location = new System.Drawing.Point(281, 0);
            this.rightHistoryButton.Margin = new System.Windows.Forms.Padding(0);
            this.rightHistoryButton.Name = "rightHistoryButton";
            this.rightHistoryButton.Size = new System.Drawing.Size(21, 20);
            this.rightHistoryButton.TabIndex = 1;
            this.rightHistoryButton.UseVisualStyleBackColor = true;
            // 
            // rightHintPanel
            // 
            this.rightHintPanel.AutoSize = true;
            this.rightHintPanel.Controls.Add(this.rightHintLabel);
            this.rightHintPanel.Controls.Add(this.rightRootButton);
            this.rightHintPanel.Controls.Add(this.rightUpButton);
            this.rightHintPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rightHintPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightHintPanel.Location = new System.Drawing.Point(0, 0);
            this.rightHintPanel.MinimumSize = new System.Drawing.Size(0, 20);
            this.rightHintPanel.Name = "rightHintPanel";
            this.rightHintPanel.Size = new System.Drawing.Size(302, 20);
            this.rightHintPanel.TabIndex = 2;
            // 
            // rightHintLabel
            // 
            this.rightHintLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightHintLabel.Location = new System.Drawing.Point(0, 0);
            this.rightHintLabel.Name = "rightHintLabel";
            this.rightHintLabel.Size = new System.Drawing.Size(256, 20);
            this.rightHintLabel.TabIndex = 3;
            this.rightHintLabel.Text = "Title";
            this.rightHintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rightRootButton
            // 
            this.rightRootButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightRootButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rightRootButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightRootButton.Location = new System.Drawing.Point(256, 0);
            this.rightRootButton.Margin = new System.Windows.Forms.Padding(0);
            this.rightRootButton.Name = "rightRootButton";
            this.rightRootButton.Size = new System.Drawing.Size(23, 20);
            this.rightRootButton.TabIndex = 2;
            this.rightRootButton.Text = "\\";
            this.rightRootButton.UseVisualStyleBackColor = true;
            this.rightRootButton.Click += new System.EventHandler(this.rootButton_Click);
            // 
            // rightUpButton
            // 
            this.rightUpButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rightUpButton.Location = new System.Drawing.Point(279, 0);
            this.rightUpButton.Margin = new System.Windows.Forms.Padding(0);
            this.rightUpButton.Name = "rightUpButton";
            this.rightUpButton.Size = new System.Drawing.Size(23, 20);
            this.rightUpButton.TabIndex = 1;
            this.rightUpButton.Text = "..";
            this.rightUpButton.UseVisualStyleBackColor = true;
            this.rightUpButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 350);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.toolStrip);
            this.Menu = this.mainMenu;
            this.Name = "CommanderForm";
            this.Text = "Form";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topSplitContainer.Panel1.ResumeLayout(false);
            this.topSplitContainer.Panel1.PerformLayout();
            this.topSplitContainer.Panel2.ResumeLayout(false);
            this.topSplitContainer.Panel2.PerformLayout();
            this.topSplitContainer.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.leftTitlePanel.ResumeLayout(false);
            this.leftHintListPanel.ResumeLayout(false);
            this.rightTitlePanel.ResumeLayout(false);
            this.rightHintPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.SplitContainer topSplitContainer;
        private System.Windows.Forms.ToolBar leftDrivesToolBar;
        private System.Windows.Forms.ToolBar rightDriveToolBar;
        private System.Windows.Forms.ListView leftListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label testLabel;
        private System.Windows.Forms.ImageList driveToolBarImageList;
        private System.Windows.Forms.ListView rightListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Panel leftHintListPanel;
        private System.Windows.Forms.ImageList listImageList;
        private System.Windows.Forms.Button leftRootButton;
        private System.Windows.Forms.Button leftUpButton;
        private System.Windows.Forms.Panel leftTitlePanel;
        private System.Windows.Forms.Label leftTitleLabel;
        private System.Windows.Forms.Button leftLinkButton;
        private System.Windows.Forms.Button leftHistoryButton;
        private System.Windows.Forms.Label leftHintLabel;
        private System.Windows.Forms.Panel rightTitlePanel;
        private System.Windows.Forms.Label rightTitleLabel;
        private System.Windows.Forms.Button rightLinkButton;
        private System.Windows.Forms.Button rightHistoryButton;
        private System.Windows.Forms.Panel rightHintPanel;
        private System.Windows.Forms.Label rightHintLabel;
        private System.Windows.Forms.Button rightRootButton;
        private System.Windows.Forms.Button rightUpButton;


    }
}

