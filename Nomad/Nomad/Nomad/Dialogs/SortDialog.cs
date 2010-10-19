namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SortDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private CheckBox chkNaturalNumber;
        private CheckBox chkRememberSort;
        private CheckBox chkReversedOrder;
        private ComboBoxEx cmbSortBy;
        private IContainer components = null;
        private VirtualPropertySet FAvailableProperties;
        private ImageList imgProperty;
        private int LastSelectedIndex = -1;
        private Label lblSortBy;

        public SortDialog()
        {
            this.InitializeComponent();
            this.imgProperty.Images.Add(IconSet.GetImage("PropertyType_Other"));
            this.imgProperty.Images.Add(IconSet.GetImage("PropertyType_Integral"));
            this.imgProperty.Images.Add(IconSet.GetImage("PropertyType_String"));
            this.imgProperty.Images.Add(IconSet.GetImage("PropertyType_DateTime"));
            this.imgProperty.Images.Add(IconSet.GetImage("PropertyType_Version"));
            int groupId = 0;
            foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) VirtualProperty.Visible)
            {
                if (property.GroupId != groupId)
                {
                    this.cmbSortBy.Items.Add(string.Empty);
                    groupId = property.GroupId;
                }
                this.cmbSortBy.Items.Add(property);
            }
        }

        private void cmbSortBy_DrawItem(object sender, DrawItemEventArgs e)
        {
            VirtualProperty property = this.cmbSortBy.Items[e.Index] as VirtualProperty;
            if (property == null)
            {
                int num4 = e.Bounds.Top + (e.Bounds.Height / 2);
                e.Graphics.DrawLine(SystemPens.ControlText, e.Bounds.Left, num4, e.Bounds.Right, num4);
            }
            else
            {
                Color windowText;
                int num;
                if ((e.State & DrawItemState.Checked) > DrawItemState.None)
                {
                    windowText = SystemColors.WindowText;
                }
                else
                {
                    e.DrawBackground();
                    windowText = e.ForeColor;
                }
                if (!this.AvailableProperties[property.PropertyId])
                {
                    windowText = SystemColors.GrayText;
                }
                switch (System.Type.GetTypeCode(property.PropertyType))
                {
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        num = 1;
                        break;

                    case TypeCode.DateTime:
                        num = 3;
                        break;

                    case TypeCode.String:
                        num = 2;
                        break;

                    default:
                        if (property.PropertyType == typeof(Version))
                        {
                            num = 4;
                        }
                        else
                        {
                            num = 0;
                        }
                        break;
                }
                int num2 = e.Bounds.Height - this.imgProperty.ImageSize.Height;
                int num3 = (num2 / 2) + (((e.State & DrawItemState.Checked) > DrawItemState.None) ? (num2 % 2) : 0);
                this.imgProperty.Draw(e.Graphics, e.Bounds.Left + 4, e.Bounds.Top + num3, num);
                Rectangle bounds = Rectangle.FromLTRB(e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom);
                TextRenderer.DrawText(e.Graphics, property.LocalizedName, e.Font, bounds, windowText, TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
                if ((e.State & DrawItemState.Focus) > DrawItemState.None)
                {
                    e.DrawFocusRectangle();
                }
            }
        }

        private void cmbSortBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = new string(e.KeyChar, 1);
            int num = (this.cmbSortBy.SelectedIndex < (this.cmbSortBy.Items.Count - 1)) ? (this.cmbSortBy.SelectedIndex + 1) : 0;
            while (num != this.cmbSortBy.SelectedIndex)
            {
                VirtualProperty property = this.cmbSortBy.Items[num] as VirtualProperty;
                if ((property != null) && property.LocalizedName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.cmbSortBy.SelectedIndex = num;
                    break;
                }
                if (++num >= this.cmbSortBy.Items.Count)
                {
                    num = 0;
                }
            }
        }

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LastSelectedIndex = this.cmbSortBy.SelectedIndex;
        }

        private void cmbSortBy_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedIndex = this.cmbSortBy.SelectedIndex;
            if (!((selectedIndex < 0) || (this.cmbSortBy.SelectedItem is VirtualProperty)))
            {
                selectedIndex += (this.cmbSortBy.SelectedIndex > this.LastSelectedIndex) ? 1 : -1;
            }
            this.cmbSortBy.SelectedIndex = selectedIndex;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute()
        {
            return (base.ShowDialog() == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SortDialog));
            this.lblSortBy = new Label();
            this.chkRememberSort = new CheckBox();
            this.chkReversedOrder = new CheckBox();
            this.cmbSortBy = new ComboBoxEx();
            this.chkNaturalNumber = new CheckBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.imgProperty = new ImageList(this.components);
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            Bevel bevel = new Bevel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblSortBy, 0, 0);
            panel.Controls.Add(this.chkRememberSort, 0, 4);
            panel.Controls.Add(this.chkReversedOrder, 0, 3);
            panel.Controls.Add(this.cmbSortBy, 0, 1);
            panel.Controls.Add(this.chkNaturalNumber, 0, 2);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblSortBy, "lblSortBy");
            this.lblSortBy.Name = "lblSortBy";
            manager.ApplyResources(this.chkRememberSort, "chkRememberSort");
            this.chkRememberSort.Name = "chkRememberSort";
            this.chkRememberSort.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkReversedOrder, "chkReversedOrder");
            this.chkReversedOrder.Name = "chkReversedOrder";
            this.chkReversedOrder.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbSortBy, "cmbSortBy");
            this.cmbSortBy.DrawMode = DrawMode.OwnerDrawFixed;
            this.cmbSortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSortBy.FormattingEnabled = true;
            this.cmbSortBy.Name = "cmbSortBy";
            this.cmbSortBy.DrawItem += new DrawItemEventHandler(this.cmbSortBy_DrawItem);
            this.cmbSortBy.SelectionChangeCommitted += new EventHandler(this.cmbSortBy_SelectionChangeCommitted);
            this.cmbSortBy.SelectedIndexChanged += new EventHandler(this.cmbSortBy_SelectedIndexChanged);
            this.cmbSortBy.KeyPress += new KeyPressEventHandler(this.cmbSortBy_KeyPress);
            manager.ApplyResources(this.chkNaturalNumber, "chkNaturalNumber");
            this.chkNaturalNumber.Name = "chkNaturalNumber";
            this.chkNaturalNumber.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.Controls.Add(this.btnOk, 1, 0);
            panel2.Controls.Add(this.btnCancel, 2, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.imgProperty.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgProperty, "imgProperty");
            this.imgProperty.TransparentColor = Color.Transparent;
            manager.ApplyResources(bevel, "bvlButtons");
            bevel.ForeColor = SystemColors.ControlDarkDark;
            bevel.Name = "bvlButtons";
            bevel.Sides = Border3DSide.Top;
            bevel.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel2);
            base.Controls.Add(bevel);
            base.Controls.Add(panel);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SortDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.SortDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Height) > BoundsSpecified.None) && (factor.Height != 1.0))
            {
                this.cmbSortBy.ItemHeight = (int) Math.Round((double) (this.cmbSortBy.ItemHeight * factor.Height));
            }
        }

        private void SortDialog_Shown(object sender, EventArgs e)
        {
            this.cmbSortBy.Select();
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                return this.FAvailableProperties;
            }
            set
            {
                this.FAvailableProperties = value;
            }
        }

        public bool RememberSort
        {
            get
            {
                return (this.chkRememberSort.Enabled && this.chkRememberSort.Checked);
            }
        }

        public bool RememberSortEnabled
        {
            get
            {
                return this.chkRememberSort.Visible;
            }
            set
            {
                this.chkRememberSort.Enabled = value;
                this.chkRememberSort.Visible = value;
            }
        }

        public VirtualItemComparer Sort
        {
            get
            {
                VirtualProperty selectedItem = (VirtualProperty) this.cmbSortBy.SelectedItem;
                return new VirtualItemComparer(selectedItem.PropertyId, this.chkReversedOrder.Checked ? ListSortDirection.Descending : ListSortDirection.Ascending, NameComparison.Alphabet | (this.chkNaturalNumber.Checked ? NameComparison.Natural : NameComparison.Default));
            }
            set
            {
                if (value == null)
                {
                    this.cmbSortBy.SelectedItem = VirtualProperty.Get(1);
                    this.chkReversedOrder.Checked = false;
                    this.chkNaturalNumber.Checked = true;
                }
                else
                {
                    this.cmbSortBy.SelectedItem = VirtualProperty.Get(value.ComparePropertyId);
                    this.chkReversedOrder.Checked = value.SortDirection == ListSortDirection.Descending;
                    this.chkNaturalNumber.Checked = (value.NameComparison & NameComparison.Natural) > NameComparison.Default;
                }
            }
        }
    }
}

