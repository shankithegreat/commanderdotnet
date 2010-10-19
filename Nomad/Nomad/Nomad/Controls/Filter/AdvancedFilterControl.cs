namespace Nomad.Controls.Filter
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class AdvancedFilterControl : CustomFilterControl, IUpdateCulture
    {
        private ComboBoxEx cmbDate;
        private ComboBoxEx cmbDateMeasure;
        private ComboBoxEx cmbDateOperation;
        private ComboBoxEx cmbSize;
        private ComboBoxEx cmbSizeOperation;
        private ComboBoxEx cmbTime;
        private ComboBoxEx cmbTimeOperation;
        private IContainer components = null;
        private DateTimePicker dtpDateFrom;
        private DateTimePicker dtpDateTo;
        private DateTimePicker dtpTimeFrom;
        private DateTimePicker dtpTimeTo;
        private Label lblDateSeparator;
        private Label lblSizeSeparator;
        private Label lblTimeSeparator;
        private NumericUpDown nudNotOlderThan;
        private NumericUpDown nudSizeFrom;
        private NumericUpDown nudSizeTo;
        private TableLayoutPanel tblBack;

        public AdvancedFilterControl()
        {
            this.InitializeComponent();
            this.UpdateCulture();
            this.nudSizeFrom.Maximum = 9223372036854775807M;
            this.nudSizeTo.Maximum = 9223372036854775807M;
            this.Clear();
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            this.cmbDate.SelectedIndex = 2;
            this.cmbDateOperation.SelectedIndex = 0;
            this.cmbTime.SelectedIndex = 2;
            this.cmbTimeOperation.SelectedIndex = 0;
            this.cmbSize.SelectedIndex = 1;
            this.cmbSizeOperation.SelectedIndex = 0;
            this.cmbDateMeasure.SelectedIndex = 0;
            base.CanRaiseFilterChanged = true;
        }

        private void cmbDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.RaiseFilterChanged();
        }

        private void cmbDateOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tblBack.SuspendLayout();
            if (this.cmbDateOperation.SelectedIndex == 6)
            {
                this.dtpDateFrom.Parent = null;
                this.dtpDateTo.Parent = null;
                this.lblDateSeparator.Parent = null;
                this.nudNotOlderThan.Parent = this.tblBack;
                this.nudNotOlderThan.Dock = DockStyle.Fill;
                this.tblBack.SetColumnSpan(this.nudNotOlderThan, 2);
                this.cmbDateMeasure.Parent = this.tblBack;
                this.cmbDateMeasure.Dock = DockStyle.Fill;
                this.tblBack.ColumnStyles[4].SizeType = SizeType.AutoSize;
            }
            else
            {
                this.nudNotOlderThan.Parent = null;
                this.cmbDateMeasure.Parent = null;
                this.dtpDateFrom.Parent = this.tblBack;
                this.tblBack.ColumnStyles[4].SizeType = SizeType.Percent;
                this.UpdateControls(this.cmbDateOperation.SelectedIndex, this.dtpDateFrom, this.dtpDateTo, this.lblDateSeparator);
            }
            this.tblBack.ResumeLayout();
            base.RaiseFilterChanged();
        }

        private void cmbSizeOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateControls(this.cmbSizeOperation.SelectedIndex, this.nudSizeFrom, this.nudSizeTo, this.lblSizeSeparator);
            base.RaiseFilterChanged();
        }

        private void cmbTimeOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateControls(this.cmbTimeOperation.SelectedIndex, this.dtpTimeFrom, this.dtpTimeTo, this.lblTimeSeparator);
            base.RaiseFilterChanged();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AdvancedFilterControl));
            this.nudSizeFrom = new NumericUpDown();
            this.cmbSize = new ComboBoxEx();
            this.dtpTimeFrom = new DateTimePicker();
            this.dtpDateFrom = new DateTimePicker();
            this.cmbSizeOperation = new ComboBoxEx();
            this.cmbTimeOperation = new ComboBoxEx();
            this.cmbTime = new ComboBoxEx();
            this.cmbDateOperation = new ComboBoxEx();
            this.cmbDate = new ComboBoxEx();
            this.dtpDateTo = new DateTimePicker();
            this.dtpTimeTo = new DateTimePicker();
            this.nudSizeTo = new NumericUpDown();
            this.lblSizeSeparator = new Label();
            this.lblTimeSeparator = new Label();
            this.lblDateSeparator = new Label();
            this.nudNotOlderThan = new NumericUpDown();
            this.cmbDateMeasure = new ComboBoxEx();
            this.tblBack = new TableLayoutPanel();
            this.nudSizeFrom.BeginInit();
            this.nudSizeTo.BeginInit();
            this.nudNotOlderThan.BeginInit();
            this.tblBack.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.nudSizeFrom, "nudSizeFrom");
            this.nudSizeFrom.Name = "nudSizeFrom";
            this.nudSizeFrom.ValueChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.cmbSize, "cmbSize");
            this.cmbSize.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSize.FormattingEnabled = true;
            this.cmbSize.Items.AddRange(new object[] { manager.GetString("cmbSize.Items"), manager.GetString("cmbSize.Items1"), manager.GetString("cmbSize.Items2") });
            this.cmbSize.MinimumSize = new Size(110, 0);
            this.cmbSize.Name = "cmbSize";
            this.cmbSize.SelectedIndexChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.dtpTimeFrom, "dtpTimeFrom");
            this.dtpTimeFrom.Format = DateTimePickerFormat.Time;
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.ValueChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.dtpDateFrom, "dtpDateFrom");
            this.dtpDateFrom.Format = DateTimePickerFormat.Short;
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.ValueChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.cmbSizeOperation, "cmbSizeOperation");
            this.cmbSizeOperation.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSizeOperation.FormattingEnabled = true;
            this.cmbSizeOperation.Items.AddRange(new object[] { manager.GetString("cmbSizeOperation.Items"), manager.GetString("cmbSizeOperation.Items1"), manager.GetString("cmbSizeOperation.Items2"), manager.GetString("cmbSizeOperation.Items3"), manager.GetString("cmbSizeOperation.Items4"), manager.GetString("cmbSizeOperation.Items5"), manager.GetString("cmbSizeOperation.Items6"), manager.GetString("cmbSizeOperation.Items7") });
            this.cmbSizeOperation.MinimumSize = new Size(0x68, 0);
            this.cmbSizeOperation.Name = "cmbSizeOperation";
            this.cmbSizeOperation.SelectedIndexChanged += new EventHandler(this.cmbSizeOperation_SelectedIndexChanged);
            manager.ApplyResources(this.cmbTimeOperation, "cmbTimeOperation");
            this.cmbTimeOperation.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTimeOperation.FormattingEnabled = true;
            this.cmbTimeOperation.Items.AddRange(new object[] { manager.GetString("cmbTimeOperation.Items"), manager.GetString("cmbTimeOperation.Items1"), manager.GetString("cmbTimeOperation.Items2"), manager.GetString("cmbTimeOperation.Items3"), manager.GetString("cmbTimeOperation.Items4"), manager.GetString("cmbTimeOperation.Items5"), manager.GetString("cmbTimeOperation.Items6"), manager.GetString("cmbTimeOperation.Items7") });
            this.cmbTimeOperation.MinimumSize = new Size(0x68, 0);
            this.cmbTimeOperation.Name = "cmbTimeOperation";
            this.cmbTimeOperation.SelectedIndexChanged += new EventHandler(this.cmbTimeOperation_SelectedIndexChanged);
            manager.ApplyResources(this.cmbTime, "cmbTime");
            this.cmbTime.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTime.FormattingEnabled = true;
            this.cmbTime.Items.AddRange(new object[] { manager.GetString("cmbTime.Items"), manager.GetString("cmbTime.Items1"), manager.GetString("cmbTime.Items2") });
            this.cmbTime.MinimumSize = new Size(110, 0);
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.SelectedIndexChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.cmbDateOperation, "cmbDateOperation");
            this.cmbDateOperation.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDateOperation.FormattingEnabled = true;
            this.cmbDateOperation.Items.AddRange(new object[] { manager.GetString("cmbDateOperation.Items"), manager.GetString("cmbDateOperation.Items1"), manager.GetString("cmbDateOperation.Items2"), manager.GetString("cmbDateOperation.Items3"), manager.GetString("cmbDateOperation.Items4"), manager.GetString("cmbDateOperation.Items5"), manager.GetString("cmbDateOperation.Items6") });
            this.cmbDateOperation.MinimumSize = new Size(0x68, 0);
            this.cmbDateOperation.Name = "cmbDateOperation";
            this.cmbDateOperation.SelectedIndexChanged += new EventHandler(this.cmbDateOperation_SelectedIndexChanged);
            manager.ApplyResources(this.cmbDate, "cmbDate");
            this.cmbDate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDate.FormattingEnabled = true;
            this.cmbDate.Items.AddRange(new object[] { manager.GetString("cmbDate.Items"), manager.GetString("cmbDate.Items1"), manager.GetString("cmbDate.Items2") });
            this.cmbDate.MinimumSize = new Size(110, 0);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.SelectedIndexChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.dtpDateTo, "dtpDateTo");
            this.dtpDateTo.Format = DateTimePickerFormat.Short;
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.ValueChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.dtpTimeTo, "dtpTimeTo");
            this.dtpTimeTo.Format = DateTimePickerFormat.Time;
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.ValueChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.nudSizeTo, "nudSizeTo");
            this.nudSizeTo.Name = "nudSizeTo";
            this.nudSizeTo.ValueChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.lblSizeSeparator, "lblSizeSeparator");
            this.lblSizeSeparator.Name = "lblSizeSeparator";
            manager.ApplyResources(this.lblTimeSeparator, "lblTimeSeparator");
            this.lblTimeSeparator.Name = "lblTimeSeparator";
            manager.ApplyResources(this.lblDateSeparator, "lblDateSeparator");
            this.lblDateSeparator.Name = "lblDateSeparator";
            manager.ApplyResources(this.nudNotOlderThan, "nudNotOlderThan");
            int[] bits = new int[4];
            bits[0] = 1;
            this.nudNotOlderThan.Minimum = new decimal(bits);
            this.nudNotOlderThan.Name = "nudNotOlderThan";
            bits = new int[4];
            bits[0] = 1;
            this.nudNotOlderThan.Value = new decimal(bits);
            this.nudNotOlderThan.ValueChanged += new EventHandler(this.nudNotOlderThan_ValueChanged);
            manager.ApplyResources(this.cmbDateMeasure, "cmbDateMeasure");
            this.cmbDateMeasure.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDateMeasure.FormattingEnabled = true;
            this.cmbDateMeasure.Items.AddRange(new object[] { manager.GetString("cmbDateMeasure.Items"), manager.GetString("cmbDateMeasure.Items1"), manager.GetString("cmbDateMeasure.Items2"), manager.GetString("cmbDateMeasure.Items3") });
            this.cmbDateMeasure.MinimumSize = new Size(90, 0);
            this.cmbDateMeasure.Name = "cmbDateMeasure";
            this.cmbDateMeasure.SelectedIndexChanged += new EventHandler(this.cmbDate_SelectedIndexChanged);
            manager.ApplyResources(this.tblBack, "tblBack");
            this.tblBack.Controls.Add(this.cmbDate, 0, 0);
            this.tblBack.Controls.Add(this.cmbTime, 0, 1);
            this.tblBack.Controls.Add(this.cmbSize, 0, 2);
            this.tblBack.Controls.Add(this.nudSizeTo, 4, 2);
            this.tblBack.Controls.Add(this.nudSizeFrom, 2, 2);
            this.tblBack.Controls.Add(this.lblTimeSeparator, 3, 1);
            this.tblBack.Controls.Add(this.dtpTimeTo, 4, 1);
            this.tblBack.Controls.Add(this.dtpTimeFrom, 2, 1);
            this.tblBack.Controls.Add(this.lblSizeSeparator, 3, 2);
            this.tblBack.Controls.Add(this.dtpDateTo, 4, 0);
            this.tblBack.Controls.Add(this.dtpDateFrom, 2, 0);
            this.tblBack.Controls.Add(this.lblDateSeparator, 3, 0);
            this.tblBack.Controls.Add(this.cmbDateOperation, 1, 0);
            this.tblBack.Controls.Add(this.cmbSizeOperation, 1, 2);
            this.tblBack.Controls.Add(this.cmbTimeOperation, 1, 1);
            this.tblBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tblBack.Name = "tblBack";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblBack);
            base.Controls.Add(this.cmbDateMeasure);
            base.Controls.Add(this.nudNotOlderThan);
            base.Name = "AdvancedFilterControl";
            this.nudSizeFrom.EndInit();
            this.nudSizeTo.EndInit();
            this.nudNotOlderThan.EndInit();
            this.tblBack.ResumeLayout(false);
            this.tblBack.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void nudNotOlderThan_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateCulture();
            base.RaiseFilterChanged();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
        }

        public void SetFilter(IVirtualItemFilter filter)
        {
            base.CanRaiseFilterChanged = false;
            try
            {
                VirtualItemDateFilter filter2 = filter as VirtualItemDateFilter;
                if (filter2 != null)
                {
                    this.cmbDate.SelectedIndex = (int) filter2.DatePart;
                    this.cmbDateOperation.SelectedIndex = (int) filter2.DateComparision;
                    this.dtpDateFrom.Value = filter2.FromDate;
                    this.dtpDateTo.Value = filter2.ToDate;
                    this.nudNotOlderThan.Value = filter2.NotOlderThan;
                    this.cmbDateMeasure.SelectedIndex = (int) filter2.DateMeasure;
                }
                else
                {
                    VirtualItemTimeFilter filter3 = filter as VirtualItemTimeFilter;
                    if (filter3 != null)
                    {
                        this.cmbTime.SelectedIndex = (int) filter3.TimePart;
                        this.cmbTimeOperation.SelectedIndex = (int) filter3.TimeComparision;
                        DateTime date = DateTime.Now.Date;
                        this.dtpTimeFrom.Value = date.AddTicks(filter3.FromTime.Ticks);
                        this.dtpTimeTo.Value = date.AddTicks(filter3.ToTime.Ticks);
                    }
                    else
                    {
                        Nomad.Commons.SizeFilter filter4 = filter as Nomad.Commons.SizeFilter;
                        if (filter4 != null)
                        {
                            this.cmbSize.SelectedIndex = (int) filter4.SizeUnit;
                            this.cmbSizeOperation.SelectedIndex = (int) filter4.SizeComparision;
                            this.nudSizeFrom.Value = filter4.FromValue;
                            this.nudSizeTo.Value = filter4.ToValue;
                        }
                    }
                }
            }
            finally
            {
                base.CanRaiseFilterChanged = true;
            }
        }

        private void UpdateControls(int selectedIndex, Control from, Control to, Label label)
        {
            this.tblBack.SuspendLayout();
            from.Enabled = selectedIndex > 0;
            if ((selectedIndex == 4) || (selectedIndex == 5))
            {
                this.tblBack.SetColumnSpan(from, 1);
                label.Parent = this.tblBack;
                to.Parent = this.tblBack;
            }
            else
            {
                label.Parent = null;
                to.Parent = null;
                this.tblBack.SetColumnSpan(from, 3);
            }
            this.tblBack.ResumeLayout();
        }

        public void UpdateCulture()
        {
            int num = (int) this.nudNotOlderThan.Value;
            ResourceManager manager = new SettingsManager.LocalizedResourceManager(typeof(AdvancedFilterControl));
            this.cmbDateMeasure.Items[0] = PluralInfo.Format(manager.GetString("cmbDateMeasure.Items"), new object[] { num });
            for (int i = 1; i < this.cmbDateMeasure.Items.Count; i++)
            {
                this.cmbDateMeasure.Items[i] = PluralInfo.Format(manager.GetString("cmbDateMeasure.Items" + i.ToString()), new object[] { num });
            }
        }

        [Browsable(false)]
        public IVirtualItemFilter DateFilter
        {
            get
            {
                if (this.cmbDateOperation.SelectedIndex == 0)
                {
                    return null;
                }
                return new VirtualItemDateFilter { DatePart = this.cmbDate.SelectedIndex, DateComparision = this.cmbDateOperation.SelectedIndex, FromDate = this.dtpDateFrom.Value, ToDate = this.dtpDateTo.Value, NotOlderThan = (int) this.nudNotOlderThan.Value, DateMeasure = this.cmbDateMeasure.SelectedIndex };
            }
        }

        [Browsable(false), DefaultValue((string) null)]
        public IVirtualItemFilter Filter
        {
            get
            {
                List<IVirtualItemFilter> filters = new List<IVirtualItemFilter>();
                IVirtualItemFilter dateFilter = this.DateFilter;
                if (dateFilter != null)
                {
                    filters.Add(dateFilter);
                }
                dateFilter = this.TimeFilter;
                if (dateFilter != null)
                {
                    filters.Add(dateFilter);
                }
                dateFilter = this.SizeFilter;
                if (dateFilter != null)
                {
                    filters.Add(dateFilter);
                }
                if (filters.Count > 0)
                {
                    return new AggregatedVirtualItemFilter(filters);
                }
                return null;
            }
            set
            {
                base.CanRaiseFilterChanged = false;
                try
                {
                    this.Clear();
                    AggregatedVirtualItemFilter filter = value as AggregatedVirtualItemFilter;
                    if ((filter != null) && (filter.Condition == AggregatedFilterCondition.All))
                    {
                        foreach (IVirtualItemFilter filter2 in filter.Filters)
                        {
                            this.SetFilter(filter2);
                        }
                    }
                    else
                    {
                        this.SetFilter(value);
                    }
                }
                finally
                {
                    base.CanRaiseFilterChanged = true;
                }
            }
        }

        [Browsable(false)]
        public IVirtualItemFilter SizeFilter
        {
            get
            {
                if (this.cmbSizeOperation.SelectedIndex == 0)
                {
                    return null;
                }
                return new VirtualItemSizeFilter { SizeUnit = this.cmbSize.SelectedIndex, SizeComparision = this.cmbSizeOperation.SelectedIndex, FromValue = Convert.ToInt64(this.nudSizeFrom.Value), ToValue = Convert.ToInt64(this.nudSizeTo.Value) };
            }
        }

        [Browsable(false)]
        public IVirtualItemFilter TimeFilter
        {
            get
            {
                if (this.cmbTimeOperation.SelectedIndex == 0)
                {
                    return null;
                }
                return new VirtualItemTimeFilter { TimePart = this.cmbTime.SelectedIndex, TimeComparision = this.cmbTimeOperation.SelectedIndex, FromTime = this.dtpTimeFrom.Value.TimeOfDay, ToTime = this.dtpTimeTo.Value.TimeOfDay };
            }
        }
    }
}

