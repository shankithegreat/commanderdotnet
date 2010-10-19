namespace Nomad.Controls.Option
{
    using Microsoft;
    using Nomad.Controls;
    using Nomad.Workers;
    using Nomad.Workers.Configuration;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class CopyOptionControl : UserControl, IPersistComponentSettings
    {
        private CheckBox[] CheckBoxList;
        private CheckBox chkAsyncFileCopy;
        private CheckBox chkCheckFreeSpace;
        private CheckBox chkClearROFromCD;
        private CheckBox chkCopyFolderTime;
        private CheckBox chkCopyItemACL;
        private CheckBox chkCopyItemTime;
        private CheckBox chkSkipEmptyFolders;
        private CheckBox chkUseSystemCopy;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public CopyOptionControl()
        {
            this.InitializeComponent();
            this.chkCopyItemTime.Tag = CopyWorkerOptions.CopyItemTime;
            this.chkCopyFolderTime.Tag = CopyWorkerOptions.CopyFolderTime;
            this.chkCopyItemACL.Tag = CopyWorkerOptions.CopyACL;
            this.chkClearROFromCD.Tag = CopyWorkerOptions.ClearROFromCD;
            this.chkCheckFreeSpace.Tag = CopyWorkerOptions.CheckFreeSpace;
            this.chkSkipEmptyFolders.Tag = CopyWorkerOptions.SkipEmptyFolders;
            this.chkUseSystemCopy.Tag = CopyWorkerOptions.UseSystemCopy;
            this.chkUseSystemCopy.Enabled = OS.IsWinNT;
            this.CheckBoxList = new CheckBox[] { this.chkCopyItemTime, this.chkCopyFolderTime, this.chkCopyItemACL, this.chkClearROFromCD, this.chkCheckFreeSpace, this.chkSkipEmptyFolders, this.chkUseSystemCopy };
        }

        private void chkCopyItemTime_CheckedChanged(object sender, EventArgs e)
        {
            this.chkCopyFolderTime.Enabled = this.chkCopyItemTime.Checked;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CopyOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            PropertyValue value8 = new PropertyValue();
            PropertyValue value9 = new PropertyValue();
            this.chkAsyncFileCopy = new CheckBox();
            this.chkCheckFreeSpace = new CheckBox();
            this.chkClearROFromCD = new CheckBox();
            this.chkCopyFolderTime = new CheckBox();
            this.chkCopyItemACL = new CheckBox();
            this.chkCopyItemTime = new CheckBox();
            this.chkSkipEmptyFolders = new CheckBox();
            this.chkUseSystemCopy = new CheckBox();
            this.ValuesWatcher = new PropertyValuesWatcher();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(this.chkAsyncFileCopy, "chkAsyncFileCopy");
            this.chkAsyncFileCopy.Checked = true;
            this.chkAsyncFileCopy.CheckState = CheckState.Indeterminate;
            this.chkAsyncFileCopy.Name = "chkAsyncFileCopy";
            this.chkAsyncFileCopy.ThreeState = true;
            this.chkAsyncFileCopy.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCheckFreeSpace, "chkCheckFreeSpace");
            this.chkCheckFreeSpace.Checked = true;
            this.chkCheckFreeSpace.CheckState = CheckState.Checked;
            this.chkCheckFreeSpace.Name = "chkCheckFreeSpace";
            this.chkCheckFreeSpace.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkClearROFromCD, "chkClearROFromCD");
            this.chkClearROFromCD.Name = "chkClearROFromCD";
            this.chkClearROFromCD.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCopyFolderTime, "chkCopyFolderTime");
            this.chkCopyFolderTime.Name = "chkCopyFolderTime";
            this.chkCopyFolderTime.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCopyItemACL, "chkCopyItemACL");
            this.chkCopyItemACL.Name = "chkCopyItemACL";
            this.chkCopyItemACL.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCopyItemTime, "chkCopyItemTime");
            this.chkCopyItemTime.Checked = true;
            this.chkCopyItemTime.CheckState = CheckState.Checked;
            this.chkCopyItemTime.Name = "chkCopyItemTime";
            this.chkCopyItemTime.UseVisualStyleBackColor = true;
            this.chkCopyItemTime.CheckedChanged += new EventHandler(this.chkCopyItemTime_CheckedChanged);
            manager.ApplyResources(this.chkSkipEmptyFolders, "chkSkipEmptyFolders");
            this.chkSkipEmptyFolders.Name = "chkSkipEmptyFolders";
            this.chkSkipEmptyFolders.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkUseSystemCopy, "chkUseSystemCopy");
            this.chkUseSystemCopy.Name = "chkUseSystemCopy";
            this.chkUseSystemCopy.UseVisualStyleBackColor = true;
            value2.DataObject = this.chkAsyncFileCopy;
            value2.PropertyName = "CheckState";
            value3.DataObject = this.chkCheckFreeSpace;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkClearROFromCD;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkCopyFolderTime;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkCopyItemACL;
            value6.PropertyName = "Checked";
            value7.DataObject = this.chkCopyItemTime;
            value7.PropertyName = "Checked";
            value8.DataObject = this.chkSkipEmptyFolders;
            value8.PropertyName = "Checked";
            value9.DataObject = this.chkUseSystemCopy;
            value9.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7, value8, value9 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.chkUseSystemCopy);
            base.Controls.Add(this.chkCopyItemACL);
            base.Controls.Add(this.chkCopyItemTime);
            base.Controls.Add(this.chkAsyncFileCopy);
            base.Controls.Add(this.chkSkipEmptyFolders);
            base.Controls.Add(this.chkCheckFreeSpace);
            base.Controls.Add(this.chkClearROFromCD);
            base.Controls.Add(this.chkCopyFolderTime);
            base.Name = "CopyOptionControl";
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            CopyWorkerOptions defaultCopyOptions = CopySettings.Default.DefaultCopyOptions;
            foreach (CheckBox box in this.CheckBoxList)
            {
                box.Checked = box.Enabled && ((defaultCopyOptions & ((CopyWorkerOptions) box.Tag)) > 0);
            }
            if ((defaultCopyOptions & CopyWorkerOptions.AutoAsyncCopy) > 0)
            {
                this.chkAsyncFileCopy.CheckState = CheckState.Indeterminate;
            }
            else
            {
                this.chkAsyncFileCopy.CheckState = ((defaultCopyOptions & CopyWorkerOptions.AsyncCopy) > 0) ? CheckState.Checked : CheckState.Unchecked;
            }
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            CopyWorkerOptions options = CopySettings.Default.DefaultCopyOptions & ~(CopyWorkerOptions.AutoAsyncCopy | CopyWorkerOptions.AsyncCopy);
            foreach (CheckBox box in this.CheckBoxList)
            {
                if (box.Checked)
                {
                    options |= (CopyWorkerOptions) box.Tag;
                }
                else
                {
                    options &= ~((CopyWorkerOptions) box.Tag);
                }
            }
            if (this.chkAsyncFileCopy.CheckState == CheckState.Indeterminate)
            {
                options |= CopyWorkerOptions.AutoAsyncCopy;
            }
            else if (this.chkAsyncFileCopy.Checked)
            {
                options |= CopyWorkerOptions.AsyncCopy;
            }
            CopySettings.Default.DefaultCopyOptions = options;
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

