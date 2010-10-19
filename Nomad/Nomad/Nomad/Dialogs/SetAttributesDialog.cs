namespace Nomad.Dialogs
{
    using Microsoft;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Controls.Specialized;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class SetAttributesDialog : BasicDialog
    {
        private CheckBox[] AttrCheckBoxList;
        private Button btnCancel;
        private Button btnOk;
        private Bevel bvlButtons;
        private CheckBox chkAttributeArchive;
        private CheckBox chkAttributeCompressed;
        private CheckBox chkAttributeEncrypted;
        private CheckBox chkAttributeHidden;
        private CheckBox chkAttributeReadOnly;
        private CheckBox chkAttributeSystem;
        private CheckBox chkCreationTime;
        private CheckBox chkIncludeSubfolders;
        private CheckBox chkLastAccessTime;
        private CheckBox chkLastWriteTime;
        private IContainer components = null;
        private DateTimePicker dtpCreationDate;
        private DateTimePicker dtpCreationTime;
        private DateTimePicker dtpLastAccessDate;
        private DateTimePicker dtpLastAccessTime;
        private DateTimePicker dtpLastWriteDate;
        private DateTimePicker dtpLastWriteTime;
        private Label lblInvalidCreationTime;
        private Label lblInvalidLastAccessTime;
        private Label lblInvalidLastWriteTime;
        private Label lblItem;
        private FileAttributes RememberResetAttributes;
        private FileAttributes RememberSetAttributes;
        private TableLayoutPanel tlpBack;
        private VirtualItemToolStrip tsItem;

        public SetAttributesDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.chkLastAccessTime.Tag = this.dtpLastAccessDate;
            this.chkCreationTime.Tag = this.dtpCreationDate;
            this.chkLastWriteTime.Tag = this.dtpLastWriteDate;
            this.dtpLastAccessDate.Tag = this.dtpLastAccessTime;
            this.dtpCreationDate.Tag = this.dtpCreationTime;
            this.dtpLastWriteDate.Tag = this.dtpLastWriteTime;
            this.dtpLastAccessTime.Tag = this.lblInvalidLastAccessTime;
            this.dtpCreationTime.Tag = this.lblInvalidCreationTime;
            this.dtpLastWriteTime.Tag = this.lblInvalidLastWriteTime;
            this.chkAttributeArchive.Tag = FileAttributes.Archive;
            this.chkAttributeReadOnly.Tag = FileAttributes.ReadOnly;
            this.chkAttributeHidden.Tag = FileAttributes.Hidden;
            this.chkAttributeSystem.Tag = FileAttributes.System;
            this.chkAttributeCompressed.Tag = FileAttributes.Compressed;
            this.chkAttributeEncrypted.Tag = FileAttributes.Encrypted;
            this.AttrCheckBoxList = new CheckBox[] { this.chkAttributeArchive, this.chkAttributeReadOnly, this.chkAttributeHidden, this.chkAttributeSystem, this.chkAttributeCompressed, this.chkAttributeEncrypted };
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SetAttributesDialog));
            manager.ApplyResources(this.lblInvalidCreationTime, "lblInvalidLastAccessTime");
            manager.ApplyResources(this.lblInvalidLastWriteTime, "lblInvalidLastAccessTime");
        }

        private void chkAttributeArchive_Click(object sender, EventArgs e)
        {
            this.UpdateOkButton();
        }

        private void chkAttributeCompressed_Click(object sender, EventArgs e)
        {
            if (this.chkAttributeCompressed.Checked)
            {
                this.chkAttributeEncrypted.Checked = false;
            }
            this.UpdateOkButton();
        }

        private void chkAttributeEncrypted_Click(object sender, EventArgs e)
        {
            if (this.chkAttributeEncrypted.Checked)
            {
                this.chkAttributeCompressed.Checked = false;
            }
            this.UpdateOkButton();
        }

        private void chkLastAccessTime_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox) sender;
            ((Control) box.Tag).Enabled = box.Checked;
        }

        private void chkLastAccessTime_Click(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox) sender;
            if (box.Checked)
            {
                DateTimePicker tag = (DateTimePicker) box.Tag;
                this.ShowDateError(tag, false);
                tag.Select();
            }
            this.UpdateOkButton();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dtpLastAccessDate_EnabledChanged(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            ((Control) control.Tag).Enabled = control.Enabled;
        }

        public bool Execute(IWin32Window owner, IEnumerable<IVirtualItem> selection)
        {
            int num = selection.Count<IVirtualItem>();
            IVirtualItem source = null;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            FileAttributes attributes = 0;
            foreach (CheckBox box in this.AttrCheckBoxList)
            {
                attributes |= (FileAttributes) box.Tag;
            }
            this.RememberSetAttributes = 0;
            this.RememberResetAttributes = 0;
            foreach (IVirtualItem item2 in selection)
            {
                if (source == null)
                {
                    source = item2;
                    this.RememberSetAttributes = item2.Attributes & attributes;
                    this.RememberResetAttributes = attributes & ~this.RememberSetAttributes;
                }
                else
                {
                    this.RememberSetAttributes &= item2.Attributes & attributes;
                    this.RememberResetAttributes &= attributes & ~item2.Attributes;
                }
                if (item2 is IVirtualFolder)
                {
                    flag = true;
                    flag2 = true;
                    flag3 = true;
                    flag4 = true;
                    flag5 = true;
                    this.RememberSetAttributes = 0;
                    this.RememberResetAttributes = 0;
                    break;
                }
                IChangeVirtualItem item3 = item2 as IChangeVirtualItem;
                flag2 = flag2 || ((item3 != null) && item3.CanSetProperty(6));
                flag3 = flag3 || ((item3 != null) && item3.CanSetProperty(7));
                flag4 = flag4 || ((item3 != null) && item3.CanSetProperty(9));
                flag5 = flag5 || ((item3 != null) && item3.CanSetProperty(8));
            }
            this.chkIncludeSubfolders.Enabled = flag;
            this.chkCreationTime.Enabled = flag3;
            this.chkLastAccessTime.Enabled = flag4;
            this.chkLastWriteTime.Enabled = flag5;
            if (num == 1)
            {
                if (this.chkCreationTime.Enabled && source.IsPropertyAvailable(7))
                {
                    this.CreationTime = (DateTime?) source[7];
                }
                if (this.chkLastAccessTime.Enabled && source.IsPropertyAvailable(9))
                {
                    this.LastAccessTime = (DateTime?) source[9];
                }
                if (this.chkLastWriteTime.Enabled && source.IsPropertyAvailable(8))
                {
                    this.LastWriteTime = (DateTime?) source[8];
                }
                this.lblItem.Text = (source is IVirtualFolder) ? Resources.sSetFolderAttributes : Resources.sSetFileAttributes;
                this.tsItem.Add(source);
            }
            else
            {
                this.lblItem.Text = PluralInfo.Format(Resources.sSetMultipleAttributes, new object[] { num });
                this.tsItem.AddRange(selection);
            }
            foreach (CheckBox box in this.AttrCheckBoxList)
            {
                box.Enabled = flag2;
                box.ThreeState = (this.RememberSetAttributes | this.RememberResetAttributes) != attributes;
                FileAttributes tag = (FileAttributes) box.Tag;
                if ((this.RememberSetAttributes & tag) > 0)
                {
                    box.CheckState = CheckState.Checked;
                }
                else if ((this.RememberResetAttributes & tag) > 0)
                {
                    box.CheckState = CheckState.Unchecked;
                }
                else
                {
                    box.CheckState = CheckState.Indeterminate;
                }
            }
            this.UpdateOkButton();
            return (base.ShowDialog(owner) == DialogResult.OK);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SetAttributesDialog));
            this.chkAttributeArchive = new CheckBox();
            this.chkAttributeReadOnly = new CheckBox();
            this.chkAttributeSystem = new CheckBox();
            this.chkAttributeHidden = new CheckBox();
            this.chkAttributeCompressed = new CheckBox();
            this.chkAttributeEncrypted = new CheckBox();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.chkIncludeSubfolders = new CheckBox();
            this.chkCreationTime = new CheckBox();
            this.chkLastWriteTime = new CheckBox();
            this.dtpLastAccessDate = new DateTimePicker();
            this.dtpCreationTime = new DateTimePicker();
            this.dtpLastWriteTime = new DateTimePicker();
            this.dtpLastAccessTime = new DateTimePicker();
            this.chkLastAccessTime = new CheckBox();
            this.dtpLastWriteDate = new DateTimePicker();
            this.dtpCreationDate = new DateTimePicker();
            this.lblInvalidLastAccessTime = new Label();
            this.lblInvalidCreationTime = new Label();
            this.lblInvalidLastWriteTime = new Label();
            this.lblItem = new Label();
            this.tsItem = new VirtualItemToolStrip(this.components);
            this.tlpBack = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            GroupBox box = new GroupBox();
            TableLayoutPanel panel = new TableLayoutPanel();
            TableLayoutPanel panel2 = new TableLayoutPanel();
            box.SuspendLayout();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            this.tlpBack.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(box, "grpAttributes");
            this.tlpBack.SetColumnSpan(box, 3);
            box.Controls.Add(panel);
            box.Name = "grpAttributes";
            box.TabStop = false;
            manager.ApplyResources(panel, "tlpAttributes");
            panel.Controls.Add(this.chkAttributeArchive, 0, 0);
            panel.Controls.Add(this.chkAttributeReadOnly, 0, 1);
            panel.Controls.Add(this.chkAttributeSystem, 1, 0);
            panel.Controls.Add(this.chkAttributeHidden, 1, 1);
            panel.Controls.Add(this.chkAttributeCompressed, 2, 0);
            panel.Controls.Add(this.chkAttributeEncrypted, 2, 1);
            panel.Name = "tlpAttributes";
            manager.ApplyResources(this.chkAttributeArchive, "chkAttributeArchive");
            this.chkAttributeArchive.Checked = true;
            this.chkAttributeArchive.CheckState = CheckState.Indeterminate;
            this.chkAttributeArchive.Name = "chkAttributeArchive";
            this.chkAttributeArchive.ThreeState = true;
            this.chkAttributeArchive.UseVisualStyleBackColor = true;
            this.chkAttributeArchive.Click += new EventHandler(this.chkAttributeArchive_Click);
            manager.ApplyResources(this.chkAttributeReadOnly, "chkAttributeReadOnly");
            this.chkAttributeReadOnly.Checked = true;
            this.chkAttributeReadOnly.CheckState = CheckState.Indeterminate;
            this.chkAttributeReadOnly.Name = "chkAttributeReadOnly";
            this.chkAttributeReadOnly.ThreeState = true;
            this.chkAttributeReadOnly.UseVisualStyleBackColor = true;
            this.chkAttributeReadOnly.Click += new EventHandler(this.chkAttributeArchive_Click);
            manager.ApplyResources(this.chkAttributeSystem, "chkAttributeSystem");
            this.chkAttributeSystem.Checked = true;
            this.chkAttributeSystem.CheckState = CheckState.Indeterminate;
            this.chkAttributeSystem.Name = "chkAttributeSystem";
            this.chkAttributeSystem.ThreeState = true;
            this.chkAttributeSystem.UseVisualStyleBackColor = true;
            this.chkAttributeSystem.Click += new EventHandler(this.chkAttributeArchive_Click);
            manager.ApplyResources(this.chkAttributeHidden, "chkAttributeHidden");
            this.chkAttributeHidden.Checked = true;
            this.chkAttributeHidden.CheckState = CheckState.Indeterminate;
            this.chkAttributeHidden.Name = "chkAttributeHidden";
            this.chkAttributeHidden.ThreeState = true;
            this.chkAttributeHidden.UseVisualStyleBackColor = true;
            this.chkAttributeHidden.Click += new EventHandler(this.chkAttributeArchive_Click);
            manager.ApplyResources(this.chkAttributeCompressed, "chkAttributeCompressed");
            this.chkAttributeCompressed.Checked = true;
            this.chkAttributeCompressed.CheckState = CheckState.Indeterminate;
            this.chkAttributeCompressed.Name = "chkAttributeCompressed";
            this.chkAttributeCompressed.ThreeState = true;
            this.chkAttributeCompressed.UseVisualStyleBackColor = true;
            this.chkAttributeCompressed.Click += new EventHandler(this.chkAttributeCompressed_Click);
            manager.ApplyResources(this.chkAttributeEncrypted, "chkAttributeEncrypted");
            this.chkAttributeEncrypted.Checked = true;
            this.chkAttributeEncrypted.CheckState = CheckState.Indeterminate;
            this.chkAttributeEncrypted.Name = "chkAttributeEncrypted";
            this.chkAttributeEncrypted.ThreeState = true;
            this.chkAttributeEncrypted.UseVisualStyleBackColor = true;
            this.chkAttributeEncrypted.Click += new EventHandler(this.chkAttributeEncrypted_Click);
            manager.ApplyResources(panel2, "tlpButtons");
            panel2.BackColor = Color.Gainsboro;
            panel2.Controls.Add(this.btnCancel, 2, 0);
            panel2.Controls.Add(this.btnOk, 1, 0);
            panel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            panel2.Name = "tlpButtons";
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.DialogResult = DialogResult.OK;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkIncludeSubfolders, "chkIncludeSubfolders");
            this.tlpBack.SetColumnSpan(this.chkIncludeSubfolders, 3);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCreationTime, "chkCreationTime");
            this.chkCreationTime.Name = "chkCreationTime";
            this.chkCreationTime.UseVisualStyleBackColor = true;
            this.chkCreationTime.Click += new EventHandler(this.chkLastAccessTime_Click);
            this.chkCreationTime.CheckedChanged += new EventHandler(this.chkLastAccessTime_CheckedChanged);
            manager.ApplyResources(this.chkLastWriteTime, "chkLastWriteTime");
            this.chkLastWriteTime.Name = "chkLastWriteTime";
            this.chkLastWriteTime.UseVisualStyleBackColor = true;
            this.chkLastWriteTime.Click += new EventHandler(this.chkLastAccessTime_Click);
            this.chkLastWriteTime.CheckedChanged += new EventHandler(this.chkLastAccessTime_CheckedChanged);
            manager.ApplyResources(this.dtpLastAccessDate, "dtpLastAccessDate");
            this.dtpLastAccessDate.Format = DateTimePickerFormat.Short;
            this.dtpLastAccessDate.Name = "dtpLastAccessDate";
            this.dtpLastAccessDate.EnabledChanged += new EventHandler(this.dtpLastAccessDate_EnabledChanged);
            manager.ApplyResources(this.dtpCreationTime, "dtpCreationTime");
            this.dtpCreationTime.Format = DateTimePickerFormat.Time;
            this.dtpCreationTime.Name = "dtpCreationTime";
            this.dtpCreationTime.ShowUpDown = true;
            manager.ApplyResources(this.dtpLastWriteTime, "dtpLastWriteTime");
            this.dtpLastWriteTime.Format = DateTimePickerFormat.Time;
            this.dtpLastWriteTime.Name = "dtpLastWriteTime";
            this.dtpLastWriteTime.ShowUpDown = true;
            manager.ApplyResources(this.dtpLastAccessTime, "dtpLastAccessTime");
            this.dtpLastAccessTime.Format = DateTimePickerFormat.Time;
            this.dtpLastAccessTime.Name = "dtpLastAccessTime";
            this.dtpLastAccessTime.ShowUpDown = true;
            manager.ApplyResources(this.chkLastAccessTime, "chkLastAccessTime");
            this.chkLastAccessTime.Name = "chkLastAccessTime";
            this.chkLastAccessTime.UseVisualStyleBackColor = true;
            this.chkLastAccessTime.Click += new EventHandler(this.chkLastAccessTime_Click);
            this.chkLastAccessTime.CheckedChanged += new EventHandler(this.chkLastAccessTime_CheckedChanged);
            manager.ApplyResources(this.dtpLastWriteDate, "dtpLastWriteDate");
            this.dtpLastWriteDate.Format = DateTimePickerFormat.Short;
            this.dtpLastWriteDate.Name = "dtpLastWriteDate";
            this.dtpLastWriteDate.EnabledChanged += new EventHandler(this.dtpLastAccessDate_EnabledChanged);
            manager.ApplyResources(this.dtpCreationDate, "dtpCreationDate");
            this.dtpCreationDate.Format = DateTimePickerFormat.Short;
            this.dtpCreationDate.Name = "dtpCreationDate";
            this.dtpCreationDate.EnabledChanged += new EventHandler(this.dtpLastAccessDate_EnabledChanged);
            manager.ApplyResources(this.lblInvalidLastAccessTime, "lblInvalidLastAccessTime");
            this.lblInvalidLastAccessTime.Name = "lblInvalidLastAccessTime";
            manager.ApplyResources(this.lblInvalidCreationTime, "lblInvalidCreationTime");
            this.lblInvalidCreationTime.Name = "lblInvalidCreationTime";
            manager.ApplyResources(this.lblInvalidLastWriteTime, "lblInvalidLastWriteTime");
            this.lblInvalidLastWriteTime.Name = "lblInvalidLastWriteTime";
            manager.ApplyResources(this.lblItem, "lblItem");
            this.tlpBack.SetColumnSpan(this.lblItem, 3);
            this.lblItem.Name = "lblItem";
            manager.ApplyResources(this.tsItem, "tsItem");
            this.tsItem.CanOverflow = false;
            this.tlpBack.SetColumnSpan(this.tsItem, 3);
            this.tsItem.GripStyle = ToolStripGripStyle.Hidden;
            this.tsItem.Name = "tsItem";
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.dtpLastWriteTime, 2, 6);
            this.tlpBack.Controls.Add(this.dtpCreationTime, 2, 5);
            this.tlpBack.Controls.Add(this.dtpLastAccessDate, 1, 4);
            this.tlpBack.Controls.Add(this.chkLastWriteTime, 0, 6);
            this.tlpBack.Controls.Add(this.dtpLastAccessTime, 2, 4);
            this.tlpBack.Controls.Add(this.chkCreationTime, 0, 5);
            this.tlpBack.Controls.Add(this.dtpLastWriteDate, 1, 6);
            this.tlpBack.Controls.Add(this.lblItem, 0, 0);
            this.tlpBack.Controls.Add(this.dtpCreationDate, 1, 5);
            this.tlpBack.Controls.Add(box, 0, 3);
            this.tlpBack.Controls.Add(this.chkIncludeSubfolders, 0, 2);
            this.tlpBack.Controls.Add(this.tsItem, 0, 1);
            this.tlpBack.Controls.Add(this.chkLastAccessTime, 0, 4);
            this.tlpBack.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpBack.Name = "tlpBack";
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
            base.Controls.Add(this.tlpBack);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SetAttributesDialog";
            base.ShowInTaskbar = false;
            box.ResumeLayout(false);
            box.PerformLayout();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            panel2.ResumeLayout(false);
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SetDate(DateTimePicker datePicker, DateTime? value)
        {
            if (value.HasValue)
            {
                DateTimePicker tag = (DateTimePicker) datePicker.Tag;
                try
                {
                    datePicker.Value = value.Value;
                    tag.Value = value.Value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    DateTime now = DateTime.Now;
                    datePicker.Value = now;
                    tag.Value = now;
                    this.ShowDateError(datePicker, true);
                }
            }
        }

        private void ShowDateError(DateTimePicker datePicker, bool show)
        {
            DateTimePicker tag = (DateTimePicker) datePicker.Tag;
            Label control = (Label) tag.Tag;
            TableLayoutPanelCellPosition cellPosition = this.tlpBack.GetCellPosition(control);
            if ((cellPosition.Column < 0) || (cellPosition.Row < 0))
            {
                cellPosition = this.tlpBack.GetCellPosition(datePicker);
            }
            if ((cellPosition.Column < 0) || (cellPosition.Row < 0))
            {
                throw new InvalidOperationException("Invalid Date Picker state");
            }
            this.tlpBack.SuspendLayout();
            if (show)
            {
                datePicker.Parent = null;
                tag.Parent = null;
                this.tlpBack.Controls.Add(control, cellPosition.Column, cellPosition.Row);
            }
            else
            {
                control.Parent = null;
                this.tlpBack.Controls.Add(datePicker, cellPosition.Column, cellPosition.Row);
                this.tlpBack.Controls.Add(tag, cellPosition.Column + 1, cellPosition.Row);
            }
            this.tlpBack.ResumeLayout();
        }

        private void UpdateOkButton()
        {
            this.chkAttributeCompressed.Enabled = this.chkAttributeCompressed.Enabled && OS.IsWinNT;
            this.chkAttributeEncrypted.Enabled = this.chkAttributeEncrypted.Enabled && OS.IsWinNT;
            this.btnOk.Enabled = (((this.SetAttributes != this.RememberSetAttributes) || (this.ResetAttributes != this.RememberResetAttributes)) || (this.chkCreationTime.Checked || this.chkLastAccessTime.Checked)) || this.chkLastWriteTime.Checked;
        }

        public DateTime? CreationTime
        {
            get
            {
                return (this.chkCreationTime.Checked ? new DateTime?(this.dtpCreationDate.Value.Date.AddTicks(this.dtpCreationTime.Value.TimeOfDay.Ticks)) : null);
            }
            set
            {
                this.SetDate(this.dtpCreationDate, value);
            }
        }

        public bool IncludeSubfolders
        {
            get
            {
                return this.chkIncludeSubfolders.Checked;
            }
        }

        public DateTime? LastAccessTime
        {
            get
            {
                return (this.chkLastAccessTime.Checked ? new DateTime?(this.dtpLastAccessDate.Value.Date.AddTicks(this.dtpLastAccessTime.Value.TimeOfDay.Ticks)) : null);
            }
            set
            {
                this.SetDate(this.dtpLastAccessDate, value);
            }
        }

        public DateTime? LastWriteTime
        {
            get
            {
                return (this.chkLastWriteTime.Checked ? new DateTime?(this.dtpLastWriteDate.Value.Date.AddTicks(this.dtpLastWriteTime.Value.TimeOfDay.Ticks)) : null);
            }
            set
            {
                this.SetDate(this.dtpLastWriteDate, value);
            }
        }

        public FileAttributes ResetAttributes
        {
            get
            {
                FileAttributes attributes = 0;
                foreach (CheckBox box in this.AttrCheckBoxList)
                {
                    if (box.CheckState == CheckState.Unchecked)
                    {
                        attributes |= (FileAttributes) box.Tag;
                    }
                }
                return attributes;
            }
        }

        public FileAttributes SetAttributes
        {
            get
            {
                FileAttributes attributes = 0;
                foreach (CheckBox box in this.AttrCheckBoxList)
                {
                    if (box.CheckState == CheckState.Checked)
                    {
                        attributes |= (FileAttributes) box.Tag;
                    }
                }
                return attributes;
            }
        }
    }
}

