namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class PropertyVersionFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private ToolStripDropDownButton tsddValueOperation;
        private ToolStripLabel tslField;
        private ToolStripLabel tslIs;
        private ToolStripLabel tslPropertyName;
        private ToolStripMenuItem tsmiOperationEquals;
        private ToolStripMenuItem tsmiOperationLarger;
        private ToolStripMenuItem tsmiOperationNotEquals;
        private ToolStripMenuItem tsmiOperationSmaller;
        private ToolStripTextBox tstbValue;
        private ToolStrip tsTop;
        private static Regex VersionRegex = new Regex(@"^\d{1,5}\.\d{1,5}(\.\d{1,5}(\.\d{1,5})?)?$", RegexOptions.Singleline | RegexOptions.Compiled);

        public PropertyVersionFilterControl(VirtualPropertyFilter filter)
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.SetFilter(filter);
        }

        public PropertyVersionFilterControl(int propertyId)
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
            this.SetProperty(propertyId);
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.tstbValue.Tag = null;
            this.tstbValue.Text = string.Empty;
            this.tsmiOperationEquals.PerformClick();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PropertyVersionFilterControl));
            this.tsTop = new ToolStrip();
            this.tslField = new ToolStripLabel();
            this.tslPropertyName = new ToolStripLabel();
            this.tslIs = new ToolStripLabel();
            this.tsddValueOperation = new ToolStripDropDownButton();
            this.tsmiOperationEquals = new ToolStripMenuItem();
            this.tsmiOperationNotEquals = new ToolStripMenuItem();
            this.tsmiOperationSmaller = new ToolStripMenuItem();
            this.tsmiOperationLarger = new ToolStripMenuItem();
            this.tstbValue = new ToolStripTextBox();
            this.tsTop.SuspendLayout();
            base.SuspendLayout();
            this.tsTop.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsTop, "tsTop");
            this.tsTop.GripStyle = ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new ToolStripItem[] { this.tslField, this.tslPropertyName, this.tslIs, this.tsddValueOperation, this.tstbValue });
            this.tsTop.Name = "tsTop";
            this.tslField.Name = "tslField";
            manager.ApplyResources(this.tslField, "tslField");
            this.tslPropertyName.Name = "tslPropertyName";
            manager.ApplyResources(this.tslPropertyName, "tslPropertyName");
            this.tslIs.Name = "tslIs";
            manager.ApplyResources(this.tslIs, "tslIs");
            this.tsddValueOperation.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddValueOperation.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOperationEquals, this.tsmiOperationNotEquals, this.tsmiOperationSmaller, this.tsmiOperationLarger });
            this.tsddValueOperation.Name = "tsddValueOperation";
            manager.ApplyResources(this.tsddValueOperation, "tsddValueOperation");
            this.tsmiOperationEquals.Name = "tsmiOperationEquals";
            manager.ApplyResources(this.tsmiOperationEquals, "tsmiOperationEquals");
            this.tsmiOperationEquals.Click += new EventHandler(this.tsmiOperationEquals_Click);
            this.tsmiOperationNotEquals.Name = "tsmiOperationNotEquals";
            manager.ApplyResources(this.tsmiOperationNotEquals, "tsmiOperationNotEquals");
            this.tsmiOperationNotEquals.Click += new EventHandler(this.tsmiOperationEquals_Click);
            this.tsmiOperationSmaller.Name = "tsmiOperationSmaller";
            manager.ApplyResources(this.tsmiOperationSmaller, "tsmiOperationSmaller");
            this.tsmiOperationSmaller.Click += new EventHandler(this.tsmiOperationEquals_Click);
            this.tsmiOperationLarger.Name = "tsmiOperationLarger";
            manager.ApplyResources(this.tsmiOperationLarger, "tsmiOperationLarger");
            this.tsmiOperationLarger.Click += new EventHandler(this.tsmiOperationEquals_Click);
            this.tstbValue.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbValue, "tstbValue");
            this.tstbValue.Name = "tstbValue";
            this.tstbValue.TextChanged += new EventHandler(this.tstbValue_TextChanged);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsTop);
            base.Name = "PropertyVersionFilterControl";
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolStripItems()
        {
            this.tsmiOperationEquals.Tag = SimpleComparision.Equals;
            this.tsmiOperationNotEquals.Tag = SimpleComparision.NotEquals;
            this.tsmiOperationSmaller.Tag = SimpleComparision.Smaller;
            this.tsmiOperationLarger.Tag = SimpleComparision.Larger;
            this.tsddValueOperation.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsTop.Font = this.Font;
            this.tstbValue.Font = this.Font;
        }

        private void SetFilter(VirtualPropertyFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.SetProperty(filter.PropertyId);
            SimpleFilter<Version> filter2 = filter.Filter as SimpleFilter<Version>;
            if (filter2 == null)
            {
                throw new ArgumentException("filter is not SimpleFilter<Version>");
            }
            base.CanRaiseFilterChanged = false;
            try
            {
                CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter2.ValueComparision);
                this.tstbValue.Tag = filter2.FromValue;
                this.tstbValue.Text = this.tstbValue.Tag.ToString();
            }
            finally
            {
                base.CanRaiseFilterChanged = true;
            }
        }

        private void SetProperty(int propertyId)
        {
            VirtualProperty property = VirtualProperty.Get(propertyId);
            if (property == null)
            {
                throw new ArgumentOutOfRangeException("Invalid propertyId");
            }
            if (property.PropertyType != typeof(Version))
            {
                throw new ArgumentException("Invalid property type (Version expected)");
            }
            this.tslPropertyName.Tag = property.PropertyId;
            this.tslPropertyName.Text = property.LocalizedName;
        }

        private void tsmiOperationEquals_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddValueOperation, (ToolStripItem) sender);
        }

        private void tstbValue_TextChanged(object sender, EventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this.tstbValue.Text);
            if (flag || VersionRegex.IsMatch(this.tstbValue.Text))
            {
                if (!flag)
                {
                    Version version = new Version(this.tstbValue.Text);
                    if (!version.Equals(this.tstbValue.Tag))
                    {
                        this.tstbValue.Tag = version;
                        base.RaiseFilterChanged();
                    }
                }
                this.tstbValue.ResetBackColor();
                this.tstbValue.ResetForeColor();
            }
            else
            {
                this.tstbValue.BackColor = Settings.TextBoxError;
                this.tstbValue.ForeColor = SystemColors.HighlightText;
            }
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddValueOperation, this.tsddValueOperation.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                if (this.IsEmpty)
                {
                    return null;
                }
                return new VirtualPropertyFilter((int) this.tslPropertyName.Tag, new SimpleFilter<Version> { ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag, FromValue = (Version) this.tstbValue.Tag });
            }
        }

        public bool IsEmpty
        {
            get
            {
                return !VersionRegex.IsMatch(this.tstbValue.Text);
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

