namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class IconOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkDisableExtractSlowIcons;
        private CheckBox chkExtractIcons;
        private CheckBox chkShowIcons;
        private CheckBox chkShowOverlayIcons;
        private ComboBoxEx cmbDelayedExtractIcons;
        private ComboBoxEx cmbImageProvider;
        private IContainer components = null;
        private Label lblDelayedExtractIcons;
        private LinkLabel lblGetMorePacks;
        private Label lblImageProvider;
        private PropertyValuesWatcher ValuesWatcher;

        public IconOptionControl()
        {
            this.InitializeComponent();
            this.cmbImageProvider.SelectedIndex = 0;
            this.cmbDelayedExtractIcons.SelectedIndex = 0;
        }

        private void chkExtractIcons_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
            if (!this.chkExtractIcons.Checked)
            {
                this.chkDisableExtractSlowIcons.Checked = true;
                this.cmbDelayedExtractIcons.SelectedIndex = 0;
            }
        }

        private void chkShowIcons_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(IconOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            this.chkShowIcons = new CheckBox();
            this.chkExtractIcons = new CheckBox();
            this.lblDelayedExtractIcons = new Label();
            this.chkDisableExtractSlowIcons = new CheckBox();
            this.chkShowOverlayIcons = new CheckBox();
            this.lblImageProvider = new Label();
            this.cmbImageProvider = new ComboBoxEx();
            this.lblGetMorePacks = new LinkLabel();
            this.cmbDelayedExtractIcons = new ComboBoxEx();
            this.ValuesWatcher = new PropertyValuesWatcher();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkShowIcons, 0, 0);
            panel.Controls.Add(this.chkExtractIcons, 0, 2);
            panel.Controls.Add(this.lblDelayedExtractIcons, 0, 5);
            panel.Controls.Add(this.chkDisableExtractSlowIcons, 0, 3);
            panel.Controls.Add(this.chkShowOverlayIcons, 0, 4);
            panel.Controls.Add(this.lblImageProvider, 0, 1);
            panel.Controls.Add(this.cmbImageProvider, 1, 1);
            panel.Controls.Add(this.lblGetMorePacks, 4, 1);
            panel.Controls.Add(this.cmbDelayedExtractIcons, 2, 5);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkShowIcons, "chkShowIcons");
            this.chkShowIcons.Checked = true;
            this.chkShowIcons.CheckState = CheckState.Checked;
            panel.SetColumnSpan(this.chkShowIcons, 5);
            this.chkShowIcons.Name = "chkShowIcons";
            this.chkShowIcons.UseVisualStyleBackColor = true;
            this.chkShowIcons.CheckedChanged += new EventHandler(this.chkShowIcons_CheckedChanged);
            manager.ApplyResources(this.chkExtractIcons, "chkExtractIcons");
            panel.SetColumnSpan(this.chkExtractIcons, 5);
            this.chkExtractIcons.Name = "chkExtractIcons";
            this.chkExtractIcons.UseVisualStyleBackColor = true;
            this.chkExtractIcons.CheckedChanged += new EventHandler(this.chkExtractIcons_CheckedChanged);
            manager.ApplyResources(this.lblDelayedExtractIcons, "lblDelayedExtractIcons");
            panel.SetColumnSpan(this.lblDelayedExtractIcons, 2);
            this.lblDelayedExtractIcons.Name = "lblDelayedExtractIcons";
            manager.ApplyResources(this.chkDisableExtractSlowIcons, "chkDisableExtractSlowIcons");
            panel.SetColumnSpan(this.chkDisableExtractSlowIcons, 5);
            this.chkDisableExtractSlowIcons.Name = "chkDisableExtractSlowIcons";
            this.chkDisableExtractSlowIcons.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowOverlayIcons, "chkShowOverlayIcons");
            panel.SetColumnSpan(this.chkShowOverlayIcons, 5);
            this.chkShowOverlayIcons.Name = "chkShowOverlayIcons";
            this.chkShowOverlayIcons.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.lblImageProvider, "lblImageProvider");
            this.lblImageProvider.Name = "lblImageProvider";
            manager.ApplyResources(this.cmbImageProvider, "cmbImageProvider");
            panel.SetColumnSpan(this.cmbImageProvider, 3);
            this.cmbImageProvider.DisplayMember = "Value";
            this.cmbImageProvider.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbImageProvider.FormattingEnabled = true;
            this.cmbImageProvider.Items.AddRange(new object[] { manager.GetString("cmbImageProvider.Items"), manager.GetString("cmbImageProvider.Items1") });
            this.cmbImageProvider.MinimumSize = new Size(140, 0);
            this.cmbImageProvider.Name = "cmbImageProvider";
            this.cmbImageProvider.ValueMember = "Key";
            manager.ApplyResources(this.lblGetMorePacks, "lblGetMorePacks");
            this.lblGetMorePacks.Name = "lblGetMorePacks";
            this.lblGetMorePacks.TabStop = true;
            this.lblGetMorePacks.Tag = "http://www.nomad-net.info/iconpack";
            this.lblGetMorePacks.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblGetMorePacks_LinkClicked);
            manager.ApplyResources(this.cmbDelayedExtractIcons, "cmbDelayedExtractIcons");
            panel.SetColumnSpan(this.cmbDelayedExtractIcons, 3);
            this.cmbDelayedExtractIcons.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDelayedExtractIcons.FormattingEnabled = true;
            this.cmbDelayedExtractIcons.Items.AddRange(new object[] { manager.GetString("cmbDelayedExtractIcons.Items"), manager.GetString("cmbDelayedExtractIcons.Items1"), manager.GetString("cmbDelayedExtractIcons.Items2") });
            this.cmbDelayedExtractIcons.MinimumSize = new Size(150, 0);
            this.cmbDelayedExtractIcons.Name = "cmbDelayedExtractIcons";
            value2.DataObject = this.chkShowIcons;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkExtractIcons;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkDisableExtractSlowIcons;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkShowOverlayIcons;
            value5.PropertyName = "Checked";
            value6.DataObject = this.cmbDelayedExtractIcons;
            value6.PropertyName = "SelectedIndex";
            value7.DataObject = this.cmbImageProvider;
            value7.PropertyName = "SelectedIndex";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "IconOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lblGetMorePacks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((string) ((Control) sender).Tag);
        }

        public void LoadComponentSettings()
        {
            this.chkShowIcons.Checked = Settings.Default.ShowIcons;
            IconOptions iconOptions = Settings.Default.IconOptions;
            this.chkDisableExtractSlowIcons.Checked = (iconOptions & IconOptions.DisableExtractSlowIcons) > 0;
            this.chkShowOverlayIcons.Checked = (iconOptions & IconOptions.ShowOverlayIcons) > 0;
            this.chkExtractIcons.Checked = (iconOptions & IconOptions.ExtractIcons) > 0;
            string path = Path.Combine(Application.StartupPath, "IconPack");
            if (Directory.Exists(path))
            {
                foreach (string str2 in Directory.GetFiles(path, "*.iconpack"))
                {
                    string fileName = Path.GetFileName(str2);
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(Path.Combine("IconPack", fileName), string.Format(Resources.sIconPackName, Path.GetFileNameWithoutExtension(fileName)));
                    this.cmbImageProvider.Items.Add(item);
                    if (string.Equals(item.Key, Settings.Default.ImageProvider, StringComparison.OrdinalIgnoreCase))
                    {
                        this.cmbImageProvider.SelectedIndex = this.cmbImageProvider.Items.Count - 1;
                    }
                }
            }
            if (Settings.Default.ImageProvider == typeof(ShellImageProvider).FullName)
            {
                this.cmbImageProvider.SelectedIndex = 1;
            }
            switch (Settings.Default.DelayedExtractMode)
            {
                case DelayedExtractMode.Always:
                    this.cmbDelayedExtractIcons.SelectedIndex = 1;
                    break;

                case DelayedExtractMode.OnSlowDrivesOnly:
                    this.cmbDelayedExtractIcons.SelectedIndex = 2;
                    break;

                default:
                    this.cmbDelayedExtractIcons.SelectedIndex = 0;
                    break;
            }
            this.chkExtractIcons_CheckedChanged(this.chkExtractIcons, EventArgs.Empty);
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            if (this.ValuesWatcher.IsValueChanged(this.chkShowIcons))
            {
                Settings.Default.ShowIcons = this.chkShowIcons.Checked;
            }
            if (this.ValuesWatcher.IsValueChanged(this.cmbImageProvider))
            {
                switch (this.cmbImageProvider.SelectedIndex)
                {
                    case 0:
                        Settings.Default.ImageProvider = string.Empty;
                        goto Label_00B6;

                    case 1:
                        Settings.Default.ImageProvider = typeof(ShellImageProvider).FullName;
                        goto Label_00B6;
                }
                KeyValuePair<string, string> selectedItem = (KeyValuePair<string, string>) this.cmbImageProvider.SelectedItem;
                Settings.Default.ImageProvider = selectedItem.Key;
            }
        Label_00B6:;
            Settings.Default.IconOptions = ((this.chkExtractIcons.Checked ? IconOptions.ExtractIcons : ((IconOptions) 0)) | ((this.chkExtractIcons.Checked && this.chkDisableExtractSlowIcons.Checked) ? IconOptions.DisableExtractSlowIcons : ((IconOptions) 0))) | (this.chkShowOverlayIcons.Checked ? IconOptions.ShowOverlayIcons : ((IconOptions) 0));
            if (this.ValuesWatcher.IsValueChanged(this.cmbDelayedExtractIcons))
            {
                DelayedExtractMode always;
                switch (this.cmbDelayedExtractIcons.SelectedIndex)
                {
                    case 1:
                        always = DelayedExtractMode.Always;
                        break;

                    case 2:
                        always = DelayedExtractMode.OnSlowDrivesOnly;
                        break;

                    default:
                        always = DelayedExtractMode.Never;
                        break;
                }
                Settings.Default.DelayedExtractMode = always;
            }
        }

        private void UpdateButtons()
        {
            this.chkExtractIcons.Enabled = this.chkShowIcons.Checked;
            this.chkDisableExtractSlowIcons.Enabled = this.chkShowIcons.Checked && this.chkExtractIcons.Checked;
            this.chkShowOverlayIcons.Enabled = this.chkShowIcons.Checked;
            this.lblImageProvider.Enabled = this.chkShowIcons.Checked;
            this.cmbImageProvider.Enabled = this.chkShowIcons.Checked;
            this.lblDelayedExtractIcons.Enabled = this.chkShowIcons.Checked && this.chkExtractIcons.Checked;
            this.cmbDelayedExtractIcons.Enabled = this.chkShowIcons.Checked && this.chkExtractIcons.Checked;
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

