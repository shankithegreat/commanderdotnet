namespace Nomad.Controls.Option
{
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Dialogs;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class ConfirmationOptionControl : UserControl, IPersistComponentSettings
    {
        private Button btnRestoreConfirmations;
        private CheckBox chkConfirmAnotherInstance;
        private CheckBox chkConfirmBookmarkFolder;
        private CheckBox chkConfirmCloseTabs;
        private CheckBox chkConfirmCopyAlternateDataStreams;
        private CheckBox chkConfirmCreateAnotherLink;
        private CheckBox chkConfirmDeleteNonEmptyFolder;
        private CheckBox chkConfirmDeleteReadOnlyFile;
        private CheckBox chkConfirmDestinationItem;
        private CheckBox chkConfirmDragDrop;
        private CheckBox chkConfirmExtractOnRun;
        private CheckBox chkConfirmPaste;
        private CheckBox chkConfirmSaveTabs;
        private CheckBox chkConfirmUploadChangedFileBack;
        private CheckBox chkShowNavigationError;
        private CheckBox chkShowSearchError;
        private IContainer components = null;
        private PropertyValuesWatcher ValuesWatcher;

        public ConfirmationOptionControl()
        {
            this.InitializeComponent();
            if (!Application.RenderWithVisualStyles)
            {
                this.btnRestoreConfirmations.BackColor = SystemColors.Control;
            }
        }

        private void btnResetConfirmations_Click(object sender, EventArgs e)
        {
            this.ResetComponentSettings();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ConfirmationOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            PropertyValue value4 = new PropertyValue();
            PropertyValue value5 = new PropertyValue();
            PropertyValue value6 = new PropertyValue();
            PropertyValue value7 = new PropertyValue();
            PropertyValue value8 = new PropertyValue();
            PropertyValue value9 = new PropertyValue();
            PropertyValue value10 = new PropertyValue();
            PropertyValue value11 = new PropertyValue();
            PropertyValue value12 = new PropertyValue();
            PropertyValue value13 = new PropertyValue();
            PropertyValue value14 = new PropertyValue();
            PropertyValue value15 = new PropertyValue();
            PropertyValue value16 = new PropertyValue();
            this.chkConfirmDeleteNonEmptyFolder = new CheckBox();
            this.chkConfirmDeleteReadOnlyFile = new CheckBox();
            this.chkConfirmDragDrop = new CheckBox();
            this.chkConfirmPaste = new CheckBox();
            this.chkConfirmCreateAnotherLink = new CheckBox();
            this.chkShowSearchError = new CheckBox();
            this.chkConfirmBookmarkFolder = new CheckBox();
            this.chkConfirmDestinationItem = new CheckBox();
            this.chkConfirmAnotherInstance = new CheckBox();
            this.chkShowNavigationError = new CheckBox();
            this.chkConfirmCloseTabs = new CheckBox();
            this.chkConfirmExtractOnRun = new CheckBox();
            this.chkConfirmSaveTabs = new CheckBox();
            this.chkConfirmUploadChangedFileBack = new CheckBox();
            this.btnRestoreConfirmations = new Button();
            this.ValuesWatcher = new PropertyValuesWatcher();
            this.chkConfirmCopyAlternateDataStreams = new CheckBox();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(this.chkConfirmDeleteNonEmptyFolder, "chkConfirmDeleteNonEmptyFolder");
            this.chkConfirmDeleteNonEmptyFolder.Name = "chkConfirmDeleteNonEmptyFolder";
            this.chkConfirmDeleteNonEmptyFolder.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmDeleteReadOnlyFile, "chkConfirmDeleteReadOnlyFile");
            this.chkConfirmDeleteReadOnlyFile.Name = "chkConfirmDeleteReadOnlyFile";
            this.chkConfirmDeleteReadOnlyFile.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmDragDrop, "chkConfirmDragDrop");
            this.chkConfirmDragDrop.Name = "chkConfirmDragDrop";
            this.chkConfirmDragDrop.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmPaste, "chkConfirmPaste");
            this.chkConfirmPaste.Name = "chkConfirmPaste";
            this.chkConfirmPaste.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmCreateAnotherLink, "chkConfirmCreateAnotherLink");
            this.chkConfirmCreateAnotherLink.Name = "chkConfirmCreateAnotherLink";
            this.chkConfirmCreateAnotherLink.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowSearchError, "chkShowSearchError");
            this.chkShowSearchError.Name = "chkShowSearchError";
            this.chkShowSearchError.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmBookmarkFolder, "chkConfirmBookmarkFolder");
            this.chkConfirmBookmarkFolder.Name = "chkConfirmBookmarkFolder";
            this.chkConfirmBookmarkFolder.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmDestinationItem, "chkConfirmDestinationItem");
            this.chkConfirmDestinationItem.Name = "chkConfirmDestinationItem";
            this.chkConfirmDestinationItem.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmAnotherInstance, "chkConfirmAnotherInstance");
            this.chkConfirmAnotherInstance.Name = "chkConfirmAnotherInstance";
            this.chkConfirmAnotherInstance.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowNavigationError, "chkShowNavigationError");
            this.chkShowNavigationError.Name = "chkShowNavigationError";
            this.chkShowNavigationError.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmCloseTabs, "chkConfirmCloseTabs");
            this.chkConfirmCloseTabs.Name = "chkConfirmCloseTabs";
            this.chkConfirmCloseTabs.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmExtractOnRun, "chkConfirmExtractOnRun");
            this.chkConfirmExtractOnRun.Name = "chkConfirmExtractOnRun";
            this.chkConfirmExtractOnRun.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmSaveTabs, "chkConfirmSaveTabs");
            this.chkConfirmSaveTabs.Name = "chkConfirmSaveTabs";
            this.chkConfirmSaveTabs.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkConfirmUploadChangedFileBack, "chkConfirmUploadChangedFileBack");
            this.chkConfirmUploadChangedFileBack.Name = "chkConfirmUploadChangedFileBack";
            this.chkConfirmUploadChangedFileBack.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnRestoreConfirmations, "btnRestoreConfirmations");
            this.btnRestoreConfirmations.Name = "btnRestoreConfirmations";
            this.btnRestoreConfirmations.UseVisualStyleBackColor = true;
            this.btnRestoreConfirmations.Click += new EventHandler(this.btnResetConfirmations_Click);
            value2.DataObject = this.chkConfirmDeleteNonEmptyFolder;
            value2.PropertyName = "Checked";
            value3.DataObject = this.chkConfirmDeleteReadOnlyFile;
            value3.PropertyName = "Checked";
            value4.DataObject = this.chkConfirmDragDrop;
            value4.PropertyName = "Checked";
            value5.DataObject = this.chkConfirmPaste;
            value5.PropertyName = "Checked";
            value6.DataObject = this.chkConfirmCreateAnotherLink;
            value6.PropertyName = "Checked";
            value7.DataObject = this.chkShowSearchError;
            value7.PropertyName = "Checked";
            value8.DataObject = this.chkConfirmBookmarkFolder;
            value8.PropertyName = "Checked";
            value9.DataObject = this.chkConfirmDestinationItem;
            value9.PropertyName = "Checked";
            value10.DataObject = this.chkConfirmAnotherInstance;
            value10.PropertyName = "Checked";
            value11.DataObject = this.chkShowNavigationError;
            value11.PropertyName = "Checked";
            value12.DataObject = this.chkConfirmCloseTabs;
            value12.PropertyName = "Checked";
            value13.DataObject = this.chkConfirmExtractOnRun;
            value13.PropertyName = "Checked";
            value14.DataObject = this.chkConfirmSaveTabs;
            value14.PropertyName = "Checked";
            value15.DataObject = this.chkConfirmUploadChangedFileBack;
            value15.PropertyName = "Checked";
            value16.DataObject = this.chkConfirmCopyAlternateDataStreams;
            value16.PropertyName = "Checked";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16 });
            manager.ApplyResources(this.chkConfirmCopyAlternateDataStreams, "chkConfirmCopyAlternateDataStreams");
            this.chkConfirmCopyAlternateDataStreams.Name = "chkConfirmCopyAlternateDataStreams";
            this.chkConfirmCopyAlternateDataStreams.UseVisualStyleBackColor = true;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.chkConfirmCopyAlternateDataStreams);
            base.Controls.Add(this.chkConfirmUploadChangedFileBack);
            base.Controls.Add(this.btnRestoreConfirmations);
            base.Controls.Add(this.chkConfirmSaveTabs);
            base.Controls.Add(this.chkConfirmExtractOnRun);
            base.Controls.Add(this.chkConfirmCloseTabs);
            base.Controls.Add(this.chkShowNavigationError);
            base.Controls.Add(this.chkConfirmAnotherInstance);
            base.Controls.Add(this.chkConfirmDestinationItem);
            base.Controls.Add(this.chkConfirmBookmarkFolder);
            base.Controls.Add(this.chkConfirmPaste);
            base.Controls.Add(this.chkConfirmDragDrop);
            base.Controls.Add(this.chkConfirmCreateAnotherLink);
            base.Controls.Add(this.chkShowSearchError);
            base.Controls.Add(this.chkConfirmDeleteReadOnlyFile);
            base.Controls.Add(this.chkConfirmDeleteNonEmptyFolder);
            base.Name = "ConfirmationOptionControl";
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.chkConfirmDeleteNonEmptyFolder.Checked = ConfirmationSettings.Default.DeleteNonEmptyFolder;
            this.chkConfirmDeleteReadOnlyFile.Checked = ConfirmationSettings.Default.DeleteReadOnlyFile;
            this.chkConfirmCopyAlternateDataStreams.Checked = ConfirmationSettings.Default.CopyAlternateDataStreams;
            this.chkConfirmDragDrop.Checked = ConfirmationSettings.Default.DragDrop;
            this.chkConfirmPaste.Checked = ConfirmationSettings.Default.Paste;
            this.chkConfirmCreateAnotherLink.Checked = ConfirmationSettings.Default.CreateAnotherLink;
            this.chkConfirmBookmarkFolder.Checked = ConfirmationSettings.Default.BookmarkFolder;
            this.chkConfirmCloseTabs.Checked = ConfirmationSettings.Default.CloseTabs;
            this.chkShowSearchError.Checked = ConfirmationSettings.Default.SearchError;
            this.chkShowNavigationError.Checked = ConfirmationSettings.Default.NavigateError;
            this.chkConfirmSaveTabs.Checked = ConfirmationSettings.Default.SaveTabs == MessageDialogResult.None;
            this.chkConfirmSaveTabs.Enabled = !this.chkConfirmSaveTabs.Checked;
            this.chkConfirmExtractOnRun.Checked = ConfirmationSettings.Default.ExtractOnRun == MessageDialogResult.None;
            this.chkConfirmExtractOnRun.Enabled = !this.chkConfirmExtractOnRun.Checked;
            this.chkConfirmUploadChangedFileBack.Checked = ConfirmationSettings.Default.UploadChangedFileBack == MessageDialogResult.None;
            this.chkConfirmUploadChangedFileBack.Enabled = !this.chkConfirmUploadChangedFileBack.Checked;
            this.chkConfirmAnotherInstance.Checked = ConfirmationSettings.Default.AnotherInstance == MessageDialogResult.None;
            this.chkConfirmAnotherInstance.Enabled = !this.chkConfirmAnotherInstance.Checked;
            this.chkConfirmDestinationItem.Checked = ConfirmationSettings.Default.CopyDestinationItem == CopyDestinationItem.Ask;
            this.chkConfirmDestinationItem.Enabled = !this.chkConfirmDestinationItem.Checked;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
            foreach (PropertyValue value2 in this.ValuesWatcher.Items)
            {
                ((CheckBox) value2.DataObject).Checked = true;
            }
        }

        public void SaveComponentSettings()
        {
            ConfirmationSettings.Default.DeleteNonEmptyFolder = this.chkConfirmDeleteNonEmptyFolder.Checked;
            ConfirmationSettings.Default.DeleteReadOnlyFile = this.chkConfirmDeleteReadOnlyFile.Checked;
            ConfirmationSettings.Default.CopyAlternateDataStreams = this.chkConfirmCopyAlternateDataStreams.Checked;
            ConfirmationSettings.Default.DragDrop = this.chkConfirmDragDrop.Checked;
            ConfirmationSettings.Default.Paste = this.chkConfirmPaste.Checked;
            ConfirmationSettings.Default.CreateAnotherLink = this.chkConfirmCreateAnotherLink.Checked;
            ConfirmationSettings.Default.BookmarkFolder = this.chkConfirmBookmarkFolder.Checked;
            ConfirmationSettings.Default.CloseTabs = this.chkConfirmCloseTabs.Checked;
            ConfirmationSettings.Default.SearchError = this.chkShowSearchError.Checked;
            ConfirmationSettings.Default.NavigateError = this.chkShowNavigationError.Checked;
            if (this.chkConfirmSaveTabs.Checked)
            {
                ConfirmationSettings.Default.SaveTabs = MessageDialogResult.None;
            }
            if (this.chkConfirmExtractOnRun.Checked)
            {
                ConfirmationSettings.Default.ExtractOnRun = MessageDialogResult.None;
            }
            if (this.chkConfirmUploadChangedFileBack.Checked)
            {
                ConfirmationSettings.Default.UploadChangedFileBack = MessageDialogResult.None;
            }
            if (this.chkConfirmAnotherInstance.Checked)
            {
                ConfirmationSettings.Default.AnotherInstance = MessageDialogResult.None;
            }
            if (this.chkConfirmDestinationItem.Checked)
            {
                ConfirmationSettings.Default.CopyDestinationItem = CopyDestinationItem.Ask;
            }
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

