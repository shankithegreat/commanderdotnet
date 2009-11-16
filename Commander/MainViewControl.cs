using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Commander
{
    public partial class MainViewControl : UserControl
    {
        private FileViewControl selectedFileView;


        public MainViewControl()
        {
            InitializeComponent();

            leftDrivesToolBar.Tag = leftFileView;
            rightDriveToolBar.Tag = rightFileView;
            leftFileView.Tag = leftDrivesToolBar;
            rightFileView.Tag = rightDriveToolBar;

            leftDrivesToolBar.SelectedDrive = DriveInfo.GetDrives()[0];
            rightDriveToolBar.SelectedDrive = DriveInfo.GetDrives()[1];
        }


        /// <summary>
        /// Gets or sets how items are displayed in the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(View.LargeIcon)]
        public View View
        {
            get
            {
                return leftFileView.View;
            }
            set
            {
                leftFileView.View = value;
                rightFileView.View = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the tiles shown in tile view.
        /// </summary>
        [Category("Appearance")]
        public Size TileSize
        {
            get
            {
                return leftFileView.TileSize;
            }
            set
            {
                leftFileView.TileSize = value;
                rightFileView.TileSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory of left file view.
        /// </summary>
        [Category("Appearance")]
        public string LeftPath
        {
            get
            {
                return (leftFileView.CurrentDirectory != null ? leftFileView.CurrentDirectory.FullName : string.Empty);
            }
            set
            {
                if (leftFileView.CurrentDirectory == null || leftFileView.CurrentDirectory.FullName != value)
                {
                    leftFileView.CurrentDirectory = new DirectoryInfo(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory of right file view.
        /// </summary>
        [Category("Appearance")]
        public string RightPath
        {
            get
            {
                return (rightFileView.CurrentDirectory != null ? rightFileView.CurrentDirectory.FullName : string.Empty);
            }
            set
            {
                if (rightFileView.CurrentDirectory == null || rightFileView.CurrentDirectory.FullName != value)
                {
                    rightFileView.CurrentDirectory = new DirectoryInfo(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the directory which is displayed in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DirectoryInfo CurrentDirectory
        {
            get
            {
                if (selectedFileView != null)
                {
                    return selectedFileView.CurrentDirectory;
                }

                return null;
            }
            set
            {
                if (selectedFileView != null)
                {
                    selectedFileView.CurrentDirectory = value;
                }
            }
        }


        private FileViewControl SelectedFileView
        {
            get
            {
                return this.selectedFileView;
            }
            set
            {
                if (value != this.selectedFileView)
                {
                    this.selectedFileView = value;

                    if (this.selectedFileView != null)
                    {
                        OnCurrentDirectoryChanged(this.selectedFileView.CurrentDirectory);
                    }
                }
            }
        }


        public event EventHandler LeftPathChanged;

        public event EventHandler RightPathChanged;

        public event DirectorySelectedEventHandler CurrentDirectoryChanged;

        public event KeyEventHandler FileViewKeyDown
        {
            add
            {
                leftFileView.ListViewKeyDown += value;
                rightFileView.ListViewKeyDown += value;
            }
            remove
            {
                leftFileView.ListViewKeyDown -= value;
                rightFileView.ListViewKeyDown -= value;
            }
        }


        public void Copy()
        {
            FileViewControl fileView = selectedFileView;
            fileView.Copy();
            GetLastFileView(fileView).Paste();
        }

        public void Paste()
        {
            selectedFileView.Paste();
        }

        public void MoveItem()
        {
            selectedFileView.Cut();
            GetDestinationFileView().Paste();
        }

        public void Delete()
        {
            selectedFileView.Delete();
        }

        public FileSystemInfo GetFocusedItem()
        {
            return selectedFileView.GetFocusedItem();
        }


        protected virtual void OnLeftPathChanged(EventArgs e)
        {
            if (LeftPathChanged != null)
            {
                LeftPathChanged(this, e);
            }
        }

        protected virtual void OnRightPathChanged(EventArgs e)
        {
            if (RightPathChanged != null)
            {
                RightPathChanged(this, e);
            }
        }

        protected virtual void OnCurrentDirectoryChanged(DirectoryInfo directory)
        {
            if (CurrentDirectoryChanged != null)
            {
                CurrentDirectoryChanged(this, directory);
            }
        }


        private FileViewControl GetDestinationFileView()
        {
            return GetLastFileView(selectedFileView);
        }

        private FileViewControl GetLastFileView(FileViewControl first)
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

        private void ShowSpliToolTip(int x, int y, int splitterDistance)
        {
            int value = (int)(((float)splitterDistance) / (splitContainer.Width - splitContainer.SplitterWidth) * 100);
            splitToolTip.Show(string.Format("{0}%", value), splitContainer, x, y - 20);
        }

        private void splitContainer_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            //ShowSpliToolTip(e.MouseCursorX, e.MouseCursorY, e.SplitX);
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            topSplitContainer.SplitterDistance = e.SplitX;
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            //ShowSpliToolTip(e.X, e.Y, splitContainer.SplitterDistance);
        }

        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            //splitToolTip.Hide(splitContainer);
        }

        private void drivesToolBar_DriveChanged(object sender, DriveInfo drive)
        {
            ToolBar toolBar = (ToolBar)sender;

            FileViewControl fileView = (FileViewControl)toolBar.Tag;
            FileViewControl lastFileView = GetLastFileView(fileView);

            if (lastFileView.CurrentDirectory != null && lastFileView.CurrentDirectory.Root.FullName == drive.RootDirectory.FullName)
            {
                fileView.CurrentDirectory = lastFileView.CurrentDirectory;
            }
            else
            {
                try
                {
                    fileView.CurrentDirectory = drive.RootDirectory;
                }
                catch (DirectoryNotFoundException)
                {
                    FileView_CurrentDirectoryChanged(fileView, fileView.CurrentDirectory);
                }
            }
        }

        private void FileView_CurrentDirectoryChanged(object sender, DirectoryInfo directory)
        {
            FileViewControl fileView = (FileViewControl)sender;

            DriveToolBar toolBar = (DriveToolBar)fileView.Tag;
            toolBar.SelectedDrive = Utility.IoHelper.GetDrive(directory);

            if (fileView == leftFileView)
            {
                OnLeftPathChanged(EventArgs.Empty);
            }
            else if (fileView == rightFileView)
            {
                OnRightPathChanged(EventArgs.Empty);
            }

            OnCurrentDirectoryChanged(directory);
        }

        private void fileView_Enter(object sender, EventArgs e)
        {
            FileViewControl fileView = (FileViewControl)sender;
            this.SelectedFileView = fileView;
        }
    }
}
