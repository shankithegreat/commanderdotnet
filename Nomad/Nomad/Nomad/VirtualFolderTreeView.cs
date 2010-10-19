namespace Nomad
{
    using Microsoft;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Controls;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public class VirtualFolderTreeView : TreeViewEx
    {
        private bool CanRaiseSelectEvents;
        private List<KeyValuePair<IVirtualFolder, WatcherChangeTypes>> ChangedFolders;
        private bool FClearOnCollapse;
        private CharacterCasing FFolderNameCasing;
        private bool FShowAllRootFolders;
        private bool FShowItemToolTips;
        private bool FWatchChanges;
        private TreeNode HoverNode;
        private Dictionary<IVirtualItem, TreeNode> ItemNodeMap;
        private Timer ItemToolTipTimer;
        private static IComparer<IVirtualFolder> NameComparer = new VirtualComparer<IVirtualFolder>(0, ListSortDirection.Ascending);
        private TreeNode RememberSelectedNode;
        private Timer UpdateTimer;
        private Dictionary<IVirtualCachedFolder, int> WatchFolderMap;

        public event TreeViewEventHandler NodeAdded;

        public VirtualFolderTreeView()
        {
            EventHandler handler = null;
            this.CanRaiseSelectEvents = true;
            this.FFolderNameCasing = CharacterCasing.Normal;
            this.FClearOnCollapse = true;
            this.FShowItemToolTips = true;
            this.ChangedFolders = new List<KeyValuePair<IVirtualFolder, WatcherChangeTypes>>();
            if (!base.DesignMode)
            {
                this.ItemToolTipTimer = new Timer();
                this.ItemToolTipTimer.Interval = OS.MouseHoverTime;
                this.ItemToolTipTimer.Tick += new EventHandler(this.ItemToolTipTimer_Tick);
                Settings.Default.PropertyChanged += new PropertyChangedEventHandler(this.SettingPropertyChanged);
                if (handler == null)
                {
                    handler = delegate (object sender, EventArgs e) {
                        this.ClearWatchFolders();
                        this.ItemNodeMap = null;
                        Settings.Default.PropertyChanged -= new PropertyChangedEventHandler(this.SettingPropertyChanged);
                    };
                }
                base.Disposed += handler;
                base.FullRowSelect = OS.IsWin7;
                base.HotTracking = OS.IsWinXP;
                base.FadePlusMinus = OS.IsWinVista;
                base.ShowLines = !OS.IsWinVista;
                base.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            }
        }

        public void Clear()
        {
            this.ClearWatchFolders();
            base.Nodes.Clear();
            this.ItemNodeMap = null;
        }

        private void ClearWatchFolders()
        {
            if (this.WatchFolderMap != null)
            {
                foreach (IVirtualCachedFolder folder in this.WatchFolderMap.Keys)
                {
                    folder.OnChanged -= new EventHandler<VirtualItemChangedEventArgs>(this.FolderChanged);
                }
                this.WatchFolderMap.Clear();
            }
        }

        private TreeNode CreateTreeNode(IVirtualItem item)
        {
            TreeNode node = new TreeNode(StringHelper.ApplyCharacterCasing(item.Name, this.FolderNameCasing)) {
                Name = item.FullName.ToLower(),
                Tag = item
            };
            if (item is IVirtualFolder)
            {
                node.Nodes.Add(new TreeNode());
            }
            node.ForeColor = VirtualItemHelper.GetForeColor(item, Color.Empty);
            return node;
        }

        private void ExecuteVerb(object sender, ExecuteVerbEventArgs e)
        {
            if (e.Verb == "rename")
            {
                base.SelectedNode.BeginEdit();
                e.Handled = true;
            }
        }

        private void FillTreeNode(TreeNode node)
        {
            IVirtualFolder tag = (IVirtualFolder) node.Tag;
            if (node.IsExpanded)
            {
                this.UpdateWatchFolder(node, false);
                this.ItemNodeMap = null;
            }
            node.Nodes.Clear();
            IVirtualItemFilter hiddenItemsFilter = VirtualFilePanelSettings.Default.HiddenItemsFilter;
            IEnumerable<IVirtualFolder> folders = tag.GetFolders();
            ICollection<IVirtualFolder> is2 = folders as ICollection<IVirtualFolder>;
            List<IVirtualFolder> list = new List<IVirtualFolder>((is2 != null) ? is2.Count : 0x40);
            foreach (IVirtualFolder folder2 in folders)
            {
                if (!((!folder2.Equals(tag) && (hiddenItemsFilter != null)) && hiddenItemsFilter.IsMatch(folder2)))
                {
                    list.Add(folder2);
                }
            }
            list.Sort(NameComparer);
            foreach (IVirtualFolder folder3 in list)
            {
                TreeNode node2 = this.CreateTreeNode(folder3);
                node.Nodes.Add(node2);
                this.OnNodeAdded(new TreeViewEventArgs(node2));
            }
            if (this.WatchChanges && node.IsExpanded)
            {
                IVirtualCachedFolder folder = tag as IVirtualCachedFolder;
                if (folder != null)
                {
                    this.WatchFolder(folder, true);
                }
            }
        }

        private void FolderChanged(object sender, VirtualItemChangedEventArgs e)
        {
            Timer timer;
            if ((e.Item == null) && (e.ChangeType == WatcherChangeTypes.All))
            {
                lock ((timer = this.UpdateTimer))
                {
                    this.ChangedFolders.Add(new KeyValuePair<IVirtualFolder, WatcherChangeTypes>((IVirtualFolder) sender, e.ChangeType));
                }
            }
            else if (e.Item is IVirtualFolder)
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                    case WatcherChangeTypes.Deleted:
                    case (WatcherChangeTypes.Changed | WatcherChangeTypes.Created):
                    case WatcherChangeTypes.Renamed:
                        lock ((timer = this.UpdateTimer))
                        {
                            this.ChangedFolders.Add(new KeyValuePair<IVirtualFolder, WatcherChangeTypes>((IVirtualFolder) e.Item, e.ChangeType));
                        }
                        return;

                    case (WatcherChangeTypes.Deleted | WatcherChangeTypes.Created):
                    case (WatcherChangeTypes.Changed | WatcherChangeTypes.Deleted):
                    case (WatcherChangeTypes.Changed | WatcherChangeTypes.Deleted | WatcherChangeTypes.Created):
                        return;

                    case WatcherChangeTypes.Changed:
                        if ((this.WatchChanges && (e.PropertySet != null)) && e.PropertySet.Equals(0x15))
                        {
                            base.BeginInvoke(new Action<IVirtualFolder>(this.InvalidateNode), new object[] { e.Item });
                        }
                        return;
                }
            }
        }

        protected Rectangle GetIconRect(TreeNode node)
        {
            Rectangle bounds = node.Bounds;
            return new Rectangle((bounds.Left - ImageHelper.DefaultSmallIconSize.Width) - 3, bounds.Top, ImageHelper.DefaultSmallIconSize.Width, bounds.Height);
        }

        private void HideToolTip(TreeNode newHoverNode)
        {
            this.ItemToolTipTimer.Stop();
            VirtualToolTip.Default.HideTooltip();
            this.HoverNode = newHoverNode;
        }

        private void InvalidateNode(IVirtualFolder item)
        {
            TreeNode node;
            if (this.ItemNodeMap == null)
            {
                this.RebuildItemNodeMap();
            }
            if (this.ItemNodeMap.TryGetValue(item, out node) && (node != null))
            {
                Rectangle iconRect = this.GetIconRect(node);
                if (!(iconRect.IsEmpty || !base.ClientRectangle.IntersectsWith(iconRect)))
                {
                    base.Invalidate(iconRect);
                }
            }
        }

        private void ItemToolTipTimer_Tick(object sender, EventArgs e)
        {
            this.ItemToolTipTimer.Stop();
            if (this.HoverNode != null)
            {
                Point pt = base.PointToClient(Cursor.Position);
                TreeViewHitTestInfo info = base.HitTest(pt);
                if (((info.Location == TreeViewHitTestLocations.Image) || (info.Location == TreeViewHitTestLocations.Label)) && (this.HoverNode == info.Node))
                {
                    IVirtualItemUI tag = info.Node.Tag as IVirtualItemUI;
                    if (tag != null)
                    {
                        VirtualToolTip.Default.ShowTooltip(tag, this, pt.X, pt.Y + this.Cursor.GetPrefferedHeight());
                    }
                }
            }
        }

        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (this.ClearOnCollapse)
                {
                    e.Node.Nodes.Clear();
                    e.Node.Nodes.Add(new TreeNode());
                    this.ItemNodeMap = null;
                }
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
            }
            base.OnAfterCollapse(e);
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            if (this.WatchChanges)
            {
                IVirtualCachedFolder tag = e.Node.Tag as IVirtualCachedFolder;
                if (tag != null)
                {
                    this.WatchFolder(tag, true);
                }
            }
            base.OnAfterExpand(e);
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            IChangeVirtualItem tag = (IChangeVirtualItem) e.Node.Tag;
            try
            {
                tag.Name = e.Label;
                e.Node.Name = tag.FullName;
            }
            catch
            {
                e.CancelEdit = true;
            }
            base.OnAfterLabelEdit(e);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (this.CanRaiseSelectEvents)
            {
                base.OnAfterSelect(e);
            }
        }

        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            if (e.Node != null)
            {
                this.UpdateWatchFolder(e.Node, false);
            }
            base.OnBeforeCollapse(e);
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            if ((e.Node.Nodes.Count == 1) && string.IsNullOrEmpty(e.Node.Nodes[0].Text))
            {
                try
                {
                    this.FillTreeNode(e.Node);
                }
                catch (SystemException exception)
                {
                    this.OnAfterCollapse(new TreeViewEventArgs(e.Node));
                    MessageDialog.ShowException(this, exception);
                    e.Cancel = true;
                }
            }
            base.OnBeforeExpand(e);
        }

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            e.CancelEdit = !(e.Node.Tag is IChangeVirtualItem);
            base.OnBeforeLabelEdit(e);
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            if (this.Focused)
            {
                try
                {
                    IVirtualFolder tag = (IVirtualFolder) e.Node.Tag;
                    if ((tag.Attributes & FileAttributes.Offline) == 0)
                    {
                        foreach (IVirtualItem item in tag.GetContent())
                        {
                            break;
                        }
                    }
                    IChangeVirtualItem item2 = tag as IChangeVirtualItem;
                    if (!((item2 == null) || item2.Exists))
                    {
                        e.Node.Remove();
                        this.ItemNodeMap = null;
                        e.Cancel = true;
                    }
                }
                catch (SystemException exception)
                {
                    MessageDialog.ShowException(this, exception);
                    e.Cancel = true;
                }
            }
            if (this.CanRaiseSelectEvents)
            {
                base.OnBeforeSelect(e);
            }
        }

        protected override void OnGetNodeColors(GetNodeColorsEventArgs e)
        {
            bool flag = (e.State & TreeNodeStates.Selected) == 0;
            if (base.ExplorerTheme || (base.HotTracking && ((e.State & (TreeNodeStates.Hot | TreeNodeStates.Selected)) == TreeNodeStates.Hot)))
            {
                e.ForeColor = e.Node.ForeColor;
                flag = flag && !base.ExplorerTheme;
            }
            else if ((e.State & TreeNodeStates.Marked) > 0)
            {
                e.ForeColor = SystemColors.HighlightText;
                flag = false;
            }
            if (flag && ImageHelper.IsCloseColors(e.ForeColor, e.BackColor))
            {
                e.ForeColor = this.ForeColor;
            }
            base.OnGetNodeColors(e);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            TreeNode item = (TreeNode) e.Item;
            IVirtualFolder tag = (IVirtualFolder) item.Tag;
            if (tag.Parent != null)
            {
                Image dragImage = null;
                if (Settings.Default.IsShowIcons)
                {
                    dragImage = VirtualIcon.GetIcon(tag, ImageHelper.DefaultSmallIconSize);
                }
                Color foreColor = VirtualItemHelper.GetForeColor(tag, this.ForeColor);
                Point hotSpot = base.PointToClient(Cursor.Position);
                hotSpot.Offset(-item.Bounds.Left, -item.Bounds.Top);
                using (new DragImage(dragImage, tag.Name, this.Font, foreColor, hotSpot))
                {
                    base.DoDragDrop(new VirtualItemDataObject(tag, false), (DragDropEffects.Copy | ((tag is IChangeVirtualItem) ? DragDropEffects.Move : DragDropEffects.None)) | ((tag is ICreateVirtualLink) ? DragDropEffects.Link : DragDropEffects.None));
                }
            }
            base.OnItemDrag(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            TreeNode selectedNode = base.SelectedNode;
            if (selectedNode == null)
            {
                goto Label_00EE;
            }
            Keys keyData = e.KeyData;
            if (keyData == Keys.Apps)
            {
                goto Label_0043;
            }
            if (keyData != Keys.F2)
            {
                if (keyData == (Keys.Shift | Keys.F10))
                {
                    goto Label_0043;
                }
            }
            else
            {
                selectedNode.BeginEdit();
            }
            goto Label_00EE;
        Label_0043:
            if ((this.ContextMenuStrip == null) && (this.ContextMenu == null))
            {
                IVirtualItemUI tag = selectedNode.Tag as IVirtualItemUI;
                if (tag != null)
                {
                    ContextMenuStrip strip = tag.CreateContextMenuStrip(base.FindForm(), (base.LabelEdit ? ContextMenuOptions.CanRename : ((ContextMenuOptions) 0)) | ContextMenuOptions.Explore, new EventHandler<ExecuteVerbEventArgs>(this.ExecuteVerb));
                    if (strip != null)
                    {
                        int num = selectedNode.Bounds.Height / 2;
                        strip.Show(this, (int) (selectedNode.Bounds.Left + num), (int) (selectedNode.Bounds.Top + num));
                    }
                }
            }
        Label_00EE:
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ToolStripDropDownClosedEventHandler handler = null;
            TreeNode nodeAt = base.GetNodeAt(e.Location);
            if ((((e.Button == MouseButtons.Right) && (this.ContextMenuStrip == null)) && (this.ContextMenu == null)) && (nodeAt != null))
            {
                IVirtualItemUI tag = nodeAt.Tag as IVirtualItemUI;
                if (tag != null)
                {
                    ContextMenuStrip strip = tag.CreateContextMenuStrip(base.FindForm(), (base.LabelEdit ? ContextMenuOptions.CanRename : ((ContextMenuOptions) 0)) | ContextMenuOptions.Explore, new EventHandler<ExecuteVerbEventArgs>(this.ExecuteVerb));
                    if (strip != null)
                    {
                        this.RememberSelectedNode = base.SelectedNode;
                        this.SetSelectedNode(nodeAt);
                        if (handler == null)
                        {
                            handler = delegate (object sender2, ToolStripDropDownClosedEventArgs e2) {
                                this.SetSelectedNode(this.RememberSelectedNode);
                                this.RememberSelectedNode = null;
                            };
                        }
                        strip.Closed += handler;
                        strip.Show(this, e.Location);
                    }
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.HideToolTip(null);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.ShowTooltip(e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.BeginInvoke(new Action<MouseEventArgs>(this.ShowTooltip), new object[] { e });
            base.OnMouseWheel(e);
        }

        protected void OnNodeAdded(TreeViewEventArgs e)
        {
            if (this.NodeAdded != null)
            {
                this.NodeAdded(this, e);
            }
            if (this.ItemNodeMap != null)
            {
                this.ItemNodeMap.Add((IVirtualItem) e.Node.Tag, e.Node);
            }
        }

        protected override void OnPostDrawNode(PostDrawTreeNodeEventArgs e)
        {
            if (base.ImageList != null)
            {
                IVirtualItem tag = e.Node.Tag as IVirtualItem;
                if (tag != null)
                {
                    Rectangle iconRect = this.GetIconRect(e.Node);
                    if (!iconRect.IsEmpty && base.ClientRectangle.IntersectsWith(iconRect))
                    {
                        Image image = VirtualIcon.GetIcon(tag, ImageHelper.DefaultSmallIconSize, this.WatchChanges ? IconStyle.CanUseDelayedExtract : ((IconStyle) 0));
                        if (image != null)
                        {
                            Color empty = Color.Empty;
                            float blendLevel = 0.7f;
                            VirtualHighligher highlighter = VirtualIcon.GetHighlighter(tag);
                            if ((highlighter != null) && highlighter.AlphaBlend)
                            {
                                empty = highlighter.BlendColor;
                                blendLevel = highlighter.BlendLevel;
                            }
                            if ((base.HotTracking && !base.FullRowSelect) && !base.ExplorerTheme)
                            {
                                using (Brush brush = new SolidBrush(this.BackColor))
                                {
                                    e.Graphics.FillRectangle(brush, iconRect);
                                }
                            }
                            lock (image)
                            {
                                iconRect.Y += (iconRect.Height - image.Height) / 2;
                                if (empty.IsEmpty)
                                {
                                    e.Graphics.DrawImage(image, iconRect.Location);
                                }
                                else
                                {
                                    ImageHelper.DrawBlendImage(e.Graphics, image, empty, blendLevel, iconRect.X, iconRect.Y);
                                }
                            }
                            base.OnPostDrawNode(e);
                        }
                    }
                }
            }
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            if ((e.KeyState & 0x12) > 0)
            {
                e.Action = DragAction.Cancel;
            }
            base.OnQueryContinueDrag(e);
        }

        private void RebuildItemNodeMap()
        {
            this.ItemNodeMap = new Dictionary<IVirtualItem, TreeNode>();
            if (base.Nodes.Count != 0)
            {
                for (TreeNode node = base.Nodes[0]; node != null; node = node.NextVisibleNode)
                {
                    if (node.Tag != null)
                    {
                        this.ItemNodeMap.Add((IVirtualItem) node.Tag, node);
                    }
                }
            }
        }

        public void ResetVisualCache()
        {
            if (base.Nodes.Count != 0)
            {
                base.BeginUpdate();
                try
                {
                    for (TreeNode node = base.Nodes[0]; node != null; node = node.NextVisibleNode)
                    {
                        IVirtualFolder tag = (IVirtualFolder) node.Tag;
                        node.ForeColor = VirtualItemHelper.GetForeColor(tag, this.ForeColor);
                    }
                }
                finally
                {
                    base.EndUpdate();
                }
            }
        }

        private void SetSelectedNode(TreeNode node)
        {
            this.CanRaiseSelectEvents = false;
            try
            {
                base.SelectedNode = node;
            }
            finally
            {
                this.CanRaiseSelectEvents = true;
            }
        }

        private void SettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if ((propertyName != null) && (propertyName == "Highlighters"))
            {
                this.ResetVisualCache();
            }
        }

        private void ShowTooltip(MouseEventArgs e)
        {
            if (this.ShowItemToolTips)
            {
                TreeViewHitTestInfo info = base.HitTest(e.Location);
                if ((info.Location == TreeViewHitTestLocations.Image) || (info.Location == TreeViewHitTestLocations.Label))
                {
                    if (this.HoverNode != info.Node)
                    {
                        this.HideToolTip(info.Node);
                        if (info.Node.Tag is IVirtualItemUI)
                        {
                            this.ItemToolTipTimer.Start();
                        }
                    }
                }
                else
                {
                    this.HideToolTip(null);
                }
            }
        }

        public TreeNode ShowVirtualFolder(IVirtualFolder folder, bool expand)
        {
            Stack<IVirtualFolder> stack = new Stack<IVirtualFolder>();
            while (folder != null)
            {
                stack.Push(folder);
                folder = folder.Parent;
            }
            base.BeginUpdate();
            try
            {
                TreeNode node = null;
                if (stack.Count > 0)
                {
                    folder = stack.Pop();
                    node = base.Nodes[folder.FullName.ToLower()];
                    if (node == null)
                    {
                        this.Clear();
                        if (this.ShowAllRootFolders)
                        {
                            foreach (IVirtualFolder folder2 in VirtualItem.GetRootFolders())
                            {
                                TreeNode node2 = this.CreateTreeNode(folder2);
                                base.Nodes.Add(node2);
                                this.OnNodeAdded(new TreeViewEventArgs(node2));
                                if (folder2.Equals(folder))
                                {
                                    node = node2;
                                }
                            }
                        }
                        if (node == null)
                        {
                            node = this.CreateTreeNode(folder);
                            base.Nodes.Add(node);
                            this.OnNodeAdded(new TreeViewEventArgs(node));
                        }
                    }
                    while ((stack.Count > 0) && (node != null))
                    {
                        folder = stack.Pop();
                        string str = folder.FullName.ToLower();
                        if (!(node.IsExpanded && (node.Nodes[str] != null)))
                        {
                            this.FillTreeNode(node);
                        }
                        TreeNode node3 = node.Nodes[str];
                        if (node3 == null)
                        {
                            IPersistVirtualItem item = folder as IPersistVirtualItem;
                            if (!((item == null) || item.Exists))
                            {
                                break;
                            }
                            node3 = this.CreateTreeNode(folder);
                            node.Nodes.Add(node3);
                            this.OnNodeAdded(new TreeViewEventArgs(node3));
                        }
                        node = node3;
                    }
                    this.SetSelectedNode(node);
                    if (node != null)
                    {
                        node.EnsureVisible();
                        if (expand)
                        {
                            node.Expand();
                        }
                    }
                    return node;
                }
                this.Clear();
            }
            finally
            {
                base.EndUpdate();
            }
            return null;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            List<KeyValuePair<IVirtualFolder, WatcherChangeTypes>> changedFolders;
            lock (this.UpdateTimer)
            {
                changedFolders = this.ChangedFolders;
                this.ChangedFolders = new List<KeyValuePair<IVirtualFolder, WatcherChangeTypes>>();
                this.UpdateTimer.Stop();
            }
            if ((base.Nodes.Count > 0) && (changedFolders.Count > 0))
            {
                if (this.ItemNodeMap == null)
                {
                    this.RebuildItemNodeMap();
                }
                Dictionary<TreeNode, int> dictionary = new Dictionary<TreeNode, int>();
                foreach (KeyValuePair<IVirtualFolder, WatcherChangeTypes> pair in changedFolders)
                {
                    TreeNode node;
                    switch (pair.Value)
                    {
                        case WatcherChangeTypes.Created:
                        case WatcherChangeTypes.Renamed:
                            if ((pair.Key.Parent != null) && this.ItemNodeMap.TryGetValue(pair.Key.Parent, out node))
                            {
                                dictionary[node] = 0;
                            }
                            goto Label_013D;

                        default:
                            if (this.ItemNodeMap.TryGetValue(pair.Key, out node))
                            {
                                switch (pair.Value)
                                {
                                    case WatcherChangeTypes.Deleted:
                                        dictionary[node] = 1;
                                        break;
                                }
                            }
                            goto Label_013D;
                    }
                    dictionary[node] = 0;
                Label_013D:;
                }
                if (dictionary.Count > 0)
                {
                    base.BeginUpdate();
                    try
                    {
                        foreach (KeyValuePair<TreeNode, int> pair2 in dictionary)
                        {
                            switch (pair2.Value)
                            {
                                case 0:
                                {
                                    this.FillTreeNode(pair2.Key);
                                    continue;
                                }
                                case 1:
                                {
                                    pair2.Key.Remove();
                                    this.ItemNodeMap = null;
                                    continue;
                                }
                            }
                        }
                    }
                    finally
                    {
                        base.EndUpdate();
                    }
                }
            }
            this.UpdateTimer.Enabled = this.WatchChanges;
        }

        private void UpdateWatchFolder(TreeNode node, bool watch)
        {
            if (((node != null) || (base.Nodes.Count != 0)) && ((node == null) || ((node.Nodes.Count != 0) && node.IsExpanded)))
            {
                TreeNode firstNode;
                IVirtualCachedFolder tag;
                if (node == null)
                {
                    firstNode = base.Nodes[0];
                }
                else
                {
                    tag = node.Tag as IVirtualCachedFolder;
                    if (tag != null)
                    {
                        this.WatchFolder(tag, watch);
                    }
                    firstNode = node.FirstNode;
                }
                while (firstNode != null)
                {
                    if (firstNode.IsExpanded)
                    {
                        tag = firstNode.Tag as IVirtualCachedFolder;
                        if (tag != null)
                        {
                            this.WatchFolder(tag, watch);
                        }
                        this.UpdateWatchFolder(firstNode, watch);
                    }
                    firstNode = firstNode.NextNode;
                }
            }
        }

        private void WatchFolder(IVirtualCachedFolder folder, bool watch)
        {
            int num;
            if ((this.WatchFolderMap != null) && this.WatchFolderMap.TryGetValue(folder, out num))
            {
                num += watch ? 1 : -1;
                if (num == 0)
                {
                    folder.OnChanged -= new EventHandler<VirtualItemChangedEventArgs>(this.FolderChanged);
                    this.WatchFolderMap.Remove(folder);
                }
                else
                {
                    this.WatchFolderMap[folder] = num;
                }
            }
            else if (watch)
            {
                if (this.WatchFolderMap == null)
                {
                    this.WatchFolderMap = new Dictionary<IVirtualCachedFolder, int>();
                }
                folder.OnChanged += new EventHandler<VirtualItemChangedEventArgs>(this.FolderChanged);
                this.WatchFolderMap.Add(folder, 1);
            }
        }

        [DefaultValue(true)]
        public bool ClearOnCollapse
        {
            get
            {
                return this.FClearOnCollapse;
            }
            set
            {
                this.FClearOnCollapse = value;
                if (this.FClearOnCollapse && (base.Nodes.Count > 0))
                {
                    for (TreeNode node = base.Nodes[0]; node != null; node = node.NextVisibleNode)
                    {
                        if (!node.IsExpanded)
                        {
                            node.Nodes.Clear();
                            node.Nodes.Add(new TreeNode());
                            this.ItemNodeMap = null;
                        }
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IVirtualFolder CurrentFolder
        {
            get
            {
                if (base.SelectedNode != null)
                {
                    return (IVirtualFolder) base.SelectedNode.Tag;
                }
                return null;
            }
            set
            {
                if (value != this.CurrentFolder)
                {
                    if (value == null)
                    {
                        this.Clear();
                    }
                    else
                    {
                        this.ShowVirtualFolder(value, true);
                    }
                }
            }
        }

        [DefaultValue(typeof(CharacterCasing), "Normal")]
        public CharacterCasing FolderNameCasing
        {
            get
            {
                return this.FFolderNameCasing;
            }
            set
            {
                if (this.FFolderNameCasing != value)
                {
                    this.FFolderNameCasing = value;
                    if (base.Nodes.Count != 0)
                    {
                        base.BeginUpdate();
                        try
                        {
                            for (TreeNode node = base.Nodes[0]; node != null; node = node.NextVisibleNode)
                            {
                                node.Text = StringHelper.ApplyCharacterCasing(((IVirtualFolder) node.Tag).Name, this.FolderNameCasing);
                            }
                        }
                        finally
                        {
                            base.EndUpdate();
                        }
                    }
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowAllRootFolders
        {
            get
            {
                return this.FShowAllRootFolders;
            }
            set
            {
                this.FShowAllRootFolders = value;
            }
        }

        [DefaultValue(true)]
        public bool ShowItemIcons
        {
            get
            {
                return (base.ImageList != null);
            }
            set
            {
                if (value)
                {
                    ImageList list;
                    if (OS.IsWinVista)
                    {
                        list = ImageListCache.Get(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
                    }
                    else
                    {
                        list = ImageListCache.Get(ImageHelper.DefaultSmallIconSize);
                    }
                    if (list != base.ImageList)
                    {
                        base.ImageList = list;
                    }
                }
                else
                {
                    base.ImageList = null;
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowItemToolTips
        {
            get
            {
                return this.FShowItemToolTips;
            }
            set
            {
                this.FShowItemToolTips = value;
            }
        }

        [DefaultValue(false)]
        public bool WatchChanges
        {
            get
            {
                return this.FWatchChanges;
            }
            set
            {
                if (this.FWatchChanges != value)
                {
                    if (this.UpdateTimer == null)
                    {
                        this.UpdateTimer = new Timer();
                        this.UpdateTimer.Interval = 500;
                        this.UpdateTimer.Tick += new EventHandler(this.UpdateTimer_Tick);
                    }
                    this.FWatchChanges = value;
                    this.UpdateWatchFolder(null, this.FWatchChanges);
                    this.UpdateTimer.Stop();
                    this.UpdateTimer.Enabled = this.FWatchChanges;
                }
            }
        }
    }
}

