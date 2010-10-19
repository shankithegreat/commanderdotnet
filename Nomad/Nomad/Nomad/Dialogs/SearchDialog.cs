namespace Nomad.Dialogs
{
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Filter;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class SearchDialog : BasicDialog
    {
        private Button btnCancel;
        private Button btnClear;
        private Button btnDeleteTemplate;
        private Button btnDrives;
        private Button btnOk;
        private Button btnSaveTemplate;
        private Bevel bvlButtons;
        private Bevel bvlTemplate;
        private CheckBox chkProcessArchives;
        private CheckBox chkProcessSubfolders;
        private CheckBox chkSkipReparsePoints;
        private ComboBox cmbLookFolder;
        private TemplateComboBox cmbSearchTemplate;
        private IContainer components = null;
        private IVirtualFolder FFolder;
        private ComplexFilterControl filterControlComplex;
        private DuplicateSearchControl FindDuplicatesControl;
        private ImageList imgView;
        private Label lblSearchTemplate;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpButtons;
        private TableLayoutPanel tlpTemplate;
        private ToolTip toolTipLook;
        private ToolStripMenuItem tsmiViewAdvanced;
        private ToolStripMenuItem tsmiViewBasic;
        private ToolStripMenuItem tsmiViewFull;
        private ToolStripSplitButton tssbChangeView;
        private ToolStrip tsView;
        private ValidatorProvider Validator;

        public SearchDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            this.Validator.TooltipTitle = Resources.sError;
            Binding binding = new Binding("View", Settings.Default, "SearchDialogView", true, DataSourceUpdateMode.OnPropertyChanged) {
                ControlUpdateMode = ControlUpdateMode.Never
            };
            this.filterControlComplex.DataBindings.Add(binding);
            this.tsmiViewBasic.Tag = ComplexFilterView.Basic;
            this.tsmiViewAdvanced.Tag = ComplexFilterView.Advanced;
            this.tsmiViewFull.Tag = ComplexFilterView.Full;
            this.tsView.DataBindings.Add(new Binding("BackColor", this.tlpButtons, "BackColor", false, DataSourceUpdateMode.Never));
            this.tsView.DataBindings.Add(new Binding("Font", this, "Font", false, DataSourceUpdateMode.Never));
            this.tsView.Renderer = BorderLessToolStripRenderer.Default;
            this.tsView.ImageList = this.imgView;
            this.tssbChangeView.Dock = DockStyle.Left;
            this.btnSaveTemplate.Image = IconSet.GetImage("SaveAs");
            this.imgView.Images.Add(Resources.ShowDetail);
            this.imgView.Images.Add(Resources.HideDetail);
            this.FindDuplicatesControl = new DuplicateSearchControl();
            this.filterControlComplex.AddNewTab(Resources.sDuplicates, this.FindDuplicatesControl);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.FindDuplicatesControl.DuplicateOptions = 0;
            this.filterControlComplex.Clear();
            this.filterControlComplex.SelectFirst(true);
        }

        private void btnDrives_Click(object sender, EventArgs e)
        {
            IVirtualFolder[] folders = null;
            try
            {
                folders = this.Folders;
            }
            catch (Exception exception)
            {
                this.Validator.SetIsValid(this.cmbLookFolder, false);
                this.Validator.SetValidateError(this.cmbLookFolder, exception.Message);
                this.cmbLookFolder.Select();
                return;
            }
            using (SelectSearchFoldersDialog dialog = new SelectSearchFoldersDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.ShowItemIcons = Settings.Default.IsShowIcons;
                dialog.ProcessSubfolders = this.chkProcessSubfolders.Checked;
                dialog.Folders = folders;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (IVirtualFolder folder in dialog.Folders)
                    {
                        builder.Append(folder.FullName);
                        builder.Append(';');
                    }
                    if (builder.Length > 0)
                    {
                        builder.Length--;
                    }
                    this.FFolder = null;
                    this.cmbLookFolder.Text = builder.ToString();
                    this.chkProcessSubfolders.Checked = dialog.ProcessSubfolders;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.Validator.Validate(true))
            {
                base.DialogResult = DialogResult.OK;
            }
            else if (string.IsNullOrEmpty(this.cmbLookFolder.Text.Trim()))
            {
                this.cmbLookFolder.Select();
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            IVirtualItemFilter filter = this.Filter;
            if (filter != null)
            {
                this.cmbSearchTemplate.Save<NamedFilter>(new NamedFilter(string.Empty, filter));
            }
            this.UpdateAcceptButton();
        }

        private void btnSaveTemplate_EnabledChanged(object sender, EventArgs e)
        {
            this.btnSaveTemplate.Enabled &= this.Filter != null;
        }

        private void cmbLookFolder_MouseEnter(object sender, EventArgs e)
        {
            string str = this.cmbLookFolder.Text.Replace(";", Environment.NewLine);
            if (!string.IsNullOrEmpty(str))
            {
                this.toolTipLook.SetToolTip(this.cmbLookFolder, str);
            }
            else
            {
                this.toolTipLook.RemoveAll();
            }
        }

        private void cmbLookFolder_TextUpdate(object sender, EventArgs e)
        {
            this.FFolder = null;
        }

        private void cmbLookFolder_Validating(object sender, CancelEventArgs e)
        {
            ComboBox control = (ComboBox) sender;
            string str = control.Text.Trim();
            if (string.IsNullOrEmpty(str))
            {
                e.Cancel = true;
                this.Validator.RemoveValidateError(control);
            }
            else
            {
                foreach (string str2 in StringHelper.SplitString(str, new char[] { ';' }))
                {
                    e.Cancel = PathHelper.GetPathType(str2) == ~PathType.Unknown;
                    if (e.Cancel)
                    {
                        break;
                    }
                }
                if (e.Cancel)
                {
                    this.Validator.SetValidateError((Control) sender, Resources.sInvalidCharsInFileName);
                }
                else
                {
                    try
                    {
                        IVirtualFolder folder = this.Folder;
                    }
                    catch (Exception exception)
                    {
                        e.Cancel = true;
                        this.Validator.SetValidateError((Control) sender, exception.Message);
                    }
                }
            }
        }

        private void cmbSearchTemplate_Enter(object sender, EventArgs e)
        {
            this.UpdateAcceptButton();
        }

        private void cmbSearchTemplate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.cmbSearchTemplate.SelectedIndex >= 0)
            {
                this.filterControlComplex.Filter = this.cmbSearchTemplate.GetValue<FilterContainer>(this.cmbSearchTemplate.SelectedIndex).Filter;
                this.FilterChanged(sender, e);
            }
            this.UpdateAcceptButton();
        }

        private void cmbSearchTemplate_TextUpdate(object sender, EventArgs e)
        {
            this.FFolder = null;
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
            HistorySettings.PopulateComboBox(this.cmbLookFolder, HistorySettings.Default.LookFolder);
            this.cmbSearchTemplate.SetItems<NamedFilter>(Settings.Default.Searches, delegate (NamedFilter x) {
                return x.Name;
            });
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                HistorySettings.Default.AddStringToLookFolder(this.cmbLookFolder.Text);
                this.filterControlComplex.SaveComponentSettings();
                return true;
            }
            return false;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            this.cmbSearchTemplate.UpdateButtons();
            this.btnSaveTemplate_EnabledChanged(this.btnSaveTemplate, EventArgs.Empty);
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SearchDialog));
            this.tlpBack = new TableLayoutPanel();
            this.cmbLookFolder = new ComboBox();
            this.filterControlComplex = new ComplexFilterControl();
            this.btnDrives = new Button();
            this.chkProcessSubfolders = new CheckBox();
            this.chkSkipReparsePoints = new CheckBox();
            this.chkProcessArchives = new CheckBox();
            this.tlpButtons = new TableLayoutPanel();
            this.tsView = new ToolStrip();
            this.tssbChangeView = new ToolStripSplitButton();
            this.tsmiViewBasic = new ToolStripMenuItem();
            this.tsmiViewAdvanced = new ToolStripMenuItem();
            this.tsmiViewFull = new ToolStripMenuItem();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.btnClear = new Button();
            this.imgView = new ImageList(this.components);
            this.toolTipLook = new ToolTip(this.components);
            this.tlpTemplate = new TableLayoutPanel();
            this.lblSearchTemplate = new Label();
            this.cmbSearchTemplate = new TemplateComboBox();
            this.btnDeleteTemplate = new Button();
            this.btnSaveTemplate = new Button();
            this.bvlButtons = new Bevel();
            this.bvlTemplate = new Bevel();
            this.Validator = new ValidatorProvider();
            Label label = new Label();
            this.tlpBack.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            this.tsView.SuspendLayout();
            this.tlpTemplate.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblLook");
            label.Name = "lblLook";
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(label, 0, 0);
            this.tlpBack.Controls.Add(this.cmbLookFolder, 1, 0);
            this.tlpBack.Controls.Add(this.filterControlComplex, 0, 2);
            this.tlpBack.Controls.Add(this.btnDrives, 5, 0);
            this.tlpBack.Controls.Add(this.chkProcessSubfolders, 0, 1);
            this.tlpBack.Controls.Add(this.chkSkipReparsePoints, 3, 1);
            this.tlpBack.Controls.Add(this.chkProcessArchives, 2, 1);
            this.tlpBack.Name = "tlpBack";
            this.tlpBack.SetColumnSpan(this.cmbLookFolder, 4);
            manager.ApplyResources(this.cmbLookFolder, "cmbLookFolder");
            this.cmbLookFolder.Name = "cmbLookFolder";
            this.Validator.SetValidateOn(this.cmbLookFolder, ValidateOn.TextChangedTimer);
            this.cmbLookFolder.SelectionChangeCommitted += new EventHandler(this.cmbLookFolder_TextUpdate);
            this.cmbLookFolder.TextUpdate += new EventHandler(this.cmbLookFolder_TextUpdate);
            this.cmbLookFolder.Validating += new CancelEventHandler(this.cmbLookFolder_Validating);
            this.tlpBack.SetColumnSpan(this.filterControlComplex, 6);
            manager.ApplyResources(this.filterControlComplex, "filterControlComplex");
            this.filterControlComplex.HideViewFilters = ViewFilters.Folder;
            this.filterControlComplex.MinimumSize = new Size(0x1c4, 0);
            this.filterControlComplex.Name = "filterControlComplex";
            this.filterControlComplex.View = Settings.Default.SearchDialogView;
            this.filterControlComplex.FilterChanged += new EventHandler(this.FilterChanged);
            this.filterControlComplex.ViewChanged += new EventHandler(this.filterControlComplex_ViewChanged);
            manager.ApplyResources(this.btnDrives, "btnDrives");
            this.btnDrives.Name = "btnDrives";
            this.btnDrives.UseVisualStyleBackColor = true;
            this.btnDrives.Click += new EventHandler(this.btnDrives_Click);
            manager.ApplyResources(this.chkProcessSubfolders, "chkProcessSubfolders");
            this.chkProcessSubfolders.Checked = true;
            this.chkProcessSubfolders.CheckState = CheckState.Checked;
            this.tlpBack.SetColumnSpan(this.chkProcessSubfolders, 2);
            this.chkProcessSubfolders.Name = "chkProcessSubfolders";
            this.chkProcessSubfolders.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSkipReparsePoints, "chkSkipReparsePoints");
            this.chkSkipReparsePoints.Checked = true;
            this.chkSkipReparsePoints.CheckState = CheckState.Checked;
            this.tlpBack.SetColumnSpan(this.chkSkipReparsePoints, 3);
            this.chkSkipReparsePoints.Name = "chkSkipReparsePoints";
            this.chkSkipReparsePoints.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkProcessArchives, "chkProcessArchives");
            this.chkProcessArchives.Name = "chkProcessArchives";
            this.chkProcessArchives.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.tsView, 0, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 3, 0);
            this.tlpButtons.Controls.Add(this.btnClear, 2, 0);
            this.tlpButtons.Name = "tlpButtons";
            this.tsView.BackColor = SystemColors.Control;
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
            this.tsmiViewBasic.Click += new EventHandler(this.tsmiShowBasic_Click);
            this.tsmiViewAdvanced.Name = "tsmiViewAdvanced";
            manager.ApplyResources(this.tsmiViewAdvanced, "tsmiViewAdvanced");
            this.tsmiViewAdvanced.Paint += new PaintEventHandler(this.tsmiViewBasic_Paint);
            this.tsmiViewAdvanced.Click += new EventHandler(this.tsmiShowBasic_Click);
            this.tsmiViewFull.Name = "tsmiViewFull";
            manager.ApplyResources(this.tsmiViewFull, "tsmiViewFull");
            this.tsmiViewFull.Paint += new PaintEventHandler(this.tsmiViewBasic_Paint);
            this.tsmiViewFull.Click += new EventHandler(this.tsmiShowBasic_Click);
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.imgView.ColorDepth = ColorDepth.Depth8Bit;
            manager.ApplyResources(this.imgView, "imgView");
            this.imgView.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.tlpTemplate, "tlpTemplate");
            this.tlpTemplate.Controls.Add(this.lblSearchTemplate, 0, 0);
            this.tlpTemplate.Controls.Add(this.cmbSearchTemplate, 1, 0);
            this.tlpTemplate.Controls.Add(this.btnSaveTemplate, 2, 0);
            this.tlpTemplate.Controls.Add(this.btnDeleteTemplate, 3, 0);
            this.tlpTemplate.Name = "tlpTemplate";
            manager.ApplyResources(this.lblSearchTemplate, "lblSearchTemplate");
            this.lblSearchTemplate.Name = "lblSearchTemplate";
            this.cmbSearchTemplate.DeleteButton = this.btnDeleteTemplate;
            manager.ApplyResources(this.cmbSearchTemplate, "cmbSearchTemplate");
            this.cmbSearchTemplate.FormattingEnabled = true;
            this.cmbSearchTemplate.Name = "cmbSearchTemplate";
            this.cmbSearchTemplate.SaveButton = this.btnSaveTemplate;
            this.cmbSearchTemplate.SelectionChangeCommitted += new EventHandler(this.cmbSearchTemplate_SelectionChangeCommitted);
            this.cmbSearchTemplate.Leave += new EventHandler(this.cmbSearchTemplate_Enter);
            this.cmbSearchTemplate.Enter += new EventHandler(this.cmbSearchTemplate_Enter);
            this.cmbSearchTemplate.TextUpdate += new EventHandler(this.cmbSearchTemplate_TextUpdate);
            manager.ApplyResources(this.btnDeleteTemplate, "btnDeleteTemplate");
            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
            this.btnDeleteTemplate.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnSaveTemplate, "btnSaveTemplate");
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new EventHandler(this.btnSaveTemplate_Click);
            this.btnSaveTemplate.EnabledChanged += new EventHandler(this.btnSaveTemplate_EnabledChanged);
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            manager.ApplyResources(this.bvlTemplate, "bvlTemplate");
            this.bvlTemplate.ForeColor = SystemColors.ControlDarkDark;
            this.bvlTemplate.Name = "bvlTemplate";
            this.bvlTemplate.Sides = Border3DSide.Top;
            this.bvlTemplate.Style = Border3DStyle.Flat;
            this.Validator.Owner = this;
            this.Validator.OwnerFormValidate = FormValidate.DisableAcceptButton;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpBack);
            base.Controls.Add(this.bvlTemplate);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpButtons);
            base.Controls.Add(this.tlpTemplate);
            base.FixMouseWheel = Settings.Default.FixMouseWheel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SearchDialog";
            base.ShowInTaskbar = false;
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
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
                    if (this.cmbSearchTemplate.Modified)
                    {
                        List<NamedFilter> list = new List<NamedFilter>();
                        foreach (KeyValuePair<string, NamedFilter> pair in this.cmbSearchTemplate.GetItems<NamedFilter>())
                        {
                            pair.Value.Name = pair.Key;
                            list.Add(pair.Value);
                        }
                        Settings.Default.Searches = (list.Count > 0) ? list.ToArray() : null;
                    }
                    break;
            }
            base.OnFormClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.filterControlComplex_ViewChanged(this.filterControlComplex, EventArgs.Empty);
            this.filterControlComplex.SelectFirst(false);
        }

        protected override void OnShown(EventArgs e)
        {
            this.cmbLookFolder.AutoCompleteMode = Settings.Default.AutoCompleteMode;
            base.OnShown(e);
            if (this.FindDuplicatesControl.DuplicateOptions != 0)
            {
                this.filterControlComplex.View = ComplexFilterView.Full;
            }
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

        private void tsmiShowBasic_Click(object sender, EventArgs e)
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
            if ((!this.cmbSearchTemplate.DroppedDown && this.btnSaveTemplate.Enabled) && this.cmbSearchTemplate.Focused)
            {
                btnOk = this.btnSaveTemplate;
            }
            if (base.AcceptButton != btnOk)
            {
                base.AcceptButton = btnOk;
            }
        }

        public FindDuplicateOptions DuplicateOptions
        {
            get
            {
                return ((this.filterControlComplex.View == ComplexFilterView.Full) ? this.FindDuplicatesControl.DuplicateOptions : ((FindDuplicateOptions) 0));
            }
            set
            {
                if (value != 0)
                {
                    this.filterControlComplex.View = ComplexFilterView.Full;
                }
                this.FindDuplicatesControl.DuplicateOptions = value;
            }
        }

        [Browsable(false)]
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

        [Browsable(false)]
        public IVirtualFolder Folder
        {
            get
            {
                if (this.FFolder == null)
                {
                    IVirtualFolder[] folders = this.Folders;
                    if ((folders != null) && (folders.Length > 0))
                    {
                        if (folders.Length == 1)
                        {
                            this.FFolder = folders[0];
                        }
                        else
                        {
                            this.FFolder = new AggregatedVirtualFolder(folders);
                        }
                    }
                }
                return this.FFolder;
            }
            set
            {
                AggregatedVirtualFolder folder = value as AggregatedVirtualFolder;
                if (folder != null)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (IVirtualItem item in folder.GetContent())
                    {
                        if (item is IVirtualFolder)
                        {
                            if (builder.Length > 0)
                            {
                                builder.Append(';');
                            }
                            builder.Append(item.FullName);
                        }
                    }
                    this.cmbLookFolder.Text = builder.ToString();
                }
                else
                {
                    this.cmbLookFolder.Text = (value == null) ? string.Empty : value.FullName;
                }
                this.FFolder = value;
            }
        }

        private IVirtualFolder[] Folders
        {
            get
            {
                IEnumerable<string> enumerable = null;
                if (this.cmbLookFolder.Text.IndexOf(';') >= 0)
                {
                    enumerable = StringHelper.SplitString(this.cmbLookFolder.Text, new char[] { ';' });
                }
                else
                {
                    enumerable = new string[] { this.cmbLookFolder.Text };
                }
                List<IVirtualFolder> list = new List<IVirtualFolder>();
                foreach (string str in enumerable)
                {
                    string str2 = str.Trim();
                    if (!string.IsNullOrEmpty(str2))
                    {
                        list.Add((IVirtualFolder) VirtualItem.FromFullName(str2, VirtualItemType.Folder));
                    }
                }
                return ((list.Count > 0) ? list.ToArray() : null);
            }
        }

        public SearchFolderOptions SearchOptions
        {
            get
            {
                return (((this.chkProcessSubfolders.Checked ? SearchFolderOptions.ProcessSubfolders : ((SearchFolderOptions) 0)) | (this.chkProcessArchives.Checked ? SearchFolderOptions.ProcessArchives : ((SearchFolderOptions) 0))) | (this.chkSkipReparsePoints.Checked ? SearchFolderOptions.SkipReparsePoints : ((SearchFolderOptions) 0)));
            }
            set
            {
                this.chkProcessSubfolders.Checked = (value & SearchFolderOptions.ProcessSubfolders) > 0;
                this.chkProcessArchives.Checked = (value & SearchFolderOptions.ProcessArchives) > 0;
                this.chkSkipReparsePoints.Checked = (value & SearchFolderOptions.SkipReparsePoints) > 0;
            }
        }
    }
}

