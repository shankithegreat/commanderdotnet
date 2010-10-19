namespace Nomad.Controls.Option
{
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class TabOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkAlwaysShowTabStrip;
        private CheckBox chkFixedWidthTabs;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public TabOptionControl()
        {
            this.InitializeComponent();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TabOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            this.chkAlwaysShowTabStrip = new CheckBox();
            this.chkFixedWidthTabs = new CheckBox();
            this.ValuesWatcher = new PropertyValuesWatcher();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(this.chkAlwaysShowTabStrip, "chkAlwaysShowTabStrip");
            this.chkAlwaysShowTabStrip.Name = "chkAlwaysShowTabStrip";
            this.chkAlwaysShowTabStrip.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkFixedWidthTabs, "chkFixedWidthTabs");
            this.chkFixedWidthTabs.Name = "chkFixedWidthTabs";
            this.chkFixedWidthTabs.UseVisualStyleBackColor = true;
            value2.DataObject = this.chkAlwaysShowTabStrip;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkFixedWidthTabs;
            value3.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.chkFixedWidthTabs);
            base.Controls.Add(this.chkAlwaysShowTabStrip);
            base.Name = "TabOptionControl";
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkAlwaysShowTabStrip.Checked = Settings.Default.AlwaysShowTabStrip;
            this.chkFixedWidthTabs.Checked = Settings.Default.TabWidth > 10;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            Settings.Default.AlwaysShowTabStrip = this.chkAlwaysShowTabStrip.Checked;
            Settings.Default.TabWidth = this.chkFixedWidthTabs.Checked ? 120 : 0;
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

