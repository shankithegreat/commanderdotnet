namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class HexContentFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private MaskedTextProvider MaskProvider;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripDropDownButton tsddContentComparision;
        private ToolStrip tsHexContent;
        private ToolStripMenuItem tsmiComparisionContains;
        private ToolStripMenuItem tsmiComparisionNotContains;
        private ToolStripTextBox tstbHex;

        public HexContentFilterControl()
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
            this.MaskProvider = new MaskedTextProvider("aa", CultureInfo.InvariantCulture, true);
        }

        public HexContentFilterControl(HexContentFilter filter)
        {
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            base.CanRaiseFilterChanged = false;
            CustomFilterControl.PerformDropDownClick(this.tsddContentComparision, filter.Comparision);
            this.MaskProvider = new MaskedTextProvider(CreateMask(filter.Sequence.Length * 2), CultureInfo.InvariantCulture, true);
            this.MaskProvider.Add(filter.SequenceAsString);
            this.tstbHex.Text = this.MaskProvider.ToDisplayString();
            base.CanRaiseFilterChanged = true;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.tsmiComparisionContains.PerformClick();
            this.MaskProvider = new MaskedTextProvider("aa", CultureInfo.InvariantCulture, true);
            this.tstbHex.Text = this.MaskProvider.ToDisplayString();
            base.CanRaiseFilterChanged = true;
        }

        private static string CreateMask(int textLength)
        {
            int num = textLength / 2;
            StringBuilder builder = new StringBuilder((num * 3) + 2);
            for (int i = 0; i < num; i++)
            {
                builder.Append("AA ");
            }
            if ((textLength % 2) == 0)
            {
                builder.Append("aa");
            }
            else
            {
                builder.Append("AA");
            }
            return builder.ToString();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(HexContentFilterControl));
            this.tsHexContent = new ToolStrip();
            this.toolStripLabel1 = new ToolStripLabel();
            this.tsddContentComparision = new ToolStripDropDownButton();
            this.tsmiComparisionContains = new ToolStripMenuItem();
            this.tsmiComparisionNotContains = new ToolStripMenuItem();
            this.toolStripLabel2 = new ToolStripLabel();
            this.tstbHex = new ToolStripTextBox();
            this.tsHexContent.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.tsHexContent, "tsHexContent");
            this.tsHexContent.GripStyle = ToolStripGripStyle.Hidden;
            this.tsHexContent.Items.AddRange(new ToolStripItem[] { this.toolStripLabel1, this.tsddContentComparision, this.toolStripLabel2, this.tstbHex });
            this.tsHexContent.Name = "tsHexContent";
            this.toolStripLabel1.Name = "toolStripLabel1";
            manager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
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
            this.toolStripLabel2.Name = "toolStripLabel2";
            manager.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            this.tstbHex.BorderStyle = BorderStyle.FixedSingle;
            this.tstbHex.Name = "tstbHex";
            manager.ApplyResources(this.tstbHex, "tstbHex");
            this.tstbHex.KeyDown += new KeyEventHandler(this.tstbHex_KeyDown);
            this.tstbHex.KeyPress += new KeyPressEventHandler(this.tstbHex_KeyPress);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsHexContent);
            base.Name = "HexContentFilterControl";
            this.tsHexContent.ResumeLayout(false);
            this.tsHexContent.PerformLayout();
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
            this.tsHexContent.Font = this.Font;
            this.tstbHex.Font = this.Font;
        }

        private void tsmiComparisionContains_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddContentComparision, (ToolStripItem) sender);
        }

        private void tstbHex_KeyDown(object sender, KeyEventArgs e)
        {
            int num;
            int startPosition = -1;
            switch (e.KeyData)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                {
                    int num3 = ((e.KeyData == Keys.Up) || (e.KeyData == Keys.Left)) ? -1 : 1;
                    num = this.MaskProvider.FindEditPositionFrom(this.tstbHex.SelectionStart + num3, num3 > 0);
                    if (num != MaskedTextProvider.InvalidIndex)
                    {
                        this.tstbHex.SelectionStart = num;
                    }
                    this.tstbHex.SelectionLength = 0;
                    e.Handled = true;
                    break;
                }
                case Keys.Delete:
                    startPosition = this.tstbHex.SelectionStart;
                    break;

                case Keys.Back:
                    if (this.tstbHex.SelectionLength == 0)
                    {
                        startPosition = this.MaskProvider.FindEditPositionFrom(this.tstbHex.SelectionStart - 1, false);
                    }
                    else
                    {
                        startPosition = this.tstbHex.SelectionStart;
                    }
                    break;
            }
            if (startPosition >= 0)
            {
                MaskedTextResultHint hint;
                if (this.MaskProvider.RemoveAt(startPosition, startPosition + ((this.tstbHex.SelectionLength == 0) ? 0 : (this.tstbHex.SelectionLength - 1)), out num, out hint))
                {
                    string input = this.MaskProvider.ToString(false, false);
                    this.MaskProvider = new MaskedTextProvider(CreateMask(input.Length), CultureInfo.InvariantCulture, true);
                    this.MaskProvider.Add(input);
                    this.tstbHex.Text = this.MaskProvider.ToDisplayString();
                    this.tstbHex.SelectionStart = num;
                    base.RaiseFilterChanged();
                }
                e.Handled = true;
            }
        }

        private void tstbHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                int num;
                MaskedTextResultHint hint;
                if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
                {
                    e.KeyChar = (char) (('A' + e.KeyChar) - 0x61);
                }
                else if (((e.KeyChar < '0') || ((e.KeyChar > '9') && (e.KeyChar < 'A'))) || (e.KeyChar > 'F'))
                {
                    return;
                }
                if (((this.tstbHex.SelectionStart <= 0) || !this.MaskProvider.IsAvailablePosition(this.tstbHex.SelectionStart - 1)) && this.MaskProvider.InsertAt(e.KeyChar, this.tstbHex.SelectionStart, out num, out hint))
                {
                    if (this.MaskProvider.MaskFull)
                    {
                        string input = this.MaskProvider.ToString(false, false);
                        this.MaskProvider = new MaskedTextProvider(CreateMask(input.Length), CultureInfo.InvariantCulture, true);
                        this.MaskProvider.Add(input);
                    }
                    this.tstbHex.Text = this.MaskProvider.ToDisplayString();
                    this.tstbHex.SelectionStart = this.MaskProvider.FindEditPositionFrom(num + 1, true);
                    base.RaiseFilterChanged();
                }
            }
            finally
            {
                e.Handled = true;
            }
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddContentComparision, this.tsddContentComparision.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                string str = this.MaskProvider.ToString(false, false);
                if (str.Length > 0)
                {
                    if ((str.Length % 2) == 1)
                    {
                        str = str + '0';
                    }
                    return new VirtualItemHexContentFilter { SequenceAsString = str, Comparision = (ContentComparision) this.tsddContentComparision.Tag };
                }
                return null;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.MaskProvider.ToString(false, false));
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsHexContent;
            }
        }
    }
}

