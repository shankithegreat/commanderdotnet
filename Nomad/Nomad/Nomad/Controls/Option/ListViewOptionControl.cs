namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ListViewOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkHideNameExtension;
        private CheckBox chkShowColumnLines;
        private CheckBox chkShowUpFolderItem;
        private IContainer components = null;
        private NumericUpDown nudMinListColumnWidth;
        private PropertyValuesWatcher ValuesWatcher;

        public ListViewOptionControl()
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ListViewOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            this.chkHideNameExtension = new CheckBox();
            this.chkShowColumnLines = new CheckBox();
            this.nudMinListColumnWidth = new NumericUpDown();
            this.chkShowUpFolderItem = new CheckBox();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            panel.SuspendLayout();
            this.nudMinListColumnWidth.BeginInit();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(control, 0, 3);
            panel.Controls.Add(this.chkHideNameExtension, 0, 2);
            panel.Controls.Add(this.chkShowColumnLines, 0, 1);
            panel.Controls.Add(this.nudMinListColumnWidth, 1, 3);
            panel.Controls.Add(this.chkShowUpFolderItem, 0, 0);
            panel.Name = "tlpBack";
            manager.ApplyResources(control, "lblMinListColumnWidth");
            control.Name = "lblMinListColumnWidth";
            manager.ApplyResources(this.chkHideNameExtension, "chkHideNameExtension");
            panel.SetColumnSpan(this.chkHideNameExtension, 2);
            this.chkHideNameExtension.Name = "chkHideNameExtension";
            this.chkHideNameExtension.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowColumnLines, "chkShowColumnLines");
            panel.SetColumnSpan(this.chkShowColumnLines, 2);
            this.chkShowColumnLines.Name = "chkShowColumnLines";
            this.chkShowColumnLines.UseVisualStyleBackColor = true;
            int[] bits = new int[4];
            bits[0] = 5;
            this.nudMinListColumnWidth.Increment = new decimal(bits);
            manager.ApplyResources(this.nudMinListColumnWidth, "nudMinListColumnWidth");
            bits = new int[4];
            bits[0] = 500;
            this.nudMinListColumnWidth.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 50;
            this.nudMinListColumnWidth.Minimum = new decimal(bits);
            this.nudMinListColumnWidth.Name = "nudMinListColumnWidth";
            bits = new int[4];
            bits[0] = 100;
            this.nudMinListColumnWidth.Value = new decimal(bits);
            manager.ApplyResources(this.chkShowUpFolderItem, "chkShowUpFolderItem");
            panel.SetColumnSpan(this.chkShowUpFolderItem, 2);
            this.chkShowUpFolderItem.Name = "chkShowUpFolderItem";
            this.chkShowUpFolderItem.UseVisualStyleBackColor = true;
            value2.DataObject = this.chkHideNameExtension;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkShowColumnLines;
            value3.PropertyName = "Checked";
            value4.DataObject = this.nudMinListColumnWidth;
            value4.PropertyName = "Value";
            value5.DataObject = this.chkShowUpFolderItem;
            value5.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "ListViewOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.nudMinListColumnWidth.EndInit();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkShowUpFolderItem.Checked = VirtualFilePanelSettings.Default.ShowUpFolderItem;
            this.chkHideNameExtension.Checked = VirtualFilePanelSettings.Default.HideNameExtension;
            this.chkShowColumnLines.Checked = VirtualFilePanelSettings.Default.ShowColumnLines;
            try
            {
                this.nudMinListColumnWidth.Value = VirtualFilePanelSettings.Default.MinListColumnWidth;
            }
            catch (ArgumentOutOfRangeException)
            {
                this.nudMinListColumnWidth.Value = this.nudMinListColumnWidth.Minimum;
            }
            if (Settings.Default.ExplorerTheme && ListViewEx.IsExplorerThemeSupported)
            {
                this.chkShowColumnLines.Checked = true;
                this.chkShowColumnLines.Enabled = false;
            }
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            VirtualFilePanelSettings.Default.ShowUpFolderItem = this.chkShowUpFolderItem.Checked;
            VirtualFilePanelSettings.Default.HideNameExtension = this.chkHideNameExtension.Checked;
            VirtualFilePanelSettings.Default.ShowColumnLines = this.chkShowColumnLines.Checked;
            VirtualFilePanelSettings.Default.MinListColumnWidth = Convert.ToInt32(this.nudMinListColumnWidth.Value);
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

