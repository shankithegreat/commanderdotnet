namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows.Forms;

    public class PasswordDialog : BasicDialog
    {
        private CheckBox chkRememberPassword;
        protected CheckBox chkShowPassword;
        private IContainer components = null;
        private SecureString FPassword;
        private PictureBox imgKeys;
        private Label lblPassword;
        private TextBoxEx txtPassword;

        public PasswordDialog()
        {
            this.InitializeComponent();
            this.chkShowPassword_CheckedChanged(this.chkShowPassword, EventArgs.Empty);
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

        public bool Execute(IWin32Window owner, string archiveName)
        {
            this.SetArchiveName(archiveName);
            return (base.ShowDialog(owner) == DialogResult.OK);
        }

        public static SecureString GetPassword(IWin32Window owner, string archiveName, out bool rememberPassword)
        {
            using (PasswordDialog dialog = new PasswordDialog())
            {
                DialogResult result;
                dialog.SetArchiveName(archiveName);
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
                    rememberPassword = dialog.RememberPassword;
                    return dialog.Password;
                }
                rememberPassword = false;
                return null;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PasswordDialog));
            this.chkShowPassword = new CheckBox();
            this.txtPassword = new TextBoxEx();
            this.lblPassword = new Label();
            this.imgKeys = new PictureBox();
            this.chkRememberPassword = new CheckBox();
            Button button = new Button();
            Button button2 = new Button();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            Bevel bevel = new Bevel();
            panel.SuspendLayout();
            ((ISupportInitialize) this.imgKeys).BeginInit();
            panel2.SuspendLayout();
            base.SuspendLayout();
            button.DialogResult = DialogResult.OK;
            manager.ApplyResources(button, "btnOk");
            button.Name = "btnOk";
            button.UseVisualStyleBackColor = true;
            button2.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(button2, "btnCancel");
            button2.Name = "btnCancel";
            button2.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.chkShowPassword, 1, 2);
            panel.Controls.Add(this.txtPassword, 1, 1);
            panel.Controls.Add(this.lblPassword, 1, 0);
            panel.Controls.Add(this.imgKeys, 0, 0);
            panel.Controls.Add(this.chkRememberPassword, 1, 3);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.chkShowPassword, "chkShowPassword");
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new EventHandler(this.chkShowPassword_CheckedChanged);
            manager.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            manager.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            manager.ApplyResources(this.imgKeys, "imgKeys");
            this.imgKeys.Name = "imgKeys";
            panel.SetRowSpan(this.imgKeys, 4);
            this.imgKeys.TabStop = false;
            manager.ApplyResources(this.chkRememberPassword, "chkRememberPassword");
            this.chkRememberPassword.Checked = true;
            this.chkRememberPassword.CheckState = CheckState.Checked;
            this.chkRememberPassword.Name = "chkRememberPassword";
            this.chkRememberPassword.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.Controls.Add(button, 1, 0);
            panel2.Controls.Add(button2, 2, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpButtons";
            manager.ApplyResources(bevel, "bvlButtons");
            bevel.ForeColor = SystemColors.ControlDarkDark;
            bevel.Name = "bvlButtons";
            bevel.Sides = Border3DSide.Top;
            bevel.Style = Border3DStyle.Flat;
            base.AcceptButton = button;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = button2;
            base.Controls.Add(panel2);
            base.Controls.Add(bevel);
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PasswordDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.PasswordDialog_Shown);
            base.FormClosed += new FormClosedEventHandler(this.PasswordDialog_FormClosed);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.imgKeys).EndInit();
            panel2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.imgKeys.Image = new Icon(Resources.Keys, this.imgKeys.Size).ToBitmap();
        }

        private void PasswordDialog_FormClosed(object sender, FormClosedEventArgs e)
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

        private void PasswordDialog_Shown(object sender, EventArgs e)
        {
            this.txtPassword.Select();
        }

        public void SetArchiveName(string archiveName)
        {
            ResourceManager manager = new ResourceManager(typeof(PasswordDialog));
            this.lblPassword.Text = string.Format(manager.GetString("lblPassword.Text"), archiveName);
        }

        public SecureString Password
        {
            get
            {
                return this.FPassword;
            }
        }

        public bool RememberPassword
        {
            get
            {
                return this.chkRememberPassword.Checked;
            }
            set
            {
                this.chkRememberPassword.Checked = value;
            }
        }

        private delegate DialogResult ShowDialogHandler(IWin32Window owner);
    }
}

