namespace Nomad.Controls.Filter
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class CustomFilterControl : UserControl
    {
        private IContainer components = null;
        private bool FCanRaiseFilterChanged = true;
        private int FCanRaiseFilterIndex;

        public event EventHandler FilterChanged;

        public CustomFilterControl()
        {
            this.InitializeComponent();
        }

        protected void Condition_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            foreach (ToolStripMenuItem item2 in item.DropDownItems)
            {
                item2.Checked = item2.Tag == item.Tag;
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

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
        }

        protected static void PerformDropDownClick(ToolStripDropDownItem dropDownItem, object ItemTag)
        {
            foreach (ToolStripItem item in dropDownItem.DropDownItems)
            {
                if (ItemTag.Equals(item.Tag))
                {
                    item.PerformClick();
                    break;
                }
            }
        }

        protected void RaiseFilterChanged()
        {
            if (this.CanRaiseFilterChanged && (this.FilterChanged != null))
            {
                this.FilterChanged(this, EventArgs.Empty);
            }
        }

        protected void UpdateDropDownCondition(ToolStripItem dropDownItem, ToolStripItem clickedItem)
        {
            if (dropDownItem.Tag != clickedItem.Tag)
            {
                dropDownItem.Text = clickedItem.Text;
                dropDownItem.Tag = clickedItem.Tag;
                this.RaiseFilterChanged();
            }
        }

        protected void UpdateDropDownText(ToolStripDropDownItem dropDownItem, object ItemTag)
        {
            foreach (ToolStripItem item in dropDownItem.DropDownItems)
            {
                if (ItemTag.Equals(item.Tag))
                {
                    dropDownItem.Text = item.Text;
                    break;
                }
            }
        }

        protected bool CanRaiseFilterChanged
        {
            get
            {
                return this.FCanRaiseFilterChanged;
            }
            set
            {
                if (!value)
                {
                    this.FCanRaiseFilterChanged = false;
                    this.FCanRaiseFilterIndex++;
                }
                else
                {
                    this.FCanRaiseFilterIndex--;
                    if (this.FCanRaiseFilterIndex == 0)
                    {
                        this.FCanRaiseFilterChanged = true;
                    }
                }
            }
        }
    }
}

