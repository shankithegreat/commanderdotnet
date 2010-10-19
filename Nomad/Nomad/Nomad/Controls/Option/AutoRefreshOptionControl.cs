namespace Nomad.Controls.Option
{
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.FileSystem.LocalFile;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class AutoRefreshOptionControl : UserControl, IPersistComponentSettings
    {
        private ComboBoxEx cmbSlowVolumeAutoRefresh;
        private IContainer components = null;
        private Label lblSlowVolumeAutoRefresh;
        private PropertyValuesWatcher ValuesWatcher;

        public AutoRefreshOptionControl()
        {
            this.InitializeComponent();
            this.cmbSlowVolumeAutoRefresh.DataSource = Enum.GetValues(typeof(AutoRefreshMode));
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AutoRefreshOptionControl));
            PropertyValue value2 = new PropertyValue();
            this.lblSlowVolumeAutoRefresh = new Label();
            this.cmbSlowVolumeAutoRefresh = new ComboBoxEx();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblSlowVolumeAutoRefresh, 0, 0);
            panel.Controls.Add(this.cmbSlowVolumeAutoRefresh, 1, 0);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblSlowVolumeAutoRefresh, "lblSlowVolumeAutoRefresh");
            this.lblSlowVolumeAutoRefresh.Name = "lblSlowVolumeAutoRefresh";
            manager.ApplyResources(this.cmbSlowVolumeAutoRefresh, "cmbSlowVolumeAutoRefresh");
            this.cmbSlowVolumeAutoRefresh.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSlowVolumeAutoRefresh.FormattingEnabled = true;
            this.cmbSlowVolumeAutoRefresh.MinimumSize = new Size(110, 0);
            this.cmbSlowVolumeAutoRefresh.Name = "cmbSlowVolumeAutoRefresh";
            value2.DataObject = this.cmbSlowVolumeAutoRefresh;
            value2.PropertyName = "SelectedIndex";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "AutoRefreshOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.cmbSlowVolumeAutoRefresh.SelectedItem = Settings.Default.SlowVolumeAutoRefresh;
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            Settings.Default.SlowVolumeAutoRefresh = (AutoRefreshMode) this.cmbSlowVolumeAutoRefresh.SelectedItem;
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

