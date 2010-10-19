namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class SelectSearchFoldersDialog : BasicDialog
    {
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHarddrives;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnReset;
        private Bevel bvlButtons;
        private System.Windows.Forms.CheckBox chkProcessSubfolders;
        private IContainer components = null;
        private ImageList imlCheck;
        private Label lblSelectFolders;
        private Nomad.Commons.Controls.SizeGripProvider SizeGripProvider;
        private TableLayoutPanel tlpButtons;
        private VirtualFolderTreeView tvFolders;

        public SelectSearchFoldersDialog()
        {
            this.InitializeComponent();
            this.SizeGripProvider = new Nomad.Commons.Controls.SizeGripProvider(this.tlpButtons);
            this.imlCheck.Images.Add(this.CreateCheckBoxGlyph(CheckBoxState.UncheckedNormal));
            this.imlCheck.Images.Add(this.CreateCheckBoxGlyph(CheckBoxState.CheckedNormal));
            this.imlCheck.Images.Add(this.CreateCheckBoxGlyph(CheckBoxState.MixedNormal));
            bool flag = SettingsManager.CheckSafeMode(SafeMode.SkipFormPlacement) || (Control.ModifierKeys == Keys.Shift);
            FormSettings.RegisterForm(this, flag ? FormPlacement.None : FormPlacement.Size);
        }

        private void btnHarddrives_Click(object sender, EventArgs e)
        {
            bool flag = false;
            this.tvFolders.BeginUpdate();
            try
            {
                this.Reset();
                foreach (TreeNode node in this.tvFolders.Nodes)
                {
                    IGetVirtualVolume tag = node.Tag as IGetVirtualVolume;
                    if ((tag != null) && (tag.VolumeType == DriveType.Fixed))
                    {
                        node.StateImageIndex = 1;
                        if (this.ProcessSubfolders)
                        {
                            this.UpdateNodeCheckState(node);
                        }
                        flag = true;
                    }
                }
            }
            finally
            {
                this.tvFolders.EndUpdate();
            }
            this.btnOk.Enabled = flag;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.Reset();
            this.btnOk.Enabled = false;
        }

        private void chkProcessSubfolders_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateProcessSubfolders(this.chkProcessSubfolders.Checked);
        }

        private Image CreateCheckBoxGlyph(CheckBoxState state)
        {
            Bitmap image = new Bitmap(this.imlCheck.ImageSize.Width, this.imlCheck.ImageSize.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Size glyphSize = CheckBoxRenderer.GetGlyphSize(graphics, state);
                CheckBoxRenderer.DrawCheckBox(graphics, new Point((image.Width - glyphSize.Width) / 2, (image.Height - glyphSize.Height) / 2), state);
            }
            return image;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SelectSearchFoldersDialog));
            this.lblSelectFolders = new Label();
            this.chkProcessSubfolders = new System.Windows.Forms.CheckBox();
            this.tvFolders = new VirtualFolderTreeView();
            this.imlCheck = new ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHarddrives = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tlpButtons = new TableLayoutPanel();
            this.bvlButtons = new Bevel();
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(panel, "tlpBack");
            panel.Controls.Add(this.lblSelectFolders, 0, 0);
            panel.Controls.Add(this.chkProcessSubfolders, 0, 2);
            panel.Controls.Add(this.tvFolders, 0, 1);
            panel.Name = "tlpBack";
            manager.ApplyResources(this.lblSelectFolders, "lblSelectFolders");
            this.lblSelectFolders.Name = "lblSelectFolders";
            manager.ApplyResources(this.chkProcessSubfolders, "chkProcessSubfolders");
            this.chkProcessSubfolders.Name = "chkProcessSubfolders";
            this.chkProcessSubfolders.UseVisualStyleBackColor = true;
            this.chkProcessSubfolders.CheckedChanged += new EventHandler(this.chkProcessSubfolders_CheckedChanged);
            this.tvFolders.ClearOnCollapse = false;
            this.tvFolders.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            this.tvFolders.DataBindings.Add(new Binding("FolderNameCasing", VirtualFilePanelSettings.Default, "FolderNameCasing", true, DataSourceUpdateMode.Never));
            manager.ApplyResources(this.tvFolders, "tvFolders");
            this.tvFolders.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.tvFolders.ExplorerTheme = Settings.Default.ExplorerTheme;
            this.tvFolders.FadePlusMinus = true;
            this.tvFolders.FolderNameCasing = VirtualFilePanelSettings.Default.FolderNameCasing;
            this.tvFolders.FullRowSelect = true;
            this.tvFolders.HotTracking = true;
            this.tvFolders.Name = "tvFolders";
            this.tvFolders.ShowAllRootFolders = true;
            this.tvFolders.ShowItemIcons = false;
            this.tvFolders.ShowLines = false;
            this.tvFolders.StateImageList = this.imlCheck;
            this.tvFolders.MouseClick += new MouseEventHandler(this.treeView_MouseClick);
            this.tvFolders.NodeAdded += new TreeViewEventHandler(this.treeView_NodeAdded);
            this.imlCheck.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.imlCheck, "imlCheck");
            this.imlCheck.TransparentColor = Color.Transparent;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.btnHarddrives, "btnHarddrives");
            this.btnHarddrives.Name = "btnHarddrives";
            this.btnHarddrives.UseVisualStyleBackColor = true;
            this.btnHarddrives.Click += new EventHandler(this.btnHarddrives_Click);
            manager.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new EventHandler(this.btnReset_Click);
            manager.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tlpButtons.Controls.Add(this.btnHarddrives, 3, 0);
            this.tlpButtons.Controls.Add(this.btnReset, 2, 0);
            this.tlpButtons.Controls.Add(this.btnOk, 1, 0);
            this.tlpButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            this.tlpButtons.Name = "tlpButtons";
            manager.ApplyResources(this.bvlButtons, "bvlButtons");
            this.bvlButtons.ForeColor = SystemColors.ControlDarkDark;
            this.bvlButtons.Name = "bvlButtons";
            this.bvlButtons.Sides = Border3DSide.Bottom;
            this.bvlButtons.Style = Border3DStyle.Flat;
            base.AcceptButton = this.btnOk;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.Controls.Add(panel);
            base.Controls.Add(this.bvlButtons);
            base.Controls.Add(this.tlpButtons);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SelectSearchFoldersDialog";
            base.ShowInTaskbar = false;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void Reset()
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            foreach (TreeNode node in this.tvFolders.Nodes)
            {
                stack.Push(node);
            }
            this.tvFolders.BeginUpdate();
            try
            {
                while (stack.Count > 0)
                {
                    TreeNode node2 = stack.Pop();
                    node2.StateImageIndex = 0;
                    foreach (TreeNode node in node2.Nodes)
                    {
                        stack.Push(node);
                    }
                }
            }
            finally
            {
                this.tvFolders.EndUpdate();
            }
        }

        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo info = this.tvFolders.HitTest(e.Location);
            if (info.Location == TreeViewHitTestLocations.StateImage)
            {
                info.Node.StateImageIndex = (info.Node.StateImageIndex == 1) ? 0 : 1;
                if (this.ProcessSubfolders)
                {
                    this.UpdateNodeCheckState(info.Node);
                }
                bool flag = false;
                if (info.Node.StateImageIndex == 1)
                {
                    flag = true;
                }
                else
                {
                    Stack<TreeNode> stack = new Stack<TreeNode>();
                    foreach (TreeNode node in this.tvFolders.Nodes)
                    {
                        stack.Push(node);
                    }
                    while (stack.Count > 0)
                    {
                        TreeNode node2 = stack.Pop();
                        if (node2.StateImageIndex == 1)
                        {
                            flag = true;
                            break;
                        }
                        foreach (TreeNode node in node2.Nodes)
                        {
                            stack.Push(node);
                        }
                    }
                }
                this.btnOk.Enabled = flag;
            }
        }

        private void treeView_NodeAdded(object sender, TreeViewEventArgs e)
        {
            e.Node.StateImageIndex = (((e.Node.Parent != null) && (e.Node.Parent.StateImageIndex == 1)) && this.ProcessSubfolders) ? 1 : 0;
        }

        private void UpdateNodeCheckState(TreeNode node)
        {
            this.tvFolders.BeginUpdate();
            try
            {
                Stack<TreeNode> stack = new Stack<TreeNode>();
                if (node.IsExpanded)
                {
                    stack.Push(node);
                }
                else
                {
                    node.Nodes.Clear();
                    node.Nodes.Add(new TreeNode());
                }
                while (stack.Count > 0)
                {
                    TreeNode node2 = stack.Pop();
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        node3.StateImageIndex = node2.StateImageIndex;
                        if (node3.IsExpanded)
                        {
                            stack.Push(node3);
                        }
                    }
                }
                int stateImageIndex = -1;
                for (TreeNode node4 = node.Parent; node4 != null; node4 = node4.Parent)
                {
                    if (stateImageIndex != 2)
                    {
                        stateImageIndex = -1;
                        foreach (TreeNode node3 in node4.Nodes)
                        {
                            if (stateImageIndex < 0)
                            {
                                stateImageIndex = node3.StateImageIndex;
                            }
                            else if (stateImageIndex != node3.StateImageIndex)
                            {
                                stateImageIndex = 2;
                                break;
                            }
                        }
                    }
                    node4.StateImageIndex = stateImageIndex;
                }
            }
            finally
            {
                this.tvFolders.EndUpdate();
            }
        }

        private void UpdateProcessSubfolders(bool processSubfolders)
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            foreach (TreeNode node in this.tvFolders.Nodes)
            {
                stack.Push(node);
            }
            if (stack.Count != 0)
            {
                this.tvFolders.BeginUpdate();
                try
                {
                    while (stack.Count > 0)
                    {
                        TreeNode node2 = stack.Pop();
                        if (!((node2.StateImageIndex != 2) || processSubfolders))
                        {
                            node2.StateImageIndex = 0;
                        }
                        else if ((node2.StateImageIndex == 1) && processSubfolders)
                        {
                            this.UpdateNodeCheckState(node2);
                            continue;
                        }
                        foreach (TreeNode node in node2.Nodes)
                        {
                            stack.Push(node);
                        }
                    }
                }
                finally
                {
                    this.tvFolders.EndUpdate();
                }
            }
        }

        public IVirtualFolder[] Folders
        {
            get
            {
                List<IVirtualFolder> list = new List<IVirtualFolder>();
                Stack<TreeNode> stack = new Stack<TreeNode>();
                foreach (TreeNode node in this.tvFolders.Nodes)
                {
                    stack.Push(node);
                }
                while (stack.Count > 0)
                {
                    TreeNode node2 = stack.Pop();
                    if (node2.StateImageIndex == 1)
                    {
                        list.Add((IVirtualFolder) node2.Tag);
                    }
                    if ((node2.StateImageIndex == 2) || !this.ProcessSubfolders)
                    {
                        foreach (TreeNode node in node2.Nodes)
                        {
                            stack.Push(node);
                        }
                    }
                }
                return ((list.Count > 0) ? list.ToArray() : null);
            }
            set
            {
                if ((value != null) && (value.Length != 0))
                {
                    this.tvFolders.BeginUpdate();
                    try
                    {
                        foreach (IVirtualFolder folder in value)
                        {
                            TreeNode node = this.tvFolders.ShowVirtualFolder(folder, false);
                            if (node != null)
                            {
                                node.StateImageIndex = 1;
                                if (this.ProcessSubfolders)
                                {
                                    this.UpdateNodeCheckState(node);
                                }
                                this.btnOk.Enabled = true;
                            }
                        }
                    }
                    finally
                    {
                        this.tvFolders.EndUpdate();
                    }
                }
            }
        }

        public bool ProcessSubfolders
        {
            get
            {
                return this.chkProcessSubfolders.Checked;
            }
            set
            {
                this.chkProcessSubfolders.Checked = value;
                this.UpdateProcessSubfolders(this.chkProcessSubfolders.Checked);
            }
        }

        [Browsable(false)]
        public bool ShowItemIcons
        {
            get
            {
                return this.tvFolders.ShowItemIcons;
            }
            set
            {
                this.tvFolders.ShowItemIcons = value;
            }
        }
    }
}

