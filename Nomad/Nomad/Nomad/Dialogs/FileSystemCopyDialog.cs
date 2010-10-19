namespace Nomad.Dialogs
{
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using Nomad.Workers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FileSystemCopyDialog : BasicDialog
    {
        private AutoCompleteProvider AutoComplete;
        private Button btnCancel;
        private Button btnOk;
        private Button btnTree;
        private Bevel bvlButtons;
        private CheckBox chkAsyncFileCopy;
        private CheckBox chkCheckFreeSpace;
        private CheckBox chkCopyItemACL;
        private CheckBox chkCopyItemTime;
        private CheckBox chkDeleteSource;
        private CheckBox chkSkipEmptyFolders;
        private ComboBox cmbCopyTo;
        private FilterComboBox cmbFilter;
        private ComboBoxEx cmbMode;
        private IContainer components = null;
        private CopyDestinationItem DestinationItem = ConfirmationSettings.Default.CopyDestinationItem;
        public IVirtualFolder FCurrentFolder;
        private IVirtualFolder FDestFolder;
        private static char[] FileMaskChars = new char[] { '?', '*' };
        private IRenameFilter FRenameFilter;
        private Label lblCopyTo;
        private Label lblFilter;
        private Label lblMode;
        private IEnumerable<IVirtualItem> Selection;
        private TableLayoutPanel tlpBack;
        private ValidatorProvider Validator;

        public FileSystemCopyDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.Validator.TooltipTitle = Resources.sError;
            this.AutoComplete.PreviewEnvironmentVariable += new EventHandler<PreviewEnvironmentVariableEventArgs>(AutoCompleteEvents.PreviewEnvironmentVariable);
            this.AutoComplete.PreviewFileSystemInfo += new EventHandler<PreviewFileSystemInfoEventArgs>(AutoCompleteEvents.PreviewFileSystemInfo);
            this.AutoComplete.GetCustomSource += new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetComboBoxSource);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.DestinationItem == CopyDestinationItem.Ask)
            {
                this.DestFolderNeeded();
                int num = this.Selection.Count<IVirtualItem>();
                if (((num > 1) && (this.RenameFilter != null)) && (Path.GetFileName(this.CopyTo).IndexOfAny(FileMaskChars) < 0))
                {
                    bool checkBoxChecked = false;
                    switch (MessageDialog.Show(this, PluralInfo.Format(Resources.sAskCopyToSingleFile, new object[] { num }), Resources.sConfirmCopyToFile, Resources.sRememberQuestionAnswer, ref checkBoxChecked, MessageDialog.ButtonsYesNoCancel, MessageBoxIcon.Question))
                    {
                        case MessageDialogResult.Yes:
                            this.DestinationItem = CopyDestinationItem.Folder;
                            this.FDestFolder = null;
                            break;

                        case MessageDialogResult.No:
                            break;

                        default:
                            return;
                    }
                    if (checkBoxChecked)
                    {
                        ConfirmationSettings.Default.CopyDestinationItem = this.DestinationItem;
                    }
                }
            }
            if (this.Validator.Validate(true))
            {
                base.DialogResult = DialogResult.OK;
            }
            else if (!this.Validator.GetIsValid(this.cmbCopyTo))
            {
                this.cmbCopyTo.Select();
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

        private void chkDeleteSource_Click(object sender, EventArgs e)
        {
            if (this.Selection != null)
            {
                this.UpdateCopyToText(this.chkDeleteSource.Checked, this.Selection);
            }
        }

        private void cmbCopyTo_TextUpdate(object sender, EventArgs e)
        {
            this.FDestFolder = null;
        }

        private void cmbCopyTo_Validating(object sender, CancelEventArgs e)
        {
            string copyTo = this.CopyTo;
            if (string.IsNullOrEmpty(copyTo))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = PathHelper.GetPathType(copyTo) == ~PathType.Unknown;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
                else
                {
                    try
                    {
                        this.DestFolderNeeded();
                    }
                    catch (Exception exception)
                    {
                        e.Cancel = true;
                        this.Validator.SetValidateError((Control) sender, exception.Message);
                    }
                }
            }
        }

        private void DestFolderNeeded()
        {
            if (this.FDestFolder == null)
            {
                this.FRenameFilter = null;
                string copyTo = this.CopyTo;
                if (string.IsNullOrEmpty(copyTo))
                {
                    this.FDestFolder = this.CurrentFolder;
                }
                else
                {
                    PathType pathType = PathHelper.GetPathType(copyTo);
                    if ((pathType & PathType.Folder) > PathType.Unknown)
                    {
                        this.FDestFolder = (IVirtualFolder) VirtualItem.FromFullName(copyTo, VirtualItemType.Folder, this.CurrentFolder);
                    }
                    else
                    {
                        if ((pathType & PathType.File) > PathType.Unknown)
                        {
                            string fileName = Path.GetFileName(copyTo);
                            if (fileName.IndexOfAny(FileMaskChars) < 0)
                            {
                                if (this.SelectionHasFolders || (this.DestinationItem == CopyDestinationItem.Folder))
                                {
                                    this.FDestFolder = (IVirtualFolder) VirtualItem.FromFullName(copyTo, VirtualItemType.Folder, this.CurrentFolder);
                                }
                                else
                                {
                                    try
                                    {
                                        IVirtualItem item = VirtualItem.FromFullName(copyTo, VirtualItemType.Unknown, this.CurrentFolder);
                                        this.FDestFolder = item as IVirtualFolder;
                                        if (this.FDestFolder == null)
                                        {
                                            this.FDestFolder = item.Parent;
                                            this.FRenameFilter = new RegexRenameFilter(fileName);
                                        }
                                    }
                                    catch (FileNotFoundException)
                                    {
                                    }
                                }
                                return;
                            }
                            copyTo = Path.GetDirectoryName(copyTo);
                            this.FRenameFilter = new RegexRenameFilter(fileName);
                        }
                        if (string.IsNullOrEmpty(copyTo))
                        {
                            this.FDestFolder = this.CurrentFolder;
                        }
                        else
                        {
                            this.FDestFolder = (IVirtualFolder) VirtualItem.FromFullName(copyTo, VirtualItemType.Folder, this.CurrentFolder);
                        }
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

        public bool Execute(IWin32Window owner, IEnumerable<IVirtualItem> selection)
        {
            this.Selection = selection;
            this.UpdateCopyToText(this.chkDeleteSource.Checked, selection);
            if (this.FDestFolder != null)
            {
                IVirtualItem item = null;
                foreach (IVirtualItem item2 in selection)
                {
                    if (item == null)
                    {
                        item = item2;
                    }
                    else
                    {
                        item = null;
                        break;
                    }
                }
                if (item is IVirtualFolder)
                {
                    item = null;
                }
                ICreateVirtualFile fDestFolder = this.FDestFolder as ICreateVirtualFile;
                if ((fDestFolder != null) && (item != null))
                {
                    IVirtualItem item3 = fDestFolder.CreateFile(item.Name);
                    this.cmbCopyTo.Text = item3.FullName;
                    this.FRenameFilter = new SimpleRenameFilter(item3.Name);
                }
                else
                {
                    this.cmbCopyTo.Text = this.FDestFolder.FullName;
                }
            }
            HistorySettings.PopulateComboBox(this.cmbCopyTo, HistorySettings.Default.CopyTo);
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                HistorySettings.Default.AddStringToCopyTo(this.CopyTo);
                if (this.cmbFilter.Enabled)
                {
                    HistorySettings.Default.AddStringToCopyFilter(this.cmbFilter.FilterString);
                }
                return true;
            }
            return false;
        }

        private void FileSystemCopyDialog_Shown(object sender, EventArgs e)
        {
            this.cmbMode.SelectedIndex = 0;
            base.ActiveControl = this.cmbCopyTo;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            preferredSize.Width = (base.Width - base.ClientSize.Width) + this.tlpBack.PreferredSize.Width;
            return preferredSize;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FileSystemCopyDialog));
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.btnTree = new Button();
            this.tlpBack = new TableLayoutPanel();
            this.lblCopyTo = new Label();
            this.chkAsyncFileCopy = new CheckBox();
            this.chkCopyItemACL = new CheckBox();
            this.chkSkipEmptyFolders = new CheckBox();
            this.cmbCopyTo = new ComboBox();
            this.chkCheckFreeSpace = new CheckBox();
            this.chkCopyItemTime = new CheckBox();
            this.lblMode = new Label();
            this.cmbMode = new ComboBoxEx();
            this.lblFilter = new Label();
            this.chkDeleteSource = new CheckBox();
            this.cmbFilter = new FilterComboBox();
            this.bvlButtons = new Bevel();
            this.Validator = new ValidatorProvider();
            this.AutoComplete = new AutoCompleteProvider();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.tlpBack.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpButtons");
            panel.Controls.Add(this.btnOk, 1, 0);
            panel.Controls.Add(this.btnCancel, 3, 0);
            panel.Controls.Add(this.btnTree, 2, 0);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnTree, "btnTree");
            this.btnTree.Name = "btnTree";
            this.btnTree.UseVisualStyleBackColor = true;
            this.btnTree.Click += new EventHandler(this.btnTree_Click);
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.lblCopyTo, 0, 0);
            this.tlpBack.Controls.Add(this.chkAsyncFileCopy, 1, 6);
            this.tlpBack.Controls.Add(this.chkCopyItemACL, 0, 6);
            this.tlpBack.Controls.Add(this.chkSkipEmptyFolders, 1, 5);
            this.tlpBack.Controls.Add(this.cmbCopyTo, 0, 1);
            this.tlpBack.Controls.Add(this.chkCheckFreeSpace, 1, 4);
            this.tlpBack.Controls.Add(this.chkCopyItemTime, 0, 5);
            this.tlpBack.Controls.Add(this.lblMode, 0, 2);
            this.tlpBack.Controls.Add(this.cmbMode, 0, 3);
            this.tlpBack.Controls.Add(this.lblFilter, 1, 2);
            this.tlpBack.Controls.Add(this.chkDeleteSource, 0, 4);
            this.tlpBack.Controls.Add(this.cmbFilter, 1, 3);
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Name = "tlpBack";
            this.lblCopyTo.AutoEllipsis = true;
            this.tlpBack.SetColumnSpan(this.lblCopyTo, 2);
            manager.ApplyResources(this.lblCopyTo, "lblCopyTo");
            this.lblCopyTo.Name = "lblCopyTo";
            manager.ApplyResources(this.chkAsyncFileCopy, "chkAsyncFileCopy");
            this.chkAsyncFileCopy.Checked = true;
            this.chkAsyncFileCopy.CheckState = CheckState.Indeterminate;
            this.chkAsyncFileCopy.Name = "chkAsyncFileCopy";
            this.chkAsyncFileCopy.ThreeState = true;
            this.chkAsyncFileCopy.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCopyItemACL, "chkCopyItemACL");
            this.chkCopyItemACL.Name = "chkCopyItemACL";
            this.chkCopyItemACL.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSkipEmptyFolders, "chkSkipEmptyFolders");
            this.chkSkipEmptyFolders.Name = "chkSkipEmptyFolders";
            this.chkSkipEmptyFolders.UseVisualStyleBackColor = true;
            this.AutoComplete.SetAutoComplete(this.cmbCopyTo, true);
            this.tlpBack.SetColumnSpan(this.cmbCopyTo, 2);
            manager.ApplyResources(this.cmbCopyTo, "cmbCopyTo");
            this.cmbCopyTo.Name = "cmbCopyTo";
            this.Validator.SetValidateOn(this.cmbCopyTo, ValidateOn.TextChangedTimer);
            this.cmbCopyTo.Validating += new CancelEventHandler(this.cmbCopyTo_Validating);
            this.cmbCopyTo.SelectionChangeCommitted += new EventHandler(this.cmbCopyTo_TextUpdate);
            this.cmbCopyTo.TextUpdate += new EventHandler(this.cmbCopyTo_TextUpdate);
            manager.ApplyResources(this.chkCheckFreeSpace, "chkCheckFreeSpace");
            this.chkCheckFreeSpace.Checked = true;
            this.chkCheckFreeSpace.CheckState = CheckState.Checked;
            this.chkCheckFreeSpace.Name = "chkCheckFreeSpace";
            this.chkCheckFreeSpace.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCopyItemTime, "chkCopyItemTime");
            this.chkCopyItemTime.Checked = true;
            this.chkCopyItemTime.CheckState = CheckState.Checked;
            this.chkCopyItemTime.Name = "chkCopyItemTime";
            this.chkCopyItemTime.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.lblMode, "lblMode");
            this.lblMode.Name = "lblMode";
            manager.ApplyResources(this.cmbMode, "cmbMode");
            this.cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMode.Items.AddRange(new object[] { manager.GetString("cmbMode.Items"), manager.GetString("cmbMode.Items1"), manager.GetString("cmbMode.Items2"), manager.GetString("cmbMode.Items3"), manager.GetString("cmbMode.Items4") });
            this.cmbMode.MinimumSize = new Size(0xca, 0);
            this.cmbMode.Name = "cmbMode";
            manager.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            manager.ApplyResources(this.chkDeleteSource, "chkDeleteSource");
            this.chkDeleteSource.Name = "chkDeleteSource";
            this.chkDeleteSource.UseVisualStyleBackColor = true;
            this.chkDeleteSource.Click += new EventHandler(this.chkDeleteSource_Click);
            manager.ApplyResources(this.cmbFilter, "cmbFilter");
            this.cmbFilter.DrawMode = DrawMode.OwnerDrawFixed;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Name = "cmbFilter";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            this.AutoComplete.UseCustomSource = Settings.Default.UseACSRecentItems;
            this.AutoComplete.UseEnvironmentVariablesSource = Settings.Default.UseACSEnvironmentVariables;
            this.AutoComplete.UseFileSystemSource = Settings.Default.UseACSFileSystem;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpBack);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FileSystemCopyDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.FileSystemCopyDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnShown(EventArgs e)
        {
            this.cmbCopyTo.AutoCompleteMode = Settings.Default.AutoCompleteMode;
            base.OnShown(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.A))
            {
                this.cmbMode.SelectedIndex = 1;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UpdateCopyToText(bool move, IEnumerable<IVirtualItem> selection)
        {
            string sRenameOrMoveFolder;
            int num = selection.Count<IVirtualItem>();
            IVirtualItem item = null;
            bool flag = false;
            bool flag2 = false;
            foreach (IVirtualItem item2 in selection)
            {
                if (item == null)
                {
                    item = item2;
                }
                if (item2 is IVirtualFolder)
                {
                    flag = true;
                }
                if (item2.IsPropertyAvailable(14))
                {
                    flag2 = true;
                }
                if (flag && flag2)
                {
                    break;
                }
            }
            if (num == 1)
            {
                if (item is IVirtualFolder)
                {
                    if (move)
                    {
                        sRenameOrMoveFolder = Resources.sRenameOrMoveFolder;
                    }
                    else
                    {
                        sRenameOrMoveFolder = Resources.sCopyFolder;
                    }
                }
                else if (move)
                {
                    sRenameOrMoveFolder = Resources.sRenameOrMoveFile;
                }
                else
                {
                    sRenameOrMoveFolder = Resources.sCopyFile;
                }
                sRenameOrMoveFolder = string.Format(sRenameOrMoveFolder, item.Name.Replace("&", "&&"));
            }
            else
            {
                if (move)
                {
                    sRenameOrMoveFolder = Resources.sRenameOrMoveMultipleFile;
                }
                else
                {
                    sRenameOrMoveFolder = Resources.sCopyMultipleFile;
                }
                sRenameOrMoveFolder = PluralInfo.Format(sRenameOrMoveFolder, new object[] { num });
            }
            this.lblFilter.Enabled = flag;
            this.cmbFilter.Enabled = flag;
            this.chkSkipEmptyFolders.Enabled = flag;
            this.chkCopyItemACL.Enabled = flag2;
            if (move)
            {
                this.Text = Resources.sRenameOrMove;
                this.btnOk.Text = Resources.sMove;
            }
            else
            {
                this.Text = Resources.sCopy;
                this.btnOk.Text = Resources.sCopy;
            }
            this.lblCopyTo.Text = sRenameOrMoveFolder;
        }

        public CopyWorkerOptions CopyOptions
        {
            get
            {
                return (((((((this.chkDeleteSource.Checked ? CopyWorkerOptions.DeleteSource : ((CopyWorkerOptions) 0)) | (this.chkCopyItemTime.Checked ? CopyWorkerOptions.CopyItemTime : ((CopyWorkerOptions) 0))) | (this.chkCopyItemACL.Checked ? CopyWorkerOptions.CopyACL : ((CopyWorkerOptions) 0))) | (this.chkSkipEmptyFolders.Checked ? CopyWorkerOptions.SkipEmptyFolders : ((CopyWorkerOptions) 0))) | (this.chkCheckFreeSpace.Checked ? CopyWorkerOptions.CheckFreeSpace : ((CopyWorkerOptions) 0))) | ((this.chkAsyncFileCopy.CheckState == CheckState.Checked) ? CopyWorkerOptions.AsyncCopy : ((CopyWorkerOptions) 0))) | ((this.chkAsyncFileCopy.CheckState == CheckState.Indeterminate) ? CopyWorkerOptions.AutoAsyncCopy : ((CopyWorkerOptions) 0)));
            }
            set
            {
                this.chkDeleteSource.Checked = (value & CopyWorkerOptions.DeleteSource) > 0;
                this.chkCopyItemTime.Checked = (value & CopyWorkerOptions.CopyItemTime) > 0;
                this.chkCopyItemACL.Checked = (value & CopyWorkerOptions.CopyACL) > 0;
                this.chkSkipEmptyFolders.Checked = (value & CopyWorkerOptions.SkipEmptyFolders) > 0;
                this.chkCheckFreeSpace.Checked = (value & CopyWorkerOptions.CheckFreeSpace) > 0;
                if ((value & CopyWorkerOptions.AutoAsyncCopy) > 0)
                {
                    this.chkAsyncFileCopy.CheckState = CheckState.Indeterminate;
                }
                else if ((value & CopyWorkerOptions.AsyncCopy) > 0)
                {
                    this.chkAsyncFileCopy.CheckState = CheckState.Checked;
                }
                else
                {
                    this.chkAsyncFileCopy.CheckState = CheckState.Unchecked;
                }
            }
        }

        private string CopyTo
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(this.cmbCopyTo.Text.Trim());
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

        public IOverwriteRule[] DefaultOverwriteRules
        {
            get
            {
                switch (this.cmbMode.SelectedIndex)
                {
                    case 1:
                        return new IOverwriteRule[] { new OverwriteAllRule(OverwriteDialogResult.Overwrite) };

                    case 2:
                        return new IOverwriteRule[] { new OverwriteAllRule(OverwriteDialogResult.Append) };

                    case 3:
                        return new IOverwriteRule[] { new OverwriteAllRule(OverwriteDialogResult.Skip) };

                    case 4:
                        return new IOverwriteRule[] { new OverwritePropertyRule(8, Compare.GreaterEqual, OverwriteDialogResult.Skip), new OverwritePropertyRule(8, Compare.Less, OverwriteDialogResult.Overwrite) };
                }
                return null;
            }
        }

        public IVirtualFolder DestFolder
        {
            get
            {
                this.DestFolderNeeded();
                return this.FDestFolder;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.cmbCopyTo.Text = value.FullName;
                this.FDestFolder = value;
                this.FRenameFilter = null;
            }
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                return this.cmbFilter.Filter;
            }
        }

        public IRenameFilter RenameFilter
        {
            get
            {
                this.DestFolderNeeded();
                return this.FRenameFilter;
            }
        }

        public bool RenameOrMove
        {
            get
            {
                return this.chkDeleteSource.Checked;
            }
            set
            {
                this.chkDeleteSource.Checked = value;
            }
        }

        private bool SelectionHasFolders
        {
            get
            {
                return this.Selection.Any<IVirtualItem>(delegate (IVirtualItem x) {
                    return (x is IVirtualFolder);
                });
            }
        }
    }
}

