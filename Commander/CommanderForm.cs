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
        private CreateFolderForm createFolderForm = new CreateFolderForm();

        public CommanderForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            leftDrivesToolBar.Tag = leftFileView;
            rightDriveToolBar.Tag = rightFileView;
            leftFileView.Tag = leftDrivesToolBar;
            rightFileView.Tag = rightDriveToolBar;


            imageIndexes.Add(DriveType.Fixed, 1);
            imageIndexes.Add(DriveType.CDRom, 2);
            imageIndexes.Add(DriveType.Removable, 3);
            imageIndexes.Add(DriveType.Network, 4);

            Load();

            toolStripButton2_Click(null, null);

            drivesToolBar_ButtonClick(leftDrivesToolBar, new ToolBarButtonClickEventArgs(leftDrivesToolBar.Buttons[0]));
            drivesToolBar_ButtonClick(rightDriveToolBar, new ToolBarButtonClickEventArgs(rightDriveToolBar.Buttons[1]));
            rightFileView.CurrentDirectory = new DirectoryInfo(@"D:\Projects\Commander\Commander\bin\Debug\");

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

        private FileView GetDestinationFileView()
        {
            return GetLastFileView(selectedFileView);
        }

        private FileView GetLastFileView(FileView first)
        {
            if (first.Equals(leftFileView))
            {
                return rightFileView;
            }
            else
            {
                return leftFileView;
            }
        }

        private ToolBar GetLastDriveToolBar(ToolBar first)
        {
            if (first.Equals(leftDrivesToolBar))
            {
                return rightDriveToolBar;
            }
            else
            {
                return leftDrivesToolBar;
            }
        }

        private void drivesToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBar toolBar = (ToolBar)sender;
            SetPushedDriveButton(toolBar, e.Button);

            FileView fileView = (FileView)toolBar.Tag;
            DriveInfo drive = (DriveInfo)e.Button.Tag;
            SelectDrive(drive, fileView);
            
            ToolBarButton selectedButton = GetDriveToolBarButtonFromDirectory(toolBar, fileView.CurrentDirectory);
            SetPushedDriveButton(toolBar, selectedButton);
        }

        private void SelectDrive(DriveInfo drive, FileView fileView)
        {
            FileView lastFileView = GetLastFileView(fileView);
            if (lastFileView.CurrentDirectory != null && lastFileView.CurrentDirectory.Root.FullName == drive.RootDirectory.FullName)
            {
                fileView.CurrentDirectory = lastFileView.CurrentDirectory;
            }
            else
            {
                fileView.CurrentDirectory = drive.RootDirectory;
            }
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
                    psi.WorkingDirectory = rightFileView.CurrentDirectory.FullName;
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
            SetCmdLabelText(directory);
            
            ToolBar toolBar = (ToolBar)fileView.Tag;
            DriveToolBarSelectDrive(toolBar, directory);
        }

        private void SetCmdLabelText(DirectoryInfo directory)
        {
            cmdLabel.Text = string.Format("{0}>", directory.FullName);
        }

        private ToolBarButton GetDriveToolBarButtonFromDirectory(ToolBar toolBar, DirectoryInfo directory)
        {
            foreach (ToolBarButton button in toolBar.Buttons)
            {
                DriveInfo d = (DriveInfo)button.Tag;
                if (d.RootDirectory.FullName == directory.Root.FullName)
                {
                    return button;
                }
            }
            return null;
        }

        private void DriveToolBarSelectDrive(ToolBar toolBar, DirectoryInfo directory)
        {
            ToolBarButton selectedButton = GetDriveToolBarButtonFromDirectory(toolBar, directory);

            if (selectedButton != null && !selectedButton.Pushed)
            {
                drivesToolBar_ButtonClick(toolBar, new ToolBarButtonClickEventArgs(selectedButton));
            }
        }

        private void fileView_Enter(object sender, EventArgs e)
        {
            FileView fileView = (FileView)sender;
            selectedFileView = fileView;

            SetCmdLabelText(fileView.CurrentDirectory);
        }

        private void fileView_Leave(object sender, EventArgs e)
        {
            FileView fileView = (FileView)sender;
            selectedFileView = fileView;
        }

        private void viewMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            FileView fileView = selectedFileView;
            fileView.Copy();
            GetLastFileView(fileView).Paste();
        }

        private void moveMenuItem_Click(object sender, EventArgs e)
        {
            selectedFileView.Cut();
            GetDestinationFileView().Paste();
        }

        private void createFolderMenuItem_Click(object sender, EventArgs e)
        {
            if (createFolderForm.ShowDialog() == DialogResult.OK)
            {
                if (CreateFolder(selectedFileView.CurrentDirectory, createFolderForm.DirectoryName))
                {
                    selectedFileView.Refresh();
                }
            }
        }

        private bool CreateFolder(DirectoryInfo directory, string localPath)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                return false;
            }

            try
            {
                directory.CreateSubdirectory(localPath);
                return true;                
            }
            catch (Exception e)
            {
                ShowErrorMessage(e);
                return false;
            }
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            selectedFileView.Delete();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowErrorMessage(Exception e)
        {
            ShowErrorMessage(e.Message);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void fileView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && !e.Alt && !e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            FileSystemInfo fsi = selectedFileView.GetFocusedItem();
                            AddToCmd(fsi.Name);
                            e.SuppressKeyPress = true;
                            break;
                        }
                    case Keys.P:
                        {
                            FileSystemInfo fsi = selectedFileView.GetFocusedItem();
                            AddToCmd(ShellFolder.GetParentDirectoryPath(fsi));
                            break;
                        }
                }
            }
        }

        private void AddToCmd(string text)
        {
            if (!string.IsNullOrEmpty(cmdComboBox.Text))
            {
                cmdComboBox.Text += " ";
            }
            cmdComboBox.Text += text;
        }

    }
}
