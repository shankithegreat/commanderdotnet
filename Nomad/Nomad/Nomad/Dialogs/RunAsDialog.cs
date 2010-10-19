namespace Nomad.Dialogs
{
    using Microsoft;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Security;
    using System.Security.Principal;
    using System.Windows.Forms;

    public class RunAsDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkRunInThread;
        private CheckBox chkShowPassword;
        private ComboBox cmbCommandLine;
        private ComboBox cmbUserName;
        private IContainer components = null;
        private SecureString FPassword;
        private Label lblItem;
        private Label lblPassword;
        private Label lblUserName;
        private RadioButton rbAdministrator;
        private RadioButton rbCurrentUser;
        private RadioButton rbSpecifiedUser;
        private bool ShieldRequired;
        private VirtualItemToolStrip tsItem;
        private TextBoxEx txtPassword;

        public RunAsDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.chkShowPassword_CheckedChanged(this.chkShowPassword, EventArgs.Empty);
            ResourceManager manager = new SettingsManager.LocalizedResourceManager(typeof(RunAsDialog));
            this.rbCurrentUser.Text = string.Format(manager.GetString("rbCurrentUser.Text"), WindowsIdentity.GetCurrent().Name);
            this.rbAdministrator.Enabled = OS.IsWinVista;
            this.rbSpecifiedUser.Enabled = OS.IsWin2k;
            switch (OS.ElevationType)
            {
                case ElevationType.Default:
                {
                    WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                    this.ShieldRequired = !principal.IsInRole(WindowsBuiltInRole.Administrator);
                    break;
                }
                case ElevationType.Limited:
                    this.ShieldRequired = true;
                    break;
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = !this.chkShowPassword.Checked;
            this.txtPassword.UseSystemPasswordChar = flag;
            this.txtPassword.ShowCapsLock = flag;
            this.txtPassword.ShowInputLanguage = flag;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute(IWin32Window owner, IVirtualFileExecute file)
        {
            this.tsItem.Add(file);
            HistorySettings.PopulateComboBox(this.cmbCommandLine, HistorySettings.Default.RunAsArguments);
            HistorySettings.PopulateComboBox(this.cmbUserName, HistorySettings.Default.UserName);
            bool flag = base.ShowDialog(owner) == DialogResult.OK;
            if (flag)
            {
                HistorySettings.Default.AddStringToRunAsArguments(this.Arguments);
                if (OS.IsWin2k)
                {
                    this.FPassword = new SecureString();
                    foreach (char ch in this.txtPassword.Text)
                    {
                        this.FPassword.AppendChar(ch);
                    }
                    this.FPassword.MakeReadOnly();
                    if (this.rbSpecifiedUser.Checked)
                    {
                        HistorySettings.Default.AddStringToUserName(this.cmbUserName.Text);
                    }
                }
            }
            this.txtPassword.Text = string.Empty;
            return flag;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(RunAsDialog));
            this.lblUserName = new Label();
            this.lblPassword = new Label();
            this.txtPassword = new TextBoxEx();
            this.chkShowPassword = new CheckBox();
            this.cmbUserName = new ComboBox();
            this.rbCurrentUser = new RadioButton();
            this.rbAdministrator = new RadioButton();
            this.rbSpecifiedUser = new RadioButton();
            this.chkRunInThread = new CheckBox();
            this.cmbCommandLine = new ComboBox();
            this.lblItem = new Label();
            this.tsItem = new VirtualItemToolStrip(this.components);
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.bvlButtons = new Bevel();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            GroupBox box = new GroupBox();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            TableLayoutPanel panel3 = new TableLayoutPanel();
            panel.SuspendLayout();
            box.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblCommandLine");
            label.Name = "lblCommandLine";
            manager.ApplyResources(panel, "tlpUserAccount");
            panel.Controls.Add(this.lblUserName, 1, 3);
            panel.Controls.Add(this.lblPassword, 1, 4);
            panel.Controls.Add(this.txtPassword, 2, 4);
            panel.Controls.Add(this.chkShowPassword, 2, 5);
            panel.Controls.Add(this.cmbUserName, 2, 3);
            panel.Controls.Add(this.rbCurrentUser, 0, 0);
            panel.Controls.Add(this.rbAdministrator, 0, 1);
            panel.Controls.Add(this.rbSpecifiedUser, 0, 2);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpUserAccount";
            manager.ApplyResources(this.lblUserName, "lblUserName");
            this.lblUserName.MinimumSize = new Size(80, 0);
            this.lblUserName.Name = "lblUserName";
            manager.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.MinimumSize = new Size(80, 0);
            this.lblPassword.Name = "lblPassword";
            manager.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            manager.ApplyResources(this.chkShowPassword, "chkShowPassword");
            this.chkShowPassword.Checked = Settings.Default.RunAsShowPassword;
            this.chkShowPassword.DataBindings.Add(new Binding("Checked", Settings.Default, "RunAsShowPassword", true, DataSourceUpdateMode.OnPropertyChanged));
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new EventHandler(this.chkShowPassword_CheckedChanged);
            manager.ApplyResources(this.cmbUserName, "cmbUserName");
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Name = "cmbUserName";
            manager.ApplyResources(this.rbCurrentUser, "rbCurrentUser");
            this.rbCurrentUser.Checked = true;
            panel.SetColumnSpan(this.rbCurrentUser, 3);
            this.rbCurrentUser.Name = "rbCurrentUser";
            this.rbCurrentUser.TabStop = true;
            this.rbCurrentUser.UseVisualStyleBackColor = true;
            this.rbCurrentUser.Click += new EventHandler(this.rbCurrentUser_Click);
            manager.ApplyResources(this.rbAdministrator, "rbAdministrator");
            panel.SetColumnSpan(this.rbAdministrator, 3);
            this.rbAdministrator.Name = "rbAdministrator";
            this.rbAdministrator.TabStop = true;
            this.rbAdministrator.UseVisualStyleBackColor = true;
            this.rbAdministrator.Click += new EventHandler(this.rbCurrentUser_Click);
            manager.ApplyResources(this.rbSpecifiedUser, "rbSpecifiedUser");
            panel.SetColumnSpan(this.rbSpecifiedUser, 3);
            this.rbSpecifiedUser.Name = "rbSpecifiedUser";
            this.rbSpecifiedUser.UseVisualStyleBackColor = true;
            this.rbSpecifiedUser.Click += new EventHandler(this.rbCurrentUser_Click);
            manager.ApplyResources(box, "grpUserAccount");
            box.Controls.Add(panel);
            box.Name = "grpUserAccount";
            box.TabStop = false;
            manager.ApplyResources(panel2, "tlpBack");
            panel2.Controls.Add(label, 0, 2);
            panel2.Controls.Add(box, 0, 4);
            panel2.Controls.Add(this.chkRunInThread, 0, 5);
            panel2.Controls.Add(this.cmbCommandLine, 0, 3);
            panel2.Controls.Add(this.lblItem, 0, 0);
            panel2.Controls.Add(this.tsItem, 0, 1);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpBack";
            manager.ApplyResources(this.chkRunInThread, "chkRunInThread");
            this.chkRunInThread.Checked = true;
            this.chkRunInThread.CheckState = CheckState.Indeterminate;
            this.chkRunInThread.Name = "chkRunInThread";
            this.chkRunInThread.ThreeState = true;
            this.chkRunInThread.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.cmbCommandLine, "cmbCommandLine");
            this.cmbCommandLine.FormattingEnabled = true;
            this.cmbCommandLine.Name = "cmbCommandLine";
            manager.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            this.tsItem.BackColor = SystemColors.ButtonFace;
            this.tsItem.GripStyle = ToolStripGripStyle.Hidden;
            manager.ApplyResources(this.tsItem, "tsItem");
            this.tsItem.Name = "tsItem";
            manager.ApplyResources(panel3, "tlpButtons");
            panel3.Controls.Add(this.btnOk, 1, 0);
            panel3.Controls.Add(this.btnCancel, 2, 0);
            panel3.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel3.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel3);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel2);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "RunAsDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.RunAsDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            box.ResumeLayout(false);
            box.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void rbCurrentUser_Click(object sender, EventArgs e)
        {
            bool flag = this.rbSpecifiedUser.Checked;
            this.lblUserName.Enabled = flag;
            this.cmbUserName.Enabled = flag;
            this.lblPassword.Enabled = flag;
            this.txtPassword.Enabled = flag;
            this.chkShowPassword.Enabled = flag;
            this.btnOk.SetElevationRequiredState(this.rbAdministrator.Checked && this.ShieldRequired);
            if (this.cmbUserName.Enabled)
            {
                this.cmbUserName.Select();
            }
        }

        private void RunAsDialog_Shown(object sender, EventArgs e)
        {
            this.txtPassword.Text = string.Empty;
            this.rbCurrentUser.Checked = true;
            this.rbCurrentUser_Click(this.rbCurrentUser, null);
            this.cmbCommandLine.Select();
        }

        public string Arguments
        {
            get
            {
                return this.cmbCommandLine.Text;
            }
            set
            {
                this.cmbCommandLine.Text = value;
            }
        }

        public SecureString Password
        {
            get
            {
                return this.FPassword;
            }
        }

        public ExecuteAsUser RunAs
        {
            get
            {
                if (this.rbAdministrator.Checked)
                {
                    return ExecuteAsUser.Administrator;
                }
                if (this.rbSpecifiedUser.Checked)
                {
                    return ExecuteAsUser.SpecifiedUser;
                }
                return ExecuteAsUser.CurrentUser;
            }
        }

        public CheckState RunInThread
        {
            get
            {
                return this.chkRunInThread.CheckState;
            }
            set
            {
                this.chkRunInThread.CheckState = value;
            }
        }

        public string UserName
        {
            get
            {
                return (this.rbCurrentUser.Checked ? null : this.cmbUserName.Text);
            }
            set
            {
                this.cmbUserName.Text = value;
            }
        }
    }
}

