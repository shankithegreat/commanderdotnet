namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class DateFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private string[] DateFormats;
        private ToolStripComboBox tscbDateFrom;
        private ToolStripComboBox tscbDateTo;
        private ToolStrip tsDate;
        private ToolStripDropDownButton tsddDate;
        private ToolStripDropDownButton tsddDateOperation;
        private ToolStripDropDownButton tsddDateUnit;
        private ToolStripLabel tslAnd;
        private ToolStripLabel tslField;
        private ToolStripLabel tslIs;
        private ToolStripLabel tslPropertyName;
        private ToolStripMenuItem tsmiCreationDate;
        private ToolStripMenuItem tsmiLastAccessDate;
        private ToolStripMenuItem tsmiLastWriteDate;
        private ToolStripMenuItem tsmiOperationAfter;
        private ToolStripMenuItem tsmiOperationBefore;
        private ToolStripMenuItem tsmiOperationBetween;
        private ToolStripMenuItem tsmiOperationNotBetween;
        private ToolStripMenuItem tsmiOperationNotOlderThan;
        private ToolStripMenuItem tsmiOperationOn;
        private ToolStripMenuItem tsmiUnitDay;
        private ToolStripMenuItem tsmiUnitMonth;
        private ToolStripMenuItem tsmiUnitWeek;
        private ToolStripMenuItem tsmiUnitYear;
        private ToolStripTextBox tstbNotOlderThan;

        public DateFilterControl()
        {
            this.DateFormats = new string[] { "d", "D", "MM", "dd", "yy", "yyyy", "d/M", "ddd" };
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
        }

        public DateFilterControl(VirtualItemDateFilter filter)
        {
            this.DateFormats = new string[] { "d", "D", "MM", "dd", "yy", "yyyy", "d/M", "ddd" };
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            base.CanRaiseFilterChanged = false;
            CustomFilterControl.PerformDropDownClick(this.tsddDate, filter.DatePart);
            this.SetFilter(filter);
            base.CanRaiseFilterChanged = true;
        }

        public DateFilterControl(VirtualPropertyFilter filter)
        {
            this.DateFormats = new string[] { "d", "D", "MM", "dd", "yy", "yyyy", "d/M", "ddd" };
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.SetProperty(filter.PropertyId);
            this.SetFilter((DateFilter) filter.Filter);
        }

        public DateFilterControl(int propertyId)
        {
            this.DateFormats = new string[] { "d", "D", "MM", "dd", "yy", "yyyy", "d/M", "ddd" };
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
            this.SetProperty(propertyId);
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            DateTime now = DateTime.Now;
            this.tscbDateFrom.Tag = now;
            this.tscbDateFrom.Text = now.ToShortDateString();
            this.tscbDateTo.Tag = this.tscbDateFrom.Tag;
            this.tscbDateTo.Text = this.tscbDateFrom.Text;
            this.tstbNotOlderThan.Tag = 1;
            this.tstbNotOlderThan.Text = this.tstbNotOlderThan.Tag.ToString();
            this.tsmiLastWriteDate.PerformClick();
            this.tsmiOperationOn.PerformClick();
            this.tsmiUnitDay.PerformClick();
            base.CanRaiseFilterChanged = true;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DateFilterControl));
            this.tsDate = new ToolStrip();
            this.tslField = new ToolStripLabel();
            this.tslPropertyName = new ToolStripLabel();
            this.tsddDate = new ToolStripDropDownButton();
            this.tsmiLastAccessDate = new ToolStripMenuItem();
            this.tsmiCreationDate = new ToolStripMenuItem();
            this.tsmiLastWriteDate = new ToolStripMenuItem();
            this.tslIs = new ToolStripLabel();
            this.tsddDateOperation = new ToolStripDropDownButton();
            this.tsmiOperationOn = new ToolStripMenuItem();
            this.tsmiOperationBefore = new ToolStripMenuItem();
            this.tsmiOperationAfter = new ToolStripMenuItem();
            this.tsmiOperationBetween = new ToolStripMenuItem();
            this.tsmiOperationNotBetween = new ToolStripMenuItem();
            this.tsmiOperationNotOlderThan = new ToolStripMenuItem();
            this.tscbDateFrom = new ToolStripComboBox();
            this.tslAnd = new ToolStripLabel();
            this.tscbDateTo = new ToolStripComboBox();
            this.tstbNotOlderThan = new ToolStripTextBox();
            this.tsddDateUnit = new ToolStripDropDownButton();
            this.tsmiUnitDay = new ToolStripMenuItem();
            this.tsmiUnitWeek = new ToolStripMenuItem();
            this.tsmiUnitMonth = new ToolStripMenuItem();
            this.tsmiUnitYear = new ToolStripMenuItem();
            this.tsDate.SuspendLayout();
            base.SuspendLayout();
            this.tsDate.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsDate, "tsDate");
            this.tsDate.GripStyle = ToolStripGripStyle.Hidden;
            this.tsDate.Items.AddRange(new ToolStripItem[] { this.tslField, this.tslPropertyName, this.tsddDate, this.tslIs, this.tsddDateOperation, this.tscbDateFrom, this.tslAnd, this.tscbDateTo, this.tstbNotOlderThan, this.tsddDateUnit });
            this.tsDate.Name = "tsDate";
            this.tslField.Name = "tslField";
            manager.ApplyResources(this.tslField, "tslField");
            this.tslPropertyName.Name = "tslPropertyName";
            manager.ApplyResources(this.tslPropertyName, "tslPropertyName");
            this.tsddDate.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddDate.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiLastAccessDate, this.tsmiCreationDate, this.tsmiLastWriteDate });
            this.tsddDate.Name = "tsddDate";
            manager.ApplyResources(this.tsddDate, "tsddDate");
            this.tsmiLastAccessDate.Name = "tsmiLastAccessDate";
            manager.ApplyResources(this.tsmiLastAccessDate, "tsmiLastAccessDate");
            this.tsmiLastAccessDate.Click += new EventHandler(this.tsmiLastAccessDate_Click);
            this.tsmiCreationDate.Name = "tsmiCreationDate";
            manager.ApplyResources(this.tsmiCreationDate, "tsmiCreationDate");
            this.tsmiCreationDate.Click += new EventHandler(this.tsmiLastAccessDate_Click);
            this.tsmiLastWriteDate.Name = "tsmiLastWriteDate";
            manager.ApplyResources(this.tsmiLastWriteDate, "tsmiLastWriteDate");
            this.tsmiLastWriteDate.Click += new EventHandler(this.tsmiLastAccessDate_Click);
            this.tslIs.Name = "tslIs";
            manager.ApplyResources(this.tslIs, "tslIs");
            this.tsddDateOperation.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddDateOperation.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOperationOn, this.tsmiOperationBefore, this.tsmiOperationAfter, this.tsmiOperationBetween, this.tsmiOperationNotBetween, this.tsmiOperationNotOlderThan });
            this.tsddDateOperation.Name = "tsddDateOperation";
            manager.ApplyResources(this.tsddDateOperation, "tsddDateOperation");
            this.tsmiOperationOn.Name = "tsmiOperationOn";
            manager.ApplyResources(this.tsmiOperationOn, "tsmiOperationOn");
            this.tsmiOperationOn.Click += new EventHandler(this.tsmiOperationOn_Click);
            this.tsmiOperationBefore.Name = "tsmiOperationBefore";
            manager.ApplyResources(this.tsmiOperationBefore, "tsmiOperationBefore");
            this.tsmiOperationBefore.Click += new EventHandler(this.tsmiOperationOn_Click);
            this.tsmiOperationAfter.Name = "tsmiOperationAfter";
            manager.ApplyResources(this.tsmiOperationAfter, "tsmiOperationAfter");
            this.tsmiOperationAfter.Click += new EventHandler(this.tsmiOperationOn_Click);
            this.tsmiOperationBetween.Name = "tsmiOperationBetween";
            manager.ApplyResources(this.tsmiOperationBetween, "tsmiOperationBetween");
            this.tsmiOperationBetween.Click += new EventHandler(this.tsmiOperationBetween_Click);
            this.tsmiOperationNotBetween.Name = "tsmiOperationNotBetween";
            manager.ApplyResources(this.tsmiOperationNotBetween, "tsmiOperationNotBetween");
            this.tsmiOperationNotBetween.Click += new EventHandler(this.tsmiOperationBetween_Click);
            this.tsmiOperationNotOlderThan.Name = "tsmiOperationNotOlderThan";
            manager.ApplyResources(this.tsmiOperationNotOlderThan, "tsmiOperationNotOlderThan");
            this.tsmiOperationNotOlderThan.Click += new EventHandler(this.tsmiOperationNotOlderThan_Click);
            manager.ApplyResources(this.tscbDateFrom, "tscbDateFrom");
            this.tscbDateFrom.Items.AddRange(new object[] { manager.GetString("tscbDateFrom.Items"), manager.GetString("tscbDateFrom.Items1"), manager.GetString("tscbDateFrom.Items2"), manager.GetString("tscbDateFrom.Items3"), manager.GetString("tscbDateFrom.Items4"), manager.GetString("tscbDateFrom.Items5"), manager.GetString("tscbDateFrom.Items6") });
            this.tscbDateFrom.Name = "tscbDateFrom";
            this.tscbDateFrom.Validated += new EventHandler(this.tscbDateFrom_Validated);
            this.tslAnd.Name = "tslAnd";
            manager.ApplyResources(this.tslAnd, "tslAnd");
            manager.ApplyResources(this.tscbDateTo, "tscbDateTo");
            this.tscbDateTo.Items.AddRange(new object[] { manager.GetString("tscbDateTo.Items"), manager.GetString("tscbDateTo.Items1"), manager.GetString("tscbDateTo.Items2"), manager.GetString("tscbDateTo.Items3"), manager.GetString("tscbDateTo.Items4"), manager.GetString("tscbDateTo.Items5"), manager.GetString("tscbDateTo.Items6") });
            this.tscbDateTo.Name = "tscbDateTo";
            this.tstbNotOlderThan.BorderStyle = BorderStyle.FixedSingle;
            manager.ApplyResources(this.tstbNotOlderThan, "tstbNotOlderThan");
            this.tstbNotOlderThan.Name = "tstbNotOlderThan";
            this.tstbNotOlderThan.KeyPress += new KeyPressEventHandler(this.tstbNotOlderThan_KeyPress);
            this.tstbNotOlderThan.TextChanged += new EventHandler(this.tstbNotOlderThan_TextChanged);
            this.tsddDateUnit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddDateUnit.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiUnitDay, this.tsmiUnitWeek, this.tsmiUnitMonth, this.tsmiUnitYear });
            this.tsddDateUnit.Name = "tsddDateUnit";
            manager.ApplyResources(this.tsddDateUnit, "tsddDateUnit");
            this.tsmiUnitDay.Name = "tsmiUnitDay";
            manager.ApplyResources(this.tsmiUnitDay, "tsmiUnitDay");
            this.tsmiUnitDay.Click += new EventHandler(this.tsmiUnitDay_Click);
            this.tsmiUnitWeek.Name = "tsmiUnitWeek";
            manager.ApplyResources(this.tsmiUnitWeek, "tsmiUnitWeek");
            this.tsmiUnitWeek.Click += new EventHandler(this.tsmiUnitDay_Click);
            this.tsmiUnitMonth.Name = "tsmiUnitMonth";
            manager.ApplyResources(this.tsmiUnitMonth, "tsmiUnitMonth");
            this.tsmiUnitMonth.Click += new EventHandler(this.tsmiUnitDay_Click);
            this.tsmiUnitYear.Name = "tsmiUnitYear";
            manager.ApplyResources(this.tsmiUnitYear, "tsmiUnitYear");
            this.tsmiUnitYear.Click += new EventHandler(this.tsmiUnitDay_Click);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsDate);
            base.Name = "DateFilterControl";
            this.tsDate.ResumeLayout(false);
            this.tsDate.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolStripItems()
        {
            this.tsmiLastAccessDate.Tag = ItemDateTimePart.LastAccessTime;
            this.tsmiCreationDate.Tag = ItemDateTimePart.CreationTime;
            this.tsmiLastWriteDate.Tag = ItemDateTimePart.LastWriteTime;
            this.tsmiOperationOn.Tag = DateComparision.On;
            this.tsmiOperationBefore.Tag = DateComparision.Before;
            this.tsmiOperationAfter.Tag = DateComparision.After;
            this.tsmiOperationBetween.Tag = DateComparision.Between;
            this.tsmiOperationNotBetween.Tag = DateComparision.NotBetween;
            this.tsmiOperationNotOlderThan.Tag = DateComparision.NotOlderThan;
            this.tsmiUnitDay.Tag = DateUnit.Day;
            this.tsmiUnitWeek.Tag = DateUnit.Week;
            this.tsmiUnitMonth.Tag = DateUnit.Month;
            this.tsmiUnitYear.Tag = DateUnit.Year;
            this.tsddDate.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
            this.tsddDateOperation.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
            this.tsddDateUnit.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsDate.Font = this.Font;
            this.tscbDateFrom.Font = this.Font;
            this.tscbDateTo.Font = this.Font;
            this.tstbNotOlderThan.Font = this.Font;
        }

        private bool ParseDateString(string s, out DateTime date)
        {
            int num;
            if (string.Equals(s, "today"))
            {
                date = DateTime.Now;
                return true;
            }
            if (int.TryParse(s, out num))
            {
                if ((num <= 0) || s.StartsWith("+", StringComparison.Ordinal))
                {
                    date = DateTime.Now.AddDays((double) num);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    date = new DateTime(now.Year, num, now.Day);
                }
                return true;
            }
            return DateTime.TryParseExact(s, this.DateFormats, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowLeadingWhite, out date);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Size) > BoundsSpecified.None) && ((factor.Width != 1.0) || (factor.Height != 1.0)))
            {
                this.tscbDateTo.ComboBox.Scale(factor);
                this.tstbNotOlderThan.TextBox.Scale(factor);
            }
        }

        private void SetFilter(DateFilter filter)
        {
            base.CanRaiseFilterChanged = false;
            try
            {
                CustomFilterControl.PerformDropDownClick(this.tsddDateOperation, filter.DateComparision);
                this.tscbDateFrom.Tag = filter.FromDate;
                this.tscbDateFrom.Text = filter.FromDate.ToShortDateString();
                this.tscbDateTo.Tag = filter.ToDate;
                this.tscbDateTo.Text = filter.ToDate.ToShortDateString();
                this.tstbNotOlderThan.Tag = filter.NotOlderThan;
                this.tstbNotOlderThan.Text = filter.NotOlderThan.ToString();
                CustomFilterControl.PerformDropDownClick(this.tsddDateUnit, filter.DateMeasure);
            }
            finally
            {
                base.CanRaiseFilterChanged = true;
            }
        }

        private void SetProperty(int propertyId)
        {
            VirtualProperty property = VirtualProperty.Get(propertyId);
            if (property == null)
            {
                throw new ArgumentOutOfRangeException("Invalid propertyId");
            }
            this.tslPropertyName.Tag = property.PropertyId;
            this.tslPropertyName.Text = property.LocalizedName;
            this.tslField.Visible = true;
            this.tslPropertyName.Visible = true;
            this.tsddDate.Visible = false;
        }

        private void tscbDateFrom_Validated(object sender, EventArgs e)
        {
            ToolStripComboBox box = (ToolStripComboBox) sender;
            bool flag = string.IsNullOrEmpty(box.Text);
            DateTime now = DateTime.Now;
            if (flag || this.ParseDateString(box.Text, out now))
            {
                if (!flag)
                {
                    box.Text = now.ToShortDateString();
                    if (!now.Equals(box.Tag))
                    {
                        box.Tag = now;
                        base.RaiseFilterChanged();
                    }
                }
                box.ResetBackColor();
                box.ResetForeColor();
            }
            else
            {
                box.BackColor = Settings.TextBoxError;
                box.ForeColor = SystemColors.HighlightText;
            }
        }

        private void tsmiLastAccessDate_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddDate, (ToolStripItem) sender);
        }

        private void tsmiOperationBetween_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddDateOperation, (ToolStripItem) sender);
            this.tscbDateFrom.Visible = true;
            this.tslAnd.Visible = true;
            this.tscbDateTo.Visible = true;
            this.tstbNotOlderThan.Visible = false;
            this.tsddDateUnit.Visible = false;
        }

        private void tsmiOperationNotOlderThan_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddDateOperation, (ToolStripItem) sender);
            this.tscbDateFrom.Visible = false;
            this.tslAnd.Visible = false;
            this.tscbDateTo.Visible = false;
            this.tstbNotOlderThan.Visible = true;
            this.tsddDateUnit.Visible = true;
        }

        private void tsmiOperationOn_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddDateOperation, (ToolStripItem) sender);
            this.tscbDateFrom.Visible = true;
            this.tslAnd.Visible = false;
            this.tscbDateTo.Visible = false;
            this.tstbNotOlderThan.Visible = false;
            this.tsddDateUnit.Visible = false;
        }

        private void tsmiUnitDay_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddDateUnit, (ToolStripItem) sender);
        }

        private void tstbNotOlderThan_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar) && (e.KeyChar != '\b');
        }

        private void tstbNotOlderThan_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox box = (ToolStripTextBox) sender;
            int result = 0;
            if (string.IsNullOrEmpty(box.Text) || (int.TryParse(box.Text, out result) && (result > 0)))
            {
                if (!result.Equals(box.Tag))
                {
                    box.Tag = result;
                    base.RaiseFilterChanged();
                }
                box.ResetBackColor();
                box.ResetForeColor();
            }
            else
            {
                box.BackColor = Settings.TextBoxError;
                box.ForeColor = SystemColors.HighlightText;
            }
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddDate, this.tsddDate.Tag);
            base.UpdateDropDownText(this.tsddDateOperation, this.tsddDateOperation.Tag);
            base.UpdateDropDownText(this.tsddDateUnit, this.tsddDateUnit.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                DateFilter filter;
                if (this.tslPropertyName.Visible)
                {
                    filter = new DateFilter();
                }
                else
                {
                    filter = new VirtualItemDateFilter();
                    ((VirtualItemDateFilter) filter).DatePart = (ItemDateTimePart) this.tsddDate.Tag;
                }
                filter.DateComparision = (DateComparision) this.tsddDateOperation.Tag;
                filter.FromDate = (DateTime) this.tscbDateFrom.Tag;
                filter.ToDate = (DateTime) this.tscbDateTo.Tag;
                filter.NotOlderThan = (int) this.tstbNotOlderThan.Tag;
                filter.DateMeasure = (DateUnit) this.tsddDateUnit.Tag;
                if (this.tslPropertyName.Visible)
                {
                    return new VirtualPropertyFilter((int) this.tslPropertyName.Tag, filter);
                }
                return (VirtualItemDateFilter) filter;
            }
        }

        public bool IsEmpty
        {
            get
            {
                DateTime time;
                switch (((DateComparision) this.tsddDateOperation.Tag))
                {
                    case DateComparision.Between:
                    case DateComparision.NotBetween:
                        return (!this.ParseDateString(this.tscbDateFrom.Text, out time) || !this.ParseDateString(this.tscbDateTo.Text, out time));

                    case DateComparision.NotOlderThan:
                        int num;
                        return (!int.TryParse(this.tstbNotOlderThan.Text, out num) || (num < 1));
                }
                return !this.ParseDateString(this.tscbDateFrom.Text, out time);
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsDate;
            }
        }
    }
}

