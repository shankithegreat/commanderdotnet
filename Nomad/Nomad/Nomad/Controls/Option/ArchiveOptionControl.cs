namespace Nomad.Controls.Option
{
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Archive.Wcx;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ArchiveOptionControl : UserControl, IPersistComponentSettings
    {
        private Button btnAddExt;
        private Button btnDeleteExt;
        private CheckBox chkCreateArchive;
        private CheckBox chkDetectFormatByContent;
        private CheckBox chkEnterOpensArchive;
        private CheckBox chkHideFormat;
        private CheckBox chkMultiFileArchive;
        private CheckBox chkUpdateArchive;
        private CheckBox chkUsePipes;
        private ColumnHeader colExtension;
        private ColumnHeader colFormat;
        private IContainer components = null;
        private GroupBox grpCapabilities;
        private GroupBox grpExtensions;
        private GroupBox grpOptions;
        private ImageList imgFormats;
        private PictureBox imgSevenZipLink;
        private Label lblFormatName;
        private Label lblHideMask;
        private Label lblNoArchiveFormats;
        private ListViewEx lvExtensions;
        private ListViewEx lvFormats;
        private bool Modified;
        private Panel pnlBack;
        private Panel pnlEnterOpensArchive;
        private TextBox txtHideMask;

        public ArchiveOptionControl()
        {
            this.InitializeComponent();
            if (this.lvFormats.ExplorerTheme)
            {
                this.imgFormats.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
            }
            else
            {
                this.imgFormats.ImageSize = ImageHelper.DefaultSmallIconSize;
            }
            this.lblFormatName.BackColor = Theme.Current.ThemeColors.OptionBlockLabelBackground;
            this.lblFormatName.ForeColor = Theme.Current.ThemeColors.OptionBlockLabelText;
            this.imgFormats.AddNormalized(IconSet.GetImage("package"), this.lvFormats.BackColor);
        }

        private void ArchiveOptionControl_Load(object sender, EventArgs e)
        {
            this.lblFormatName.Font = new Font(this.lblFormatName.Font, FontStyle.Bold);
            this.imgSevenZipLink.Width = this.imgSevenZipLink.Image.Width;
            if (this.lvFormats.Items.Count > 0)
            {
                this.lvFormats.Items[0].Focus(true, true);
            }
        }

        private void btnAddExt_Click(object sender, EventArgs e)
        {
            this.lvExtensions.Items.Add(string.Empty).BeginEdit();
        }

        private void btnDeleteExt_Click(object sender, EventArgs e)
        {
            if (this.lvExtensions.FocusedItem != null)
            {
                this.CurrentFormat.ExtMap.Remove(this.lvExtensions.FocusedItem.Text);
                this.lvExtensions.FocusedItem.Delete(true);
                this.Modified = true;
            }
        }

        private void chkEnterOpensArchive_Click(object sender, EventArgs e)
        {
            this.Modified = true;
        }

        private void chkHideFormat_Click(object sender, EventArgs e)
        {
            this.CurrentFormat.HideFormat = this.chkHideFormat.Checked;
            this.Modified = true;
        }

        private void chkUsePipes_Click(object sender, EventArgs e)
        {
            this.CurrentFormat.UsePipes = this.chkUsePipes.Checked;
            this.Modified = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void imgSevenZipLink_Click(object sender, EventArgs e)
        {
            Process.Start((string) ((Control) sender).Tag);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ArchiveOptionControl));
            this.lvExtensions = new ListViewEx();
            this.colExtension = new ColumnHeader();
            this.lvFormats = new ListViewEx();
            this.colFormat = new ColumnHeader();
            this.imgFormats = new ImageList(this.components);
            this.lblNoArchiveFormats = new Label();
            this.chkEnterOpensArchive = new CheckBox();
            this.pnlEnterOpensArchive = new Panel();
            this.imgSevenZipLink = new PictureBox();
            this.lblHideMask = new Label();
            this.txtHideMask = new TextBox();
            this.pnlBack = new Panel();
            this.grpExtensions = new GroupBox();
            this.btnAddExt = new Button();
            this.btnDeleteExt = new Button();
            this.grpOptions = new GroupBox();
            this.chkHideFormat = new CheckBox();
            this.chkUsePipes = new CheckBox();
            this.grpCapabilities = new GroupBox();
            this.chkMultiFileArchive = new CheckBox();
            this.chkUpdateArchive = new CheckBox();
            this.chkCreateArchive = new CheckBox();
            this.chkDetectFormatByContent = new CheckBox();
            this.lblFormatName = new Label();
            this.pnlEnterOpensArchive.SuspendLayout();
            ((ISupportInitialize) this.imgSevenZipLink).BeginInit();
            this.pnlBack.SuspendLayout();
            this.grpExtensions.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpCapabilities.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.lvExtensions, "lvExtensions");
            this.lvExtensions.Columns.AddRange(new ColumnHeader[] { this.colExtension });
            this.lvExtensions.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            this.lvExtensions.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvExtensions.FullRowSelect = true;
            this.lvExtensions.HeaderStyle = ColumnHeaderStyle.None;
            this.lvExtensions.HideSelection = false;
            this.lvExtensions.LabelEdit = true;
            this.lvExtensions.MultiSelect = false;
            this.lvExtensions.Name = "lvExtensions";
            this.lvExtensions.Sorting = SortOrder.Ascending;
            this.lvExtensions.UseCompatibleStateImageBehavior = false;
            this.lvExtensions.View = View.Details;
            this.lvExtensions.KeyDown += new KeyEventHandler(this.lvExtensions_KeyDown);
            this.lvExtensions.AfterLabelEdit += new LabelEditEventHandler(this.lvExtensions_AfterLabelEdit);
            this.lvExtensions.SelectedIndexChanged += new EventHandler(this.lvExtensions_SelectedIndexChanged);
            this.lvExtensions.SizeChanged += new EventHandler(this.lvFormats_ClientSizeChanged);
            this.lvFormats.CheckBoxes = true;
            this.lvFormats.Columns.AddRange(new ColumnHeader[] { this.colFormat });
            this.lvFormats.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvFormats, "lvFormats");
            this.lvFormats.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvFormats.FullRowSelect = true;
            this.lvFormats.HeaderStyle = ColumnHeaderStyle.None;
            this.lvFormats.HideSelection = false;
            this.lvFormats.MultiSelect = false;
            this.lvFormats.Name = "lvFormats";
            this.lvFormats.SmallImageList = this.imgFormats;
            this.lvFormats.UseCompatibleStateImageBehavior = false;
            this.lvFormats.View = View.Details;
            this.lvFormats.KeyDown += new KeyEventHandler(this.lvFormats_KeyDown);
            this.lvFormats.ClientSizeChanged += new EventHandler(this.lvFormats_ClientSizeChanged);
            this.lvFormats.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvFormats_ItemSelectionChanged);
            manager.ApplyResources(this.colFormat, "colFormat");
            this.imgFormats.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgFormats, "imgFormats");
            this.imgFormats.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.lblNoArchiveFormats, "lblNoArchiveFormats");
            this.lblNoArchiveFormats.Image = Resources.GetSevenZipLogo;
            this.lblNoArchiveFormats.Name = "lblNoArchiveFormats";
            this.lblNoArchiveFormats.MouseMove += new MouseEventHandler(this.lblNoArchiveFormats_MouseMove);
            this.lblNoArchiveFormats.MouseClick += new MouseEventHandler(this.lblNoArchiveFormats_MouseClick);
            manager.ApplyResources(this.chkEnterOpensArchive, "chkEnterOpensArchive");
            this.chkEnterOpensArchive.Checked = Settings.Default.EnterOpensArchive;
            this.chkEnterOpensArchive.CheckState = CheckState.Checked;
            this.chkEnterOpensArchive.DataBindings.Add(new Binding("Checked", Settings.Default, "EnterOpensArchive", true, DataSourceUpdateMode.Never));
            this.chkEnterOpensArchive.Name = "chkEnterOpensArchive";
            this.chkEnterOpensArchive.UseVisualStyleBackColor = true;
            this.chkEnterOpensArchive.Click += new EventHandler(this.chkEnterOpensArchive_Click);
            manager.ApplyResources(this.pnlEnterOpensArchive, "pnlEnterOpensArchive");
            this.pnlEnterOpensArchive.Controls.Add(this.imgSevenZipLink);
            this.pnlEnterOpensArchive.Controls.Add(this.chkEnterOpensArchive);
            this.pnlEnterOpensArchive.Name = "pnlEnterOpensArchive";
            this.imgSevenZipLink.Cursor = Cursors.Hand;
            manager.ApplyResources(this.imgSevenZipLink, "imgSevenZipLink");
            this.imgSevenZipLink.Image = Resources.GetSevenZipLogo;
            this.imgSevenZipLink.Name = "imgSevenZipLink";
            this.imgSevenZipLink.TabStop = false;
            this.imgSevenZipLink.Tag = "http://www.7-zip.org/";
            this.imgSevenZipLink.Click += new EventHandler(this.imgSevenZipLink_Click);
            manager.ApplyResources(this.lblHideMask, "lblHideMask");
            this.lblHideMask.MaximumSize = new Size(0x18c, 0);
            this.lblHideMask.Name = "lblHideMask";
            manager.ApplyResources(this.txtHideMask, "txtHideMask");
            this.txtHideMask.Name = "txtHideMask";
            this.txtHideMask.TextChanged += new EventHandler(this.chkEnterOpensArchive_Click);
            this.txtHideMask.SizeChanged += new EventHandler(this.txtHideMask_SizeChanged);
            this.pnlBack.Controls.Add(this.grpExtensions);
            this.pnlBack.Controls.Add(this.grpOptions);
            this.pnlBack.Controls.Add(this.grpCapabilities);
            this.pnlBack.Controls.Add(this.lblFormatName);
            manager.ApplyResources(this.pnlBack, "pnlBack");
            this.pnlBack.Name = "pnlBack";
            this.grpExtensions.Controls.Add(this.lvExtensions);
            this.grpExtensions.Controls.Add(this.btnAddExt);
            this.grpExtensions.Controls.Add(this.btnDeleteExt);
            manager.ApplyResources(this.grpExtensions, "grpExtensions");
            this.grpExtensions.Name = "grpExtensions";
            this.grpExtensions.TabStop = false;
            manager.ApplyResources(this.btnAddExt, "btnAddExt");
            this.btnAddExt.Name = "btnAddExt";
            this.btnAddExt.UseVisualStyleBackColor = true;
            this.btnAddExt.Click += new EventHandler(this.btnAddExt_Click);
            manager.ApplyResources(this.btnDeleteExt, "btnDeleteExt");
            this.btnDeleteExt.Name = "btnDeleteExt";
            this.btnDeleteExt.UseVisualStyleBackColor = true;
            this.btnDeleteExt.Click += new EventHandler(this.btnDeleteExt_Click);
            this.grpOptions.Controls.Add(this.chkHideFormat);
            this.grpOptions.Controls.Add(this.chkUsePipes);
            manager.ApplyResources(this.grpOptions, "grpOptions");
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.TabStop = false;
            manager.ApplyResources(this.chkHideFormat, "chkHideFormat");
            this.chkHideFormat.Name = "chkHideFormat";
            this.chkHideFormat.UseVisualStyleBackColor = true;
            this.chkHideFormat.Click += new EventHandler(this.chkHideFormat_Click);
            manager.ApplyResources(this.chkUsePipes, "chkUsePipes");
            this.chkUsePipes.Name = "chkUsePipes";
            this.chkUsePipes.UseVisualStyleBackColor = true;
            this.chkUsePipes.Click += new EventHandler(this.chkUsePipes_Click);
            this.grpCapabilities.Controls.Add(this.chkMultiFileArchive);
            this.grpCapabilities.Controls.Add(this.chkUpdateArchive);
            this.grpCapabilities.Controls.Add(this.chkCreateArchive);
            this.grpCapabilities.Controls.Add(this.chkDetectFormatByContent);
            manager.ApplyResources(this.grpCapabilities, "grpCapabilities");
            this.grpCapabilities.Name = "grpCapabilities";
            this.grpCapabilities.TabStop = false;
            manager.ApplyResources(this.chkMultiFileArchive, "chkMultiFileArchive");
            this.chkMultiFileArchive.Name = "chkMultiFileArchive";
            this.chkMultiFileArchive.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkUpdateArchive, "chkUpdateArchive");
            this.chkUpdateArchive.Name = "chkUpdateArchive";
            this.chkUpdateArchive.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCreateArchive, "chkCreateArchive");
            this.chkCreateArchive.Name = "chkCreateArchive";
            this.chkCreateArchive.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkDetectFormatByContent, "chkDetectFormatByContent");
            this.chkDetectFormatByContent.Name = "chkDetectFormatByContent";
            this.chkDetectFormatByContent.UseVisualStyleBackColor = true;
            this.lblFormatName.BackColor = Color.FromArgb(0xdd, 0xe7, 0xee);
            manager.ApplyResources(this.lblFormatName, "lblFormatName");
            this.lblFormatName.ForeColor = Color.Navy;
            this.lblFormatName.Name = "lblFormatName";
            this.lblFormatName.Paint += new PaintEventHandler(this.lblFormatName_Paint);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.pnlBack);
            base.Controls.Add(this.lvFormats);
            base.Controls.Add(this.lblNoArchiveFormats);
            base.Controls.Add(this.pnlEnterOpensArchive);
            base.Controls.Add(this.lblHideMask);
            base.Controls.Add(this.txtHideMask);
            base.Name = "ArchiveOptionControl";
            base.Load += new EventHandler(this.ArchiveOptionControl_Load);
            this.pnlEnterOpensArchive.ResumeLayout(false);
            this.pnlEnterOpensArchive.PerformLayout();
            ((ISupportInitialize) this.imgSevenZipLink).EndInit();
            this.pnlBack.ResumeLayout(false);
            this.grpExtensions.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpCapabilities.ResumeLayout(false);
            this.grpCapabilities.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool IsOnNoArchiveImage(Point location)
        {
            Rectangle rectangle = new Rectangle(0, 0, this.lblNoArchiveFormats.Image.Width + this.lblNoArchiveFormats.Margin.Horizontal, this.lblNoArchiveFormats.Image.Height + this.lblNoArchiveFormats.Margin.Horizontal);
            rectangle.Offset(this.lblNoArchiveFormats.Width - rectangle.Width, this.lblNoArchiveFormats.Height - rectangle.Height);
            return rectangle.Contains(location);
        }

        private void lblFormatName_Paint(object sender, PaintEventArgs e)
        {
            Rectangle clientRectangle = ((Label) sender).ClientRectangle;
            using (Pen pen = new Pen(Theme.Current.ThemeColors.OptionBlockLabelBorder))
            {
                e.Graphics.DrawLine(pen, 0, clientRectangle.Bottom - 1, clientRectangle.Right, clientRectangle.Bottom - 1);
            }
        }

        private void lblNoArchiveFormats_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.IsOnNoArchiveImage(e.Location))
            {
                this.imgSevenZipLink_Click(this.imgSevenZipLink, EventArgs.Empty);
            }
        }

        private void lblNoArchiveFormats_MouseMove(object sender, MouseEventArgs e)
        {
            this.lblNoArchiveFormats.Cursor = this.IsOnNoArchiveImage(e.Location) ? Cursors.Hand : Cursors.Default;
        }

        public void LoadComponentSettings()
        {
            this.lvFormats.BeginUpdate();
            try
            {
                foreach (ArchiveFormatInfo info in ArchiveFormatManager.GetFormats())
                {
                    ArchiveFormatConfig config = new ArchiveFormatConfig(info);
                    Image img = null;
                    ListViewItem item = new ListViewItem(info.Name) {
                        Checked = !info.Disabled,
                        Tag = config
                    };
                    this.lvFormats.Items.Add(item);
                    SevenZipFormatInfo info2 = info as SevenZipFormatInfo;
                    if (info2 != null)
                    {
                        img = info2.GetFormatImage(ImageHelper.DefaultSmallIconSize);
                    }
                    if (img != null)
                    {
                        item.ImageIndex = this.imgFormats.AddNormalized(img, this.lvFormats.BackColor);
                    }
                    if ((item.ImageIndex < 0) && (this.imgFormats.Images.Count > 0))
                    {
                        item.ImageIndex = 0;
                    }
                }
                this.lvFormats.Sorting = SortOrder.Ascending;
                this.lvFormats.Sort();
            }
            finally
            {
                this.lvFormats.EndUpdate();
            }
            if (this.lvFormats.Items.Count == 0)
            {
                base.Controls.Clear();
                this.lblNoArchiveFormats.Visible = true;
                base.Controls.Add(this.lblNoArchiveFormats);
            }
            else
            {
                this.lvFormats.Items[0].Focus(true, false);
            }
            this.txtHideMask.Text = ArchiveFormatSettings.Default.HideMask;
        }

        private void lvExtensions_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
            }
            else if (this.CurrentFormat.ExtMap.ContainsKey(e.Label))
            {
                MessageDialog.Show(this, string.Format(Resources.sExtUsedByThisArchiveFormat, e.Label), Resources.sWarning, MessageDialog.ButtonsOk, MessageBoxIcon.Hand);
                e.CancelEdit = true;
            }
            if (e.CancelEdit)
            {
                if (string.IsNullOrEmpty(this.lvExtensions.Items[e.Item].Text))
                {
                    this.lvExtensions.Items.RemoveAt(e.Item);
                }
            }
            else
            {
                this.CurrentFormat.ExtMap[e.Label] = 0;
                this.Modified = true;
            }
        }

        private void lvExtensions_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    this.btnDeleteExt.PerformClick();
                    e.Handled = true;
                    break;

                case Keys.F2:
                    if (this.lvExtensions.FocusedItem != null)
                    {
                        this.lvExtensions.FocusedItem.BeginEdit();
                    }
                    e.Handled = true;
                    break;

                case (Keys.Control | Keys.Add):
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void lvExtensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PostUpdateExtButtons();
        }

        private void lvFormats_ClientSizeChanged(object sender, EventArgs e)
        {
            ListViewEx lv = (ListViewEx) sender;
            if (!lv.IsEditing)
            {
                if (base.IsHandleCreated)
                {
                    base.BeginInvoke(delegate (ListView dummy) {
                        lv.Columns[0].Width = lv.ClientSize.Width;
                    }, new object[] { lv });
                }
                else
                {
                    lv.Columns[0].Width = lv.ClientSize.Width;
                }
            }
        }

        private void lvFormats_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Focused)
            {
                e.Item.Selected = true;
                this.lblFormatName.Text = e.Item.Text;
                this.lblFormatName.Tag = e.Item.Tag;
                this.chkDetectFormatByContent.Checked = (this.CurrentFormat.Format.Capabilities & ArchiveFormatCapabilities.DetectFormatByContent) > 0;
                this.chkCreateArchive.Checked = (this.CurrentFormat.Format.Capabilities & ArchiveFormatCapabilities.CreateArchive) > 0;
                this.chkUpdateArchive.Checked = (this.CurrentFormat.Format.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0;
                this.chkMultiFileArchive.Checked = (this.CurrentFormat.Format.Capabilities & ArchiveFormatCapabilities.MultiFileArchive) > 0;
                this.chkHideFormat.Checked = this.CurrentFormat.HideFormat;
                this.chkUsePipes.Enabled = this.CurrentFormat.Format is WcxFormatInfo;
                this.chkUsePipes.Checked = this.chkUsePipes.Enabled && this.CurrentFormat.UsePipes;
                this.lvExtensions.BeginUpdate();
                try
                {
                    this.lvExtensions.Items.Clear();
                    foreach (string str in this.CurrentFormat.ExtMap.Keys)
                    {
                        this.lvExtensions.Items.Add(str);
                    }
                }
                finally
                {
                    this.lvExtensions.EndUpdate();
                }
                this.UpdateExtButtons();
            }
        }

        private void lvFormats_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = e.KeyData == (Keys.Control | Keys.Add);
        }

        private void PostUpdateExtButtons()
        {
            base.BeginInvoke(new MethodInvoker(this.UpdateExtButtons));
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            foreach (ListViewItem item in this.lvFormats.Items)
            {
                ArchiveFormatConfig tag = (ArchiveFormatConfig) item.Tag;
                tag.Write();
                tag.Format.Disabled = !item.Checked;
                IPersistComponentSettings format = tag.Format as IPersistComponentSettings;
                if ((format != null) && format.SaveSettings)
                {
                    format.SaveComponentSettings();
                }
            }
            Settings.Default.EnterOpensArchive = this.chkEnterOpensArchive.Checked;
            ArchiveFormatSettings.Default.HideMask = this.txtHideMask.Text;
        }

        private void txtHideMask_SizeChanged(object sender, EventArgs e)
        {
            this.lblHideMask.MaximumSize = new Size(this.txtHideMask.Width, 0);
        }

        private void UpdateExtButtons()
        {
            ListViewItem focusedItem = this.lvExtensions.FocusedItem;
            if (!((focusedItem == null) || focusedItem.Selected))
            {
                focusedItem = null;
            }
            this.btnDeleteExt.Enabled = focusedItem != null;
        }

        private ArchiveFormatConfig CurrentFormat
        {
            get
            {
                return (this.lblFormatName.Tag as ArchiveFormatConfig);
            }
        }

        public bool SaveSettings
        {
            get
            {
                return this.Modified;
            }
            set
            {
                this.Modified = value;
            }
        }

        public string SettingsKey
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        private class ArchiveFormatConfig
        {
            public readonly Dictionary<string, int> ExtMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            public readonly ArchiveFormatInfo Format;
            public bool HideFormat;
            public bool UsePipes;

            public ArchiveFormatConfig(ArchiveFormatInfo formatInfo)
            {
                this.Format = formatInfo;
                this.Read();
            }

            public void Read()
            {
                this.HideFormat = this.Format.HideFormat;
                WcxFormatInfo format = this.Format as WcxFormatInfo;
                if (format != null)
                {
                    this.UsePipes = format.UsePipes;
                }
                if (this.Format.Extension != null)
                {
                    foreach (string str in this.Format.Extension)
                    {
                        this.ExtMap[str] = 0;
                    }
                }
            }

            public void Write()
            {
                this.Format.HideFormat = this.HideFormat;
                WcxFormatInfo format = this.Format as WcxFormatInfo;
                if (format != null)
                {
                    format.UsePipes = this.UsePipes;
                }
                List<string> list = new List<string>();
                foreach (string str in this.ExtMap.Keys)
                {
                    list.Add(str);
                }
                list.Sort(StringComparer.OrdinalIgnoreCase);
                this.Format.Extension = list.ToArray();
            }
        }
    }
}

