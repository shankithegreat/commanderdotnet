namespace Nomad.Controls.Option
{
    using Nomad.Configuration;
    using Nomad.Controls;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class HistoryOptionControl : UserControl, IPersistComponentSettings
    {
        private Button btnClearHistory;
        private CheckBox chkHistoryEnabled;
        private bool ClearHistory;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public HistoryOptionControl()
        {
            this.InitializeComponent();
            if (!Application.RenderWithVisualStyles)
            {
                this.btnClearHistory.FlatStyle = FlatStyle.System;
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            this.ClearHistory = true;
            this.btnClearHistory.Enabled = false;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(HistoryOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            this.chkHistoryEnabled = new CheckBox();
            this.btnClearHistory = new Button();
            this.ValuesWatcher = new PropertyValuesWatcher();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(this.chkHistoryEnabled, "chkHistoryEnabled");
            this.chkHistoryEnabled.Checked = true;
            this.chkHistoryEnabled.CheckState = CheckState.Checked;
            this.chkHistoryEnabled.Name = "chkHistoryEnabled";
            this.chkHistoryEnabled.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnClearHistory, "btnClearHistory");
            this.btnClearHistory.Name = "btnClearHistory";
            this.btnClearHistory.UseVisualStyleBackColor = true;
            this.btnClearHistory.Click += new EventHandler(this.btnClearHistory_Click);
            value2.DataObject = this.chkHistoryEnabled;
            value2.PropertyName = "Checked";
            value3.DataObject = this.btnClearHistory;
            value3.PropertyName = "Enabled";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnClearHistory);
            base.Controls.Add(this.chkHistoryEnabled);
            base.Name = "HistoryOptionControl";
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkHistoryEnabled.Checked = HistorySettings.Default.HistoryEnabled;
            this.btnClearHistory.Enabled = HistorySettings.Default.HistoryExists;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            if (this.ClearHistory)
            {
                HistorySettings.Default.Reset();
            }
            HistorySettings.Default.HistoryEnabled = this.chkHistoryEnabled.Checked;
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

