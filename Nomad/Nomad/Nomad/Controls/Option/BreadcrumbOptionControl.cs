namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.Controls;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class BreadcrumbOptionControl : UserControl, IPersistComponentSettings, IUpdateCulture
    {
        private CheckBox chkShowActiveState;
        private CheckBox chkShowDriveMenuOnHover;
        private CheckBox chkShowFolderIcon;
        private CheckBox chkShowIconForEveryFolder;
        private CheckBox chkShowShortRootName;
        private ComboBoxEx cmbBreadcrumbStyle;
        private ComboBoxEx cmbDriveProperty;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public BreadcrumbOptionControl()
        {
            this.InitializeComponent();
            this.UpdateCulture();
        }

        private void cmbBreadcrumbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkShowFolderIcon.Enabled = this.cmbBreadcrumbStyle.SelectedIndex == 1;
            this.chkShowIconForEveryFolder.Enabled = this.cmbBreadcrumbStyle.SelectedIndex == 0;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BreadcrumbOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            PropertyValue value8 = new PropertyValue();
            this.chkShowShortRootName = new CheckBox();
            this.chkShowIconForEveryFolder = new CheckBox();
            this.cmbDriveProperty = new ComboBoxEx();
            this.chkShowActiveState = new CheckBox();
            this.chkShowDriveMenuOnHover = new CheckBox();
            this.chkShowFolderIcon = new CheckBox();
            this.cmbBreadcrumbStyle = new ComboBoxEx();
            this.ValuesWatcher = new PropertyValuesWatcher();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblDriveProperty");
            panel.SetColumnSpan(label, 2);
            label.Name = "lblDriveProperty";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkShowShortRootName, 0, 2);
            panel.Controls.Add(this.chkShowIconForEveryFolder, 0, 4);
            panel.Controls.Add(label, 0, 6);
            panel.Controls.Add(this.cmbDriveProperty, 2, 6);
            panel.Controls.Add(this.chkShowActiveState, 0, 1);
            panel.Controls.Add(this.chkShowDriveMenuOnHover, 0, 5);
            panel.Controls.Add(control, 0, 0);
            panel.Controls.Add(this.chkShowFolderIcon, 0, 3);
            panel.Controls.Add(this.cmbBreadcrumbStyle, 1, 0);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkShowShortRootName, "chkShowShortRootName");
            panel.SetColumnSpan(this.chkShowShortRootName, 3);
            this.chkShowShortRootName.Name = "chkShowShortRootName";
            this.chkShowShortRootName.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowIconForEveryFolder, "chkShowIconForEveryFolder");
            panel.SetColumnSpan(this.chkShowIconForEveryFolder, 3);
            this.chkShowIconForEveryFolder.DataBindings.Add(new Binding("Enabled", Settings.Default, "ShowIcons", true, DataSourceUpdateMode.Never));
            this.chkShowIconForEveryFolder.Enabled = Settings.Default.ShowIcons;
            this.chkShowIconForEveryFolder.Name = "chkShowIconForEveryFolder";
            this.chkShowIconForEveryFolder.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbDriveProperty, "cmbDriveProperty");
            this.cmbDriveProperty.DisplayMember = "LocalizedName";
            this.cmbDriveProperty.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDriveProperty.FormattingEnabled = true;
            this.cmbDriveProperty.Items.AddRange(new object[] { manager.GetString("cmbDriveProperty.Items") });
            this.cmbDriveProperty.MinimumSize = new Size(120, 0);
            this.cmbDriveProperty.Name = "cmbDriveProperty";
            manager.ApplyResources(this.chkShowActiveState, "chkShowActiveState");
            panel.SetColumnSpan(this.chkShowActiveState, 3);
            this.chkShowActiveState.Name = "chkShowActiveState";
            this.chkShowActiveState.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowDriveMenuOnHover, "chkShowDriveMenuOnHover");
            panel.SetColumnSpan(this.chkShowDriveMenuOnHover, 3);
            this.chkShowDriveMenuOnHover.Name = "chkShowDriveMenuOnHover";
            this.chkShowDriveMenuOnHover.UseVisualStyleBackColor = true;
            manager.ApplyResources(control, "lblBreadcrumbStyle");
            control.Name = "lblBreadcrumbStyle";
            manager.ApplyResources(this.chkShowFolderIcon, "chkShowFolderIcon");
            panel.SetColumnSpan(this.chkShowFolderIcon, 3);
            this.chkShowFolderIcon.Name = "chkShowFolderIcon";
            this.chkShowFolderIcon.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbBreadcrumbStyle, "cmbBreadcrumbStyle");
            panel.SetColumnSpan(this.cmbBreadcrumbStyle, 2);
            this.cmbBreadcrumbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbBreadcrumbStyle.FormattingEnabled = true;
            this.cmbBreadcrumbStyle.Items.AddRange(new object[] { manager.GetString("cmbBreadcrumbStyle.Items"), manager.GetString("cmbBreadcrumbStyle.Items1") });
            this.cmbBreadcrumbStyle.MinimumSize = new Size(120, 0);
            this.cmbBreadcrumbStyle.Name = "cmbBreadcrumbStyle";
            this.cmbBreadcrumbStyle.SelectedIndexChanged += new EventHandler(this.cmbBreadcrumbStyle_SelectedIndexChanged);
            value2.DataObject = this.chkShowShortRootName;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkShowIconForEveryFolder;
            value3.PropertyName = "Checked";
            value4.DataObject = this.cmbDriveProperty;
            value4.PropertyName = "SelectedIndex";
            value5.DataObject = this.chkShowActiveState;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkShowDriveMenuOnHover;
            value6.PropertyName = "Checked";
            value7.DataObject = this.chkShowFolderIcon;
            value7.PropertyName = "Checked";
            value8.DataObject = this.cmbBreadcrumbStyle;
            value8.PropertyName = "SelectedIndex";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7, value8 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "BreadcrumbOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            PathView breadcrumbOptions = VirtualFilePanelSettings.Default.BreadcrumbOptions;
            this.cmbBreadcrumbStyle.SelectedIndex = ((breadcrumbOptions & PathView.VistaLikeBreadcrumb) > PathView.ShowNormalRootName) ? 1 : 0;
            this.chkShowActiveState.Checked = (breadcrumbOptions & PathView.ShowActiveState) > PathView.ShowNormalRootName;
            this.chkShowShortRootName.Checked = (breadcrumbOptions & PathView.ShowShortRootName) > PathView.ShowNormalRootName;
            this.chkShowFolderIcon.Checked = (breadcrumbOptions & PathView.ShowFolderIcon) > PathView.ShowNormalRootName;
            this.chkShowIconForEveryFolder.Checked = (breadcrumbOptions & PathView.ShowIconForEveryFolder) > PathView.ShowNormalRootName;
            this.chkShowDriveMenuOnHover.Checked = (breadcrumbOptions & PathView.ShowDriveMenuOnHover) > PathView.ShowNormalRootName;
            VirtualProperty property = VirtualProperty.Get(VirtualFilePanelSettings.Default.DriveMenuProperty);
            if (property == null)
            {
                this.cmbDriveProperty.SelectedIndex = 0;
            }
            else
            {
                this.cmbDriveProperty.SelectedItem = property;
            }
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            PathView view = ((((((this.cmbBreadcrumbStyle.SelectedIndex == 1) ? PathView.VistaLikeBreadcrumb : PathView.ShowNormalRootName) | (this.chkShowActiveState.Checked ? PathView.ShowActiveState : PathView.ShowNormalRootName)) | (this.chkShowShortRootName.Checked ? PathView.ShowShortRootName : PathView.ShowNormalRootName)) | (this.chkShowFolderIcon.Checked ? PathView.ShowFolderIcon : PathView.ShowNormalRootName)) | ((this.chkShowIconForEveryFolder.Checked && (this.cmbBreadcrumbStyle.SelectedIndex == 0)) ? PathView.ShowIconForEveryFolder : PathView.ShowNormalRootName)) | (this.chkShowDriveMenuOnHover.Checked ? PathView.ShowDriveMenuOnHover : PathView.ShowNormalRootName);
            VirtualFilePanelSettings.Default.BreadcrumbOptions = view;
            VirtualFilePanelSettings.Default.DriveMenuProperty = (this.cmbDriveProperty.SelectedIndex > 0) ? ((VirtualProperty) this.cmbDriveProperty.SelectedItem).PropertyName : string.Empty;
        }

        public void UpdateCulture()
        {
            this.cmbBreadcrumbStyle.SelectedIndex = 0;
            VirtualPropertySet groupSet = VirtualProperty.GetGroupSet(VirtualProperty.GetGroup("Volume"));
            foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) groupSet)
            {
                this.cmbDriveProperty.Items.Add(property);
            }
            this.cmbDriveProperty.SelectedIndex = 0;
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

