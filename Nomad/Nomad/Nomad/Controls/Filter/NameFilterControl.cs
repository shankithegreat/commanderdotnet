namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class NameFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private ToolStripDropDownButton tsddNameCondition;
        private ToolStripLabel tslMask;
        private ToolStripLabel tslName;
        private ToolStripMenuItem tsmiConditionEqual;
        private ToolStripMenuItem tsmiConditionNotEqual;
        private ToolStrip tsName;
        private ToolStripTextBox tstbMask;

        public NameFilterControl()
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
        }

        public NameFilterControl(NameFilter filter)
        {
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            base.CanRaiseFilterChanged = false;
            CustomFilterControl.PerformDropDownClick(this.tsddNameCondition, filter.NameCondition);
            this.tstbMask.Text = filter.ToString();
            base.CanRaiseFilterChanged = true;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.tsmiConditionEqual.PerformClick();
            this.tstbMask.Text = string.Empty;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(NameFilterControl));
            this.tsName = new ToolStrip();
            this.tslName = new ToolStripLabel();
            this.tsddNameCondition = new ToolStripDropDownButton();
            this.tsmiConditionEqual = new ToolStripMenuItem();
            this.tsmiConditionNotEqual = new ToolStripMenuItem();
            this.tslMask = new ToolStripLabel();
            this.tstbMask = new ToolStripTextBox();
            this.tsName.SuspendLayout();
            base.SuspendLayout();
            this.tsName.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsName, "tsName");
            this.tsName.GripStyle = ToolStripGripStyle.Hidden;
            this.tsName.Items.AddRange(new ToolStripItem[] { this.tslName, this.tsddNameCondition, this.tslMask, this.tstbMask });
            this.tsName.Name = "tsName";
            this.tslName.Name = "tslName";
            manager.ApplyResources(this.tslName, "tslName");
            this.tsddNameCondition.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddNameCondition.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiConditionEqual, this.tsmiConditionNotEqual });
            manager.ApplyResources(this.tsddNameCondition, "tsddNameCondition");
            this.tsddNameCondition.Name = "tsddNameCondition";
            this.tsmiConditionEqual.Name = "tsmiConditionEqual";
            manager.ApplyResources(this.tsmiConditionEqual, "tsmiConditionEqual");
            this.tsmiConditionEqual.Click += new EventHandler(this.tsmiConditionEqual_Click);
            this.tsmiConditionNotEqual.Name = "tsmiConditionNotEqual";
            manager.ApplyResources(this.tsmiConditionNotEqual, "tsmiConditionNotEqual");
            this.tsmiConditionNotEqual.Click += new EventHandler(this.tsmiConditionEqual_Click);
            this.tslMask.Name = "tslMask";
            manager.ApplyResources(this.tslMask, "tslMask");
            this.tstbMask.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbMask, "tstbMask");
            this.tstbMask.Name = "tstbMask";
            this.tstbMask.TextChanged += new EventHandler(this.tstbMask_TextChanged);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsName);
            base.Name = "NameFilterControl";
            this.tsName.ResumeLayout(false);
            this.tsName.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolStripItems()
        {
            this.tsmiConditionEqual.Tag = NamePatternCondition.Equal;
            this.tsmiConditionNotEqual.Tag = NamePatternCondition.NotEqual;
            this.tsddNameCondition.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsName.Font = this.Font;
            this.tstbMask.Font = this.Font;
        }

        private void tsmiConditionEqual_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddNameCondition, (ToolStripItem) sender);
        }

        private void tstbMask_TextChanged(object sender, EventArgs e)
        {
            base.RaiseFilterChanged();
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddNameCondition, this.tsddNameCondition.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                if (!this.IsEmpty)
                {
                    return new VirtualItemNameFilter((NamePatternCondition) this.tsddNameCondition.Tag, this.tstbMask.Text);
                }
                return null;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.tstbMask.Text);
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsName;
            }
        }
    }
}

