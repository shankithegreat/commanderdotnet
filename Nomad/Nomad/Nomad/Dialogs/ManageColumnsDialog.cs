namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Collections;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Property;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ManageColumnsDialog : BasicDialog
    {
        protected Button btnCancel;
        private Button btnDeleteTemplate;
        protected Button btnDown;
        private Button btnHide;
        protected Button btnOk;
        private Button btnSaveTemplate;
        private Button btnShow;
        protected Button btnUp;
        private Bevel bvlButtons;
        private Bevel bvlTemplate;
        private CheckBox chkAutosizeColumns;
        private CheckBox chkRememberColumns;
        private TemplateComboBox cmbTemplate;
        private Dictionary<int, ListViewColumnInfo> ColumnInfoMap = new Dictionary<int, ListViewColumnInfo>();
        private IContainer components = null;
        private VirtualPropertySet FAvailableProperties;
        private Dictionary<int, ListViewGroup> GroupMap = new Dictionary<int, ListViewGroup>();
        private ImageList imgAligns;
        protected ListViewEx lvItems;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpButtons;
        private ToolStrip tsAlign;
        private ToolStripButton tsbCenter;
        private ToolStripButton tsbLeftAlign;
        private ToolStripButton tsbRightAlign;

        public ManageColumnsDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            if (this.lvItems.ExplorerTheme)
            {
                this.imgAligns.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
            }
            else
            {
                this.imgAligns.ImageSize = ImageHelper.DefaultSmallIconSize;
            }
            this.imgAligns.AddAspected(IconSet.GetImage("TextAlignLeft"));
            this.imgAligns.AddAspected(IconSet.GetImage("TextAlignRight"));
            this.imgAligns.AddAspected(IconSet.GetImage("TextAlignCenter"));
            this.tsAlign.BackColor = this.BackColor;
            this.tsAlign.Renderer = BorderLessToolStripRenderer.Default;
            this.btnSaveTemplate.Image = IconSet.GetImage("SaveAs");
            this.tsbLeftAlign.Image = IconSet.GetImage("TextAlignLeft");
            this.tsbCenter.Image = IconSet.GetImage("TextAlignCenter");
            this.tsbRightAlign.Image = IconSet.GetImage("TextAlignRight");
            this.tsbLeftAlign.Tag = HorizontalAlignment.Left;
            this.tsbCenter.Tag = HorizontalAlignment.Center;
            this.tsbRightAlign.Tag = HorizontalAlignment.Right;
            this.tsAlign.Padding = new Padding((this.tsAlign.Width - ((this.tsbLeftAlign.Width + this.tsbCenter.Width) + this.tsbRightAlign.Width)) / 2, 0, 0, 0);
            this.cmbTemplate.SetItems<ListViewColumnInfo[]>(Settings.Default.ColumnTemplates);
            bool flag = SettingsManager.CheckSafeMode(SafeMode.SkipFormPlacement) || (Control.ModifierKeys == Keys.Shift);
            FormSettings.RegisterForm(this, flag ? FormPlacement.None : FormPlacement.Size);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.lvItems.FocusedItem.Checked = false;
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (focusedItem != null)
            {
                if (sender == this.btnUp)
                {
                    focusedItem.MoveUpInGroup(true);
                }
                else
                {
                    focusedItem.MoveDownInGroup(true);
                }
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            this.cmbTemplate.Save<ListViewColumnInfo[]>(new List<ListViewColumnInfo>(this.Columns).ToArray());
            this.UpdateAcceptButton();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.lvItems.FocusedItem.Checked = true;
        }

        private void cmbTemplate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.cmbTemplate.SelectedIndex >= 0)
            {
                this.Columns = this.cmbTemplate.GetValue<IEnumerable<ListViewColumnInfo>>(this.cmbTemplate.SelectedIndex);
            }
            this.UpdateAcceptButton();
        }

        private void cmbTemplate_TextUpdate(object sender, EventArgs e)
        {
            this.UpdateAcceptButton();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ManageColumnsDialog));
            this.lvItems = new ListViewEx();
            this.imgAligns = new ImageList(this.components);
            this.btnUp = new Button();
            this.btnHide = new Button();
            this.btnShow = new Button();
            this.btnDown = new Button();
            this.tsAlign = new ToolStrip();
            this.tsbLeftAlign = new ToolStripButton();
            this.tsbCenter = new ToolStripButton();
            this.tsbRightAlign = new ToolStripButton();
            this.chkRememberColumns = new CheckBox();
            this.chkAutosizeColumns = new CheckBox();
            this.cmbTemplate = new TemplateComboBox();
            this.btnDeleteTemplate = new Button();
            this.btnSaveTemplate = new Button();
            this.bvlButtons = new Bevel();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.tlpButtons = new TableLayoutPanel();
            this.bvlTemplate = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            ColumnHeader header = new ColumnHeader();
            ColumnHeader header2 = new ColumnHeader();
            Label control = new Label();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            Label label2 = new Label();
            panel.SuspendLayout();
            this.tsAlign.SuspendLayout();
            panel2.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lvItems, 0, 1);
            panel.Controls.Add(this.btnUp, 1, 1);
            panel.Controls.Add(this.btnHide, 1, 4);
            panel.Controls.Add(this.btnShow, 1, 3);
            panel.Controls.Add(this.btnDown, 1, 2);
            panel.Controls.Add(this.tsAlign, 1, 5);
            panel.Controls.Add(this.chkRememberColumns, 0, 8);
            panel.Controls.Add(this.chkAutosizeColumns, 0, 7);
            panel.Controls.Add(control, 0, 0);
            panel.Name = "tlpBack";
            this.lvItems.AllowDrop = true;
            this.lvItems.CheckBoxes = true;
            this.lvItems.CollapsibleGroups = true;
            this.lvItems.Columns.AddRange(new ColumnHeader[] { header, header2 });
            manager.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.ExplorerTheme = true;
            this.lvItems.FullRowSelect = true;
            this.lvItems.HeaderStyle = ColumnHeaderStyle.None;
            this.lvItems.HideSelection = false;
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            panel.SetRowSpan(this.lvItems, 6);
            this.lvItems.SmallImageList = this.imgAligns;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = View.Details;
            this.lvItems.ItemCheck += new ItemCheckEventHandler(this.lvItems_ItemCheck);
            this.lvItems.KeyDown += new KeyEventHandler(this.lvItems_KeyDown);
            this.lvItems.ItemDrag += new ItemDragEventHandler(this.lvItems_ItemDrag);
            this.lvItems.SelectedIndexChanged += new EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvItems.ItemChecked += new ItemCheckedEventHandler(this.lvItems_ItemChecked);
            this.lvItems.ClientSizeChanged += new EventHandler(this.lvItems_ClientSizeChanged);
            manager.ApplyResources(header, "NameColumn");
            manager.ApplyResources(header2, "WidthColumn");
            this.imgAligns.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgAligns, "imgAligns");
            this.imgAligns.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnMove_Click);
            manager.ApplyResources(this.btnHide, "btnHide");
            this.btnHide.Name = "btnHide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new EventHandler(this.btnHide_Click);
            manager.ApplyResources(this.btnShow, "btnShow");
            this.btnShow.Name = "btnShow";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new EventHandler(this.btnShow_Click);
            manager.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Name = "btnDown";
            this.btnDown.Tag = "1";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnMove_Click);
            this.tsAlign.BackColor = SystemColors.Control;
            this.tsAlign.GripStyle = ToolStripGripStyle.Hidden;
            this.tsAlign.Items.AddRange(new ToolStripItem[] { this.tsbLeftAlign, this.tsbCenter, this.tsbRightAlign });
            manager.ApplyResources(this.tsAlign, "tsAlign");
            this.tsAlign.Name = "tsAlign";
            this.tsAlign.TabStop = true;
            this.tsbLeftAlign.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbLeftAlign, "tsbLeftAlign");
            this.tsbLeftAlign.Image = Resources.ImageThrobber;
            this.tsbLeftAlign.Name = "tsbLeftAlign";
            this.tsbLeftAlign.Paint += new PaintEventHandler(this.tsbAlign_Paint);
            this.tsbLeftAlign.Click += new EventHandler(this.tsbAlign_Click);
            this.tsbCenter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbCenter, "tsbCenter");
            this.tsbCenter.Image = Resources.ImageThrobber;
            this.tsbCenter.Name = "tsbCenter";
            this.tsbCenter.Paint += new PaintEventHandler(this.tsbAlign_Paint);
            this.tsbCenter.Click += new EventHandler(this.tsbAlign_Click);
            this.tsbRightAlign.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbRightAlign, "tsbRightAlign");
            this.tsbRightAlign.Image = Resources.ImageThrobber;
            this.tsbRightAlign.Name = "tsbRightAlign";
            this.tsbRightAlign.Paint += new PaintEventHandler(this.tsbAlign_Paint);
            this.tsbRightAlign.Click += new EventHandler(this.tsbAlign_Click);
            manager.ApplyResources(this.chkRememberColumns, "chkRememberColumns");
            panel.SetColumnSpan(this.chkRememberColumns, 2);
            this.chkRememberColumns.Name = "chkRememberColumns";
            this.chkRememberColumns.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkAutosizeColumns, "chkAutosizeColumns");
            panel.SetColumnSpan(this.chkAutosizeColumns, 2);
            this.chkAutosizeColumns.Name = "chkAutosizeColumns";
            this.chkAutosizeColumns.UseVisualStyleBackColor = true;
            manager.ApplyResources(control, "lblColumns");
            panel.SetColumnSpan(control, 2);
            control.Name = "lblColumns";
            manager.ApplyResources(panel2, "tlpTemplate");
            panel2.Controls.Add(label2, 0, 0);
            panel2.Controls.Add(this.cmbTemplate, 1, 0);
            panel2.Controls.Add(this.btnSaveTemplate, 2, 0);
            panel2.Controls.Add(this.btnDeleteTemplate, 3, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpTemplate";
            manager.ApplyResources(label2, "lblTemplate");
            label2.Name = "lblTemplate";
            this.cmbTemplate.DeleteButton = this.btnDeleteTemplate;
            manager.ApplyResources(this.cmbTemplate, "cmbTemplate");
            this.cmbTemplate.FormattingEnabled = true;
            this.cmbTemplate.Name = "cmbTemplate";
            this.cmbTemplate.SaveButton = this.btnSaveTemplate;
            this.cmbTemplate.SelectionChangeCommitted += new EventHandler(this.cmbTemplate_SelectionChangeCommitted);
            this.cmbTemplate.Leave += new EventHandler(this.cmbTemplate_TextUpdate);
            this.cmbTemplate.Enter += new EventHandler(this.cmbTemplate_TextUpdate);
            this.cmbTemplate.TextUpdate += new EventHandler(this.cmbTemplate_TextUpdate);
            manager.ApplyResources(this.btnDeleteTemplate, "btnDeleteTemplate");
            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
            this.btnDeleteTemplate.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnSaveTemplate, "btnSaveTemplate");
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new EventHandler(this.btnSaveTemplate_Click);
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.bvlTemplate, "bvlTemplate");
            this.bvlTemplate.ForeColor = SystemColors.ControlDarkDark;
            this.bvlTemplate.Name = "bvlTemplate";
            this.bvlTemplate.Sides = Border3DSide.Top;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel);
            base.Controls.Add(this.bvlTemplate);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(panel2);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ManageColumnsDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.ManageColumnsDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tsAlign.ResumeLayout(false);
            this.tsAlign.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem item = e.Item;
            if (item.Checked)
            {
                item.Group = this.lvItems.Groups[0];
                item.Checked = true;
            }
            else
            {
                VirtualProperty property = VirtualProperty.Get(((ListViewColumnInfo) item.Tag).PropertyId);
                item.Group = this.GroupMap[property.GroupId];
                item.Checked = false;
            }
        }

        private void lvItems_ClientSizeChanged(object sender, EventArgs e)
        {
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new MethodInvoker(this.UpdateItemsListView));
            }
            else
            {
                this.UpdateItemsListView();
            }
        }

        private void lvItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (((ListViewColumnInfo) this.lvItems.Items[e.Index].Tag).PropertyId == 0)
            {
                e.NewValue = CheckState.Checked;
            }
        }

        private void lvItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            base.BeginInvoke(new ItemCheckedEventHandler(this.ItemChecked), new object[] { sender, e });
            this.cmbTemplate.ClearModified();
            this.UpdateButtons();
        }

        private void lvItems_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem item = (ListViewItem) e.Item;
            if (item.Group == this.lvItems.Groups[0])
            {
                this.lvItems.DoDragMove(item);
            }
        }

        private void lvItems_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (focusedItem != null)
            {
                switch (e.KeyData)
                {
                    case (Keys.Alt | Keys.Up):
                        focusedItem.MoveUpInGroup(true);
                        break;

                    case (Keys.Alt | Keys.Down):
                        focusedItem.MoveDownInGroup(true);
                        break;
                }
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(this.UpdateButtons));
        }

        private void ManageColumnsDialog_Shown(object sender, EventArgs e)
        {
            this.PopulateColumnItems();
            this.UpdateButtons();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.ApplicationExitCall:
                case CloseReason.WindowsShutDown:
                    break;

                default:
                    if (this.cmbTemplate.Modified)
                    {
                        KeyValueList<string, ListViewColumnInfo[]> list = new KeyValueList<string, ListViewColumnInfo[]>();
                        list.AddRange(this.cmbTemplate.GetItems<ListViewColumnInfo[]>());
                        Settings.Default.ColumnTemplates = (list.Count > 0) ? list : null;
                    }
                    break;
            }
            base.OnFormClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsAlign.Padding = new Padding((this.tsAlign.Width - ((this.tsbLeftAlign.Width + this.tsbCenter.Width) + this.tsbRightAlign.Width)) / 2, 0, 0, 0);
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            BasicDialog.UpdateBevel(this.bvlTemplate);
            BasicDialog.UpdateBevel(this.bvlButtons);
            base.OnThemeChanged(e);
        }

        private void PopulateColumnItems()
        {
            this.lvItems.ItemChecked -= new ItemCheckedEventHandler(this.lvItems_ItemChecked);
            this.lvItems.BeginUpdate();
            try
            {
                this.lvItems.Groups.Clear();
                this.lvItems.Items.Clear();
                this.GroupMap.Clear();
                this.lvItems.Groups.Add("grpVisible", Resources.sGroupVisible);
                foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) VirtualProperty.Visible)
                {
                    ListViewColumnInfo info;
                    ListViewGroup group;
                    ListViewItem item = new ListViewItem(property.LocalizedName);
                    if (!this.ColumnInfoMap.TryGetValue(property.PropertyId, out info))
                    {
                        info = new ListViewColumnInfo(property.PropertyId, VirtualFilePanelSettings.DefaultColumnWidth(property.PropertyId, this.lvItems.Font), false);
                    }
                    item.SubItems.Add(info.Width.ToString());
                    item.Checked = info.Visible || (property.PropertyId == 0);
                    item.Tag = info;
                    if (!this.GroupMap.TryGetValue(property.GroupId, out group))
                    {
                        group = new ListViewGroup(property.LocalizedGroupName);
                        this.lvItems.Groups.Add(group);
                        this.GroupMap.Add(property.GroupId, group);
                    }
                    item.Group = item.Checked ? this.lvItems.Groups[0] : group;
                    item.ImageIndex = (int) info.TextAlign;
                    if (!this.AvailableProperties[property.PropertyId])
                    {
                        item.ForeColor = SystemColors.GrayText;
                    }
                    this.lvItems.Items.Add(item);
                }
                this.lvItems.Sort(new ListViewColumnInfoComparer());
                this.UpdateItemsListView();
            }
            finally
            {
                this.lvItems.ItemChecked += new ItemCheckedEventHandler(this.lvItems_ItemChecked);
                this.lvItems.EndUpdate();
            }
        }

        private void tsbAlign_Click(object sender, EventArgs e)
        {
            int tag = (int) ((ToolStripButton) sender).Tag;
            if (tag != this.lvItems.FocusedItem.ImageIndex)
            {
                this.lvItems.FocusedItem.ImageIndex = tag;
                this.cmbTemplate.ClearModified();
                this.UpdateButtons();
            }
        }

        private void tsbAlign_Paint(object sender, PaintEventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (!((focusedItem == null) || focusedItem.Selected))
            {
                focusedItem = null;
            }
            ToolStripButton button = (ToolStripButton) sender;
            button.Enabled = focusedItem != null;
            button.Checked = button.Enabled && (focusedItem.ImageIndex == ((int) button.Tag));
        }

        protected void UpdateAcceptButton()
        {
            IButtonControl btnOk = this.btnOk;
            if ((!this.cmbTemplate.DroppedDown && this.btnSaveTemplate.Enabled) && this.cmbTemplate.Focused)
            {
                btnOk = this.btnSaveTemplate;
            }
            if (base.AcceptButton != btnOk)
            {
                base.AcceptButton = btnOk;
            }
        }

        protected void UpdateButtons()
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (!((focusedItem == null) || focusedItem.Selected))
            {
                focusedItem = null;
            }
            this.btnShow.Enabled = (focusedItem != null) && !focusedItem.Checked;
            this.btnHide.Enabled = ((focusedItem != null) && focusedItem.Checked) && (((ListViewColumnInfo) focusedItem.Tag).PropertyId != 0);
            this.tsbAlign_Paint(this.tsbLeftAlign, null);
            this.tsbAlign_Paint(this.tsbCenter, null);
            this.tsbAlign_Paint(this.tsbRightAlign, null);
            CanMoveListViewItem item2 = ((focusedItem != null) && (focusedItem.Group == this.lvItems.Groups[0])) ? focusedItem.CanMove() : ((CanMoveListViewItem) 0);
            this.btnUp.Enabled = (item2 & CanMoveListViewItem.UpInGroup) > 0;
            this.btnDown.Enabled = (item2 & CanMoveListViewItem.DownInGroup) > 0;
        }

        private void UpdateItemsListView()
        {
            this.lvItems.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.lvItems.Columns[0].Width = this.lvItems.ClientSize.Width - this.lvItems.Columns[1].Width;
        }

        public bool AutosizeColumns
        {
            get
            {
                return this.chkAutosizeColumns.Checked;
            }
            set
            {
                this.chkAutosizeColumns.Checked = value;
            }
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                return this.FAvailableProperties;
            }
            set
            {
                if ((this.FAvailableProperties != value) && ((this.FAvailableProperties == null) || !this.FAvailableProperties.Equals(value)))
                {
                    this.FAvailableProperties = value;
                    if (base.Visible)
                    {
                        this.PopulateColumnItems();
                    }
                }
            }
        }

        public IEnumerable<ListViewColumnInfo> Columns
        {
            get
            {
                List<ListViewColumnInfo> list = new List<ListViewColumnInfo>();
                int num = 0;
                foreach (ListViewItem item in this.lvItems.Items)
                {
                    ListViewColumnInfo tag = (ListViewColumnInfo) item.Tag;
                    if ((item.Checked || (item.ImageIndex != tag.TextAlign)) || this.ColumnInfoMap.ContainsKey(tag.PropertyId))
                    {
                        ListViewColumnInfo info2 = (ListViewColumnInfo) tag.Clone();
                        info2.Visible = item.Checked;
                        info2.TextAlign = (HorizontalAlignment) item.ImageIndex;
                        info2.DisplayIndex = info2.Visible ? num++ : -1;
                        list.Add(info2);
                    }
                }
                return ((list.Count > 0) ? list : null);
            }
            set
            {
                this.ColumnInfoMap.Clear();
                if (value != null)
                {
                    foreach (ListViewColumnInfo info in value)
                    {
                        this.ColumnInfoMap.Add(info.PropertyId, info);
                    }
                }
                if (base.Visible)
                {
                    this.PopulateColumnItems();
                }
            }
        }

        public bool RememberColumns
        {
            get
            {
                return (this.chkRememberColumns.Enabled & this.chkRememberColumns.Checked);
            }
        }

        public bool RememberColumnsEnabled
        {
            get
            {
                return this.chkRememberColumns.Visible;
            }
            set
            {
                this.chkRememberColumns.Enabled = value;
                this.chkRememberColumns.Visible = value;
            }
        }

        private class ListViewColumnInfoComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListViewColumnInfo tag = (ListViewColumnInfo) ((ListViewItem) x).Tag;
                ListViewColumnInfo info2 = (ListViewColumnInfo) ((ListViewItem) y).Tag;
                int num = 0;
                if (!(!tag.Visible || info2.Visible))
                {
                    num = -1;
                }
                else if (!(tag.Visible || !info2.Visible))
                {
                    num = 1;
                }
                if (num != 0)
                {
                    return num;
                }
                if (tag.Visible && info2.Visible)
                {
                    return (tag.DisplayIndex - info2.DisplayIndex);
                }
                return (tag.PropertyId - info2.PropertyId);
            }
        }
    }
}

