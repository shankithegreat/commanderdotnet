namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class BookmarkTabDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkDoNotShowAgain;
        private IContainer components = null;
        private Label lblBookmarkName;
        private Label lblHotkey;
        private TextBox txtBookmarkName;
        private HotKeyBox txtHotkey;
        private ValidatorProvider Validator;

        public event EventHandler<PreviewHotKeyEventArgs> PreviewHotKey
        {
            add
            {
                this.txtHotkey.PreviewHotKey += value;
            }
            remove
            {
                this.txtHotkey.PreviewHotKey -= value;
            }
        }

        public BookmarkTabDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.txtHotkey.KeysConverter = new SettingsManager.LocalizedEnumConverter(typeof(Keys));
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
            return (base.ShowDialog() == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BookmarkTabDialog));
            this.txtHotkey = new HotKeyBox();
            this.lblHotkey = new Label();
            this.lblBookmarkName = new Label();
            this.txtBookmarkName = new TextBox();
            this.chkDoNotShowAgain = new CheckBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.Validator = new ValidatorProvider();
            this.bvlButtons = new Bevel();
            GroupBox box = new GroupBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            TableLayoutPanel panel3 = new TableLayoutPanel();
            box.SuspendLayout();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(box, "grpAdditional");
            box.Controls.Add(panel);
            box.Name = "grpAdditional";
            box.TabStop = false;
            manager.ApplyResources(panel, "tlpAdditional");
            panel.Controls.Add(this.txtHotkey, 1, 0);
            panel.Controls.Add(this.lblHotkey, 0, 0);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpAdditional";
            manager.ApplyResources(this.txtHotkey, "txtHotkey");
            this.txtHotkey.Name = "txtHotkey";
            manager.ApplyResources(this.lblHotkey, "lblHotkey");
            this.lblHotkey.Name = "lblHotkey";
            manager.ApplyResources(panel2, "tlpBack");
            panel2.Controls.Add(this.lblBookmarkName, 0, 0);
            panel2.Controls.Add(this.txtBookmarkName, 0, 1);
            panel2.Controls.Add(box, 0, 2);
            panel2.Controls.Add(this.chkDoNotShowAgain, 0, 3);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpBack";
            manager.ApplyResources(this.lblBookmarkName, "lblBookmarkName");
            this.lblBookmarkName.Name = "lblBookmarkName";
            manager.ApplyResources(this.txtBookmarkName, "txtBookmarkName");
            this.txtBookmarkName.Name = "txtBookmarkName";
            this.Validator.SetValidateOn(this.txtBookmarkName, ValidateOn.TextChanged);
            this.txtBookmarkName.Validating += new CancelEventHandler(this.txtBookmarkName_Validating);
            manager.ApplyResources(this.chkDoNotShowAgain, "chkDoNotShowAgain");
            this.chkDoNotShowAgain.Name = "chkDoNotShowAgain";
            this.chkDoNotShowAgain.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel3, "tlpButtons");
            panel3.BackColor = Color.Gainsboro;
            panel3.Controls.Add(this.btnOk, 1, 0);
            panel3.Controls.Add(this.btnCancel, 2, 0);
            panel3.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel3.Name = "tlpButtons";
            this.btnOk.DialogResult = DialogResult.OK;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
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
            base.Controls.Add(panel3);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "BookmarkTabDialog";
            base.ShowInTaskbar = false;
            box.ResumeLayout(false);
            box.PerformLayout();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void txtBookmarkName_Validating(object sender, CancelEventArgs e)
        {
            string bookmarkName = this.BookmarkName;
            if (string.IsNullOrEmpty(bookmarkName))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError((Control) sender);
            }
            else
            {
                e.Cancel = bookmarkName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInBookmarkName);
                }
            }
        }

        public string BookmarkName
        {
            get
            {
                return this.txtBookmarkName.Text.Trim();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.txtBookmarkName.Text = string.Empty;
                }
                else
                {
                    char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
                    StringBuilder builder = new StringBuilder(value);
                    for (int i = builder.Length - 1; i >= 0; i--)
                    {
                        if (Array.IndexOf<char>(invalidFileNameChars, builder[i]) >= 0)
                        {
                            builder.Remove(i, 1);
                        }
                    }
                    this.txtBookmarkName.Text = builder.ToString();
                }
            }
        }

        public bool DoNotShowAgain
        {
            get
            {
                return this.chkDoNotShowAgain.Checked;
            }
            set
            {
                this.chkDoNotShowAgain.Checked = value;
            }
        }

        public Keys Hotkey
        {
            get
            {
                return this.txtHotkey.HotKey;
            }
        }
    }
}

