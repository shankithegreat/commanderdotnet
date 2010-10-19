namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class EditDescriptionDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private IContainer components = null;
        private VirtualItemToolStrip tsItem;
        private TextBox txtDescription;

        public EditDescriptionDialog()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute(IVirtualItem item)
        {
            this.tsItem.Add(item);
            return (base.ShowDialog() == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(EditDescriptionDialog));
            this.tsItem = new VirtualItemToolStrip(this.components);
            this.txtDescription = new TextBox();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.bvlButtons = new Bevel();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label control = new Label();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblDescription");
            label.Name = "lblDescription";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.tsItem, 0, 1);
            panel.Controls.Add(this.txtDescription, 0, 3);
            panel.Controls.Add(label, 0, 2);
            panel.Controls.Add(control, 0, 0);
            panel.Name = "tlpBack";
            this.tsItem.BackColor = SystemColors.ButtonFace;
            manager.ApplyResources(this.tsItem, "tsItem");
            this.tsItem.GripStyle = ToolStripGripStyle.Hidden;
            this.tsItem.MinimumSize = new Size(0, 0x19);
            this.tsItem.Name = "tsItem";
            manager.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.TextChanged += new EventHandler(this.txtDescription_TextChanged);
            manager.ApplyResources(control, "lblItem");
            control.Name = "lblItem";
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.Controls.Add(this.btnCancel, 2, 0);
            panel2.Controls.Add(this.btnOk, 1, 0);
            panel2.Name = "tlpButtons";
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
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel2);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "EditDescriptionDialog";
            base.ShowInTaskbar = false;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            this.btnOk.Enabled = !this.txtDescription.Text.Equals(this.txtDescription.Tag);
        }

        public string Description
        {
            get
            {
                return this.txtDescription.Text;
            }
            set
            {
                this.txtDescription.Tag = value;
                this.txtDescription.Text = value;
            }
        }
    }
}

