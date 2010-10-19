namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Commons.Drawing;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Actions;
    using Nomad.Dialogs;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ToolbarOptionControl : UserControl, IPersistComponentSettings
    {
        private ListViewItem AllBookmarksItem;
        private ListViewItem AllDrivesItem;
        private ListViewItem AllToolsItem;
        private Button btnAdd;
        private Button btnDisplayAsImage;
        private Button btnDisplayAsImageAndText;
        private Button btnDisplayAsText;
        private Button btnDown;
        private Button btnRemove;
        private Button btnReset;
        private Button btnUp;
        private ComboBox cmbCategory;
        private ComboBox cmbToolbar;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private Dictionary<string, ListViewItem> CommandMap = new Dictionary<string, ListViewItem>(StringComparer.OrdinalIgnoreCase);
        private IContainer components = null;
        private string FCurrentToolbar;
        private ImageList imgCommand;
        private bool Initialized;
        private ListViewEx lvCommands;
        private ListViewEx lvToolbarItems;
        private ToolbarSettingsInfo PreviousToolbarInfo;
        private ListViewItem SeparatorItem;
        private TableLayoutPanel tlpBack;

        public ToolbarOptionControl()
        {
            this.InitializeComponent();
            this.imgCommand.ImageSize = ImageHelper.DefaultSmallIconSize;
            if (this.lvCommands.ExplorerTheme)
            {
                this.imgCommand.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
                Bitmap image = new Bitmap(10, 1);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.Clear(this.lvCommands.BackColor);
                }
                this.lvCommands.BackgroundImage = image;
                this.lvCommands.BackgroundImageTiled = true;
                this.lvToolbarItems.BackgroundImage = image;
                this.lvToolbarItems.BackgroundImageTiled = true;
            }
            this.imgCommand.AddAspected(Resources.ButtonMarkSplit);
            this.imgCommand.AddAspected(Resources.ButtonMarkDropDown);
            this.imgCommand.AddAspected(IconSet.GetImage("Button_Text"));
            this.imgCommand.AddAspected(IconSet.GetImage("Button_Image"));
            this.imgCommand.AddAspected(IconSet.GetImage("Button_ImageAndText"));
            this.btnDisplayAsText.Image = IconSet.GetImage("Button_Text");
            this.btnDisplayAsImage.Image = IconSet.GetImage("Button_Image");
            this.btnDisplayAsImageAndText.Image = IconSet.GetImage("Button_ImageAndText");
            if (!Application.RenderWithVisualStyles)
            {
                this.btnAdd.BackColor = SystemColors.Control;
                this.btnUp.BackColor = SystemColors.Control;
                this.btnDown.BackColor = SystemColors.Control;
                this.btnDisplayAsText.BackColor = SystemColors.Control;
                this.btnDisplayAsImage.BackColor = SystemColors.Control;
                this.btnDisplayAsImageAndText.BackColor = SystemColors.Control;
            }
            this.btnDisplayAsText.Tag = ToolStripItemDisplayStyle.Text;
            this.btnDisplayAsImage.Tag = ToolStripItemDisplayStyle.Image;
            this.btnDisplayAsImageAndText.Tag = ToolStripItemDisplayStyle.ImageAndText;
            this.SeparatorItem = new ListViewItem(Resources.sToolbarSeparator);
            this.SeparatorItem.Tag = "-";
            this.SeparatorItem.SubItems.Add(new ListViewItem.ListViewSubItem());
            this.AllBookmarksItem = new ListViewItem(Resources.sToolbarAllBookmarks);
            this.AllBookmarksItem.Tag = "bookmarks";
            this.AllBookmarksItem.SubItems.Add(new ListViewItem.ListViewSubItem());
            this.AllToolsItem = new ListViewItem(Resources.sToolbarAllTools);
            this.AllToolsItem.Tag = "tools";
            this.AllToolsItem.SubItems.Add(new ListViewItem.ListViewSubItem());
            this.AllDrivesItem = new ListViewItem(Resources.sToolbarDrives);
            this.AllDrivesItem.Tag = "drives";
            this.AllDrivesItem.SubItems.Add(new ListViewItem.ListViewSubItem());
        }

        private void AddComponentItem(List<ListViewItem> componentList, ListViewItem item)
        {
            if (item.Tag != null)
            {
                item.Text = item.Text.Replace("&", "");
                componentList.Add(item);
                this.CommandMap.Add((string) item.Tag, item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvCommands.FocusedItem;
            if (focusedItem != null)
            {
                ListViewItem item = (ListViewItem) focusedItem.Clone();
                if (!this.IsSpecialItem(item))
                {
                    item.StateImageIndex = (item.ImageIndex >= 0) ? 2 : 1;
                }
                if (this.lvToolbarItems.FocusedItem != null)
                {
                    this.lvToolbarItems.Items.Insert(this.lvToolbarItems.FocusedItem.Index + 1, item);
                }
                else
                {
                    this.lvToolbarItems.Items.Add(item);
                }
                item.Focus(true, true);
                int num = focusedItem.Index + 1;
                if (num < this.lvCommands.Items.Count)
                {
                    this.lvCommands.Items[num].Focus(true, true);
                }
            }
        }

        private void btnDisplayAs_Click(object sender, EventArgs e)
        {
            if (this.lvToolbarItems.FocusedItem != null)
            {
                ToolStripItemDisplayStyle tag = (ToolStripItemDisplayStyle) ((Control) sender).Tag;
                if (this.lvToolbarItems.FocusedItem.StateImageIndex != tag)
                {
                    this.lvToolbarItems.FocusedItem.StateImageIndex = (int) tag;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvToolbarItems.FocusedItem;
            if (focusedItem != null)
            {
                focusedItem.Delete(true);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show(this, Resources.sAskResetToolbar, Resources.sConfirmResetToolbar, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) == MessageDialogResult.Yes)
            {
                this.PreviousToolbarInfo = null;
                ToolbarSettingsInfo selectedItem = (ToolbarSettingsInfo) this.cmbToolbar.SelectedItem;
                selectedItem.Items = this.InitializeToolbarItems(selectedItem.Settings.DefaultCommands);
                this.cmbToolbar_SelectedIndexChanged(this.cmbToolbar, EventArgs.Empty);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvToolbarItems.FocusedItem;
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

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lvCommands.BeginUpdate();
            try
            {
                this.lvCommands.Items.Clear();
                this.lvCommands.Items.Add(this.SeparatorItem);
                KeyValuePair<string, ListViewItem[]> selectedItem = (KeyValuePair<string, ListViewItem[]>) this.cmbCategory.SelectedItem;
                foreach (ListViewItem item in selectedItem.Value)
                {
                    this.lvCommands.Items.Add(item);
                }
            }
            finally
            {
                this.lvCommands.EndUpdate();
            }
        }

        private void cmbToolbar_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolbarSettingsInfo selectedItem = (ToolbarSettingsInfo) this.cmbToolbar.SelectedItem;
            if (selectedItem != this.PreviousToolbarInfo)
            {
                if (this.PreviousToolbarInfo != null)
                {
                    List<ListViewItem> list = new List<ListViewItem>(this.lvToolbarItems.Items.Count);
                    foreach (ListViewItem item in this.lvToolbarItems.Items)
                    {
                        list.Add(item);
                    }
                    this.PreviousToolbarInfo.Items = list;
                }
                this.lvToolbarItems.BeginUpdate();
                this.lvToolbarItems.Items.Clear();
                foreach (ListViewItem item in selectedItem.Items)
                {
                    this.lvToolbarItems.Items.Add(item);
                }
                this.lvToolbarItems.EndUpdate();
                this.PreviousToolbarInfo = selectedItem;
                this.UpdateButtons();
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

        private string GetToolbarString(IEnumerable<ListViewItem> items)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ListViewItem item in items)
            {
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }
                ToolStripItemDisplayStyle image = ToolStripItemDisplayStyle.Image;
                if (item.StateImageIndex > 0)
                {
                    image = (ToolStripItemDisplayStyle) item.StateImageIndex;
                }
                builder.Append(ToolbarSettings.CreateToolbarButtonLine((string) item.Tag, image, (IconLocation) item.SubItems[0].Tag));
            }
            return builder.ToString();
        }

        private void InitializeCommands()
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (Category category in MainForm.Instance.categoryManager.CategoryList)
            {
                string name;
                List<ListViewItem> componentList = new List<ListViewItem>(category.Components.Count);
                foreach (IComponent component in category.Components)
                {
                    ListViewItem item = new ListViewItem();
                    ListViewItem.ListViewSubItem item2 = new ListViewItem.ListViewSubItem();
                    Image img = null;
                    Action action = component as Action;
                    if (action != null)
                    {
                        item.Text = action.Text;
                        item.ToolTipText = category.Text + " | " + action.Name;
                        item.Tag = action.Name;
                        img = action.Image;
                        name = action.Name;
                        switch (name)
                        {
                            case "actBack":
                            case "actForward":
                            case "actFind":
                            case "actSaveCurrentLayout":
                            case "actChangeView":
                            case "actAdvancedFilter":
                            case "actSelectSort":
                            case "actBookmarkCurrentFolder":
                            case "actNewFile":
                                item2.Tag = 0;
                                break;

                            case "actShowBookmarks":
                                item2.Tag = 1;
                                break;
                        }
                    }
                    ToolStripMenuItem item3 = component as ToolStripMenuItem;
                    if ((item3 != null) && (item3.DropDownItems.Count > 0))
                    {
                        item.Text = item3.Text;
                        item.ToolTipText = category.Text + " | " + item3.Name;
                        item.Tag = item3.Name;
                        img = item3.Image;
                        item2.Tag = 1;
                    }
                    if (img != null)
                    {
                        item.ImageIndex = this.imgCommand.AddNormalized(img, this.lvCommands.BackColor);
                    }
                    item.SubItems.Add(item2);
                    this.AddComponentItem(componentList, item);
                }
                name = category.Name;
                if (name != null)
                {
                    if (!(name == "catPanel"))
                    {
                        if (name == "catBookmarks")
                        {
                            goto Label_0316;
                        }
                        if (name == "catMisc")
                        {
                            goto Label_0329;
                        }
                    }
                    else
                    {
                        this.AddComponentItem(componentList, this.AllDrivesItem);
                    }
                }
                goto Label_041E;
            Label_0316:
                this.AddComponentItem(componentList, this.AllBookmarksItem);
                goto Label_041E;
            Label_0329:
                if (Directory.Exists(SettingsManager.SpecialFolders.Tools))
                {
                    foreach (string str in Directory.GetFiles(SettingsManager.SpecialFolders.Tools, "*.lnk"))
                    {
                        ListViewItem item4 = new ListViewItem {
                            Text = string.Format(Resources.sToolbarToolName, Path.GetFileNameWithoutExtension(str)),
                            ToolTipText = category.Text + " | " + Path.GetFileNameWithoutExtension(str),
                            Tag = @"tools\" + Path.GetFileName(str)
                        };
                        Image fileIcon = ImageProvider.Default.GetFileIcon(str, ImageHelper.DefaultSmallIconSize);
                        item4.ImageIndex = this.imgCommand.AddNormalized(fileIcon, this.lvCommands.BackColor);
                        this.AddComponentItem(componentList, item4);
                    }
                }
                this.AddComponentItem(componentList, this.AllToolsItem);
            Label_041E:
                this.cmbCategory.Items.Add(new KeyValuePair<string, ListViewItem[]>(category.Text, componentList.ToArray()));
                list.AddRange(componentList);
            }
            this.cmbCategory.Items.Add(new KeyValuePair<string, ListViewItem[]>(Resources.sAll, list.ToArray()));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ToolbarOptionControl));
            this.cmbCategory = new ComboBox();
            this.lvCommands = new ListViewEx();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.imgCommand = new ImageList(this.components);
            this.lvToolbarItems = new ListViewEx();
            this.columnHeader3 = new ColumnHeader();
            this.columnHeader4 = new ColumnHeader();
            this.cmbToolbar = new ComboBox();
            this.tlpBack = new TableLayoutPanel();
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.btnUp = new Button();
            this.btnDown = new Button();
            this.btnReset = new Button();
            this.btnDisplayAsImageAndText = new Button();
            this.btnDisplayAsImage = new Button();
            this.btnDisplayAsText = new Button();
            this.tlpBack.SuspendLayout();
            base.SuspendLayout();
            this.cmbCategory.DisplayMember = "Key";
            manager.ApplyResources(this.cmbCategory, "cmbCategory");
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.SelectedIndexChanged += new EventHandler(this.cmbCategory_SelectedIndexChanged);
            this.lvCommands.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.lvCommands.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvCommands, "lvCommands");
            this.lvCommands.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvCommands.FullRowSelect = true;
            this.lvCommands.HeaderStyle = ColumnHeaderStyle.None;
            this.lvCommands.HideSelection = false;
            this.lvCommands.MultiSelect = false;
            this.lvCommands.Name = "lvCommands";
            this.lvCommands.OwnerDraw = true;
            this.tlpBack.SetRowSpan(this.lvCommands, 8);
            this.lvCommands.ShowItemToolTips = true;
            this.lvCommands.SmallImageList = this.imgCommand;
            this.lvCommands.Sorting = SortOrder.Ascending;
            this.lvCommands.UseCompatibleStateImageBehavior = false;
            this.lvCommands.View = View.Details;
            this.lvCommands.KeyDown += new KeyEventHandler(this.lvCommands_KeyDown);
            this.lvCommands.MouseDoubleClick += new MouseEventHandler(this.lvCommands_MouseDoubleClick);
            this.lvCommands.SizeChanged += new EventHandler(this.ListView_SizeChanged);
            this.lvCommands.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged);
            this.lvCommands.PostDrawSubItem += new EventHandler<PostDrawListViewSubItemEventArgs>(this.lvCommands_PostDrawSubItem);
            manager.ApplyResources(this.columnHeader2, "columnHeader2");
            this.imgCommand.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgCommand, "imgCommand");
            this.imgCommand.TransparentColor = Color.Transparent;
            this.lvToolbarItems.AllowDrop = true;
            this.lvToolbarItems.Columns.AddRange(new ColumnHeader[] { this.columnHeader3, this.columnHeader4 });
            this.lvToolbarItems.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvToolbarItems, "lvToolbarItems");
            this.lvToolbarItems.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvToolbarItems.FullRowSelect = true;
            this.lvToolbarItems.HeaderStyle = ColumnHeaderStyle.None;
            this.lvToolbarItems.HideSelection = false;
            this.lvToolbarItems.MultiSelect = false;
            this.lvToolbarItems.Name = "lvToolbarItems";
            this.lvToolbarItems.OwnerDraw = true;
            this.tlpBack.SetRowSpan(this.lvToolbarItems, 8);
            this.lvToolbarItems.ShowItemToolTips = true;
            this.lvToolbarItems.SmallImageList = this.imgCommand;
            this.lvToolbarItems.UseCompatibleStateImageBehavior = false;
            this.lvToolbarItems.View = View.Details;
            this.lvToolbarItems.KeyDown += new KeyEventHandler(this.lvToolbarItems_KeyDown);
            this.lvToolbarItems.MouseDoubleClick += new MouseEventHandler(this.lvToolbarItems_MouseDoubleClick);
            this.lvToolbarItems.SizeChanged += new EventHandler(this.ListView_SizeChanged);
            this.lvToolbarItems.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged);
            this.lvToolbarItems.ItemDrag += new ItemDragEventHandler(this.lvToolbarItems_ItemDrag);
            this.lvToolbarItems.PostDrawSubItem += new EventHandler<PostDrawListViewSubItemEventArgs>(this.lvToolbarItems_PostDrawSubItem);
            manager.ApplyResources(this.columnHeader4, "columnHeader4");
            this.cmbToolbar.DisplayMember = "Caption";
            manager.ApplyResources(this.cmbToolbar, "cmbToolbar");
            this.cmbToolbar.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbToolbar.FormattingEnabled = true;
            this.cmbToolbar.Name = "cmbToolbar";
            this.cmbToolbar.ValueMember = "Items";
            this.cmbToolbar.SelectedIndexChanged += new EventHandler(this.cmbToolbar_SelectedIndexChanged);
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.cmbCategory, 0, 0);
            this.tlpBack.Controls.Add(this.lvCommands, 0, 1);
            this.tlpBack.Controls.Add(this.lvToolbarItems, 2, 1);
            this.tlpBack.Controls.Add(this.btnAdd, 1, 2);
            this.tlpBack.Controls.Add(this.btnRemove, 1, 3);
            this.tlpBack.Controls.Add(this.btnUp, 3, 2);
            this.tlpBack.Controls.Add(this.btnDown, 3, 3);
            this.tlpBack.Controls.Add(this.btnReset, 1, 4);
            this.tlpBack.Controls.Add(this.btnDisplayAsImageAndText, 3, 8);
            this.tlpBack.Controls.Add(this.btnDisplayAsImage, 3, 7);
            this.tlpBack.Controls.Add(this.btnDisplayAsText, 3, 6);
            this.tlpBack.Controls.Add(this.cmbToolbar, 2, 0);
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Image = Resources.SmallRightArrow;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            manager.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            manager.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Image = Resources.SmallUpArrow;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnUp_Click);
            manager.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Image = Resources.SmallDownArrow;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnUp_Click);
            manager.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new EventHandler(this.btnReset_Click);
            manager.ApplyResources(this.btnDisplayAsImageAndText, "btnDisplayAsImageAndText");
            this.btnDisplayAsImageAndText.Name = "btnDisplayAsImageAndText";
            this.btnDisplayAsImageAndText.UseVisualStyleBackColor = true;
            this.btnDisplayAsImageAndText.Click += new EventHandler(this.btnDisplayAs_Click);
            manager.ApplyResources(this.btnDisplayAsImage, "btnDisplayAsImage");
            this.btnDisplayAsImage.Name = "btnDisplayAsImage";
            this.btnDisplayAsImage.UseVisualStyleBackColor = true;
            this.btnDisplayAsImage.Click += new EventHandler(this.btnDisplayAs_Click);
            manager.ApplyResources(this.btnDisplayAsText, "btnDisplayAsText");
            this.btnDisplayAsText.Name = "btnDisplayAsText";
            this.btnDisplayAsText.UseVisualStyleBackColor = true;
            this.btnDisplayAsText.Click += new EventHandler(this.btnDisplayAs_Click);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tlpBack);
            base.Name = "ToolbarOptionControl";
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            base.ResumeLayout(false);
        }

        private IEnumerable<ListViewItem> InitializeToolbarItems(string actions)
        {
            return new <InitializeToolbarItems>d__0(-2) { <>4__this = this, <>3__actions = actions };
        }

        private bool IsSpecialItem(ListViewItem item)
        {
            return item.Text.StartsWith("<", StringComparison.OrdinalIgnoreCase);
        }

        private void ListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Focused)
            {
                e.Item.Selected = true;
            }
            base.BeginInvoke(new MethodInvoker(this.UpdateButtons));
        }

        private void ListView_SizeChanged(ListView listView)
        {
            int width = listView.ClientSize.Width;
            if (listView.Columns.Count > 1)
            {
                for (int i = 1; i < listView.Columns.Count; i++)
                {
                    width -= listView.Columns[i].Width;
                }
            }
            listView.Columns[0].Width = width;
        }

        private void ListView_SizeChanged(object sender, EventArgs e)
        {
            ListView listView = (ListView) sender;
            if (listView.Columns.Count >= 1)
            {
                if (base.IsHandleCreated)
                {
                    base.BeginInvoke(new Action<ListView>(this.ListView_SizeChanged), new object[] { listView });
                }
                else
                {
                    this.ListView_SizeChanged(listView);
                }
            }
        }

        public void LoadComponentSettings()
        {
            if (!this.Initialized)
            {
                this.InitializeCommands();
                this.Initialized = true;
                this.cmbCategory.SelectedIndex = 0;
            }
            List<ToolbarSettingsInfo> list = new List<ToolbarSettingsInfo>();
            foreach (ToolbarSettings settings in ToolbarSettings.Toolbars)
            {
                ToolbarSettingsInfo info;
                info = new ToolbarSettingsInfo {
                    Settings = settings,
                    Items = this.InitializeToolbarItems(info.Settings.Commands),
                    Caption = Resources.ResourceManager.GetString(settings.Caption)
                };
                if (string.IsNullOrEmpty(info.Caption))
                {
                    info.Caption = settings.Caption;
                }
                list.Add(info);
            }
            this.cmbToolbar.DataSource = list;
            if (!string.IsNullOrEmpty(this.FCurrentToolbar))
            {
                this.CurrentToolbar = this.FCurrentToolbar;
            }
        }

        private void lvCommands_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Space:
                    this.SeparatorItem.Focus(true, true);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case (Keys.Control | Keys.Add):
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void lvCommands_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnAdd.PerformClick();
        }

        private void lvCommands_PostDrawSubItem(object sender, PostDrawListViewSubItemEventArgs e)
        {
            if ((e.ColumnIndex != 0) && (e.SubItem.Tag != null))
            {
                e.Graphics.DrawImage(this.imgCommand.Images[(int) e.SubItem.Tag], e.Bounds.X, e.Bounds.Y + 1);
            }
        }

        private void lvToolbarItems_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.lvToolbarItems.DoDragMove((ListViewItem) e.Item);
        }

        private void lvToolbarItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Add))
            {
                e.SuppressKeyPress = true;
            }
            else
            {
                ListViewItem focusedItem = this.lvToolbarItems.FocusedItem;
                if (focusedItem != null)
                {
                    switch (e.KeyData)
                    {
                        case (Keys.Control | Keys.Up):
                            focusedItem.MoveUp(true);
                            e.Handled = true;
                            return;

                        case (Keys.Control | Keys.Right):
                            return;

                        case (Keys.Control | Keys.Down):
                            focusedItem.MoveDown(true);
                            e.Handled = true;
                            return;

                        case Keys.Delete:
                            focusedItem.Delete(true);
                            e.Handled = true;
                            return;
                    }
                }
            }
        }

        private void lvToolbarItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnRemove.PerformClick();
        }

        private void lvToolbarItems_PostDrawSubItem(object sender, PostDrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                ToolStripItemDisplayStyle stateImageIndex = (ToolStripItemDisplayStyle) e.Item.StateImageIndex;
                if (stateImageIndex > ToolStripItemDisplayStyle.None)
                {
                    e.Graphics.DrawImage(this.imgCommand.Images[((int) stateImageIndex) + 1], e.Bounds.X, e.Bounds.Y);
                }
                if (e.SubItem.Tag != null)
                {
                    e.Graphics.DrawImage(this.imgCommand.Images[(int) e.SubItem.Tag], (int) (e.Bounds.X + this.imgCommand.ImageSize.Width), (int) (e.Bounds.Y + 1));
                }
            }
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            foreach (ToolbarSettingsInfo info in this.cmbToolbar.Items)
            {
                if (info == this.cmbToolbar.SelectedItem)
                {
                    List<ListViewItem> list = new List<ListViewItem>(this.lvToolbarItems.Items.Count);
                    foreach (ListViewItem item in this.lvToolbarItems.Items)
                    {
                        list.Add(item);
                    }
                    info.Items = list;
                }
                string toolbarString = this.GetToolbarString(info.Items);
                if (!string.Equals(info.Settings.Commands, toolbarString, StringComparison.OrdinalIgnoreCase))
                {
                    info.Settings.Commands = toolbarString;
                    SettingsManager.RegisterSettings(info.Settings);
                }
            }
        }

        private void UpdateButtons()
        {
            this.btnAdd.Enabled = this.lvCommands.FocusedItem != null;
            this.btnRemove.Enabled = this.lvToolbarItems.FocusedItem != null;
            this.btnUp.Enabled = (this.lvToolbarItems.FocusedItem != null) && (this.lvToolbarItems.FocusedItem.Index > 0);
            this.btnDown.Enabled = (this.lvToolbarItems.FocusedItem != null) && (this.lvToolbarItems.FocusedItem.Index < (this.lvToolbarItems.Items.Count - 1));
            this.btnDisplayAsText.Enabled = (this.lvToolbarItems.FocusedItem != null) && !this.IsSpecialItem(this.lvToolbarItems.FocusedItem);
            this.btnDisplayAsImage.Enabled = this.btnDisplayAsText.Enabled && (this.lvToolbarItems.FocusedItem.ImageIndex >= 0);
            this.btnDisplayAsImageAndText.Enabled = this.btnDisplayAsImage.Enabled;
            this.btnReset.Enabled = this.cmbToolbar.SelectedIndex == 0;
        }

        public string CurrentToolbar
        {
            get
            {
                ToolbarSettingsInfo selectedItem = (ToolbarSettingsInfo) this.cmbToolbar.SelectedItem;
                if (selectedItem != null)
                {
                    return selectedItem.Settings.SettingsKey;
                }
                if (this.cmbToolbar.Items.Count == 0)
                {
                    return this.FCurrentToolbar;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.cmbToolbar.Items.Count; i++)
                {
                    if (((ToolbarSettingsInfo) this.cmbToolbar.Items[i]).Settings.SettingsKey == value)
                    {
                        this.cmbToolbar.SelectedIndex = i;
                        this.FCurrentToolbar = null;
                        return;
                    }
                }
                this.FCurrentToolbar = value;
            }
        }

        public bool SaveSettings
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public string SettingsKey
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        [CompilerGenerated]
        private sealed class <InitializeToolbarItems>d__0 : IEnumerable<ListViewItem>, IEnumerable, IEnumerator<ListViewItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ListViewItem <>2__current;
            public string <>3__actions;
            public ToolbarOptionControl <>4__this;
            private int <>l__initialThreadId;
            public string <Command>5__3;
            public ListViewItem <CommandItem>5__6;
            public ToolStripItemDisplayStyle <DisplayStyle>5__4;
            public IconLocation <ImageLocation>5__5;
            public string <Line>5__2;
            public TextReader <Reader>5__1;
            public ListViewItem <ToolbarItem>5__7;
            public string actions;

            [DebuggerHidden]
            public <InitializeToolbarItems>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally8()
            {
                this.<>1__state = -1;
                if (this.<Reader>5__1 != null)
                {
                    this.<Reader>5__1.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            goto Label_00AB;

                        case 3:
                            goto Label_0215;

                        default:
                            goto Label_024B;
                    }
                    this.<>1__state = -1;
                    this.<Reader>5__1 = new StringReader(this.actions);
                    this.<>1__state = 1;
                    while ((this.<Line>5__2 = this.<Reader>5__1.ReadLine()) != null)
                    {
                        string str = this.<Line>5__2;
                        switch (str)
                        {
                            case null:
                                goto Label_00B7;

                            case "":
                            {
                                continue;
                            }
                            default:
                                if (!(str == "-"))
                                {
                                    goto Label_00B7;
                                }
                                this.<>2__current = (ListViewItem) this.<>4__this.SeparatorItem.Clone();
                                this.<>1__state = 2;
                                return true;
                        }
                    Label_00AB:
                        this.<>1__state = 1;
                        continue;
                    Label_00B7:
                        this.<DisplayStyle>5__4 = ToolStripItemDisplayStyle.Image;
                        if (!ToolbarSettings.ParseToolbarButtonLine(this.<Line>5__2, out this.<Command>5__3, ref this.<DisplayStyle>5__4, out this.<ImageLocation>5__5) || !((this.<Command>5__3 != null) && this.<>4__this.CommandMap.TryGetValue(this.<Command>5__3, out this.<CommandItem>5__6)))
                        {
                            continue;
                        }
                        this.<ToolbarItem>5__7 = (ListViewItem) this.<CommandItem>5__6.Clone();
                        if (this.<ImageLocation>5__5 != null)
                        {
                            Image image;
                            if (Path.IsPathRooted(this.<ImageLocation>5__5.IconFileName))
                            {
                                image = CustomImageProvider.LoadIconFromLocation(this.<ImageLocation>5__5, ImageHelper.DefaultSmallIconSize);
                            }
                            else
                            {
                                image = IconSet.GetImage(this.<ImageLocation>5__5.IconFileName);
                            }
                            if (image != null)
                            {
                                this.<ToolbarItem>5__7.ImageIndex = this.<>4__this.imgCommand.AddNormalized(image, this.<>4__this.lvToolbarItems.BackColor);
                                this.<ToolbarItem>5__7.SubItems[0].Tag = this.<ImageLocation>5__5;
                            }
                        }
                        if (!this.<>4__this.IsSpecialItem(this.<ToolbarItem>5__7))
                        {
                            this.<ToolbarItem>5__7.StateImageIndex = (int) this.<DisplayStyle>5__4;
                        }
                        this.<>2__current = this.<ToolbarItem>5__7;
                        this.<>1__state = 3;
                        return true;
                    Label_0215:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally8();
                Label_024B:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<ListViewItem> IEnumerable<ListViewItem>.GetEnumerator()
            {
                ToolbarOptionControl.<InitializeToolbarItems>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ToolbarOptionControl.<InitializeToolbarItems>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.actions = this.<>3__actions;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ListViewItem>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally8();
                        }
                        break;
                }
            }

            ListViewItem IEnumerator<ListViewItem>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        private class ToolbarSettingsInfo
        {
            public string Caption { get; set; }

            public IEnumerable<ListViewItem> Items { get; set; }

            public ToolbarSettings Settings { get; set; }
        }
    }
}

