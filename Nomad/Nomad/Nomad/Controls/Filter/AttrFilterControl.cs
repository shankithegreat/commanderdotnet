namespace Nomad.Controls.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class AttrFilterControl : CustomFilterControl, IFilterControl
    {
        private CheckBox[] AttrCheckBoxList;
        private CheckBox chkAttributeArchive;
        private CheckBox chkAttributeCompressed;
        private CheckBox chkAttributeEncrypted;
        private CheckBox chkAttributeFolder;
        private CheckBox chkAttributeHidden;
        private CheckBox chkAttributeRO;
        private CheckBox chkAttributeSystem;
        private IContainer components;
        private FlowLayoutPanel flpBack;
        private TableLayoutPanel tlpAttributes;
        private ToolStripLabel tslAttributes;
        private ToolStrip tsTop;

        public AttrFilterControl()
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeControls();
        }

        public AttrFilterControl(AttributeFilter filter)
        {
            this.components = null;
            this.InitializeComponent();
            this.InitializeControls();
            this.SetFilter(filter);
        }

        private void CalculateAttributes(out FileAttributes includeAttributes, out FileAttributes excludeAttributes)
        {
            includeAttributes = 0;
            excludeAttributes = 0;
            foreach (CheckBox box in this.AttrCheckBoxList)
            {
                switch (box.CheckState)
                {
                    case CheckState.Unchecked:
                        excludeAttributes |= (FileAttributes) box.Tag;
                        break;

                    case CheckState.Checked:
                        includeAttributes |= (FileAttributes) box.Tag;
                        break;
                }
            }
        }

        private void chkAttribute_Click(object sender, EventArgs e)
        {
            base.RaiseFilterChanged();
        }

        public void Clear()
        {
            foreach (CheckBox box in this.AttrCheckBoxList)
            {
                box.CheckState = CheckState.Indeterminate;
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

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AttrFilterControl));
            this.tlpAttributes = new TableLayoutPanel();
            this.chkAttributeArchive = new CheckBox();
            this.chkAttributeCompressed = new CheckBox();
            this.chkAttributeRO = new CheckBox();
            this.chkAttributeFolder = new CheckBox();
            this.chkAttributeHidden = new CheckBox();
            this.chkAttributeSystem = new CheckBox();
            this.chkAttributeEncrypted = new CheckBox();
            this.flpBack = new FlowLayoutPanel();
            this.tsTop = new ToolStrip();
            this.tslAttributes = new ToolStripLabel();
            this.tlpAttributes.SuspendLayout();
            this.flpBack.SuspendLayout();
            this.tsTop.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.tlpAttributes, "tlpAttributes");
            this.tlpAttributes.Controls.Add(this.chkAttributeArchive, 0, 0);
            this.tlpAttributes.Controls.Add(this.chkAttributeCompressed, 2, 1);
            this.tlpAttributes.Controls.Add(this.chkAttributeRO, 0, 1);
            this.tlpAttributes.Controls.Add(this.chkAttributeFolder, 2, 0);
            this.tlpAttributes.Controls.Add(this.chkAttributeHidden, 1, 0);
            this.tlpAttributes.Controls.Add(this.chkAttributeSystem, 1, 1);
            this.tlpAttributes.Controls.Add(this.chkAttributeEncrypted, 3, 0);
            this.tlpAttributes.Name = "tlpAttributes";
            manager.ApplyResources(this.chkAttributeArchive, "chkAttributeArchive");
            this.chkAttributeArchive.Checked = true;
            this.chkAttributeArchive.CheckState = CheckState.Indeterminate;
            this.chkAttributeArchive.Name = "chkAttributeArchive";
            this.chkAttributeArchive.ThreeState = true;
            this.chkAttributeArchive.UseVisualStyleBackColor = true;
            this.chkAttributeArchive.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.chkAttributeCompressed, "chkAttributeCompressed");
            this.chkAttributeCompressed.Checked = true;
            this.chkAttributeCompressed.CheckState = CheckState.Indeterminate;
            this.chkAttributeCompressed.Name = "chkAttributeCompressed";
            this.chkAttributeCompressed.ThreeState = true;
            this.chkAttributeCompressed.UseVisualStyleBackColor = true;
            this.chkAttributeCompressed.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.chkAttributeRO, "chkAttributeRO");
            this.chkAttributeRO.Checked = true;
            this.chkAttributeRO.CheckState = CheckState.Indeterminate;
            this.chkAttributeRO.Name = "chkAttributeRO";
            this.chkAttributeRO.ThreeState = true;
            this.chkAttributeRO.UseVisualStyleBackColor = true;
            this.chkAttributeRO.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.chkAttributeFolder, "chkAttributeFolder");
            this.chkAttributeFolder.Checked = true;
            this.chkAttributeFolder.CheckState = CheckState.Indeterminate;
            this.chkAttributeFolder.Name = "chkAttributeFolder";
            this.chkAttributeFolder.ThreeState = true;
            this.chkAttributeFolder.UseVisualStyleBackColor = true;
            this.chkAttributeFolder.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.chkAttributeHidden, "chkAttributeHidden");
            this.chkAttributeHidden.Checked = true;
            this.chkAttributeHidden.CheckState = CheckState.Indeterminate;
            this.chkAttributeHidden.Name = "chkAttributeHidden";
            this.chkAttributeHidden.ThreeState = true;
            this.chkAttributeHidden.UseVisualStyleBackColor = true;
            this.chkAttributeHidden.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.chkAttributeSystem, "chkAttributeSystem");
            this.chkAttributeSystem.Checked = true;
            this.chkAttributeSystem.CheckState = CheckState.Indeterminate;
            this.chkAttributeSystem.Name = "chkAttributeSystem";
            this.chkAttributeSystem.ThreeState = true;
            this.chkAttributeSystem.UseVisualStyleBackColor = true;
            this.chkAttributeSystem.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.chkAttributeEncrypted, "chkAttributeEncrypted");
            this.chkAttributeEncrypted.Checked = true;
            this.chkAttributeEncrypted.CheckState = CheckState.Indeterminate;
            this.chkAttributeEncrypted.Name = "chkAttributeEncrypted";
            this.chkAttributeEncrypted.ThreeState = true;
            this.chkAttributeEncrypted.UseVisualStyleBackColor = true;
            this.chkAttributeEncrypted.Click += new EventHandler(this.chkAttribute_Click);
            manager.ApplyResources(this.flpBack, "flpBack");
            this.flpBack.Controls.Add(this.tsTop);
            this.flpBack.Controls.Add(this.tlpAttributes);
            this.flpBack.Name = "flpBack";
            this.tsTop.BackColor = Color.Transparent;
            manager.ApplyResources(this.tsTop, "tsTop");
            this.tsTop.GripStyle = ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new ToolStripItem[] { this.tslAttributes });
            this.tsTop.Name = "tsTop";
            this.tsTop.ItemAdded += new ToolStripItemEventHandler(this.tsTop_ItemAdded);
            this.tslAttributes.Name = "tslAttributes";
            manager.ApplyResources(this.tslAttributes, "tslAttributes");
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.flpBack);
            base.Name = "AttrFilterControl";
            this.tlpAttributes.ResumeLayout(false);
            this.tlpAttributes.PerformLayout();
            this.flpBack.ResumeLayout(false);
            this.flpBack.PerformLayout();
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeControls()
        {
            this.chkAttributeArchive.Tag = FileAttributes.Archive;
            this.chkAttributeRO.Tag = FileAttributes.ReadOnly;
            this.chkAttributeHidden.Tag = FileAttributes.Hidden;
            this.chkAttributeSystem.Tag = FileAttributes.System;
            this.chkAttributeFolder.Tag = FileAttributes.Directory;
            this.chkAttributeCompressed.Tag = FileAttributes.Compressed;
            this.chkAttributeEncrypted.Tag = FileAttributes.Encrypted;
            this.AttrCheckBoxList = new CheckBox[] { this.chkAttributeArchive, this.chkAttributeRO, this.chkAttributeHidden, this.chkAttributeSystem, this.chkAttributeFolder, this.chkAttributeCompressed, this.chkAttributeEncrypted };
        }

        public void SetFilter(AttributeFilter filter)
        {
            if (filter == null)
            {
                this.Clear();
            }
            else
            {
                foreach (CheckBox box in this.AttrCheckBoxList)
                {
                    if ((filter.IncludeAttributes & ((FileAttributes) box.Tag)) > 0)
                    {
                        box.CheckState = CheckState.Checked;
                    }
                    else if ((filter.ExcludeAttributes & ((FileAttributes) box.Tag)) > 0)
                    {
                        box.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        box.CheckState = CheckState.Indeterminate;
                    }
                }
            }
        }

        private void tsTop_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (this.tlpAttributes.Padding.Left != 0x17)
            {
                this.tlpAttributes.Padding = new Padding(0x17, 0, 0, 0);
            }
        }

        public IVirtualItemFilter AttrFilter
        {
            get
            {
                FileAttributes attributes;
                FileAttributes attributes2;
                this.CalculateAttributes(out attributes, out attributes2);
                if ((attributes != 0) || (attributes2 != 0))
                {
                    return new VirtualItemAttributeFilter(attributes, attributes2);
                }
                return null;
            }
        }

        [Browsable(false), DefaultValue((string) null)]
        public IVirtualItemFilter Filter
        {
            get
            {
                return this.AttrFilter;
            }
            set
            {
                AttributeFilter filter = value as AttributeFilter;
                if (filter != null)
                {
                    this.SetFilter(filter);
                }
                else
                {
                    this.Clear();
                    AggregatedVirtualItemFilter filter2 = value as AggregatedVirtualItemFilter;
                    if (filter2 != null)
                    {
                        foreach (IVirtualItemFilter filter3 in filter2.Filters)
                        {
                            AttributeFilter filter4 = filter3 as AttributeFilter;
                            if (filter4 != null)
                            {
                                this.SetFilter(filter4);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                FileAttributes attributes;
                FileAttributes attributes2;
                this.CalculateAttributes(out attributes, out attributes2);
                return ((attributes == 0) && (attributes2 == 0));
            }
        }

        [Browsable(false)]
        public ToolStrip TopToolStrip
        {
            get
            {
                this.tsTop.Visible = true;
                return this.tsTop;
            }
        }
    }
}

