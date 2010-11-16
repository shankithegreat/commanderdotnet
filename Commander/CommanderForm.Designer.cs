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
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.cmdControl = new Commander.CommandBar();
            this.buttonCommandPanel = new Commander.ButtonCommandBar();
            this.listImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mainView = new Commander.MainViewControl();
            this.toolStrip = new Commander.CommandToolStrip();
            this.bottomPanel.SuspendLayout();
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
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.cmdControl);
            this.bottomPanel.Controls.Add(this.buttonCommandPanel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 460);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(874, 55);
            this.bottomPanel.TabIndex = 1;
            // 
            // cmdControl
            // 
            this.cmdControl.DataBindings.Add(new System.Windows.Forms.Binding("Lines", global::Commander.Properties.Settings.Default, "CmdLines", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmdControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdControl.Lines = global::Commander.Properties.Settings.Default.CmdLines;
            this.cmdControl.Location = new System.Drawing.Point(0, 0);
            this.cmdControl.Name = "cmdControl";
            this.cmdControl.Size = new System.Drawing.Size(874, 28);
            this.cmdControl.TabIndex = 4;
            this.cmdControl.Title = "";
            this.cmdControl.ComboBoxKeyDown += new System.Windows.Forms.KeyEventHandler(this.cmdControl_ComboBoxKeyDown);
            // 
            // buttonCommandPanel
            // 
            this.buttonCommandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCommandPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCommandPanel.Location = new System.Drawing.Point(0, 28);
            this.buttonCommandPanel.Name = "buttonCommandPanel";
            this.buttonCommandPanel.Size = new System.Drawing.Size(874, 27);
            this.buttonCommandPanel.TabIndex = 5;
            this.buttonCommandPanel.EditClick += new System.EventHandler(this.editMenuItem_Click);
            this.buttonCommandPanel.CopyClick += new System.EventHandler(this.copyMenuItem_Click);
            this.buttonCommandPanel.MoveClick += new System.EventHandler(this.moveMenuItem_Click);
            this.buttonCommandPanel.CreteFolderClick += new System.EventHandler(this.createFolderMenuItem_Click);
            this.buttonCommandPanel.DeleteClick += new System.EventHandler(this.deleteMenuItem_Click);
            this.buttonCommandPanel.ExitClick += new System.EventHandler(this.exitMenuItem_Click);
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
            // mainView
            // 
            this.mainView.DataBindings.Add(new System.Windows.Forms.Binding("LeftPath", global::Commander.Properties.Settings.Default, "LeftPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.mainView.DataBindings.Add(new System.Windows.Forms.Binding("RightPath", global::Commander.Properties.Settings.Default, "RightPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.mainView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainView.LeftPath = global::Commander.Properties.Settings.Default.LeftPath;
            this.mainView.Location = new System.Drawing.Point(0, 25);
            this.mainView.Name = "mainView";
            this.mainView.RightPath = global::Commander.Properties.Settings.Default.RightPath;
            this.mainView.Size = new System.Drawing.Size(874, 435);
            this.mainView.TabIndex = 2;
            this.mainView.TileSize = new System.Drawing.Size(220, 36);
            this.mainView.View = System.Windows.Forms.View.Details;
            this.mainView.CurrentDirectoryChanged += new Commander.DirectorySelectedEventHandler(this.mainView_DirectorySelected);
            this.mainView.FileViewKeyDown += new System.Windows.Forms.KeyEventHandler(this.fileView_KeyDown);
            // 
            // toolStrip
            // 
            this.toolStrip.AllowDrop = true;
            this.toolStrip.CommandButtons = global::Commander.Properties.Settings.Default.CommandButtons;
            this.toolStrip.DataBindings.Add(new System.Windows.Forms.Binding("CommandButtons", global::Commander.Properties.Settings.Default, "CommandButtons", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(874, 25);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "toolStrip";
            this.toolStrip.ButtonClick += new Commander.ButtonClickEventHandler(this.toolStrip_ButtonClick);
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = global::Commander.Properties.Settings.Default.Size;
            this.Controls.Add(this.mainView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.bottomPanel);
            this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::Commander.Properties.Settings.Default, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Menu = this.mainMenu;
            this.Name = "CommanderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommanderForm_FormClosing);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.ImageList listImageList;
        private System.Windows.Forms.ToolTip splitToolTip;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem viewMenuItem;
        private System.Windows.Forms.MenuItem editMenuItem;
        private System.Windows.Forms.MenuItem copyMenuItem;
        private System.Windows.Forms.MenuItem moveMenuItem;
        private System.Windows.Forms.MenuItem createFolderMenuItem;
        private System.Windows.Forms.MenuItem deleteMenuItem;
        private MainViewControl mainView;
        private CommandToolStrip toolStrip;
        private CommandBar cmdControl;
        private ButtonCommandBar buttonCommandPanel;


    }
}

