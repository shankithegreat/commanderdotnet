namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ExceptionDialog : BasicForm
    {
        private Button btnContinue;
        private Button btnCopyToClipboard;
        private Button btnDetails;
        private Button btnQuit;
        private IContainer components = null;
        private PictureBox imgIcon;
        private Label lblErrorMessage;
        private Label lblErrorNote;
        private LinkLabel lblSendError;
        private TextBox txtErrorDetails;

        public ExceptionDialog()
        {
            this.InitializeComponent();
            try
            {
                this.lblSendError.ParseLinks();
            }
            catch
            {
                this.lblSendError.Visible = false;
            }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            this.ErrorDetailsNeeded();
            try
            {
                Clipboard.SetText(this.txtErrorDetails.Text);
            }
            catch (ExternalException)
            {
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            this.ErrorDetailsNeeded();
            this.txtErrorDetails.Visible = !this.txtErrorDetails.Visible;
            this.btnDetails.Image = this.txtErrorDetails.Visible ? Resources.SmallUpArrow : Resources.SmallDownArrow;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ErrorDetailsNeeded()
        {
            if (this.txtErrorDetails.Tag == null)
            {
                this.txtErrorDetails.Tag = this.lblErrorMessage.Tag;
                Exception tag = (Exception) this.txtErrorDetails.Tag;
                this.txtErrorDetails.Text = ErrorReport.CreateErrorReport(tag);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ExceptionDialog));
            this.imgIcon = new PictureBox();
            this.btnDetails = new Button();
            this.txtErrorDetails = new TextBox();
            this.lblErrorMessage = new Label();
            this.lblErrorNote = new Label();
            this.lblSendError = new LinkLabel();
            this.btnContinue = new Button();
            this.btnCopyToClipboard = new Button();
            this.btnQuit = new Button();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            ((ISupportInitialize) this.imgIcon).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.imgIcon, 0, 0);
            panel.Controls.Add(this.btnDetails, 0, 3);
            panel.Controls.Add(this.txtErrorDetails, 0, 4);
            panel.Controls.Add(this.lblErrorMessage, 1, 1);
            panel.Controls.Add(this.lblErrorNote, 1, 0);
            panel.Controls.Add(this.lblSendError, 1, 2);
            panel.Controls.Add(this.btnContinue, 5, 3);
            panel.Controls.Add(this.btnCopyToClipboard, 4, 3);
            panel.Controls.Add(this.btnQuit, 3, 3);
            panel.MaximumSize = new Size(0x1a8, 0);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.imgIcon, "imgIcon");
            this.imgIcon.Name = "imgIcon";
            panel.SetRowSpan(this.imgIcon, 2);
            this.imgIcon.TabStop = false;
            manager.ApplyResources(this.btnDetails, "btnDetails");
            panel.SetColumnSpan(this.btnDetails, 2);
            this.btnDetails.Image = Resources.SmallDownArrow;
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
            panel.SetColumnSpan(this.txtErrorDetails, 6);
            manager.ApplyResources(this.txtErrorDetails, "txtErrorDetails");
            this.txtErrorDetails.Name = "txtErrorDetails";
            this.txtErrorDetails.ReadOnly = true;
            manager.ApplyResources(this.lblErrorMessage, "lblErrorMessage");
            panel.SetColumnSpan(this.lblErrorMessage, 5);
            this.lblErrorMessage.MaximumSize = new Size(0x16c, 0);
            this.lblErrorMessage.Name = "lblErrorMessage";
            manager.ApplyResources(this.lblErrorNote, "lblErrorNote");
            panel.SetColumnSpan(this.lblErrorNote, 5);
            this.lblErrorNote.MaximumSize = new Size(0x16c, 0);
            this.lblErrorNote.Name = "lblErrorNote";
            manager.ApplyResources(this.lblSendError, "lblSendError");
            panel.SetColumnSpan(this.lblSendError, 5);
            this.lblSendError.MaximumSize = new Size(0x16c, 0);
            this.lblSendError.Name = "lblSendError";
            this.lblSendError.Tag = "mailto:support@nomad-net.info?subject=Exception: {0}";
            this.lblSendError.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblSendError_LinkClicked);
            manager.ApplyResources(this.btnContinue, "btnContinue");
            this.btnContinue.DialogResult = DialogResult.Ignore;
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCopyToClipboard, "btnCopyToClipboard");
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new EventHandler(this.btnCopyToClipboard_Click);
            manager.ApplyResources(this.btnQuit, "btnQuit");
            this.btnQuit.DialogResult = DialogResult.Abort;
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnContinue;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnContinue;
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ExceptionDialog";
            base.ShowInTaskbar = false;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((ISupportInitialize) this.imgIcon).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void InitializeInfoTips(Control container)
        {
        }

        private void lblSendError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(string.Format((string) ((Control) sender).Tag, this.lblErrorMessage.Text));
            }
            catch
            {
            }
        }

        public DialogResult ShowError(string errorMessage, string errorText, string note)
        {
            this.lblErrorNote.Text = note;
            this.lblErrorMessage.Text = errorMessage;
            this.txtErrorDetails.Text = errorText;
            this.txtErrorDetails.Tag = true;
            this.btnQuit.Visible = false;
            this.btnContinue.Text = Resources.sMessageButtonClose;
            this.imgIcon.Image = SystemIcons.Warning.ToBitmap();
            return base.ShowDialog();
        }

        public DialogResult ShowException(Exception e)
        {
            this.lblErrorMessage.Tag = e;
            this.lblErrorMessage.Text = e.Message;
            this.imgIcon.Image = SystemIcons.Error.ToBitmap();
            return base.ShowDialog();
        }
    }
}

