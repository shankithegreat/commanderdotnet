namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Controls;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class HiddenItemsOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkHideSysHidItems;
        private CheckBox chkUseHiddenItemsList;
        private IContainer components = null;
        private TextBox txtHiddenItemsList;
        private PropertyValuesWatcher ValuesWatcher;

        public HiddenItemsOptionControl()
        {
            this.InitializeComponent();
        }

        private void chkUseHiddenItemsList_CheckedChanged(object sender, EventArgs e)
        {
            this.txtHiddenItemsList.Enabled = this.chkUseHiddenItemsList.Checked;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(HiddenItemsOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            this.chkHideSysHidItems = new CheckBox();
            this.chkUseHiddenItemsList = new CheckBox();
            this.txtHiddenItemsList = new TextBox();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            panel.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.chkHideSysHidItems, "chkHideSysHidItems");
            this.chkHideSysHidItems.Name = "chkHideSysHidItems";
            this.chkHideSysHidItems.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkUseHiddenItemsList, "chkUseHiddenItemsList");
            this.chkUseHiddenItemsList.Name = "chkUseHiddenItemsList";
            this.chkUseHiddenItemsList.UseVisualStyleBackColor = true;
            this.chkUseHiddenItemsList.CheckedChanged += new EventHandler(this.chkUseHiddenItemsList_CheckedChanged);
            this.txtHiddenItemsList.AcceptsReturn = true;
            manager.ApplyResources(this.txtHiddenItemsList, "txtHiddenItemsList");
            this.txtHiddenItemsList.Name = "txtHiddenItemsList";
            value2.DataObject = this.chkHideSysHidItems;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkUseHiddenItemsList;
            value3.PropertyName = "Checked";
            value4.DataObject = this.txtHiddenItemsList;
            value4.PropertyName = "Text";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4 });
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkHideSysHidItems, 0, 0);
            panel.Controls.Add(this.chkUseHiddenItemsList, 0, 1);
            panel.Name = "tlpBack";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.txtHiddenItemsList);
            base.Controls.Add(panel);
            base.Name = "HiddenItemsOptionControl";
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkHideSysHidItems.Checked = VirtualFilePanelSettings.Default.HideSysHidItems;
            this.chkUseHiddenItemsList.Checked = VirtualFilePanelSettings.Default.UseHiddenItemsList;
            this.txtHiddenItemsList.Text = VirtualFilePanelSettings.Default.HiddenItemsList;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            VirtualFilePanelSettings.Default.HideSysHidItems = this.chkHideSysHidItems.Checked;
            VirtualFilePanelSettings.Default.UseHiddenItemsList = this.chkUseHiddenItemsList.Checked;
            VirtualFilePanelSettings.Default.HiddenItemsList = this.txtHiddenItemsList.Text;
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

