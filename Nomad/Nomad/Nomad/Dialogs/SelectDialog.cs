namespace Nomad.Dialogs
{
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class SelectDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkExceptMask;
        private CheckBox chkSelectFolders;
        private ComboBox cmbMask;
        private IContainer components = null;
        private ListBox lstFilters;
        private RadioButton rbFilter;
        private RadioButton rbMask;

        public SelectDialog()
        {
            this.InitializeComponent();
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {
            this.rbFilter.Enabled = this.lstFilters.Items.Count > 0;
            this.cmbMask.Enabled = this.rbMask.Checked;
            this.chkSelectFolders.Enabled = this.rbMask.Checked;
            this.chkExceptMask.Enabled = this.rbMask.Checked;
            this.lstFilters.Enabled = this.rbFilter.Checked;
            if (!this.lstFilters.Enabled)
            {
                this.lstFilters.SelectedItem = null;
            }
            this.btnOk.Enabled = !string.IsNullOrEmpty(this.cmbMask.Text);
        }

        private void cmbMask_TextUpdate(object sender, EventArgs e)
        {
            this.ApplicationIdle(sender, e);
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
            this.lstFilters.BeginUpdate();
            try
            {
                this.lstFilters.Items.Clear();
                NamedFilter[] filters = Settings.Default.Filters;
                if ((filters != null) && (filters.Length > 0))
                {
                    this.lstFilters.Items.AddRange(filters);
                }
            }
            finally
            {
                this.lstFilters.EndUpdate();
            }
            HistorySettings.PopulateComboBox(this.cmbMask, HistorySettings.Default.SelectFileMasks);
            bool flag = base.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                HistorySettings.Default.AddStringToSelectFileMasks(this.cmbMask.Text);
            }
            return flag;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SelectDialog));
            this.rbMask = new RadioButton();
            this.cmbMask = new ComboBox();
            this.chkExceptMask = new CheckBox();
            this.rbFilter = new RadioButton();
            this.lstFilters = new ListBox();
            this.chkSelectFolders = new CheckBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.bvlButtons = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.rbMask, 0, 0);
            panel.Controls.Add(this.cmbMask, 1, 1);
            panel.Controls.Add(this.chkExceptMask, 1, 3);
            panel.Controls.Add(this.rbFilter, 0, 4);
            panel.Controls.Add(this.lstFilters, 1, 5);
            panel.Controls.Add(this.chkSelectFolders, 1, 2);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.rbMask, "rbMask");
            this.rbMask.Checked = true;
            panel.SetColumnSpan(this.rbMask, 2);
            this.rbMask.Name = "rbMask";
            this.rbMask.TabStop = true;
            this.rbMask.UseVisualStyleBackColor = true;
            this.rbMask.Click += new EventHandler(this.rbMask_Click);
            manager.ApplyResources(this.cmbMask, "cmbMask");
            this.cmbMask.Name = "cmbMask";
            this.cmbMask.TextUpdate += new EventHandler(this.cmbMask_TextUpdate);
            manager.ApplyResources(this.chkExceptMask, "chkExceptMask");
            this.chkExceptMask.Name = "chkExceptMask";
            this.chkExceptMask.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.rbFilter, "rbFilter");
            panel.SetColumnSpan(this.rbFilter, 2);
            this.rbFilter.Name = "rbFilter";
            this.rbFilter.UseVisualStyleBackColor = true;
            this.rbFilter.Click += new EventHandler(this.rbFilter_Click);
            manager.ApplyResources(this.lstFilters, "lstFilters");
            this.lstFilters.FormattingEnabled = true;
            this.lstFilters.Name = "lstFilters";
            this.lstFilters.Enter += new EventHandler(this.lstFilters_Enter);
            manager.ApplyResources(this.chkSelectFolders, "chkSelectFolders");
            this.chkSelectFolders.Name = "chkSelectFolders";
            this.chkSelectFolders.UseVisualStyleBackColor = true;
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.Controls.Add(this.btnOk, 1, 0);
            panel2.Controls.Add(this.btnCancel, 2, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpButtons";
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.CausesValidation = false;
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
            base.Controls.Add(panel2);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SelectDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.SelectDialog_Shown);
            base.FormClosed += new FormClosedEventHandler(this.SelectDialog_FormClosed);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lstFilters_Enter(object sender, EventArgs e)
        {
            if ((this.lstFilters.SelectedItem == null) && (this.lstFilters.Items.Count > 0))
            {
                this.lstFilters.SelectedIndex = 0;
            }
        }

        private void rbFilter_Click(object sender, EventArgs e)
        {
            this.ApplicationIdle(sender, e);
            this.lstFilters.Focus();
        }

        private void rbMask_Click(object sender, EventArgs e)
        {
            this.ApplicationIdle(sender, e);
            this.cmbMask.Focus();
        }

        private void SelectDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Idle -= new EventHandler(this.ApplicationIdle);
        }

        private void SelectDialog_Shown(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(this.ApplicationIdle);
            this.ApplicationIdle(sender, e);
            if (this.lstFilters.Enabled)
            {
                this.lstFilters.Select();
            }
            else
            {
                this.cmbMask.Select();
            }
        }

        [Browsable(false)]
        public IVirtualItemFilter Filter
        {
            get
            {
                if (this.rbMask.Checked)
                {
                    IVirtualItemFilter filter = new VirtualItemNameFilter(this.chkExceptMask.Checked ? NamePatternCondition.NotEqual : NamePatternCondition.Equal, this.cmbMask.Text);
                    if (!this.SelectFolders)
                    {
                        return new AggregatedVirtualItemFilter(new VirtualItemAttributeFilter(0, FileAttributes.Directory), filter);
                    }
                    return filter;
                }
                return ((NamedFilter) this.lstFilters.SelectedItem).Filter;
            }
        }

        public bool SelectFolders
        {
            get
            {
                return this.chkSelectFolders.Checked;
            }
            set
            {
                this.chkSelectFolders.Checked = value;
            }
        }

        public bool SelectFoldersVisible
        {
            get
            {
                return this.chkSelectFolders.Visible;
            }
            set
            {
                this.chkSelectFolders.Visible = value;
            }
        }
    }
}

