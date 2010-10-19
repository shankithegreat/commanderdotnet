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
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class PropertyIntegralFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private long MaxValue;
        private long MinValue;
        private ToolStripDropDownButton tsddValueOperation;
        private ToolStripLabel tslAnd;
        private ToolStripLabel tslField;
        private ToolStripLabel tslIs;
        private ToolStripLabel tslPropertyName;
        private ToolStripMenuItem tsmiOperationBetween;
        private ToolStripMenuItem tsmiOperationEquals;
        private ToolStripMenuItem tsmiOperationLarger;
        private ToolStripMenuItem tsmiOperationNotBetween;
        private ToolStripMenuItem tsmiOperationNotEquals;
        private ToolStripMenuItem tsmiOperationSmaller;
        private ToolStripTextBox tstbValueFrom;
        private ToolStripTextBox tstbValueTo;
        private ToolStrip tsTop;

        public PropertyIntegralFilterControl(VirtualPropertyFilter filter)
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.SetFilter(filter);
        }

        public PropertyIntegralFilterControl(int propertyId)
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
            this.tstbValueFrom.Tag = 0L;
            this.tstbValueFrom.Text = this.tstbValueFrom.Tag.ToString();
            this.tstbValueTo.Tag = this.tstbValueFrom.Tag;
            this.tstbValueTo.Text = this.tstbValueFrom.Text;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PropertyIntegralFilterControl));
            this.tsTop = new ToolStrip();
            this.tslField = new ToolStripLabel();
            this.tslPropertyName = new ToolStripLabel();
            this.tslIs = new ToolStripLabel();
            this.tsddValueOperation = new ToolStripDropDownButton();
            this.tsmiOperationEquals = new ToolStripMenuItem();
            this.tsmiOperationNotEquals = new ToolStripMenuItem();
            this.tsmiOperationSmaller = new ToolStripMenuItem();
            this.tsmiOperationLarger = new ToolStripMenuItem();
            this.tsmiOperationBetween = new ToolStripMenuItem();
            this.tsmiOperationNotBetween = new ToolStripMenuItem();
            this.tstbValueFrom = new ToolStripTextBox();
            this.tslAnd = new ToolStripLabel();
            this.tstbValueTo = new ToolStripTextBox();
            this.tsTop.SuspendLayout();
            base.SuspendLayout();
            this.tsTop.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsTop, "tsTop");
            this.tsTop.GripStyle = ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new ToolStripItem[] { this.tslField, this.tslPropertyName, this.tslIs, this.tsddValueOperation, this.tstbValueFrom, this.tslAnd, this.tstbValueTo });
            this.tsTop.Name = "tsTop";
            this.tslField.Name = "tslField";
            manager.ApplyResources(this.tslField, "tslField");
            this.tslPropertyName.Name = "tslPropertyName";
            manager.ApplyResources(this.tslPropertyName, "tslPropertyName");
            this.tslIs.Name = "tslIs";
            manager.ApplyResources(this.tslIs, "tslIs");
            this.tsddValueOperation.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddValueOperation.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOperationEquals, this.tsmiOperationNotEquals, this.tsmiOperationSmaller, this.tsmiOperationLarger, this.tsmiOperationBetween, this.tsmiOperationNotBetween });
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
            this.tsmiOperationBetween.Name = "tsmiOperationBetween";
            manager.ApplyResources(this.tsmiOperationBetween, "tsmiOperationBetween");
            this.tsmiOperationBetween.Click += new EventHandler(this.tsmiOperationBetween_Click);
            this.tsmiOperationNotBetween.Name = "tsmiOperationNotBetween";
            manager.ApplyResources(this.tsmiOperationNotBetween, "tsmiOperationNotBetween");
            this.tsmiOperationNotBetween.Click += new EventHandler(this.tsmiOperationBetween_Click);
            this.tstbValueFrom.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbValueFrom, "tstbValueFrom");
            this.tstbValueFrom.Name = "tstbValueFrom";
            this.tstbValueFrom.KeyPress += new KeyPressEventHandler(this.tstbValueFrom_KeyPress);
            this.tstbValueFrom.TextChanged += new EventHandler(this.tstbValueFrom_TextChanged);
            this.tslAnd.Name = "tslAnd";
            manager.ApplyResources(this.tslAnd, "tslAnd");
            this.tstbValueTo.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbValueTo, "tstbValueTo");
            this.tstbValueTo.Name = "tstbValueTo";
            this.tstbValueTo.KeyPress += new KeyPressEventHandler(this.tstbValueFrom_KeyPress);
            this.tstbValueTo.TextChanged += new EventHandler(this.tstbValueFrom_TextChanged);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsTop);
            base.Name = "PropertyIntegralFilterControl";
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
            this.tsmiOperationBetween.Tag = SimpleComparision.Between;
            this.tsmiOperationNotBetween.Tag = SimpleComparision.NotBetween;
            this.tsddValueOperation.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        private bool IsValidValue(long value)
        {
            return ((value >= this.MinValue) && (value <= this.MaxValue));
        }

        private bool IsValidValue(string str, out long value)
        {
            return (long.TryParse(str, out value) && this.IsValidValue(value));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsTop.Font = this.Font;
            this.tstbValueFrom.Font = this.Font;
            this.tstbValueTo.Font = this.Font;
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Size) > BoundsSpecified.None) && ((factor.Width != 1.0) || (factor.Height != 1.0)))
            {
                this.tstbValueTo.TextBox.Scale(factor);
            }
        }

        private void SetFilter(VirtualPropertyFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.SetProperty(filter.PropertyId);
            IntegralFilter<byte> filter2 = filter.Filter as IntegralFilter<byte>;
            IntegralFilter<short> filter3 = filter.Filter as IntegralFilter<short>;
            IntegralFilter<ushort> filter4 = filter.Filter as IntegralFilter<ushort>;
            IntegralFilter<int> filter5 = filter.Filter as IntegralFilter<int>;
            IntegralFilter<uint> filter6 = filter.Filter as IntegralFilter<uint>;
            IntegralFilter<long> filter7 = filter.Filter as IntegralFilter<long>;
            if ((((filter2 == null) && (filter5 == null)) && (filter6 == null)) && (filter7 == null))
            {
                throw new ArgumentException("filter is not integral filder of supported type");
            }
            base.CanRaiseFilterChanged = false;
            try
            {
                if (filter2 != null)
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter2.ValueComparision);
                    this.tstbValueFrom.Tag = filter2.FromValue;
                    this.tstbValueTo.Tag = filter2.ToValue;
                }
                if (filter3 != null)
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter3.ValueComparision);
                    this.tstbValueFrom.Tag = filter3.FromValue;
                    this.tstbValueTo.Tag = filter3.ToValue;
                }
                if (filter4 != null)
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter4.ValueComparision);
                    this.tstbValueFrom.Tag = filter4.FromValue;
                    this.tstbValueTo.Tag = filter4.ToValue;
                }
                if (filter5 != null)
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter5.ValueComparision);
                    this.tstbValueFrom.Tag = filter5.FromValue;
                    this.tstbValueTo.Tag = filter5.ToValue;
                }
                if (filter6 != null)
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter6.ValueComparision);
                    this.tstbValueFrom.Tag = filter6.FromValue;
                    this.tstbValueTo.Tag = filter6.ToValue;
                }
                if (filter7 != null)
                {
                    CustomFilterControl.PerformDropDownClick(this.tsddValueOperation, filter7.ValueComparision);
                    this.tstbValueFrom.Tag = filter7.FromValue;
                    this.tstbValueTo.Tag = filter7.ToValue;
                }
                this.tstbValueFrom.Text = this.tstbValueFrom.Tag.ToString();
                this.tstbValueTo.Text = this.tstbValueTo.Tag.ToString();
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
            switch (System.Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Byte:
                    this.MinValue = 0L;
                    this.MaxValue = 0xffL;
                    break;

                case TypeCode.Int16:
                    this.MinValue = -32768L;
                    this.MaxValue = 0x7fffL;
                    break;

                case TypeCode.UInt16:
                    this.MinValue = 0L;
                    this.MaxValue = 0xffffL;
                    break;

                case TypeCode.Int32:
                    this.MinValue = -2147483648L;
                    this.MaxValue = 0x7fffffffL;
                    break;

                case TypeCode.UInt32:
                    this.MinValue = 0L;
                    this.MaxValue = 0xffffffffL;
                    break;

                case TypeCode.Int64:
                    this.MinValue = -9223372036854775808L;
                    this.MaxValue = 0x7fffffffffffffffL;
                    break;

                default:
                    throw new ArgumentException("Unsupported property type");
            }
            this.tslPropertyName.Tag = property.PropertyId;
            this.tslPropertyName.Text = property.LocalizedName;
        }

        private void tsmiOperationBetween_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddValueOperation, (ToolStripItem) sender);
            this.tslAnd.Visible = true;
            this.tstbValueTo.Visible = true;
        }

        private void tsmiOperationEquals_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddValueOperation, (ToolStripItem) sender);
            this.tslAnd.Visible = false;
            this.tstbValueTo.Visible = false;
        }

        private void tstbValueFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar) && (e.KeyChar != '\b');
        }

        private void tstbValueFrom_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox box = (ToolStripTextBox) sender;
            long num = 0L;
            if (string.IsNullOrEmpty(box.Text) || this.IsValidValue(box.Text, out num))
            {
                if (!num.Equals(box.Tag))
                {
                    box.Tag = num;
                    base.RaiseFilterChanged();
                }
                box.ResetForeColor();
                box.ResetBackColor();
            }
            else
            {
                box.BackColor = Settings.TextBoxError;
                box.ForeColor = SystemColors.HighlightText;
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
                VirtualProperty property = VirtualProperty.Get((int) this.tslPropertyName.Tag);
                BasicFilter filter = null;
                switch (System.Type.GetTypeCode(property.PropertyType))
                {
                    case TypeCode.Byte:
                        filter = new IntegralFilter<byte> {
                            ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag,
                            FromValue = Convert.ToByte(this.tstbValueFrom.Tag),
                            ToValue = Convert.ToByte(this.tstbValueTo.Tag)
                        };
                        break;

                    case TypeCode.Int16:
                        filter = new IntegralFilter<short> {
                            ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag,
                            FromValue = Convert.ToInt16(this.tstbValueFrom.Tag),
                            ToValue = Convert.ToInt16(this.tstbValueTo.Tag)
                        };
                        break;

                    case TypeCode.UInt16:
                        filter = new IntegralFilter<ushort> {
                            ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag,
                            FromValue = Convert.ToUInt16(this.tstbValueFrom.Tag),
                            ToValue = Convert.ToUInt16(this.tstbValueTo.Tag)
                        };
                        break;

                    case TypeCode.Int32:
                        filter = new IntegralFilter<int> {
                            ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag,
                            FromValue = Convert.ToInt32(this.tstbValueFrom.Tag),
                            ToValue = Convert.ToInt32(this.tstbValueTo.Tag)
                        };
                        break;

                    case TypeCode.UInt32:
                        filter = new IntegralFilter<uint> {
                            ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag,
                            FromValue = Convert.ToUInt32(this.tstbValueFrom.Tag),
                            ToValue = Convert.ToUInt32(this.tstbValueTo.Tag)
                        };
                        break;

                    case TypeCode.Int64:
                        filter = new IntegralFilter<long> {
                            ValueComparision = (SimpleComparision) this.tsddValueOperation.Tag,
                            FromValue = Convert.ToInt64(this.tstbValueFrom.Tag),
                            ToValue = Convert.ToInt64(this.tstbValueTo.Tag)
                        };
                        break;
                }
                if (filter == null)
                {
                    throw new InvalidOperationException("Unsupported property type");
                }
                return new VirtualPropertyFilter { PropertyId = property.PropertyId, Filter = filter };
            }
        }

        public bool IsEmpty
        {
            get
            {
                long num;
                bool flag = this.IsValidValue(this.tstbValueFrom.Text, out num);
                SimpleComparision tag = (SimpleComparision) this.tsddValueOperation.Tag;
                if (flag && ((tag == SimpleComparision.Between) || (tag == SimpleComparision.NotBetween)))
                {
                    flag = this.IsValidValue(this.tstbValueTo.Text, out num);
                }
                return !flag;
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

