namespace Nomad.Controls.Filter
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ComplexFilterControl : CustomFilterControl
    {
        private CheckBox chkAlwaysMatchFolders;
        private ComboBox cmbExcludeMasks;
        private ComboBox cmbIncludeMasks;
        private IContainer components = null;
        private ViewFilters FAdvancedViewFilters = (ViewFilters.Advanced | ViewFilters.Attributes | ViewFilters.Content | ViewFilters.ExcludeMask | ViewFilters.IncludeMask);
        private ViewFilters FBasicViewFilters = (ViewFilters.Attributes | ViewFilters.IncludeMask);
        private ComplexFilterView FFilterView = ComplexFilterView.Basic;
        private ViewFilters FHideViewFilters = 0;
        private AdvancedFilterControl filterControlAdvanced;
        private AttrFilterControl filterControlAttributes;
        private ContainerFilterControl filterControlComplex;
        private ContentFilterControl filterControlContent;
        private ComplexFilterView FView = ComplexFilterView.Full;
        private ComplexFilterView FVisibleView = ComplexFilterView.Full;
        private GroupBox grpAttributes;
        private GroupBox grpContent;
        private Label lblExcludeMasks;
        private Label lblIncludeMasks;
        private Panel pnlComplexFilter;
        private int SuspendViewLayoutCount;
        private TabControl tabControlFilter;
        private TabPage tabPageAdvanced;
        private TabPage tabPageBasic;
        private TableLayoutPanel tlpBack;

        public event EventHandler ViewChanged;

        public ComplexFilterControl()
        {
            this.InitializeComponent();
            if (!Application.RenderWithVisualStyles)
            {
                this.tabPageAdvanced.Paint -= new PaintEventHandler(this.tabPageAdvanced_Paint);
                this.pnlComplexFilter.BorderStyle = BorderStyle.Fixed3D;
            }
            this.LoadComponentSettings();
        }

        public void AddNewTab(string tabName, Control tabControl)
        {
            tabControl.Dock = DockStyle.Fill;
            TabPage page = new TabPage(tabName) {
                Padding = new Padding(6, 11, 8, 11),
                UseVisualStyleBackColor = true
            };
            page.Controls.Add(tabControl);
            this.tabControlFilter.TabPages.Add(page);
        }

        public void Clear()
        {
            this.ClearBasicFilter();
            if (this.filterControlComplex != null)
            {
                this.filterControlComplex.Clear();
            }
        }

        private void ClearBasicFilter()
        {
            this.cmbIncludeMasks.Text = string.Empty;
            this.cmbExcludeMasks.Text = string.Empty;
            this.filterControlContent.Clear();
            this.filterControlAttributes.Clear();
            this.filterControlAdvanced.Clear();
            this.chkAlwaysMatchFolders.Checked = true;
        }

        private void cmbIncludeMasks_HandleCreated(object sender, EventArgs e)
        {
            ComboBox SenderComboBox = (ComboBox) sender;
            SenderComboBox.BeginInvoke(delegate {
                SenderComboBox.SelectionLength = 0;
            });
        }

        private void ComplexFilterControl_VisibleChanged(object sender, EventArgs e)
        {
            if (((Control) sender).Visible && (this.FFilterView > this.View))
            {
                this.View = this.FFilterView;
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

        private void FilterControl_FilterChanged(object sender, EventArgs e)
        {
            base.RaiseFilterChanged();
        }

        private IVirtualItemFilter GetBasicFilter(ViewFilters filters)
        {
            IVirtualItemFilter filter3;
            List<IVirtualItemFilter> list = new List<IVirtualItemFilter>();
            if (((filters & ViewFilters.IncludeMask) > 0) && !string.IsNullOrEmpty(this.cmbIncludeMasks.Text))
            {
                foreach (IVirtualItemFilter filter in this.GetNameFilter(this.cmbIncludeMasks.Text, NamePatternCondition.Equal))
                {
                    list.Add(filter);
                }
            }
            if (((filters & ViewFilters.ExcludeMask) > 0) && !string.IsNullOrEmpty(this.cmbExcludeMasks.Text))
            {
                foreach (IVirtualItemFilter filter in this.GetNameFilter(this.cmbExcludeMasks.Text, NamePatternCondition.NotEqual))
                {
                    list.Add(filter);
                }
            }
            if (!(((filters & ViewFilters.Content) <= 0) || this.filterControlContent.IsEmpty))
            {
                list.Add(this.filterControlContent.Filter);
            }
            if (!(((filters & ViewFilters.Attributes) <= 0) || this.filterControlAttributes.IsEmpty))
            {
                list.Add(this.filterControlAttributes.Filter);
            }
            if ((filters & ViewFilters.Advanced) > 0)
            {
                AggregatedVirtualItemFilter filter2 = this.filterControlAdvanced.Filter as AggregatedVirtualItemFilter;
                if (filter2 != null)
                {
                    list.AddRange(filter2.Filters);
                }
            }
            if (list.Count == 1)
            {
                filter3 = list[0];
            }
            else if (list.Count > 1)
            {
                filter3 = new AggregatedVirtualItemFilter(list);
            }
            else
            {
                filter3 = null;
            }
            if ((((filters & ViewFilters.Folder) > 0) && this.chkAlwaysMatchFolders.Checked) && (filter3 != null))
            {
                filter3 = new AggregatedVirtualItemFilter(AggregatedFilterCondition.Any, new VirtualItemAttributeFilter(FileAttributes.Directory), filter3);
            }
            return filter3;
        }

        private IEnumerable<IVirtualItemFilter> GetNameFilter(string mask, NamePatternCondition condition)
        {
            return new <GetNameFilter>d__0(-2) { <>4__this = this, <>3__mask = mask, <>3__condition = condition };
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (this.FVisibleView != this.View)
            {
                this.UpdateViewLayout(this.View, false);
            }
            if (this.View == ComplexFilterView.Full)
            {
                Size preferredSize = this.tabControlFilter.GetPreferredSize(proposedSize);
                preferredSize.Height = ((this.tabControlFilter.Height - this.tabPageBasic.Height) + this.tabPageBasic.Padding.Vertical) + this.tlpBack.GetPreferredSize(proposedSize).Height;
                return preferredSize;
            }
            return this.tlpBack.GetPreferredSize(proposedSize);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ComplexFilterControl));
            this.lblIncludeMasks = new Label();
            this.lblExcludeMasks = new Label();
            this.grpAttributes = new GroupBox();
            this.filterControlAttributes = new AttrFilterControl();
            this.cmbIncludeMasks = new ComboBox();
            this.tabControlFilter = new TabControl();
            this.tabPageBasic = new TabPage();
            this.tlpBack = new TableLayoutPanel();
            this.cmbExcludeMasks = new ComboBox();
            this.chkAlwaysMatchFolders = new CheckBox();
            this.filterControlAdvanced = new AdvancedFilterControl();
            this.grpContent = new GroupBox();
            this.filterControlContent = new ContentFilterControl();
            this.tabPageAdvanced = new TabPage();
            this.pnlComplexFilter = new Panel();
            this.grpAttributes.SuspendLayout();
            this.tabControlFilter.SuspendLayout();
            this.tabPageBasic.SuspendLayout();
            this.tlpBack.SuspendLayout();
            this.grpContent.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.lblIncludeMasks, "lblIncludeMasks");
            this.lblIncludeMasks.Name = "lblIncludeMasks";
            manager.ApplyResources(this.lblExcludeMasks, "lblExcludeMasks");
            this.lblExcludeMasks.Name = "lblExcludeMasks";
            manager.ApplyResources(this.grpAttributes, "grpAttributes");
            this.tlpBack.SetColumnSpan(this.grpAttributes, 2);
            this.grpAttributes.Controls.Add(this.filterControlAttributes);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.TabStop = false;
            manager.ApplyResources(this.filterControlAttributes, "filterControlAttributes");
            this.filterControlAttributes.Name = "filterControlAttributes";
            this.filterControlAttributes.FilterChanged += new EventHandler(this.FilterControl_FilterChanged);
            manager.ApplyResources(this.cmbIncludeMasks, "cmbIncludeMasks");
            this.cmbIncludeMasks.FormattingEnabled = true;
            this.cmbIncludeMasks.Name = "cmbIncludeMasks";
            this.cmbIncludeMasks.HandleCreated += new EventHandler(this.cmbIncludeMasks_HandleCreated);
            this.cmbIncludeMasks.TextUpdate += new EventHandler(this.FilterControl_FilterChanged);
            this.tabControlFilter.Controls.Add(this.tabPageBasic);
            this.tabControlFilter.Controls.Add(this.tabPageAdvanced);
            manager.ApplyResources(this.tabControlFilter, "tabControlFilter");
            this.tabControlFilter.Name = "tabControlFilter";
            this.tabControlFilter.SelectedIndex = 0;
            this.tabControlFilter.Selecting += new TabControlCancelEventHandler(this.tabControlFilter_Selecting);
            this.tabControlFilter.Selected += new TabControlEventHandler(this.tabControlFilter_Selected);
            this.tabControlFilter.Deselecting += new TabControlCancelEventHandler(this.tabControlFilter_Deselecting);
            this.tabPageBasic.Controls.Add(this.tlpBack);
            manager.ApplyResources(this.tabPageBasic, "tabPageBasic");
            this.tabPageBasic.Name = "tabPageBasic";
            this.tabPageBasic.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.cmbExcludeMasks, 1, 1);
            this.tlpBack.Controls.Add(this.lblExcludeMasks, 0, 1);
            this.tlpBack.Controls.Add(this.chkAlwaysMatchFolders, 0, 5);
            this.tlpBack.Controls.Add(this.filterControlAdvanced, 0, 4);
            this.tlpBack.Controls.Add(this.grpAttributes, 0, 3);
            this.tlpBack.Controls.Add(this.grpContent, 0, 2);
            this.tlpBack.Controls.Add(this.cmbIncludeMasks, 1, 0);
            this.tlpBack.Controls.Add(this.lblIncludeMasks, 0, 0);
            this.tlpBack.Name = "tlpBack";
            manager.ApplyResources(this.cmbExcludeMasks, "cmbExcludeMasks");
            this.cmbExcludeMasks.FormattingEnabled = true;
            this.cmbExcludeMasks.Name = "cmbExcludeMasks";
            this.cmbExcludeMasks.HandleCreated += new EventHandler(this.cmbIncludeMasks_HandleCreated);
            this.cmbExcludeMasks.TextUpdate += new EventHandler(this.FilterControl_FilterChanged);
            manager.ApplyResources(this.chkAlwaysMatchFolders, "chkAlwaysMatchFolders");
            this.chkAlwaysMatchFolders.Checked = true;
            this.chkAlwaysMatchFolders.CheckState = CheckState.Checked;
            this.tlpBack.SetColumnSpan(this.chkAlwaysMatchFolders, 2);
            this.chkAlwaysMatchFolders.Name = "chkAlwaysMatchFolders";
            this.chkAlwaysMatchFolders.UseVisualStyleBackColor = true;
            this.chkAlwaysMatchFolders.Click += new EventHandler(this.FilterControl_FilterChanged);
            manager.ApplyResources(this.filterControlAdvanced, "filterControlAdvanced");
            this.tlpBack.SetColumnSpan(this.filterControlAdvanced, 2);
            this.filterControlAdvanced.Name = "filterControlAdvanced";
            this.filterControlAdvanced.FilterChanged += new EventHandler(this.FilterControl_FilterChanged);
            manager.ApplyResources(this.grpContent, "grpContent");
            this.tlpBack.SetColumnSpan(this.grpContent, 2);
            this.grpContent.Controls.Add(this.filterControlContent);
            this.grpContent.Name = "grpContent";
            this.grpContent.TabStop = false;
            manager.ApplyResources(this.filterControlContent, "filterControlContent");
            this.filterControlContent.FilterOptions = ContentFilterOptions.DetectEncoding | ContentFilterOptions.UseIFilter;
            this.filterControlContent.MinimumSize = new Size(0x1a5, 0);
            this.filterControlContent.Name = "filterControlContent";
            this.filterControlContent.FilterChanged += new EventHandler(this.FilterControl_FilterChanged);
            this.tabPageAdvanced.Controls.Add(this.pnlComplexFilter);
            manager.ApplyResources(this.tabPageAdvanced, "tabPageAdvanced");
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            this.tabPageAdvanced.Paint += new PaintEventHandler(this.tabPageAdvanced_Paint);
            manager.ApplyResources(this.pnlComplexFilter, "pnlComplexFilter");
            this.pnlComplexFilter.BackColor = SystemColors.Window;
            this.pnlComplexFilter.Name = "pnlComplexFilter";
            manager.ApplyResources(this, "$this");
            base.Controls.Add(this.tabControlFilter);
            this.MinimumSize = new Size(0x1c4, 0);
            base.Name = "ComplexFilterControl";
            base.VisibleChanged += new EventHandler(this.ComplexFilterControl_VisibleChanged);
            this.grpAttributes.ResumeLayout(false);
            this.grpAttributes.PerformLayout();
            this.tabControlFilter.ResumeLayout(false);
            this.tabPageBasic.ResumeLayout(false);
            this.tabPageBasic.PerformLayout();
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.grpContent.ResumeLayout(false);
            this.tabPageAdvanced.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void LoadComponentSettings()
        {
            HistorySettings.PopulateComboBox(this.cmbIncludeMasks, HistorySettings.Default.IncludeMasks);
            HistorySettings.PopulateComboBox(this.cmbExcludeMasks, HistorySettings.Default.ExcludeMasks);
            this.filterControlContent.LoadComponentSettings();
        }

        private void ResumeViewLayout()
        {
            this.SuspendViewLayoutCount--;
            if ((this.SuspendViewLayoutCount == 0) && (this.FVisibleView != this.View))
            {
                this.UpdateViewLayout(this.View, false);
            }
        }

        public void SaveComponentSettings()
        {
            HistorySettings.Default.AddStringToIncludeMasks(this.cmbIncludeMasks.Text);
            HistorySettings.Default.AddStringToExcludeMasks(this.cmbExcludeMasks.Text);
            this.filterControlContent.SaveComponentSettings();
        }

        public void SelectFirst(bool resetToBasic)
        {
            if (resetToBasic)
            {
                this.tabControlFilter.SelectedTab = this.tabPageBasic;
            }
            base.SelectNextControl(this, true, true, true, false);
            if (base.ActiveControl is TabControl)
            {
                base.SelectNextControl(base.ActiveControl, true, true, true, false);
            }
        }

        private void SetBasicFilter(IVirtualItemFilter filter)
        {
            this.ClearBasicFilter();
            if (filter != null)
            {
                this.chkAlwaysMatchFolders.Checked = false;
                AggregatedVirtualItemFilter filter2 = filter as AggregatedVirtualItemFilter;
                if (filter2 == null)
                {
                    this.SetFilter(filter);
                }
                else
                {
                    switch (filter2.Condition)
                    {
                        case AggregatedFilterCondition.All:
                            this.SetFilters(filter2.Filters);
                            return;

                        case AggregatedFilterCondition.Any:
                        {
                            bool flag = false;
                            bool flag2 = false;
                            for (int i = 0; (i < filter2.Filters.Count) && (!flag || !flag2); i++)
                            {
                                IVirtualItemFilter filter3 = filter2.Filters[i];
                                AttributeFilter filter4 = filter3 as AttributeFilter;
                                if (((!flag && (filter4 != null)) && (filter4.IncludeAttributes == FileAttributes.Directory)) && (filter4.ExcludeAttributes == 0))
                                {
                                    this.chkAlwaysMatchFolders.Checked = true;
                                    flag = true;
                                }
                                else if (!flag2)
                                {
                                    AggregatedVirtualItemFilter filter5 = filter3 as AggregatedVirtualItemFilter;
                                    if ((filter5 != null) && (filter5.Condition == AggregatedFilterCondition.All))
                                    {
                                        this.SetFilters(filter5.Filters);
                                    }
                                    else
                                    {
                                        this.SetFilter(filter3);
                                    }
                                    flag2 = true;
                                }
                            }
                            return;
                        }
                    }
                }
            }
        }

        public void SetFilter(IVirtualItemFilter filter)
        {
            NameFilter filter2 = filter as NameFilter;
            if ((filter2 != null) && (filter2.NameComparision != NamePatternComparision.Ignore))
            {
                ComboBox box = (filter2.NameCondition == NamePatternCondition.Equal) ? this.cmbIncludeMasks : this.cmbExcludeMasks;
                switch (filter2.NameComparision)
                {
                    case NamePatternComparision.Wildcards:
                    case NamePatternComparision.RegEx:
                        box.Text = filter2.ToString();
                        return;

                    case NamePatternComparision.Ignore:
                        return;
                }
                string text = box.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    text = text + ';';
                }
                text = text + filter2.ToString();
                box.Text = text;
                int index = box.Items.IndexOf(text);
                if (index >= 0)
                {
                    box.SelectedIndex = index;
                }
                else
                {
                    box.SelectedIndex = -1;
                }
            }
            else if (filter is AttributeFilter)
            {
                this.filterControlAttributes.SetFilter((AttributeFilter) filter);
            }
            else if (filter is ContentFilter)
            {
                this.filterControlContent.SetFilter((ContentFilter) filter);
            }
            else
            {
                this.filterControlAdvanced.SetFilter(filter);
            }
        }

        private void SetFilters(IEnumerable<IVirtualItemFilter> filters)
        {
            foreach (IVirtualItemFilter filter in filters)
            {
                this.SetFilter(filter);
            }
        }

        private void ShowBasicFilter(ViewFilters showFilters, ViewFilters hideFilters, ViewFilters filter, Control filterControl)
        {
            filterControl.Visible = ((hideFilters & filter) == 0) && ((showFilters == 0) || ((showFilters & filter) > 0));
        }

        private void ShowBasicFilters(ViewFilters showFilters, ViewFilters hideFilters)
        {
            this.tlpBack.SuspendLayout();
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.IncludeMask, this.cmbIncludeMasks);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.IncludeMask, this.lblIncludeMasks);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.ExcludeMask, this.cmbExcludeMasks);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.ExcludeMask, this.lblExcludeMasks);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.Content, this.grpContent);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.Attributes, this.grpAttributes);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.Advanced, this.filterControlAdvanced);
            this.ShowBasicFilter(showFilters, hideFilters, ViewFilters.Folder, this.chkAlwaysMatchFolders);
            this.tlpBack.ResumeLayout();
        }

        private void SuspendViewLayout()
        {
            this.SuspendViewLayoutCount++;
        }

        private void tabControlFilter_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == this.tabPageAdvanced)
            {
                IVirtualItemFilter filter = this.filterControlComplex.Filter;
                this.SetBasicFilter(filter);
                IVirtualItemFilter basicFilter = this.GetBasicFilter(this.AllViewFilters);
                if (!FilterHelper.FilterEquals(filter, basicFilter))
                {
                    e.Cancel = MessageDialog.Show(this, Resources.sAskCannotDisplayTooComplexFilter, Resources.sConfirmChangeView, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) != MessageDialogResult.Yes;
                }
            }
        }

        private void tabControlFilter_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == this.tabPageAdvanced)
            {
                this.filterControlComplex.Filter = this.GetBasicFilter(this.AllViewFilters);
            }
            else
            {
                base.RaiseFilterChanged();
            }
        }

        private void tabControlFilter_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if ((e.TabPage == this.tabPageAdvanced) && (this.filterControlComplex == null))
            {
                this.filterControlComplex = new ContainerFilterControl();
                this.filterControlComplex.Location = new Point(3, 3);
                BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
                if (argument != null)
                {
                    argument.Localize(this.filterControlComplex);
                }
                this.pnlComplexFilter.Controls.Add(this.filterControlComplex);
            }
        }

        private void tabPageAdvanced_Paint(object sender, PaintEventArgs e)
        {
            Rectangle bounds = this.pnlComplexFilter.Bounds;
            bounds = Rectangle.FromLTRB(bounds.Left - 2, bounds.Top - 2, bounds.Right + 1, bounds.Bottom + 1);
            e.Graphics.DrawRectangle(Pens.DarkGray, bounds);
        }

        private bool UpdateViewLayout(ComplexFilterView newView, bool showWarning)
        {
            ViewFilters basicViewFilters;
            switch (newView)
            {
                case ComplexFilterView.Basic:
                    basicViewFilters = this.BasicViewFilters;
                    break;

                case ComplexFilterView.Advanced:
                    basicViewFilters = this.AdvancedViewFilters;
                    break;

                default:
                    basicViewFilters = 0;
                    break;
            }
            if ((newView < this.View) && showWarning)
            {
                IVirtualItemFilter filter = this.Filter;
                if (this.tabControlFilter.SelectedTab == this.tabPageAdvanced)
                {
                    this.SetBasicFilter(filter);
                }
                IVirtualItemFilter basicFilter = this.GetBasicFilter(basicViewFilters);
                if (!FilterHelper.FilterEquals(filter, basicFilter))
                {
                    if (MessageDialog.Show(this, Resources.sAskCannotDisplayTooComplexFilter, Resources.sConfirmChangeView, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) != MessageDialogResult.Yes)
                    {
                        return false;
                    }
                }
                else
                {
                    this.FFilterView = newView;
                }
            }
            base.SuspendLayout();
            switch (newView)
            {
                case ComplexFilterView.Basic:
                case ComplexFilterView.Advanced:
                    this.tabControlFilter.Visible = false;
                    if (this.tlpBack.Parent != this)
                    {
                        base.Padding = this.tlpBack.Margin;
                        this.tlpBack.Parent = this;
                    }
                    this.ShowBasicFilters(basicViewFilters, this.HideViewFilters);
                    break;

                case ComplexFilterView.Full:
                    if (this.tlpBack.Parent != this.tabPageBasic)
                    {
                        this.tlpBack.Parent = this.tabPageBasic;
                    }
                    this.ShowBasicFilters(0, this.HideViewFilters);
                    this.tabControlFilter.Visible = true;
                    base.Padding = this.tabControlFilter.Margin;
                    break;
            }
            base.ResumeLayout();
            this.lblIncludeMasks.Visible = false;
            this.lblIncludeMasks.Visible = true;
            this.FVisibleView = newView;
            return true;
        }

        [DefaultValue(0x1f)]
        public ViewFilters AdvancedViewFilters
        {
            get
            {
                return (this.FAdvancedViewFilters & ~this.HideViewFilters);
            }
            set
            {
                if (this.FAdvancedViewFilters != value)
                {
                    this.FAdvancedViewFilters = value;
                    if (this.View == ComplexFilterView.Advanced)
                    {
                        this.ShowBasicFilters(this.FAdvancedViewFilters, this.HideViewFilters);
                    }
                }
            }
        }

        private ViewFilters AllViewFilters
        {
            get
            {
                return ((ViewFilters.Folder | ViewFilters.Advanced | ViewFilters.Attributes | ViewFilters.Content | ViewFilters.ExcludeMask | ViewFilters.IncludeMask) & ~this.HideViewFilters);
            }
        }

        [DefaultValue(9)]
        public ViewFilters BasicViewFilters
        {
            get
            {
                return (this.FBasicViewFilters & ~this.HideViewFilters);
            }
            set
            {
                if (this.FBasicViewFilters != value)
                {
                    this.FBasicViewFilters = value;
                    if (this.View == ComplexFilterView.Basic)
                    {
                        this.ShowBasicFilters(this.FBasicViewFilters, this.HideViewFilters);
                    }
                }
            }
        }

        [DefaultValue((string) null), Browsable(false)]
        public IVirtualItemFilter Filter
        {
            get
            {
                switch (this.View)
                {
                    case ComplexFilterView.Basic:
                        return this.GetBasicFilter(this.BasicViewFilters);

                    case ComplexFilterView.Advanced:
                        return this.GetBasicFilter(this.AdvancedViewFilters);
                }
                if (this.tabControlFilter.SelectedTab == this.tabPageBasic)
                {
                    return this.GetBasicFilter(this.AllViewFilters);
                }
                if (this.tabControlFilter.SelectedTab == this.tabPageAdvanced)
                {
                    return this.filterControlComplex.Filter;
                }
                IVirtualItemFilter basicFilter = (this.filterControlComplex != null) ? this.filterControlComplex.Filter : null;
                if (basicFilter == null)
                {
                    basicFilter = this.GetBasicFilter(this.AllViewFilters);
                }
                return basicFilter;
            }
            set
            {
                if (value == null)
                {
                    this.Clear();
                }
                else
                {
                    this.SetBasicFilter(value);
                    this.SuspendViewLayout();
                    try
                    {
                        if (!((this.View != ComplexFilterView.Basic) || FilterHelper.FilterEquals(this.GetBasicFilter(this.BasicViewFilters), value)))
                        {
                            this.View = ComplexFilterView.Advanced;
                        }
                        if (!((this.View != ComplexFilterView.Advanced) || FilterHelper.FilterEquals(this.GetBasicFilter(this.AdvancedViewFilters), value)))
                        {
                            this.View = ComplexFilterView.Full;
                        }
                        if ((this.View == ComplexFilterView.Full) && ((this.tabControlFilter.SelectedTab == this.tabPageAdvanced) || ((this.tabControlFilter.SelectedTab == this.tabPageBasic) && !FilterHelper.FilterEquals(this.GetBasicFilter(this.AllViewFilters), value))))
                        {
                            this.tabControlFilter.SelectedTab = this.tabPageAdvanced;
                            this.tabControlFilter_Selecting(this.tabControlFilter, new TabControlCancelEventArgs(this.tabPageAdvanced, 1, false, TabControlAction.Selecting));
                            this.filterControlComplex.Filter = value;
                        }
                    }
                    finally
                    {
                        this.ResumeViewLayout();
                    }
                    this.FFilterView = this.View;
                }
            }
        }

        [DefaultValue(0)]
        public ViewFilters HideViewFilters
        {
            get
            {
                return this.FHideViewFilters;
            }
            set
            {
                if (this.FHideViewFilters != value)
                {
                    ViewFilters basicViewFilters;
                    this.FHideViewFilters = value;
                    switch (this.View)
                    {
                        case ComplexFilterView.Basic:
                            basicViewFilters = this.BasicViewFilters;
                            break;

                        case ComplexFilterView.Advanced:
                            basicViewFilters = this.AdvancedViewFilters;
                            break;

                        default:
                            basicViewFilters = 0;
                            break;
                    }
                    this.ShowBasicFilters(basicViewFilters, this.HideViewFilters);
                }
            }
        }

        [DefaultValue(2)]
        public ComplexFilterView View
        {
            get
            {
                return this.FView;
            }
            set
            {
                if ((value != this.FView) && ((this.SuspendViewLayoutCount != 0) || this.UpdateViewLayout(value, true)))
                {
                    this.FView = value;
                    if (this.ViewChanged != null)
                    {
                        this.ViewChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetNameFilter>d__0 : IEnumerable<IVirtualItemFilter>, IEnumerable, IEnumerator<IVirtualItemFilter>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItemFilter <>2__current;
            public NamePatternCondition <>3__condition;
            public string <>3__mask;
            public ComplexFilterControl <>4__this;
            public IEnumerator<string> <>7__wrap3;
            private int <>l__initialThreadId;
            public VirtualItemNameFilter <MaskFilter>5__1;
            public string <NextMask>5__2;
            public NamePatternCondition condition;
            public string mask;

            [DebuggerHidden]
            public <GetNameFilter>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<MaskFilter>5__1 = new VirtualItemNameFilter(this.condition, this.mask);
                            switch (this.<MaskFilter>5__1.NameComparision)
                            {
                                case NamePatternComparision.Wildcards:
                                case NamePatternComparision.RegEx:
                                    this.<>2__current = this.<MaskFilter>5__1;
                                    this.<>1__state = 1;
                                    return true;

                                case NamePatternComparision.Ignore:
                                    goto Label_0182;
                            }
                            break;

                        case 1:
                            this.<>1__state = -1;
                            goto Label_0182;

                        case 2:
                            this.<>1__state = -1;
                            goto Label_0182;

                        case 4:
                            goto Label_0161;

                        default:
                            goto Label_0182;
                    }
                    if (this.mask.IndexOf(';') < 0)
                    {
                        this.<>2__current = this.<MaskFilter>5__1;
                        this.<>1__state = 2;
                        return true;
                    }
                    this.<>7__wrap3 = StringHelper.SplitString(this.mask, new char[] { ';' }).GetEnumerator();
                    this.<>1__state = 3;
                    while (this.<>7__wrap3.MoveNext())
                    {
                        this.<NextMask>5__2 = this.<>7__wrap3.Current;
                        this.<MaskFilter>5__1 = new VirtualItemNameFilter(this.condition, this.<NextMask>5__2);
                        if (this.<MaskFilter>5__1.NameComparision == NamePatternComparision.Ignore)
                        {
                            goto Label_0168;
                        }
                        this.<>2__current = this.<MaskFilter>5__1;
                        this.<>1__state = 4;
                        return true;
                    Label_0161:
                        this.<>1__state = 3;
                    Label_0168:;
                    }
                    this.<>m__Finally4();
                Label_0182:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<IVirtualItemFilter> IEnumerable<IVirtualItemFilter>.GetEnumerator()
            {
                ComplexFilterControl.<GetNameFilter>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ComplexFilterControl.<GetNameFilter>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.mask = this.<>3__mask;
                d__.condition = this.<>3__condition;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.Filter.IVirtualItemFilter>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            IVirtualItemFilter IEnumerator<IVirtualItemFilter>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

