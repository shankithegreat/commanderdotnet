namespace Nomad.Controls.Option
{
    using Nomad.Controls;
    using Nomad.Controls.Actions;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class EditorViewerOptionControl : UserControl, IPersistComponentSettings
    {
        private Button btnBrowseEditor;
        private Button btnBrowseViewer;
        private Button btnClearEditor;
        private Button btnClearViewer;
        private ContextMenuStrip cmsEdit;
        private IContainer components = null;
        private OpenFileDialog FindExeFileDialog;
        private Label lblEditor;
        private TableLayoutPanel tlpBack;
        private ToolStripMenuItem tsmiCopy;
        private ToolStripMenuItem tsmiSelectAll;
        private ToolStripSeparator tssEdit1;
        private TextBox txtEditor;
        private TextBox txtViewer;
        private PropertyValuesWatcher ValuesWatcher;

        public EditorViewerOptionControl()
        {
            this.InitializeComponent();
            this.btnBrowseViewer.Tag = this.txtViewer;
            this.btnBrowseEditor.Tag = this.txtEditor;
            this.btnClearViewer.Tag = this.txtViewer;
            this.btnClearEditor.Tag = this.txtEditor;
            this.txtViewer.Tag = Resources.sFindViewer;
            this.txtEditor.Tag = Resources.sFindEditor;
            new ActionToolStripItemLink(StandardActions.Copy, this.tsmiCopy, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
            new ActionToolStripItemLink(StandardActions.SelectAll, this.tsmiSelectAll, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
        }

        private void btnBrowseViewer_Click(object sender, EventArgs e)
        {
            TextBox tag = (TextBox) ((Control) sender).Tag;
            if (File.Exists(tag.Text))
            {
                this.FindExeFileDialog.FileName = tag.Text;
            }
            else
            {
                try
                {
                    this.FindExeFileDialog.InitialDirectory = Path.GetDirectoryName(tag.Text);
                }
                catch
                {
                }
            }
            this.FindExeFileDialog.Title = (string) tag.Tag;
            if (this.FindExeFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                tag.Text = this.FindExeFileDialog.FileName;
            }
        }

        private void btnClearViewer_Click(object sender, EventArgs e)
        {
            TextBox tag = (TextBox) ((Control) sender).Tag;
            tag.Text = string.Empty;
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(EditorViewerOptionControl));
            PropertyValue value2 = new PropertyValue();
            PropertyValue value3 = new PropertyValue();
            this.txtViewer = new TextBox();
            this.txtEditor = new TextBox();
            this.btnBrowseViewer = new Button();
            this.btnBrowseEditor = new Button();
            this.lblEditor = new Label();
            this.btnClearViewer = new Button();
            this.btnClearEditor = new Button();
            this.FindExeFileDialog = new OpenFileDialog();
            this.tlpBack = new TableLayoutPanel();
            this.cmsEdit = new ContextMenuStrip(this.components);
            this.tsmiCopy = new ToolStripMenuItem();
            this.tssEdit1 = new ToolStripSeparator();
            this.tsmiSelectAll = new ToolStripMenuItem();
            this.ValuesWatcher = new PropertyValuesWatcher();
            Label label = new Label();
            this.tlpBack.SuspendLayout();
            this.cmsEdit.SuspendLayout();
            ((ISupportInitialize) this.ValuesWatcher).BeginInit();
            base.SuspendLayout();
            manager.ApplyResources(label, "lblViewer");
            label.Name = "lblViewer";
            this.txtViewer.ContextMenuStrip = this.cmsEdit;
            manager.ApplyResources(this.txtViewer, "txtViewer");
            this.txtViewer.Name = "txtViewer";
            this.txtViewer.ReadOnly = true;
            this.txtViewer.TabStop = false;
            this.txtEditor.ContextMenuStrip = this.cmsEdit;
            manager.ApplyResources(this.txtEditor, "txtEditor");
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.ReadOnly = true;
            this.txtEditor.TabStop = false;
            manager.ApplyResources(this.btnBrowseViewer, "btnBrowseViewer");
            this.btnBrowseViewer.Name = "btnBrowseViewer";
            this.btnBrowseViewer.UseVisualStyleBackColor = true;
            this.btnBrowseViewer.Click += new EventHandler(this.btnBrowseViewer_Click);
            manager.ApplyResources(this.btnBrowseEditor, "btnBrowseEditor");
            this.btnBrowseEditor.Name = "btnBrowseEditor";
            this.btnBrowseEditor.UseVisualStyleBackColor = true;
            this.btnBrowseEditor.Click += new EventHandler(this.btnBrowseViewer_Click);
            manager.ApplyResources(this.lblEditor, "lblEditor");
            this.lblEditor.Name = "lblEditor";
            manager.ApplyResources(this.btnClearViewer, "btnClearViewer");
            this.btnClearViewer.Name = "btnClearViewer";
            this.btnClearViewer.UseVisualStyleBackColor = true;
            this.btnClearViewer.Click += new EventHandler(this.btnClearViewer_Click);
            manager.ApplyResources(this.btnClearEditor, "btnClearEditor");
            this.btnClearEditor.Name = "btnClearEditor";
            this.btnClearEditor.UseVisualStyleBackColor = true;
            this.btnClearEditor.Click += new EventHandler(this.btnClearViewer_Click);
            this.FindExeFileDialog.AddExtension = false;
            manager.ApplyResources(this.FindExeFileDialog, "FindExeFileDialog");
            manager.ApplyResources(this.tlpBack, "tlpBack");
            this.tlpBack.Controls.Add(this.btnClearViewer, 3, 0);
            this.tlpBack.Controls.Add(this.txtEditor, 1, 1);
            this.tlpBack.Controls.Add(this.lblEditor, 0, 1);
            this.tlpBack.Controls.Add(this.txtViewer, 1, 0);
            this.tlpBack.Controls.Add(this.btnClearEditor, 3, 1);
            this.tlpBack.Controls.Add(this.btnBrowseViewer, 2, 0);
            this.tlpBack.Controls.Add(label, 0, 0);
            this.tlpBack.Controls.Add(this.btnBrowseEditor, 2, 1);
            this.tlpBack.Name = "tlpBack";
            this.cmsEdit.Items.AddRange(new ToolStripItem[] { this.tsmiCopy, this.tssEdit1, this.tsmiSelectAll });
            this.cmsEdit.Name = "cmsEdit";
            manager.ApplyResources(this.cmsEdit, "cmsEdit");
            this.tsmiCopy.Name = "tsmiCopy";
            manager.ApplyResources(this.tsmiCopy, "tsmiCopy");
            this.tssEdit1.Name = "tssEdit1";
            manager.ApplyResources(this.tssEdit1, "tssEdit1");
            this.tsmiSelectAll.Name = "tsmiSelectAll";
            manager.ApplyResources(this.tsmiSelectAll, "tsmiSelectAll");
            value2.DataObject = this.txtViewer;
            value2.PropertyName = "Text";
            value3.DataObject = this.txtEditor;
            value3.PropertyName = "Text";
            this.ValuesWatcher.Items.AddRange(new PropertyValue[] { value2, value3 });
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tlpBack);
            base.Name = "EditorViewerOptionControl";
            this.tlpBack.ResumeLayout(false);
            this.tlpBack.PerformLayout();
            this.cmsEdit.ResumeLayout(false);
            ((ISupportInitialize) this.ValuesWatcher).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadComponentSettings()
        {
            this.txtViewer.Text = Settings.Default.ViewerPath;
            this.txtEditor.Text = Settings.Default.EditorPath;
            this.ValuesWatcher.RememberValues();
        }

        public void ResetComponentSettings()
        {
        }

        public void SaveComponentSettings()
        {
            Settings.Default.ViewerPath = this.txtViewer.Text;
            Settings.Default.EditorPath = this.txtEditor.Text;
        }

        public bool SaveSettings
        {
            get
            {
                return this.ValuesWatcher.AnyValueChanged;
            }
            set
            {
            }
        }

        public string SettingsKey
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }
    }
}

