namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class SelectButtonImage : BasicDialog
    {
        private Button btnBrowse;
        private Button btnCancel;
        private Button btnOk;
        private Button btnReset;
        private Bevel bvlButtons;
        private IContainer components;
        private ListViewItem[] DefaultItems;
        private Size ImageSize;
        private ListViewEx lvDefault;
        private ListViewEx lvUserDefined;
        private OpenFileDialog OpenImageDialog;
        private RadioButton rbDefault;
        private RadioButton rbNoImage;
        private RadioButton rbUserDefined;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpButtons;
        private TextBox txtUserDefinedImage;
        private IDisposableContainer UserDefinedContainer;
        private ListViewItem[] UserDefinedItems;

        public SelectButtonImage()
        {
            EventHandler handler = null;
            this.components = null;
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (DictionaryEntry entry in IconSet.IconResourceSet)
            {
                Image image = entry.Value as Image;
                if (image != null)
                {
                    if (this.ImageSize.IsEmpty)
                    {
                        this.ImageSize = image.Size;
                    }
                    ListViewItem item = new ListViewItem {
                        Tag = image,
                        ImageKey = (string) entry.Key
                    };
                    list.Add(item);
                }
            }
            this.DefaultItems = list.ToArray();
            if (this.ImageSize.IsEmpty)
            {
                this.ImageSize = new Size(0x10, 0x10);
            }
            this.lvDefault.HandleCreated += new EventHandler(this.ImageListView_HandleCreated);
            this.lvUserDefined.HandleCreated += new EventHandler(this.ImageListView_HandleCreated);
            if (handler == null)
            {
                handler = delegate (object sender, EventArgs e) {
                    if (this.UserDefinedContainer != null)
                    {
                        this.UserDefinedContainer.Dispose();
                    }
                };
            }
            base.Disposed += handler;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if ((this.OpenImageDialog.ShowDialog(this) == DialogResult.OK) && !(this.txtUserDefinedImage.Text == this.OpenImageDialog.FileName))
            {
                base.UseWaitCursor = true;
                Application.DoEvents();
                base.UseWaitCursor = false;
                this.LoadUserDefined(this.OpenImageDialog.FileName);
                this.UpdateButtons();
                if (this.lvUserDefined.Enabled)
                {
                    ListViewItem item = this.lvUserDefined.Items[0];
                    this.lvUserDefined.Tag = item;
                    item.Focus(false, true);
                    this.lvUserDefined.Select();
                    this.btnOk.Enabled = true;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string defaultImageName = this.DefaultImageName;
            if (string.IsNullOrEmpty(defaultImageName))
            {
                this.rbNoImage.Checked = true;
            }
            else
            {
                ListViewItem focusItem = this.GetFocusItem(this.DefaultItems, new IconLocation(defaultImageName, -1));
                if (focusItem != null)
                {
                    this.rbDefault.Checked = true;
                    focusItem.Focus(false, true);
                    this.lvDefault.Select();
                    this.lvDefault.Tag = focusItem;
                    this.btnReset.Enabled = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private ListViewItem GetFocusItem(IEnumerable<ListViewItem> items, IconLocation imageLocation)
        {
            if (items != null)
            {
                string iconFileName = imageLocation.IconFileName;
                if (imageLocation.IconIndex >= 0)
                {
                    iconFileName = iconFileName + ',' + imageLocation.IconIndex.ToString();
                }
                foreach (ListViewItem item in items)
                {
                    if (item.ImageKey.Equals(iconFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        private void ImageListView_HandleCreated(object sender, EventArgs e)
        {
            ((ListViewEx) sender).SetIconSpacing(20, 20);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SelectButtonImage));
            this.rbNoImage = new RadioButton();
            this.rbDefault = new RadioButton();
            this.lvDefault = new ListViewEx();
            this.rbUserDefined = new RadioButton();
            this.btnBrowse = new Button();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.tlpBack = new TableLayoutPanel();
            this.txtUserDefinedImage = new TextBox();
            this.lvUserDefined = new ListViewEx();
            this.btnReset = new Button();
            this.OpenImageDialog = new OpenFileDialog();
            this.tlpButtons = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            this.tlpBack.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.rbNoImage, "rbNoImage");
            this.tlpBack.SetColumnSpan(this.rbNoImage, 3);
            this.rbNoImage.Name = "rbNoImage";
            this.rbNoImage.TabStop = true;
            this.rbNoImage.UseVisualStyleBackColor = true;
            this.rbNoImage.CheckedChanged += new EventHandler(this.rbNoImage_CheckedChanged);
            manager.ApplyResources(this.rbDefault, "rbDefault");
            this.tlpBack.SetColumnSpan(this.rbDefault, 3);
            this.rbDefault.Name = "rbDefault";
            this.rbDefault.TabStop = true;
            this.rbDefault.UseVisualStyleBackColor = true;
            this.rbDefault.CheckedChanged += new EventHandler(this.rbOtherImage_CheckedChanged);
            this.tlpBack.SetColumnSpan(this.lvDefault, 2);
            manager.ApplyResources(this.lvDefault, "lvDefault");
            this.lvDefault.MultiSelect = false;
            this.lvDefault.Name = "lvDefault";
            this.lvDefault.OwnerDraw = true;
            this.lvDefault.UseCompatibleStateImageBehavior = false;
            this.lvDefault.DrawItem += new DrawListViewItemEventHandler(this.lvImage_DrawItem);
            this.lvDefault.SelectedIndexChanged += new EventHandler(this.lvImage_SelectedIndexChanged);
            manager.ApplyResources(this.rbUserDefined, "rbUserDefined");
            this.tlpBack.SetColumnSpan(this.rbUserDefined, 2);
            this.rbUserDefined.Name = "rbUserDefined";
            this.rbUserDefined.TabStop = true;
            this.rbUserDefined.UseVisualStyleBackColor = true;
            this.rbUserDefined.CheckedChanged += new EventHandler(this.rbOtherImage_CheckedChanged);
            manager.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.rbNoImage, 0, 0);
            this.tlpBack.Controls.Add(this.btnBrowse, 2, 4);
            this.tlpBack.Controls.Add(this.rbDefault, 0, 1);
            this.tlpBack.Controls.Add(this.rbUserDefined, 0, 3);
            this.tlpBack.Controls.Add(this.txtUserDefinedImage, 1, 4);
            this.tlpBack.Controls.Add(this.lvDefault, 1, 2);
            this.tlpBack.Controls.Add(this.lvUserDefined, 1, 5);
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.txtUserDefinedImage, "txtUserDefinedImage");
            this.txtUserDefinedImage.Name = "txtUserDefinedImage";
            this.txtUserDefinedImage.ReadOnly = true;
            this.txtUserDefinedImage.TabStop = false;
            this.tlpBack.SetColumnSpan(this.lvUserDefined, 2);
            manager.ApplyResources(this.lvUserDefined, "lvUserDefined");
            this.lvUserDefined.MultiSelect = false;
            this.lvUserDefined.Name = "lvUserDefined";
            this.lvUserDefined.OwnerDraw = true;
            this.lvUserDefined.UseCompatibleStateImageBehavior = false;
            this.lvUserDefined.DrawItem += new DrawListViewItemEventHandler(this.lvImage_DrawItem);
            this.lvUserDefined.SelectedIndexChanged += new EventHandler(this.lvImage_SelectedIndexChanged);
            manager.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new EventHandler(this.btnReset_Click);
            manager.ApplyResources(this.OpenImageDialog, "OpenImageDialog");
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnReset, 2, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 3, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpBack);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpButtons);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SelectButtonImage";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.SelectButtonImage_Shown);
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadUserDefined(string fileName)
        {
            ListViewItem item;
            this.txtUserDefinedImage.Text = fileName;
            bool flag = false;
            string str = Path.GetExtension(fileName).ToLower();
            if ((str != null) && (((str == ".dll") || (str == ".exe")) || (str == ".ico")))
            {
                flag = true;
            }
            List<ListViewItem> list = new List<ListViewItem>();
            if (!flag)
            {
                try
                {
                    Image original = Image.FromFile(fileName);
                    if (original.Size != this.ImageSize)
                    {
                        original = new Bitmap(original, this.ImageSize);
                    }
                    item = new ListViewItem {
                        Tag = original,
                        ImageKey = fileName
                    };
                    list.Add(item);
                }
                catch
                {
                    flag = true;
                }
            }
            if (flag)
            {
                int iconIndex = 0;
                for (Image image2 = CustomImageProvider.LoadIcon(fileName, iconIndex, this.ImageSize); image2 != null; image2 = CustomImageProvider.LoadIcon(fileName, ++iconIndex, this.ImageSize))
                {
                    item = new ListViewItem {
                        Tag = image2,
                        ImageKey = fileName + ',' + iconIndex.ToString()
                    };
                    list.Add(item);
                }
            }
            this.UserDefinedItems = list.ToArray();
            if (this.UserDefinedContainer != null)
            {
                this.UserDefinedContainer.Dispose();
            }
            this.UserDefinedContainer = new DisposableContainer();
            foreach (ListViewItem item2 in this.UserDefinedItems)
            {
                this.UserDefinedContainer.Add((IDisposable) item2.Tag);
            }
            if (this.lvUserDefined.IsHandleCreated)
            {
                this.lvUserDefined.BeginUpdate();
                try
                {
                    this.lvUserDefined.Items.Clear();
                    this.lvUserDefined.Items.AddRange(this.UserDefinedItems);
                }
                finally
                {
                    this.lvUserDefined.EndUpdate();
                }
            }
        }

        private void lvImage_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Rectangle bounds = e.Bounds;
            bounds.Height = bounds.Width;
            if ((e.State & ListViewItemStates.Focused) > 0)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds);
            }
            Image tag = (Image) e.Item.Tag;
            e.Graphics.DrawImage(tag, (int) (bounds.Left + ((bounds.Width - tag.Width) / 2)), (int) (bounds.Top + ((bounds.Height - tag.Height) / 2)));
            if ((e.State & ListViewItemStates.Focused) > 0)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, bounds, SystemColors.HighlightText, SystemColors.Highlight);
            }
        }

        private void lvImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView view = (ListView) sender;
            view.Tag = view.FocusedItem;
            this.btnOk.Enabled = view.Tag != null;
            this.UpdateResetButton();
        }

        private void rbNoImage_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
        }

        private void rbOtherImage_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
            this.tlpBack.SelectNextControl((Control) sender, true, true, false, false);
        }

        private void SelectButtonImage_Shown(object sender, EventArgs e)
        {
            if (((this.lvDefault.Items.Count == 0) && (this.DefaultItems != null)) && (this.DefaultItems.Length > 0))
            {
                this.lvDefault.Items.AddRange(this.DefaultItems);
            }
            if (((this.lvUserDefined.Items.Count == 0) && (this.UserDefinedItems != null)) && (this.UserDefinedItems.Length > 0))
            {
                this.lvUserDefined.Items.AddRange(this.UserDefinedItems);
            }
            this.UpdateButtons();
            ListViewItem tag = null;
            if (this.rbDefault.Checked && this.lvDefault.Enabled)
            {
                tag = this.lvDefault.Tag as ListViewItem;
            }
            if (this.rbUserDefined.Checked && this.lvUserDefined.Enabled)
            {
                tag = this.lvUserDefined.Tag as ListViewItem;
            }
            if (tag != null)
            {
                tag.Focus(false, true);
                tag.ListView.Select();
            }
            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            this.lvDefault.Enabled = this.rbDefault.Checked;
            this.txtUserDefinedImage.Enabled = this.rbUserDefined.Checked;
            this.btnBrowse.Enabled = this.rbUserDefined.Checked;
            this.lvUserDefined.Enabled = this.rbUserDefined.Checked && (this.lvUserDefined.Items.Count > 0);
            this.btnOk.Enabled = (this.rbNoImage.Checked || ((this.rbDefault.Checked && this.lvDefault.Enabled) && (this.lvDefault.Tag != null))) || ((this.rbUserDefined.Checked && this.lvUserDefined.Enabled) && (this.lvUserDefined.Tag != null));
            this.UpdateResetButton();
        }

        private void UpdateResetButton()
        {
            if (string.IsNullOrEmpty(this.DefaultImageName))
            {
                this.btnReset.Enabled = !this.rbNoImage.Checked;
            }
            else
            {
                this.btnReset.Enabled = ((!this.rbDefault.Checked || !this.lvDefault.Enabled) || (this.lvDefault.FocusedItem == null)) || (this.lvDefault.FocusedItem.ImageKey != this.DefaultImageName);
            }
        }

        public string DefaultImageName
        {
            get
            {
                return (this.rbDefault.Tag as string);
            }
            set
            {
                this.rbDefault.Tag = value;
                if (string.IsNullOrEmpty(value))
                {
                    this.rbNoImage.Checked = true;
                }
                else
                {
                    foreach (ListViewItem item in this.DefaultItems)
                    {
                        if (item.ImageKey == this.DefaultImageName)
                        {
                            this.rbDefault.Checked = true;
                            this.lvDefault.Tag = item;
                            break;
                        }
                    }
                }
            }
        }

        public IconLocation ImageLocation
        {
            get
            {
                ListViewItem tag = this.lvDefault.Tag as ListViewItem;
                if (this.lvDefault.Enabled && (tag != null))
                {
                    return new IconLocation(tag.ImageKey, -1);
                }
                tag = this.lvUserDefined.Tag as ListViewItem;
                if (this.lvUserDefined.Enabled && (tag != null))
                {
                    IconLocation location = IconLocation.TryParse(tag.ImageKey);
                    if (!(((location == null) || (location.IconIndex != 0)) || tag.ImageKey.EndsWith(",0", StringComparison.Ordinal)))
                    {
                        location = new IconLocation(location.IconFileName, -1);
                    }
                    return location;
                }
                return null;
            }
            set
            {
                this.rbNoImage.Checked = true;
                if (value != null)
                {
                    if (Path.IsPathRooted(value.IconFileName))
                    {
                        this.LoadUserDefined(value.IconFileName);
                        this.lvUserDefined.Tag = this.GetFocusItem(this.UserDefinedItems, value);
                        this.rbUserDefined.Checked = this.lvUserDefined.Tag != null;
                    }
                    else
                    {
                        this.lvDefault.Tag = this.GetFocusItem(this.DefaultItems, value);
                        this.rbDefault.Checked = this.lvDefault.Tag != null;
                    }
                }
            }
        }
    }
}

