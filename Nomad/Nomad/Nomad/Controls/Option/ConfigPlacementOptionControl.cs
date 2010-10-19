namespace Nomad.Controls.Option
{
    using Nomad.Commons.IO;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Controls;
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
    using System.Xml;

    [ToolboxItem(false)]
    public class ConfigPlacementOptionControl : UserControl, IPersistComponentSettings, IUpdateCulture
    {
        private FolderBrowserDialog BrowseFolderDialog;
        private Button btnAppendBookmarks;
        private Button btnAppendTools;
        private Button btnAppendUserDefined;
        private Button btnBrowseBookmarksFolder;
        private Button btnBrowseToolsFolder;
        private Button btnBrowseUserDefinedFolder;
        private CheckBox chkCustomizeBookmarks;
        private CheckBox chkCustomizeTools;
        private ContextMenuStrip cmsSpecialFolder;
        private ContextMenuStrip cmsUserDefined;
        private IContainer components = null;
        private const string DefaultBookmarksFolder = @"%nomadcfgdir%\..\Bookmarks";
        private const string DefaultToolsFolder = @"%nomadcfgdir%\..\Tools";
        private const string DefaultUserDefinedFolder = @"%nomaddir%\users\%username%\%nomadver%";
        private Label lblCustomizeSpecialFolders;
        private LinkLabel lblEffectiveConfigPath;
        private RadioButton rbLocalConfig;
        private RadioButton rbRoamingConfig;
        private RadioButton rbUserDefinedConfig;
        private Dictionary<string, string> StoredSpecialFolderMap;
        private string StoreUserDefinedFolder;
        private ToolStripMenuItem tsmiNomadConfigFolder;
        private ToolStripMenuItem tsmiNomadFolder;
        private ToolStripMenuItem tsmiNomadFolder2;
        private ToolStripMenuItem tsmiNomadVersion;
        private ToolStripMenuItem tsmiNomadVersion2;
        private ToolStripMenuItem tsmiUserName;
        private ToolStripMenuItem tsmiUserName2;
        private ToolStripSeparator tssSpecialFolder1;
        private ToolStripSeparator tssSpecialFolder2;
        private ToolStripSeparator tssUserDefined1;
        private TextBox txtBookmarksFolder;
        private TextBox txtToolsFolder;
        private TextBox txtUserDefinedFolder;
        private PropertyValuesWatcher ValuesWatcher;

        public ConfigPlacementOptionControl()
        {
            this.InitializeComponent();
            this.cmsUserDefined.Tag = this.txtUserDefinedFolder;
            this.btnAppendBookmarks.Tag = this.txtBookmarksFolder;
            this.btnAppendTools.Tag = this.txtToolsFolder;
            this.btnBrowseUserDefinedFolder.Tag = this.txtUserDefinedFolder;
            this.btnBrowseBookmarksFolder.Tag = this.txtBookmarksFolder;
            this.btnBrowseToolsFolder.Tag = this.txtToolsFolder;
        }

        private void btnAppendSpecialFolder_Click(object sender, EventArgs e)
        {
            Button control = (Button) sender;
            this.cmsSpecialFolder.Tag = control.Tag;
            this.cmsSpecialFolder.Show(control, 0, control.Height + 1);
        }

        private void btnAppendUserDefined_Click(object sender, EventArgs e)
        {
            this.cmsUserDefined.Show(this.btnAppendUserDefined, 0, this.btnAppendUserDefined.Height + 1);
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            TextBox tag = (TextBox) ((Control) sender).Tag;
            this.BrowseFolderDialog.SelectedPath = Environment.ExpandEnvironmentVariables(tag.Text);
            if (this.BrowseFolderDialog.ShowDialog(this) == DialogResult.OK)
            {
                tag.Text = this.BrowseFolderDialog.SelectedPath;
                this.ValidateFolderTextBox(tag);
                if (tag == this.txtUserDefinedFolder)
                {
                    this.UpdateEffectiveConfigPath();
                }
            }
        }

        private IEnumerable<KeyValuePair<string, string>> DeserializeNameValueXml(string rawXml)
        {
            return new <DeserializeNameValueXml>d__0(-2) { <>4__this = this, <>3__rawXml = rawXml };
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ConfigPlacementOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            this.rbLocalConfig = new RadioButton();
            this.rbRoamingConfig = new RadioButton();
            this.rbUserDefinedConfig = new RadioButton();
            this.txtUserDefinedFolder = new TextBox();
            this.btnBrowseUserDefinedFolder = new Button();
            this.lblCustomizeSpecialFolders = new Label();
            this.chkCustomizeBookmarks = new CheckBox();
            this.btnBrowseBookmarksFolder = new Button();
            this.txtBookmarksFolder = new TextBox();
            this.chkCustomizeTools = new CheckBox();
            this.txtToolsFolder = new TextBox();
            this.btnBrowseToolsFolder = new Button();
            this.btnAppendUserDefined = new Button();
            this.btnAppendBookmarks = new Button();
            this.btnAppendTools = new Button();
            this.lblEffectiveConfigPath = new LinkLabel();
            this.cmsUserDefined = new ContextMenuStrip(this.components);
            this.tsmiNomadFolder = new ToolStripMenuItem();
            this.tsmiNomadVersion = new ToolStripMenuItem();
            this.tssUserDefined1 = new ToolStripSeparator();
            this.tsmiUserName = new ToolStripMenuItem();
            this.cmsSpecialFolder = new ContextMenuStrip(this.components);
            this.tsmiNomadConfigFolder = new ToolStripMenuItem();
            this.tssSpecialFolder1 = new ToolStripSeparator();
            this.tsmiNomadFolder2 = new ToolStripMenuItem();
            this.tsmiNomadVersion2 = new ToolStripMenuItem();
            this.tssSpecialFolder2 = new ToolStripSeparator();
            this.tsmiUserName2 = new ToolStripMenuItem();
            this.BrowseFolderDialog = new FolderBrowserDialog();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            Label label2 = new Label();
            panel.SuspendLayout();
            this.cmsUserDefined.SuspendLayout();
            this.cmsSpecialFolder.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.rbLocalConfig, 1, 1);
            panel.Controls.Add(this.rbRoamingConfig, 1, 2);
            panel.Controls.Add(this.rbUserDefinedConfig, 1, 3);
            panel.Controls.Add(this.txtUserDefinedFolder, 1, 4);
            panel.Controls.Add(this.btnBrowseUserDefinedFolder, 3, 4);
            panel.Controls.Add(control, 0, 0);
            panel.Controls.Add(this.lblCustomizeSpecialFolders, 0, 5);
            panel.Controls.Add(this.chkCustomizeBookmarks, 1, 6);
            panel.Controls.Add(this.btnBrowseBookmarksFolder, 3, 7);
            panel.Controls.Add(this.txtBookmarksFolder, 1, 7);
            panel.Controls.Add(this.chkCustomizeTools, 1, 8);
            panel.Controls.Add(this.txtToolsFolder, 1, 9);
            panel.Controls.Add(this.btnBrowseToolsFolder, 3, 9);
            panel.Controls.Add(this.btnAppendUserDefined, 2, 4);
            panel.Controls.Add(this.btnAppendBookmarks, 2, 7);
            panel.Controls.Add(this.btnAppendTools, 2, 9);
            panel.Controls.Add(label2, 0, 10);
            panel.Controls.Add(this.lblEffectiveConfigPath, 1, 11);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.rbLocalConfig, "rbLocalConfig");
            panel.SetColumnSpan(this.rbLocalConfig, 3);
            this.rbLocalConfig.Name = "rbLocalConfig";
            this.rbLocalConfig.TabStop = true;
            this.rbLocalConfig.UseVisualStyleBackColor = true;
            this.rbLocalConfig.Click += new EventHandler(this.rbConfig_Click);
            manager.ApplyResources(this.rbRoamingConfig, "rbRoamingConfig");
            panel.SetColumnSpan(this.rbRoamingConfig, 3);
            this.rbRoamingConfig.Name = "rbRoamingConfig";
            this.rbRoamingConfig.TabStop = true;
            this.rbRoamingConfig.UseVisualStyleBackColor = true;
            this.rbRoamingConfig.Click += new EventHandler(this.rbConfig_Click);
            manager.ApplyResources(this.rbUserDefinedConfig, "rbUserDefinedConfig");
            panel.SetColumnSpan(this.rbUserDefinedConfig, 3);
            this.rbUserDefinedConfig.Name = "rbUserDefinedConfig";
            this.rbUserDefinedConfig.TabStop = true;
            this.rbUserDefinedConfig.UseVisualStyleBackColor = true;
            this.rbUserDefinedConfig.Click += new EventHandler(this.rbConfig_Click);
            manager.ApplyResources(this.txtUserDefinedFolder, "txtUserDefinedFolder");
            this.txtUserDefinedFolder.Name = "txtUserDefinedFolder";
            this.txtUserDefinedFolder.Validated += new EventHandler(this.txtUserDefinedFolder_Validated);
            this.txtUserDefinedFolder.EnabledChanged += new EventHandler(this.txtFolder_EnabledChanged);
            this.txtUserDefinedFolder.Validating += new CancelEventHandler(this.txtFolder_Validating);
            manager.ApplyResources(this.btnBrowseUserDefinedFolder, "btnBrowseUserDefinedFolder");
            this.btnBrowseUserDefinedFolder.Name = "btnBrowseUserDefinedFolder";
            this.btnBrowseUserDefinedFolder.UseVisualStyleBackColor = true;
            this.btnBrowseUserDefinedFolder.Click += new EventHandler(this.btnBrowseFolder_Click);
            manager.ApplyResources(control, "lblSelectConfigFolder");
            panel.SetColumnSpan(control, 4);
            control.Name = "lblSelectConfigFolder";
            manager.ApplyResources(this.lblCustomizeSpecialFolders, "lblCustomizeSpecialFolders");
            panel.SetColumnSpan(this.lblCustomizeSpecialFolders, 4);
            this.lblCustomizeSpecialFolders.Name = "lblCustomizeSpecialFolders";
            manager.ApplyResources(this.chkCustomizeBookmarks, "chkCustomizeBookmarks");
            panel.SetColumnSpan(this.chkCustomizeBookmarks, 3);
            this.chkCustomizeBookmarks.Name = "chkCustomizeBookmarks";
            this.chkCustomizeBookmarks.UseVisualStyleBackColor = true;
            this.chkCustomizeBookmarks.Click += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.btnBrowseBookmarksFolder, "btnBrowseBookmarksFolder");
            this.btnBrowseBookmarksFolder.Name = "btnBrowseBookmarksFolder";
            this.btnBrowseBookmarksFolder.UseVisualStyleBackColor = true;
            this.btnBrowseBookmarksFolder.Click += new EventHandler(this.btnBrowseFolder_Click);
            manager.ApplyResources(this.txtBookmarksFolder, "txtBookmarksFolder");
            this.txtBookmarksFolder.Name = "txtBookmarksFolder";
            this.txtBookmarksFolder.EnabledChanged += new EventHandler(this.txtFolder_EnabledChanged);
            this.txtBookmarksFolder.Validating += new CancelEventHandler(this.txtFolder_Validating);
            manager.ApplyResources(this.chkCustomizeTools, "chkCustomizeTools");
            panel.SetColumnSpan(this.chkCustomizeTools, 3);
            this.chkCustomizeTools.Name = "chkCustomizeTools";
            this.chkCustomizeTools.UseVisualStyleBackColor = true;
            this.chkCustomizeTools.Click += new EventHandler(this.UpdateButtons);
            manager.ApplyResources(this.txtToolsFolder, "txtToolsFolder");
            this.txtToolsFolder.Name = "txtToolsFolder";
            this.txtToolsFolder.EnabledChanged += new EventHandler(this.txtFolder_EnabledChanged);
            this.txtToolsFolder.Validating += new CancelEventHandler(this.txtFolder_Validating);
            manager.ApplyResources(this.btnBrowseToolsFolder, "btnBrowseToolsFolder");
            this.btnBrowseToolsFolder.Name = "btnBrowseToolsFolder";
            this.btnBrowseToolsFolder.UseVisualStyleBackColor = true;
            this.btnBrowseToolsFolder.Click += new EventHandler(this.btnBrowseFolder_Click);
            manager.ApplyResources(this.btnAppendUserDefined, "btnAppendUserDefined");
            this.btnAppendUserDefined.Name = "btnAppendUserDefined";
            this.btnAppendUserDefined.UseVisualStyleBackColor = true;
            this.btnAppendUserDefined.Click += new EventHandler(this.btnAppendUserDefined_Click);
            manager.ApplyResources(this.btnAppendBookmarks, "btnAppendBookmarks");
            this.btnAppendBookmarks.Name = "btnAppendBookmarks";
            this.btnAppendBookmarks.UseVisualStyleBackColor = true;
            this.btnAppendBookmarks.Click += new EventHandler(this.btnAppendSpecialFolder_Click);
            manager.ApplyResources(this.btnAppendTools, "btnAppendTools");
            this.btnAppendTools.Name = "btnAppendTools";
            this.btnAppendTools.UseVisualStyleBackColor = true;
            this.btnAppendTools.Click += new EventHandler(this.btnAppendSpecialFolder_Click);
            manager.ApplyResources(label2, "lblEffectivePathCaption");
            panel.SetColumnSpan(label2, 4);
            label2.Name = "lblEffectivePathCaption";
            manager.ApplyResources(this.lblEffectiveConfigPath, "lblEffectiveConfigPath");
            panel.SetColumnSpan(this.lblEffectiveConfigPath, 3);
            this.lblEffectiveConfigPath.Name = "lblEffectiveConfigPath";
            this.lblEffectiveConfigPath.TabStop = true;
            this.lblEffectiveConfigPath.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblEffectiveConfigPath_LinkClicked);
            this.cmsUserDefined.Items.AddRange(new ToolStripItem[] { this.tsmiNomadFolder, this.tsmiNomadVersion, this.tssUserDefined1, this.tsmiUserName });
            this.cmsUserDefined.Name = "cmsUserDefined";
            manager.ApplyResources(this.cmsUserDefined, "cmsUserDefined");
            this.tsmiNomadFolder.Name = "tsmiNomadFolder";
            manager.ApplyResources(this.tsmiNomadFolder, "tsmiNomadFolder");
            this.tsmiNomadFolder.Tag = "%nomaddir%";
            this.tsmiNomadFolder.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.tsmiNomadVersion.Name = "tsmiNomadVersion";
            manager.ApplyResources(this.tsmiNomadVersion, "tsmiNomadVersion");
            this.tsmiNomadVersion.Tag = "%nomadver%";
            this.tsmiNomadVersion.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.tssUserDefined1.Name = "tssUserDefined1";
            manager.ApplyResources(this.tssUserDefined1, "tssUserDefined1");
            this.tsmiUserName.Name = "tsmiUserName";
            manager.ApplyResources(this.tsmiUserName, "tsmiUserName");
            this.tsmiUserName.Tag = "%username%";
            this.tsmiUserName.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.cmsSpecialFolder.Items.AddRange(new ToolStripItem[] { this.tsmiNomadConfigFolder, this.tssSpecialFolder1, this.tsmiNomadFolder2, this.tsmiNomadVersion2, this.tssSpecialFolder2, this.tsmiUserName2 });
            this.cmsSpecialFolder.Name = "cmsSpecialFolder";
            manager.ApplyResources(this.cmsSpecialFolder, "cmsSpecialFolder");
            this.tsmiNomadConfigFolder.Name = "tsmiNomadConfigFolder";
            manager.ApplyResources(this.tsmiNomadConfigFolder, "tsmiNomadConfigFolder");
            this.tsmiNomadConfigFolder.Tag = "%nomadcfgdir%";
            this.tsmiNomadConfigFolder.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.tssSpecialFolder1.Name = "tssSpecialFolder1";
            manager.ApplyResources(this.tssSpecialFolder1, "tssSpecialFolder1");
            this.tsmiNomadFolder2.Name = "tsmiNomadFolder2";
            manager.ApplyResources(this.tsmiNomadFolder2, "tsmiNomadFolder2");
            this.tsmiNomadFolder2.Tag = "%nomaddir%";
            this.tsmiNomadFolder2.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.tsmiNomadVersion2.Name = "tsmiNomadVersion2";
            manager.ApplyResources(this.tsmiNomadVersion2, "tsmiNomadVersion2");
            this.tsmiNomadVersion2.Tag = "%nomadver%";
            this.tsmiNomadVersion2.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.tssSpecialFolder2.Name = "tssSpecialFolder2";
            manager.ApplyResources(this.tssSpecialFolder2, "tssSpecialFolder2");
            this.tsmiUserName2.Name = "tsmiUserName2";
            manager.ApplyResources(this.tsmiUserName2, "tsmiUserName2");
            this.tsmiUserName2.Tag = "%username%";
            this.tsmiUserName2.Click += new EventHandler(this.tsmiFolderVar_Click);
            this.BrowseFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            value2.DataObject = this.rbLocalConfig;
            value2.PropertyName = "Checked";
            value3.DataObject = this.rbRoamingConfig;
            value3.PropertyName = "Checked";
            value4.DataObject = this.rbUserDefinedConfig;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkCustomizeBookmarks;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkCustomizeTools;
            value6.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "ConfigPlacementOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.cmsUserDefined.ResumeLayout(false);
            this.cmsSpecialFolder.ResumeLayout(false);
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
        }

        private void lblEffectiveConfigPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(this.lblEffectiveConfigPath.Text);
            }
            catch (Win32Exception)
            {
            }
        }

        public void LoadComponentSettings()
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SettingsConfigurationSection section = configuration.GetSection("settingsConfiguration/settingsProvider") as SettingsConfigurationSection;
            if (section != null)
            {
                if (!string.IsNullOrEmpty(section.LocalUserConfigDir))
                {
                    this.rbUserDefinedConfig.Checked = true;
                    this.txtUserDefinedFolder.Text = section.LocalUserConfigDir;
                }
                else if (!string.IsNullOrEmpty(section.RoamingUserConfigDir))
                {
                    this.rbUserDefinedConfig.Checked = true;
                    this.txtUserDefinedFolder.Text = section.RoamingUserConfigDir;
                }
                else if (section.UserLevel == ConfigurationUserLevel.PerUserRoaming)
                {
                    this.rbRoamingConfig.Checked = true;
                }
                else
                {
                    this.rbLocalConfig.Checked = true;
                }
            }
            else
            {
                this.rbLocalConfig.Checked = true;
            }
            if (string.IsNullOrEmpty(this.txtUserDefinedFolder.Text))
            {
                this.txtUserDefinedFolder.Text = @"%nomaddir%\users\%username%\%nomadver%";
            }
            this.StoreUserDefinedFolder = this.txtUserDefinedFolder.Text;
            ConfigurationSection section2 = configuration.GetSection("settingsConfiguration/specialFolders");
            if (section2 != null)
            {
                this.StoredSpecialFolderMap = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> pair in this.DeserializeNameValueXml(section2.SectionInformation.GetRawXml()))
                {
                    this.StoredSpecialFolderMap[pair.Key] = pair.Value;
                    if (pair.Key == "Bookmarks")
                    {
                        this.txtBookmarksFolder.Text = pair.Value;
                    }
                    else if (pair.Key == "Tools")
                    {
                        this.txtToolsFolder.Text = pair.Value;
                    }
                }
            }
            if (string.IsNullOrEmpty(this.txtBookmarksFolder.Text))
            {
                this.txtBookmarksFolder.Text = @"%nomadcfgdir%\..\Bookmarks";
            }
            if (string.IsNullOrEmpty(this.txtToolsFolder.Text))
            {
                this.txtToolsFolder.Text = @"%nomadcfgdir%\..\Tools";
            }
            this.chkCustomizeBookmarks.Checked = !string.Equals(this.txtBookmarksFolder.Text, @"%nomadcfgdir%\..\Bookmarks", StringComparison.OrdinalIgnoreCase);
            this.chkCustomizeTools.Checked = !string.Equals(this.txtToolsFolder.Text, @"%nomadcfgdir%\..\Tools", StringComparison.OrdinalIgnoreCase);
            this.UpdateEffectiveConfigPath();
            this.UpdateButtons(null, EventArgs.Empty);
            this.ValuesWatcher.RememberValues();
        }

        private void rbConfig_Click(object sender, EventArgs e)
        {
            this.UpdateEffectiveConfigPath();
            this.UpdateButtons(sender, e);
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSectionGroup sectionGroup = configuration.SectionGroups["settingsConfiguration"];
            if (sectionGroup == null)
            {
                sectionGroup = new ConfigurationSectionGroup();
                configuration.SectionGroups.Add("settingsConfiguration", sectionGroup);
            }
            SettingsConfigurationSection section = configuration.GetSection("settingsConfiguration/settingsProvider") as SettingsConfigurationSection;
            if (section == null)
            {
                section = new SettingsConfigurationSection();
                sectionGroup.Sections.Add("settingsProvider", section);
            }
            section.LocalUserConfigDir = null;
            section.RoamingUserConfigDir = null;
            if (this.rbLocalConfig.Checked)
            {
                section.UserLevel = ConfigurationUserLevel.PerUserRoamingAndLocal;
            }
            else
            {
                section.UserLevel = ConfigurationUserLevel.PerUserRoaming;
                if (this.rbUserDefinedConfig.Checked)
                {
                    section.RoamingUserConfigDir = this.txtUserDefinedFolder.Text.Trim();
                }
            }
            ConfigurationSection section2 = configuration.GetSection("settingsConfiguration/specialFolders");
            if (section2 == null)
            {
                section2 = new DefaultSection {
                    SectionInformation = { Type = typeof(NameValueSectionHandler).ToString() }
                };
                sectionGroup.Sections.Add("specialFolders", section2);
            }
            if (this.StoredSpecialFolderMap == null)
            {
                this.StoredSpecialFolderMap = new Dictionary<string, string>(2);
            }
            this.StoredSpecialFolderMap["Bookmarks"] = this.chkCustomizeBookmarks.Checked ? this.txtBookmarksFolder.Text.Trim() : @"%nomadcfgdir%\..\Bookmarks";
            this.StoredSpecialFolderMap["Tools"] = this.chkCustomizeTools.Checked ? this.txtToolsFolder.Text.Trim() : @"%nomadcfgdir%\..\Tools";
            section2.SectionInformation.SetRawXml(this.SerializeNameValueXml(section2.SectionInformation.Name, this.StoredSpecialFolderMap));
            configuration.Save();
        }

        private string SerializeNameValueXml(string rootName, IEnumerable<KeyValuePair<string, string>> values)
        {
            StringBuilder output = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings {
                OmitXmlDeclaration = true
            };
            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                writer.WriteStartElement(rootName);
                foreach (KeyValuePair<string, string> pair in values)
                {
                    writer.WriteStartElement("add");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteAttributeString("value", pair.Value);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            return output.ToString();
        }

        private void SetTextBoxState(TextBox box, bool error)
        {
            if (error)
            {
                box.BackColor = Settings.TextBoxError;
                box.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                box.ResetBackColor();
                box.ResetForeColor();
            }
        }

        private void tsmiFolderVar_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            TextBox tag = (TextBox) item.Owner.Tag;
            tag.Text = tag.Text + ((string) item.Tag);
            if (tag == this.txtUserDefinedFolder)
            {
                this.UpdateEffectiveConfigPath();
            }
        }

        private void txtFolder_EnabledChanged(object sender, EventArgs e)
        {
            TextBox box = (TextBox) sender;
            if (!box.Enabled)
            {
                this.SetTextBoxState(box, false);
            }
        }

        private void txtFolder_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateFolderTextBox((TextBox) sender);
        }

        private void txtUserDefinedFolder_Validated(object sender, EventArgs e)
        {
            this.UpdateEffectiveConfigPath();
        }

        private void UpdateButtons(object sender, EventArgs e)
        {
            this.txtUserDefinedFolder.Enabled = this.rbUserDefinedConfig.Checked;
            this.btnAppendUserDefined.Enabled = this.txtUserDefinedFolder.Enabled;
            this.btnBrowseUserDefinedFolder.Enabled = this.txtUserDefinedFolder.Enabled;
            this.txtBookmarksFolder.Enabled = this.chkCustomizeBookmarks.Checked;
            this.btnAppendBookmarks.Enabled = this.txtBookmarksFolder.Enabled;
            this.btnBrowseBookmarksFolder.Enabled = this.txtBookmarksFolder.Enabled;
            this.txtToolsFolder.Enabled = this.chkCustomizeTools.Checked;
            this.btnAppendTools.Enabled = this.txtToolsFolder.Enabled;
            this.btnBrowseToolsFolder.Enabled = this.txtToolsFolder.Enabled;
        }

        public void UpdateCulture()
        {
            this.UpdateEffectiveConfigPath();
        }

        private void UpdateEffectiveConfigPath()
        {
            if (this.rbUserDefinedConfig.Checked)
            {
                this.lblEffectiveConfigPath.Text = Environment.ExpandEnvironmentVariables(this.txtUserDefinedFolder.Text);
            }
            else if (this.rbRoamingConfig.Checked)
            {
                this.lblEffectiveConfigPath.Text = Path.GetDirectoryName(ConfigurableSettingsProvider.GetDefaultExeConfigPath(ConfigurationUserLevel.PerUserRoaming));
            }
            else
            {
                this.lblEffectiveConfigPath.Text = Path.GetDirectoryName(ConfigurableSettingsProvider.GetDefaultExeConfigPath(ConfigurationUserLevel.PerUserRoamingAndLocal));
            }
        }

        public override bool ValidateChildren()
        {
            bool flag = base.ValidateChildren();
            if (this.rbUserDefinedConfig.Checked)
            {
                if (!this.ValidateFolderTextBox(this.txtUserDefinedFolder))
                {
                    flag = false;
                }
                if (!(!this.chkCustomizeBookmarks.Checked || this.ValidateFolderTextBox(this.txtBookmarksFolder)))
                {
                    flag = false;
                }
                if (!(!this.chkCustomizeTools.Checked || this.ValidateFolderTextBox(this.txtToolsFolder)))
                {
                    flag = false;
                }
                string a = this.txtUserDefinedFolder.Text.Trim();
                if (this.chkCustomizeBookmarks.Checked && string.Equals(a, this.txtBookmarksFolder.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    this.SetTextBoxState(this.txtUserDefinedFolder, true);
                    this.SetTextBoxState(this.txtBookmarksFolder, true);
                    flag = false;
                }
                if (this.chkCustomizeTools.Checked && string.Equals(a, this.txtToolsFolder.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    this.SetTextBoxState(this.txtUserDefinedFolder, true);
                    this.SetTextBoxState(this.txtToolsFolder, true);
                    flag = false;
                }
            }
            if ((this.chkCustomizeBookmarks.Checked && this.chkCustomizeTools.Checked) && string.Equals(this.txtBookmarksFolder.Text.Trim(), this.txtToolsFolder.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                this.SetTextBoxState(this.txtBookmarksFolder, true);
                this.SetTextBoxState(this.txtToolsFolder, true);
                flag = false;
            }
            return flag;
        }

        private bool ValidateFolderTextBox(TextBox box)
        {
            PathType pathType = PathHelper.GetPathType(box.Text.Trim());
            bool error = (pathType == ~PathType.Unknown) || ((pathType & (PathType.Uri | PathType.NetworkServer)) > PathType.Unknown);
            this.SetTextBoxState(box, error);
            return !error;
        }

        public bool SaveSettings
        {
            get
            {
                return (((this.ValuesWatcher.AnyValueChanged || (this.rbUserDefinedConfig.Checked && (this.txtUserDefinedFolder.Text != this.StoreUserDefinedFolder))) || (this.chkCustomizeBookmarks.Checked && ((this.StoredSpecialFolderMap == null) || (this.txtBookmarksFolder.Text != this.StoredSpecialFolderMap["Bookmarks"])))) || (this.chkCustomizeTools.Checked && ((this.StoredSpecialFolderMap == null) || (this.txtToolsFolder.Text != this.StoredSpecialFolderMap["Tools"]))));
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
        private sealed class <DeserializeNameValueXml>d__0 : IEnumerable<KeyValuePair<string, string>>, IEnumerable, IEnumerator<KeyValuePair<string, string>>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private KeyValuePair<string, string> <>2__current;
            public string <>3__rawXml;
            public ConfigPlacementOptionControl <>4__this;
            private int <>l__initialThreadId;
            public string <Key>5__2;
            public XmlReader <Reader>5__1;
            public string <Value>5__3;
            public string rawXml;

            [DebuggerHidden]
            public <DeserializeNameValueXml>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                if (this.<Reader>5__1 != null)
                {
                    ((IDisposable) this.<Reader>5__1).Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<Reader>5__1 = XmlReader.Create(new StringReader(this.rawXml));
                            this.<>1__state = 1;
                            this.<Reader>5__1.ReadStartElement();
                            while (this.<Reader>5__1.Read() && (this.<Reader>5__1.NodeType != XmlNodeType.EndElement))
                            {
                                if (this.<Reader>5__1.NodeType != XmlNodeType.Element)
                                {
                                    goto Label_00C8;
                                }
                                this.<Key>5__2 = this.<Reader>5__1.GetAttribute("key");
                                this.<Value>5__3 = this.<Reader>5__1.GetAttribute("value");
                                this.<>2__current = new KeyValuePair<string, string>(this.<Key>5__2, this.<Value>5__3);
                                this.<>1__state = 2;
                                return true;
                            Label_00C0:
                                this.<>1__state = 1;
                            Label_00C8:;
                            }
                            this.<Reader>5__1.ReadEndElement();
                            this.<>m__Finally4();
                            break;

                        case 2:
                            goto Label_00C0;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
            {
                ConfigPlacementOptionControl.<DeserializeNameValueXml>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ConfigPlacementOptionControl.<DeserializeNameValueXml>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.rawXml = this.<>3__rawXml;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.String>>.GetEnumerator();
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
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            KeyValuePair<string, string> IEnumerator<KeyValuePair<string, string>>.Current
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
    }
}

