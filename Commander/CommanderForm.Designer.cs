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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmdComboBox = new System.Windows.Forms.ComboBox();
            this.cmdLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.Controls.Add(this.button7, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.button6, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.button5, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.button4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(670, 27);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // button7
            // 
            this.button7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button7.Location = new System.Drawing.Point(573, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(94, 21);
            this.button7.TabIndex = 7;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button6.Location = new System.Drawing.Point(478, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(89, 21);
            this.button6.TabIndex = 6;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.Location = new System.Drawing.Point(383, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(89, 21);
            this.button5.TabIndex = 5;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Location = new System.Drawing.Point(288, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 21);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(193, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 21);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(98, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 21);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
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
            this.leftFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftFileView.Location = new System.Drawing.Point(0, 0);
            this.leftFileView.Name = "leftFileView";
            this.leftFileView.SelectedDirectory = null;
            this.leftFileView.Size = new System.Drawing.Size(332, 305);
            this.leftFileView.TabIndex = 0;
            this.leftFileView.DirectorySelected += new Commander.DirectorySelectedEventHandler(this.fileView_DirectorySelected);
            this.leftFileView.Leave += new System.EventHandler(this.fileView_Leave);
            this.leftFileView.Enter += new System.EventHandler(this.fileView_Enter);
            // 
            // rightFileView
            // 
            this.rightFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightFileView.Location = new System.Drawing.Point(0, 0);
            this.rightFileView.Name = "rightFileView";
            this.rightFileView.SelectedDirectory = null;
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
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
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
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;


    }
}

