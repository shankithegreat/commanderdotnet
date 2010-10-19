namespace Nomad.Controls.Option
{
    using Microsoft;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.Controls;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    [ToolboxItem(false)]
    public class AppAppearanceOptionControl : UserControl, IPersistComponentSettings
    {
        private System.Windows.Forms.CheckBox chkExplorerTheme;
        private System.Windows.Forms.CheckBox chkUseVisualStyles;
        private ComboBoxEx cmbLanguage;
        private ComboBoxEx cmbThemeColor;
        private ComboBoxEx cmbThemeFamily;
        private IContainer components = null;
        private ThemeConfigurationSection DefaultTheme;
        private Label lblLanguage;
        private Label lblLanguageFile;
        private Label lblThemeColor;
        private Label lblThemeFamily;
        private List<ThemeConfigurationSection> ThemeList;
        private PropertyValuesWatcher ValuesWatcher;

        public AppAppearanceOptionControl()
        {
            this.InitializeComponent();
            this.chkUseVisualStyles.Enabled = VisualStyleInformation.IsEnabledByUser;
            this.chkExplorerTheme.Enabled = ListViewEx.IsExplorerThemeSupported;
            this.ThemeList = new List<ThemeConfigurationSection>(Theme.GetAllThemes());
            if (this.ThemeList.Count == 0)
            {
                this.cmbThemeFamily.Enabled = false;
                this.lblThemeFamily.Enabled = this.cmbThemeFamily.Enabled;
            }
            else
            {
                foreach (ThemeConfigurationSection section in this.ThemeList)
                {
                    if (section.IsDefault)
                    {
                        this.DefaultTheme = section;
                    }
                    if (section.ThemeColorName == null)
                    {
                        this.cmbThemeFamily.Items.Add(section);
                    }
                    else if (this.cmbThemeFamily.Items.IndexOf(section.ThemeFamilyName) < 0)
                    {
                        this.cmbThemeFamily.Items.Add(section.ThemeFamilyName);
                    }
                }
            }
            List<CultureInfo> list = new List<CultureInfo>(SettingsManager.GetInstalledCultures());
            list.Sort(delegate (CultureInfo x, CultureInfo y) {
                return string.Compare(x.DisplayName, y.DisplayName, StringComparison.InvariantCultureIgnoreCase);
            });
            this.cmbLanguage.DataSource = list;
            IniFormStringLocalizer argument = SettingsManager.GetArgument<IniFormStringLocalizer>(ArgumentKey.FormLocalizer);
            if (argument != null)
            {
                this.lblLanguageFile.Text = Path.GetFileName(argument.IniPath);
                this.lblLanguageFile.Visible = true;
                this.cmbLanguage.Enabled = false;
            }
        }

        private void chkUseVisualStyles_CheckedChanged(object sender, EventArgs e)
        {
            this.chkExplorerTheme.Enabled = OS.IsWinVista && this.chkUseVisualStyles.Checked;
        }

        private void cmbThemeFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = this.cmbThemeFamily.SelectedItem as string;
            this.cmbThemeColor.Visible = selectedItem != null;
            this.lblThemeColor.Visible = this.cmbThemeColor.Visible;
            if (this.cmbThemeColor.Visible)
            {
                this.cmbThemeColor.BeginUpdate();
                this.cmbThemeColor.Items.Clear();
                foreach (ThemeConfigurationSection section in this.ThemeList)
                {
                    if (section.ThemeFamilyName == selectedItem)
                    {
                        this.cmbThemeColor.Items.Add(section);
                    }
                }
                this.cmbThemeColor.SelectedIndex = 0;
                this.cmbThemeColor.EndUpdate();
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

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AppAppearanceOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            this.chkUseVisualStyles = new System.Windows.Forms.CheckBox();
            this.lblLanguage = new Label();
            this.lblThemeFamily = new Label();
            this.lblThemeColor = new Label();
            this.lblLanguageFile = new Label();
            this.chkExplorerTheme = new System.Windows.Forms.CheckBox();
            this.cmbLanguage = new ComboBoxEx();
            this.cmbThemeFamily = new ComboBoxEx();
            this.cmbThemeColor = new ComboBoxEx();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkUseVisualStyles, 0, 0);
            panel.Controls.Add(this.cmbLanguage, 1, 3);
            panel.Controls.Add(this.lblLanguage, 0, 3);
            panel.Controls.Add(this.cmbThemeFamily, 1, 2);
            panel.Controls.Add(this.lblThemeFamily, 0, 2);
            panel.Controls.Add(this.lblThemeColor, 2, 2);
            panel.Controls.Add(this.cmbThemeColor, 3, 2);
            panel.Controls.Add(this.lblLanguageFile, 2, 3);
            panel.Controls.Add(this.chkExplorerTheme, 0, 1);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkUseVisualStyles, "chkUseVisualStyles");
            panel.SetColumnSpan(this.chkUseVisualStyles, 4);
            this.chkUseVisualStyles.Name = "chkUseVisualStyles";
            this.chkUseVisualStyles.UseVisualStyleBackColor = true;
            this.chkUseVisualStyles.CheckedChanged += new EventHandler(this.chkUseVisualStyles_CheckedChanged);
            manager.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            manager.ApplyResources(this.lblThemeFamily, "lblThemeFamily");
            this.lblThemeFamily.Name = "lblThemeFamily";
            manager.ApplyResources(this.lblThemeColor, "lblThemeColor");
            this.lblThemeColor.Name = "lblThemeColor";
            manager.ApplyResources(this.lblLanguageFile, "lblLanguageFile");
            this.lblLanguageFile.Name = "lblLanguageFile";
            manager.ApplyResources(this.chkExplorerTheme, "chkExplorerTheme");
            panel.SetColumnSpan(this.chkExplorerTheme, 4);
            this.chkExplorerTheme.Name = "chkExplorerTheme";
            this.chkExplorerTheme.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.DisplayMember = "DisplayName";
            this.cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.MinimumSize = new Size(120, 0);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.ValueMember = "Name";
            manager.ApplyResources(this.cmbThemeFamily, "cmbThemeFamily");
            this.cmbThemeFamily.DisplayMember = "ThemeFamilyName";
            this.cmbThemeFamily.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbThemeFamily.MinimumSize = new Size(120, 0);
            this.cmbThemeFamily.Name = "cmbThemeFamily";
            this.cmbThemeFamily.Sorted = true;
            this.cmbThemeFamily.SelectedIndexChanged += new EventHandler(this.cmbThemeFamily_SelectedIndexChanged);
            manager.ApplyResources(this.cmbThemeColor, "cmbThemeColor");
            this.cmbThemeColor.DisplayMember = "ThemeColorName";
            this.cmbThemeColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbThemeColor.FormattingEnabled = true;
            this.cmbThemeColor.MinimumSize = new Size(110, 0);
            this.cmbThemeColor.Name = "cmbThemeColor";
            this.cmbThemeColor.Sorted = true;
            value2.DataObject = this.chkUseVisualStyles;
            value2.PropertyName = "Checked";
            value3.DataObject = this;
            value3.PropertyName = "SelectedTheme";
            value4.DataObject = this.cmbLanguage;
            value4.PropertyName = "SelectedValue";
            value5.DataObject = this.chkExplorerTheme;
            value5.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "AppAppearanceOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkUseVisualStyles.Checked = (Settings.Default.VisualStyleState != VisualStyleState.NoneEnabled) && (Settings.Default.VisualStyleState != VisualStyleState.NonClientAreaEnabled);
            this.chkExplorerTheme.Checked = Settings.Default.ExplorerTheme;
            this.SelectedTheme = Settings.Default.Theme;
            this.SetLanguage(Settings.Default.UICulture);
            if (this.cmbLanguage.SelectedItem == null)
            {
                this.SetLanguage(Thread.CurrentThread.CurrentUICulture);
            }
            if (this.cmbLanguage.SelectedItem == null)
            {
                this.cmbLanguage.SelectedIndex = 0;
            }
            this.ValuesWatcher.RememberValues();
            if ((this.cmbThemeFamily.SelectedItem == null) && (this.DefaultTheme != null))
            {
                this.SelectedTheme = this.DefaultTheme.ThemeKey;
            }
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            if (this.ValuesWatcher.IsValueChanged(this.chkUseVisualStyles))
            {
                Settings.Default.VisualStyleState = this.chkUseVisualStyles.Checked ? VisualStyleState.ClientAndNonClientAreasEnabled : VisualStyleState.NonClientAreaEnabled;
            }
            if (this.ValuesWatcher.IsValueChanged(this.chkExplorerTheme))
            {
                Settings.Default.ExplorerTheme = this.chkExplorerTheme.Checked;
            }
            if (this.ValuesWatcher.IsValueChanged(this))
            {
                Settings.Default.Theme = this.SelectedTheme;
            }
            if (this.ValuesWatcher.IsValueChanged(this.cmbLanguage))
            {
                Settings.Default.UICulture = (CultureInfo) this.cmbLanguage.SelectedItem;
            }
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

        public bool SaveSettings
        {
            get
            {
                return this.ValuesWatcher.AnyValueChanged;
            }
            set
            {
            }
        }

        public string SelectedTheme
        {
            get
            {
                ThemeConfigurationSection selectedItem;
                if (this.cmbThemeColor.Visible)
                {
                    selectedItem = this.cmbThemeColor.SelectedItem as ThemeConfigurationSection;
                }
                else
                {
                    selectedItem = this.cmbThemeFamily.SelectedItem as ThemeConfigurationSection;
                }
                return ((selectedItem != null) ? selectedItem.ThemeKey : null);
            }
            set
            {
                foreach (ThemeConfigurationSection section in this.ThemeList)
                {
                    if (!(section.ThemeKey == value))
                    {
                        continue;
                    }
                    if (section.ThemeColorName == null)
                    {
                        this.cmbThemeFamily.SelectedItem = section;
                    }
                    else
                    {
                        this.cmbThemeFamily.SelectedItem = section.ThemeFamilyName;
                        this.cmbThemeColor.SelectedItem = section;
                    }
                }
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

