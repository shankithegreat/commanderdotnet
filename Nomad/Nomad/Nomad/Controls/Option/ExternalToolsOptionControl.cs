namespace Nomad.Controls.Option
{
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.FileSystem.LocalFile;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ExternalToolsOptionControl : UserControl, IPersistComponentSettings
    {
        private OpenFileDialog BrowsePathDialog;
        private Button btnAppendArgument;
        private Button btnAppendWorkDir;
        private Button btnBrowsePath;
        private Button btnDelete;
        private Button btnDown;
        private Button btnSaveAs;
        private Button btnSort;
        private Button btnUp;
        private CheckBox chkRunAsAdmin;
        private ComboBox cmbWindowState;
        private ContextMenuStrip cmsArguments;
        private ContextMenuStrip cmsWorkingDirectory;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private IContainer components;
        private ListViewItem CurrentItem;
        private NamedShellLink CurrentLink;
        private ImageList imgTools;
        private Label lblArguments;
        private Label lblCommand;
        private Label lblHotkey;
        private Label lblTitle;
        private Label lblWindowState;
        private Label lblWorkingDirectory;
        private ListViewEx lvTools;
        private TableLayoutPanel tlpBack;
        private List<string> ToolHashList;
        private ToolStripMenuItem tsmiCurrentFolder;
        private ToolStripMenuItem tsmiCurrentFolder2;
        private ToolStripMenuItem tsmiCurrentItemName;
        private ToolStripMenuItem tsmiCurrentItemPath;
        private ToolStripMenuItem tsmiCurrentSelectionName;
        private ToolStripMenuItem tsmiCurrentSelectionPath;
        private ToolStripMenuItem tsmiFarFolder;
        private ToolStripMenuItem tsmiFarFolder2;
        private ToolStripMenuItem tsmiFarItemName;
        private ToolStripMenuItem tsmiFarItemPath;
        private ToolStripMenuItem tsmiFarSelectionName;
        private ToolStripMenuItem tsmiFarSelectionPath;
        private ToolStripMenuItem tsmiNomadFolder;
        private ToolStripMenuItem tsmiNomadFolder2;
        private ToolStripMenuItem tsmiUserValue;
        private ToolStripSeparator tssArguments4;
        private ToolStripSeparator tssArguments5;
        private TextBox txtArguments;
        private HotKeyBox txtHotKey;
        private TextBox txtPath;
        private TextBox txtTitle;
        private TextBox txtWorkingDirectory;
        private ValidatorProvider Validator;

        public ExternalToolsOptionControl()
        {
            EventHandler handler = null;
            this.CurrentLink = new NamedShellLink();
            this.components = null;
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.InitializeEnvironmentKeys();
            this.btnSaveAs.Image = IconSet.GetImage("SaveAs");
            if (this.lvTools.ExplorerTheme)
            {
                this.imgTools.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
            }
            else
            {
                this.imgTools.ImageSize = ImageHelper.DefaultSmallIconSize;
            }
            if (!Application.RenderWithVisualStyles)
            {
                this.btnSaveAs.BackColor = SystemColors.Control;
                this.btnAppendArgument.BackColor = SystemColors.Control;
                this.btnAppendWorkDir.BackColor = SystemColors.Control;
            }
            this.txtHotKey.KeysConverter = new SettingsManager.LocalizedEnumConverter(typeof(Keys));
            this.cmbWindowState.DataSource = Enum.GetValues(typeof(FormWindowState));
            this.chkRunAsAdmin.Enabled = !Settings.Default.DirectToolStart;
            if (handler == null)
            {
                handler = delegate (object sender, EventArgs e) {
                    foreach (ListViewItem item in this.lvTools.Items)
                    {
                        IDisposable tag = item.Tag as IDisposable;
                        if (tag != null)
                        {
                            tag.Dispose();
                        }
                    }
                    this.CurrentLink.Dispose();
                };
            }
            base.Disposed += handler;
        }

        private void btnAppendArgument_Click(object sender, EventArgs e)
        {
            this.cmsArguments.Show(this.btnAppendArgument, 0, this.btnAppendArgument.Height + 1);
        }

        private void btnAppendWorkDir_Click(object sender, EventArgs e)
        {
            this.cmsWorkingDirectory.Show(this.btnAppendWorkDir, 0, this.btnAppendWorkDir.Height + 1);
        }

        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            if (this.BrowsePathDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.BrowsePathDialog.FileName;
                this.ValidateChildren(ValidationConstraints.Enabled);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvTools.FocusedItem;
            if (focusedItem != null)
            {
                focusedItem.Delete(true);
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvTools.FocusedItem;
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

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren(ValidationConstraints.Enabled))
            {
                TypeConverter keysConverter = TypeDescriptor.GetConverter(typeof(Keys));
                this.lvTools.BeginUpdate();
                try
                {
                    ListViewItem item = null;
                    foreach (ListViewItem item2 in this.lvTools.Items)
                    {
                        if (string.Equals(item2.Text, this.CurrentLink.Name))
                        {
                            item = item2;
                            break;
                        }
                    }
                    Image img = null;
                    if (item == null)
                    {
                        item = this.CreateToolItem(this.CurrentLink, keysConverter);
                        img = ImageProvider.Default.GetFileIcon(this.CurrentLink.Path, ImageHelper.DefaultSmallIconSize);
                        this.lvTools.Items.Add(item);
                    }
                    else
                    {
                        if (!string.Equals(((NamedShellLink) item.Tag).Path, this.CurrentLink.Path))
                        {
                            img = ImageProvider.Default.GetFileIcon(this.CurrentLink.Path, ImageHelper.DefaultSmallIconSize);
                        }
                        item.SubItems[1].Text = keysConverter.ConvertToString(this.CurrentLink.Hotkey);
                        item.Tag = this.CurrentLink;
                    }
                    if (img != null)
                    {
                        item.ImageIndex = this.imgTools.AddNormalized(img, this.lvTools.BackColor);
                    }
                    item.Focus(true, true);
                }
                finally
                {
                    this.lvTools.EndUpdate();
                }
                this.CurrentLink = (NamedShellLink) this.CurrentLink.Clone();
                this.UpdateButtons();
                this.ListView_SizeChanged(this.lvTools);
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvTools.FocusedItem;
            this.lvTools.Sorting = SortOrder.Ascending;
            this.lvTools.Sort();
            this.lvTools.Sorting = SortOrder.None;
            if (focusedItem != null)
            {
                focusedItem.Focus(true, false);
            }
        }

        private void chkRunAsAdmin_Click(object sender, EventArgs e)
        {
            this.CurrentLink.SetFlags(SHELL_LINK_DATA_FLAGS.SLDF_RUNAS_USER, this.chkRunAsAdmin.Checked);
            this.UpdateButtons();
        }

        private void cmbWindowState_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.CurrentLink.WindowState = (FormWindowState) this.cmbWindowState.SelectedItem;
            this.UpdateButtons();
        }

        private ListViewItem CreateToolItem(NamedShellLink tool, TypeConverter keysConverter)
        {
            ListViewItem item = new ListViewItem(tool.Name) {
                Tag = tool
            };
            ListViewItem.ListViewSubItem item2 = item.SubItems.Add(keysConverter.ConvertToString(tool.Hotkey));
            if (tool.Hotkey == Keys.None)
            {
                item.UseItemStyleForSubItems = false;
                item2.ForeColor = SystemColors.GrayText;
            }
            return item;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ExternalToolsOptionControl));
            this.imgTools = new ImageList(this.components);
            this.BrowsePathDialog = new OpenFileDialog();
            this.tlpBack = new TableLayoutPanel();
            this.txtPath = new TextBox();
            this.lblWindowState = new Label();
            this.txtArguments = new TextBox();
            this.btnAppendWorkDir = new Button();
            this.lblWorkingDirectory = new Label();
            this.btnAppendArgument = new Button();
            this.btnBrowsePath = new Button();
            this.lblHotkey = new Label();
            this.txtWorkingDirectory = new TextBox();
            this.cmbWindowState = new ComboBox();
            this.lblCommand = new Label();
            this.lblArguments = new Label();
            this.txtHotKey = new HotKeyBox();
            this.lvTools = new ListViewEx();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.btnUp = new Button();
            this.btnDown = new Button();
            this.btnSort = new Button();
            this.btnDelete = new Button();
            this.btnSaveAs = new Button();
            this.lblTitle = new Label();
            this.txtTitle = new TextBox();
            this.chkRunAsAdmin = new CheckBox();
            this.cmsArguments = new ContextMenuStrip(this.components);
            this.tsmiCurrentItemName = new ToolStripMenuItem();
            this.tsmiCurrentItemPath = new ToolStripMenuItem();
            this.tsmiCurrentSelectionName = new ToolStripMenuItem();
            this.tsmiCurrentSelectionPath = new ToolStripMenuItem();
            this.tsmiFarItemName = new ToolStripMenuItem();
            this.tsmiFarItemPath = new ToolStripMenuItem();
            this.tsmiFarSelectionName = new ToolStripMenuItem();
            this.tsmiFarSelectionPath = new ToolStripMenuItem();
            this.tssArguments4 = new ToolStripSeparator();
            this.tsmiCurrentFolder = new ToolStripMenuItem();
            this.tsmiFarFolder = new ToolStripMenuItem();
            this.tsmiNomadFolder = new ToolStripMenuItem();
            this.tssArguments5 = new ToolStripSeparator();
            this.tsmiUserValue = new ToolStripMenuItem();
            this.cmsWorkingDirectory = new ContextMenuStrip(this.components);
            this.tsmiCurrentFolder2 = new ToolStripMenuItem();
            this.tsmiFarFolder2 = new ToolStripMenuItem();
            this.tsmiNomadFolder2 = new ToolStripMenuItem();
            this.Validator = new ValidatorProvider();
            ToolStripSeparator separator = new ToolStripSeparator();
            ToolStripSeparator separator2 = new ToolStripSeparator();
            ToolStripSeparator separator3 = new ToolStripSeparator();
            this.tlpBack.SuspendLayout();
            this.cmsArguments.SuspendLayout();
            this.cmsWorkingDirectory.SuspendLayout();
            base.SuspendLayout();
            this.imgTools.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgTools, "imgTools");
            this.imgTools.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.BrowsePathDialog, "BrowsePathDialog");
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.txtPath, 1, 6);
            this.tlpBack.Controls.Add(this.lblWindowState, 0, 11);
            this.tlpBack.Controls.Add(this.txtArguments, 1, 7);
            this.tlpBack.Controls.Add(this.btnAppendWorkDir, 3, 8);
            this.tlpBack.Controls.Add(this.lblWorkingDirectory, 0, 8);
            this.tlpBack.Controls.Add(this.btnAppendArgument, 3, 7);
            this.tlpBack.Controls.Add(this.btnBrowsePath, 3, 6);
            this.tlpBack.Controls.Add(this.lblHotkey, 0, 10);
            this.tlpBack.Controls.Add(this.txtWorkingDirectory, 1, 8);
            this.tlpBack.Controls.Add(this.cmbWindowState, 1, 11);
            this.tlpBack.Controls.Add(this.lblCommand, 0, 6);
            this.tlpBack.Controls.Add(this.lblArguments, 0, 7);
            this.tlpBack.Controls.Add(this.txtHotKey, 1, 10);
            this.tlpBack.Controls.Add(this.lvTools, 0, 0);
            this.tlpBack.Controls.Add(this.btnUp, 3, 0);
            this.tlpBack.Controls.Add(this.btnDown, 3, 1);
            this.tlpBack.Controls.Add(this.btnSort, 3, 2);
            this.tlpBack.Controls.Add(this.btnDelete, 3, 3);
            this.tlpBack.Controls.Add(this.btnSaveAs, 3, 5);
            this.tlpBack.Controls.Add(this.lblTitle, 0, 5);
            this.tlpBack.Controls.Add(this.txtTitle, 1, 5);
            this.tlpBack.Controls.Add(this.chkRunAsAdmin, 1, 9);
            this.tlpBack.Name = "tlpBack";
            this.tlpBack.SetColumnSpan(this.txtPath, 2);
            manager.ApplyResources(this.txtPath, "txtPath");
            this.txtPath.Name = "txtPath";
            this.Validator.SetValidateOn(this.txtPath, ValidateOn.TextChangedTimer);
            this.txtPath.Validating += new CancelEventHandler(this.txtPath_Validating);
            manager.ApplyResources(this.lblWindowState, "lblWindowState");
            this.lblWindowState.Name = "lblWindowState";
            this.tlpBack.SetColumnSpan(this.txtArguments, 2);
            manager.ApplyResources(this.txtArguments, "txtArguments");
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.TextChanged += new EventHandler(this.txtArguments_TextChanged);
            manager.ApplyResources(this.btnAppendWorkDir, "btnAppendWorkDir");
            this.btnAppendWorkDir.Image = Resources.SmallDownArrow;
            this.btnAppendWorkDir.Name = "btnAppendWorkDir";
            this.btnAppendWorkDir.UseVisualStyleBackColor = true;
            this.btnAppendWorkDir.Click += new EventHandler(this.btnAppendWorkDir_Click);
            manager.ApplyResources(this.lblWorkingDirectory, "lblWorkingDirectory");
            this.lblWorkingDirectory.Name = "lblWorkingDirectory";
            manager.ApplyResources(this.btnAppendArgument, "btnAppendArgument");
            this.btnAppendArgument.Image = Resources.SmallDownArrow;
            this.btnAppendArgument.Name = "btnAppendArgument";
            this.btnAppendArgument.UseVisualStyleBackColor = true;
            this.btnAppendArgument.Click += new EventHandler(this.btnAppendArgument_Click);
            manager.ApplyResources(this.btnBrowsePath, "btnBrowsePath");
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.UseVisualStyleBackColor = true;
            this.btnBrowsePath.Click += new EventHandler(this.btnBrowsePath_Click);
            manager.ApplyResources(this.lblHotkey, "lblHotkey");
            this.lblHotkey.Name = "lblHotkey";
            this.tlpBack.SetColumnSpan(this.txtWorkingDirectory, 2);
            manager.ApplyResources(this.txtWorkingDirectory, "txtWorkingDirectory");
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.TextChanged += new EventHandler(this.txtWorkingDirectory_TextChanged);
            manager.ApplyResources(this.cmbWindowState, "cmbWindowState");
            this.cmbWindowState.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbWindowState.FormattingEnabled = true;
            this.cmbWindowState.Name = "cmbWindowState";
            this.cmbWindowState.SelectionChangeCommitted += new EventHandler(this.cmbWindowState_SelectionChangeCommitted);
            manager.ApplyResources(this.lblCommand, "lblCommand");
            this.lblCommand.Name = "lblCommand";
            manager.ApplyResources(this.lblArguments, "lblArguments");
            this.lblArguments.Name = "lblArguments";
            manager.ApplyResources(this.txtHotKey, "txtHotKey");
            this.txtHotKey.Name = "txtHotKey";
            this.txtHotKey.HotKeyChanged += new EventHandler(this.txtHotKey_HotKeyChanged);
            this.txtHotKey.PreviewHotKey += new EventHandler<PreviewHotKeyEventArgs>(this.txtHotKey_PreviewHotKey);
            this.lvTools.AllowDrop = true;
            this.lvTools.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.tlpBack.SetColumnSpan(this.lvTools, 3);
            this.lvTools.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.lvTools, "lvTools");
            this.lvTools.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.lvTools.FullRowSelect = true;
            this.lvTools.HeaderStyle = ColumnHeaderStyle.None;
            this.lvTools.HideSelection = false;
            this.lvTools.MultiSelect = false;
            this.lvTools.Name = "lvTools";
            this.tlpBack.SetRowSpan(this.lvTools, 5);
            this.lvTools.ShowColumnLines = true;
            this.lvTools.SmallImageList = this.imgTools;
            this.lvTools.SortColumn = 0;
            this.lvTools.UseCompatibleStateImageBehavior = false;
            this.lvTools.View = View.Details;
            this.lvTools.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvTools_ItemSelectionChanged);
            this.lvTools.KeyDown += new KeyEventHandler(this.lvTools_KeyDown);
            this.lvTools.ItemDrag += new ItemDragEventHandler(this.lvTools_ItemDrag);
            this.lvTools.SizeChanged += new EventHandler(this.ListView_SizeChanged);
            manager.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnMove_Click);
            manager.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnMove_Click);
            manager.ApplyResources(this.btnSort, "btnSort");
            this.btnSort.Name = "btnSort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new EventHandler(this.btnSort_Click);
            manager.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            manager.ApplyResources(this.btnSaveAs, "btnSaveAs");
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new EventHandler(this.btnSaveAs_Click);
            manager.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            this.tlpBack.SetColumnSpan(this.txtTitle, 2);
            manager.ApplyResources(this.txtTitle, "txtTitle");
            this.txtTitle.Name = "txtTitle";
            this.Validator.SetValidateOn(this.txtTitle, ValidateOn.TextChanged);
            this.txtTitle.Validated += new EventHandler(this.txtTitle_Validated);
            this.txtTitle.Validating += new CancelEventHandler(this.txtTitle_Validating);
            manager.ApplyResources(this.chkRunAsAdmin, "chkRunAsAdmin");
            this.tlpBack.SetColumnSpan(this.chkRunAsAdmin, 3);
            this.chkRunAsAdmin.Name = "chkRunAsAdmin";
            this.chkRunAsAdmin.UseVisualStyleBackColor = true;
            this.chkRunAsAdmin.Click += new EventHandler(this.chkRunAsAdmin_Click);
            separator.Name = "tssArguments1";
            manager.ApplyResources(separator, "tssArguments1");
            separator2.Name = "tssArguments2";
            manager.ApplyResources(separator2, "tssArguments2");
            separator3.Name = "tssArguments3";
            manager.ApplyResources(separator3, "tssArguments3");
            this.cmsArguments.Items.AddRange(new ToolStripItem[] { 
                this.tsmiCurrentItemName, this.tsmiCurrentItemPath, separator, this.tsmiCurrentSelectionName, this.tsmiCurrentSelectionPath, separator2, this.tsmiFarItemName, this.tsmiFarItemPath, separator3, this.tsmiFarSelectionName, this.tsmiFarSelectionPath, this.tssArguments4, this.tsmiCurrentFolder, this.tsmiFarFolder, this.tsmiNomadFolder, this.tssArguments5, 
                this.tsmiUserValue
             });
            this.cmsArguments.Name = "cmsArguments";
            manager.ApplyResources(this.cmsArguments, "cmsArguments");
            this.tsmiCurrentItemName.Name = "tsmiCurrentItemName";
            manager.ApplyResources(this.tsmiCurrentItemName, "tsmiCurrentItemName");
            this.tsmiCurrentItemName.Tag = "%curitemname%";
            this.tsmiCurrentItemName.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiCurrentItemPath.Name = "tsmiCurrentItemPath";
            manager.ApplyResources(this.tsmiCurrentItemPath, "tsmiCurrentItemPath");
            this.tsmiCurrentItemPath.Tag = "%curitempath%";
            this.tsmiCurrentItemPath.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiCurrentSelectionName.Name = "tsmiCurrentSelectionName";
            manager.ApplyResources(this.tsmiCurrentSelectionName, "tsmiCurrentSelectionName");
            this.tsmiCurrentSelectionName.Tag = "%curselname%";
            this.tsmiCurrentSelectionName.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiCurrentSelectionPath.Name = "tsmiCurrentSelectionPath";
            manager.ApplyResources(this.tsmiCurrentSelectionPath, "tsmiCurrentSelectionPath");
            this.tsmiCurrentSelectionPath.Tag = "%curselpath%";
            this.tsmiCurrentSelectionPath.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiFarItemName.Name = "tsmiFarItemName";
            manager.ApplyResources(this.tsmiFarItemName, "tsmiFarItemName");
            this.tsmiFarItemName.Tag = "%faritemname%";
            this.tsmiFarItemName.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiFarItemPath.Name = "tsmiFarItemPath";
            manager.ApplyResources(this.tsmiFarItemPath, "tsmiFarItemPath");
            this.tsmiFarItemPath.Tag = "%faritempath%";
            this.tsmiFarItemPath.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiFarSelectionName.Name = "tsmiFarSelectionName";
            manager.ApplyResources(this.tsmiFarSelectionName, "tsmiFarSelectionName");
            this.tsmiFarSelectionName.Tag = "%farselname%";
            this.tsmiFarSelectionName.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiFarSelectionPath.Name = "tsmiFarSelectionPath";
            manager.ApplyResources(this.tsmiFarSelectionPath, "tsmiFarSelectionPath");
            this.tsmiFarSelectionPath.Tag = "%farselpath%";
            this.tsmiFarSelectionPath.Click += new EventHandler(this.tsmiArgument_Click);
            this.tssArguments4.Name = "tssArguments4";
            manager.ApplyResources(this.tssArguments4, "tssArguments4");
            this.tsmiCurrentFolder.Name = "tsmiCurrentFolder";
            manager.ApplyResources(this.tsmiCurrentFolder, "tsmiCurrentFolder");
            this.tsmiCurrentFolder.Tag = "%curdir%";
            this.tsmiCurrentFolder.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiFarFolder.Name = "tsmiFarFolder";
            manager.ApplyResources(this.tsmiFarFolder, "tsmiFarFolder");
            this.tsmiFarFolder.Tag = "%fardir%";
            this.tsmiFarFolder.Click += new EventHandler(this.tsmiArgument_Click);
            this.tsmiNomadFolder.Name = "tsmiNomadFolder";
            manager.ApplyResources(this.tsmiNomadFolder, "tsmiNomadFolder");
            this.tsmiNomadFolder.Tag = "%nomaddir%";
            this.tsmiNomadFolder.Click += new EventHandler(this.tsmiArgument_Click);
            this.tssArguments5.Name = "tssArguments5";
            manager.ApplyResources(this.tssArguments5, "tssArguments5");
            this.tsmiUserValue.Name = "tsmiUserValue";
            manager.ApplyResources(this.tsmiUserValue, "tsmiUserValue");
            this.tsmiUserValue.Tag = "%user%";
            this.tsmiUserValue.Click += new EventHandler(this.tsmiArgument_Click);
            this.cmsWorkingDirectory.Items.AddRange(new ToolStripItem[] { this.tsmiCurrentFolder2, this.tsmiFarFolder2, this.tsmiNomadFolder2 });
            this.cmsWorkingDirectory.Name = "cmsWorkingDirectory";
            manager.ApplyResources(this.cmsWorkingDirectory, "cmsWorkingDirectory");
            this.tsmiCurrentFolder2.Name = "tsmiCurrentFolder2";
            manager.ApplyResources(this.tsmiCurrentFolder2, "tsmiCurrentFolder2");
            this.tsmiCurrentFolder2.Tag = "%curdir%";
            this.tsmiCurrentFolder2.Click += new EventHandler(this.tsmiWorkingDirectory_Click);
            this.tsmiFarFolder2.Name = "tsmiFarFolder2";
            manager.ApplyResources(this.tsmiFarFolder2, "tsmiFarFolder2");
            this.tsmiFarFolder2.Tag = "%fardir%";
            this.tsmiFarFolder2.Click += new EventHandler(this.tsmiWorkingDirectory_Click);
            this.tsmiNomadFolder2.Name = "tsmiNomadFolder2";
            manager.ApplyResources(this.tsmiNomadFolder2, "tsmiNomadFolder2");
            this.tsmiNomadFolder2.Tag = "%nomaddir%";
            this.tsmiNomadFolder2.Click += new EventHandler(this.tsmiWorkingDirectory_Click);
            this.Validator.Owner = this;
            this.Validator.TimerInterval = 250;
            this.Validator.ValidatingControl += new EventHandler<ValidatingControlEventArgs>(this.Validator_ValidatingControl);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.Controls.Add(this.tlpBack);
            base.Name = "ExternalToolsOptionControl";
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.cmsArguments.ResumeLayout(false);
            this.cmsWorkingDirectory.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitializeEnvironmentKeys()
        {
            this.tsmiCurrentItemName.Tag = SettingsManager.EnvironmentVariables.GetVarName("curitemname");
            this.tsmiCurrentItemPath.Tag = SettingsManager.EnvironmentVariables.GetVarName("curitempath");
            this.tsmiCurrentSelectionName.Tag = SettingsManager.EnvironmentVariables.GetVarName("curselname");
            this.tsmiCurrentSelectionPath.Tag = SettingsManager.EnvironmentVariables.GetVarName("curselpath");
            this.tsmiFarItemName.Tag = SettingsManager.EnvironmentVariables.GetVarName("faritemname");
            this.tsmiFarItemPath.Tag = SettingsManager.EnvironmentVariables.GetVarName("faritempath");
            this.tsmiFarSelectionName.Tag = SettingsManager.EnvironmentVariables.GetVarName("farselname");
            this.tsmiFarSelectionPath.Tag = SettingsManager.EnvironmentVariables.GetVarName("farselpath");
            this.tsmiCurrentFolder.Tag = SettingsManager.EnvironmentVariables.GetVarName("curdir");
            this.tsmiFarFolder.Tag = SettingsManager.EnvironmentVariables.GetVarName("fardir");
            this.tsmiNomadFolder.Tag = SettingsManager.EnvironmentVariables.GetVarName("nomaddir");
            this.tsmiUserValue.Tag = SettingsManager.EnvironmentVariables.GetVarName("user");
            this.tsmiCurrentFolder2.Tag = this.tsmiCurrentFolder.Tag;
            this.tsmiFarFolder2.Tag = this.tsmiFarFolder.Tag;
            this.tsmiNomadFolder2.Tag = this.tsmiNomadFolder.Tag;
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
            if (Directory.Exists(SettingsManager.SpecialFolders.Tools))
            {
                TypeConverter keysConverter = TypeDescriptor.GetConverter(typeof(Keys));
                this.ToolHashList = new List<string>();
                List<NamedShellLink> list = new List<NamedShellLink>();
                foreach (string str in Directory.GetFiles(SettingsManager.SpecialFolders.Tools, "*.lnk"))
                {
                    list.Add(new NamedShellLink(str));
                }
                list.Sort(delegate (NamedShellLink link1, NamedShellLink link2) {
                    return link1.Index - link2.Index;
                });
                this.lvTools.BeginUpdate();
                try
                {
                    this.lvTools.Items.Clear();
                    foreach (NamedShellLink link in list)
                    {
                        this.ToolHashList.Add(link.GenerateHash());
                        ListViewItem item = this.CreateToolItem(link, keysConverter);
                        Image fileIcon = ImageProvider.Default.GetFileIcon(link.OriginalName, ImageHelper.DefaultSmallIconSize);
                        item.ImageIndex = this.imgTools.AddNormalized(fileIcon, this.lvTools.BackColor);
                        this.lvTools.Items.Add(item);
                    }
                }
                finally
                {
                    this.lvTools.EndUpdate();
                }
                this.UpdateButtons();
                this.ListView_SizeChanged(this.lvTools);
            }
        }

        private void lvTools_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.lvTools.DoDragMove((ListViewItem) e.Item);
        }

        private void lvTools_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.Focused)
            {
                e.Item.Selected = true;
                if ((this.CurrentItem == null) || (this.CurrentItem != e.Item))
                {
                    this.CurrentItem = e.Item;
                    this.CurrentLink = (NamedShellLink) ((NamedShellLink) this.CurrentItem.Tag).Clone();
                    this.txtTitle.Text = this.CurrentLink.Name;
                    this.txtPath.Text = this.CurrentLink.Path;
                    this.txtArguments.Text = this.CurrentLink.Arguments;
                    this.txtWorkingDirectory.Text = this.CurrentLink.WorkingDirectory;
                    this.txtPath.ResetBackColor();
                    this.txtPath.ResetForeColor();
                    this.txtHotKey.HotKey = this.CurrentLink.Hotkey;
                    this.cmbWindowState.SelectedItem = this.CurrentLink.WindowState;
                    this.chkRunAsAdmin.Checked = this.chkRunAsAdmin.Enabled && ((this.CurrentLink.Flags & SHELL_LINK_DATA_FLAGS.SLDF_RUNAS_USER) > ((SHELL_LINK_DATA_FLAGS) 0));
                }
            }
            base.BeginInvoke(new MethodInvoker(this.UpdateButtons));
        }

        private void lvTools_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Add))
            {
                e.SuppressKeyPress = true;
            }
            else
            {
                ListViewItem focusedItem = this.lvTools.FocusedItem;
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

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            if (Directory.Exists(SettingsManager.SpecialFolders.Tools))
            {
                foreach (string str in Directory.GetFiles(SettingsManager.SpecialFolders.Tools, "*.lnk"))
                {
                    File.Delete(str);
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Deleted, str);
                }
            }
            else
            {
                Directory.CreateDirectory(SettingsManager.SpecialFolders.Tools);
                LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, SettingsManager.SpecialFolders.Tools);
            }
            for (int i = 0; i < this.lvTools.Items.Count; i++)
            {
                NamedShellLink tag = (NamedShellLink) this.lvTools.Items[i].Tag;
                tag.Index = i + 1;
                str = tag.Name + ".lnk";
                string fileName = Path.Combine(SettingsManager.SpecialFolders.Tools, str);
                tag.Save(fileName);
                LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, fileName);
            }
        }

        private void tsmiArgument_Click(object sender, EventArgs e)
        {
            this.txtArguments.Text = this.txtArguments.Text + ((string) ((ToolStripItem) sender).Tag);
        }

        private void tsmiWorkingDirectory_Click(object sender, EventArgs e)
        {
            this.txtWorkingDirectory.Text = this.txtWorkingDirectory.Text + ((string) ((ToolStripItem) sender).Tag);
        }

        private void txtArguments_TextChanged(object sender, EventArgs e)
        {
            this.CurrentLink.Arguments = this.txtArguments.Text;
            this.UpdateButtons();
        }

        private void txtHotKey_HotKeyChanged(object sender, EventArgs e)
        {
            if (this.txtHotKey.HotKey != this.CurrentLink.Hotkey)
            {
                this.CurrentLink.Hotkey = this.txtHotKey.HotKey;
                this.UpdateButtons();
            }
        }

        private void txtHotKey_PreviewHotKey(object sender, PreviewHotKeyEventArgs e)
        {
            if (MainForm.Instance.KeyMap.ContainsKey(e.HotKey))
            {
                e.Cancel = true;
            }
        }

        private void txtPath_Validating(object sender, CancelEventArgs e)
        {
            string text = this.txtPath.Text;
            if (string.IsNullOrEmpty(text))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = text.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
                else
                {
                    try
                    {
                        this.CurrentLink.Path = this.txtPath.Text;
                        if ((this.txtPath.Text != string.Empty) && (this.CurrentLink.Path == string.Empty))
                        {
                            throw new ArgumentException();
                        }
                    }
                    catch (ArgumentException exception)
                    {
                        e.Cancel = true;
                        this.CurrentLink.Path = string.Empty;
                        this.Validator.SetValidateError((Control) sender, exception.Message);
                    }
                }
            }
        }

        private void txtTitle_Validated(object sender, EventArgs e)
        {
            this.CurrentLink.Name = this.txtTitle.Text;
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            string text = this.txtTitle.Text;
            if (string.IsNullOrEmpty(text))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
            }
        }

        private void txtWorkingDirectory_TextChanged(object sender, EventArgs e)
        {
            this.CurrentLink.WorkingDirectory = this.txtWorkingDirectory.Text;
            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            ListViewItem focusedItem = this.lvTools.FocusedItem;
            if (!((focusedItem == null) || focusedItem.Selected))
            {
                focusedItem = null;
            }
            CanMoveListViewItem item2 = (focusedItem != null) ? focusedItem.CanMove() : ((CanMoveListViewItem) 0);
            this.btnUp.Enabled = (item2 & CanMoveListViewItem.Up) > 0;
            this.btnDown.Enabled = (item2 & CanMoveListViewItem.Down) > 0;
            this.btnSort.Enabled = this.lvTools.Items.Count > 1;
            this.btnDelete.Enabled = focusedItem != null;
            this.btnSaveAs.Enabled = (this.Validator.Validate(false) && !string.IsNullOrEmpty(this.CurrentLink.Name)) && (((focusedItem == null) && !string.IsNullOrEmpty(this.CurrentLink.Path)) || ((focusedItem != null) && !this.CurrentLink.Equals((NamedShellLink) focusedItem.Tag)));
        }

        private void Validator_ValidatingControl(object sender, ValidatingControlEventArgs e)
        {
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new MethodInvoker(this.UpdateButtons));
            }
        }

        public bool SaveSettings
        {
            get
            {
                bool flag = false;
                if (this.ToolHashList != null)
                {
                    if (this.ToolHashList.Count == this.lvTools.Items.Count)
                    {
                        for (int i = 0; (i < this.ToolHashList.Count) && !flag; i++)
                        {
                            NamedShellLink tag = (NamedShellLink) this.lvTools.Items[i].Tag;
                            flag = !this.ToolHashList[i].Equals(tag.GenerateHash());
                        }
                        return flag;
                    }
                    return true;
                }
                return (this.lvTools.Items.Count > 0);
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

