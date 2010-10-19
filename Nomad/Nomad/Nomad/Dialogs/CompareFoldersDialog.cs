namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Workers;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CompareFoldersDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkAsyncCompareContent;
        private CheckBox chkCompareAttributes;
        private CheckBox chkCompareContent;
        private CheckBox chkCompareDateTime;
        private CheckBox chkCompareSize;
        private IContainer components = null;
        private RadioButton rbSelect;
        private RadioButton rbUnselect;
        private TableLayoutPanel tlpButtons;

        public CompareFoldersDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
        }

        private void chkCompareContent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkCompareContent.Checked)
            {
                this.chkCompareSize.Checked = true;
            }
            this.chkAsyncCompareContent.Enabled = this.chkCompareContent.Checked;
        }

        private void chkCompareSize_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkCompareSize.Checked)
            {
                this.chkCompareContent.Checked = false;
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

        public bool Execute(IWin32Window owner)
        {
            return (base.ShowDialog(owner) == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CompareFoldersDialog));
            this.chkCompareContent = new CheckBox();
            this.chkCompareSize = new CheckBox();
            this.chkAsyncCompareContent = new CheckBox();
            this.chkCompareDateTime = new CheckBox();
            this.chkCompareAttributes = new CheckBox();
            this.rbUnselect = new RadioButton();
            this.rbSelect = new RadioButton();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.tlpButtons = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            GroupBox box = new GroupBox();
            GroupBox box2 = new GroupBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            box.SuspendLayout();
            box2.SuspendLayout();
            panel.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            box.Controls.Add(this.chkCompareContent);
            box.Controls.Add(this.chkCompareSize);
            box.Controls.Add(this.chkAsyncCompareContent);
            box.Controls.Add(this.chkCompareDateTime);
            box.Controls.Add(this.chkCompareAttributes);
            manager.ApplyResources(box, "grpOptions");
            box.Name = "grpOptions";
            box.TabStop = false;
            manager.ApplyResources(this.chkCompareContent, "chkCompareContent");
            this.chkCompareContent.Name = "chkCompareContent";
            this.chkCompareContent.UseVisualStyleBackColor = true;
            this.chkCompareContent.CheckedChanged += new EventHandler(this.chkCompareContent_CheckedChanged);
            manager.ApplyResources(this.chkCompareSize, "chkCompareSize");
            this.chkCompareSize.Checked = true;
            this.chkCompareSize.CheckState = CheckState.Checked;
            this.chkCompareSize.Name = "chkCompareSize";
            this.chkCompareSize.UseVisualStyleBackColor = true;
            this.chkCompareSize.CheckedChanged += new EventHandler(this.chkCompareSize_CheckedChanged);
            manager.ApplyResources(this.chkAsyncCompareContent, "chkAsyncCompareContent");
            this.chkAsyncCompareContent.Checked = true;
            this.chkAsyncCompareContent.CheckState = CheckState.Indeterminate;
            this.chkAsyncCompareContent.Name = "chkAsyncCompareContent";
            this.chkAsyncCompareContent.ThreeState = true;
            this.chkAsyncCompareContent.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCompareDateTime, "chkCompareDateTime");
            this.chkCompareDateTime.Checked = true;
            this.chkCompareDateTime.CheckState = CheckState.Checked;
            this.chkCompareDateTime.Name = "chkCompareDateTime";
            this.chkCompareDateTime.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCompareAttributes, "chkCompareAttributes");
            this.chkCompareAttributes.Name = "chkCompareAttributes";
            this.chkCompareAttributes.UseVisualStyleBackColor = true;
            box2.Controls.Add(this.rbUnselect);
            box2.Controls.Add(this.rbSelect);
            manager.ApplyResources(box2, "grpSelect");
            box2.Name = "grpSelect";
            box2.TabStop = false;
            manager.ApplyResources(this.rbUnselect, "rbUnselect");
            this.rbUnselect.Name = "rbUnselect";
            this.rbUnselect.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.rbSelect, "rbSelect");
            this.rbSelect.Checked = true;
            this.rbSelect.Name = "rbSelect";
            this.rbSelect.TabStop = true;
            this.rbSelect.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(box2, 0, 1);
            panel.Controls.Add(box, 0, 0);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CompareFoldersDialog";
            base.ShowInTaskbar = false;
            box.ResumeLayout(false);
            box.PerformLayout();
            box2.ResumeLayout(false);
            box2.PerformLayout();
            panel.ResumeLayout(false);
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public CompareFoldersOptions Options
        {
            get
            {
                CompareFoldersOptions options = 0;
                if (this.chkCompareAttributes.Checked)
                {
                    options |= CompareFoldersOptions.CompareAttributes;
                }
                if (this.chkCompareDateTime.Checked)
                {
                    options |= CompareFoldersOptions.CompareLastWriteTime;
                }
                if (this.chkCompareSize.Checked)
                {
                    options |= CompareFoldersOptions.CompareSize;
                }
                if (this.chkCompareContent.Checked)
                {
                    options |= CompareFoldersOptions.CompareContent;
                }
                switch (this.chkAsyncCompareContent.CheckState)
                {
                    case CheckState.Checked:
                        return (options | CompareFoldersOptions.CompareContentAsync);

                    case CheckState.Indeterminate:
                        return (options | CompareFoldersOptions.AutoCompareContentAsync);
                }
                return options;
            }
        }

        public bool SelectItems
        {
            get
            {
                return this.rbSelect.Checked;
            }
        }
    }
}

