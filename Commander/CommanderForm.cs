using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public partial class CommanderForm : Form
    {

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
            TestForm testForem = new TestForm();
            testForem.Show();
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
            SetPushedDriveButton((ToolBar)sender, e.Button);
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
    }
}
