namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Commons.Threading;
    using Nomad.Configuration;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Network;
    using Nomad.FileSystem.Shell;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class SelectFolderDialog : BasicDialog
    {
        private AutoCompleteProvider AutoComplete;
        private Button btnCancel;
        private Button btnMakeFolder;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkUpdateTree;
        private ComboBox cmbFolder;
        private IContainer components = null;
        private IVirtualFolder FCurrentFolder;
        private IVirtualFolder FFolder;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpButtons;
        private ToolTip toolTipError;
        private VirtualFolderTreeView tvFolders;

        public SelectFolderDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            this.AutoComplete.PreviewEnvironmentVariable += new EventHandler<PreviewEnvironmentVariableEventArgs>(AutoCompleteEvents.PreviewEnvironmentVariable);
            this.AutoComplete.PreviewFileSystemInfo += new EventHandler<PreviewFileSystemInfoEventArgs>(AutoCompleteEvents.PreviewFileSystemInfo);
            if (Settings.Default.UseACSKnownShellFolders)
            {
                this.AutoComplete.GetCustomSource += new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetKnownFoldersSource);
                this.AutoComplete.UseCustomSource = true;
            }
            if (Settings.Default.UseACSRecentItems)
            {
                this.AutoComplete.GetCustomSource += new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetComboBoxSource);
                this.AutoComplete.UseCustomSource = true;
            }
            bool flag = SettingsManager.CheckSafeMode(SafeMode.SkipFormPlacement) || (Control.ModifierKeys == Keys.Shift);
            FormSettings.RegisterForm(this, flag ? FormPlacement.None : FormPlacement.Size);
        }

        private void btnMakeFolder_Click(object sender, EventArgs e)
        {
            ICreateVirtualFolder folder = null;
            if (this.FFolder is FileSystemFolder)
            {
                string fullName = this.FFolder.FullName;
                if (Directory.Exists(fullName))
                {
                    return;
                }
                while ((fullName != null) && !Directory.Exists(fullName))
                {
                    fullName = Path.GetDirectoryName(PathHelper.ExcludeTrailingDirectorySeparator(fullName));
                }
                if (fullName != null)
                {
                    folder = VirtualItem.FromFullName(fullName, VirtualItemType.Folder) as ICreateVirtualFolder;
                }
            }
            else
            {
                IChangeVirtualItem fFolder = this.FFolder as IChangeVirtualItem;
                if ((fFolder == null) || fFolder.Exists)
                {
                    return;
                }
                while ((fFolder != null) && !fFolder.Exists)
                {
                    fFolder = fFolder.Parent as IChangeVirtualItem;
                }
                folder = fFolder as ICreateVirtualFolder;
            }
            if (folder != null)
            {
                IVirtualFolder folder2 = folder.CreateFolder(this.FFolder.FullName);
                this.UpdateTree(folder2.FullName);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.FFolder = (IVirtualFolder) VirtualItem.FromFullName(this.FolderPath, VirtualItemType.Folder, this.CurrentFolder);
                Task.Create<IVirtualFolder>(delegate (IVirtualFolder x) {
                    HistorySettings.Default.AddStringToChangeFolder(x.FullName);
                }, this.FFolder).Start();
                base.DialogResult = DialogResult.OK;
            }
            catch (SystemException exception)
            {
                MessageDialog.ShowException(this, exception);
            }
        }

        private void cmbFolder_DropDownClosed(object sender, EventArgs e)
        {
            string tag = this.cmbFolder.Tag as string;
            this.cmbFolder.Tag = null;
            if (tag != null)
            {
                this.UpdateTree(tag);
            }
        }

        private void cmbFolder_Leave(object sender, EventArgs e)
        {
            this.toolTipError.Hide(this.cmbFolder);
        }

        private void cmbFolder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!this.cmbFolder.DroppedDown)
            {
                this.UpdateTree((string) this.cmbFolder.SelectedItem);
            }
            else
            {
                this.cmbFolder.Tag = this.cmbFolder.SelectedItem;
            }
        }

        private void cmbFolder_TextUpdate(object sender, EventArgs e)
        {
            if (!this.cmbFolder.DroppedDown)
            {
                this.UpdateTree(this.FolderPath);
            }
            else
            {
                this.cmbFolder.Tag = this.FolderPath;
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

        public bool Execute(IWin32Window owner)
        {
            HistorySettings.PopulateComboBox(this.cmbFolder, HistorySettings.Default.ChangeFolder);
            return (base.ShowDialog(owner) == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SelectFolderDialog));
            this.chkUpdateTree = new CheckBox();
            this.cmbFolder = new ComboBox();
            this.tvFolders = new VirtualFolderTreeView();
            this.tlpButtons = new TableLayoutPanel();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.btnMakeFolder = new Button();
            this.toolTipError = new ToolTip(this.components);
            this.bvlButtons = new Bevel();
            this.AutoComplete = new AutoCompleteProvider();
            Label label = new Label();
            Label label2 = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblFolder");
            label.Name = "lblFolder";
            manager.ApplyResources(label2, "lblTree");
            label2.Name = "lblTree";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(label, 0, 0);
            panel.Controls.Add(this.chkUpdateTree, 0, 4);
            panel.Controls.Add(this.cmbFolder, 0, 1);
            panel.Controls.Add(label2, 0, 2);
            panel.Controls.Add(this.tvFolders, 0, 3);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkUpdateTree, "chkUpdateTree");
            this.chkUpdateTree.Checked = true;
            this.chkUpdateTree.CheckState = CheckState.Checked;
            this.chkUpdateTree.Name = "chkUpdateTree";
            this.chkUpdateTree.UseVisualStyleBackColor = true;
            this.AutoComplete.SetAutoComplete(this.cmbFolder, true);
            manager.ApplyResources(this.cmbFolder, "cmbFolder");
            this.cmbFolder.FormattingEnabled = true;
            this.cmbFolder.Name = "cmbFolder";
            this.cmbFolder.SelectionChangeCommitted += new EventHandler(this.cmbFolder_SelectionChangeCommitted);
            this.cmbFolder.Leave += new EventHandler(this.cmbFolder_Leave);
            this.cmbFolder.DropDownClosed += new EventHandler(this.cmbFolder_DropDownClosed);
            this.cmbFolder.TextUpdate += new EventHandler(this.cmbFolder_TextUpdate);
            this.tvFolders.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            this.tvFolders.DataBindings.Add(new Binding("FolderNameCasing", VirtualFilePanelSettings.Default, "FolderNameCasing", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.tvFolders, "tvFolders");
            this.tvFolders.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.tvFolders.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.tvFolders.FadePlusMinus = true;
            this.tvFolders.FolderNameCasing = VirtualFilePanelSettings.Default.FolderNameCasing;
            this.tvFolders.FullRowSelect = true;
            this.tvFolders.HideSelection = false;
            this.tvFolders.HotTracking = true;
            this.tvFolders.Name = "tvFolders";
            this.tvFolders.ShowAllRootFolders = true;
            this.tvFolders.ShowItemIcons = false;
            this.tvFolders.ShowLines = false;
            this.tvFolders.WatchChanges = Settings.Default.WatchFolderTree;
            this.tvFolders.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnCancel, 3, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnMakeFolder, 2, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnMakeFolder, "btnMakeFolder");
            this.btnMakeFolder.Name = "btnMakeFolder";
            this.btnMakeFolder.UseVisualStyleBackColor = true;
            this.btnMakeFolder.Click += new EventHandler(this.btnMakeFolder_Click);
            this.toolTipError.ToolTipIcon = ToolTipIcon.Error;
            this.toolTipError.ToolTipTitle = "Invalid Folder Name";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.AutoComplete.UseEnvironmentVariablesSource = Settings.Default.UseACSEnvironmentVariables;
            this.AutoComplete.UseFileSystemSource = Settings.Default.UseACSFileSystem;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpButtons);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SelectFolderDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.SelectFolderDialog_Shown);
            base.FormClosed += new FormClosedEventHandler(this.SelectFolderDialog_FormClosed);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnShown(EventArgs e)
        {
            this.cmbFolder.AutoCompleteMode = Settings.Default.AutoCompleteMode;
            base.OnShown(e);
        }

        private void SelectFolderDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.cmbFolder.Text = string.Empty;
            this.tvFolders.Nodes.Clear();
        }

        private void SelectFolderDialog_Shown(object sender, EventArgs e)
        {
            this.cmbFolder.Select();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.tvFolders.Focused)
            {
                this.FFolder = (IVirtualFolder) e.Node.Tag;
                this.cmbFolder.Text = this.FFolder.FullName;
                this.btnOk.Enabled = true;
                this.btnMakeFolder.Enabled = false;
            }
        }

        private void UpdateTree(string folderName)
        {
            bool flag = !string.IsNullOrEmpty(folderName);
            bool flag2 = false;
            if (flag && this.chkUpdateTree.Checked)
            {
                try
                {
                    this.FFolder = null;
                    IVirtualFolder fFolder = null;
                    PathType pathType = PathHelper.GetPathType(folderName);
                    if ((pathType & PathType.Uri) > PathType.Unknown)
                    {
                        Uri uri = new Uri(folderName);
                        if (uri.Scheme == Uri.UriSchemeFile)
                        {
                            folderName = uri.LocalPath;
                            pathType &= ~PathType.Uri;
                        }
                        else if (((uri.Scheme != Uri.UriSchemeFtp) && (uri.Scheme != NetworkFileSystemCreator.UriScheme)) && (uri.Scheme != ShellFileSystemCreator.UriScheme))
                        {
                            throw new WarningException(Resources.sErrorUnsupportedUriScheme);
                        }
                    }
                    if (pathType == PathType.NetworkServer)
                    {
                        if (folderName.EndsWith(new string(Path.DirectorySeparatorChar, 1)))
                        {
                            this.FFolder = (IVirtualFolder) VirtualItem.FromFullName(folderName, VirtualItemType.Folder, this.CurrentFolder);
                            fFolder = this.FFolder;
                        }
                    }
                    else if (pathType == PathType.NetworkShare)
                    {
                        int length = folderName.LastIndexOf(Path.DirectorySeparatorChar);
                        if (length == (folderName.Length - 1))
                        {
                            this.FFolder = (IVirtualFolder) VirtualItem.FromFullName(folderName, VirtualItemType.Folder, this.CurrentFolder);
                            fFolder = this.FFolder;
                        }
                        else
                        {
                            fFolder = (IVirtualFolder) VirtualItem.FromFullName(folderName.Substring(0, length), VirtualItemType.Folder, this.CurrentFolder);
                            this.FFolder = null;
                        }
                    }
                    else if ((pathType & PathType.Uri) == PathType.Unknown)
                    {
                        this.FFolder = (IVirtualFolder) VirtualItem.FromFullName(folderName, VirtualItemType.Folder, this.CurrentFolder);
                        IPersistVirtualItem parent = this.FFolder as IPersistVirtualItem;
                        while ((parent != null) && !parent.Exists)
                        {
                            parent = parent.Parent as IPersistVirtualItem;
                        }
                        fFolder = parent as IVirtualFolder;
                    }
                    if ((this.FFolder == null) || (fFolder == null))
                    {
                        this.tvFolders.SelectedNode = null;
                    }
                    else
                    {
                        this.tvFolders.ShowVirtualFolder(fFolder, true);
                    }
                    flag2 = ((this.FFolder != null) && !this.FFolder.Equals(fFolder)) && (fFolder is ICreateVirtualFolder);
                }
                catch (Exception exception)
                {
                    this.cmbFolder.BackColor = Settings.TextBoxError;
                    this.cmbFolder.ForeColor = SystemColors.HighlightText;
                    this.toolTipError.Show(exception.Message, this.cmbFolder, 0, this.cmbFolder.Height + this.cmbFolder.Margin.Bottom);
                    flag = false;
                    flag2 = false;
                }
            }
            if (flag)
            {
                this.cmbFolder.ResetForeColor();
                this.cmbFolder.ResetBackColor();
                this.toolTipError.Hide(this.cmbFolder);
            }
            this.btnOk.Enabled = flag;
            this.btnMakeFolder.Enabled = flag2;
        }

        public IVirtualFolder CurrentFolder
        {
            get
            {
                return this.FCurrentFolder;
            }
            set
            {
                this.FCurrentFolder = value;
                CustomFileSystemFolder folder = value as CustomFileSystemFolder;
                this.AutoComplete.CurrentDirectory = (folder != null) ? folder.FullName : string.Empty;
            }
        }

        [Browsable(false)]
        public IVirtualFolder Folder
        {
            get
            {
                return this.FFolder;
            }
            set
            {
                if (value != null)
                {
                    this.FFolder = value;
                    this.tvFolders.CurrentFolder = value;
                    if (this.CurrentFolder == null)
                    {
                        this.CurrentFolder = value;
                    }
                    if (this.FFolder == this.CurrentFolder)
                    {
                        this.cmbFolder.Text = string.Empty;
                    }
                    else
                    {
                        this.cmbFolder.Text = this.FFolder.FullName;
                    }
                }
            }
        }

        public string FolderPath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(this.cmbFolder.Text.Trim());
            }
        }

        public bool ShowItemIcons
        {
            get
            {
                return this.tvFolders.ShowItemIcons;
            }
            set
            {
                this.tvFolders.ShowItemIcons = value;
            }
        }
    }
}

