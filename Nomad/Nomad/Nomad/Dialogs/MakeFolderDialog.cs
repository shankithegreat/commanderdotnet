namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MakeFolderDialog : BasicDialog
    {
        private AutoCompleteProvider AutoComplete;
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private ComboBox cmbNewFolder;
        private IContainer components = null;
        private TableLayoutPanel tlpButtons;
        private VirtualItemToolStrip tsCurrentFolder;
        private ValidatorProvider Validator;

        public MakeFolderDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.AutoComplete.PreviewEnvironmentVariable += new EventHandler<PreviewEnvironmentVariableEventArgs>(AutoCompleteEvents.PreviewEnvironmentVariable);
            this.AutoComplete.PreviewFileSystemInfo += new EventHandler<PreviewFileSystemInfoEventArgs>(AutoCompleteEvents.PreviewFileSystemInfo);
        }

        private void cmbNewFolder_Validating(object sender, CancelEventArgs e)
        {
            string newFolderName = this.NewFolderName;
            if (string.IsNullOrEmpty(newFolderName))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = PathHelper.GetPathType(newFolderName) == ~PathType.Unknown;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute(IWin32Window owner, IVirtualFolder currentFolder)
        {
            this.tsCurrentFolder.Add(currentFolder);
            HistorySettings.PopulateComboBox(this.cmbNewFolder, HistorySettings.Default.NewFolderName);
            CustomFileSystemFolder folder = currentFolder as CustomFileSystemFolder;
            if (folder != null)
            {
                this.AutoComplete.CurrentDirectory = folder.FullName;
            }
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                HistorySettings.Default.AddStringToNewFolderName(this.NewFolderName);
                return true;
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MakeFolderDialog));
            this.cmbNewFolder = new ComboBox();
            this.tsCurrentFolder = new VirtualItemToolStrip(this.components);
            this.tlpButtons = new TableLayoutPanel();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.bvlButtons = new Bevel();
            this.Validator = new ValidatorProvider();
            this.AutoComplete = new AutoCompleteProvider();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            panel.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblNewFolder");
            label.Name = "lblNewFolder";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(label, 0, 2);
            panel.Controls.Add(this.cmbNewFolder, 0, 3);
            panel.Controls.Add(control, 0, 0);
            panel.Controls.Add(this.tsCurrentFolder, 0, 1);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            this.AutoComplete.SetAutoComplete(this.cmbNewFolder, true);
            manager.ApplyResources(this.cmbNewFolder, "cmbNewFolder");
            this.cmbNewFolder.Name = "cmbNewFolder";
            this.Validator.SetValidateOn(this.cmbNewFolder, ValidateOn.TextChanged);
            this.cmbNewFolder.Validating += new CancelEventHandler(this.cmbNewFolder_Validating);
            manager.ApplyResources(control, "lblItem");
            control.Name = "lblItem";
            this.tsCurrentFolder.BackColor = SystemColors.ButtonFace;
            manager.ApplyResources(this.tsCurrentFolder, "tsCurrentFolder");
            this.tsCurrentFolder.GripStyle = ToolStripGripStyle.Hidden;
            this.tsCurrentFolder.Name = "tsCurrentFolder";
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.DialogResult = DialogResult.OK;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            this.AutoComplete.UseEnvironmentVariablesSource = Settings.Default.UseACSEnvironmentVariables;
            this.AutoComplete.UseFileSystemSource = Settings.Default.UseACSFileSystem;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MakeFolderDialog";
            base.ShowInTaskbar = false;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnShown(EventArgs e)
        {
            this.cmbNewFolder.AutoCompleteMode = Settings.Default.AutoCompleteMode;
            base.OnShown(e);
            base.Validate();
        }

        public string NewFolderName
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(this.cmbNewFolder.Text.Trim());
            }
            set
            {
                this.cmbNewFolder.Text = value;
            }
        }
    }
}

