namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.Controls.Option;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class FtpConnectDialog : BasicDialog
    {
        private VisualStyleRenderer BackgroundRenderer;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private Bevel bvlButtons;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.ComboBox cmbUserName;
        private IContainer components = null;
        private FtpOptionControl FtpOptions;
        private PictureBox imgNote;
        private Label lblNote;
        private Label lblPassword;
        private Label lblServer;
        private Label lblUserName;
        private System.Windows.Forms.RadioButton rbAnonymous;
        private System.Windows.Forms.RadioButton rbCredentials;
        private TabControl tbcBack;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpBasic;
        private TableLayoutPanel tlpButtons;
        private TableLayoutPanel tlpNote;
        private TabPage tpBasic;
        private TabPage tpContext;
        private System.Windows.Forms.TextBox txtPassword;
        private ValidatorProvider Validator;

        public FtpConnectDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidFtpUri;
            this.imgNote.Image = IconSet.Information;
            this.FtpOptions.LoadComponentSettings();
            this.FtpOptions.Settings = new FtpSettings();
            this.chkShowPassword_CheckedChanged(this.chkShowPassword, EventArgs.Empty);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren(ValidationConstraints.Enabled))
            {
                Uri serverUri = this.ServerUri;
                if (this.FtpOptions.SaveSettings)
                {
                    this.FtpOptions.SaveComponentSettings();
                }
                base.DialogResult = DialogResult.OK;
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPassword.UseSystemPasswordChar = !this.chkShowPassword.Checked;
        }

        private void cmbServer_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cmbServer.Text.Trim()))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                try
                {
                    Uri serverUri = this.ServerUri;
                }
                catch (Exception exception)
                {
                    e.Cancel = true;
                    this.Validator.SetValidateError((Control) sender, exception.Message);
                }
            }
        }

        private void cmbUserName_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = string.IsNullOrEmpty(((Control) sender).Text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute()
        {
            HistorySettings.PopulateComboBox(this.cmbServer, HistorySettings.Default.FtpFolder);
            bool flag = base.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                HistorySettings.Default.AddStringToFtpFolder(this.cmbServer.Text.Trim());
            }
            return flag;
        }

        private void FtpConnectDialog_Shown(object sender, EventArgs e)
        {
            this.cmbServer.Select();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FtpConnectDialog));
            this.rbAnonymous = new System.Windows.Forms.RadioButton();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.lblPassword = new Label();
            this.rbCredentials = new System.Windows.Forms.RadioButton();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.lblUserName = new Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.tbcBack = new TabControl();
            this.tpBasic = new TabPage();
            this.tlpBasic = new TableLayoutPanel();
            this.lblServer = new Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.tlpNote = new TableLayoutPanel();
            this.lblNote = new Label();
            this.imgNote = new PictureBox();
            this.tpContext = new TabPage();
            this.FtpOptions = new FtpOptionControl();
            this.tlpBack = new TableLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Validator = new ValidatorProvider();
            this.tlpButtons = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            System.Windows.Forms.GroupBox box = new System.Windows.Forms.GroupBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            box.SuspendLayout();
            panel.SuspendLayout();
            this.tbcBack.SuspendLayout();
            this.tpBasic.SuspendLayout();
            this.tlpBasic.SuspendLayout();
            this.tlpNote.SuspendLayout();
            ((ISupportInitialize) this.imgNote).BeginInit();
            this.tpContext.SuspendLayout();
            this.tlpBack.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(box, "grpLogin");
            box.Controls.Add(panel);
            box.Name = "grpLogin";
            box.TabStop = false;
            manager.ApplyResources(panel, "tlpCredentials");
            panel.Controls.Add(this.rbAnonymous, 0, 0);
            panel.Controls.Add(this.chkShowPassword, 2, 4);
            panel.Controls.Add(this.lblPassword, 1, 3);
            panel.Controls.Add(this.rbCredentials, 0, 1);
            panel.Controls.Add(this.cmbUserName, 2, 2);
            panel.Controls.Add(this.lblUserName, 1, 2);
            panel.Controls.Add(this.txtPassword, 2, 3);
            panel.Name = "tlpCredentials";
            manager.ApplyResources(this.rbAnonymous, "rbAnonymous");
            this.rbAnonymous.Checked = true;
            panel.SetColumnSpan(this.rbAnonymous, 3);
            this.rbAnonymous.Name = "rbAnonymous";
            this.rbAnonymous.TabStop = true;
            this.rbAnonymous.UseVisualStyleBackColor = true;
            this.rbAnonymous.CheckedChanged += new EventHandler(this.rbAnonymous_CheckedChanged);
            manager.ApplyResources(this.chkShowPassword, "chkShowPassword");
            this.chkShowPassword.Checked = Settings.Default.RunAsShowPassword;
            this.chkShowPassword.DataBindings.Add(new Binding("Checked", Settings.Default, "RunAsShowPassword", true, DataSourceUpdateMode.OnPropertyChanged));
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new EventHandler(this.chkShowPassword_CheckedChanged);
            manager.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.MinimumSize = new Size(80, 0);
            this.lblPassword.Name = "lblPassword";
            manager.ApplyResources(this.rbCredentials, "rbCredentials");
            panel.SetColumnSpan(this.rbCredentials, 3);
            this.rbCredentials.Name = "rbCredentials";
            this.rbCredentials.UseVisualStyleBackColor = true;
            this.rbCredentials.CheckedChanged += new EventHandler(this.rbAnonymous_CheckedChanged);
            manager.ApplyResources(this.cmbUserName, "cmbUserName");
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Name = "cmbUserName";
            this.Validator.SetValidateOn(this.cmbUserName, ValidateOn.TextChanged);
            this.cmbUserName.Validating += new CancelEventHandler(this.cmbUserName_Validating);
            manager.ApplyResources(this.lblUserName, "lblUserName");
            this.lblUserName.MinimumSize = new Size(80, 0);
            this.lblUserName.Name = "lblUserName";
            manager.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            this.tbcBack.Controls.Add(this.tpBasic);
            this.tbcBack.Controls.Add(this.tpContext);
            manager.ApplyResources(this.tbcBack, "tbcBack");
            this.tbcBack.Name = "tbcBack";
            this.tbcBack.SelectedIndex = 0;
            this.tpBasic.Controls.Add(this.tlpBasic);
            manager.ApplyResources(this.tpBasic, "tpBasic");
            this.tpBasic.Name = "tpBasic";
            this.tpBasic.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpBasic, "tlpBasic");
            this.tlpBasic.Controls.Add(this.lblServer, 0, 0);
            this.tlpBasic.Controls.Add(box, 0, 2);
            this.tlpBasic.Controls.Add(this.cmbServer, 0, 1);
            this.tlpBasic.Controls.Add(this.tlpNote, 0, 3);
            this.tlpBasic.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBasic.Name = "tlpBasic";
            manager.ApplyResources(this.lblServer, "lblServer");
            this.lblServer.Name = "lblServer";
            manager.ApplyResources(this.cmbServer, "cmbServer");
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Name = "cmbServer";
            this.Validator.SetValidateOn(this.cmbServer, ValidateOn.TextChangedTimer);
            this.cmbServer.Validating += new CancelEventHandler(this.cmbServer_Validating);
            manager.ApplyResources(this.tlpNote, "tlpNote");
            this.tlpNote.Controls.Add(this.lblNote, 1, 0);
            this.tlpNote.Controls.Add(this.imgNote, 0, 0);
            this.tlpNote.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpNote.Name = "tlpNote";
            this.tlpNote.Paint += new PaintEventHandler(this.tlpNote_Paint);
            manager.ApplyResources(this.lblNote, "lblNote");
            this.lblNote.Name = "lblNote";
            manager.ApplyResources(this.imgNote, "imgNote");
            this.imgNote.Name = "imgNote";
            this.imgNote.TabStop = false;
            this.tpContext.Controls.Add(this.FtpOptions);
            manager.ApplyResources(this.tpContext, "tpContext");
            this.tpContext.Name = "tpContext";
            this.tpContext.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.FtpOptions, "FtpOptions");
            this.FtpOptions.Name = "FtpOptions";
            this.FtpOptions.SaveSettings = false;
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.tbcBack, 0, 0);
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpBack);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FtpConnectDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.FtpConnectDialog_Shown);
            box.ResumeLayout(false);
            box.PerformLayout();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tbcBack.ResumeLayout(false);
            this.tpBasic.ResumeLayout(false);
            this.tpBasic.PerformLayout();
            this.tlpBasic.ResumeLayout(false);
            this.tlpBasic.PerformLayout();
            this.tlpNote.ResumeLayout(false);
            this.tlpNote.PerformLayout();
            ((ISupportInitialize) this.imgNote).EndInit();
            this.tpContext.ResumeLayout(false);
            this.tpContext.PerformLayout();
            this.tlpBack.ResumeLayout(false);
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.lblNote.Font = new Font(SystemFonts.IconTitleFont.FontFamily, this.lblNote.Font.SizeInPoints);
            this.tbcBack.Height += (this.tlpBasic.Height - this.tpBasic.Height) + this.tpBasic.Padding.Vertical;
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolTip.Standard.Normal))
            {
                this.BackgroundRenderer = new VisualStyleRenderer(VisualStyleElement.ToolTip.Standard.Normal);
                this.tlpNote.ResetBackColor();
                this.imgNote.BackColor = Color.Transparent;
                this.lblNote.BackColor = Color.Transparent;
                this.lblNote.ForeColor = this.BackgroundRenderer.GetColor(ColorProperty.TextColor);
            }
            else
            {
                this.BackgroundRenderer = null;
                this.tlpNote.BackColor = SystemColors.Info;
                this.imgNote.ResetBackColor();
                this.lblNote.ResetBackColor();
                this.lblNote.ForeColor = SystemColors.InfoText;
            }
            base.OnThemeChanged(e);
        }

        private void rbAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.RadioButton) sender).Checked)
            {
                bool flag = this.rbCredentials.Checked;
                this.lblUserName.Enabled = flag;
                this.cmbUserName.Enabled = flag;
                this.lblPassword.Enabled = flag;
                this.txtPassword.Enabled = flag;
                this.chkShowPassword.Enabled = flag;
                if (this.cmbUserName.Enabled)
                {
                    this.cmbUserName.Select();
                    base.Validate();
                }
            }
        }

        private void tlpNote_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control) sender;
            if (this.BackgroundRenderer != null)
            {
                this.BackgroundRenderer.DrawBackground(e.Graphics, control.ClientRectangle);
            }
            else
            {
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, control.ClientSize.Width - 1, control.ClientSize.Height - 1);
            }
        }

        public IVirtualFolder Folder
        {
            get
            {
                FtpContext context = new FtpContext();
                context.InitializeContext(this.FtpOptions.Settings);
                if (this.rbCredentials.Checked)
                {
                    context.Credentials = new NetworkCredential(this.cmbUserName.Text, this.txtPassword.Text);
                }
                return (IVirtualFolder) FtpItem.FromUri(context, this.ServerUri, VirtualItemType.Folder, null);
            }
        }

        private Uri ServerUri
        {
            get
            {
                Uri uri = new Uri(this.cmbServer.Text.Trim(), UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri)
                {
                    if (uri.Scheme != Uri.UriSchemeFtp)
                    {
                        throw new WarningException(string.Format(Resources.sAnotherUriSchemeExpected, Uri.UriSchemeFtp, uri.Scheme));
                    }
                    return uri;
                }
                return new Uri(Uri.UriSchemeFtp + Uri.SchemeDelimiter + uri.OriginalString);
            }
        }
    }
}

