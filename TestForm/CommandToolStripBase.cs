using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Shell;

namespace Commander
{
    public partial class CommandToolStripBase : ToolStrip
    {
        private string commandButtons = string.Empty;
        private List<ToolStripButton> items = new List<ToolStripButton>();


        public CommandToolStripBase()
        {
            InitializeComponent();
        }


        public event ButtonClickEventHandler ButtonClick;

        public event EventHandler CommandButtonsChanged;


        [Category("Data")]
        [DefaultValue("")]
        public string CommandButtons
        {
            get
            {
                StringBuilder list = new StringBuilder(this.Items.Count);
                foreach (ToolStripButton button in items)
                {
                    String path = (string)button.Tag;

                    if (list.Length > 0)
                    {
                        list.Append('?');
                    }
                    list.Append(path);
                }

                return list.ToString();
            }
            set
            {
                if (value != commandButtons)
                {
                    commandButtons = value;
                    LoadButtons(commandButtons);
                    OnCommandButtonsChanged(EventArgs.Empty);
                }
            }
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCommandButtonsChanged(EventArgs e)
        {
            if (CommandButtonsChanged != null)
            {
                CommandButtonsChanged(this, e);
            }
        }

        protected virtual void OnButtonClick(string path, string[] args)
        {
            if (ButtonClick != null)
            {
                ButtonClick(path, args);
            }
        }


        private void LoadButtons(string value)
        {
            foreach (ToolStripButton item in items)
            {
                this.Items.Remove(item);
            }

            items.Clear();
            if (!string.IsNullOrEmpty(value))
            {
                string[] list = value.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String path in list)
                {
                    ToolStripButton button = CreateButton(path);
                    this.Items.Add(button);
                    items.Add(button);
                }
            }
        }

        private ToolStripButton CreateButton(string path)
        {
            ToolStripButton button = new ToolStripButton();
            button.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button.Image = GetImage(path);
            button.Text = path;
            button.Tag = path;
            button.MouseUp += delegate(object bsender, MouseEventArgs be)
            {
                if (bsender is ToolStripButton && be.Button == MouseButtons.Right)
                {
                    ToolStripButton b = (ToolStripButton)bsender;

                    contextMenuStrip.Tag = b;
                    contextMenuStrip.Show(this, b.Bounds.X + be.X, b.Bounds.Y + be.Y);
                }
            };
            button.Click += delegate(object bsender, EventArgs be)
            {
                if (bsender is ToolStripButton)
                {
                    ToolStripButton b = (ToolStripButton)bsender;
                    OnButtonClick((string)b.Tag, new string[0]);
                }
            };

            return button;
        }

        private Image GetImage(String path)
        {
            try
            {
                return ShellHelper.GetSmallAssociatedIcon(path).ToBitmap();
            }
            catch
            {
            }

            return ((Image)(TestForm.Properties.Resources.ResourceManager.GetObject("toolStripButton1.Image")));
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripButton item = (ToolStripButton)contextMenuStrip.Tag;
            contextMenuStrip.Tag = null;
            this.Items.Remove(item);
            items.Remove(item);
            OnCommandButtonsChanged(EventArgs.Empty);
        }

        private void CommandToolStrip_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void CommandToolStrip_DragDrop(object sender, DragEventArgs e)
        {
            Point point = this.PointToClient(new Point(e.X, e.Y));
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            ToolStripItem item = this.GetItemAt(point);
            if (item != null && item is ToolStripButton)
            {
                String path = (String)item.Tag;
                if (path != null)
                {
                    OnButtonClick(path, files);
                }
            }
            else if (files.Length > 0)
            {
                int index;
                if (item is ToolStripSeparator)
                {
                    index = this.Items.IndexOf(item);
                }
                else
                {
                    index = this.Items.Count;
                }

                ToolStripButton button = CreateButton(files[0]);
                this.Items.Insert(index, button);
                items.Add(button);
                OnCommandButtonsChanged(EventArgs.Empty);
            }
        }
    }


    public delegate void ButtonClickEventHandler(string path, string[] args);
}
