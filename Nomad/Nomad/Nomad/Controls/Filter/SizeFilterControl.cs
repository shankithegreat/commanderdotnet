namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class SizeFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private ToolStripDropDownButton tsddSizeOperation;
        private ToolStripDropDownButton tsddSizeUnit;
        private ToolStripLabel tslAnd;
        private ToolStripLabel tslSize;
        private ToolStripMenuItem tsmiOperationBetween;
        private ToolStripMenuItem tsmiOperationEquals;
        private ToolStripMenuItem tsmiOperationLarger;
        private ToolStripMenuItem tsmiOperationNotBetween;
        private ToolStripMenuItem tsmiOperationPercentOf25;
        private ToolStripMenuItem tsmiOperationPercentOf50;
        private ToolStripMenuItem tsmiOperationSmaller;
        private ToolStripMenuItem tsmiUnitByte;
        private ToolStripMenuItem tsmiUnitKiloByte;
        private ToolStripMenuItem tsmiUnitMegaByte;
        private ToolStrip tsSize;
        private ToolStripTextBox tstbSizeFrom;
        private ToolStripTextBox tstbSizeTo;

        public SizeFilterControl()
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
        }

        public SizeFilterControl(SizeFilter filter)
        {
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            base.CanRaiseFilterChanged = false;
            CustomFilterControl.PerformDropDownClick(this.tsddSizeOperation, filter.SizeComparision);
            this.tstbSizeFrom.Tag = filter.FromValue;
            this.tstbSizeFrom.Text = filter.FromValue.ToString();
            this.tstbSizeTo.Tag = filter.ToValue;
            this.tstbSizeTo.Text = filter.ToValue.ToString();
            CustomFilterControl.PerformDropDownClick(this.tsddSizeUnit, filter.SizeUnit);
            base.CanRaiseFilterChanged = true;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.tstbSizeFrom.Tag = 0L;
            this.tstbSizeFrom.Text = this.tstbSizeFrom.Tag.ToString();
            this.tstbSizeTo.Tag = this.tstbSizeFrom.Tag;
            this.tstbSizeTo.Text = this.tstbSizeFrom.Text;
            this.tsmiOperationEquals.PerformClick();
            this.tsmiUnitKiloByte.PerformClick();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SizeFilterControl));
            this.tsSize = new ToolStrip();
            this.tslSize = new ToolStripLabel();
            this.tsddSizeOperation = new ToolStripDropDownButton();
            this.tsmiOperationEquals = new ToolStripMenuItem();
            this.tsmiOperationSmaller = new ToolStripMenuItem();
            this.tsmiOperationLarger = new ToolStripMenuItem();
            this.tsmiOperationBetween = new ToolStripMenuItem();
            this.tsmiOperationNotBetween = new ToolStripMenuItem();
            this.tsmiOperationPercentOf25 = new ToolStripMenuItem();
            this.tsmiOperationPercentOf50 = new ToolStripMenuItem();
            this.tstbSizeFrom = new ToolStripTextBox();
            this.tslAnd = new ToolStripLabel();
            this.tstbSizeTo = new ToolStripTextBox();
            this.tsddSizeUnit = new ToolStripDropDownButton();
            this.tsmiUnitByte = new ToolStripMenuItem();
            this.tsmiUnitKiloByte = new ToolStripMenuItem();
            this.tsmiUnitMegaByte = new ToolStripMenuItem();
            this.tsSize.SuspendLayout();
            base.SuspendLayout();
            this.tsSize.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsSize, "tsSize");
            this.tsSize.GripStyle = ToolStripGripStyle.Hidden;
            this.tsSize.Items.AddRange(new ToolStripItem[] { this.tslSize, this.tsddSizeOperation, this.tstbSizeFrom, this.tslAnd, this.tstbSizeTo, this.tsddSizeUnit });
            this.tsSize.Name = "tsSize";
            this.tslSize.Name = "tslSize";
            manager.ApplyResources(this.tslSize, "tslSize");
            this.tsddSizeOperation.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddSizeOperation.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOperationEquals, this.tsmiOperationSmaller, this.tsmiOperationLarger, this.tsmiOperationBetween, this.tsmiOperationNotBetween, this.tsmiOperationPercentOf25, this.tsmiOperationPercentOf50 });
            this.tsddSizeOperation.Name = "tsddSizeOperation";
            manager.ApplyResources(this.tsddSizeOperation, "tsddSizeOperation");
            this.tsmiOperationEquals.Name = "tsmiOperationEquals";
            manager.ApplyResources(this.tsmiOperationEquals, "tsmiOperationEquals");
            this.tsmiOperationEquals.Click += new EventHandler(this.tsmiOperationEquals_Click);
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
            this.tsmiOperationPercentOf25.Name = "tsmiOperationPercentOf25";
            manager.ApplyResources(this.tsmiOperationPercentOf25, "tsmiOperationPercentOf25");
            this.tsmiOperationPercentOf25.Click += new EventHandler(this.tsmiOperationEquals_Click);
            this.tsmiOperationPercentOf50.Name = "tsmiOperationPercentOf50";
            manager.ApplyResources(this.tsmiOperationPercentOf50, "tsmiOperationPercentOf50");
            this.tsmiOperationPercentOf50.Click += new EventHandler(this.tsmiOperationEquals_Click);
            this.tstbSizeFrom.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbSizeFrom, "tstbSizeFrom");
            this.tstbSizeFrom.Name = "tstbSizeFrom";
            this.tstbSizeFrom.KeyPress += new KeyPressEventHandler(this.tstbSizeFrom_KeyPress);
            this.tstbSizeFrom.TextChanged += new EventHandler(this.tstbSizeFrom_TextChanged);
            this.tslAnd.Name = "tslAnd";
            manager.ApplyResources(this.tslAnd, "tslAnd");
            this.tstbSizeTo.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbSizeTo, "tstbSizeTo");
            this.tstbSizeTo.Name = "tstbSizeTo";
            this.tstbSizeTo.KeyPress += new KeyPressEventHandler(this.tstbSizeFrom_KeyPress);
            this.tstbSizeTo.TextChanged += new EventHandler(this.tstbSizeFrom_TextChanged);
            this.tsddSizeUnit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddSizeUnit.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiUnitByte, this.tsmiUnitKiloByte, this.tsmiUnitMegaByte });
            this.tsddSizeUnit.Name = "tsddSizeUnit";
            manager.ApplyResources(this.tsddSizeUnit, "tsddSizeUnit");
            this.tsmiUnitByte.Name = "tsmiUnitByte";
            manager.ApplyResources(this.tsmiUnitByte, "tsmiUnitByte");
            this.tsmiUnitByte.Click += new EventHandler(this.tsmiUnitByte_Click);
            this.tsmiUnitKiloByte.Name = "tsmiUnitKiloByte";
            manager.ApplyResources(this.tsmiUnitKiloByte, "tsmiUnitKiloByte");
            this.tsmiUnitKiloByte.Click += new EventHandler(this.tsmiUnitByte_Click);
            this.tsmiUnitMegaByte.Name = "tsmiUnitMegaByte";
            manager.ApplyResources(this.tsmiUnitMegaByte, "tsmiUnitMegaByte");
            this.tsmiUnitMegaByte.Click += new EventHandler(this.tsmiUnitByte_Click);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsSize);
            base.Name = "SizeFilterControl";
            this.tsSize.ResumeLayout(false);
            this.tsSize.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolStripItems()
        {
            this.tsmiOperationEquals.Tag = SizeComparision.Equals;
            this.tsmiOperationSmaller.Tag = SizeComparision.Smaller;
            this.tsmiOperationLarger.Tag = SizeComparision.Larger;
            this.tsmiOperationBetween.Tag = SizeComparision.Between;
            this.tsmiOperationNotBetween.Tag = SizeComparision.NotBetween;
            this.tsmiOperationPercentOf25.Tag = SizeComparision.PercentOf25;
            this.tsmiOperationPercentOf50.Tag = SizeComparision.PercentOf50;
            this.tsmiUnitByte.Tag = SizeUnit.Byte;
            this.tsmiUnitKiloByte.Tag = SizeUnit.KiloByte;
            this.tsmiUnitMegaByte.Tag = SizeUnit.MegaByte;
            this.tsddSizeOperation.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
            this.tsddSizeUnit.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsSize.Font = this.Font;
            this.tstbSizeFrom.Font = this.Font;
            this.tstbSizeTo.Font = this.Font;
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Size) > BoundsSpecified.None) && ((factor.Width != 1.0) || (factor.Height != 1.0)))
            {
                this.tstbSizeTo.TextBox.Scale(factor);
            }
        }

        private void tsmiOperationBetween_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddSizeOperation, (ToolStripItem) sender);
            this.tslAnd.Visible = true;
            this.tstbSizeTo.Visible = true;
        }

        private void tsmiOperationEquals_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddSizeOperation, (ToolStripItem) sender);
            this.tslAnd.Visible = false;
            this.tstbSizeTo.Visible = false;
        }

        private void tsmiUnitByte_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddSizeUnit, (ToolStripItem) sender);
        }

        private void tstbSizeFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar) && (e.KeyChar != '\b');
        }

        private void tstbSizeFrom_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox box = (ToolStripTextBox) sender;
            long result = 0L;
            if ((string.IsNullOrEmpty(box.Text) || long.TryParse(box.Text, out result)) && (result >= 0L))
            {
                if (!result.Equals(box.Tag))
                {
                    box.Tag = result;
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
            base.UpdateDropDownText(this.tsddSizeOperation, this.tsddSizeOperation.Tag);
            base.UpdateDropDownText(this.tsddSizeUnit, this.tsddSizeUnit.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                if (this.IsEmpty)
                {
                    return null;
                }
                return new VirtualItemSizeFilter { SizeComparision = (SizeComparision) this.tsddSizeOperation.Tag, FromValue = Convert.ToInt64(this.tstbSizeFrom.Tag), ToValue = Convert.ToInt64(this.tstbSizeTo.Tag), SizeUnit = (SizeUnit) this.tsddSizeUnit.Tag };
            }
        }

        public bool IsEmpty
        {
            get
            {
                long num;
                bool flag = long.TryParse(this.tstbSizeFrom.Text, out num) && (num >= 0L);
                SizeComparision tag = (SizeComparision) this.tsddSizeOperation.Tag;
                if (flag && ((tag == SizeComparision.Between) || (tag == SizeComparision.NotBetween)))
                {
                    flag = long.TryParse(this.tstbSizeTo.Text, out num) && (num >= 0L);
                }
                return !flag;
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsSize;
            }
        }
    }
}

