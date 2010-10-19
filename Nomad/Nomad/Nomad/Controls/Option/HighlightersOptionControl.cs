namespace Nomad.Controls.Option
{
    using Microsoft;
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.Controls.Filter;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class HighlightersOptionControl : UserControl, IPersistComponentSettings
    {
        private OpenFileDialog BrowseIconDialog;
        private ColorButton btnBlendColor;
        private Button btnBrowseIcon;
        private Button btnDeleteHighlighter;
        private ColorButton btnItemForeColor;
        private Button btnSaveHighlighter;
        private CheckBox chkAlphaBlendIcon;
        private CheckBox chkItemIcon;
        private TemplateComboBox cmbHighlighters;
        private ComboBox cmbIconLocation;
        private IContainer components = null;
        private ComplexFilterControl filterControlComplex;
        private ImageList imageList;
        private PanelEx pnlIcon;
        private ToolTip toolTipLevel;
        private TrackBar trkBlendLevel;
        private ToolStripButton tsbChangeView;
        private ToolStrip tsView;

        public HighlightersOptionControl()
        {
            this.InitializeComponent();
            this.btnItemForeColor.DefaultColor = Color.Empty;
            if (!Application.RenderWithVisualStyles)
            {
                this.btnSaveHighlighter.BackColor = SystemColors.Control;
                this.btnItemForeColor.BackColor = SystemColors.Control;
                this.btnBlendColor.BackColor = SystemColors.Control;
            }
            this.tsView.Renderer = BorderLessToolStripRenderer.Default;
            this.tsView.ImageList = this.imageList;
            this.btnSaveHighlighter.Image = IconSet.GetImage("SaveAs");
            this.imageList.Images.Add(Resources.ShowDetail);
            this.imageList.Images.Add(Resources.HideDetail);
            this.pnlIcon.Tag = ImageProvider.Default.GetDefaultIcon(DefaultIcon.DefaultDocument, ImageHelper.DefaultLargeIconSize);
            this.chkAlphaBlendIcon.Click += new EventHandler(this.InvalidateIcon);
            this.trkBlendLevel.ValueChanged += new EventHandler(this.InvalidateIcon);
            this.btnBlendColor.ColorChanged += new EventHandler(this.InvalidateIcon);
            this.chkItemIcon.Click += new EventHandler(this.IconChanged);
        }

        private void btnBrowseIcon_Click(object sender, EventArgs e)
        {
            this.cmbIconLocation.Tag = this.cmbIconLocation.Text;
            IconLocation location = IconLocation.TryParse(this.cmbIconLocation.Text);
            if (OS.IsWinXP)
            {
                StringBuilder builder;
                int piIconIndex = 0;
                if (location != null)
                {
                    builder = new StringBuilder(location.IconFileName, 260);
                    piIconIndex = location.IconIndex;
                }
                else
                {
                    builder = new StringBuilder(260);
                }
                if (Microsoft.Shell.Shell32.PickIconDlg(base.Handle, builder, builder.Capacity, ref piIconIndex) != 0)
                {
                    this.cmbIconLocation.Text = string.Format("{0},{1}", builder.ToString(), piIconIndex);
                    this.cmbIconLocation_Validated(this.cmbIconLocation, EventArgs.Empty);
                }
            }
            else
            {
                if (location != null)
                {
                    this.BrowseIconDialog.FileName = Environment.ExpandEnvironmentVariables(location.IconFileName);
                }
                if (this.BrowseIconDialog.ShowDialog() == DialogResult.OK)
                {
                    this.cmbIconLocation.Text = this.BrowseIconDialog.FileName;
                    this.cmbIconLocation_Validated(this.cmbIconLocation, EventArgs.Empty);
                }
            }
        }

        private void btnSaveHighlighter_Click(object sender, EventArgs e)
        {
            ListViewHighlighter highlighter = this.CreateHighlighter(true);
            if (highlighter != null)
            {
                this.cmbHighlighters.Save<ListViewHighlighter>(highlighter);
                this.btnSaveHighlighter.Enabled = false;
            }
        }

        private void CheckIconValidity()
        {
            if (this.cmbIconLocation.Text != string.Empty)
            {
                IconLocation location = IconLocation.TryParse(this.cmbIconLocation.Text);
                if (!((location != null) && File.Exists(Environment.ExpandEnvironmentVariables(location.IconFileName))))
                {
                    this.cmbIconLocation.BackColor = Settings.TextBoxError;
                    this.cmbIconLocation.ForeColor = SystemColors.HighlightText;
                }
                else
                {
                    this.cmbIconLocation.ResetBackColor();
                    this.cmbIconLocation.ResetForeColor();
                }
            }
        }

        private void chkAlphaBlendIcon_CheckedChanged(object sender, EventArgs e)
        {
            this.btnBlendColor.Enabled = this.chkAlphaBlendIcon.Checked;
            this.trkBlendLevel.Enabled = this.chkAlphaBlendIcon.Checked;
        }

        private void chkItemIcon_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbIconLocation.Enabled = this.chkItemIcon.Checked;
            this.btnBrowseIcon.Enabled = this.chkItemIcon.Checked;
            if (this.chkItemIcon.Checked)
            {
                this.CheckIconValidity();
            }
        }

        private void cmbHighlighters_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.cmbHighlighters.SelectedIndex >= 0)
            {
                ListViewHighlighter highlighter = this.cmbHighlighters.GetValue<ListViewHighlighter>(this.cmbHighlighters.SelectedIndex);
                this.filterControlComplex.Filter = highlighter.Filter;
                this.btnItemForeColor.Color = highlighter.ForeColor;
                this.chkAlphaBlendIcon.Checked = highlighter.AlphaBlend;
                this.btnBlendColor.Color = highlighter.BlendColor;
                this.trkBlendLevel.Value = Convert.ToInt32((float) (highlighter.BlendLevel * 100f));
                this.chkItemIcon.Checked = highlighter.IconType == HighlighterIconType.HighlighterIcon;
                this.cmbIconLocation.Text = !string.IsNullOrEmpty(highlighter.IconLocation) ? highlighter.IconLocation : @"%SystemRoot%\System32\Shell32.dll,1";
                this.CheckIconValidity();
                this.UpdateIcon(highlighter);
                this.cmbHighlighters.UpdateButtons();
            }
        }

        private void cmbIconLocation_Enter(object sender, EventArgs e)
        {
            this.cmbIconLocation.Tag = this.cmbIconLocation.Text;
        }

        private void cmbIconLocation_Validated(object sender, EventArgs e)
        {
            this.CheckIconValidity();
            if (!string.Equals(this.cmbIconLocation.Text, this.cmbIconLocation.Tag as string))
            {
                this.UpdateButtons(sender, e);
                this.UpdateIcon();
            }
        }

        private ListViewHighlighter CreateHighlighter(bool withFilter)
        {
            IVirtualItemFilter filter = null;
            if (withFilter)
            {
                filter = this.filterControlComplex.Filter;
                if (filter == null)
                {
                    return null;
                }
            }
            return new ListViewHighlighter { Filter = filter, ForeColor = this.btnItemForeColor.Color, AlphaBlend = this.chkAlphaBlendIcon.Checked, BlendColor = this.btnBlendColor.Color, BlendLevel = ((float) this.trkBlendLevel.Value) / 100f, IconType = this.chkItemIcon.Checked ? 0 : 2, IconLocation = this.cmbIconLocation.Text };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void filterControlComplex_ViewChanged(object sender, EventArgs e)
        {
            base.Height += this.filterControlComplex.PreferredSize.Height - this.filterControlComplex.Height;
        }

        private void IconChanged(object sender, EventArgs e)
        {
            this.UpdateIcon();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(HighlightersOptionControl));
            this.tsView = new ToolStrip();
            this.tsbChangeView = new ToolStripButton();
            this.trkBlendLevel = new TrackBar();
            this.filterControlComplex = new ComplexFilterControl();
            this.pnlIcon = new PanelEx();
            this.btnBlendColor = new ColorButton();
            this.cmbHighlighters = new TemplateComboBox();
            this.btnDeleteHighlighter = new Button();
            this.btnBrowseIcon = new Button();
            this.btnSaveHighlighter = new Button();
            this.cmbIconLocation = new ComboBox();
            this.chkItemIcon = new CheckBox();
            this.chkAlphaBlendIcon = new CheckBox();
            this.btnItemForeColor = new ColorButton();
            this.imageList = new ImageList(this.components);
            this.toolTipLevel = new ToolTip(this.components);
            this.BrowseIconDialog = new OpenFileDialog();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            panel.SuspendLayout();
            this.tsView.SuspendLayout();
            this.trkBlendLevel.BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblItemForeColor");
            label.Name = "lblItemForeColor";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(control, 0, 0);
            panel.Controls.Add(this.tsView, 0, 5);
            panel.Controls.Add(this.trkBlendLevel, 2, 3);
            panel.Controls.Add(this.filterControlComplex, 0, 4);
            panel.Controls.Add(this.pnlIcon, 4, 2);
            panel.Controls.Add(this.btnBlendColor, 1, 3);
            panel.Controls.Add(this.cmbHighlighters, 1, 0);
            panel.Controls.Add(this.btnBrowseIcon, 3, 2);
            panel.Controls.Add(this.btnSaveHighlighter, 3, 0);
            panel.Controls.Add(this.cmbIconLocation, 1, 2);
            panel.Controls.Add(this.btnDeleteHighlighter, 4, 0);
            panel.Controls.Add(this.chkItemIcon, 0, 2);
            panel.Controls.Add(label, 0, 1);
            panel.Controls.Add(this.chkAlphaBlendIcon, 0, 3);
            panel.Controls.Add(this.btnItemForeColor, 1, 1);
            panel.Name = "tlpBack";
            manager.ApplyResources(control, "lblStoredHighlighter");
            control.Name = "lblStoredHighlighter";
            this.tsView.BackColor = Color.Transparent;
            panel.SetColumnSpan(this.tsView, 5);
            this.tsView.GripStyle = ToolStripGripStyle.Hidden;
            this.tsView.Items.AddRange(new ToolStripItem[] { this.tsbChangeView });
            manager.ApplyResources(this.tsView, "tsView");
            this.tsView.Name = "tsView";
            this.tsView.TabStop = true;
            this.tsbChangeView.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbChangeView.Image = Resources.ShowDetail;
            this.tsbChangeView.Name = "tsbChangeView";
            manager.ApplyResources(this.tsbChangeView, "tsbChangeView");
            this.tsbChangeView.Click += new EventHandler(this.tsbChangeView_Click);
            this.tsbChangeView.Paint += new PaintEventHandler(this.tsbChangeView_Paint);
            manager.ApplyResources(this.trkBlendLevel, "trkBlendLevel");
            panel.SetColumnSpan(this.trkBlendLevel, 2);
            this.trkBlendLevel.Maximum = 100;
            this.trkBlendLevel.Name = "trkBlendLevel";
            this.trkBlendLevel.Value = 50;
            this.trkBlendLevel.Scroll += new EventHandler(this.trkBlendLevel_Scroll);
            this.trkBlendLevel.ValueChanged += new EventHandler(this.UpdateButtons);
            this.trkBlendLevel.Enter += new EventHandler(this.trkBlendLevel_Enter);
            this.trkBlendLevel.Leave += new EventHandler(this.trkBlendLevel_Leave);
            this.trkBlendLevel.MouseDown += new MouseEventHandler(this.trkBlendLevel_MouseDown);
            this.trkBlendLevel.MouseUp += new MouseEventHandler(this.trkBlendLevel_MouseUp);
            this.filterControlComplex.AdvancedViewFilters = ViewFilters.Advanced | ViewFilters.Attributes | ViewFilters.ExcludeMask | ViewFilters.IncludeMask;
            manager.ApplyResources(this.filterControlComplex, "filterControlComplex");
            panel.SetColumnSpan(this.filterControlComplex, 5);
            this.filterControlComplex.HideViewFilters = ViewFilters.Folder | ViewFilters.Content;
            this.filterControlComplex.MinimumSize = new Size(0x1c4, 0);
            this.filterControlComplex.Name = "filterControlComplex";
            this.filterControlComplex.View = ComplexFilterView.Advanced;
            this.filterControlComplex.ViewChanged += new EventHandler(this.filterControlComplex_ViewChanged);
            this.filterControlComplex.FilterChanged += new EventHandler(this.UpdateButtons);
            this.pnlIcon.BorderColor = Color.FromArgb(0xab, 0xad, 0xb3);
            manager.ApplyResources(this.pnlIcon, "pnlIcon");
            this.pnlIcon.Name = "pnlIcon";
            panel.SetRowSpan(this.pnlIcon, 2);
            this.pnlIcon.Paint += new PaintEventHandler(this.pnlIcon_Paint);
            manager.ApplyResources(this.btnBlendColor, "btnBlendColor");
            this.btnBlendColor.Name = "btnBlendColor";
            this.btnBlendColor.UseVisualStyleBackColor = true;
            this.btnBlendColor.ColorChanged += new EventHandler(this.UpdateButtons);
            panel.SetColumnSpan(this.cmbHighlighters, 2);
            this.cmbHighlighters.DeleteButton = this.btnDeleteHighlighter;
            manager.ApplyResources(this.cmbHighlighters, "cmbHighlighters");
            this.cmbHighlighters.FormattingEnabled = true;
            this.cmbHighlighters.Name = "cmbHighlighters";
            this.cmbHighlighters.SelectionChangeCommitted += new EventHandler(this.cmbHighlighters_SelectionChangeCommitted);
            this.cmbHighlighters.TextUpdate += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.btnDeleteHighlighter, "btnDeleteHighlighter");
            this.btnDeleteHighlighter.Name = "btnDeleteHighlighter";
            this.btnDeleteHighlighter.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnBrowseIcon, "btnBrowseIcon");
            this.btnBrowseIcon.Name = "btnBrowseIcon";
            this.btnBrowseIcon.UseVisualStyleBackColor = true;
            this.btnBrowseIcon.Click += new EventHandler(this.btnBrowseIcon_Click);
            manager.ApplyResources(this.btnSaveHighlighter, "btnSaveHighlighter");
            this.btnSaveHighlighter.Name = "btnSaveHighlighter";
            this.btnSaveHighlighter.UseVisualStyleBackColor = true;
            this.btnSaveHighlighter.Click += new EventHandler(this.btnSaveHighlighter_Click);
            panel.SetColumnSpan(this.cmbIconLocation, 2);
            manager.ApplyResources(this.cmbIconLocation, "cmbIconLocation");
            this.cmbIconLocation.FormattingEnabled = true;
            this.cmbIconLocation.Name = "cmbIconLocation";
            this.cmbIconLocation.Enter += new EventHandler(this.cmbIconLocation_Enter);
            this.cmbIconLocation.Validated += new EventHandler(this.cmbIconLocation_Validated);
            manager.ApplyResources(this.chkItemIcon, "chkItemIcon");
            this.chkItemIcon.Name = "chkItemIcon";
            this.chkItemIcon.UseVisualStyleBackColor = true;
            this.chkItemIcon.CheckedChanged += new EventHandler(this.chkItemIcon_CheckedChanged);
            this.chkItemIcon.Click += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.chkAlphaBlendIcon, "chkAlphaBlendIcon");
            this.chkAlphaBlendIcon.Name = "chkAlphaBlendIcon";
            this.chkAlphaBlendIcon.UseVisualStyleBackColor = true;
            this.chkAlphaBlendIcon.CheckedChanged += new EventHandler(this.chkAlphaBlendIcon_CheckedChanged);
            this.chkAlphaBlendIcon.Click += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.btnItemForeColor, "btnItemForeColor");
            this.btnItemForeColor.Name = "btnItemForeColor";
            this.btnItemForeColor.UseVisualStyleBackColor = true;
            this.btnItemForeColor.ColorChanged += new EventHandler(this.UpdateButtons);
            this.imageList.ColorDepth = ColorDepth.Depth8Bit;
            manager.ApplyResources(this.imageList, "imageList");
            this.imageList.TransparentColor = Color.Transparent;
            this.BrowseIconDialog.AddExtension = false;
            manager.ApplyResources(this.BrowseIconDialog, "BrowseIconDialog");
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            this.MinimumSize = new Size(0x1c4, 0);
            base.Name = "HighlightersOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tsView.ResumeLayout(false);
            this.tsView.PerformLayout();
            this.trkBlendLevel.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InvalidateIcon(object sender, EventArgs e)
        {
            this.pnlIcon.Invalidate();
        }

        public void LoadComponentSettings()
        {
            this.cmbHighlighters.SetItems<ListViewHighlighter>(Settings.Default.Highlighters, delegate (ListViewHighlighter x) {
                return x.Name;
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.filterControlComplex_ViewChanged(this.filterControlComplex, EventArgs.Empty);
        }

        private void pnlIcon_Paint(object sender, PaintEventArgs e)
        {
            Image tag = (Image) this.pnlIcon.Tag;
            int x = (this.pnlIcon.Width - tag.Width) / 2;
            int y = (this.pnlIcon.Height - tag.Height) / 2;
            if (this.chkAlphaBlendIcon.Checked)
            {
                ImageHelper.DrawBlendImage(e.Graphics, tag, this.btnBlendColor.Color, ((float) this.trkBlendLevel.Value) / 100f, x, y);
            }
            else
            {
                e.Graphics.DrawImage(tag, x, y);
            }
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            List<ListViewHighlighter> list = new List<ListViewHighlighter>();
            foreach (KeyValuePair<string, ListViewHighlighter> pair in this.cmbHighlighters.GetItems<ListViewHighlighter>())
            {
                pair.Value.Name = pair.Key;
                list.Add(pair.Value);
            }
            Settings.Default.Highlighters = (list.Count > 0) ? list.ToArray() : new ListViewHighlighter[0];
        }

        private void trkBlendLevel_Enter(object sender, EventArgs e)
        {
            this.toolTipLevel.Show(string.Format(Resources.sTransparencyValue, this.trkBlendLevel.Value), this.trkBlendLevel, 0, this.trkBlendLevel.Height + 2);
        }

        private void trkBlendLevel_Leave(object sender, EventArgs e)
        {
            this.toolTipLevel.Hide(this.trkBlendLevel);
        }

        private void trkBlendLevel_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTipLevel.Show(string.Format(Resources.sTransparencyValue, this.trkBlendLevel.Value), this.trkBlendLevel, e.X, e.Y + this.trkBlendLevel.Cursor.GetPrefferedHeight());
        }

        private void trkBlendLevel_MouseUp(object sender, MouseEventArgs e)
        {
            this.toolTipLevel.Hide(this.trkBlendLevel);
        }

        private void trkBlendLevel_Scroll(object sender, EventArgs e)
        {
            int x;
            int num2;
            if (Control.MouseButtons == MouseButtons.Left)
            {
                Point point = this.trkBlendLevel.PointToClient(Cursor.Position);
                x = point.X;
                num2 = point.Y + this.trkBlendLevel.Cursor.GetPrefferedHeight();
            }
            else
            {
                x = 0;
                num2 = this.trkBlendLevel.Height + 2;
            }
            this.toolTipLevel.Show(string.Format(Resources.sTransparencyValue, this.trkBlendLevel.Value), this.trkBlendLevel, x, num2);
        }

        private void tsbChangeView_Click(object sender, EventArgs e)
        {
            using (new LockWindowRedraw(this.filterControlComplex, true))
            {
                if (this.filterControlComplex.View != ComplexFilterView.Full)
                {
                    this.filterControlComplex.View = ComplexFilterView.Full;
                }
                else
                {
                    this.filterControlComplex.View = ComplexFilterView.Advanced;
                }
            }
            this.UpdateButtons(this.filterControlComplex, EventArgs.Empty);
        }

        private void tsbChangeView_Paint(object sender, PaintEventArgs e)
        {
            int num = 0;
            if (this.filterControlComplex.View == ComplexFilterView.Full)
            {
                num = 1;
            }
            if (num != this.tsbChangeView.ImageIndex)
            {
                this.tsbChangeView.ImageIndex = num;
            }
        }

        private void UpdateButtons(object sender, EventArgs e)
        {
            bool flag = (!string.IsNullOrEmpty(this.cmbHighlighters.Text) && ((!this.btnItemForeColor.Color.IsEmpty || this.chkItemIcon.Checked) || this.chkAlphaBlendIcon.Checked)) && (this.filterControlComplex.Filter != null);
            if (flag && (this.cmbHighlighters.SelectedIndex >= 0))
            {
                ListViewHighlighter highlighter = this.cmbHighlighters.GetValue<ListViewHighlighter>(this.cmbHighlighters.SelectedIndex);
                flag = (highlighter == null) || !highlighter.Equals(this.CreateHighlighter(true));
            }
            this.btnSaveHighlighter.Enabled = flag;
            this.cmbHighlighters.UpdateButtons();
        }

        private void UpdateIcon()
        {
            this.UpdateIcon(this.CreateHighlighter(false));
        }

        private void UpdateIcon(VirtualHighligher highlighter)
        {
            Image icon = null;
            if (highlighter.IconType == HighlighterIconType.HighlighterIcon)
            {
                icon = highlighter.GetIcon(ImageHelper.DefaultLargeIconSize);
            }
            this.pnlIcon.Tag = icon ?? ImageProvider.Default.GetDefaultIcon(DefaultIcon.DefaultDocument, ImageHelper.DefaultLargeIconSize);
            this.pnlIcon.Invalidate();
        }

        public bool SaveSettings
        {
            get
            {
                return this.cmbHighlighters.Modified;
            }
            set
            {
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
    }
}

