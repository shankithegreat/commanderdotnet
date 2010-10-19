namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PropertyStringFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private ToolStripDropDownButton tsddContentComparision;
        private ToolStripDropDownButton tsddSearchOptions;
        private ToolStripLabel tslField;
        private ToolStripLabel tslPropertyName;
        private ToolStripMenuItem tsmiComparisionContains;
        private ToolStripMenuItem tsmiComparisionNotContains;
        private ToolStripMenuItem tsmiOptionCaseSensitive;
        private ToolStripMenuItem tsmiOptionRegex;
        private ToolStripMenuItem tsmiOptionSpaceCompress;
        private ToolStripMenuItem tsmiOptionWholeWords;
        private ToolStripSeparator tssString;
        private ToolStripTextBox tstbText;
        private ToolStrip tsTop;

        public PropertyStringFilterControl(VirtualPropertyFilter filter)
        {
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (!(filter.Filter is StringFilter))
            {
                throw new ArgumentException("filter is not StringFilter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.SetFilter(filter);
        }

        public PropertyStringFilterControl(int propertyId)
        {
            this.components = null;
            VirtualProperty property = VirtualProperty.Get(propertyId);
            if (property == null)
            {
                throw new ArgumentOutOfRangeException("Invalid propertyId");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
            this.tslPropertyName.Tag = propertyId;
            this.tslPropertyName.Text = property.LocalizedName;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.tsmiComparisionContains.PerformClick();
            this.tstbText.Text = string.Empty;
            base.CanRaiseFilterChanged = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PropertyStringFilterControl));
            this.tsTop = new ToolStrip();
            this.tslField = new ToolStripLabel();
            this.tslPropertyName = new ToolStripLabel();
            this.tsddContentComparision = new ToolStripDropDownButton();
            this.tsmiComparisionContains = new ToolStripMenuItem();
            this.tsmiComparisionNotContains = new ToolStripMenuItem();
            this.tstbText = new ToolStripTextBox();
            this.tssString = new ToolStripSeparator();
            this.tsddSearchOptions = new ToolStripDropDownButton();
            this.tsmiOptionWholeWords = new ToolStripMenuItem();
            this.tsmiOptionSpaceCompress = new ToolStripMenuItem();
            this.tsmiOptionRegex = new ToolStripMenuItem();
            this.tsmiOptionCaseSensitive = new ToolStripMenuItem();
            this.tsTop.SuspendLayout();
            base.SuspendLayout();
            this.tsTop.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsTop, "tsTop");
            this.tsTop.GripStyle = ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new ToolStripItem[] { this.tslField, this.tslPropertyName, this.tsddContentComparision, this.tstbText, this.tssString, this.tsddSearchOptions });
            this.tsTop.Name = "tsTop";
            this.tslField.Name = "tslField";
            manager.ApplyResources(this.tslField, "tslField");
            this.tslPropertyName.Name = "tslPropertyName";
            manager.ApplyResources(this.tslPropertyName, "tslPropertyName");
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
            this.tstbText.BorderStyle = BorderStyle.FixedSingle;
            this.tstbText.Name = "tstbText";
            manager.ApplyResources(this.tstbText, "tstbText");
            this.tstbText.TextChanged += new EventHandler(this.tstbText_TextChanged);
            this.tssString.Name = "tssString";
            manager.ApplyResources(this.tssString, "tssString");
            this.tsddSearchOptions.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddSearchOptions.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOptionWholeWords, this.tsmiOptionSpaceCompress, this.tsmiOptionRegex, this.tsmiOptionCaseSensitive });
            manager.ApplyResources(this.tsddSearchOptions, "tsddSearchOptions");
            this.tsddSearchOptions.Name = "tsddSearchOptions";
            this.tsddSearchOptions.DropDownOpening += new EventHandler(this.tsddSearchOptions_DropDownOpening);
            this.tsmiOptionWholeWords.CheckOnClick = true;
            this.tsmiOptionWholeWords.Name = "tsmiOptionWholeWords";
            manager.ApplyResources(this.tsmiOptionWholeWords, "tsmiOptionWholeWords");
            this.tsmiOptionSpaceCompress.CheckOnClick = true;
            this.tsmiOptionSpaceCompress.Name = "tsmiOptionSpaceCompress";
            manager.ApplyResources(this.tsmiOptionSpaceCompress, "tsmiOptionSpaceCompress");
            this.tsmiOptionRegex.CheckOnClick = true;
            this.tsmiOptionRegex.Name = "tsmiOptionRegex";
            manager.ApplyResources(this.tsmiOptionRegex, "tsmiOptionRegex");
            this.tsmiOptionCaseSensitive.CheckOnClick = true;
            this.tsmiOptionCaseSensitive.Name = "tsmiOptionCaseSensitive";
            manager.ApplyResources(this.tsmiOptionCaseSensitive, "tsmiOptionCaseSensitive");
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsTop);
            base.Name = "PropertyStringFilterControl";
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolStripItems()
        {
            this.tsmiComparisionContains.Tag = ContentComparision.Contains;
            this.tsmiComparisionNotContains.Tag = ContentComparision.NotContains;
            this.tsddContentComparision.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsTop.Font = this.Font;
            this.tstbText.Font = this.Font;
        }

        public void SetFilter(VirtualPropertyFilter filter)
        {
            if (filter == null)
            {
                this.Clear();
            }
            else
            {
                VirtualProperty property = VirtualProperty.Get(filter.PropertyId);
                if (property == null)
                {
                    throw new ArgumentException("filter has unknown PropertyId");
                }
                base.CanRaiseFilterChanged = false;
                try
                {
                    this.tslPropertyName.Tag = filter.PropertyId;
                    this.tslPropertyName.Text = property.LocalizedName;
                    StringFilter filter2 = (StringFilter) filter.Filter;
                    CustomFilterControl.PerformDropDownClick(this.tsddContentComparision, filter2.Comparision);
                    this.tstbText.Text = filter2.Text;
                    this.tsmiOptionWholeWords.Checked = (filter2.Options & ContentFilterOptions.WholeWords) > 0;
                    this.tsmiOptionSpaceCompress.Checked = (filter2.Options & ContentFilterOptions.SpaceCompress) > 0;
                    this.tsmiOptionRegex.Checked = (filter2.Options & ContentFilterOptions.Regex) > 0;
                    this.tsmiOptionCaseSensitive.Checked = (filter2.Options & ContentFilterOptions.CaseSensitive) > 0;
                }
                finally
                {
                    base.CanRaiseFilterChanged = true;
                }
            }
        }

        private void tsddSearchOptions_DropDownOpening(object sender, EventArgs e)
        {
            this.tsmiOptionWholeWords.Enabled = !this.tsmiOptionRegex.Checked;
            this.tsmiOptionSpaceCompress.Enabled = !this.tsmiOptionRegex.Checked;
        }

        private void tsmiComparisionContains_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddContentComparision, (ToolStripItem) sender);
        }

        private void tstbText_TextChanged(object sender, EventArgs e)
        {
            base.RaiseFilterChanged();
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddContentComparision, this.tsddContentComparision.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                if (!this.IsEmpty)
                {
                    ContentFilterOptions options = (((this.tsmiOptionWholeWords.Checked ? ContentFilterOptions.WholeWords : ((ContentFilterOptions) 0)) | (this.tsmiOptionSpaceCompress.Checked ? ContentFilterOptions.SpaceCompress : ((ContentFilterOptions) 0))) | (this.tsmiOptionRegex.Checked ? ContentFilterOptions.Regex : ((ContentFilterOptions) 0))) | (this.tsmiOptionCaseSensitive.Checked ? ContentFilterOptions.CaseSensitive : ((ContentFilterOptions) 0));
                    return new VirtualPropertyFilter((int) this.tslPropertyName.Tag, new StringFilter((ContentComparision) this.tsddContentComparision.Tag, this.tstbText.Text, options));
                }
                return null;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.tstbText.Text);
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsTop;
            }
        }
    }
}

