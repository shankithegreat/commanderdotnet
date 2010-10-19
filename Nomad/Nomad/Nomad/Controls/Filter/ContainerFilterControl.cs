namespace Nomad.Controls.Filter
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Resources;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ContainerFilterControl : CustomFilterControl, IFilterControl, IUpdateCulture
    {
        private IContainer components;
        private FlowLayoutPanel flpFilters;
        private static Dictionary<System.Type, System.Type> FPropertyFilterMap;
        private ImageList imgPropertyTag;
        private ToolStripDropDownButton tsddAddCondition;
        private ToolStripDropDownButton tsddCondition;
        private ToolStripLabel tslApplyWith;
        private ToolStripLabel tslMatch;
        private ToolStrip tsMatch;
        private ToolStripMenuItem tsmiAdditional;
        private ToolStripMenuItem tsmiAttributes;
        private ToolStripMenuItem tsmiConditionAll;
        private ToolStripMenuItem tsmiConditionAny;
        private ToolStripMenuItem tsmiConditionNone;
        private ToolStripMenuItem tsmiContainer;
        private ToolStripMenuItem tsmiContent;
        private ToolStripMenuItem tsmiDate;
        private ToolStripMenuItem tsmiHexContent;
        private ToolStripMenuItem tsmiName;
        private ToolStripMenuItem tsmiSize;
        private ToolStripMenuItem tsmiTime;
        private ToolStrip tsNewCondition;
        private ToolStripSeparator tssFilter1;
        private ToolStripSeparator tssFilter2;

        public ContainerFilterControl()
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.Clear();
        }

        public ContainerFilterControl(AggregatedVirtualItemFilter filter)
        {
            this.components = null;
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.InitializeComponent();
            this.InitializeToolStripItems();
            this.InitializeContainerFilterControl(filter);
        }

        private void AddFilterControl(Control filterControl)
        {
            filterControl.SuspendLayout();
            UserControl userControl = filterControl as UserControl;
            if (userControl != null)
            {
                BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
                if (argument != null)
                {
                    argument.Localize(userControl);
                }
            }
            filterControl.Margin = new Padding(0);
            if (filterControl is IFilterControl)
            {
                this.InitializeFilterControl((IFilterControl) filterControl);
            }
            filterControl.ResumeLayout();
            this.flpFilters.Controls.Add(filterControl);
            this.flpFilters.Controls.SetChildIndex(this.tsNewCondition, this.flpFilters.Controls.Count);
            this.tsNewCondition.TabIndex = this.flpFilters.Controls.Count - 1;
        }

        public void Clear()
        {
            base.CanRaiseFilterChanged = false;
            try
            {
                this.tsmiConditionAll.PerformClick();
                if (this.flpFilters.Controls.Count > 1)
                {
                    this.flpFilters.SuspendLayout();
                    IContainer container = new Container();
                    foreach (Control control in this.flpFilters.Controls)
                    {
                        if (control != this.tsNewCondition)
                        {
                            container.Add(control);
                        }
                    }
                    this.flpFilters.Controls.Clear();
                    this.flpFilters.Controls.Add(this.tsNewCondition);
                    container.Dispose();
                    this.flpFilters.ResumeLayout();
                }
            }
            finally
            {
                base.CanRaiseFilterChanged = true;
            }
        }

        private void CloseStrip_Click(object sender, EventArgs e)
        {
            Control tag = (Control) ((ToolStripItem) sender).Tag;
            this.flpFilters.Controls.Remove(tag);
            tag.Dispose();
        }

        protected static Control CreateFilterControl(IVirtualItemFilter filter)
        {
            VirtualPropertyFilter filter2 = filter as VirtualPropertyFilter;
            if (filter2 != null)
            {
                System.Type type;
                VirtualProperty property = VirtualProperty.Get(filter2.PropertyId);
                if (PropertyFilterMap.TryGetValue(property.PropertyType, out type))
                {
                    return (Activator.CreateInstance(type, new object[] { filter2 }) as Control);
                }
                return null;
            }
            if (filter is NameFilter)
            {
                return new NameFilterControl((NameFilter) filter);
            }
            if (filter is AttributeFilter)
            {
                return new AttrFilterControl((AttributeFilter) filter);
            }
            if (filter is SizeFilter)
            {
                return new SizeFilterControl((SizeFilter) filter);
            }
            if (filter is VirtualItemDateFilter)
            {
                return new DateFilterControl((VirtualItemDateFilter) filter);
            }
            if (filter is VirtualItemTimeFilter)
            {
                return new TimeFilterControl((VirtualItemTimeFilter) filter);
            }
            if (filter is ContentFilter)
            {
                return new ContentFilterControl((ContentFilter) filter);
            }
            if (filter is HexContentFilter)
            {
                return new HexContentFilterControl((HexContentFilter) filter);
            }
            if (filter is AggregatedVirtualItemFilter)
            {
                return new ContainerFilterControl((AggregatedVirtualItemFilter) filter);
            }
            return null;
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

        private void flpFilters_ControlAdded(object sender, ControlEventArgs e)
        {
            base.RaiseFilterChanged();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ContainerFilterControl));
            this.tsMatch = new ToolStrip();
            this.tslMatch = new ToolStripLabel();
            this.tsddCondition = new ToolStripDropDownButton();
            this.tsmiConditionAll = new ToolStripMenuItem();
            this.tsmiConditionAny = new ToolStripMenuItem();
            this.tsmiConditionNone = new ToolStripMenuItem();
            this.tslApplyWith = new ToolStripLabel();
            this.flpFilters = new FlowLayoutPanel();
            this.tsNewCondition = new ToolStrip();
            this.tsddAddCondition = new ToolStripDropDownButton();
            this.tsmiContainer = new ToolStripMenuItem();
            this.tssFilter1 = new ToolStripSeparator();
            this.tsmiName = new ToolStripMenuItem();
            this.tsmiAttributes = new ToolStripMenuItem();
            this.tsmiDate = new ToolStripMenuItem();
            this.tsmiTime = new ToolStripMenuItem();
            this.tsmiSize = new ToolStripMenuItem();
            this.tsmiContent = new ToolStripMenuItem();
            this.tsmiHexContent = new ToolStripMenuItem();
            this.tssFilter2 = new ToolStripSeparator();
            this.tsmiAdditional = new ToolStripMenuItem();
            this.imgPropertyTag = new ImageList(this.components);
            this.tsMatch.SuspendLayout();
            this.flpFilters.SuspendLayout();
            this.tsNewCondition.SuspendLayout();
            base.SuspendLayout();
            this.tsMatch.BackColor = SystemColors.Window;
            manager.ApplyResources(this.tsMatch, "tsMatch");
            this.tsMatch.GripStyle = ToolStripGripStyle.Hidden;
            this.tsMatch.Items.AddRange(new ToolStripItem[] { this.tslMatch, this.tsddCondition, this.tslApplyWith });
            this.tsMatch.Name = "tsMatch";
            this.tsMatch.TabStop = true;
            this.tsMatch.ItemAdded += new ToolStripItemEventHandler(this.tsMatch_ItemAdded);
            this.tslMatch.Name = "tslMatch";
            manager.ApplyResources(this.tslMatch, "tslMatch");
            this.tsddCondition.AutoToolTip = false;
            this.tsddCondition.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsddCondition.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiConditionAll, this.tsmiConditionAny, this.tsmiConditionNone });
            this.tsddCondition.Name = "tsddCondition";
            manager.ApplyResources(this.tsddCondition, "tsddCondition");
            this.tsmiConditionAll.Name = "tsmiConditionAll";
            manager.ApplyResources(this.tsmiConditionAll, "tsmiConditionAll");
            this.tsmiConditionAll.Click += new EventHandler(this.tsmiConditionAll_Click);
            this.tsmiConditionAny.Name = "tsmiConditionAny";
            manager.ApplyResources(this.tsmiConditionAny, "tsmiConditionAny");
            this.tsmiConditionAny.Click += new EventHandler(this.tsmiConditionAll_Click);
            this.tsmiConditionNone.Name = "tsmiConditionNone";
            manager.ApplyResources(this.tsmiConditionNone, "tsmiConditionNone");
            this.tsmiConditionNone.Click += new EventHandler(this.tsmiConditionAll_Click);
            this.tslApplyWith.Name = "tslApplyWith";
            manager.ApplyResources(this.tslApplyWith, "tslApplyWith");
            this.flpFilters.AllowDrop = true;
            manager.ApplyResources(this.flpFilters, "flpFilters");
            this.flpFilters.Controls.Add(this.tsNewCondition);
            this.flpFilters.Name = "flpFilters";
            this.flpFilters.ControlAdded += new ControlEventHandler(this.flpFilters_ControlAdded);
            this.flpFilters.ControlRemoved += new ControlEventHandler(this.flpFilters_ControlAdded);
            this.tsNewCondition.BackColor = SystemColors.Window;
            manager.ApplyResources(this.tsNewCondition, "tsNewCondition");
            this.tsNewCondition.GripStyle = ToolStripGripStyle.Hidden;
            this.tsNewCondition.Items.AddRange(new ToolStripItem[] { this.tsddAddCondition });
            this.tsNewCondition.Name = "tsNewCondition";
            this.tsNewCondition.TabStop = true;
            this.tsddAddCondition.AutoToolTip = false;
            this.tsddAddCondition.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiContainer, this.tssFilter1, this.tsmiName, this.tsmiAttributes, this.tsmiDate, this.tsmiTime, this.tsmiSize, this.tsmiContent, this.tsmiHexContent, this.tssFilter2, this.tsmiAdditional });
            this.tsddAddCondition.Name = "tsddAddCondition";
            manager.ApplyResources(this.tsddAddCondition, "tsddAddCondition");
            this.tsmiContainer.Name = "tsmiContainer";
            manager.ApplyResources(this.tsmiContainer, "tsmiContainer");
            this.tsmiContainer.Click += new EventHandler(this.tsmiDate_Click);
            this.tssFilter1.Name = "tssFilter1";
            manager.ApplyResources(this.tssFilter1, "tssFilter1");
            this.tsmiName.Name = "tsmiName";
            manager.ApplyResources(this.tsmiName, "tsmiName");
            this.tsmiName.Click += new EventHandler(this.tsmiDate_Click);
            this.tsmiAttributes.Name = "tsmiAttributes";
            manager.ApplyResources(this.tsmiAttributes, "tsmiAttributes");
            this.tsmiAttributes.Click += new EventHandler(this.tsmiDate_Click);
            this.tsmiDate.Name = "tsmiDate";
            manager.ApplyResources(this.tsmiDate, "tsmiDate");
            this.tsmiDate.Click += new EventHandler(this.tsmiDate_Click);
            this.tsmiTime.Name = "tsmiTime";
            manager.ApplyResources(this.tsmiTime, "tsmiTime");
            this.tsmiTime.Click += new EventHandler(this.tsmiDate_Click);
            this.tsmiSize.Name = "tsmiSize";
            manager.ApplyResources(this.tsmiSize, "tsmiSize");
            this.tsmiSize.Click += new EventHandler(this.tsmiDate_Click);
            this.tsmiContent.Name = "tsmiContent";
            manager.ApplyResources(this.tsmiContent, "tsmiContent");
            this.tsmiContent.Click += new EventHandler(this.tsmiDate_Click);
            this.tsmiHexContent.Name = "tsmiHexContent";
            manager.ApplyResources(this.tsmiHexContent, "tsmiHexContent");
            this.tsmiHexContent.Click += new EventHandler(this.tsmiDate_Click);
            this.tssFilter2.Name = "tssFilter2";
            manager.ApplyResources(this.tssFilter2, "tssFilter2");
            this.tsmiAdditional.Name = "tsmiAdditional";
            manager.ApplyResources(this.tsmiAdditional, "tsmiAdditional");
            this.imgPropertyTag.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imgPropertyTag, "imgPropertyTag");
            this.imgPropertyTag.TransparentColor = Color.Transparent;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.flpFilters);
            base.Controls.Add(this.tsMatch);
            base.Name = "ContainerFilterControl";
            this.tsMatch.ResumeLayout(false);
            this.tsMatch.PerformLayout();
            this.flpFilters.ResumeLayout(false);
            this.flpFilters.PerformLayout();
            this.tsNewCondition.ResumeLayout(false);
            this.tsNewCondition.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeContainerFilterControl(AggregatedVirtualItemFilter filter)
        {
            base.CanRaiseFilterChanged = false;
            try
            {
                switch (filter.Condition)
                {
                    case AggregatedFilterCondition.All:
                        this.tsmiConditionAll.PerformClick();
                        break;

                    case AggregatedFilterCondition.Any:
                        this.tsmiConditionAny.PerformClick();
                        break;

                    case AggregatedFilterCondition.None:
                        this.tsmiConditionNone.PerformClick();
                        break;

                    default:
                        throw new InvalidEnumArgumentException();
                }
                this.flpFilters.SuspendLayout();
                foreach (IVirtualItemFilter filter2 in filter.Filters)
                {
                    Control filterControl = CreateFilterControl(filter2);
                    this.AddFilterControl(filterControl);
                }
                this.flpFilters.ResumeLayout();
            }
            finally
            {
                base.CanRaiseFilterChanged = true;
            }
        }

        private void InitializeFilterControl(IFilterControl filterControl)
        {
            filterControl.FilterChanged += new EventHandler(this.FilterControl_FilterChanged);
            ToolStrip topToolStrip = filterControl.TopToolStrip;
            topToolStrip.TabStop = true;
            topToolStrip.Font = this.tsMatch.Font;
            topToolStrip.BackColor = this.tsMatch.BackColor;
            topToolStrip.Renderer = this.tsMatch.Renderer;
            topToolStrip.Items.Insert(0, new ToolStripSeparator());
            ToolStripItem item = new ToolStripButton(Resources.sRemoveCondition, IconSet.GetImage("ContainerFilterControl.RemoveCondition")) {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Tag = filterControl
            };
            item.Click += new EventHandler(this.CloseStrip_Click);
            topToolStrip.Items.Insert(0, item);
        }

        private void InitializeToolStripItems()
        {
            this.tsmiConditionAll.Tag = AggregatedFilterCondition.All;
            this.tsmiConditionAny.Tag = AggregatedFilterCondition.Any;
            this.tsmiConditionNone.Tag = AggregatedFilterCondition.None;
            this.tsmiContainer.Tag = typeof(ContainerFilterControl);
            this.tsmiName.Tag = typeof(NameFilterControl);
            this.tsmiAttributes.Tag = typeof(AttrFilterControl);
            this.tsmiDate.Tag = typeof(DateFilterControl);
            this.tsmiTime.Tag = typeof(TimeFilterControl);
            this.tsmiSize.Tag = typeof(SizeFilterControl);
            this.tsmiContent.Tag = typeof(ContentFilterControl);
            this.tsmiHexContent.Tag = typeof(HexContentFilterControl);
            this.imgPropertyTag.Images.Add(IconSet.GetImage("PropertyType_Other"));
            this.imgPropertyTag.Images.Add(IconSet.GetImage("PropertyType_Integral"));
            this.imgPropertyTag.Images.Add(IconSet.GetImage("PropertyType_String"));
            this.imgPropertyTag.Images.Add(IconSet.GetImage("PropertyType_DateTime"));
            this.imgPropertyTag.Images.Add(IconSet.GetImage("PropertyType_Version"));
            this.tsmiAdditional.DropDown.ImageList = this.imgPropertyTag;
            if (!base.DesignMode)
            {
                VirtualPropertySet set = new VirtualPropertySet(VirtualProperty.Visible);
                set[0] = false;
                set[1] = false;
                set[6] = false;
                set[3] = false;
                set[7] = false;
                set[9] = false;
                set[8] = false;
                set[0x1b] = false;
                set[0x1a] = false;
                int groupId = -1;
                ToolStripMenuItem item = null;
                foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) set)
                {
                    int num2 = -1;
                    switch (System.Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                            num2 = 1;
                            break;

                        case TypeCode.DateTime:
                            num2 = 3;
                            break;

                        case TypeCode.String:
                            num2 = 2;
                            break;

                        default:
                            if (property.PropertyType != typeof(Version))
                            {
                                continue;
                            }
                            num2 = 4;
                            break;
                    }
                    if (property.GroupId != groupId)
                    {
                        item = new ToolStripMenuItem(property.LocalizedGroupName) {
                            DropDown = { ImageList = this.imgPropertyTag }
                        };
                        this.tsmiAdditional.DropDownItems.Add(item);
                        groupId = property.GroupId;
                    }
                    ToolStripMenuItem item2 = new ToolStripMenuItem {
                        Text = property.LocalizedName,
                        ImageIndex = num2,
                        Tag = property
                    };
                    item2.Click += new EventHandler(this.tsmiProperty_Click);
                    item.DropDownItems.Add(item2);
                }
            }
            this.tsddAddCondition.Image = IconSet.GetImage("ContainerFilterControl.tsddAddCondition.Image");
            this.tsddCondition.DropDownOpening += new EventHandler(this.Condition_DropDownOpening);
            this.tsMatch.Renderer = BorderLessToolStripRenderer.Default;
            this.tsNewCondition.Renderer = this.tsMatch.Renderer;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsMatch.Font = this.Font;
            this.tsNewCondition.Font = this.Font;
        }

        private void tsMatch_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (this.flpFilters.Padding.Left != 0x17)
            {
                this.flpFilters.Padding = new Padding(0x17, 0, 0, 0);
            }
            this.tslMatch.Visible = false;
        }

        private void tsmiConditionAll_Click(object sender, EventArgs e)
        {
            base.UpdateDropDownCondition(this.tsddCondition, (ToolStripItem) sender);
        }

        private void tsmiDate_Click(object sender, EventArgs e)
        {
            Control filterControl = (Control) Activator.CreateInstance((System.Type) ((ToolStripItem) sender).Tag);
            this.AddFilterControl(filterControl);
        }

        private void tsmiProperty_Click(object sender, EventArgs e)
        {
            System.Type type;
            VirtualProperty tag = (VirtualProperty) ((ToolStripItem) sender).Tag;
            if (PropertyFilterMap.TryGetValue(tag.PropertyType, out type))
            {
                this.AddFilterControl((Control) Activator.CreateInstance(type, new object[] { tag.PropertyId }));
            }
        }

        public void UpdateCulture()
        {
            base.UpdateDropDownText(this.tsddCondition, this.tsddCondition.Tag);
        }

        [Browsable(false), DefaultValue((string) null)]
        public IVirtualItemFilter Filter
        {
            get
            {
                if (this.flpFilters.Controls.Count > 1)
                {
                    List<IVirtualItemFilter> filters = new List<IVirtualItemFilter>(this.flpFilters.Controls.Count - 1);
                    foreach (Control control in this.flpFilters.Controls)
                    {
                        IFilterControl control2 = control as IFilterControl;
                        if (!((control2 == null) || control2.IsEmpty))
                        {
                            filters.Add(control2.Filter);
                        }
                    }
                    AggregatedFilterCondition tag = (AggregatedFilterCondition) this.tsddCondition.Tag;
                    if ((filters.Count == 1) && (tag != AggregatedFilterCondition.None))
                    {
                        return filters[0];
                    }
                    if (filters.Count > 0)
                    {
                        return new AggregatedVirtualItemFilter(tag, filters);
                    }
                }
                return null;
            }
            set
            {
                this.Clear();
                if (value != null)
                {
                    AggregatedVirtualItemFilter filter = value as AggregatedVirtualItemFilter;
                    if (filter != null)
                    {
                        this.InitializeContainerFilterControl(filter);
                    }
                    else
                    {
                        base.CanRaiseFilterChanged = false;
                        try
                        {
                            this.tsmiConditionAll.PerformClick();
                            Control filterControl = CreateFilterControl(value);
                            this.AddFilterControl(filterControl);
                        }
                        finally
                        {
                            base.CanRaiseFilterChanged = true;
                        }
                    }
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                bool flag = this.flpFilters.Controls.Count < 2;
                if (!flag)
                {
                    flag = true;
                    foreach (Control control in this.flpFilters.Controls)
                    {
                        if (!(!(control is IFilterControl) || ((IFilterControl) control).IsEmpty))
                        {
                            return false;
                        }
                    }
                }
                return flag;
            }
        }

        protected static Dictionary<System.Type, System.Type> PropertyFilterMap
        {
            get
            {
                if (FPropertyFilterMap == null)
                {
                    FPropertyFilterMap = new Dictionary<System.Type, System.Type>();
                    FPropertyFilterMap.Add(typeof(byte), typeof(PropertyIntegralFilterControl));
                    FPropertyFilterMap.Add(typeof(short), typeof(PropertyIntegralFilterControl));
                    FPropertyFilterMap.Add(typeof(ushort), typeof(PropertyIntegralFilterControl));
                    FPropertyFilterMap.Add(typeof(int), typeof(PropertyIntegralFilterControl));
                    FPropertyFilterMap.Add(typeof(uint), typeof(PropertyIntegralFilterControl));
                    FPropertyFilterMap.Add(typeof(long), typeof(PropertyIntegralFilterControl));
                    FPropertyFilterMap.Add(typeof(string), typeof(PropertyStringFilterControl));
                    FPropertyFilterMap.Add(typeof(DateTime), typeof(DateFilterControl));
                    FPropertyFilterMap.Add(typeof(Version), typeof(PropertyVersionFilterControl));
                }
                return FPropertyFilterMap;
            }
        }

        public ToolStrip TopToolStrip
        {
            get
            {
                return this.tsMatch;
            }
        }
    }
}

