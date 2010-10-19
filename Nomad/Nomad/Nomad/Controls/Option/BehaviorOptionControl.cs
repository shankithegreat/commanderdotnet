namespace Nomad.Controls.Option
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class BehaviorOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox chkClearSelectionBeforeWork;
        private CheckBox chkDeleteToBin;
        private CheckBox chkHideNotReadyDrives;
        private CheckBox chkMinimizeToTray;
        private CheckBox chkProcessFolderShortcuts;
        private CheckBox chkRightClickSelect;
        private CheckBox chkRunInThread;
        private CheckBox chkShowKeyboardCues;
        private ComboBoxEx cmbEmptySpaceDoubleClickAction;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public BehaviorOptionControl()
        {
            this.InitializeComponent();
            this.cmbEmptySpaceDoubleClickAction.DataSource = Enum.GetValues(typeof(DoubleClickAction));
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BehaviorOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            PropertyValue value8 = new PropertyValue();
            PropertyValue value9 = new PropertyValue();
            PropertyValue value10 = new PropertyValue();
            this.chkRightClickSelect = new CheckBox();
            this.chkRunInThread = new CheckBox();
            this.chkClearSelectionBeforeWork = new CheckBox();
            this.chkShowKeyboardCues = new CheckBox();
            this.chkDeleteToBin = new CheckBox();
            this.chkHideNotReadyDrives = new CheckBox();
            this.chkProcessFolderShortcuts = new CheckBox();
            this.cmbEmptySpaceDoubleClickAction = new ComboBoxEx();
            this.ValuesWatcher = new PropertyValuesWatcher();
            this.chkMinimizeToTray = new CheckBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            panel.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkRightClickSelect, 0, 0);
            panel.Controls.Add(this.chkRunInThread, 0, 6);
            panel.Controls.Add(this.chkClearSelectionBeforeWork, 0, 1);
            panel.Controls.Add(this.chkShowKeyboardCues, 0, 5);
            panel.Controls.Add(this.chkDeleteToBin, 0, 2);
            panel.Controls.Add(this.chkHideNotReadyDrives, 0, 4);
            panel.Controls.Add(this.chkProcessFolderShortcuts, 0, 3);
            panel.Controls.Add(control, 0, 8);
            panel.Controls.Add(this.cmbEmptySpaceDoubleClickAction, 1, 8);
            panel.Controls.Add(this.chkMinimizeToTray, 0, 7);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkRightClickSelect, "chkRightClickSelect");
            panel.SetColumnSpan(this.chkRightClickSelect, 2);
            this.chkRightClickSelect.Name = "chkRightClickSelect";
            this.chkRightClickSelect.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkRunInThread, "chkRunInThread");
            panel.SetColumnSpan(this.chkRunInThread, 2);
            this.chkRunInThread.Name = "chkRunInThread";
            this.chkRunInThread.ThreeState = true;
            this.chkRunInThread.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkClearSelectionBeforeWork, "chkClearSelectionBeforeWork");
            panel.SetColumnSpan(this.chkClearSelectionBeforeWork, 2);
            this.chkClearSelectionBeforeWork.Name = "chkClearSelectionBeforeWork";
            this.chkClearSelectionBeforeWork.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowKeyboardCues, "chkShowKeyboardCues");
            panel.SetColumnSpan(this.chkShowKeyboardCues, 2);
            this.chkShowKeyboardCues.Name = "chkShowKeyboardCues";
            this.chkShowKeyboardCues.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkDeleteToBin, "chkDeleteToBin");
            panel.SetColumnSpan(this.chkDeleteToBin, 2);
            this.chkDeleteToBin.Name = "chkDeleteToBin";
            this.chkDeleteToBin.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkHideNotReadyDrives, "chkHideNotReadyDrives");
            panel.SetColumnSpan(this.chkHideNotReadyDrives, 2);
            this.chkHideNotReadyDrives.Name = "chkHideNotReadyDrives";
            this.chkHideNotReadyDrives.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkProcessFolderShortcuts, "chkProcessFolderShortcuts");
            panel.SetColumnSpan(this.chkProcessFolderShortcuts, 2);
            this.chkProcessFolderShortcuts.Name = "chkProcessFolderShortcuts";
            this.chkProcessFolderShortcuts.UseVisualStyleBackColor = true;
            manager.ApplyResources(control, "lblEmptySpaceDoubleClickAction");
            control.Name = "lblEmptySpaceDoubleClickAction";
            manager.ApplyResources(this.cmbEmptySpaceDoubleClickAction, "cmbEmptySpaceDoubleClickAction");
            this.cmbEmptySpaceDoubleClickAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEmptySpaceDoubleClickAction.FormattingEnabled = true;
            this.cmbEmptySpaceDoubleClickAction.MinimumSize = new Size(120, 0);
            this.cmbEmptySpaceDoubleClickAction.Name = "cmbEmptySpaceDoubleClickAction";
            value2.DataObject = this.chkDeleteToBin;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkHideNotReadyDrives;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkProcessFolderShortcuts;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkShowKeyboardCues;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkRightClickSelect;
            value6.PropertyName = "Checked";
            value7.DataObject = this.chkRunInThread;
            value7.PropertyName = "CheckState";
            value8.DataObject = this.chkClearSelectionBeforeWork;
            value8.PropertyName = "Checked";
            value9.DataObject = this.cmbEmptySpaceDoubleClickAction;
            value9.PropertyName = "SelectedItem";
            value10.DataObject = this.chkMinimizeToTray;
            value10.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7, value8, value9, value10 });
            manager.ApplyResources(this.chkMinimizeToTray, "chkMinimizeToTray");
            panel.SetColumnSpan(this.chkMinimizeToTray, 2);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.UseVisualStyleBackColor = true;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(panel);
            base.Name = "BehaviorOptionControl";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkDeleteToBin.Checked = Settings.Default.DeleteToBin;
            this.chkProcessFolderShortcuts.Checked = Settings.Default.ProcessFolderShortcuts;
            this.chkHideNotReadyDrives.Checked = Settings.Default.HideNotReadyDrives;
            this.chkShowKeyboardCues.Checked = Settings.Default.ShowKeyboardCues;
            this.chkRightClickSelect.Checked = Settings.Default.RightClickSelect;
            this.chkClearSelectionBeforeWork.Checked = Settings.Default.ClearSelectionBeforeWork;
            this.chkRunInThread.CheckState = Settings.Default.RunInThread;
            this.chkMinimizeToTray.Checked = Settings.Default.MinimizeToTray;
            this.cmbEmptySpaceDoubleClickAction.SelectedItem = VirtualFilePanelSettings.Default.EmptySpaceDoubleClickAction;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            Settings.Default.DeleteToBin = this.chkDeleteToBin.Checked;
            Settings.Default.ProcessFolderShortcuts = this.chkProcessFolderShortcuts.Checked;
            Settings.Default.HideNotReadyDrives = this.chkHideNotReadyDrives.Checked;
            Settings.Default.ShowKeyboardCues = this.chkShowKeyboardCues.Checked;
            Settings.Default.RightClickSelect = this.chkRightClickSelect.Checked;
            Settings.Default.ClearSelectionBeforeWork = this.chkClearSelectionBeforeWork.Checked;
            Settings.Default.RunInThread = this.chkRunInThread.CheckState;
            Settings.Default.MinimizeToTray = this.chkMinimizeToTray.Checked;
            VirtualFilePanelSettings.Default.EmptySpaceDoubleClickAction = (DoubleClickAction) this.cmbEmptySpaceDoubleClickAction.SelectedItem;
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

