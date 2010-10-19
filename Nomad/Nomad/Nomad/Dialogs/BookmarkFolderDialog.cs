namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class BookmarkFolderDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnCustomize;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkDoNotShowAgain;
        private IContainer components = null;
        private ICustomizeFolder FCustomize;
        private IVirtualFolder FFolder;
        private Label lblBookmarkName;
        private Label lblCustomize;
        private Label lblHotkey;
        private Label lblItem;
        private VirtualItemToolStrip tsItem;
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

        public BookmarkFolderDialog()
        {
            this.InitializeComponent();
            this.Validator.TooltipTitle = Resources.sInvalidName;
            this.txtHotkey.KeysConverter = new SettingsManager.LocalizedEnumConverter(typeof(Keys));
        }

        private void btnCustomize_Click(object sender, EventArgs e)
        {
            using (CustomizeFolderDialog dialog = new CustomizeFolderDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.ApplyToChildrenEnabled = false;
                this.FCustomize = new SimpleCustomizeFolder();
                int[] properties = new int[1];
                if (dialog.Execute(this.FFolder, this.FCustomize, null, new VirtualPropertySet(properties)))
                {
                    dialog.UpdateCustomizeFolder(this.FCustomize);
                }
                else
                {
                    this.FCustomize = null;
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

        public bool Execute(IVirtualFolder folder)
        {
            this.FFolder = folder;
            this.tsItem.Add(this.FFolder);
            return (base.ShowDialog() == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BookmarkFolderDialog));
            this.lblBookmarkName = new Label();
            this.txtBookmarkName = new TextBox();
            this.chkDoNotShowAgain = new CheckBox();
            this.lblHotkey = new Label();
            this.btnCustomize = new Button();
            this.txtHotkey = new HotKeyBox();
            this.lblCustomize = new Label();
            this.lblItem = new Label();
            this.tsItem = new VirtualItemToolStrip(this.components);
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.Validator = new ValidatorProvider();
            this.bvlButtons = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            GroupBox control = new GroupBox();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            TableLayoutPanel panel3 = new TableLayoutPanel();
            panel.SuspendLayout();
            control.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblBookmarkName, 0, 2);
            panel.Controls.Add(this.txtBookmarkName, 0, 3);
            panel.Controls.Add(this.chkDoNotShowAgain, 0, 5);
            panel.Controls.Add(control, 0, 4);
            panel.Controls.Add(this.lblItem, 0, 0);
            panel.Controls.Add(this.tsItem, 0, 1);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblBookmarkName, "lblBookmarkName");
            this.lblBookmarkName.Name = "lblBookmarkName";
            manager.ApplyResources(this.txtBookmarkName, "txtBookmarkName");
            this.txtBookmarkName.Name = "txtBookmarkName";
            this.Validator.SetValidateOn(this.txtBookmarkName, ValidateOn.TextChanged);
            this.txtBookmarkName.Validating += new CancelEventHandler(this.txtBookmarkName_Validating);
            manager.ApplyResources(this.chkDoNotShowAgain, "chkDoNotShowAgain");
            this.chkDoNotShowAgain.Name = "chkDoNotShowAgain";
            this.chkDoNotShowAgain.UseVisualStyleBackColor = true;
            manager.ApplyResources(control, "grpAdditional");
            control.Controls.Add(panel2);
            control.Name = "grpAdditional";
            control.TabStop = false;
            manager.ApplyResources(panel2, "tlpAdditional");
            panel2.Controls.Add(this.lblHotkey, 0, 0);
            panel2.Controls.Add(this.btnCustomize, 1, 1);
            panel2.Controls.Add(this.txtHotkey, 1, 0);
            panel2.Controls.Add(this.lblCustomize, 0, 1);
            panel2.Name = "tlpAdditional";
            manager.ApplyResources(this.lblHotkey, "lblHotkey");
            this.lblHotkey.Name = "lblHotkey";
            manager.ApplyResources(this.btnCustomize, "btnCustomize");
            this.btnCustomize.Name = "btnCustomize";
            this.btnCustomize.UseVisualStyleBackColor = true;
            this.btnCustomize.Click += new EventHandler(this.btnCustomize_Click);
            manager.ApplyResources(this.txtHotkey, "txtHotkey");
            this.txtHotkey.Name = "txtHotkey";
            manager.ApplyResources(this.lblCustomize, "lblCustomize");
            this.lblCustomize.Name = "lblCustomize";
            manager.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            this.tsItem.BackColor = SystemColors.ButtonFace;
            manager.ApplyResources(this.tsItem, "tsItem");
            this.tsItem.GripStyle = ToolStripGripStyle.Hidden;
            this.tsItem.Name = "tsItem";
            manager.ApplyResources(panel3, "tlpButtons");
            panel3.BackColor = Color.Gainsboro;
            panel3.Controls.Add(this.btnOk, 1, 0);
            panel3.Controls.Add(this.btnCancel, 2, 0);
            panel3.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
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
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "BookmarkFolderDialog";
            base.ShowInTaskbar = false;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            control.ResumeLayout(false);
            control.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
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

        public ICustomizeFolder CustomizeBookmark
        {
            get
            {
                return this.FCustomize;
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

