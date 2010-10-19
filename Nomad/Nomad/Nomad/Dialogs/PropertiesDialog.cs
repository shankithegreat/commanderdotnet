namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class PropertiesDialog : BasicForm
    {
        private CheckBox[] AttributeList;
        private Button btnApply;
        private Button btnCancel;
        private Button btnOk;
        private CheckBox chkArchive;
        private CheckBox chkCompressed;
        private CheckBox chkEncrypted;
        private CheckBox chkHidden;
        private CheckBox chkReadOnly;
        private CheckBox chkSystem;
        private IContainer components = null;
        private EventWaitHandle DialogClosed;
        private string FileAndFolderCountFormat;
        private int FolderCount;
        private Stack<IVirtualFolder> Folders = new Stack<IVirtualFolder>();
        private PictureBox imgIcon;
        private IVirtualItem Item;
        private int ItemCount;
        private string LargeSizeFormat;
        private Label lblCompressedSize;
        private Label lblContains;
        private Label lblCreationTime;
        private Label lblLastAccessTime;
        private Label lblLastWriteTime;
        private Label lblLocation;
        private Label lblName;
        private Label lblSize;
        private Label lblType;
        private Panel pnlAttributes;
        private Panel pnlCompressedSize;
        private Panel pnlContains;
        private Panel pnlCreationTime;
        private Panel pnlDelimiter2;
        private Panel pnlDelimiter4;
        private Panel pnlItemName;
        private Panel pnlLastAccessTime;
        private Panel pnlLastWriteTime;
        private Panel pnlLocation;
        private Panel pnlSize;
        private Panel pnlType;
        private string SmallSizeFormat;
        private TabControl tabControlProperties;
        private TabPage tabPageProperties;
        private System.Windows.Forms.Timer tmrUpdateSize;
        private long TotalCompressedSize;
        private long TotalSize;
        private ToolStripButton tsbStop;
        private TextBox txtName;
        private bool UpdateSize;

        public PropertiesDialog()
        {
            this.InitializeComponent();
            this.chkArchive.Tag = FileAttributes.Archive;
            this.chkReadOnly.Tag = FileAttributes.ReadOnly;
            this.chkHidden.Tag = FileAttributes.Hidden;
            this.chkSystem.Tag = FileAttributes.System;
            this.chkCompressed.Tag = FileAttributes.Compressed;
            this.chkEncrypted.Tag = FileAttributes.Encrypted;
            this.AttributeList = new CheckBox[] { this.chkArchive, this.chkReadOnly, this.chkHidden, this.chkSystem, this.chkCompressed, this.chkEncrypted };
        }

        public void AddNewTab(string tabName, Control tabControl)
        {
            tabControl.Dock = DockStyle.Fill;
            TabPage page = new TabPage(tabName) {
                Padding = new Padding(6, 11, 8, 11),
                UseVisualStyleBackColor = true
            };
            page.Controls.Add(tabControl);
            this.tabControlProperties.TabPages.Add(page);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            bool flag = this.txtName.Visible && !this.txtName.Text.Equals(this.Item.Name);
            if (!flag)
            {
                for (int i = 1; (i < this.tabControlProperties.TabCount) && !flag; i++)
                {
                    IPersistComponentSettings settings = null;
                    if (this.tabControlProperties.TabPages[i].Controls.Count > 0)
                    {
                        settings = this.tabControlProperties.TabPages[i].Controls[0] as IPersistComponentSettings;
                    }
                    flag = ((settings != null) && Convert.ToBoolean(this.tabControlProperties.TabPages[i].Tag)) && settings.SaveSettings;
                }
            }
            this.btnApply.Enabled = flag;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.DoApply();
            this.lblType.Text = this.Item[2] as string;
            this.pnlType.Visible = !string.IsNullOrEmpty(this.lblType.Text);
            this.pnlDelimiter2.Visible = this.pnlType.Visible;
            this.imgIcon.Image = VirtualIcon.GetIcon(this.Item, this.imgIcon.Size);
            this.btnApply.Enabled = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DoApply();
            base.DialogResult = DialogResult.OK;
        }

        private void CalculateFolderSize(object value)
        {
            try
            {
                while (this.Folders.Count > 0)
                {
                    if (this.DialogClosed.WaitOne(0, false))
                    {
                        return;
                    }
                    using (IVirtualFolder folder = this.Folders.Pop())
                    {
                        foreach (IVirtualItem item in folder.GetContent())
                        {
                            if (this.DialogClosed.WaitOne(0, false))
                            {
                                goto Label_00F6;
                            }
                            lock (this.tmrUpdateSize)
                            {
                                this.ItemCount++;
                                IVirtualFolder folder2 = item as IVirtualFolder;
                                if (folder2 != null)
                                {
                                    this.FolderCount++;
                                    this.Folders.Push(folder2);
                                }
                                else
                                {
                                    this.UpdateTotalSize(item);
                                }
                            }
                        }
                    }
                Label_00F6:;
                }
            }
            finally
            {
                this.UpdateSize = false;
            }
        }

        private void DelimiterPanel_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(SystemColors.ButtonShadow))
            {
                e.Graphics.DrawLine(pen, e.ClipRectangle.Left, e.ClipRectangle.Top + (e.ClipRectangle.Height / 2), e.ClipRectangle.Right, e.ClipRectangle.Top + (e.ClipRectangle.Height / 2));
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

        private void DoApply()
        {
            if (!this.txtName.Text.Equals(this.Item.Name))
            {
                try
                {
                    ((IChangeVirtualItem) this.Item).Name = this.txtName.Text;
                }
                catch (Exception exception)
                {
                    MessageDialog.ShowException(this, exception, true);
                }
                finally
                {
                    this.txtName.Text = this.Item.Name;
                }
            }
            for (int i = 1; i < this.tabControlProperties.TabCount; i++)
            {
                IPersistComponentSettings settings = null;
                if (this.tabControlProperties.TabPages[i].Controls.Count > 0)
                {
                    settings = this.tabControlProperties.TabPages[i].Controls[0] as IPersistComponentSettings;
                }
                if ((settings != null) && settings.SaveSettings)
                {
                    settings.SaveComponentSettings();
                    settings.SaveSettings = false;
                }
            }
        }

        public bool Execute(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            FileAttributes attributes = 0;
            FileAttributes attributes2 = 0;
            IVirtualFolder parent = null;
            string str = null;
            foreach (IVirtualItem item in items)
            {
                this.ItemCount++;
                IVirtualFolder folder2 = item as IVirtualFolder;
                if (folder2 != null)
                {
                    this.FolderCount++;
                    this.Folders.Push(folder2);
                }
                if (this.ItemCount == 1)
                {
                    DateTime time;
                    this.Item = item;
                    this.lblName.Text = item.Name;
                    str = item[2] as string;
                    parent = item.Parent;
                    object obj2 = item[7];
                    this.pnlCreationTime.Visible = obj2 != null;
                    if (obj2 != null)
                    {
                        time = (DateTime) obj2;
                        this.lblCreationTime.Text = time.ToString("F");
                    }
                    obj2 = item[8];
                    this.pnlLastWriteTime.Visible = obj2 != null;
                    if (obj2 != null)
                    {
                        time = (DateTime) obj2;
                        this.lblLastWriteTime.Text = time.ToString("F");
                    }
                    obj2 = item[9];
                    this.pnlLastAccessTime.Visible = obj2 != null;
                    if (obj2 != null)
                    {
                        this.lblLastAccessTime.Text = ((DateTime) obj2).ToString("F");
                    }
                }
                else
                {
                    if (!(string.IsNullOrEmpty(str) || string.Equals(str, item[2] as string)))
                    {
                        str = null;
                    }
                    if (!((parent == null) || parent.Equals(item.Parent)))
                    {
                        parent = null;
                    }
                }
                this.UpdateTotalSize(item);
                attributes |= item.Attributes;
                attributes2 |= ~item.Attributes;
            }
            if (this.ItemCount > 1)
            {
                this.pnlCreationTime.Visible = false;
                this.pnlLastWriteTime.Visible = false;
                this.pnlLastAccessTime.Visible = false;
                this.pnlContains.Visible = false;
                if (str != null)
                {
                    this.lblType.Text = string.Format(Resources.sItemsAllOfType, str);
                }
                if (parent != null)
                {
                    this.lblLocation.Text = string.Format(Resources.sItemsAllIn, parent.FullName);
                }
                this.Text = string.Format(this.Text, this.Item.Name + ", ...");
            }
            else
            {
                this.pnlType.Visible = !string.IsNullOrEmpty(str);
                if (!string.IsNullOrEmpty(str))
                {
                    this.lblType.Text = str;
                }
                this.pnlLocation.Visible = parent != null;
                if (parent != null)
                {
                    this.lblLocation.Text = parent.FullName;
                }
                this.Text = string.Format(this.Text, this.Item.Name);
                if (this.Item is IChangeVirtualItem)
                {
                    this.lblName.Visible = false;
                    this.txtName.Text = this.Item.Name;
                    this.txtName.Visible = true;
                }
                this.imgIcon.Image = VirtualIcon.GetIcon(this.Item, this.imgIcon.Size);
                this.pnlContains.Visible = this.Item is IVirtualFolder;
                if (this.Item is IVirtualFolder)
                {
                    this.ItemCount = 0;
                    this.FolderCount = 0;
                }
            }
            foreach (CheckBox box in this.AttributeList)
            {
                FileAttributes tag = (FileAttributes) box.Tag;
                if (((attributes ^ attributes2) & tag) == 0)
                {
                    box.CheckState = CheckState.Indeterminate;
                }
                else if ((attributes & tag) > 0)
                {
                    box.Checked = true;
                }
                else if ((attributes2 & tag) > 0)
                {
                    box.Checked = false;
                }
            }
            if (this.Folders.Count > 0)
            {
                this.DialogClosed = new ManualResetEvent(false);
                this.UpdateSize = true;
                this.tsbStop.Enabled = true;
                this.tmrUpdateSize.Start();
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CalculateFolderSize));
            }
            return (base.ShowDialog(owner) == DialogResult.OK);
        }

        public bool Execute(IWin32Window owner, params IVirtualItem[] items)
        {
            return this.Execute(owner, (IEnumerable<IVirtualItem>) items);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PropertiesDialog));
            this.pnlItemName = new Panel();
            this.txtName = new TextBox();
            this.lblName = new Label();
            this.imgIcon = new PictureBox();
            this.pnlType = new Panel();
            this.lblType = new Label();
            this.pnlDelimiter2 = new Panel();
            this.pnlLocation = new Panel();
            this.lblLocation = new Label();
            this.pnlSize = new Panel();
            this.lblSize = new Label();
            this.pnlCompressedSize = new Panel();
            this.lblCompressedSize = new Label();
            this.pnlContains = new Panel();
            this.lblContains = new Label();
            this.pnlCreationTime = new Panel();
            this.lblCreationTime = new Label();
            this.pnlLastWriteTime = new Panel();
            this.lblLastWriteTime = new Label();
            this.pnlLastAccessTime = new Panel();
            this.lblLastAccessTime = new Label();
            this.pnlDelimiter4 = new Panel();
            this.pnlAttributes = new Panel();
            this.chkEncrypted = new CheckBox();
            this.chkCompressed = new CheckBox();
            this.chkSystem = new CheckBox();
            this.chkReadOnly = new CheckBox();
            this.chkHidden = new CheckBox();
            this.chkArchive = new CheckBox();
            this.tabControlProperties = new TabControl();
            this.tabPageProperties = new TabPage();
            this.btnApply = new Button();
            this.tmrUpdateSize = new System.Windows.Forms.Timer(this.components);
            this.tsbStop = new ToolStripButton();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            FlowLayoutPanel panel = new FlowLayoutPanel();
            Panel panel2 = new Panel();
            Label label = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            Label label4 = new Label();
            Label label5 = new Label();
            Panel panel3 = new Panel();
            Label label6 = new Label();
            Label label7 = new Label();
            Label label8 = new Label();
            Label label9 = new Label();
            panel.SuspendLayout();
            this.pnlItemName.SuspendLayout();
            ((ISupportInitialize) this.imgIcon).BeginInit();
            this.pnlType.SuspendLayout();
            this.pnlLocation.SuspendLayout();
            this.pnlSize.SuspendLayout();
            this.pnlCompressedSize.SuspendLayout();
            this.pnlContains.SuspendLayout();
            this.pnlCreationTime.SuspendLayout();
            this.pnlLastWriteTime.SuspendLayout();
            this.pnlLastAccessTime.SuspendLayout();
            this.pnlAttributes.SuspendLayout();
            this.tabControlProperties.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            base.SuspendLayout();
            panel.Controls.Add(this.pnlItemName);
            panel.Controls.Add(panel2);
            panel.Controls.Add(this.pnlType);
            panel.Controls.Add(this.pnlDelimiter2);
            panel.Controls.Add(this.pnlLocation);
            panel.Controls.Add(this.pnlSize);
            panel.Controls.Add(this.pnlCompressedSize);
            panel.Controls.Add(this.pnlContains);
            panel.Controls.Add(panel3);
            panel.Controls.Add(this.pnlCreationTime);
            panel.Controls.Add(this.pnlLastWriteTime);
            panel.Controls.Add(this.pnlLastAccessTime);
            panel.Controls.Add(this.pnlDelimiter4);
            panel.Controls.Add(this.pnlAttributes);
            manager.ApplyResources(panel, "flpProperties");
            panel.Name = "flpProperties";
            manager.ApplyResources(this.pnlItemName, "pnlItemName");
            this.pnlItemName.Controls.Add(this.txtName);
            this.pnlItemName.Controls.Add(this.lblName);
            this.pnlItemName.Controls.Add(this.imgIcon);
            this.pnlItemName.Name = "pnlItemName";
            manager.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            manager.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            manager.ApplyResources(this.imgIcon, "imgIcon");
            this.imgIcon.Name = "imgIcon";
            this.imgIcon.TabStop = false;
            manager.ApplyResources(panel2, "pnlDelimiter1");
            panel2.Name = "pnlDelimiter1";
            panel2.Paint += new PaintEventHandler(this.DelimiterPanel_Paint);
            manager.ApplyResources(this.pnlType, "pnlType");
            this.pnlType.Controls.Add(this.lblType);
            this.pnlType.Controls.Add(label);
            this.pnlType.Name = "pnlType";
            this.lblType.AutoEllipsis = true;
            manager.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            manager.ApplyResources(label, "label1");
            label.Name = "label1";
            manager.ApplyResources(this.pnlDelimiter2, "pnlDelimiter2");
            this.pnlDelimiter2.Name = "pnlDelimiter2";
            this.pnlDelimiter2.Paint += new PaintEventHandler(this.DelimiterPanel_Paint);
            manager.ApplyResources(this.pnlLocation, "pnlLocation");
            this.pnlLocation.Controls.Add(this.lblLocation);
            this.pnlLocation.Controls.Add(label2);
            this.pnlLocation.Name = "pnlLocation";
            this.lblLocation.AutoEllipsis = true;
            manager.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            manager.ApplyResources(label2, "label4");
            label2.Name = "label4";
            manager.ApplyResources(this.pnlSize, "pnlSize");
            this.pnlSize.Controls.Add(this.lblSize);
            this.pnlSize.Controls.Add(label3);
            this.pnlSize.Name = "pnlSize";
            manager.ApplyResources(this.lblSize, "lblSize");
            this.lblSize.Name = "lblSize";
            manager.ApplyResources(label3, "label6");
            label3.Name = "label6";
            manager.ApplyResources(this.pnlCompressedSize, "pnlCompressedSize");
            this.pnlCompressedSize.Controls.Add(this.lblCompressedSize);
            this.pnlCompressedSize.Controls.Add(label4);
            this.pnlCompressedSize.Name = "pnlCompressedSize";
            manager.ApplyResources(this.lblCompressedSize, "lblCompressedSize");
            this.lblCompressedSize.Name = "lblCompressedSize";
            manager.ApplyResources(label4, "label8");
            label4.Name = "label8";
            manager.ApplyResources(this.pnlContains, "pnlContains");
            this.pnlContains.Controls.Add(this.lblContains);
            this.pnlContains.Controls.Add(label5);
            this.pnlContains.Name = "pnlContains";
            manager.ApplyResources(this.lblContains, "lblContains");
            this.lblContains.Name = "lblContains";
            manager.ApplyResources(label5, "label3");
            label5.Name = "label3";
            manager.ApplyResources(panel3, "pnlDelimiter3");
            panel3.Name = "pnlDelimiter3";
            panel3.Paint += new PaintEventHandler(this.DelimiterPanel_Paint);
            manager.ApplyResources(this.pnlCreationTime, "pnlCreationTime");
            this.pnlCreationTime.Controls.Add(this.lblCreationTime);
            this.pnlCreationTime.Controls.Add(label6);
            this.pnlCreationTime.Name = "pnlCreationTime";
            manager.ApplyResources(this.lblCreationTime, "lblCreationTime");
            this.lblCreationTime.Name = "lblCreationTime";
            manager.ApplyResources(label6, "label10");
            label6.Name = "label10";
            manager.ApplyResources(this.pnlLastWriteTime, "pnlLastWriteTime");
            this.pnlLastWriteTime.Controls.Add(this.lblLastWriteTime);
            this.pnlLastWriteTime.Controls.Add(label7);
            this.pnlLastWriteTime.Name = "pnlLastWriteTime";
            manager.ApplyResources(this.lblLastWriteTime, "lblLastWriteTime");
            this.lblLastWriteTime.Name = "lblLastWriteTime";
            manager.ApplyResources(label7, "label12");
            label7.Name = "label12";
            manager.ApplyResources(this.pnlLastAccessTime, "pnlLastAccessTime");
            this.pnlLastAccessTime.Controls.Add(this.lblLastAccessTime);
            this.pnlLastAccessTime.Controls.Add(label8);
            this.pnlLastAccessTime.Name = "pnlLastAccessTime";
            manager.ApplyResources(this.lblLastAccessTime, "lblLastAccessTime");
            this.lblLastAccessTime.Name = "lblLastAccessTime";
            manager.ApplyResources(label8, "label14");
            label8.Name = "label14";
            manager.ApplyResources(this.pnlDelimiter4, "pnlDelimiter4");
            this.pnlDelimiter4.Name = "pnlDelimiter4";
            this.pnlDelimiter4.Paint += new PaintEventHandler(this.DelimiterPanel_Paint);
            manager.ApplyResources(this.pnlAttributes, "pnlAttributes");
            this.pnlAttributes.Controls.Add(this.chkEncrypted);
            this.pnlAttributes.Controls.Add(this.chkCompressed);
            this.pnlAttributes.Controls.Add(this.chkSystem);
            this.pnlAttributes.Controls.Add(this.chkReadOnly);
            this.pnlAttributes.Controls.Add(this.chkHidden);
            this.pnlAttributes.Controls.Add(label9);
            this.pnlAttributes.Controls.Add(this.chkArchive);
            this.pnlAttributes.Name = "pnlAttributes";
            manager.ApplyResources(this.chkEncrypted, "chkEncrypted");
            this.chkEncrypted.Name = "chkEncrypted";
            this.chkEncrypted.ThreeState = true;
            this.chkEncrypted.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkCompressed, "chkCompressed");
            this.chkCompressed.Name = "chkCompressed";
            this.chkCompressed.ThreeState = true;
            this.chkCompressed.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkSystem, "chkSystem");
            this.chkSystem.Name = "chkSystem";
            this.chkSystem.ThreeState = true;
            this.chkSystem.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkReadOnly, "chkReadOnly");
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.ThreeState = true;
            this.chkReadOnly.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.chkHidden, "chkHidden");
            this.chkHidden.Name = "chkHidden";
            this.chkHidden.ThreeState = true;
            this.chkHidden.UseVisualStyleBackColor = true;
            manager.ApplyResources(label9, "label16");
            label9.Name = "label16";
            manager.ApplyResources(this.chkArchive, "chkArchive");
            this.chkArchive.Name = "chkArchive";
            this.chkArchive.ThreeState = true;
            this.chkArchive.UseVisualStyleBackColor = true;
            this.tabControlProperties.Controls.Add(this.tabPageProperties);
            manager.ApplyResources(this.tabControlProperties, "tabControlProperties");
            this.tabControlProperties.Name = "tabControlProperties";
            this.tabControlProperties.SelectedIndex = 0;
            this.tabControlProperties.Selecting += new TabControlCancelEventHandler(this.tabControlProperties_Selecting);
            this.tabPageProperties.Controls.Add(panel);
            manager.ApplyResources(this.tabPageProperties, "tabPageProperties");
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new EventHandler(this.btnApply_Click);
            this.tmrUpdateSize.Tick += new EventHandler(this.tmrUpdateSize_Tick);
            this.tsbStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbStop, "tsbStop");
            this.tsbStop.Image = Resources.ImageThrobber;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.EnabledChanged += new EventHandler(this.tsbStop_EnabledChanged);
            this.tsbStop.Click += new EventHandler(this.tsbStop_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnApply);
            base.Controls.Add(this.tabControlProperties);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PropertiesDialog";
            base.ShowInTaskbar = false;
            base.Load += new EventHandler(this.PropertiesDialog_Load);
            base.Shown += new EventHandler(this.PropertiesDialog_Shown);
            base.FormClosed += new FormClosedEventHandler(this.PropertiesDialog_FormClosed);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.pnlItemName.ResumeLayout(false);
            this.pnlItemName.PerformLayout();
            ((ISupportInitialize) this.imgIcon).EndInit();
            this.pnlType.ResumeLayout(false);
            this.pnlType.PerformLayout();
            this.pnlLocation.ResumeLayout(false);
            this.pnlLocation.PerformLayout();
            this.pnlSize.ResumeLayout(false);
            this.pnlSize.PerformLayout();
            this.pnlCompressedSize.ResumeLayout(false);
            this.pnlCompressedSize.PerformLayout();
            this.pnlContains.ResumeLayout(false);
            this.pnlContains.PerformLayout();
            this.pnlCreationTime.ResumeLayout(false);
            this.pnlCreationTime.PerformLayout();
            this.pnlLastWriteTime.ResumeLayout(false);
            this.pnlLastWriteTime.PerformLayout();
            this.pnlLastAccessTime.ResumeLayout(false);
            this.pnlLastAccessTime.PerformLayout();
            this.pnlAttributes.ResumeLayout(false);
            this.pnlAttributes.PerformLayout();
            this.tabControlProperties.ResumeLayout(false);
            this.tabPageProperties.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void PropertiesDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Idle -= new EventHandler(this.Application_Idle);
            if (this.DialogClosed != null)
            {
                this.DialogClosed.Set();
            }
        }

        private void PropertiesDialog_Load(object sender, EventArgs e)
        {
            this.SmallSizeFormat = this.lblCompressedSize.Text;
            this.LargeSizeFormat = this.lblSize.Text;
            this.FileAndFolderCountFormat = this.lblContains.Text;
        }

        private void PropertiesDialog_Shown(object sender, EventArgs e)
        {
            this.pnlDelimiter2.Visible = this.pnlType.Visible;
            this.pnlDelimiter4.Visible = (this.pnlCreationTime.Visible || this.pnlLastWriteTime.Visible) || this.pnlLastAccessTime.Visible;
            this.UpdateFolderSize();
            Application.Idle += new EventHandler(this.Application_Idle);
        }

        private void tabControlProperties_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!((((e.TabPage == this.tabPageProperties) || (e.TabPage.Controls.Count <= 0)) || !(e.TabPage.Controls[0] is IPersistComponentSettings)) || Convert.ToBoolean(e.TabPage.Tag)))
            {
                ((IPersistComponentSettings) e.TabPage.Controls[0]).LoadComponentSettings();
                e.TabPage.Tag = true;
            }
        }

        private void tmrUpdateSize_Tick(object sender, EventArgs e)
        {
            this.tmrUpdateSize.Stop();
            this.UpdateFolderSize();
            this.tmrUpdateSize.Interval = 500;
            this.tsbStop.Enabled = this.UpdateSize;
            this.tmrUpdateSize.Enabled = this.UpdateSize;
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            this.DialogClosed.Set();
        }

        private void tsbStop_EnabledChanged(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            if (item.Enabled)
            {
                item.Image = Resources.ImageAnimatedThrobber;
            }
            else
            {
                item.Image = Resources.ImageThrobber;
            }
        }

        private void UpdateFolderSize()
        {
            long totalSize;
            long totalCompressedSize;
            int itemCount;
            int folderCount;
            lock (this.tmrUpdateSize)
            {
                totalSize = this.TotalSize;
                totalCompressedSize = this.TotalCompressedSize;
                itemCount = this.ItemCount;
                folderCount = this.FolderCount;
            }
            this.lblSize.Text = PluralInfo.Format((totalSize < 0x400L) ? this.SmallSizeFormat : this.LargeSizeFormat, new object[] { totalSize, SizeTypeConverter.Default.ConvertToString(totalSize) });
            if ((totalCompressedSize > 0L) && (totalCompressedSize != totalSize))
            {
                this.lblCompressedSize.Text = PluralInfo.Format((totalCompressedSize < 0x400L) ? this.SmallSizeFormat : this.LargeSizeFormat, new object[] { totalCompressedSize, SizeTypeConverter.Default.ConvertToString(totalCompressedSize) });
                this.pnlCompressedSize.Visible = true;
            }
            if (this.pnlContains.Visible)
            {
                this.lblContains.Text = PluralInfo.Format(this.FileAndFolderCountFormat, new object[] { itemCount - folderCount, folderCount });
            }
            else if (itemCount > 1)
            {
                this.lblName.Text = PluralInfo.Format(this.FileAndFolderCountFormat, new object[] { itemCount - folderCount, folderCount });
            }
        }

        private void UpdateTotalSize(IVirtualItem item)
        {
            object obj2 = item[3];
            if (obj2 != null)
            {
                this.TotalSize += Convert.ToInt64(obj2);
            }
            if (item.IsPropertyAvailable(5))
            {
                obj2 = item[5];
            }
            if (obj2 != null)
            {
                this.TotalCompressedSize += Convert.ToInt64(obj2);
            }
        }
    }
}

