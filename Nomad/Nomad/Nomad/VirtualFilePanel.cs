namespace Nomad
{
    using Microsoft;
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.Commons.Resources;
    using Nomad.Commons.Threading;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Actions;
    using Nomad.Controls.Specialized;
    using Nomad.Dialogs;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Network;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public class VirtualFilePanel : UserControl, ICloneable, IPersistComponentSettings, IUpdateCulture
    {
        private ListViewItem[] CacheEndItems;
        private ListViewItem CacheFirstItem;
        private ListViewItem[] CacheHitItems;
        private int CacheHitStartIndex;
        internal ContextMenuStrip cmsColumns;
        private ContextMenuStrip cmsListView;
        private ContextMenuStrip cmsPath;
        private ContextMenuStrip cmsToolStrip;
        private IContainer components = null;
        private Keys ControlModifierKeys;
        private IVirtualItem DelayedFocusedItem;
        private DragDropEffects DropEffect = DragDropEffects.None;
        private Dictionary<int, ListViewColumnInfo> FColumnInfoMap;
        private IVirtualFolder FCurrentFolder;
        private static HashSet<IVirtualItem> FCuttedItems;
        private int FDropItemIndex = -1;
        private CharacterCasing FFileNameCasing = CharacterCasing.Normal;
        private IVirtualItemFilter FFilter;
        private QuickFindOptions FFindOptions = (QuickFindOptions.AutoHide | QuickFindOptions.ExecuteOnEnter | QuickFindOptions.PrefixSearch);
        private System.Drawing.Color FFocusedBackColor = System.Drawing.Color.Silver;
        private System.Drawing.Color FFocusedForeColor;
        private CharacterCasing FFolderNameCasing = CharacterCasing.Normal;
        private string FFolderNameTemplate = "{0}";
        private History<IVirtualFolder> FHistory;
        private HashList<IVirtualItem> FItems;
        private VirtualItemContainer<IVirtualFolder> FLazyFolder;
        private System.Drawing.Color FListActiveBackColor;
        private System.Drawing.Color FListBackColor;
        private int FListViewColumnCount = 3;
        private int FocusedIndex = -1;
        private System.Drawing.Color FOddLineBackColor;
        private IVirtualItem FolderChangeItem;
        private VirtualPropertySet FolderChangePropertySet;
        private WatcherChangeTypes FolderChangeRequested;
        private const string FormatStoredDragOverItem = "Nomad_Stored_DragOverItem";
        private const string FormatStoredKeyState = "Nomad_Stored_KeyState";
        private PanelState FPanelState;
        private Form FParentForm;
        private System.Drawing.Color FSelectedForeColor = System.Drawing.Color.Red;
        private HashSet<IVirtualItem> FSelection = new HashSet<IVirtualItem>();
        private VirtualFilePanelSettings FSettings;
        private IComparer<IVirtualItem> FSort = VirtualItemComparer.DefaultSort;
        private int FStoredChangeVector;
        private Size FThumbnailSpacing = new Size(-1, -1);
        private PanelToolbar FToolbarsVisible = (PanelToolbar.Item | PanelToolbar.Folder);
        private const int HastListSortDivider = 0x180;
        private ListViewItem HoverItem;
        private ImageList imgThumbnail;
        private static WorkQueue InitializeQueue = new IdleQueue();
        private ToolTip ItemToolTip;
        private Point LastMousePosition;
        private ListViewEx listView;
        private int MouseColumnIndex = -1;
        private Orientation? NewOrientation;
        private int NewSplitterPercent = -1;
        private bool? RememberAutoSizeColumns;
        private Dictionary<int, ListViewColumnInfo> RememberColumnInfoMap;
        private string RememberDesktopIniPath;
        private DateTime RememberDesktopIniTime;
        private IVirtualItemFilter RememberFilter;
        private System.Drawing.Color RememberListBackColor;
        private System.Drawing.Color RememberListForeColor;
        private int? RememberListViewCount;
        private IComparer<IVirtualItem> RememberSort;
        private Control RememberSplitterActiveControl;
        private Size RememberThumbnailSize;
        private Size RememberThumbnailSpacing;
        private PanelView? RememberView;
        private ClickSelectMode RightMoveSelect;
        private bool SelectNameWithoutExt;
        private ContextMenuSource ShowContextMenu;
        private SplitContainer splitContainer;
        private Action<Tuple<Image, Size, WeakReference, WeakReference>> ThumbnailCallback;
        private Dictionary<Image, int> ThumbnailIconMap;
        private Dictionary<string, int> ThumbnailNameMap;
        private System.Windows.Forms.Timer tmrExpandNode;
        private System.Windows.Forms.Timer tmrToolTip;
        private System.Windows.Forms.Timer tmrUpdateItems;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private VirtualFolderTreeView treeView;
        private ToolStripButton tsbClearFilter;
        private ToolStripButton tsbClearSelection;
        private ToolStripButton tsbFindNext;
        private ToolStripButton tsbFindPrevious;
        private ToolStripButton tsbUnlockFolder;
        private ToolStripDropDownButton tsddFind;
        private ToolStrip tsFind;
        private ToolStrip tsFolderInfo;
        private ToolStrip tsItemInfo;
        private ToolStripLabel tslFindNonFound;
        private ToolStripLabel tslFolderCount;
        private ToolStripLabel tslFolderSize;
        private ToolStripLabel tslItemCount;
        private ToolStripLabel tslItemDate;
        private ToolStripLabel tslItemName;
        private ToolStripLabel tslItemSize;
        private ToolStripLabel tslSelectionInfo;
        private ToolStripMenuItem tsmiAlwaysShowFolders;
        private ToolStripMenuItem tsmiAutoHide;
        private ToolStripMenuItem tsmiAutoSizeColumns;
        private ToolStripMenuItem tsmiBack;
        private ToolStripMenuItem tsmiChangeFolder;
        private ToolStripMenuItem tsmiContains;
        private ToolStripMenuItem tsmiCopyPathAsText;
        private ToolStripMenuItem tsmiFindVisible;
        private ToolStripMenuItem tsmiFolderInfoVisible;
        private ToolStripMenuItem tsmiForward;
        private ToolStripMenuItem tsmiItemInfoVisible;
        private ToolStripMenuItem tsmiManageColumns;
        private ToolStripMenuItem tsmiQuickFilter;
        private ToolStripMenuItem tsmiQuickFind;
        private ToolStripMenuItem tsmiRefresh;
        private ToolStripMenuItem tsmiRememberWidthAsDefault;
        private ToolStripMenuItem tsmiResetDefaultWidth;
        private ToolStripMenuItem tsmiStartsWith;
        private VirtualPathBreadcrumb tsPath;
        private ToolStripSeparator tssColumns1;
        private ToolStripSeparator tssColumns2;
        private ToolStripSeparator tssColumns3;
        private ToolStripSeparator tssColumns4;
        private ToolStripSeparator tssFolder1;
        private ToolStripSeparator tssFolder2;
        private ToolStripSeparator tssFolder3;
        private ToolStripSeparator tssFolder4;
        private ToolStripSeparator tssItemDate;
        private ToolStripSeparator tssItemSize;
        private ToolStripSeparator tssPath1;
        private ToolStripSeparator tssPath2;
        private ToolStripSeparator tssPath3;
        private ToolStripSeparator tssToolstrip1;
        private ToolStripTextBox tstFind;
        private ListViewUpdateAction UpdateAction;
        private int UpdateCount;

        public event EventHandler<VirtualFolderChangedEventArgs> CurrentFolderChanged;

        public event EventHandler<VirtualItemDragEventArg> DragDropOnItem
        {
            add
            {
                this.FDragDropOnItem = (EventHandler<VirtualItemDragEventArg>) Delegate.Combine(this.FDragDropOnItem, value);
                this.tsPath.DragDropOnItem += value;
            }
            remove
            {
                this.FDragDropOnItem = (EventHandler<VirtualItemDragEventArg>) Delegate.Remove(this.FDragDropOnItem, value);
                this.tsPath.DragDropOnItem -= value;
            }
        }

        public event EventHandler<VirtualItemDragEventArg> DragOverItem
        {
            add
            {
                this.FDragOverItem = (EventHandler<VirtualItemDragEventArg>) Delegate.Combine(this.FDragOverItem, value);
                this.tsPath.DragOverItem += value;
            }
            remove
            {
                this.FDragOverItem = (EventHandler<VirtualItemDragEventArg>) Delegate.Remove(this.FDragOverItem, value);
                this.tsPath.DragOverItem -= value;
            }
        }

        public event EventHandler<HandleVirtualItemEventArgs> ExecuteItem;

        private event EventHandler<VirtualItemDragEventArg> FDragDropOnItem;

        private event EventHandler<VirtualItemDragEventArg> FDragOverItem;

        public event EventHandler<PreviewContextMenuEventArgs> PreviewContextMenu;

        public event EventHandler SelectionChanged;

        public VirtualFilePanel()
        {
            this.InitializeComponent();
            this.tslItemName.MouseHover += new EventHandler(VirtualItemToolStripEvents.MouseHover);
            this.tslItemName.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintForeColor);
            this.tslItemName.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintImage);
            if (OS.IsWinXP)
            {
                this.imgThumbnail.ColorDepth = ColorDepth.Depth32Bit;
            }
            this.tsFolderInfo.SuspendLayout();
            this.tsFolderInfo.LayoutStyle = ToolStripLayoutStyle.Table;
            TableLayoutSettings layoutSettings = (TableLayoutSettings) this.tsFolderInfo.LayoutSettings;
            layoutSettings.RowCount = 1;
            layoutSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutSettings.ColumnCount = 10;
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.tslFolderSize.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tslFolderSize, new TableLayoutPanelCellPosition(0, 0));
            this.tssFolder1.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tssFolder1, new TableLayoutPanelCellPosition(1, 0));
            this.tslItemCount.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tslItemCount, new TableLayoutPanelCellPosition(2, 0));
            this.tssFolder2.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tssFolder2, new TableLayoutPanelCellPosition(3, 0));
            this.tslFolderCount.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tslFolderCount, new TableLayoutPanelCellPosition(4, 0));
            this.tssFolder3.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tssFolder3, new TableLayoutPanelCellPosition(5, 0));
            this.tslSelectionInfo.Dock = DockStyle.Fill;
            layoutSettings.SetCellPosition(this.tslSelectionInfo, new TableLayoutPanelCellPosition(6, 0));
            this.tssFolder4.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tssFolder4, new TableLayoutPanelCellPosition(7, 0));
            this.tsbUnlockFolder.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tsbClearSelection, new TableLayoutPanelCellPosition(8, 0));
            this.tsbUnlockFolder.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tsbUnlockFolder, new TableLayoutPanelCellPosition(9, 0));
            this.tsbClearFilter.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tsbClearFilter, new TableLayoutPanelCellPosition(10, 0));
            this.tsFolderInfo.ResumeLayout();
            this.tsbClearSelection.Enabled = false;
            this.tsbUnlockFolder.Enabled = false;
            this.tsbClearFilter.Enabled = false;
            this.tsItemInfo.SuspendLayout();
            this.tsItemInfo.LayoutStyle = ToolStripLayoutStyle.Table;
            layoutSettings = (TableLayoutSettings) this.tsItemInfo.LayoutSettings;
            layoutSettings.RowCount = 1;
            layoutSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutSettings.ColumnCount = 5;
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.tslItemName.Dock = DockStyle.Fill;
            layoutSettings.SetCellPosition(this.tslItemName, new TableLayoutPanelCellPosition(0, 0));
            this.tssItemSize.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tssItemSize, new TableLayoutPanelCellPosition(1, 0));
            this.tslItemSize.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tslItemSize, new TableLayoutPanelCellPosition(2, 0));
            this.tssItemDate.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tssItemDate, new TableLayoutPanelCellPosition(3, 0));
            this.tslItemDate.Dock = DockStyle.Left;
            layoutSettings.SetCellPosition(this.tslItemDate, new TableLayoutPanelCellPosition(4, 0));
            this.tsItemInfo.ResumeLayout();
            BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
            if (argument != null)
            {
                argument.Localize(this);
            }
            this.tsFolderInfo.Tag = PanelToolbar.Folder;
            this.tsItemInfo.Tag = PanelToolbar.Item;
            this.tsFind.Tag = PanelToolbar.Find;
            this.tsmiFolderInfoVisible.Tag = this.tsFolderInfo;
            this.tsmiItemInfoVisible.Tag = this.tsItemInfo;
            this.tsmiFindVisible.Tag = this.tsFind;
            this.tsmiQuickFind.Tag = QuickFindOptions.QuickFilter;
            this.tsmiQuickFilter.Tag = QuickFindOptions.QuickFilter;
            this.tsmiStartsWith.Tag = QuickFindOptions.PrefixSearch;
            this.tsmiContains.Tag = QuickFindOptions.PrefixSearch;
            this.imgThumbnail.ImageSize = VirtualFilePanelSettings.Default.ThumbnailSize;
            this.ThumbnailIconMap = new Dictionary<Image, int>();
            this.ThumbnailNameMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.ThumbnailCallback = new Action<Tuple<Image, Size, WeakReference, WeakReference>>(this.ExtractThumbnail);
            this.tsmiManageColumns.Image = IconSet.GetImage(string.Format("{0}.{1}.Image", base.Name, this.tsmiManageColumns.Name));
            this.tsbClearSelection.Image = IconSet.GetImage(string.Format("{0}.{1}.Image", base.Name, this.tsbClearSelection.Name));
            this.tsbUnlockFolder.Image = IconSet.GetImage(string.Format("{0}.{1}.Image", base.Name, this.tsbUnlockFolder.Name));
            this.tsbClearFilter.Image = IconSet.GetImage(string.Format("{0}.{1}.Image", base.Name, this.tsbClearFilter.Name));
            this.FSettings = new VirtualFilePanelSettings();
            if (!base.DesignMode)
            {
                this.SetPanelState(PanelState.BindingsNeeded, true);
                this.UpdateColorsFromTheme();
                Application.Idle += new EventHandler(this.Event_ApplicationIdle);
                VirtualFilePanelSettings.Default.PropertyChanged += new PropertyChangedEventHandler(this.SettingPropertyChanged);
                Settings.Default.PropertyChanged += new PropertyChangedEventHandler(this.SettingPropertyChanged);
                base.Disposed += new EventHandler(this.VirtualFilePanel_Disposed);
                this.FStoredChangeVector = ChangeVector.Value;
            }
        }

        private void ApplyThumbnailSpacing(ref Size thumbnailSpacing)
        {
            if ((thumbnailSpacing.Width < 0) && (thumbnailSpacing.Height < 0))
            {
                this.listView.SetIconSpacing(-1, -1);
            }
            else
            {
                if (thumbnailSpacing.Width < 0)
                {
                    thumbnailSpacing.Width = (this.imgThumbnail.ImageSize.Width + SystemInformation.IconSpacingSize.Width) - SystemInformation.IconSize.Width;
                }
                if (thumbnailSpacing.Height < 0)
                {
                    thumbnailSpacing.Height = (this.imgThumbnail.ImageSize.Height + SystemInformation.IconSpacingSize.Height) - SystemInformation.IconSize.Height;
                }
                this.listView.SetIconSpacing(thumbnailSpacing);
            }
        }

        public bool Back()
        {
            if (this.History.BackCount > 0)
            {
                if (this.SetCurrentFolder(this.History.PeekBack(), false))
                {
                    this.History.Back();
                }
                return true;
            }
            return false;
        }

        protected void BeginListViewUpdate(bool beginUpdate, bool suspendLayout)
        {
            if (!this.CheckPanelState(PanelState.ProcessingEndUpdate))
            {
                this.UpdateCount++;
            }
            if (beginUpdate && ((this.UpdateAction & ListViewUpdateAction.EndUpdate) == 0))
            {
                this.listView.BeginUpdate();
                this.UpdateAction |= ListViewUpdateAction.EndUpdate;
            }
            if (suspendLayout && ((this.UpdateAction & ListViewUpdateAction.ResumeLayout) == 0))
            {
                this.listView.SuspendLayout();
                this.UpdateAction |= ListViewUpdateAction.ResumeLayout;
            }
        }

        public void BeginRenameItem()
        {
            if (this.treeView.Focused)
            {
                TreeNode selectedNode = this.treeView.SelectedNode;
                if (selectedNode != null)
                {
                    selectedNode.BeginEdit();
                }
            }
            else
            {
                ListViewItem focusedItem = this.listView.FocusedItem;
                if (focusedItem != null)
                {
                    if (!this.listView.IsEditing)
                    {
                        this.SelectNameWithoutExt = Settings.Default.SelectNameWithoutExt;
                    }
                    focusedItem.BeginEdit();
                }
            }
        }

        private void CancelAsyncFolder()
        {
            IAsyncVirtualFolder fCurrentFolder = this.FCurrentFolder as IAsyncVirtualFolder;
            if (fCurrentFolder != null)
            {
                fCurrentFolder.CancelAsync();
            }
            this.tsPath.ShowProgress = false;
            this.tsPath.View = BreadcrumbView.Breadcrumb;
        }

        private void ChangeFoundPanelState(bool Found)
        {
            if (Found)
            {
                this.tstFind.ResetBackColor();
                this.tstFind.ResetForeColor();
                this.tslFindNonFound.Visible = false;
            }
            else
            {
                this.tstFind.BackColor = Settings.TextBoxError;
                this.tstFind.ForeColor = SystemColors.HighlightText;
                this.tslFindNonFound.Visible = true;
            }
        }

        private bool CheckPanelState(PanelState state)
        {
            return ((this.FPanelState & state) == state);
        }

        private void ClearListViewCache()
        {
            this.CacheFirstItem = null;
            this.CacheHitItems = null;
            this.CacheEndItems = null;
            this.CacheHitStartIndex = -1;
        }

        private void ClearThumbnails()
        {
            lock (this.imgThumbnail)
            {
                this.imgThumbnail.Images.Clear();
                this.ThumbnailIconMap = new Dictionary<Image, int>();
                this.ThumbnailNameMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            }
            if (this.IsThumbnailView)
            {
                this.ClearListViewCache();
            }
        }

        public object Clone()
        {
            VirtualFilePanel panel = new VirtualFilePanel {
                Sort = this.Sort,
                Filter = this.Filter,
                FolderBarOrientation = this.FolderBarOrientation,
                FolderBarVisible = this.FolderBarVisible,
                SplitterPercent = this.SplitterPercent,
                RememberSort = this.RememberSort,
                RememberFilter = this.RememberFilter
            };
            panel.SetPanelState(PanelState.HasRememberFilter, this.CheckPanelState(PanelState.HasRememberFilter));
            panel.RememberAutoSizeColumns = this.RememberAutoSizeColumns;
            panel.RememberListViewCount = this.RememberListViewCount;
            panel.RememberThumbnailSize = this.RememberThumbnailSize;
            panel.RememberThumbnailSpacing = this.RememberThumbnailSpacing;
            panel.RememberView = this.RememberView;
            panel.FindOptions = this.FindOptions;
            ListViewColumnInfo[] columns = this.Columns;
            if (columns != null)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    columns[i] = (ListViewColumnInfo) columns[i].Clone();
                }
            }
            panel.Columns = columns;
            panel.View = this.View;
            panel.ListViewColumnCount = this.ListViewColumnCount;
            panel.ThumbnailSize = this.ThumbnailSize;
            panel.ThumbnailSpacing = this.ThumbnailSpacing;
            panel.ToolbarsVisible = this.ToolbarsVisible;
            panel.FHistory = (History<IVirtualFolder>) this.History.Clone();
            if (this.CurrentFolder == null)
            {
                panel.FLazyFolder = this.FLazyFolder;
            }
            else
            {
                panel.SetCurrentFolder(this.CurrentFolder, false);
            }
            panel.IsFolderLocked = this.IsFolderLocked;
            return panel;
        }

        private void cmsColumns_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.cmsColumns.Items.Clear();
            this.cmsColumns.Items.Add(this.tsmiManageColumns);
            IDisposable tag = this.cmsColumns.Tag as IDisposable;
            if (tag != null)
            {
                base.BeginInvoke(new MethodInvoker(tag.Dispose));
            }
            this.cmsColumns.Tag = null;
        }

        private void cmsColumns_Opening(object sender, CancelEventArgs e)
        {
            VirtualPropertySet availableProperties = this.AvailableProperties;
            int[] properties = new int[1];
            VirtualPropertySet set2 = new VirtualPropertySet(properties);
            if (this.listView.View == System.Windows.Forms.View.Details)
            {
                foreach (ColumnHeader header in this.listView.Columns)
                {
                    set2[((ListViewColumnInfo) header.Tag).PropertyId] = true;
                }
            }
            else
            {
                foreach (ListViewColumnInfo info in this.ColumnInfoMap.Values)
                {
                    if (info.Visible)
                    {
                        set2[info.PropertyId] = true;
                    }
                }
            }
            ListViewColumnInfo tag = null;
            if (this.MouseColumnIndex >= 0)
            {
                ColumnHeader header2 = this.listView.Columns[this.MouseColumnIndex];
                tag = (ListViewColumnInfo) header2.Tag;
            }
            this.cmsColumns.SuspendLayout();
            try
            {
                ToolStripMenuItem item;
                this.cmsColumns.Items.Clear();
                this.cmsColumns.Items.Add(this.tsmiManageColumns);
                this.cmsColumns.Items.Add(this.tssColumns1);
                IDisposableContainer container = new DisposableContainer();
                this.cmsColumns.Tag = container;
                KeyValueList<string, ListViewColumnInfo[]> columnTemplates = Settings.Default.ColumnTemplates;
                if ((columnTemplates != null) && (columnTemplates.Count > 0))
                {
                    IEnumerable<ListViewColumnInfo> columns = this.Columns;
                    foreach (KeyValuePair<string, ListViewColumnInfo[]> pair in columnTemplates)
                    {
                        item = new ToolStripMenuItem(pair.Key) {
                            Checked = (columns == null) ? false : this.ColumnsEqual(pair.Value, columns),
                            Tag = pair.Value
                        };
                        item.Click += new EventHandler(this.ColumnTemplateMenuItem_Click);
                        container.Add(item);
                        this.cmsColumns.Items.Add(item);
                    }
                    this.cmsColumns.Items.Add(this.tssColumns2);
                }
                foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) VirtualProperty.Visible)
                {
                    if (set2[property.PropertyId])
                    {
                        item = new ToolStripMenuItem(property.LocalizedName) {
                            Checked = true,
                            Enabled = property.PropertyId != 0,
                            Tag = property
                        };
                        item.Click += new EventHandler(this.ColumnMenuItem_Click);
                        if ((tag != null) && (tag.PropertyId == property.PropertyId))
                        {
                            item.Font = new Font(item.Font, FontStyle.Bold);
                            container.Add(item.Font);
                        }
                        if (!availableProperties[property.PropertyId])
                        {
                            item.ForeColor = SystemColors.GrayText;
                        }
                        container.Add(item);
                        this.cmsColumns.Items.Add(item);
                    }
                }
                ToolStripSeparator separator = null;
                Dictionary<int, ToolStripMenuItem> dictionary = new Dictionary<int, ToolStripMenuItem>();
                foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) VirtualProperty.Visible)
                {
                    if (availableProperties[property.PropertyId] && !set2[property.PropertyId])
                    {
                        ToolStripMenuItem item2;
                        if (separator == null)
                        {
                            separator = new ToolStripSeparator();
                            container.Add(separator);
                            this.cmsColumns.Items.Add(separator);
                        }
                        if (!dictionary.TryGetValue(property.GroupId, out item2))
                        {
                            item2 = new ToolStripMenuItem(property.LocalizedGroupName);
                            item2.DropDown.SuspendLayout();
                            container.Add(item2);
                            this.cmsColumns.Items.Add(item2);
                            dictionary.Add(property.GroupId, item2);
                        }
                        item = new ToolStripMenuItem(property.LocalizedName) {
                            Tag = property
                        };
                        item.Click += new EventHandler(this.ColumnMenuItem_Click);
                        container.Add(item);
                        item2.DropDownItems.Add(item);
                    }
                }
                foreach (ToolStripMenuItem item3 in dictionary.Values)
                {
                    item3.DropDown.ResumeLayout();
                }
                if (tag != null)
                {
                    this.cmsColumns.Items.Add(this.tssColumns3);
                    bool flag = !this.AutoSizeColumns;
                    if (flag)
                    {
                        int num = VirtualFilePanelSettings.DefaultColumnWidth(tag.PropertyId, this.listView.Font);
                        flag = tag.DefaultWidth != num;
                    }
                    this.tsmiRememberWidthAsDefault.Tag = this.MouseColumnIndex;
                    this.tsmiRememberWidthAsDefault.Enabled = flag;
                    this.cmsColumns.Items.Add(this.tsmiRememberWidthAsDefault);
                    this.tsmiResetDefaultWidth.Tag = this.MouseColumnIndex;
                    this.tsmiResetDefaultWidth.Enabled = !this.AutoSizeColumns || (tag.DefaultWidth >= 0);
                    this.cmsColumns.Items.Add(this.tsmiResetDefaultWidth);
                }
                this.cmsColumns.Items.Add(this.tssColumns4);
                this.tsmiAutoSizeColumns.Checked = this.AutoSizeColumns;
                this.cmsColumns.Items.Add(this.tsmiAutoSizeColumns);
            }
            finally
            {
                this.cmsColumns.ResumeLayout();
                this.MouseColumnIndex = -1;
            }
        }

        private void cmsListView_Opening(object sender, CancelEventArgs e)
        {
            Point point;
            e.Cancel = true;
            ListViewItem itemAt = null;
            ContextMenuStrip contextMenu = null;
            switch (this.ShowContextMenu)
            {
                case ContextMenuSource.Mouse:
                    this.ShowContextMenu = ContextMenuSource.Default;
                    point = this.listView.PointToClient(Cursor.Position);
                    if (this.MouseColumnIndex >= 0)
                    {
                        contextMenu = this.cmsColumns;
                        break;
                    }
                    itemAt = this.listView.GetItemAt(point.X, point.Y);
                    break;

                case ContextMenuSource.Ignore:
                    this.ShowContextMenu = ContextMenuSource.Default;
                    return;

                default:
                    itemAt = this.listView.FocusedItem;
                    if (itemAt == null)
                    {
                        return;
                    }
                    point = new Point(itemAt.Position.X + (itemAt.Bounds.Height / 2), itemAt.Position.Y + (itemAt.Bounds.Height / 2));
                    break;
            }
            if (contextMenu == null)
            {
                IVirtualItem tag;
                ContextMenuOptions options = this.treeView.Visible ? ContextMenuOptions.Explore : ((ContextMenuOptions) 0);
                if (itemAt != null)
                {
                    tag = (IVirtualItem) itemAt.Tag;
                    options |= ContextMenuOptions.CanRename;
                }
                else
                {
                    tag = this.CurrentFolder;
                }
                IVirtualFolderUI currentFolder = this.CurrentFolder as IVirtualFolderUI;
                if (((contextMenu == null) && this.FSelection.Contains(tag)) && (currentFolder != null))
                {
                    contextMenu = currentFolder.CreateContextMenuStrip(base.FindForm(), this.Selection, options & ~ContextMenuOptions.CanRename, new EventHandler<ExecuteVerbEventArgs>(this.ExecuteVerb));
                }
                else
                {
                    PreviewContextMenuEventArgs args = new PreviewContextMenuEventArgs(tag, contextMenu, options);
                    if (this.PreviewContextMenu != null)
                    {
                        this.PreviewContextMenu(this, args);
                    }
                    if (!args.Cancel)
                    {
                        contextMenu = args.ContextMenu;
                        options = args.Options;
                        if (contextMenu == null)
                        {
                            IVirtualItemUI mui = tag as IVirtualItemUI;
                            if (mui != null)
                            {
                                contextMenu = mui.CreateContextMenuStrip(base.FindForm(), options, new EventHandler<ExecuteVerbEventArgs>(this.ExecuteVerb));
                            }
                        }
                    }
                    else
                    {
                        contextMenu = null;
                    }
                }
            }
            this.HideItemToolTip();
            if (contextMenu != null)
            {
                contextMenu.Show(this.listView, point);
            }
        }

        private void cmsPath_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            ToolStrip sourceControl = (ToolStrip) strip.SourceControl;
            ToolStripItem itemAt = sourceControl.GetItemAt(sourceControl.PointToClient(strip.Location));
            e.Cancel = (itemAt != null) && (itemAt.Tag is IVirtualItem);
        }

        private void cmsToolbars_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            ToolStrip sourceControl = (ToolStrip) strip.SourceControl;
            ToolStripItem itemAt = sourceControl.GetItemAt(sourceControl.PointToClient(Cursor.Position));
            if ((itemAt != null) && (itemAt.Tag is IVirtualItemUI))
            {
                ContextMenuStrip strip3 = ((IVirtualItemUI) itemAt.Tag).CreateContextMenuStrip(base.FindForm(), 0, new EventHandler<ExecuteVerbEventArgs>(this.ExecuteVerb));
                if (strip3 != null)
                {
                    strip3.Show(strip.Location);
                    e.Cancel = true;
                    return;
                }
            }
            foreach (ToolStripItem item2 in strip.Items)
            {
                if (item2.Tag == sourceControl)
                {
                    item2.Font = new Font(item2.Font, FontStyle.Bold);
                }
                else
                {
                    item2.ResetFont();
                }
            }
            this.tsmiAutoHide.Checked = (this.FindOptions & QuickFindOptions.AutoHide) > 0;
        }

        private void ColumnMenuItem_Click(object sender, EventArgs e)
        {
            ListViewColumnInfo info;
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            VirtualProperty tag = (VirtualProperty) item.Tag;
            if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
            {
                this.RememberDesktopIniPath = null;
                this.RememberColumnInfoMap = null;
            }
            if (this.listView.View != System.Windows.Forms.View.Details)
            {
                if (!this.ColumnInfoMap.TryGetValue(tag.PropertyId, out info))
                {
                    info = new ListViewColumnInfo(tag.PropertyId, VirtualFilePanelSettings.DefaultColumnWidth(tag.PropertyId, this.listView.Font), false);
                    int displayIndex = -1;
                    foreach (ListViewColumnInfo info2 in this.ColumnInfoMap.Values)
                    {
                        if (info2.DisplayIndex > displayIndex)
                        {
                            displayIndex = info2.DisplayIndex;
                        }
                    }
                    info.DisplayIndex = displayIndex + 1;
                    this.ColumnInfoMap.Add(tag.PropertyId, info);
                }
                info.Visible = !item.Checked;
            }
            else
            {
                VirtualItemComparer fSort;
                if (item.Checked)
                {
                    fSort = this.FSort as VirtualItemComparer;
                    foreach (ColumnHeader header in this.listView.Columns)
                    {
                        info = (ListViewColumnInfo) header.Tag;
                        if (info.PropertyId == tag.PropertyId)
                        {
                            info.Visible = false;
                            this.listView.Columns.Remove(header);
                            if ((fSort != null) && (fSort.ComparePropertyId == tag.PropertyId))
                            {
                                this.listView.SortColumn = -1;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    ColumnHeader header2 = new ColumnHeader {
                        Text = tag.LocalizedName
                    };
                    if (this.ColumnInfoMap.TryGetValue(tag.PropertyId, out info))
                    {
                        header2.TextAlign = info.TextAlign;
                        header2.Width = info.Width;
                    }
                    else
                    {
                        header2.Width = VirtualFilePanelSettings.DefaultColumnWidth(tag.PropertyId, this.listView.Font);
                        info = new ListViewColumnInfo {
                            PropertyId = tag.PropertyId,
                            TextAlign = header2.TextAlign,
                            Width = header2.Width
                        };
                        this.ColumnInfoMap.Add(tag.PropertyId, info);
                    }
                    info.Visible = true;
                    header2.Tag = info;
                    this.listView.Columns.Add(header2);
                    info.DisplayIndex = header2.DisplayIndex;
                    fSort = this.FSort as VirtualItemComparer;
                    if ((fSort != null) && (fSort.ComparePropertyId == tag.PropertyId))
                    {
                        this.listView.SetSortColumn(header2.Index, fSort.SortDirection);
                    }
                }
                this.ClearListViewCache();
                this.UpdateListView();
            }
        }

        private bool ColumnsEqual(IEnumerable<ListViewColumnInfo> list1, IEnumerable<ListViewColumnInfo> list2)
        {
            using (IEnumerator<ListViewColumnInfo> enumerator = list1.GetEnumerator())
            {
                using (IEnumerator<ListViewColumnInfo> enumerator2 = list2.GetEnumerator())
                {
                    bool flag;
                    bool flag2;
                    bool flag4;
                    goto Label_009F;
                Label_0015:;
                    while ((flag = enumerator.MoveNext()) && !enumerator.Current.Visible)
                    {
                    }
                    while ((flag2 = enumerator2.MoveNext()) && !enumerator2.Current.Visible)
                    {
                    }
                    if (flag ^ flag2)
                    {
                        return false;
                    }
                    if (!flag)
                    {
                        return true;
                    }
                    if (!enumerator.Current.Equals(enumerator2.Current, !this.AutoSizeColumns))
                    {
                        return false;
                    }
                Label_009F:
                    flag4 = true;
                    goto Label_0015;
                }
            }
        }

        private void ColumnTemplateMenuItem_Click(object sender, EventArgs e)
        {
            this.Columns = (ListViewColumnInfo[]) ((ToolStripItem) sender).Tag;
        }

        private ListViewItem CreateListViewItem(IVirtualItem item, int itemIndex, bool isParentFolder)
        {
            ListViewItem item2 = new ListViewItem {
                Tag = item
            };
            if (isParentFolder)
            {
                item2.Text = "..";
            }
            else if (item is IVirtualFolder)
            {
                item2.Text = StringHelper.ApplyCharacterCasing(string.Format(this.FolderNameTemplate, item.Name), this.FolderNameCasing);
            }
            else
            {
                item2.Text = StringHelper.ApplyCharacterCasing(item.Name, this.FileNameCasing);
            }
            if (!base.IsDisposed && !base.Disposing)
            {
                System.Windows.Forms.View view = this.listView.View;
                switch (view)
                {
                    case System.Windows.Forms.View.LargeIcon:
                        if (this.listView.LargeImageList == this.imgThumbnail)
                        {
                            int num;
                            ImageList list;
                            lock ((list = this.imgThumbnail))
                            {
                                if (this.ThumbnailNameMap.TryGetValue(item.FullName, out num))
                                {
                                    item2.ImageIndex = num;
                                }
                            }
                            if (item2.ImageIndex < 0)
                            {
                                Size defaultLargeIconSize = new Size(0x30, 0x30);
                                if ((this.imgThumbnail.ImageSize.Width < defaultLargeIconSize.Width) || (this.imgThumbnail.ImageSize.Height < defaultLargeIconSize.Height))
                                {
                                    if ((this.imgThumbnail.ImageSize.Width >= ImageHelper.DefaultLargeIconSize.Width) && (this.imgThumbnail.ImageSize.Height >= ImageHelper.DefaultLargeIconSize.Height))
                                    {
                                        defaultLargeIconSize = ImageHelper.DefaultLargeIconSize;
                                    }
                                    else
                                    {
                                        defaultLargeIconSize = ImageHelper.DefaultSmallIconSize;
                                    }
                                }
                                Image key = VirtualIcon.GetIcon(item, defaultLargeIconSize, IconStyle.DefaultIcon);
                                lock ((list = this.imgThumbnail))
                                {
                                    if (this.ThumbnailIconMap.TryGetValue(key, out num))
                                    {
                                        item2.ImageIndex = num;
                                    }
                                }
                                if (item2.ImageIndex < 0)
                                {
                                    using (Image image2 = this.CreateThumbnail(item, key, this.imgThumbnail.ImageSize, true))
                                    {
                                        VirtualHighligher highlighter = VirtualIcon.GetHighlighter(item);
                                        lock ((list = this.imgThumbnail))
                                        {
                                            this.imgThumbnail.Images.Add(image2);
                                            item2.ImageIndex = this.imgThumbnail.Images.Count - 1;
                                            if (!((highlighter != null) && highlighter.AlphaBlend))
                                            {
                                                this.ThumbnailIconMap.Add(key, item2.ImageIndex);
                                            }
                                        }
                                    }
                                }
                                this.ThumbnailNameMap.Add(item.FullName, item2.ImageIndex);
                                VirtualIcon.ExtractIconQueue.Value.QueueWeakWorkItem<Tuple<Image, Size, WeakReference, WeakReference>>(this.ThumbnailCallback, Tuple.Create<Image, Size, WeakReference, WeakReference>(key, defaultLargeIconSize, new WeakReference(this.CurrentFolder), new WeakReference(item)));
                            }
                        }
                        break;

                    case System.Windows.Forms.View.Details:
                    case System.Windows.Forms.View.SmallIcon:
                    case System.Windows.Forms.View.List:
                        if (view == System.Windows.Forms.View.Details)
                        {
                            foreach (ColumnHeader header in this.listView.Columns)
                            {
                                object obj2;
                                if (header.Index <= 0)
                                {
                                    continue;
                                }
                                ListViewColumnInfo tag = (ListViewColumnInfo) header.Tag;
                                int propertyId = tag.PropertyId;
                                switch (item.GetPropertyAvailability(propertyId))
                                {
                                    case PropertyAvailability.Normal:
                                        obj2 = item[propertyId];
                                        break;

                                    case PropertyAvailability.Slow:
                                        obj2 = isParentFolder ? null : item[propertyId];
                                        break;

                                    default:
                                        obj2 = null;
                                        break;
                                }
                                if (obj2 == null)
                                {
                                    item2.SubItems.Add(string.Empty);
                                }
                                else if (propertyId == 1)
                                {
                                    item2.SubItems.Add(!isParentFolder ? obj2.ToString() : string.Empty);
                                    if (VirtualFilePanelSettings.Default.HideNameExtension)
                                    {
                                        item2.Text = Path.GetFileNameWithoutExtension(item2.Text);
                                    }
                                }
                                else
                                {
                                    item2.SubItems.Add(tag.Property.ConvertToString(obj2));
                                }
                            }
                            break;
                        }
                        break;
                }
                if (this.FSelection.Contains(item))
                {
                    item2.ForeColor = this.ListSelectedForeColor;
                }
                else
                {
                    item2.ForeColor = VirtualItemHelper.GetForeColor(item, this.listView.ForeColor);
                }
            }
            return item2;
        }

        private ListViewItem[] CreateListViewItemCache(int startIndex, int length)
        {
            ListViewItem[] itemArray = new ListViewItem[length];
            for (int i = 0; i < length; i++)
            {
                int itemIndex = startIndex + i;
                IVirtualItem item = this.FItems[itemIndex];
                itemArray[i] = this.CreateListViewItem(item, itemIndex, item.Equals(this.ParentFolder));
            }
            return itemArray;
        }

        private IVirtualItemFilter CreateQuickFilter()
        {
            QuickFindOptions findOptions = this.FindOptions;
            if ((this.tsFind.Visible && !string.IsNullOrEmpty(this.tstFind.Text)) && ((findOptions & QuickFindOptions.QuickFilter) > 0))
            {
                IVirtualItemFilter filter;
                if ((findOptions & QuickFindOptions.PrefixSearch) > 0)
                {
                    filter = new VirtualItemNameFilter(NamePatternComparision.StartsWith, this.tstFind.Text);
                }
                else
                {
                    filter = new VirtualItemNameFilter(NamePatternComparision.Wildcards, '*' + this.tstFind.Text + '*');
                }
                if ((findOptions & QuickFindOptions.AlwaysShowFolders) > 0)
                {
                    filter = new AggregatedVirtualItemFilter(AggregatedFilterCondition.Any, filter, new VirtualItemAttributeFilter(FileAttributes.Directory));
                }
                return filter;
            }
            return null;
        }

        private Image CreateThumbnail(IVirtualItem item, Image thumbnail, Size thumbnailSize, bool isIcon)
        {
            PixelFormat format;
            if (this.imgThumbnail.ColorDepth == ColorDepth.Depth32Bit)
            {
                format = PixelFormat.Format32bppArgb;
            }
            else
            {
                format = PixelFormat.Format24bppRgb;
            }
            Bitmap bitmap = new Bitmap(thumbnailSize.Width, thumbnailSize.Height, format);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                if (format != PixelFormat.Format32bppArgb)
                {
                    graphics.Clear(this.ListBackColor);
                }
                if ((thumbnail.Width > bitmap.Width) || (thumbnail.Height > bitmap.Height))
                {
                    thumbnailSize = ImageHelper.GetThumbnailSize(thumbnail.Size, bitmap.Size);
                }
                else
                {
                    thumbnailSize = thumbnail.Size;
                }
                int x = (bitmap.Width - thumbnailSize.Width) / 2;
                int y = (bitmap.Height - thumbnailSize.Height) / 2;
                if (isIcon)
                {
                    VirtualHighligher highlighter = VirtualIcon.GetHighlighter(item);
                    if ((highlighter != null) && highlighter.AlphaBlend)
                    {
                        ImageHelper.DrawBlendImage(graphics, thumbnail, highlighter.BlendColor, highlighter.BlendLevel, x, y);
                    }
                    else
                    {
                        graphics.DrawImage(thumbnail, x, y, thumbnailSize.Width, thumbnailSize.Height);
                    }
                }
                else
                {
                    graphics.DrawImage(thumbnail, x, y, thumbnailSize.Width, thumbnailSize.Height);
                    if (VirtualIcon.CheckIconOption(IconOptions.ShowOverlayIcons) && (item is FileSystemItem))
                    {
                        Size size = thumbnailSize;
                        if ((size.Width > 0x30) && (size.Height > 0x30))
                        {
                            size = new Size(0x30, 0x30);
                        }
                        Image itemOverlay = ImageProvider.Default.GetItemOverlay(item.FullName, new Size(0x30, 0x30));
                        if (itemOverlay != null)
                        {
                            lock (itemOverlay)
                            {
                                graphics.DrawImage(itemOverlay, 0, bitmap.Height - itemOverlay.Height);
                            }
                        }
                    }
                }
                graphics.DrawRectangle(Pens.DarkGray, new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));
            }
            return bitmap;
        }

        private void CurrentFolderDeleted(object sender, VirtualItemChangedEventArgs e)
        {
            IVirtualFolder folderRoot = VirtualItemHelper.GetFolderRoot(this.CurrentFolder);
            IPersistVirtualItem item = folderRoot as IPersistVirtualItem;
            if ((item == null) || item.Exists)
            {
                for (IVirtualFolder folder2 = e.Item.Parent; folder2 != null; folder2 = folder2.Parent)
                {
                    IPersistVirtualItem item2 = folder2 as IPersistVirtualItem;
                    if (((item2 != null) && item2.Exists) && this.SetCurrentFolder(folder2, true))
                    {
                        return;
                    }
                }
                if (((folderRoot != null) && !folderRoot.Equals(this.CurrentFolder)) && this.SetCurrentFolder(folderRoot, true))
                {
                    return;
                }
            }
            if (folderRoot != null)
            {
                switch (PathHelper.GetPathType(folderRoot.FullName))
                {
                    case PathType.NetworkServer:
                    case PathType.NetworkShare:
                        this.CurrentFolder = NetworkFileSystemCreator.NetworkRoot;
                        return;
                }
            }
            this.SetDefaultPathAsCurrent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoVisibleChanged()
        {
            this.OnVisibleChanged(EventArgs.Empty);
        }

        private void DrawCueText(ToolStrip strip, Graphics g, string text, Font textFont, Size textSize, int left)
        {
            Rectangle rect = new Rectangle(left, (strip.ClientSize.Height - textSize.Height) - 3, textSize.Width, textSize.Height + 1);
            using (GraphicsPath path = GraphicsHelper.RoundRect(rect, 2f))
            {
                g.FillPath(SystemBrushes.Info, path);
                g.DrawPath(SystemPens.InfoText, path);
                TextRenderer.DrawText(g, text, textFont, new Point(rect.Left + 1, rect.Top + 1), SystemColors.InfoText, SystemColors.Info);
            }
        }

        protected void EndListViewUpdate()
        {
            if (!this.CheckPanelState(PanelState.ProcessingEndUpdate))
            {
                this.UpdateCount--;
                if (((this.UpdateCount <= 0) && !base.Disposing) && !base.IsDisposed)
                {
                    this.SetPanelState(PanelState.ProcessingEndUpdate, true);
                    try
                    {
                        if ((this.UpdateAction & ListViewUpdateAction.RecreateColumns) > 0)
                        {
                            this.RecreateColumns();
                        }
                        if ((this.UpdateAction & ListViewUpdateAction.UpdateListView) > 0)
                        {
                            this.UpdateListView();
                        }
                        if ((this.UpdateAction & ListViewUpdateAction.ResumeLayout) > 0)
                        {
                            this.listView.ResumeLayout();
                        }
                        if ((this.UpdateAction & ListViewUpdateAction.EndUpdate) > 0)
                        {
                            this.listView.EndUpdate();
                        }
                        if ((this.UpdateAction & ListViewUpdateAction.Refresh) > 0)
                        {
                            this.listView.Parent.Refresh();
                        }
                        else if ((this.UpdateAction & ListViewUpdateAction.Invalidate) > 0)
                        {
                            this.listView.Invalidate();
                        }
                        this.UpdateCount = 0;
                        this.UpdateAction = 0;
                    }
                    finally
                    {
                        this.SetPanelState(PanelState.ProcessingEndUpdate, false);
                    }
                }
            }
        }

        private void Event_ApplicationIdle(object sender, EventArgs e)
        {
            Keys keys = ((Settings.Default.IsShowKeyboardCues && this.listView.Focused) && (this.tsPath.Items.Count > 0)) ? Control.ModifierKeys : Keys.None;
            if (this.ControlModifierKeys != keys)
            {
                this.ControlModifierKeys = keys;
                if (this.tsPath.Visible)
                {
                    this.tsPath.Invalidate(new Rectangle(this.tsPath.ClientRectangle.Left, this.tsPath.ClientRectangle.Top, 100, this.tsPath.ClientRectangle.Height));
                }
            }
            this.SetPanelState(PanelState.PopulatingItems, false);
            if (this.CheckPanelState(PanelState.UpdateFocusSelectionNeeded))
            {
                this.SetPanelState(PanelState.UpdateFocusSelectionNeeded, false);
                if (this.CheckPanelState(PanelState.UseFocusSelection) || this.listView.ExplorerTheme)
                {
                    ListViewItem focusedItem = this.listView.FocusedItem;
                    if (focusedItem != null)
                    {
                        focusedItem.Selected = true;
                    }
                }
            }
            if (!ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.CuttedItems))
            {
                ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.CuttedItems);
                if (this.ShowItemIcons)
                {
                    this.InvalidateListView(false);
                }
            }
            if (this.CheckPanelState(PanelState.RedrawDisabled))
            {
                this.SetPanelState(PanelState.RedrawDisabled, false);
                Windows.SendMessage(base.Handle, 11, (IntPtr) 1, IntPtr.Zero);
                if (base.Enabled)
                {
                    this.Refresh();
                }
            }
        }

        private void ExecuteVerb(object sender, ExecuteVerbEventArgs e)
        {
            if (e.Verb == "rename")
            {
                this.BeginRenameItem();
                e.Handled = true;
            }
        }

        private void ExtractThumbnail(Tuple<Image, Size, WeakReference, WeakReference> state)
        {
            Image key = state.Item1;
            Size size = state.Item2;
            WeakReference reference = state.Item3;
            if ((reference != null) && reference.IsAlive)
            {
                IVirtualFolder target = (IVirtualFolder) reference.Target;
                if (target.Equals(this.CurrentFolder))
                {
                    WeakReference reference2 = state.Item4;
                    if ((reference2 != null) && reference2.IsAlive)
                    {
                        int num;
                        ImageList list;
                        IVirtualItem item = (IVirtualItem) reference2.Target;
                        Image thumbnail = VirtualIcon.GetThumbnail(item, this.imgThumbnail.ImageSize);
                        VirtualHighligher highlighter = VirtualIcon.GetHighlighter(item);
                        bool flag = (highlighter != null) && highlighter.AlphaBlend;
                        bool isIcon = thumbnail == null;
                        if (isIcon)
                        {
                            thumbnail = VirtualIcon.GetIcon(item, size) ?? key;
                            if (!flag && (thumbnail == key))
                            {
                                lock ((list = this.imgThumbnail))
                                {
                                    if (this.ThumbnailIconMap.TryGetValue(key, out num))
                                    {
                                        this.ThumbnailNameMap[item.FullName] = num;
                                    }
                                }
                                return;
                            }
                        }
                        using (Image image3 = this.CreateThumbnail(item, thumbnail, this.imgThumbnail.ImageSize, isIcon))
                        {
                            lock ((list = this.imgThumbnail))
                            {
                                if (!((isIcon && !flag) && this.ThumbnailIconMap.TryGetValue(thumbnail, out num)))
                                {
                                    this.imgThumbnail.Images.Add(image3);
                                    num = this.imgThumbnail.Images.Count - 1;
                                }
                                this.ThumbnailNameMap[item.FullName] = num;
                            }
                        }
                        this.FolderContentChanged(this, new VirtualItemChangedEventArgs(item, new VirtualPropertySet(new int[] { 0x15 })));
                    }
                }
            }
        }

        private HashList<T> FastHashListSort<T>(HashList<T> list, HashList<T> reference, int referenceStartIndex)
        {
            int num;
            int[] OrderArray = new int[reference.Count - referenceStartIndex];
            list.Initialize();
            Parallel.For(0, (OrderArray.Length / 0x180) + Math.Sign((int) (OrderArray.Length % 0x180)), delegate (int x) {
                int num = x * 0x180;
                int num2 = num + Math.Min(OrderArray.Length - num, 0x180);
                for (int j = num; j < num2; j++)
                {
                    OrderArray[j] = list.IndexOf(reference[j + referenceStartIndex]);
                }
            });
            HashList<T> list2 = new HashList<T>(list.Count);
            BitArray array = new BitArray(list.Count, true);
            for (num = 0; num < OrderArray.Length; num++)
            {
                int num2 = OrderArray[num];
                if (num2 >= 0)
                {
                    list2.Add(list[num2]);
                    array[num2] = false;
                }
            }
            for (num = 0; num < array.Length; num++)
            {
                if (array[num])
                {
                    list2.Add(list[num]);
                }
            }
            return list2;
        }

        private int FindVirtualItem(string text, int startIndex, SearchDirectionHint direction, bool isPrefixSearch)
        {
            if ((this.FItems != null) && (this.FItems.Count != 0))
            {
                int num;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                else if (startIndex >= this.FItems.Count)
                {
                    startIndex = this.FItems.Count - 1;
                }
                if (direction == SearchDirectionHint.Up)
                {
                    num = ((startIndex - 1) < 0) ? (this.FItems.Count - 1) : (startIndex - 1);
                }
                else
                {
                    num = ((startIndex + 1) < this.FItems.Count) ? (startIndex + 1) : 0;
                }
                while (num != startIndex)
                {
                    if (IsNameEqual(text, this.FItems[num].Name, isPrefixSearch))
                    {
                        return num;
                    }
                    if (direction == SearchDirectionHint.Up)
                    {
                        num = ((num - 1) < 0) ? (this.FItems.Count - 1) : (num - 1);
                    }
                    else
                    {
                        num = ((num + 1) < this.FItems.Count) ? (num + 1) : 0;
                    }
                }
            }
            return -1;
        }

        private bool FocusFindItem(string text, SearchDirectionHint direction)
        {
            ListViewItem focusedItem = this.listView.FocusedItem;
            if (focusedItem == null)
            {
                if (this.listView.Items.Count <= 0)
                {
                    return false;
                }
                focusedItem = this.listView.Items[0];
            }
            bool isPrefixSearch = (this.FindOptions & QuickFindOptions.PrefixSearch) > 0;
            int num = this.FindVirtualItem(text, (focusedItem != null) ? focusedItem.Index : 0, direction, isPrefixSearch);
            bool flag2 = num >= 0;
            if (!flag2)
            {
                flag2 = IsNameEqual(text, focusedItem.Text, isPrefixSearch);
            }
            else
            {
                focusedItem = this.listView.Items[num];
            }
            if (flag2)
            {
                this.FocusItem(focusedItem, true);
                this.HideItemToolTip();
            }
            return flag2;
        }

        private void FocusItem(ListViewItem item, bool ensureVisible)
        {
            ListViewItem focusedItem = this.listView.FocusedItem;
            if (item != focusedItem)
            {
                if (focusedItem != null)
                {
                    focusedItem.Focused = false;
                    focusedItem.Selected = true;
                }
                this.listView.SelectedIndices.Clear();
                item.Focused = true;
                item.Selected = true;
            }
            if (ensureVisible)
            {
                item.EnsureVisible();
            }
        }

        private void FolderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lock (this.tmrUpdateItems)
            {
                this.tsPath.Tag = null;
                this.FolderChangeRequested = WatcherChangeTypes.All;
                this.SetPanelState(PanelState.FolderChangePending, false);
                this.SetPanelState(PanelState.ProgressUpdatePending | PanelState.DoFolderChangedTick, true);
            }
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new EventHandler(this.tmrUpdateItems_Tick), new object[] { this.tmrUpdateItems, EventArgs.Empty });
                if (e.Error != null)
                {
                    if (base.InvokeRequired)
                    {
                        base.BeginInvoke(new Action<IWin32Window, Exception>(MessageDialog.ShowException), new object[] { this, e.Error });
                    }
                    else
                    {
                        MessageDialog.ShowException(this, e.Error);
                    }
                }
            }
        }

        private void FolderContentChanged(object sender, VirtualItemChangedEventArgs e)
        {
            System.Windows.Forms.Timer timer;
            if (this.CurrentFolder.Equals(e.Item))
            {
                if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    lock ((timer = this.tmrUpdateItems))
                    {
                        this.FolderChangeRequested = 0;
                        this.SetPanelState(PanelState.DoFolderChangedTick, false);
                    }
                    if (base.InvokeRequired)
                    {
                        base.BeginInvoke(new EventHandler<VirtualItemChangedEventArgs>(this.CurrentFolderDeleted), new object[] { sender, e });
                    }
                    else
                    {
                        this.CurrentFolderDeleted(sender, e);
                    }
                }
            }
            else if ((e.ChangeType != WatcherChangeTypes.Created) || !this.RemoveByFilter(e.Item, this.Filter, VirtualFilePanelSettings.Default.HiddenItemsFilter))
            {
                if ((((base.IsHandleCreated && !this.IsThumbnailView) && ((e.Item != null) && (e.PropertySet != null))) && (e.ChangeType == WatcherChangeTypes.Changed)) && e.PropertySet.Equals(0x15))
                {
                    base.BeginInvoke(new Action<IVirtualItem>(this.InvalidateItemIcon), new object[] { e.Item });
                }
                else
                {
                    lock ((timer = this.tmrUpdateItems))
                    {
                        if (((this.FolderChangeItem != null) && this.FolderChangeItem.Equals(e.Item)) || ((this.FolderChangeRequested & e.ChangeType) == 0))
                        {
                            this.FolderChangeItem = e.Item;
                        }
                        else
                        {
                            this.FolderChangeItem = null;
                            if ((this.FolderChangeRequested & e.ChangeType) == e.ChangeType)
                            {
                                this.SetPanelState(PanelState.FolderChangePending, true);
                            }
                        }
                        this.FolderChangeRequested |= e.ChangeType;
                        this.FolderChangePropertySet |= e.PropertySet;
                        this.SetPanelState(PanelState.DoFolderChangedTick, true);
                    }
                }
            }
        }

        private void FolderProgress(object sender, ProgressChangedEventArgs e)
        {
            IVirtualFolder userState = e.UserState as IVirtualFolder;
            string str = (e.UserState as string) ?? ((userState != null) ? userState.FullName : null);
            if (str != null)
            {
                lock (this.tmrUpdateItems)
                {
                    this.tsPath.Tag = str;
                    this.SetPanelState(PanelState.ProgressUpdatePending | PanelState.DoFolderChangedTick, true);
                }
            }
        }

        public bool Forward()
        {
            if (this.History.ForwardCount > 0)
            {
                if (this.SetCurrentFolder(this.History.PeekForward(), false))
                {
                    this.History.Forward();
                }
                return true;
            }
            return false;
        }

        public IEnumerable<ListViewColumnInfo> GetAllColumns()
        {
            return this.ColumnInfoMap.Values;
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

        private ListViewItem GetItemAtEx(int x, int y)
        {
            ListViewItem itemAt = this.listView.GetItemAt(x, y);
            if (itemAt != null)
            {
                return itemAt;
            }
            if (this.listView.View == System.Windows.Forms.View.List)
            {
                ListViewItem topItem = this.listView.TopItem;
                if (topItem != null)
                {
                    for (int i = topItem.Index; i < this.listView.Items.Count; i++)
                    {
                        Rectangle itemRect = this.listView.GetItemRect(i, ItemBoundsPortion.Entire);
                        if (itemRect.Contains(x, y))
                        {
                            return this.listView.Items[i];
                        }
                        if (itemRect.Left >= this.listView.ClientSize.Width)
                        {
                            break;
                        }
                    }
                }
            }
            return null;
        }

        private IEnumerable<IVirtualItem> GetListItems()
        {
            return new <GetListItems>d__0(-2) { <>4__this = this };
        }

        public PanelContentContainer GetPanelContent(bool useRemembered)
        {
            return new PanelContentContainer { Folder = { Value = this.CurrentFolder }, Locked = this.IsFolderLocked, Filter = (useRemembered && this.CheckPanelState(PanelState.HasRememberFilter)) ? this.RememberFilter : this.Filter, Sort = (useRemembered && (this.RememberSort != null)) ? this.RememberSort : this.Sort, QuickFindOptions = this.FindOptions };
        }

        public IEnumerable<IVirtualItem> GetPanelItems()
        {
            return this.GetPanelItems(!VirtualFilePanelSettings.Default.ShowUpFolderItem);
        }

        public IEnumerable<IVirtualItem> GetPanelItems(bool skipUpFolderItemCheck)
        {
            return new <GetPanelItems>d__a(-2) { <>4__this = this, <>3__skipUpFolderItemCheck = skipUpFolderItemCheck };
        }

        public Nomad.Configuration.PanelLayout GetPanelLayout(bool useRemembered, PanelLayoutEntry forceEntry)
        {
            Nomad.Configuration.PanelLayout layout = new Nomad.Configuration.PanelLayout {
                StoreEntry = PanelLayoutEntry.FolderBarVisible,
                FolderBarVisible = this.FolderBarVisible
            };
            if (layout.FolderBarVisible || ((forceEntry & PanelLayoutEntry.FolderBarOrientation) > PanelLayoutEntry.None))
            {
                layout.FolderBarOrientation = this.FolderBarOrientation;
                layout.SplitterPercent = this.SplitterPercent;
                layout.StoreEntry |= PanelLayoutEntry.FolderBarOrientation;
            }
            layout.View = (useRemembered && this.RememberView.HasValue) ? this.RememberView.Value : this.View;
            layout.StoreEntry |= PanelLayoutEntry.View;
            if ((layout.View == PanelView.Details) || ((forceEntry & PanelLayoutEntry.Columns) > PanelLayoutEntry.None))
            {
                layout.AutoSizeColumns = (useRemembered && this.RememberAutoSizeColumns.HasValue) ? this.RememberAutoSizeColumns.Value : this.AutoSizeColumns;
                layout.Columns = this.Columns;
                layout.StoreEntry |= PanelLayoutEntry.Columns;
            }
            if ((layout.View == PanelView.List) || ((forceEntry & PanelLayoutEntry.ListColumnCount) > PanelLayoutEntry.None))
            {
                layout.ListColumnCount = (useRemembered && this.RememberListViewCount.HasValue) ? this.RememberListViewCount.Value : this.ListViewColumnCount;
                layout.StoreEntry |= PanelLayoutEntry.ListColumnCount;
            }
            if ((layout.View == PanelView.Thumbnail) || ((forceEntry & PanelLayoutEntry.ThumbnailSize) > PanelLayoutEntry.None))
            {
                layout.ThumbnailSize = (useRemembered && !this.RememberThumbnailSize.IsEmpty) ? this.RememberThumbnailSize : this.ThumbnailSize;
                layout.ThumbnailSpacing = (useRemembered && !this.RememberThumbnailSpacing.IsEmpty) ? this.RememberThumbnailSpacing : this.ThumbnailSpacing;
                layout.StoreEntry |= PanelLayoutEntry.ThumbnailSize;
            }
            layout.ToolbarsVisible = this.ToolbarsVisible;
            layout.StoreEntry |= PanelLayoutEntry.ToolbarsVisible;
            return layout;
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

        public IEnumerable<ListViewColumnInfo> GetVisibleColumns()
        {
            return new <GetVisibleColumns>d__1e(-2) { <>4__this = this };
        }

        private void HideItemToolTip()
        {
            this.tmrToolTip.Stop();
            this.tmrToolTip.Tag = null;
            VirtualToolTip.Default.HideTooltip();
            this.HoverItem = null;
        }

        private void InfoToolbar_VisibleChanged(object sender, EventArgs e)
        {
            ToolStrip strip = (ToolStrip) sender;
            if (strip.Visible)
            {
                this.FToolbarsVisible |= (PanelToolbar) strip.Tag;
            }
            else
            {
                this.FToolbarsVisible &= ~((PanelToolbar) strip.Tag);
            }
        }

        private void InitializeBindings()
        {
            this.BeginListViewUpdate(false, true);
            this.listView.DataBindings.Add(new Binding("ShowColumnLines", VirtualFilePanelSettings.Default, "ShowColumnLines", true, DataSourceUpdateMode.Never));
            this.listView.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            this.treeView.DataBindings.Add(new Binding("Font", VirtualFilePanelSettings.Default, "ListFont", true, DataSourceUpdateMode.Never));
            this.treeView.DataBindings.Add(new Binding("ExplorerTheme", Settings.Default, "ExplorerTheme", true, DataSourceUpdateMode.Never));
            this.treeView.DataBindings.Add(new Binding("WatchChanges", Settings.Default, "WatchFolderTree", true, DataSourceUpdateMode.Never));
            base.DataBindings.Add(new Binding("FileNameCasing", VirtualFilePanelSettings.Default, "FileNameCasing", true, DataSourceUpdateMode.Never));
            base.DataBindings.Add(new Binding("FolderNameCasing", VirtualFilePanelSettings.Default, "FolderNameCasing", true, DataSourceUpdateMode.Never));
            base.DataBindings.Add(new Binding("FolderNameTemplate", VirtualFilePanelSettings.Default, "FolderNameTemplate", true, DataSourceUpdateMode.Never));
            base.DataBindings.Add(new Binding("ListFont", VirtualFilePanelSettings.Default, "ListFont", true, DataSourceUpdateMode.Never));
            base.DataBindings.Add(new Binding("ShowItemIcons", Settings.Default, "ShowIcons", true, DataSourceUpdateMode.Never));
            this.tsPath.DataBindings.Add(new Binding("HideNotReadyDrives", Settings.Default, "HideNotReadyDrives", true, DataSourceUpdateMode.Never));
            this.tsPath.DataBindings.Add(new Binding("PathOptions", VirtualFilePanelSettings.Default, "BreadcrumbOptions", true, DataSourceUpdateMode.Never));
            this.EndListViewUpdate();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(VirtualFilePanel));
            this.splitContainer = new SplitContainer();
            this.treeView = new VirtualFolderTreeView();
            this.listView = new ListViewEx();
            this.cmsListView = new ContextMenuStrip(this.components);
            this.tsFind = new ToolStrip();
            this.cmsToolStrip = new ContextMenuStrip(this.components);
            this.tsmiItemInfoVisible = new ToolStripMenuItem();
            this.tsmiFolderInfoVisible = new ToolStripMenuItem();
            this.tsmiFindVisible = new ToolStripMenuItem();
            this.tssToolstrip1 = new ToolStripSeparator();
            this.tsmiAutoHide = new ToolStripMenuItem();
            this.tsddFind = new ToolStripDropDownButton();
            this.tsmiQuickFind = new ToolStripMenuItem();
            this.tsmiQuickFilter = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.tsmiStartsWith = new ToolStripMenuItem();
            this.tsmiContains = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.tsmiAlwaysShowFolders = new ToolStripMenuItem();
            this.tstFind = new ToolStripTextBox();
            this.tsbFindNext = new ToolStripButton();
            this.tsbFindPrevious = new ToolStripButton();
            this.tslFindNonFound = new ToolStripLabel();
            this.tsFolderInfo = new ToolStrip();
            this.tslFolderSize = new ToolStripLabel();
            this.tssFolder1 = new ToolStripSeparator();
            this.tslItemCount = new ToolStripLabel();
            this.tssFolder2 = new ToolStripSeparator();
            this.tslFolderCount = new ToolStripLabel();
            this.tssFolder3 = new ToolStripSeparator();
            this.tslSelectionInfo = new ToolStripLabel();
            this.tssFolder4 = new ToolStripSeparator();
            this.tsbClearSelection = new ToolStripButton();
            this.tsbUnlockFolder = new ToolStripButton();
            this.tsbClearFilter = new ToolStripButton();
            this.tsItemInfo = new ToolStrip();
            this.tslItemName = new ToolStripLabel();
            this.tslItemDate = new ToolStripLabel();
            this.tssItemDate = new ToolStripSeparator();
            this.tslItemSize = new ToolStripLabel();
            this.tssItemSize = new ToolStripSeparator();
            this.tsPath = new VirtualPathBreadcrumb();
            this.cmsPath = new ContextMenuStrip(this.components);
            this.tsmiCopyPathAsText = new ToolStripMenuItem();
            this.tssPath1 = new ToolStripSeparator();
            this.tsmiBack = new ToolStripMenuItem();
            this.tsmiForward = new ToolStripMenuItem();
            this.tssPath2 = new ToolStripSeparator();
            this.tsmiChangeFolder = new ToolStripMenuItem();
            this.tssPath3 = new ToolStripSeparator();
            this.tsmiRefresh = new ToolStripMenuItem();
            this.tmrUpdateItems = new System.Windows.Forms.Timer(this.components);
            this.tmrExpandNode = new System.Windows.Forms.Timer(this.components);
            this.ItemToolTip = new ToolTip(this.components);
            this.cmsColumns = new ContextMenuStrip(this.components);
            this.tsmiManageColumns = new ToolStripMenuItem();
            this.tssColumns1 = new ToolStripSeparator();
            this.tssColumns2 = new ToolStripSeparator();
            this.tssColumns3 = new ToolStripSeparator();
            this.tsmiRememberWidthAsDefault = new ToolStripMenuItem();
            this.tsmiResetDefaultWidth = new ToolStripMenuItem();
            this.tssColumns4 = new ToolStripSeparator();
            this.tsmiAutoSizeColumns = new ToolStripMenuItem();
            this.tmrToolTip = new System.Windows.Forms.Timer(this.components);
            this.imgThumbnail = new ImageList(this.components);
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tsFind.SuspendLayout();
            this.cmsToolStrip.SuspendLayout();
            this.tsFolderInfo.SuspendLayout();
            this.tsItemInfo.SuspendLayout();
            this.cmsPath.SuspendLayout();
            this.cmsColumns.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            this.splitContainer.Panel2.Controls.Add(this.listView);
            this.splitContainer.Panel2.Controls.Add(this.tsFind);
            this.splitContainer.Panel2.Controls.Add(this.tsFolderInfo);
            this.splitContainer.Panel2.Controls.Add(this.tsItemInfo);
            this.splitContainer.TabStop = false;
            this.splitContainer.SplitterMoving += new SplitterCancelEventHandler(this.splitContainer_SplitterMoving);
            this.splitContainer.SplitterMoved += new SplitterEventHandler(this.splitContainer_SplitterMoved);
            this.splitContainer.DoubleClick += new EventHandler(this.splitContainer_DoubleClick);
            this.splitContainer.MouseCaptureChanged += new EventHandler(this.splitContainer_MouseCaptureChanged);
            this.splitContainer.MouseDown += new MouseEventHandler(this.splitContainer_MouseDown);
            this.treeView.AllowDrop = true;
            manager.ApplyResources(this.treeView, "treeView");
            this.treeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.treeView.FadePlusMinus = true;
            this.treeView.FullRowSelect = true;
            this.treeView.HideSelection = false;
            this.treeView.HotTracking = true;
            this.treeView.LabelEdit = true;
            this.treeView.Name = "treeView";
            this.treeView.ShowItemIcons = false;
            this.treeView.ShowLines = false;
            this.treeView.TabStop = false;
            this.treeView.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
            this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.DragDrop += new DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragEnter += new DragEventHandler(this.listView_DragEnter);
            this.treeView.DragOver += new DragEventHandler(this.treeView_DragOver);
            this.treeView.DragLeave += new EventHandler(this.listView_DragLeave);
            this.listView.AllowColumnReorder = true;
            this.listView.AllowDrop = true;
            this.listView.CanResizeColumns = false;
            this.listView.ContextMenuStrip = this.cmsListView;
            manager.ApplyResources(this.listView, "listView");
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.LabelEdit = true;
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.ShowGroups = false;
            this.listView.ShowItemToolTips = true;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            this.listView.VirtualMode = true;
            this.listView.BeforeLabelEdit += new BeforeLabelEditEventHandler(this.listView_BeforeLabelEdit);
            this.listView.ColumnRightClick += new ColumnClickEventHandler(this.listView_ColumnRightClick);
            this.listView.ItemTooltip += new EventHandler<ItemTooltipEventArgs>(this.listView_ItemTooltip);
            this.listView.GetItemColors += new EventHandler<GetItemColorsEventArgs>(this.listView_GetItemColors);
            this.listView.GetItemState += new EventHandler<GetItemStateEventArgs>(this.listView_GetItemState);
            this.listView.PostDrawItem += new EventHandler<PostDrawListViewItemEventArgs>(this.listView_PostDrawItem);
            this.listView.AfterLabelEdit += new LabelEditEventHandler(this.listView_AfterLabelEdit);
            this.listView.CacheVirtualItems += new CacheVirtualItemsEventHandler(this.listView_CacheVirtualItems);
            this.listView.ColumnClick += new ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.ColumnReordered += new ColumnReorderedEventHandler(this.listView_ColumnReordered);
            this.listView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.listView_ColumnWidthChanged);
            this.listView.ItemDrag += new ItemDragEventHandler(this.listView_ItemDrag);
            this.listView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.listView_ItemSelectionChanged);
            this.listView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.listView_RetrieveVirtualItem);
            this.listView.SearchForVirtualItem += new SearchForVirtualItemEventHandler(this.listView_SearchForVirtualItem);
            this.listView.ClientSizeChanged += new EventHandler(this.listView_ClientSizeChanged);
            this.listView.DragDrop += new DragEventHandler(this.listView_DragDrop);
            this.listView.DragEnter += new DragEventHandler(this.listView_DragEnter);
            this.listView.DragOver += new DragEventHandler(this.listView_DragOver);
            this.listView.DragLeave += new EventHandler(this.listView_DragLeave);
            this.listView.QueryContinueDrag += new QueryContinueDragEventHandler(this.listView_QueryContinueDrag);
            this.listView.Enter += new EventHandler(this.listView_Enter);
            this.listView.KeyDown += new KeyEventHandler(this.listView_KeyDown);
            this.listView.KeyPress += new KeyPressEventHandler(this.listView_KeyPress);
            this.listView.KeyUp += new KeyEventHandler(this.listView_KeyUp);
            this.listView.Leave += new EventHandler(this.listView_Leave);
            this.listView.MouseClick += new MouseEventHandler(this.listView_MouseClick);
            this.listView.MouseDoubleClick += new MouseEventHandler(this.listView_MouseDoubleClick);
            this.listView.MouseCaptureChanged += new EventHandler(this.listView_MouseCaptureChanged);
            this.listView.MouseDown += new MouseEventHandler(this.listView_MouseDown);
            this.listView.MouseLeave += new EventHandler(this.listView_MouseLeave);
            this.listView.MouseMove += new MouseEventHandler(this.listView_MouseMove);
            this.cmsListView.Name = "contextMenuStrip1";
            manager.ApplyResources(this.cmsListView, "cmsListView");
            this.cmsListView.Opening += new CancelEventHandler(this.cmsListView_Opening);
            this.tsFind.CanOverflow = false;
            this.tsFind.ContextMenuStrip = this.cmsToolStrip;
            manager.ApplyResources(this.tsFind, "tsFind");
            this.tsFind.GripStyle = ToolStripGripStyle.Hidden;
            this.tsFind.Items.AddRange(new ToolStripItem[] { this.tsddFind, this.tstFind, this.tsbFindNext, this.tsbFindPrevious, this.tslFindNonFound });
            this.tsFind.Name = "tsFind";
            this.tsFind.VisibleChanged += new EventHandler(this.tsFind_VisibleChanged);
            this.tsFind.DoubleClick += new EventHandler(this.tsFind_DoubleClick);
            this.tsFind.Leave += new EventHandler(this.tsFind_Leave);
            this.tsFind.PreviewKeyDown += new PreviewKeyDownEventHandler(this.tsFind_PreviewKeyDown);
            this.cmsToolStrip.Items.AddRange(new ToolStripItem[] { this.tsmiItemInfoVisible, this.tsmiFolderInfoVisible, this.tsmiFindVisible, this.tssToolstrip1, this.tsmiAutoHide });
            this.cmsToolStrip.Name = "cmsToolbars";
            manager.ApplyResources(this.cmsToolStrip, "cmsToolStrip");
            this.cmsToolStrip.Opening += new CancelEventHandler(this.cmsToolbars_Opening);
            this.tsmiItemInfoVisible.Name = "tsmiItemInfoVisible";
            manager.ApplyResources(this.tsmiItemInfoVisible, "tsmiItemInfoVisible");
            this.tsmiItemInfoVisible.Click += new EventHandler(this.tsmiToolStripVisible_Click);
            this.tsmiItemInfoVisible.Paint += new PaintEventHandler(this.tsmiToolStripVisible_Paint);
            this.tsmiFolderInfoVisible.Name = "tsmiFolderInfoVisible";
            manager.ApplyResources(this.tsmiFolderInfoVisible, "tsmiFolderInfoVisible");
            this.tsmiFolderInfoVisible.Click += new EventHandler(this.tsmiToolStripVisible_Click);
            this.tsmiFolderInfoVisible.Paint += new PaintEventHandler(this.tsmiToolStripVisible_Paint);
            this.tsmiFindVisible.Name = "tsmiFindVisible";
            manager.ApplyResources(this.tsmiFindVisible, "tsmiFindVisible");
            this.tsmiFindVisible.Click += new EventHandler(this.tsmiToolStripVisible_Click);
            this.tsmiFindVisible.Paint += new PaintEventHandler(this.tsmiToolStripVisible_Paint);
            this.tssToolstrip1.Name = "tssToolstrip1";
            manager.ApplyResources(this.tssToolstrip1, "tssToolstrip1");
            this.tsmiAutoHide.Name = "tsmiAutoHide";
            manager.ApplyResources(this.tsmiAutoHide, "tsmiAutoHide");
            this.tsmiAutoHide.Click += new EventHandler(this.tsmiAutoHide_Click);
            this.tsddFind.AutoToolTip = false;
            this.tsddFind.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiQuickFind, this.tsmiQuickFilter, this.toolStripSeparator4, this.tsmiStartsWith, this.tsmiContains, this.toolStripSeparator5, this.tsmiAlwaysShowFolders });
            this.tsddFind.Name = "tsddFind";
            manager.ApplyResources(this.tsddFind, "tsddFind");
            this.tsddFind.TextChanged += new EventHandler(this.tsddFind_TextChanged);
            this.tsmiQuickFind.Checked = true;
            this.tsmiQuickFind.CheckState = CheckState.Checked;
            this.tsmiQuickFind.Name = "tsmiQuickFind";
            manager.ApplyResources(this.tsmiQuickFind, "tsmiQuickFind");
            this.tsmiQuickFind.Click += new EventHandler(this.tsmiQuickFind_Click);
            this.tsmiQuickFind.Paint += new PaintEventHandler(this.tsmiQuickFind_Paint);
            this.tsmiQuickFilter.Name = "tsmiQuickFilter";
            manager.ApplyResources(this.tsmiQuickFilter, "tsmiQuickFilter");
            this.tsmiQuickFilter.Click += new EventHandler(this.tsmiQuickFilter_Click);
            this.tsmiQuickFilter.Paint += new PaintEventHandler(this.tsmiQuickFilter_Paint);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            manager.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.tsmiStartsWith.Checked = true;
            this.tsmiStartsWith.CheckState = CheckState.Checked;
            this.tsmiStartsWith.Name = "tsmiStartsWith";
            manager.ApplyResources(this.tsmiStartsWith, "tsmiStartsWith");
            this.tsmiStartsWith.Click += new EventHandler(this.tsmiQuickFilter_Click);
            this.tsmiStartsWith.Paint += new PaintEventHandler(this.tsmiQuickFilter_Paint);
            this.tsmiContains.Name = "tsmiContains";
            manager.ApplyResources(this.tsmiContains, "tsmiContains");
            this.tsmiContains.Click += new EventHandler(this.tsmiQuickFind_Click);
            this.tsmiContains.Paint += new PaintEventHandler(this.tsmiQuickFind_Paint);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            manager.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.tsmiAlwaysShowFolders.CheckOnClick = true;
            this.tsmiAlwaysShowFolders.Name = "tsmiAlwaysShowFolders";
            manager.ApplyResources(this.tsmiAlwaysShowFolders, "tsmiAlwaysShowFolders");
            this.tsmiAlwaysShowFolders.Click += new EventHandler(this.tsmiAlwaysShowFolders_Click);
            this.tsmiAlwaysShowFolders.Paint += new PaintEventHandler(this.tsmiAlwaysShowFolders_Paint);
            this.tstFind.Name = "tstFind";
            manager.ApplyResources(this.tstFind, "tstFind");
            this.tstFind.Enter += new EventHandler(this.tstFind_Enter);
            this.tstFind.KeyDown += new KeyEventHandler(this.tstFind_KeyDown);
            this.tstFind.TextChanged += new EventHandler(this.tstFind_TextChanged);
            manager.ApplyResources(this.tsbFindNext, "tsbFindNext");
            this.tsbFindNext.Image = Resources.FindNext;
            this.tsbFindNext.Name = "tsbFindNext";
            this.tsbFindNext.Click += new EventHandler(this.tsbFind_Click);
            this.tsbFindNext.Paint += new PaintEventHandler(this.tsbFind_Paint);
            manager.ApplyResources(this.tsbFindPrevious, "tsbFindPrevious");
            this.tsbFindPrevious.Image = Resources.FindPrev;
            this.tsbFindPrevious.Name = "tsbFindPrevious";
            this.tsbFindPrevious.Click += new EventHandler(this.tsbFind_Click);
            this.tsbFindPrevious.Paint += new PaintEventHandler(this.tsbFind_Paint);
            this.tslFindNonFound.Image = Resources.FindNotFound;
            this.tslFindNonFound.Name = "tslFindNonFound";
            manager.ApplyResources(this.tslFindNonFound, "tslFindNonFound");
            this.tsFolderInfo.ContextMenuStrip = this.cmsToolStrip;
            manager.ApplyResources(this.tsFolderInfo, "tsFolderInfo");
            this.tsFolderInfo.GripStyle = ToolStripGripStyle.Hidden;
            this.tsFolderInfo.Items.AddRange(new ToolStripItem[] { this.tslFolderSize, this.tssFolder1, this.tslItemCount, this.tssFolder2, this.tslFolderCount, this.tssFolder3, this.tslSelectionInfo, this.tssFolder4, this.tsbClearSelection, this.tsbUnlockFolder, this.tsbClearFilter });
            this.tsFolderInfo.Name = "tsFolderInfo";
            this.tsFolderInfo.VisibleChanged += new EventHandler(this.tsFolderInfo_VisibleChanged);
            this.tslFolderSize.Name = "tslFolderSize";
            this.tslFolderSize.Padding = new Padding(0, 3, 0, 3);
            manager.ApplyResources(this.tslFolderSize, "tslFolderSize");
            this.tssFolder1.Name = "tssFolder1";
            manager.ApplyResources(this.tssFolder1, "tssFolder1");
            this.tslItemCount.Name = "tslItemCount";
            this.tslItemCount.Padding = new Padding(0, 3, 0, 3);
            manager.ApplyResources(this.tslItemCount, "tslItemCount");
            this.tslItemCount.Click += new EventHandler(this.tslItemCount_Click);
            this.tssFolder2.Name = "tssFolder2";
            manager.ApplyResources(this.tssFolder2, "tssFolder2");
            this.tslFolderCount.Name = "tslFolderCount";
            this.tslFolderCount.Padding = new Padding(0, 3, 0, 3);
            manager.ApplyResources(this.tslFolderCount, "tslFolderCount");
            this.tslFolderCount.Click += new EventHandler(this.tslFolderCount_Click);
            this.tssFolder3.Name = "tssFolder3";
            manager.ApplyResources(this.tssFolder3, "tssFolder3");
            manager.ApplyResources(this.tslSelectionInfo, "tslSelectionInfo");
            this.tslSelectionInfo.Name = "tslSelectionInfo";
            this.tslSelectionInfo.Padding = new Padding(0, 3, 0, 3);
            this.tssFolder4.Name = "tssFolder4";
            manager.ApplyResources(this.tssFolder4, "tssFolder4");
            this.tsbClearSelection.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbClearSelection, "tsbClearSelection");
            this.tsbClearSelection.Name = "tsbClearSelection";
            this.tsbClearSelection.Click += new EventHandler(this.tsbClearSelection_Click);
            this.tsbClearSelection.EnabledChanged += new EventHandler(this.tsbFolderButton_EnabledChanged);
            this.tsbClearSelection.MouseEnter += new EventHandler(this.tsbFolderButton_MouseEnter);
            this.tsbClearSelection.MouseLeave += new EventHandler(this.tsbFolderButton_MouseLeave);
            this.tsbUnlockFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbUnlockFolder, "tsbUnlockFolder");
            this.tsbUnlockFolder.Name = "tsbUnlockFolder";
            this.tsbUnlockFolder.Click += new EventHandler(this.tsbUnlockFolder_Click);
            this.tsbUnlockFolder.EnabledChanged += new EventHandler(this.tsbFolderButton_EnabledChanged);
            this.tsbUnlockFolder.MouseEnter += new EventHandler(this.tsbFolderButton_MouseEnter);
            this.tsbUnlockFolder.MouseLeave += new EventHandler(this.tsbFolderButton_MouseLeave);
            this.tsbClearFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            manager.ApplyResources(this.tsbClearFilter, "tsbClearFilter");
            this.tsbClearFilter.Name = "tsbClearFilter";
            this.tsbClearFilter.Click += new EventHandler(this.tsbClearFilter_Click);
            this.tsbClearFilter.EnabledChanged += new EventHandler(this.tsbFolderButton_EnabledChanged);
            this.tsbClearFilter.MouseEnter += new EventHandler(this.tsbFolderButton_MouseEnter);
            this.tsbClearFilter.MouseLeave += new EventHandler(this.tsbFolderButton_MouseLeave);
            this.tsItemInfo.CanOverflow = false;
            this.tsItemInfo.ContextMenuStrip = this.cmsToolStrip;
            manager.ApplyResources(this.tsItemInfo, "tsItemInfo");
            this.tsItemInfo.GripStyle = ToolStripGripStyle.Hidden;
            this.tsItemInfo.Items.AddRange(new ToolStripItem[] { this.tslItemName, this.tslItemDate, this.tssItemDate, this.tslItemSize, this.tssItemSize });
            this.tsItemInfo.MinimumSize = new Size(0, 0x19);
            this.tsItemInfo.Name = "tsItemInfo";
            this.tsItemInfo.VisibleChanged += new EventHandler(this.InfoToolbar_VisibleChanged);
            this.tsItemInfo.Paint += new PaintEventHandler(this.tsItemInfo_Paint);
            manager.ApplyResources(this.tslItemName, "tslItemName");
            this.tslItemName.Margin = new Padding(2, 1, 0, 2);
            this.tslItemName.Name = "tslItemName";
            this.tslItemName.Padding = new Padding(0, 3, 0, 3);
            this.tslItemName.MouseDown += new MouseEventHandler(this.treeView_MouseDown);
            this.tslItemName.MouseLeave += new EventHandler(this.listView_MouseLeave);
            this.tslItemDate.Alignment = ToolStripItemAlignment.Right;
            this.tslItemDate.Name = "tslItemDate";
            this.tslItemDate.Padding = new Padding(0, 3, 0, 3);
            manager.ApplyResources(this.tslItemDate, "tslItemDate");
            this.tssItemDate.Alignment = ToolStripItemAlignment.Right;
            this.tssItemDate.Name = "tssItemDate";
            manager.ApplyResources(this.tssItemDate, "tssItemDate");
            this.tslItemSize.Alignment = ToolStripItemAlignment.Right;
            this.tslItemSize.Name = "tslItemSize";
            this.tslItemSize.Padding = new Padding(0, 3, 0, 3);
            manager.ApplyResources(this.tslItemSize, "tslItemSize");
            this.tssItemSize.Alignment = ToolStripItemAlignment.Right;
            this.tssItemSize.Name = "tssItemSize";
            manager.ApplyResources(this.tssItemSize, "tssItemSize");
            this.tsPath.ContextMenuStrip = this.cmsPath;
            this.tsPath.HideNotReadyDrives = false;
            manager.ApplyResources(this.tsPath, "tsPath");
            this.tsPath.Name = "tsPath";
            this.tsPath.AfterPaint += new PaintEventHandler(this.tsPath_AfterPaint);
            this.tsPath.CommandClicked += new EventHandler(this.tsPath_CommandClicked);
            this.tsPath.DriveClicked += new EventHandler<VirtualItemEventArgs>(this.tsPath_DriveClicked);
            this.tsPath.FolderClicked += new EventHandler<VirtualItemEventArgs>(this.tsPath_FolderClicked);
            this.tsPath.KeyUp += new KeyEventHandler(this.tsPath_KeyUp);
            this.cmsPath.Items.AddRange(new ToolStripItem[] { this.tsmiCopyPathAsText, this.tssPath1, this.tsmiBack, this.tsmiForward, this.tssPath2, this.tsmiChangeFolder, this.tssPath3, this.tsmiRefresh });
            this.cmsPath.Name = "cmsPath";
            manager.ApplyResources(this.cmsPath, "cmsPath");
            this.cmsPath.Opening += new CancelEventHandler(this.cmsPath_Opening);
            this.tsmiCopyPathAsText.Name = "tsmiCopyPathAsText";
            manager.ApplyResources(this.tsmiCopyPathAsText, "tsmiCopyPathAsText");
            this.tssPath1.Name = "tssPath1";
            manager.ApplyResources(this.tssPath1, "tssPath1");
            this.tsmiBack.Name = "tsmiBack";
            manager.ApplyResources(this.tsmiBack, "tsmiBack");
            this.tsmiForward.Name = "tsmiForward";
            manager.ApplyResources(this.tsmiForward, "tsmiForward");
            this.tssPath2.Name = "tssPath2";
            manager.ApplyResources(this.tssPath2, "tssPath2");
            this.tsmiChangeFolder.Name = "tsmiChangeFolder";
            manager.ApplyResources(this.tsmiChangeFolder, "tsmiChangeFolder");
            this.tssPath3.Name = "tssPath3";
            manager.ApplyResources(this.tssPath3, "tssPath3");
            this.tsmiRefresh.Name = "tsmiRefresh";
            manager.ApplyResources(this.tsmiRefresh, "tsmiRefresh");
            this.tmrUpdateItems.Enabled = true;
            this.tmrUpdateItems.Interval = 500;
            this.tmrUpdateItems.Tick += new EventHandler(this.tmrUpdateItems_Tick);
            this.tmrExpandNode.Interval = 0x5dc;
            this.tmrExpandNode.Tick += new EventHandler(this.tmrExpandNode_Tick);
            this.ItemToolTip.AutoPopDelay = 0x1388;
            this.ItemToolTip.InitialDelay = 0x5dc;
            this.ItemToolTip.ReshowDelay = 100;
            this.cmsColumns.Items.AddRange(new ToolStripItem[] { this.tsmiManageColumns, this.tssColumns1, this.tssColumns2, this.tssColumns3, this.tsmiRememberWidthAsDefault, this.tsmiResetDefaultWidth, this.tssColumns4, this.tsmiAutoSizeColumns });
            this.cmsColumns.Name = "cmsColumns";
            manager.ApplyResources(this.cmsColumns, "cmsColumns");
            this.cmsColumns.Closed += new ToolStripDropDownClosedEventHandler(this.cmsColumns_Closed);
            this.cmsColumns.Opening += new CancelEventHandler(this.cmsColumns_Opening);
            this.tsmiManageColumns.Name = "tsmiManageColumns";
            manager.ApplyResources(this.tsmiManageColumns, "tsmiManageColumns");
            this.tsmiManageColumns.Click += new EventHandler(this.tsmiManageColumns_Click);
            this.tssColumns1.Name = "tssColumns1";
            manager.ApplyResources(this.tssColumns1, "tssColumns1");
            this.tssColumns2.Name = "tssColumns2";
            manager.ApplyResources(this.tssColumns2, "tssColumns2");
            this.tssColumns3.Name = "tssColumns3";
            manager.ApplyResources(this.tssColumns3, "tssColumns3");
            this.tsmiRememberWidthAsDefault.Name = "tsmiRememberWidthAsDefault";
            manager.ApplyResources(this.tsmiRememberWidthAsDefault, "tsmiRememberWidthAsDefault");
            this.tsmiRememberWidthAsDefault.Click += new EventHandler(this.tsmiRememberWidthAsDefault_Click);
            this.tsmiResetDefaultWidth.Name = "tsmiResetDefaultWidth";
            manager.ApplyResources(this.tsmiResetDefaultWidth, "tsmiResetDefaultWidth");
            this.tsmiResetDefaultWidth.Click += new EventHandler(this.tsmiResetDefaultWidth_Click);
            this.tssColumns4.Name = "tssColumns4";
            manager.ApplyResources(this.tssColumns4, "tssColumns4");
            this.tsmiAutoSizeColumns.Name = "tsmiAutoSizeColumns";
            manager.ApplyResources(this.tsmiAutoSizeColumns, "tsmiAutoSizeColumns");
            this.tsmiAutoSizeColumns.Click += new EventHandler(this.tsmiAutoSizeColumns_Click);
            this.tmrToolTip.Tick += new EventHandler(this.tmrToolTip_Tick);
            this.imgThumbnail.ColorDepth = ColorDepth.Depth24Bit;
            manager.ApplyResources(this.imgThumbnail, "imgThumbnail");
            this.imgThumbnail.TransparentColor = System.Drawing.Color.Transparent;
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.splitContainer);
            base.Controls.Add(this.tsPath);
            base.Name = "VirtualFilePanel";
            base.Load += new EventHandler(this.VirtualFilePanel_Load);
            base.Enter += new EventHandler(this.VirtualFilePanel_Enter);
            base.Leave += new EventHandler(this.VirtualFilePanel_Leave);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.tsFind.ResumeLayout(false);
            this.tsFind.PerformLayout();
            this.cmsToolStrip.ResumeLayout(false);
            this.tsFolderInfo.ResumeLayout(false);
            this.tsFolderInfo.PerformLayout();
            this.tsItemInfo.ResumeLayout(false);
            this.tsItemInfo.PerformLayout();
            this.cmsPath.ResumeLayout(false);
            this.cmsColumns.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InvalidateItemIcon(IVirtualItem item)
        {
            if (this.FItems != null)
            {
                int index = -1;
                if ((this.CacheHitItems != null) && (this.CacheHitStartIndex > 0))
                {
                    int num2 = this.CacheHitStartIndex + this.CacheHitItems.Length;
                    for (int i = this.CacheHitStartIndex; i < num2; i++)
                    {
                        if (this.FItems[i].Equals(item))
                        {
                            index = i;
                            break;
                        }
                    }
                }
                if (index < 0)
                {
                    index = this.FItems.IndexOf(item);
                }
                if (index >= 0)
                {
                    Rectangle itemRect = this.listView.GetItemRect(index, ItemBoundsPortion.Icon);
                    if (this.listView.ClientRectangle.IntersectsWith(itemRect))
                    {
                        this.listView.Invalidate(itemRect);
                    }
                }
            }
        }

        protected void InvalidateListView(bool refresh)
        {
            if (this.UpdateCount > 0)
            {
                this.UpdateAction |= refresh ? 0x20 : 4;
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                if (refresh)
                {
                    this.listView.Parent.Refresh();
                }
                else
                {
                    this.listView.Invalidate();
                }
            }
        }

        public void InvertFocusedItemSelection(bool retrieveOnDemandProperties)
        {
            ListViewItem focusedItem = this.listView.FocusedItem;
            if (focusedItem != null)
            {
                IVirtualItem tag = (IVirtualItem) focusedItem.Tag;
                if (!tag.Equals(this.ParentFolder))
                {
                    int index = focusedItem.Index;
                    if (retrieveOnDemandProperties && !this.FSelection.Contains(tag))
                    {
                        if (this.listView.View == System.Windows.Forms.View.Details)
                        {
                            foreach (ColumnHeader header in this.listView.Columns)
                            {
                                if (header.Index > 0)
                                {
                                    ListViewColumnInfo info = (ListViewColumnInfo) header.Tag;
                                    int propertyId = info.PropertyId;
                                    if (tag.GetPropertyAvailability(propertyId) == PropertyAvailability.OnDemand)
                                    {
                                        object obj2 = tag[propertyId];
                                    }
                                }
                            }
                        }
                        object obj3 = tag[3];
                    }
                    this.ToggleSelectItem(tag);
                    this.OnSelectionChanged(EventArgs.Empty);
                    ListViewItem item = (index < (this.listView.Items.Count - 1)) ? this.listView.Items[index + 1] : null;
                    if (item != null)
                    {
                        this.FocusItem(item, true);
                    }
                    else
                    {
                        this.listView.RedrawItem(index, true);
                    }
                }
            }
        }

        public void InvertSelection(IVirtualItemFilter filter)
        {
            int startIndex = -1;
            int endIndex = 0;
            for (int i = 0; i < this.FItems.Count; i++)
            {
                IVirtualItem item = this.FItems[i];
                if (!(item.Equals(this.ParentFolder) || ((filter != null) && !filter.IsMatch(item))))
                {
                    this.ToggleSelectItem(item);
                    if (startIndex < 0)
                    {
                        startIndex = i;
                    }
                    endIndex = i;
                }
            }
            if (startIndex >= 0)
            {
                this.OnSelectionChanged(EventArgs.Empty);
                this.listView.RedrawItems(startIndex, endIndex, true);
            }
        }

        private static bool IsNameEqual(string Substr, string Str, bool IsPrefixSearch)
        {
            if (IsPrefixSearch)
            {
                return Str.StartsWith(Substr, StringComparison.OrdinalIgnoreCase);
            }
            return (Str.IndexOf(Substr, StringComparison.OrdinalIgnoreCase) >= 0);
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

        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
            }
            else
            {
                IChangeVirtualItem item = this.FItems[e.Item] as IChangeVirtualItem;
                if ((item == null) || string.Equals(item.Name, e.Label, StringComparison.Ordinal))
                {
                    e.CancelEdit = true;
                }
                else
                {
                    try
                    {
                        item.Name = e.Label;
                        this.ClearListViewCache();
                        if (this.listView.View == System.Windows.Forms.View.List)
                        {
                            base.BeginInvoke(new MethodInvoker(this.UpdateListView));
                        }
                    }
                    catch
                    {
                        e.CancelEdit = true;
                    }
                }
            }
        }

        private void listView_BeforeLabelEdit(object sender, BeforeLabelEditEventArgs e)
        {
            IVirtualItem item = this.FItems[e.Item];
            if (!(!item.Equals(this.ParentFolder) && (item is IChangeVirtualItem)))
            {
                e.CancelEdit = true;
            }
            else
            {
                e.MaxLength = 260;
                e.Label = item.Name;
                if ((this.SelectNameWithoutExt && !(item is IVirtualFolder)) && (e.Label.IndexOfAny(Path.GetInvalidFileNameChars()) < 0))
                {
                    int num;
                    e.SelectionLength = ((num = e.Label.LastIndexOf('.')) > 0) ? num : 0x7fffffff;
                }
                this.SelectNameWithoutExt = !this.SelectNameWithoutExt;
            }
        }

        private void listView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            if ((this.FItems != null) && (this.FItems.Count != 0))
            {
                if (this.CacheFirstItem == null)
                {
                    IVirtualItem item = this.FItems[0];
                    this.CacheFirstItem = this.CreateListViewItem(item, 0, item.Equals(this.ParentFolder));
                }
                if (this.FItems.Count >= 2)
                {
                    if (this.CacheEndItems == null)
                    {
                        int length = Math.Min(10, this.FItems.Count - 2);
                        this.CacheEndItems = this.CreateListViewItemCache(this.FItems.Count - length, length);
                    }
                    int startIndex = e.StartIndex;
                    if (startIndex == 0)
                    {
                        startIndex++;
                    }
                    int endIndex = e.EndIndex;
                    if (endIndex > (this.FItems.Count - this.CacheEndItems.Length))
                    {
                        endIndex = this.FItems.Count - this.CacheEndItems.Length;
                    }
                    if ((endIndex >= startIndex) && (((this.CacheHitItems == null) || (startIndex < this.CacheHitStartIndex)) || (endIndex > (this.CacheHitStartIndex + this.CacheHitItems.Length))))
                    {
                        this.CacheHitItems = this.CreateListViewItemCache(startIndex, endIndex - startIndex);
                        this.CacheHitStartIndex = startIndex;
                    }
                }
            }
        }

        private void listView_ClientSizeChanged(object sender, EventArgs e)
        {
            if (base.IsHandleCreated)
            {
                if (((this.listView.View == System.Windows.Forms.View.Details) && this.AutoSizeColumns) && this.CheckPanelState(PanelState.PopulatingItems))
                {
                    this.SetPanelState(PanelState.RedrawDisabled, true);
                    Windows.SendMessage(base.Handle, 11, IntPtr.Zero, IntPtr.Zero);
                }
                base.BeginInvoke(new MethodInvoker(this.UpdateListView));
            }
            else
            {
                this.UpdateListView();
            }
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListSortDirection sortDirection;
            int propertyId = ((ListViewColumnInfo) this.listView.Columns[e.Column].Tag).PropertyId;
            VirtualItemComparer sort = this.Sort as VirtualItemComparer;
            if (sort != null)
            {
                if (sort.ComparePropertyId == propertyId)
                {
                    sortDirection = (sort.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }
                else
                {
                    sortDirection = sort.SortDirection;
                }
            }
            else
            {
                sort = VirtualItemComparer.DefaultSort;
                sortDirection = sort.SortDirection;
            }
            this.Sort = new VirtualItemComparer(propertyId, sortDirection, sort.NameComparison);
        }

        private void listView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(this.UpdateColumnInfos));
        }

        private void listView_ColumnRightClick(object sender, ColumnClickEventArgs e)
        {
            this.ShowContextMenu = ContextMenuSource.Mouse;
            this.MouseColumnIndex = e.Column;
        }

        private void listView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            ColumnHeader header = this.listView.Columns[e.ColumnIndex];
            if (header.Tag != null)
            {
                ((ListViewColumnInfo) header.Tag).Width = header.Width;
            }
        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            DragImage.DragDrop((IWin32Window) sender, e);
            this.DropItemIndex = -1;
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.FDragDropOnItem != null))
            {
                Point point = this.listView.PointToClient(new Point(e.X, e.Y));
                ListViewHitTestInfo2 info = this.listView.HitTest(point);
                ListViewItem item = info.Item;
                if (((item != null) && (this.listView.View == System.Windows.Forms.View.Details)) && (info.ColumnIndex > 0))
                {
                    item = null;
                }
                IVirtualItem currentItem = null;
                if (item != null)
                {
                    currentItem = this.listView.VirtualMode ? this.FItems[item.Index] : ((IVirtualItem) item.Tag);
                    this.RaiseDragOverItem(currentItem, this.DropEffect, e);
                    if (e.Effect == DragDropEffects.None)
                    {
                        currentItem = null;
                    }
                }
                if (currentItem == null)
                {
                    currentItem = this.CurrentFolder;
                }
                this.FDragDropOnItem(this, new VirtualItemDragEventArg(currentItem, e));
            }
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            DragImage.DragEnter((IWin32Window) sender, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.CurrentFolder != null))
            {
                this.DropEffect = DragDropEffects.None;
                this.SetPanelState(PanelState.UseDragOverOptimization, true);
                if (e.Data.GetDataPresent("Preferred DropEffect"))
                {
                    object dataPresent = e.Data.GetDataPresent("Preferred DropEffect");
                    if (dataPresent is int)
                    {
                        this.DropEffect = (DragDropEffects) dataPresent;
                    }
                    else
                    {
                        MemoryStream input = dataPresent as MemoryStream;
                        if ((input != null) && (input.Length == 4L))
                        {
                            using (BinaryReader reader = new BinaryReader(input))
                            {
                                this.DropEffect = (DragDropEffects) reader.ReadInt32();
                            }
                        }
                    }
                }
                if (this.DropEffect == DragDropEffects.None)
                {
                    this.DropEffect = GetPrefferedDropEffect(VirtualItemHelper.GetFolderRoot(this.CurrentFolder), e.Data);
                }
                if (sender == this.listView)
                {
                    if (this.CheckPanelState(PanelState.UseDragOverOptimization))
                    {
                        try
                        {
                            e.Data.SetData("Nomad_Stored_DragOverItem", null);
                            e.Data.SetData("Nomad_Stored_KeyState", null);
                        }
                        catch
                        {
                            this.SetPanelState(PanelState.UseDragOverOptimization, false);
                        }
                    }
                    this.listView_DragOver(sender, e);
                }
                else if (sender == this.treeView)
                {
                    this.treeView_DragOver(sender, e);
                }
            }
        }

        private void listView_DragLeave(object sender, EventArgs e)
        {
            DragImage.DragLeave((IWin32Window) sender);
            this.DropEffect = DragDropEffects.None;
            this.DropItemIndex = -1;
            if (this.treeView.IsHandleCreated)
            {
                this.treeView.SetDropHilited(null);
            }
            this.tmrExpandNode.Stop();
        }

        private void listView_DragOver(object sender, DragEventArgs e)
        {
            DragImage.DragOver((Control) sender, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.FDragOverItem != null))
            {
                Point point = this.listView.PointToClient(new Point(e.X, e.Y));
                ListViewHitTestInfo2 info = this.listView.HitTest(point);
                ListViewItem item = info.Item;
                if (((item != null) && (this.listView.View == System.Windows.Forms.View.Details)) && (info.ColumnIndex > 0))
                {
                    item = null;
                }
                if (this.CheckPanelState(PanelState.UseDragOverOptimization))
                {
                    try
                    {
                        object data = e.Data.GetData("Nomad_Stored_DragOverItem");
                        object obj3 = e.Data.GetData("Nomad_Stored_KeyState");
                        int num = (item != null) ? item.Index : -1;
                        if ((((data != null) && (obj3 != null)) && (num == ((int) data))) && (e.KeyState == ((int) obj3)))
                        {
                            return;
                        }
                        e.Data.SetData("Nomad_Stored_DragOverItem", num);
                        e.Data.SetData("Nomad_Stored_KeyState", e.KeyState);
                    }
                    catch
                    {
                        this.SetPanelState(PanelState.UseDragOverOptimization, false);
                    }
                }
                IVirtualItem currentItem = null;
                if (item != null)
                {
                    currentItem = this.listView.VirtualMode ? this.FItems[item.Index] : ((IVirtualItem) item.Tag);
                }
                int index = -1;
                if (currentItem != null)
                {
                    this.RaiseDragOverItem(currentItem, this.DropEffect, e);
                    if (e.Effect == DragDropEffects.None)
                    {
                        currentItem = null;
                    }
                    else
                    {
                        index = item.Index;
                    }
                }
                if (currentItem == null)
                {
                    this.RaiseDragOverItem(this.CurrentFolder, this.DropEffect, e);
                }
                if (this.DropItemIndex != index)
                {
                    DragImage.Hide();
                    this.DropItemIndex = index;
                    DragImage.Show();
                }
            }
        }

        private void listView_Enter(object sender, EventArgs e)
        {
            this.UpdateFocusedItem();
            this.SetPanelState(PanelState.UpdateFocusSelectionNeeded, true);
        }

        private void listView_GetItemColors(object sender, GetItemColorsEventArgs e)
        {
            ListViewEx ex = (ListViewEx) sender;
            bool flag = (e.State & ListViewItemStates.Selected) == 0;
            if ((((e.State & ListViewItemStates.Focused) > 0) && !ex.ExplorerTheme) && this.CheckPanelState(PanelState.LastParentFormActive | PanelState.LastContainsFocus))
            {
                if (this.CheckPanelState(PanelState.UseFocusSelection))
                {
                    e.BackColor = SystemColors.Highlight;
                    e.ForeColor = SystemColors.HighlightText;
                    flag = false;
                }
                else
                {
                    e.BackColor = this.ListFocusedBackColor;
                    if (!this.ListFocusedForeColor.IsEmpty)
                    {
                        e.ForeColor = this.ListFocusedForeColor;
                    }
                }
            }
            else if (!(((ex.View != System.Windows.Forms.View.Details) || ((e.ItemIndex % 2) != 0)) || this.OddLineBackColor.IsEmpty))
            {
                e.BackColor = this.OddLineBackColor;
            }
            if (ex.ExplorerTheme && ((e.State & (ListViewItemStates.Hot | ListViewItemStates.Focused | ListViewItemStates.Selected)) > 0))
            {
                e.ForeColor = e.Item.ForeColor;
                flag = false;
            }
            if (flag && ImageHelper.IsCloseColors(e.ForeColor, e.BackColor))
            {
                e.ForeColor = ex.ForeColor;
            }
        }

        private void listView_GetItemState(object sender, GetItemStateEventArgs e)
        {
            e.Cut = ((this.FItems != null) && (FCuttedItems != null)) && FCuttedItems.Contains(this.FItems[e.ItemIndex]);
            e.DropHilited = e.ItemIndex == this.DropItemIndex;
        }

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem item = (ListViewItem) e.Item;
            IVirtualItem tag = (IVirtualItem) item.Tag;
            if (!tag.Equals(this.ParentFolder))
            {
                Image dragImage = null;
                if (this.ShowItemIcons)
                {
                    dragImage = VirtualIcon.GetIcon(tag, ImageHelper.DefaultSmallIconSize);
                }
                System.Drawing.Color foreColor = VirtualItemHelper.GetForeColor(tag, this.listView.ForeColor);
                Point hotSpot = this.listView.PointToClient(Cursor.Position);
                hotSpot.Offset(-item.Bounds.Left, -item.Bounds.Top);
                using (new DragImage(dragImage, tag.Name, this.listView.Font, foreColor, hotSpot))
                {
                    IEnumerable<IVirtualItem> selection;
                    if (this.FSelection.Contains(tag))
                    {
                        selection = this.Selection;
                    }
                    else
                    {
                        selection = new IVirtualItem[] { tag };
                    }
                    DragDropEffects copy = DragDropEffects.Copy;
                    foreach (IVirtualItem item3 in selection)
                    {
                        if (item3 is IChangeVirtualItem)
                        {
                            copy |= DragDropEffects.Move;
                        }
                        if (item3 is ICreateVirtualLink)
                        {
                            copy |= DragDropEffects.Link;
                        }
                    }
                    this.listView.DoDragDrop(new VirtualItemDataObject(selection, false), copy);
                }
            }
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListViewEx ex = (ListViewEx) sender;
            ListViewItem item = e.Item;
            if (((!this.CheckPanelState(PanelState.UseFocusSelection) && !ex.ExplorerTheme) && (!ex.IsBoxSelectionActive && ((Control.ModifierKeys & Keys.Shift) == Keys.None))) && ex.Focused)
            {
                item.Selected = false;
            }
            if (item.Focused && (e.ItemIndex != this.FocusedIndex))
            {
                this.FocusedIndex = e.ItemIndex;
                IVirtualItem tag = (IVirtualItem) item.Tag;
                if (this.tslItemName.Tag != tag)
                {
                    this.ShowFocusedItemInfo(tag);
                }
                if ((((Settings.Default.ShowItemToolTips && Settings.Default.ShowItemTooltipsKbd) && ex.Focused) && !this.CheckPanelState(PanelState.SkipKeyboardTooltip)) && ((this.HoverItem == null) || (this.HoverItem.Index != e.ItemIndex)))
                {
                    this.HideItemToolTip();
                    this.tmrToolTip.Tag = item;
                    this.tmrToolTip.Interval = OS.MouseHoverTime * 2;
                    this.tmrToolTip.Start();
                }
                this.SetPanelState(PanelState.SkipMouseTooltip, true);
            }
        }

        private void listView_ItemTooltip(object sender, ItemTooltipEventArgs e)
        {
            e.Cancel = true;
            if ((Settings.Default.ShowItemToolTips && !this.CheckPanelState(PanelState.SkipMouseTooltip)) && !this.listView.IsEditing)
            {
                Point position = Cursor.Position;
                position = this.listView.PointToClient(position);
                if (this.listView.GetItemAt(position.X, position.Y) == e.Item)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(e.Tooltip);
                    IVirtualItemUI tag = e.Item.Tag as IVirtualItemUI;
                    if (tag != null)
                    {
                        string toolTip = tag.ToolTip;
                        if (!string.IsNullOrEmpty(toolTip))
                        {
                            if (builder.Length > 0)
                            {
                                builder.AppendLine();
                            }
                            builder.Append(toolTip);
                        }
                    }
                    if (builder.Length != 0)
                    {
                        position.Y += this.Cursor.GetPrefferedHeight();
                        VirtualToolTip.Default.ShowTooltip(tag, builder.ToString(), this.listView, position.X, position.Y);
                        this.HoverItem = e.Item;
                    }
                }
            }
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    switch (this.listView.View)
                    {
                        case System.Windows.Forms.View.Details:
                            if ((this.listView.ScollBarsVisibility & ScrollBars.Horizontal) == ScrollBars.None)
                            {
                                if (!(this.FSettings.ShowUpFolderItem && !VirtualItemHelper.Equals(this.ParentFolder, this.FocusedItem)))
                                {
                                    this.SetCurrentFolder(this.ParentFolder, true);
                                    e.SuppressKeyPress = true;
                                }
                                else if (this.listView.Items.Count > 0)
                                {
                                    this.FocusItem(this.listView.Items[0], true);
                                }
                            }
                            break;

                        case System.Windows.Forms.View.List:
                        {
                            ListViewItem focusedItem = this.listView.FocusedItem;
                            if (focusedItem != null)
                            {
                                if (VirtualItemHelper.Equals(this.ParentFolder, (IVirtualItem) focusedItem.Tag))
                                {
                                    this.SetCurrentFolder(this.ParentFolder, true);
                                }
                                else if (focusedItem.Index < this.listView.RowCount)
                                {
                                    this.FocusItem(this.listView.TopItem, true);
                                }
                            }
                            break;
                        }
                    }
                    break;

                case Keys.Right:
                    switch (this.listView.View)
                    {
                        case System.Windows.Forms.View.Details:
                            if ((this.listView.ScollBarsVisibility & ScrollBars.Horizontal) == ScrollBars.None)
                            {
                                IVirtualFolder folder = this.FocusedItem as IVirtualFolder;
                                if (!((folder == null) || folder.Equals(this.ParentFolder)))
                                {
                                    this.SetCurrentFolder(folder, true);
                                    e.SuppressKeyPress = true;
                                }
                                else if (this.listView.Items.Count > 0)
                                {
                                    this.FocusItem(this.listView.Items[this.listView.Items.Count - 1], true);
                                }
                            }
                            break;

                        case System.Windows.Forms.View.List:
                        {
                            int focusedItemIndex = this.listView.FocusedItemIndex;
                            if (((focusedItemIndex >= 0) && (this.listView.Items.Count > 0)) && ((this.listView.Items.Count - focusedItemIndex) < this.listView.RowCount))
                            {
                                this.FocusItem(this.listView.Items[this.listView.Items.Count - 1], true);
                            }
                            break;
                        }
                    }
                    break;

                case Keys.Escape:
                    this.CancelAsyncFolder();
                    this.HideItemToolTip();
                    break;

                case Keys.BrowserBack:
                case (Keys.Alt | Keys.Left):
                    this.Back();
                    break;

                case Keys.BrowserForward:
                case (Keys.Alt | Keys.Right):
                    this.Forward();
                    break;

                case (Keys.Control | Keys.Left):
                case (Keys.Control | Keys.Right):
                    this.tsPath.StartDriveSelection();
                    this.tsPath.SelectNextDrive(e.KeyCode == Keys.Right);
                    e.Handled = true;
                    break;

                case (Keys.Control | Keys.Add):
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void listView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                this.tsFind.Visible = true;
                this.tsFind.Focus();
                this.tstFind.Text = new string(e.KeyChar, 1);
                this.tstFind.Focus();
                this.tstFind.SelectionStart = 1;
                this.tstFind.SelectionLength = 0;
                e.Handled = true;
            }
        }

        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.ShiftKey) && (this.listView.SelectedIndices.Count > 1))
            {
                this.SelectIndices();
                this.UpdateFocusedItem();
            }
        }

        private void listView_Leave(object sender, EventArgs e)
        {
            this.HideItemToolTip();
            if (!((!this.listView.IsHandleCreated || this.listView.IsDisposed) || this.listView.Disposing))
            {
                this.listView.BeginInvoke(new MethodInvoker(this.UpdateFocusedItem));
            }
        }

        private void listView_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (!(this.listView.Capture || !this.listView.IsBoxSelectionActive))
            {
                this.SelectIndices();
            }
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.listView.IsEditing && (e.Button == MouseButtons.Left))
            {
                ListViewItem itemAtEx = this.GetItemAtEx(e.X, e.Y);
                if (itemAtEx != null)
                {
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        IVirtualItem tag = (IVirtualItem) itemAtEx.Tag;
                        if (!tag.Equals(this.ParentFolder))
                        {
                            this.ToggleSelectItem(tag);
                            this.OnSelectionChanged(EventArgs.Empty);
                        }
                    }
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        this.SelectIndices();
                    }
                    ListViewItem focusedItem = this.listView.FocusedItem;
                    if ((focusedItem == null) || (focusedItem.Index != itemAtEx.Index))
                    {
                        this.FocusItem(itemAtEx, false);
                    }
                    else
                    {
                        this.UpdateFocusedItem();
                    }
                }
                this.LastMousePosition = e.Location;
                this.HideItemToolTip();
            }
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (Control.ModifierKeys == Keys.None))
            {
                ListViewItem itemAtEx = this.GetItemAtEx(e.X, e.Y);
                if (itemAtEx != null)
                {
                    if (itemAtEx.Index == this.listView.FocusedItemIndex)
                    {
                        this.VirtualItemExecute((IVirtualItem) itemAtEx.Tag);
                    }
                }
                else
                {
                    switch (VirtualFilePanelSettings.Default.EmptySpaceDoubleClickAction)
                    {
                        case DoubleClickAction.GoToParent:
                            this.UpFolder();
                            return;

                        case DoubleClickAction.SelectAll:
                            this.SelectAll();
                            return;

                        case DoubleClickAction.UnselectAll:
                            this.UnselectItems(null);
                            return;

                        case DoubleClickAction.ToggleSelection:
                            if (this.FSelection.Count <= 0)
                            {
                                this.SelectAll();
                                return;
                            }
                            this.UnselectItems(null);
                            return;
                    }
                }
            }
        }

        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.listView.IsEditing)
            {
                return;
            }
            this.RightMoveSelect = ClickSelectMode.None;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    this.SetPanelState(PanelState.UpdateFocusSelectionNeeded, this.listView.GetItemAt(e.X, e.Y) == null);
                    break;

                case MouseButtons.Right:
                    this.ShowContextMenu = ContextMenuSource.Mouse;
                    if (Settings.Default.RightClickSelect)
                    {
                        ListViewHitTestInfo2 info = this.listView.HitTest(e.X, e.Y);
                        if (((info.Location & ListViewHitTestLocations.Label) > 0) || (info.ColumnIndex > 0))
                        {
                            IVirtualItem tag = (IVirtualItem) info.Item.Tag;
                            if (!tag.Equals(this.ParentFolder))
                            {
                                this.ToggleSelectItem(tag);
                                this.OnSelectionChanged(EventArgs.Empty);
                                this.listView.RedrawItem(info.Item.Index, true);
                                this.RightMoveSelect = this.FSelection.Contains(tag) ? ClickSelectMode.Select : ClickSelectMode.Unselect;
                            }
                            this.ShowContextMenu = ContextMenuSource.Ignore;
                            this.HideItemToolTip();
                        }
                    }
                    else
                    {
                        this.SetPanelState(PanelState.UpdateFocusSelectionNeeded, this.listView.GetItemAt(e.X, e.Y) == null);
                    }
                    goto Label_015A;
            }
        Label_015A:
            this.listView.MultiSelect = this.RightMoveSelect == ClickSelectMode.None;
        }

        private void listView_MouseLeave(object sender, EventArgs e)
        {
            this.HideItemToolTip();
        }

        private void listView_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.LastMousePosition != e.Location)
            {
                ListViewItem itemAt;
                this.LastMousePosition = e.Location;
                if (((e.Button & MouseButtons.Right) > MouseButtons.None) && (this.RightMoveSelect != ClickSelectMode.None))
                {
                    itemAt = this.listView.GetItemAt(e.X, e.Y);
                    if (itemAt != null)
                    {
                        int count = this.FSelection.Count;
                        IVirtualItem tag = (IVirtualItem) itemAt.Tag;
                        if (this.FSelection.Contains(tag))
                        {
                            if (this.RightMoveSelect == ClickSelectMode.Unselect)
                            {
                                this.FSelection.Remove(tag);
                            }
                        }
                        else if (this.RightMoveSelect == ClickSelectMode.Select)
                        {
                            this.FSelection.Add(tag);
                        }
                        this.FocusItem(itemAt, false);
                        this.HideItemToolTip();
                        if (this.FSelection.Count != count)
                        {
                            this.OnSelectionChanged(EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    Form activeForm = Form.ActiveForm;
                    if ((activeForm == null) || (activeForm != base.FindForm()))
                    {
                        this.HideItemToolTip();
                    }
                    else
                    {
                        itemAt = this.listView.GetItemAt(e.X, e.Y);
                        if (((itemAt == null) || (this.HoverItem == null)) || (this.HoverItem.Index != itemAt.Index))
                        {
                            this.HideItemToolTip();
                        }
                    }
                    this.SetPanelState(PanelState.SkipMouseTooltip, false);
                }
            }
        }

        private void listView_PostDrawItem(object sender, PostDrawListViewItemEventArgs e)
        {
            Size defaultLargeIconSize;
            ListViewEx ex = (ListViewEx) sender;
            switch (ex.View)
            {
                case System.Windows.Forms.View.LargeIcon:
                    if ((ex.LargeImageList != null) && (ex.LargeImageList != this.imgThumbnail))
                    {
                        defaultLargeIconSize = ImageHelper.DefaultLargeIconSize;
                        break;
                    }
                    return;

                case System.Windows.Forms.View.Details:
                case System.Windows.Forms.View.SmallIcon:
                case System.Windows.Forms.View.List:
                    if (ex.SmallImageList != null)
                    {
                        defaultLargeIconSize = ImageHelper.DefaultSmallIconSize;
                        break;
                    }
                    return;

                default:
                    return;
            }
            if (((this.FItems != null) && (e.ItemIndex < this.FItems.Count)) && (e.State != 0))
            {
                IVirtualItem item = this.FItems[e.ItemIndex];
                System.Drawing.Color empty = System.Drawing.Color.Empty;
                float blendLevel = 0.7f;
                if (!(ex.ExplorerTheme || ((((e.State & ListViewItemStates.Selected) <= 0) || !ex.Focused) && (e.ItemIndex != this.DropItemIndex))))
                {
                    empty = SystemColors.Highlight;
                }
                else if (this.FSelection.Contains(item))
                {
                    empty = this.ListSelectedForeColor;
                }
                else if ((FCuttedItems != null) && FCuttedItems.Contains(item))
                {
                    empty = System.Drawing.Color.White;
                }
                Rectangle itemRect = this.listView.GetItemRect(e.ItemIndex, ItemBoundsPortion.Icon);
                if (!itemRect.IsEmpty && ex.ClientRectangle.IntersectsWith(itemRect))
                {
                    Image image = VirtualIcon.GetIcon(item, defaultLargeIconSize, IconStyle.CanUseDelayedExtract);
                    if (image != null)
                    {
                        if (empty.IsEmpty)
                        {
                            VirtualHighligher highlighter = VirtualIcon.GetHighlighter(item);
                            if ((highlighter != null) && highlighter.AlphaBlend)
                            {
                                empty = highlighter.BlendColor;
                                blendLevel = highlighter.BlendLevel;
                            }
                        }
                        if (((e.State & ListViewItemStates.Focused) > 0) && !ex.ExplorerTheme)
                        {
                            using (Brush brush = new SolidBrush(ex.BackColor))
                            {
                                e.Graphics.FillRectangle(brush, itemRect);
                            }
                        }
                        lock (image)
                        {
                            switch (ex.View)
                            {
                                case System.Windows.Forms.View.Details:
                                case System.Windows.Forms.View.SmallIcon:
                                case System.Windows.Forms.View.List:
                                    itemRect.X += itemRect.Width - image.Width;
                                    break;

                                default:
                                    itemRect.X += (itemRect.Width - image.Width) / 2;
                                    break;
                            }
                            itemRect.Y += (itemRect.Height - image.Height) / 2;
                            if (empty.IsEmpty)
                            {
                                e.Graphics.DrawImage(image, itemRect.Location);
                            }
                            else
                            {
                                ImageHelper.DrawBlendImage(e.Graphics, image, empty, blendLevel, itemRect.X, itemRect.Y);
                            }
                        }
                    }
                }
            }
        }

        private void listView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if ((e.KeyState & 0x12) > 0)
            {
                e.Action = DragAction.Cancel;
            }
        }

        private void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex == 0)
            {
                e.Item = this.CacheFirstItem;
            }
            else if (((this.CacheEndItems != null) && (this.FItems != null)) && (e.ItemIndex >= (this.FItems.Count - this.CacheEndItems.Length)))
            {
                e.Item = this.CacheEndItems[(e.ItemIndex - this.FItems.Count) + this.CacheEndItems.Length];
            }
            else
            {
                int num = e.ItemIndex - this.CacheHitStartIndex;
                if (((this.CacheHitItems != null) && (num >= 0)) && (num < this.CacheHitItems.Length))
                {
                    e.Item = this.CacheHitItems[e.ItemIndex - this.CacheHitStartIndex];
                }
            }
            if ((e.Item == null) && (this.FItems != null))
            {
                IVirtualItem item = this.FItems[e.ItemIndex];
                e.Item = this.CreateListViewItem(item, e.ItemIndex, VirtualFilePanelSettings.Default.ShowUpFolderItem && item.Equals(this.ParentFolder));
                if (e.ItemIndex == 0)
                {
                    this.CacheFirstItem = e.Item;
                }
            }
        }

        private void listView_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            if (e.IsTextSearch)
            {
                int num = this.FindVirtualItem(e.Text, e.StartIndex, e.Direction, e.IsPrefixSearch);
                if (num >= 0)
                {
                    e.Index = num;
                }
            }
        }

        public void LoadComponentSettings()
        {
            base.SuspendLayout();
            this.BeginListViewUpdate(false, true);
            this.FindOptions = this.FSettings.QuickFindOptions;
            Nomad.Configuration.PanelLayout layout = this.FSettings.Layout;
            if (layout == null)
            {
                this.FolderBarVisible = false;
                this.Columns = VirtualFilePanelSettings.DefaultColumns;
            }
            else
            {
                if ((layout.StoreEntry & PanelLayoutEntry.Columns) == PanelLayoutEntry.None)
                {
                    layout.Columns = VirtualFilePanelSettings.DefaultColumns;
                    layout.StoreEntry |= PanelLayoutEntry.Columns;
                }
                this.PanelLayout = this.FSettings.Layout;
            }
            this.PanelContent = this.FSettings.Content;
            this.EndListViewUpdate();
            base.ResumeLayout();
        }

        public void ManageColumns(Form parentForm)
        {
            DesktopIni desktopIni = VirtualItem.GetDesktopIni(this.CurrentFolder, false);
            bool rememberColumns = false;
            IEnumerable<ListViewColumnInfo> collection = null;
            using (ManageColumnsDialog dialog = new ManageColumnsDialog())
            {
                if (parentForm != null)
                {
                    parentForm.AddOwnedForm(dialog);
                }
                dialog.AutosizeColumns = this.AutoSizeColumns;
                dialog.AvailableProperties = this.AvailableProperties;
                dialog.Columns = this.GetAllColumns();
                dialog.RememberColumnsEnabled = desktopIni != null;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    rememberColumns = dialog.RememberColumns;
                    collection = dialog.Columns;
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberColumnInfoMap = null;
                    }
                    this.ColumnInfoMap.Clear();
                    foreach (ListViewColumnInfo info in collection)
                    {
                        this.ColumnInfoMap.Add(info.PropertyId, info);
                    }
                    this.BeginListViewUpdate(this.listView.View == System.Windows.Forms.View.Details, false);
                    try
                    {
                        if (this.listView.View == System.Windows.Forms.View.Details)
                        {
                            this.RecreateColumns();
                        }
                        this.AutoSizeColumns = dialog.AutosizeColumns;
                        this.UpdateListView();
                    }
                    finally
                    {
                        this.EndListViewUpdate();
                    }
                }
            }
            if (rememberColumns)
            {
                try
                {
                    desktopIni.Read();
                    desktopIni.AutoSizeColumns = new bool?(this.AutoSizeColumns);
                    desktopIni.View = new PanelView?(this.View);
                    desktopIni.Columns = new List<ListViewColumnInfo>(collection).ToArray();
                    desktopIni.Write();
                }
                catch (Exception exception)
                {
                    if (!VirtualItem.IsWarningIOException(exception))
                    {
                        throw;
                    }
                    MessageDialog.ShowException(this, exception, true);
                }
            }
        }

        protected void OnCurrentFolderChanged(VirtualFolderChangedEventArgs e)
        {
            if (this.CurrentFolderChanged != null)
            {
                this.CurrentFolderChanged(this, e);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            MethodInvoker method = null;
            base.OnEnabledChanged(e);
            if (!base.DesignMode)
            {
                if (base.Enabled && base.Visible)
                {
                    if (this.CurrentFolder == null)
                    {
                        if (!this.RestoreFromLazyFolder())
                        {
                            this.SetDefaultPathAsCurrent();
                        }
                        this.tmrUpdateItems.Enabled = true;
                    }
                    else
                    {
                        if (this.FItems == null)
                        {
                            this.FolderChangeRequested = WatcherChangeTypes.All;
                            this.SetPanelState(PanelState.DoFolderChangedTick, true);
                        }
                        if (base.IsHandleCreated)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.tmrUpdateItems_Tick(this.tmrUpdateItems, EventArgs.Empty);
                                    this.ProcessDesktopIni();
                                    if (this.tsPath.CurrentFolder == null)
                                    {
                                        this.UpdatePathCurrentFolder(this.FCurrentFolder);
                                    }
                                    this.tmrUpdateItems.Enabled = true;
                                };
                            }
                            base.BeginInvoke(method);
                        }
                        else
                        {
                            this.tmrUpdateItems_Tick(this.tmrUpdateItems, EventArgs.Empty);
                            this.ProcessDesktopIni();
                            this.tmrUpdateItems.Enabled = true;
                        }
                    }
                }
                else
                {
                    this.tmrUpdateItems.Enabled = base.Enabled;
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            MethodInvoker method = null;
            base.OnLayout(e);
            if ((e.AffectedControl == this) && ((e.AffectedProperty == "Bounds") || (e.AffectedProperty == "Parent")))
            {
                this.SetPanelState(PanelState.ProcessingOnLayout, true);
                try
                {
                    if (this.NewOrientation.HasValue)
                    {
                        this.FolderBarOrientation = this.NewOrientation.Value;
                        this.NewOrientation = null;
                    }
                    if (this.NewSplitterPercent >= 0)
                    {
                        this.SplitterPercent = this.NewSplitterPercent;
                        if (base.IsHandleCreated)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.NewSplitterPercent = -1;
                                };
                            }
                            base.BeginInvoke(method);
                        }
                    }
                }
                finally
                {
                    this.SetPanelState(PanelState.ProcessingOnLayout, false);
                }
            }
        }

        protected void OnSelectionChanged(EventArgs e)
        {
            this.ClearListViewCache();
            this.tsbClearSelection.Enabled = this.FSelection.Count > 0;
            this.UpdateSelectionInfo();
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, e);
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            WaitCallback callBack = null;
            base.OnVisibleChanged(e);
            if (!base.DesignMode)
            {
                if (!base.Visible)
                {
                    if (this.CurrentFolder != null)
                    {
                        this.FLazyFolder = new VirtualItemContainer<IVirtualFolder>();
                        this.FLazyFolder.Value = this.CurrentFolder;
                        this.SetCurrentFolder(null, false);
                        this.treeView.Clear();
                    }
                }
                else
                {
                    if (this.CheckPanelState(PanelState.BindingsNeeded) && base.Enabled)
                    {
                        this.SetPanelState(PanelState.BindingsNeeded, false);
                        this.InitializeBindings();
                    }
                    if (this.CurrentFolder == null)
                    {
                        if (callBack == null)
                        {
                            callBack = delegate (object target) {
                                if (!((this.CurrentFolder != null) || this.RestoreFromLazyFolder()))
                                {
                                    this.SetDefaultPathAsCurrent();
                                }
                            };
                        }
                        InitializeQueue.QueueUserWorkItem(callBack);
                    }
                }
            }
        }

        public void OpenRecentFolders()
        {
            this.tsPath.OpenRecentFolders();
        }

        private void ParentForm_Activated(object sender, EventArgs e)
        {
            this.SetPanelState(PanelState.LastParentFormActive, true);
            ((Form) sender).BeginInvoke(new Action<IVirtualFolder, bool>(this.SetUpdateTimerInterval), new object[] { this.FCurrentFolder, true });
            this.tsPath.Active = base.ContainsFocus || this.CheckPanelState(PanelState.LastContainsFocus);
            if (this.listView.Focused)
            {
                this.listView_Enter(sender, e);
            }
        }

        private void ParentForm_Deactivate(object sender, EventArgs e)
        {
            this.SetPanelState(PanelState.LastParentFormActive, false);
            this.SetUpdateTimerInterval(this.FCurrentFolder, false);
            if (this.listView.Focused)
            {
                this.listView_Leave(sender, e);
            }
            else
            {
                this.HideItemToolTip();
            }
        }

        private void ParentForm_ResizeBegin(object sender, EventArgs e)
        {
            this.HideItemToolTip();
            this.BeginListViewUpdate(false, false);
            this.SetPanelState(PanelState.ParentFormResizing, true);
        }

        private void ParentForm_ResizeEnd(object sender, EventArgs e)
        {
            this.SetPanelState(PanelState.ParentFormResizing, false);
            this.EndListViewUpdate();
        }

        private void ParentForm_WindowStateChanging(object sender, WindowStateChangingEventArgs e)
        {
            if ((e.NewWindowState == FormWindowState.Maximized) || (e.NewWindowState == FormWindowState.Normal))
            {
                this.SetPanelState(PanelState.RedrawDisabled, true);
                Windows.SendMessage(base.Handle, 11, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public void PopSelection()
        {
            this.Selection = this.StoredSelection;
            this.StoredSelection = null;
        }

        private void PopulateListViewItems(IEnumerable<IVirtualItem> content, ListViewSort sort)
        {
            IEnumerable<IVirtualItem> enumerable;
            IVirtualItemFilter hiddenItemsFilter = VirtualFilePanelSettings.Default.HiddenItemsFilter;
            if (!this.listView.VirtualMode)
            {
                lock ((enumerable = content))
                {
                    this.FItems = new HashList<IVirtualItem>(content);
                }
                this.BeginListViewUpdate(true, false);
                try
                {
                    ListViewItem item3;
                    this.listView.Items.Clear();
                    if (this.ParentFolder != null)
                    {
                        item3 = this.CreateListViewItem(this.ParentFolder, -1, true);
                        this.listView.Items.Add(item3);
                    }
                    foreach (IVirtualItem item2 in this.FItems)
                    {
                        item3 = this.CreateListViewItem(item2, -1, false);
                        this.listView.Items.Add(item3);
                    }
                    if (this.listView.FocusedItem != null)
                    {
                        this.listView.FocusedItem.Selected = true;
                    }
                }
                finally
                {
                    this.EndListViewUpdate();
                }
            }
            else
            {
                HashList<IVirtualItem> list;
                lock ((enumerable = content))
                {
                    ICollection<IVirtualItem> is2 = content as ICollection<IVirtualItem>;
                    list = new HashList<IVirtualItem>((is2 != null) ? is2.Count : 0x80);
                    foreach (IVirtualItem item in content)
                    {
                        if (!this.RemoveByFilter(item, this.Filter, hiddenItemsFilter))
                        {
                            list.Add(item);
                        }
                    }
                }
                try
                {
                    this.FocusedIndex = -1;
                    this.SetPanelState(PanelState.PopulatingItems, true);
                    int num = list.Count + ((this.ParentFolder != null) ? 1 : 0);
                    if (this.listView.VirtualListSize > num)
                    {
                        try
                        {
                            this.listView.VirtualListSize = num;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            if (this.listView.View != System.Windows.Forms.View.Details)
                            {
                                throw;
                            }
                        }
                    }
                    if (sort == ListViewSort.Full)
                    {
                        list.Sort(this.Sort);
                    }
                    else if (((sort == ListViewSort.Fast) && (this.FItems != null)) && (this.FItems.Count > 0))
                    {
                        list = this.FastHashListSort<IVirtualItem>(list, this.FItems, this.FItems[0].Equals(this.ParentFolder) ? 1 : 0);
                    }
                    this.FItems = list;
                    this.RecreateSelection();
                    this.UpdateFolderInfo(true);
                    if ((this.ParentFolder != null) && VirtualFilePanelSettings.Default.ShowUpFolderItem)
                    {
                        this.FItems.Insert(0, this.ParentFolder);
                    }
                    if ((this.treeView.Visible && (this.treeView.SelectedNode != null)) && ((IVirtualFolder) this.treeView.SelectedNode.Tag).Equals(this.CurrentFolder))
                    {
                        bool flag = false;
                        foreach (IVirtualItem item2 in this.FItems)
                        {
                            if (item2 is IVirtualFolder)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this.treeView.SelectedNode.Nodes.Clear();
                        }
                        else if (this.treeView.SelectedNode.Nodes.Count == 0)
                        {
                            this.treeView.SelectedNode.Collapse();
                        }
                    }
                }
                finally
                {
                    this.ClearListViewCache();
                    if (this.FItems.Count != this.listView.VirtualListSize)
                    {
                        try
                        {
                            this.listView.VirtualListSize = this.FItems.Count;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            if (this.listView.View != System.Windows.Forms.View.Details)
                            {
                                throw;
                            }
                        }
                    }
                }
                if ((this.listView.View == System.Windows.Forms.View.List) && VirtualFilePanelSettings.Default.OptimizedColumnCount)
                {
                    this.UpdateListView();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.tstFind.Focused && ((keyData & Keys.KeyCode) == Keys.Return))
            {
                this.listView.Select();
                if ((this.FindOptions & QuickFindOptions.ExecuteOnEnter) > 0)
                {
                    this.VirtualItemExecute(this.FocusedItem);
                }
                return true;
            }
            if ((this.listView.Focused && this.treeView.Visible) && (keyData == (Keys.Shift | Keys.Tab)))
            {
                this.treeView.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProcessCustomizeFolder(ICustomizeFolder customizeFolder)
        {
            bool flag = true;
            bool flag2 = true;
            bool flag3 = true;
            bool flag4 = true;
            bool flag5 = true;
            bool flag6 = true;
            bool flag7 = true;
            bool flag8 = true;
            bool flag9 = true;
            bool flag10 = true;
            if (customizeFolder != null)
            {
                this.SetPanelState(PanelState.ProcessingCustomizeFolder, true);
                try
                {
                    bool? autoSizeColumns = customizeFolder.AutoSizeColumns;
                    if (autoSizeColumns.HasValue)
                    {
                        if (!this.RememberAutoSizeColumns.HasValue)
                        {
                            this.RememberAutoSizeColumns = new bool?(this.AutoSizeColumns);
                        }
                        this.AutoSizeColumns = autoSizeColumns.Value;
                        flag = false;
                    }
                    ListViewColumnInfo[] columns = customizeFolder.Columns;
                    if ((columns != null) && (columns.Length > 0))
                    {
                        if (this.RememberColumnInfoMap == null)
                        {
                            this.RememberColumnInfoMap = this.FColumnInfoMap;
                        }
                        this.Columns = columns;
                        flag2 = false;
                    }
                    IVirtualItemFilter filter = customizeFolder.Filter;
                    if (filter != null)
                    {
                        if (!this.CheckPanelState(PanelState.HasRememberFilter))
                        {
                            this.RememberFilter = this.Filter;
                            this.SetPanelState(PanelState.HasRememberFilter, true);
                        }
                        this.Filter = filter;
                        flag3 = false;
                    }
                    VirtualItemComparer sort = customizeFolder.Sort;
                    if (sort != null)
                    {
                        if (this.RememberSort == null)
                        {
                            this.RememberSort = this.Sort;
                        }
                        this.Sort = sort;
                        flag4 = false;
                    }
                    int? listColumnCount = customizeFolder.ListColumnCount;
                    if (listColumnCount.HasValue)
                    {
                        if (!this.RememberListViewCount.HasValue)
                        {
                            this.RememberListViewCount = new int?(this.ListViewColumnCount);
                        }
                        this.ListViewColumnCount = listColumnCount.Value;
                        flag5 = false;
                    }
                    Size thumbnailSize = customizeFolder.ThumbnailSize;
                    if (!thumbnailSize.IsEmpty)
                    {
                        if (this.RememberThumbnailSize.IsEmpty)
                        {
                            this.RememberThumbnailSize = this.ThumbnailSize;
                        }
                        this.ThumbnailSize = thumbnailSize;
                        flag6 = false;
                    }
                    Size thumbnailSpacing = customizeFolder.ThumbnailSpacing;
                    if (!thumbnailSpacing.IsEmpty)
                    {
                        if (this.RememberThumbnailSpacing.IsEmpty)
                        {
                            this.RememberThumbnailSpacing = this.ThumbnailSpacing;
                        }
                        this.ThumbnailSpacing = thumbnailSpacing;
                        flag7 = false;
                    }
                    PanelView? view = customizeFolder.View;
                    if (view.HasValue)
                    {
                        if (!this.RememberView.HasValue)
                        {
                            this.RememberView = new PanelView?(this.View);
                        }
                        this.View = view.Value;
                        flag8 = false;
                    }
                    System.Drawing.Color foreColor = customizeFolder.ForeColor;
                    if (!foreColor.IsEmpty)
                    {
                        if (this.RememberListForeColor.IsEmpty)
                        {
                            this.RememberListForeColor = this.ListForeColor;
                        }
                        this.ListForeColor = foreColor;
                        flag10 = false;
                    }
                    foreColor = customizeFolder.BackColor;
                    if (!foreColor.IsEmpty)
                    {
                        if (this.RememberListBackColor.IsEmpty)
                        {
                            this.RememberListBackColor = this.ListBackColor;
                        }
                        this.ListBackColor = foreColor;
                        flag9 = false;
                    }
                }
                catch (Exception exception)
                {
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                }
                finally
                {
                    this.SetPanelState(PanelState.ProcessingCustomizeFolder, false);
                }
            }
            if (flag && this.RememberAutoSizeColumns.HasValue)
            {
                this.AutoSizeColumns = this.RememberAutoSizeColumns.Value;
                this.RememberAutoSizeColumns = null;
            }
            if (flag2 && (this.RememberColumnInfoMap != null))
            {
                this.FColumnInfoMap = this.RememberColumnInfoMap;
                this.RememberColumnInfoMap = null;
                this.RecreateColumns();
            }
            if (flag3 && this.CheckPanelState(PanelState.HasRememberFilter))
            {
                this.Filter = this.RememberFilter;
                this.RememberFilter = null;
                this.SetPanelState(PanelState.HasRememberFilter, false);
            }
            if (flag4 && (this.RememberSort != null))
            {
                this.Sort = this.RememberSort;
                this.RememberSort = null;
            }
            if (flag5 && this.RememberListViewCount.HasValue)
            {
                this.ListViewColumnCount = this.RememberListViewCount.Value;
                this.RememberListViewCount = null;
            }
            if (!(!flag6 || this.RememberThumbnailSize.IsEmpty))
            {
                this.ThumbnailSize = this.RememberThumbnailSize;
                this.RememberThumbnailSize = Size.Empty;
            }
            if (!(!flag7 || this.RememberThumbnailSpacing.IsEmpty))
            {
                this.ThumbnailSpacing = this.RememberThumbnailSpacing;
                this.RememberThumbnailSpacing = Size.Empty;
            }
            if (flag8 && this.RememberView.HasValue)
            {
                this.View = this.RememberView.Value;
                this.RememberView = null;
            }
            if (!(!flag9 || this.RememberListBackColor.IsEmpty))
            {
                this.ListBackColor = this.RememberListBackColor;
                this.RememberListBackColor = System.Drawing.Color.Empty;
            }
            if (!(!flag10 || this.RememberListForeColor.IsEmpty))
            {
                this.ListForeColor = this.RememberListForeColor;
                this.RememberListForeColor = System.Drawing.Color.Empty;
            }
        }

        private void ProcessDesktopIni()
        {
            if (this.CheckPanelState(PanelState.ProcessDesktopIniNeeded))
            {
                this.SetPanelState(PanelState.ProcessDesktopIniNeeded, false);
                DesktopIni inheritedDesktopIni = VirtualItem.GetInheritedDesktopIni(this.CurrentFolder);
                if (inheritedDesktopIni != null)
                {
                    DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(inheritedDesktopIni.FileName);
                    if (string.Equals(inheritedDesktopIni.FileName, this.RememberDesktopIniPath, StringComparison.OrdinalIgnoreCase) && (lastWriteTimeUtc == this.RememberDesktopIniTime))
                    {
                        return;
                    }
                    this.RememberDesktopIniPath = null;
                    try
                    {
                        inheritedDesktopIni.Read();
                        this.RememberDesktopIniPath = inheritedDesktopIni.FileName;
                        this.RememberDesktopIniTime = lastWriteTimeUtc;
                    }
                    catch (Exception exception)
                    {
                        inheritedDesktopIni = null;
                        Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                    }
                }
                else
                {
                    this.RememberDesktopIniPath = null;
                }
                this.ProcessCustomizeFolder(inheritedDesktopIni);
            }
        }

        public void PushSelection(bool clearSelection)
        {
            this.StoredSelection = (this.FSelection.Count > 0) ? new List<IVirtualItem>(this.FSelection) : null;
            if (clearSelection)
            {
                this.Selection = null;
            }
        }

        public void QuickChangeFolder()
        {
            if (this.tsPath.View != BreadcrumbView.SimpleText)
            {
                this.tsPath.View = BreadcrumbView.EnterPath;
                this.tsPath.Focus();
                this.tsPath.PathTextBox.Focus();
            }
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
                this.FDragOverItem(this, arg);
                e.Effect = arg.Effect;
            }
        }

        private void RecreateColumns()
        {
            if (this.UpdateCount > 0)
            {
                this.UpdateAction |= ListViewUpdateAction.RecreateColumns;
            }
            else
            {
                VirtualItemComparer fSort = this.FSort as VirtualItemComparer;
                this.BeginListViewUpdate(false, true);
                try
                {
                    this.listView.Columns.Clear();
                    this.listView.SortColumn = -1;
                    foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) VirtualProperty.Visible)
                    {
                        ListViewColumnInfo info = null;
                        this.ColumnInfoMap.TryGetValue(property.PropertyId, out info);
                        if ((property.PropertyId == 0) || ((info != null) && info.Visible))
                        {
                            ColumnHeader header = new ColumnHeader {
                                Text = property.LocalizedName
                            };
                            if (info == null)
                            {
                                header.Width = VirtualFilePanelSettings.DefaultColumnWidth(property.PropertyId, this.listView.Font);
                                info = new ListViewColumnInfo(property.PropertyId, header.TextAlign, header.Width, true);
                                this.ColumnInfoMap.Add(property.PropertyId, info);
                            }
                            else
                            {
                                header.TextAlign = info.TextAlign;
                                header.Width = info.Width;
                            }
                            header.Tag = info;
                            this.listView.Columns.Add(header);
                            if ((fSort != null) && (property.PropertyId == fSort.ComparePropertyId))
                            {
                                this.listView.SetSortColumn(header.Index, fSort.SortDirection);
                            }
                        }
                    }
                    try
                    {
                        foreach (ColumnHeader header2 in this.listView.Columns)
                        {
                            int displayIndex = ((ListViewColumnInfo) header2.Tag).DisplayIndex;
                            if (displayIndex >= 0)
                            {
                                header2.DisplayIndex = displayIndex;
                            }
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                    }
                }
                finally
                {
                    this.EndListViewUpdate();
                }
                this.ClearListViewCache();
                this.UpdateColumnInfos();
            }
        }

        private void RecreateSelection()
        {
            if (this.FSelection.Count > 0)
            {
                HashSet<IVirtualItem> set = new HashSet<IVirtualItem>(this.FSelection.Count);
                foreach (IVirtualItem item in this.Items)
                {
                    if (this.FSelection.Contains(item))
                    {
                        set.Add(item);
                    }
                }
                bool flag = set.Count != this.FSelection.Count;
                this.FSelection.Clear();
                this.FSelection = set;
                if (flag)
                {
                    this.OnSelectionChanged(EventArgs.Empty);
                }
                else
                {
                    this.UpdateSelectionInfo();
                }
            }
        }

        private void RefreshContent()
        {
            if ((this.FItems != null) && (this.CurrentFolder != null))
            {
                int count = this.FItems.Count;
                try
                {
                    IEnumerable<IVirtualItem> enumerable;
                    IVirtualCachedFolder currentFolder = this.CurrentFolder as IVirtualCachedFolder;
                    if (currentFolder != null)
                    {
                        enumerable = new List<IVirtualItem>(currentFolder.GetCachedContent());
                    }
                    else
                    {
                        enumerable = new List<IVirtualItem>(this.CurrentFolder.GetContent());
                    }
                    IVirtualItem focusedItem = this.FocusedItem;
                    this.PopulateListViewItems(enumerable, ListViewSort.Full);
                    this.FocusedItem = focusedItem;
                    if ((count != this.FItems.Count) && this.listView.Visible)
                    {
                        this.InvalidateListView(false);
                    }
                }
                catch (SystemException exception)
                {
                    if (!VirtualItem.IsFolderInaccessibleException(exception))
                    {
                        throw;
                    }
                    this.CurrentFolderDeleted(null, new VirtualItemChangedEventArgs(WatcherChangeTypes.Deleted, this.CurrentFolder));
                }
            }
        }

        public void RefreshCurrentFolder()
        {
            if (this.CurrentFolder != null)
            {
                IVirtualCachedFolder currentFolder = this.CurrentFolder as IVirtualCachedFolder;
                if (currentFolder != null)
                {
                    currentFolder.ClearContentCache();
                }
                this.CurrentFolder = this.CurrentFolder;
                if (this.treeView.Visible)
                {
                    this.treeView.BeginUpdate();
                    try
                    {
                        this.treeView.Clear();
                        this.treeView.ShowVirtualFolder(this.FCurrentFolder, false);
                    }
                    finally
                    {
                        this.treeView.EndUpdate();
                    }
                }
            }
        }

        private bool RemoveByFilter(IVirtualItem item, IVirtualItemFilter showFilter, IVirtualItemFilter hideFilter)
        {
            return (!((showFilter == null) || showFilter.IsMatch(item)) || ((hideFilter != null) && hideFilter.IsMatch(item)));
        }

        public void ResetComponentSettings()
        {
            this.FolderBarVisible = false;
            this.SplitterPercent = 500;
            this.Columns = VirtualFilePanelSettings.DefaultColumns;
        }

        public void ResetVisualCache()
        {
            this.tsPath.UpdatePath();
            this.ShowFocusedItemInfo(this.FocusedItem);
            this.ClearListViewCache();
            this.ClearThumbnails();
            if (base.Visible)
            {
                if (this.treeView.Visible)
                {
                    this.treeView.ResetVisualCache();
                }
                this.listView.Refresh();
            }
        }

        private bool RestoreFromLazyFolder()
        {
            if (this.FLazyFolder != null)
            {
                string serializableItemPath = this.FLazyFolder.SerializableItemPath;
                System.Diagnostics.Debug.WriteLine(string.Format("Start, '{0}'", serializableItemPath), "RestoreFromLazyFolder");
                try
                {
                    if (!this.FLazyFolder.IsEmpty)
                    {
                        IVirtualFolder folder = this.FLazyFolder.Value;
                        ICustomizeFolder customize = null;
                        VirtualResolveContainer<IVirtualFolder> fLazyFolder = this.FLazyFolder as VirtualResolveContainer<IVirtualFolder>;
                        if (fLazyFolder != null)
                        {
                            IVirtualLink link = fLazyFolder.Link;
                            if (link != null)
                            {
                                customize = link[0x17] as ICustomizeFolder;
                            }
                        }
                        bool updateHistory = (folder != null) && (!this.History.HasCurrent || !folder.Equals(this.History.Current));
                        return this.SetCurrentFolder(folder, updateHistory, customize);
                    }
                }
                catch (TargetInvocationException exception)
                {
                    if (!(exception.InnerException is AbortException))
                    {
                        if (VirtualItem.IsWarningIOException(exception.InnerException))
                        {
                            this.ShowNavigationError(exception.InnerException, serializableItemPath);
                        }
                        else
                        {
                            Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                        }
                    }
                }
                catch (TimeoutException exception2)
                {
                    ApplicationException e = new ApplicationException(string.Format(Resources.sErrorNavigateToFolder, serializableItemPath, Resources.sTimeOutElapsed), exception2);
                    this.ShowNavigationError(e);
                }
                catch (Exception exception4)
                {
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception4);
                }
                finally
                {
                    this.FLazyFolder = null;
                    System.Diagnostics.Debug.WriteLine("Finish", "RestoreFromLazyFolder");
                }
            }
            return false;
        }

        public void SaveComponentSettings()
        {
            this.FSettings.QuickFindOptions = this.FindOptions;
            this.FSettings.Layout = this.GetPanelLayout(true, PanelLayoutEntry.ListColumnCount | PanelLayoutEntry.ThumbnailSize | PanelLayoutEntry.Columns | PanelLayoutEntry.FolderBarOrientation);
            this.FSettings.Content = this.GetPanelContent(true);
            SettingsManager.RegisterSettings(this.FSettings);
        }

        public void SelectAll()
        {
            this.SelectItems(null, false);
        }

        public void SelectDrive()
        {
            this.tsPath.SelectDrive();
        }

        private void SelectIndices()
        {
            if (this.listView.SelectedIndices.Count != 0)
            {
                int startIndex = -1;
                int endIndex = 0;
                foreach (int num3 in this.listView.SelectedIndices)
                {
                    IVirtualItem item = this.FItems[num3];
                    if (!item.Equals(this.ParentFolder) && !this.FSelection.Contains(item))
                    {
                        if ((startIndex < 0) || (num3 < startIndex))
                        {
                            startIndex = num3;
                        }
                        if (num3 > endIndex)
                        {
                            endIndex = num3;
                        }
                        this.FSelection.Add(item);
                    }
                }
                this.listView.SelectedIndices.Clear();
                if (startIndex >= 0)
                {
                    this.OnSelectionChanged(EventArgs.Empty);
                    this.listView.RedrawItems(startIndex, endIndex, true);
                }
            }
        }

        public void SelectItems(IVirtualItemFilter filter, bool clearSelection)
        {
            HashSet<IVirtualItem> fSelection;
            int num3;
            IVirtualItem tag;
            bool flag;
            int startIndex = -1;
            int endIndex = 0;
            if (clearSelection)
            {
                fSelection = new HashSet<IVirtualItem>();
            }
            else
            {
                fSelection = this.FSelection;
            }
            if (this.listView.VirtualMode)
            {
                for (num3 = 0; num3 < this.FItems.Count; num3++)
                {
                    bool flag2;
                    tag = this.FItems[num3];
                    flag = this.FSelection.Contains(tag);
                    if (!(tag.Equals(this.ParentFolder) || ((filter != null) && !filter.IsMatch(tag))))
                    {
                        if (!(flag && !clearSelection))
                        {
                            fSelection.Add(tag);
                        }
                        flag2 = !flag;
                    }
                    else
                    {
                        flag2 = flag && clearSelection;
                    }
                    if (flag2)
                    {
                        if (startIndex < 0)
                        {
                            startIndex = num3;
                        }
                        endIndex = num3;
                    }
                }
                if (clearSelection)
                {
                    this.FSelection = fSelection;
                }
                if (startIndex >= 0)
                {
                    this.OnSelectionChanged(EventArgs.Empty);
                    this.listView.RedrawItems(startIndex, endIndex, true);
                }
            }
            else
            {
                this.BeginListViewUpdate(true, false);
                try
                {
                    for (num3 = 0; num3 < this.listView.Items.Count; num3++)
                    {
                        ListViewItem item2 = this.listView.Items[num3];
                        tag = (IVirtualItem) this.listView.Items[num3].Tag;
                        flag = this.FSelection.Contains(tag);
                        if (!(tag.Equals(this.ParentFolder) || ((filter != null) && !filter.IsMatch(tag))))
                        {
                            if (!flag)
                            {
                                fSelection.Add(tag);
                                item2.ForeColor = this.FSelectedForeColor;
                            }
                        }
                        else if (clearSelection)
                        {
                            item2.ForeColor = VirtualItemHelper.GetForeColor(tag, this.listView.ForeColor);
                        }
                    }
                }
                finally
                {
                    this.EndListViewUpdate();
                }
            }
        }

        public bool SetCurrentFolder(History<IVirtualFolder> history)
        {
            bool flag = this.SetCurrentFolder(history.Current, false);
            this.FHistory = history;
            return flag;
        }

        public bool SetCurrentFolder(IVirtualFolder value, bool updateHistory)
        {
            return this.SetCurrentFolder(value, updateHistory, null);
        }

        public bool SetCurrentFolder(string folderName, VirtualItemType type)
        {
            if (base.Visible && base.IsHandleCreated)
            {
                IVirtualFolder folder = null;
                if (!string.IsNullOrEmpty(folderName))
                {
                    try
                    {
                        IVirtualItem item = VirtualItem.FromFullName(folderName, type, this.CurrentFolder);
                        IChangeVirtualFile archiveFile = item as IChangeVirtualFile;
                        if (archiveFile != null)
                        {
                            folder = VirtualItem.OpenArchive(archiveFile, this.CurrentFolder);
                        }
                        else
                        {
                            folder = item as IVirtualFolder;
                        }
                        if ((item != null) && (folder == null))
                        {
                            throw new DirectoryNotFoundException();
                        }
                    }
                    catch (SystemException exception)
                    {
                        ApplicationException e = new ApplicationException(string.Format(Resources.sErrorNavigateToFolder, folderName, exception.Message), exception);
                        if (!(exception is ArgumentException) && !VirtualItem.IsWarningIOException(exception))
                        {
                            throw e;
                        }
                        this.ShowNavigationError(e);
                        return false;
                    }
                }
                bool updateHistory = (folder != null) && (!this.History.HasCurrent || !folder.Equals(this.History.Current));
                return this.SetCurrentFolder(folder, updateHistory);
            }
            this.SetCurrentFolder(null, false);
            this.FLazyFolder = new VirtualResolveContainer<IVirtualFolder>();
            this.FLazyFolder.SerializableItemPath = folderName;
            return true;
        }

        public bool SetCurrentFolder(IVirtualFolder value, bool updateHistory, ICustomizeFolder customize)
        {
            string fullName = null;
            try
            {
                IEnumerable<IVirtualItem> content;
                bool flag = false;
                IVirtualItem focusedItem = null;
                IAsyncResult result = null;
                bool flag2 = (value == null) || !value.Equals(this.FCurrentFolder);
                bool flag3 = false;
                if (((this.FCurrentFolder != null) && flag2) && this.IsFolderLocked)
                {
                    throw new WarningException(Resources.sCannotChangeLockedFolder);
                }
                if (!((value == null) || flag2))
                {
                    value = this.FCurrentFolder;
                }
                this.SetPanelState(PanelState.FolderChangePending, false);
                if (value == null)
                {
                    goto Label_02B3;
                }
                ISetOwnerWindow window = value as ISetOwnerWindow;
                if (window != null)
                {
                    window.Owner = this;
                }
                fullName = value.FullName;
                IAsyncVirtualFolder folder = value as IAsyncVirtualFolder;
                if (folder != null)
                {
                    result = folder.BeginGetContent();
                    if (!result.IsCompleted)
                    {
                        Application.DoEvents();
                        this.SetPanelState(PanelState.FolderChangePending, !result.AsyncWaitHandle.WaitOne(250, false));
                    }
                    if (result.IsCompleted)
                    {
                        content = folder.EndGetContent(result);
                    }
                    else
                    {
                        content = folder.GetCachedContent();
                        flag3 = true;
                    }
                    goto Label_02BA;
                }
                if (!base.Enabled)
                {
                    content = null;
                    goto Label_02BA;
                }
                bool flag4 = false;
                content = value.GetContent();
                try
                {
                    foreach (IVirtualItem item2 in content)
                    {
                        goto Label_0268;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    IElevatable elevatable = value as IElevatable;
                    if ((elevatable == null) || !elevatable.CanElevate)
                    {
                        throw;
                    }
                    bool flag5 = true;
                    IElevatable fCurrentFolder = this.FCurrentFolder as IElevatable;
                    if (!((fCurrentFolder == null) || fCurrentFolder.CanElevate))
                    {
                        IElevatableFrom from = elevatable as IElevatableFrom;
                        flag5 = (from == null) || !from.ElevateFrom(fCurrentFolder);
                    }
                    if (flag5)
                    {
                        if (MessageDialog.Show(this, string.Format(Resources.sAskElevateFolderPermissions, value.FullName), Resources.sWarning, new MessageDialogResult[] { MessageDialogResult.Shield, MessageDialogResult.Cancel }, MessageBoxIcon.Exclamation, MessageDialogResult.Shield) != MessageDialogResult.Shield)
                        {
                            return false;
                        }
                        if (!elevatable.Elevate(new ElevatedProcess()))
                        {
                            throw;
                        }
                    }
                    flag4 = true;
                }
            Label_0268:
                if (flag4)
                {
                    foreach (IVirtualItem item2 in content)
                    {
                        break;
                    }
                }
                goto Label_02BA;
            Label_02B3:
                content = new List<IVirtualItem>();
            Label_02BA:
                if (flag2)
                {
                    if (this.FCurrentFolder is IVirtualCachedFolder)
                    {
                        ((IVirtualCachedFolder) this.FCurrentFolder).OnChanged -= new EventHandler<VirtualItemChangedEventArgs>(this.FolderContentChanged);
                    }
                    if (this.FCurrentFolder is IAsyncVirtualFolder)
                    {
                        ((IAsyncVirtualFolder) this.FCurrentFolder).Completed -= new AsyncCompletedEventHandler(this.FolderCompleted);
                        ((IAsyncVirtualFolder) this.FCurrentFolder).ProgressChanged -= new ProgressChangedEventHandler(this.FolderProgress);
                    }
                    if (this.FCurrentFolder != null)
                    {
                        this.FCurrentFolder.Dispose();
                    }
                    this.SetPanelState(PanelState.ProgressUpdatePending | PanelState.DoFolderChangedTick, false);
                    this.FolderChangeRequested = 0;
                    this.HideItemToolTip();
                    flag = this.FSelection.Count > 0;
                    this.FSelection.Clear();
                    this.StoredSelection = null;
                    focusedItem = this.FCurrentFolder;
                    this.FCurrentFolder = value;
                    IVirtualCachedFolder folder2 = this.FCurrentFolder as IVirtualCachedFolder;
                    if (folder2 != null)
                    {
                        folder2.OnChanged += new EventHandler<VirtualItemChangedEventArgs>(this.FolderContentChanged);
                    }
                    folder = this.FCurrentFolder as IAsyncVirtualFolder;
                    if (folder != null)
                    {
                        folder.Completed += new AsyncCompletedEventHandler(this.FolderCompleted);
                        folder.ProgressChanged += new ProgressChangedEventHandler(this.FolderProgress);
                    }
                    if (updateHistory)
                    {
                        if (this.FCurrentFolder == null)
                        {
                            this.History.Clear();
                        }
                        else
                        {
                            IVirtualFolder folder3 = this.FCurrentFolder;
                            foreach (IVirtualFolder folder4 in this.History)
                            {
                                if (folder3.Equals(folder4))
                                {
                                    folder3 = folder4;
                                    break;
                                }
                            }
                            this.History.Current = folder3;
                        }
                    }
                    if (this.FCurrentFolder != null)
                    {
                        IVirtualFolder folderRoot = VirtualItemHelper.GetFolderRoot(this.FCurrentFolder);
                        if (folderRoot != null)
                        {
                            VirtualItem.DefaultDrivePath[folderRoot.FullName] = this.FCurrentFolder.FullName;
                        }
                    }
                    this.SetUpdateTimerInterval(this.FCurrentFolder, this.CheckPanelState(PanelState.LastParentFormActive));
                    if (this.treeView.Visible)
                    {
                        this.treeView.ShowVirtualFolder(this.FCurrentFolder, false);
                    }
                    this.OnCurrentFolderChanged(new VirtualFolderChangedEventArgs((IVirtualFolder) focusedItem, this.FCurrentFolder));
                    if (flag)
                    {
                        this.OnSelectionChanged(EventArgs.Empty);
                    }
                    this.UpdatePathCurrentFolder((content != null) ? this.FCurrentFolder : null);
                }
                else
                {
                    focusedItem = this.FocusedItem;
                }
                if ((result != null) && result.IsCompleted)
                {
                    flag3 = false;
                    this.FolderChangeRequested = WatcherChangeTypes.All;
                    this.SetPanelState(PanelState.DoFolderChangedTick, true);
                }
                this.tsPath.ShowProgress = flag3;
                this.BeginListViewUpdate(true, false);
                try
                {
                    if (this.IsThumbnailView)
                    {
                        this.ClearThumbnails();
                    }
                    if (customize != null)
                    {
                        this.RememberDesktopIniPath = null;
                        this.ProcessCustomizeFolder(customize);
                    }
                    else
                    {
                        this.SetPanelState(PanelState.ProcessDesktopIniNeeded, true);
                        if (base.Enabled)
                        {
                            this.ProcessDesktopIni();
                        }
                    }
                    if (content != null)
                    {
                        this.PopulateListViewItems(content, ListViewSort.Full);
                    }
                    else
                    {
                        this.FItems = null;
                        this.listView.VirtualListSize = 0;
                    }
                }
                finally
                {
                    this.EndListViewUpdate();
                }
                ListViewItem item3 = this.SetFocusedItem(focusedItem, this.FCurrentFolder is IAsyncVirtualFolder, false);
                if (item3 != null)
                {
                    using (new LockWindowRedraw(this, true))
                    {
                        item3.EnsureVisible();
                    }
                }
                return true;
            }
            catch (AbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                Exception exception2;
                if (fullName != null)
                {
                    exception2 = new ApplicationException(string.Format(Resources.sErrorNavigateToFolder, fullName, exception.Message), exception);
                }
                else
                {
                    exception2 = exception;
                }
                if (!VirtualItem.IsWarningIOException(exception))
                {
                    throw exception2;
                }
                this.ShowNavigationError(exception2);
                return false;
            }
        }

        public static void SetCuttedItems(IEnumerable<IVirtualItem> items)
        {
            if (items != null)
            {
                FCuttedItems = new HashSet<IVirtualItem>(items);
                ChangeVector.Increment(ChangeVector.CuttedItems);
            }
            else if (FCuttedItems != null)
            {
                FCuttedItems = null;
                ChangeVector.Increment(ChangeVector.CuttedItems);
            }
        }

        private void SetDefaultPathAsCurrent()
        {
            this.SetCurrentFolder(VirtualFilePanelSettings.Default.DefaultPath, VirtualItemType.Folder);
        }

        private void SetDrive(IVirtualFolder drive, bool useDefaultDrivePath)
        {
            string str;
            if (useDefaultDrivePath && VirtualItem.DefaultDrivePath.TryGetValue(drive.FullName, out str))
            {
                if (!this.SetCurrentFolder(str, VirtualItemType.Folder))
                {
                    IPersistVirtualItem item = drive as IPersistVirtualItem;
                    if ((item == null) || item.Exists)
                    {
                        this.CurrentFolder = drive;
                    }
                }
            }
            else
            {
                this.CurrentFolder = drive;
            }
        }

        public ListViewItem SetFocusedItem(IVirtualItem item, bool delayedFocus, bool ensureVisible)
        {
            ListViewItem item4;
            try
            {
                if (item != null)
                {
                    ListViewItem item2;
                    string name = item.Name;
                    int num = -1;
                    if (this.FItems != null)
                    {
                        for (int i = 0; i < this.FItems.Count; i++)
                        {
                            if (item.Equals(this.FItems[i]))
                            {
                                item2 = this.listView.Items[i];
                                this.FocusItem(item2, ensureVisible);
                                this.DelayedFocusedItem = null;
                                return item2;
                            }
                            if ((num < 0) && string.Equals(name, this.FItems[i].Name, StringComparison.OrdinalIgnoreCase))
                            {
                                num = i;
                            }
                        }
                    }
                    if (num >= 0)
                    {
                        item2 = this.listView.Items[num];
                        this.FocusItem(item2, ensureVisible);
                        this.DelayedFocusedItem = null;
                        return item2;
                    }
                    if (delayedFocus)
                    {
                        this.DelayedFocusedItem = item;
                    }
                }
                ListViewItem focusedItem = this.listView.FocusedItem;
                if (focusedItem != null)
                {
                    focusedItem.Focused = false;
                }
                item4 = null;
            }
            finally
            {
                this.HideItemToolTip();
            }
            return item4;
        }

        private void SetFolderBarVisible(bool visible, bool selectTreeView)
        {
            using (new LockWindowRedraw(this, true))
            {
                this.splitContainer.Panel1Collapsed = !visible;
                if (this.treeView.Visible)
                {
                    if (selectTreeView)
                    {
                        this.treeView.TabStop = true;
                    }
                    this.treeView.ShowVirtualFolder(this.FCurrentFolder, false);
                }
                else
                {
                    this.treeView.Clear();
                }
            }
            if (selectTreeView)
            {
                this.splitContainer.SelectNextControl(null, true, true, true, false);
                this.treeView.TabStop = false;
            }
        }

        private void SetPanelState(PanelState state, bool value)
        {
            if (value)
            {
                this.FPanelState |= state;
            }
            else
            {
                this.FPanelState &= ~state;
            }
        }

        private void SettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.BeginListViewUpdate(false, false);
            try
            {
                switch (e.PropertyName)
                {
                    case "Highlighters":
                        this.ClearListViewCache();
                        this.InvalidateListView(false);
                        this.ShowFocusedItemInfo(this.FocusedItem);
                        this.tsPath.UpdatePath();
                        return;

                    case "FileNameCasing":
                    case "FolderNameCasing":
                    case "FolderNameTemplate":
                        this.ClearListViewCache();
                        this.InvalidateListView(false);
                        return;

                    case "DateTimeFormat":
                    case "SizeFormat":
                        if (this.listView.View == System.Windows.Forms.View.Details)
                        {
                            this.UpdateListView();
                            this.InvalidateListView(false);
                        }
                        this.ShowFocusedItemInfo(this.FocusedItem);
                        return;

                    case "IconOptions":
                        if (!(ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Icon) || !this.IsThumbnailView))
                        {
                            this.ClearThumbnails();
                        }
                        ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Icon);
                        return;

                    case "HideNameExtension":
                        if (this.listView.View == System.Windows.Forms.View.Details)
                        {
                            this.InvalidateListView(false);
                        }
                        return;

                    case "HiddenProperties":
                        if (this.listView.View == System.Windows.Forms.View.Details)
                        {
                            this.RecreateColumns();
                        }
                        return;

                    case "HideSysHidItems":
                    case "UseHiddenItemsList":
                    case "HiddenItemsList":
                        this.FolderChangeRequested = WatcherChangeTypes.All;
                        this.SetPanelState(PanelState.DoFolderChangedTick, true);
                        return;

                    case "MinListColumnWidth":
                        if (this.listView.View == System.Windows.Forms.View.List)
                        {
                            this.UpdateListView();
                        }
                        return;

                    case "ShowUpFolderItem":
                        this.RefreshContent();
                        return;

                    case "VisualStyleState":
                    case "ExplorerTheme":
                        base.BeginInvoke(new MethodInvoker(this.UpdateImageLists));
                        return;

                    case "ListColorMap":
                    case "Theme":
                        this.UpdateColorsFromTheme();
                        return;
                }
            }
            finally
            {
                this.EndListViewUpdate();
            }
        }

        private void SetUpdateTimerInterval(IVirtualFolder folder, bool formActive)
        {
            int num;
            if (folder == null)
            {
                num = 0x2710;
            }
            else if (!formActive)
            {
                num = 0x7d0;
            }
            else
            {
                IGetVirtualVolume fCurrentFolder = this.FCurrentFolder as IGetVirtualVolume;
                if (fCurrentFolder == null)
                {
                    num = 500;
                }
                else
                {
                    switch (fCurrentFolder.VolumeType)
                    {
                        case DriveType.Fixed:
                        case DriveType.Ram:
                            num = 500;
                            goto Label_0063;
                    }
                    num = 0x3e8;
                }
            }
        Label_0063:
            this.tmrUpdateItems.Interval = num;
        }

        private void ShowFocusedItemInfo(IVirtualItem item)
        {
            if (item == null)
            {
                this.tslItemName.Visible = false;
                this.tslItemSize.Visible = false;
                this.tslItemDate.Visible = false;
            }
            else
            {
                this.tslItemName.Text = item.Name.Replace("&", "&&");
                this.tslItemName.Tag = item;
                this.tslItemName.MergeAction = MergeAction.Remove;
                VirtualItemToolStripEvents.UpdateItemImage(this.tslItemName, item);
                System.Drawing.Color foreColor = VirtualItemHelper.GetForeColor(item, System.Drawing.Color.Empty);
                if (!foreColor.IsEmpty)
                {
                    this.tslItemName.ForeColor = foreColor;
                }
                else
                {
                    this.tslItemName.ResetForeColor();
                }
                this.tslItemName.Visible = true;
                object obj2 = null;
                switch (item.GetPropertyAvailability(3))
                {
                    case PropertyAvailability.Normal:
                    case PropertyAvailability.Slow:
                        this.tslItemSize.Visible = (obj2 = item[3]) != null;
                        if (this.tslItemSize.Visible)
                        {
                            this.tslItemSize.Text = VirtualProperty.ConvertToString(3, obj2);
                        }
                        break;

                    default:
                        this.tslItemSize.Visible = false;
                        break;
                }
                this.tslItemDate.Visible = item.IsPropertyAvailable(8) && ((obj2 = item[8]) != null);
                if (this.tslItemDate.Visible)
                {
                    this.tslItemDate.Text = VirtualProperty.ConvertToString(8, obj2);
                }
            }
        }

        public void ShowItem(IVirtualItem item)
        {
            if (item != null)
            {
                IVirtualFolder parent = item.Parent;
                if (parent == null)
                {
                    return;
                }
                if (!parent.Equals(this.CurrentFolder))
                {
                    this.CurrentFolder = parent;
                }
            }
            this.FocusedItem = item;
        }

        private void ShowNavigationError(Exception e)
        {
            this.ShowNavigationError(e, null);
        }

        private void ShowNavigationError(Exception e, string folderName)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                e = new ApplicationException(string.Format(Resources.sErrorNavigateToFolder, folderName, e.Message), e);
            }
            Nomad.Trace.Error.TraceException(TraceEventType.Warning, e);
            Nomad.Trace.Error.Flush();
            bool checkBoxChecked = !ConfirmationSettings.Default.NavigateError;
            if (!checkBoxChecked)
            {
                MessageDialog.Show(this, e.Message, Resources.sWarning, Resources.sDoNotShowAgain, ref checkBoxChecked, MessageDialog.ButtonsOk, MessageBoxIcon.Exclamation);
                if (checkBoxChecked)
                {
                    ConfirmationSettings.Default.NavigateError = false;
                }
            }
        }

        private void splitContainer_DoubleClick(object sender, EventArgs e)
        {
            this.SplitterPercent = 500;
        }

        private void splitContainer_MouseCaptureChanged(object sender, EventArgs e)
        {
            if ((!this.splitContainer.Capture & (this.RememberSplitterActiveControl != null)) && (this.splitContainer.ActiveControl == null))
            {
                this.splitContainer.ActiveControl = this.RememberSplitterActiveControl;
            }
            this.RememberSplitterActiveControl = null;
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.splitContainer.Capture)
            {
                this.RememberSplitterActiveControl = this.splitContainer.ActiveControl;
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.ItemToolTip.Hide(this.splitContainer);
        }

        private void splitContainer_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            double num;
            this.NewSplitterPercent = -1;
            if (this.splitContainer.Orientation == Orientation.Vertical)
            {
                num = (e.SplitX * 100f) / ((float) (this.splitContainer.Panel1.Width + this.splitContainer.Panel2.Width));
            }
            else
            {
                num = (e.SplitY * 100f) / ((float) (this.splitContainer.Panel1.Height + this.listView.Height));
            }
            this.ItemToolTip.Show(string.Format("{0:0.0}%", num), this.splitContainer, e.MouseCursorX, e.MouseCursorY + this.splitContainer.Cursor.GetPrefferedHeight());
        }

        private void tmrExpandNode_Tick(object sender, EventArgs e)
        {
            this.tmrExpandNode.Stop();
            TreeViewHitTestInfo info = this.treeView.HitTest(this.treeView.PointToClient(Cursor.Position));
            if ((info.Node != null) && (info.Node == this.tmrExpandNode.Tag))
            {
                if (info.Node.IsExpanded)
                {
                    if (info.Location == TreeViewHitTestLocations.PlusMinus)
                    {
                        info.Node.Collapse();
                    }
                }
                else
                {
                    info.Node.Expand();
                }
            }
            this.tmrExpandNode.Tag = null;
        }

        private void tmrToolTip_Tick(object sender, EventArgs e)
        {
            this.tmrToolTip.Stop();
            ListViewItem tag = this.tmrToolTip.Tag as ListViewItem;
            if (((tag != null) && !this.listView.IsEditing) && !this.listView.IsBoxSelectionActive)
            {
                ListViewItem focusedItem = this.listView.FocusedItem;
                if ((focusedItem != null) && (tag.Index == focusedItem.Index))
                {
                    IVirtualItem item3 = (IVirtualItem) focusedItem.Tag;
                    StringBuilder builder = new StringBuilder();
                    Rectangle itemRect = this.listView.GetItemRect(focusedItem.Index, ItemBoundsPortion.Label);
                    Size size = TextRenderer.MeasureText(item3.Name, this.listView.Font, itemRect.Size, TextFormatFlags.NoPadding);
                    if (this.listView.View == System.Windows.Forms.View.LargeIcon)
                    {
                        Rectangle rectangle2 = this.listView.GetItemRect(focusedItem.Index, ItemBoundsPortion.Entire);
                        if ((itemRect.Width == rectangle2.Width) && (itemRect.Height < (size.Height * 2)))
                        {
                            builder.Append(item3.Name);
                        }
                    }
                    else if (size.Width > itemRect.Width)
                    {
                        builder.Append(item3.Name);
                    }
                    IVirtualItemUI item = item3 as IVirtualItemUI;
                    if (item != null)
                    {
                        string toolTip = item.ToolTip;
                        if (!string.IsNullOrEmpty(toolTip))
                        {
                            if (builder.Length > 0)
                            {
                                builder.AppendLine();
                            }
                            builder.Append(toolTip);
                        }
                    }
                    if (builder.Length != 0)
                    {
                        int left;
                        Rectangle bounds;
                        switch (this.listView.View)
                        {
                            case System.Windows.Forms.View.Details:
                            case System.Windows.Forms.View.SmallIcon:
                            case System.Windows.Forms.View.List:
                                bounds = focusedItem.GetBounds(ItemBoundsPortion.Label);
                                left = bounds.Left;
                                break;

                            default:
                                bounds = focusedItem.GetBounds(ItemBoundsPortion.Entire);
                                left = bounds.Right;
                                break;
                        }
                        int bottom = bounds.Bottom;
                        VirtualToolTip.Default.ShowTooltip(item, builder.ToString(), this.listView, left, bottom);
                        this.HoverItem = tag;
                    }
                }
            }
        }

        private void tmrUpdateItems_Tick(object sender, EventArgs e)
        {
            if (this.CheckPanelState(PanelState.DoFolderChangedTick))
            {
                System.Windows.Forms.Timer timer;
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                VirtualPropertySet a = null;
                WatcherChangeTypes folderChangeRequested = 0;
                bool flag4 = false;
                string tag = null;
                lock ((timer = this.tmrUpdateItems))
                {
                    folderChangeRequested = this.FolderChangeRequested;
                    a = this.FolderChangePropertySet;
                    flag4 = this.CheckPanelState(PanelState.ProgressUpdatePending);
                    tag = this.tsPath.Tag as string;
                    this.FolderChangeRequested = 0;
                    this.FolderChangePropertySet = null;
                    this.tsPath.Tag = null;
                    this.SetPanelState(PanelState.ProgressUpdatePending | PanelState.DoFolderChangedTick, false);
                    if (folderChangeRequested == WatcherChangeTypes.Changed)
                    {
                        VirtualItemComparer sort = this.Sort as VirtualItemComparer;
                        flag2 = (sort != null) && VirtualPropertySet.Has(a, sort.ComparePropertyId);
                    }
                    else
                    {
                        flag2 = folderChangeRequested != 0;
                    }
                    flag3 = flag2 && this.CheckPanelState(PanelState.FolderChangePending);
                    flag2 = flag2 && !this.CheckPanelState(PanelState.FolderChangePending);
                }
                if (flag4)
                {
                    if (tag != null)
                    {
                        this.tsPath.SimpleText = tag;
                        this.tsPath.View = BreadcrumbView.SimpleText;
                        this.tsPath.ShowProgress = true;
                    }
                    else
                    {
                        this.tsPath.ShowProgress = false;
                        this.tsPath.View = BreadcrumbView.Breadcrumb;
                    }
                }
                if ((folderChangeRequested & WatcherChangeTypes.Changed) > 0)
                {
                    this.ClearListViewCache();
                }
                flag = flag2 || (((folderChangeRequested != 0) && (folderChangeRequested != WatcherChangeTypes.Created)) && (folderChangeRequested != (WatcherChangeTypes.Changed | WatcherChangeTypes.Created)));
                HashList<IVirtualItem> fItems = this.FItems;
                try
                {
                    if (((folderChangeRequested != 0) && (folderChangeRequested != WatcherChangeTypes.Changed)) || flag2)
                    {
                        IEnumerable<IVirtualItem> cachedContent;
                        int focusedIndex = this.FocusedIndex;
                        bool ensureVisible = this.DelayedFocusedItem != null;
                        IVirtualItem item = ensureVisible ? this.DelayedFocusedItem : this.FocusedItem;
                        while (true)
                        {
                            try
                            {
                                cachedContent = ((IVirtualCachedFolder) this.CurrentFolder).GetCachedContent();
                                break;
                            }
                            catch (UnauthorizedAccessException)
                            {
                                if (this.FItems != null)
                                {
                                    throw;
                                }
                                IElevatable currentFolder = this.CurrentFolder as IElevatable;
                                if ((currentFolder == null) || !currentFolder.CanElevate)
                                {
                                    throw;
                                }
                                if (MessageDialog.Show(this, string.Format(Resources.sAskElevateFolderPermissions, this.CurrentFolder.FullName), Resources.sWarning, new MessageDialogResult[] { MessageDialogResult.Shield, MessageDialogResult.Cancel }, MessageBoxIcon.Exclamation, MessageDialogResult.Shield) == MessageDialogResult.Shield)
                                {
                                    if (!currentFolder.Elevate(new ElevatedProcess()))
                                    {
                                        throw;
                                    }
                                }
                                else
                                {
                                    folderChangeRequested = 0;
                                    a = null;
                                    this.CurrentFolderDeleted(null, new VirtualItemChangedEventArgs(WatcherChangeTypes.Deleted, this.CurrentFolder));
                                    return;
                                }
                            }
                        }
                        this.PopulateListViewItems(cachedContent, flag2 ? ListViewSort.Full : ListViewSort.Fast);
                        cachedContent = null;
                        this.SetFocusedItem(item, false, ensureVisible);
                        if (((((folderChangeRequested & WatcherChangeTypes.Deleted) > 0) && (this.FocusedItem == null)) && (focusedIndex >= 0)) && (this.FItems != null))
                        {
                            int num4;
                            int num2 = -1;
                            if (flag2)
                            {
                                List<int> list2 = new List<int>(fItems.Count);
                                int num3 = 0;
                                for (num4 = 0; num4 < fItems.Count; num4++)
                                {
                                    if ((num3 < this.FItems.Count) && fItems[num4].Equals(this.FItems[num3]))
                                    {
                                        list2.Add(num3++);
                                    }
                                    else
                                    {
                                        list2.Add(-1);
                                    }
                                }
                                for (num4 = focusedIndex; num4 < list2.Count; num4++)
                                {
                                    if (list2[num4] >= 0)
                                    {
                                        num2 = list2[num4];
                                        break;
                                    }
                                }
                                if (num2 < 0)
                                {
                                    for (num4 = Math.Min(focusedIndex - 1, list2.Count); num4 >= 0; num4--)
                                    {
                                        if (list2[num4] >= 0)
                                        {
                                            num2 = list2[num4];
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Dictionary<IVirtualItem, int> dictionary = new Dictionary<IVirtualItem, int>(this.FItems.Count);
                                for (num4 = 0; num4 < this.FItems.Count; num4++)
                                {
                                    dictionary.Add(this.FItems[num4], num4);
                                }
                                for (num4 = focusedIndex; num4 < fItems.Count; num4++)
                                {
                                    if (dictionary.TryGetValue(fItems[num4], out num2))
                                    {
                                        break;
                                    }
                                    num2 = -1;
                                }
                                if (num2 < 0)
                                {
                                    for (num4 = focusedIndex - 1; num4 >= 0; num4--)
                                    {
                                        if (dictionary.TryGetValue(fItems[num4], out num2))
                                        {
                                            break;
                                        }
                                        num2 = -1;
                                    }
                                }
                            }
                            if (num2 >= 0)
                            {
                                this.FocusItem(this.listView.Items[num2], true);
                            }
                        }
                    }
                    IVirtualItem folderChangeItem = null;
                    lock ((timer = this.tmrUpdateItems))
                    {
                        folderChangeItem = this.FolderChangeItem;
                        this.FolderChangeItem = null;
                    }
                    if (!flag || (this.FItems == null))
                    {
                        return;
                    }
                    int startIndex = -1;
                    int endIndex = this.FItems.Count - 1;
                    if (!((folderChangeItem == null) || flag2))
                    {
                        switch (folderChangeRequested)
                        {
                            case WatcherChangeTypes.Created:
                            case (WatcherChangeTypes.Changed | WatcherChangeTypes.Created):
                                startIndex = this.FItems.IndexOf(folderChangeItem);
                                goto Label_0601;

                            case WatcherChangeTypes.Deleted:
                            case (WatcherChangeTypes.Changed | WatcherChangeTypes.Deleted):
                                startIndex = fItems.IndexOf(folderChangeItem);
                                goto Label_0601;

                            case (WatcherChangeTypes.Deleted | WatcherChangeTypes.Created):
                                goto Label_0601;

                            case WatcherChangeTypes.Changed:
                            case (WatcherChangeTypes.Renamed | WatcherChangeTypes.Changed):
                                goto Label_05CE;
                        }
                    }
                    goto Label_0601;
                Label_05CE:
                    startIndex = this.FItems.IndexOf(folderChangeItem);
                    endIndex = startIndex;
                Label_0601:
                    if ((startIndex >= 0) && (startIndex < this.FItems.Count))
                    {
                        this.listView.RedrawItems(startIndex, endIndex, true);
                    }
                    else
                    {
                        this.InvalidateListView(false);
                    }
                    bool flag6 = (folderChangeRequested & WatcherChangeTypes.Deleted) > 0;
                    if (((folderChangeItem == null) || folderChangeItem.Equals(this.FocusedItem)) || flag6)
                    {
                        this.UpdateFocusedItem();
                        this.ShowFocusedItemInfo(this.FocusedItem);
                    }
                    if ((flag6 || (((folderChangeRequested & (WatcherChangeTypes.Changed | WatcherChangeTypes.Created)) == WatcherChangeTypes.Changed) && VirtualPropertySet.Has(a, 3))) && ((folderChangeItem == null) || this.FSelection.Contains(folderChangeItem)))
                    {
                        this.UpdateFolderInfo(!VirtualFilePanelSettings.Default.ShowUpFolderItem);
                        this.UpdateSelectionInfo();
                    }
                }
                catch (SystemException exception)
                {
                    folderChangeRequested = 0;
                    a = null;
                    if (!VirtualItem.IsFolderInaccessibleException(exception))
                    {
                        throw;
                    }
                    if (this.tsPath.CurrentFolder == null)
                    {
                        this.ShowNavigationError(exception, this.CurrentFolder.FullName);
                    }
                    else
                    {
                        MessageDialog.ShowException(this, exception);
                    }
                    Application.DoEvents();
                    this.CurrentFolderDeleted(null, new VirtualItemChangedEventArgs(WatcherChangeTypes.Deleted, this.CurrentFolder));
                }
                finally
                {
                    lock ((timer = this.tmrUpdateItems))
                    {
                        if ((this.FolderChangeRequested == 0) && this.CheckPanelState(PanelState.FolderChangePending))
                        {
                            if (flag3)
                            {
                                this.FolderChangeRequested = folderChangeRequested;
                                this.FolderChangePropertySet = a;
                                this.SetPanelState(PanelState.DoFolderChangedTick, true);
                            }
                            this.SetPanelState(PanelState.FolderChangePending, false);
                        }
                        else
                        {
                            if (this.CheckPanelState(PanelState.FolderChangePending) && flag3)
                            {
                                this.FolderChangeRequested |= folderChangeRequested;
                                this.FolderChangePropertySet |= a;
                            }
                            this.SetPanelState(PanelState.DoFolderChangedTick, this.FolderChangeRequested > 0);
                            if (((this.FolderChangeRequested == WatcherChangeTypes.Created) && (this.FItems != null)) && (this.FItems.Count > 0xfa0))
                            {
                                this.tmrUpdateItems.Interval += 250;
                            }
                            else
                            {
                                this.SetUpdateTimerInterval(this.FCurrentFolder, this.CheckPanelState(PanelState.LastParentFormActive));
                            }
                        }
                    }
                }
            }
        }

        public void ToggleQuickFind()
        {
            if (this.tsFind.Focused || this.tstFind.Focused)
            {
                this.tsFind.Visible = false;
            }
            else
            {
                this.tsFind.Visible = true;
                this.tsFind.Focus();
                this.tstFind.Focus();
            }
        }

        private void ToggleSelectItem(IVirtualItem Item)
        {
            if (this.FSelection.Contains(Item))
            {
                this.FSelection.Remove(Item);
            }
            else
            {
                this.FSelection.Add(Item);
            }
        }

        public bool ToRoot()
        {
            if (this.FCurrentFolder != null)
            {
                IVirtualFolder folder = null;
                IVirtualFolder folderRoot = VirtualItemHelper.GetFolderRoot(this.FCurrentFolder);
                IVirtualFolder fCurrentFolder = this.FCurrentFolder;
                while ((fCurrentFolder.Parent != null) && !fCurrentFolder.Equals(folderRoot))
                {
                    folder = fCurrentFolder;
                    fCurrentFolder = fCurrentFolder.Parent;
                }
                this.CurrentFolder = fCurrentFolder;
                this.FocusedItem = folder;
                return true;
            }
            return false;
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!e.CancelEdit)
            {
                this.tsPath.UpdatePath();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.CurrentFolder = (IVirtualFolder) e.Node.Tag;
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            DragImage.DragDrop((IWin32Window) sender, e);
            this.treeView.SetDropHilited(null);
            this.tmrExpandNode.Stop();
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.FDragDropOnItem != null))
            {
                Point pt = ((Control) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode nodeAt = this.treeView.GetNodeAt(pt);
                if (nodeAt != null)
                {
                    this.FDragDropOnItem(this, new VirtualItemDragEventArg((IVirtualFolder) nodeAt.Tag, e));
                }
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            Control owner = (Control) sender;
            DragImage.DragOver(owner, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data) && (this.FDragOverItem != null))
            {
                Point pt = owner.PointToClient(new Point(e.X, e.Y));
                TreeNode nodeAt = this.treeView.GetNodeAt(pt);
                if (nodeAt != null)
                {
                    TreeNode prevVisibleNode = null;
                    if (pt.Y < nodeAt.Bounds.Height)
                    {
                        prevVisibleNode = nodeAt.PrevVisibleNode;
                    }
                    else if (pt.Y > (owner.Size.Height - nodeAt.Bounds.Height))
                    {
                        prevVisibleNode = nodeAt.NextVisibleNode;
                    }
                    if (prevVisibleNode != null)
                    {
                        nodeAt = prevVisibleNode;
                        DragImage.Hide();
                        nodeAt.EnsureVisible();
                        DragImage.Show();
                    }
                    this.RaiseDragOverItem((IVirtualFolder) nodeAt.Tag, this.DropEffect, e);
                    if (this.tmrExpandNode.Tag != nodeAt)
                    {
                        this.tmrExpandNode.Stop();
                        this.tmrExpandNode.Tag = nodeAt;
                    }
                    this.tmrExpandNode.Start();
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
                if (e.Effect == DragDropEffects.None)
                {
                    nodeAt = null;
                }
                if (nodeAt != this.treeView.GetDropHilited())
                {
                    DragImage.Hide();
                    this.treeView.SetDropHilited(nodeAt);
                    DragImage.Show();
                }
            }
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ShowContextMenu = ContextMenuSource.Mouse;
            }
        }

        private void tsbClearFilter_Click(object sender, EventArgs e)
        {
            this.Filter = null;
        }

        private void tsbClearSelection_Click(object sender, EventArgs e)
        {
            this.UnselectItems(null);
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            SearchDirectionHint direction = (sender == this.tsbFindPrevious) ? SearchDirectionHint.Up : SearchDirectionHint.Down;
            this.ChangeFoundPanelState(this.FocusFindItem(this.tstFind.Text, direction));
        }

        private void tsbFind_Paint(object sender, PaintEventArgs e)
        {
            ((ToolStripItem) sender).Enabled = !string.IsNullOrEmpty(this.tstFind.Text);
        }

        private void tsbFolderButton_EnabledChanged(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            if (!(!item.Enabled || item.Visible))
            {
                item.Image = IconSet.GetImage(string.Format("{0}.{1}.Image", base.Name, item.Name));
            }
            item.Visible = item.Enabled;
            this.tssFolder4.Visible = (this.tsbClearSelection.Enabled || this.tsbUnlockFolder.Enabled) || this.tsbClearFilter.Enabled;
        }

        private void tsbFolderButton_MouseEnter(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            Image image = IconSet.GetImage(string.Format("{0}.{1}.Image|MouseOver", base.Name, item.Name));
            if (image != null)
            {
                item.Image = image;
            }
        }

        private void tsbFolderButton_MouseLeave(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            item.Image = IconSet.GetImage(string.Format("{0}.{1}.Image", base.Name, item.Name));
        }

        private void tsbUnlockFolder_Click(object sender, EventArgs e)
        {
            this.IsFolderLocked = false;
        }

        private void tsddFind_TextChanged(object sender, EventArgs e)
        {
            this.tsddFind.Text = ((this.FindOptions & QuickFindOptions.QuickFilter) > 0) ? Resources.sFilter : Resources.sFind;
        }

        private void tsFind_DoubleClick(object sender, EventArgs e)
        {
            if (this.tsFind.GetItemAt(this.tsFind.PointToClient(Cursor.Position)) == null)
            {
                this.tsFind.Visible = false;
            }
        }

        private void tsFind_Leave(object sender, EventArgs e)
        {
            if ((this.FindOptions & (QuickFindOptions.AutoHide | QuickFindOptions.QuickFilter)) == QuickFindOptions.AutoHide)
            {
                this.tsFind.Visible = false;
            }
        }

        private void tsFind_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.tsFind.Visible = false;
            }
        }

        private void tsFind_VisibleChanged(object sender, EventArgs e)
        {
            this.InfoToolbar_VisibleChanged(sender, e);
            if ((this.FindOptions & QuickFindOptions.QuickFilter) > 0)
            {
                this.Filter = this.tsPath.Visible ? this.CreateQuickFilter() : null;
            }
        }

        private void tsFolderInfo_VisibleChanged(object sender, EventArgs e)
        {
            this.InfoToolbar_VisibleChanged(sender, e);
            this.UpdateFolderInfo(!VirtualFilePanelSettings.Default.ShowUpFolderItem);
            this.UpdateSelectionInfo();
        }

        private void tsItemInfo_Paint(object sender, PaintEventArgs e)
        {
            this.tssItemSize.Visible = this.tslItemSize.Visible;
            this.tssItemDate.Visible = this.tslItemDate.Visible;
        }

        private void tslFolderCount_Click(object sender, EventArgs e)
        {
            IVirtualItemFilter filter = new VirtualItemAttributeFilter(FileAttributes.Directory);
            if (this.FSelection.Any<IVirtualItem>(delegate (IVirtualItem item) {
                return item is IVirtualFolder;
            }))
            {
                this.UnselectItems(filter);
            }
            else
            {
                this.SelectItems(filter, false);
            }
        }

        private void tslItemCount_Click(object sender, EventArgs e)
        {
            IVirtualItemFilter filter = new VirtualItemAttributeFilter(0, FileAttributes.Directory);
            if (this.FSelection.Any<IVirtualItem>(delegate (IVirtualItem item) {
                return !(item is IVirtualFolder);
            }))
            {
                this.UnselectItems(filter);
            }
            else
            {
                this.SelectItems(filter, false);
            }
        }

        private void tsmiAlwaysShowFolders_Click(object sender, EventArgs e)
        {
            if ((this.FindOptions & QuickFindOptions.AlwaysShowFolders) > 0)
            {
                this.FindOptions &= ~QuickFindOptions.AlwaysShowFolders;
            }
            else
            {
                this.FindOptions |= QuickFindOptions.AlwaysShowFolders;
            }
            this.Filter = this.CreateQuickFilter();
        }

        private void tsmiAlwaysShowFolders_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            item.Checked = (this.FindOptions & QuickFindOptions.AlwaysShowFolders) > 0;
            item.Enabled = this.tsmiQuickFilter.Checked;
        }

        private void tsmiAutoHide_Click(object sender, EventArgs e)
        {
            if (this.tsmiAutoHide.Checked)
            {
                this.FindOptions &= ~QuickFindOptions.AutoHide;
            }
            else
            {
                this.FindOptions |= QuickFindOptions.AutoHide;
            }
        }

        private void tsmiAutoSizeColumns_Click(object sender, EventArgs e)
        {
            this.AutoSizeColumns = !this.AutoSizeColumns;
        }

        private void tsmiManageColumns_Click(object sender, EventArgs e)
        {
            this.ManageColumns(base.FindForm());
        }

        private void tsmiQuickFilter_Click(object sender, EventArgs e)
        {
            this.FindOptions |= (QuickFindOptions) ((ToolStripItem) sender).Tag;
            this.Filter = ((this.FindOptions & QuickFindOptions.QuickFilter) > 0) ? this.CreateQuickFilter() : null;
        }

        private void tsmiQuickFilter_Paint(object sender, PaintEventArgs e)
        {
            ((ToolStripMenuItem) sender).Checked = (this.FindOptions & ((QuickFindOptions) ((ToolStripItem) sender).Tag)) > 0;
        }

        private void tsmiQuickFind_Click(object sender, EventArgs e)
        {
            this.FindOptions &= ~((QuickFindOptions) ((ToolStripItem) sender).Tag);
            this.Filter = ((this.FindOptions & QuickFindOptions.QuickFilter) > 0) ? this.CreateQuickFilter() : null;
        }

        private void tsmiQuickFind_Paint(object sender, PaintEventArgs e)
        {
            ((ToolStripMenuItem) sender).Checked = (this.FindOptions & ((QuickFindOptions) ((ToolStripItem) sender).Tag)) == 0;
        }

        private void tsmiRememberWidthAsDefault_Click(object sender, EventArgs e)
        {
            int tag = (int) ((ToolStripItem) sender).Tag;
            ColumnHeader header = this.listView.Columns[tag];
            ListViewColumnInfo info = (ListViewColumnInfo) header.Tag;
            info.DefaultWidth = header.Width;
        }

        private void tsmiResetDefaultWidth_Click(object sender, EventArgs e)
        {
            int tag = (int) ((ToolStripItem) sender).Tag;
            ColumnHeader header = this.listView.Columns[tag];
            ListViewColumnInfo info = (ListViewColumnInfo) header.Tag;
            info.DefaultWidth = -1;
            this.UpdateListView();
        }

        private void tsmiToolStripVisible_Click(object sender, EventArgs e)
        {
            ToolStrip tag = (ToolStrip) ((ToolStripMenuItem) sender).Tag;
            tag.Visible = !tag.Visible;
        }

        private void tsmiToolStripVisible_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            item.Checked = ((ToolStrip) item.Tag).Visible;
        }

        private void tsPath_AfterPaint(object sender, PaintEventArgs e)
        {
            ToolStrip strip = (ToolStrip) sender;
            if ((this.ControlModifierKeys == Keys.Control) && (strip.Items.Count > 0))
            {
                using (Font font = new Font(strip.Font.FontFamily, 6.5f))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                    string text = converter.ConvertToString(Keys.Left);
                    string str2 = converter.ConvertToString(Keys.Right);
                    int left = strip.Items[0].Bounds.Left;
                    Size textSize = TextRenderer.MeasureText(e.Graphics, text, font);
                    this.DrawCueText(strip, e.Graphics, text, font, textSize, left);
                    this.DrawCueText(strip, e.Graphics, str2, font, TextRenderer.MeasureText(e.Graphics, str2, font), (left + textSize.Width) + 4);
                }
            }
        }

        private void tsPath_CommandClicked(object sender, EventArgs e)
        {
            switch (this.tsPath.View)
            {
                case BreadcrumbView.Breadcrumb:
                case BreadcrumbView.SimpleText:
                    if (!this.tsPath.ShowProgress)
                    {
                        this.RefreshCurrentFolder();
                        break;
                    }
                    this.CancelAsyncFolder();
                    break;

                case BreadcrumbView.EnterPath:
                    base.UseWaitCursor = true;
                    Application.DoEvents();
                    base.UseWaitCursor = false;
                    if (!this.SetCurrentFolder(this.tsPath.CurrentText, VirtualItemType.Unknown))
                    {
                        this.tsPath.PathTextBox.SelectAll();
                        break;
                    }
                    if (this.CurrentFolder != null)
                    {
                        HistorySettings.Default.AddStringToChangeFolder(this.CurrentFolder.FullName);
                    }
                    this.listView.Focus();
                    break;
            }
        }

        private void tsPath_DriveClicked(object sender, VirtualItemEventArgs e)
        {
            this.SetDrive((IVirtualFolder) e.Item, (Control.ModifierKeys & Keys.Shift) == Keys.None);
            this.listView.Focus();
        }

        private void tsPath_FolderClicked(object sender, VirtualItemEventArgs e)
        {
            this.CurrentFolder = (IVirtualFolder) e.Item;
            this.listView.Focus();
        }

        private void tsPath_KeyUp(object sender, KeyEventArgs e)
        {
            if ((this.tsPath.View == BreadcrumbView.Drives) && ((Control.ModifierKeys & Keys.Control) == Keys.None))
            {
                IVirtualFolder selectedFolder = this.tsPath.SelectedFolder;
                if (selectedFolder != null)
                {
                    this.SetDrive(selectedFolder, (Control.ModifierKeys & Keys.Shift) == Keys.None);
                }
                this.tsPath.View = BreadcrumbView.Breadcrumb;
                this.listView.Focus();
            }
        }

        private void tssbAsyncFolder_EnabledChanged(object sender, EventArgs e)
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

        private void tstFind_Enter(object sender, EventArgs e)
        {
            this.tstFind.SelectAll();
        }

        private void tstFind_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    this.ChangeFoundPanelState(this.FocusFindItem(this.tstFind.Text, SearchDirectionHint.Up));
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Down:
                    this.ChangeFoundPanelState(this.FocusFindItem(this.tstFind.Text, SearchDirectionHint.Down));
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void tstFind_TextChanged(object sender, EventArgs e)
        {
            QuickFindOptions findOptions = this.FindOptions;
            if ((findOptions & QuickFindOptions.QuickFilter) > 0)
            {
                this.Filter = this.CreateQuickFilter();
            }
            bool found = string.IsNullOrEmpty(this.tstFind.Text);
            if (!found)
            {
                ListViewItem focusedItem = this.listView.FocusedItem;
                if (focusedItem != null)
                {
                    found = IsNameEqual(this.tstFind.Text, focusedItem.Text, (findOptions & QuickFindOptions.PrefixSearch) > 0);
                }
            }
            if (!found)
            {
                found = this.FocusFindItem(this.tstFind.Text, SearchDirectionHint.Down);
            }
            this.ChangeFoundPanelState(found);
        }

        public void UnselectItems(IVirtualItemFilter filter)
        {
            bool flag = false;
            if (filter == null)
            {
                flag = this.FSelection.Count > 0;
                this.FSelection.Clear();
            }
            else
            {
                flag = this.FSelection.RemoveWhere(delegate (IVirtualItem x) {
                    return filter.IsMatch(x);
                }) > 0;
            }
            if (flag)
            {
                this.OnSelectionChanged(EventArgs.Empty);
                this.InvalidateListView(false);
            }
        }

        private void UpdateColorsFromTheme()
        {
            ListViewColorTable listViewColors;
            IDictionary<KnownListViewColor, System.Drawing.Color> listColorMap = VirtualFilePanelSettings.Default.ListColorMap;
            if ((listColorMap != null) && (listColorMap.Count > 0))
            {
                listViewColors = new ListViewMapColorTable(Theme.Current.ListViewColors, listColorMap);
            }
            else
            {
                listViewColors = Theme.Current.ListViewColors;
            }
            this.BeginListViewUpdate(false, false);
            try
            {
                this.OddLineBackColor = listViewColors.OddLineBack;
                this.ListBackColor = listViewColors.Back;
                this.ListActiveBackColor = listViewColors.ActiveBack;
                this.ListForeColor = listViewColors.Text;
                this.ListFocusedBackColor = listViewColors.FocusedBack;
                this.ListFocusedForeColor = listViewColors.FocusedText;
                this.ListSelectedForeColor = listViewColors.SelectedText;
            }
            finally
            {
                this.EndListViewUpdate();
            }
        }

        private void UpdateColumnInfos()
        {
            if (this.listView.View == System.Windows.Forms.View.Details)
            {
                foreach (ColumnHeader header in this.listView.Columns)
                {
                    ListViewColumnInfo tag = (ListViewColumnInfo) header.Tag;
                    tag.DisplayIndex = header.DisplayIndex;
                    tag.Width = header.Width;
                }
            }
        }

        public void UpdateCulture()
        {
            if (this.listView.View == System.Windows.Forms.View.Details)
            {
                foreach (ColumnHeader header in this.listView.Columns)
                {
                    ListViewColumnInfo tag = (ListViewColumnInfo) header.Tag;
                    header.Text = VirtualProperty.Get(tag.PropertyId).LocalizedName;
                }
            }
            this.UpdateFolderInfo(!VirtualFilePanelSettings.Default.ShowUpFolderItem);
            this.UpdateSelectionInfo();
            if (this.tsItemInfo.Visible)
            {
                this.ShowFocusedItemInfo(this.FocusedItem);
            }
            MainForm fParentForm = (MainForm) this.FParentForm;
            if (fParentForm != null)
            {
                this.tsmiCopyPathAsText.Text = fParentForm.actCopyCurrentFolderAsText.Text;
                this.tsmiBack.Text = fParentForm.actBack.Text;
                this.tsmiForward.Text = fParentForm.actForward.Text;
                this.tsmiChangeFolder.Text = fParentForm.actChangeFolder.Text;
                this.tsmiRefresh.Text = fParentForm.actRefresh.Text;
            }
        }

        private void UpdateFocusedItem()
        {
            int focusedIndex = this.FocusedIndex;
            ListViewItem focusedItem = this.listView.FocusedItem;
            if (this.listView.VirtualMode)
            {
                this.listView.SelectedIndices.Clear();
            }
            else
            {
                this.listView.SelectedItems.Clear();
            }
            if (focusedItem != null)
            {
                this.SetPanelState(PanelState.SkipKeyboardTooltip, true);
                try
                {
                    if (!((!this.CheckPanelState(PanelState.UseFocusSelection) && !this.listView.ExplorerTheme) && this.listView.Focused))
                    {
                        focusedItem.Selected = true;
                    }
                    if (focusedItem.Index != this.FocusedIndex)
                    {
                        this.listView_ItemSelectionChanged(this.listView, new ListViewItemSelectionChangedEventArgs(focusedItem, focusedItem.Index, false));
                    }
                }
                finally
                {
                    this.SetPanelState(PanelState.SkipKeyboardTooltip, false);
                }
            }
            else
            {
                this.FocusedIndex = -1;
            }
        }

        private void UpdateFolderInfo(bool skipUpFolderItemCheck)
        {
            if (this.tsFolderInfo.Visible && (this.CurrentFolder != null))
            {
                int num = 0;
                int num2 = 0;
                long num3 = 0L;
                foreach (IVirtualItem item in this.GetPanelItems(skipUpFolderItemCheck))
                {
                    switch (item.GetPropertyAvailability(3))
                    {
                        case PropertyAvailability.Normal:
                        case PropertyAvailability.Slow:
                            num3 += Convert.ToInt64(item[3]);
                            break;
                    }
                    if (item is IVirtualFolder)
                    {
                        num2++;
                    }
                    else
                    {
                        num++;
                    }
                }
                this.tslFolderSize.Text = SizeTypeConverter.Default.ConvertToString(num3);
                this.tslItemCount.Text = num.ToString();
                this.tslFolderCount.Text = num2.ToString();
            }
        }

        private void UpdateImageLists()
        {
            Size defaultSmallIconSize;
            switch (this.listView.View)
            {
                case System.Windows.Forms.View.Details:
                case System.Windows.Forms.View.SmallIcon:
                case System.Windows.Forms.View.List:
                    if (this.ShowItemIcons)
                    {
                        defaultSmallIconSize = ImageHelper.DefaultSmallIconSize;
                        if (this.listView.ExplorerTheme)
                        {
                            switch (this.listView.View)
                            {
                                case System.Windows.Forms.View.Details:
                                    defaultSmallIconSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
                                    break;

                                case System.Windows.Forms.View.List:
                                    defaultSmallIconSize = new Size(ImageHelper.DefaultSmallIconSize.Width + 2, ImageHelper.DefaultSmallIconSize.Height + 3);
                                    break;
                            }
                        }
                        break;
                    }
                    return;

                default:
                    return;
            }
            ImageList list = ImageListCache.Get(defaultSmallIconSize);
            if (this.listView.SmallImageList != list)
            {
                this.listView.SmallImageList = list;
            }
        }

        private void UpdateListView()
        {
            if (this.UpdateCount > 0)
            {
                this.UpdateAction |= ListViewUpdateAction.UpdateListView;
            }
            else
            {
                switch (this.listView.View)
                {
                    case System.Windows.Forms.View.LargeIcon:
                    case System.Windows.Forms.View.SmallIcon:
                        this.InvalidateListView(false);
                        return;

                    case System.Windows.Forms.View.Details:
                        if (this.AutoSizeColumns)
                        {
                            this.BeginListViewUpdate(false, true);
                            try
                            {
                                ColumnHeader header = null;
                                int num = 0;
                                foreach (ColumnHeader header2 in this.listView.Columns)
                                {
                                    ListViewColumnInfo tag = (ListViewColumnInfo) header2.Tag;
                                    if (tag.PropertyId == 0)
                                    {
                                        header = header2;
                                    }
                                    else
                                    {
                                        header2.Width = (tag.DefaultWidth > 0) ? tag.DefaultWidth : VirtualFilePanelSettings.DefaultColumnWidth(tag.PropertyId, this.listView.Font);
                                        num += header2.Width;
                                    }
                                }
                                if (header != null)
                                {
                                    header.Width = ((this.listView.ClientSize.Width - num) > 120) ? (this.listView.ClientSize.Width - num) : 120;
                                }
                            }
                            finally
                            {
                                this.EndListViewUpdate();
                            }
                        }
                        return;

                    case System.Windows.Forms.View.List:
                        if (this.ListViewColumnCount > 0)
                        {
                            this.listView.SetColumnWidth(0, this.listView.ClientSize.Width / this.ActualListViewColumnCount);
                            return;
                        }
                        this.listView.SetColumnWidth(0, -1);
                        return;
                }
            }
        }

        private void UpdatePathCurrentFolder(IVirtualFolder currentFolder)
        {
            if (currentFolder == null)
            {
                this.tsPath.CurrentFolder = null;
            }
            else
            {
                this.tsPath.SuspendLayout();
                try
                {
                    this.tsPath.CurrentFolder = currentFolder;
                    this.tsPath.View = BreadcrumbView.Breadcrumb;
                }
                finally
                {
                    this.tsPath.ResumeLayout();
                }
            }
        }

        private void UpdateSelectionInfo()
        {
            if (this.tsFolderInfo.Visible)
            {
                if (this.FSelection.Count == 0)
                {
                    this.tslSelectionInfo.Text = Resources.sNoSelectedItems;
                }
                else
                {
                    long num = 0L;
                    foreach (IVirtualItem item in this.FSelection)
                    {
                        switch (item.GetPropertyAvailability(3))
                        {
                            case PropertyAvailability.Normal:
                            case PropertyAvailability.Slow:
                            {
                                num += Convert.ToInt64(item[3]);
                                continue;
                            }
                        }
                    }
                    this.tslSelectionInfo.Text = PluralInfo.Format(Resources.sSelectedItemsInfo, new object[] { SizeTypeConverter.Default.ConvertToString(num), this.FSelection.Count });
                }
            }
        }

        public bool UpFolder()
        {
            if (this.ParentFolder != null)
            {
                this.CurrentFolder = this.ParentFolder;
                return true;
            }
            return false;
        }

        private void VirtualFilePanel_Disposed(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(this.Event_ApplicationIdle);
            Settings.Default.PropertyChanged -= new PropertyChangedEventHandler(this.SettingPropertyChanged);
            VirtualFilePanelSettings.Default.PropertyChanged -= new PropertyChangedEventHandler(this.SettingPropertyChanged);
            if (this.FParentForm != null)
            {
                this.FParentForm.Activated -= new EventHandler(this.ParentForm_Activated);
                this.FParentForm.Deactivate -= new EventHandler(this.ParentForm_Deactivate);
                this.FParentForm.ResizeBegin -= new EventHandler(this.ParentForm_ResizeBegin);
                this.FParentForm.ResizeEnd -= new EventHandler(this.ParentForm_ResizeEnd);
                FormEx fParentForm = this.FParentForm as FormEx;
                if (fParentForm != null)
                {
                    fParentForm.WindowStateChanging -= new EventHandler<WindowStateChangingEventArgs>(this.ParentForm_WindowStateChanging);
                }
            }
            if (this.SaveSettings)
            {
                this.SaveComponentSettings();
            }
        }

        private void VirtualFilePanel_Enter(object sender, EventArgs e)
        {
            this.SetPanelState(PanelState.LastParentFormActive | PanelState.LastContainsFocus, true);
            this.tsPath.Active = true;
            if (!(this.ListActiveBackColor.IsEmpty || !(this.listView.BackColor != this.ListActiveBackColor)))
            {
                this.listView.BackColor = this.ListActiveBackColor;
                this.InvalidateListView(true);
            }
        }

        private void VirtualFilePanel_Leave(object sender, EventArgs e)
        {
            this.SetPanelState(PanelState.LastContainsFocus, false);
            this.tsPath.Active = false;
            if (this.listView.BackColor != this.ListBackColor)
            {
                this.listView.BackColor = this.ListBackColor;
                this.InvalidateListView(true);
            }
        }

        private void VirtualFilePanel_Load(object sender, EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.FParentForm = base.FindForm();
                System.Diagnostics.Debug.Assert(this.FParentForm != null);
                this.FParentForm.Activated += new EventHandler(this.ParentForm_Activated);
                this.FParentForm.Deactivate += new EventHandler(this.ParentForm_Deactivate);
                this.FParentForm.ResizeBegin += new EventHandler(this.ParentForm_ResizeBegin);
                this.FParentForm.ResizeEnd += new EventHandler(this.ParentForm_ResizeEnd);
                FormEx fParentForm = this.FParentForm as FormEx;
                if (fParentForm != null)
                {
                    fParentForm.WindowStateChanging += new EventHandler<WindowStateChangingEventArgs>(this.ParentForm_WindowStateChanging);
                }
                MainForm form = (MainForm) this.FParentForm;
                new ActionToolStripItemLink(form.actCopyCurrentFolderAsText, this.tsmiCopyPathAsText, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                new ActionToolStripItemLink(form.actBack, this.tsmiBack, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                new ActionToolStripItemLink(form.actForward, this.tsmiForward, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                new ActionToolStripItemLink(form.actChangeFolder, this.tsmiChangeFolder, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                new ActionToolStripItemLink(form.actRefresh, this.tsmiRefresh, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
            }
        }

        protected void VirtualItemExecute(IVirtualItem item)
        {
            if (item != null)
            {
                ICustomizeFolder customize = null;
                IVirtualLink link = item as IVirtualLink;
                if (link != null)
                {
                    IVirtualItem target = link.Target;
                    if (target is IVirtualFolder)
                    {
                        item = target;
                        customize = link[0x17] as ICustomizeFolder;
                    }
                }
                IVirtualFolder folder2 = item as IVirtualFolder;
                if (folder2 != null)
                {
                    this.SetCurrentFolder(folder2, true, customize);
                }
                else
                {
                    HandleVirtualItemEventArgs e = new HandleVirtualItemEventArgs(item);
                    if (this.ExecuteItem != null)
                    {
                        this.ExecuteItem(this, e);
                    }
                    if (!e.Handled)
                    {
                        IVirtualFileExecute execute = item as IVirtualFileExecute;
                        if (execute != null)
                        {
                            execute.Execute(this);
                        }
                    }
                }
            }
        }

        private void VirtualItemToolStripItem_MouseUp(object sender, MouseEventArgs e)
        {
            VirtualItemToolStripEvents.MouseUp(sender, e, new EventHandler<ExecuteVerbEventArgs>(this.ExecuteVerb));
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x319)
            {
                m.Result = (IntPtr) 1;
                switch (WindowsWrapper.GET_APPCOMMAND_LPARAM(m.LParam))
                {
                    case APPCOMMAND.APPCOMMAND_BROWSER_BACKWARD:
                        this.Back();
                        return;

                    case APPCOMMAND.APPCOMMAND_BROWSER_FORWARD:
                        this.Forward();
                        return;

                    case APPCOMMAND.APPCOMMAND_BROWSER_REFRESH:
                        this.RefreshCurrentFolder();
                        return;

                    case APPCOMMAND.APPCOMMAND_BROWSER_STOP:
                        this.CancelAsyncFolder();
                        return;
                }
                m.Result = IntPtr.Zero;
            }
            base.WndProc(ref m);
        }

        [Browsable(false)]
        public int ActualListViewColumnCount
        {
            get
            {
                int listViewColumnCount = this.ListViewColumnCount;
                if (VirtualFilePanelSettings.Default.OptimizedColumnCount)
                {
                    int num2 = this.listView.RowCount - 1;
                    if (num2 > 0)
                    {
                        listViewColumnCount = (this.listView.Items.Count / num2) + Math.Sign((int) (this.listView.Items.Count % num2));
                    }
                    if ((listViewColumnCount == 0) || (listViewColumnCount > this.ListViewColumnCount))
                    {
                        listViewColumnCount = this.ListViewColumnCount;
                    }
                }
                int minListColumnWidth = VirtualFilePanelSettings.Default.MinListColumnWidth;
                while ((listViewColumnCount > 1) && ((this.listView.ClientSize.Width / listViewColumnCount) < minListColumnWidth))
                {
                    listViewColumnCount--;
                }
                return listViewColumnCount;
            }
        }

        [DefaultValue(false)]
        public bool AutoSizeColumns
        {
            get
            {
                return !this.listView.CanResizeColumns;
            }
            set
            {
                if (this.listView.CanResizeColumns == value)
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberAutoSizeColumns = null;
                    }
                    this.listView.CanResizeColumns = !value;
                    this.UpdateListView();
                }
            }
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                VirtualPropertySet set = new VirtualPropertySet();
                foreach (IVirtualItem item in this.GetPanelItems())
                {
                    set.Or(item.AvailableProperties);
                }
                return set;
            }
        }

        private Dictionary<int, ListViewColumnInfo> ColumnInfoMap
        {
            get
            {
                if (this.FColumnInfoMap == null)
                {
                    this.FColumnInfoMap = new Dictionary<int, ListViewColumnInfo>();
                }
                return this.FColumnInfoMap;
            }
        }

        private ListViewColumnInfo[] Columns
        {
            get
            {
                Dictionary<int, ListViewColumnInfo> dictionary = (this.RememberColumnInfoMap != null) ? this.RememberColumnInfoMap : this.ColumnInfoMap;
                if (dictionary.Count > 0)
                {
                    List<ListViewColumnInfo> list = new List<ListViewColumnInfo>(dictionary.Count);
                    foreach (ListViewColumnInfo info in dictionary.Values)
                    {
                        if (!(!VirtualProperty.IsVisibleProperty(info.PropertyId) || info.IsEmpty))
                        {
                            list.Add(info);
                        }
                    }
                    return list.ToArray();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.FColumnInfoMap = new Dictionary<int, ListViewColumnInfo>(value.Length);
                    foreach (ListViewColumnInfo info in value)
                    {
                        this.FColumnInfoMap.Add(info.PropertyId, info);
                    }
                    this.BeginListViewUpdate(false, true);
                    try
                    {
                        this.RecreateColumns();
                        if ((this.listView.View == System.Windows.Forms.View.Details) && this.AutoSizeColumns)
                        {
                            this.UpdateListView();
                        }
                    }
                    finally
                    {
                        this.EndListViewUpdate();
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), DefaultValue((string) null)]
        public IVirtualFolder CurrentFolder
        {
            get
            {
                return this.FCurrentFolder;
            }
            set
            {
                if (base.Visible && base.IsHandleCreated)
                {
                    this.SetCurrentFolder(value, true);
                }
                else
                {
                    this.FLazyFolder = new VirtualItemContainer<IVirtualFolder>();
                    this.FLazyFolder.Value = value;
                }
            }
        }

        private int DropItemIndex
        {
            get
            {
                return this.FDropItemIndex;
            }
            set
            {
                if (this.FDropItemIndex != value)
                {
                    int fDropItemIndex = this.FDropItemIndex;
                    if (value < this.FItems.Count)
                    {
                        this.FDropItemIndex = value;
                    }
                    if ((fDropItemIndex >= 0) || (this.FDropItemIndex >= 0))
                    {
                        if (fDropItemIndex >= 0)
                        {
                            this.listView.RedrawItem(fDropItemIndex, true);
                        }
                        if (this.FDropItemIndex >= 0)
                        {
                            this.listView.RedrawItem(this.FDropItemIndex, true);
                        }
                        this.listView.Update();
                    }
                }
            }
        }

        [DefaultValue(0)]
        public CharacterCasing FileNameCasing
        {
            get
            {
                return this.FFileNameCasing;
            }
            set
            {
                if (this.FFileNameCasing != value)
                {
                    this.FFileNameCasing = value;
                    this.InvalidateListView(false);
                }
            }
        }

        [Browsable(false), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IVirtualItemFilter Filter
        {
            get
            {
                return this.FFilter;
            }
            set
            {
                if (((this.FFilter == null) && (value != null)) || ((this.FFilter != null) && !this.FFilter.Equals(value)))
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberFilter = null;
                        this.SetPanelState(PanelState.HasRememberFilter, false);
                    }
                    this.FFilter = value;
                    this.tsbClearFilter.Enabled = this.FFilter != null;
                    this.RefreshContent();
                }
            }
        }

        [DefaultValue(0x19)]
        public QuickFindOptions FindOptions
        {
            get
            {
                return this.FFindOptions;
            }
            set
            {
                if (this.FFindOptions != value)
                {
                    this.FFindOptions = value;
                    this.tsddFind.Text = string.Empty;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), DefaultValue((string) null)]
        public IVirtualItem FocusedItem
        {
            get
            {
                ListViewItem focusedItem = this.listView.FocusedItem;
                if (focusedItem != null)
                {
                    return (IVirtualItem) focusedItem.Tag;
                }
                return null;
            }
            set
            {
                this.SetFocusedItem(value, false, true);
            }
        }

        [DefaultValue(0)]
        public Orientation FolderBarOrientation
        {
            get
            {
                return (this.NewOrientation.HasValue ? this.NewOrientation.Value : this.splitContainer.Orientation);
            }
            set
            {
                if (this.IsLayoutSuspended)
                {
                    this.NewOrientation = new Orientation?(value);
                }
                else if (this.splitContainer.Orientation != value)
                {
                    LockWindowRedraw redraw = this.FolderBarVisible ? new LockWindowRedraw(this.splitContainer, true) : null;
                    try
                    {
                        int splitterPercent = this.SplitterPercent;
                        this.splitContainer.Orientation = value;
                        if (!((this.NewSplitterPercent >= 0) && this.CheckPanelState(PanelState.ProcessingOnLayout)))
                        {
                            this.SplitterPercent = splitterPercent;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    finally
                    {
                        if (redraw != null)
                        {
                            redraw.Dispose();
                        }
                    }
                }
            }
        }

        [DefaultValue(true)]
        public bool FolderBarVisible
        {
            get
            {
                return !this.splitContainer.Panel1Collapsed;
            }
            set
            {
                if (this.FolderBarVisible != value)
                {
                    this.SetFolderBarVisible(value, base.ContainsFocus);
                }
            }
        }

        [DefaultValue(0)]
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
                    this.InvalidateListView(false);
                    this.treeView.FolderNameCasing = value;
                }
            }
        }

        [DefaultValue("{0}")]
        public string FolderNameTemplate
        {
            get
            {
                return this.FFolderNameTemplate;
            }
            set
            {
                if (this.FFolderNameTemplate != value)
                {
                    this.FFolderNameTemplate = string.IsNullOrEmpty(value) ? "{0}" : value;
                    this.InvalidateListView(false);
                }
            }
        }

        [Browsable(false)]
        public History<IVirtualFolder> History
        {
            get
            {
                if (this.FHistory == null)
                {
                    this.FHistory = new History<IVirtualFolder>();
                    this.FHistory.MaxBackDepth = VirtualFilePanelSettings.Default.MaxBackDepth;
                    this.FHistory.MaxForwardDepth = VirtualFilePanelSettings.Default.MaxForwardDepth;
                }
                return this.FHistory;
            }
        }

        [Browsable(false)]
        public bool IsContentInitialized
        {
            get
            {
                return ((this.FCurrentFolder != null) && (this.FItems != null));
            }
        }

        [DefaultValue(false)]
        public bool IsFolderLocked
        {
            get
            {
                return this.CheckPanelState(PanelState.FolderLocked);
            }
            set
            {
                if (this.IsFolderLocked != value)
                {
                    this.SetPanelState(PanelState.FolderLocked, value);
                    this.tsbUnlockFolder.Enabled = this.IsFolderLocked;
                }
            }
        }

        private bool IsLayoutSuspended
        {
            get
            {
                return ((this.IsLayoutSuspended() || (base.Parent == null)) && !this.CheckPanelState(PanelState.ProcessingOnLayout));
            }
        }

        private bool IsThumbnailView
        {
            get
            {
                return ((this.listView.View == System.Windows.Forms.View.LargeIcon) && (this.listView.LargeImageList == this.imgThumbnail));
            }
        }

        [Browsable(false)]
        public IEnumerable<IVirtualItem> Items
        {
            get
            {
                if (this.listView.VirtualMode)
                {
                    return this.FItems;
                }
                return ((this.listView.Items.Count == 0) ? null : this.GetListItems());
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "Empty")]
        public System.Drawing.Color ListActiveBackColor
        {
            get
            {
                return this.FListActiveBackColor;
            }
            set
            {
                if (this.FListActiveBackColor != value)
                {
                    this.FListActiveBackColor = value;
                    if (this.CheckPanelState(PanelState.LastContainsFocus))
                    {
                        this.listView.BackColor = this.FListActiveBackColor.IsEmpty ? this.FListBackColor : this.FListActiveBackColor;
                    }
                    if (this.IsThumbnailView && (this.imgThumbnail.ColorDepth != ColorDepth.Depth32Bit))
                    {
                        this.ClearThumbnails();
                        if (this.listView.Visible)
                        {
                            this.InvalidateListView(true);
                        }
                    }
                }
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "Window")]
        public System.Drawing.Color ListBackColor
        {
            get
            {
                return this.FListBackColor;
            }
            set
            {
                if (this.FListBackColor != value)
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberListBackColor = System.Drawing.Color.Empty;
                    }
                    bool visible = false;
                    this.FListBackColor = value;
                    if (!(!this.FListActiveBackColor.IsEmpty && this.CheckPanelState(PanelState.LastContainsFocus)))
                    {
                        visible = this.listView.Visible && (this.listView.BackColor != this.FListBackColor);
                        this.listView.BackColor = this.FListBackColor;
                    }
                    this.treeView.BackColor = this.FListBackColor;
                    if (this.IsThumbnailView && (this.imgThumbnail.ColorDepth != ColorDepth.Depth32Bit))
                    {
                        this.ClearThumbnails();
                        visible = this.listView.Visible;
                    }
                    if (visible)
                    {
                        this.InvalidateListView(true);
                    }
                }
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "Silver")]
        public System.Drawing.Color ListFocusedBackColor
        {
            get
            {
                return this.FFocusedBackColor;
            }
            set
            {
                this.FFocusedBackColor = value;
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "Empty")]
        public System.Drawing.Color ListFocusedForeColor
        {
            get
            {
                return this.FFocusedForeColor;
            }
            set
            {
                this.FFocusedForeColor = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font ListFont
        {
            get
            {
                return this.listView.Font;
            }
            set
            {
                this.listView.Font = value;
                this.UpdateListView();
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "WindowText")]
        public System.Drawing.Color ListForeColor
        {
            get
            {
                return this.listView.ForeColor;
            }
            set
            {
                if (this.listView.ForeColor != value)
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberListForeColor = System.Drawing.Color.Empty;
                    }
                    this.ClearListViewCache();
                    this.listView.ForeColor = value;
                    this.treeView.ForeColor = value;
                }
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "Red")]
        public System.Drawing.Color ListSelectedForeColor
        {
            get
            {
                return this.FSelectedForeColor;
            }
            set
            {
                if (this.FSelectedForeColor != value)
                {
                    this.FSelectedForeColor = value;
                    if ((this.FSelection.Count > 0) && this.listView.Visible)
                    {
                        int num3;
                        if (this.listView.VirtualMode)
                        {
                            int startIndex = -1;
                            int endIndex = 0;
                            for (num3 = 0; num3 < this.FItems.Count; num3++)
                            {
                                if (this.FSelection.Contains(this.FItems[num3]))
                                {
                                    if (startIndex < 0)
                                    {
                                        startIndex = num3;
                                    }
                                    endIndex = num3;
                                }
                            }
                            if (startIndex >= 0)
                            {
                                this.ClearListViewCache();
                                this.listView.RedrawItems(startIndex, endIndex, true);
                            }
                        }
                        else
                        {
                            this.BeginListViewUpdate(true, false);
                            try
                            {
                                for (num3 = 0; num3 < this.listView.Items.Count; num3++)
                                {
                                    ListViewItem item = this.listView.Items[num3];
                                    if (this.FSelection.Contains((IVirtualItem) item.Tag))
                                    {
                                        item.ForeColor = this.FSelectedForeColor;
                                    }
                                }
                            }
                            finally
                            {
                                this.EndListViewUpdate();
                            }
                        }
                    }
                }
            }
        }

        [DefaultValue(3)]
        public int ListViewColumnCount
        {
            get
            {
                return this.FListViewColumnCount;
            }
            set
            {
                if ((value != this.FListViewColumnCount) && ((value == -1) || (value > 0)))
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberListViewCount = null;
                    }
                    this.FListViewColumnCount = value;
                    this.UpdateListView();
                }
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "Empty")]
        public System.Drawing.Color OddLineBackColor
        {
            get
            {
                return this.FOddLineBackColor;
            }
            set
            {
                if (!(this.FOddLineBackColor == value))
                {
                    this.FOddLineBackColor = value;
                    if ((this.listView.View == System.Windows.Forms.View.Details) && this.listView.Visible)
                    {
                        this.InvalidateListView(false);
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelContentContainer PanelContent
        {
            get
            {
                return this.GetPanelContent(false);
            }
            set
            {
                if (value == null)
                {
                    this.Filter = null;
                    this.Sort = VirtualItemComparer.DefaultSort;
                }
                else
                {
                    IVirtualFolder fCurrentFolder = this.FCurrentFolder;
                    try
                    {
                        this.FCurrentFolder = null;
                        this.IsFolderLocked = value.Locked;
                        this.Filter = value.Filter;
                        this.Sort = value.Sort;
                        this.FindOptions = value.QuickFindOptions;
                    }
                    finally
                    {
                        this.FCurrentFolder = fCurrentFolder;
                    }
                    this.FLazyFolder = value.Folder;
                    if (base.Visible && base.IsHandleCreated)
                    {
                        this.RestoreFromLazyFolder();
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Nomad.Configuration.PanelLayout PanelLayout
        {
            get
            {
                return this.GetPanelLayout(false, PanelLayoutEntry.None);
            }
            set
            {
                if ((value.StoreEntry & PanelLayoutEntry.FolderBarVisible) > PanelLayoutEntry.None)
                {
                    this.SetFolderBarVisible(value.FolderBarVisible, false);
                }
                if ((value.StoreEntry & PanelLayoutEntry.FolderBarOrientation) > PanelLayoutEntry.None)
                {
                    this.FolderBarOrientation = value.FolderBarOrientation;
                    this.SplitterPercent = value.SplitterPercent;
                }
                if ((value.StoreEntry & PanelLayoutEntry.View) > PanelLayoutEntry.None)
                {
                    this.View = value.View;
                    this.RememberView = null;
                }
                if ((value.StoreEntry & PanelLayoutEntry.Columns) > PanelLayoutEntry.None)
                {
                    this.AutoSizeColumns = value.AutoSizeColumns;
                    this.RememberAutoSizeColumns = null;
                    this.Columns = value.Columns;
                    this.RememberColumnInfoMap = null;
                }
                if ((value.StoreEntry & PanelLayoutEntry.ListColumnCount) > PanelLayoutEntry.None)
                {
                    this.ListViewColumnCount = value.ListColumnCount;
                    this.RememberListViewCount = null;
                }
                if ((value.StoreEntry & PanelLayoutEntry.ThumbnailSize) > PanelLayoutEntry.None)
                {
                    this.ThumbnailSize = value.ThumbnailSize;
                    this.ThumbnailSpacing = value.ThumbnailSpacing;
                    this.RememberThumbnailSize = Size.Empty;
                    this.RememberThumbnailSpacing = Size.Empty;
                }
                if ((value.StoreEntry & PanelLayoutEntry.ToolbarsVisible) > PanelLayoutEntry.None)
                {
                    this.ToolbarsVisible = value.ToolbarsVisible;
                }
            }
        }

        [Browsable(false)]
        public IVirtualFolder ParentFolder
        {
            get
            {
                return ((this.CurrentFolder != null) ? this.CurrentFolder.Parent : null);
            }
        }

        [DefaultValue(false)]
        public bool SaveSettings
        {
            get
            {
                return this.CheckPanelState(PanelState.SaveSettings);
            }
            set
            {
                this.SetPanelState(PanelState.SaveSettings, value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue((string) null)]
        public ICollection<IVirtualItem> Selection
        {
            get
            {
                return ((this.FSelection.Count > 0) ? this.FSelection : null);
            }
            set
            {
                bool flag = (this.FSelection.Count > 0) ^ (value != null);
                this.FSelection.Clear();
                if (value != null)
                {
                    foreach (IVirtualItem item in value)
                    {
                        if (this.FItems.Contains(item))
                        {
                            this.FSelection.Add(item);
                        }
                    }
                }
                if (flag)
                {
                    this.OnSelectionChanged(EventArgs.Empty);
                    this.InvalidateListView(false);
                }
            }
        }

        [Browsable(false)]
        public IEnumerable<IVirtualItem> SelectionOrFocused
        {
            get
            {
                if (this.FSelection.Count > 0)
                {
                    return this.FSelection;
                }
                IVirtualItem focusedItem = this.FocusedItem;
                if (!((focusedItem == null) || focusedItem.Equals(this.ParentFolder)))
                {
                    return new IVirtualItem[] { focusedItem };
                }
                return null;
            }
        }

        public string SettingsKey
        {
            get
            {
                return (string.IsNullOrEmpty(this.FSettings.SettingsKey) ? base.Name : this.FSettings.SettingsKey);
            }
            set
            {
                this.FSettings.SettingsKey = value;
            }
        }

        [DefaultValue(true)]
        public bool ShowItemIcons
        {
            get
            {
                return (this.CheckPanelState(PanelState.ShowItemIcons) && !SettingsManager.CheckSafeMode(SafeMode.DisableIcons));
            }
            set
            {
                if (this.ShowItemIcons != value)
                {
                    this.SetPanelState(PanelState.ShowItemIcons, value);
                    if (this.ShowItemIcons)
                    {
                        if (this.listView.LargeImageList != this.imgThumbnail)
                        {
                            this.listView.LargeImageList = ImageListCache.Get(ImageHelper.DefaultLargeIconSize);
                        }
                        this.UpdateImageLists();
                    }
                    else
                    {
                        IVirtualItem focusedItem = this.FocusedItem;
                        if (this.listView.LargeImageList != this.imgThumbnail)
                        {
                            this.listView.LargeImageList = null;
                        }
                        this.listView.SmallImageList = null;
                        this.FocusedItem = focusedItem;
                    }
                    this.treeView.ShowItemIcons = value;
                    this.tsPath.UpdatePath();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IComparer<IVirtualItem> Sort
        {
            get
            {
                return this.FSort;
            }
            set
            {
                if ((value != null) && !value.Equals(this.FSort))
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberSort = null;
                    }
                    this.FSort = value;
                    if ((this.FItems != null) && (this.FItems.Count > 0))
                    {
                        IVirtualItem focusedItem = this.FocusedItem;
                        bool flag = (this.ParentFolder != null) && this.FItems.Remove(this.ParentFolder);
                        this.ClearListViewCache();
                        this.FItems.Sort(this.Sort);
                        if (flag)
                        {
                            this.FItems.Insert(0, this.ParentFolder);
                        }
                        this.FocusedItem = focusedItem;
                        this.InvalidateListView(false);
                    }
                    if (this.listView.View == System.Windows.Forms.View.Details)
                    {
                        VirtualItemComparer fSort = this.FSort as VirtualItemComparer;
                        if (fSort != null)
                        {
                            foreach (ColumnHeader header in this.listView.Columns)
                            {
                                ListViewColumnInfo tag = header.Tag as ListViewColumnInfo;
                                if ((tag != null) && (tag.PropertyId == fSort.ComparePropertyId))
                                {
                                    this.listView.SetSortColumn(header.Index, fSort.SortDirection);
                                    return;
                                }
                            }
                        }
                        this.listView.SortColumn = -1;
                    }
                }
            }
        }

        protected int SplitterPercent
        {
            get
            {
                int num;
                if (this.NewSplitterPercent >= 0)
                {
                    return this.NewSplitterPercent;
                }
                if (this.splitContainer.Orientation == Orientation.Vertical)
                {
                    num = this.splitContainer.Panel1.Width + this.splitContainer.Panel2.Width;
                }
                else
                {
                    num = this.splitContainer.Panel1.Height + this.listView.Height;
                }
                int num2 = (int) Math.Round((double) ((this.splitContainer.SplitterDistance * 1000.0) / ((double) num)));
                return (((num2 < 0) || (num2 > 0x3e8)) ? -1 : num2);
            }
            set
            {
                if ((value >= 0) && (value <= 0x3e8))
                {
                    if (this.IsLayoutSuspended)
                    {
                        this.NewSplitterPercent = value;
                    }
                    else
                    {
                        int num;
                        if (this.splitContainer.Orientation == Orientation.Vertical)
                        {
                            num = this.splitContainer.Panel1.Width + this.splitContainer.Panel2.Width;
                        }
                        else
                        {
                            num = this.splitContainer.Panel1.Height + this.listView.Height;
                        }
                        try
                        {
                            this.splitContainer.SplitterDistance = (int) Math.Round((double) (((double) (value * num)) / 1000.0));
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }
                }
            }
        }

        public ICollection<IVirtualItem> StoredSelection { get; private set; }

        public Size ThumbnailSize
        {
            get
            {
                return this.imgThumbnail.ImageSize;
            }
            set
            {
                if ((this.imgThumbnail.ImageSize != value) && !value.IsEmpty)
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberThumbnailSize = Size.Empty;
                    }
                    this.ClearThumbnails();
                    this.imgThumbnail.ImageSize = value;
                    if (this.IsThumbnailView && this.listView.Visible)
                    {
                        this.InvalidateListView(false);
                    }
                }
            }
        }

        public Size ThumbnailSpacing
        {
            get
            {
                return this.FThumbnailSpacing;
            }
            set
            {
                if ((this.FThumbnailSpacing != value) && !value.IsEmpty)
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberThumbnailSpacing = Size.Empty;
                    }
                    if (value.Width < this.imgThumbnail.ImageSize.Width)
                    {
                        value.Width = -1;
                    }
                    if (value.Height < this.imgThumbnail.ImageSize.Height)
                    {
                        value.Height = -1;
                    }
                    this.FThumbnailSpacing = value;
                    if (this.IsThumbnailView)
                    {
                        this.ApplyThumbnailSpacing(ref this.FThumbnailSpacing);
                    }
                }
            }
        }

        protected PanelToolbar ToolbarsVisible
        {
            get
            {
                if (base.Visible)
                {
                    this.FToolbarsVisible = ((this.tsFolderInfo.Visible ? PanelToolbar.Folder : PanelToolbar.None) | (this.tsItemInfo.Visible ? PanelToolbar.Item : PanelToolbar.None)) | (this.tsFind.Visible ? PanelToolbar.Find : PanelToolbar.None);
                }
                return this.FToolbarsVisible;
            }
            set
            {
                this.FToolbarsVisible = value;
                this.tsFolderInfo.Visible = (this.FToolbarsVisible & PanelToolbar.Folder) > PanelToolbar.None;
                this.tsItemInfo.Visible = (this.FToolbarsVisible & PanelToolbar.Item) > PanelToolbar.None;
                this.tsFind.Visible = (this.FToolbarsVisible & PanelToolbar.Find) > PanelToolbar.None;
            }
        }

        [DefaultValue(3)]
        public PanelView View
        {
            get
            {
                if (this.IsThumbnailView)
                {
                    return PanelView.Thumbnail;
                }
                return (PanelView) this.listView.View;
            }
            set
            {
                if (this.View != value)
                {
                    if (!this.CheckPanelState(PanelState.ProcessingCustomizeFolder))
                    {
                        this.RememberDesktopIniPath = null;
                        this.RememberView = null;
                    }
                    this.ClearListViewCache();
                    this.BeginListViewUpdate(value == PanelView.Details, false);
                    try
                    {
                        this.ClearThumbnails();
                        switch (value)
                        {
                            case PanelView.LargeIcon:
                                this.listView.LargeImageList = this.ShowItemIcons ? ImageListCache.Get(ImageHelper.DefaultLargeIconSize) : null;
                                this.listView.SetIconSpacing(-1, -1);
                                break;

                            case PanelView.Details:
                                this.RecreateColumns();
                                break;

                            case PanelView.Thumbnail:
                                this.listView.LargeImageList = this.imgThumbnail;
                                this.ApplyThumbnailSpacing(ref this.FThumbnailSpacing);
                                break;
                        }
                        this.listView.View = (value == PanelView.Thumbnail) ? System.Windows.Forms.View.LargeIcon : ((System.Windows.Forms.View) value);
                        this.UpdateImageLists();
                        this.UpdateListView();
                    }
                    finally
                    {
                        this.EndListViewUpdate();
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetListItems>d__0 : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public VirtualFilePanel <>4__this;
            public IEnumerator <>7__wrap2;
            public IDisposable <>7__wrap3;
            private int <>l__initialThreadId;
            public ListViewItem <NextListItem>5__1;

            [DebuggerHidden]
            public <GetListItems>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                this.<>7__wrap3 = this.<>7__wrap2 as IDisposable;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap2 = this.<>4__this.listView.Items.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap2.MoveNext())
                            {
                                this.<NextListItem>5__1 = (ListViewItem) this.<>7__wrap2.Current;
                                this.<>2__current = (IVirtualItem) this.<NextListItem>5__1.Tag;
                                this.<>1__state = 2;
                                return true;
                            Label_0088:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally4();
                            break;

                        case 2:
                            goto Label_0088;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IVirtualItem> IEnumerable<IVirtualItem>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new VirtualFilePanel.<GetListItems>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.IVirtualItem>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            IVirtualItem IEnumerator<IVirtualItem>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetPanelItems>d__a : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public bool <>3__skipUpFolderItemCheck;
            public VirtualFilePanel <>4__this;
            public IEnumerator<IVirtualItem> <>7__wrapd;
            private int <>l__initialThreadId;
            public bool <IsUpFolderItem>5__c;
            public IVirtualItem <NextItem>5__b;
            public bool skipUpFolderItemCheck;

            [DebuggerHidden]
            public <GetPanelItems>d__a(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallye()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapd != null)
                {
                    this.<>7__wrapd.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrapd = this.<>4__this.Items.AsEnumerable<IVirtualItem>().GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrapd.MoveNext())
                            {
                                this.<NextItem>5__b = this.<>7__wrapd.Current;
                                this.<IsUpFolderItem>5__c = false;
                                if (!this.skipUpFolderItemCheck && (this.<IsUpFolderItem>5__c = this.<NextItem>5__b.Equals(this.<>4__this.ParentFolder)))
                                {
                                    goto Label_00B9;
                                }
                                this.<>2__current = this.<NextItem>5__b;
                                this.<>1__state = 2;
                                return true;
                            Label_00B2:
                                this.<>1__state = 1;
                            Label_00B9:
                                this.skipUpFolderItemCheck |= this.<IsUpFolderItem>5__c;
                            }
                            this.<>m__Finallye();
                            break;

                        case 2:
                            goto Label_00B2;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IVirtualItem> IEnumerable<IVirtualItem>.GetEnumerator()
            {
                VirtualFilePanel.<GetPanelItems>d__a _a;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    _a = this;
                }
                else
                {
                    _a = new VirtualFilePanel.<GetPanelItems>d__a(0) {
                        <>4__this = this.<>4__this
                    };
                }
                _a.skipUpFolderItemCheck = this.<>3__skipUpFolderItemCheck;
                return _a;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.IVirtualItem>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallye();
                        }
                        break;
                }
            }

            IVirtualItem IEnumerator<IVirtualItem>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetVisibleColumns>d__1e : IEnumerable<ListViewColumnInfo>, IEnumerable, IEnumerator<ListViewColumnInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ListViewColumnInfo <>2__current;
            public VirtualFilePanel <>4__this;
            public IEnumerator<ColumnHeader> <>7__wrap20;
            private int <>l__initialThreadId;
            public ColumnHeader <NextHeader>5__1f;

            [DebuggerHidden]
            public <GetVisibleColumns>d__1e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally21()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap20 != null)
                {
                    this.<>7__wrap20.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap20 = this.<>4__this.listView.GetOrderedColumns().GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap20.MoveNext())
                            {
                                this.<NextHeader>5__1f = this.<>7__wrap20.Current;
                                this.<>2__current = (ListViewColumnInfo) this.<NextHeader>5__1f.Tag;
                                this.<>1__state = 2;
                                return true;
                            Label_0080:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally21();
                            break;

                        case 2:
                            goto Label_0080;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ListViewColumnInfo> IEnumerable<ListViewColumnInfo>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new VirtualFilePanel.<GetVisibleColumns>d__1e(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Configuration.ListViewColumnInfo>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally21();
                        }
                        break;
                }
            }

            ListViewColumnInfo IEnumerator<ListViewColumnInfo>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        private enum ClickSelectMode
        {
            None,
            Select,
            Unselect
        }

        private enum ContextMenuSource
        {
            Default,
            Mouse,
            Ignore
        }

        public enum ListViewSort
        {
            None,
            Fast,
            Full
        }

        [Flags]
        private enum ListViewUpdateAction
        {
            EndUpdate = 1,
            Invalidate = 4,
            RecreateColumns = 8,
            Refresh = 0x20,
            ResumeLayout = 2,
            UpdateListView = 0x10
        }

        [Flags]
        private enum PanelState
        {
            BindingsNeeded = 0x200000,
            DoFolderChangedTick = 0x2000,
            FolderChangePending = 0x4000,
            FolderLocked = 0x800,
            HasRememberFilter = 8,
            LastContainsFocus = 2,
            LastParentFormActive = 0x40,
            ParentFormResizing = 0x80,
            PopulatingItems = 0x80000,
            ProcessDesktopIniNeeded = 0x40000,
            ProcessingCustomizeFolder = 0x20,
            ProcessingEndUpdate = 0x10,
            ProcessingOnLayout = 0x10000,
            ProgressUpdatePending = 0x20000,
            RedrawDisabled = 0x100,
            SaveSettings = 4,
            ShowItemIcons = 1,
            SkipKeyboardTooltip = 0x400,
            SkipMouseTooltip = 0x200,
            UpdateFocusSelectionNeeded = 0x100000,
            UseDragOverOptimization = 0x8000,
            UseFocusSelection = 0x1000
        }
    }
}

