namespace Nomad.Dialogs
{
    using Microsoft;
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class CustomizeFolderDialog : BasicDialog
    {
        private OpenFileDialog BrowseIconDialog;
        private Button btnBrowseIcon;
        private Button btnCancel;
        private Button btnClear;
        private Button btnCustomizeColumns;
        private Button btnCustomizeFilter;
        private Button btnCustomizeSort;
        private ColorButton btnListBackColor;
        private ColorButton btnListForeColor;
        private Button btnOk;
        private Bevel bvlButtons;
        private PropertyValuesWatcher CheckWatcher;
        private CheckBox chkApplyToChildren;
        private CheckBox chkCustomizeColumns;
        private CheckBox chkCustomizeFilter;
        private CheckBox chkCustomizeIcon;
        private CheckBox chkCustomizeSort;
        private CheckBox chkCustomizeView;
        private ComboBox cmbView;
        private IContainer components = null;
        private bool FAutoSizeColumns;
        private VirtualPropertySet FAvailableProperties;
        private bool FChanged;
        private ICustomizeFolder FCustomizeFolder;
        private PanelView? FFolderView;
        private int FListColumnCount;
        private Size FThumbnailSize;
        private Size FThumbnailSpacing;
        private ImageList imgViews;
        private Label lblFolder;
        private PictureBox picIcon;
        private PanelEx pnlIcon;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpButtons;
        private VirtualItemToolStrip tsFolder;
        private TextBox txtIconLocation;
        private TypeConverter ViewTypeConverter;

        public CustomizeFolderDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.btnListBackColor.DefaultColor = Color.Empty;
            this.btnListForeColor.DefaultColor = Color.Empty;
            if (!this.InitializeViewIcons(new string[] { "actViewAsThumbnail", "actViewAsLargeIcon", "actViewAsSmallIcon", "actViewAsList", "actViewAsDetails" }))
            {
                this.cmbView.DrawMode = DrawMode.Normal;
            }
            this.cmbView.Items.Add(PanelView.Thumbnail);
            this.cmbView.Items.Add(PanelView.LargeIcon);
            this.cmbView.Items.Add(PanelView.SmallIcon);
            this.cmbView.Items.Add(PanelView.List);
            this.cmbView.Items.Add(PanelView.Details);
            this.cmbView.SelectedItem = PanelView.List;
            if (!Application.RenderWithVisualStyles)
            {
                this.pnlIcon.BorderStyle = BorderStyle.Fixed3D;
                this.pnlIcon.FlatBorder = AnchorStyles.None;
                this.pnlIcon.Padding = new Padding(0);
            }
        }

        private void btnBrowseIcon_Click(object sender, EventArgs e)
        {
            this.txtIconLocation.Tag = this.txtIconLocation.Text;
            IconLocation location = IconLocation.TryParse(this.txtIconLocation.Text);
            if (OS.IsWinXP)
            {
                StringBuilder builder;
                int piIconIndex = 0;
                if (location != null)
                {
                    builder = new StringBuilder(location.IconFileName, 260);
                    piIconIndex = location.IconIndex;
                }
                else
                {
                    builder = new StringBuilder(260);
                }
                if (Microsoft.Shell.Shell32.PickIconDlg(base.Handle, builder, builder.Capacity, ref piIconIndex) != 0)
                {
                    this.txtIconLocation.Text = string.Format("{0},{1}", builder.ToString(), piIconIndex);
                    this.txtIconLocation_Validated(this.txtIconLocation, EventArgs.Empty);
                }
            }
            else
            {
                if (location != null)
                {
                    this.BrowseIconDialog.FileName = Environment.ExpandEnvironmentVariables(location.IconFileName);
                }
                if (this.BrowseIconDialog.ShowDialog() == DialogResult.OK)
                {
                    this.txtIconLocation.Text = this.BrowseIconDialog.FileName;
                    this.txtIconLocation_Validated(this.txtIconLocation, EventArgs.Empty);
                }
            }
        }

