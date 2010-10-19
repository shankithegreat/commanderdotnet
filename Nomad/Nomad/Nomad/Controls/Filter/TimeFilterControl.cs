namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class TimeFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private string[] TimeFormats;
        private ToolStripComboBox tscbTimeFrom;
        private ToolStripComboBox tscbTimeTo;
        private ToolStripDropDownButton tsddTime;
        private ToolStripDropDownButton tsddTimeOperation;
        private ToolStripLabel tslAnd;
        private ToolStripLabel tslIs;
        private ToolStripMenuItem tsmiCreationTime;
        private ToolStripMenuItem tsmiLastAccessTime;
        private ToolStripMenuItem tsmiLastWriteTime;
        private ToolStripMenuItem tsmiOperationAfter;
        private ToolStripMenuItem tsmiOperationAt;
        private ToolStripMenuItem tsmiOperationBefore;
        private ToolStripMenuItem tsmiOperationBetween;
        private ToolStripMenuItem tsmiOperationHourOf1;
        private ToolStripMenuItem tsmiOperationHoursOf6;
        private ToolStripMenuItem tsmiOperationNotBetween;
        private ToolStrip tsTime;

        public TimeFilterControl()
        {
            this.TimeFormats = new string[] { "t", "T", "HH", "HH:m", "HH:m:s" };
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
        }

        public TimeFilterControl(VirtualItemTimeFilter filter)
        {
            this.TimeFormats = new string[] { "t", "T", "HH", "HH:m", "HH:m:s" };
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            base.CanRaiseFilterChanged = false;
            CustomFilterControl.PerformDropDownClick(this.tsddTime, filter.TimePart);
            CustomFilterControl.PerformDropDownClick(this.tsddTimeOperation, filter.TimeComparision);
            this.tscbTimeFrom.Tag = filter.FromTime;
            this.tscbTimeFrom.Text = new DateTime(filter.FromTime.Ticks).ToShortTimeString();
            this.tscbTimeTo.Tag = filter.ToTime;
            this.tscbTimeTo.Text = new DateTime(filter.ToTime.Ticks).ToShortTimeString();
            base.CanRaiseFilterChanged = true;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            DateTime now = DateTime.Now;
            this.tscbTimeFrom.Tag = now.TimeOfDay;
            this.tscbTimeFrom.Text = now.ToShortTimeString();
            this.tscbTimeTo.Tag = this.tscbTimeFrom.Tag;
            this.tscbTimeTo.Text = this.tscbTimeFrom.Text;
            this.tsmiLastWriteTime.PerformClick();
            this.tsmiOperationAt.PerformClick();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TimeFilterControl));
            this.tsTime = new ToolStrip();
            this.tsddTime = new ToolStripDropDownButton();
            this.tsmiLastAccessTime = new ToolStripMenuItem();
            this.tsmiCreationTime = new ToolStripMenuItem();
            this.tsmiLastWriteTime = new ToolStripMenuItem();
            this.tslIs = new ToolStripLabel();
            this.tsddTimeOperation = new ToolStripDropDownButton();
            this.tsmiOperationAt = new ToolStripMenuItem();
            this.tsmiOperationBefore = new ToolStripMenuItem();
            this.tsmiOperationAfter = new ToolStripMenuItem();
            this.tsmiOperationBetween = new ToolStripMenuItem();
            this.tsmiOperationNotBetween = new ToolStripMenuItem();
            this.tsmiOperationHourOf1 = new ToolStripMenuItem();
            this.tsmiOperationHoursOf6 = new ToolStripMenuItem();
            this.tscbTimeFrom = new ToolStripComboBox();
            this.tslAnd = new ToolStripLabel();
            this.tscbTimeTo = new ToolStripComboBox();
            this.tsTime.SuspendLayout();
            base.SuspendLayout();
            this.tsTime.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsTime, "tsTime");
            this.tsTime.GripStyle = ToolStripGripStyle.Hidden;
            this.tsTime.Items.AddRange(new ToolStripItem[] { this.tsddTime, this.tslIs, this.tsddTimeOperation, this.tscbTimeFrom, this.tslAnd, this.tscbTimeTo });
            this.tsTime.Name = "tsTime";
            this.tsddTime.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddTime.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiLastAccessTime, this.tsmiCreationTime, this.tsmiLastWriteTime });
            manager.ApplyResources(this.tsddTime, "tsddTime");
            this.tsddTime.Name = "tsddTime";
            this.tsmiLastAccessTime.Name = "tsmiLastAccessTime";
            manager.ApplyResources(this.tsmiLastAccessTime, "tsmiLastAccessTime");
            this.tsmiLastAccessTime.Click += new EventHandler(this.tsmiLastAccessTime_Click);
            this.tsmiCreationTime.Name = "tsmiCreationTime";
            manager.ApplyResources(this.tsmiCreationTime, "tsmiCreationTime");
            this.tsmiCreationTime.Click += new EventHandler(this.tsmiLastAccessTime_Click);
            this.tsmiLastWriteTime.Name = "tsmiLastWriteTime";
            manager.ApplyResources(this.tsmiLastWriteTime, "tsmiLastWriteTime");
            this.tsmiLastWriteTime.Click += new EventHandler(this.tsmiLastAccessTime_Click);
            this.tslIs.Name = "tslIs";
            manager.ApplyResources(this.tslIs, "tslIs");
            this.tsddTimeOperation.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddTimeOperation.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOperationAt, this.tsmiOperationBefore, this.tsmiOperationAfter, this.tsmiOperationBetween, this.tsmiOperationNotBetween, this.tsmiOperationHourOf1, this.tsmiOperationHoursOf6 });
            manager.ApplyResources(this.tsddTimeOperation, "tsddTimeOperation");
            this.tsddTimeOperation.Name = "tsddTimeOperation";
            this.tsmiOperationAt.Name = "tsmiOperationAt";
            manager.ApplyResources(this.tsmiOperationAt, "tsmiOperationAt");
            this.tsmiOperationAt.Click += new EventHandler(this.tsmiOperationAt_Click);
            this.tsmiOperationBefore.Name = "tsmiOperationBefore";
            manager.ApplyResources(this.tsmiOperationBefore, "tsmiOperationBefore");
            this.tsmiOperationBefore.Click += new EventHandler(this.tsmiOperationAt_Click);
            this.tsmiOperationAfter.Name = "tsmiOperationAfter";
            manager.ApplyResources(this.tsmiOperationAfter, "tsmiOperationAfter");
            this.tsmiOperationAfter.Click += new EventHandler(this.tsmiOperationAt_Click);
            this.tsmiOperationBetween.Name = "tsmiOperationBetween";
            manager.ApplyResources(this.tsmiOperationBetween, "tsmiOperationBetween");
            this.tsmiOperationBetween.Click += new EventHandler(this.tsmiOperationBetween_Click);
            this.tsmiOperationNotBetween.Name = "tsmiOperationNotBetween";
            manager.ApplyResources(this.tsmiOperationNotBetween, "tsmiOperationNotBetween");
            this.tsmiOperationNotBetween.Click += new EventHandler(this.tsmiOperationBetween_Click);
            this.tsmiOperationHourOf1.Name = "tsmiOperationHourOf1";
            manager.ApplyResources(this.tsmiOperationHourOf1, "tsmiOperationHourOf1");
            this.tsmiOperationHourOf1.Click += new EventHandler(this.tsmiOperationAt_Click);
            this.tsmiOperationHoursOf6.Name = "tsmiOperationHoursOf6";
            manager.ApplyResources(this.tsmiOperationHoursOf6, "tsmiOperationHoursOf6");
            this.tsmiOperationHoursOf6.Click += new EventHandler(this.tsmiOperationAt_Click);
            this.tscbTimeFrom.AutoCompleteMode = AutoCompleteMode.Append;
            this.tscbTimeFrom.AutoCompleteSource = AutoCompleteSource.ListItems;
            manager.ApplyResources(this.tscbTimeFrom, "tscbTimeFrom");
            this.tscbTimeFrom.Name = "tscbTimeFrom";
            this.tscbTimeFrom.Validating += new CancelEventHandler(this.tscbTimeFrom_Validating);
            this.tscbTimeFrom.Validated += new EventHandler(this.tscbTimeFrom_Validated);
            this.tslAnd.Name = "tslAnd";
            manager.ApplyResources(this.tslAnd, "tslAnd");
            this.tscbTimeTo.AutoCompleteMode = AutoCompleteMode.Append;
            this.tscbTimeTo.AutoCompleteSource = AutoCompleteSource.ListItems;
            manager.ApplyResources(this.tscbTimeTo, "tscbTimeTo");
            this.tscbTimeTo.Name = "tscbTimeTo";
            this.tscbTimeTo.Validating += new CancelEventHandler(this.tscbTimeFrom_Validating);
            this.tscbTimeTo.Validated += new EventHandler(this.tscbTimeFrom_Validated);
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tsTime);
            base.Name = "TimeFilterControl";
            this.tsTime.ResumeLayout(false);
            this.tsTime.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolStripItems()
        {
            this.tsmiLastAccessTime.Tag = ItemDateTimePart.LastAccessTime;
            this.tsmiCreationTime.Tag = ItemDateTimePart.CreationTime;
            this.tsmiLastWriteTime.Tag = ItemDateTimePart.LastWriteTime;
            this.tsmiOperationAt.Tag = TimeComparision.At;
            this.tsmiOperationBefore.Tag = TimeComparision.Before;
            this.tsmiOperationAfter.Tag = TimeComparision.After;
            this.tsmiOperationBetween.Tag = TimeComparision.Between;
            this.tsmiOperationNotBetween.Tag = TimeComparision.NotBetween;
            this.tsmiOperationHourOf1.Tag = TimeComparision.HoursOf1;
            this.tsmiOperationHoursOf6.Tag = TimeComparision.HoursOf6;
            this.tsddTime.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
            this.tsddTimeOperation.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
            DateTime now = DateTime.Now;
            for (int i = 0; i < 0x30; i++)
            {
                DateTime time2 = new DateTime(now.Year, now.Month, now.Day, i / 2, (i % 2) * 30, 0);
                this.tscbTimeFrom.Items.Add(time2.ToShortTimeString());
            }
            foreach (string str in this.tscbTimeFrom.Items)
            {
                this.tscbTimeTo.Items.Add(str);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsTime.Font = this.Font;
            this.tscbTimeFrom.Font = this.Font;
            this.tscbTimeTo.Font = this.Font;
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (((specified & BoundsSpecified.Size) > BoundsSpecified.None) && ((factor.Width != 1.0) || (factor.Height != 1.0)))
            {
                this.tscbTimeTo.ComboBox.Scale(factor);
            }
        }

        private void tscbTimeFrom_Validated(object sender, EventArgs e)
        {
            ToolStripComboBox box = (ToolStripComboBox) sender;
            DateTime time = DateTime.ParseExact(box.Text, this.TimeFormats, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowLeadingWhite);
            if (time.Second > 0)
            {
                box.Text = time.ToLongTimeString();
            }
            else
            {
                box.Text = time.ToShortTimeString();
            }
            TimeSpan timeOfDay = time.TimeOfDay;
            if (!timeOfDay.Equals(box.Tag))
            {
                box.Tag = timeOfDay;
                base.RaiseFilterChanged();
            }
        }

        private void tscbTimeFrom_Validating(object sender, CancelEventArgs e)
        {
            DateTime time;
            e.Cancel = !DateTime.TryParseExact(((ToolStripComboBox) sender).Text, this.TimeFormats, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowLeadingWhite, out time);
        }

        private void tsmiLastAccessTime_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddTime, (ToolStripItem) sender);
        }

        private void tsmiOperationAt_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddTimeOperation, (ToolStripItem) sender);
            this.tslAnd.Visible = false;
            this.tscbTimeTo.Visible = false;
        }

        private void tsmiOperationBetween_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddTimeOperation, (ToolStripItem) sender);
            this.tslAnd.Visible = true;
            this.tscbTimeTo.Visible = true;
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddTime, this.tsddTime.Tag);
            base.UpdateDropDownText(this.tsddTimeOperation, this.tsddTimeOperation.Tag);
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                return new VirtualItemTimeFilter { TimePart = (ItemDateTimePart) this.tsddTime.Tag, TimeComparision = (TimeComparision) this.tsddTimeOperation.Tag, FromTime = (TimeSpan) this.tscbTimeFrom.Tag, ToTime = (TimeSpan) this.tscbTimeTo.Tag };
            }
        }

        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsTime;
            }
        }
    }
}

