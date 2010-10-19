namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ManageListDialog : BasicDialog
    {
        protected Button btnCancel;
        private Button btnDelete;
        protected Button btnDown;
        protected Button btnOk;
        private Button btnSort;
        protected Button btnUp;
        private IContainer components = null;
        protected ListViewEx lvItems;
        private ColumnHeader NameColumn;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpButtons;

        public ManageListDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            this.lvItems.Columns[0].Width = this.lvItems.ClientSize.Width;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (focusedItem != null)
            {
                focusedItem.Delete(true);
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (focusedItem != null)
            {
                if (sender == this.btnUp)
                {
                    focusedItem.MoveUp(true);
                }
                else
                {
                    focusedItem.MoveDown(true);
                }
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            this.lvItems.Sorting = SortOrder.Ascending;
            this.lvItems.Sort();
            this.lvItems.Sorting = SortOrder.None;
            focusedItem.Focus(true, false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public T[] GetItems<T>() where T: class
        {
            T[] localArray = new T[this.lvItems.Items.Count];
            for (int i = 0; i < this.lvItems.Items.Count; i++)
            {
                localArray[i] = (T) this.lvItems.Items[i].Tag;
            }
            return localArray;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ManageListDialog));
            this.lvItems = new ListViewEx();
            this.NameColumn = new ColumnHeader();
            this.btnSort = new Button();
            this.btnDown = new Button();
            this.btnUp = new Button();
            this.btnDelete = new Button();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.tlpButtons = new TableLayoutPanel();
            TableLayoutPanel panel = new TableLayoutPanel();
            Bevel bevel = new Bevel();
            panel.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lvItems, 0, 0);
            panel.Controls.Add(this.btnSort, 1, 3);
            panel.Controls.Add(this.btnDown, 1, 1);
            panel.Controls.Add(this.btnUp, 1, 0);
            panel.Controls.Add(this.btnDelete, 1, 2);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            this.lvItems.AllowDrop = true;
            this.lvItems.Columns.AddRange(new ColumnHeader[] { this.NameColumn });
            manager.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.ExplorerTheme = true;
            this.lvItems.FullRowSelect = true;
            this.lvItems.HeaderStyle = ColumnHeaderStyle.None;
            this.lvItems.HideSelection = false;
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            panel.SetRowSpan(this.lvItems, 5);
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = View.Details;
            this.lvItems.ItemDrag += new ItemDragEventHandler(this.lvItems_ItemDrag);
            this.lvItems.SelectedIndexChanged += new EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvItems.ClientSizeChanged += new EventHandler(this.lvItems_ClientSizeChanged);
            this.lvItems.KeyDown += new KeyEventHandler(this.lvItems_KeyDown);
            manager.ApplyResources(this.btnSort, "btnSort");
            this.btnSort.Name = "btnSort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new EventHandler(this.btnSort_Click);
            manager.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Name = "btnDown";
            this.btnDown.Tag = "1";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnMove_Click);
            manager.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnMove_Click);
            manager.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            manager.ApplyResources(bevel, "bvlButtons");
            bevel.ForeColor = SystemColors.ControlDarkDark;
            bevel.Name = "bvlButtons";
            bevel.Sides = Border3DSide.Top;
            bevel.Style = Border3DStyle.Flat;
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
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel);
            base.Controls.Add(bevel);
            base.Controls.Add(this.tlpButtons);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ManageListDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.ManageListDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
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

        private void lvItems_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem item = (ListViewItem) e.Item;
            this.lvItems.DoDragMove(item);
        }

        private void lvItems_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (focusedItem != null)
            {
                switch (e.KeyData)
                {
                    case (Keys.Alt | Keys.Up):
                        focusedItem.MoveUp(true);
                        break;

                    case (Keys.Alt | Keys.Down):
                        focusedItem.MoveDown(true);
                        break;

                    case Keys.Delete:
                        focusedItem.Delete(true);
                        break;
                }
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(this.UpdateButtons));
        }

        private void ManageListDialog_Shown(object sender, EventArgs e)
        {
            this.UpdateButtons();
        }

        protected void UpdateButtons()
        {
            ListViewItem focusedItem = this.lvItems.FocusedItem;
            if (!((focusedItem == null) || focusedItem.Selected))
            {
                focusedItem = null;
            }
            CanMoveListViewItem item2 = (focusedItem != null) ? focusedItem.CanMove() : ((CanMoveListViewItem) 0);
            this.btnUp.Enabled = (item2 & CanMoveListViewItem.Up) > 0;
            this.btnDown.Enabled = (item2 & CanMoveListViewItem.Down) > 0;
            this.btnDelete.Enabled = focusedItem != null;
            this.btnSort.Enabled = this.lvItems.Items.Count > 1;
        }

        private void UpdateItemsListView()
        {
            this.lvItems.Columns[0].Width = this.lvItems.ClientSize.Width;
        }

        public object[] Items
        {
            get
            {
                return this.GetItems<object>();
            }
            set
            {
                this.lvItems.BeginUpdate();
                try
                {
                    this.lvItems.Items.Clear();
                    if (value != null)
                    {
                        foreach (object obj2 in value)
                        {
                            ListViewItem item = new ListViewItem(obj2.ToString()) {
                                Tag = obj2
                            };
                            this.lvItems.Items.Add(item);
                        }
                    }
                }
                finally
                {
                    this.lvItems.EndUpdate();
                }
            }
        }
    }
}

