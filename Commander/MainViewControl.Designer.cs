namespace Commander
{
    partial class MainViewControl
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
            this.topPanel = new System.Windows.Forms.Panel();
            this.topSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftDrivesToolBar = new Commander.DriveToolBar();
            this.rightDriveToolBar = new Commander.DriveToolBar();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.leftFileView = new Commander.FileViewControl();
            this.rightFileView = new Commander.FileViewControl();
            this.splitToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.topPanel.SuspendLayout();
            this.topSplitContainer.Panel1.SuspendLayout();
            this.topSplitContainer.Panel2.SuspendLayout();
            this.topSplitContainer.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.topSplitContainer);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(561, 33);
            this.topPanel.TabIndex = 3;
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
            this.topSplitContainer.Size = new System.Drawing.Size(561, 33);
            this.topSplitContainer.SplitterDistance = 277;
            this.topSplitContainer.TabIndex = 0;
            // 
            // leftDrivesToolBar
            // 
            this.leftDrivesToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.leftDrivesToolBar.AutoSize = false;
            this.leftDrivesToolBar.ButtonSize = new System.Drawing.Size(36, 21);
            this.leftDrivesToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftDrivesToolBar.DropDownArrows = true;
            this.leftDrivesToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftDrivesToolBar.Location = new System.Drawing.Point(0, 0);
            this.leftDrivesToolBar.Name = "leftDrivesToolBar";
            this.leftDrivesToolBar.ShowToolTips = true;
            this.leftDrivesToolBar.Size = new System.Drawing.Size(277, 42);
            this.leftDrivesToolBar.TabIndex = 0;
            this.leftDrivesToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.leftDrivesToolBar.SelectedDriveChanged += new Commander.DriveChangedEventHandler(this.drivesToolBar_DriveChanged);
            // 
            // rightDriveToolBar
            // 
            this.rightDriveToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.rightDriveToolBar.AutoSize = false;
            this.rightDriveToolBar.ButtonSize = new System.Drawing.Size(36, 21);
            this.rightDriveToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightDriveToolBar.DropDownArrows = true;
            this.rightDriveToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightDriveToolBar.Location = new System.Drawing.Point(0, 0);
            this.rightDriveToolBar.Name = "rightDriveToolBar";
            this.rightDriveToolBar.ShowToolTips = true;
            this.rightDriveToolBar.Size = new System.Drawing.Size(280, 42);
            this.rightDriveToolBar.TabIndex = 1;
            this.rightDriveToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.rightDriveToolBar.SelectedDriveChanged += new Commander.DriveChangedEventHandler(this.drivesToolBar_DriveChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer.Location = new System.Drawing.Point(0, 33);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.leftFileView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rightFileView);
            this.splitContainer.Size = new System.Drawing.Size(561, 287);
            this.splitContainer.SplitterDistance = 277;
            this.splitContainer.TabIndex = 4;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            this.splitContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseDown);
            this.splitContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseUp);
            // 
            // leftFileView
            // 
            this.leftFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftFileView.Location = new System.Drawing.Point(0, 0);
            this.leftFileView.Name = "leftFileView";
            this.leftFileView.Size = new System.Drawing.Size(277, 287);
            this.leftFileView.TabIndex = 0;
            this.leftFileView.TileSize = new System.Drawing.Size(0, 0);
            this.leftFileView.CurrentDirectoryChanged += new Commander.DirectorySelectedEventHandler(this.FileView_CurrentDirectoryChanged);
            this.leftFileView.Leave += new System.EventHandler(this.fileView_Enter);
            this.leftFileView.Enter += new System.EventHandler(this.fileView_Enter);
            // 
            // rightFileView
            // 
            this.rightFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightFileView.Location = new System.Drawing.Point(0, 0);
            this.rightFileView.Name = "rightFileView";
            this.rightFileView.Size = new System.Drawing.Size(280, 287);
            this.rightFileView.TabIndex = 1;
            this.rightFileView.TileSize = new System.Drawing.Size(0, 0);
            this.rightFileView.CurrentDirectoryChanged += new Commander.DirectorySelectedEventHandler(this.FileView_CurrentDirectoryChanged);
            this.rightFileView.Leave += new System.EventHandler(this.fileView_Enter);
            this.rightFileView.Enter += new System.EventHandler(this.fileView_Enter);
            // 
            // MainViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Name = "MainViewControl";
            this.Size = new System.Drawing.Size(561, 320);
            this.topPanel.ResumeLayout(false);
            this.topSplitContainer.Panel1.ResumeLayout(false);
            this.topSplitContainer.Panel2.ResumeLayout(false);
            this.topSplitContainer.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.SplitContainer topSplitContainer;
        private DriveToolBar leftDrivesToolBar;
        private DriveToolBar rightDriveToolBar;
        private System.Windows.Forms.SplitContainer splitContainer;
        private FileViewControl leftFileView;
        private FileViewControl rightFileView;
        private System.Windows.Forms.ToolTip splitToolTip;
    }
}
