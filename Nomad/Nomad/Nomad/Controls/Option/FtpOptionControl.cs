namespace Nomad.Controls.Option
{
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.FileSystem.Ftp;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class FtpOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkStoreCredential;
        private CheckBox chkUseCache;
        private CheckBox chkUsePassive;
        private CheckBox chkUsePrefetch;
        private ComboBoxEx cmbEncoding;
        private ComboBoxEx cmbUploadFileNameCasing;
        private IContainer components;
        private Label lblEncoding;
        internal FtpSettings Settings;
        private PropertyValuesWatcher ValuesWatcher;

        public FtpOptionControl() : this(FtpSettings.Default)
        {
        }

        internal FtpOptionControl(FtpSettings settings)
        {
            this.components = null;
            this.InitializeComponent();
            this.cmbUploadFileNameCasing.DataSource = Enum.GetValues(typeof(CharacterCasing));
            Dictionary<string, Encoding> dictionary = new Dictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase);
            dictionary.Add(Encoding.Default.WebName, Encoding.Default);
            Encoding encoding = Encoding.GetEncoding(Windows.GetOEMCP());
            dictionary[encoding.WebName] = encoding;
            dictionary[Encoding.UTF8.WebName] = Encoding.UTF8;
            foreach (string str in StringHelper.SplitString(settings.AdditionalEncodings, new char[] { ',' }))
            {
                if (!dictionary.ContainsKey(str))
                {
                    try
                    {
                        Encoding encoding2 = Encoding.GetEncoding(str);
                        dictionary[encoding2.WebName] = encoding2;
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
            this.cmbEncoding.DataSource = new List<Encoding>(dictionary.Values);
            this.Settings = settings;
        }

        private void chkUseCache_CheckedChanged(object sender, EventArgs e)
        {
            this.chkUsePrefetch.Enabled = this.chkUseCache.Checked;
            this.chkUsePrefetch.Checked = this.chkUsePrefetch.Checked && this.chkUsePrefetch.Enabled;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FtpOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            this.chkStoreCredential = new CheckBox();
            this.cmbUploadFileNameCasing = new ComboBoxEx();
            this.chkUsePrefetch = new CheckBox();
            this.chkUseCache = new CheckBox();
            this.chkUsePassive = new CheckBox();
            this.lblEncoding = new Label();
            this.cmbEncoding = new ComboBoxEx();
            this.ValuesWatcher = new PropertyValuesWatcher();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblUploadFileNameCasing");
            label.Name = "lblUploadFileNameCasing";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(label, 0, 4);
            panel.Controls.Add(this.chkStoreCredential, 0, 3);
            panel.Controls.Add(this.cmbUploadFileNameCasing, 1, 4);
            panel.Controls.Add(this.chkUsePrefetch, 0, 2);
            panel.Controls.Add(this.chkUseCache, 0, 1);
            panel.Controls.Add(this.chkUsePassive, 0, 0);
            panel.Controls.Add(this.lblEncoding, 0, 5);
            panel.Controls.Add(this.cmbEncoding, 1, 5);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkStoreCredential, "chkStoreCredential");
            panel.SetColumnSpan(this.chkStoreCredential, 2);
            this.chkStoreCredential.Name = "chkStoreCredential";
            this.chkStoreCredential.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbUploadFileNameCasing, "cmbUploadFileNameCasing");
            this.cmbUploadFileNameCasing.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUploadFileNameCasing.FormattingEnabled = true;
            this.cmbUploadFileNameCasing.MaximumSize = new Size(150, 0);
            this.cmbUploadFileNameCasing.MinimumSize = new Size(110, 0);
            this.cmbUploadFileNameCasing.Name = "cmbUploadFileNameCasing";
            manager.ApplyResources(this.chkUsePrefetch, "chkUsePrefetch");
            panel.SetColumnSpan(this.chkUsePrefetch, 2);
            this.chkUsePrefetch.Name = "chkUsePrefetch";
            this.chkUsePrefetch.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkUseCache, "chkUseCache");
            this.chkUseCache.Checked = true;
            this.chkUseCache.CheckState = CheckState.Checked;
            panel.SetColumnSpan(this.chkUseCache, 2);
            this.chkUseCache.Name = "chkUseCache";
            this.chkUseCache.UseVisualStyleBackColor = true;
            this.chkUseCache.CheckedChanged += new EventHandler(this.chkUseCache_CheckedChanged);
            manager.ApplyResources(this.chkUsePassive, "chkUsePassive");
            this.chkUsePassive.Checked = true;
            this.chkUsePassive.CheckState = CheckState.Checked;
            panel.SetColumnSpan(this.chkUsePassive, 2);
            this.chkUsePassive.Name = "chkUsePassive";
            this.chkUsePassive.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.lblEncoding, "lblEncoding");
            this.lblEncoding.Name = "lblEncoding";
            manager.ApplyResources(this.cmbEncoding, "cmbEncoding");
            this.cmbEncoding.DisplayMember = "EncodingName";
            this.cmbEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEncoding.FormattingEnabled = true;
            this.cmbEncoding.MaximumSize = new Size(150, 0);
            this.cmbEncoding.MinimumSize = new Size(110, 0);
            this.cmbEncoding.Name = "cmbEncoding";
            this.cmbEncoding.ValueMember = "WebName";
            value2.DataObject = this.chkUsePassive;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkUseCache;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkUsePrefetch;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkStoreCredential;
            value5.PropertyName = "Checked";
            value6.DataObject = this.cmbUploadFileNameCasing;
            value6.PropertyName = "SelectedItem";
            value7.DataObject = this.cmbEncoding;
            value7.PropertyName = "SelectedIndex";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "FtpOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkUsePassive.Checked = this.Settings.UsePassive;
            this.chkUseCache.Checked = this.Settings.UseCache;
            this.chkUsePrefetch.Checked = this.chkUseCache.Checked && this.Settings.UsePrefetch;
            this.chkStoreCredential.Checked = this.Settings.StoreCredential;
            this.cmbUploadFileNameCasing.SelectedItem = this.Settings.UploadFileNameCasing;
            if (string.IsNullOrEmpty(this.Settings.Encoding))
            {
                this.cmbEncoding.SelectedValue = Encoding.Default.WebName;
            }
            else
            {
                this.cmbEncoding.SelectedValue = this.Settings.Encoding;
            }
            this.chkUseCache_CheckedChanged(this.chkUseCache, EventArgs.Empty);
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            this.Settings.UsePassive = this.chkUsePassive.Checked;
            this.Settings.UseCache = this.chkUseCache.Checked;
            this.Settings.UsePrefetch = this.chkUseCache.Checked && this.chkUsePrefetch.Checked;
            this.Settings.StoreCredential = this.chkStoreCredential.Checked;
            this.Settings.UploadFileNameCasing = (CharacterCasing) this.cmbUploadFileNameCasing.SelectedItem;
            this.Settings.Encoding = (this.cmbEncoding.SelectedItem == Encoding.Default) ? string.Empty : ((string) this.cmbEncoding.SelectedValue);
        }

        public bool SaveSettings
        {
            get
            {
                return this.ValuesWatcher.AnyValueChanged;
            }
            set
            {
                if (!value)
                {
                    this.ValuesWatcher.RememberValues();
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

