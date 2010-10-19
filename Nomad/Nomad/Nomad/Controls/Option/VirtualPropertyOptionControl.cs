namespace Nomad.Controls.Option
{
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Property.Providers;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    [ToolboxItem(false)]
    public class VirtualPropertyOptionControl : UserControl, IPersistComponentSettings
    {
        private VisualStyleRenderer BackgroundRenderer;
        private IContainer components = null;
        private Dictionary<string, int> DisabledProviders = new Dictionary<string, int>();
        private FormEx FOwner;
        private PictureBox imgNote1;
        private PictureBox imgNote2;
        private ImageList imgPropertyTag;
        private Label lblNote1;
        private Label lblNote2;
        private Label lblProperties;
        private Label lblProviders;
        private ListViewEx lvProperties;
        private ListViewEx lvProviders;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpNote1;
        private TableLayoutPanel tlpNote2;

        public VirtualPropertyOptionControl()
        {
            this.InitializeComponent();
            this.OnThemeChanged(null, EventArgs.Empty);
            if (this.lvProperties.ExplorerTheme)
            {
                this.imgPropertyTag.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
            }
            else
            {
                this.imgPropertyTag.ImageSize = ImageHelper.DefaultSmallIconSize;
            }
            this.imgNote1.Image = IconSet.Information;
            this.imgNote2.Image = IconSet.Information;
            this.lblProviders.BackColor = Theme.Current.ThemeColors.OptionBlockLabelBackground;
            this.lblProviders.ForeColor = Theme.Current.ThemeColors.OptionBlockLabelText;
            this.lblProperties.BackColor = this.lblProviders.BackColor;
            this.lblProperties.ForeColor = this.lblProviders.ForeColor;
            this.lvProviders.Columns.Add(string.Empty);
            this.lvProviders.Columns.Add(string.Empty);
            this.lvProperties.Columns.Add(string.Empty);
            this.imgPropertyTag.AddAspected(IconSet.GetImage("PropertyType_Other"));
            this.imgPropertyTag.AddAspected(IconSet.GetImage("PropertyType_Integral"));
            this.imgPropertyTag.AddAspected(IconSet.GetImage("PropertyType_String"));
            this.imgPropertyTag.AddAspected(IconSet.GetImage("PropertyType_DateTime"));
            this.imgPropertyTag.AddAspected(IconSet.GetImage("PropertyType_Version"));
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(VirtualPropertyOptionControl));
            this.imgPropertyTag = new ImageList(this.components);
            this.lblProviders = new Label();
            this.lblProperties = new Label();
            this.tlpBack = new TableLayoutPanel();
            this.lvProviders = new ListViewEx();
            this.lvProperties = new ListViewEx();
            this.tlpNote1 = new TableLayoutPanel();
            this.lblNote1 = new Label();
            this.imgNote1 = new PictureBox();
            this.tlpNote2 = new TableLayoutPanel();
            this.imgNote2 = new PictureBox();
            this.lblNote2 = new Label();
            this.tlpBack.SuspendLayout();
            this.tlpNote1.SuspendLayout();
            ((ISupportInitialize) this.imgNote1).BeginInit();
            this.tlpNote2.SuspendLayout();
            ((ISupportInitialize) this.imgNote2).BeginInit();
            base.SuspendLayout();
            this.imgPropertyTag.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgPropertyTag, "imgPropertyTag");
            this.imgPropertyTag.TransparentColor = Color.Transparent;
            this.lblProviders.BackColor = Color.FromArgb(0xdd, 0xe7, 0xee);
            manager.ApplyResources(this.lblProviders, "lblProviders");
            this.lblProviders.ForeColor = Color.Navy;
            this.lblProviders.Name = "lblProviders";
            this.lblProviders.Paint += new PaintEventHandler(this.lblProviders_Paint);
            this.lblProperties.BackColor = Color.FromArgb(0xdd, 0xe7, 0xee);
            manager.ApplyResources(this.lblProperties, "lblProperties");
            this.lblProperties.ForeColor = Color.Navy;
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Paint += new PaintEventHandler(this.lblProviders_Paint);
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.lblProperties, 1, 0);
            this.tlpBack.Controls.Add(this.lblProviders, 0, 0);
            this.tlpBack.Controls.Add(this.lvProviders, 0, 1);
            this.tlpBack.Controls.Add(this.lvProperties, 1, 1);
            this.tlpBack.Controls.Add(this.tlpNote1, 0, 2);
            this.tlpBack.Controls.Add(this.tlpNote2, 0, 3);
            this.tlpBack.Name = "tlpBack";
            this.lvProviders.CheckBoxes = true;
            this.lvProviders.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvProviders, "lvProviders");
            this.lvProviders.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvProviders.FullRowSelect = true;
            this.lvProviders.HeaderStyle = ColumnHeaderStyle.None;
            this.lvProviders.HideSelection = false;
            this.lvProviders.MultiSelect = false;
            this.lvProviders.Name = "lvProviders";
            this.lvProviders.UseCompatibleStateImageBehavior = false;
            this.lvProviders.View = View.Details;
            this.lvProviders.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvProviders_ItemSelectionChanged);
            this.lvProviders.KeyDown += new KeyEventHandler(this.ListView_KeyDown);
            this.lvProviders.SizeChanged += new EventHandler(this.ListView_SizeChanged);
            this.lvProperties.CheckBoxes = true;
            this.lvProperties.CollapsibleGroups = true;
            this.lvProperties.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvProperties, "lvProperties");
            this.lvProperties.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvProperties.FullRowSelect = true;
            this.lvProperties.HeaderStyle = ColumnHeaderStyle.None;
            this.lvProperties.MultiSelect = false;
            this.lvProperties.Name = "lvProperties";
            this.lvProperties.SmallImageList = this.imgPropertyTag;
            this.lvProperties.UseCompatibleStateImageBehavior = false;
            this.lvProperties.View = View.Details;
            this.lvProperties.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvProperties_ItemSelectionChanged);
            this.lvProperties.KeyDown += new KeyEventHandler(this.ListView_KeyDown);
            this.lvProperties.ItemChecked += new ItemCheckedEventHandler(this.lvProperties_ItemChecked);
            this.lvProperties.SizeChanged += new EventHandler(this.ListView_SizeChanged);
            manager.ApplyResources(this.tlpNote1, "tlpNote1");
            this.tlpBack.SetColumnSpan(this.tlpNote1, 2);
            this.tlpNote1.Controls.Add(this.lblNote1, 1, 0);
            this.tlpNote1.Controls.Add(this.imgNote1, 0, 0);
            this.tlpNote1.Name = "tlpNote1";
            this.tlpNote1.Paint += new PaintEventHandler(this.tlpNote_Paint);
            manager.ApplyResources(this.lblNote1, "lblNote1");
            this.lblNote1.Name = "lblNote1";
            manager.ApplyResources(this.imgNote1, "imgNote1");
            this.imgNote1.Name = "imgNote1";
            this.imgNote1.TabStop = false;
            manager.ApplyResources(this.tlpNote2, "tlpNote2");
            this.tlpBack.SetColumnSpan(this.tlpNote2, 2);
            this.tlpNote2.Controls.Add(this.imgNote2, 0, 0);
            this.tlpNote2.Controls.Add(this.lblNote2, 1, 0);
            this.tlpNote2.Name = "tlpNote2";
            this.tlpNote2.Paint += new PaintEventHandler(this.tlpNote_Paint);
            manager.ApplyResources(this.imgNote2, "imgNote2");
            this.imgNote2.Name = "imgNote2";
            this.imgNote2.TabStop = false;
            manager.ApplyResources(this.lblNote2, "lblNote2");
            this.lblNote2.Name = "lblNote2";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tlpBack);
            base.Name = "VirtualPropertyOptionControl";
            base.Load += new EventHandler(this.VirtualPropertyOptionControl_Load);
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.tlpNote1.ResumeLayout(false);
            this.tlpNote1.PerformLayout();
            ((ISupportInitialize) this.imgNote1).EndInit();
            this.tlpNote2.ResumeLayout(false);
            this.tlpNote2.PerformLayout();
            ((ISupportInitialize) this.imgNote2).EndInit();
            base.ResumeLayout(false);
        }

        private void lblProviders_Paint(object sender, PaintEventArgs e)
        {
            Rectangle clientRectangle = ((Label) sender).ClientRectangle;
            using (Pen pen = new Pen(Theme.Current.ThemeColors.OptionBlockLabelBorder))
            {
                e.Graphics.DrawLine(pen, 0, clientRectangle.Bottom - 1, clientRectangle.Right, clientRectangle.Bottom - 1);
            }
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = e.KeyData == (Keys.Control | Keys.Add);
        }

        private void ListView_SizeChanged(System.Windows.Forms.ListView listView)
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
            System.Windows.Forms.ListView listView = (System.Windows.Forms.ListView) sender;
            if (listView.Columns.Count >= 1)
            {
                if (base.IsHandleCreated)
                {
                    base.BeginInvoke(new Action<System.Windows.Forms.ListView>(this.ListView_SizeChanged), new object[] { listView });
                }
                else
                {
                    this.ListView_SizeChanged(listView);
                }
            }
        }

        public void LoadComponentSettings()
        {
            ListViewItem item;
            VirtualPropertySet set = new VirtualPropertySet(DefaultProperty.DefaultSet);
            this.lvProviders.BeginUpdate();
            try
            {
                this.lvProviders.Items.Clear();
                foreach (string str in StringHelper.SplitString(Settings.Default.DisabledPropertyProviders, new char[] { ',' }))
                {
                    this.DisabledProviders.Add(str, 0);
                }
                foreach (PropertyProviderInfo info in PropertyProviderManager.GetAllProviders())
                {
                    item = new ListViewItem(info.DisplayName) {
                        Tag = info,
                        Checked = !this.DisabledProviders.ContainsKey(info.Key)
                    };
                    IPropertyProvider component = info.Provider;
                    if (component == null)
                    {
                        item.SubItems.Add(Resources.sNotAvailable);
                        item.ForeColor = SystemColors.GrayText;
                    }
                    else
                    {
                        VersionAttribute attribute = TypeDescriptor.GetAttributes(component).OfType<VersionAttribute>().FirstOrDefault<VersionAttribute>();
                        if (attribute != null)
                        {
                            item.SubItems.Add(attribute.Version.ToString());
                        }
                        if (item.Checked)
                        {
                            set.Or(component.GetRegisteredProperties());
                        }
                    }
                    this.lvProviders.Items.Add(item);
                }
            }
            finally
            {
                this.lvProviders.EndUpdate();
            }
            this.lvProperties.BeginUpdate();
            try
            {
                this.lvProperties.Items.Clear();
                Dictionary<int, ListViewGroup> dictionary = new Dictionary<int, ListViewGroup>();
                foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) VirtualProperty.All)
                {
                    ListViewGroup group;
                    if ((property.PropertyOptions & VirtualPropertyOption.Hidden) > 0)
                    {
                        continue;
                    }
                    if (!dictionary.TryGetValue(property.GroupId, out group))
                    {
                        group = new ListViewGroup(property.LocalizedGroupName);
                        this.lvProperties.Groups.Add(group);
                        dictionary.Add(property.GroupId, group);
                    }
                    item = new ListViewItem(property.LocalizedName) {
                        Group = group,
                        Checked = !property.PropertyVisible ? false : set[property.PropertyId],
                        Tag = property
                    };
                    if (!((property.PropertyId != 0) && set[property.PropertyId]))
                    {
                        item.ForeColor = SystemColors.GrayText;
                    }
                    switch (System.Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                            item.ImageIndex = 1;
                            break;

                        case TypeCode.DateTime:
                            item.ImageIndex = 3;
                            break;

                        case TypeCode.String:
                            item.ImageIndex = 2;
                            break;

                        default:
                            if (property.PropertyType == typeof(Version))
                            {
                                item.ImageIndex = 4;
                            }
                            else
                            {
                                item.ImageIndex = 0;
                            }
                            break;
                    }
                    this.lvProperties.Items.Add(item);
                }
            }
            finally
            {
                this.lvProperties.EndUpdate();
            }
        }

        private void lvProperties_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lvProperties.Items[e.Index].ForeColor == SystemColors.GrayText)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void lvProperties_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) > Keys.None)
            {
                foreach (ListViewItem item in e.Item.Group.Items)
                {
                    item.Checked = e.Item.Checked;
                }
            }
        }

        private void lvProperties_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Focused)
            {
                e.Item.Selected = true;
            }
        }

        private void lvProviders_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                VirtualPropertySet set = new VirtualPropertySet(DefaultProperty.DefaultSet);
                foreach (ListViewItem item in this.lvProviders.Items)
                {
                    PropertyProviderInfo tag = (PropertyProviderInfo) item.Tag;
                    if ((item.Checked && (tag != null)) && (tag.Provider != null))
                    {
                        set.Or(tag.Provider.GetRegisteredProperties());
                    }
                }
                this.lvProperties.BeginUpdate();
                try
                {
                    foreach (ListViewItem item in this.lvProperties.Items)
                    {
                        VirtualProperty property = (VirtualProperty) item.Tag;
                        if (!set[property.PropertyId])
                        {
                            item.Checked = false;
                            item.ForeColor = SystemColors.GrayText;
                        }
                        else if (property.PropertyId != 0)
                        {
                            item.ForeColor = this.lvProperties.ForeColor;
                        }
                    }
                }
                finally
                {
                    this.lvProperties.EndUpdate();
                }
            }
        }

        private void lvProviders_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Focused)
            {
                e.Item.Selected = true;
                PropertyProviderInfo tag = (PropertyProviderInfo) e.Item.Tag;
                if ((tag != null) && (tag.Provider != null))
                {
                    VirtualPropertySet registeredProperties = tag.Provider.GetRegisteredProperties();
                    this.lvProperties.BeginUpdate();
                    try
                    {
                        foreach (ListViewItem item in this.lvProperties.Items)
                        {
                            if (registeredProperties[((VirtualProperty) item.Tag).PropertyId])
                            {
                                item.BackColor = Color.PaleGreen;
                            }
                            else
                            {
                                item.BackColor = this.lvProperties.BackColor;
                            }
                        }
                    }
                    finally
                    {
                        this.lvProperties.EndUpdate();
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.lblNote1.Font = new Font(SystemFonts.IconTitleFont.FontFamily, this.lblNote1.Font.SizeInPoints);
            this.lblNote2.Font = this.lblNote1.Font;
            base.OnLoad(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (this.FOwner != null)
            {
                this.FOwner.ThemeChanged -= new EventHandler(this.OnThemeChanged);
            }
            base.OnParentChanged(e);
            this.FOwner = base.ParentForm as FormEx;
            if (this.FOwner != null)
            {
                this.FOwner.ThemeChanged += new EventHandler(this.OnThemeChanged);
            }
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolTip.Standard.Normal))
            {
                this.BackgroundRenderer = new VisualStyleRenderer(VisualStyleElement.ToolTip.Standard.Normal);
                this.tlpNote1.ResetBackColor();
                this.imgNote1.BackColor = Color.Transparent;
                this.lblNote1.BackColor = Color.Transparent;
                this.lblNote1.ForeColor = this.BackgroundRenderer.GetColor(ColorProperty.TextColor);
                this.tlpNote2.ResetBackColor();
                this.imgNote2.BackColor = Color.Transparent;
                this.lblNote2.BackColor = Color.Transparent;
                this.lblNote2.ForeColor = this.BackgroundRenderer.GetColor(ColorProperty.TextColor);
            }
            else
            {
                this.BackgroundRenderer = null;
                this.tlpNote1.BackColor = SystemColors.Info;
                this.imgNote1.ResetBackColor();
                this.lblNote1.ResetBackColor();
                this.lblNote1.ForeColor = SystemColors.InfoText;
                this.tlpNote2.BackColor = SystemColors.Info;
                this.imgNote2.ResetBackColor();
                this.lblNote2.ResetBackColor();
                this.lblNote2.ForeColor = SystemColors.InfoText;
            }
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            StringBuilder builder = new StringBuilder();
            foreach (ListViewItem item in this.lvProviders.Items)
            {
                if (!item.Checked)
                {
                    PropertyProviderInfo tag = (PropertyProviderInfo) item.Tag;
                    if (builder.Length > 0)
                    {
                        builder.Append(',');
                    }
                    builder.Append(tag.Key);
                }
            }
            Settings.Default.DisabledPropertyProviders = builder.ToString();
            Settings.Default.HiddenProperties = TypeDescriptor.GetConverter(typeof(VirtualPropertySet)).ConvertToInvariantString(~this.NewVisible);
        }

        private void tlpNote_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control) sender;
            if (this.BackgroundRenderer != null)
            {
                this.BackgroundRenderer.DrawBackground(e.Graphics, control.ClientRectangle);
            }
            else
            {
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, control.ClientSize.Width - 1, control.ClientSize.Height - 1);
            }
        }

        private void VirtualPropertyOptionControl_Load(object sender, EventArgs e)
        {
            this.lblProviders.Font = new Font(this.lblProviders.Font, FontStyle.Bold);
            this.lblProperties.Font = this.lblProviders.Font;
            this.lvProviders.ItemChecked += new ItemCheckedEventHandler(this.lvProviders_ItemChecked);
            this.lvProperties.ItemCheck += new ItemCheckEventHandler(this.lvProperties_ItemCheck);
        }

        private VirtualPropertySet NewVisible
        {
            get
            {
                VirtualPropertySet set = new VirtualPropertySet();
                foreach (ListViewItem item in this.lvProperties.Items)
                {
                    if (item.Checked)
                    {
                        VirtualProperty tag = (VirtualProperty) item.Tag;
                        set[tag.PropertyId] = true;
                    }
                }
                return set;
            }
        }

        public bool SaveSettings
        {
            get
            {
                bool flag = false;
                foreach (ListViewItem item in this.lvProviders.Items)
                {
                    PropertyProviderInfo tag = (PropertyProviderInfo) item.Tag;
                    flag = item.Checked ^ !this.DisabledProviders.ContainsKey(tag.Key);
                    if (flag)
                    {
                        break;
                    }
                }
                if (!flag)
                {
                    flag = !this.NewVisible.Equals(VirtualProperty.Visible);
                }
                return flag;
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
    }
}

