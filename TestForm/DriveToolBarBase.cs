using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Shell;
using TestForm;

namespace Commander
{
    public partial class DriveToolBarBase : ToolBar
    {
        private ShellContextMenu contextMenu = new ShellContextMenu();
        private Dictionary<DriveType, int> imageIndexes = new Dictionary<DriveType, int>();
        private DriveInfo selectedDrive;
        private uint notifyId;
        private delegate void UpdateEventHandler();


        public DriveToolBarBase()
        {
            InitializeComponent();

            imageIndexes.Add(DriveType.Fixed, 1);
            imageIndexes.Add(DriveType.CDRom, 2);
            imageIndexes.Add(DriveType.Removable, 3);
            imageIndexes.Add(DriveType.Network, 4);

            RefreshDrives += LoadDiskDrives;
            OnRefreshDrives();
        }

        ~DriveToolBarBase()
        {
            if (notifyId > 0)
            {
                Shell32.SHChangeNotifyDeregister(notifyId);
                GC.SuppressFinalize(this);
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DriveInfo SelectedDrive
        {
            get
            {
                return selectedDrive;
            }
            set
            {
                if (!Utility.IoHelper.Equals(value, this.selectedDrive))
                {
                    this.selectedDrive = value;

                    ToolBarButton button = GetButtonFromDrive(this.selectedDrive);

                    if (button != null)
                    {
                        SetPushedButton(button);
                        OnSelectedDriveChanged(this.selectedDrive);
                    }
                }
            }
        }


        public event DriveChangedEventHandler SelectedDriveChanged;

        private event UpdateEventHandler RefreshDrives;


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            notifyId = ShellBrowserUpdater.RegisterShellNotify(this.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM.SH_NOTIFY)
            {
                SHNOTIFYSTRUCT shNotify = (SHNOTIFYSTRUCT)Marshal.PtrToStructure(m.WParam, typeof(SHNOTIFYSTRUCT));

                switch ((SHCNE)m.LParam)
                {
                    case SHCNE.DRIVEADD:
                    case SHCNE.DRIVEADDGUI:
                    case SHCNE.DRIVEREMOVED:
                    case SHCNE.MEDIAINSERTED:
                    case SHCNE.MEDIAREMOVED:
                        {
                            OnRefreshDrives();
                            break;
                        }
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnButtonClick(ToolBarButtonClickEventArgs e)
        {
            this.selectedDrive = (DriveInfo)e.Button.Tag;
            SetPushedButton(e.Button);
            OnSelectedDriveChanged(this.selectedDrive);

            base.OnButtonClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolBarButton button = GetButtonAtPoint(e.Location);

                if (button != null)
                {
                    DriveInfo drive = (DriveInfo)button.Tag;

                    Point location = this.PointToScreen(e.Location);
                    contextMenu.Show(location, new[] { drive.RootDirectory.FullName, });
                }
            }

            base.OnMouseUp(e);
        }

        protected virtual void OnSelectedDriveChanged(DriveInfo drive)
        {
            if (SelectedDriveChanged != null)
            {
                SelectedDriveChanged(this, drive);
            }
        }

        protected virtual void OnRefreshDrives()
        {
            if (RefreshDrives != null)
            {
                RefreshDrives();
            }
        }


        private ToolBarButton GetButtonAtPoint(Point point)
        {
            foreach (ToolBarButton button in this.Buttons)
            {
                if (button.Rectangle.Contains(point))
                {
                    return button;
                }
            }

            return null;
        }

        private void LoadDiskDrives()
        {
            this.Buttons.Clear();

            bool selected = false;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                ToolBarButton button = CreateButton(drive);

                if (!selected && Utility.IoHelper.Equals(drive, this.selectedDrive))
                {
                    button.Pushed = true;
                    selected = true;
                }

                this.Buttons.Add(button);
            }

            if (!selected && this.selectedDrive != null && this.Buttons.Count > 0)
            {
                this.SelectedDrive = (DriveInfo)this.Buttons[0].Tag;
            }
        }

        private ToolBarButton CreateButton(DriveInfo drive)
        {
            return new ToolBarButton
            {
                Name = string.Format("{0}DriveButton", drive.Name.ToLower()),
                Text = drive.Name.Remove(drive.Name.Length - 2, 2).ToLower(),
                Tag = drive,
                ImageIndex = imageIndexes[drive.DriveType]
            };
        }

        private ToolBarButton GetButtonFromDrive(DriveInfo drive)
        {
            foreach (ToolBarButton button in this.Buttons)
            {
                DriveInfo d = (DriveInfo)button.Tag;
                if (string.Equals(d.Name, drive.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return button;
                }
            }
            return null;
        }

        private void SetPushedButton(ToolBarButton button)
        {
            int index = this.Buttons.IndexOf(button);

            for (int i = 0; i < this.Buttons.Count; i++)
            {
                this.Buttons[i].Pushed = (i == index);
            }
        }
    }

    public delegate void DriveChangedEventHandler(object sender, DriveInfo drive);
}
