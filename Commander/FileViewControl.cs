using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShellDll;

namespace Commander
{
    public partial class FileViewControl : UserControl
    {
        public FileViewControl()
        {
            InitializeComponent();

            if (this.Focused)
            {
                FileView_Enter(this, null);
            }
            else
            {
                FileView_Leave(this, null);
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
                return listView.CurrentDirectory;
            }
            set
            {
                listView.CurrentDirectory = value;
            }
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
                return listView.View;
            }
            set
            {
                listView.View = value;
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
                return listView.TileSize;
            }
            set
            {
                listView.TileSize = value;
            }
        }


        public event DirectorySelectedEventHandler CurrentDirectoryChanged;

        public event KeyEventHandler ListViewKeyDown
        {
            add
            {
                listView.KeyDown += value;
            }
            remove
            {
                listView.KeyDown -= value;
            }
        }



        public void Copy()
        {
            listView.Copy();
        }

        public void Paste()
        {
            listView.Paste();
        }

        public void Cut()
        {
            listView.Cut();
        }

        public void Delete()
        {
            listView.Delete();
        }

        public FileSystemInfo GetFocusedItem()
        {
            return listView.GetFocusedItem();
        }


        protected virtual void OnCurrentDirectoryChanged(DirectoryInfo directory)
        {
            if (CurrentDirectoryChanged != null)
            {
                CurrentDirectoryChanged(this, directory);
            }
        }


        private static string GetTitleLabelText(DirectoryInfo directory)
        {
            return Path.Combine(directory.FullName, "*.*");
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            listView.CurrentDirectory = listView.CurrentDirectory.Parent ?? listView.CurrentDirectory;
        }

        private void rootButton_Click(object sender, EventArgs e)
        {
            listView.CurrentDirectory = listView.CurrentDirectory.Root ?? listView.CurrentDirectory;
        }

        private void titleLabel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point location = ((Control)sender).PointToScreen(e.Location);

                listView.ShowCurrentContextMenu(location);
            }
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {
            listView.Focus();
        }

        private void FileView_Enter(object sender, EventArgs e)
        {
            titleLabel.BackColor = SystemColors.ActiveCaption;
            titleLabel.ForeColor = SystemColors.ActiveCaptionText;
        }

        private void FileView_Leave(object sender, EventArgs e)
        {
            titleLabel.BackColor = SystemColors.InactiveCaption;
            titleLabel.ForeColor = SystemColors.InactiveCaptionText;
        }

        private void titleLabel_BeforeEdit(object sender, BeforeEditEventArgs e)
        {
            e.Text = listView.CurrentDirectory.FullName;
        }

        private void titleLabel_AfterEdit(object sender, AfterEditEventArgs e)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(e.Text);

                listView.CurrentDirectory = directory;
            }
            catch (DirectoryNotFoundException)
            {
                e.Cancel = true;
            }
        }

        private void listView_DirectorySelected(object sender, DirectoryInfo directory)
        {
            titleLabel.Text = GetTitleLabelText(directory);

            OnCurrentDirectoryChanged(directory);
        }
    }


    public delegate void DirectorySelectedEventHandler(object sender, DirectoryInfo directory);
}
