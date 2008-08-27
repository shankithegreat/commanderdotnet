using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ShellDll;


namespace Commander
{
    public partial class CommanderForm : Form
    {
        private Dictionary<DriveType, int> imageIndexes = new Dictionary<DriveType, int>();
        private FileView selectedFileView = null;

        public CommanderForm()
        {
            InitializeComponent();

            leftDrivesToolBar.Tag = leftFileView;
            rightDriveToolBar.Tag = rightFileView;


            imageIndexes.Add(DriveType.Fixed, 1);
            imageIndexes.Add(DriveType.CDRom, 2);
            imageIndexes.Add(DriveType.Removable, 3);
            imageIndexes.Add(DriveType.Network, 4);

            Load();

            toolStripButton2_Click(null, null);

            drivesToolBar_ButtonClick(leftDrivesToolBar, new ToolBarButtonClickEventArgs(leftDrivesToolBar.Buttons[0]));
            drivesToolBar_ButtonClick(rightDriveToolBar, new ToolBarButtonClickEventArgs(rightDriveToolBar.Buttons[1]));

#if DEBUG
            //TestForm testForem = new TestForm();
            //testForem.Show();
#endif
        }

        private void Load()
        {
            LoadDiskDrives(leftDrivesToolBar);
            LoadDiskDrives(rightDriveToolBar);
        }

        private ToolBarButton CreateDiskDriveButton(DriveInfo drive)
        {
            ToolBarButton button = new ToolBarButton();
            button.Name = string.Format("{0}DriveButton", drive.Name.ToLower());
            button.Text = drive.Name.Remove(drive.Name.Length - 2, 2).ToLower();
            button.Tag = drive;
            button.ImageIndex = imageIndexes[drive.DriveType];            


            return button;
        }
        
        private void LoadDiskDrives(ToolBar toolBar)
        {
            toolBar.Buttons.Clear();

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                ToolBarButton button = CreateDiskDriveButton(d);
                toolBar.Buttons.Add(button);
            }
        }

        private void drivesToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBar toolBar = (ToolBar)sender;
            SetPushedDriveButton(toolBar, e.Button);

            FileView fileView = (FileView)toolBar.Tag;
            DriveInfo drive = (DriveInfo)e.Button.Tag;
            fileView.SelectedDirectory = drive.RootDirectory;
        }

        private void SetPushedDriveButton(ToolBar toolBar, ToolBarButton button)
        {
            int index = toolBar.Buttons.IndexOf(button);

            for (int i = 0; i < leftDrivesToolBar.Buttons.Count; i++)
            {
                toolBar.Buttons[i].Pushed = (i == index);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            leftFileView.ListView.View = View.Details;
            rightFileView.ListView.View = View.Details;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            leftFileView.ListView.View = View.Tile;
            leftFileView.ListView.TileSize = new Size(230, 32);
            rightFileView.ListView.View = View.Tile;
            rightFileView.ListView.TileSize = new Size(230, 32);
            
            leftFileView.ListView.View = View.Details;
            rightFileView.ListView.View = View.Details;

            leftFileView.ListView.View = View.Tile;
            leftFileView.ListView.TileSize = new Size(230, 32);
            rightFileView.ListView.View = View.Tile;
            rightFileView.ListView.TileSize = new Size(230, 32);
        }

        private void leftDrivesToolBar_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = leftDrivesToolBar.GetChildAtPoint(e.Location);
        }

        private void splitContainer_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            //ShowSpliToolTip(e.MouseCursorX, e.MouseCursorY, e.SplitX);
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            topSplitContainer.SplitterDistance = e.SplitX;
        }

        private void ShowSpliToolTip(int x, int y, int splitterDistance)
        {
            int value = (int)(((float)splitterDistance) / (splitContainer.Width - splitContainer.SplitterWidth) * 100);
            splitToolTip.Show(string.Format("{0}%", value), splitContainer, x, y - 20);
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            //ShowSpliToolTip(e.X, e.Y, splitContainer.SplitterDistance);
        }

        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            //splitToolTip.Hide(splitContainer);
        }

        private void cmdComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cmd = cmdComboBox.Text;
                cmdComboBox.Text = string.Empty;
                cmdComboBox.Items.Add(cmd);
                try
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(cmd);
                    psi.WorkingDirectory = rightFileView.SelectedDirectory.FullName;
                    System.Diagnostics.Process.Start(psi);                    
                }
                catch(Exception exp)
                {
                    MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }      
            }
        }

        private void fileView_DirectorySelected(object sender, DirectoryInfo directory)
        {
            FileView fileView = (FileView)sender;
            cmdLabel.Text = string.Format("{0}>", directory.FullName);
        }

        private void fileView_Enter(object sender, EventArgs e)
        {
            FileView fileView = (FileView)sender;
            selectedFileView = fileView;
        }

        private void fileView_Leave(object sender, EventArgs e)
        {
            FileView fileView = (FileView)sender;
            selectedFileView = fileView;
        }        

    }
}
