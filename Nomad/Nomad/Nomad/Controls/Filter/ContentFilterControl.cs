namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class ContentFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private CheckBox chkCaseSensitive;
        private CheckBox chkDetectEncoding;
        private CheckBox chkRegex;
        private CheckBox chkSpaceCompress;
        private CheckBox chkUseIFilter;
        private CheckBox chkWholeWords;
        private ComboBox cmbComparision;
        private ComboBox cmbText;
        private IContainer components;
        private FlowLayoutPanel flpBack;
        private Panel pnlContent;
        private TableLayoutPanel tlpContent;
        private ToolStripLabel toolStripLabel1;
        private ToolStripDropDownButton tsddContentComparision;
        private ToolStripLabel tslContent;
        private ToolStripMenuItem tsmiComparisionContains;
        private ToolStripMenuItem tsmiComparisionNotContains;
        private ToolStripTextBox tstbText;
        private ToolStrip tsTop;

        public ContentFilterControl()
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
            this.LoadComponentSettings();
        }

        public ContentFilterControl(ContentFilter filter)
        {
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.LoadComponentSettings();
            this.SetFilter(filter);
        }

        private void chkRegex_CheckedChanged(object sender, EventArgs e)
        {
            this.chkWholeWords.Enabled = !this.chkRegex.Checked;
            this.chkSpaceCompress.Enabled = !this.chkRegex.Checked;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.tsmiComparisionContains.PerformClick();
            this.tstbText.Text = string.Empty;
            this.cmbComparision.SelectedIndex = 0;
            this.cmbText.Text = string.Empty;
            this.chkCaseSensitive.Checked = false;
            this.chkRegex.Checked = false;
            this.chkWholeWords.Checked = false;
            this.chkSpaceCompress.Checked = false;
            this.chkUseIFilter.Checked = true;
            this.chkDetectEncoding.Checked = true;
            base.CanRaiseFilterChanged = true;
        }

        private static int CompareEncodingInfoDisplayName(EncodingInfo x, EncodingInfo y)
        {
            return string.Compare(x.DisplayName, y.DisplayName);
        }

        private void ContentFilterControl_AutoSizeChanged(object sender, EventArgs e)
        {
            this.flpBack.AutoSize = this.AutoSize;
            this.tlpContent.AutoSize = this.AutoSize;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FilterChanged_Click(object sender, EventArgs e)
        {
            base.RaiseFilterChanged();
        }

        private void flbBack_SizeChanged(object sender, EventArgs e)
        {
            this.tlpContent.Width = this.flpBack.Width;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ContentFilterControl));
            this.pnlContent = new Panel();
            this.chkDetectEncoding = new CheckBox();
            this.chkUseIFilter = new CheckBox();
            this.chkSpaceCompress = new CheckBox();
            this.chkWholeWords = new CheckBox();
            this.chkRegex = new CheckBox();
            this.chkCaseSensitive = new CheckBox();
            this.cmbText = new ComboBox();
            this.cmbComparision = new ComboBox();
            this.flpBack = new FlowLayoutPanel();
            this.tsTop = new ToolStrip();
            this.tslContent = new ToolStripLabel();
            this.tsddContentComparision = new ToolStripDropDownButton();
            this.tsmiComparisionContains = new ToolStripMenuItem();
            this.tsmiComparisionNotContains = new ToolStripMenuItem();
            this.toolStripLabel1 = new ToolStripLabel();
            this.tstbText = new ToolStripTextBox();
            this.tlpContent = new TableLayoutPanel();
            this.pnlContent.SuspendLayout();
            this.flpBack.SuspendLayout();
            this.tsTop.SuspendLayout();
            this.tlpContent.SuspendLayout();
            base.SuspendLayout();
            this.pnlContent.Controls.Add(this.chkDetectEncoding);
            this.pnlContent.Controls.Add(this.chkUseIFilter);
            this.pnlContent.Controls.Add(this.chkSpaceCompress);
            this.pnlContent.Controls.Add(this.chkWholeWords);
            this.pnlContent.Controls.Add(this.chkRegex);
            this.pnlContent.Controls.Add(this.chkCaseSensitive);
            manager.ApplyResources(this.pnlContent, "pnlContent");
            this.pnlContent.Name = "pnlContent";
            manager.ApplyResources(this.chkDetectEncoding, "chkDetectEncoding");
            this.chkDetectEncoding.Checked = true;
            this.chkDetectEncoding.CheckState = CheckState.Checked;
            this.chkDetectEncoding.Name = "chkDetectEncoding";
            this.chkDetectEncoding.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkUseIFilter, "chkUseIFilter");
            this.chkUseIFilter.Checked = true;
            this.chkUseIFilter.CheckState = CheckState.Checked;
            this.chkUseIFilter.Name = "chkUseIFilter";
            this.chkUseIFilter.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSpaceCompress, "chkSpaceCompress");
            this.chkSpaceCompress.Name = "chkSpaceCompress";
            this.chkSpaceCompress.UseVisualStyleBackColor = true;
            this.chkSpaceCompress.Click += new EventHandler(this.FilterChanged_Click);
            manager.ApplyResources(this.chkWholeWords, "chkWholeWords");
            this.chkWholeWords.Name = "chkWholeWords";
            this.chkWholeWords.UseVisualStyleBackColor = true;
            this.chkWholeWords.Click += new EventHandler(this.FilterChanged_Click);
            manager.ApplyResources(this.chkRegex, "chkRegex");
            this.chkRegex.Name = "chkRegex";
            this.chkRegex.UseVisualStyleBackColor = true;
            this.chkRegex.Click += new EventHandler(this.FilterChanged_Click);
            this.chkRegex.CheckedChanged += new EventHandler(this.chkRegex_CheckedChanged);
            manager.ApplyResources(this.chkCaseSensitive, "chkCaseSensitive");
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            this.chkCaseSensitive.Click += new EventHandler(this.FilterChanged_Click);
            manager.ApplyResources(this.cmbText, "cmbText");
            this.cmbText.FormattingEnabled = true;
            this.cmbText.Name = "cmbText";
            this.cmbText.TextUpdate += new EventHandler(this.FilterChanged_Click);
            this.cmbComparision.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbComparision.FormattingEnabled = true;
            this.cmbComparision.Items.AddRange(new object[] { manager.GetString("cmbComparision.Items"), manager.GetString("cmbComparision.Items1") });
            manager.ApplyResources(this.cmbComparision, "cmbComparision");
            this.cmbComparision.Name = "cmbComparision";
            this.cmbComparision.SelectedIndexChanged += new EventHandler(this.FilterChanged_Click);
            manager.ApplyResources(this.flpBack, "flpBack");
            this.flpBack.Controls.Add(this.tsTop);
            this.flpBack.Controls.Add(this.tlpContent);
            this.flpBack.Name = "flpBack";
            this.flpBack.SizeChanged += new EventHandler(this.flbBack_SizeChanged);
            this.tsTop.BackColor = Color.Transparent;
            this.tsTop.CanOverflow = false;
            manager.ApplyResources(this.tsTop, "tsTop");
            this.tsTop.GripStyle = ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new ToolStripItem[] { this.tslContent, this.tsddContentComparision, this.toolStripLabel1, this.tstbText });
            this.tsTop.Name = "tsTop";
            this.tsTop.ItemAdded += new ToolStripItemEventHandler(this.tsTop_ItemAdded);
            this.tsTop.VisibleChanged += new EventHandler(this.tsTop_VisibleChanged);
            this.tslContent.Name = "tslContent";
            manager.ApplyResources(this.tslContent, "tslContent");
            this.tsddContentComparision.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddContentComparision.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiComparisionContains, this.tsmiComparisionNotContains });
            manager.ApplyResources(this.tsddContentComparision, "tsddContentComparision");
            this.tsddContentComparision.Name = "tsddContentComparision";
            this.tsmiComparisionContains.Name = "tsmiComparisionContains";
            manager.ApplyResources(this.tsmiComparisionContains, "tsmiComparisionContains");
            this.tsmiComparisionContains.Click += new EventHandler(this.tsmiComparisionContains_Click);
            this.tsmiComparisionNotContains.Name = "tsmiComparisionNotContains";
            manager.ApplyResources(this.tsmiComparisionNotContains, "tsmiComparisionNotContains");
            this.tsmiComparisionNotContains.Click += new EventHandler(this.tsmiComparisionContains_Click);
            this.toolStripLabel1.Name = "toolStripLabel1";
            manager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.tstbText.BorderStyle = BorderStyle.FixedSingle;
            this.tstbText.Name = "tstbText";
            manager.ApplyResources(this.tstbText, "tstbText");
            manager.ApplyResources(this.tlpContent, "tlpContent");
            this.tlpContent.Controls.Add(this.cmbComparision, 0, 0);
            this.tlpContent.Controls.Add(this.pnlContent, 1, 1);
            this.tlpContent.Controls.Add(this.cmbText, 1, 0);
            this.tlpContent.Name = "tlpContent";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.flpBack);
            base.Name = "ContentFilterControl";
            base.AutoSizeChanged += new EventHandler(this.ContentFilterControl_AutoSizeChanged);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.flpBack.ResumeLayout(false);
            this.flpBack.PerformLayout();
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitializeToolStripItems()
        {
            this.tsmiComparisionContains.Tag = ContentComparision.Contains;
            this.tsmiComparisionNotContains.Tag = ContentComparision.NotContains;
            this.tsddContentComparision.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        public void LoadComponentSettings()
        {
            this.FilterOptions = Settings.Default.SearchContentOptions;
            HistorySettings.PopulateComboBox(this.cmbText, HistorySettings.Default.SearchContent);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsTop.Font = this.Font;
            this.tstbText.Font = this.Font;
        }

        public void SaveComponentSettings()
        {
            HistorySettings.Default.AddStringToSearchContent(this.cmbText.Text);
            Settings.Default.SearchContentOptions = this.FilterOptions;
        }

        public void SetFilter(ContentFilter filter)
        {
            if (filter == null)
            {
                this.Clear();
            }
            else
            {
                base.CanRaiseFilterChanged = false;
                try
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddContentComparision, filter.Comparision);
                    this.cmbComparision.SelectedIndex = (filter.Comparision == ContentComparision.Contains) ? 0 : 1;
                    this.tstbText.Text = filter.Text;
                    this.cmbText.Text = filter.Text;
                    this.FilterOptions = filter.Options;
                }
                finally
                {
                    base.CanRaiseFilterChanged = true;
                }
            }
        }

        private void tsmiComparisionContains_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddContentComparision, (ToolStripItem) sender);
        }

        private void tsTop_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (this.tlpContent.Padding.Left != 0x17)
            {
                this.tlpContent.Padding = new Padding(0x17, 0, 0, 0);
            }
        }

        private void tsTop_VisibleChanged(object sender, EventArgs e)
        {
            this.cmbComparision.Visible = !this.tsTop.Visible;
            this.cmbText.Visible = !this.tsTop.Visible;
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddContentComparision, this.tsddContentComparision.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                if (this.IsEmpty)
                {
                    return null;
                }
                VirtualItemContentFilter filter = new VirtualItemContentFilter();
                if (this.tsTop.Enabled)
                {
                    filter.Text = this.tstbText.Text;
                    filter.Comparision = (ContentComparision) this.tsddContentComparision.Tag;
                }
                else
                {
                    filter.Text = this.cmbText.Text;
                    filter.Comparision = (this.cmbComparision.SelectedIndex == 0) ? ContentComparision.Contains : ContentComparision.NotContains;
                }
                filter.Options = this.FilterOptions;
                if (!this.chkDetectEncoding.Checked)
                {
                    filter.Encoding = Encoding.Default;
                }
                return filter;
            }
        }

        public ContentFilterOptions FilterOptions
        {
            get
            {
                return ((((((this.chkCaseSensitive.Checked ? ContentFilterOptions.CaseSensitive : ((ContentFilterOptions) 0)) | (this.chkRegex.Checked ? ContentFilterOptions.Regex : ((ContentFilterOptions) 0))) | (this.chkWholeWords.Checked ? ContentFilterOptions.WholeWords : ((ContentFilterOptions) 0))) | (this.chkSpaceCompress.Checked ? ContentFilterOptions.SpaceCompress : ((ContentFilterOptions) 0))) | (this.chkUseIFilter.Checked ? ContentFilterOptions.UseIFilter : ((ContentFilterOptions) 0))) | (this.chkDetectEncoding.Checked ? ContentFilterOptions.DetectEncoding : ((ContentFilterOptions) 0)));
            }
            set
            {
                this.chkCaseSensitive.Checked = (value & ContentFilterOptions.CaseSensitive) > 0;
                this.chkRegex.Checked = (value & ContentFilterOptions.Regex) > 0;
                this.chkWholeWords.Checked = (value & ContentFilterOptions.WholeWords) > 0;
                this.chkSpaceCompress.Checked = (value & ContentFilterOptions.SpaceCompress) > 0;
                this.chkUseIFilter.Checked = (value & ContentFilterOptions.UseIFilter) > 0;
                this.chkDetectEncoding.Checked = (value & ContentFilterOptions.DetectEncoding) > 0;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.tsTop.Enabled ? string.IsNullOrEmpty(this.tstbText.Text) : string.IsNullOrEmpty(this.cmbText.Text));
            }
        }

        [Browsable(false)]
        public ToolStrip TopToolStrip
        {
            get
            {
                this.tsTop.Enabled = true;
                this.tsTop.Visible = true;
                return this.tsTop;
            }
        }
    }
}

