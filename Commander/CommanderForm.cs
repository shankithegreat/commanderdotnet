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
        private ContextMenu contextMenu = new ContextMenu();

        private Dictionary<DriveType, int> imageIndexes = new Dictionary<DriveType, int>();

        public CommanderForm()
        {
            InitializeComponent();

            imageIndexes.Add(DriveType.Removable, 0);
            imageIndexes.Add(DriveType.Fixed, 1);
            imageIndexes.Add(DriveType.CDRom, 2);
            imageIndexes.Add(DriveType.Network, 3);

            Load();

#if DEBUG
            //TestForm testForem = new TestForm();
            //testForem.Show();
#endif
        }

        private void Load()
        {
            ShellImageList.SetSmallImageList(leftListView);
            ShellImageList.SetLargeImageList(leftListView);
            ShellImageList.SetSmallImageList(rightListView);
            ShellImageList.SetLargeImageList(rightListView);
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
            SetPushedDriveButton((ToolBar)sender, e.Button);

            DriveInfo drive = (DriveInfo)e.Button.Tag;            
            LoadDirectory(drive.RootDirectory);
        }

        private void SetPushedDriveButton(ToolBar toolBar, ToolBarButton button)
        {
            int index = toolBar.Buttons.IndexOf(button);

            for (int i = 0; i < leftDrivesToolBar.Buttons.Count; i++)
            {
                leftDrivesToolBar.Buttons[i].Pushed = (i == index);
                rightDriveToolBar.Buttons[i].Pushed = leftDrivesToolBar.Buttons[i].Pushed;
            }
        }

        private void LoadDirectory(DirectoryInfo directory)
        {
            leftListView.Items.Clear();
            rightListView.Items.Clear();

            foreach (FileSystemInfo fsi in directory.GetFileSystemInfos())
            {
                ListViewItem item = leftListView.Items.Add(fsi.Name, SafeNativeMethods.GetAssociatedIconIndex(fsi.FullName));
                item.Tag = fsi;
                ListViewItem item2 = rightListView.Items.Add(fsi.Name, SafeNativeMethods.GetAssociatedIconIndex(fsi.FullName));
                item2.Tag = fsi;
            }
        }

        private void listView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && sender is ListView)
            {
                ListView listView = (ListView)sender;
                if (leftListView.SelectedItems.Count > 0)
                {
                    Point location = listView.PointToScreen(e.Location);

                    List<string> list = new List<string>(listView.SelectedItems.Count);
                    foreach (ListViewItem selectItem in listView.SelectedItems)
                    {
                        FileSystemInfo fsi = (FileSystemInfo)selectItem.Tag;
                        list.Add(fsi.FullName);
                    }

                    contextMenu.Show(location, list.ToArray());
                }
            }
        }

        

        
        
    }
}
