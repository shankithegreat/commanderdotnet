namespace Nomad.Controls.Specialized
{
    using Microsoft;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Layout;

    [DesignerCategory("Code")]
    public class VirtualPathBreadcrumb : ToolStrip
    {
        private ToolStripButton CommandButton;
        private static ToolStripRenderer DefaultRenderer;
        private ToolStripDropDownItem DriveItem;
        private string DropPathRoot;
        private static readonly object EventAfterPaint = new object();
        private static readonly object EventBeforePaint = new object();
        private static AutoCompleteProvider FAutoComplete;
        private IVirtualFolder FCurrentFolder;
        private bool FHideNotReadyDrives;
        private System.Windows.Forms.Layout.LayoutEngine FLayoutEngine;
        private ToolStripDropDownButton FMoreButton;
        private PathView FPathOptions = (PathView.ShowDriveMenuOnHover | PathView.ShowActiveState | PathView.ShowShortRootName);
        private ToolStripTextBox FPathTextBox;
        private ToolStripLabel FSimpleTextLabel;
        private BreadcrumbState FState;
        private ControlTimer FTooltipTimer;
        private BreadcrumbView FView;
        private ToolStripItem LastItem;
        private Timer OpenDropDownTimer;
        private ToolStripDropDownButton RecentButton;
        private ToolStripSeparator Separator;

        [Category("Appearance")]
        public event PaintEventHandler AfterPaint
        {
            add
            {
                base.Events.AddHandler(EventAfterPaint, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventAfterPaint, value);
            }
        }

        [Category("Appearance")]
        public event PaintEventHandler BeforePaint
        {
            add
            {
                base.Events.AddHandler(EventBeforePaint, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventBeforePaint, value);
            }
        }

        public event EventHandler CommandClicked;

        public event EventHandler<VirtualItemDragEventArg> DragDropOnItem;

        public event EventHandler<VirtualItemDragEventArg> DragOverItem;

        public event EventHandler<VirtualItemEventArgs> DriveClicked;

        public event EventHandler<VirtualItemEventArgs> FolderClicked;

        public VirtualPathBreadcrumb()
        {
            base.AllowItemReorder = false;
            base.AllowMerge = false;
            base.CanOverflow = false;
            if (!base.DesignMode)
            {
                base.DragDrop += new DragEventHandler(this.tsPath_DragDrop);
                base.DragEnter += new DragEventHandler(this.tsPath_DragEnter);
                base.DragLeave += new EventHandler(this.tsPath_DragLeave);
                base.DragOver += new DragEventHandler(this.tsPath_DragOver);
                this.AllowDrop = true;
                base.SuspendLayout();
                if (DefaultRenderer == null)
                {
                    ToolStripRenderer defaultRenderer = DefaultRenderer;
                }
                base.Renderer = DefaultRenderer = new PathBreadcrumbRenderer();
                this.RecentButton = new ToolStripDropDownButton();
                this.RecentButton.DisplayStyle = ToolStripItemDisplayStyle.None;
                this.RecentButton.Alignment = ToolStripItemAlignment.Right;
                this.RecentButton.DropDownDirection = ToolStripDropDownDirection.BelowLeft;
                this.RecentButton.DropDownOpening += new EventHandler(this.RecentButton_DropDownOpening);
                this.RecentButton.DropDownOpened += new EventHandler(this.RecentButton_DropDownOpened);
                this.RecentButton.DropDown.Closed += new ToolStripDropDownClosedEventHandler(this.RecentButton_DropDown_Closed);
                this.Items.Add(this.RecentButton);
                this.Separator = new ToolStripSeparator();
                this.Separator.Alignment = ToolStripItemAlignment.Right;
                this.Items.Add(this.Separator);
                this.CommandButton = new ToolStripButton();
                this.CommandButton.Image = IconSet.GetImage("Breadcrumb.Refresh");
                this.CommandButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
                this.CommandButton.Alignment = ToolStripItemAlignment.Right;
                this.CommandButton.Click += new EventHandler(this.CommandButton_Click);
                this.Items.Add(this.CommandButton);
                base.ResumeLayout();
            }
        }

        private static void BeforeSourcesLookup(object sender, CancelEventArgs e)
        {
            AutoCompleteProvider provider = (AutoCompleteProvider) sender;
            provider.UseFileSystemSource = Settings.Default.UseACSFileSystem;
            provider.UseEnvironmentVariablesSource = Settings.Default.UseACSEnvironmentVariables;
            provider.UseCustomSource = false;
            provider.GetCustomSource -= new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetKnownFoldersSource);
            provider.GetCustomSource -= new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetRecentFoldersSource);
            if (Settings.Default.UseACSKnownShellFolders)
            {
                provider.GetCustomSource += new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetKnownFoldersSource);
                provider.UseCustomSource = true;
            }
            if (Settings.Default.UseACSRecentItems)
            {
                provider.GetCustomSource += new EventHandler<GetCustomSourceEventArgs>(AutoCompleteEvents.GetRecentFoldersSource);
                provider.UseCustomSource = true;
            }
        }

        protected bool CheckState(BreadcrumbState value)
        {
            return ((this.FState & value) == value);
        }

        private void CleanToolstrip()
        {
            IDisposableContainer container = new DisposableContainer();
            if (this.DriveItem != null)
            {
                container.Add(this.DriveItem);
            }
            foreach (ToolStripItem item in this.Items)
            {
                if ((item.Tag is IVirtualItem) && (item != this.DriveItem))
                {
                    container.Add(item);
                }
            }
            container.Dispose();
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            if (this.CommandClicked != null)
            {
                this.CommandClicked(this, e);
            }
        }

        private ToolStripItem CreatePathItem(IVirtualFolder folder, IVirtualFolder parent, bool dropDownNeeded, bool useShortName)
        {
            ToolStripItem item;
            bool flag = (this.PathOptions & PathView.VistaLikeBreadcrumb) > PathView.ShowNormalRootName;
            bool flag2 = (parent == null) && !flag;
            if (!dropDownNeeded)
            {
                item = new ToolStripButton();
            }
            else
            {
                item = flag ? new VistaLikeBreadcrumbSplitButton() : new BreadcrumbSplitButton();
            }
            this.InitializeVirtualItemToolStrip(item, folder);
            if (useShortName && ((this.PathOptions & PathView.ShowShortRootName) > PathView.ShowNormalRootName))
            {
                item.Text = folder.ShortName;
            }
            item.DisplayStyle = (((this.PathOptions & PathView.ShowIconForEveryFolder) > PathView.ShowNormalRootName) || flag2) ? ToolStripItemDisplayStyle.ImageAndText : ToolStripItemDisplayStyle.Text;
            ToolStripSplitButton button = item as ToolStripSplitButton;
            if (button != null)
            {
                if (flag2)
                {
                    this.DriveItem = button;
                }
                else
                {
                    button.DropDownOpening += new EventHandler(this.tssbFolder_DropDownOpening);
                    button.DropDownClosed += new EventHandler(this.tssbDrive_DropDownClosed);
                }
                button.ButtonClick += new EventHandler(this.tssbFolder_ButtonClick);
                return item;
            }
            item.Click += new EventHandler(this.tssbFolder_ButtonClick);
            return item;
        }

        private void CurrentFolder_CachedContentChanged(object sender, EventArgs e)
        {
            if ((!base.IsDisposed && !base.Disposing) && base.IsHandleCreated)
            {
                ToolStripItem lastItem = this.LastItem;
                if ((lastItem != null) && (lastItem.Tag == sender))
                {
                    IVirtualCachedFolder tag = (IVirtualCachedFolder) lastItem.Tag;
                    CacheState cacheState = tag.CacheState;
                    bool dropDownNeeded = (cacheState == CacheState.Unknown) || ((cacheState & CacheState.HasFolders) > CacheState.Unknown);
                    bool flag2 = lastItem is ToolStripSplitButton;
                    if (dropDownNeeded ^ flag2)
                    {
                        IVirtualFolder parent = tag.Parent;
                        ToolStripItem item = this.CreatePathItem(tag, parent, dropDownNeeded, parent == null);
                        if (base.InvokeRequired)
                        {
                            base.Invoke(new Action<ToolStripItem, ToolStripItem, IVirtualFolder>(this.ReplaceLastItem), new object[] { lastItem, item, tag });
                        }
                        else
                        {
                            this.ReplaceLastItem(lastItem, item, tag);
                        }
                        this.LastItem = item;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.FMoreButton != null)
            {
                this.FMoreButton.Dispose();
            }
            if (this.FPathTextBox != null)
            {
                this.FPathTextBox.Dispose();
            }
            if (this.FSimpleTextLabel != null)
            {
                this.FSimpleTextLabel.Dispose();
            }
            if (this.CommandButton != null)
            {
                this.CommandButton.Dispose();
            }
            if (this.Separator != null)
            {
                this.Separator.Dispose();
            }
            if (this.RecentButton != null)
            {
                this.RecentButton.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Down:
                    if (!e.Handled)
                    {
                        this.OpenRecentFolders();
                        e.Handled = true;
                    }
                    break;

                case (Keys.Alt | Keys.Down):
                    this.OpenRecentFolders();
                    e.Handled = true;
                    return;
            }
        }

        private static string GetCommonPathRoot(IDataObject dataObject)
        {
            bool flag = true;
            string pathRoot = null;
            IEnumerable data = dataObject.GetData(DataFormats.FileDrop) as IEnumerable;
            if (data == null)
            {
                return null;
            }
            foreach (string str2 in data)
            {
                if (flag)
                {
                    pathRoot = Path.GetPathRoot(str2);
                    flag = false;
                }
                else if (!((pathRoot == null) || pathRoot.Equals(Path.GetPathRoot(str2), StringComparison.OrdinalIgnoreCase)))
                {
                    return null;
                }
                if (pathRoot == null)
                {
                    return null;
                }
            }
            return pathRoot;
        }

        private static void GetCurrentDirectory(object sender, GetCurrentDirectoryEventArgs e)
        {
            VirtualPathBreadcrumb parent = e.Target.Parent as VirtualPathBreadcrumb;
            if (parent != null)
            {
                CustomFileSystemFolder currentFolder = parent.CurrentFolder as CustomFileSystemFolder;
                if (currentFolder != null)
                {
                    e.CurrentDirectory = currentFolder.FullName;
                }
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size empty = Size.Empty;
            Size size2 = this.DefaultSize - base.Padding.Size;
            empty.Width = 2;
            empty.Height = Math.Max(0, size2.Height);
            if (this.Items.Count == 0)
            {
                empty = size2;
            }
            else
            {
                foreach (ToolStripItem item in this.Items)
                {
                    if (!(!item.Available || (item is ToolStripSeparator)))
                    {
                        Padding margin = item.Margin;
                        Size preferredSize = item.GetPreferredSize(proposedSize);
                        empty.Width += margin.Horizontal + preferredSize.Width;
                        empty.Height = Math.Max(empty.Height, margin.Vertical + preferredSize.Height);
                    }
                }
            }
            return (empty + base.Padding.Size);
        }

        private static DragDropEffects GetPrefferedDropEffect(IVirtualFolder root, string commonPathRoot)
        {
            return ((((root == null) || (commonPathRoot == null)) || !commonPathRoot.Equals(Path.GetPathRoot(root.FullName), StringComparison.OrdinalIgnoreCase)) ? DragDropEffects.Copy : DragDropEffects.Move);
        }

        private static DragDropEffects GetPrefferedDropEffect(IVirtualFolder root, IDataObject dataObject)
        {
            if (!VirtualClipboardItem.DataObjectContainItems(dataObject))
            {
                return DragDropEffects.None;
            }
            return GetPrefferedDropEffect(root, GetCommonPathRoot(dataObject));
        }

        private void InitializeToolStripDropDownItem(ToolStripDropDownItem DropDownItem)
        {
            if (DropDownItem.DropDown.OwnerItem == null)
            {
                DropDownItem.DropDown.OwnerItem = DropDownItem;
            }
            DropDownItem.DropDownOpened += new EventHandler(this.ToolStripDropDownItem_DropDownOpened);
            DropDownItem.DropDownClosed += new EventHandler(this.ToolStripDropDownItem_DropDownClosed);
            DropDownItem.DropDown.DragDrop += new DragEventHandler(this.tsPath_DragDrop);
            DropDownItem.DropDown.DragEnter += new DragEventHandler(this.tsPath_DragEnter);
            DropDownItem.DropDown.DragLeave += new EventHandler(this.tsPath_DragLeave);
            DropDownItem.DropDown.DragOver += new DragEventHandler(this.tsPath_DragOver);
            DropDownItem.DropDown.AllowDrop = true;
        }

        private void InitializeVirtualItemToolStrip(ToolStripItem toolStripItem, IVirtualItem item)
        {
            if (string.IsNullOrEmpty(toolStripItem.Text))
            {
                toolStripItem.Text = item.Name.Replace("&", "&&");
            }
            toolStripItem.Disposed += new EventHandler(VirtualItemToolStripEvents.Disposed);
            toolStripItem.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintImage);
            toolStripItem.MouseUp += new MouseEventHandler(VirtualItemToolStripEvents.MouseUp);
            toolStripItem.MouseHover += new EventHandler(VirtualItemToolStripEvents.MouseHover);
            toolStripItem.MouseLeave += new EventHandler(this.ToolStripItem_MouseLeave);
            toolStripItem.Tag = item;
            toolStripItem.AutoToolTip = false;
            ToolStripDropDownItem dropDownItem = toolStripItem as ToolStripDropDownItem;
            if (dropDownItem != null)
            {
                this.InitializeToolStripDropDownItem(dropDownItem);
            }
        }

        private void InitializeVirtualRootToolStrip(ToolStripItem toolStripItem, IVirtualItem item)
        {
            string str = item.Name.Replace("&", "&&");
            int index = str.IndexOf(':');
            if (index >= 0)
            {
                str = str.Insert(index - 1, "&");
            }
            toolStripItem.Text = str;
            this.InitializeVirtualItemToolStrip(toolStripItem, item);
            toolStripItem.Click += new EventHandler(this.tsbDrive_Click);
        }

        public static bool IsRootFolderVisible(IVirtualFolder folder)
        {
            if ((folder.Attributes & FileAttributes.Offline) > 0)
            {
                return false;
            }
            FileSystemDrive drive = folder as FileSystemDrive;
            if (drive != null)
            {
                string str = VirtualFilePanelSettings.Default.HideDrives.ToUpper();
                char ch = drive.ShortName.Substring(0, 1).ToUpper()[0];
                if (str.IndexOf(ch) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void MoreButton_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownButton button = (ToolStripDropDownButton) sender;
            IDisposableContainer container = new DisposableContainer();
            foreach (ToolStripItem item in this.Items)
            {
                if (item.IsOnOverflow)
                {
                    ToolStripItem toolStripItem = new ToolStripMenuItem(item.Text);
                    this.InitializeVirtualItemToolStrip(toolStripItem, (IVirtualItem) item.Tag);
                    toolStripItem.Click += new EventHandler(this.tssbFolder_ButtonClick);
                    container.Add(toolStripItem);
                    button.DropDownItems.Add(toolStripItem);
                }
            }
            button.DropDown.Tag = container;
        }

        protected virtual void OnAfterPaint(PaintEventArgs e)
        {
            PaintEventHandler handler = base.Events[EventAfterPaint] as PaintEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBeforePaint(PaintEventArgs e)
        {
            PaintEventHandler handler = base.Events[EventBeforePaint] as PaintEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnDriveClicked(VirtualItemEventArgs e)
        {
            if (this.DriveClicked != null)
            {
                this.DriveClicked(this, e);
            }
        }

        protected void OnFolderClicked(VirtualItemEventArgs e)
        {
            if (this.FolderClicked != null)
            {
                this.FolderClicked(this, e);
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.UnselectAll();
            VirtualToolTip.Default.HideTooltip();
            switch (this.View)
            {
                case BreadcrumbView.Drives:
                case BreadcrumbView.EnterPath:
                    this.View = BreadcrumbView.Breadcrumb;
                    break;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.SetState(BreadcrumbState.MouseOver, true);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.SetState(BreadcrumbState.MouseOver, false);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            if (((this.View == BreadcrumbView.Breadcrumb) && (base.GetItemAt(mea.Location) == null)) && (mea.Button == MouseButtons.Left))
            {
                this.View = BreadcrumbView.EnterPath;
                this.Select(true, true);
                this.PathTextBox.Focus();
            }
            base.OnMouseUp(mea);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.OnBeforePaint(e);
            base.OnPaint(e);
            this.OnAfterPaint(e);
        }

        public void OpenRecentFolders()
        {
            if (this.RecentButton.Visible)
            {
                this.RecentButton.ShowDropDown();
                foreach (ToolStripItem item in this.RecentButton.DropDownItems)
                {
                    if (string.Equals(item.Text, this.PathTextBox.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Select();
                        break;
                    }
                }
            }
        }

        private void PathTextBox_TextChanged(object sender, EventArgs e)
        {
            this.CommandButton.Enabled = !string.IsNullOrEmpty(this.PathTextBox.Text);
        }

        private IDisposableContainer PopulateToolStripFromRoot(ToolStrip toolStrip, IVirtualFolder currentRoot, bool hideNotReadyDrives)
        {
            IDisposableContainer container2;
            toolStrip.SuspendLayout();
            try
            {
                ToolStripSeparator separator;
                toolStrip.Items.Clear();
                VirtualProperty property = VirtualProperty.Get(VirtualFilePanelSettings.Default.DriveMenuProperty);
                if ((property != null) && (property.GroupName != "Volume"))
                {
                    property = null;
                }
                IDisposableContainer container = new DisposableContainer();
                System.Type type = null;
                int num = 0;
                Image image = Settings.Default.IsShowIcons ? ImageProvider.Default.GetDefaultIcon(DefaultIcon.Drive, ImageHelper.DefaultSmallIconSize) : null;
                foreach (IVirtualFolder folder in VirtualItem.GetRootFolders())
                {
                    if (!(!hideNotReadyDrives || IsRootFolderVisible(folder)))
                    {
                        num++;
                        continue;
                    }
                    if ((type != null) && (folder.GetType() != type))
                    {
                        separator = new ToolStripSeparator();
                        container.Add(separator);
                        toolStrip.Items.Add(separator);
                    }
                    type = folder.GetType();
                    string str = folder.Name.Replace("&", "&&");
                    int index = str.IndexOf(':');
                    if (index >= 0)
                    {
                        str = str.Insert(index - 1, "&");
                    }
                    ToolStripMenuItem toolStripItem = new ToolStripMenuItem();
                    this.InitializeVirtualRootToolStrip(toolStripItem, folder);
                    if (folder.Equals(currentRoot))
                    {
                        toolStripItem.Font = new Font(toolStripItem.Font, FontStyle.Bold);
                        container.Add(toolStripItem.Font);
                    }
                    if (property != null)
                    {
                        try
                        {
                            object obj2 = folder[property.PropertyId];
                            if (obj2 != null)
                            {
                                toolStripItem.ShortcutKeyDisplayString = property.ConvertToString(obj2);
                                toolStripItem.ShowShortcutKeys = true;
                            }
                        }
                        catch (SystemException exception)
                        {
                            if (!(exception is IOException) && !(exception is UnauthorizedAccessException))
                            {
                                throw;
                            }
                            toolStripItem.ShortcutKeyDisplayString = Resources.sDeviceNotReady;
                            toolStripItem.ShowShortcutKeys = true;
                        }
                    }
                    container.Add(toolStripItem);
                    toolStrip.Items.Add(toolStripItem);
                    VirtualItemToolStripEvents.UpdateItemImage(toolStripItem, folder);
                }
                if (num > 0)
                {
                    separator = new ToolStripSeparator();
                    container.Add(separator);
                    toolStrip.Items.Add(separator);
                    ToolStripItem item = new ToolStripMenuItem(string.Format(Resources.sShowHiddenDrives, num)) {
                        Tag = toolStrip
                    };
                    item.Click += new EventHandler(this.tsmiShowHidden_Click);
                    container.Add(item);
                    toolStrip.Items.Add(item);
                }
                container2 = container;
            }
            finally
            {
                toolStrip.ResumeLayout();
            }
            return container2;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (this.View)
            {
                case BreadcrumbView.Breadcrumb:
                    if (keyData == Keys.Down)
                    {
                        foreach (ToolStripItem item in this.Items)
                        {
                            if ((item.Available && item.Selected) && (item is ToolStripDropDownItem))
                            {
                                ((ToolStripDropDownItem) item).ShowDropDown();
                                return true;
                            }
                        }
                    }
                    break;

                case BreadcrumbView.Drives:
                    if (((keyData & Keys.Left) > Keys.None) || ((keyData & Keys.Right) > Keys.None))
                    {
                        VirtualToolTip.Default.HideTooltip();
                        if (Settings.Default.ShowItemToolTips)
                        {
                            this.TooltipTimer.Start(0, OS.MouseHoverTime);
                        }
                    }
                    break;

                case BreadcrumbView.EnterPath:
                    if (keyData != Keys.Return)
                    {
                        break;
                    }
                    this.CommandButton.PerformClick();
                    return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        protected void RaiseDragOverItem(IVirtualItem currentItem, DragDropEffects dropEffect, DragEventArgs e)
        {
            if (currentItem == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                VirtualItemDragEventArg arg = new VirtualItemDragEventArg(currentItem, e) {
                    Effect = dropEffect
                };
                switch ((e.KeyState & 0x2c))
                {
                    case 4:
                        arg.Effect = DragDropEffects.Move;
                        break;

                    case 8:
                        arg.Effect = DragDropEffects.Copy;
                        break;

                    case 0x20:
                        arg.Effect = DragDropEffects.Link;
                        break;
                }
                this.DragOverItem(this, arg);
                e.Effect = arg.Effect;
            }
        }

        private void RecentButton_DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            switch (e.CloseReason)
            {
                case ToolStripDropDownCloseReason.ItemClicked:
                case ToolStripDropDownCloseReason.Keyboard:
                    base.BeginInvoke(new MethodInvoker(this.PathTextBox.Focus));
                    break;
            }
        }

        private void RecentButton_DropDownOpened(object sender, EventArgs e)
        {
            this.View = BreadcrumbView.EnterPath;
            this.Select(true, true);
            this.PathTextBox.SelectAll();
        }

        private void RecentButton_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            item.DropDown.SuspendLayout();
            try
            {
                item.DropDownItems.Clear();
                string[] changeFolder = HistorySettings.Default.ChangeFolder;
                if (changeFolder != null)
                {
                    foreach (string str in changeFolder)
                    {
                        ToolStripItem item2 = new HistoryMenuItem {
                            Text = str,
                            Image = ImageProvider.Default.GetDefaultFolderIcon(str, ImageHelper.DefaultSmallIconSize)
                        };
                        item2.Click += new EventHandler(this.RecentMenuItem_Click);
                        item2.Tag = this.View != BreadcrumbView.EnterPath;
                        item.DropDownItems.Add(item2);
                    }
                }
                item.DropDown.MinimumSize = new Size(item.Bounds.Right - this.DisplayRectangle.Left, 0);
                item.DropDown.MaximumSize = item.DropDown.MinimumSize;
            }
            finally
            {
                item.DropDown.ResumeLayout();
            }
        }

        private void RecentMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            this.PathTextBox.Text = item.Text;
            if (Convert.ToBoolean(item.Tag))
            {
                this.CommandButton_Click(this.CommandButton, e);
            }
            this.PathTextBox.SelectAll();
        }

        private void RecreateDrivesToolstrip(bool hideNotReadyDrives)
        {
            base.SuspendLayout();
            try
            {
                this.CleanToolstrip();
                int num = 0;
                foreach (IVirtualFolder folder in VirtualItem.GetRootFolders())
                {
                    if (!(hideNotReadyDrives && !IsRootFolderVisible(folder)))
                    {
                        ToolStripItem toolStripItem = new ToolStripButton();
                        this.InitializeVirtualItemToolStrip(toolStripItem, folder);
                        toolStripItem.Text = folder.ShortName;
                        toolStripItem.Click += new EventHandler(this.tsbDrive_Click);
                        this.Items.Insert(num++, toolStripItem);
                        VirtualItemToolStripEvents.UpdateItemImage(toolStripItem, folder);
                    }
                }
            }
            finally
            {
                base.ResumeLayout();
            }
        }

        private void RecreatePathToolStrip(IVirtualFolder folder)
        {
            base.SuspendLayout();
            try
            {
                IVirtualFolder parent;
                this.CleanToolstrip();
                bool flag = (this.PathOptions & PathView.VistaLikeBreadcrumb) > PathView.ShowNormalRootName;
                bool dropDownNeeded = true;
                if (flag)
                {
                    IVirtualCachedFolder folder2 = folder as IVirtualCachedFolder;
                    if (folder2 != null)
                    {
                        CacheState cacheState = folder2.CacheState;
                        dropDownNeeded = (cacheState == CacheState.Unknown) || ((cacheState & CacheState.HasFolders) > CacheState.Unknown);
                        if (!this.CheckState(BreadcrumbState.HasCacheContentEvent))
                        {
                            folder2.CachedContentChanged += new EventHandler(this.CurrentFolder_CachedContentChanged);
                            this.SetState(BreadcrumbState.HasCacheContentEvent, true);
                        }
                    }
                }
                this.DriveItem = null;
                this.LastItem = null;
                bool flag3 = false;
                for (IVirtualFolder folder3 = folder; folder3 != null; folder3 = parent)
                {
                    parent = folder3.Parent;
                    flag3 |= parent != null;
                    ToolStripItem item = this.CreatePathItem(folder3, parent, dropDownNeeded, (parent == null) && flag3);
                    this.Items.Insert(0, item);
                    VirtualItemToolStripEvents.UpdateItemImage(item, folder3);
                    if (this.LastItem == null)
                    {
                        this.LastItem = item;
                    }
                    dropDownNeeded = true;
                }
                if (flag)
                {
                    this.DriveItem = new BreadcrumbDropDownButton();
                    this.DriveItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    this.DriveItem.ImageAlign = ContentAlignment.MiddleLeft;
                    this.DriveItem.Padding = new Padding(0, 0, 8, 0);
                    this.InitializeToolStripDropDownItem(this.DriveItem);
                    if ((this.Items.Count > 0) && (this.Items[0].Tag is IVirtualFolder))
                    {
                        this.DriveItem.Tag = this.Items[0].Tag;
                        if ((this.PathOptions & PathView.ShowFolderIcon) > PathView.ShowNormalRootName)
                        {
                            VirtualItemToolStripEvents.UpdateItemImage(this.DriveItem, folder);
                        }
                        else
                        {
                            VirtualItemToolStripEvents.UpdateItemImage(this.DriveItem, (IVirtualItem) this.DriveItem.Tag);
                        }
                    }
                    if (this.DriveItem.Image == null)
                    {
                        this.DriveItem.Image = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Drive, ImageHelper.DefaultSmallIconSize);
                    }
                    this.Items.Insert(0, this.DriveItem);
                }
                if (this.DriveItem != null)
                {
                    this.DriveItem.DropDownOpening += new EventHandler(this.tssbDrive_DropDownOpening);
                    this.DriveItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.tssbDrive_ItemClicked);
                    this.DriveItem.DropDownClosed += new EventHandler(this.tssbDrive_DropDownClosed);
                    if ((this.PathOptions & PathView.ShowDriveMenuOnHover) > PathView.ShowNormalRootName)
                    {
                        this.DriveItem.MouseHover += new EventHandler(this.tssbDrive_MouseHover);
                        this.DriveItem.MouseHover -= new EventHandler(VirtualItemToolStripEvents.MouseHover);
                        this.DriveItem.MouseLeave += new EventHandler(this.tssbDrive_MouseLeave);
                        this.DriveItem.DropDown.MouseLeave += new EventHandler(this.tssbDrive_DropDown_MouseLeave);
                    }
                }
            }
            finally
            {
                base.ResumeLayout();
            }
        }

        private void ReplaceLastItem(ToolStripItem lastItem, ToolStripItem item, IVirtualFolder folder)
        {
            int index = this.Items.IndexOf(lastItem);
            if (index >= 0)
            {
                base.SuspendLayout();
                this.Items.Insert(index, item);
                VirtualItemToolStripEvents.UpdateItemImage(item, folder);
                this.LastItem.Dispose();
                base.ResumeLayout();
            }
        }

        public void SelectDrive()
        {
            if ((this.DriveItem != null) && this.DriveItem.Visible)
            {
                this.Select(true, true);
                this.DriveItem.ShowDropDown();
                foreach (ToolStripItem item in this.DriveItem.DropDown.Items)
                {
                    IVirtualFolder tag = item.Tag as IVirtualFolder;
                    if ((tag != null) && tag.IsChild(this.CurrentFolder))
                    {
                        item.Select();
                    }
                }
            }
            else
            {
                ContextMenuStrip MenuStrip = new ContextMenuStrip();
                this.PopulateToolStripFromRoot(MenuStrip, null, this.HideNotReadyDrives);
                MenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.tssbDrive_ItemClicked);
                MenuStrip.Closed += delegate (object sender, ToolStripDropDownClosedEventArgs e) {
                    this.BeginInvoke(new MethodInvoker(MenuStrip.Dispose));
                };
                MenuStrip.Show(this, new Point(0, base.Height));
            }
        }

        public void SelectNextDrive(bool forward)
        {
            if (this.View == BreadcrumbView.Drives)
            {
                ToolStripItem item = null;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        int num2 = forward ? (i + 1) : (i - 1);
                        if (num2 < 0)
                        {
                            num2 = this.Items.Count - 1;
                        }
                        if (num2 >= this.Items.Count)
                        {
                            num2 = 0;
                        }
                        item = this.Items[num2];
                        break;
                    }
                }
                if (item != null)
                {
                    item.Select();
                    if (Settings.Default.ShowItemToolTips)
                    {
                        this.TooltipTimer.Start(0, OS.MouseHoverTime);
                    }
                }
            }
        }

        protected override void SetDisplayedItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            foreach (ToolStripItem item in this.Items)
            {
                if (item.IsOnOverflow)
                {
                    list.Add(item);
                }
            }
            base.SetDisplayedItems();
            if (this.LayoutEngine is PathLayoutEngine)
            {
                foreach (ToolStripItem item in list)
                {
                    PathLayoutEngine.SetToolStripItemPlacement(item, ToolStripItemPlacement.Overflow);
                }
            }
        }

        protected void SetState(BreadcrumbState state, bool value)
        {
            bool isHighlighted = this.IsHighlighted;
            bool flag2 = this.CheckState(BreadcrumbState.Active);
            if (value)
            {
                this.FState |= state;
            }
            else
            {
                this.FState &= ~state;
            }
            bool flag3 = false;
            if (this.IsHighlighted ^ isHighlighted)
            {
                flag3 = true;
            }
            else if (this.CheckState(BreadcrumbState.Active) ^ flag2)
            {
                flag3 = true;
            }
            if (flag3)
            {
                base.Invalidate();
            }
        }

        private void ShowSelectedItemTooltip()
        {
            foreach (ToolStripItem item in this.Items)
            {
                IVirtualItemUI mui;
                if (item.Selected && ((mui = item.Tag as IVirtualItemUI) != null))
                {
                    VirtualToolTip.Default.ShowTooltip(mui, this, item.Bounds.Left, item.Bounds.Bottom);
                    break;
                }
            }
        }

        public void StartDriveSelection()
        {
            if (this.View != BreadcrumbView.Drives)
            {
                this.View = BreadcrumbView.Drives;
                this.Select(true, true);
                if (this.CurrentFolder != null)
                {
                    foreach (ToolStripItem item in this.Items)
                    {
                        IVirtualFolder folder;
                        if ((item.Available && ((folder = item.Tag as IVirtualFolder) != null)) && folder.IsChild(this.CurrentFolder))
                        {
                            item.Select();
                        }
                    }
                }
            }
        }

        private void tmrOpenDropDown_Tick(object sender, EventArgs e)
        {
            this.OpenDropDownTimer.Stop();
            ToolStripDropDownItem itemAt = base.GetItemAt(base.PointToClient(Cursor.Position)) as ToolStripDropDownItem;
            if ((itemAt != null) && (itemAt == this.OpenDropDownTimer.Tag))
            {
                itemAt.ShowDropDown();
            }
            this.OpenDropDownTimer.Tag = null;
        }

        public void ToolStripDropDownItem_DropDownClosed(object sender, EventArgs e)
        {
            this.SetState(BreadcrumbState.DropDownOpened, false);
            VirtualToolTip.Default.HideTooltip();
        }

        public void ToolStripDropDownItem_DropDownOpened(object sender, EventArgs e)
        {
            this.SetState(BreadcrumbState.DropDownOpened, true);
            VirtualToolTip.Default.HideTooltip();
        }

        private void ToolStripItem_MouseLeave(object sender, EventArgs e)
        {
            VirtualToolTip.Default.HideTooltip();
        }

        private void TooltipTimer_Tick(object sender, TimerEventArgs e)
        {
            e.Cancel = true;
            this.ShowSelectedItemTooltip();
        }

        private void tsbDrive_Click(object sender, EventArgs e)
        {
            this.OnDriveClicked(new VirtualItemEventArgs((IVirtualFolder) ((ToolStripItem) sender).Tag));
        }

        private void tsmiShowHidden_Click(object sender, EventArgs e)
        {
            ToolStripDropDown tag = (ToolStripDropDown) ((ToolStripItem) sender).Tag;
            tag.AutoClose = true;
            tag.SuspendLayout();
            IVirtualFolder currentRoot = null;
            foreach (ToolStripItem item in tag.Items)
            {
                if (item.Font.Bold)
                {
                    currentRoot = item.Tag as IVirtualFolder;
                    break;
                }
            }
            ToolStripDropDownItem ownerItem = tag.OwnerItem as ToolStripDropDownItem;
            if (ownerItem != null)
            {
                this.tssbDrive_DropDownClosed(ownerItem, EventArgs.Empty);
            }
            IDisposableContainer container = this.PopulateToolStripFromRoot(tag, currentRoot, false);
            tag.ResumeLayout();
            if (ownerItem != null)
            {
                tag.Tag = container;
            }
        }

        private void tsPath_DragDrop(object sender, DragEventArgs e)
        {
            this.tsPath_DragLeave(sender, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.DragDropOnItem != null))
            {
                ToolStrip strip = (ToolStrip) sender;
                ToolStripItem itemAt = strip.GetItemAt(strip.PointToClient(new Point(e.X, e.Y)));
                IVirtualFolder item = null;
                if (itemAt != null)
                {
                    item = itemAt.Tag as IVirtualFolder;
                }
                if (item != null)
                {
                    this.DragDropOnItem(this, new VirtualItemDragEventArg(item, e));
                }
            }
        }

        private void tsPath_DragEnter(object sender, DragEventArgs e)
        {
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.CurrentFolder != null))
            {
                this.DropPathRoot = GetCommonPathRoot(e.Data);
                this.tsPath_DragOver(sender, e);
            }
        }

        private void tsPath_DragLeave(object sender, EventArgs e)
        {
            this.DropPathRoot = null;
            if (this.OpenDropDownTimer != null)
            {
                this.OpenDropDownTimer.Stop();
            }
            this.UnselectAll();
        }

        private void tsPath_DragOver(object sender, DragEventArgs e)
        {
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.DragOverItem != null))
            {
                ToolStrip ts = (ToolStrip) sender;
                ToolStripItem itemAt = ts.GetItemAt(ts.PointToClient(new Point(e.X, e.Y)));
                IVirtualFolder tag = null;
                if (itemAt != null)
                {
                    tag = itemAt.Tag as IVirtualFolder;
                }
                DragDropEffects none = DragDropEffects.None;
                if (tag != null)
                {
                    none = GetPrefferedDropEffect(VirtualItemHelper.GetFolderRoot(tag), this.DropPathRoot);
                }
                this.RaiseDragOverItem(tag, none, e);
                if ((this.OpenDropDownTimer != null) && (this.OpenDropDownTimer.Tag != itemAt))
                {
                    this.OpenDropDownTimer.Stop();
                }
                if (itemAt is ToolStripDropDownItem)
                {
                    if (this.OpenDropDownTimer == null)
                    {
                        this.OpenDropDownTimer = new Timer();
                        this.OpenDropDownTimer.Interval = 0x5dc;
                        this.OpenDropDownTimer.Tick += new EventHandler(this.tmrOpenDropDown_Tick);
                    }
                    this.OpenDropDownTimer.Tag = itemAt;
                    this.OpenDropDownTimer.Start();
                }
                if (e.Effect != DragDropEffects.None)
                {
                    itemAt.Select();
                }
                else
                {
                    ts.UnselectAll();
                }
            }
        }

        private void tssbDrive_DropDown_MouseLeave(object sender, EventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            if (!(!down.Stretch || down.OwnerItem.Bounds.Contains(down.OwnerItem.Owner.PointToClient(Cursor.Position))))
            {
                down.Hide();
            }
        }

        private void tssbDrive_DropDownClosed(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            item.DropDownItems.Clear();
            IDisposable tag = item.DropDown.Tag as IDisposable;
            if (tag != null)
            {
                if (base.IsHandleCreated)
                {
                    base.BeginInvoke(new MethodInvoker(tag.Dispose));
                }
                else
                {
                    tag.Dispose();
                }
            }
            item.DropDown.Tag = null;
            item.DropDown.Stretch = false;
        }

        private void tssbDrive_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            if (!(item.IsDisposed || (item.Owner == null)))
            {
                item.DropDown.Tag = this.PopulateToolStripFromRoot(item.DropDown, (IVirtualFolder) item.Tag, this.HideNotReadyDrives);
            }
        }

        private void tssbDrive_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag is ToolStrip)
            {
                ToolStripDropDown dropDown = sender as ToolStripDropDown;
                if (dropDown == null)
                {
                    dropDown = ((ToolStripDropDownItem) sender).DropDown;
                }
                dropDown.AutoClose = false;
            }
        }

        private void tssbDrive_MouseHover(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            if (!((item.IsDisposed || (item.Owner == null)) || item.DropDown.Visible))
            {
                item.ShowDropDown();
                item.DropDown.Stretch = true;
            }
        }

        private void tssbDrive_MouseLeave(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            if (!(!item.DropDown.Stretch || item.DropDown.Bounds.Contains(Cursor.Position)))
            {
                item.HideDropDown();
            }
        }

        private void tssbFolder_ButtonClick(object sender, EventArgs e)
        {
            this.OnFolderClicked(new VirtualItemEventArgs((IVirtualFolder) ((ToolStripItem) sender).Tag));
        }

        private void tssbFolder_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem) sender;
            IVirtualFolder tag = (IVirtualFolder) item.Tag;
            IVirtualFolder folder2 = ((this.PathOptions & PathView.VistaLikeBreadcrumb) > PathView.ShowNormalRootName) ? tag : tag.Parent;
            if (folder2 != null)
            {
                Image image = Settings.Default.IsShowIcons ? ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, ImageHelper.DefaultSmallIconSize) : null;
                IVirtualItemFilter hiddenItemsFilter = VirtualFilePanelSettings.Default.HiddenItemsFilter;
                IDisposableContainer container = new DisposableContainer();
                item.DropDown.SuspendLayout();
                try
                {
                    foreach (IVirtualFolder folder3 in folder2.GetFolders())
                    {
                        bool flag = ((this.PathOptions & PathView.VistaLikeBreadcrumb) == PathView.ShowNormalRootName) && folder3.Equals(tag);
                        if ((flag || (hiddenItemsFilter == null)) || !hiddenItemsFilter.IsMatch(folder3))
                        {
                            ToolStripItem toolStripItem = new ToolStripMenuItem();
                            this.InitializeVirtualItemToolStrip(toolStripItem, folder3);
                            toolStripItem.Image = image;
                            toolStripItem.MergeAction = MergeAction.Replace;
                            if (flag)
                            {
                                Font font = new Font(toolStripItem.Font, FontStyle.Bold);
                                container.Add(font);
                                toolStripItem.Font = font;
                            }
                            toolStripItem.Click += new EventHandler(this.tssbFolder_ButtonClick);
                            container.Add(toolStripItem);
                            item.DropDown.Items.Add(toolStripItem);
                        }
                    }
                }
                finally
                {
                    item.DropDown.ResumeLayout();
                }
                item.DropDown.Tag = container;
            }
        }

        public void UpdatePath()
        {
            this.RecreatePathToolStrip(this.FCurrentFolder);
        }

        [DefaultValue(false)]
        public bool Active
        {
            get
            {
                return this.CheckState(BreadcrumbState.Active);
            }
            set
            {
                this.SetState(BreadcrumbState.Active, value);
            }
        }

        [DefaultValue(true)]
        public override bool AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                base.AllowDrop = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), DefaultValue(false)]
        public bool AllowItemReorder
        {
            get
            {
                return base.AllowItemReorder;
            }
            set
            {
            }
        }

        [DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool AllowMerge
        {
            get
            {
                return base.AllowMerge;
            }
            set
            {
            }
        }

        protected static AutoCompleteProvider AutoComplete
        {
            get
            {
                if (FAutoComplete == null)
                {
                    FAutoComplete = new AutoCompleteProvider();
                    FAutoComplete.UseCustomSource = true;
                    FAutoComplete.UseEnvironmentVariablesSource = true;
                    FAutoComplete.UseFileSystemSource = true;
                    FAutoComplete.BeforeSourcesLookup += new EventHandler<CancelEventArgs>(VirtualPathBreadcrumb.BeforeSourcesLookup);
                    FAutoComplete.GetCurrentDirectory += new EventHandler<GetCurrentDirectoryEventArgs>(VirtualPathBreadcrumb.GetCurrentDirectory);
                    FAutoComplete.PreviewEnvironmentVariable += new EventHandler<PreviewEnvironmentVariableEventArgs>(AutoCompleteEvents.PreviewEnvironmentVariable);
                    FAutoComplete.PreviewFileSystemInfo += new EventHandler<PreviewFileSystemInfoEventArgs>(AutoCompleteEvents.PreviewFileSystemInfo);
                }
                return FAutoComplete;
            }
        }

        [DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool CanOverflow
        {
            get
            {
                return base.CanOverflow;
            }
            set
            {
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IVirtualFolder CurrentFolder
        {
            get
            {
                return this.FCurrentFolder;
            }
            set
            {
                if (value != this.FCurrentFolder)
                {
                    if (this.CheckState(BreadcrumbState.HasCacheContentEvent))
                    {
                        IVirtualCachedFolder fCurrentFolder = this.FCurrentFolder as IVirtualCachedFolder;
                        if (fCurrentFolder != null)
                        {
                            fCurrentFolder.CachedContentChanged -= new EventHandler(this.CurrentFolder_CachedContentChanged);
                        }
                        this.SetState(BreadcrumbState.HasCacheContentEvent, false);
                    }
                    this.FCurrentFolder = value;
                    if (this.View == BreadcrumbView.Breadcrumb)
                    {
                        this.RecreatePathToolStrip(this.FCurrentFolder);
                    }
                }
            }
        }

        [Browsable(false)]
        public string CurrentText
        {
            get
            {
                switch (this.View)
                {
                    case BreadcrumbView.Drives:
                        return ((this.SelectedFolder != null) ? this.SelectedFolder.FullName : string.Empty);

                    case BreadcrumbView.SimpleText:
                        return this.SimpleText;

                    case BreadcrumbView.EnterPath:
                        return Environment.ExpandEnvironmentVariables(this.PathTextBox.Text.Trim());
                }
                return ((this.CurrentFolder != null) ? this.CurrentFolder.FullName : string.Empty);
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(0, 1, 1, 2);
            }
        }

        [DefaultValue(true)]
        public bool HideNotReadyDrives
        {
            get
            {
                return this.FHideNotReadyDrives;
            }
            set
            {
                if (this.FHideNotReadyDrives != value)
                {
                    this.FHideNotReadyDrives = value;
                    if (this.View == BreadcrumbView.Drives)
                    {
                        this.RecreateDrivesToolstrip(this.HideNotReadyDrives);
                    }
                }
            }
        }

        protected bool IsHighlighted
        {
            get
            {
                return ((this.View == BreadcrumbView.Drives) || ((this.View == BreadcrumbView.Breadcrumb) && ((this.FState & (BreadcrumbState.DropDownOpened | BreadcrumbState.MouseOver)) > 0)));
            }
        }

        public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                if (this.FLayoutEngine == null)
                {
                    this.FLayoutEngine = new PathLayoutEngine();
                }
                return this.FLayoutEngine;
            }
        }

        protected ToolStripDropDownButton MoreButton
        {
            get
            {
                if (this.FMoreButton == null)
                {
                    this.FMoreButton = new BreadcrumbDropDownButton();
                    this.FMoreButton.Text = "…";
                    this.FMoreButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    this.FMoreButton.AutoToolTip = false;
                    this.InitializeToolStripDropDownItem(this.FMoreButton);
                    this.FMoreButton.DropDownOpening += new EventHandler(this.MoreButton_DropDownOpening);
                    this.FMoreButton.DropDownClosed += new EventHandler(this.tssbDrive_DropDownClosed);
                }
                return this.FMoreButton;
            }
        }

        [DefaultValue(13)]
        public PathView PathOptions
        {
            get
            {
                return this.FPathOptions;
            }
            set
            {
                if (this.FPathOptions != value)
                {
                    PathView fPathOptions = this.FPathOptions;
                    this.FPathOptions = value;
                    base.GripStyle = ((this.FPathOptions & PathView.ShowActiveState) > PathView.ShowNormalRootName) ? ToolStripGripStyle.Visible : ToolStripGripStyle.Hidden;
                    if ((fPathOptions & this.FPathOptions) != PathView.ShowActiveState)
                    {
                        this.RecreatePathToolStrip(this.FCurrentFolder);
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ToolStripTextBox PathTextBox
        {
            get
            {
                if (this.FPathTextBox == null)
                {
                    this.FPathTextBox = new ToolStripTextBox();
                    this.FPathTextBox.Margin = new Padding(4, 0, 2, 0);
                    this.FPathTextBox.BorderStyle = BorderStyle.None;
                    this.FPathTextBox.TextBox.TabStop = false;
                    this.FPathTextBox.AutoCompleteMode = Settings.Default.AutoCompleteMode;
                    AutoComplete.SetAutoComplete(this.FPathTextBox.TextBox, true);
                    this.FPathTextBox.TextChanged += new EventHandler(this.PathTextBox_TextChanged);
                    this.FPathTextBox.TextBox.KeyDown += new KeyEventHandler(this.EditTextBox_KeyDown);
                }
                return this.FPathTextBox;
            }
        }

        [Browsable(false)]
        public IVirtualFolder SelectedFolder
        {
            get
            {
                foreach (ToolStripItem item in this.Items)
                {
                    IVirtualFolder folder;
                    if (item.Selected && ((folder = item.Tag as IVirtualFolder) != null))
                    {
                        return folder;
                    }
                }
                return null;
            }
        }

        [DefaultValue(false)]
        public bool ShowProgress
        {
            get
            {
                return ((this.FView != BreadcrumbView.Drives) && this.CheckState(BreadcrumbState.ShowProgress));
            }
            set
            {
                if (this.ShowProgress != value)
                {
                    this.SetState(BreadcrumbState.ShowProgress, value);
                    if (this.FView != BreadcrumbView.Drives)
                    {
                        base.SuspendLayout();
                        try
                        {
                            switch (this.FView)
                            {
                                case BreadcrumbView.Breadcrumb:
                                case BreadcrumbView.SimpleText:
                                    this.CommandButton.Image = IconSet.GetImage(this.ShowProgress ? "Breadcrumb.Stop" : "Breadcrumb.Refresh");
                                    break;
                            }
                            ToolStripItem senderItem = ((this.PathOptions & PathView.ShowIconForEveryFolder) > PathView.ShowNormalRootName) ? this.LastItem : this.DriveItem;
                            if (senderItem != null)
                            {
                                if (value)
                                {
                                    senderItem.Image = Resources.ImageAnimatedThrobber;
                                }
                                else
                                {
                                    IVirtualFolder tag;
                                    senderItem.MergeAction = MergeAction.Remove;
                                    if ((this.PathOptions & (PathView.ShowFolderIcon | PathView.VistaLikeBreadcrumb)) == (PathView.ShowFolderIcon | PathView.VistaLikeBreadcrumb))
                                    {
                                        tag = (IVirtualFolder) this.LastItem.Tag;
                                    }
                                    else
                                    {
                                        tag = (IVirtualFolder) senderItem.Tag;
                                    }
                                    if (tag != null)
                                    {
                                        VirtualItemToolStripEvents.UpdateItemImage(senderItem, tag);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            base.ResumeLayout();
                        }
                    }
                }
            }
        }

        [DefaultValue("")]
        public string SimpleText
        {
            get
            {
                return ((this.FSimpleTextLabel != null) ? this.FSimpleTextLabel.Text : string.Empty);
            }
            set
            {
                this.SimpleTextLabel.Text = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStripLabel SimpleTextLabel
        {
            get
            {
                if (this.FSimpleTextLabel == null)
                {
                    this.FSimpleTextLabel = new ToolStripLabel();
                    this.FSimpleTextLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    this.FSimpleTextLabel.AutoSize = false;
                    this.FSimpleTextLabel.AutoToolTip = false;
                    this.FSimpleTextLabel.TextAlign = ContentAlignment.MiddleLeft;
                }
                return this.FSimpleTextLabel;
            }
        }

        private ControlTimer TooltipTimer
        {
            get
            {
                if (this.FTooltipTimer == null)
                {
                    this.FTooltipTimer = new ControlTimer(this);
                    this.FTooltipTimer.Tick += new EventHandler<TimerEventArgs>(this.TooltipTimer_Tick);
                }
                return this.FTooltipTimer;
            }
        }

        [DefaultValue(0)]
        public BreadcrumbView View
        {
            get
            {
                return this.FView;
            }
            set
            {
                if (this.FView != value)
                {
                    BreadcrumbView fView = this.FView;
                    this.FView = value;
                    bool flag = false;
                    bool flag2 = false;
                    base.SuspendLayout();
                    try
                    {
                        switch (this.FView)
                        {
                            case BreadcrumbView.Breadcrumb:
                            case BreadcrumbView.Drives:
                                flag = true;
                                flag2 = true;
                                goto Label_015E;

                            case BreadcrumbView.SimpleText:
                                flag2 = true;
                                if ((this.FSimpleTextLabel != null) && (this.FSimpleTextLabel.Owner == this))
                                {
                                    break;
                                }
                                this.Items.Add(this.SimpleTextLabel);
                                goto Label_015E;

                            case BreadcrumbView.EnterPath:
                                flag = true;
                                if ((this.FPathTextBox != null) && (this.FPathTextBox.Owner == this))
                                {
                                    goto Label_00D5;
                                }
                                this.Items.Add(this.PathTextBox);
                                goto Label_00E2;

                            default:
                                goto Label_015E;
                        }
                        this.SimpleTextLabel.Available = true;
                        goto Label_015E;
                    Label_00D5:
                        this.PathTextBox.Available = true;
                    Label_00E2:
                        this.CommandButton.Image = IconSet.GetImage("Breadcrumb.Go");
                        this.PathTextBox.Text = (this.CurrentFolder != null) ? this.CurrentFolder.FullName : string.Empty;
                        this.PathTextBox.SelectAll();
                        if (this.CurrentFolder is CustomFileSystemFolder)
                        {
                            try
                            {
                                Directory.SetCurrentDirectory(this.CurrentFolder.FullName);
                            }
                            catch
                            {
                            }
                        }
                    Label_015E:
                        if (this.FView != BreadcrumbView.EnterPath)
                        {
                            this.CommandButton.Image = IconSet.GetImage(this.ShowProgress ? "Breadcrumb.Stop" : "Breadcrumb.Refresh");
                            this.CommandButton.Enabled = true;
                        }
                        if ((this.FView == BreadcrumbView.Breadcrumb) && (fView != this.FView))
                        {
                            this.RecreatePathToolStrip(this.CurrentFolder);
                        }
                        if ((this.FView == BreadcrumbView.Drives) && (fView != this.FView))
                        {
                            this.RecreateDrivesToolstrip(this.HideNotReadyDrives);
                        }
                        this.CommandButton.Available = this.FView != BreadcrumbView.Drives;
                        this.Separator.Available = this.CommandButton.Available;
                        this.RecentButton.Available = this.CommandButton.Available && (this.FView != BreadcrumbView.SimpleText);
                        if (flag && (this.FSimpleTextLabel != null))
                        {
                            this.FSimpleTextLabel.Available = false;
                        }
                        if (flag2 && (this.FPathTextBox != null))
                        {
                            this.FPathTextBox.Available = false;
                        }
                    }
                    finally
                    {
                        base.ResumeLayout();
                    }
                }
            }
        }

        private class BreadcrumbDropDownButton : ToolStripDropDownButton
        {
            protected override ToolStripDropDown CreateDefaultDropDown()
            {
                return new VirtualPathBreadcrumb.BreadcrumbDropDownMenu();
            }
        }

        private class BreadcrumbDropDownMenu : ToolStripDropDownMenu
        {
            protected override bool ProcessDialogKey(Keys keyData)
            {
                bool flag = base.ProcessDialogKey(keyData);
                if (keyData == Keys.Escape)
                {
                    VirtualPathBreadcrumb breadcrumb = (base.OwnerItem != null) ? (base.OwnerItem.Owner as VirtualPathBreadcrumb) : null;
                    if (breadcrumb != null)
                    {
                        return (breadcrumb.ProcessDialogKey(keyData) || flag);
                    }
                }
                return flag;
            }
        }

        private class BreadcrumbSplitButton : ToolStripSplitButton
        {
            protected override ToolStripDropDown CreateDefaultDropDown()
            {
                return new VirtualPathBreadcrumb.BreadcrumbDropDownMenu();
            }
        }

        [Flags]
        protected enum BreadcrumbState
        {
            Active = 1,
            DropDownOpened = 4,
            HasCacheContentEvent = 0x10,
            MouseOver = 2,
            ShowProgress = 8
        }

        private class HistoryMenuItem : ToolStripMenuItem
        {
            public override Size GetPreferredSize(Size constrainingSize)
            {
                Size preferredSize = base.GetPreferredSize(constrainingSize);
                if (base.Owner is ToolStripDropDownMenu)
                {
                    preferredSize.Width = base.Owner.Width - base.Owner.Padding.Right;
                }
                return preferredSize;
            }
        }

        private class PathBreadcrumbRenderer : BreadcrumbToolStripRenderer
        {
            private Color GetApproximateItemBackColor(ToolStripItem item, ToolStrip toolStrip)
            {
                bool flag = item.Pressed || item.Selected;
                Color empty = Color.Empty;
                if (!flag)
                {
                    empty = item.BackColor;
                    if (empty.IsEmpty || (empty.A == 0))
                    {
                        empty = toolStrip.BackColor;
                    }
                }
                if (empty.IsEmpty || (empty.A == 0))
                {
                    ToolStripProfessionalRenderer rootRenderer = base.RootRenderer as ToolStripProfessionalRenderer;
                    if (rootRenderer != null)
                    {
                        if (item is ToolStripMenuItem)
                        {
                            if (item.Pressed)
                            {
                                empty = rootRenderer.ColorTable.MenuItemPressedGradientMiddle;
                            }
                            else if (item.Selected)
                            {
                                empty = rootRenderer.ColorTable.MenuItemSelected;
                            }
                            else
                            {
                                empty = rootRenderer.ColorTable.ToolStripDropDownBackground;
                            }
                        }
                        else if (item.Pressed)
                        {
                            ToolStripDropDownItem item2 = item as ToolStripDropDownItem;
                            if (!((item2 != null) && item2.DropDown.Visible))
                            {
                                empty = rootRenderer.ColorTable.ButtonPressedGradientMiddle;
                            }
                            else
                            {
                                empty = rootRenderer.ColorTable.MenuItemPressedGradientMiddle;
                            }
                        }
                        else if (item.Selected)
                        {
                            empty = rootRenderer.ColorTable.ButtonSelectedGradientMiddle;
                        }
                        else
                        {
                            empty = rootRenderer.ColorTable.ToolStripGradientMiddle;
                        }
                    }
                    ToolStripSystemRenderer baseRenderer = base.BaseRenderer as ToolStripSystemRenderer;
                    if (baseRenderer != null)
                    {
                        if ((item is ToolStripMenuItem) && flag)
                        {
                            empty = SystemColors.MenuHighlight;
                        }
                        else
                        {
                            empty = SystemColors.ButtonFace;
                        }
                    }
                }
                if (!flag)
                {
                    VirtualPathBreadcrumb breadcrumb = toolStrip as VirtualPathBreadcrumb;
                    if (breadcrumb != null)
                    {
                        empty = ImageHelper.MergeColors(empty, this.GetOverlayColor(breadcrumb));
                    }
                }
                return empty;
            }

            private Color GetBorderColor(VirtualPathBreadcrumb breadcrumb)
            {
                Color empty = Color.Empty;
                if ((breadcrumb.PathOptions & PathView.ShowActiveState) > PathView.ShowNormalRootName)
                {
                    if (breadcrumb.Active)
                    {
                        empty = Theme.Current.ThemeColors.ActiveBreadcrumbBorder;
                    }
                    else
                    {
                        empty = Theme.Current.ThemeColors.InactiveBreadcrumbBorder;
                    }
                }
                if (empty.IsEmpty)
                {
                    ToolStripProfessionalRenderer rootRenderer = base.RootRenderer as ToolStripProfessionalRenderer;
                    empty = (rootRenderer != null) ? rootRenderer.ColorTable.MenuBorder : PanelEx.DefaultBorderColor;
                }
                return empty;
            }

            private Color GetOverlayColor(VirtualPathBreadcrumb breadcrumb)
            {
                Color empty = Color.Empty;
                if (breadcrumb.View == BreadcrumbView.EnterPath)
                {
                    return SystemColors.Window;
                }
                if (breadcrumb.IsHighlighted)
                {
                    return Color.FromArgb(160, SystemColors.Window);
                }
                if ((breadcrumb.PathOptions & PathView.ShowActiveState) <= PathView.ShowNormalRootName)
                {
                    return empty;
                }
                if (breadcrumb.Active)
                {
                    return Theme.Current.ThemeColors.ActiveBreadcrumbBackground;
                }
                return Theme.Current.ThemeColors.InactiveBreadcrumbBackground;
            }

            protected override void InitializeItem(ToolStripItem item)
            {
                base.InitializeItem(item);
                if (item is ToolStripSeparator)
                {
                    item.Width = 1;
                }
                else if (!(item is ToolStripTextBox))
                {
                    item.Margin = new Padding(0, 1, 0, 1);
                }
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                VirtualPathBreadcrumb owner = e.Item.Owner as VirtualPathBreadcrumb;
                if ((((owner != null) && ((owner.PathOptions & PathView.ShowActiveState) > PathView.ShowNormalRootName)) && !e.Item.Selected) && !e.Item.Pressed)
                {
                    ThemeColorTable themeColors = Theme.Current.ThemeColors;
                    Color x = owner.Active ? themeColors.ActiveBreadcrumbText : themeColors.InactiveBreadcrumbText;
                    if (!x.IsEmpty)
                    {
                        Color approximateItemBackColor = this.GetApproximateItemBackColor(e.Item, owner);
                        if (!((approximateItemBackColor.IsEmpty || (approximateItemBackColor.A <= 0)) || ImageHelper.IsCloseColors(x, approximateItemBackColor)))
                        {
                            e.ArrowColor = x;
                        }
                    }
                }
                if ((owner != null) && (e.Item == owner.RecentButton))
                {
                    base.BaseRenderer.DrawArrow(e);
                }
                else
                {
                    base.OnRenderArrow(e);
                }
            }

            protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
            {
                IVirtualFolder tag = e.Item.Tag as IVirtualFolder;
                if ((e.Image == null) || (tag == null))
                {
                    base.OnRenderItemImage(e);
                }
                else
                {
                    Color empty = Color.Empty;
                    float blendLevel = 0.7f;
                    VirtualHighligher highlighter = VirtualIcon.GetHighlighter(tag);
                    if ((highlighter != null) && highlighter.AlphaBlend)
                    {
                        empty = highlighter.BlendColor;
                        blendLevel = highlighter.BlendLevel;
                    }
                    lock (e.Image)
                    {
                        if (empty.IsEmpty)
                        {
                            e.Graphics.DrawImage(e.Image, e.ImageRectangle.Location);
                        }
                        else
                        {
                            ImageHelper.DrawBlendImage(e.Graphics, e.Image, empty, blendLevel, e.ImageRectangle.X, e.ImageRectangle.Y);
                        }
                    }
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                Color foreColor;
                Color empty = Color.Empty;
                VirtualPathBreadcrumb toolStrip = e.ToolStrip as VirtualPathBreadcrumb;
                if ((((toolStrip != null) && ((toolStrip.PathOptions & PathView.ShowActiveState) > PathView.ShowNormalRootName)) && !e.Item.Selected) && !e.Item.Pressed)
                {
                    ThemeColorTable themeColors = Theme.Current.ThemeColors;
                    foreColor = toolStrip.Active ? themeColors.ActiveBreadcrumbText : themeColors.InactiveBreadcrumbText;
                    if (!foreColor.IsEmpty)
                    {
                        empty = this.GetApproximateItemBackColor(e.Item, e.ToolStrip);
                        if (!((empty.IsEmpty || (empty.A <= 0)) || ImageHelper.IsCloseColors(foreColor, empty)))
                        {
                            e.TextColor = foreColor;
                        }
                    }
                }
                if (e.Item is ToolStripLabel)
                {
                    e.TextFormat |= TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix;
                    Rectangle contentRectangle = e.Item.ContentRectangle;
                    contentRectangle.Y = e.TextRectangle.Y;
                    contentRectangle.Height = e.TextRectangle.Height;
                    if (e.Item.Image != null)
                    {
                        contentRectangle.X += e.Item.Image.Width;
                        contentRectangle.Width -= e.Item.Image.Width;
                    }
                    e.TextRectangle = contentRectangle;
                }
                else
                {
                    if (!e.Item.AutoSize)
                    {
                        e.TextFormat |= TextFormatFlags.WordEllipsis;
                    }
                    IVirtualItem tag = e.Item.Tag as IVirtualItem;
                    if (tag != null)
                    {
                        foreColor = VirtualItemHelper.GetForeColor(tag, Color.Empty);
                        if (!foreColor.IsEmpty)
                        {
                            if (empty.IsEmpty)
                            {
                                empty = this.GetApproximateItemBackColor(e.Item, e.ToolStrip);
                            }
                            if (!((empty.IsEmpty || (empty.A <= 0)) || ImageHelper.IsCloseColors(foreColor, empty)))
                            {
                                e.TextColor = foreColor;
                            }
                        }
                    }
                }
                base.OnRenderItemText(e);
            }

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                VirtualPathBreadcrumb toolStrip = e.ToolStrip as VirtualPathBreadcrumb;
                if (e.Vertical && (toolStrip != null))
                {
                    Rectangle displayRectangle = e.ToolStrip.DisplayRectangle;
                    using (Pen pen = new Pen(this.GetBorderColor(toolStrip)))
                    {
                        int num = e.Item.Width / 2;
                        e.Graphics.DrawLine(pen, num, displayRectangle.Top, num, displayRectangle.Bottom - 2);
                    }
                }
                else
                {
                    base.OnRenderSeparator(e);
                }
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                base.OnRenderToolStripBackground(e);
                VirtualPathBreadcrumb toolStrip = e.ToolStrip as VirtualPathBreadcrumb;
                if (toolStrip != null)
                {
                    Color overlayColor = this.GetOverlayColor(toolStrip);
                    Rectangle displayRectangle = e.ToolStrip.DisplayRectangle;
                    if (!overlayColor.IsEmpty)
                    {
                        using (Brush brush = new SolidBrush(overlayColor))
                        {
                            e.Graphics.FillRectangle(brush, displayRectangle);
                        }
                    }
                    displayRectangle.Width--;
                    displayRectangle.Height--;
                    using (Pen pen = new Pen(this.GetBorderColor(toolStrip)))
                    {
                        e.Graphics.DrawRectangle(pen, displayRectangle);
                    }
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDown)
                {
                    base.OnRenderToolStripBorder(e);
                }
            }
        }

        private class PathLayoutEngine : LayoutEngine
        {
            private static MethodInfo SetToolStripItemPlacementMethod = typeof(ToolStripItem).GetMethod("SetPlacement", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(ToolStripItemPlacement) }, null);

            static PathLayoutEngine()
            {
                Debug.Assert(SetToolStripItemPlacementMethod != null);
            }

            public override bool Layout(object container, LayoutEventArgs e)
            {
                ToolStripItem current;
                IEnumerator enumerator;
                if ((e.AffectedComponent == container) && (e.AffectedProperty == "ShowKeyboardFocusCues"))
                {
                    return false;
                }
                VirtualPathBreadcrumb breadcrumb = (VirtualPathBreadcrumb) container;
                if (breadcrumb.Items.Count == 0)
                {
                    return false;
                }
                Rectangle displayRectangle = breadcrumb.DisplayRectangle;
                if (displayRectangle.Width <= 0)
                {
                    return false;
                }
                if ((e.AffectedComponent == breadcrumb.FSimpleTextLabel) && (e.AffectedProperty == "Text"))
                {
                    return false;
                }
                if ((e.AffectedComponent == breadcrumb.FPathTextBox) && (e.AffectedProperty == "Text"))
                {
                    return false;
                }
                if (breadcrumb.FMoreButton != null)
                {
                    breadcrumb.Items.Remove(breadcrumb.FMoreButton);
                }
                int width = displayRectangle.Width;
                int num2 = 0;
                List<ToolStripItem> list = new List<ToolStripItem>(breadcrumb.Items.Count);
                using (enumerator = breadcrumb.Items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = (ToolStripItem) enumerator.Current;
                        if (current.Tag is IVirtualItem)
                        {
                            current.AutoSize = true;
                            current.Available = (breadcrumb.View == BreadcrumbView.Breadcrumb) || (breadcrumb.View == BreadcrumbView.Drives);
                            if (current.Available)
                            {
                                list.Add(current);
                            }
                        }
                        if (current.Available)
                        {
                            if (current.AutoSize)
                            {
                                if (current is ToolStripSeparator)
                                {
                                    current.Size = new Size(1, displayRectangle.Height);
                                }
                                else
                                {
                                    current.Size = new Size(current.GetPreferredSize(displayRectangle.Size).Width, displayRectangle.Height);
                                }
                            }
                            if (!(current is ToolStripSeparator))
                            {
                                if (current.Alignment == ToolStripItemAlignment.Right)
                                {
                                    width -= current.Width;
                                }
                                else
                                {
                                    num2 += current.Width;
                                }
                            }
                        }
                    }
                }
                if ((num2 > width) && (list.Count > 0))
                {
                    int num3;
                    ToolStripDropDownButton moreButton = breadcrumb.MoreButton;
                    moreButton.Size = new Size(moreButton.GetPreferredSize(displayRectangle.Size).Width, displayRectangle.Height);
                    num2 += moreButton.Width;
                    int[] numArray = new int[list.Count - 1];
                    for (num3 = 0; num3 < (list.Count - 1); num3++)
                    {
                        int num4 = 0;
                        int num5 = num3;
                        while ((num5 < (list.Count - 1)) && ((num2 - num4) > width))
                        {
                            num4 += list[num5].Width;
                            num5++;
                        }
                        if ((num2 - num4) <= width)
                        {
                            numArray[num3] = num5 - num3;
                        }
                        else
                        {
                            numArray[num3] = -1;
                        }
                    }
                    int num6 = ((breadcrumb.PathOptions & PathView.VistaLikeBreadcrumb) > PathView.ShowNormalRootName) ? 2 : 1;
                    int index = 0;
                    if (numArray.Length > num6)
                    {
                        index = num6;
                        for (num3 = num6; num3 < numArray.Length; num3++)
                        {
                            if ((numArray[num3] >= 0) && ((numArray[num3] < numArray[index]) || ((index == 0) && (numArray[num3] == numArray[index]))))
                            {
                                index = num3;
                            }
                        }
                    }
                    if (numArray.Length > 0)
                    {
                        int num8;
                        if (numArray[index] < 0)
                        {
                            num8 = list.Count - 2;
                        }
                        else
                        {
                            num8 = (index + numArray[index]) - 1;
                        }
                        for (num3 = num8; num3 >= index; num3--)
                        {
                            current = list[num3];
                            current.Available = false;
                            SetToolStripItemPlacement(current, ToolStripItemPlacement.Overflow);
                        }
                        breadcrumb.Items.Insert(index, moreButton);
                    }
                }
                Stack<ToolStripItem> stack = new Stack<ToolStripItem>();
                ToolStripItem item2 = null;
                Point location = displayRectangle.Location;
                using (enumerator = breadcrumb.Items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = (ToolStripItem) enumerator.Current;
                        if (current.Available)
                        {
                            if (current.Alignment == ToolStripItemAlignment.Right)
                            {
                                stack.Push(current);
                            }
                            else
                            {
                                location.X += current.Margin.Left;
                                breadcrumb.SetItemLocation(current, location);
                                SetToolStripItemPlacement(current, ToolStripItemPlacement.Main);
                                location.X += current.Width + current.Margin.Right;
                                item2 = current;
                            }
                        }
                    }
                }
                while (stack.Count > 0)
                {
                    current = stack.Pop();
                    Point point2 = new Point(displayRectangle.Right - current.Width, displayRectangle.Top);
                    if (current is ToolStripSeparator)
                    {
                        point2.X++;
                        displayRectangle.Width++;
                    }
                    else
                    {
                        displayRectangle.Width -= current.Width;
                    }
                    breadcrumb.SetItemLocation(current, point2);
                }
                if ((item2 != null) && (item2.Bounds.Right > (displayRectangle.Right + 1)))
                {
                    item2.Width = ((displayRectangle.Right - item2.Bounds.Left) - item2.Margin.Right) + 1;
                    item2.AutoSize = false;
                }
                ToolStripItem simpleTextLabel = null;
                switch (breadcrumb.View)
                {
                    case BreadcrumbView.SimpleText:
                        simpleTextLabel = breadcrumb.SimpleTextLabel;
                        break;

                    case BreadcrumbView.EnterPath:
                        simpleTextLabel = breadcrumb.PathTextBox;
                        break;
                }
                if (simpleTextLabel != null)
                {
                    simpleTextLabel.Available = true;
                    simpleTextLabel.Size = new Size(displayRectangle.Width - simpleTextLabel.Margin.Horizontal, displayRectangle.Height);
                    breadcrumb.SetItemLocation(simpleTextLabel, new Point(displayRectangle.Location.X + simpleTextLabel.Margin.Left, displayRectangle.Location.Y));
                }
                return breadcrumb.AutoSize;
            }

            internal static void SetToolStripItemPlacement(ToolStripItem item, ToolStripItemPlacement placement)
            {
                SetToolStripItemPlacementMethod.Invoke(item, new object[] { placement });
            }
        }

        private class VistaLikeBreadcrumbSplitButton : VirtualPathBreadcrumb.BreadcrumbSplitButton
        {
            protected override Point DropDownLocation
            {
                get
                {
                    Point dropDownLocation = base.DropDownLocation;
                    if (((base.DropDownDirection == ToolStripDropDownDirection.BelowRight) && (base.Owner != null)) && (base.Owner.PointToScreen(this.Bounds.Location).X == dropDownLocation.X))
                    {
                        dropDownLocation.X += base.Width / 3;
                    }
                    return dropDownLocation;
                }
            }
        }
    }
}