        private void btnBrowseIcon_SizeChanged(object sender, EventArgs e)
        {
            this.btnListBackColor.Width = this.btnBrowseIcon.Width;
            this.btnListForeColor.Width = this.btnBrowseIcon.Width;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.chkCustomizeIcon.Checked = false;
            this.btnListBackColor.Color = Color.Empty;
            this.btnListForeColor.Color = Color.Empty;
            this.chkCustomizeFilter.Checked = false;
            this.chkCustomizeSort.Checked = false;
            this.chkCustomizeView.Checked = false;
            this.chkCustomizeColumns.Checked = false;
            this.chkApplyToChildren.Checked = false;
        }

        private void btnCustomizeColumns_Click(object sender, EventArgs e)
        {
            using (ManageColumnsDialog dialog = new ManageColumnsDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.AutosizeColumns = this.FAutoSizeColumns;
                dialog.AvailableProperties = this.FAvailableProperties;
                dialog.Columns = this.btnCustomizeColumns.Tag as IEnumerable<ListViewColumnInfo>;
                dialog.RememberColumnsEnabled = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.btnCustomizeColumns.Tag = dialog.Columns;
                    this.FAutoSizeColumns = dialog.AutosizeColumns;
                    this.OnChanged(EventArgs.Empty);
                }
            }
        }

        private void btnCustomizeFilter_Click(object sender, EventArgs e)
        {
            using (FilterDialog dialog = new FilterDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.Filter = (IVirtualItemFilter) this.btnCustomizeFilter.Tag;
                dialog.RememberFilterEnabled = false;
                if (dialog.Execute(this))
                {
                    this.btnCustomizeFilter.Tag = dialog.Filter;
                    this.OnChanged(EventArgs.Empty);
                }
            }
        }

        private void btnCustomizeSort_Click(object sender, EventArgs e)
        {
            using (SortDialog dialog = new SortDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.AvailableProperties = this.FAvailableProperties;
                dialog.Sort = this.btnCustomizeSort.Tag as VirtualItemComparer;
                dialog.RememberSortEnabled = false;
                if (dialog.Execute())
                {
                    this.btnCustomizeSort.Tag = dialog.Sort;
                    this.OnChanged(EventArgs.Empty);
                }
            }
        }

        private void chkCustomizeIcon_CheckedChanged(object sender, EventArgs e)
        {
            this.txtIconLocation.Enabled = this.chkCustomizeIcon.Checked;
            this.btnBrowseIcon.Enabled = this.chkCustomizeIcon.Checked;
            if (this.chkCustomizeIcon.Checked)
            {
                this.UpdateIcon(this.txtIconLocation.Text);
            }
            else
            {
                this.picIcon.Image = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, new Size(0x20, 0x20));
            }
            this.UpdateButtons(sender, e);
        }

        private void cmbView_DrawItem(object sender, DrawItemEventArgs e)
        {
            int num;
            if (this.cmbView.Enabled)
            {
                e.DrawBackground();
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
            }
            PanelView view = (PanelView) this.cmbView.Items[e.Index];
            switch (view)
            {
                case PanelView.LargeIcon:
                    num = 1;
                    break;

                case PanelView.Details:
                    num = 4;
                    break;

                case PanelView.SmallIcon:
                    num = 2;
                    break;

                case PanelView.List:
                    num = 3;
                    break;

                case PanelView.Thumbnail:
                    num = 0;
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
            this.imgViews.Draw(e.Graphics, e.Bounds.Left + 4, (e.Bounds.Top + 1) + ((e.Bounds.Height - this.imgViews.ImageSize.Height) / 2), num + (this.cmbView.Enabled ? 0 : this.cmbView.Items.Count));
            Rectangle bounds = Rectangle.FromLTRB(e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom);
            TextRenderer.DrawText(e.Graphics, this.ViewTypeConverter.ConvertToString(view), e.Font, bounds, this.cmbView.Enabled ? e.ForeColor : SystemColors.GrayText, TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
            e.DrawFocusRectangle();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute(IVirtualFolder folder, ICustomizeFolder customize, ICustomizeFolder defaultCustomize, VirtualPropertySet availableProperties)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            if (customize == null)
            {
                throw new ArgumentNullException("customizeFolder");
            }
            this.FAvailableProperties = availableProperties;
            this.FCustomizeFolder = customize;
            this.tsFolder.Add(folder);
            this.btnListBackColor.Color = (customize.BackColor.IsEmpty && (defaultCustomize != null)) ? defaultCustomize.BackColor : customize.BackColor;
            this.btnListForeColor.Color = (customize.ForeColor.IsEmpty && (defaultCustomize != null)) ? defaultCustomize.ForeColor : customize.ForeColor;
            this.chkCustomizeFilter.Enabled = (this.FCustomizeFolder.UpdatableParts & CustomizeFolderParts.Filter) > 0;
            if (this.chkCustomizeFilter.Enabled)
            {
                this.btnCustomizeFilter.Tag = this.FCustomizeFolder.Filter;
                this.chkCustomizeFilter.Checked = this.btnCustomizeFilter.Tag != null;
                if ((this.btnCustomizeFilter.Tag == null) && (defaultCustomize != null))
                {
                    this.btnCustomizeFilter.Tag = defaultCustomize.Filter;
                }
            }
            this.chkCustomizeSort.Enabled = (this.FCustomizeFolder.UpdatableParts & CustomizeFolderParts.Sort) > 0;
            if (this.chkCustomizeSort.Enabled)
            {
                this.btnCustomizeSort.Tag = this.FCustomizeFolder.Sort;
                this.chkCustomizeSort.Checked = this.btnCustomizeSort.Tag != null;
                if ((this.btnCustomizeSort.Tag == null) && (defaultCustomize != null))
                {
                    this.btnCustomizeSort.Tag = defaultCustomize.Sort;
                }
            }
            this.chkCustomizeView.Enabled = (this.FCustomizeFolder.UpdatableParts & CustomizeFolderParts.View) > 0;
            if (this.chkCustomizeView.Enabled)
            {
                this.FFolderView = this.FCustomizeFolder.View;
                this.chkCustomizeView.Checked = this.FFolderView.HasValue;
                if (this.FFolderView.HasValue)
                {
                    this.cmbView.SelectedItem = this.FFolderView.Value;
                }
                else if (defaultCustomize != null)
                {
                    this.cmbView.SelectedItem = defaultCustomize.View;
                }
                int? listColumnCount = this.FCustomizeFolder.ListColumnCount;
                if (listColumnCount.HasValue)
                {
                    this.FListColumnCount = listColumnCount.Value;
                }
                else if ((defaultCustomize != null) && defaultCustomize.ListColumnCount.HasValue)
                {
                    this.FListColumnCount = defaultCustomize.ListColumnCount.Value;
                }
                this.FThumbnailSize = this.FCustomizeFolder.ThumbnailSize;
                if (this.FThumbnailSize.IsEmpty && (defaultCustomize != null))
                {
                    this.FThumbnailSize = defaultCustomize.ThumbnailSize;
                }
                this.FThumbnailSpacing = this.FCustomizeFolder.ThumbnailSpacing;
                if (this.FThumbnailSpacing.IsEmpty && (defaultCustomize != null))
                {
                    this.FThumbnailSpacing = defaultCustomize.ThumbnailSpacing;
                }
            }
            this.chkCustomizeColumns.Enabled = (this.FCustomizeFolder.UpdatableParts & CustomizeFolderParts.Columns) > 0;
            if (this.chkCustomizeColumns.Enabled)
            {
                this.btnCustomizeColumns.Tag = this.FCustomizeFolder.Columns;
                this.chkCustomizeColumns.Checked = this.btnCustomizeColumns.Tag != null;
                if ((this.btnCustomizeColumns.Tag == null) && (defaultCustomize != null))
                {
                    this.btnCustomizeColumns.Tag = defaultCustomize.Columns;
                }
                bool? autoSizeColumns = this.FCustomizeFolder.AutoSizeColumns;
                if (autoSizeColumns.HasValue)
                {
                    this.FAutoSizeColumns = autoSizeColumns.Value;
                }
                else if ((defaultCustomize != null) && defaultCustomize.AutoSizeColumns.HasValue)
                {
                    this.FAutoSizeColumns = defaultCustomize.AutoSizeColumns.Value;
                }
            }
            Size size = new Size(0x20, 0x20);
            if ((this.FCustomizeFolder.UpdatableParts & CustomizeFolderParts.Icon) == 0)
            {
                this.chkCustomizeIcon.Enabled = false;
                IVirtualItemUI mui = folder as IVirtualItemUI;
                if (mui != null)
                {
                    this.picIcon.Image = mui.GetIcon(size, 0);
                }
                else
                {
                    this.picIcon.Image = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Drive, size);
                }
            }
            else
            {
                IconLocation icon = this.FCustomizeFolder.Icon;
                if (icon != null)
                {
                    this.picIcon.Image = CustomImageProvider.LoadIcon(Environment.ExpandEnvironmentVariables(icon.IconFileName), icon.IconIndex, size);
                }
                if (this.picIcon.Image != null)
                {
                    this.txtIconLocation.Text = icon.ToString();
                    this.chkCustomizeIcon.Checked = true;
                }
                else
                {
                    this.picIcon.Image = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, size);
                }
            }
            this.ViewTypeConverter = TypeDescriptor.GetConverter(typeof(PanelView));
            this.CheckWatcher.RememberValues();
            return (base.ShowDialog() == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CustomizeFolderDialog));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            PropertyValue value8 = new PropertyValue();
            PropertyValue value9 = new PropertyValue();
            this.btnCustomizeFilter = new Button();
            this.btnCustomizeColumns = new Button();
            this.cmbView = new ComboBox();
            this.chkCustomizeColumns = new CheckBox();
            this.chkCustomizeView = new CheckBox();
            this.chkCustomizeFilter = new CheckBox();
            this.chkCustomizeSort = new CheckBox();
            this.btnCustomizeSort = new Button();
            this.chkCustomizeIcon = new CheckBox();
            this.btnBrowseIcon = new Button();
            this.txtIconLocation = new TextBox();
            this.pnlIcon = new PanelEx();
            this.picIcon = new PictureBox();
            this.btnListForeColor = new ColorButton();
            this.btnListBackColor = new ColorButton();
            this.chkApplyToChildren = new CheckBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.imgViews = new ImageList(this.components);
            this.CheckWatcher = new PropertyValuesWatcher();
            this.BrowseIconDialog = new OpenFileDialog();
            this.btnClear = new Button();
            this.tlpBack = new TableLayoutPanel();
            this.lblFolder = new Label();
            this.tsFolder = new VirtualItemToolStrip(this.components);
            this.tlpButtons = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            GroupBox box = new GroupBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            GroupBox box2 = new GroupBox();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            GroupBox box3 = new GroupBox();
            TableLayoutPanel panel3 = new TableLayoutPanel();
            Label control = new Label();
            Label label2 = new Label();
            box.SuspendLayout();
            panel.SuspendLayout();
            box2.SuspendLayout();
            panel2.SuspendLayout();
            this.pnlIcon.SuspendLayout();
            ((ISupportInitialize) this.picIcon).BeginInit();
            box3.SuspendLayout();
            panel3.SuspendLayout();
            ((ISupportInitialize) this.CheckWatcher).BeginInit();
            this.tlpBack.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(box, "grpOptions");
            box.Controls.Add(panel);
            box.Name = "grpOptions";
            box.TabStop = false;
            manager.ApplyResources(panel, "tlpOptions");
            panel.Controls.Add(this.btnCustomizeFilter, 1, 0);
            panel.Controls.Add(this.btnCustomizeColumns, 1, 3);
            panel.Controls.Add(this.cmbView, 1, 2);
            panel.Controls.Add(this.chkCustomizeColumns, 0, 3);
            panel.Controls.Add(this.chkCustomizeView, 0, 2);
            panel.Controls.Add(this.chkCustomizeFilter, 0, 0);
            panel.Controls.Add(this.chkCustomizeSort, 0, 1);
            panel.Controls.Add(this.btnCustomizeSort, 1, 1);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpOptions";
            manager.ApplyResources(this.btnCustomizeFilter, "btnCustomizeFilter");
            this.btnCustomizeFilter.Name = "btnCustomizeFilter";
            this.btnCustomizeFilter.UseVisualStyleBackColor = true;
            this.btnCustomizeFilter.Click += new EventHandler(this.btnCustomizeFilter_Click);
            manager.ApplyResources(this.btnCustomizeColumns, "btnCustomizeColumns");
            this.btnCustomizeColumns.Name = "btnCustomizeColumns";
            this.btnCustomizeColumns.UseVisualStyleBackColor = true;
            this.btnCustomizeColumns.Click += new EventHandler(this.btnCustomizeColumns_Click);
            this.cmbView.DrawMode = DrawMode.OwnerDrawFixed;
            this.cmbView.DropDownStyle = ComboBoxStyle.DropDownList;
            manager.ApplyResources(this.cmbView, "cmbView");
            this.cmbView.FormattingEnabled = true;
            this.cmbView.Name = "cmbView";
            this.cmbView.DrawItem += new DrawItemEventHandler(this.cmbView_DrawItem);
            this.cmbView.SelectedIndexChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.chkCustomizeColumns, "chkCustomizeColumns");
            this.chkCustomizeColumns.Name = "chkCustomizeColumns";
            this.chkCustomizeColumns.UseVisualStyleBackColor = true;
            this.chkCustomizeColumns.CheckedChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.chkCustomizeView, "chkCustomizeView");
            this.chkCustomizeView.Name = "chkCustomizeView";
            this.chkCustomizeView.UseVisualStyleBackColor = true;
            this.chkCustomizeView.CheckedChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.chkCustomizeFilter, "chkCustomizeFilter");
            this.chkCustomizeFilter.Name = "chkCustomizeFilter";
            this.chkCustomizeFilter.UseVisualStyleBackColor = true;
            this.chkCustomizeFilter.CheckedChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.chkCustomizeSort, "chkCustomizeSort");
            this.chkCustomizeSort.Name = "chkCustomizeSort";
            this.chkCustomizeSort.UseVisualStyleBackColor = true;
            this.chkCustomizeSort.CheckedChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.btnCustomizeSort, "btnCustomizeSort");
            this.btnCustomizeSort.Name = "btnCustomizeSort";
            this.btnCustomizeSort.UseVisualStyleBackColor = true;
            this.btnCustomizeSort.Click += new EventHandler(this.btnCustomizeSort_Click);
            manager.ApplyResources(box2, "grpIcon");
            box2.Controls.Add(panel2);
            box2.Name = "grpIcon";
            box2.TabStop = false;
            manager.ApplyResources(panel2, "tlpIcon");
            panel2.Controls.Add(this.chkCustomizeIcon, 0, 0);
            panel2.Controls.Add(this.btnBrowseIcon, 2, 2);
            panel2.Controls.Add(this.txtIconLocation, 1, 1);
            panel2.Controls.Add(this.pnlIcon, 0, 1);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpIcon";
            manager.ApplyResources(this.chkCustomizeIcon, "chkCustomizeIcon");
            panel2.SetColumnSpan(this.chkCustomizeIcon, 3);
            this.chkCustomizeIcon.Name = "chkCustomizeIcon";
            this.chkCustomizeIcon.UseVisualStyleBackColor = true;
            this.chkCustomizeIcon.CheckedChanged += new EventHandler(this.chkCustomizeIcon_CheckedChanged);
            manager.ApplyResources(this.btnBrowseIcon, "btnBrowseIcon");
            this.btnBrowseIcon.Name = "btnBrowseIcon";
            this.btnBrowseIcon.UseVisualStyleBackColor = true;
            this.btnBrowseIcon.Click += new EventHandler(this.btnBrowseIcon_Click);
            this.btnBrowseIcon.SizeChanged += new EventHandler(this.btnBrowseIcon_SizeChanged);
            panel2.SetColumnSpan(this.txtIconLocation, 2);
            manager.ApplyResources(this.txtIconLocation, "txtIconLocation");
            this.txtIconLocation.Name = "txtIconLocation";
            this.txtIconLocation.Validated += new EventHandler(this.txtIconLocation_Validated);
            this.txtIconLocation.Enter += new EventHandler(this.txtIconLocation_Enter);
            this.pnlIcon.BorderColor = Color.FromArgb(0xa7, 0xa6, 170);
            this.pnlIcon.Controls.Add(this.picIcon);
            manager.ApplyResources(this.pnlIcon, "pnlIcon");
            this.pnlIcon.Name = "pnlIcon";
            panel2.SetRowSpan(this.pnlIcon, 2);
            manager.ApplyResources(this.picIcon, "picIcon");
            this.picIcon.Name = "picIcon";
            this.picIcon.TabStop = false;
            manager.ApplyResources(box3, "grpListColors");
            box3.Controls.Add(panel3);
            box3.Name = "grpListColors";
            box3.TabStop = false;
            manager.ApplyResources(panel3, "tlpListColors");
            panel3.Controls.Add(control, 0, 0);
            panel3.Controls.Add(this.btnListForeColor, 1, 1);
            panel3.Controls.Add(label2, 0, 1);
            panel3.Controls.Add(this.btnListBackColor, 1, 0);
            panel3.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel3.Name = "tlpListColors";
            manager.ApplyResources(control, "lblListBackColor");
            control.Name = "lblListBackColor";
            this.btnListForeColor.Image = null;
            manager.ApplyResources(this.btnListForeColor, "btnListForeColor");
            this.btnListForeColor.Name = "btnListForeColor";
            this.btnListForeColor.UseVisualStyleBackColor = true;
            this.btnListForeColor.ColorChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(label2, "lblListForeColor");
            label2.Name = "lblListForeColor";
            this.btnListBackColor.Image = null;
            manager.ApplyResources(this.btnListBackColor, "btnListBackColor");
            this.btnListBackColor.Name = "btnListBackColor";
            this.btnListBackColor.UseVisualStyleBackColor = true;
            this.btnListBackColor.ColorChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.chkApplyToChildren, "chkApplyToChildren");
            this.chkApplyToChildren.Name = "chkApplyToChildren";
            this.chkApplyToChildren.UseVisualStyleBackColor = true;
            this.chkApplyToChildren.CheckedChanged += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.imgViews.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgViews, "imgViews");
            this.imgViews.TransparentColor = Color.Transparent;
            value2.DataObject = this.chkCustomizeFilter;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkCustomizeSort;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkCustomizeView;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkCustomizeColumns;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkCustomizeIcon;
            value6.PropertyName = "Checked";
            value7.DataObject = this.btnListBackColor;
            value7.PropertyName = "Color";
            value8.DataObject = this.btnListForeColor;
            value8.PropertyName = "Color";
            value9.DataObject = this.chkApplyToChildren;
            value9.PropertyName = "Checked";
            this.CheckWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7, value8, value9 });
            this.BrowseIconDialog.AddExtension = false;
            manager.ApplyResources(this.BrowseIconDialog, "BrowseIconDialog");
            manager.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.lblFolder, 0, 0);
            this.tlpBack.Controls.Add(this.chkApplyToChildren, 0, 5);
            this.tlpBack.Controls.Add(box2, 0, 2);
            this.tlpBack.Controls.Add(this.tsFolder, 0, 1);
            this.tlpBack.Controls.Add(box, 0, 4);
            this.tlpBack.Controls.Add(box3, 0, 3);
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.lblFolder, "lblFolder");
            this.lblFolder.Name = "lblFolder";
            this.tsFolder.BackColor = SystemColors.ButtonFace;
            manager.ApplyResources(this.tsFolder, "tsFolder");
            this.tsFolder.GripStyle = ToolStripGripStyle.Hidden;
            this.tsFolder.MinimumSize = new Size(0, 0x19);
            this.tsFolder.Name = "tsFolder";
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnClear, 2, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 3, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpBack);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CustomizeFolderDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.UpdateButtons);
            box.ResumeLayout(false);
            box.PerformLayout();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            box2.ResumeLayout(false);
            box2.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            this.pnlIcon.ResumeLayout(false);
            ((ISupportInitialize) this.picIcon).EndInit();
            box3.ResumeLayout(false);
            box3.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((ISupportInitialize) this.CheckWatcher).EndInit();
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool InitializeViewIcons(params string[] iconNameList)
        {
            foreach (string str in iconNameList)
            {
                Image image = IconSet.GetImage(str);
                if (image == null)
                {
                    return false;
                }
                this.imgViews.Images.Add(image);
            }
            for (int i = 0; i < iconNameList.Length; i++)
            {
                this.imgViews.Images.Add(ToolStripRenderer.CreateDisabledImage(this.imgViews.Images[i]));
            }
            return true;
        }

        private void OnChanged(EventArgs e)
        {
            this.FChanged = true;
            this.UpdateButtons(this, e);
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            if (Application.RenderWithVisualStyles)
            {
                this.btnListBackColor.UseVisualStyleBackColor = true;
                this.btnListForeColor.UseVisualStyleBackColor = true;
            }
            else
            {
                this.btnListBackColor.BackColor = SystemColors.Control;
                this.btnListForeColor.BackColor = SystemColors.Control;
            }
            base.OnThemeChanged(e);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Height) > BoundsSpecified.None) && (factor.Height != 1.0))
            {
                this.cmbView.ItemHeight = (int) Math.Round((double) (this.cmbView.ItemHeight * factor.Height));
            }
        }

        private void txtIconLocation_Enter(object sender, EventArgs e)
        {
            this.txtIconLocation.Tag = this.txtIconLocation.Text;
        }

        private void txtIconLocation_Validated(object sender, EventArgs e)
        {
            if (!string.Equals(this.txtIconLocation.Text, this.txtIconLocation.Tag as string))
            {
                this.UpdateIcon(this.txtIconLocation.Text);
                this.OnChanged(EventArgs.Empty);
            }
            this.txtIconLocation.Tag = null;
        }

        private void UpdateButtons(object sender, EventArgs e)
        {
            this.btnCustomizeFilter.Enabled = this.chkCustomizeFilter.Checked;
            this.btnCustomizeSort.Enabled = this.chkCustomizeSort.Checked;
            this.cmbView.Enabled = this.chkCustomizeView.Checked;
            this.btnCustomizeColumns.Enabled = this.chkCustomizeColumns.Checked;
            this.btnOk.Enabled = (this.CheckWatcher.AnyValueChanged || this.FChanged) || (this.chkCustomizeView.Checked && !this.cmbView.SelectedItem.Equals(this.FFolderView));
        }

        public void UpdateCustomizeFolder(ICustomizeFolder customize)
        {
            if ((customize.UpdatableParts & CustomizeFolderParts.Colors) > 0)
            {
                customize.BackColor = this.btnListBackColor.Color;
                customize.ForeColor = this.btnListForeColor.Color;
            }
            else
            {
                customize.BackColor = Color.Empty;
                customize.ForeColor = Color.Empty;
            }
            if ((customize.UpdatableParts & CustomizeFolderParts.Filter) > 0)
            {
                customize.Filter = this.chkCustomizeFilter.Checked ? (this.btnCustomizeFilter.Tag as IVirtualItemFilter) : null;
            }
            if ((customize.UpdatableParts & CustomizeFolderParts.Sort) > 0)
            {
                customize.Sort = this.chkCustomizeSort.Checked ? (this.btnCustomizeSort.Tag as VirtualItemComparer) : null;
            }
            if ((customize.UpdatableParts & CustomizeFolderParts.View) > 0)
            {
                customize.View = this.chkCustomizeView.Checked ? ((PanelView?) this.cmbView.SelectedItem) : null;
            }
            CustomizeFolderParts parts = CustomizeFolderParts.ListColumnCount | CustomizeFolderParts.View;
            if (((((customize.UpdatableParts & parts) == parts) && customize.View.HasValue) && (((PanelView) customize.View.Value) == PanelView.List)) && (this.FListColumnCount > 0))
            {
                customize.ListColumnCount = new int?(this.FListColumnCount);
            }
            else
            {
                customize.ListColumnCount = null;
            }
            CustomizeFolderParts parts2 = CustomizeFolderParts.ThumbnailSize | CustomizeFolderParts.View;
            if ((((customize.UpdatableParts & parts2) == parts2) && customize.View.HasValue) && (((PanelView) customize.View.Value) == PanelView.Thumbnail))
            {
                customize.ThumbnailSize = this.FThumbnailSize;
                customize.ThumbnailSpacing = this.FThumbnailSpacing;
            }
            else
            {
                customize.ThumbnailSize = Size.Empty;
                customize.ThumbnailSpacing = Size.Empty;
            }
            if ((customize.UpdatableParts & CustomizeFolderParts.Columns) > 0)
            {
                customize.AutoSizeColumns = this.chkCustomizeColumns.Checked ? new bool?(this.FAutoSizeColumns) : null;
                IEnumerable<ListViewColumnInfo> tag = this.btnCustomizeColumns.Tag as IEnumerable<ListViewColumnInfo>;
                if (this.chkCustomizeColumns.Checked && (tag != null))
                {
                    customize.Columns = new List<ListViewColumnInfo>(tag).ToArray();
                }
                else
                {
                    customize.Columns = null;
                }
            }
            if ((customize.UpdatableParts & CustomizeFolderParts.Icon) > 0)
            {
                if (this.chkCustomizeIcon.Checked && this.UpdateIcon(this.txtIconLocation.Text))
                {
                    customize.Icon = IconLocation.TryParse(this.txtIconLocation.Text);
                }
                else
                {
                    customize.Icon = null;
                }
            }
        }

        private bool UpdateIcon(string iconPath)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(iconPath))
            {
                IconLocation location = IconLocation.TryParse(iconPath);
                flag = (location == null) || !File.Exists(Environment.ExpandEnvironmentVariables(location.IconFileName));
                if (!flag)
                {
                    Image image = CustomImageProvider.LoadIcon(Environment.ExpandEnvironmentVariables(location.IconFileName), location.IconIndex, new Size(0x20, 0x20));
                    if (image != null)
                    {
                        this.picIcon.Image = image;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                this.txtIconLocation.BackColor = Settings.TextBoxError;
                this.txtIconLocation.ForeColor = SystemColors.HighlightText;
                this.picIcon.Image = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, new Size(0x20, 0x20));
            }
            else
            {
                this.txtIconLocation.ResetBackColor();
                this.txtIconLocation.ResetForeColor();
            }
            return !flag;
        }

        public bool ApplyToChildren
        {
            get
            {
                return (this.chkApplyToChildren.Enabled && this.chkApplyToChildren.Checked);
            }
            set
            {
                this.chkApplyToChildren.Checked = value;
            }
        }

        public bool ApplyToChildrenEnabled
        {
            get
            {
                return this.chkApplyToChildren.Visible;
            }
            set
            {
                this.chkApplyToChildren.Enabled = value;
                this.chkApplyToChildren.Visible = value;
            }
        }
    }
}

