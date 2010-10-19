namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows.Forms;

    public class BasicLoginDialog : BasicForm
    {
        private Button btnCancel;
        private Button btnOk;
        private CheckBox chkShowPassword;
        private ComboBox cmbUserName;
        private IContainer components = null;
        private SecureString FPassword;
        private PictureBox imgKeys;
        private Label lblMessage;
        private TextBoxEx txtPassword;

        public BasicLoginDialog()
        {
            this.InitializeComponent();
            this.chkShowPassword_CheckedChanged(this.chkShowPassword, EventArgs.Empty);
        }

        private void BasicLoginDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (base.DialogResult == DialogResult.OK)
            {
                this.FPassword = new SecureString();
                foreach (char ch in this.txtPassword.Text)
                {
                    this.FPassword.AppendChar(ch);
                }
                this.FPassword.MakeReadOnly();
            }
            this.txtPassword.Text = string.Empty;
        }

        private void BasicLoginDialog_Shown(object sender, EventArgs e)
        {
            this.cmbUserName.Select();
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

        public static bool GetLogin(IWin32Window owner, string message, ref string userName, out SecureString password)
        {
            using (BasicLoginDialog dialog = new BasicLoginDialog())
            {
                DialogResult result;
                dialog.Message = message;
                dialog.UserName = userName;
                Control control = owner as Control;
                if ((control != null) && control.InvokeRequired)
                {
                    result = (DialogResult) control.Invoke(new ShowDialogHandler(dialog.ShowDialog), new object[] { owner });
                }
                else if (owner != null)
                {
                    result = dialog.ShowDialog(owner);
                }
                else
                {
                    result = dialog.ShowDialog();
                }
                if (result == DialogResult.OK)
                {
                    userName = dialog.UserName;
                    password = dialog.Password;
                    return true;
                }
                userName = null;
                password = null;
                return false;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BasicLoginDialog));
            this.btnCancel = new Button();
            this.chkShowPassword = new CheckBox();
            this.cmbUserName = new ComboBox();
            this.txtPassword = new TextBoxEx();
            this.btnOk = new Button();
            this.imgKeys = new PictureBox();
            this.lblMessage = new Label();
            Label label = new Label();
            Label label2 = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            ((ISupportInitialize) this.imgKeys).BeginInit();
            panel.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblUserName");
            label.MinimumSize = new Size(80, 0);
            label.Name = "lblUserName";
            manager.ApplyResources(label2, "lblPassword");
            label2.MinimumSize = new Size(80, 0);
            label2.Name = "lblPassword";
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkShowPassword, "chkShowPassword");
            this.chkShowPassword.Checked = Settings.Default.RunAsShowPassword;
            panel.SetColumnSpan(this.chkShowPassword, 3);
            this.chkShowPassword.DataBindings.Add(new Binding("Checked", Settings.Default, "RunAsShowPassword", true, DataSourceUpdateMode.OnPropertyChanged));
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new EventHandler(this.chkShowPassword_CheckedChanged);
            panel.SetColumnSpan(this.cmbUserName, 3);
            manager.ApplyResources(this.cmbUserName, "cmbUserName");
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Name = "cmbUserName";
            panel.SetColumnSpan(this.txtPassword, 3);
            manager.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.imgKeys, "imgKeys");
            this.imgKeys.Name = "imgKeys";
            panel.SetRowSpan(this.imgKeys, 5);
            this.imgKeys.TabStop = false;
            manager.ApplyResources(this.lblMessage, "lblMessage");
            panel.SetColumnSpan(this.lblMessage, 4);
            this.lblMessage.MaximumSize = new Size(0x132, 0);
            this.lblMessage.Name = "lblMessage";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(label2, 1, 2);
            panel.Controls.Add(this.chkShowPassword, 2, 3);
            panel.Controls.Add(this.lblMessage, 1, 0);
            panel.Controls.Add(this.btnCancel, 4, 4);
            panel.Controls.Add(this.txtPassword, 2, 2);
            panel.Controls.Add(this.cmbUserName, 2, 1);
            panel.Controls.Add(this.imgKeys, 0, 0);
            panel.Controls.Add(this.btnOk, 3, 4);
            panel.Controls.Add(label, 1, 1);
            panel.Name = "tlpBack";
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "BasicLoginDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.BasicLoginDialog_Shown);
            base.FormClosed += new FormClosedEventHandler(this.BasicLoginDialog_FormClosed);
            ((ISupportInitialize) this.imgKeys).EndInit();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.imgKeys.Image = new Icon(Resources.Keys, this.imgKeys.Size).ToBitmap();
        }

        public string Message
        {
            get
            {
                return this.lblMessage.Text;
            }
            set
            {
                this.lblMessage.Text = value;
            }
        }

        public SecureString Password
        {
            get
            {
                return this.FPassword;
            }
        }

        public string UserName
        {
            get
            {
                return this.cmbUserName.Text;
            }
            set
            {
                this.cmbUserName.Text = value;
            }
        }

        private delegate DialogResult ShowDialogHandler(IWin32Window owner);
    }
}

