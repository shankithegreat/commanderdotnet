namespace Nomad.Dialogs
{
    using Microsoft;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Option;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Security.Principal;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class SetupAndUpgradeDialog : BasicDialog, IUpdateCulture
    {
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnHideDetails;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnShowDetails;
        private Bevel bvlButtons;
        private Bevel bvlCaption;
        private System.Windows.Forms.CheckBox chkImportBookmarks;
        private System.Windows.Forms.CheckBox chkImportTools;
        private System.Windows.Forms.CheckBox chkOptimizeFiles;
        private System.Windows.Forms.CheckBox chkPerformImport;
        private System.Windows.Forms.CheckBox chkPerformInitialInit;
        private System.Windows.Forms.CheckBox chkPerformUserConfigSetup;
        private System.Windows.Forms.CheckBox chkPlaceShortcut;
        private ColumnHeader clOperation;
        private ColumnHeader clStatus;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.ComboBox cmbPreviousVersion;
        private IContainer components = null;
        private bool IsUserConfigControlInitialized;
        private Label lblLanguageFile;
        private LinkLabel lblLicense;
        private Label lblPageCaption;
        private Label lblProgressUpgrade;
        private string LocalizedStringFinish;
        private string LocalizedStringNext;
        private ListViewEx lvProcessDetails;
        private MessageDialogResult OverwriteResult;
        private VistaProgressBar ProcessProgressBar;
        private System.Windows.Forms.RadioButton rbImportCopy;
        private System.Windows.Forms.RadioButton rbImportSkip;
        private System.Windows.Forms.RadioButton rbImportUpgrade;
        private BackgroundWorker SetupWorker;
        private bool ShieldRequired;
        private TabStripPage TabPageImport;
        private TabStripPage TabPageProcess;
        private TabStripPage TabPageUserConfig;
        private TabStripPage TabPageWelcome;
        private const int TaskImportBookmarks = 3;
        private const int TaskImportCopy = 1;
        private const int TaskImportTools = 4;
        private const int TaskImportUpgrade = 2;
        private const int TaskUserConfigSetup = 0;
        private TableLayoutPanel tlpProcess;
        private ConfigPlacementOptionControl UserConfigControl;
        private TabPageSwitcher WizardPageSwitcher;

        public SetupAndUpgradeDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.UpdateCulture();
            this.btnShowDetails.ForeColor = this.lblLicense.LinkColor;
            this.btnShowDetails.FlatAppearance.MouseDownBackColor = this.btnShowDetails.BackColor;
            this.btnShowDetails.FlatAppearance.MouseOverBackColor = this.btnShowDetails.BackColor;
            this.btnHideDetails.ForeColor = this.lblLicense.LinkColor;
            this.btnHideDetails.FlatAppearance.MouseDownBackColor = this.btnHideDetails.BackColor;
            this.btnHideDetails.FlatAppearance.MouseOverBackColor = this.btnHideDetails.BackColor;
            this.InitializeWelcomePage();
            switch (OS.ElevationType)
            {
                case ElevationType.Default:
                {
                    WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                    this.ShieldRequired = !principal.IsInRole(WindowsBuiltInRole.Administrator);
                    break;
                }
                case ElevationType.Limited:
                    this.ShieldRequired = true;
                    break;
            }
            this.TabPageWelcome.Activate();
            this.UpdateDetailsListView();
            this.UpdateButtons();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageImport)
            {
                if (this.chkPerformUserConfigSetup.Checked)
                {
                    this.TabPageUserConfig.Activate();
                }
                else
                {
                    this.TabPageWelcome.Activate();
                }
            }
            else if (this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageUserConfig)
            {
                this.TabPageWelcome.Activate();
            }
            this.UpdateButtons();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageWelcome)
            {
                if (this.chkPerformUserConfigSetup.Checked)
                {
                    this.TabPageUserConfig.Activate();
                }
                else if (this.chkPerformImport.Checked)
                {
                    this.TabPageImport.Activate();
                }
                else if ((this.chkPerformInitialInit.Checked || this.chkPlaceShortcut.Checked) || this.chkOptimizeFiles.Checked)
                {
                    this.Process();
                }
                else
                {
                    this.ProcessCulture();
                    base.DialogResult = DialogResult.OK;
                }
            }
            else if (this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageUserConfig)
            {
                if (this.UserConfigControl.ValidateChildren())
                {
                    if (this.chkPerformImport.Checked)
                    {
                        this.TabPageImport.Activate();
                    }
                    else
                    {
                        this.Process();
                    }
                }
            }
            else if (this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageImport)
            {
                this.Process();
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
            this.UpdateButtons();
        }

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            this.tlpProcess.SuspendLayout();
            this.btnShowDetails.Visible = !this.btnShowDetails.Visible;
            this.btnHideDetails.Visible = !this.btnHideDetails.Visible;
            this.lvProcessDetails.Visible = !this.lvProcessDetails.Visible;
            this.tlpProcess.ResumeLayout();
        }

        private void chkPerformUserConfigSetup_CheckStateChanged(object sender, EventArgs e)
        {
            this.UpdateNextButtonText();
        }

        private void cmbLanguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FormStringLocalizer.LocalizeForm(this, (CultureInfo) this.cmbLanguage.SelectedItem);
            this.UpdateButtons();
        }

        private void cmbPreviousVersion_Format(object sender, ListControlConvertEventArgs e)
        {
            e.Value = ((SettingsManager.PreviousVersionConfig) e.ListItem).Version.ToString();
        }

        private void cmbPreviousVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.PreviousVersionConfig selectedItem = (SettingsManager.PreviousVersionConfig) this.cmbPreviousVersion.SelectedItem;
            this.rbImportCopy.Enabled = (selectedItem.Version.Major >= 2) && (selectedItem.Version.Minor >= 6);
            this.rbImportCopy.Checked = this.rbImportCopy.Enabled;
            this.rbImportUpgrade.Checked = !this.rbImportCopy.Checked;
            bool flag = string.Equals(selectedItem.BookmarksDir, SettingsManager.SpecialFolders.Bookmarks, StringComparison.OrdinalIgnoreCase);
            this.chkImportBookmarks.Enabled = !string.IsNullOrEmpty(selectedItem.BookmarksDir) && !flag;
            this.chkImportBookmarks.Checked = this.chkImportBookmarks.Enabled || flag;
            bool flag2 = string.Equals(selectedItem.ToolsDir, SettingsManager.SpecialFolders.Tools, StringComparison.OrdinalIgnoreCase);
            this.chkImportTools.Enabled = !string.IsNullOrEmpty(selectedItem.ToolsDir) && !flag2;
            this.chkImportTools.Checked = this.chkImportTools.Enabled || flag2;
        }

        private bool CopyFiles(string source, string dest, string text, string caption)
        {
            if (string.Equals(source, dest, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            bool flag = false;
            Directory.CreateDirectory(dest);
            MessageDialogResult[] buttons = new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.YesToAll, MessageDialogResult.Skip };
            foreach (string str in Directory.GetFiles(source))
            {
                string path = Path.Combine(dest, Path.GetFileName(str));
                if (File.Exists(path) && (this.OverwriteResult != MessageDialogResult.Yes))
                {
                    switch (MessageDialog.Show(this, string.Format(text, Path.GetFileName(path)), caption, buttons, MessageBoxIcon.Exclamation))
                    {
                        case MessageDialogResult.Yes:
                            goto Label_00BF;

                        case MessageDialogResult.No:
                            goto Label_00CB;

                        case MessageDialogResult.YesToAll:
                            this.OverwriteResult = MessageDialogResult.Yes;
                            goto Label_00BF;
                    }
                    goto Label_00CB;
                }
            Label_00BF:
                File.Copy(str, path, true);
                flag = true;
            Label_00CB:;
            }
            return flag;
        }

        private bool CopyFolder(string source, string dest, string text, string caption)
        {
            bool flag = this.CopyFiles(source, dest, text, caption);
            foreach (string str in Directory.GetDirectories(source))
            {
                if (!string.Equals(str, "Bookmarks") && !string.Equals(str, "Tools"))
                {
                    flag |= this.CopyFolder(str, Path.Combine(dest, Path.GetFileName(str)), text, caption);
                }
            }
            return flag;
        }

        private ListViewItem CreateTaskItem(InitTask task)
        {
            return this.CreateTaskItem(task, 1);
        }

        private ListViewItem CreateTaskItem(InitTask task, int weight)
        {
            ListViewItem item = new ListViewItem();
            item.Text = TypeDescriptor.GetConverter(typeof(InitTask)).ConvertToString(task);
            item.StateImageIndex = weight;
            item.Tag = task;
            return item;
        }

        private ListViewItem CreateTaskItem(string text, int task)
        {
            return new ListViewItem(text) { StateImageIndex = 1, Tag = task };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private SettingsManager.PreviousVersionConfig GetPreviousVersion()
        {
            return (base.InvokeRequired ? ((SettingsManager.PreviousVersionConfig) base.Invoke(delegate {
                return this.cmbPreviousVersion.SelectedItem;
            })) : ((SettingsManager.PreviousVersionConfig) this.cmbPreviousVersion.SelectedItem));
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SetupAndUpgradeDialog));
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.chkPerformUserConfigSetup = new System.Windows.Forms.CheckBox();
            this.chkPerformImport = new System.Windows.Forms.CheckBox();
            this.lblLanguageFile = new Label();
            this.chkPerformInitialInit = new System.Windows.Forms.CheckBox();
            this.lblLicense = new LinkLabel();
            this.chkOptimizeFiles = new System.Windows.Forms.CheckBox();
            this.chkPlaceShortcut = new System.Windows.Forms.CheckBox();
            this.rbImportCopy = new System.Windows.Forms.RadioButton();
            this.rbImportUpgrade = new System.Windows.Forms.RadioButton();
            this.chkImportBookmarks = new System.Windows.Forms.CheckBox();
            this.chkImportTools = new System.Windows.Forms.CheckBox();
            this.cmbPreviousVersion = new System.Windows.Forms.ComboBox();
            this.rbImportSkip = new System.Windows.Forms.RadioButton();
            this.lblPageCaption = new Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.lblProgressUpgrade = new Label();
            this.WizardPageSwitcher = new TabPageSwitcher();
            this.TabPageWelcome = new TabStripPage();
            this.TabPageProcess = new TabStripPage();
            this.tlpProcess = new TableLayoutPanel();
            this.lvProcessDetails = new ListViewEx();
            this.clOperation = new ColumnHeader();
            this.clStatus = new ColumnHeader();
            this.btnHideDetails = new System.Windows.Forms.Button();
            this.btnShowDetails = new System.Windows.Forms.Button();
            this.ProcessProgressBar = new VistaProgressBar();
            this.TabPageImport = new TabStripPage();
            this.TabPageUserConfig = new TabStripPage();
            this.UserConfigControl = new ConfigPlacementOptionControl();
            this.bvlButtons = new Bevel();
            this.bvlCaption = new Bevel();
            this.SetupWorker = new BackgroundWorker();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            Label label4 = new Label();
            Label label5 = new Label();
            Label label6 = new Label();
            Label label7 = new Label();
            Panel panel3 = new Panel();
            TableLayoutPanel panel4 = new TableLayoutPanel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            this.WizardPageSwitcher.SuspendLayout();
            this.TabPageWelcome.SuspendLayout();
            this.TabPageProcess.SuspendLayout();
            this.tlpProcess.SuspendLayout();
            this.TabPageImport.SuspendLayout();
            this.TabPageUserConfig.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBackWelcome");
            panel.Controls.Add(control, 0, 0);
            panel.Controls.Add(label2, 0, 1);
            panel.Controls.Add(label3, 0, 3);
            panel.Controls.Add(this.cmbLanguage, 1, 2);
            panel.Controls.Add(this.chkPerformUserConfigSetup, 1, 4);
            panel.Controls.Add(this.chkPerformImport, 1, 5);
            panel.Controls.Add(this.lblLanguageFile, 2, 2);
            panel.Controls.Add(this.chkPerformInitialInit, 1, 6);
            panel.Controls.Add(this.lblLicense, 0, 10);
            panel.Controls.Add(this.chkOptimizeFiles, 1, 8);
            panel.Controls.Add(this.chkPlaceShortcut, 1, 7);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBackWelcome";
            manager.ApplyResources(control, "lblWelcomeMsg");
            panel.SetColumnSpan(control, 3);
            control.Name = "lblWelcomeMsg";
            control.UseMnemonic = false;
            manager.ApplyResources(label2, "lblSelectLanguage");
            panel.SetColumnSpan(label2, 3);
            label2.Name = "lblSelectLanguage";
            manager.ApplyResources(label3, "lblSelectInitialTasks");
            panel.SetColumnSpan(label3, 3);
            label3.Name = "lblSelectInitialTasks";
            this.cmbLanguage.DisplayMember = "DisplayName";
            this.cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            manager.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.ValueMember = "Name";
            this.cmbLanguage.SelectionChangeCommitted += new EventHandler(this.cmbLanguage_SelectionChangeCommitted);
            manager.ApplyResources(this.chkPerformUserConfigSetup, "chkPerformUserConfigSetup");
            panel.SetColumnSpan(this.chkPerformUserConfigSetup, 2);
            this.chkPerformUserConfigSetup.Name = "chkPerformUserConfigSetup";
            this.chkPerformUserConfigSetup.UseVisualStyleBackColor = true;
            this.chkPerformUserConfigSetup.CheckStateChanged += new EventHandler(this.chkPerformUserConfigSetup_CheckStateChanged);
            manager.ApplyResources(this.chkPerformImport, "chkPerformImport");
            panel.SetColumnSpan(this.chkPerformImport, 2);
            this.chkPerformImport.Name = "chkPerformImport";
            this.chkPerformImport.UseVisualStyleBackColor = true;
            this.chkPerformImport.CheckStateChanged += new EventHandler(this.chkPerformUserConfigSetup_CheckStateChanged);
            manager.ApplyResources(this.lblLanguageFile, "lblLanguageFile");
            this.lblLanguageFile.Name = "lblLanguageFile";
            manager.ApplyResources(this.chkPerformInitialInit, "chkPerformInitialInit");
            panel.SetColumnSpan(this.chkPerformInitialInit, 2);
            this.chkPerformInitialInit.Name = "chkPerformInitialInit";
            this.chkPerformInitialInit.UseVisualStyleBackColor = true;
            this.chkPerformInitialInit.CheckStateChanged += new EventHandler(this.chkPerformUserConfigSetup_CheckStateChanged);
            manager.ApplyResources(this.lblLicense, "lblLicense");
            panel.SetColumnSpan(this.lblLicense, 3);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.UseMnemonic = false;
            this.lblLicense.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblLicense_LinkClicked);
            manager.ApplyResources(this.chkOptimizeFiles, "chkOptimizeFiles");
            panel.SetColumnSpan(this.chkOptimizeFiles, 2);
            this.chkOptimizeFiles.Name = "chkOptimizeFiles";
            this.chkOptimizeFiles.UseVisualStyleBackColor = true;
            this.chkOptimizeFiles.CheckStateChanged += new EventHandler(this.chkPerformUserConfigSetup_CheckStateChanged);
            manager.ApplyResources(this.chkPlaceShortcut, "chkPlaceShortcut");
            panel.SetColumnSpan(this.chkPlaceShortcut, 2);
            this.chkPlaceShortcut.Name = "chkPlaceShortcut";
            this.chkPlaceShortcut.UseVisualStyleBackColor = true;
            this.chkPlaceShortcut.CheckStateChanged += new EventHandler(this.chkPerformUserConfigSetup_CheckStateChanged);
            manager.ApplyResources(panel2, "tlpBackImport");
            panel2.Controls.Add(label4, 0, 0);
            panel2.Controls.Add(this.rbImportCopy, 1, 5);
            panel2.Controls.Add(this.rbImportUpgrade, 1, 6);
            panel2.Controls.Add(label5, 0, 7);
            panel2.Controls.Add(this.chkImportBookmarks, 1, 8);
            panel2.Controls.Add(this.chkImportTools, 1, 9);
            panel2.Controls.Add(label6, 0, 1);
            panel2.Controls.Add(label7, 0, 3);
            panel2.Controls.Add(this.cmbPreviousVersion, 1, 2);
            panel2.Controls.Add(this.rbImportSkip, 1, 4);
            panel2.Name = "tlpBackImport";
            manager.ApplyResources(label4, "lblImportMsg");
            panel2.SetColumnSpan(label4, 2);
            label4.Name = "lblImportMsg";
            manager.ApplyResources(this.rbImportCopy, "rbImportCopy");
            this.rbImportCopy.Name = "rbImportCopy";
            this.rbImportCopy.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.rbImportUpgrade, "rbImportUpgrade");
            this.rbImportUpgrade.Checked = true;
            this.rbImportUpgrade.Name = "rbImportUpgrade";
            this.rbImportUpgrade.TabStop = true;
            this.rbImportUpgrade.UseVisualStyleBackColor = true;
            manager.ApplyResources(label5, "lblSelectAdditionalImportOptions");
            panel2.SetColumnSpan(label5, 2);
            label5.Name = "lblSelectAdditionalImportOptions";
            manager.ApplyResources(this.chkImportBookmarks, "chkImportBookmarks");
            this.chkImportBookmarks.Checked = true;
            this.chkImportBookmarks.CheckState = CheckState.Checked;
            this.chkImportBookmarks.Name = "chkImportBookmarks";
            this.chkImportBookmarks.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkImportTools, "chkImportTools");
            this.chkImportTools.Checked = true;
            this.chkImportTools.CheckState = CheckState.Checked;
            this.chkImportTools.Name = "chkImportTools";
            this.chkImportTools.UseVisualStyleBackColor = true;
            manager.ApplyResources(label6, "lblSelectPreviousVersion");
            panel2.SetColumnSpan(label6, 2);
            label6.Name = "lblSelectPreviousVersion";
            manager.ApplyResources(label7, "lblSelectImportMethod");
            panel2.SetColumnSpan(label7, 2);
            label7.Name = "lblSelectImportMethod";
            this.cmbPreviousVersion.DisplayMember = "Version";
            this.cmbPreviousVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPreviousVersion.FormattingEnabled = true;
            manager.ApplyResources(this.cmbPreviousVersion, "cmbPreviousVersion");
            this.cmbPreviousVersion.Name = "cmbPreviousVersion";
            this.cmbPreviousVersion.ValueMember = "UserConfigPath";
            this.cmbPreviousVersion.SelectedIndexChanged += new EventHandler(this.cmbPreviousVersion_SelectedIndexChanged);
            this.cmbPreviousVersion.Format += new ListControlConvertEventHandler(this.cmbPreviousVersion_Format);
            manager.ApplyResources(this.rbImportSkip, "rbImportSkip");
            this.rbImportSkip.Name = "rbImportSkip";
            this.rbImportSkip.UseVisualStyleBackColor = true;
            panel3.BackColor = Color.White;
            panel3.Controls.Add(this.lblPageCaption);
            manager.ApplyResources(panel3, "pnlPageCaption");
            panel3.Name = "pnlPageCaption";
            manager.ApplyResources(this.lblPageCaption, "lblPageCaption");
            this.lblPageCaption.Name = "lblPageCaption";
            manager.ApplyResources(panel4, "tlpButtons");
            panel4.BackColor = Color.Gainsboro;
            panel4.Controls.Add(this.btnCancel, 3, 0);
            panel4.Controls.Add(this.btnNext, 2, 0);
            panel4.Controls.Add(this.btnBack, 1, 0);
            panel4.Controls.Add(this.btnFinish, 0, 0);
            panel4.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel4.Name = "tlpButtons";
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.Name = "btnNext";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            manager.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            manager.ApplyResources(this.btnFinish, "btnFinish");
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.lblProgressUpgrade, "lblProgressUpgrade");
            this.lblProgressUpgrade.Name = "lblProgressUpgrade";
            this.WizardPageSwitcher.BackColor = SystemColors.ButtonFace;
            this.WizardPageSwitcher.Controls.Add(this.TabPageWelcome);
            this.WizardPageSwitcher.Controls.Add(this.TabPageProcess);
            this.WizardPageSwitcher.Controls.Add(this.TabPageImport);
            this.WizardPageSwitcher.Controls.Add(this.TabPageUserConfig);
            manager.ApplyResources(this.WizardPageSwitcher, "WizardPageSwitcher");
            this.WizardPageSwitcher.Name = "WizardPageSwitcher";
            this.WizardPageSwitcher.SelectedTabStripPage = this.TabPageWelcome;
            this.WizardPageSwitcher.TabStrip = null;
            this.TabPageWelcome.Controls.Add(panel);
            manager.ApplyResources(this.TabPageWelcome, "TabPageWelcome");
            this.TabPageWelcome.Name = "TabPageWelcome";
            this.TabPageProcess.Controls.Add(this.tlpProcess);
            this.TabPageProcess.Controls.Add(this.ProcessProgressBar);
            this.TabPageProcess.Controls.Add(this.lblProgressUpgrade);
            manager.ApplyResources(this.TabPageProcess, "TabPageProcess");
            this.TabPageProcess.Name = "TabPageProcess";
            manager.ApplyResources(this.tlpProcess, "tlpProcess");
            this.tlpProcess.Controls.Add(this.lvProcessDetails, 0, 1);
            this.tlpProcess.Controls.Add(this.btnHideDetails, 2, 0);
            this.tlpProcess.Controls.Add(this.btnShowDetails, 1, 0);
            this.tlpProcess.Name = "tlpProcess";
            this.lvProcessDetails.CanResizeColumns = false;
            this.lvProcessDetails.Columns.AddRange(new ColumnHeader[] { this.clOperation, this.clStatus });
            this.tlpProcess.SetColumnSpan(this.lvProcessDetails, 3);
            manager.ApplyResources(this.lvProcessDetails, "lvProcessDetails");
            this.lvProcessDetails.FullRowSelect = true;
            this.lvProcessDetails.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvProcessDetails.MultiSelect = false;
            this.lvProcessDetails.Name = "lvProcessDetails";
            this.lvProcessDetails.UseCompatibleStateImageBehavior = false;
            this.lvProcessDetails.View = View.Details;
            this.lvProcessDetails.ClientSizeChanged += new EventHandler(this.lvProcessDetails_ClientSizeChanged);
            manager.ApplyResources(this.clOperation, "clOperation");
            manager.ApplyResources(this.clStatus, "clStatus");
            manager.ApplyResources(this.btnHideDetails, "btnHideDetails");
            this.btnHideDetails.Cursor = Cursors.Hand;
            this.btnHideDetails.FlatAppearance.BorderSize = 0;
            this.btnHideDetails.Name = "btnHideDetails";
            this.btnHideDetails.UseVisualStyleBackColor = false;
            this.btnHideDetails.Click += new EventHandler(this.btnShowDetails_Click);
            manager.ApplyResources(this.btnShowDetails, "btnShowDetails");
            this.btnShowDetails.Cursor = Cursors.Hand;
            this.btnShowDetails.FlatAppearance.BorderSize = 0;
            this.btnShowDetails.Name = "btnShowDetails";
            this.btnShowDetails.UseVisualStyleBackColor = false;
            this.btnShowDetails.Click += new EventHandler(this.btnShowDetails_Click);
            manager.ApplyResources(this.ProcessProgressBar, "ProcessProgressBar");
            this.ProcessProgressBar.Name = "ProcessProgressBar";
            this.TabPageImport.Controls.Add(panel2);
            manager.ApplyResources(this.TabPageImport, "TabPageImport");
            this.TabPageImport.Name = "TabPageImport";
            this.TabPageImport.EnabledChanged += new EventHandler(this.TabPageUpgrade_EnabledChanged);
            this.TabPageUserConfig.Controls.Add(this.UserConfigControl);
            manager.ApplyResources(this.TabPageUserConfig, "TabPageUserConfig");
            this.TabPageUserConfig.Name = "TabPageUserConfig";
            this.TabPageUserConfig.EnabledChanged += new EventHandler(this.TabPageUserConfig_EnabledChanged);
            manager.ApplyResources(this.UserConfigControl, "UserConfigControl");
            this.UserConfigControl.Name = "UserConfigControl";
            this.UserConfigControl.SaveSettings = false;
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            manager.ApplyResources(this.bvlCaption, "bvlCaption");
            this.bvlCaption.ForeColor = SystemColors.ControlDarkDark;
            this.bvlCaption.Name = "bvlCaption";
            this.bvlCaption.Sides = Border3DSide.Top;
            this.bvlCaption.Style = Border3DStyle.Flat;
            this.SetupWorker.WorkerReportsProgress = true;
            this.SetupWorker.DoWork += new DoWorkEventHandler(this.SetupWorker_DoWork);
            this.SetupWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SetupWorker_RunWorkerCompleted);
            this.SetupWorker.ProgressChanged += new ProgressChangedEventHandler(this.SetupWorker_ProgressChanged);
            base.AcceptButton = this.btnNext;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.WizardPageSwitcher);
            base.Controls.Add(this.bvlCaption);
            base.Controls.Add(panel3);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel4);
            base.FixMouseWheel = true;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SetupAndUpgradeDialog";
            base.FormClosing += new FormClosingEventHandler(this.SetupAndUpgradeDialog_FormClosing);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            this.WizardPageSwitcher.ResumeLayout(false);
            this.TabPageWelcome.ResumeLayout(false);
            this.TabPageProcess.ResumeLayout(false);
            this.TabPageProcess.PerformLayout();
            this.tlpProcess.ResumeLayout(false);
            this.tlpProcess.PerformLayout();
            this.TabPageImport.ResumeLayout(false);
            this.TabPageUserConfig.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeWelcomePage()
        {
            bool flag = File.Exists(ConfigurableSettingsProvider.UserConfigPath);
            bool flag2 = false;
            CultureInfo argument = SettingsManager.GetArgument<CultureInfo>(ArgumentKey.Culture);
            if ((argument == null) && flag)
            {
                argument = Settings.Default.UICulture;
            }
            IList<SettingsManager.PreviousVersionConfig> previousVersions = SettingsManager.GetPreviousVersions();
            if ((previousVersions == null) || (previousVersions.Count == 0))
            {
                this.chkPerformImport.Enabled = false;
            }
            else
            {
                ConfigurableSettingsProvider.PreviousUserConfigPath = previousVersions[0].UserConfigPath;
                this.cmbPreviousVersion.DataSource = previousVersions;
                if (!flag)
                {
                    object previousVersion = Settings.Default.GetPreviousVersion("VisualStyleState");
                    if (previousVersion != null)
                    {
                        Application.VisualStyleState = (VisualStyleState) previousVersion;
                    }
                    flag2 = true;
                    if (argument == null)
                    {
                        argument = (CultureInfo) Settings.Default.GetPreviousVersion("UICulture");
                    }
                }
            }
            IniFormStringLocalizer localizer = SettingsManager.GetArgument<IniFormStringLocalizer>(ArgumentKey.FormLocalizer);
            if (localizer != null)
            {
                this.lblLanguageFile.Text = Path.GetFileName(localizer.IniPath);
                this.lblLanguageFile.Visible = true;
                this.cmbLanguage.Enabled = false;
            }
            this.cmbLanguage.DataSource = new List<CultureInfo>(SettingsManager.GetInstalledCultures());
            if (this.cmbLanguage.Enabled)
            {
                this.SetLanguage(argument);
                if (this.cmbLanguage.SelectedItem == null)
                {
                    this.SetLanguage(Thread.CurrentThread.CurrentUICulture);
                }
                this.cmbLanguage_SelectionChangeCommitted(this.cmbLanguage, EventArgs.Empty);
            }
            if (this.cmbLanguage.SelectedItem == null)
            {
                this.cmbLanguage.SelectedIndex = 0;
            }
            this.chkPerformImport.Checked = flag2;
            if (!flag)
            {
                this.chkPerformInitialInit.Checked = true;
                this.chkPerformInitialInit.Enabled = false;
            }
            this.chkOptimizeFiles.Enabled = OS.IsWin2k;
            this.chkOptimizeFiles.Checked = this.chkOptimizeFiles.Enabled && !flag;
        }

        private void lblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SettingsManager.ShowLicenseInformation();
        }

        private void lvProcessDetails_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.lvProcessDetails.IsHandleCreated)
            {
                this.lvProcessDetails.BeginInvoke(new MethodInvoker(this.UpdateDetailsListView));
            }
            else
            {
                this.UpdateDetailsListView();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.btnShowDetails.Font = new Font(this.btnShowDetails.Font, FontStyle.Underline);
            this.btnHideDetails.Font = this.btnShowDetails.Font;
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            BasicDialog.UpdateBevel(this.bvlCaption);
            BasicDialog.UpdateBevel(this.bvlButtons);
            base.OnThemeChanged(e);
        }

        private void Process()
        {
            this.ProcessCulture();
            this.lvProcessDetails.BeginUpdate();
            try
            {
                this.clOperation.Text = Resources.sListColumnOperation;
                this.clStatus.Text = Resources.sListColumnStatus;
                if (this.chkPerformUserConfigSetup.Checked)
                {
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(Resources.sInitTaskUserConfigSetup, 0));
                }
                if (this.chkPerformImport.Checked)
                {
                    if (this.rbImportCopy.Checked)
                    {
                        this.lvProcessDetails.Items.Add(this.CreateTaskItem(Resources.sInitTaskImportCopy, 1));
                    }
                    else if (this.rbImportUpgrade.Checked)
                    {
                        this.lvProcessDetails.Items.Add(this.CreateTaskItem(Resources.sInitTaskImportUpgrade, 2));
                    }
                    if (this.chkImportBookmarks.Checked)
                    {
                        this.lvProcessDetails.Items.Add(this.CreateTaskItem(Resources.sInitTaskImportBookmarks, 3));
                    }
                    if (this.chkImportTools.Checked)
                    {
                        this.lvProcessDetails.Items.Add(this.CreateTaskItem(Resources.sInitTaskImportTools, 4));
                    }
                }
                if (this.chkPerformInitialInit.Checked)
                {
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.SetupAppearance));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.SetupEditor));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.SetupViewer));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.SetupExternalTools));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.MakeArchivesHighligter));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.ExcludeFromWer));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.RegisterJumpListTasks));
                }
                if (this.chkPlaceShortcut.Checked)
                {
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.CreateDesktopShortcut));
                }
                if (this.chkOptimizeFiles.Checked)
                {
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.CompressFiles, 3));
                    this.lvProcessDetails.Items.Add(this.CreateTaskItem(InitTask.NGen, 9));
                }
            }
            finally
            {
                this.lvProcessDetails.EndUpdate();
            }
            int num = 0;
            foreach (ListViewItem item in this.lvProcessDetails.Items)
            {
                num += item.StateImageIndex;
            }
            this.ProcessProgressBar.Value = 0;
            this.ProcessProgressBar.Maximum = num;
            this.TabPageProcess.Activate();
            this.SetupWorker.RunWorkerAsync();
        }

        private void ProcessCulture()
        {
            CultureInfo selectedItem = (CultureInfo) this.cmbLanguage.SelectedItem;
            Settings.Default.UICulture = selectedItem;
            if ((selectedItem != null) && (CultureInfo.CurrentUICulture.Name != selectedItem.Name))
            {
                Thread.CurrentThread.CurrentUICulture = selectedItem;
            }
        }

        private bool ProcessImportBookmarks(SettingsManager.PreviousVersionConfig previousVersion)
        {
            return this.CopyFolder(previousVersion.BookmarksDir, SettingsManager.SpecialFolders.Bookmarks, Resources.sBookmarkAlreadyExists, Resources.sConfirmOverwriteBookmark);
        }

        private bool ProcessImportCopy(SettingsManager.PreviousVersionConfig previousVersion)
        {
            bool flag = this.CopyFolder(previousVersion.UserConfigDir, Path.GetDirectoryName(ConfigurableSettingsProvider.UserConfigPath), Resources.sFileAlreadyExists, Resources.sConfirmOverwriteFile);
            Settings.Default.Reload();
            return flag;
        }

        private bool ProcessImportTools(SettingsManager.PreviousVersionConfig previousVersion)
        {
            return this.CopyFiles(previousVersion.ToolsDir, SettingsManager.SpecialFolders.Tools, Resources.sToolAlreadyExists, Resources.sConfirmOverwriteTool);
        }

        private void ProcessImportUpgrade(SettingsManager.PreviousVersionConfig previousVersion)
        {
            ConfigurableSettingsProvider.PreviousUserConfigPath = previousVersion.UserConfigPath;
            SettingsManager.UpgradeSettings();
        }

        private InitTaskResult ProcessInitTask(object task)
        {
            if (task is int)
            {
                try
                {
                    switch (((int) task))
                    {
                        case 0:
                            if (!this.UserConfigControl.SaveSettings)
                            {
                                break;
                            }
                            this.ProcessUserConfigSetup();
                            return InitTaskResult.Successed;

                        case 1:
                            if (!this.ProcessImportCopy(this.GetPreviousVersion()))
                            {
                                break;
                            }
                            return InitTaskResult.Successed;

                        case 2:
                            this.ProcessImportUpgrade(this.GetPreviousVersion());
                            return InitTaskResult.Successed;

                        case 3:
                            if (!this.ProcessImportBookmarks(this.GetPreviousVersion()))
                            {
                                break;
                            }
                            return InitTaskResult.Successed;

                        case 4:
                            if (!this.ProcessImportTools(this.GetPreviousVersion()))
                            {
                                break;
                            }
                            return InitTaskResult.Successed;
                    }
                    return InitTaskResult.Skipped;
                }
                catch (Exception exception)
                {
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                    return InitTaskResult.Failed;
                }
            }
            return SettingsManager.PerformInitTask((InitTask) task);
        }

        private void ProcessUserConfigSetup()
        {
            this.UserConfigControl.SaveComponentSettings();
            ConfigurableSettingsProvider.Reinitialize();
            Environment.SetEnvironmentVariable("nomadcfgdir", SettingsManager.SpecialFolders.UserConfig);
        }

        private void SetLanguage(CultureInfo culture)
        {
            if (culture != null)
            {
                this.cmbLanguage.SelectedValue = culture.Name;
                if ((this.cmbLanguage.SelectedItem == null) && (culture.Parent != null))
                {
                    this.cmbLanguage.SelectedValue = culture.Parent.Name;
                }
            }
        }

        private void SetupAndUpgradeDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.ApplicationExitCall:
                case CloseReason.WindowsShutDown:
                    return;
            }
            e.Cancel = this.SetupWorker.IsBusy;
        }

        private void SetupWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker) sender;
            foreach (ListViewItem item in this.lvProcessDetails.Items)
            {
                if (worker.CancellationPending)
                {
                    break;
                }
                worker.ReportProgress(0, item);
                InitTaskResult userState = this.ProcessInitTask(item.Tag);
                worker.ReportProgress(0, userState);
            }
        }

        private void SetupWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ListViewItem.ListViewSubItem tag;
            ListViewItem userState = e.UserState as ListViewItem;
            if (userState != null)
            {
                tag = new ListViewItem.ListViewSubItem(userState, Resources.sProcessingInitTask) {
                    Tag = userState.StateImageIndex
                };
                userState.SubItems.Add(tag);
                userState.EnsureVisible();
                this.lblProgressUpgrade.Text = string.Format(Resources.sUpgradeTaskTitle, userState.Text);
                this.lblProgressUpgrade.Tag = tag;
            }
            else
            {
                tag = (ListViewItem.ListViewSubItem) this.lblProgressUpgrade.Tag;
                InitTaskResult result = (InitTaskResult) e.UserState;
                tag.Text = TypeDescriptor.GetConverter(typeof(InitTaskResult)).ConvertToString(result);
                this.ProcessProgressBar.Value += (int) tag.Tag;
            }
        }

        private void SetupWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, e.Error);
            }
            this.lblProgressUpgrade.Text = Resources.sUpgrageCompleted;
            this.ProcessProgressBar.Value = this.ProcessProgressBar.Maximum;
            this.btnNext.Enabled = true;
        }

        private void TabPageUpgrade_EnabledChanged(object sender, EventArgs e)
        {
            if (this.TabPageImport.Enabled)
            {
                this.cmbPreviousVersion_SelectedIndexChanged(this.cmbPreviousVersion, EventArgs.Empty);
            }
        }

        private void TabPageUserConfig_EnabledChanged(object sender, EventArgs e)
        {
            if (!(!this.TabPageUserConfig.Enabled || this.IsUserConfigControlInitialized))
            {
                this.UserConfigControl.LoadComponentSettings();
                this.IsUserConfigControlInitialized = true;
            }
        }

        private void UpdateButtons()
        {
            this.btnBack.Enabled = (this.WizardPageSwitcher.SelectedTabStripPage != this.TabPageWelcome) && (this.WizardPageSwitcher.SelectedTabStripPage != this.TabPageProcess);
            this.btnCancel.Enabled = this.WizardPageSwitcher.SelectedTabStripPage != this.TabPageProcess;
            this.lblPageCaption.Text = this.WizardPageSwitcher.SelectedTabStripPage.Text;
            this.UpdateNextButtonText();
        }

        public void UpdateCulture()
        {
            this.LocalizedStringNext = this.btnNext.Text;
            this.LocalizedStringFinish = this.btnFinish.Text;
            this.lblLicense.Links.Clear();
            this.lblLicense.ParseLinks();
        }

        private void UpdateDetailsListView()
        {
            this.lvProcessDetails.Columns[0].Width = this.lvProcessDetails.ClientSize.Width - this.lvProcessDetails.Columns[1].Width;
        }

        private void UpdateNextButtonText()
        {
            bool flag = this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageWelcome;
            bool flag2 = this.WizardPageSwitcher.SelectedTabStripPage == this.TabPageProcess;
            bool flag3 = ((this.chkPerformUserConfigSetup.Checked || this.chkPerformImport.Checked) || (this.chkPerformInitialInit.Checked || this.chkPlaceShortcut.Checked)) || this.chkOptimizeFiles.Checked;
            this.btnNext.Text = ((flag && !flag3) || flag2) ? this.LocalizedStringFinish : this.LocalizedStringNext;
            this.btnNext.SetElevationRequiredState(((this.chkOptimizeFiles.Checked && this.ShieldRequired) && !flag2) && (!flag || ((flag && !this.chkPerformUserConfigSetup.Checked) && !this.chkPerformImport.Checked)));
            this.btnNext.Enabled = (flag3 || this.cmbLanguage.Enabled) && !this.SetupWorker.IsBusy;
        }
    }
}

