namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class MakeLinkDialog : BasicDialog
    {
        private AutoCompleteProvider AutoComplete;
        private Button btnCancel;
        private Button btnOk;
        private Button btnTree;
        private Bevel bvlButtons;
        private ComboBox cmbDestFolder;
        private IContainer components = null;
        private ICreateVirtualLink CreateLink;
        private IVirtualFolder FCurrentFolder;
        private IVirtualFolder FDestFolder;
        private string FLinkName;
        private Label lblItem;
        private RadioButton[] LinkTypeButtons;
        private RadioButton rbLinkDefault;
        private RadioButton rbLinkHard;
        private RadioButton rbLinkJunction;
        private RadioButton rbLinkShellFolder;
        private RadioButton rbLinkSymbolic;
        private VirtualItemToolStrip tsItem;
        private ValidatorProvider Validator;

        public MakeLinkDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.AutoComplete.PreviewEnvironmentVariable += new EventHandler<PreviewEnvironmentVariableEventArgs>(AutoCompleteEvents.PreviewEnvironmentVariable);
            this.AutoComplete.PreviewFileSystemInfo += new EventHandler<PreviewFileSystemInfoEventArgs>(AutoCompleteEvents.PreviewFileSystemInfo);
            this.rbLinkDefault.Tag = Nomad.FileSystem.Virtual.LinkType.Default;
            this.rbLinkHard.Tag = Nomad.FileSystem.Virtual.LinkType.HardLink;
            this.rbLinkShellFolder.Tag = Nomad.FileSystem.Virtual.LinkType.ShellFolderLink;
            this.rbLinkJunction.Tag = Nomad.FileSystem.Virtual.LinkType.JuntionPoint;
            this.rbLinkSymbolic.Tag = Nomad.FileSystem.Virtual.LinkType.SymbolicLink;
            this.LinkTypeButtons = new RadioButton[] { this.rbLinkDefault, this.rbLinkHard, this.rbLinkShellFolder, this.rbLinkJunction, this.rbLinkSymbolic };
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.Validator.Validate(true))
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void btnTree_Click(object sender, EventArgs e)
        {
            using (SelectFolderDialog dialog = new SelectFolderDialog())
            {
                dialog.ShowItemIcons = Settings.Default.IsShowIcons;
                dialog.CurrentFolder = this.CurrentFolder;
                try
                {
                    dialog.Folder = this.DestFolder;
                }
                catch
                {
                    dialog.Folder = this.CurrentFolder;
                }
                if (dialog.Execute(this))
                {
                    this.DestFolder = dialog.Folder;
                }
            }
        }

        private void cmbDestFolder_Enter(object sender, EventArgs e)
        {
            if (Settings.Default.SelectNameWithoutExt && this.Validator.GetIsValid(this.cmbDestFolder))
            {
                int Len;
                if (!(PathHelper.HasTrailingDirectorySeparator(this.cmbDestFolder.Text) || ((Len = this.cmbDestFolder.Text.LastIndexOf('.')) <= 0)))
                {
                    base.BeginInvoke(delegate {
                        this.cmbDestFolder.SelectionLength = Len;
                    });
                }
            }
        }

        private void cmbDestFolder_TextUpdate(object sender, EventArgs e)
        {
            this.FDestFolder = null;
        }

        private void cmbDestFolder_Validated(object sender, EventArgs e)
        {
            Nomad.FileSystem.Virtual.LinkType type = this.CreateLink.CanCreateLinkIn(this.DestFolder);
            foreach (RadioButton button in this.LinkTypeButtons)
            {
                button.Enabled = (type & ((Nomad.FileSystem.Virtual.LinkType) button.Tag)) > Nomad.FileSystem.Virtual.LinkType.None;
            }
            bool flag = false;
            for (int i = this.LinkTypeButtons.Length - 1; i >= 0; i--)
            {
                button = this.LinkTypeButtons[i];
                if (flag && (button.Enabled || (button == this.rbLinkDefault)))
                {
                    button.Checked = true;
                    break;
                }
                if (!(!button.Checked || button.Enabled))
                {
                    flag = true;
                }
            }
        }

        private void cmbDestFolder_Validating(object sender, CancelEventArgs e)
        {
            string destFolderText = this.DestFolderText;
            if (!string.IsNullOrEmpty(destFolderText))
            {
                e.Cancel = PathHelper.GetPathType(destFolderText) == ~PathType.Unknown;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
                else
                {
                    try
                    {
                        if (this.FDestFolder == null)
                        {
                            this.DestFolderNeeded();
                        }
                    }
                    catch (Exception exception)
                    {
                        e.Cancel = true;
                        this.Validator.SetValidateError((Control) sender, exception.Message);
                    }
                }
                if (e.Cancel)
                {
                    for (int i = this.LinkTypeButtons.Length - 1; i >= 0; i--)
                    {
                        this.LinkTypeButtons[i].Enabled = false;
                    }
                }
            }
        }

        private void DestFolderNeeded()
        {
            this.FLinkName = null;
            string destFolderText = this.DestFolderText;
            if (string.IsNullOrEmpty(destFolderText))
            {
                this.FDestFolder = this.CurrentFolder;
            }
            else
            {
                PathType pathType = PathHelper.GetPathType(destFolderText);
                if ((pathType & PathType.Folder) > PathType.Unknown)
                {
                    this.FDestFolder = (IVirtualFolder) VirtualItem.FromFullName(destFolderText, VirtualItemType.Folder, this.CurrentFolder);
                }
                else
                {
                    if ((pathType & PathType.File) > PathType.Unknown)
                    {
                        this.FLinkName = Path.GetFileName(destFolderText);
                        destFolderText = Path.GetDirectoryName(destFolderText);
                    }
                    if (string.IsNullOrEmpty(destFolderText))
                    {
                        this.FDestFolder = this.CurrentFolder;
                    }
                    else
                    {
                        this.FDestFolder = (IVirtualFolder) VirtualItem.FromFullName(destFolderText, VirtualItemType.Folder, this.CurrentFolder);
                    }
                }
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

        public bool Execute(IWin32Window owner, IVirtualItem item)
        {
            this.tsItem.Add(item);
            this.CreateLink = item as ICreateVirtualLink;
            if (this.CreateLink == null)
            {
                this.cmbDestFolder.Enabled = false;
                this.btnTree.Enabled = false;
            }
            else
            {
                this.cmbDestFolder.Text = Path.Combine(this.cmbDestFolder.Text, this.CreateLink.GetPrefferedLinkName(Nomad.FileSystem.Virtual.LinkType.Default));
            }
            HistorySettings.PopulateComboBox(this.cmbDestFolder, HistorySettings.Default.MakeLinkName);
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                HistorySettings.Default.AddStringToMakeLinkName(this.cmbDestFolder.Text.Trim());
                return true;
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MakeLinkDialog));
            this.lblItem = new Label();
            this.rbLinkDefault = new RadioButton();
            this.rbLinkSymbolic = new RadioButton();
            this.rbLinkHard = new RadioButton();
            this.rbLinkJunction = new RadioButton();
            this.rbLinkShellFolder = new RadioButton();
            this.cmbDestFolder = new ComboBox();
            this.tsItem = new VirtualItemToolStrip(this.components);
            this.btnOk = new Button();
            this.btnTree = new Button();
            this.btnCancel = new Button();
            this.bvlButtons = new Bevel();
            this.Validator = new ValidatorProvider();
            this.AutoComplete = new AutoCompleteProvider();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            GroupBox control = new GroupBox();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            TableLayoutPanel panel3 = new TableLayoutPanel();
            panel.SuspendLayout();
            control.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblIn");
            label.Name = "lblIn";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblItem, 0, 0);
            panel.Controls.Add(control, 0, 4);
            panel.Controls.Add(this.cmbDestFolder, 0, 3);
            panel.Controls.Add(this.tsItem, 0, 1);
            panel.Controls.Add(label, 0, 2);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            manager.ApplyResources(control, "grpLinkType");
            control.Controls.Add(panel2);
            control.Name = "grpLinkType";
            control.TabStop = false;
            manager.ApplyResources(panel2, "tlpLinkType");
            panel2.Controls.Add(this.rbLinkDefault, 0, 0);
            panel2.Controls.Add(this.rbLinkSymbolic, 0, 4);
            panel2.Controls.Add(this.rbLinkHard, 0, 1);
            panel2.Controls.Add(this.rbLinkJunction, 0, 3);
            panel2.Controls.Add(this.rbLinkShellFolder, 0, 2);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpLinkType";
            panel2.Validating += new CancelEventHandler(this.tlpLinkType_Validating);
            manager.ApplyResources(this.rbLinkDefault, "rbLinkDefault");
            this.rbLinkDefault.Checked = true;
            this.rbLinkDefault.Name = "rbLinkDefault";
            this.rbLinkDefault.TabStop = true;
            this.rbLinkDefault.UseVisualStyleBackColor = true;
            this.rbLinkDefault.Click += new EventHandler(this.rbLinkDefault_Click);
            manager.ApplyResources(this.rbLinkSymbolic, "rbLinkSymbolic");
            this.rbLinkSymbolic.Name = "rbLinkSymbolic";
            this.rbLinkSymbolic.UseVisualStyleBackColor = true;
            this.rbLinkSymbolic.Click += new EventHandler(this.rbLinkDefault_Click);
            manager.ApplyResources(this.rbLinkHard, "rbLinkHard");
            this.rbLinkHard.Name = "rbLinkHard";
            this.rbLinkHard.UseVisualStyleBackColor = true;
            this.rbLinkHard.Click += new EventHandler(this.rbLinkDefault_Click);
            manager.ApplyResources(this.rbLinkJunction, "rbLinkJunction");
            this.rbLinkJunction.Name = "rbLinkJunction";
            this.rbLinkJunction.UseVisualStyleBackColor = true;
            this.rbLinkJunction.Click += new EventHandler(this.rbLinkDefault_Click);
            manager.ApplyResources(this.rbLinkShellFolder, "rbLinkShellFolder");
            this.rbLinkShellFolder.Name = "rbLinkShellFolder";
            this.rbLinkShellFolder.UseVisualStyleBackColor = true;
            this.rbLinkShellFolder.Click += new EventHandler(this.rbLinkDefault_Click);
            this.AutoComplete.SetAutoComplete(this.cmbDestFolder, true);
            manager.ApplyResources(this.cmbDestFolder, "cmbDestFolder");
            this.cmbDestFolder.Name = "cmbDestFolder";
            this.Validator.SetValidateOn(this.cmbDestFolder, ValidateOn.TextChangedTimer);
            this.cmbDestFolder.Validating += new CancelEventHandler(this.cmbDestFolder_Validating);
            this.cmbDestFolder.Enter += new EventHandler(this.cmbDestFolder_Enter);
            this.cmbDestFolder.Validated += new EventHandler(this.cmbDestFolder_Validated);
            this.cmbDestFolder.TextUpdate += new EventHandler(this.cmbDestFolder_TextUpdate);
            this.tsItem.BackColor = SystemColors.ButtonFace;
            this.tsItem.GripStyle = ToolStripGripStyle.Hidden;
            manager.ApplyResources(this.tsItem, "tsItem");
            this.tsItem.Name = "tsItem";
            manager.ApplyResources(panel3, "tlpButtons");
            panel3.Controls.Add(this.btnOk, 1, 0);
            panel3.Controls.Add(this.btnTree, 2, 0);
            panel3.Controls.Add(this.btnCancel, 3, 0);
            panel3.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel3.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnTree, "btnTree");
            this.btnTree.Name = "btnTree";
            this.btnTree.UseVisualStyleBackColor = true;
            this.btnTree.Click += new EventHandler(this.btnTree_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            this.AutoComplete.UseEnvironmentVariablesSource = Settings.Default.UseACSEnvironmentVariables;
            this.AutoComplete.UseFileSystemSource = Settings.Default.UseACSFileSystem;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel3);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MakeLinkDialog";
            base.ShowInTaskbar = false;
            base.Activated += new EventHandler(this.MakeLinkDialog_Activated);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            control.ResumeLayout(false);
            control.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void MakeLinkDialog_Activated(object sender, EventArgs e)
        {
            if (this.cmbDestFolder.Focused)
            {
                this.cmbDestFolder_Enter(sender, e);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            this.cmbDestFolder.AutoCompleteMode = Settings.Default.AutoCompleteMode;
            base.OnShown(e);
            this.MakeLinkDialog_Activated(this, e);
        }

        private void rbLinkDefault_Click(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton) sender;
            if (button.Checked)
            {
                string prefferedLinkName = this.CreateLink.GetPrefferedLinkName((Nomad.FileSystem.Virtual.LinkType) button.Tag);
                string directoryName = this.cmbDestFolder.Text.Trim();
                if (!(string.IsNullOrEmpty(directoryName) || PathHelper.HasTrailingDirectorySeparator(directoryName)))
                {
                    directoryName = Path.GetDirectoryName(directoryName);
                }
                this.cmbDestFolder.Text = Path.Combine(directoryName, prefferedLinkName);
            }
        }

        private void tlpLinkType_Validating(object sender, CancelEventArgs e)
        {
            foreach (RadioButton button in this.LinkTypeButtons)
            {
                if (!(!button.Checked || button.Enabled))
                {
                    e.Cancel = true;
                    break;
                }
            }
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

        public IVirtualFolder DestFolder
        {
            get
            {
                if (this.FDestFolder == null)
                {
                    this.DestFolderNeeded();
                }
                return this.FDestFolder;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.FDestFolder = value;
                this.cmbDestFolder.Text = this.FDestFolder.FullName;
            }
        }

        private string DestFolderText
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(this.cmbDestFolder.Text.Trim());
            }
        }

        public string LinkName
        {
            get
            {
                if (this.FLinkName == null)
                {
                    this.DestFolderNeeded();
                    if (this.FLinkName == null)
                    {
                        this.FLinkName = this.CreateLink.GetPrefferedLinkName(this.LinkType);
                    }
                }
                return this.FLinkName;
            }
        }

        public Nomad.FileSystem.Virtual.LinkType LinkType
        {
            get
            {
                foreach (RadioButton button in this.LinkTypeButtons)
                {
                    if (button.Checked)
                    {
                        return (Nomad.FileSystem.Virtual.LinkType) button.Tag;
                    }
                }
                return Nomad.FileSystem.Virtual.LinkType.None;
            }
        }
    }
}

