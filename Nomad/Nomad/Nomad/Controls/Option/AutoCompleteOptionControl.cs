namespace Nomad.Controls.Option
{
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    public class AutoCompleteOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkSourceEnvironmentVariables;
        private CheckBox chkSourceFileSystem;
        private CheckBox chkSourceKnownShellFolders;
        private CheckBox chkSourceRecentItems;
        private ComboBoxEx cmbAutoCompleteMode;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public AutoCompleteOptionControl()
        {
            this.InitializeComponent();
            this.cmbAutoCompleteMode.DataSource = Enum.GetValues(typeof(AutoCompleteMode));
        }

        private void cmbAutoCompleteMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = ((AutoCompleteMode) this.cmbAutoCompleteMode.SelectedItem) != AutoCompleteMode.None;
            this.chkSourceFileSystem.Enabled = flag;
            this.chkSourceEnvironmentVariables.Enabled = flag;
            this.chkSourceKnownShellFolders.Enabled = flag;
            this.chkSourceRecentItems.Enabled = flag;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AutoCompleteOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            this.cmbAutoCompleteMode = new ComboBoxEx();
            this.chkSourceFileSystem = new CheckBox();
            this.chkSourceEnvironmentVariables = new CheckBox();
            this.chkSourceKnownShellFolders = new CheckBox();
            this.chkSourceRecentItems = new CheckBox();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(control, 0, 0);
            panel.Controls.Add(this.cmbAutoCompleteMode, 1, 0);
            panel.Controls.Add(this.chkSourceFileSystem, 0, 1);
            panel.Controls.Add(this.chkSourceEnvironmentVariables, 0, 2);
            panel.Controls.Add(this.chkSourceKnownShellFolders, 0, 3);
            panel.Controls.Add(this.chkSourceRecentItems, 0, 4);
            panel.Name = "tlpBack";
            manager.ApplyResources(control, "lblAutoCompleteMode");
            control.Name = "lblAutoCompleteMode";
            manager.ApplyResources(this.cmbAutoCompleteMode, "cmbAutoCompleteMode");
            this.cmbAutoCompleteMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAutoCompleteMode.FormattingEnabled = true;
            this.cmbAutoCompleteMode.MinimumSize = new Size(120, 0);
            this.cmbAutoCompleteMode.Name = "cmbAutoCompleteMode";
            this.cmbAutoCompleteMode.SelectedIndexChanged += new EventHandler(this.cmbAutoCompleteMode_SelectedIndexChanged);
            manager.ApplyResources(this.chkSourceFileSystem, "chkSourceFileSystem");
            panel.SetColumnSpan(this.chkSourceFileSystem, 2);
            this.chkSourceFileSystem.Name = "chkSourceFileSystem";
            this.chkSourceFileSystem.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSourceEnvironmentVariables, "chkSourceEnvironmentVariables");
            panel.SetColumnSpan(this.chkSourceEnvironmentVariables, 2);
            this.chkSourceEnvironmentVariables.Name = "chkSourceEnvironmentVariables";
            this.chkSourceEnvironmentVariables.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSourceKnownShellFolders, "chkSourceKnownShellFolders");
            panel.SetColumnSpan(this.chkSourceKnownShellFolders, 2);
            this.chkSourceKnownShellFolders.Name = "chkSourceKnownShellFolders";
            this.chkSourceKnownShellFolders.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSourceRecentItems, "chkSourceRecentItems");
            panel.SetColumnSpan(this.chkSourceRecentItems, 2);
            this.chkSourceRecentItems.Name = "chkSourceRecentItems";
            this.chkSourceRecentItems.UseVisualStyleBackColor = true;
            value2.DataObject = this.cmbAutoCompleteMode;
            value2.PropertyName = "SelectedItem";
            value3.DataObject = this.chkSourceFileSystem;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkSourceEnvironmentVariables;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkSourceKnownShellFolders;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkSourceRecentItems;
            value6.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "AutoCompleteOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.cmbAutoCompleteMode.SelectedItem = Settings.Default.AutoCompleteMode;
            this.chkSourceFileSystem.Checked = Settings.Default.UseACSFileSystem;
            this.chkSourceEnvironmentVariables.Checked = Settings.Default.UseACSEnvironmentVariables;
            this.chkSourceKnownShellFolders.Checked = Settings.Default.UseACSKnownShellFolders;
            this.chkSourceRecentItems.Checked = Settings.Default.UseACSRecentItems;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            Settings.Default.AutoCompleteMode = (AutoCompleteMode) this.cmbAutoCompleteMode.SelectedItem;
            Settings.Default.UseACSFileSystem = this.chkSourceFileSystem.Checked;
            Settings.Default.UseACSEnvironmentVariables = this.chkSourceEnvironmentVariables.Checked;
            Settings.Default.UseACSKnownShellFolders = this.chkSourceKnownShellFolders.Checked;
            Settings.Default.UseACSRecentItems = this.chkSourceRecentItems.Checked;
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

