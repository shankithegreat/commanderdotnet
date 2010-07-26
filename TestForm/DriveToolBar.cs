using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Commander;
using TestForm.Messages;

namespace TestForm
{
    public partial class DriveToolBar : DriveToolBarBase
    {
        public DriveToolBar()
        {
            InitializeComponent();

            MessageDispatcher.Dispatcher.Subscribe(this);
        }


        private void DriveToolBar_SelectedDriveChanged(object sender, DriveInfo drive)
        {
            MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(drive.RootDirectory.FullName));
        }


        [DirectorySelected]
        private void dispatcher_DirectorySelected(DirectorySelectedArgs e)
        {
            if (Directory.Exists(e.SelectedDirectory))
            {
                string root = Directory.GetDirectoryRoot(e.SelectedDirectory);

                if (this.SelectedDrive == null || this.SelectedDrive.RootDirectory.FullName != root)
                {
                    this.SelectedDrive = new DriveInfo(root);
                }
            }
        }
    }
}
