namespace Nomad.Workers
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.Threading;
    using Nomad.Controls;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using Nomad.Workers.Configuration;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;

    public class OverwritePromtDialog : BasicForm
    {
        private string AlreadyExistsFormat;
        private const int ApplyToAll = 1;
        private const int ApplyToAllBigger = 7;
        private const int ApplyToAllDifferentSize = 4;
        private const int ApplyToAllDifferentTime = 8;
        private const int ApplyToAllNewer = 11;
        private const int ApplyToAllOlder = 10;
        private const int ApplyToAllSameSize = 5;
        private const int ApplyToAllSameTime = 9;
        private const int ApplyToAllSmaller = 6;
        private const int ApplyToAllThisExt = 3;
        private const int ApplyToAllThisName = 2;
        private const int ApplyToCurrent = 0;
        private Button btnAppend;
        private Button btnCancel;
        private Button btnOverwrite;
        private Button btnRename;
        private Button btnResume;
        private Button btnSkip;
        private ComboBox cmbChoice;
        private ContextMenuStrip cmsItem;
        private IContainer components = null;
        private string CurrentExt;
        private string CurrentName;
        private IconStyle DefaultIconStyle;
        private string FOldName;
        private OverwriteDialogResult FOverwriteResult = OverwriteDialogResult.Overwrite;
        private ScalablePictureBox imgDestIcon;
        private ScalablePictureBox imgSourceIcon;
        private string ItemLastWriteTimeFormat;
        private string ItemSizeFormat;
        private Label lblAlreadyExists;
        private Label lblApplyTo;
        private Label lblExistingLastWriteTime;
        private Label lblExistingName;
        private Label lblExistingSize;
        private Label lblSourceLastWriteTime;
        private Label lblSourceName;
        private Label lblSourceSize;
        private WaitCallback LoadThumbnailHandler;
        private TableLayoutPanel tlpBack;
        private TableLayoutPanel tlpDest;
        private TableLayoutPanel tlpSource;
        private VirtualToolTip ToolTip;
        private TextBox txtRename;

        public OverwritePromtDialog()
        {
            this.InitializeComponent();
            base.LocalizeForm();
            this.btnOverwrite.Tag = OverwriteDialogResult.Overwrite;
            this.btnAppend.Tag = OverwriteDialogResult.Append;
            this.btnResume.Tag = OverwriteDialogResult.Resume;
            this.btnSkip.Tag = OverwriteDialogResult.Skip;
            this.btnRename.Tag = OverwriteDialogResult.Rename;
            ResourceManager manager = new SettingsManager.LocalizedResourceManager(typeof(OverwritePromtDialog));
            this.AlreadyExistsFormat = manager.GetString("lblAlreadyExists.Text");
            this.ItemLastWriteTimeFormat = manager.GetString("lblSourceLastWriteTime.Text");
            this.ItemSizeFormat = manager.GetString("lblSourceSize.Text");
            if (CopySettings.Default.ShowThumbnailInOverwriteDialog)
            {
                base.SuspendLayout();
                this.tlpBack.SuspendLayout();
                this.imgSourceIcon.Scalable = false;
                this.imgSourceIcon.Size = CopySettings.Default.ThumbnailSize;
                this.imgSourceIcon.Paint += new PaintEventHandler(this.imgSourceIcon_Paint);
                this.imgDestIcon.Scalable = false;
                this.imgDestIcon.Size = CopySettings.Default.ThumbnailSize;
                this.imgDestIcon.Paint += new PaintEventHandler(this.imgSourceIcon_Paint);
                this.btnOverwrite.Width = Math.Max(this.btnOverwrite.Width, 90);
                this.btnAppend.Width = Math.Max(this.btnAppend.Width, 90);
                this.btnSkip.Width = Math.Max(this.btnSkip.Width, 90);
                this.btnCancel.Width = Math.Max(this.btnCancel.Width, 90);
                this.tlpBack.ResumeLayout();
                base.ResumeLayout();
                this.DefaultIconStyle = IconStyle.DefaultIcon;
                base.Shown += new EventHandler(this.ShowThumnails);
            }
        }

        private void btnOverwrite_Click(object sender, EventArgs e)
        {
            this.FOverwriteResult = (OverwriteDialogResult) ((Button) sender).Tag;
            base.DialogResult = DialogResult.OK;
        }

        private void cmbChoice_Enter(object sender, EventArgs e)
        {
            base.AcceptButton = this.btnOverwrite;
        }

        private void cmbChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
        }

        private void cmsItem_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            if (strip.SourceControl.Tag is IVirtualItemUI)
            {
                ContextMenuStrip strip2 = ((IVirtualItemUI) strip.SourceControl.Tag).CreateContextMenuStrip(this, 0, null);
                if (strip2 != null)
                {
                    strip2.Show(strip.Location);
                }
            }
        }

        private static CompareResult CompareProperties(IVirtualItem a, IVirtualItem b, int propertyId)
        {
            int num;
            if (!((((a != null) && (b != null)) && a.IsPropertyAvailable(propertyId)) && b.IsPropertyAvailable(propertyId)))
            {
                return CompareResult.Unknown;
            }
            switch (propertyId)
            {
                case 7:
                case 8:
                case 9:
                {
                    DateTime time = (DateTime) a[8];
                    DateTime time2 = (DateTime) b[8];
                    TimeSpan span = (TimeSpan) (time - time2);
                    num = Math.Sign((long) (span.Ticks / 0x989680L));
                    break;
                }
                default:
                    num = Comparer.DefaultInvariant.Compare(a[propertyId], b[propertyId]);
                    break;
            }
            if (num < 0)
            {
                return CompareResult.Less;
            }
            if (num > 0)
            {
                return CompareResult.Greater;
            }
            return CompareResult.Equal;
        }

        private static string CompareResultToString(CompareResult result, string less, string greater, string equals)
        {
            switch (result)
            {
                case CompareResult.Less:
                    return less;

                case CompareResult.Greater:
                    return greater;

                case CompareResult.Equal:
                    return equals;
            }
            return "?";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Execute(IWin32Window owner, IVirtualItem source, IVirtualItem dest)
        {
            this.CurrentName = source.Name;
            this.CurrentExt = (string) source[1];
            if (string.IsNullOrEmpty(this.CurrentExt))
            {
                this.CurrentExt = Path.GetExtension(this.CurrentName);
            }
            this.cmbChoice.Items.Clear();
            this.cmbChoice.Items.Add(new KeyValuePair<int, string>(0, Resources.sOverwriteRuleCurrentFile));
            this.cmbChoice.Items.Add(new KeyValuePair<int, string>(1, Resources.sOverwriteRuleAll + " (Alt+A)"));
            this.cmbChoice.Items.Add(new KeyValuePair<int, string>(2, string.Format(Resources.sOverwriteRuleAllThisName, this.CurrentName)));
            this.cmbChoice.Items.Add(new KeyValuePair<int, string>(3, string.Format(Resources.sOverwriteRuleAllThisExt, this.CurrentExt)));
            this.cmbChoice.SelectedIndex = 0;
            this.lblSourceName.Text = source.FullName;
            long? nullable = (long?) source[3];
            this.imgSourceIcon.Image = VirtualIcon.GetIcon(source, ImageHelper.DefaultLargeIconSize, this.DefaultIconStyle);
            this.lblSourceName.Tag = source;
            this.imgSourceIcon.Tag = source;
            this.lblExistingName.Text = dest.FullName;
            long? nullable2 = (long?) dest[3];
            this.imgDestIcon.Image = VirtualIcon.GetIcon(dest, ImageHelper.DefaultLargeIconSize, this.DefaultIconStyle);
            this.lblExistingName.Tag = dest;
            this.imgDestIcon.Tag = dest;
            CompareResult result = CompareProperties(source, dest, 3);
            switch (result)
            {
                case CompareResult.Less:
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(4, Resources.sOverwriteRuleAllDifferentSize));
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(7, Resources.sOverwriteRuleAllBigger));
                    break;

                case CompareResult.Greater:
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(4, Resources.sOverwriteRuleAllDifferentSize));
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(6, Resources.sOverwriteRuleAllSmaller));
                    break;

                case CompareResult.Equal:
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(5, Resources.sOverwriteRuleAllSameSize));
                    break;
            }
            CompareResult result2 = CompareProperties(source, dest, 8);
            switch (result2)
            {
                case CompareResult.Less:
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(8, Resources.sOverwriteRuleAllDifferentTime));
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(11, Resources.sOverwriteRuleAllNewer));
                    break;

                case CompareResult.Greater:
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(8, Resources.sOverwriteRuleAllDifferentTime));
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(10, Resources.sOverwriteRuleAllOlder));
                    break;

                case CompareResult.Equal:
                    this.cmbChoice.Items.Add(new KeyValuePair<int, string>(9, Resources.sOverwriteRuleAllSameTime));
                    break;
            }
            this.lblSourceLastWriteTime.Text = string.Format(this.ItemLastWriteTimeFormat, source[8] ?? "?", CompareResultToString(result2, Resources.sTimeOlder, Resources.sTimeNewer, Resources.sTimeEqual));
            object[] args = new object[] { nullable ?? "?", CompareResultToString(result, Resources.sSizeSmaller, Resources.sSizeBigger, Resources.sSizeEqual) };
            this.lblSourceSize.Text = PluralInfo.Format(this.ItemSizeFormat, args);
            this.lblExistingLastWriteTime.Text = string.Format(this.ItemLastWriteTimeFormat, dest[8] ?? "?", CompareResultToString(NegateCompareResult(result2), Resources.sTimeOlder, Resources.sTimeNewer, Resources.sTimeEqual));
            args = new object[] { nullable2 ?? "?", CompareResultToString(NegateCompareResult(result), Resources.sSizeSmaller, Resources.sSizeBigger, Resources.sSizeEqual) };
            this.lblExistingSize.Text = PluralInfo.Format(this.ItemSizeFormat, args);
            if (source.Equals(dest))
            {
                this.lblAlreadyExists.Text = string.Format(Resources.sCannotCopyFileToItself, source.Name);
                this.btnOverwrite.Enabled = false;
                this.btnAppend.Enabled = false;
                this.btnResume.Enabled = false;
            }
            else
            {
                this.lblAlreadyExists.Text = string.Format(this.AlreadyExistsFormat, source.Name);
                this.btnResume.Enabled = result == CompareResult.Greater;
            }
            this.FOldName = source.Name;
            this.txtRename.Text = this.FOldName;
            bool flag = base.ShowDialog(owner) == DialogResult.OK;
            if (!flag)
            {
                this.FOverwriteResult = OverwriteDialogResult.Abort;
            }
            return flag;
        }

        private void imgSourceIcon_MouseHover(object sender, EventArgs e)
        {
            if (Settings.Default.ShowItemToolTips && (Form.ActiveForm == this))
            {
                Control control = (Control) sender;
                IVirtualItemUI tag = control.Tag as IVirtualItemUI;
                if (tag != null)
                {
                    if (this.ToolTip == null)
                    {
                        this.ToolTip = new VirtualToolTip();
                    }
                    this.ToolTip.ShowTooltip(tag);
                }
            }
        }

        private void imgSourceIcon_MouseLeave(object sender, EventArgs e)
        {
            if (this.ToolTip != null)
            {
                this.ToolTip.HideTooltip();
            }
        }

        private void imgSourceIcon_Paint(object sender, PaintEventArgs e)
        {
            Rectangle clientRectangle = ((Control) sender).ClientRectangle;
            clientRectangle.Width--;
            clientRectangle.Height--;
            e.Graphics.DrawRectangle(Pens.DarkGray, clientRectangle);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(OverwritePromtDialog));
            this.tlpSource = new TableLayoutPanel();
            this.imgSourceIcon = new ScalablePictureBox();
            this.cmsItem = new ContextMenuStrip(this.components);
            this.lblSourceSize = new Label();
            this.lblSourceName = new Label();
            this.lblSourceLastWriteTime = new Label();
            this.tlpDest = new TableLayoutPanel();
            this.imgDestIcon = new ScalablePictureBox();
            this.lblExistingSize = new Label();
            this.lblExistingName = new Label();
            this.lblExistingLastWriteTime = new Label();
            this.lblAlreadyExists = new Label();
            this.btnOverwrite = new Button();
            this.txtRename = new TextBox();
            this.btnAppend = new Button();
            this.btnSkip = new Button();
            this.btnCancel = new Button();
            this.btnRename = new Button();
            this.btnResume = new Button();
            this.lblApplyTo = new Label();
            this.cmbChoice = new ComboBox();
            this.tlpBack = new TableLayoutPanel();
            GroupBox box = new GroupBox();
            GroupBox box2 = new GroupBox();
            PictureBox box3 = new PictureBox();
            box.SuspendLayout();
            this.tlpSource.SuspendLayout();
            ((ISupportInitialize) this.imgSourceIcon).BeginInit();
            box2.SuspendLayout();
            this.tlpDest.SuspendLayout();
            ((ISupportInitialize) this.imgDestIcon).BeginInit();
            ((ISupportInitialize) box3).BeginInit();
            this.tlpBack.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(box, "grpSource");
            this.tlpBack.SetColumnSpan(box, 5);
            box.Controls.Add(this.tlpSource);
            box.Name = "grpSource";
            box.TabStop = false;
            manager.ApplyResources(this.tlpSource, "tlpSource");
            this.tlpSource.Controls.Add(this.imgSourceIcon, 0, 0);
            this.tlpSource.Controls.Add(this.lblSourceSize, 1, 2);
            this.tlpSource.Controls.Add(this.lblSourceName, 1, 0);
            this.tlpSource.Controls.Add(this.lblSourceLastWriteTime, 1, 1);
            this.tlpSource.MinimumSize = new Size(0x138, 0);
            this.tlpSource.Name = "tlpSource";
            this.imgSourceIcon.ContextMenuStrip = this.cmsItem;
            manager.ApplyResources(this.imgSourceIcon, "imgSourceIcon");
            this.imgSourceIcon.Name = "imgSourceIcon";
            this.tlpSource.SetRowSpan(this.imgSourceIcon, 3);
            this.imgSourceIcon.TabStop = false;
            this.imgSourceIcon.MouseLeave += new EventHandler(this.imgSourceIcon_MouseLeave);
            this.imgSourceIcon.MouseHover += new EventHandler(this.imgSourceIcon_MouseHover);
            this.cmsItem.Name = "cmsItem";
            manager.ApplyResources(this.cmsItem, "cmsItem");
            this.cmsItem.Opening += new CancelEventHandler(this.cmsItem_Opening);
            manager.ApplyResources(this.lblSourceSize, "lblSourceSize");
            this.lblSourceSize.Name = "lblSourceSize";
            this.lblSourceName.ContextMenuStrip = this.cmsItem;
            manager.ApplyResources(this.lblSourceName, "lblSourceName");
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.UseMnemonic = false;
            this.lblSourceName.MouseLeave += new EventHandler(this.imgSourceIcon_MouseLeave);
            this.lblSourceName.MouseHover += new EventHandler(this.imgSourceIcon_MouseHover);
            manager.ApplyResources(this.lblSourceLastWriteTime, "lblSourceLastWriteTime");
            this.lblSourceLastWriteTime.ContextMenuStrip = this.cmsItem;
            this.lblSourceLastWriteTime.Name = "lblSourceLastWriteTime";
            manager.ApplyResources(box2, "grpExisting");
            this.tlpBack.SetColumnSpan(box2, 5);
            box2.Controls.Add(this.tlpDest);
            box2.Name = "grpExisting";
            box2.TabStop = false;
            manager.ApplyResources(this.tlpDest, "tlpDest");
            this.tlpDest.Controls.Add(this.imgDestIcon, 0, 0);
            this.tlpDest.Controls.Add(this.lblExistingSize, 1, 2);
            this.tlpDest.Controls.Add(this.lblExistingName, 1, 0);
            this.tlpDest.Controls.Add(this.lblExistingLastWriteTime, 1, 1);
            this.tlpDest.MinimumSize = new Size(0x138, 0);
            this.tlpDest.Name = "tlpDest";
            this.imgDestIcon.ContextMenuStrip = this.cmsItem;
            manager.ApplyResources(this.imgDestIcon, "imgDestIcon");
            this.imgDestIcon.Name = "imgDestIcon";
            this.tlpDest.SetRowSpan(this.imgDestIcon, 3);
            this.imgDestIcon.TabStop = false;
            this.imgDestIcon.MouseLeave += new EventHandler(this.imgSourceIcon_MouseLeave);
            this.imgDestIcon.MouseHover += new EventHandler(this.imgSourceIcon_MouseHover);
            manager.ApplyResources(this.lblExistingSize, "lblExistingSize");
            this.lblExistingSize.Name = "lblExistingSize";
            this.lblExistingName.ContextMenuStrip = this.cmsItem;
            manager.ApplyResources(this.lblExistingName, "lblExistingName");
            this.lblExistingName.Name = "lblExistingName";
            this.lblExistingName.UseMnemonic = false;
            this.lblExistingName.MouseLeave += new EventHandler(this.imgSourceIcon_MouseLeave);
            this.lblExistingName.MouseHover += new EventHandler(this.imgSourceIcon_MouseHover);
            manager.ApplyResources(this.lblExistingLastWriteTime, "lblExistingLastWriteTime");
            this.lblExistingLastWriteTime.ContextMenuStrip = this.cmsItem;
            this.lblExistingLastWriteTime.Name = "lblExistingLastWriteTime";
            manager.ApplyResources(box3, "imgAlreadyExists");
            box3.Name = "imgAlreadyExists";
            box3.TabStop = false;
            this.tlpBack.SetColumnSpan(this.lblAlreadyExists, 4);
            manager.ApplyResources(this.lblAlreadyExists, "lblAlreadyExists");
            this.lblAlreadyExists.Name = "lblAlreadyExists";
            this.lblAlreadyExists.UseMnemonic = false;
            manager.ApplyResources(this.btnOverwrite, "btnOverwrite");
            this.btnOverwrite.CausesValidation = false;
            this.tlpBack.SetColumnSpan(this.btnOverwrite, 2);
            this.btnOverwrite.Name = "btnOverwrite";
            this.btnOverwrite.UseVisualStyleBackColor = true;
            this.btnOverwrite.Click += new EventHandler(this.btnOverwrite_Click);
            this.tlpBack.SetColumnSpan(this.txtRename, 2);
            manager.ApplyResources(this.txtRename, "txtRename");
            this.txtRename.Name = "txtRename";
            this.txtRename.TextChanged += new EventHandler(this.txtRename_TextChanged);
            this.txtRename.Enter += new EventHandler(this.txtRename_Enter);
            manager.ApplyResources(this.btnAppend, "btnAppend");
            this.btnAppend.CausesValidation = false;
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new EventHandler(this.btnOverwrite_Click);
            manager.ApplyResources(this.btnSkip, "btnSkip");
            this.btnSkip.CausesValidation = false;
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new EventHandler(this.btnOverwrite_Click);
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnRename, "btnRename");
            this.btnRename.Name = "btnRename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new EventHandler(this.btnOverwrite_Click);
            manager.ApplyResources(this.btnResume, "btnResume");
            this.tlpBack.SetColumnSpan(this.btnResume, 2);
            this.btnResume.Name = "btnResume";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new EventHandler(this.btnOverwrite_Click);
            manager.ApplyResources(this.lblApplyTo, "lblApplyTo");
            this.tlpBack.SetColumnSpan(this.lblApplyTo, 2);
            this.lblApplyTo.Name = "lblApplyTo";
            this.tlpBack.SetColumnSpan(this.cmbChoice, 3);
            this.cmbChoice.DisplayMember = "Value";
            manager.ApplyResources(this.cmbChoice, "cmbChoice");
            this.cmbChoice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbChoice.Name = "cmbChoice";
            this.cmbChoice.ValueMember = "Key";
            this.cmbChoice.SelectedIndexChanged += new EventHandler(this.cmbChoice_SelectedIndexChanged);
            this.cmbChoice.Enter += new EventHandler(this.cmbChoice_Enter);
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(box3, 0, 0);
            this.tlpBack.Controls.Add(this.lblAlreadyExists, 1, 0);
            this.tlpBack.Controls.Add(this.txtRename, 3, 5);
            this.tlpBack.Controls.Add(this.btnRename, 2, 5);
            this.tlpBack.Controls.Add(this.btnResume, 0, 5);
            this.tlpBack.Controls.Add(this.lblApplyTo, 0, 3);
            this.tlpBack.Controls.Add(box, 0, 1);
            this.tlpBack.Controls.Add(box2, 0, 2);
            this.tlpBack.Controls.Add(this.btnOverwrite, 0, 4);
            this.tlpBack.Controls.Add(this.btnCancel, 4, 4);
            this.tlpBack.Controls.Add(this.btnSkip, 3, 4);
            this.tlpBack.Controls.Add(this.btnAppend, 2, 4);
            this.tlpBack.Controls.Add(this.cmbChoice, 2, 3);
            this.tlpBack.Name = "tlpBack";
            base.AcceptButton = this.btnOverwrite;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.tlpBack);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "OverwritePromtDialog";
            base.ShowInTaskbar = false;
            base.Shown += new EventHandler(this.OverwritePromtDialog_Shown);
            base.FormClosed += new FormClosedEventHandler(this.OverwritePromtDialog_FormClosed);
            base.KeyDown += new KeyEventHandler(this.OverwritePromtDialog_KeyDown);
            box.ResumeLayout(false);
            box.PerformLayout();
            this.tlpSource.ResumeLayout(false);
            this.tlpSource.PerformLayout();
            ((ISupportInitialize) this.imgSourceIcon).EndInit();
            box2.ResumeLayout(false);
            box2.PerformLayout();
            this.tlpDest.ResumeLayout(false);
            this.tlpDest.PerformLayout();
            ((ISupportInitialize) this.imgDestIcon).EndInit();
            ((ISupportInitialize) box3).EndInit();
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadThumbnail(object state)
        {
            Image Thumbnail;
            WeakReference reference = (WeakReference) state;
            if (reference.IsAlive)
            {
                PictureBox target = (PictureBox) reference.Target;
                IVirtualItem tag = (IVirtualItem) target.Tag;
                Image image = target.Image;
                Size maxThumbnailSize = target.Size;
                target = null;
                if (tag != null)
                {
                    Thumbnail = tag[0x15] as Image;
                    if (Thumbnail == null)
                    {
                        Thumbnail = VirtualIcon.GetIcon(tag, ImageHelper.DefaultLargeIconSize);
                        if ((Thumbnail == null) || (Thumbnail == image))
                        {
                            return;
                        }
                    }
                    if ((Thumbnail.Width > maxThumbnailSize.Width) || (Thumbnail.Height > maxThumbnailSize.Height))
                    {
                        Thumbnail = new Bitmap(Thumbnail, ImageHelper.GetThumbnailSize(Thumbnail.Size, maxThumbnailSize));
                    }
                    if (reference.IsAlive)
                    {
                        base.Invoke(delegate (PictureBox box) {
                            box.Image = Thumbnail;
                        }, new object[] { (PictureBox) reference.Target });
                    }
                }
            }
        }

        private static CompareResult NegateCompareResult(CompareResult result)
        {
            switch (result)
            {
                case CompareResult.Less:
                    return CompareResult.Greater;

                case CompareResult.Greater:
                    return CompareResult.Less;
            }
            return result;
        }

        private void OverwritePromtDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.lblSourceName.Tag = null;
            this.imgSourceIcon.Tag = null;
            this.lblExistingName.Tag = null;
            this.imgDestIcon.Tag = null;
            this.imgSourceIcon_MouseLeave(null, EventArgs.Empty);
        }

        private void OverwritePromtDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Alt | Keys.A))
            {
                this.cmbChoice.SelectedIndex = (this.cmbChoice.SelectedIndex != 1) ? 1 : 0;
            }
        }

        private void OverwritePromtDialog_Shown(object sender, EventArgs e)
        {
            this.lblSourceName.Text = StringHelper.CompactString(this.lblSourceName.Text, this.lblSourceName.Width, this.lblSourceName.Font, TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix);
            this.lblExistingName.Text = StringHelper.CompactString(this.lblExistingName.Text, this.lblExistingName.Width, this.lblExistingName.Font, TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix);
        }

        private void ShowThumnails(object sender, EventArgs e)
        {
            base.BeginInvoke(delegate {
                this.LoadThumbnailHandler = new WaitCallback(this.LoadThumbnail);
                VirtualIcon.ExtractIconQueue.Value.QueueWeakWorkItem(this.LoadThumbnailHandler, new WeakReference(this.imgSourceIcon));
                VirtualIcon.ExtractIconQueue.Value.QueueWeakWorkItem(this.LoadThumbnailHandler, new WeakReference(this.imgDestIcon));
            });
        }

        private void txtRename_Enter(object sender, EventArgs e)
        {
            base.AcceptButton = this.btnRename;
        }

        private void txtRename_TextChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            this.btnRename.Enabled = ((this.cmbChoice.SelectedIndex == 0) && !string.IsNullOrEmpty(this.txtRename.Text)) && !this.FOldName.Equals(this.NewName, StringComparison.OrdinalIgnoreCase);
            this.txtRename.Enabled = this.cmbChoice.SelectedIndex == 0;
        }

        public bool AcceptToAll
        {
            get
            {
                return (Convert.ToInt32(this.cmbChoice.SelectedValue) == 1);
            }
        }

        public string NewName
        {
            get
            {
                return this.txtRename.Text.Trim();
            }
        }

        public OverwriteDialogResult OverwriteResult
        {
            get
            {
                return this.FOverwriteResult;
            }
        }

        public IOverwriteRule OverwriteRule
        {
            get
            {
                switch (this.OverwriteResult)
                {
                    case OverwriteDialogResult.None:
                    case OverwriteDialogResult.Resume:
                    case OverwriteDialogResult.Rename:
                    case OverwriteDialogResult.Abort:
                        return null;
                }
                KeyValuePair<int, string> selectedItem = (KeyValuePair<int, string>) this.cmbChoice.SelectedItem;
                switch (selectedItem.Key)
                {
                    case 1:
                        return new OverwriteAllRule(this.OverwriteResult);

                    case 2:
                        return new OverwritePropertyValueRule(0, this.CurrentName, this.OverwriteResult);

                    case 3:
                        return new OverwritePropertyValueRule(1, this.CurrentExt, this.OverwriteResult);

                    case 4:
                        return new OverwritePropertyRule(3, Compare.NotEqual, this.OverwriteResult);

                    case 5:
                        return new OverwritePropertyRule(3, Compare.Equal, this.OverwriteResult);

                    case 6:
                        return new OverwritePropertyRule(3, Compare.Less, this.OverwriteResult);

                    case 7:
                        return new OverwritePropertyRule(3, Compare.Greater, this.OverwriteResult);

                    case 8:
                        return new OverwritePropertyRule(8, Compare.NotEqual, this.OverwriteResult);

                    case 9:
                        return new OverwritePropertyRule(8, Compare.Equal, this.OverwriteResult);

                    case 10:
                        return new OverwritePropertyRule(8, Compare.Less, this.OverwriteResult);

                    case 11:
                        return new OverwritePropertyRule(8, Compare.Greater, this.OverwriteResult);
                }
                return null;
            }
        }

        private enum CompareResult
        {
            Unknown,
            Less,
            Greater,
            Equal
        }
    }
}

