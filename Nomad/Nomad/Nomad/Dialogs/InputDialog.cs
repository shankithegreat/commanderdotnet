namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class InputDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private IContainer components = null;
        private InputDialogOption FOptions;
        private string FOriginalValue;
        private Label lblCaption;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpButtons;
        private TextBox txtValue;

        public InputDialog()
        {
            this.InitializeComponent();
            this.btnOk.Text = MessageDialog.GetMessageButtonText(MessageDialogResult.OK);
            this.btnCancel.Text = MessageDialog.GetMessageButtonText(MessageDialogResult.Cancel);
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
            this.tlpButtons = new TableLayoutPanel();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.tlpBack = new TableLayoutPanel();
            this.lblCaption = new Label();
            this.txtValue = new TextBox();
            this.bvlButtons = new Bevel();
            this.tlpButtons.SuspendLayout();
            this.tlpBack.SuspendLayout();
            base.SuspendLayout();
            this.tlpButtons.AutoSize = true;
            this.tlpButtons.BackColor = Color.Gainsboro;
            this.tlpButtons.ColumnCount = 3;
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new ColumnStyle());
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Dock = DockStyle.Top;
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Location = new Point(0, 0x3a);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.Padding = new Padding(6, 4, 6, 4);
            this.tlpButtons.RowCount = 1;
            this.tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tlpButtons.Size = new Size(0x16a, 0x25);
            this.tlpButtons.TabIndex = 1;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatStyle = FlatStyle.System;
            this.btnCancel.Location = new Point(0x116, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.FlatStyle = FlatStyle.System;
            this.btnOk.Location = new Point(0xc5, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x4b, 0x17);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.tlpBack.AutoSize = true;
            this.tlpBack.ColumnCount = 1;
            this.tlpBack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tlpBack.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tlpBack.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tlpBack.Controls.Add(this.lblCaption, 0, 0);
            this.tlpBack.Controls.Add(this.txtValue, 0, 1);
            this.tlpBack.Dock = DockStyle.Top;
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Location = new Point(0, 0);
            this.tlpBack.Name = "tlpBack";
            this.tlpBack.Padding = new Padding(9);
            this.tlpBack.RowCount = 2;
            this.tlpBack.RowStyles.Add(new RowStyle());
            this.tlpBack.RowStyles.Add(new RowStyle());
            this.tlpBack.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tlpBack.Size = new Size(0x16a, 0x39);
            this.tlpBack.TabIndex = 0;
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new Point(10, 9);
            this.lblCaption.Margin = new Padding(1, 0, 3, 0);
            this.lblCaption.MaximumSize = new Size(0x15c, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new Size(0x35, 13);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "lblCaption";
            this.txtValue.Dock = DockStyle.Fill;
            this.txtValue.Location = new Point(12, 0x19);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new Size(0x152, 20);
            this.txtValue.TabIndex = 1;
            this.txtValue.TextChanged += new EventHandler(this.txtValue_TextChanged);
            this.bvlButtons.Dock = DockStyle.Top;
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Location = new Point(0, 0x39);
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Top;
            this.bvlButtons.Size = new Size(0x16a, 1);
            this.bvlButtons.Style = Border3DStyle.Flat;
            this.bvlButtons.TabIndex = 2;
            base.AcceptButton = this.btnOk;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x16a, 0x5f);
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpBack);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x170, 0);
            base.Name = "InputDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "InputDialog";
            base.Shown += new EventHandler(this.txtValue_TextChanged);
            this.tlpButtons.ResumeLayout(false);
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static bool Input(IWin32Window owner, string label, string caption, ref string value)
        {
            return Input(owner, label, caption, ref value, 0);
        }

        public static bool Input(IWin32Window owner, string label, string caption, ref string value, InputDialogOption options)
        {
            using (InputDialog dialog = new InputDialog())
            {
                Form form = owner as Form;
                if (form != null)
                {
                    form.AddOwnedForm(dialog);
                }
                dialog.lblCaption.Text = label;
                dialog.Text = caption;
                dialog.Value = value;
                dialog.SetOptions(options);
                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    value = dialog.Value;
                    return true;
                }
                return false;
            }
        }

        private void SetOptions(InputDialogOption options)
        {
            this.FOptions = options;
            this.txtValue.Enabled = (options & InputDialogOption.ReadOnly) == 0;
            this.lblCaption.Enabled = this.txtValue.Enabled;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            this.btnOk.Enabled = (this.txtValue.Enabled && (((this.FOptions & InputDialogOption.AllowEmptyValue) > 0) || !string.IsNullOrEmpty(this.Value))) && (((this.FOptions & InputDialogOption.AllowSameValue) > 0) || !string.Equals(this.FOriginalValue, this.Value));
        }

        private string Value
        {
            get
            {
                return this.txtValue.Text;
            }
            set
            {
                this.txtValue.Text = value;
                this.FOriginalValue = this.txtValue.Text;
            }
        }
    }
}

