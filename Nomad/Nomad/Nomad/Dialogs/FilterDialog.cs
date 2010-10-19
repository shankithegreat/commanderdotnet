namespace Nomad.Dialogs
{
    using Nomad.Commons.Controls;
    using Nomad.Controls.Filter;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FilterDialog : BasicDialog
    {
        private Button btnApply;
        private Button btnCancel;
        private Button btnClear;
        private Button btnDeleteTemplate;
        private Button btnOk;
        private Button btnSaveTemplate;
        private Bevel bvlButtons;
        private Bevel bvlTemplate;
        private CheckBox chkRememberFilter;
        private TemplateComboBox cmbFilterTemplate;
        private IContainer components = null;
        private ComplexFilterControl filterControlComplex;
        private ImageList imageList;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private IVirtualItemFilter StoredFilter;
        private TableLayoutPanel tlpButtons;
        private TableLayoutPanel tlpTemplate;
        private ToolStripMenuItem tsmiViewAdvanced;
        private ToolStripMenuItem tsmiViewBasic;
        private ToolStripMenuItem tsmiViewFull;
        private ToolStripSplitButton tssbChangeView;
        private ToolStrip tsView;

        public event EventHandler<ApplyFilterEventArgs> OnApplyFilter;

        public FilterDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            Binding binding = new Binding("View", Settings.Default, "FilterDialogView", true, DataSourceUpdateMode.OnPropertyChanged) {
                ControlUpdateMode = ControlUpdateMode.Never
            };
            this.filterControlComplex.DataBindings.Add(binding);
            this.tsmiViewBasic.Tag = ComplexFilterView.Basic;
            this.tsmiViewAdvanced.Tag = ComplexFilterView.Advanced;
            this.tsmiViewFull.Tag = ComplexFilterView.Full;
            this.tsView.DataBindings.Add(new Binding("BackColor", this.tlpButtons, "BackColor", false, DataSourceUpdateMode.Never));
            this.tsView.DataBindings.Add(new Binding("Font", this, "Font", false, DataSourceUpdateMode.Never));
            this.tsView.Renderer = BorderLessToolStripRenderer.Default;
            this.tsView.ImageList = this.imageList;
            this.tssbChangeView.Dock = DockStyle.Left;
            this.btnSaveTemplate.Image = IconSet.GetImage("SaveAs");
            this.imageList.Images.Add(Resources.ShowDetail);
            this.imageList.Images.Add(Resources.HideDetail);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            IVirtualItemFilter filter = this.Filter;
            if (this.OnApplyFilter != null)
            {
                this.OnApplyFilter(this, new ApplyFilterEventArgs(filter));
            }
            this.StoredFilter = filter;
            this.filterControlComplex.SaveComponentSettings();
            this.filterControlComplex.LoadComponentSettings();
            this.btnApply.Enabled = false;
            this.filterControlComplex.SelectFirst(false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.filterControlComplex.Clear();
            this.filterControlComplex.SelectFirst(true);
            this.FilterChanged(sender, e);
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            IVirtualItemFilter filter = this.Filter;
            if (filter != null)
            {
                this.cmbFilterTemplate.Save<NamedFilter>(new NamedFilter(string.Empty, filter));
            }
            this.UpdateAcceptButton();
        }

        private void btnSaveTemplate_EnabledChanged(object sender, EventArgs e)
        {
            this.btnSaveTemplate.Enabled &= this.Filter != null;
        }

        private void cmbFilterTemplate_Enter(object sender, EventArgs e)
        {
            this.UpdateAcceptButton();
        }

        private void cmbFilterTemplate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.cmbFilterTemplate.SelectedIndex >= 0)
            {
                this.filterControlComplex.Filter = this.cmbFilterTemplate.GetValue<FilterContainer>(this.cmbFilterTemplate.SelectedIndex).Filter;
                this.FilterChanged(sender, e);
            }
            this.UpdateAcceptButton();
        }

        private void cmbFilterTemplate_TextUpdate(object sender, EventArgs e)
        {
            this.UpdateAcceptButton();
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
            this.cmbFilterTemplate.SetItems<NamedFilter>(Settings.Default.Filters, delegate (NamedFilter x) {
                return x.Name;
            });
            this.StoredFilter = this.Filter;
            if (this.StoredFilter != null)
            {
                for (int i = 0; i < this.cmbFilterTemplate.Items.Count; i++)
                {
                    if (this.StoredFilter.Equals(this.cmbFilterTemplate.GetValue<FilterContainer>(i).Filter))
                    {
                        this.cmbFilterTemplate.SelectedIndex = i;
                        break;
                    }
                }
            }
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                this.filterControlComplex.SaveComponentSettings();
                return true;
            }
            return false;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            this.cmbFilterTemplate.UpdateButtons();
            this.btnSaveTemplate_EnabledChanged(this.btnSaveTemplate, EventArgs.Empty);
            this.btnApply.Enabled = (this.OnApplyFilter != null) && !FilterHelper.FilterEquals(this.Filter, this.StoredFilter);
        }

        private void filterControlComplex_ViewChanged(object sender, EventArgs e)
        {
            int num;
            string text;
            base.Height += this.filterControlComplex.PreferredSize.Height - this.filterControlComplex.Height;
            switch (this.filterControlComplex.View)
            {
                case ComplexFilterView.Basic:
                    num = 0;
                    text = this.tsmiViewBasic.Text;
                    break;

                case ComplexFilterView.Advanced:
                    num = 0;
                    text = this.tsmiViewAdvanced.Text;
                    break;

                case ComplexFilterView.Full:
                    num = 1;
                    text = this.tsmiViewFull.Text;
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
            if (this.tssbChangeView.ImageIndex != num)
            {
                this.tssbChangeView.ImageIndex = num;
            }
            if (this.tssbChangeView.Text != text)
            {
                this.tssbChangeView.Text = text;
            }
        }

        private void FilterDialog_Shown(object sender, EventArgs e)
        {
            this.filterControlComplex_ViewChanged(this.filterControlComplex, EventArgs.Empty);
            this.filterControlComplex.SelectFirst(false);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FilterDialog));
            this.filterControlComplex = new ComplexFilterControl();
            this.chkRememberFilter = new CheckBox();
            this.tlpButtons = new TableLayoutPanel();
            this.tsView = new ToolStrip();
            this.tssbChangeView = new ToolStripSplitButton();
            this.tsmiViewBasic = new ToolStripMenuItem();
            this.tsmiViewAdvanced = new ToolStripMenuItem();
            this.tsmiViewFull = new ToolStripMenuItem();
            this.btnOk = new Button();
            this.btnApply = new Button();
            this.btnClear = new Button();
            this.btnCancel = new Button();
            this.bvlTemplate = new Bevel();
            this.imageList = new ImageList(this.components);
            this.tlpTemplate = new TableLayoutPanel();
            this.btnSaveTemplate = new Button();
            this.btnDeleteTemplate = new Button();
            this.cmbFilterTemplate = new TemplateComboBox();
            this.bvlButtons = new Bevel();
            Label label = new Label();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            this.tsView.SuspendLayout();
            this.tlpTemplate.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblFilterTemplate");
            label.Name = "lblFilterTemplate";
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.filterControlComplex, 0, 0);
            panel.Controls.Add(this.chkRememberFilter, 0, 1);
            panel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel.Name = "tlpBack";
            this.filterControlComplex.AdvancedViewFilters = ViewFilters.Folder | ViewFilters.Advanced | ViewFilters.Attributes | ViewFilters.ExcludeMask | ViewFilters.IncludeMask;
            this.filterControlComplex.BasicViewFilters = ViewFilters.Folder | ViewFilters.Attributes | ViewFilters.IncludeMask;
            manager.ApplyResources(this.filterControlComplex, "filterControlComplex");
            this.filterControlComplex.HideViewFilters = ViewFilters.Content;
            this.filterControlComplex.MinimumSize = new Size(0x1c4, 0);
            this.filterControlComplex.Name = "filterControlComplex";
            this.filterControlComplex.View = Settings.Default.FilterDialogView;
            this.filterControlComplex.FilterChanged += new EventHandler(this.FilterChanged);
            this.filterControlComplex.ViewChanged += new EventHandler(this.filterControlComplex_ViewChanged);
            manager.ApplyResources(this.chkRememberFilter, "chkRememberFilter");
            this.chkRememberFilter.Name = "chkRememberFilter";
            this.chkRememberFilter.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.tsView, 0, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnApply, 4, 0);
            this.tlpButtons.Controls.Add(this.btnClear, 2, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 3, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.tsView, "tsView");
            this.tsView.GripStyle = ToolStripGripStyle.Hidden;
            this.tsView.Items.AddRange(new ToolStripItem[] { this.tssbChangeView });
            this.tsView.LayoutStyle = ToolStripLayoutStyle.Table;
            this.tsView.Name = "tsView";
            this.tsView.TabStop = true;
            this.tssbChangeView.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiViewBasic, this.tsmiViewAdvanced, this.tsmiViewFull });
            this.tssbChangeView.Image = Resources.HideDetail;
            manager.ApplyResources(this.tssbChangeView, "tssbChangeView");
            this.tssbChangeView.Name = "tssbChangeView";
            this.tssbChangeView.ButtonClick += new EventHandler(this.tssbChangeView_ButtonClick);
            this.tsmiViewBasic.Name = "tsmiViewBasic";
            manager.ApplyResources(this.tsmiViewBasic, "tsmiViewBasic");
            this.tsmiViewBasic.Paint += new PaintEventHandler(this.tsmiViewBasic_Paint);
            this.tsmiViewBasic.Click += new EventHandler(this.tsmiViewBasic_Click);
            this.tsmiViewAdvanced.Name = "tsmiViewAdvanced";
            manager.ApplyResources(this.tsmiViewAdvanced, "tsmiViewAdvanced");
            this.tsmiViewAdvanced.Paint += new PaintEventHandler(this.tsmiViewBasic_Paint);
            this.tsmiViewAdvanced.Click += new EventHandler(this.tsmiViewBasic_Click);
            this.tsmiViewFull.Name = "tsmiViewFull";
            manager.ApplyResources(this.tsmiViewFull, "tsmiViewFull");
            this.tsmiViewFull.Paint += new PaintEventHandler(this.tsmiViewBasic_Paint);
            this.tsmiViewFull.Click += new EventHandler(this.tsmiViewBasic_Click);
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new EventHandler(this.btnApply_Click);
            manager.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.bvlTemplate, "bvlTemplate");
            this.bvlTemplate.ForeColor = SystemColors.ControlDarkDark;
            this.bvlTemplate.Name = "bvlTemplate";
            this.bvlTemplate.Sides = Border3DSide.Top;
            this.bvlTemplate.Style = Border3DStyle.Flat;
            this.imageList.ColorDepth = ColorDepth.Depth8Bit;
            manager.ApplyResources(this.imageList, "imageList");
            this.imageList.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.tlpTemplate, "tlpTemplate");
            this.tlpTemplate.Controls.Add(this.btnSaveTemplate, 2, 0);
            this.tlpTemplate.Controls.Add(this.btnDeleteTemplate, 3, 0);
            this.tlpTemplate.Controls.Add(label, 0, 0);
            this.tlpTemplate.Controls.Add(this.cmbFilterTemplate, 1, 0);
            this.tlpTemplate.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpTemplate.Name = "tlpTemplate";
            manager.ApplyResources(this.btnSaveTemplate, "btnSaveTemplate");
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new EventHandler(this.btnSaveTemplate_Click);
            this.btnSaveTemplate.EnabledChanged += new EventHandler(this.btnSaveTemplate_EnabledChanged);
            manager.ApplyResources(this.btnDeleteTemplate, "btnDeleteTemplate");
            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
            this.btnDeleteTemplate.UseVisualStyleBackColor = true;
            this.cmbFilterTemplate.DeleteButton = this.btnDeleteTemplate;
            manager.ApplyResources(this.cmbFilterTemplate, "cmbFilterTemplate");
            this.cmbFilterTemplate.FormattingEnabled = true;
            this.cmbFilterTemplate.Name = "cmbFilterTemplate";
            this.cmbFilterTemplate.SaveButton = this.btnSaveTemplate;
            this.cmbFilterTemplate.SelectionChangeCommitted += new EventHandler(this.cmbFilterTemplate_SelectionChangeCommitted);
            this.cmbFilterTemplate.Leave += new EventHandler(this.cmbFilterTemplate_Enter);
            this.cmbFilterTemplate.Enter += new EventHandler(this.cmbFilterTemplate_Enter);
            this.cmbFilterTemplate.TextUpdate += new EventHandler(this.cmbFilterTemplate_TextUpdate);
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(panel);
            base.Controls.Add(this.bvlTemplate);
            base.Controls.Add(this.tlpTemplate);
            base.Controls.Add(this.tlpButtons);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FilterDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.FilterDialog_Shown);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            this.tsView.ResumeLayout(false);
            this.tsView.PerformLayout();
            this.tlpTemplate.ResumeLayout(false);
            this.tlpTemplate.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.ApplicationExitCall:
                case CloseReason.WindowsShutDown:
                    break;

                default:
                    if (this.cmbFilterTemplate.Modified)
                    {
                        List<NamedFilter> list = new List<NamedFilter>();
                        foreach (KeyValuePair<string, NamedFilter> pair in this.cmbFilterTemplate.GetItems<NamedFilter>())
                        {
                            pair.Value.Name = pair.Key;
                            list.Add(pair.Value);
                        }
                        Settings.Default.Filters = (list.Count > 0) ? list.ToArray() : null;
                    }
                    break;
            }
            base.OnFormClosed(e);
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            BasicDialog.UpdateBevel(this.bvlTemplate);
            BasicDialog.UpdateBevel(this.bvlButtons);
            base.OnThemeChanged(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                this.btnSaveTemplate.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tsmiViewBasic_Click(object sender, EventArgs e)
        {
            ComplexFilterView tag = (ComplexFilterView) ((ToolStripItem) sender).Tag;
            if (this.filterControlComplex.View != tag)
            {
                using (new LockWindowRedraw(this.filterControlComplex, true))
                {
                    this.filterControlComplex.View = tag;
                }
                this.FilterChanged(sender, e);
            }
        }

        private void tsmiViewBasic_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            item.Checked = this.filterControlComplex.View == ((ComplexFilterView) item.Tag);
        }

        private void tssbChangeView_ButtonClick(object sender, EventArgs e)
        {
            ComplexFilterView advanced;
            switch (this.filterControlComplex.View)
            {
                case ComplexFilterView.Basic:
                    advanced = ComplexFilterView.Advanced;
                    break;

                case ComplexFilterView.Advanced:
                    advanced = ComplexFilterView.Full;
                    break;

                default:
                    advanced = ComplexFilterView.Basic;
                    break;
            }
            using (new LockWindowRedraw(this.filterControlComplex, true))
            {
                this.filterControlComplex.View = advanced;
            }
            this.FilterChanged(sender, e);
        }

        protected void UpdateAcceptButton()
        {
            IButtonControl btnOk = this.btnOk;
            if ((!this.cmbFilterTemplate.DroppedDown && this.btnSaveTemplate.Enabled) && this.cmbFilterTemplate.Focused)
            {
                btnOk = this.btnSaveTemplate;
            }
            if (base.AcceptButton != btnOk)
            {
                base.AcceptButton = btnOk;
            }
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                return this.filterControlComplex.Filter;
            }
            set
            {
                this.filterControlComplex.Filter = value;
            }
        }

        public bool RememberFilter
        {
            get
            {
                return (this.chkRememberFilter.Enabled & this.chkRememberFilter.Checked);
            }
        }

        public bool RememberFilterEnabled
        {
            get
            {
                return this.chkRememberFilter.Visible;
            }
            set
            {
                this.chkRememberFilter.Enabled = value;
                this.chkRememberFilter.Visible = value;
            }
        }
    }
}

