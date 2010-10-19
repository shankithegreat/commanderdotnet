namespace Nomad.Dialogs
{
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Archive.Wcx;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using Nomad.Workers;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class FilePackDialog : BasicDialog
    {
        private AutoCompleteProvider AutoComplete;
        private Button btnCancel;
        private Button btnConfigure;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkEncryptFileNames;
        private CheckBox chkShowPassword;
        private ComboBox cmbEncryptMethod;
        private FilterComboBox cmbFilter;
        private ComboBoxEx cmbFormat;
        private ComboBox cmbLevel;
        private ComboBox cmbMethod;
        private ComboBox cmbPackTo;
        private ComboBox cmbSolid;
        private ComboBox cmbThreadCount;
        private ComboBoxEx cmbUpdateMode;
        private IContainer components = null;
        private ArchiveUpdateMode[] CreateNewMode = new ArchiveUpdateMode[1];
        private IVirtualFolder FCurrentFolder;
        private IVirtualFile FDestArchiveFile;
        private string FDestSubFolder;
        private ArchiveUpdateMode[] FullModeList = EnumHelper.GetValues<ArchiveUpdateMode>();
        private GroupBox grpCompression;
        private GroupBox grpEncryption;
        private string LastFormatExt;
        private Label lblConfirmPassword;
        private Label lblEncryptMethod;
        private Label lblFilter;
        private Label lblFormat;
        private Label lblLevel;
        private Label lblMaxThreads;
        private Label lblMethod;
        private Label lblPackTo;
        private Label lblPassword;
        private Label lblProperties;
        private Label lblSolid;
        private Label lblThreadCount;
        private Label lblUpdateMode;
        private int ProcessorCount = Environment.ProcessorCount;
        private string SourceExt;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpButtons;
        private TableLayoutPanel tlpOptions;
        private TextBoxEx txtConfirmPassword;
        private TextBoxEx txtPassword;
        private TextBox txtProperties;
        private ValidatorProvider Validator;
        private string ValueNonSolid;
        private string ValueSolid;

        public FilePackDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sError;
            base.LocalizeForm();
            this.chkShowPassword_CheckedChanged(this.chkShowPassword, EventArgs.Empty);
            this.AutoComplete.PreviewEnvironmentVariable += new EventHandler<PreviewEnvironmentVariableEventArgs>(AutoCompleteEvents.PreviewEnvironmentVariable);
            this.AutoComplete.PreviewFileSystemInfo += new EventHandler<PreviewFileSystemInfoEventArgs>(AutoCompleteEvents.PreviewFileSystemInfo);
            this.AutoComplete.GetCustomSource += new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetComboBoxSource);
            this.ValueNonSolid = (this.cmbSolid.Items.Count > 0) ? ((string) this.cmbSolid.Items[0]) : "Non-Solid";
            this.ValueSolid = (this.cmbSolid.Items.Count > 1) ? ((string) this.cmbSolid.Items[1]) : "Solid";
            this.InitializeSolidSizes();
            this.cmbUpdateMode.DataSource = this.FullModeList;
            this.cmbUpdateMode.SelectedItem = ArchiveUpdateMode.OverwriteAll;
            this.lblMaxThreads.Text = string.Format(this.lblMaxThreads.Text, this.ProcessorCount);
            for (int i = 1; i <= (this.ProcessorCount * 2); i++)
            {
                this.cmbThreadCount.Items.Add(i);
            }
            this.cmbThreadCount.SelectedIndex = 0;
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            ((WcxFormatInfo) this.cmbFormat.SelectedItem).ShowConfigurePackerDialog(this);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.Validator.Validate(true))
            {
                if (!(string.IsNullOrEmpty(this.FDestSubFolder) || ((this.Format.Capabilities & ArchiveFormatCapabilities.MultiFileArchive) != 0)))
                {
                    MessageDialog.Show(this, string.Format(Resources.sArchiveDoNotSupportFolders, this.Format.Name), Resources.sWarning, MessageDialog.ButtonsOk);
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
                }
            }
            else if (!this.Validator.GetIsValid(this.cmbPackTo))
            {
                this.cmbPackTo.Select();
            }
            else if (!this.PasswordsMatch)
            {
                this.Validator.SetValidateError(this.txtPassword, Resources.sPasswordsNotMatch);
                this.Validator.SetValidateError(this.txtConfirmPassword, Resources.sPasswordsNotMatch);
                this.txtConfirmPassword.Select();
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = !this.chkShowPassword.Checked;
            this.txtPassword.UseSystemPasswordChar = flag;
            this.lblConfirmPassword.Enabled = flag;
            this.txtConfirmPassword.Enabled = flag;
            this.txtPassword.ShowCapsLock = flag;
            this.txtPassword.ShowInputLanguage = flag;
            this.txtConfirmPassword.ShowCapsLock = flag;
            this.txtConfirmPassword.ShowInputLanguage = flag;
        }

        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArchiveFormatInfo selectedItem = (ArchiveFormatInfo) this.cmbFormat.SelectedItem;
            this.grpEncryption.Enabled = (selectedItem.Capabilities & ArchiveFormatCapabilities.EncryptContent) > 0;
            SevenZipFormatInfo info2 = selectedItem as SevenZipFormatInfo;
            if (info2 != null)
            {
                this.cmbLevel.Enabled = true;
                this.cmbMethod.Enabled = true;
                this.cmbSolid.Enabled = (info2.SevenZipCapabilities & SevenZipFormatCapabilities.Solid) > 0;
                this.UpdateComboBox(this.cmbLevel, info2.SupportedLevels, CompressionLevel.Normal);
                if (!CompressionLevel.Store.Equals(this.cmbLevel.SelectedItem))
                {
                    this.UpdateComboBox(this.cmbMethod, info2.SupportedMethods, null);
                }
                if (this.cmbEncryptMethod.Enabled)
                {
                    this.UpdateComboBox(this.cmbEncryptMethod, info2.SupportedEncryption, null);
                }
                this.cmbThreadCount.Enabled = (this.ProcessorCount > 1) && ((info2.SevenZipCapabilities & SevenZipFormatCapabilities.MultiThread) > 0);
                this.chkEncryptFileNames.Enabled = (info2.SevenZipCapabilities & SevenZipFormatCapabilities.EncryptFileNames) > 0;
                this.txtProperties.Enabled = true;
                this.btnConfigure.Enabled = false;
            }
            else
            {
                this.cmbLevel.Enabled = false;
                this.cmbMethod.Enabled = false;
                this.cmbSolid.Enabled = false;
                this.cmbThreadCount.Enabled = false;
                this.chkEncryptFileNames.Enabled = false;
                this.txtProperties.Enabled = false;
                WcxFormatInfo info3 = selectedItem as WcxFormatInfo;
                this.btnConfigure.Enabled = (info3 != null) && ((info3.WcxCapabilities & PK_CAPS.PK_CAPS_OPTIONS) > 0);
            }
            this.cmbUpdateMode.DataSource = ((selectedItem.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0) ? this.FullModeList : this.CreateNewMode;
            this.lblLevel.Enabled = this.cmbLevel.Enabled;
            this.lblMethod.Enabled = this.cmbMethod.Enabled;
            this.lblSolid.Enabled = this.cmbSolid.Enabled;
            this.lblThreadCount.Enabled = this.cmbThreadCount.Enabled;
            this.lblMaxThreads.Enabled = this.cmbThreadCount.Enabled;
            this.lblProperties.Enabled = this.txtProperties.Enabled;
            if (!this.cmbLevel.Enabled)
            {
                this.cmbLevel.DataSource = null;
            }
            if (!this.cmbMethod.Enabled)
            {
                this.cmbMethod.DataSource = null;
            }
            if (!this.cmbSolid.Enabled)
            {
                this.cmbSolid.SelectedIndex = -1;
            }
            if (!this.cmbThreadCount.Enabled)
            {
                this.cmbThreadCount.SelectedIndex = 0;
            }
            if (!this.cmbEncryptMethod.Enabled)
            {
                this.cmbEncryptMethod.DataSource = null;
            }
            if (((this.LastFormatExt != null) && (selectedItem.Extension != null)) && (selectedItem.Extension.Length > 0))
            {
                string sourceExt = string.Empty;
                if (!(((info2 == null) || ((info2.SevenZipCapabilities & SevenZipFormatCapabilities.AppendExt) <= 0)) || string.IsNullOrEmpty(this.SourceExt)))
                {
                    sourceExt = this.SourceExt;
                }
                sourceExt = sourceExt + '.' + selectedItem.Extension[0];
                string packTo = this.PackTo;
                if (((PathHelper.GetPathType(packTo) & PathType.Uri) == PathType.Unknown) && packTo.EndsWith(this.LastFormatExt))
                {
                    this.cmbPackTo.Text = packTo.Substring(0, packTo.Length - this.LastFormatExt.Length) + sourceExt;
                    this.FDestArchiveFile = null;
                }
                this.LastFormatExt = sourceExt;
            }
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompressionLevel.Store.Equals(this.cmbLevel.SelectedItem))
            {
                CompressionMethod[] source = new CompressionMethod[1];
                this.UpdateComboBox(this.cmbMethod, source, null);
                this.cmbSolid.Enabled = false;
                this.cmbThreadCount.Enabled = false;
            }
            else
            {
                SevenZipFormatInfo selectedItem = this.cmbFormat.SelectedItem as SevenZipFormatInfo;
                if (selectedItem != null)
                {
                    this.UpdateComboBox(this.cmbMethod, selectedItem.SupportedMethods, null);
                    this.cmbSolid.Enabled = (selectedItem.SevenZipCapabilities & SevenZipFormatCapabilities.Solid) > 0;
                    this.cmbThreadCount.Enabled = (this.ProcessorCount > 1) && ((selectedItem.SevenZipCapabilities & SevenZipFormatCapabilities.MultiThread) > 0);
                }
            }
            this.lblSolid.Enabled = this.cmbSolid.Enabled;
            if (!this.cmbSolid.Enabled)
            {
                this.cmbSolid.SelectedIndex = -1;
            }
            else
            {
                switch (((CompressionLevel) this.cmbLevel.SelectedItem))
                {
                    case CompressionLevel.Fastest:
                        this.cmbSolid.SelectedItem = new SolidBlock(0x10, SevenZipPropertiesBuilder.SolidSizeUnit.Mb);
                        goto Label_0191;

                    case CompressionLevel.Fast:
                        this.cmbSolid.SelectedItem = new SolidBlock(0x80, SevenZipPropertiesBuilder.SolidSizeUnit.Mb);
                        goto Label_0191;

                    case CompressionLevel.Normal:
                        this.cmbSolid.SelectedItem = new SolidBlock(2, SevenZipPropertiesBuilder.SolidSizeUnit.Gb);
                        goto Label_0191;
                }
                this.cmbSolid.SelectedItem = new SolidBlock(4, SevenZipPropertiesBuilder.SolidSizeUnit.Gb);
            }
        Label_0191:
            this.lblThreadCount.Enabled = this.cmbThreadCount.Enabled;
            this.lblMaxThreads.Enabled = this.cmbThreadCount.Enabled;
            this.cmbThreadCount.SelectedItem = this.cmbThreadCount.Enabled ? this.ProcessorCount : 1;
        }

        private void cmbPackTo_TextUpdate(object sender, EventArgs e)
        {
            this.FDestArchiveFile = null;
        }

        private void cmbPackTo_Validating(object sender, CancelEventArgs e)
        {
            string packTo = this.PackTo;
            if (string.IsNullOrEmpty(packTo))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = PathHelper.GetPathType(packTo) == ~PathType.Unknown;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
                else
                {
                    try
                    {
                        this.DestArchiveFileNeeded();
                    }
                    catch (Exception exception)
                    {
                        e.Cancel = true;
                        this.Validator.SetValidateError((Control) sender, exception.Message);
                    }
                }
            }
        }

        private void cmbSolid_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is bool)
            {
                e.Value = ((bool) e.ListItem) ? this.ValueSolid : this.ValueNonSolid;
            }
            else if (e.ListItem is SolidBlock)
            {
                SolidBlock listItem = (SolidBlock) e.ListItem;
                switch (listItem.Unit)
                {
                    case SevenZipPropertiesBuilder.SolidSizeUnit.Kb:
                        e.Value = SizeTypeConverter.FormatSize<int>(listItem.Size * 0x400, SizeFormat.Kilobytes);
                        return;

                    case SevenZipPropertiesBuilder.SolidSizeUnit.Mb:
                        e.Value = SizeTypeConverter.FormatSize<int>((listItem.Size * 0x400) * 0x400, SizeFormat.Dynamic);
                        return;

                    case SevenZipPropertiesBuilder.SolidSizeUnit.Gb:
                        e.Value = SizeTypeConverter.FormatSize<long>(((listItem.Size * 0x400L) * 0x400L) * 0x400L, SizeFormat.Dynamic);
                        return;
                }
                e.Value = SizeTypeConverter.FormatSize<int>(listItem.Size, SizeFormat.Bytes);
            }
        }

        private void DestArchiveFileNeeded()
        {
            if (this.FDestArchiveFile == null)
            {
                string packTo = this.PackTo;
                if (string.IsNullOrEmpty(packTo))
                {
                    this.FDestArchiveFile = null;
                    this.FDestSubFolder = null;
                }
                else
                {
                    PathType pathType = PathHelper.GetPathType(packTo);
                    if (pathType == ~PathType.Unknown)
                    {
                        throw new ArgumentException(Resources.sErrorInvalidPath);
                    }
                    if ((pathType & PathType.Uri) > PathType.Unknown)
                    {
                        Uri uri = new Uri(packTo);
                        this.FDestArchiveFile = (IVirtualFile) VirtualItem.FromFullName(uri.GetComponents(UriComponents.PathAndQuery | UriComponents.Host | UriComponents.UserInfo | UriComponents.Scheme, UriFormat.Unescaped), VirtualItemType.File);
                        this.FDestSubFolder = uri.GetComponents(UriComponents.Fragment, UriFormat.Unescaped);
                    }
                    else
                    {
                        if ((pathType & PathType.Folder) > PathType.Unknown)
                        {
                            throw new ArgumentException(Resources.sCannotPackToFolder);
                        }
                        this.FDestArchiveFile = (IVirtualFile) VirtualItem.FromFullName(packTo, VirtualItemType.File, this.CurrentFolder);
                        this.FDestSubFolder = null;
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

        public bool Execute(IWin32Window owner, IEnumerable<IVirtualItem> items, IVirtualFolder destFolder)
        {
            ArchiveFormatInfo format;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            IVirtualItem item = this.InitializeFormats(items);
            ArchiveFolder folder = destFolder as ArchiveFolder;
            if (folder != null)
            {
                format = folder[ArchiveProperty.ArchiveFormat] as ArchiveFormatInfo;
                if (format == null)
                {
                    string str = folder[ArchiveProperty.ArchiveFormat] as string;
                    if (!string.IsNullOrEmpty(str))
                    {
                        format = ArchiveFormatManager.GetFormat(str);
                    }
                }
                if ((format != null) && ((format.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0))
                {
                    this.cmbPackTo.Text = folder.FullName;
                    this.cmbFormat.SelectedItem = format;
                }
                else
                {
                    destFolder = destFolder.Parent;
                    while (destFolder is ArchiveFolder)
                    {
                        destFolder = destFolder.Parent;
                    }
                }
            }
            if ((this.cmbFormat.SelectedIndex < 0) && (this.cmbFormat.Items.Count > 0))
            {
                this.cmbFormat.SelectedIndex = 0;
            }
            if (string.IsNullOrEmpty(this.cmbPackTo.Text))
            {
                string fileNameWithoutExtension = string.Empty;
                if (item != null)
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.Name);
                    this.SourceExt = Path.GetExtension(item.Name);
                }
                else if (this.CurrentFolder != null)
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.CurrentFolder.Name);
                }
                StringBuilder builder = new StringBuilder();
                if (destFolder != null)
                {
                    builder.Append(Path.Combine(destFolder.FullName, fileNameWithoutExtension));
                }
                else
                {
                    builder.Append(fileNameWithoutExtension);
                }
                format = this.cmbFormat.SelectedItem as ArchiveFormatInfo;
                if (format != null)
                {
                    string[] extension = format.Extension;
                    if ((extension != null) && (extension.Length > 0))
                    {
                        this.LastFormatExt = string.Empty;
                        SevenZipFormatInfo info2 = format as SevenZipFormatInfo;
                        if (!(((info2 == null) || ((info2.SevenZipCapabilities & SevenZipFormatCapabilities.AppendExt) <= 0)) || string.IsNullOrEmpty(this.SourceExt)))
                        {
                            this.LastFormatExt = this.SourceExt;
                        }
                        this.LastFormatExt = this.LastFormatExt + '.' + extension[0];
                        builder.Append(this.LastFormatExt);
                    }
                }
                this.cmbPackTo.Text = builder.ToString();
            }
            HistorySettings.PopulateComboBox(this.cmbPackTo, HistorySettings.Default.PackTo);
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                HistorySettings.Default.AddStringToPackTo(this.PackTo);
                if (this.cmbFilter.Enabled)
                {
                    HistorySettings.Default.AddStringToCopyFilter(this.cmbFilter.FilterString);
                }
                return true;
            }
            return false;
        }

        public bool Execute(IWin32Window owner, IEnumerable<IVirtualItem> items, IChangeVirtualFile destFile, ArchiveFormatInfo format)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (destFile == null)
            {
                throw new ArgumentNullException("destFile");
            }
            IVirtualItem item = this.InitializeFormats(items);
            this.cmbPackTo.Text = destFile.FullName;
            if ((format != null) && ((format.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0))
            {
                this.cmbFormat.SelectedItem = format;
            }
            if ((this.cmbFormat.SelectedIndex < 0) && (this.cmbFormat.Items.Count > 0))
            {
                this.cmbFormat.SelectedIndex = 0;
            }
            HistorySettings.PopulateComboBox(this.cmbPackTo, HistorySettings.Default.PackTo);
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                HistorySettings.Default.AddStringToPackTo(this.PackTo);
                if (this.cmbFilter.Enabled)
                {
                    HistorySettings.Default.AddStringToCopyFilter(this.cmbFilter.FilterString);
                }
                return true;
            }
            return false;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FilePackDialog));
            this.lblPassword = new Label();
            this.txtPassword = new TextBoxEx();
            this.cmbEncryptMethod = new ComboBox();
            this.lblEncryptMethod = new Label();
            this.lblConfirmPassword = new Label();
            this.chkShowPassword = new CheckBox();
            this.txtConfirmPassword = new TextBoxEx();
            this.chkEncryptFileNames = new CheckBox();
            this.lblMethod = new Label();
            this.lblLevel = new Label();
            this.lblSolid = new Label();
            this.cmbSolid = new ComboBox();
            this.cmbLevel = new ComboBox();
            this.cmbMethod = new ComboBox();
            this.cmbThreadCount = new ComboBox();
            this.lblThreadCount = new Label();
            this.lblMaxThreads = new Label();
            this.lblPackTo = new Label();
            this.cmbPackTo = new ComboBox();
            this.lblFormat = new Label();
            this.cmbFormat = new ComboBoxEx();
            this.grpEncryption = new GroupBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.btnConfigure = new Button();
            this.grpCompression = new GroupBox();
            this.tlpBack = new TableLayoutPanel();
            this.cmbUpdateMode = new ComboBoxEx();
            this.lblUpdateMode = new Label();
            this.lblFilter = new Label();
            this.cmbFilter = new FilterComboBox();
            this.lblProperties = new Label();
            this.txtProperties = new TextBox();
            this.tlpOptions = new TableLayoutPanel();
            this.tlpButtons = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            this.Validator = new ValidatorProvider();
            this.AutoComplete = new AutoCompleteProvider();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            this.grpEncryption.SuspendLayout();
            this.grpCompression.SuspendLayout();
            this.tlpBack.SuspendLayout();
            this.tlpOptions.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpEncryption");
            panel.Controls.Add(this.lblPassword, 0, 0);
            panel.Controls.Add(this.txtPassword, 1, 0);
            panel.Controls.Add(this.cmbEncryptMethod, 1, 3);
            panel.Controls.Add(this.lblEncryptMethod, 0, 3);
            panel.Controls.Add(this.lblConfirmPassword, 0, 1);
            panel.Controls.Add(this.chkShowPassword, 0, 2);
            panel.Controls.Add(this.txtConfirmPassword, 1, 1);
            panel.Controls.Add(this.chkEncryptFileNames, 0, 4);
            panel.MinimumSize = new Size(0xe4, 0);
            panel.Name = "tlpEncryption";
            manager.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            manager.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new EventHandler(this.txtPassword_TextChange);
            this.txtPassword.Validating += new CancelEventHandler(this.txtPassword_Validating);
            manager.ApplyResources(this.cmbEncryptMethod, "cmbEncryptMethod");
            this.cmbEncryptMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEncryptMethod.FormattingEnabled = true;
            this.cmbEncryptMethod.Name = "cmbEncryptMethod";
            manager.ApplyResources(this.lblEncryptMethod, "lblEncryptMethod");
            this.lblEncryptMethod.Name = "lblEncryptMethod";
            manager.ApplyResources(this.lblConfirmPassword, "lblConfirmPassword");
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            manager.ApplyResources(this.chkShowPassword, "chkShowPassword");
            panel.SetColumnSpan(this.chkShowPassword, 2);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new EventHandler(this.chkShowPassword_CheckedChanged);
            manager.ApplyResources(this.txtConfirmPassword, "txtConfirmPassword");
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            this.txtConfirmPassword.TextChanged += new EventHandler(this.txtPassword_TextChange);
            this.txtConfirmPassword.Validating += new CancelEventHandler(this.txtPassword_Validating);
            manager.ApplyResources(this.chkEncryptFileNames, "chkEncryptFileNames");
            panel.SetColumnSpan(this.chkEncryptFileNames, 2);
            this.chkEncryptFileNames.Name = "chkEncryptFileNames";
            this.chkEncryptFileNames.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel2, "tlpCompression");
            panel2.Controls.Add(this.lblMethod, 0, 3);
            panel2.Controls.Add(this.lblLevel, 0, 2);
            panel2.Controls.Add(this.lblSolid, 0, 4);
            panel2.Controls.Add(this.cmbSolid, 1, 4);
            panel2.Controls.Add(this.cmbLevel, 1, 2);
            panel2.Controls.Add(this.cmbMethod, 1, 3);
            panel2.Controls.Add(this.cmbThreadCount, 1, 5);
            panel2.Controls.Add(this.lblThreadCount, 0, 5);
            panel2.Controls.Add(this.lblMaxThreads, 2, 5);
            panel2.Name = "tlpCompression";
            manager.ApplyResources(this.lblMethod, "lblMethod");
            this.lblMethod.Name = "lblMethod";
            manager.ApplyResources(this.lblLevel, "lblLevel");
            this.lblLevel.Name = "lblLevel";
            manager.ApplyResources(this.lblSolid, "lblSolid");
            this.lblSolid.Name = "lblSolid";
            panel2.SetColumnSpan(this.cmbSolid, 2);
            manager.ApplyResources(this.cmbSolid, "cmbSolid");
            this.cmbSolid.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSolid.FormattingEnabled = true;
            this.cmbSolid.Items.AddRange(new object[] { manager.GetString("cmbSolid.Items"), manager.GetString("cmbSolid.Items1") });
            this.cmbSolid.Name = "cmbSolid";
            this.cmbSolid.Format += new ListControlConvertEventHandler(this.cmbSolid_Format);
            panel2.SetColumnSpan(this.cmbLevel, 2);
            manager.ApplyResources(this.cmbLevel, "cmbLevel");
            this.cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Items.AddRange(new object[] { manager.GetString("cmbLevel.Items"), manager.GetString("cmbLevel.Items1"), manager.GetString("cmbLevel.Items2"), manager.GetString("cmbLevel.Items3"), manager.GetString("cmbLevel.Items4"), manager.GetString("cmbLevel.Items5") });
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.SelectedIndexChanged += new EventHandler(this.cmbLevel_SelectedIndexChanged);
            panel2.SetColumnSpan(this.cmbMethod, 2);
            manager.ApplyResources(this.cmbMethod, "cmbMethod");
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Name = "cmbMethod";
            this.cmbThreadCount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbThreadCount.FormattingEnabled = true;
            manager.ApplyResources(this.cmbThreadCount, "cmbThreadCount");
            this.cmbThreadCount.Name = "cmbThreadCount";
            manager.ApplyResources(this.lblThreadCount, "lblThreadCount");
            this.lblThreadCount.Name = "lblThreadCount";
            manager.ApplyResources(this.lblMaxThreads, "lblMaxThreads");
            this.lblMaxThreads.Name = "lblMaxThreads";
            manager.ApplyResources(this.lblPackTo, "lblPackTo");
            this.tlpBack.SetColumnSpan(this.lblPackTo, 3);
            this.lblPackTo.Name = "lblPackTo";
            this.AutoComplete.SetAutoComplete(this.cmbPackTo, true);
            this.tlpBack.SetColumnSpan(this.cmbPackTo, 4);
            manager.ApplyResources(this.cmbPackTo, "cmbPackTo");
            this.cmbPackTo.Name = "cmbPackTo";
            this.Validator.SetValidateOn(this.cmbPackTo, ValidateOn.TextChangedTimer);
            this.cmbPackTo.Validating += new CancelEventHandler(this.cmbPackTo_Validating);
            this.cmbPackTo.SelectionChangeCommitted += new EventHandler(this.cmbPackTo_TextUpdate);
            this.cmbPackTo.TextUpdate += new EventHandler(this.cmbPackTo_TextUpdate);
            manager.ApplyResources(this.lblFormat, "lblFormat");
            this.lblFormat.Name = "lblFormat";
            manager.ApplyResources(this.cmbFormat, "cmbFormat");
            this.cmbFormat.DisplayMember = "Name";
            this.cmbFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.MinimumSize = new Size(100, 0);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.SelectedIndexChanged += new EventHandler(this.cmbFormat_SelectedIndexChanged);
            manager.ApplyResources(this.grpEncryption, "grpEncryption");
            this.grpEncryption.Controls.Add(panel);
            this.grpEncryption.Name = "grpEncryption";
            this.grpEncryption.TabStop = false;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnConfigure, "btnConfigure");
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new EventHandler(this.btnConfigure_Click);
            manager.ApplyResources(this.grpCompression, "grpCompression");
            this.grpCompression.Controls.Add(panel2);
            this.grpCompression.Name = "grpCompression";
            this.grpCompression.TabStop = false;
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.lblFormat, 0, 2);
            this.tlpBack.Controls.Add(this.lblPackTo, 0, 0);
            this.tlpBack.Controls.Add(this.cmbFormat, 0, 3);
            this.tlpBack.Controls.Add(this.btnConfigure, 1, 3);
            this.tlpBack.Controls.Add(this.cmbPackTo, 0, 1);
            this.tlpBack.Controls.Add(this.cmbUpdateMode, 2, 3);
            this.tlpBack.Controls.Add(this.lblUpdateMode, 2, 2);
            this.tlpBack.Controls.Add(this.lblFilter, 3, 2);
            this.tlpBack.Controls.Add(this.cmbFilter, 3, 3);
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.cmbUpdateMode, "cmbUpdateMode");
            this.cmbUpdateMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUpdateMode.FormattingEnabled = true;
            this.cmbUpdateMode.MinimumSize = new Size(100, 0);
            this.cmbUpdateMode.Name = "cmbUpdateMode";
            manager.ApplyResources(this.lblUpdateMode, "lblUpdateMode");
            this.lblUpdateMode.Name = "lblUpdateMode";
            manager.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            manager.ApplyResources(this.cmbFilter, "cmbFilter");
            this.cmbFilter.DrawMode = DrawMode.OwnerDrawFixed;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Name = "cmbFilter";
            manager.ApplyResources(this.lblProperties, "lblProperties");
            this.lblProperties.Name = "lblProperties";
            this.tlpOptions.SetColumnSpan(this.txtProperties, 2);
            manager.ApplyResources(this.txtProperties, "txtProperties");
            this.txtProperties.Name = "txtProperties";
            manager.ApplyResources(this.tlpOptions, "tlpOptions");
            this.tlpOptions.Controls.Add(this.grpCompression, 0, 0);
            this.tlpOptions.Controls.Add(this.grpEncryption, 1, 0);
            this.tlpOptions.Controls.Add(this.lblProperties, 0, 1);
            this.tlpOptions.Controls.Add(this.txtProperties, 0, 2);
            this.tlpOptions.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpOptions.Name = "tlpOptions";
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            this.Validator.Validating += new EventHandler<CancelEventArgs>(this.Validator_Validating);
            this.AutoComplete.UseCustomSource = Settings.Default.UseACSRecentItems;
            this.AutoComplete.UseEnvironmentVariablesSource = Settings.Default.UseACSEnvironmentVariables;
            this.AutoComplete.UseFileSystemSource = Settings.Default.UseACSFileSystem;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpOptions);
            base.Controls.Add(this.tlpBack);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FilePackDialog";
            base.ShowInTaskbar = false;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            this.grpEncryption.ResumeLayout(false);
            this.grpEncryption.PerformLayout();
            this.grpCompression.ResumeLayout(false);
            this.grpCompression.PerformLayout();
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.tlpOptions.ResumeLayout(false);
            this.tlpOptions.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private IVirtualItem InitializeFormats(IEnumerable<IVirtualItem> items)
        {
            int num = items.Count<IVirtualItem>();
            bool flag = false;
            IVirtualItem item = null;
            foreach (IVirtualItem item2 in items)
            {
                if (item == null)
                {
                    item = item2;
                }
                flag = item2 is IVirtualFolder;
                if (flag)
                {
                    break;
                }
            }
            if (num == 1)
            {
                this.lblPackTo.Text = string.Format((item is IVirtualFolder) ? Resources.sPackFolder : Resources.sPackFile, item.Name);
            }
            else
            {
                this.lblPackTo.Text = PluralInfo.Format(Resources.sPackMultipleFile, new object[] { num });
            }
            ArchiveFormatCapabilities createArchive = ArchiveFormatCapabilities.CreateArchive;
            if (flag || (num > 1))
            {
                createArchive |= ArchiveFormatCapabilities.MultiFileArchive;
            }
            List<ArchiveFormatInfo> list = new List<ArchiveFormatInfo>();
            foreach (ArchiveFormatInfo info in ArchiveFormatManager.GetFormats())
            {
                if ((info.Capabilities & createArchive) == createArchive)
                {
                    list.Add(info);
                }
            }
            list.Sort(delegate (ArchiveFormatInfo x, ArchiveFormatInfo y) {
                return string.Compare(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);
            });
            this.cmbFormat.DataSource = list;
            this.cmbFormat.Enabled = this.cmbFormat.Items.Count > 0;
            this.lblFormat.Enabled = this.cmbFormat.Enabled;
            if (!this.cmbFormat.Enabled)
            {
                this.cmbFormat.DataSource = new string[] { Resources.sNotAvailable };
            }
            this.cmbUpdateMode.Enabled = this.cmbFormat.Enabled;
            this.lblUpdateMode.Enabled = this.cmbUpdateMode.Enabled;
            this.cmbFilter.Enabled = this.cmbFormat.Enabled && flag;
            this.lblFilter.Enabled = this.cmbFilter.Enabled;
            this.tlpOptions.Enabled = this.cmbFormat.Enabled;
            return ((num == 1) ? item : null);
        }

        private void InitializeSolidSizes()
        {
            this.cmbSolid.BeginUpdate();
            this.cmbSolid.Items.Clear();
            this.cmbSolid.Items.Add(false);
            NameValueCollection section = ConfigurationManager.GetSection("solidSizes") as NameValueCollection;
            if (section != null)
            {
                for (int i = 0; i < section.Count; i++)
                {
                    string str = section[i];
                    try
                    {
                        for (int j = 0; j < str.Length; j++)
                        {
                            if (!char.IsDigit(str[j]))
                            {
                                int size = int.Parse(str.Substring(0, j));
                                SevenZipPropertiesBuilder.SolidSizeUnit unit = (SevenZipPropertiesBuilder.SolidSizeUnit) Enum.Parse(typeof(SevenZipPropertiesBuilder.SolidSizeUnit), str.Substring(j));
                                this.cmbSolid.Items.Add(new SolidBlock(size, unit));
                                goto Label_00E7;
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                Label_00E7:;
                }
            }
            this.cmbSolid.Items.Add(true);
            this.cmbSolid.EndUpdate();
        }

        protected override void OnShown(EventArgs e)
        {
            this.cmbPackTo.AutoCompleteMode = Settings.Default.AutoCompleteMode;
            base.OnShown(e);
            this.Validator.Validate();
        }

        private void txtPassword_TextChange(object sender, EventArgs e)
        {
            this.Validator.RemoveValidateError(this.txtPassword);
            this.Validator.RemoveValidateError(this.txtConfirmPassword);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.PasswordsMatch;
        }

        private void UpdateComboBox(ComboBox box, object source, object defaultValue)
        {
            box.BeginUpdate();
            object obj2 = (box.SelectedItem == null) ? defaultValue : box.SelectedItem;
            box.DataSource = source;
            box.SelectedItem = obj2;
            if (box.SelectedIndex < 0)
            {
                box.SelectedIndex = 0;
            }
            box.EndUpdate();
        }

        private void Validator_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel |= !(this.cmbFormat.SelectedItem is ArchiveFormatInfo);
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

        public IVirtualFile DestArchiveFile
        {
            get
            {
                this.DestArchiveFileNeeded();
                return this.FDestArchiveFile;
            }
        }

        public string DestSubFolder
        {
            get
            {
                this.DestArchiveFileNeeded();
                return this.FDestSubFolder;
            }
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                return this.cmbFilter.Filter;
            }
        }

        public ArchiveFormatInfo Format
        {
            get
            {
                return (this.cmbFormat.SelectedItem as ArchiveFormatInfo);
            }
        }

        private string PackTo
        {
            get
            {
                return this.cmbPackTo.Text.Trim();
            }
        }

        public string Password
        {
            get
            {
                return this.txtPassword.Text;
            }
        }

        private bool PasswordsMatch
        {
            get
            {
                return ((this.chkShowPassword.Checked || ((this.txtPassword.Text == string.Empty) && (this.txtConfirmPassword.Text == string.Empty))) || this.txtConfirmPassword.Text.Equals(this.txtPassword.Text));
            }
        }

        public SevenZipPropertiesBuilder SevenZipProperties
        {
            get
            {
                SevenZipFormatInfo selectedItem = this.cmbFormat.SelectedItem as SevenZipFormatInfo;
                if (selectedItem == null)
                {
                    return null;
                }
                SevenZipPropertiesBuilder builder = new SevenZipPropertiesBuilder(selectedItem.KnownFormat);
                builder.SetLevel((CompressionLevel) this.cmbLevel.SelectedItem);
                builder.SetMethod((CompressionMethod) this.cmbMethod.SelectedItem);
                if (this.cmbThreadCount.Enabled)
                {
                    builder.SetThreadCount((int) this.cmbThreadCount.SelectedItem);
                }
                if (this.cmbSolid.Enabled && (this.cmbSolid.SelectedItem != null))
                {
                    if (this.cmbSolid.SelectedItem is bool)
                    {
                        builder.SetSolid((bool) this.cmbSolid.SelectedItem);
                    }
                    else
                    {
                        SolidBlock block = (SolidBlock) this.cmbSolid.SelectedItem;
                        builder.SetSolidSize(block.Size, block.Unit);
                    }
                }
                if (this.cmbEncryptMethod.Enabled)
                {
                    builder.SetEncryptionMethod((EncryptionMethod) this.cmbEncryptMethod.SelectedItem);
                }
                if (this.chkEncryptFileNames.Enabled)
                {
                    builder.SetEncryptHeaders(this.chkEncryptFileNames.Checked);
                }
                if (this.txtProperties.Enabled)
                {
                    builder.SetProperties(this.txtProperties.Text);
                }
                return builder;
            }
        }

        public ArchiveUpdateMode UpdateMode
        {
            get
            {
                return (ArchiveUpdateMode) this.cmbUpdateMode.SelectedItem;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SolidBlock
        {
            public readonly int Size;
            public readonly SevenZipPropertiesBuilder.SolidSizeUnit Unit;
            public SolidBlock(int size, SevenZipPropertiesBuilder.SolidSizeUnit unit)
            {
                this.Size = size;
                this.Unit = unit;
            }
        }
    }
}

