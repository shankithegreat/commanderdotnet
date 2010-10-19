namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.Controls.Actions;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class KeyboardMapOptionControl : UserControl, IPersistComponentSettings
    {
        private Button btnAssign;
        private Button btnRemove;
        private ComboBox cmbCommandShortcut;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private IContainer components = null;
        private ImageList imgCommand;
        private IDictionary<Keys, IComponent> KeyMap;
        private Label lblCategories;
        private Label lblCommands;
        private Label lblCommandShortcut;
        private Label lblShortcut;
        private Label lblShortcutUsedBy;
        private ListBox lstCategory;
        private ListViewEx lvCommands;
        private bool Modified;
        private TableLayoutPanel tlpBack;
        private HotKeyBox txtShortcut;
        private TextBox txtShortcutUsedBy;

        public KeyboardMapOptionControl()
        {
            this.InitializeComponent();
            if (this.lvCommands.ExplorerTheme)
            {
                this.imgCommand.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
            }
            else
            {
                this.imgCommand.ImageSize = ImageHelper.DefaultSmallIconSize;
            }
            this.lblCategories.BackColor = Theme.Current.ThemeColors.OptionBlockLabelBackground;
            this.lblCategories.ForeColor = Theme.Current.ThemeColors.OptionBlockLabelText;
            this.lblCommands.BackColor = this.lblCategories.BackColor;
            this.lblCommands.ForeColor = this.lblCategories.ForeColor;
            if (!Application.RenderWithVisualStyles)
            {
                this.btnRemove.FlatStyle = FlatStyle.System;
                this.btnAssign.FlatStyle = FlatStyle.System;
            }
            this.txtShortcut.KeysConverter = new SettingsManager.LocalizedEnumConverter(typeof(Keys));
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            Keys hotKey = this.txtShortcut.HotKey;
            this.KeyMap.Remove(hotKey);
            this.KeyMap.Add(hotKey, (IComponent) this.lvCommands.FocusedItem.Tag);
            this.cmbCommandShortcut.Items.Add(hotKey);
            this.txtShortcut.HotKey = Keys.None;
            this.UpdateButtons(true);
            this.Modified = true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.KeyMap.Remove((Keys) this.cmbCommandShortcut.SelectedItem);
            this.cmbCommandShortcut.Items.RemoveAt(this.cmbCommandShortcut.SelectedIndex);
            this.UpdateShortcutUsedBy();
            this.UpdateButtons(true);
            this.Modified = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static string GetCommandName(IComponent command)
        {
            Action action = command as Action;
            if (action != null)
            {
                return action.Name;
            }
            ToolStripMenuItem item = command as ToolStripMenuItem;
            if (item == null)
            {
                throw new ArgumentException(string.Format("Invalid command type ({0}).", command.GetType()));
            }
            return item.Name;
        }

        private void InitializeCommands()
        {
            TypeConverter keysConverter = TypeDescriptor.GetConverter(typeof(Keys));
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (Category category in MainForm.Instance.categoryManager.CategoryList)
            {
                List<ListViewItem> collection = new List<ListViewItem>(category.Components.Count);
                foreach (IComponent component in category.Components)
                {
                    ListViewItem item = new ListViewItem();
                    Image img = null;
                    Keys none = Keys.None;
                    Action action = component as Action;
                    if (action != null)
                    {
                        item.Text = action.Text;
                        item.ToolTipText = category.Text + " | " + action.Name;
                        none = action.ShortcutKeys;
                        img = action.Image;
                    }
                    ToolStripMenuItem item2 = component as ToolStripMenuItem;
                    if (item2 != null)
                    {
                        if (item2.DropDownItems.Count > 0)
                        {
                            continue;
                        }
                        item.Text = item2.Text;
                        item.ToolTipText = category.Text + " | " + item2.Name;
                        none = item2.ShortcutKeys;
                        img = item2.Image;
                    }
                    if (img != null)
                    {
                        item.ImageIndex = this.imgCommand.AddNormalized(img, this.lvCommands.BackColor);
                    }
                    item.Text = item.Text.Replace("&", "");
                    item.Tag = component;
                    this.UpdateCommandShortcut(item, none, keysConverter);
                    collection.Add(item);
                }
                this.lstCategory.Items.Add(new KeyValuePair<string, ListViewItem[]>(category.Text, collection.ToArray()));
                list.AddRange(collection);
            }
            this.lstCategory.Items.Add(new KeyValuePair<string, ListViewItem[]>(Resources.sAll, list.ToArray()));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(KeyboardMapOptionControl));
            this.lstCategory = new ListBox();
            this.cmbCommandShortcut = new ComboBox();
            this.lvCommands = new ListViewEx();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.imgCommand = new ImageList(this.components);
            this.tlpBack = new TableLayoutPanel();
            this.lblCategories = new Label();
            this.lblCommands = new Label();
            this.lblCommandShortcut = new Label();
            this.btnRemove = new Button();
            this.lblShortcut = new Label();
            this.btnAssign = new Button();
            this.lblShortcutUsedBy = new Label();
            this.txtShortcutUsedBy = new TextBox();
            this.txtShortcut = new HotKeyBox();
            this.tlpBack.SuspendLayout();
            base.SuspendLayout();
            this.lstCategory.DisplayMember = "Key";
            manager.ApplyResources(this.lstCategory, "lstCategory");
            this.lstCategory.Name = "lstCategory";
            this.lstCategory.SelectedIndexChanged += new EventHandler(this.lstCategory_SelectedIndexChanged);
            this.tlpBack.SetColumnSpan(this.cmbCommandShortcut, 2);
            manager.ApplyResources(this.cmbCommandShortcut, "cmbCommandShortcut");
            this.cmbCommandShortcut.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCommandShortcut.FormattingEnabled = true;
            this.cmbCommandShortcut.Name = "cmbCommandShortcut";
            this.lvCommands.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.tlpBack.SetColumnSpan(this.lvCommands, 2);
            this.lvCommands.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvCommands, "lvCommands");
            this.lvCommands.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvCommands.FullRowSelect = true;
            this.lvCommands.HeaderStyle = ColumnHeaderStyle.None;
            this.lvCommands.HideSelection = false;
            this.lvCommands.MultiSelect = false;
            this.lvCommands.Name = "lvCommands";
            this.lvCommands.ShowColumnLines = true;
            this.lvCommands.ShowItemToolTips = true;
            this.lvCommands.SmallImageList = this.imgCommand;
            this.lvCommands.Sorting = SortOrder.Ascending;
            this.lvCommands.UseCompatibleStateImageBehavior = false;
            this.lvCommands.View = View.Details;
            this.lvCommands.KeyDown += new KeyEventHandler(this.lvCommands_KeyDown);
            this.lvCommands.SizeChanged += new EventHandler(this.ListView_SizeChanged);
            this.lvCommands.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvCommands_ItemSelectionChanged);
            this.imgCommand.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgCommand, "imgCommand");
            this.imgCommand.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.lblCategories, 0, 0);
            this.tlpBack.Controls.Add(this.lstCategory, 0, 1);
            this.tlpBack.Controls.Add(this.lblCommands, 1, 0);
            this.tlpBack.Controls.Add(this.lblCommandShortcut, 0, 2);
            this.tlpBack.Controls.Add(this.cmbCommandShortcut, 0, 3);
            this.tlpBack.Controls.Add(this.btnRemove, 2, 3);
            this.tlpBack.Controls.Add(this.lblShortcut, 0, 4);
            this.tlpBack.Controls.Add(this.btnAssign, 2, 5);
            this.tlpBack.Controls.Add(this.lblShortcutUsedBy, 0, 6);
            this.tlpBack.Controls.Add(this.txtShortcutUsedBy, 0, 7);
            this.tlpBack.Controls.Add(this.txtShortcut, 0, 5);
            this.tlpBack.Controls.Add(this.lvCommands, 1, 1);
            this.tlpBack.Name = "tlpBack";
            this.lblCategories.BackColor = Color.FromArgb(0xdd, 0xe7, 0xee);
            manager.ApplyResources(this.lblCategories, "lblCategories");
            this.lblCategories.ForeColor = Color.Navy;
            this.lblCategories.Name = "lblCategories";
            this.lblCategories.Paint += new PaintEventHandler(this.lblCategories_Paint);
            this.lblCommands.BackColor = Color.FromArgb(0xdd, 0xe7, 0xee);
            this.tlpBack.SetColumnSpan(this.lblCommands, 2);
            manager.ApplyResources(this.lblCommands, "lblCommands");
            this.lblCommands.ForeColor = Color.Navy;
            this.lblCommands.Name = "lblCommands";
            this.lblCommands.Paint += new PaintEventHandler(this.lblCategories_Paint);
            manager.ApplyResources(this.lblCommandShortcut, "lblCommandShortcut");
            this.tlpBack.SetColumnSpan(this.lblCommandShortcut, 3);
            this.lblCommandShortcut.Name = "lblCommandShortcut";
            manager.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            manager.ApplyResources(this.lblShortcut, "lblShortcut");
            this.tlpBack.SetColumnSpan(this.lblShortcut, 3);
            this.lblShortcut.Name = "lblShortcut";
            manager.ApplyResources(this.btnAssign, "btnAssign");
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.UseVisualStyleBackColor = true;
            this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
            manager.ApplyResources(this.lblShortcutUsedBy, "lblShortcutUsedBy");
            this.tlpBack.SetColumnSpan(this.lblShortcutUsedBy, 3);
            this.lblShortcutUsedBy.Name = "lblShortcutUsedBy";
            this.tlpBack.SetColumnSpan(this.txtShortcutUsedBy, 3);
            manager.ApplyResources(this.txtShortcutUsedBy, "txtShortcutUsedBy");
            this.txtShortcutUsedBy.Name = "txtShortcutUsedBy";
            this.tlpBack.SetColumnSpan(this.txtShortcut, 2);
            manager.ApplyResources(this.txtShortcut, "txtShortcut");
            this.txtShortcut.Name = "txtShortcut";
            this.txtShortcut.HotKeyChanged += new EventHandler(this.txtShortcut_HotKeyChanged);
            this.txtShortcut.PreviewHotKey += new EventHandler<PreviewHotKeyEventArgs>(this.txtShortcut_PreviewHotKey);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tlpBack);
            base.Name = "KeyboardMapOptionControl";
            base.Load += new EventHandler(this.KeyboardMapOptionControl_Load);
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            base.ResumeLayout(false);
        }

        private void KeyboardMapOptionControl_Load(object sender, EventArgs e)
        {
            this.lblCategories.Font = new Font(this.lblCategories.Font, FontStyle.Bold);
            this.lblCommands.Font = this.lblCategories.Font;
            this.InitializeCommands();
            GlobalHotKeyManager.SuspendHotKeys();
            base.Disposed += delegate (object dummy1, EventArgs dummy2) {
                GlobalHotKeyManager.ResumeHotKeys();
            };
            if (this.lstCategory.SelectedIndex < 0)
            {
                this.lstCategory.SelectedIndex = 0;
            }
        }

        private void lblCategories_Paint(object sender, PaintEventArgs e)
        {
            Rectangle clientRectangle = ((Label) sender).ClientRectangle;
            using (Pen pen = new Pen(Theme.Current.ThemeColors.OptionBlockLabelBorder))
            {
                e.Graphics.DrawLine(pen, 0, clientRectangle.Bottom - 1, clientRectangle.Right, clientRectangle.Bottom - 1);
            }
        }

        private void ListView_SizeChanged(ListView listView)
        {
            int width = listView.ClientSize.Width;
            if (listView.Columns.Count > 1)
            {
                listView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                width -= listView.Columns[1].Width;
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
            this.KeyMap = MainForm.Instance.KeyMap;
        }

        private void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lvCommands.BeginUpdate();
            try
            {
                this.lvCommands.Items.Clear();
                object selectedItem = ((ListBox) sender).SelectedItem;
                if (selectedItem != null)
                {
                    KeyValuePair<string, ListViewItem[]> pair = (KeyValuePair<string, ListViewItem[]>) selectedItem;
                    this.lvCommands.Items.AddRange(pair.Value);
                }
                if (this.lvCommands.Items.Count > 0)
                {
                    this.lvCommands.Items[0].Focus(true, true);
                }
                else
                {
                    this.UpdateButtons(false);
                }
                this.ListView_SizeChanged(this.lvCommands);
            }
            finally
            {
                this.lvCommands.EndUpdate();
            }
        }

        private void lvCommands_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Focused)
            {
                e.Item.Selected = true;
                IComponent tag = (IComponent) e.Item.Tag;
                this.cmbCommandShortcut.BeginUpdate();
                try
                {
                    this.cmbCommandShortcut.Items.Clear();
                    if (tag != null)
                    {
                        foreach (KeyValuePair<Keys, IComponent> pair in this.KeyMap)
                        {
                            if (pair.Value == tag)
                            {
                                this.cmbCommandShortcut.Items.Add(pair.Key);
                            }
                        }
                    }
                }
                finally
                {
                    this.cmbCommandShortcut.EndUpdate();
                }
            }
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new Action<bool>(this.UpdateButtons), new object[] { false });
            }
            else
            {
                this.UpdateButtons(false);
            }
        }

        private void lvCommands_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = e.KeyData == (Keys.Control | Keys.Add);
        }

        public void ResetComponentSettings()
        {
            this.KeyMap = new Dictionary<Keys, IComponent>(Settings.Default.DefaultKeyMap);
        }

        public void SaveComponentSettings()
        {
            Dictionary<Keys, IComponent> dictionary = new Dictionary<Keys, IComponent>(Settings.Default.DefaultKeyMap);
            Dictionary<IComponent, List<Keys>> dictionary2 = new Dictionary<IComponent, List<Keys>>();
            foreach (KeyValuePair<Keys, IComponent> pair in this.KeyMap)
            {
                IComponent component;
                if (!dictionary.TryGetValue(pair.Key, out component))
                {
                    dictionary2[pair.Value] = null;
                }
                else if (component != pair.Value)
                {
                    dictionary2[component] = null;
                    dictionary2[pair.Value] = null;
                }
                dictionary.Remove(pair.Key);
            }
            foreach (KeyValuePair<Keys, IComponent> pair in dictionary)
            {
                dictionary2[pair.Value] = null;
            }
            foreach (KeyValuePair<Keys, IComponent> pair in this.KeyMap)
            {
                List<Keys> list;
                if (dictionary2.TryGetValue(pair.Value, out list))
                {
                    if (list == null)
                    {
                        list = new List<Keys>();
                        dictionary2[pair.Value] = list;
                    }
                    list.Add(pair.Key);
                }
            }
            TypeConverter converter = new KeysConverter();
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<IComponent, List<Keys>> pair2 in dictionary2)
            {
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }
                builder.Append(GetCommandName(pair2.Key));
                if (pair2.Value != null)
                {
                    foreach (Keys keys in pair2.Value)
                    {
                        builder.Append(',');
                        builder.Append(converter.ConvertToInvariantString(keys));
                    }
                }
            }
            Settings.Default.KeyboardMap = builder.ToString();
        }

        private void txtShortcut_HotKeyChanged(object sender, EventArgs e)
        {
            this.UpdateShortcutUsedBy();
            this.UpdateButtons(false);
        }

        private void txtShortcut_PreviewHotKey(object sender, PreviewHotKeyEventArgs e)
        {
            switch (e.HotKey)
            {
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Space:
                case Keys.Back:
                case Keys.Return:
                    e.Cancel = false;
                    break;
            }
        }

        private void UpdateButtons(bool updateFocusedShortcut)
        {
            ListViewItem focusedItem = this.lvCommands.FocusedItem;
            this.btnAssign.Enabled = (this.txtShortcut.HotKey != Keys.None) && (focusedItem != null);
            this.cmbCommandShortcut.Enabled = this.cmbCommandShortcut.Items.Count > 0;
            if ((this.cmbCommandShortcut.SelectedIndex < 0) && (this.cmbCommandShortcut.Items.Count > 0))
            {
                this.cmbCommandShortcut.SelectedIndex = 0;
            }
            this.btnRemove.Enabled = this.cmbCommandShortcut.SelectedItem != null;
            if (updateFocusedShortcut && (focusedItem != null))
            {
                this.UpdateCommandShortcut(focusedItem, (this.cmbCommandShortcut.Items.Count > 0) ? ((Keys) this.cmbCommandShortcut.Items[0]) : Keys.None, null);
                this.ListView_SizeChanged(this.lvCommands);
            }
        }

        private void UpdateCommandShortcut(ListViewItem item, Keys shortcutKeys, TypeConverter keysConverter)
        {
            ListViewItem.ListViewSubItem item2 = (item.SubItems.Count > 1) ? item.SubItems[1] : item.SubItems.Add("-");
            if (keysConverter == null)
            {
                keysConverter = TypeDescriptor.GetConverter(typeof(Keys));
            }
            item2.Text = keysConverter.ConvertToString(shortcutKeys);
            item2.ForeColor = SystemColors.GrayText;
            item.UseItemStyleForSubItems = shortcutKeys != Keys.None;
        }

        private void UpdateShortcutUsedBy()
        {
            IComponent component;
            this.txtShortcutUsedBy.Text = this.KeyMap.TryGetValue(this.txtShortcut.HotKey, out component) ? GetCommandName(component) : string.Empty;
        }

        public bool SaveSettings
        {
            get
            {
                return this.Modified;
            }
            set
            {
                this.Modified = value;
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
    }
}

