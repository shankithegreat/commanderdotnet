namespace Nomad
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Microsoft.Win32.Network;
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.Controls;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Actions;
    using Nomad.Controls.Specialized;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Archive.Wcx;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Property.Providers;
    using Nomad.FileSystem.Property.Providers.Wdx;
    using Nomad.FileSystem.Special;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using Nomad.Themes;
    using Nomad.Workers;
    using Nomad.Workers.Configuration;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class MainForm : FormEx, IUpdateCulture
    {
        private static MainForm _Instance;
        private Action actAbout;
        private Action actAddFolderToRecent;
        private Action actAdvancedFilter;
        public Action actBack;
        private Action actBookmarkCurrentFolder;
        private Action actBookmarkCurrentTab;
        public Action actBringToFront;
        private Action actCalculateOnDemandProperties;
        private Action actChangeDrive;
        private Action actChangeDriveLeft;
        private Action actChangeDriveRight;
        public Action actChangeFolder;
        private Action actChangeView;
        private Action actCheckForUpdates;
        private Action actClearFilter;
        private Action actCloseOtherTabs;
        private Action actCloseTab;
        private Action actCloseTabsToRight;
        private Action actCompareFolders;
        private Action actCopy;
        public Action actCopyCurrentFolderAsText;
        private Action actCopyDetailsAsCSV;
        private Action actCopyFullNameAsText;
        private Action actCopyNameAsText;
        private Action actCopyToClipboard;
        private Action actCustomizeFolder;
        private Action actCustomizeToolbars;
        private Action actCustomizeTools;
        private Action actCutToClipboard;
        private Action actDelete;
        private Action actDeleteSingleItem;
        private Action actDisconnectNetworkDrive;
        private Action actDuplicateTab;
        private Action actEditDescription;
        private Action actEditItem;
        private Action actEmptyClipboard;
        private Action actEqualizePanels;
        private Action actExit;
        private Action actFind;
        private Action actFolderBranch;
        public Action actForward;
        private Action actFtpConnect;
        private Action actGCCollect;
        private Action actGoToParent;
        private Action actGoToRoot;
        private Action actHelpContents;
        private Action actInvertEntireSelection;
        private Action actInvertSelection;
        private ActionManager actionManager;
        private Action actLeftPanelToRight;
        private Action actLockFolderChange;
        private Action actMakeFolder;
        private Action actMakeLink;
        private Action actManageColumns;
        private Action actManageLayouts;
        private Action actMapNetworkDrive;
        private Action actMinimizeToTray;
        private Action actMoveToEighthTab;
        private Action actMoveToFifthTab;
        private Action actMoveToFirstTab;
        private Action actMoveToFourthTab;
        private Action actMoveToLastTab;
        private Action actMoveToNextTab;
        private Action actMoveToPreviousTab;
        private Action actMoveToSecondTab;
        private Action actMoveToSeventhTab;
        private Action actMoveToSixthTab;
        private Action actMoveToThirdTab;
        private Action actNavigationLink;
        private Action actNewFile;
        private Action actOnePanel;
        private Action actOpen;
        private Action actOpenAsArchive;
        private Action actOpenContainingFolder;
        private Action actOpenInFarPanel;
        private Action actOpenOutside;
        private Action actOpenRecentFolders;
        private Action actOptions;
        private Action actOrganizeBookmarks;
        private Action actPack;
        private Action actPasteFromClipboard;
        private Action actPasteShortCut;
        private Action actQuickChangeFolder;
        public Action actRefresh;
        private Action actRefreshToolbars;
        private Action actRenameMove;
        private Action actRenameSingleItem;
        private Action actRenameTab;
        private Action actResetVisualCache;
        private Action actRestoreSelection;
        private Action actRightPanelToLeft;
        private Action actRunAs;
        private Action actRunAsAdmin;
        private Action actSaveCurrentLayout;
        private Action actSaveSettings;
        private Action actSelect;
        private Action actSelectAll;
        private Action actSelectByExtension;
        private Action actSelectByName;
        private Action actSelectSingleItem;
        private Action actSelectSingleItemAndCalculate;
        private Action actSelectSort;
        private Action actSetAttributes;
        private Action actSetEightListColumns;
        private Action actSetFiveListColumns;
        private Action actSetFourListColumns;
        private Action actSetNineListColumns;
        private Action actSetOneListColumn;
        private Action actSetSevenListColumns;
        private Action actSetSixListColumns;
        private Action actSetThreeListColumns;
        private Action actSetTwoListColumns;
        private Action actShowBookmarks;
        private Action actShowCmdLineHelp;
        private Action actShowProperties;
        private Action actSortByExtension;
        private Action actSortByLastWriteTime;
        private Action actSortByName;
        private Action actSortBySize;
        private Action actSortDescending;
        private Action actSwapPanels;
        private Action actToggleFolderBar;
        private Action actToggleOnePanelMode;
        private Action actToggleQuickFind;
        private Action actTwoHorizontalPanel;
        private Action actTwoVerticalPanel;
        private Action actUnselect;
        private Action actUnselectAll;
        private Action actUnselectByExtension;
        private Action actViewAsDetails;
        private Action actViewAsLargeIcon;
        private Action actViewAsList;
        private Action actViewAsSmallIcon;
        private Action actViewAsThumbnail;
        private Action actViewItem;
        private Action actVolumeLabel;
        private IDictionary<APPCOMMAND, Action> AppCommandActionMap;
        private FileSystemWatcher BookmarksWatcher;
        private Category catBookmarks;
        private Category catEdit;
        public CategoryManager categoryManager;
        private Category catFile;
        private Category catMisc;
        private Category catPanel;
        private Category catTab;
        private Category catView;
        private int ClipbdTickCount;
        private ClipboardState ClipbrdMask;
        private ClipboardState ClipbrdState;
        private ContextMenuStrip cmsMenuFilter;
        private ContextMenuStrip cmsMenuFind;
        private ContextMenuStrip cmsMenuNew;
        private ContextMenuStrip cmsMenuSort;
        private ContextMenuStrip cmsMenuTab;
        private ContextMenuStrip cmsMenuViewAs;
        private ContextMenuStrip cmsMenuWindowLayout;
        private ContextMenuStrip cmsTab;
        private ContextMenuStrip cmsToolbar;
        private ContextMenuStrip cmsTray;
        private Dictionary<ToolStripItem, DragDropEffects> CommandDropMap;
        private IContainer components = null;
        private Keys ControlModifierKeys;
        private IDictionary<Action, ToolStripDropDown> FActionDropDownMap;
        private IDictionary<string, Action> FActionMap;
        private HashSet<Action> FDroppableActions;
        private OpenFileDialog FindExeFileDialog;
        private Tab FPreviousTab;
        private Regex FToolbarCommandRegex;
        private UIState FUpdateUIMask;
        private ImageList largeImageList;
        private MenuStrip MainMenu;
        private TabPageSwitcher MainPageSwitcher;
        private TabStrip MainTabStrip;
        private const string NamePrefixAction = "Action_";
        private const string NamePrefixAllBookmarks = "Bookmark_All_";
        private const string NamePrefixAllDrives = "Drive_All_";
        private const string NamePrefixAllTools = "Tool_All_";
        private const string NamePrefixBookmark = "Bookmark_";
        private const string NamePrefixDrive = "Drive_";
        private const string NamePrefixDropDown = "DropDown_";
        private const string NamePrefixSeparator = "Separator_";
        private const string NamePrefixTool = "Tool_";
        private const string NamePrefixToolbar = "Toolbar_";
        private FormSettings PlacementSettings;
        private ImageList smallImageList;
        private IDictionary<Keys, IComponent> SpecialKeyMap = new Dictionary<Keys, IComponent>();
        private const string TabBookmarkExt = ".tab";
        private const int tagKeyIconLocation = 2;
        private const int tagKeyToolPath = 1;
        private FileSystemWatcher ToolsWatcher;
        private ToolTip toolTipDefault;
        private NotifyIcon TrayIcon;
        private ToolStripButton tsbCloseTab;
        private ToolStripMenuItem tsmiAbout;
        private ToolStripMenuItem tsmiAbout2;
        private ToolStripMenuItem tsmiBack;
        private ToolStripMenuItem tsmiBookmarkCurrentFolder;
        private ToolStripMenuItem tsmiBookmarkCurrentTab;
        private ToolStripMenuItem tsmiBookmarks;
        private ToolStripMenuItem tsmiBringToFront;
        private ToolStripMenuItem tsmiCalculateFolderSizes;
        private ToolStripMenuItem tsmiChangeButtonImage;
        private ToolStripMenuItem tsmiChangeDrive;
        private ToolStripMenuItem tsmiChangeFolder;
        private ToolStripMenuItem tsmiCheckForUpdates;
        private ToolStripMenuItem tsmiClearFilter;
        private ToolStripMenuItem tsmiCloseOtherTabs;
        private ToolStripMenuItem tsmiCloseOtherTabs2;
        private ToolStripMenuItem tsmiCloseTab;
        private ToolStripMenuItem tsmiCloseTab2;
        private ToolStripMenuItem tsmiCloseTabsToRight2;
        private ToolStripMenuItem tsmiColumns;
        private ToolStripMenuItem tsmiCompareFolders;
        private ToolStripMenuItem tsmiCopy;
        private ToolStripMenuItem tsmiCopyDetailsAsCSV;
        private ToolStripMenuItem tsmiCopyFullNameAsText;
        private ToolStripMenuItem tsmiCopyNameAsText;
        private ToolStripMenuItem tsmiCopyToClipboard;
        private ToolStripMenuItem tsmiCustomizeFolder;
        private ToolStripMenuItem tsmiCustomizeToolbars1;
        private ToolStripMenuItem tsmiCustomizeToolbars2;
        private ToolStripMenuItem tsmiCustomizeTools1;
        private ToolStripMenuItem tsmiCustomizeTools2;
        private ToolStripMenuItem tsmiCutToClipboard;
        private ToolStripMenuItem tsmiDelete;
        private ToolStripMenuItem tsmiDeleteSingleItem;
        private ToolStripMenuItem tsmiDuplicateTab;
        private ToolStripMenuItem tsmiDuplicateTab2;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripMenuItem tsmiEditDescription;
        private ToolStripMenuItem tsmiEditItem;
        private ToolStripMenuItem tsmiEmpty;
        private ToolStripMenuItem tsmiEmptyClipboard;
        private ToolStripMenuItem tsmiEqualizePanels;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem tsmiExit2;
        private ToolStripMenuItem tsmiFile;
        private ToolStripMenuItem tsmiFilter;
        private ToolStripMenuItem tsmiFilterDialog;
        private ToolStripMenuItem tsmiFind;
        private ToolStripMenuItem tsmiFindDialog;
        private ToolStripMenuItem tsmiFolderBar;
        private ToolStripMenuItem tsmiFolderBarHidden;
        private ToolStripMenuItem tsmiFolderBarHorizontal;
        private ToolStripMenuItem tsmiFolderBarVertical;
        private ToolStripMenuItem tsmiFolderBranch;
        private ToolStripMenuItem tsmiFolderDesktop;
        private ToolStripMenuItem tsmiFolderFavorites;
        private ToolStripMenuItem tsmiFolderMyDocuments;
        private ToolStripMenuItem tsmiFolderMyMusic;
        private ToolStripMenuItem tsmiFolderMyPictures;
        private ToolStripMenuItem tsmiFolderSystem;
        private ToolStripMenuItem tsmiFolderTemp;
        private ToolStripMenuItem tsmiFolderWindows;
        private ToolStripMenuItem tsmiForward;
        private ToolStripMenuItem tsmiFtpConnect;
        private ToolStripMenuItem tsmiHelp;
        private ToolStripMenuItem tsmiHelpContents;
        private ToolStripMenuItem tsmiInvertSelection;
        private ToolStripMenuItem tsmiJustifyToolbar;
        private ToolStripMenuItem tsmiLockFolderChange;
        private ToolStripMenuItem tsmiMainMenuVisible;
        private ToolStripMenuItem tsmiMakeFolder;
        private ToolStripMenuItem tsmiMakeLink;
        private ToolStripMenuItem tsmiManageColumns;
        private ToolStripMenuItem tsmiManageColumns2;
        private ToolStripMenuItem tsmiManageLayouts;
        private ToolStripMenuItem tsmiNavigationLink;
        private ToolStripMenuItem tsmiNew;
        private ToolStripMenuItem tsmiNewFile;
        private ToolStripMenuItem tsmiNoStoredFilters;
        private ToolStripMenuItem tsmiNoStoredLayouts;
        private ToolStripMenuItem tsmiNoStoredSearches;
        private ToolStripMenuItem tsmiNoToolbars;
        private ToolStripMenuItem tsmiOnePanel;
        private ToolStripMenuItem tsmiOpenContainingFolder;
        private ToolStripMenuItem tsmiOpenInFarPanel;
        private ToolStripMenuItem tsmiOptions;
        private ToolStripMenuItem tsmiOrganizeBookmarks;
        private ToolStripMenuItem tsmiPack;
        private ToolStripMenuItem tsmiPanel;
        private ToolStripMenuItem tsmiPasteFromClipboard;
        private ToolStripMenuItem tsmiPasteShortcut;
        private ToolStripMenuItem tsmiRefresh;
        private ToolStripMenuItem tsmiRemoveToolbarButton;
        private ToolStripMenuItem tsmiRenameMove;
        private ToolStripMenuItem tsmiRenameSingleItem;
        private ToolStripMenuItem tsmiRenameTab;
        private ToolStripMenuItem tsmiRenameTab2;
        private ToolStripMenuItem tsmiRestoreSelection;
        private ToolStripMenuItem tsmiRunAs;
        private ToolStripMenuItem tsmiSaveCurrentLayout;
        private ToolStripMenuItem tsmiSelect;
        private ToolStripMenuItem tsmiSelectAll;
        private ToolStripMenuItem tsmiSelectSort;
        private ToolStripMenuItem tsmiSetAttributes;
        private ToolStripMenuItem tsmiSetEightListColumns;
        private ToolStripMenuItem tsmiSetEightListColumns2;
        private ToolStripMenuItem tsmiSetFiveListColumns;
        private ToolStripMenuItem tsmiSetFiveListColumns2;
        private ToolStripMenuItem tsmiSetFourListColumns;
        private ToolStripMenuItem tsmiSetFourListColumns2;
        private ToolStripMenuItem tsmiSetNineListColumns;
        private ToolStripMenuItem tsmiSetNineListColumns2;
        private ToolStripMenuItem tsmiSetOneListColumn;
        private ToolStripMenuItem tsmiSetOneListColumn2;
        private ToolStripMenuItem tsmiSetSevenListColumns;
        private ToolStripMenuItem tsmiSetSevenListColumns2;
        private ToolStripMenuItem tsmiSetSixListColumns;
        private ToolStripMenuItem tsmiSetSixListColumns2;
        private ToolStripMenuItem tsmiSetThreeListColumns;
        private ToolStripMenuItem tsmiSetThreeListColumns2;
        private ToolStripMenuItem tsmiSetTwoListColumns;
        private ToolStripMenuItem tsmiSetTwoListColumns2;
        private ToolStripMenuItem tsmiShowCmdLineHelp;
        private ToolStripMenuItem tsmiShowProperties;
        private ToolStripMenuItem tsmiSort;
        private ToolStripMenuItem tsmiSortByExtension;
        private ToolStripMenuItem tsmiSortByLastWriteTime;
        private ToolStripMenuItem tsmiSortByName;
        private ToolStripMenuItem tsmiSortBySize;
        private ToolStripMenuItem tsmiSortDescending;
        private ToolStripMenuItem tsmiSpecialFolders;
        private ToolStripMenuItem tsmiSwapPanels;
        private ToolStripMenuItem tsmiTab;
        private ToolStripMenuItem tsmiToolbarButtonImage;
        private ToolStripMenuItem tsmiToolbarButtonImageAndText;
        private ToolStripMenuItem tsmiToolbarButtonText;
        private ToolStripMenuItem tsmiToolbarMoveToBottom;
        private ToolStripMenuItem tsmiToolbarMoveToTop;
        private ToolStripMenuItem tsmiToolbars;
        private ToolStripMenuItem tsmiTools;
        private ToolStripMenuItem tsmiTwoHorizontalPanel;
        private ToolStripMenuItem tsmiTwoVerticalPanel;
        private ToolStripMenuItem tsmiUnselect;
        private ToolStripMenuItem tsmiView;
        private ToolStripMenuItem tsmiViewAs;
        private ToolStripMenuItem tsmiViewAsDetails;
        private ToolStripMenuItem tsmiViewAsDetails2;
        private ToolStripMenuItem tsmiViewAsLargeIcon;
        private ToolStripMenuItem tsmiViewAsLargeIcon2;
        private ToolStripMenuItem tsmiViewAsList;
        private ToolStripMenuItem tsmiViewAsList2;
        private ToolStripMenuItem tsmiViewAsSmallIcon;
        private ToolStripMenuItem tsmiViewAsSmallIcon2;
        private ToolStripMenuItem tsmiViewAsThumbnail;
        private ToolStripMenuItem tsmiViewAsThumbnail2;
        private ToolStripMenuItem tsmiViewItem;
        private ToolStripMenuItem tsmiWindowLayout;
        private ToolStripSeparator tssAdvancedFilter1;
        private ToolStripSeparator tssAdvancedFilter2;
        private ToolStripSeparator tssBookmarks1;
        private ToolStripSeparator tssEdit1;
        private ToolStripSeparator tssEdit2;
        private ToolStripSeparator tssEdit3;
        private ToolStripSeparator tssEdit4;
        private ToolStripSeparator tssFile1;
        private ToolStripSeparator tssFile2;
        private ToolStripSeparator tssFile3;
        private ToolStripSeparator tssFile4;
        private ToolStripSeparator tssFile5;
        private ToolStripSeparator tssFind;
        private ToolStripSeparator tssHelp1;
        private ToolStripSeparator tssHelp2;
        private ToolStripSeparator tssLayout1;
        private ToolStripSeparator tssNewFile;
        private ToolStripSeparator tssPanel1;
        private ToolStripSeparator tssPanel2;
        private ToolStripSeparator tssPanel3;
        private ToolStripSeparator tssPanel4;
        private ToolStripSeparator tssPanel5;
        private ToolStripSeparator tssPanel6;
        private ToolStripSeparator tssSort1;
        private ToolStripSeparator tssSort2;
        private ToolStripSeparator tssSpecialFolder1;
        private ToolStripSeparator tssSpecialFolder2;
        private ToolStripSeparator tssTab1;
        private ToolStripSeparator tssTab10;
        private ToolStripSeparator tssTab11;
        private ToolStripSeparator tssTab2;
        private ToolStripSeparator tssToolbar1;
        private ToolStripSeparator tssToolbar2;
        private ToolStripSeparator tssToolbar3;
        private ToolStripSeparator tssToolbar4;
        private ToolStripSeparator tssToolbar5;
        private ToolStripSeparator tssToolbar6;
        private ToolStripSeparator tssToolbar7;
        private ToolStripSeparator tssTools1;
        private ToolStripSeparator tssTray1;
        private ToolStripSeparator tssView1;
        private ToolStripSeparator tssView2;
        private ToolStripSeparator tssView3;
        private ToolStripSeparator tssView4;
        private ToolStripSeparator tssViewAs1;
        private int UniqueIndex;
        private EventHandler VolumeChangedHandler;
        private Dictionary<Process, WatchProcessInfo> WatchProcessMap;

        public MainForm()
        {
            this.InitializeComponent();
            BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
            if (argument != null)
            {
                argument.Localize(this);
            }
            else
            {
                this.UpdateCulture();
            }
            base.Icon = Resources.Camel;
            this.actSelectSingleItemAndCalculate.Tag = true;
            this.actViewAsThumbnail.Tag = PanelView.Thumbnail;
            this.actViewAsLargeIcon.Tag = PanelView.LargeIcon;
            this.actViewAsSmallIcon.Tag = PanelView.SmallIcon;
            this.actViewAsList.Tag = PanelView.List;
            this.actViewAsDetails.Tag = PanelView.Details;
            this.actSortByName.Tag = 0;
            this.actSortByExtension.Tag = 1;
            this.actSortByLastWriteTime.Tag = 8;
            this.actSortBySize.Tag = 3;
            this.actChangeDrive.Tag = TwoPanelContainer.SinglePanel.None;
            this.actChangeDriveLeft.Tag = TwoPanelContainer.SinglePanel.Left;
            this.actChangeDriveRight.Tag = TwoPanelContainer.SinglePanel.Right;
            this.actSetOneListColumn.Tag = 1;
            this.actSetTwoListColumns.Tag = 2;
            this.actSetThreeListColumns.Tag = 3;
            this.actSetFourListColumns.Tag = 4;
            this.actSetFiveListColumns.Tag = 5;
            this.actSetSixListColumns.Tag = 6;
            this.actSetSevenListColumns.Tag = 7;
            this.actSetEightListColumns.Tag = 8;
            this.actSetNineListColumns.Tag = 9;
            this.actMoveToNextTab.Tag = true;
            this.actMoveToFirstTab.Tag = 1;
            this.actMoveToSecondTab.Tag = 2;
            this.actMoveToThirdTab.Tag = 3;
            this.actMoveToFourthTab.Tag = 4;
            this.actMoveToFifthTab.Tag = 5;
            this.actMoveToSixthTab.Tag = 6;
            this.actMoveToSeventhTab.Tag = 7;
            this.actMoveToEighthTab.Tag = 8;
            this.tsmiFolderBarHorizontal.Tag = Orientation.Horizontal;
            this.tsmiFolderBarVertical.Tag = Orientation.Vertical;
            this.actHelpContents.Tag = HelpNavigator.TableOfContents;
            this.tsmiToolbarButtonImage.Tag = ToolStripItemDisplayStyle.Image;
            this.tsmiToolbarButtonImageAndText.Tag = ToolStripItemDisplayStyle.ImageAndText;
            this.tsmiToolbarButtonText.Tag = ToolStripItemDisplayStyle.Text;
            this.tsmiToolbarMoveToTop.Tag = DockStyle.Top;
            this.tsmiToolbarMoveToBottom.Tag = DockStyle.Bottom;
            this.tsmiView.Tag = this.tsmiColumns.DropDown;
            this.actOpen.ShortcutKeys = Keys.Return;
            this.actOpenAsArchive.ShortcutKeys = Keys.Shift | Keys.Return;
            this.actRunAs.ShortcutKeys = Keys.Control | Keys.Return;
            this.actRunAsAdmin.ShortcutKeys = Keys.Alt | Keys.Control | Keys.Shift | Keys.Return;
            this.actShowProperties.ShortcutKeys = Keys.Alt | Keys.Return;
            this.actSelect.ShortcutKeys = Keys.Add;
            this.actUnselect.ShortcutKeys = Keys.Subtract;
            this.actInvertSelection.ShortcutKeys = Keys.Multiply;
            this.actInvertEntireSelection.ShortcutKeys = Keys.Control | Keys.Multiply;
            this.actRestoreSelection.ShortcutKeys = Keys.Divide;
            this.actSelectByExtension.ShortcutKeys = Keys.Alt | Keys.Control | Keys.Add;
            this.actUnselectByExtension.ShortcutKeys = Keys.Alt | Keys.Control | Keys.Subtract;
            this.actGoToParent.Shortcuts = new Keys[] { Keys.Back, Keys.Control | Keys.Prior, Keys.Alt | Keys.Up };
            this.actGoToRoot.ShortcutKeys = Keys.Control | Keys.OemPipe;
            this.actionManager.SetAction(this.tsbCloseTab, this.actCloseTab, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Shortcuts | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
            this.actionManager.SetAction(this.tsmiFolderBarHidden, this.actToggleFolderBar, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Shortcuts | BindActionProperty.Visible | BindActionProperty.Enabled);
            GlobalHotKeyManager.RegisterAction(this.actBringToFront);
            this.actionManager.SetAction(this.tsmiAbout2, this.actAbout, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
            this.actionManager.SetAction(this.tsmiBringToFront, this.actBringToFront, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
            this.actionManager.SetAction(this.tsmiExit2, this.actExit, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
            this.InitializeSpecialFolders();
            this.InitializeDropDownMenuItems();
            this.actTwoHorizontalPanel.Tag = Orientation.Horizontal;
            this.actTwoVerticalPanel.Tag = Orientation.Vertical;
            if (OS.IsWinVista)
            {
                this.smallImageList.ImageSize = new Size(ImageHelper.DefaultSmallIconSize.Width, ImageHelper.DefaultSmallIconSize.Height + 3);
            }
            else
            {
                this.smallImageList.ImageSize = ImageHelper.DefaultSmallIconSize;
            }
            this.largeImageList.ImageSize = ImageHelper.DefaultLargeIconSize;
            this.PlacementSettings = FormSettings.RegisterForm(this, SettingsManager.CheckSafeMode(SafeMode.SkipFormPlacement) ? FormPlacement.None : ~FormPlacement.None);
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(this.SettingPropertyChanged);
            this.InitializeToolStrip(true);
            InitializePropertyList();
            VirtualIcon.IconOptions = Settings.Default.IconOptions;
            VirtualIcon.DelayedExtractMode = Settings.Default.DelayedExtractMode;
            FileSystemFolder.ProcessFolderShortcuts = Settings.Default.ProcessFolderShortcuts;
            CustomFileSystemFolder.SlowVolumeAutoRefresh = Settings.Default.SlowVolumeAutoRefresh;
            this.InitializeImages();
            this.InitializeToolbars();
            Settings.Default.DefaultKeyMap = this.KeyMap;
            this.InitializeKeyboardMap(Settings.Default.KeyboardMap);
            this.InitializeTabs();
            if (RestartAndRecoveryManager.IsSupported)
            {
                RestartAndRecoveryManager.RegisterRestart("-new");
                RestartAndRecoveryManager.RecoveryPingInterval = 0x7530;
                RestartAndRecoveryManager.Recovery += new EventHandler<RecoveryEventArgs>(this.OnRecovery);
                RestartAndRecoveryManager.RegisterRecovery();
            }
        }

        private void actAbout_OnExecute(object sender, ActionEventArgs e)
        {
            using (AboutDialog dialog = new AboutDialog())
            {
                if (base.WindowState != FormWindowState.Minimized)
                {
                    base.AddOwnedForm(dialog);
                }
                else
                {
                    dialog.ShowInTaskbar = true;
                    dialog.StartPosition = FormStartPosition.CenterScreen;
                }
                dialog.ShowDialog();
            }
        }

        private void actAddFolderToRecent_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualFolder onlyOneItem = GetOnlyOneItem<IVirtualFolder>(e.Target, this.CurrentPanel.CurrentFolder);
            if (onlyOneItem != null)
            {
                HistorySettings.Default.AddStringToChangeFolder(onlyOneItem.FullName);
            }
        }

        private void actAddFolderToRecent_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = GetOnlyOneItem<IVirtualFolder>(e.Target, this.CurrentPanel.CurrentFolder) != null;
        }

        private void actAdvancedFilter_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFilePanel currentPanel = this.CurrentPanel;
            DesktopIni desktopIni = VirtualItem.GetDesktopIni(currentPanel.CurrentFolder, false);
            bool rememberFilter = false;
            WaitCursor.ShowUntilIdle();
            using (FilterDialog dialog = new FilterDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.OnApplyFilter += new EventHandler<ApplyFilterEventArgs>(this.OnApplyFilter);
                dialog.Filter = currentPanel.Filter;
                dialog.Tag = currentPanel;
                dialog.RememberFilterEnabled = desktopIni != null;
                if (dialog.Execute(this))
                {
                    rememberFilter = dialog.RememberFilter;
                    currentPanel.Filter = dialog.Filter;
                }
            }
            if (rememberFilter)
            {
                try
                {
                    desktopIni.Read();
                    desktopIni.Filter = this.CurrentPanel.Filter;
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

        private void actBack_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).Back();
        }

        private void actBack_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = (panel.History.BackCount > 0) && !panel.IsFolderLocked;
        }

        private void actBookmarkCurrentFolder_OnExecute(object sender, ActionEventArgs e)
        {
            ICreateVirtualLink onlyOneItem = GetOnlyOneItem<ICreateVirtualLink>(e.Target, this.CurrentPanel.CurrentFolder);
            if (onlyOneItem != null)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(onlyOneItem.GetPrefferedLinkName(LinkType.Default));
                ICustomizeFolder customizeBookmark = null;
                Keys none = Keys.None;
                if (ConfirmationSettings.Default.BookmarkFolder ^ ((Control.ModifierKeys & Keys.Shift) > Keys.None))
                {
                    using (BookmarkFolderDialog dialog = new BookmarkFolderDialog())
                    {
                        base.AddOwnedForm(dialog);
                        dialog.BookmarkName = fileNameWithoutExtension;
                        dialog.DoNotShowAgain = !ConfirmationSettings.Default.BookmarkFolder;
                        dialog.PreviewHotKey += new EventHandler<PreviewHotKeyEventArgs>(this.PreviewHotKey);
                        if (dialog.Execute((IVirtualFolder) onlyOneItem))
                        {
                            fileNameWithoutExtension = dialog.BookmarkName;
                            customizeBookmark = dialog.CustomizeBookmark;
                            none = dialog.Hotkey;
                        }
                        else
                        {
                            fileNameWithoutExtension = null;
                        }
                        ConfirmationSettings.Default.BookmarkFolder = !dialog.DoNotShowAgain;
                    }
                }
                if (!string.IsNullOrEmpty(fileNameWithoutExtension))
                {
                    string str2 = fileNameWithoutExtension + Path.GetExtension(onlyOneItem.GetPrefferedLinkName(LinkType.Default));
                    MessageDialogResult yes = MessageDialogResult.Yes;
                    if (System.IO.File.Exists(Path.Combine(SettingsManager.SpecialFolders.Bookmarks, str2)))
                    {
                        yes = MessageDialog.Show(this, string.Format(Resources.sBookmarkAlreadyExists, fileNameWithoutExtension), Resources.sConfirmOverwriteBookmark, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question);
                    }
                    if (yes == MessageDialogResult.Yes)
                    {
                        IChangeVirtualItem item = onlyOneItem.CreateLink(this.BookmarksFolder, str2, LinkType.Default) as IChangeVirtualItem;
                        if (item.CanSetProperty(0x16) && (none != Keys.None))
                        {
                            item[0x16] = none;
                        }
                        if (item.CanSetProperty(0x17) && (customizeBookmark != null))
                        {
                            item[0x17] = customizeBookmark;
                        }
                    }
                }
            }
        }

        private void actBookmarkCurrentFolder_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = GetOnlyOneItem<ICreateVirtualLink>(e.Target, this.CurrentPanel.CurrentFolder) != null;
        }

        private void actBookmarkCurrentTab_OnExecute(object sender, ActionEventArgs e)
        {
            string text = this.CurrentTabContent.Text;
            if (string.IsNullOrEmpty(text) && (this.CurrentPanel.CurrentFolder != null))
            {
                text = this.CurrentPanel.CurrentFolder.Name;
            }
            Keys none = Keys.None;
            if (ConfirmationSettings.Default.BookmarkFolder ^ ((Control.ModifierKeys & Keys.Shift) > Keys.None))
            {
                using (BookmarkTabDialog dialog = new BookmarkTabDialog())
                {
                    base.AddOwnedForm(dialog);
                    dialog.BookmarkName = text;
                    dialog.DoNotShowAgain = !ConfirmationSettings.Default.BookmarkFolder;
                    dialog.PreviewHotKey += new EventHandler<PreviewHotKeyEventArgs>(this.PreviewHotKey);
                    if (dialog.Execute())
                    {
                        text = dialog.BookmarkName;
                        none = dialog.Hotkey;
                    }
                    else
                    {
                        text = null;
                    }
                    ConfirmationSettings.Default.BookmarkFolder = !dialog.DoNotShowAgain;
                }
            }
            if (!string.IsNullOrEmpty(text))
            {
                string path = Path.Combine(SettingsManager.SpecialFolders.Bookmarks, text + ".tab");
                MessageDialogResult yes = MessageDialogResult.Yes;
                if (System.IO.File.Exists(path))
                {
                    yes = MessageDialog.Show(this, string.Format(Resources.sBookmarkAlreadyExists, text), Resources.sConfirmOverwriteBookmark, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question);
                }
                if (yes == MessageDialogResult.Yes)
                {
                    GeneralTab tabBookmark = this.CurrentTabContent.TabBookmark;
                    if (none != Keys.None)
                    {
                        tabBookmark.Hotkey = none;
                    }
                    using (XmlWriter writer = XmlWriter.Create(path))
                    {
                        TwoPanelContainer.SerializeBookmark(writer, tabBookmark);
                    }
                }
            }
        }

        private void actBringToFront_OnExecute(object sender, ActionEventArgs e)
        {
            this.SetUIState(UIState.ProcessingTray, true);
            try
            {
                base.ShowInTaskbar = true;
                this.TrayIcon.Visible = false;
            }
            finally
            {
                this.SetUIState(UIState.ProcessingTray, false);
            }
            if (base.WindowState == FormWindowState.Minimized)
            {
                Windows.ShowWindow(base.Handle, SW.SW_RESTORE);
                base.Activate();
            }
            else
            {
                Form form = null;
                Stack<Form> stack = new Stack<Form>();
                stack.Push(this);
                while (stack.Count > 0)
                {
                    form = stack.Pop();
                    foreach (Form form2 in form.OwnedForms)
                    {
                        if (form2.Modal)
                        {
                            stack.Push(form2);
                            break;
                        }
                    }
                }
                form.Activate();
            }
        }

        private void actCalculateOnDemandProperties_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.Selection;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems == null)
            {
                allItems = this.CurrentPanel.Items;
            }
            if (allItems != null)
            {
                VirtualPropertySet set = new VirtualPropertySet(new int[] { 3 });
                if (this.CurrentPanel.View == PanelView.Details)
                {
                    foreach (ListViewColumnInfo info in this.CurrentPanel.GetVisibleColumns())
                    {
                        set[info.PropertyId] = true;
                    }
                }
                foreach (IVirtualItem item in allItems.Where<IVirtualItem>(delegate (IVirtualItem x) {
                    return !(x is IVirtualFolder) || !x.Equals(this.CurrentPanel.ParentFolder);
                }))
                {
                    foreach (int num in set)
                    {
                        if (item.GetPropertyAvailability(num) == PropertyAvailability.OnDemand)
                        {
                            object obj2 = item[num];
                        }
                    }
                }
                if (clearSelectionBeforeWork)
                {
                    this.CurrentPanel.PushSelection(true);
                }
            }
        }

        private void actChangeDrive_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.GetPanelFromMode((TwoPanelContainer.SinglePanel) e.Tag);
            if (panel != null)
            {
                panel.SelectDrive();
            }
        }

        private void actChangeDrive_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.GetPanelFromMode((TwoPanelContainer.SinglePanel) e.Tag);
            e.Enabled = (panel != null) && !panel.IsFolderLocked;
        }

        private void actChangeFolder_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualFolder folder = null;
            WaitCursor.ShowUntilIdle();
            using (SelectFolderDialog dialog = new SelectFolderDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.ShowItemIcons = Settings.Default.IsShowIcons;
                dialog.Folder = this.CurrentPanel.CurrentFolder;
                if (dialog.Execute(this))
                {
                    folder = dialog.Folder;
                }
            }
            if (folder != null)
            {
                WaitCursor.ShowUntilIdle();
                VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
                panel.CurrentFolder = folder;
            }
        }

        private void actChangeFolder_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = !this.CurrentPanel.IsFolderLocked;
        }

        private void actChangeView_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.View = this.GetNextView(this.CurrentPanel.View);
        }

        private void actChangeView_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            Action action = (Action) sender;
            PanelView nextView = this.GetNextView(this.CurrentPanel.View);
            if (!nextView.Equals(action.Tag))
            {
                action.Image = IconSet.GetImage("actViewAs" + nextView.ToString());
                action.Tag = nextView;
            }
        }

        private void actCheckForUpdates_OnExecute(object sender, ActionEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is CheckForUpdatesDialog)
                {
                    form.Activate();
                    return;
                }
            }
            WaitCursor.ShowUntilIdle();
            CheckForUpdatesDialog.CheckForUpdates();
        }

        private void actClearFilter_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.Filter = null;
        }

        private void actClearFilter_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CurrentPanel.Filter != null;
        }

        private void actCloseOtherTabs_OnExecute(object sender, ActionEventArgs e)
        {
            IContainer tabsToClose = new Container();
            foreach (TabStripPage page in this.MainPageSwitcher.Controls)
            {
                if (page != this.MainPageSwitcher.SelectedTabStripPage)
                {
                    tabsToClose.Add(page);
                }
            }
            this.CloseTabs(tabsToClose);
        }

        private void actCloseTab_OnExecute(object sender, ActionEventArgs e)
        {
            Tab fPreviousTab = this.FPreviousTab;
            using ((!Settings.Default.AlwaysShowTabStrip && (this.MainPageSwitcher.Controls.Count == 2)) ? new LockWindowRedraw(this, true) : null)
            {
                this.MainTabStrip.SuspendLayout();
                this.MainPageSwitcher.SelectedTabStripPage.Dispose();
                if (fPreviousTab != null)
                {
                    fPreviousTab.PerformClick();
                }
                this.MainTabStrip.ResumeLayout();
            }
        }

        private void actCloseTab_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.MainPageSwitcher.Controls.Count > 1;
        }

        private void actCloseTabsToRight_OnExecute(object sender, ActionEventArgs e)
        {
            IContainer tabsToClose = new Container();
            for (Tab tab = this.MainTabStrip.GetNextTab(this.MainTabStrip.SelectedTab, true, false); tab != null; tab = this.MainTabStrip.GetNextTab(tab, true, false))
            {
                tabsToClose.Add(tab.TabStripPage);
            }
            this.CloseTabs(tabsToClose);
        }

        private void actCloseTabsToRight_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = (this.MainPageSwitcher.Controls.Count > 0) && !this.MainTabStrip.SelectedTab.IsLastTab;
        }

        private void actCompareFolders_OnExecute(object sender, ActionEventArgs e)
        {
            using (CompareFoldersDialog dialog = new CompareFoldersDialog())
            {
                base.AddOwnedForm(dialog);
                if (dialog.Execute(this))
                {
                    IEnumerable<string> equalNames;
                    bool flag = true;
                    if ((dialog.Options & CompareFoldersOptions.CompareContent) > 0)
                    {
                        CompareFoldersWorker woker = new CompareFoldersWorker(this.CurrentPanel.Items, this.FarPanel.Items, dialog.Options);
                        using (CustomWorkerDialog dialog2 = new CustomWorkerDialog())
                        {
                            dialog2.OperationName = Resources.sCompareFolders;
                            flag = dialog2.Run(this, woker);
                        }
                        equalNames = woker.EqualNames;
                    }
                    else
                    {
                        equalNames = new CompareFoldersWorker().CompareFolders(this.CurrentPanel.Items, this.FarPanel.Items, dialog.Options);
                    }
                    if (flag && (equalNames != null))
                    {
                        IVirtualItemFilter filter = new AggregatedVirtualItemFilter(new IVirtualItemFilter[] { new VirtualItemAttributeFilter(0, FileAttributes.Directory), new VirtualItemNameListFilter(NameListCondition.NotInList, equalNames) });
                        if (dialog.SelectItems)
                        {
                            this.CurrentPanel.SelectItems(filter, true);
                            this.FarPanel.SelectItems(filter, true);
                        }
                        else
                        {
                            this.CurrentPanel.UnselectItems(filter);
                            this.FarPanel.UnselectItems(filter);
                        }
                    }
                }
            }
        }

        private void actCopy_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                WaitCursor.ShowUntilIdle();
                clearSelectionBeforeWork &= this.ShowFileCopyDialog(this.FarPanel.CurrentFolder, allItems, Convert.ToBoolean(e.Tag));
            }
            if (clearSelectionBeforeWork)
            {
                this.CurrentPanel.PushSelection(true);
            }
        }

        private void actCopy_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = ((e.Target is IEnumerable<IVirtualItem>) || (e.Target is IVirtualItem)) || (this.CurrentPanel.SelectionOrFocused != null);
        }

        private void actCopyCurrentFolderAsText_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            if (panel.CurrentFolder != null)
            {
                try
                {
                    Clipboard.SetText(panel.CurrentFolder.FullName);
                }
                catch (ExternalException exception)
                {
                    MessageDialog.ShowException(this, new ApplicationException(string.Format(Resources.sErrorPutToClipboard, exception.Message), exception), true);
                }
            }
        }

        private void actCopyDetailsAsCSV_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                List<VirtualProperty> list = new List<VirtualProperty>();
                foreach (ListViewColumnInfo info in this.CurrentPanel.GetVisibleColumns())
                {
                    list.Add(info.Property);
                }
                StringBuilder builder = new StringBuilder();
                int num = 0;
                while (num < list.Count)
                {
                    if (num > 0)
                    {
                        builder.Append('\t');
                    }
                    builder.Append(list[num].PropertyName);
                    num++;
                }
                foreach (IVirtualItem item in allItems)
                {
                    builder.AppendLine();
                    for (num = 0; num < list.Count; num++)
                    {
                        if (num > 0)
                        {
                            builder.Append('\t');
                        }
                        string str = null;
                        VirtualProperty property = list[num];
                        if (item.IsPropertyAvailable(property.PropertyId))
                        {
                            str = property.ConvertToString(item[property.PropertyId]);
                        }
                        if (!string.IsNullOrEmpty(str))
                        {
                            builder.Append(str);
                        }
                    }
                }
                DataObject data = new DataObject();
                string textData = builder.ToString();
                data.SetText(textData);
                textData = textData.Replace("\t", CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                data.SetText(textData, TextDataFormat.CommaSeparatedValue);
                if (clearSelectionBeforeWork & this.SetClipboardDataObject(data))
                {
                    this.CurrentPanel.PushSelection(true);
                }
            }
        }

        private void actCopyDetailsAsCSV_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = (this.CurrentPanel.View == PanelView.Details) && (((e.Target is IEnumerable<IVirtualItem>) || (e.Target is IVirtualItem)) || (this.CurrentPanel.SelectionOrFocused != null));
        }

        private void actCopyNameAsText_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                bool flag2 = sender == this.actCopyFullNameAsText;
                StringBuilder builder = new StringBuilder();
                foreach (IVirtualItem item in allItems)
                {
                    if (builder.Length > 0)
                    {
                        builder.AppendLine();
                    }
                    builder.Append(flag2 ? item.FullName : item.Name);
                }
                try
                {
                    Clipboard.SetText(builder.ToString());
                }
                catch (ExternalException exception)
                {
                    MessageDialog.ShowException(this, new ApplicationException(string.Format(Resources.sErrorPutToClipboard, exception.Message), exception), true);
                    clearSelectionBeforeWork = false;
                }
                if (clearSelectionBeforeWork)
                {
                    this.CurrentPanel.PushSelection(true);
                }
            }
        }

        private void actCopyToClipboard_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                DataObject data = new VirtualItemDataObject(allItems, Convert.ToBoolean(e.Tag));
                data.SetData("Virtual Item Operation", false, true);
                if (clearSelectionBeforeWork & this.SetClipboardDataObject(data))
                {
                    this.CurrentPanel.PushSelection(true);
                }
            }
        }

        private void actCustomizeFolder_OnExecute(object sender, ActionEventArgs e)
        {
            DesktopIni desktopIni = VirtualItem.GetDesktopIni(this.CurrentPanel.CurrentFolder, false);
            if (desktopIni != null)
            {
                try
                {
                    using (CustomizeFolderDialog dialog = new CustomizeFolderDialog())
                    {
                        base.AddOwnedForm(dialog);
                        desktopIni.Read();
                        SimpleCustomizeFolder defaultCustomize = new SimpleCustomizeFolder {
                            AutoSizeColumns = new bool?(this.CurrentPanel.AutoSizeColumns)
                        };
                        defaultCustomize.SetColumns(this.CurrentPanel.GetAllColumns());
                        defaultCustomize.Filter = this.CurrentPanel.Filter;
                        defaultCustomize.Sort = this.CurrentPanel.Sort as VirtualItemComparer;
                        defaultCustomize.ListColumnCount = new int?(this.CurrentPanel.ListViewColumnCount);
                        defaultCustomize.ThumbnailSize = this.CurrentPanel.ThumbnailSize;
                        defaultCustomize.ThumbnailSpacing = this.CurrentPanel.ThumbnailSpacing;
                        defaultCustomize.View = new PanelView?(this.CurrentPanel.View);
                        dialog.ApplyToChildren = desktopIni.ApplyToChildren;
                        if (!dialog.Execute(this.CurrentPanel.CurrentFolder, desktopIni, defaultCustomize, this.CurrentPanel.AvailableProperties))
                        {
                            return;
                        }
                        dialog.UpdateCustomizeFolder(desktopIni);
                        desktopIni.ApplyToChildren = dialog.ApplyToChildren && !desktopIni.IsEmpty();
                    }
                    desktopIni.Write();
                    this.CurrentPanel.RefreshCurrentFolder();
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

        private void actCustomizeFolder_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualFolder currentFolder = this.CurrentPanel.CurrentFolder;
            e.Enabled = ((currentFolder != null) && !(currentFolder is VirtualSearchFolder)) && !(currentFolder is ArchiveFolder);
        }

        private void actDelete_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                clearSelectionBeforeWork &= this.DeleteSelection(allItems);
            }
            if (clearSelectionBeforeWork)
            {
                this.CurrentPanel.PushSelection(true);
            }
        }

        private void actDeleteSingleItem_OnExecute(object sender, ActionEventArgs e)
        {
            this.DeleteSelection(new IVirtualItem[] { this.CurrentPanel.FocusedItem });
        }

        private void actDisconnectNetworkDrive_OnExecute(object sender, ActionEventArgs e)
        {
            try
            {
                WNet.DisconnectNetworkDrive(this);
            }
            catch (IOException exception)
            {
                MessageDialog.ShowException(this, exception);
            }
        }

        private void actDuplicateTab_OnExecute(object sender, ActionEventArgs e)
        {
            Tab selectedTab = this.MainTabStrip.SelectedTab;
            using (!this.MainTabStrip.Visible ? new LockWindowRedraw(this, true) : null)
            {
                this.MainTabStrip.SuspendLayout();
                TwoPanelContainer newTabContent = (TwoPanelContainer) this.CurrentTabContent.Clone();
                newTabContent.BeginLayout();
                Tab tab2 = this.AddNewTab(newTabContent);
                newTabContent.EndLayout(true);
                tab2.PerformClick();
                this.MainTabStrip.ResumeLayout();
            }
            this.FPreviousTab = selectedTab;
        }

        private void actEditDescription_OnExecute(object sender, ActionEventArgs e)
        {
            IChangeVirtualItem onlyOneItem = GetOnlyOneItem<IChangeVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            if ((onlyOneItem != null) && onlyOneItem.CanSetProperty(11))
            {
                string description;
                using (EditDescriptionDialog dialog = new EditDescriptionDialog())
                {
                    base.AddOwnedForm(dialog);
                    dialog.Description = (string) onlyOneItem[11];
                    if (dialog.Execute(onlyOneItem))
                    {
                        description = dialog.Description;
                    }
                    else
                    {
                        return;
                    }
                }
                try
                {
                    onlyOneItem[11] = description;
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

        private void actEditDescription_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IChangeVirtualItem onlyOneItem = GetOnlyOneItem<IChangeVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = ((onlyOneItem != null) && onlyOneItem.CanSetProperty(11)) && !onlyOneItem.Equals(this.CurrentPanel.ParentFolder);
        }

        private void actEditItem_OnExecute(object sender, ActionEventArgs e)
        {
            IChangeVirtualFile onlyOneItem = GetOnlyOneItem<IChangeVirtualFile>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                string editorPath = Settings.Default.EditorPath;
                bool flag = Control.ModifierKeys == Keys.Shift;
                if (!flag)
                {
                    string text = null;
                    if (string.IsNullOrEmpty(editorPath))
                    {
                        text = Resources.sEditorPathEmpty;
                    }
                    else if (!System.IO.File.Exists(editorPath))
                    {
                        text = string.Format(Resources.sEditorNotFound, editorPath);
                    }
                    if (text != null)
                    {
                        if (MessageDialog.Show(this, text, Resources.sFindEditor, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) != MessageDialogResult.Yes)
                        {
                            return;
                        }
                        flag = true;
                    }
                }
                if (flag)
                {
                    if (System.IO.File.Exists(editorPath))
                    {
                        this.FindExeFileDialog.FileName = editorPath;
                    }
                    else
                    {
                        try
                        {
                            this.FindExeFileDialog.InitialDirectory = Path.GetDirectoryName(editorPath);
                        }
                        catch
                        {
                        }
                    }
                    this.FindExeFileDialog.Title = Resources.sFindEditor;
                    if (this.FindExeFileDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                    editorPath = this.FindExeFileDialog.FileName;
                    Settings.Default.EditorPath = editorPath;
                }
                IChangeVirtualFile item = this.GetLocalSystemItemCopy(onlyOneItem, false, false);
                if (item != null)
                {
                    Process watchProcess = StartItem(item, editorPath);
                    if ((onlyOneItem is FtpFile) && (ConfirmationSettings.Default.UploadChangedFileBack != MessageDialogResult.No))
                    {
                        this.StartProcessWatch(watchProcess, onlyOneItem, item);
                    }
                }
            }
        }

        private void actEditItem_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = GetOnlyOneItem<IChangeVirtualFile>(e.Target, this.CurrentPanel.FocusedItem) != null;
        }

        private void actEmptyClipboard_OnExecute(object sender, ActionEventArgs e)
        {
            try
            {
                Clipboard.Clear();
            }
            catch (ExternalException exception)
            {
                MessageDialog.ShowException(this, exception, true);
            }
        }

        private void actEmptyClipboard_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CheckClipboard(ClipboardState.ContainsAnyData, delegate {
                return Clipboard.GetDataObject().GetFormats().Length > 0;
            });
        }

        private void actEqualizePanels_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFilePanel currentPanel;
            VirtualFilePanel farPanel;
            switch (Convert.ToInt32(e.Tag))
            {
                case 0:
                    currentPanel = this.CurrentTabContent.CurrentPanel;
                    farPanel = this.CurrentTabContent.FarPanel;
                    break;

                case 1:
                    currentPanel = this.CurrentTabContent.LeftPanel;
                    farPanel = this.CurrentTabContent.RightPanel;
                    break;

                case 2:
                    currentPanel = this.CurrentTabContent.RightPanel;
                    farPanel = this.CurrentTabContent.LeftPanel;
                    break;

                default:
                    return;
            }
            if (currentPanel.CurrentFolder == null)
            {
                farPanel.CurrentFolder = null;
            }
            else
            {
                farPanel.SetCurrentFolder((History<IVirtualFolder>) currentPanel.History.Clone());
            }
        }

        private void actExit_Click(object sender, ActionEventArgs e)
        {
            Application.Exit();
        }

        private void actFindDialog_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualFolder currentFolder = this.CurrentPanel.CurrentFolder;
            VirtualSearchFolder folder2 = currentFolder as VirtualSearchFolder;
            currentFolder = (folder2 != null) ? folder2.Parent : currentFolder;
            WaitCursor.ShowUntilIdle();
            using (SearchDialog dialog = new SearchDialog())
            {
                base.AddOwnedForm(dialog);
                if (folder2 != null)
                {
                    dialog.Folder = folder2.SearchFolder;
                    dialog.Filter = folder2.SearchFilter;
                    dialog.SearchOptions = folder2.SearchOptions;
                    dialog.DuplicateOptions = folder2.DuplicateOptions;
                }
                else
                {
                    dialog.Folder = currentFolder;
                }
                if (dialog.Execute(this))
                {
                    folder2 = new VirtualSearchFolder(dialog.Folder, dialog.Filter, ((dialog.SearchOptions | SearchFolderOptions.DetectChanges) | SearchFolderOptions.AutoAsyncSearch) | SearchFolderOptions.ExpandAggregatedRoot, dialog.DuplicateOptions);
                    folder2.SearchError += new EventHandler<SearchErrorEventArgs>(this.SearchFolderError);
                    folder2.Parent = (dialog.Folder is AggregatedVirtualFolder) ? currentFolder : dialog.Folder;
                }
                else
                {
                    folder2 = null;
                }
            }
            if (folder2 != null)
            {
                this.CurrentPanel.CurrentFolder = folder2;
            }
        }

        private void actFolderBranch_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFolderBranch branch = new VirtualFolderBranch(this.CurrentPanel.CurrentFolder);
            branch.SearchError += new EventHandler<SearchErrorEventArgs>(this.SearchFolderError);
            this.CurrentPanel.CurrentFolder = branch;
        }

        private void actFolderBranch_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = (!this.CurrentPanel.IsFolderLocked && (this.CurrentPanel.CurrentFolder != null)) && !(this.CurrentPanel.CurrentFolder is VirtualSearchFolder);
        }

        private void actForward_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).Forward();
        }

        private void actForward_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = (panel.History.ForwardCount > 0) && !panel.IsFolderLocked;
        }

        private void actFtpConnect_OnExecute(object sender, ActionEventArgs e)
        {
            using (FtpConnectDialog dialog = new FtpConnectDialog())
            {
                base.AddOwnedForm(dialog);
                if (dialog.Execute())
                {
                    this.CurrentPanel.CurrentFolder = dialog.Folder;
                }
            }
        }

        private void actGCCollect_OnExecute(object sender, ActionEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void actGoToParent_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.UpFolder();
        }

        private void actGoToParent_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CurrentPanel.ParentFolder != null;
        }

        private void actGoToRoot_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.ToRoot();
        }

        private void actGoToRoot_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CurrentPanel.CurrentFolder is IGetVirtualRoot;
        }

        private void actHelpContents_OnExecute(object sender, ActionEventArgs e)
        {
            string startupPath = Application.StartupPath;
            string str2 = Path.ChangeExtension(Path.GetFileName(Application.ExecutablePath), ".chm");
            string path = Path.Combine(Path.Combine(startupPath, CultureInfo.CurrentUICulture.Name), str2);
            if (!System.IO.File.Exists(path))
            {
                path = Path.Combine(startupPath, str2);
            }
            if (System.IO.File.Exists(path))
            {
                Help.ShowHelp(this, path, (HelpNavigator) e.Tag);
            }
        }

        private void actInvertEntireSelection_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.InvertSelection(null);
        }

        private void actInvertSelection_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.InvertSelection(new VirtualItemAttributeFilter(0, FileAttributes.Directory));
        }

        private void actionManager_PreviewExecuteAction(object sender, ActionEventArgs e)
        {
            ToolStripMenuItem source = e.Source as ToolStripMenuItem;
            e.Handled = ((source != null) && ((source.ShortcutKeys & (Keys.Alt | Keys.Control)) > Keys.None)) && (Form.ActiveForm != this);
        }

        private void actLockFolderChange_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.IsFolderLocked = !this.CurrentPanel.IsFolderLocked;
        }

        private void actLockFolderChange_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Checked = this.CurrentPanel.IsFolderLocked;
        }

        private void actMakeFolder_OnExecute(object sender, ActionEventArgs e)
        {
            string NewFolderName;
            IVirtualFolder currentFolder = this.CurrentPanel.CurrentFolder;
            ICreateVirtualFolder CreateFolder = currentFolder as ICreateVirtualFolder;
            if (CreateFolder != null)
            {
                using (MakeFolderDialog dialog = new MakeFolderDialog())
                {
                    base.AddOwnedForm(dialog);
                    IVirtualItem focusedItem = this.CurrentPanel.FocusedItem;
                    if (!((focusedItem == null) || focusedItem.Equals(this.CurrentPanel.ParentFolder)))
                    {
                        dialog.NewFolderName = (focusedItem is IVirtualFolder) ? focusedItem.Name : Path.GetFileNameWithoutExtension(focusedItem.Name);
                    }
                    if (!dialog.Execute(this, currentFolder))
                    {
                        return;
                    }
                    NewFolderName = dialog.NewFolderName;
                }
                try
                {
                    IVirtualFolder NewFolder = null;
                    if (this.ExecuteElevated(currentFolder, delegate {
                        WaitCursor.ShowUntilIdle();
                        NewFolder = CreateFolder.CreateFolder(NewFolderName);
                    }) && (NewFolder != null))
                    {
                        IVirtualFolder parent = NewFolder.Parent;
                        while ((parent != null) && !parent.Equals(currentFolder))
                        {
                            NewFolder = parent;
                            parent = parent.Parent;
                        }
                        if (parent != null)
                        {
                            this.CurrentPanel.SetFocusedItem(NewFolder, true, true);
                        }
                    }
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

        private void actMakeFolder_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CurrentPanel.CurrentFolder is ICreateVirtualFolder;
        }

        private void actMakeLink_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            ICreateVirtualLink createLink = onlyOneItem as ICreateVirtualLink;
            if (createLink != null)
            {
                IVirtualFolder destFolder;
                LinkType linkType;
                string linkName;
                using (MakeLinkDialog dialog = new MakeLinkDialog())
                {
                    base.AddOwnedForm(dialog);
                    dialog.CurrentFolder = this.CurrentPanel.CurrentFolder;
                    if (this.FarPanel.CurrentFolder != null)
                    {
                        dialog.DestFolder = this.FarPanel.CurrentFolder;
                    }
                    if (!dialog.Execute(this, onlyOneItem))
                    {
                        return;
                    }
                    destFolder = dialog.DestFolder;
                    linkType = dialog.LinkType;
                    linkName = dialog.LinkName;
                }
                this.DoCreateLink(destFolder, createLink, linkType, linkName);
            }
        }

        private void actMakeLink_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = (onlyOneItem is ICreateVirtualLink) && !onlyOneItem.Equals(this.CurrentPanel.ParentFolder);
        }

        private void actManageColumns_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.ManageColumns(this);
        }

        private void actManageLayouts_OnExecute(object sender, ActionEventArgs e)
        {
            using (ManageListDialog dialog = new ManageListDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.Items = Settings.Default.Layouts;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.Layouts = dialog.GetItems<TwoPanelLayout>();
                }
            }
        }

        private void actManageLayouts_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            TwoPanelLayout[] layouts = Settings.Default.Layouts;
            e.Enabled = (layouts != null) && (layouts.Length > 0);
        }

        private void actMapNetworkDrive_OnExecute(object sender, ActionEventArgs e)
        {
            try
            {
                WNet.MapNetworkDrive(this);
            }
            catch (IOException exception)
            {
                MessageDialog.ShowException(this, exception);
            }
        }

        private void actMinimizeToTray_OnExecute(object sender, ActionEventArgs e)
        {
            this.SetUIState(UIState.ProcessingTray, true);
            try
            {
                if (this.TrayIcon.Icon == null)
                {
                    this.TrayIcon.Icon = new Icon(Resources.Camel, ImageHelper.DefaultSmallIconSize);
                }
                base.WindowState = FormWindowState.Minimized;
                base.ShowInTaskbar = false;
                this.TrayIcon.Visible = true;
            }
            finally
            {
                this.SetUIState(UIState.ProcessingTray, false);
            }
            ToolStripItem source = e.Source as ToolStripItem;
            if (source != null)
            {
                source.Unselect();
            }
        }

        private void actMoveToLastTab_OnExecute(object sender, ActionEventArgs e)
        {
            for (int i = this.MainTabStrip.Items.Count - 1; i >= 0; i--)
            {
                if (this.MainTabStrip.Items[i] is Tab)
                {
                    this.MainTabStrip.Items[i].PerformClick();
                    break;
                }
            }
        }

        private void actMoveToLastTab_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            int count = this.MainPageSwitcher.Controls.Count;
            e.Enabled = (count > 1) && ((this.MainTabStrip.SelectedTab == null) || !this.MainTabStrip.SelectedTab.IsLastTab);
        }

        private void actMoveToNextTab_OnExecute(object sender, ActionEventArgs e)
        {
            this.MainTabStrip.SelectNextTab(Convert.ToBoolean(e.Tag));
        }

        private void actMoveToNextTab_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            int count = this.MainPageSwitcher.Controls.Count;
            e.Enabled = count > 1;
        }

        private void actMoveToNthTab_OnExecute(object sender, ActionEventArgs e)
        {
            int tag = (int) e.Tag;
            int num2 = 0;
            for (int i = 0; i < this.MainTabStrip.Items.Count; i++)
            {
                if (this.MainTabStrip.Items[i] is Tab)
                {
                    num2++;
                }
                if (num2 == tag)
                {
                    this.MainTabStrip.Items[i].PerformClick();
                    break;
                }
            }
        }

        private void actMoveToNthTab_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            int num = ((int) e.Tag) - 1;
            int count = this.MainPageSwitcher.Controls.Count;
            e.Enabled = (num < count) && ((this.MainTabStrip.SelectedTab == null) || (this.MainTabStrip.SelectedTab.TabIndex != num));
        }

        private void actNavigationLink_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentTabContent.NavigationLock = !this.CurrentTabContent.NavigationLock;
        }

        private void actNavigationLink_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = ((this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None) && !this.CurrentPanel.IsFolderLocked) && !this.FarPanel.IsFolderLocked;
            e.Checked = e.Enabled && this.CurrentTabContent.NavigationLock;
        }

        private void actNewFile_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            if (panel.CurrentFolder != null)
            {
                this.MakeNewFile(panel.CurrentFolder, null);
            }
        }

        private void actNewFile_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = panel.CurrentFolder is ICreateVirtualFile;
        }

        private void actOnePanel_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentTabContent.OnePanelMode = (this.CurrentTabContent.CurrentPanel == this.CurrentTabContent.LeftPanel) ? TwoPanelContainer.SinglePanel.Left : TwoPanelContainer.SinglePanel.Right;
        }

        private void actOnePanel_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Checked = this.CurrentTabContent.OnePanelMode != TwoPanelContainer.SinglePanel.None;
        }

        private void actOpen_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                ICustomizeFolder customize = null;
                IVirtualLink link = onlyOneItem as IVirtualLink;
                if (link != null)
                {
                    IVirtualItem target = link.Target;
                    if (target is IVirtualFolder)
                    {
                        onlyOneItem = target;
                        customize = link[0x17] as ICustomizeFolder;
                    }
                }
                IVirtualFolder folder2 = onlyOneItem as IVirtualFolder;
                if (folder2 != null)
                {
                    this.CurrentPanel.SetCurrentFolder(folder2, true, customize);
                }
                else if (!this.ExecuteItem(onlyOneItem, false))
                {
                    IVirtualFileExecute execute = onlyOneItem as IVirtualFileExecute;
                    if (execute != null)
                    {
                        execute.Execute(this);
                    }
                }
            }
        }

        private void actOpen_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = onlyOneItem != null;
        }

        private void actOpenAsArchive_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                this.ExecuteItem(onlyOneItem, true);
            }
        }

        private void actOpenAsArchive_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = (onlyOneItem != null) && !(onlyOneItem is IVirtualFolder);
        }

        private void actOpenContainingFolder_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                try
                {
                    if (!onlyOneItem.IsPropertyAvailable(12))
                    {
                        WaitCursor.ShowUntilIdle();
                        IVirtualLink link = onlyOneItem as IVirtualLink;
                        if (link != null)
                        {
                            onlyOneItem = link.Target;
                        }
                        else if (onlyOneItem.IsPropertyAvailable(10))
                        {
                            onlyOneItem = VirtualItem.FromFullName((string) onlyOneItem[10], VirtualItemType.Unknown);
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (onlyOneItem != null)
                    {
                        IVirtualFolder parent = onlyOneItem.Parent;
                        if (parent != null)
                        {
                            this.CurrentPanel.CurrentFolder = parent;
                            this.CurrentPanel.SetFocusedItem(onlyOneItem, true, true);
                        }
                    }
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

        private void actOpenContainingFolder_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = (!this.CurrentPanel.IsFolderLocked && (onlyOneItem != null)) && (((onlyOneItem.IsPropertyAvailable(12) && !string.Equals(this.CurrentPanel.CurrentFolder.FullName, (string) onlyOneItem[12], StringComparison.OrdinalIgnoreCase)) || (onlyOneItem is IVirtualLink)) || onlyOneItem.IsPropertyAvailable(10));
        }

        private void actOpenInFarPanel_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                IVirtualFolder folder = onlyOneItem as IVirtualFolder;
                if (folder != null)
                {
                    this.FarPanel.CurrentFolder = folder;
                }
                else
                {
                    IVirtualFolder parent = onlyOneItem.Parent;
                    if (parent != null)
                    {
                        this.FarPanel.CurrentFolder = parent;
                        this.FarPanel.SetFocusedItem(onlyOneItem, true, true);
                    }
                }
            }
        }

        private void actOpenInFarPanel_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = (onlyOneItem != null) && (this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None);
        }

        private void actOpenOutside_OnExecute(object sender, ActionEventArgs e)
        {
            try
            {
                if (e.Target == null)
                {
                    IVirtualItemUI focusedItem = this.CurrentPanel.FocusedItem as IVirtualItemUI;
                    if (focusedItem != null)
                    {
                        focusedItem.ExecuteVerb(this, "open");
                    }
                }
                else
                {
                    foreach (IVirtualItemUI mui2 in GetAllItems(e.Target).AsEnumerable<IVirtualItem>().OfType<IVirtualItemUI>())
                    {
                        mui2.ExecuteVerb(this, "open");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageDialog.ShowException(this, exception, Resources.sCaptionRunError, VirtualItem.IsWarningIOException(exception));
            }
        }

        private void actOpenOutside_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItemUI onlyOneItem = GetOnlyOneItem<IVirtualItemUI>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = (onlyOneItem != null) || (e.Target is IEnumerable<IVirtualItem>);
        }

        private void actOpenRecentFolders_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).OpenRecentFolders();
        }

        private void actOptions_OnExecute(object sender, ActionEventArgs e)
        {
            WaitCursor.ShowUntilIdle();
            this.actResetVisualCache.Tag = null;
            using (OptionsDialog dialog = new OptionsDialog())
            {
                base.AddOwnedForm(dialog);
                string tag = (string) e.Tag;
                if (!string.IsNullOrEmpty(tag))
                {
                    System.Type blockType = System.Type.GetType(tag);
                    if (blockType != null)
                    {
                        dialog.SetInitialSection(blockType);
                    }
                }
                dialog.ShowDialog();
            }
            if (Convert.ToBoolean(this.actResetVisualCache.Tag))
            {
                this.actResetVisualCache.Execute();
            }
        }

        private void actOrganizeBookmarks_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.CurrentFolder = this.BookmarksFolder;
        }

        private void actPack_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                clearSelectionBeforeWork &= this.ShowPackDialog(this.FarPanel.CurrentFolder, allItems, null);
            }
            if (clearSelectionBeforeWork)
            {
                this.CurrentPanel.PushSelection(true);
            }
        }

        private void actPasteFromClipboard_OnExecute(object sender, ActionEventArgs e)
        {
            ExternalException exception;
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            IVirtualFolder currentFolder = panel.CurrentFolder;
            IEnumerable<IVirtualItem> clipboardItems = this.GetClipboardItems();
            if (clipboardItems != null)
            {
                bool flag;
                try
                {
                    flag = VirtualItemDataObject.ReadInt32FromClipboard("Preferred DropEffect") == 2;
                }
                catch (ExternalException exception1)
                {
                    exception = exception1;
                    MessageDialog.ShowException(this, new ApplicationException(string.Format(Resources.sErrorGetClipboardItems, exception.Message), exception), true);
                    return;
                }
                if (currentFolder is ArchiveFolder)
                {
                    this.ShowPackDialog(currentFolder, clipboardItems, null);
                }
                else if (!(ConfirmationSettings.Default.Paste || !VirtualItemHelper.CanCreateInFolder(currentFolder)))
                {
                    CopyWorkerOptions options = ~CopyWorkerOptions.DeleteSource;
                    DoStartCopy(currentFolder, clipboardItems, (CopySettings.Default.DefaultCopyOptions & options) | (flag ? 1 : 0), null, null, null);
                }
                else
                {
                    this.ShowFileCopyDialog(currentFolder, clipboardItems, flag);
                }
            }
            else if (currentFolder != null)
            {
                try
                {
                    IDataObject dataObject = Clipboard.GetDataObject();
                    if (ConfirmationSettings.Default.Paste)
                    {
                        this.ShowPasteDialog(currentFolder, dataObject, "Clipboard");
                    }
                    else
                    {
                        this.PasteObject(currentFolder, "Clipboard", dataObject);
                    }
                }
                catch (ExternalException exception2)
                {
                    exception = exception2;
                    MessageDialog.ShowException(this, new ApplicationException(string.Format(Resources.sErrorGetClipboardItems, exception.Message), exception), true);
                }
            }
        }

        private void actPasteFromClipboard_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CheckClipboard(ClipboardState.ContainsItems, delegate {
                return VirtualClipboardItem.ClipboardContainItems();
            }) || ((((e.Target as VirtualFilePanel) ?? this.CurrentPanel).CurrentFolder is ICreateVirtualFile) && (this.CheckClipboard(ClipboardState.ContainsText, new Func<bool>(this.ClipboardContainText)) || this.CheckClipboard(ClipboardState.ContainsImage, delegate {
                return Clipboard.ContainsImage();
            })));
        }

        private void actPasteShortCut_OnExecute(object sender, ActionEventArgs e)
        {
            IEnumerable<IVirtualItem> clipboardItems = this.GetClipboardItems();
            if (clipboardItems != null)
            {
                this.DoCreateLinks(this.CurrentPanel.CurrentFolder, clipboardItems);
            }
        }

        private void actPasteShortCut_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = (this.CurrentPanel.CurrentFolder != null) && this.CheckClipboard(ClipboardState.ContainsItems, delegate {
                return VirtualClipboardItem.ClipboardContainItems();
            });
        }

        private void actQuickChangeFolder_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).QuickChangeFolder();
        }

        private void actRefresh_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).RefreshCurrentFolder();
        }

        private void actRefresh_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = panel.CurrentFolder != null;
        }

        private void actRefreshToolbars_OnExecute(object sender, ActionEventArgs e)
        {
            foreach (ToolStrip strip in this.Toolbars)
            {
                this.SetUpdateToolbarsNeeded(strip);
            }
            this.UpdateToolbars();
        }

        private void actRenameSingleItem_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.BeginRenameItem();
        }

        private void actRenameSingleItem_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem focusedItem = this.CurrentPanel.FocusedItem;
            e.Enabled = (focusedItem is IChangeVirtualItem) && !focusedItem.Equals(this.CurrentPanel.ParentFolder);
        }

        private void actRenameTab_OnExecute(object sender, ActionEventArgs e)
        {
            string text = this.MainTabStrip.SelectedTab.Text;
            if (InputDialog.Input(this, Resources.sAskForTabName, Resources.sCaptionRenameTab, ref text))
            {
                this.CurrentTabContent.Text = text;
                this.MainTabStrip.SelectedTab.Text = text;
            }
        }

        private void actRenameTab_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.MainTabStrip.Visible;
        }

        private void actResetVisualCache_OnExecute(object sender, ActionEventArgs e)
        {
            CustomImageProvider.ResetIconCache();
            string path = Path.Combine(SettingsManager.SpecialFolders.IconCache, SettingsManager.IconCacheName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Deleted, path);
            }
            this.ResetBookmarks();
            this.ResetTools();
            foreach (ToolStripItem item in this.tsmiSpecialFolders.DropDownItems)
            {
                item.Image = null;
            }
            ChangeVector.Increment(ChangeVector.Icon);
            foreach (TabStripPage page in this.MainPageSwitcher.Controls)
            {
                ((TwoPanelContainer) page.Controls[0]).ResetVisualCache();
            }
        }

        private void actRestoreSelection_OnExecute(object sender, ActionEventArgs e)
        {
            if (this.CurrentPanel.StoredSelection != null)
            {
                this.CurrentPanel.Selection = this.CurrentPanel.StoredSelection;
            }
            else
            {
                try
                {
                    this.CurrentPanel.SelectItems(new RestoreSelectionFilter(), true);
                }
                catch (ExternalException exception)
                {
                    MessageDialog.ShowException(this, exception, true);
                }
            }
        }

        private void actRestoreSelection_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = (this.CurrentPanel.CurrentFolder != null) && ((this.CurrentPanel.StoredSelection != null) || this.CheckClipboard(ClipboardState.ContainsSelection, delegate {
                return RestoreSelectionFilter.ClipboardContainsSelection();
            }));
        }

        private void actRunAs_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualFileExecute onlyOneItem = GetOnlyOneItem<IVirtualFileExecute>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                this.ShowRunAsDialog(onlyOneItem, null);
            }
        }

        private void actRunAs_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualFileExecute onlyOneItem = GetOnlyOneItem<IVirtualFileExecute>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = (onlyOneItem != null) && onlyOneItem.CanExecuteEx;
        }

        private void actRunAsAdmin_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualFileExecute onlyOneItem = GetOnlyOneItem<IVirtualFileExecute>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem != null)
            {
                this.RunAs(this, onlyOneItem, null, ExecuteAsUser.Administrator, null, null, Settings.Default.RunInThread);
            }
        }

        private void actRunAsAdmin_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            if (OS.IsWinVista)
            {
                IVirtualFileExecute onlyOneItem = GetOnlyOneItem<IVirtualFileExecute>(e.Target, this.CurrentPanel.FocusedItem);
                e.Enabled = (onlyOneItem != null) && onlyOneItem.CanExecuteEx;
            }
            else
            {
                e.Enabled = false;
            }
        }

        private void actSaveCurrentLayout_OnExecute(object sender, ActionEventArgs e)
        {
            string str = string.Empty;
            if (InputDialog.Input(this, Resources.sEnterLayoutName, Resources.sCaptionSaveCurrentLayout, ref str))
            {
                TwoPanelLayout windowLayout = this.CurrentTabContent.WindowLayout;
                windowLayout.Name = str;
                TwoPanelLayout[] layouts = Settings.Default.Layouts;
                if (layouts == null)
                {
                    layouts = new TwoPanelLayout[] { windowLayout };
                }
                else
                {
                    List<TwoPanelLayout> list = new List<TwoPanelLayout>(layouts);
                    bool flag = true;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Name.Equals(str, StringComparison.CurrentCultureIgnoreCase))
                        {
                            list[i] = windowLayout;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        list.Add(windowLayout);
                    }
                    layouts = list.ToArray();
                }
                Settings.Default.Layouts = layouts;
            }
        }

        private void actSaveSettings_OnExecute(object sender, ActionEventArgs e)
        {
            this.PlacementSettings.SavePlacement();
            this.CurrentTabContent.SaveComponentSettings();
            Settings.Default.RestoreTabsOnStart = false;
            Program.SafeSaveSettings();
        }

        private void actSelect_OnExecute(object sender, ActionEventArgs e)
        {
            using (SelectDialog dialog = new SelectDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.Text = Resources.sSelectFiles;
                dialog.SelectFolders = Settings.Default.SelectDialogSelectFolders;
                dialog.SelectFoldersVisible = true;
                if (dialog.Execute())
                {
                    ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).SelectItems(dialog.Filter, false);
                }
                Settings.Default.SelectDialogSelectFolders = dialog.SelectFolders;
            }
        }

        private void actSelectAll_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).SelectItems(null, true);
        }

        private void actSelectByExtension_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem focusedItem = this.CurrentPanel.FocusedItem;
            if (focusedItem != null)
            {
                string str = (string) focusedItem[1];
                if (!string.IsNullOrEmpty(str))
                {
                    IVirtualItemFilter filter = new AggregatedVirtualItemFilter(new VirtualItemAttributeFilter(0, FileAttributes.Directory), new VirtualItemNameFilter("*" + ((str == string.Empty) ? "." : str)));
                    if (object.Equals(e.Tag, "select"))
                    {
                        this.CurrentPanel.SelectItems(filter, false);
                    }
                    else
                    {
                        this.CurrentPanel.UnselectItems(filter);
                    }
                }
            }
        }

        private void actSelectByExtension_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = (this.CurrentPanel.FocusedItem != null) && !(this.CurrentPanel.FocusedItem is IVirtualFolder);
        }

        private void actSelectByName_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem focusedItem = this.CurrentPanel.FocusedItem;
            if (focusedItem != null)
            {
                string name = focusedItem.Name;
                int index = name.IndexOf('.');
                if (index > 0)
                {
                    name = name.Substring(0, index);
                }
                IVirtualItemFilter filter = new AggregatedVirtualItemFilter(new VirtualItemAttributeFilter(0, FileAttributes.Directory), new VirtualItemNameFilter(string.Format(@"^({0}|{0}\..+)$", name)));
                this.CurrentPanel.SelectItems(filter, false);
            }
        }

        private void actSelectSingleItem_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).InvertFocusedItemSelection(Convert.ToBoolean(e.Tag));
        }

        private void actSelectSingleItem_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = (panel.FocusedItem != null) && !panel.FocusedItem.Equals(panel.ParentFolder);
        }

        private void actSelectSort_OnExecute(object sender, ActionEventArgs e)
        {
            DesktopIni desktopIni = VirtualItem.GetDesktopIni(this.CurrentPanel.CurrentFolder, false);
            bool rememberSort = false;
            using (SortDialog dialog = new SortDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.AvailableProperties = this.CurrentPanel.AvailableProperties;
                dialog.Sort = this.CurrentPanel.Sort as VirtualItemComparer;
                dialog.RememberSortEnabled = desktopIni != null;
                if (dialog.Execute())
                {
                    rememberSort = dialog.RememberSort;
                    this.CurrentPanel.Sort = dialog.Sort;
                }
            }
            if (rememberSort)
            {
                try
                {
                    desktopIni.Read();
                    desktopIni.Sort = this.CurrentPanel.Sort as VirtualItemComparer;
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

        private void actSetAttributes_OnExecute(object sender, ActionEventArgs e)
        {
            bool clearSelectionBeforeWork = false;
            IEnumerable<IVirtualItem> allItems = GetAllItems(e.Target);
            if (allItems == null)
            {
                allItems = this.CurrentPanel.SelectionOrFocused;
                clearSelectionBeforeWork = Settings.Default.ClearSelectionBeforeWork;
            }
            if (allItems != null)
            {
                using (SetAttributesDialog dialog = new SetAttributesDialog())
                {
                    base.AddOwnedForm(dialog);
                    if (!dialog.Execute(this, allItems))
                    {
                        return;
                    }
                    SetAttributesWorkerDialog.ShowAsync(new SetAttributesWorker(allItems, dialog.IncludeSubfolders, dialog.SetAttributes, dialog.ResetAttributes, dialog.CreationTime, dialog.LastAccessTime, dialog.LastWriteTime)).SetAttributesWorker.RunAsync(ThreadPriority.Normal);
                }
                if (clearSelectionBeforeWork)
                {
                    this.CurrentPanel.PushSelection(true);
                }
            }
        }

        private void actSetNListColumn_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            panel.ListViewColumnCount = (int) e.Tag;
        }

        private void actSetNListColumn_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            e.Enabled = panel.View == PanelView.List;
            e.Checked = e.Enabled && (panel.ListViewColumnCount == ((int) e.Tag));
            if (((e.Enabled && !e.Checked) && (e.Source is ToolStripMenuItem)) && (panel.ActualListViewColumnCount == ((int) e.Tag)))
            {
                e.CheckState = CheckState.Indeterminate;
            }
        }

        private void actShowBookmarks_OnExecute(object sender, ActionEventArgs e)
        {
            ToolStripDropDownItem tsmiBookmarks = this.tsmiBookmarks;
            foreach (ToolStrip strip in this.Toolbars)
            {
                if (!strip.Visible)
                {
                    continue;
                }
                foreach (ToolStripItem item2 in strip.Items)
                {
                    ToolStripDropDownButton button = item2 as ToolStripDropDownButton;
                    if ((button != null) && (button.DropDown == this.tsmiBookmarks.DropDown))
                    {
                        tsmiBookmarks = button;
                        break;
                    }
                }
            }
            tsmiBookmarks.ShowDropDown();
        }

        private void actShowCmdLineHelp_OnExecute(object sender, ActionEventArgs e)
        {
            MessageBox.Show(this, Resources.sCommandLineHelp, "Nomad.NET");
        }

        private void actShowProperties_OnExecute(object sender, ActionEventArgs e)
        {
            IEnumerable<IVirtualItem> items = GetAllItems(e.Target) ?? this.CurrentPanel.SelectionOrFocused;
            if (items != null)
            {
                IVirtualItem item = null;
                foreach (IVirtualItem item2 in items)
                {
                    if (item == null)
                    {
                        item = item2;
                    }
                    else
                    {
                        item = null;
                        break;
                    }
                }
                if (item != null)
                {
                    IVirtualItemUI mui = item as IVirtualItemUI;
                    if (mui != null)
                    {
                        mui.ShowProperties(this);
                    }
                }
                else
                {
                    IVirtualFolderUI currentFolder = this.CurrentPanel.CurrentFolder as IVirtualFolderUI;
                    if (currentFolder != null)
                    {
                        currentFolder.ShowProperties(this, items);
                    }
                }
            }
        }

        private void actSortBy_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualItemComparer sort = this.CurrentPanel.Sort as VirtualItemComparer;
            this.CurrentPanel.Sort = new VirtualItemComparer((sort != null) ? sort : VirtualItemComparer.DefaultSort, (int) e.Tag);
        }

        private void actSortBy_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualItemComparer sort = this.CurrentPanel.Sort as VirtualItemComparer;
            e.Checked = (sort != null) && (sort.ComparePropertyId == ((int) e.Tag));
        }

        private void actSortDescending_OnExecute(object sender, ActionEventArgs e)
        {
            VirtualItemComparer sort = this.CurrentPanel.Sort as VirtualItemComparer;
            ListSortDirection descending = ListSortDirection.Descending;
            if ((sort != null) && (sort.SortDirection == ListSortDirection.Descending))
            {
                descending = ListSortDirection.Ascending;
            }
            this.CurrentPanel.Sort = new VirtualItemComparer((sort != null) ? sort : VirtualItemComparer.DefaultSort, descending);
        }

        private void actSortDescending_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualItemComparer sort = this.CurrentPanel.Sort as VirtualItemComparer;
            e.Checked = (sort != null) && (sort.SortDirection == ListSortDirection.Descending);
        }

        private void actSwapPanels_OnExecute(object sender, ActionEventArgs e)
        {
            History<IVirtualFolder> history = this.CurrentTabContent.LeftPanel.History;
            this.CurrentTabContent.LeftPanel.SetCurrentFolder(this.CurrentTabContent.RightPanel.History);
            this.CurrentTabContent.RightPanel.SetCurrentFolder(history);
        }

        private void actSwapPanels_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Enabled = this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None;
        }

        private void actToggleFolderBar_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.FolderBarVisible = !this.CurrentPanel.FolderBarVisible;
        }

        private void actToggleFolderBar_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Checked = this.CurrentPanel.FolderBarVisible;
        }

        private void actToggleOnePanelMode_OnExecute(object sender, ActionEventArgs e)
        {
            if (this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None)
            {
                this.CurrentTabContent.OnePanelMode = (this.CurrentTabContent.CurrentPanel == this.CurrentTabContent.LeftPanel) ? TwoPanelContainer.SinglePanel.Left : TwoPanelContainer.SinglePanel.Right;
            }
            else
            {
                this.CurrentTabContent.OnePanelMode = TwoPanelContainer.SinglePanel.None;
            }
        }

        private void actToggleOnePanelMode_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            Action action = (Action) sender;
            Image image = null;
            if (this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None)
            {
                image = this.actOnePanel.Image;
            }
            else if (this.CurrentTabContent.Orientation == Orientation.Vertical)
            {
                image = this.actTwoVerticalPanel.Image;
            }
            else
            {
                image = this.actTwoHorizontalPanel.Image;
            }
            action.Image = image;
        }

        private void actToggleQuickFind_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.ToggleQuickFind();
        }

        private void actTwoPanel_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentTabContent.OnePanelMode = TwoPanelContainer.SinglePanel.None;
            this.CurrentTabContent.Orientation = (Orientation) e.Tag;
        }

        private void actTwoPanel_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Checked = (this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None) && (this.CurrentTabContent.Orientation == ((Orientation) e.Tag));
        }

        private void actUnselect_OnExecute(object sender, ActionEventArgs e)
        {
            using (SelectDialog dialog = new SelectDialog())
            {
                base.AddOwnedForm(dialog);
                Icon icon = IconSet.GetIcon(this.actUnselect.Name);
                if (icon != null)
                {
                    dialog.Icon = icon;
                    dialog.ShowIcon = true;
                }
                dialog.Text = Resources.sUnselectFiles;
                dialog.SelectFolders = true;
                dialog.SelectFoldersVisible = false;
                if (dialog.Execute())
                {
                    ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).UnselectItems(dialog.Filter);
                }
            }
        }

        private void actUnselect_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            VirtualFilePanel panel = (e.Target as VirtualFilePanel) ?? this.CurrentPanel;
            ICollection<IVirtualItem> selection = panel.Selection;
            e.Enabled = (selection != null) && (selection.Count > 0);
        }

        private void actUnselectAll_OnExecute(object sender, ActionEventArgs e)
        {
            ((e.Target as VirtualFilePanel) ?? this.CurrentPanel).UnselectItems(null);
        }

        private void actViewAs_OnExecute(object sender, ActionEventArgs e)
        {
            this.CurrentPanel.View = (PanelView) e.Tag;
        }

        private void actViewAs_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            e.Checked = this.CurrentPanel.View == ((PanelView) e.Tag);
        }

        private void actViewItem_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            if (onlyOneItem is IVirtualFolder)
            {
                object obj2 = onlyOneItem[3];
            }
            else if (onlyOneItem is IChangeVirtualFile)
            {
                string viewerPath = Settings.Default.ViewerPath;
                bool flag = Control.ModifierKeys == Keys.Shift;
                if (!flag)
                {
                    string text = null;
                    if (string.IsNullOrEmpty(viewerPath))
                    {
                        text = Resources.sViewerPathEmpty;
                    }
                    else if (!System.IO.File.Exists(viewerPath))
                    {
                        text = string.Format(Resources.sViewerNotFound, viewerPath);
                    }
                    if (text != null)
                    {
                        if (MessageDialog.Show(this, text, Resources.sFindViewer, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) != MessageDialogResult.Yes)
                        {
                            return;
                        }
                        flag = true;
                    }
                }
                if (flag)
                {
                    if (System.IO.File.Exists(viewerPath))
                    {
                        this.FindExeFileDialog.FileName = viewerPath;
                    }
                    else
                    {
                        try
                        {
                            this.FindExeFileDialog.InitialDirectory = Path.GetDirectoryName(viewerPath);
                        }
                        catch
                        {
                        }
                    }
                    this.FindExeFileDialog.Title = Resources.sFindViewer;
                    if (this.FindExeFileDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                    viewerPath = this.FindExeFileDialog.FileName;
                    Settings.Default.ViewerPath = viewerPath;
                }
                IChangeVirtualFile item = this.GetLocalSystemItemCopy(onlyOneItem, false, false);
                if (item != null)
                {
                    StartItem(item, viewerPath);
                }
            }
        }

        private void actViewItem_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IVirtualItem onlyOneItem = GetOnlyOneItem<IVirtualItem>(e.Target, this.CurrentPanel.FocusedItem);
            e.Enabled = onlyOneItem != null;
        }

        private void actVolumeLabel_OnExecute(object sender, ActionEventArgs e)
        {
            IVirtualFolder root = null;
            IGetVirtualRoot currentFolder = this.CurrentPanel.CurrentFolder as IGetVirtualRoot;
            if (currentFolder != null)
            {
                root = currentFolder.Root;
            }
            if (root != null)
            {
                try
                {
                    string a = (string) root[30];
                    InputDialogOption allowEmptyValue = InputDialogOption.AllowEmptyValue;
                    ISetVirtualProperty property = root as ISetVirtualProperty;
                    if (!((property != null) && property.CanSetProperty(30)))
                    {
                        allowEmptyValue |= InputDialogOption.ReadOnly;
                    }
                    string str2 = a;
                    if (!((!InputDialog.Input(this, Resources.sEnterVolumeLabel, Resources.sCaptionVolumeLabel, ref str2, allowEmptyValue) || ((allowEmptyValue & InputDialogOption.ReadOnly) != 0)) || string.Equals(a, str2)))
                    {
                        WaitCursor.ShowUntilIdle();
                        property[30] = str2;
                    }
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

        private void actVolumeLabel_OnUpdate(object sender, UpdateActionEventArgs e)
        {
            IGetVirtualRoot currentFolder = this.CurrentPanel.CurrentFolder as IGetVirtualRoot;
            e.Enabled = (currentFolder != null) && (currentFolder.Root != null);
        }

        private Tab AddNewTab(TwoPanelContainer newTabContent)
        {
            TabStripPage page = new TabStripPage();
            newTabContent.HintTooltip = this.toolTipDefault;
            newTabContent.Dock = DockStyle.Fill;
            newTabContent.DragDropOnItem += new EventHandler<VirtualItemDragEventArg>(this.CurrentPanel_DragDropOnItem);
            newTabContent.DragOverItem += new EventHandler<VirtualItemDragEventArg>(this.CurrentPanel_DragOverItem);
            newTabContent.ExecuteItem += new EventHandler<HandleVirtualItemEventArgs>(this.CurrentPanel_ExecuteItem);
            newTabContent.PreviewContextMenu += new EventHandler<PreviewContextMenuEventArgs>(this.CurrentPanel_PreviewContextMenu);
            newTabContent.CurrentFolderChanged += new EventHandler(this.CurrentTab_CurrentFolderChanged);
            newTabContent.Disposed += delegate (object sender, EventArgs e) {
                TwoPanelContainer container = (TwoPanelContainer) sender;
                container.DragDropOnItem -= new EventHandler<VirtualItemDragEventArg>(this.CurrentPanel_DragDropOnItem);
                container.DragOverItem -= new EventHandler<VirtualItemDragEventArg>(this.CurrentPanel_DragOverItem);
                container.ExecuteItem -= new EventHandler<HandleVirtualItemEventArgs>(this.CurrentPanel_ExecuteItem);
                container.CurrentFolderChanged -= new EventHandler(this.CurrentTab_CurrentFolderChanged);
                container.PreviewContextMenu -= new EventHandler<PreviewContextMenuEventArgs>(this.CurrentPanel_PreviewContextMenu);
            };
            page.Controls.Add(newTabContent);
            this.MainPageSwitcher.Controls.Add(page);
            Tab tab = new Tab();
            tab.MouseEnter += new EventHandler(this.TabButton_MouseEnter);
            tab.MouseClick += new MouseEventHandler(this.TabButton_MouseClick);
            tab.DragOver += new DragEventHandler(this.CurrentTab_DragOver);
            tab.DragHover += new DragEventHandler(this.CurrentTab_DragHover);
            tab.TabStripPage = page;
            if (!string.IsNullOrEmpty(newTabContent.Text))
            {
                tab.Text = newTabContent.Text;
            }
            else
            {
                IVirtualFolder currentFolder = newTabContent.CurrentPanel.CurrentFolder;
                if (currentFolder != null)
                {
                    tab.Text = currentFolder.Name.Replace("&", "&&");
                }
                else
                {
                    tab.Text = Resources.sTab;
                }
            }
            if (Settings.Default.TabWidth > 10)
            {
                tab.FixedWidth = Settings.Default.TabWidth;
                tab.Height = tab.GetPreferredSize(this.MainTabStrip.DisplayRectangle.Size).Height;
            }
            this.MainTabStrip.Items.Add(tab);
            return tab;
        }

        private Tab AddNewTab(Stream tabStream, bool performLayout)
        {
            using (XmlReader reader = XmlReader.Create(tabStream))
            {
                GeneralTab tab = TwoPanelContainer.ParseBookmark(reader);
                if (tab != null)
                {
                    TwoPanelContainer newTabContent = TwoPanelContainer.Create();
                    newTabContent.BeginLayout();
                    try
                    {
                        newTabContent.FixMouseWheel = Settings.Default.FixMouseWheel;
                        newTabContent.TabBookmark = tab;
                        return this.AddNewTab(newTabContent);
                    }
                    finally
                    {
                        newTabContent.EndLayout(performLayout);
                    }
                }
            }
            return null;
        }

        private void AppendPanelFolder(StringBuilder builder, VirtualFilePanel panel, bool selected)
        {
            if (selected)
            {
                builder.Append('[');
            }
            if (panel.CurrentFolder != null)
            {
                builder.Append(panel.CurrentFolder.Name);
            }
            else
            {
                builder.Append("Empty");
            }
            if (selected)
            {
                builder.Append(']');
            }
        }

        private void BookmarksDropDown_Opening(object sender, CancelEventArgs e)
        {
            if (!this.CheckUIState(UIState.HasBookmarks))
            {
                this.ReloadBookmarks((ToolStripDropDown) sender);
                this.SetUIState(UIState.HasBookmarks, true);
            }
        }

        private void BookmarksFolderChanged(object sender, FileSystemEventArgs e)
        {
            this.ResetBookmarks();
        }

        private bool CheckClipboard(ClipboardState state, Func<bool> checkClipboardFunc)
        {
            if (this.CheckUIState(UIState.CheckClipboardUsingTicks) && (Math.Abs((int) (Environment.TickCount - this.ClipbdTickCount)) > 250))
            {
                this.ClipbrdMask &= ~state;
                this.ClipbdTickCount = Environment.TickCount;
            }
            if ((this.ClipbrdMask & state) == 0)
            {
                try
                {
                    if (checkClipboardFunc())
                    {
                        this.ClipbrdState |= state;
                    }
                    else
                    {
                        this.ClipbrdState &= ~state;
                    }
                    this.SetUIState(UIState.CheckClipboardUsingTicks, !this.CheckUIState(UIState.CheckClipboardUsingMessages));
                }
                catch (ExternalException)
                {
                    this.ClipbrdState &= ~state;
                    this.ClipbdTickCount = Environment.TickCount;
                    this.SetUIState(UIState.CheckClipboardUsingTicks, true);
                }
                this.ClipbrdMask |= state;
            }
            return ((this.ClipbrdState & state) > 0);
        }

        private void CheckPluginInstall(IVirtualFolder folder)
        {
            ArchiveFolder source = folder as ArchiveFolder;
            if (source != null)
            {
                Exception exception;
                try
                {
                    IChangeVirtualFile file = source.FromName("pluginst.inf") as IChangeVirtualFile;
                    if (file != null)
                    {
                        Ini.IniSection section;
                        using (Stream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.SequentialScan, 0L))
                        {
                            using (TextReader reader = new StreamReader(stream, Encoding.Default))
                            {
                                Ini ini = new Ini();
                                ini.Read(reader);
                                section = ini["plugininstall"];
                            }
                        }
                        if (section != null)
                        {
                            string str = section["type"];
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    string str2 = str.ToUpper();
                                    if (str2 != null)
                                    {
                                        if (!(str2 == "WCX"))
                                        {
                                            if (str2 == "WDX")
                                            {
                                                goto Label_0113;
                                            }
                                        }
                                        else
                                        {
                                            this.InstallWcxPlugin(section, source);
                                        }
                                    }
                                    return;
                                Label_0113:
                                    this.InstallWdxPlugin(section, source);
                                }
                                catch (Exception exception1)
                                {
                                    exception = exception1;
                                    ApplicationException error = new ApplicationException(string.Format(Resources.sErrorInstallingPlugin, exception.Message), exception);
                                    MessageDialog.ShowException(this, error, true);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                }
            }
        }

        private bool CheckUIState(UIState mask)
        {
            return ((this.FUpdateUIMask & mask) == mask);
        }

        private void CleanupDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            down.SuspendLayout();
            try
            {
                IDisposable tag = down.Tag as IDisposable;
                if (tag != null)
                {
                    tag.Dispose();
                }
                down.Tag = null;
                if (down.Items.Count == 0)
                {
                    down.Items.Add(new ToolStripMenuItem());
                }
            }
            finally
            {
                down.ResumeLayout();
            }
        }

        private static void CleanupToolbar(ToolStrip toolbar)
        {
            int num = toolbar.Items.Count - 1;
            while ((num >= 0) && (toolbar.Items[num] is ToolStripSeparator))
            {
                toolbar.Items.RemoveAt(num--);
            }
            for (int i = toolbar.Items.Count - 1; i > 0; i--)
            {
                if ((toolbar.Items[i] is ToolStripSeparator) && (toolbar.Items[i - 1] is ToolStripSeparator))
                {
                    toolbar.Items.RemoveAt(i);
                }
            }
        }

        private bool ClipboardContainText()
        {
            return (((Clipboard.ContainsText(TextDataFormat.Text) || Clipboard.ContainsText(TextDataFormat.UnicodeText)) || (Clipboard.ContainsText(TextDataFormat.Rtf) || Clipboard.ContainsText(TextDataFormat.Html))) || Clipboard.ContainsText(TextDataFormat.CommaSeparatedValue));
        }

        private void CloseTabs(IContainer tabsToClose)
        {
            LockWindowRedraw redraw;
            bool flag = !Settings.Default.AlwaysShowTabStrip && ((this.MainPageSwitcher.Controls.Count - tabsToClose.Components.Count) == 1);
            if (tabsToClose.Components.Count < 2)
            {
                using (redraw = flag ? new LockWindowRedraw(this, true) : null)
                {
                    tabsToClose.Dispose();
                }
            }
            else
            {
                MessageDialogResult yes = MessageDialogResult.Yes;
                if (ConfirmationSettings.Default.CloseTabs)
                {
                    bool checkBoxChecked = false;
                    yes = MessageDialog.Show(this, string.Format(Resources.sAskCloseTabs, tabsToClose.Components.Count), Resources.sConfirmCloseTabs, Resources.sDoNotAskAgain, ref checkBoxChecked, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question);
                    if (checkBoxChecked)
                    {
                        ConfirmationSettings.Default.CloseTabs = false;
                    }
                }
                if (yes == MessageDialogResult.Yes)
                {
                    using (redraw = flag ? new LockWindowRedraw(this, true) : null)
                    {
                        this.MainPageSwitcher.SuspendLayout();
                        this.MainTabStrip.SuspendLayout();
                        tabsToClose.Dispose();
                        this.MainTabStrip.ResumeLayout();
                        this.MainPageSwitcher.ResumeLayout();
                    }
                }
            }
        }

        private void cmsTab_Opening(object sender, CancelEventArgs e)
        {
            Tab itemAt = this.MainTabStrip.GetItemAt(this.MainTabStrip.PointToClient(Cursor.Position)) as Tab;
            if (itemAt == null)
            {
                e.Cancel = true;
            }
            else if (!itemAt.Checked)
            {
                itemAt.PerformClick();
            }
        }

        private void cmsToolbar_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ToolStripItem tag = (ToolStripItem) this.tsmiRemoveToolbarButton.Tag;
            if (tag != null)
            {
                tag.Invalidate();
            }
        }

        private void cmsToolbar_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            ToolStrip sourceControl = (ToolStrip) strip.SourceControl;
            if (sourceControl == null)
            {
                if (strip.OwnerItem == this.tsmiToolbars)
                {
                    sourceControl = this.MainMenu;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            ToolStripItem itemAt = null;
            if (sourceControl != this.MainMenu)
            {
                itemAt = sourceControl.GetItemAt(sourceControl.PointToClient(Cursor.Position));
            }
            if ((itemAt != null) && (itemAt.Name.StartsWith("Bookmark_", StringComparison.Ordinal) || itemAt.Name.StartsWith("Drive_", StringComparison.Ordinal)))
            {
                e.Cancel = true;
            }
            else
            {
                strip.SuspendLayout();
                try
                {
                    strip.Items.Clear();
                    IDisposableContainer container = new DisposableContainer();
                    strip.Tag = container;
                    this.tsmiRemoveToolbarButton.Tag = null;
                    if ((sourceControl != this.MainMenu) && (itemAt != null))
                    {
                        this.tsmiRemoveToolbarButton.Tag = itemAt;
                        strip.Items.Add(this.tsmiRemoveToolbarButton);
                        strip.Items.Add(this.tssToolbar1);
                        if (!(itemAt is ToolStripSeparator))
                        {
                            strip.Items.Add(this.tsmiToolbarButtonImage);
                            strip.Items.Add(this.tsmiToolbarButtonImageAndText);
                            strip.Items.Add(this.tsmiToolbarButtonText);
                            strip.Items.Add(this.tssToolbar2);
                            strip.Items.Add(this.tsmiChangeButtonImage);
                            strip.Items.Add(this.tssToolbar3);
                            this.tsmiToolbarButtonImage.Enabled = itemAt.Image != null;
                            this.tsmiToolbarButtonImageAndText.Enabled = this.tsmiToolbarButtonImage.Enabled;
                            this.tsmiToolbarButtonText.Enabled = this.tsmiToolbarButtonImage.Enabled;
                        }
                        itemAt.Invalidate();
                    }
                    if (sourceControl != this.MainMenu)
                    {
                        strip.Items.Add(this.tsmiToolbarMoveToTop);
                        strip.Items.Add(this.tsmiToolbarMoveToBottom);
                        strip.Items.Add(this.tssToolbar4);
                        strip.Items.Add(this.tsmiJustifyToolbar);
                        strip.Items.Add(this.tssToolbar5);
                        this.tsmiToolbarMoveToTop.Enabled = sourceControl.Dock != DockStyle.Top;
                        this.tsmiToolbarMoveToBottom.Enabled = sourceControl.Dock != DockStyle.Bottom;
                    }
                    Font font = null;
                    strip.Items.Add(this.tsmiMainMenuVisible);
                    if (sourceControl == this.MainMenu)
                    {
                        container.Add(font ?? (font = new Font(this.tsmiMainMenuVisible.Font, FontStyle.Bold)));
                        this.tsmiMainMenuVisible.Font = font;
                    }
                    else
                    {
                        this.tsmiMainMenuVisible.ResetFont();
                    }
                    bool flag = false;
                    foreach (ToolStripItem item2 in this.CreateToolbarItemList())
                    {
                        if (item2.Tag == sourceControl)
                        {
                            container.Add(font ?? (font = new Font(item2.Font, FontStyle.Bold)));
                            item2.Font = font;
                        }
                        container.Add(item2);
                        strip.Items.Add(item2);
                        flag = true;
                    }
                    if (!flag)
                    {
                        strip.Items.Add(this.tssToolbar6);
                        strip.Items.Add(this.tsmiNoToolbars);
                    }
                    strip.Items.Add(this.tssToolbar7);
                    if ((itemAt != null) && itemAt.Name.StartsWith("Tool_", StringComparison.Ordinal))
                    {
                        strip.Items.Add(this.tsmiCustomizeTools2);
                    }
                    strip.Items.Add(this.tsmiCustomizeToolbars2);
                }
                finally
                {
                    strip.ResumeLayout();
                }
            }
        }

        private void cmsViewAs_Opening(object sender, CancelEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            bool flag = (down.OwnerItem == null) || (down.OwnerItem.OwnerItem != this.tsmiView);
            down.SuspendLayout();
            try
            {
                down.Items.Clear();
                if (flag)
                {
                    down.Items.AddRange(new ToolStripItem[] { this.tsmiViewAsThumbnail2, this.tsmiViewAsLargeIcon2, this.tsmiViewAsSmallIcon2, this.tsmiViewAsList2, this.tsmiViewAsDetails2 });
                }
                IContainer container = null;
                switch (this.CurrentPanel.View)
                {
                    case PanelView.Details:
                        if (flag)
                        {
                            down.Items.Add(this.tssViewAs1);
                        }
                        down.Items.Add(this.tsmiManageColumns2);
                        break;

                    case PanelView.List:
                        if (flag)
                        {
                            down.Items.Add(this.tssViewAs1);
                        }
                        down.Items.AddRange(new ToolStripMenuItem[] { this.tsmiSetOneListColumn2, this.tsmiSetTwoListColumns2, this.tsmiSetThreeListColumns2, this.tsmiSetFourListColumns2, this.tsmiSetFiveListColumns2, this.tsmiSetSixListColumns2, this.tsmiSetSevenListColumns2, this.tsmiSetEightListColumns2, this.tsmiSetNineListColumns2 });
                        break;

                    case PanelView.Thumbnail:
                    {
                        NameValueCollection section = ConfigurationManager.GetSection("thumbnailSizeTemplates") as NameValueCollection;
                        if (section != null)
                        {
                            if (flag)
                            {
                                down.Items.Add(this.tssViewAs1);
                            }
                            container = new Container();
                            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Rectangle));
                            for (int i = 0; i < section.Count; i++)
                            {
                                Size thumbnailSize = this.CurrentPanel.ThumbnailSize;
                                Rectangle rectangle = (Rectangle) converter.ConvertFromInvariantString(section[i]);
                                ToolStripMenuItem item = new ToolStripMenuItem();
                                string str = Resources.ResourceManager.GetString(section.Keys[i]);
                                if (string.IsNullOrEmpty(str))
                                {
                                    str = section.Keys[i];
                                }
                                item.Text = str;
                                item.Checked = (thumbnailSize.Width == rectangle.Left) && (thumbnailSize.Height == rectangle.Top);
                                item.Tag = rectangle;
                                item.Click += new EventHandler(this.ThumbnailSizeMenuItem_Click);
                                down.Items.Add(item);
                                container.Add(item);
                            }
                        }
                        break;
                    }
                }
                down.Tag = container;
            }
            finally
            {
                down.ResumeLayout();
            }
        }

        private IEnumerable<ToolStripItem> CreateBookmarkList(string bookmarkFolder, System.Type itemType, ToolStripItemDisplayStyle displayStyle)
        {
            return new <CreateBookmarkList>d__26(-2) { <>4__this = this, <>3__bookmarkFolder = bookmarkFolder, <>3__itemType = itemType, <>3__displayStyle = displayStyle };
        }

        private void CreateFolderContextMenu(ContextMenuStrip strip, IVirtualItem target, string commands)
        {
            int num = 0;
            using (TextReader reader = new StringReader(commands))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    string str2;
                    Action action;
                    ToolStripItem component = null;
                    IconLocation imageLocation = null;
                    ToolStripItemDisplayStyle imageAndText = ToolStripItemDisplayStyle.ImageAndText;
                    string str3 = str;
                    if (str3 == null)
                    {
                        goto Label_0063;
                    }
                    if (str3 != "")
                    {
                        if (!(str3 == "?"))
                        {
                            if (str3 == "-")
                            {
                                goto Label_0058;
                            }
                            goto Label_0063;
                        }
                        num = -1;
                    }
                    continue;
                Label_0058:
                    component = new ToolStripSeparator();
                    goto Label_016C;
                Label_0063:
                    if (!ToolbarSettings.ParseToolbarButtonLine(str, out str2, ref imageAndText, out imageLocation))
                    {
                        continue;
                    }
                    if (str2.StartsWith("act", StringComparison.OrdinalIgnoreCase) && this.ActionMap.TryGetValue(str2, out action))
                    {
                        component = new ToolStripMenuItem();
                        this.actionManager.SetAction(component, action, target, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                    }
                    if ((component == null) && str2.StartsWith("tsmi", StringComparison.OrdinalIgnoreCase))
                    {
                        ToolStripItem[] itemArray = this.MainMenu.Items.Find(str2, true);
                        ToolStripMenuItem item2 = (itemArray.Length > 0) ? (itemArray[0] as ToolStripMenuItem) : null;
                        if ((item2 != null) && (item2.DropDownItems.Count > 0))
                        {
                            component = new ToolStripMenuItem {
                                Text = item2.Text,
                                Image = item2.Image,
                                DropDown = item2.DropDown
                            };
                        }
                    }
                Label_016C:
                    if (component != null)
                    {
                        if (imageLocation != null)
                        {
                            component.SetTag(2, imageLocation);
                            this.SetToolbarButtonImage(component, imageLocation);
                        }
                        component.DisplayStyle = imageAndText;
                        if (num >= 0)
                        {
                            strip.Items.Insert(num++, component);
                        }
                        else
                        {
                            strip.Items.Add(component);
                        }
                    }
                }
            }
            CleanupToolbar(strip);
        }

        private static bool CreateLinkName(IVirtualFolder destFolder, LinkType linkType, ref string originalName)
        {
            VirtualItemType file;
            bool flag = true;
            string str = originalName;
            switch (linkType)
            {
                case LinkType.Default:
                case LinkType.HardLink:
                case LinkType.SymbolicLink:
                    file = VirtualItemType.File;
                    break;

                case LinkType.ShellFolderLink:
                case LinkType.JuntionPoint:
                    file = VirtualItemType.Folder;
                    break;

                default:
                    throw new ApplicationException("Unsupported Link Type.");
            }
            IPersistVirtualItem item = VirtualItem.FromFullName(Path.Combine(destFolder.FullName, originalName), file) as IPersistVirtualItem;
            int num = 2;
            while ((item != null) && item.Exists)
            {
                flag = false;
                str = string.Format(Settings.Default.AnotherLinkPattern, Path.GetFileNameWithoutExtension(originalName), Path.GetExtension(originalName), num++);
                item = VirtualItem.FromFullName(Path.Combine(destFolder.FullName, str), file) as IPersistVirtualItem;
            }
            originalName = str;
            return flag;
        }

        private IEnumerable<ToolStripItem> CreateToolbarItemList()
        {
            return new <CreateToolbarItemList>d__a(-2) { <>4__this = this };
        }

        private IEnumerable<ToolStripItem> CreateToolList(string toolFolder, System.Type itemType, ToolStripItemDisplayStyle displayStyle)
        {
            return new <CreateToolList>d__1c(-2) { <>4__this = this, <>3__toolFolder = toolFolder, <>3__itemType = itemType, <>3__displayStyle = displayStyle };
        }

        private static string CreateToolTipText(string text, Keys shortcutKeys)
        {
            if (shortcutKeys == Keys.None)
            {
                return text;
            }
            StringBuilder builder = new StringBuilder(text);
            TypeConverter converter = TypeDescriptor.GetConverter(shortcutKeys);
            builder.Append(" (");
            builder.Append(converter.ConvertToString(shortcutKeys));
            builder.Append(')');
            return builder.ToString();
        }

        private void CurrentPanel_DragDropOnItem(object sender, VirtualItemDragEventArg e)
        {
            IEnumerable<IVirtualItem> dataObjectItems;
            IVirtualFolder item = e.Item as IVirtualFolder;
            if (item != null)
            {
                dataObjectItems = VirtualClipboardItem.GetDataObjectItems(e.Data);
                if (dataObjectItems != null)
                {
                    if (e.Item is ArchiveFolder)
                    {
                        this.ShowPackDialog(item, dataObjectItems, null);
                    }
                    else if (e.Effect == DragDropEffects.Link)
                    {
                        this.DoCreateLinks(item, dataObjectItems);
                    }
                    else if (!(ConfirmationSettings.Default.DragDrop || !VirtualItemHelper.CanCreateInFolder(item)))
                    {
                        CopyWorkerOptions options = ~CopyWorkerOptions.DeleteSource;
                        DoStartCopy(item, dataObjectItems, (CopySettings.Default.DefaultCopyOptions & options) | ((e.Effect == DragDropEffects.Move) ? 1 : 0), null, null, null);
                    }
                    else
                    {
                        this.ShowFileCopyDialog(item, dataObjectItems, e.Effect == DragDropEffects.Move);
                    }
                }
                else if (PasteNewFileDialog.HasSupportedFormats(e.Data) && (item is ICreateVirtualFile))
                {
                    if (ConfirmationSettings.Default.DragDrop)
                    {
                        this.ShowPasteDialog(item, e.Data, "Drop");
                    }
                    else
                    {
                        this.PasteObject(item, "Drop", e.Data);
                    }
                }
            }
            else
            {
                IVirtualFileExecute execute = e.Item as IVirtualFileExecute;
                if (((execute != null) && execute.CanExecuteEx) && e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string str in e.Data.GetData(DataFormats.FileDrop) as IEnumerable)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append(' ');
                        }
                        if (str.IndexOf(' ') >= 0)
                        {
                            builder.Append('"');
                            builder.Append(str);
                            builder.Append('"');
                        }
                        else
                        {
                            builder.Append(str);
                        }
                    }
                    if (!ConfirmationSettings.Default.DragDrop)
                    {
                        this.RunAs(this, execute, builder.ToString(), ExecuteAsUser.CurrentUser, null, null, Settings.Default.RunInThread);
                    }
                    else
                    {
                        this.ShowRunAsDialog(execute, builder.ToString());
                    }
                }
                else
                {
                    ArchiveFormatInfo format = null;
                    IChangeVirtualFile destItem = e.Item as IChangeVirtualFile;
                    if (destItem != null)
                    {
                        foreach (FindFormatResult result in ArchiveFormatManager.FindFormat(destItem.Extension))
                        {
                            if ((result.Format.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0)
                            {
                                format = result.Format;
                                break;
                            }
                        }
                    }
                    if (format != null)
                    {
                        dataObjectItems = VirtualClipboardItem.GetDataObjectItems(e.Data);
                        if (dataObjectItems != null)
                        {
                            this.ShowPackDialog(destItem, dataObjectItems, format);
                        }
                    }
                }
            }
        }

        private void CurrentPanel_DragOverItem(object sender, VirtualItemDragEventArg e)
        {
            IVirtualFileExecute item = e.Item as IVirtualFileExecute;
            if ((item != null) && item.CanExecuteEx)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                ArchiveFolder folder = e.Item as ArchiveFolder;
                if (folder != null)
                {
                    ArchiveFormatInfo info = folder[ArchiveProperty.ArchiveFormat] as ArchiveFormatInfo;
                    if ((info == null) || ((info.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0))
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
                IVirtualFolder folder2 = e.Item as IVirtualFolder;
                if (folder2 != null)
                {
                    switch (e.Effect)
                    {
                        case DragDropEffects.Copy:
                        case DragDropEffects.Move:
                            if (!VirtualItemHelper.CanCreateInFolder(folder2))
                            {
                                break;
                            }
                            return;

                        case DragDropEffects.Link:
                            if (VirtualItemHelper.CanCreateLinkIn(VirtualClipboardItem.GetDataObjectItems(e.Data), folder2) == CanMoveResult.None)
                            {
                                break;
                            }
                            return;
                    }
                }
                IChangeVirtualFile file = e.Item as IChangeVirtualFile;
                if (file != null)
                {
                    foreach (FindFormatResult result in ArchiveFormatManager.FindFormat(file.Extension))
                    {
                        if ((result.Format.Capabilities & ArchiveFormatCapabilities.UpdateArchive) > 0)
                        {
                            e.Effect = DragDropEffects.Copy;
                            return;
                        }
                    }
                }
                e.Effect = DragDropEffects.None;
            }
        }

        private void CurrentPanel_ExecuteItem(object sender, HandleVirtualItemEventArgs e)
        {
            e.Handled = this.ExecuteItem(e.Item, Control.ModifierKeys == Keys.Shift);
        }

        private void CurrentPanel_PreviewContextMenu(object sender, PreviewContextMenuEventArgs e)
        {
            EventHandler<ExecuteVerbEventArgs> onExecuteVerb = null;
            CancelEventHandler handler2 = null;
            ToolStripDropDownClosedEventHandler handler3 = null;
            string ContextMenuCommands;
            VirtualFilePanel panel = sender as VirtualFilePanel;
            if (panel != null)
            {
                ContextMenuCommands = null;
                if (e.Item.Equals(panel.CurrentFolder))
                {
                    e.Options |= ContextMenuOptions.VerbsOnly;
                    ContextMenuCommands = Settings.Default.FolderContextMenuCommands;
                }
                else if (e.Item.IsPropertyAvailable(12) || e.Item.IsPropertyAvailable(10))
                {
                    ContextMenuCommands = this.actOpenContainingFolder.Name + "\r\n-\r\n?";
                }
                if (!string.IsNullOrEmpty(ContextMenuCommands))
                {
                    IVirtualItemUI item = e.Item as IVirtualItemUI;
                    if (item != null)
                    {
                        if (onExecuteVerb == null)
                        {
                            onExecuteVerb = delegate (object sender1, ExecuteVerbEventArgs e1) {
                                if (e1.Verb == "rename")
                                {
                                    this.actRenameSingleItem.Execute(e.Item);
                                    e1.Handled = true;
                                }
                            };
                        }
                        e.ContextMenu = item.CreateContextMenuStrip(this, e.Options, onExecuteVerb);
                    }
                    if (e.ContextMenu == null)
                    {
                        e.ContextMenu = new ContextMenuStrip();
                        this.CreateFolderContextMenu(e.ContextMenu, e.Item, ContextMenuCommands);
                    }
                    else
                    {
                        if (handler2 == null)
                        {
                            handler2 = delegate (object sender1, CancelEventArgs e1) {
                                ContextMenuStrip strip = (ContextMenuStrip) sender1;
                                this.CreateFolderContextMenu(strip, e.Item, ContextMenuCommands);
                            };
                        }
                        e.ContextMenu.Opening += handler2;
                    }
                    if (handler3 == null)
                    {
                        handler3 = delegate (object sender1, ToolStripDropDownClosedEventArgs e1) {
                            IDisposable disposable1 = (IDisposable) sender1;
                            base.BeginInvoke(new MethodInvoker(disposable1.Dispose));
                        };
                    }
                    e.ContextMenu.Closed += handler3;
                }
            }
        }

        private void CurrentTab_CurrentFolderChanged(object sender, EventArgs e)
        {
            this.SetUpdateDriveButtonsNeeded();
            if (OS.IsWinVista)
            {
                this.SetUIState(UIState.GCCollectNeeded, true);
            }
            TwoPanelContainer container = (TwoPanelContainer) sender;
            if (string.IsNullOrEmpty(container.Text))
            {
                TabStripPage parent = (TabStripPage) container.Parent;
                foreach (ToolStripItem item in this.MainTabStrip.Items)
                {
                    Tab tab = item as Tab;
                    if ((tab != null) && (tab.TabStripPage == parent))
                    {
                        IVirtualFolder currentFolder = container.CurrentPanel.CurrentFolder;
                        if (currentFolder != null)
                        {
                            tab.Text = currentFolder.Name.Replace("&", "&&");
                        }
                        break;
                    }
                }
            }
        }

        private void CurrentTab_DragHover(object sender, DragEventArgs e)
        {
            if (VirtualClipboardItem.DataObjectContainItems(e.Data))
            {
                ((Tab) sender).PerformClick();
            }
        }

        private void CurrentTab_DragOver(object sender, DragEventArgs e)
        {
            if (VirtualClipboardItem.DataObjectContainItems(e.Data))
            {
                ((Tab) sender).Select();
            }
        }

        private bool DeleteSelection(IEnumerable<IVirtualItem> selection)
        {
            string sConfirmMultipleFileDelete;
            StringBuilder builder = new StringBuilder();
            int num = selection.Count<IVirtualItem>();
            if (num != 1)
            {
                sConfirmMultipleFileDelete = Resources.sConfirmMultipleFileDelete;
                builder.Append(PluralInfo.Format(Resources.sAskDeleteMultipleFile, new object[] { num }));
                builder.AppendLine();
                builder.AppendLine();
                int num2 = 0;
                foreach (IVirtualItem item2 in selection)
                {
                    if (++num2 > 5)
                    {
                        builder.AppendLine("...");
                        break;
                    }
                    builder.AppendLine(item2.Name);
                }
            }
            else
            {
                IVirtualItem item = null;
                foreach (IVirtualItem item2 in selection)
                {
                    item = item2;
                    break;
                }
                if (item is IVirtualFolder)
                {
                    sConfirmMultipleFileDelete = Resources.sConfirmFolderDelete;
                    builder.AppendFormat(Resources.sAskDeleteFolder, item.Name);
                }
                else
                {
                    sConfirmMultipleFileDelete = Resources.sConfirmFileDelete;
                    builder.AppendFormat(Resources.sAskDeleteFile, item.Name);
                    IVirtualItemUI mui = item as IVirtualItemUI;
                    if (mui != null)
                    {
                        string toolTip = mui.ToolTip;
                        if (!string.IsNullOrEmpty(toolTip))
                        {
                            builder.AppendLine();
                            builder.AppendLine();
                            builder.AppendLine(toolTip);
                        }
                    }
                }
                goto Label_01A0;
            }
            builder.AppendLine();
        Label_01A0:
            return this.DeleteSelection(selection, builder.ToString(), sConfirmMultipleFileDelete);
        }

        private bool DeleteSelection(IEnumerable<IVirtualItem> selection, string Text, string Caption)
        {
            MessageDialogResult result;
            bool checkBoxChecked = false;
            foreach (IVirtualItem item in selection)
            {
                IChangeVirtualItem item2 = item as IChangeVirtualItem;
                if ((item2 != null) && item2.CanSendToBin)
                {
                    checkBoxChecked = true;
                    break;
                }
            }
            MessageDialogResult[] buttons = new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.Cancel };
            Dictionary<MessageDialogResult, string> buttonTextMap = new Dictionary<MessageDialogResult, string>(1);
            buttonTextMap.Add(MessageDialogResult.Yes, Resources.sMessageButtonDelete);
            if (checkBoxChecked)
            {
                checkBoxChecked = Settings.Default.DeleteToBin ^ ((Control.ModifierKeys & Keys.Shift) > Keys.None);
                result = MessageDialog.Show(this, Text, Caption, Resources.sDeleteToBin, ref checkBoxChecked, buttons, buttonTextMap, Resources.ConfirmDelete, Settings.Default.DefaultDeleteDialogButton);
            }
            else
            {
                result = MessageDialog.Show(this, Text, Caption, buttons, buttonTextMap, MessageBoxIcon.Question, Settings.Default.DefaultDeleteDialogButton);
            }
            if (result != MessageDialogResult.Yes)
            {
                return false;
            }
            DeleteWorkerDialog.ShowAsync(new DeleteWorker(selection, checkBoxChecked)).DeleteWorker.RunAsync(ThreadPriority.Normal);
            return true;
        }

        private bool DiscardSpecialKey(Keys keyData)
        {
            return (this.SpecialKeyMap.ContainsKey(keyData) && !(Control.FromHandle(Windows.GetFocus()) is ListView));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoCreateLink(IVirtualFolder destFolder, ICreateVirtualLink createLink, LinkType linkType, string linkName)
        {
            if ((createLink.CanCreateLinkIn(destFolder) & linkType) != LinkType.None)
            {
                try
                {
                    if (linkName == null)
                    {
                        linkName = createLink.GetPrefferedLinkName(linkType);
                    }
                    string NewName = linkName;
                    bool flag = CreateLinkName(destFolder, linkType, ref NewName);
                    if (!(flag || ConfirmationSettings.Default.CreateAnotherLink))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        bool checkBoxChecked = false;
                        flag = MessageDialog.Show(this, string.Format(Resources.sAskCreateAnotherLink, linkName, destFolder.FullName, NewName), Resources.sConfirmCreateAnotherLink, Resources.sAlwaysCreateAnotherLink, ref checkBoxChecked, MessageDialog.ButtonsYesNo, MessageBoxIcon.Exclamation) == MessageDialogResult.Yes;
                        if (checkBoxChecked)
                        {
                            ConfirmationSettings.Default.CreateAnotherLink = false;
                        }
                    }
                    if (flag)
                    {
                        this.ExecuteElevated((IVirtualItem) createLink, delegate {
                            WaitCursor.ShowUntilIdle();
                            createLink.CreateLink(destFolder, NewName, linkType);
                        });
                    }
                }
                catch (Exception exception)
                {
                    MessageDialog.ShowException(this, exception, VirtualItem.IsWarningIOException(exception));
                }
            }
        }

        private void DoCreateLinks(IVirtualFolder destFolder, IEnumerable<IVirtualItem> items)
        {
            MessageDialogResult yes = ConfirmationSettings.Default.CreateAnotherLink ? MessageDialogResult.No : MessageDialogResult.Yes;
            ElevatedProcess process = null;
            using (new WaitCursor(this))
            {
                foreach (IVirtualItem item in items)
                {
                    LinkType NextType = LinkType.None;
                    ICreateVirtualLink CreateLink = item as ICreateVirtualLink;
                    if (CreateLink != null)
                    {
                        NextType = CreateLink.CanCreateLinkIn(destFolder) & LinkType.Default;
                    }
                    if (NextType > LinkType.None)
                    {
                        try
                        {
                            MethodInvoker action = null;
                            string prefferedLinkName = CreateLink.GetPrefferedLinkName(NextType);
                            string NewName = prefferedLinkName;
                            MessageDialogResult yesToAll = CreateLinkName(destFolder, NextType, ref NewName) ? MessageDialogResult.Yes : MessageDialogResult.No;
                            if (yesToAll == MessageDialogResult.No)
                            {
                                yesToAll = yes;
                            }
                            if (yesToAll == MessageDialogResult.No)
                            {
                                bool checkBoxChecked = false;
                                yesToAll = MessageDialog.Show(this, string.Format(Resources.sAskCreateAnotherLink, prefferedLinkName, destFolder.FullName, NewName), Resources.sConfirmCreateAnotherLink, Resources.sAlwaysCreateAnotherLink, ref checkBoxChecked, new MessageDialogResult[] { MessageDialogResult.Yes, MessageDialogResult.YesToAll, MessageDialogResult.Skip, MessageDialogResult.Cancel }, MessageBoxIcon.Exclamation);
                                if (checkBoxChecked)
                                {
                                    ConfirmationSettings.Default.CreateAnotherLink = false;
                                    if (yesToAll == MessageDialogResult.Yes)
                                    {
                                        yesToAll = MessageDialogResult.YesToAll;
                                    }
                                }
                            }
                            switch (yesToAll)
                            {
                                case MessageDialogResult.Yes:
                                case MessageDialogResult.YesToAll:
                                    if (yesToAll == MessageDialogResult.YesToAll)
                                    {
                                        yes = MessageDialogResult.Yes;
                                    }
                                    if (action == null)
                                    {
                                        action = delegate {
                                            CreateLink.CreateLink(destFolder, NewName, NextType);
                                        };
                                    }
                                    this.ExecuteElevated(item, ref process, true, action);
                                    goto Label_0224;

                                case MessageDialogResult.No:
                                case MessageDialogResult.NoToAll:
                                    return;

                                case MessageDialogResult.Skip:
                                    if (Control.ModifierKeys == Keys.Shift)
                                    {
                                        yes = MessageDialogResult.Skip;
                                    }
                                    goto Label_0224;
                            }
                            return;
                        }
                        catch (Exception exception)
                        {
                            MessageDialog.ShowException(this, exception);
                        }
                    Label_0224:;
                    }
                }
            }
        }

        private static void DoStartCopy(IVirtualFolder destFolder, IEnumerable<IVirtualItem> items, CopyWorkerOptions copyOptions, IVirtualItemFilter filter, IRenameFilter renameFilter, IOverwriteRule[] defaultOverwriteRules)
        {
            CopyWorker worker = new CopyWorker(items, destFolder, CopySettings.Default, copyOptions | (CopySettings.Default.DefaultCopyOptions & CopyWorkerOptions.UseSystemCopy), filter, renameFilter);
            CopyWorkerDialog dialog = CopyWorkerDialog.ShowAsync(worker);
            if (defaultOverwriteRules != null)
            {
                dialog.DefaultOverwriteRules = new List<IOverwriteRule>(defaultOverwriteRules);
            }
            worker.RunAsync(ThreadPriority.Normal);
        }

        private void DrawKeyboardCue(Graphics g, ToolStripItem item, string text, Font textFont)
        {
            Size size = TextRenderer.MeasureText(g, text, textFont);
            size.Height++;
            Rectangle bounds = item.Bounds;
            bounds = new Rectangle(bounds.Left + ((bounds.Width - size.Width) / 2), (item.Owner.ClientRectangle.Bottom - size.Height) - 1, size.Width, size.Height);
            using (GraphicsPath path = GraphicsHelper.RoundRect(bounds, 2f))
            {
                System.Drawing.Color color = item.Enabled ? SystemColors.InfoText : SystemColors.GrayText;
                g.FillPath(SystemBrushes.Info, path);
                using (Pen pen = new Pen(color))
                {
                    g.DrawPath(pen, path);
                }
                TextRenderer.DrawText(g, text, textFont, new Point(bounds.Left + 1, bounds.Top + 1), color, SystemColors.Info);
            }
        }

        private void DrawKeyboardCues(ToolStrip strip, Dictionary<ToolStripItem, Keys> buttonKeyMap)
        {
            using (Graphics graphics = Graphics.FromHwnd(strip.Handle))
            {
                this.DrawKeyboardCues(strip, graphics, buttonKeyMap);
            }
        }

        private void DrawKeyboardCues(ToolStrip strip, Graphics graphics, Dictionary<ToolStripItem, Keys> buttonKeyMap)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
            using (Font font = new Font(strip.Font.FontFamily, 6.5f))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                foreach (ToolStripItem item in strip.Items)
                {
                    Keys keys;
                    if (buttonKeyMap.TryGetValue(item, out keys))
                    {
                        string text = converter.ConvertToString(keys);
                        this.DrawKeyboardCue(graphics, item, text, font);
                    }
                }
            }
        }

        private void DriveButton_Click(object sender, EventArgs e)
        {
            string str;
            IVirtualFolder tag = (IVirtualFolder) ((ToolStripItem) sender).Tag;
            bool flag = true;
            if (((Control.ModifierKeys & Keys.Shift) == Keys.None) && VirtualItem.DefaultDrivePath.TryGetValue(tag.FullName, out str))
            {
                flag = !this.CurrentPanel.SetCurrentFolder(str, VirtualItemType.Folder);
            }
            if (flag)
            {
                IPersistVirtualItem item = tag as IPersistVirtualItem;
                if ((item == null) || item.Exists)
                {
                    this.CurrentPanel.CurrentFolder = tag;
                }
            }
        }

        private static string EnquoteString(string value)
        {
            if (value.IndexOf(' ') >= 0)
            {
                return ('"' + value + '"');
            }
            return value;
        }

        public void Event_ApplicationIdle(object sender, EventArgs e)
        {
            if (!(!this.CheckUIState(UIState.IsToolbarsDirty) || this.CheckUIState(UIState.UpdatingToolbars)))
            {
                this.SetUIState(UIState.UpdatingToolbars, true);
                base.BeginInvoke(new MethodInvoker(this.UpdateToolbars));
            }
            Keys none = Keys.None;
            if ((Settings.Default.IsShowKeyboardCues && base.ContainsFocus) && (Form.ActiveForm == this))
            {
                none = Control.ModifierKeys;
            }
            if (this.ControlModifierKeys != none)
            {
                this.ControlModifierKeys = none;
                foreach (ToolStrip strip in this.Toolbars)
                {
                    if (strip.Visible)
                    {
                        strip.Invalidate();
                    }
                }
                if (this.MainTabStrip.Visible)
                {
                    this.MainTabStrip.Invalidate();
                }
                this.MainMenu.Invalidate();
            }
            if (this.CheckUIState(UIState.ShowCrushLogNeeded))
            {
                base.BeginInvoke(new MethodInvoker(this.ShowCrushLog));
            }
            if (this.CheckUIState(UIState.StartController))
            {
                this.SetUIState(UIState.StartController, false);
                Controller.StartServer();
            }
            if (this.CheckUIState(UIState.IsCuttedItemsDirty))
            {
                this.SetUIState(UIState.IsCuttedItemsDirty, false);
                IEnumerable<IVirtualItem> items = null;
                if (this.CheckClipboard(ClipboardState.ContainsItems, delegate {
                    return VirtualClipboardItem.ClipboardContainItems();
                }))
                {
                    try
                    {
                        if (VirtualItemDataObject.ReadInt32FromClipboard("Preferred DropEffect") == 2)
                        {
                            items = VirtualClipboardItem.GetClipboardItems();
                        }
                    }
                    catch (Exception exception)
                    {
                        if (!(VirtualItem.IsWarningIOException(exception) || (exception is ExternalException)))
                        {
                            throw;
                        }
                    }
                }
                VirtualFilePanel.SetCuttedItems(items);
            }
            if (this.CheckUIState(UIState.GCCollectNeeded))
            {
                this.SetUIState(UIState.GCCollectNeeded, false);
                if (LocalFileSystemCreator.Sponsor.ActiveClientCount > 0)
                {
                    GC.Collect();
                }
            }
        }

        private bool ExecuteElevated(IVirtualItem elevatableObj, MethodInvoker action)
        {
            ElevatedProcess process = null;
            return this.ExecuteElevated(elevatableObj, ref process, false, action);
        }

        private bool ExecuteElevated(IVirtualItem elevatableObj, ref ElevatedProcess process, bool remember, MethodInvoker action)
        {
            try
            {
                action();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                IElevatable elevatable = elevatableObj as IElevatable;
                if ((elevatable == null) || !elevatable.CanElevate)
                {
                    throw;
                }
                if (process == null)
                {
                    MessageDialogResult result;
                    MessageDialogResult[] buttons = new MessageDialogResult[] { MessageDialogResult.Shield, MessageDialogResult.Cancel };
                    if (remember)
                    {
                        result = MessageDialog.Show(this, string.Format(Resources.sAskElevateOperationPermissions, elevatableObj.FullName), Resources.sWarning, Resources.sDoThisForAll, ref remember, buttons, MessageBoxIcon.Exclamation, MessageDialogResult.Shield);
                    }
                    else
                    {
                        result = MessageDialog.Show(this, string.Format(Resources.sAskElevateOperationPermissions, elevatableObj.FullName), Resources.sWarning, buttons, MessageBoxIcon.Exclamation, MessageDialogResult.Shield);
                    }
                    if (result == MessageDialogResult.Shield)
                    {
                        process = new ElevatedProcess();
                    }
                    else
                    {
                        return false;
                    }
                }
                if (!elevatable.Elevate(process))
                {
                    throw;
                }
            }
            finally
            {
                if (!remember)
                {
                    process = null;
                }
            }
            action();
            return true;
        }

        private bool ExecuteItem(IVirtualItem item, bool alternate)
        {
            Exception exception;
            IChangeVirtualFile archiveFile = this.GetLocalSystemItemCopy(item, true, alternate);
            if (archiveFile == null)
            {
                return false;
            }
            if (".tab".Equals(Path.GetExtension(item.Name), StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (Stream stream = archiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.None, 0L))
                    {
                        Tab selectedTab = this.MainTabStrip.SelectedTab;
                        this.MainTabStrip.SuspendLayout();
                        Tab tab2 = this.AddNewTab(stream, true);
                        if (tab2 != null)
                        {
                            tab2.PerformClick();
                            this.MainTabStrip.ResumeLayout();
                            this.FPreviousTab = selectedTab;
                            return true;
                        }
                        this.MainTabStrip.ResumeLayout(false);
                    }
                }
                catch (XmlException)
                {
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    if (!VirtualItem.IsWarningIOException(exception))
                    {
                        throw;
                    }
                }
            }
            bool flag = false;
            try
            {
                IVirtualFolder folder = null;
                if (Settings.Default.EnterOpensArchive)
                {
                    folder = VirtualItem.OpenArchive(archiveFile, this.CurrentPanel.CurrentFolder, !alternate);
                }
                else if (alternate)
                {
                    folder = VirtualItem.OpenArchive(archiveFile, this.CurrentPanel.CurrentFolder);
                }
                if (folder != null)
                {
                    this.CurrentPanel.CurrentFolder = folder;
                    this.CheckPluginInstall(folder);
                    flag = true;
                }
                else
                {
                    IVirtualFileExecute file = archiveFile as IVirtualFileExecute;
                    if (file != null)
                    {
                        bool flag2 = ((item is FtpFile) && !PathHelper.IsExecutableFile(item.Name)) && (ConfirmationSettings.Default.UploadChangedFileBack != MessageDialogResult.No);
                        CheckState runInThread = flag2 ? CheckState.Unchecked : Settings.Default.RunInThread;
                        Process watchProcess = this.RunAs(this, file, null, ExecuteAsUser.CurrentUser, null, null, runInThread);
                        flag = true;
                        if (flag2 && (watchProcess != null))
                        {
                            this.StartProcessWatch(watchProcess, (IVirtualFile) item, archiveFile);
                        }
                    }
                }
            }
            catch (AbortException)
            {
                return true;
            }
            catch (Exception exception4)
            {
                exception = exception4;
                if (!VirtualItem.IsWarningIOException(exception))
                {
                    throw;
                }
                MessageDialog.ShowException(this, exception, true);
                flag = true;
            }
            return flag;
        }

        private IEnumerable<ToolStripItem> FillBookmarkMenuList(string bookmarkFolder)
        {
            return new <FillBookmarkMenuList>d__2e(-2) { <>4__this = this, <>3__bookmarkFolder = bookmarkFolder };
        }

        private void FilterDropDown_Opening(object sender, CancelEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            down.SuspendLayout();
            try
            {
                down.Items.Clear();
                down.Items.AddRange(new ToolStripItem[] { this.tsmiFilterDialog, this.tssAdvancedFilter1, this.tsmiClearFilter, this.tssAdvancedFilter2 });
                NamedFilter[] filters = Settings.Default.Filters;
                if ((filters != null) && (filters.Length > 0))
                {
                    IContainer container = new Container();
                    foreach (NamedFilter filter in filters)
                    {
                        ToolStripMenuItem component = new ToolStripMenuItem(filter.Name) {
                            Checked = filter.Filter.Equals(this.CurrentPanel.Filter),
                            Tag = filter.Filter
                        };
                        component.Click += new EventHandler(this.FilterItem_Click);
                        container.Add(component);
                        down.Items.Add(component);
                    }
                    down.Tag = container;
                }
                else
                {
                    down.Items.Add(this.tsmiNoStoredFilters);
                }
            }
            finally
            {
                down.ResumeLayout();
            }
        }

        private void FilterItem_Click(object sender, EventArgs e)
        {
            this.CurrentPanel.Filter = ((ToolStripItem) sender).Tag as IVirtualItemFilter;
        }

        private void FindDropDown_Opening(object sender, CancelEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            down.SuspendLayout();
            try
            {
                down.Items.Clear();
                down.Items.AddRange(new ToolStripItem[] { this.tsmiFindDialog, this.tssFind });
                NamedFilter[] searches = Settings.Default.Searches;
                if ((searches != null) && (searches.Length > 0))
                {
                    IContainer container = new Container();
                    foreach (NamedFilter filter in searches)
                    {
                        ToolStripMenuItem component = new ToolStripMenuItem(filter.Name) {
                            Tag = filter.Filter
                        };
                        component.Click += new EventHandler(this.SearchItem_Click);
                        container.Add(component);
                        down.Items.Add(component);
                    }
                    down.Tag = container;
                }
                else
                {
                    down.Items.Add(this.tsmiNoStoredSearches);
                }
            }
            finally
            {
                down.ResumeLayout();
            }
        }

        private void FolderBookmark_Click(object sender, EventArgs e)
        {
            WaitCursor.ShowUntilIdle();
            try
            {
                IVirtualLink tag = (IVirtualLink) ((ToolStripItem) sender).Tag;
                IVirtualFolder target = tag.Target as IVirtualFolder;
                if (target != null)
                {
                    VirtualFilePanel currentPanel = this.CurrentPanel;
                    if ((Control.ModifierKeys == Keys.Shift) && (this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None))
                    {
                        currentPanel = this.FarPanel;
                    }
                    currentPanel.SetCurrentFolder(target, true, tag[0x17] as ICustomizeFolder);
                }
                else
                {
                    this.DeleteSelection(new IVirtualItem[] { tag }, string.Format(Resources.sConfirmDeleteInvalidBookmark, tag.Name), Resources.sConfirmFileDelete);
                }
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

        private static IEnumerable<IVirtualItem> GetAllItems(object target)
        {
            IEnumerable<IVirtualItem> enumerable = target as IEnumerable<IVirtualItem>;
            if (enumerable != null)
            {
                return enumerable;
            }
            IVirtualItem item = target as IVirtualItem;
            if (item != null)
            {
                return new IVirtualItem[] { item };
            }
            return null;
        }

        private IEnumerable<IVirtualItem> GetClipboardItems()
        {
            try
            {
                return VirtualClipboardItem.GetClipboardItems();
            }
            catch (Exception exception)
            {
                if (!VirtualItem.IsWarningIOException(exception) && !(exception is ExternalException))
                {
                    throw;
                }
                MessageDialog.ShowException(this, new ApplicationException(string.Format(Resources.sErrorGetClipboardItems, exception.Message), exception), true);
                return null;
            }
        }

        private IChangeVirtualFile GetLocalSystemItemCopy(IVirtualItem item, bool execute, bool alternate)
        {
            EventHandler<CopyItemEventArgs> handler = null;
            IChangeVirtualFile CurrentItem = item as IChangeVirtualFile;
            if ((CurrentItem != null) && !CurrentItem.CanSeek)
            {
                IEnumerable<IVirtualItem> items = null;
                if (((item is CustomArchiveItem) && execute) && ((alternate || PathHelper.IsExecutableFile(item.Name)) || Settings.Default.ExtractOnRunFilter.MatchName(item.Name)))
                {
                    IVirtualFolder parent = item.Parent;
                    IGetVirtualRoot root = parent as IGetVirtualRoot;
                    if (root != null)
                    {
                        IVirtualFolder folder2 = root.Root;
                        if (folder2 is ArchiveFolder)
                        {
                            parent = folder2;
                        }
                    }
                    if (parent != null)
                    {
                        MessageDialogResult none = MessageDialogResult.None;
                        if (alternate)
                        {
                            none = MessageDialogResult.Yes;
                        }
                        else
                        {
                            none = ConfirmationSettings.Default.ExtractOnRun;
                        }
                        if (none == MessageDialogResult.None)
                        {
                            foreach (IVirtualItem item2 in parent.GetContent())
                            {
                                if (item2.Equals(item))
                                {
                                    none = MessageDialogResult.No;
                                }
                                else
                                {
                                    none = MessageDialogResult.None;
                                    break;
                                }
                            }
                            if (none == MessageDialogResult.None)
                            {
                                string sAskExtractArchiveOnRunExe;
                                Dictionary<MessageDialogResult, string> buttonTextMap = new Dictionary<MessageDialogResult, string>(2);
                                buttonTextMap.Add(MessageDialogResult.Yes, Resources.sMessageButtonExtractAndRun);
                                buttonTextMap.Add(MessageDialogResult.No, Resources.sMessageButtonRun);
                                if (PathHelper.IsExecutableFile(item.Name))
                                {
                                    sAskExtractArchiveOnRunExe = Resources.sAskExtractArchiveOnRunExe;
                                }
                                else
                                {
                                    sAskExtractArchiveOnRunExe = Resources.sAskExtractArchiveOnRunFile;
                                }
                                bool checkBoxChecked = false;
                                none = MessageDialog.Show(this, sAskExtractArchiveOnRunExe, Resources.sConfirmArchiveAction, Resources.sRememberQuestionAnswer, ref checkBoxChecked, MessageDialog.ButtonsYesNoCancel, buttonTextMap, MessageBoxIcon.Question, MessageDialogResult.Yes);
                                if (checkBoxChecked)
                                {
                                    switch (none)
                                    {
                                        case MessageDialogResult.Yes:
                                        case MessageDialogResult.No:
                                            ConfirmationSettings.Default.ExtractOnRun = none;
                                            break;
                                    }
                                }
                            }
                        }
                        switch (none)
                        {
                            case MessageDialogResult.Yes:
                                items = parent.GetContent();
                                break;

                            case MessageDialogResult.No:
                                break;

                            default:
                                return null;
                        }
                    }
                }
                if (items == null)
                {
                    items = new IVirtualItem[] { CurrentItem };
                }
                IVirtualFolder dest = VirtualTempFolder.Default.CreateFolder(item.Name);
                CopyWorker woker = new CopyWorker(items, dest);
                if (handler == null)
                {
                    handler = delegate (object sender, CopyItemEventArgs e) {
                        if (e.Source.Equals(item))
                        {
                            CurrentItem = e.Dest as IChangeVirtualFile;
                        }
                    };
                }
                woker.OnAfterCopyItem += handler;
                using (CopyWorkerDialog dialog = new CopyWorkerDialog())
                {
                    if (!(dialog.Run(this, woker) && !item.Equals(CurrentItem)))
                    {
                        CurrentItem = null;
                    }
                }
            }
            return CurrentItem;
        }

        private static string GetMainMenuFileName(string filePath, bool skipExt)
        {
            string str = skipExt ? Path.GetFileNameWithoutExtension(filePath) : Path.GetFileName(filePath);
            if (Settings.Default.PreserveMainMenuFileNameAmpersand)
            {
                str = str.Replace("&", "&&");
            }
            return str;
        }

        private PanelView GetNextView(PanelView currentView)
        {
            switch (currentView)
            {
                case PanelView.LargeIcon:
                    return PanelView.Thumbnail;

                case PanelView.Details:
                    return PanelView.List;

                case PanelView.SmallIcon:
                    return PanelView.LargeIcon;

                case PanelView.List:
                    return PanelView.SmallIcon;

                case PanelView.Thumbnail:
                    return PanelView.Details;
            }
            throw new InvalidOperationException();
        }

        private static T GetOnlyOneItem<T>(object target, IVirtualItem item) where T: class
        {
            IVirtualItem item2 = target as IVirtualItem;
            if (item2 == null)
            {
                IEnumerable<IVirtualItem> enumerable = target as IEnumerable<IVirtualItem>;
                if (enumerable != null)
                {
                    foreach (IVirtualItem item3 in enumerable)
                    {
                        if (item2 == null)
                        {
                            item2 = item3;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
                else
                {
                    item2 = item;
                }
            }
            return (item2 as T);
        }

        private VirtualFilePanel GetPanelFromMode(TwoPanelContainer.SinglePanel mode)
        {
            if ((mode == TwoPanelContainer.SinglePanel.None) || (this.CurrentTabContent.OnePanelMode != TwoPanelContainer.SinglePanel.None))
            {
                return this.CurrentPanel;
            }
            switch (mode)
            {
                case TwoPanelContainer.SinglePanel.Left:
                    return this.CurrentTabContent.LeftPanel;

                case TwoPanelContainer.SinglePanel.Right:
                    return this.CurrentTabContent.RightPanel;
            }
            return null;
        }

        private IEnumerable<ToolStripItem> GetRelatedToolbarItems(ToolStripItem item, string prefix)
        {
            return new <GetRelatedToolbarItems>d__13(-2) { <>4__this = this, <>3__item = item, <>3__prefix = prefix };
        }

        private string GetToolbarCommands(ToolStrip toolbar)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            StringBuilder builder = new StringBuilder();
            foreach (ToolStripItem item in toolbar.Items)
            {
                Match match = this.ToolbarCommandRegex.Match(item.Name);
                if (match.Success)
                {
                    switch (match.Groups["CommandType"].Value)
                    {
                        case "Action_":
                        case "DropDown_":
                            builder.AppendLine(ToolbarSettings.CreateToolbarButtonLine(match.Groups["CommandName"].Value, item.DisplayStyle, (IconLocation) item.GetTag(2)));
                            break;

                        case "Separator_":
                            builder.AppendLine("-");
                            break;

                        case "Tool_":
                            builder.AppendLine(ToolbarSettings.CreateToolbarButtonLine(string.Format(@"tools\{0}.lnk", match.Groups["CommandName"].Value), item.DisplayStyle, (IconLocation) item.GetTag(2)));
                            break;

                        case "Tool_All_":
                        {
                            if (!flag2)
                            {
                                builder.AppendLine("tools");
                            }
                            flag2 = true;
                            continue;
                        }
                        case "Bookmark_":
                            builder.AppendFormat(@"bookmarks\{0}", match.Groups["CommandName"].Value);
                            builder.AppendLine();
                            break;

                        case "Bookmark_All_":
                        {
                            if (!flag)
                            {
                                builder.AppendLine("bookmarks");
                            }
                            flag = true;
                            continue;
                        }
                        case "Drive_All_":
                        {
                            if (!flag3)
                            {
                                builder.AppendLine("drives");
                            }
                            flag3 = true;
                            continue;
                        }
                    }
                    flag = false;
                    flag2 = false;
                    flag3 = false;
                }
            }
            return builder.ToString();
        }

        private void HistoryButton_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem component = (ToolStripDropDownItem) sender;
            component.DropDown.SuspendLayout();
            try
            {
                int num;
                component.DropDownItems.Clear();
                Action action = this.actionManager.GetAction(component);
                if (action == this.actBack)
                {
                    num = -1;
                }
                else if (action == this.actForward)
                {
                    num = 1;
                }
                else
                {
                    return;
                }
                int num2 = (num < 0) ? this.CurrentPanel.History.BackCount : this.CurrentPanel.History.ForwardCount;
                if (num2 > 0)
                {
                    ToolStripMenuItem item2;
                    IContainer container = new Container();
                    int num3 = 1;
                    IEnumerable<IVirtualFolder> source = (num < 0) ? this.CurrentPanel.History.BackHistory : this.CurrentPanel.History.ForwardHistory;
                    foreach (IVirtualFolder folder in source.Take<IVirtualFolder>(15))
                    {
                        item2 = new ToolStripMenuItem {
                            Text = folder.FullName,
                            MergeIndex = num3++ * num,
                            Tag = folder
                        };
                        item2.MouseUp += new MouseEventHandler(VirtualItemToolStripEvents.MouseUp);
                        item2.MouseHover += new EventHandler(VirtualItemToolStripEvents.MouseHover);
                        item2.MouseLeave += new EventHandler(VirtualItemToolStripEvents.MouseLeave);
                        item2.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintImage);
                        item2.Click += new EventHandler(this.tsmiHistory_Click);
                        container.Add(item2);
                        component.DropDownItems.Add(item2);
                    }
                    if (num2 > 15)
                    {
                        item2 = new ToolStripMenuItem("...") {
                            Enabled = false
                        };
                        container.Add(item2);
                        component.DropDownItems.Add(item2);
                    }
                    component.DropDown.Tag = container;
                }
            }
            finally
            {
                component.DropDown.ResumeLayout();
            }
        }

        private void InitializeBookmarkItem(ToolStripItem bookmarkItem, IVirtualLink virtualLink)
        {
            bookmarkItem.Name = string.Format("{0}{1}_{2}", "Bookmark_", this.UniqueIndex++, virtualLink.Name);
            bookmarkItem.Text = GetMainMenuFileName(virtualLink.Name, true);
            bookmarkItem.AutoToolTip = false;
            bookmarkItem.Tag = virtualLink;
            bookmarkItem.MouseUp += new MouseEventHandler(VirtualItemToolStripEvents.MouseUp);
            bookmarkItem.MouseHover += new EventHandler(VirtualItemToolStripEvents.MouseHover);
            bookmarkItem.MouseLeave += new EventHandler(VirtualItemToolStripEvents.MouseLeave);
            bookmarkItem.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintImage);
            bookmarkItem.Click += new EventHandler(this.FolderBookmark_Click);
        }

        private void InitializeBookmarksWatcher()
        {
            if (((this.BookmarksWatcher == null) && OS.IsWinNT) && System.IO.Directory.Exists(SettingsManager.SpecialFolders.Bookmarks))
            {
                this.BookmarksWatcher = new FileSystemWatcher(SettingsManager.SpecialFolders.Bookmarks);
                this.BookmarksWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName;
                this.BookmarksWatcher.Created += new FileSystemEventHandler(this.BookmarksFolderChanged);
                this.BookmarksWatcher.Deleted += new FileSystemEventHandler(this.BookmarksFolderChanged);
                this.BookmarksWatcher.Changed += new FileSystemEventHandler(this.BookmarksFolderChanged);
                this.BookmarksWatcher.Renamed += new RenamedEventHandler(this.BookmarksFolderChanged);
                this.BookmarksWatcher.SynchronizingObject = this;
                this.BookmarksWatcher.EnableRaisingEvents = true;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MainForm));
            this.smallImageList = new ImageList(this.components);
            this.MainMenu = new MenuStrip();
            this.cmsToolbar = new ContextMenuStrip(this.components);
            this.tsmiRemoveToolbarButton = new ToolStripMenuItem();
            this.tssToolbar1 = new ToolStripSeparator();
            this.tsmiToolbarButtonImageAndText = new ToolStripMenuItem();
            this.tsmiToolbarButtonImage = new ToolStripMenuItem();
            this.tsmiToolbarButtonText = new ToolStripMenuItem();
            this.tssToolbar2 = new ToolStripSeparator();
            this.tsmiChangeButtonImage = new ToolStripMenuItem();
            this.tssToolbar3 = new ToolStripSeparator();
            this.tsmiMainMenuVisible = new ToolStripMenuItem();
            this.tssToolbar4 = new ToolStripSeparator();
            this.tsmiNoToolbars = new ToolStripMenuItem();
            this.tssToolbar5 = new ToolStripSeparator();
            this.tsmiToolbarMoveToTop = new ToolStripMenuItem();
            this.tsmiToolbarMoveToBottom = new ToolStripMenuItem();
            this.tssToolbar6 = new ToolStripSeparator();
            this.tsmiJustifyToolbar = new ToolStripMenuItem();
            this.tssToolbar7 = new ToolStripSeparator();
            this.tsmiCustomizeToolbars2 = new ToolStripMenuItem();
            this.tsmiCustomizeTools2 = new ToolStripMenuItem();
            this.tsmiFile = new ToolStripMenuItem();
            this.tsmiNew = new ToolStripMenuItem();
            this.tsmiViewItem = new ToolStripMenuItem();
            this.tsmiEditItem = new ToolStripMenuItem();
            this.tssFile1 = new ToolStripSeparator();
            this.tsmiFind = new ToolStripMenuItem();
            this.tssFile2 = new ToolStripSeparator();
            this.tsmiCopy = new ToolStripMenuItem();
            this.tsmiRenameMove = new ToolStripMenuItem();
            this.tsmiPack = new ToolStripMenuItem();
            this.tsmiRenameSingleItem = new ToolStripMenuItem();
            this.tsmiMakeLink = new ToolStripMenuItem();
            this.tsmiMakeFolder = new ToolStripMenuItem();
            this.tssFile3 = new ToolStripSeparator();
            this.tsmiDelete = new ToolStripMenuItem();
            this.tsmiDeleteSingleItem = new ToolStripMenuItem();
            this.tssFile4 = new ToolStripSeparator();
            this.tsmiRunAs = new ToolStripMenuItem();
            this.tsmiSetAttributes = new ToolStripMenuItem();
            this.tsmiEditDescription = new ToolStripMenuItem();
            this.tsmiShowProperties = new ToolStripMenuItem();
            this.tssFile5 = new ToolStripSeparator();
            this.tsmiExit = new ToolStripMenuItem();
            this.tsmiEdit = new ToolStripMenuItem();
            this.tsmiCutToClipboard = new ToolStripMenuItem();
            this.tsmiCopyToClipboard = new ToolStripMenuItem();
            this.tsmiPasteFromClipboard = new ToolStripMenuItem();
            this.tsmiPasteShortcut = new ToolStripMenuItem();
            this.tssEdit1 = new ToolStripSeparator();
            this.tsmiCopyNameAsText = new ToolStripMenuItem();
            this.tsmiCopyFullNameAsText = new ToolStripMenuItem();
            this.tsmiCopyDetailsAsCSV = new ToolStripMenuItem();
            this.tssEdit2 = new ToolStripSeparator();
            this.tsmiEmptyClipboard = new ToolStripMenuItem();
            this.tssEdit3 = new ToolStripSeparator();
            this.tsmiSelect = new ToolStripMenuItem();
            this.tsmiUnselect = new ToolStripMenuItem();
            this.tsmiInvertSelection = new ToolStripMenuItem();
            this.tsmiRestoreSelection = new ToolStripMenuItem();
            this.tssEdit4 = new ToolStripSeparator();
            this.tsmiSelectAll = new ToolStripMenuItem();
            this.tsmiView = new ToolStripMenuItem();
            this.tsmiOnePanel = new ToolStripMenuItem();
            this.tsmiTwoHorizontalPanel = new ToolStripMenuItem();
            this.tsmiTwoVerticalPanel = new ToolStripMenuItem();
            this.tssView1 = new ToolStripSeparator();
            this.tsmiWindowLayout = new ToolStripMenuItem();
            this.tsmiFolderBar = new ToolStripMenuItem();
            this.tsmiFolderBarHidden = new ToolStripMenuItem();
            this.tsmiFolderBarHorizontal = new ToolStripMenuItem();
            this.tsmiFolderBarVertical = new ToolStripMenuItem();
            this.tsmiToolbars = new ToolStripMenuItem();
            this.tssView2 = new ToolStripSeparator();
            this.tsmiViewAsThumbnail = new ToolStripMenuItem();
            this.tsmiViewAsLargeIcon = new ToolStripMenuItem();
            this.tsmiViewAsSmallIcon = new ToolStripMenuItem();
            this.tsmiViewAsList = new ToolStripMenuItem();
            this.tsmiViewAsDetails = new ToolStripMenuItem();
            this.tssView3 = new ToolStripSeparator();
            this.tsmiColumns = new ToolStripMenuItem();
            this.tsmiManageColumns = new ToolStripMenuItem();
            this.tsmiCustomizeToolbars1 = new ToolStripMenuItem();
            this.tsmiCustomizeTools1 = new ToolStripMenuItem();
            this.tsmiViewAs = new ToolStripMenuItem();
            this.tsmiSetOneListColumn = new ToolStripMenuItem();
            this.tsmiSetTwoListColumns = new ToolStripMenuItem();
            this.tsmiSetThreeListColumns = new ToolStripMenuItem();
            this.tsmiSetFourListColumns = new ToolStripMenuItem();
            this.tsmiSetFiveListColumns = new ToolStripMenuItem();
            this.tsmiSetSixListColumns = new ToolStripMenuItem();
            this.tsmiSetSevenListColumns = new ToolStripMenuItem();
            this.tsmiSetEightListColumns = new ToolStripMenuItem();
            this.tsmiSetNineListColumns = new ToolStripMenuItem();
            this.tsmiFilter = new ToolStripMenuItem();
            this.tsmiSort = new ToolStripMenuItem();
            this.tssView4 = new ToolStripSeparator();
            this.tsmiCustomizeFolder = new ToolStripMenuItem();
            this.tsmiPanel = new ToolStripMenuItem();
            this.tsmiCompareFolders = new ToolStripMenuItem();
            this.tsmiFolderBranch = new ToolStripMenuItem();
            this.tsmiCalculateFolderSizes = new ToolStripMenuItem();
            this.tssPanel1 = new ToolStripSeparator();
            this.tsmiNavigationLink = new ToolStripMenuItem();
            this.tsmiLockFolderChange = new ToolStripMenuItem();
            this.tssPanel2 = new ToolStripSeparator();
            this.tsmiSwapPanels = new ToolStripMenuItem();
            this.tsmiEqualizePanels = new ToolStripMenuItem();
            this.tsmiOpenInFarPanel = new ToolStripMenuItem();
            this.tssPanel3 = new ToolStripSeparator();
            this.tsmiBack = new ToolStripMenuItem();
            this.tsmiForward = new ToolStripMenuItem();
            this.tssPanel4 = new ToolStripSeparator();
            this.tsmiChangeDrive = new ToolStripMenuItem();
            this.tsmiChangeFolder = new ToolStripMenuItem();
            this.tsmiFtpConnect = new ToolStripMenuItem();
            this.tsmiSpecialFolders = new ToolStripMenuItem();
            this.tsmiFolderMyDocuments = new ToolStripMenuItem();
            this.tsmiFolderMyPictures = new ToolStripMenuItem();
            this.tsmiFolderMyMusic = new ToolStripMenuItem();
            this.tssSpecialFolder1 = new ToolStripSeparator();
            this.tsmiFolderFavorites = new ToolStripMenuItem();
            this.tsmiFolderDesktop = new ToolStripMenuItem();
            this.tssSpecialFolder2 = new ToolStripSeparator();
            this.tsmiFolderTemp = new ToolStripMenuItem();
            this.tsmiFolderWindows = new ToolStripMenuItem();
            this.tsmiFolderSystem = new ToolStripMenuItem();
            this.tssPanel5 = new ToolStripSeparator();
            this.tsmiOpenContainingFolder = new ToolStripMenuItem();
            this.tssPanel6 = new ToolStripSeparator();
            this.tsmiRefresh = new ToolStripMenuItem();
            this.tsmiTab = new ToolStripMenuItem();
            this.tsmiBookmarks = new ToolStripMenuItem();
            this.tsmiBookmarkCurrentFolder = new ToolStripMenuItem();
            this.tsmiBookmarkCurrentTab = new ToolStripMenuItem();
            this.tsmiOrganizeBookmarks = new ToolStripMenuItem();
            this.tssBookmarks1 = new ToolStripSeparator();
            this.tsmiEmpty = new ToolStripMenuItem();
            this.tsmiTools = new ToolStripMenuItem();
            this.tsmiOptions = new ToolStripMenuItem();
            this.tssTools1 = new ToolStripSeparator();
            this.tsmiHelp = new ToolStripMenuItem();
            this.tsmiHelpContents = new ToolStripMenuItem();
            this.tsmiShowCmdLineHelp = new ToolStripMenuItem();
            this.tssHelp1 = new ToolStripSeparator();
            this.tsmiCheckForUpdates = new ToolStripMenuItem();
            this.tssHelp2 = new ToolStripSeparator();
            this.tsmiAbout = new ToolStripMenuItem();
            this.tsmiSelectSort = new ToolStripMenuItem();
            this.tssSort1 = new ToolStripSeparator();
            this.tsmiSortByName = new ToolStripMenuItem();
            this.tsmiSortByExtension = new ToolStripMenuItem();
            this.tsmiSortByLastWriteTime = new ToolStripMenuItem();
            this.tsmiSortBySize = new ToolStripMenuItem();
            this.tssSort2 = new ToolStripSeparator();
            this.tsmiSortDescending = new ToolStripMenuItem();
            this.tsmiDuplicateTab = new ToolStripMenuItem();
            this.tsmiRenameTab = new ToolStripMenuItem();
            this.tssTab1 = new ToolStripSeparator();
            this.tsmiCloseTab = new ToolStripMenuItem();
            this.tsmiCloseOtherTabs = new ToolStripMenuItem();
            this.tssTab2 = new ToolStripSeparator();
            this.tsmiFilterDialog = new ToolStripMenuItem();
            this.tssAdvancedFilter1 = new ToolStripSeparator();
            this.tsmiClearFilter = new ToolStripMenuItem();
            this.tssAdvancedFilter2 = new ToolStripSeparator();
            this.tsmiNoStoredFilters = new ToolStripMenuItem();
            this.tsmiSaveCurrentLayout = new ToolStripMenuItem();
            this.tsmiManageLayouts = new ToolStripMenuItem();
            this.tssLayout1 = new ToolStripSeparator();
            this.tsmiNoStoredLayouts = new ToolStripMenuItem();
            this.tsmiFindDialog = new ToolStripMenuItem();
            this.tssFind = new ToolStripSeparator();
            this.tsmiNoStoredSearches = new ToolStripMenuItem();
            this.tsmiNewFile = new ToolStripMenuItem();
            this.tssNewFile = new ToolStripSeparator();
            this.FindExeFileDialog = new OpenFileDialog();
            this.toolTipDefault = new ToolTip(this.components);
            this.largeImageList = new ImageList(this.components);
            this.cmsMenuViewAs = new ContextMenuStrip(this.components);
            this.tsmiViewAsThumbnail2 = new ToolStripMenuItem();
            this.tsmiViewAsLargeIcon2 = new ToolStripMenuItem();
            this.tsmiViewAsSmallIcon2 = new ToolStripMenuItem();
            this.tsmiViewAsList2 = new ToolStripMenuItem();
            this.tsmiViewAsDetails2 = new ToolStripMenuItem();
            this.tssViewAs1 = new ToolStripSeparator();
            this.tsmiManageColumns2 = new ToolStripMenuItem();
            this.tsmiSetOneListColumn2 = new ToolStripMenuItem();
            this.tsmiSetTwoListColumns2 = new ToolStripMenuItem();
            this.tsmiSetThreeListColumns2 = new ToolStripMenuItem();
            this.tsmiSetFourListColumns2 = new ToolStripMenuItem();
            this.tsmiSetFiveListColumns2 = new ToolStripMenuItem();
            this.tsmiSetSixListColumns2 = new ToolStripMenuItem();
            this.tsmiSetSevenListColumns2 = new ToolStripMenuItem();
            this.tsmiSetEightListColumns2 = new ToolStripMenuItem();
            this.tsmiSetNineListColumns2 = new ToolStripMenuItem();
            this.cmsTab = new ContextMenuStrip(this.components);
            this.tsmiDuplicateTab2 = new ToolStripMenuItem();
            this.tssTab10 = new ToolStripSeparator();
            this.tsmiCloseTab2 = new ToolStripMenuItem();
            this.tsmiCloseOtherTabs2 = new ToolStripMenuItem();
            this.tsmiCloseTabsToRight2 = new ToolStripMenuItem();
            this.tssTab11 = new ToolStripSeparator();
            this.tsmiRenameTab2 = new ToolStripMenuItem();
            this.MainPageSwitcher = new TabPageSwitcher();
            this.MainTabStrip = new TabStrip();
            this.tsbCloseTab = new ToolStripButton();
            this.actViewItem = new Action();
            this.actEditItem = new Action();
            this.actFind = new Action();
            this.actCopy = new Action();
            this.actRenameMove = new Action();
            this.actRenameSingleItem = new Action();
            this.actMakeFolder = new Action();
            this.actDelete = new Action();
            this.actDeleteSingleItem = new Action();
            this.actRunAs = new Action();
            this.actSetAttributes = new Action();
            this.actShowProperties = new Action();
            this.actCutToClipboard = new Action();
            this.actCopyToClipboard = new Action();
            this.actPasteFromClipboard = new Action();
            this.actPasteShortCut = new Action();
            this.actCopyNameAsText = new Action();
            this.actCopyFullNameAsText = new Action();
            this.actEmptyClipboard = new Action();
            this.actSelect = new Action();
            this.actUnselect = new Action();
            this.actInvertSelection = new Action();
            this.actRestoreSelection = new Action();
            this.actSelectAll = new Action();
            this.actOnePanel = new Action();
            this.actTwoHorizontalPanel = new Action();
            this.actTwoVerticalPanel = new Action();
            this.actViewAsSmallIcon = new Action();
            this.actViewAsList = new Action();
            this.actViewAsDetails = new Action();
            this.actAdvancedFilter = new Action();
            this.actClearFilter = new Action();
            this.actSelectSort = new Action();
            this.actSortByName = new Action();
            this.actSortByExtension = new Action();
            this.actSortByLastWriteTime = new Action();
            this.actSortBySize = new Action();
            this.actSortDescending = new Action();
            this.actCompareFolders = new Action();
            this.actFolderBranch = new Action();
            this.actSwapPanels = new Action();
            this.actEqualizePanels = new Action();
            this.actBack = new Action();
            this.actForward = new Action();
            this.actChangeDrive = new Action();
            this.actChangeFolder = new Action();
            this.actFtpConnect = new Action();
            this.actOpenContainingFolder = new Action();
            this.actRefresh = new Action();
            this.actBookmarkCurrentFolder = new Action();
            this.actOrganizeBookmarks = new Action();
            this.actOptions = new Action();
            this.actAbout = new Action();
            this.actMakeLink = new Action();
            this.catFile = new Category(this.components);
            this.catEdit = new Category(this.components);
            this.catView = new Category(this.components);
            this.catPanel = new Category(this.components);
            this.catBookmarks = new Category(this.components);
            this.catMisc = new Category(this.components);
            this.actManageColumns = new Action();
            this.actSelectByExtension = new Action();
            this.actUnselectByExtension = new Action();
            this.actGoToParent = new Action();
            this.actGoToRoot = new Action();
            this.actEditDescription = new Action();
            this.actViewAsLargeIcon = new Action();
            this.actCustomizeFolder = new Action();
            this.actInvertEntireSelection = new Action();
            this.actViewAsThumbnail = new Action();
            this.actCheckForUpdates = new Action();
            this.actUnselectAll = new Action();
            this.actChangeView = new Action();
            this.actToggleQuickFind = new Action();
            this.actChangeDriveLeft = new Action();
            this.actChangeDriveRight = new Action();
            this.actShowBookmarks = new Action();
            this.actResetVisualCache = new Action();
            this.actPack = new Action();
            this.actCopyDetailsAsCSV = new Action();
            this.actSaveCurrentLayout = new Action();
            this.actManageLayouts = new Action();
            this.actNewFile = new Action();
            this.actCalculateOnDemandProperties = new Action();
            this.actDuplicateTab = new Action();
            this.actCloseTab = new Action();
            this.catTab = new Category(this.components);
            this.actCloseOtherTabs = new Action();
            this.actRenameTab = new Action();
            this.actMoveToNextTab = new Action();
            this.actMoveToPreviousTab = new Action();
            this.actMoveToFirstTab = new Action();
            this.actMoveToSecondTab = new Action();
            this.actMoveToThirdTab = new Action();
            this.actMoveToFourthTab = new Action();
            this.actMoveToFifthTab = new Action();
            this.actMoveToSixthTab = new Action();
            this.actMoveToSeventhTab = new Action();
            this.actMoveToEighthTab = new Action();
            this.actMoveToLastTab = new Action();
            this.actNavigationLink = new Action();
            this.actLockFolderChange = new Action();
            this.actCloseTabsToRight = new Action();
            this.actSelectByName = new Action();
            this.actToggleOnePanelMode = new Action();
            this.actToggleFolderBar = new Action();
            this.actCustomizeToolbars = new Action();
            this.actExit = new Action();
            this.actCustomizeTools = new Action();
            this.actSaveSettings = new Action();
            this.actBringToFront = new Action();
            this.actVolumeLabel = new Action();
            this.actOpenOutside = new Action();
            this.actRefreshToolbars = new Action();
            this.actionManager = new ActionManager(this.components);
            this.actSetOneListColumn = new Action();
            this.actSetTwoListColumns = new Action();
            this.actSetThreeListColumns = new Action();
            this.actSetFourListColumns = new Action();
            this.actSetFiveListColumns = new Action();
            this.actSetSixListColumns = new Action();
            this.actSetSevenListColumns = new Action();
            this.actSetEightListColumns = new Action();
            this.actSetNineListColumns = new Action();
            this.actOpenInFarPanel = new Action();
            this.actBookmarkCurrentTab = new Action();
            this.actHelpContents = new Action();
            this.actShowCmdLineHelp = new Action();
            this.actRunAsAdmin = new Action();
            this.actOpen = new Action();
            this.actMapNetworkDrive = new Action();
            this.actDisconnectNetworkDrive = new Action();
            this.actCopyCurrentFolderAsText = new Action();
            this.actSelectSingleItem = new Action();
            this.actSelectSingleItemAndCalculate = new Action();
            this.actOpenAsArchive = new Action();
            this.actLeftPanelToRight = new Action();
            this.actRightPanelToLeft = new Action();
            this.actQuickChangeFolder = new Action();
            this.actOpenRecentFolders = new Action();
            this.actAddFolderToRecent = new Action();
            this.actGCCollect = new Action();
            this.actMinimizeToTray = new Action();
            this.tsmiAbout2 = new ToolStripMenuItem();
            this.tsmiBringToFront = new ToolStripMenuItem();
            this.tsmiExit2 = new ToolStripMenuItem();
            this.categoryManager = new CategoryManager(this.components);
            this.cmsMenuNew = new ContextMenuStrip(this.components);
            this.cmsMenuFind = new ContextMenuStrip(this.components);
            this.cmsMenuWindowLayout = new ContextMenuStrip(this.components);
            this.cmsMenuFilter = new ContextMenuStrip(this.components);
            this.cmsMenuTab = new ContextMenuStrip(this.components);
            this.cmsMenuSort = new ContextMenuStrip(this.components);
            this.TrayIcon = new NotifyIcon(this.components);
            this.cmsTray = new ContextMenuStrip(this.components);
            this.tssTray1 = new ToolStripSeparator();
            this.MainMenu.SuspendLayout();
            this.cmsToolbar.SuspendLayout();
            this.cmsMenuViewAs.SuspendLayout();
            this.cmsTab.SuspendLayout();
            this.MainTabStrip.SuspendLayout();
            this.actionManager.BeginInit();
            this.cmsMenuNew.SuspendLayout();
            this.cmsMenuFind.SuspendLayout();
            this.cmsMenuWindowLayout.SuspendLayout();
            this.cmsMenuFilter.SuspendLayout();
            this.cmsMenuTab.SuspendLayout();
            this.cmsMenuSort.SuspendLayout();
            this.cmsTray.SuspendLayout();
            base.SuspendLayout();
            this.smallImageList.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.smallImageList, "smallImageList");
            this.smallImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MainMenu.ContextMenuStrip = this.cmsToolbar;
            this.MainMenu.DataBindings.Add(new Binding("Visible", Settings.Default, "MainMenuVisible", true, DataSourceUpdateMode.Never));
            this.MainMenu.Items.AddRange(new ToolStripItem[] { this.tsmiFile, this.tsmiEdit, this.tsmiView, this.tsmiPanel, this.tsmiTab, this.tsmiBookmarks, this.tsmiTools, this.tsmiHelp });
            manager.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Visible = Settings.Default.MainMenuVisible;
            this.MainMenu.MenuActivate += new EventHandler(this.MainMenu_MenuActivate);
            this.MainMenu.MenuDeactivate += new EventHandler(this.MainMenu_MenuDeactivate);
            this.MainMenu.Paint += new PaintEventHandler(this.MainMenu_Paint);
            this.cmsToolbar.Items.AddRange(new ToolStripItem[] { 
                this.tsmiRemoveToolbarButton, this.tssToolbar1, this.tsmiToolbarButtonImageAndText, this.tsmiToolbarButtonImage, this.tsmiToolbarButtonText, this.tssToolbar2, this.tsmiChangeButtonImage, this.tssToolbar3, this.tsmiMainMenuVisible, this.tssToolbar4, this.tsmiNoToolbars, this.tssToolbar5, this.tsmiToolbarMoveToTop, this.tsmiToolbarMoveToBottom, this.tssToolbar6, this.tsmiJustifyToolbar, 
                this.tssToolbar7, this.tsmiCustomizeToolbars2, this.tsmiCustomizeTools2
             });
            this.cmsToolbar.Name = "cmsToolbar";
            manager.ApplyResources(this.cmsToolbar, "cmsToolbar");
            this.cmsToolbar.Closed += new ToolStripDropDownClosedEventHandler(this.cmsToolbar_Closed);
            this.cmsToolbar.Opening += new CancelEventHandler(this.cmsToolbar_Opening);
            this.tsmiRemoveToolbarButton.Name = "tsmiRemoveToolbarButton";
            manager.ApplyResources(this.tsmiRemoveToolbarButton, "tsmiRemoveToolbarButton");
            this.tsmiRemoveToolbarButton.Click += new EventHandler(this.tsmiRemoveToolbarButton_Click);
            this.tssToolbar1.Name = "tssToolbar1";
            manager.ApplyResources(this.tssToolbar1, "tssToolbar1");
            this.tsmiToolbarButtonImageAndText.Name = "tsmiToolbarButtonImageAndText";
            manager.ApplyResources(this.tsmiToolbarButtonImageAndText, "tsmiToolbarButtonImageAndText");
            this.tsmiToolbarButtonImageAndText.Click += new EventHandler(this.tsmiToolbarButtonDisplayStyle_Click);
            this.tsmiToolbarButtonImageAndText.Paint += new PaintEventHandler(this.tsmiToolbarButtonDisplayStyle_Paint);
            this.tsmiToolbarButtonImage.Name = "tsmiToolbarButtonImage";
            manager.ApplyResources(this.tsmiToolbarButtonImage, "tsmiToolbarButtonImage");
            this.tsmiToolbarButtonImage.Click += new EventHandler(this.tsmiToolbarButtonDisplayStyle_Click);
            this.tsmiToolbarButtonImage.Paint += new PaintEventHandler(this.tsmiToolbarButtonDisplayStyle_Paint);
            this.tsmiToolbarButtonText.Name = "tsmiToolbarButtonText";
            manager.ApplyResources(this.tsmiToolbarButtonText, "tsmiToolbarButtonText");
            this.tsmiToolbarButtonText.Click += new EventHandler(this.tsmiToolbarButtonDisplayStyle_Click);
            this.tsmiToolbarButtonText.Paint += new PaintEventHandler(this.tsmiToolbarButtonDisplayStyle_Paint);
            this.tssToolbar2.Name = "tssToolbar2";
            manager.ApplyResources(this.tssToolbar2, "tssToolbar2");
            this.tsmiChangeButtonImage.Name = "tsmiChangeButtonImage";
            manager.ApplyResources(this.tsmiChangeButtonImage, "tsmiChangeButtonImage");
            this.tsmiChangeButtonImage.Click += new EventHandler(this.tsmiChangeButtonImage_Click);
            this.tssToolbar3.Name = "tssToolbar3";
            manager.ApplyResources(this.tssToolbar3, "tssToolbar3");
            this.tsmiMainMenuVisible.Name = "tsmiMainMenuVisible";
            manager.ApplyResources(this.tsmiMainMenuVisible, "tsmiMainMenuVisible");
            this.tsmiMainMenuVisible.Click += new EventHandler(this.tsmiMainMenuVisible_Click);
            this.tsmiMainMenuVisible.Paint += new PaintEventHandler(this.tsmiMainMenuVisible_Paint);
            this.tssToolbar4.Name = "tssToolbar4";
            manager.ApplyResources(this.tssToolbar4, "tssToolbar4");
            manager.ApplyResources(this.tsmiNoToolbars, "tsmiNoToolbars");
            this.tsmiNoToolbars.Name = "tsmiNoToolbars";
            this.tssToolbar5.Name = "tssToolbar5";
            manager.ApplyResources(this.tssToolbar5, "tssToolbar5");
            this.tsmiToolbarMoveToTop.Name = "tsmiToolbarMoveToTop";
            manager.ApplyResources(this.tsmiToolbarMoveToTop, "tsmiToolbarMoveToTop");
            this.tsmiToolbarMoveToTop.Click += new EventHandler(this.tsmiToolbarMoveToTop_Click);
            this.tsmiToolbarMoveToBottom.Name = "tsmiToolbarMoveToBottom";
            manager.ApplyResources(this.tsmiToolbarMoveToBottom, "tsmiToolbarMoveToBottom");
            this.tsmiToolbarMoveToBottom.Click += new EventHandler(this.tsmiToolbarMoveToTop_Click);
            this.tssToolbar6.Name = "tssToolbar6";
            manager.ApplyResources(this.tssToolbar6, "tssToolbar6");
            this.tsmiJustifyToolbar.Name = "tsmiJustifyToolbar";
            manager.ApplyResources(this.tsmiJustifyToolbar, "tsmiJustifyToolbar");
            this.tsmiJustifyToolbar.Click += new EventHandler(this.tsmiJustifyToolbar_Click);
            this.tsmiJustifyToolbar.Paint += new PaintEventHandler(this.tsmiJustifyToolbar_Paint);
            this.tssToolbar7.Name = "tssToolbar7";
            manager.ApplyResources(this.tssToolbar7, "tssToolbar7");
            this.actionManager.SetAction(this.tsmiCustomizeToolbars2, this.actCustomizeToolbars);
            this.tsmiCustomizeToolbars2.Name = "tsmiCustomizeToolbars2";
            manager.ApplyResources(this.tsmiCustomizeToolbars2, "tsmiCustomizeToolbars2");
            this.actionManager.SetAction(this.tsmiCustomizeTools2, this.actCustomizeTools);
            this.tsmiCustomizeTools2.Name = "tsmiCustomizeTools2";
            manager.ApplyResources(this.tsmiCustomizeTools2, "tsmiCustomizeTools2");
            this.tsmiFile.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiFile.DropDownItems.AddRange(new ToolStripItem[] { 
                this.tsmiNew, this.tsmiViewItem, this.tsmiEditItem, this.tssFile1, this.tsmiFind, this.tssFile2, this.tsmiCopy, this.tsmiRenameMove, this.tsmiPack, this.tsmiRenameSingleItem, this.tsmiMakeLink, this.tsmiMakeFolder, this.tssFile3, this.tsmiDelete, this.tsmiDeleteSingleItem, this.tssFile4, 
                this.tsmiRunAs, this.tsmiSetAttributes, this.tsmiEditDescription, this.tsmiShowProperties, this.tssFile5, this.tsmiExit
             });
            this.tsmiFile.Name = "tsmiFile";
            manager.ApplyResources(this.tsmiFile, "tsmiFile");
            this.tsmiNew.Name = "tsmiNew";
            manager.ApplyResources(this.tsmiNew, "tsmiNew");
            this.actionManager.SetAction(this.tsmiViewItem, this.actViewItem);
            manager.ApplyResources(this.tsmiViewItem, "tsmiViewItem");
            this.tsmiViewItem.Name = "tsmiViewItem";
            this.actionManager.SetAction(this.tsmiEditItem, this.actEditItem);
            manager.ApplyResources(this.tsmiEditItem, "tsmiEditItem");
            this.tsmiEditItem.Name = "tsmiEditItem";
            this.tssFile1.Name = "tssFile1";
            manager.ApplyResources(this.tssFile1, "tssFile1");
            this.tsmiFind.Name = "tsmiFind";
            manager.ApplyResources(this.tsmiFind, "tsmiFind");
            this.tssFile2.Name = "tssFile2";
            manager.ApplyResources(this.tssFile2, "tssFile2");
            this.actionManager.SetAction(this.tsmiCopy, this.actCopy);
            manager.ApplyResources(this.tsmiCopy, "tsmiCopy");
            this.tsmiCopy.Name = "tsmiCopy";
            this.actionManager.SetAction(this.tsmiRenameMove, this.actRenameMove);
            manager.ApplyResources(this.tsmiRenameMove, "tsmiRenameMove");
            this.tsmiRenameMove.Name = "tsmiRenameMove";
            this.actionManager.SetAction(this.tsmiPack, this.actPack);
            manager.ApplyResources(this.tsmiPack, "tsmiPack");
            this.tsmiPack.Name = "tsmiPack";
            this.actionManager.SetAction(this.tsmiRenameSingleItem, this.actRenameSingleItem);
            manager.ApplyResources(this.tsmiRenameSingleItem, "tsmiRenameSingleItem");
            this.tsmiRenameSingleItem.Name = "tsmiRenameSingleItem";
            this.actionManager.SetAction(this.tsmiMakeLink, this.actMakeLink);
            manager.ApplyResources(this.tsmiMakeLink, "tsmiMakeLink");
            this.tsmiMakeLink.Name = "tsmiMakeLink";
            this.actionManager.SetAction(this.tsmiMakeFolder, this.actMakeFolder);
            manager.ApplyResources(this.tsmiMakeFolder, "tsmiMakeFolder");
            this.tsmiMakeFolder.Name = "tsmiMakeFolder";
            this.tssFile3.Name = "tssFile3";
            manager.ApplyResources(this.tssFile3, "tssFile3");
            this.actionManager.SetAction(this.tsmiDelete, this.actDelete);
            manager.ApplyResources(this.tsmiDelete, "tsmiDelete");
            this.tsmiDelete.Name = "tsmiDelete";
            this.actionManager.SetAction(this.tsmiDeleteSingleItem, this.actDeleteSingleItem);
            manager.ApplyResources(this.tsmiDeleteSingleItem, "tsmiDeleteSingleItem");
            this.tsmiDeleteSingleItem.Name = "tsmiDeleteSingleItem";
            this.tssFile4.Name = "tssFile4";
            manager.ApplyResources(this.tssFile4, "tssFile4");
            this.actionManager.SetAction(this.tsmiRunAs, this.actRunAs);
            manager.ApplyResources(this.tsmiRunAs, "tsmiRunAs");
            this.tsmiRunAs.Name = "tsmiRunAs";
            this.actionManager.SetAction(this.tsmiSetAttributes, this.actSetAttributes);
            manager.ApplyResources(this.tsmiSetAttributes, "tsmiSetAttributes");
            this.tsmiSetAttributes.Name = "tsmiSetAttributes";
            this.actionManager.SetAction(this.tsmiEditDescription, this.actEditDescription);
            manager.ApplyResources(this.tsmiEditDescription, "tsmiEditDescription");
            this.tsmiEditDescription.Name = "tsmiEditDescription";
            this.actionManager.SetAction(this.tsmiShowProperties, this.actShowProperties);
            manager.ApplyResources(this.tsmiShowProperties, "tsmiShowProperties");
            this.tsmiShowProperties.Name = "tsmiShowProperties";
            this.tssFile5.Name = "tssFile5";
            manager.ApplyResources(this.tssFile5, "tssFile5");
            this.actionManager.SetAction(this.tsmiExit, this.actExit);
            this.tsmiExit.Name = "tsmiExit";
            manager.ApplyResources(this.tsmiExit, "tsmiExit");
            this.tsmiEdit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiEdit.DropDownItems.AddRange(new ToolStripItem[] { 
                this.tsmiCutToClipboard, this.tsmiCopyToClipboard, this.tsmiPasteFromClipboard, this.tsmiPasteShortcut, this.tssEdit1, this.tsmiCopyNameAsText, this.tsmiCopyFullNameAsText, this.tsmiCopyDetailsAsCSV, this.tssEdit2, this.tsmiEmptyClipboard, this.tssEdit3, this.tsmiSelect, this.tsmiUnselect, this.tsmiInvertSelection, this.tsmiRestoreSelection, this.tssEdit4, 
                this.tsmiSelectAll
             });
            this.tsmiEdit.Name = "tsmiEdit";
            manager.ApplyResources(this.tsmiEdit, "tsmiEdit");
            this.actionManager.SetAction(this.tsmiCutToClipboard, this.actCutToClipboard);
            manager.ApplyResources(this.tsmiCutToClipboard, "tsmiCutToClipboard");
            this.tsmiCutToClipboard.Name = "tsmiCutToClipboard";
            this.actionManager.SetAction(this.tsmiCopyToClipboard, this.actCopyToClipboard);
            manager.ApplyResources(this.tsmiCopyToClipboard, "tsmiCopyToClipboard");
            this.tsmiCopyToClipboard.Name = "tsmiCopyToClipboard";
            this.actionManager.SetAction(this.tsmiPasteFromClipboard, this.actPasteFromClipboard);
            manager.ApplyResources(this.tsmiPasteFromClipboard, "tsmiPasteFromClipboard");
            this.tsmiPasteFromClipboard.Name = "tsmiPasteFromClipboard";
            this.actionManager.SetAction(this.tsmiPasteShortcut, this.actPasteShortCut);
            manager.ApplyResources(this.tsmiPasteShortcut, "tsmiPasteShortcut");
            this.tsmiPasteShortcut.Name = "tsmiPasteShortcut";
            this.tssEdit1.Name = "tssEdit1";
            manager.ApplyResources(this.tssEdit1, "tssEdit1");
            this.actionManager.SetAction(this.tsmiCopyNameAsText, this.actCopyNameAsText);
            manager.ApplyResources(this.tsmiCopyNameAsText, "tsmiCopyNameAsText");
            this.tsmiCopyNameAsText.Name = "tsmiCopyNameAsText";
            this.actionManager.SetAction(this.tsmiCopyFullNameAsText, this.actCopyFullNameAsText);
            manager.ApplyResources(this.tsmiCopyFullNameAsText, "tsmiCopyFullNameAsText");
            this.tsmiCopyFullNameAsText.Name = "tsmiCopyFullNameAsText";
            this.actionManager.SetAction(this.tsmiCopyDetailsAsCSV, this.actCopyDetailsAsCSV);
            manager.ApplyResources(this.tsmiCopyDetailsAsCSV, "tsmiCopyDetailsAsCSV");
            this.tsmiCopyDetailsAsCSV.Name = "tsmiCopyDetailsAsCSV";
            this.tssEdit2.Name = "tssEdit2";
            manager.ApplyResources(this.tssEdit2, "tssEdit2");
            this.actionManager.SetAction(this.tsmiEmptyClipboard, this.actEmptyClipboard);
            manager.ApplyResources(this.tsmiEmptyClipboard, "tsmiEmptyClipboard");
            this.tsmiEmptyClipboard.Name = "tsmiEmptyClipboard";
            this.tssEdit3.Name = "tssEdit3";
            manager.ApplyResources(this.tssEdit3, "tssEdit3");
            this.actionManager.SetAction(this.tsmiSelect, this.actSelect);
            this.tsmiSelect.Name = "tsmiSelect";
            manager.ApplyResources(this.tsmiSelect, "tsmiSelect");
            this.actionManager.SetAction(this.tsmiUnselect, this.actUnselect);
            manager.ApplyResources(this.tsmiUnselect, "tsmiUnselect");
            this.tsmiUnselect.Name = "tsmiUnselect";
            this.actionManager.SetAction(this.tsmiInvertSelection, this.actInvertSelection);
            this.tsmiInvertSelection.Name = "tsmiInvertSelection";
            manager.ApplyResources(this.tsmiInvertSelection, "tsmiInvertSelection");
            this.actionManager.SetAction(this.tsmiRestoreSelection, this.actRestoreSelection);
            manager.ApplyResources(this.tsmiRestoreSelection, "tsmiRestoreSelection");
            this.tsmiRestoreSelection.Name = "tsmiRestoreSelection";
            this.tssEdit4.Name = "tssEdit4";
            manager.ApplyResources(this.tssEdit4, "tssEdit4");
            this.actionManager.SetAction(this.tsmiSelectAll, this.actSelectAll);
            this.tsmiSelectAll.Name = "tsmiSelectAll";
            manager.ApplyResources(this.tsmiSelectAll, "tsmiSelectAll");
            this.tsmiView.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiView.DropDownItems.AddRange(new ToolStripItem[] { 
                this.tsmiOnePanel, this.tsmiTwoHorizontalPanel, this.tsmiTwoVerticalPanel, this.tssView1, this.tsmiWindowLayout, this.tsmiFolderBar, this.tsmiToolbars, this.tssView2, this.tsmiViewAsThumbnail, this.tsmiViewAsLargeIcon, this.tsmiViewAsSmallIcon, this.tsmiViewAsList, this.tsmiViewAsDetails, this.tssView3, this.tsmiColumns, this.tsmiFilter, 
                this.tsmiSort, this.tssView4, this.tsmiCustomizeFolder
             });
            this.tsmiView.Name = "tsmiView";
            manager.ApplyResources(this.tsmiView, "tsmiView");
            this.tsmiView.DropDownClosed += new EventHandler(this.tsmiView_DropDownClosed);
            this.tsmiView.DropDownOpening += new EventHandler(this.tsmiView_DropDownOpening);
            this.actionManager.SetAction(this.tsmiOnePanel, this.actOnePanel);
            this.tsmiOnePanel.Name = "tsmiOnePanel";
            manager.ApplyResources(this.tsmiOnePanel, "tsmiOnePanel");
            this.actionManager.SetAction(this.tsmiTwoHorizontalPanel, this.actTwoHorizontalPanel);
            this.tsmiTwoHorizontalPanel.Name = "tsmiTwoHorizontalPanel";
            manager.ApplyResources(this.tsmiTwoHorizontalPanel, "tsmiTwoHorizontalPanel");
            this.actionManager.SetAction(this.tsmiTwoVerticalPanel, this.actTwoVerticalPanel);
            this.tsmiTwoVerticalPanel.Name = "tsmiTwoVerticalPanel";
            manager.ApplyResources(this.tsmiTwoVerticalPanel, "tsmiTwoVerticalPanel");
            this.tssView1.Name = "tssView1";
            manager.ApplyResources(this.tssView1, "tssView1");
            this.categoryManager.SetCategory(this.tsmiWindowLayout, this.catView);
            this.tsmiWindowLayout.Name = "tsmiWindowLayout";
            manager.ApplyResources(this.tsmiWindowLayout, "tsmiWindowLayout");
            this.categoryManager.SetCategory(this.tsmiFolderBar, this.catView);
            this.tsmiFolderBar.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiFolderBarHidden, this.tsmiFolderBarHorizontal, this.tsmiFolderBarVertical });
            this.tsmiFolderBar.Name = "tsmiFolderBar";
            manager.ApplyResources(this.tsmiFolderBar, "tsmiFolderBar");
            this.tsmiFolderBarHidden.Name = "tsmiFolderBarHidden";
            manager.ApplyResources(this.tsmiFolderBarHidden, "tsmiFolderBarHidden");
            this.tsmiFolderBarHidden.Paint += new PaintEventHandler(this.tsmiFolderBarHidden_Paint);
            this.tsmiFolderBarHorizontal.Name = "tsmiFolderBarHorizontal";
            manager.ApplyResources(this.tsmiFolderBarHorizontal, "tsmiFolderBarHorizontal");
            this.tsmiFolderBarHorizontal.Click += new EventHandler(this.tsmiFolderBarOrientation_Click);
            this.tsmiFolderBarHorizontal.Paint += new PaintEventHandler(this.tsmiFolderBarOrientation_Paint);
            this.tsmiFolderBarVertical.Name = "tsmiFolderBarVertical";
            manager.ApplyResources(this.tsmiFolderBarVertical, "tsmiFolderBarVertical");
            this.tsmiFolderBarVertical.Click += new EventHandler(this.tsmiFolderBarOrientation_Click);
            this.tsmiFolderBarVertical.Paint += new PaintEventHandler(this.tsmiFolderBarOrientation_Paint);
            this.tsmiToolbars.Name = "tsmiToolbars";
            manager.ApplyResources(this.tsmiToolbars, "tsmiToolbars");
            this.tssView2.Name = "tssView2";
            manager.ApplyResources(this.tssView2, "tssView2");
            this.actionManager.SetAction(this.tsmiViewAsThumbnail, this.actViewAsThumbnail);
            this.tsmiViewAsThumbnail.Name = "tsmiViewAsThumbnail";
            manager.ApplyResources(this.tsmiViewAsThumbnail, "tsmiViewAsThumbnail");
            this.actionManager.SetAction(this.tsmiViewAsLargeIcon, this.actViewAsLargeIcon);
            this.tsmiViewAsLargeIcon.Name = "tsmiViewAsLargeIcon";
            manager.ApplyResources(this.tsmiViewAsLargeIcon, "tsmiViewAsLargeIcon");
            this.actionManager.SetAction(this.tsmiViewAsSmallIcon, this.actViewAsSmallIcon);
            this.tsmiViewAsSmallIcon.Name = "tsmiViewAsSmallIcon";
            manager.ApplyResources(this.tsmiViewAsSmallIcon, "tsmiViewAsSmallIcon");
            this.actionManager.SetAction(this.tsmiViewAsList, this.actViewAsList);
            this.tsmiViewAsList.Name = "tsmiViewAsList";
            manager.ApplyResources(this.tsmiViewAsList, "tsmiViewAsList");
            this.actionManager.SetAction(this.tsmiViewAsDetails, this.actViewAsDetails);
            this.tsmiViewAsDetails.Name = "tsmiViewAsDetails";
            manager.ApplyResources(this.tsmiViewAsDetails, "tsmiViewAsDetails");
            this.tssView3.Name = "tssView3";
            manager.ApplyResources(this.tssView3, "tssView3");
            this.tsmiColumns.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiManageColumns, this.tsmiCustomizeToolbars1, this.tsmiCustomizeTools1, this.tsmiViewAs, this.tsmiSetOneListColumn, this.tsmiSetTwoListColumns, this.tsmiSetThreeListColumns, this.tsmiSetFourListColumns, this.tsmiSetFiveListColumns, this.tsmiSetSixListColumns, this.tsmiSetSevenListColumns, this.tsmiSetEightListColumns, this.tsmiSetNineListColumns });
            this.tsmiColumns.Name = "tsmiColumns";
            manager.ApplyResources(this.tsmiColumns, "tsmiColumns");
            this.actionManager.SetAction(this.tsmiManageColumns, this.actManageColumns);
            this.tsmiManageColumns.Name = "tsmiManageColumns";
            manager.ApplyResources(this.tsmiManageColumns, "tsmiManageColumns");
            this.actionManager.SetAction(this.tsmiCustomizeToolbars1, this.actCustomizeToolbars);
            this.tsmiCustomizeToolbars1.Name = "tsmiCustomizeToolbars1";
            manager.ApplyResources(this.tsmiCustomizeToolbars1, "tsmiCustomizeToolbars1");
            this.actionManager.SetAction(this.tsmiCustomizeTools1, this.actCustomizeTools);
            this.tsmiCustomizeTools1.Name = "tsmiCustomizeTools1";
            manager.ApplyResources(this.tsmiCustomizeTools1, "tsmiCustomizeTools1");
            this.tsmiViewAs.Name = "tsmiViewAs";
            manager.ApplyResources(this.tsmiViewAs, "tsmiViewAs");
            this.actionManager.SetAction(this.tsmiSetOneListColumn, this.actSetOneListColumn);
            manager.ApplyResources(this.tsmiSetOneListColumn, "tsmiSetOneListColumn");
            this.tsmiSetOneListColumn.Name = "tsmiSetOneListColumn";
            this.actionManager.SetAction(this.tsmiSetTwoListColumns, this.actSetTwoListColumns);
            manager.ApplyResources(this.tsmiSetTwoListColumns, "tsmiSetTwoListColumns");
            this.tsmiSetTwoListColumns.Name = "tsmiSetTwoListColumns";
            this.actionManager.SetAction(this.tsmiSetThreeListColumns, this.actSetThreeListColumns);
            manager.ApplyResources(this.tsmiSetThreeListColumns, "tsmiSetThreeListColumns");
            this.tsmiSetThreeListColumns.Name = "tsmiSetThreeListColumns";
            this.actionManager.SetAction(this.tsmiSetFourListColumns, this.actSetFourListColumns);
            manager.ApplyResources(this.tsmiSetFourListColumns, "tsmiSetFourListColumns");
            this.tsmiSetFourListColumns.Name = "tsmiSetFourListColumns";
            this.actionManager.SetAction(this.tsmiSetFiveListColumns, this.actSetFiveListColumns);
            manager.ApplyResources(this.tsmiSetFiveListColumns, "tsmiSetFiveListColumns");
            this.tsmiSetFiveListColumns.Name = "tsmiSetFiveListColumns";
            this.actionManager.SetAction(this.tsmiSetSixListColumns, this.actSetSixListColumns);
            manager.ApplyResources(this.tsmiSetSixListColumns, "tsmiSetSixListColumns");
            this.tsmiSetSixListColumns.Name = "tsmiSetSixListColumns";
            this.actionManager.SetAction(this.tsmiSetSevenListColumns, this.actSetSevenListColumns);
            manager.ApplyResources(this.tsmiSetSevenListColumns, "tsmiSetSevenListColumns");
            this.tsmiSetSevenListColumns.Name = "tsmiSetSevenListColumns";
            this.actionManager.SetAction(this.tsmiSetEightListColumns, this.actSetEightListColumns);
            manager.ApplyResources(this.tsmiSetEightListColumns, "tsmiSetEightListColumns");
            this.tsmiSetEightListColumns.Name = "tsmiSetEightListColumns";
            this.actionManager.SetAction(this.tsmiSetNineListColumns, this.actSetNineListColumns);
            manager.ApplyResources(this.tsmiSetNineListColumns, "tsmiSetNineListColumns");
            this.tsmiSetNineListColumns.Name = "tsmiSetNineListColumns";
            this.tsmiFilter.Name = "tsmiFilter";
            manager.ApplyResources(this.tsmiFilter, "tsmiFilter");
            this.tsmiSort.Name = "tsmiSort";
            manager.ApplyResources(this.tsmiSort, "tsmiSort");
            this.tsmiSort.DropDownOpening += new EventHandler(this.tsmiSort_DropDownOpening);
            this.tssView4.Name = "tssView4";
            manager.ApplyResources(this.tssView4, "tssView4");
            this.actionManager.SetAction(this.tsmiCustomizeFolder, this.actCustomizeFolder);
            this.tsmiCustomizeFolder.Name = "tsmiCustomizeFolder";
            manager.ApplyResources(this.tsmiCustomizeFolder, "tsmiCustomizeFolder");
            this.tsmiPanel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiPanel.DropDownItems.AddRange(new ToolStripItem[] { 
                this.tsmiCompareFolders, this.tsmiFolderBranch, this.tsmiCalculateFolderSizes, this.tssPanel1, this.tsmiNavigationLink, this.tsmiLockFolderChange, this.tssPanel2, this.tsmiSwapPanels, this.tsmiEqualizePanels, this.tsmiOpenInFarPanel, this.tssPanel3, this.tsmiBack, this.tsmiForward, this.tssPanel4, this.tsmiChangeDrive, this.tsmiChangeFolder, 
                this.tsmiFtpConnect, this.tsmiSpecialFolders, this.tssPanel5, this.tsmiOpenContainingFolder, this.tssPanel6, this.tsmiRefresh
             });
            this.tsmiPanel.Name = "tsmiPanel";
            manager.ApplyResources(this.tsmiPanel, "tsmiPanel");
            this.actionManager.SetAction(this.tsmiCompareFolders, this.actCompareFolders);
            manager.ApplyResources(this.tsmiCompareFolders, "tsmiCompareFolders");
            this.tsmiCompareFolders.Name = "tsmiCompareFolders";
            this.actionManager.SetAction(this.tsmiFolderBranch, this.actFolderBranch);
            manager.ApplyResources(this.tsmiFolderBranch, "tsmiFolderBranch");
            this.tsmiFolderBranch.Name = "tsmiFolderBranch";
            this.actionManager.SetAction(this.tsmiCalculateFolderSizes, this.actCalculateOnDemandProperties);
            this.tsmiCalculateFolderSizes.Name = "tsmiCalculateFolderSizes";
            manager.ApplyResources(this.tsmiCalculateFolderSizes, "tsmiCalculateFolderSizes");
            this.tssPanel1.Name = "tssPanel1";
            manager.ApplyResources(this.tssPanel1, "tssPanel1");
            this.actionManager.SetAction(this.tsmiNavigationLink, this.actNavigationLink);
            manager.ApplyResources(this.tsmiNavigationLink, "tsmiNavigationLink");
            this.tsmiNavigationLink.Name = "tsmiNavigationLink";
            this.actionManager.SetAction(this.tsmiLockFolderChange, this.actLockFolderChange);
            this.tsmiLockFolderChange.Name = "tsmiLockFolderChange";
            manager.ApplyResources(this.tsmiLockFolderChange, "tsmiLockFolderChange");
            this.tssPanel2.Name = "tssPanel2";
            manager.ApplyResources(this.tssPanel2, "tssPanel2");
            this.actionManager.SetAction(this.tsmiSwapPanels, this.actSwapPanels);
            manager.ApplyResources(this.tsmiSwapPanels, "tsmiSwapPanels");
            this.tsmiSwapPanels.Name = "tsmiSwapPanels";
            this.actionManager.SetAction(this.tsmiEqualizePanels, this.actEqualizePanels);
            manager.ApplyResources(this.tsmiEqualizePanels, "tsmiEqualizePanels");
            this.tsmiEqualizePanels.Name = "tsmiEqualizePanels";
            this.actionManager.SetAction(this.tsmiOpenInFarPanel, this.actOpenInFarPanel);
            manager.ApplyResources(this.tsmiOpenInFarPanel, "tsmiOpenInFarPanel");
            this.tsmiOpenInFarPanel.Name = "tsmiOpenInFarPanel";
            this.tssPanel3.Name = "tssPanel3";
            manager.ApplyResources(this.tssPanel3, "tssPanel3");
            this.actionManager.SetAction(this.tsmiBack, this.actBack);
            manager.ApplyResources(this.tsmiBack, "tsmiBack");
            this.tsmiBack.Name = "tsmiBack";
            this.actionManager.SetAction(this.tsmiForward, this.actForward);
            manager.ApplyResources(this.tsmiForward, "tsmiForward");
            this.tsmiForward.Name = "tsmiForward";
            this.tssPanel4.Name = "tssPanel4";
            manager.ApplyResources(this.tssPanel4, "tssPanel4");
            this.actionManager.SetAction(this.tsmiChangeDrive, this.actChangeDrive);
            this.tsmiChangeDrive.Name = "tsmiChangeDrive";
            manager.ApplyResources(this.tsmiChangeDrive, "tsmiChangeDrive");
            this.actionManager.SetAction(this.tsmiChangeFolder, this.actChangeFolder);
            this.tsmiChangeFolder.Name = "tsmiChangeFolder";
            manager.ApplyResources(this.tsmiChangeFolder, "tsmiChangeFolder");
            this.actionManager.SetAction(this.tsmiFtpConnect, this.actFtpConnect);
            this.tsmiFtpConnect.Name = "tsmiFtpConnect";
            manager.ApplyResources(this.tsmiFtpConnect, "tsmiFtpConnect");
            this.categoryManager.SetCategory(this.tsmiSpecialFolders, this.catPanel);
            this.tsmiSpecialFolders.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiFolderMyDocuments, this.tsmiFolderMyPictures, this.tsmiFolderMyMusic, this.tssSpecialFolder1, this.tsmiFolderFavorites, this.tsmiFolderDesktop, this.tssSpecialFolder2, this.tsmiFolderTemp, this.tsmiFolderWindows, this.tsmiFolderSystem });
            this.tsmiSpecialFolders.Name = "tsmiSpecialFolders";
            manager.ApplyResources(this.tsmiSpecialFolders, "tsmiSpecialFolders");
            this.categoryManager.SetCategory(this.tsmiFolderMyDocuments, this.catPanel);
            this.tsmiFolderMyDocuments.Name = "tsmiFolderMyDocuments";
            manager.ApplyResources(this.tsmiFolderMyDocuments, "tsmiFolderMyDocuments");
            this.tsmiFolderMyDocuments.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderMyDocuments.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.categoryManager.SetCategory(this.tsmiFolderMyPictures, this.catPanel);
            this.tsmiFolderMyPictures.Name = "tsmiFolderMyPictures";
            manager.ApplyResources(this.tsmiFolderMyPictures, "tsmiFolderMyPictures");
            this.tsmiFolderMyPictures.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderMyPictures.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.categoryManager.SetCategory(this.tsmiFolderMyMusic, this.catPanel);
            this.tsmiFolderMyMusic.Name = "tsmiFolderMyMusic";
            manager.ApplyResources(this.tsmiFolderMyMusic, "tsmiFolderMyMusic");
            this.tsmiFolderMyMusic.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderMyMusic.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.tssSpecialFolder1.Name = "tssSpecialFolder1";
            manager.ApplyResources(this.tssSpecialFolder1, "tssSpecialFolder1");
            this.categoryManager.SetCategory(this.tsmiFolderFavorites, this.catPanel);
            this.tsmiFolderFavorites.Name = "tsmiFolderFavorites";
            manager.ApplyResources(this.tsmiFolderFavorites, "tsmiFolderFavorites");
            this.tsmiFolderFavorites.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderFavorites.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.categoryManager.SetCategory(this.tsmiFolderDesktop, this.catPanel);
            this.tsmiFolderDesktop.Name = "tsmiFolderDesktop";
            manager.ApplyResources(this.tsmiFolderDesktop, "tsmiFolderDesktop");
            this.tsmiFolderDesktop.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderDesktop.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.tssSpecialFolder2.Name = "tssSpecialFolder2";
            manager.ApplyResources(this.tssSpecialFolder2, "tssSpecialFolder2");
            this.categoryManager.SetCategory(this.tsmiFolderTemp, this.catPanel);
            this.tsmiFolderTemp.Name = "tsmiFolderTemp";
            manager.ApplyResources(this.tsmiFolderTemp, "tsmiFolderTemp");
            this.tsmiFolderTemp.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderTemp.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.categoryManager.SetCategory(this.tsmiFolderWindows, this.catPanel);
            this.tsmiFolderWindows.Name = "tsmiFolderWindows";
            manager.ApplyResources(this.tsmiFolderWindows, "tsmiFolderWindows");
            this.tsmiFolderWindows.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderWindows.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.categoryManager.SetCategory(this.tsmiFolderSystem, this.catPanel);
            this.tsmiFolderSystem.Name = "tsmiFolderSystem";
            manager.ApplyResources(this.tsmiFolderSystem, "tsmiFolderSystem");
            this.tsmiFolderSystem.Click += new EventHandler(this.tsmiSpecialFolder_Click);
            this.tsmiFolderSystem.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
            this.tssPanel5.Name = "tssPanel5";
            manager.ApplyResources(this.tssPanel5, "tssPanel5");
            this.actionManager.SetAction(this.tsmiOpenContainingFolder, this.actOpenContainingFolder);
            manager.ApplyResources(this.tsmiOpenContainingFolder, "tsmiOpenContainingFolder");
            this.tsmiOpenContainingFolder.Name = "tsmiOpenContainingFolder";
            this.tssPanel6.Name = "tssPanel6";
            manager.ApplyResources(this.tssPanel6, "tssPanel6");
            this.actionManager.SetAction(this.tsmiRefresh, this.actRefresh);
            this.tsmiRefresh.Name = "tsmiRefresh";
            manager.ApplyResources(this.tsmiRefresh, "tsmiRefresh");
            this.tsmiTab.Name = "tsmiTab";
            manager.ApplyResources(this.tsmiTab, "tsmiTab");
            this.tsmiBookmarks.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiBookmarks.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiBookmarkCurrentFolder, this.tsmiBookmarkCurrentTab, this.tsmiOrganizeBookmarks, this.tssBookmarks1, this.tsmiEmpty });
            this.tsmiBookmarks.Name = "tsmiBookmarks";
            manager.ApplyResources(this.tsmiBookmarks, "tsmiBookmarks");
            this.actionManager.SetAction(this.tsmiBookmarkCurrentFolder, this.actBookmarkCurrentFolder);
            manager.ApplyResources(this.tsmiBookmarkCurrentFolder, "tsmiBookmarkCurrentFolder");
            this.tsmiBookmarkCurrentFolder.Name = "tsmiBookmarkCurrentFolder";
            this.actionManager.SetAction(this.tsmiBookmarkCurrentTab, this.actBookmarkCurrentTab);
            this.tsmiBookmarkCurrentTab.Name = "tsmiBookmarkCurrentTab";
            manager.ApplyResources(this.tsmiBookmarkCurrentTab, "tsmiBookmarkCurrentTab");
            this.actionManager.SetAction(this.tsmiOrganizeBookmarks, this.actOrganizeBookmarks);
            this.tsmiOrganizeBookmarks.Name = "tsmiOrganizeBookmarks";
            manager.ApplyResources(this.tsmiOrganizeBookmarks, "tsmiOrganizeBookmarks");
            this.tssBookmarks1.Name = "tssBookmarks1";
            manager.ApplyResources(this.tssBookmarks1, "tssBookmarks1");
            manager.ApplyResources(this.tsmiEmpty, "tsmiEmpty");
            this.tsmiEmpty.Name = "tsmiEmpty";
            this.tsmiTools.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiTools.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiOptions, this.tssTools1 });
            this.tsmiTools.Name = "tsmiTools";
            manager.ApplyResources(this.tsmiTools, "tsmiTools");
            this.tsmiTools.DropDownOpening += new EventHandler(this.tsmiTools_DropDownOpening);
            this.actionManager.SetAction(this.tsmiOptions, this.actOptions);
            this.tsmiOptions.Name = "tsmiOptions";
            manager.ApplyResources(this.tsmiOptions, "tsmiOptions");
            this.tssTools1.Name = "tssTools1";
            manager.ApplyResources(this.tssTools1, "tssTools1");
            this.tsmiHelp.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmiHelp.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiHelpContents, this.tsmiShowCmdLineHelp, this.tssHelp1, this.tsmiCheckForUpdates, this.tssHelp2, this.tsmiAbout });
            this.tsmiHelp.Name = "tsmiHelp";
            manager.ApplyResources(this.tsmiHelp, "tsmiHelp");
            this.actionManager.SetAction(this.tsmiHelpContents, this.actHelpContents);
            this.tsmiHelpContents.Name = "tsmiHelpContents";
            manager.ApplyResources(this.tsmiHelpContents, "tsmiHelpContents");
            this.actionManager.SetAction(this.tsmiShowCmdLineHelp, this.actShowCmdLineHelp);
            this.tsmiShowCmdLineHelp.Name = "tsmiShowCmdLineHelp";
            manager.ApplyResources(this.tsmiShowCmdLineHelp, "tsmiShowCmdLineHelp");
            this.tssHelp1.Name = "tssHelp1";
            manager.ApplyResources(this.tssHelp1, "tssHelp1");
            this.actionManager.SetAction(this.tsmiCheckForUpdates, this.actCheckForUpdates);
            this.tsmiCheckForUpdates.Name = "tsmiCheckForUpdates";
            manager.ApplyResources(this.tsmiCheckForUpdates, "tsmiCheckForUpdates");
            this.tssHelp2.Name = "tssHelp2";
            manager.ApplyResources(this.tssHelp2, "tssHelp2");
            this.actionManager.SetAction(this.tsmiAbout, this.actAbout);
            this.tsmiAbout.Name = "tsmiAbout";
            manager.ApplyResources(this.tsmiAbout, "tsmiAbout");
            this.actionManager.SetAction(this.tsmiSelectSort, this.actSelectSort);
            this.tsmiSelectSort.Name = "tsmiSelectSort";
            manager.ApplyResources(this.tsmiSelectSort, "tsmiSelectSort");
            this.tssSort1.Name = "tssSort1";
            manager.ApplyResources(this.tssSort1, "tssSort1");
            this.actionManager.SetAction(this.tsmiSortByName, this.actSortByName);
            this.tsmiSortByName.Name = "tsmiSortByName";
            manager.ApplyResources(this.tsmiSortByName, "tsmiSortByName");
            this.actionManager.SetAction(this.tsmiSortByExtension, this.actSortByExtension);
            this.tsmiSortByExtension.Name = "tsmiSortByExtension";
            manager.ApplyResources(this.tsmiSortByExtension, "tsmiSortByExtension");
            this.actionManager.SetAction(this.tsmiSortByLastWriteTime, this.actSortByLastWriteTime);
            this.tsmiSortByLastWriteTime.Name = "tsmiSortByLastWriteTime";
            manager.ApplyResources(this.tsmiSortByLastWriteTime, "tsmiSortByLastWriteTime");
            this.actionManager.SetAction(this.tsmiSortBySize, this.actSortBySize);
            this.tsmiSortBySize.Name = "tsmiSortBySize";
            manager.ApplyResources(this.tsmiSortBySize, "tsmiSortBySize");
            this.tssSort2.Name = "tssSort2";
            manager.ApplyResources(this.tssSort2, "tssSort2");
            this.actionManager.SetAction(this.tsmiSortDescending, this.actSortDescending);
            this.tsmiSortDescending.Name = "tsmiSortDescending";
            manager.ApplyResources(this.tsmiSortDescending, "tsmiSortDescending");
            this.actionManager.SetAction(this.tsmiDuplicateTab, this.actDuplicateTab);
            this.tsmiDuplicateTab.Name = "tsmiDuplicateTab";
            manager.ApplyResources(this.tsmiDuplicateTab, "tsmiDuplicateTab");
            this.actionManager.SetAction(this.tsmiRenameTab, this.actRenameTab);
            this.tsmiRenameTab.Name = "tsmiRenameTab";
            manager.ApplyResources(this.tsmiRenameTab, "tsmiRenameTab");
            this.tssTab1.Name = "tssTab1";
            manager.ApplyResources(this.tssTab1, "tssTab1");
            this.actionManager.SetAction(this.tsmiCloseTab, this.actCloseTab);
            this.tsmiCloseTab.Name = "tsmiCloseTab";
            manager.ApplyResources(this.tsmiCloseTab, "tsmiCloseTab");
            this.actionManager.SetAction(this.tsmiCloseOtherTabs, this.actCloseOtherTabs);
            this.tsmiCloseOtherTabs.Name = "tsmiCloseOtherTabs";
            manager.ApplyResources(this.tsmiCloseOtherTabs, "tsmiCloseOtherTabs");
            this.tssTab2.Name = "tssTab2";
            manager.ApplyResources(this.tssTab2, "tssTab2");
            this.actionManager.SetAction(this.tsmiFilterDialog, this.actAdvancedFilter);
            this.tsmiFilterDialog.Name = "tsmiFilterDialog";
            manager.ApplyResources(this.tsmiFilterDialog, "tsmiFilterDialog");
            this.tssAdvancedFilter1.Name = "tssAdvancedFilter1";
            manager.ApplyResources(this.tssAdvancedFilter1, "tssAdvancedFilter1");
            this.actionManager.SetAction(this.tsmiClearFilter, this.actClearFilter);
            this.tsmiClearFilter.Name = "tsmiClearFilter";
            manager.ApplyResources(this.tsmiClearFilter, "tsmiClearFilter");
            this.tssAdvancedFilter2.Name = "tssAdvancedFilter2";
            manager.ApplyResources(this.tssAdvancedFilter2, "tssAdvancedFilter2");
            manager.ApplyResources(this.tsmiNoStoredFilters, "tsmiNoStoredFilters");
            this.tsmiNoStoredFilters.Name = "tsmiNoStoredFilters";
            this.actionManager.SetAction(this.tsmiSaveCurrentLayout, this.actSaveCurrentLayout);
            this.tsmiSaveCurrentLayout.Name = "tsmiSaveCurrentLayout";
            manager.ApplyResources(this.tsmiSaveCurrentLayout, "tsmiSaveCurrentLayout");
            this.actionManager.SetAction(this.tsmiManageLayouts, this.actManageLayouts);
            this.tsmiManageLayouts.Name = "tsmiManageLayouts";
            manager.ApplyResources(this.tsmiManageLayouts, "tsmiManageLayouts");
            this.tssLayout1.Name = "tssLayout1";
            manager.ApplyResources(this.tssLayout1, "tssLayout1");
            manager.ApplyResources(this.tsmiNoStoredLayouts, "tsmiNoStoredLayouts");
            this.tsmiNoStoredLayouts.Name = "tsmiNoStoredLayouts";
            this.actionManager.SetAction(this.tsmiFindDialog, this.actFind);
            this.tsmiFindDialog.Name = "tsmiFindDialog";
            manager.ApplyResources(this.tsmiFindDialog, "tsmiFindDialog");
            this.tssFind.Name = "tssFind";
            manager.ApplyResources(this.tssFind, "tssFind");
            manager.ApplyResources(this.tsmiNoStoredSearches, "tsmiNoStoredSearches");
            this.tsmiNoStoredSearches.Name = "tsmiNoStoredSearches";
            this.actionManager.SetAction(this.tsmiNewFile, this.actNewFile);
            this.tsmiNewFile.Name = "tsmiNewFile";
            manager.ApplyResources(this.tsmiNewFile, "tsmiNewFile");
            this.tssNewFile.Name = "tssNewFile";
            manager.ApplyResources(this.tssNewFile, "tssNewFile");
            this.FindExeFileDialog.AddExtension = false;
            manager.ApplyResources(this.FindExeFileDialog, "FindExeFileDialog");
            this.largeImageList.ColorDepth = ColorDepth.Depth32Bit;
            manager.ApplyResources(this.largeImageList, "largeImageList");
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.cmsMenuViewAs.Items.AddRange(new ToolStripItem[] { this.tsmiViewAsThumbnail2, this.tsmiViewAsLargeIcon2, this.tsmiViewAsSmallIcon2, this.tsmiViewAsList2, this.tsmiViewAsDetails2, this.tssViewAs1, this.tsmiManageColumns2, this.tsmiSetOneListColumn2, this.tsmiSetTwoListColumns2, this.tsmiSetThreeListColumns2, this.tsmiSetFourListColumns2, this.tsmiSetFiveListColumns2, this.tsmiSetSixListColumns2, this.tsmiSetSevenListColumns2, this.tsmiSetEightListColumns2, this.tsmiSetNineListColumns2 });
            this.cmsMenuViewAs.Name = "cmsViewAs";
            manager.ApplyResources(this.cmsMenuViewAs, "cmsMenuViewAs");
            this.cmsMenuViewAs.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
            this.cmsMenuViewAs.Opening += new CancelEventHandler(this.cmsViewAs_Opening);
            this.actionManager.SetAction(this.tsmiViewAsThumbnail2, this.actViewAsThumbnail);
            this.tsmiViewAsThumbnail2.Name = "tsmiViewAsThumbnail2";
            manager.ApplyResources(this.tsmiViewAsThumbnail2, "tsmiViewAsThumbnail2");
            this.actionManager.SetAction(this.tsmiViewAsLargeIcon2, this.actViewAsLargeIcon);
            this.tsmiViewAsLargeIcon2.Name = "tsmiViewAsLargeIcon2";
            manager.ApplyResources(this.tsmiViewAsLargeIcon2, "tsmiViewAsLargeIcon2");
            this.actionManager.SetAction(this.tsmiViewAsSmallIcon2, this.actViewAsSmallIcon);
            this.tsmiViewAsSmallIcon2.Name = "tsmiViewAsSmallIcon2";
            manager.ApplyResources(this.tsmiViewAsSmallIcon2, "tsmiViewAsSmallIcon2");
            this.actionManager.SetAction(this.tsmiViewAsList2, this.actViewAsList);
            this.tsmiViewAsList2.Name = "tsmiViewAsList2";
            manager.ApplyResources(this.tsmiViewAsList2, "tsmiViewAsList2");
            this.actionManager.SetAction(this.tsmiViewAsDetails2, this.actViewAsDetails);
            this.tsmiViewAsDetails2.Name = "tsmiViewAsDetails2";
            manager.ApplyResources(this.tsmiViewAsDetails2, "tsmiViewAsDetails2");
            this.tssViewAs1.Name = "tssViewAs1";
            manager.ApplyResources(this.tssViewAs1, "tssViewAs1");
            this.actionManager.SetAction(this.tsmiManageColumns2, this.actManageColumns);
            this.tsmiManageColumns2.Name = "tsmiManageColumns2";
            manager.ApplyResources(this.tsmiManageColumns2, "tsmiManageColumns2");
            this.actionManager.SetAction(this.tsmiSetOneListColumn2, this.actSetOneListColumn);
            manager.ApplyResources(this.tsmiSetOneListColumn2, "tsmiSetOneListColumn2");
            this.tsmiSetOneListColumn2.Name = "tsmiSetOneListColumn2";
            this.actionManager.SetAction(this.tsmiSetTwoListColumns2, this.actSetTwoListColumns);
            manager.ApplyResources(this.tsmiSetTwoListColumns2, "tsmiSetTwoListColumns2");
            this.tsmiSetTwoListColumns2.Name = "tsmiSetTwoListColumns2";
            this.actionManager.SetAction(this.tsmiSetThreeListColumns2, this.actSetThreeListColumns);
            manager.ApplyResources(this.tsmiSetThreeListColumns2, "tsmiSetThreeListColumns2");
            this.tsmiSetThreeListColumns2.Name = "tsmiSetThreeListColumns2";
            this.actionManager.SetAction(this.tsmiSetFourListColumns2, this.actSetFourListColumns);
            manager.ApplyResources(this.tsmiSetFourListColumns2, "tsmiSetFourListColumns2");
            this.tsmiSetFourListColumns2.Name = "tsmiSetFourListColumns2";
            this.actionManager.SetAction(this.tsmiSetFiveListColumns2, this.actSetFiveListColumns);
            manager.ApplyResources(this.tsmiSetFiveListColumns2, "tsmiSetFiveListColumns2");
            this.tsmiSetFiveListColumns2.Name = "tsmiSetFiveListColumns2";
            this.actionManager.SetAction(this.tsmiSetSixListColumns2, this.actSetSixListColumns);
            manager.ApplyResources(this.tsmiSetSixListColumns2, "tsmiSetSixListColumns2");
            this.tsmiSetSixListColumns2.Name = "tsmiSetSixListColumns2";
            this.actionManager.SetAction(this.tsmiSetSevenListColumns2, this.actSetSevenListColumns);
            manager.ApplyResources(this.tsmiSetSevenListColumns2, "tsmiSetSevenListColumns2");
            this.tsmiSetSevenListColumns2.Name = "tsmiSetSevenListColumns2";
            this.actionManager.SetAction(this.tsmiSetEightListColumns2, this.actSetEightListColumns);
            manager.ApplyResources(this.tsmiSetEightListColumns2, "tsmiSetEightListColumns2");
            this.tsmiSetEightListColumns2.Name = "tsmiSetEightListColumns2";
            this.actionManager.SetAction(this.tsmiSetNineListColumns2, this.actSetNineListColumns);
            manager.ApplyResources(this.tsmiSetNineListColumns2, "tsmiSetNineListColumns2");
            this.tsmiSetNineListColumns2.Name = "tsmiSetNineListColumns2";
            this.cmsTab.Items.AddRange(new ToolStripItem[] { this.tsmiDuplicateTab2, this.tssTab10, this.tsmiCloseTab2, this.tsmiCloseOtherTabs2, this.tsmiCloseTabsToRight2, this.tssTab11, this.tsmiRenameTab2 });
            this.cmsTab.Name = "cmsTab";
            manager.ApplyResources(this.cmsTab, "cmsTab");
            this.cmsTab.Opening += new CancelEventHandler(this.cmsTab_Opening);
            this.actionManager.SetAction(this.tsmiDuplicateTab2, this.actDuplicateTab);
            this.tsmiDuplicateTab2.Name = "tsmiDuplicateTab2";
            manager.ApplyResources(this.tsmiDuplicateTab2, "tsmiDuplicateTab2");
            this.tssTab10.Name = "tssTab10";
            manager.ApplyResources(this.tssTab10, "tssTab10");
            this.actionManager.SetAction(this.tsmiCloseTab2, this.actCloseTab);
            manager.ApplyResources(this.tsmiCloseTab2, "tsmiCloseTab2");
            this.tsmiCloseTab2.Name = "tsmiCloseTab2";
            this.actionManager.SetAction(this.tsmiCloseOtherTabs2, this.actCloseOtherTabs);
            manager.ApplyResources(this.tsmiCloseOtherTabs2, "tsmiCloseOtherTabs2");
            this.tsmiCloseOtherTabs2.Name = "tsmiCloseOtherTabs2";
            this.actionManager.SetAction(this.tsmiCloseTabsToRight2, this.actCloseTabsToRight);
            manager.ApplyResources(this.tsmiCloseTabsToRight2, "tsmiCloseTabsToRight2");
            this.tsmiCloseTabsToRight2.Name = "tsmiCloseTabsToRight2";
            this.tssTab11.Name = "tssTab11";
            manager.ApplyResources(this.tssTab11, "tssTab11");
            this.actionManager.SetAction(this.tsmiRenameTab2, this.actRenameTab);
            manager.ApplyResources(this.tsmiRenameTab2, "tsmiRenameTab2");
            this.tsmiRenameTab2.Name = "tsmiRenameTab2";
            this.MainPageSwitcher.BackColor = System.Drawing.Color.FromArgb(0xeb, 0xe9, 0xed);
            manager.ApplyResources(this.MainPageSwitcher, "MainPageSwitcher");
            this.MainPageSwitcher.Name = "MainPageSwitcher";
            this.MainPageSwitcher.SelectedTabStripPage = null;
            this.MainPageSwitcher.TabStrip = this.MainTabStrip;
            this.MainPageSwitcher.ControlAdded += new ControlEventHandler(this.MainPageSwitcher_ControlAdded);
            this.MainPageSwitcher.ControlRemoved += new ControlEventHandler(this.MainPageSwitcher_ControlAdded);
            this.MainTabStrip.AllowItemReorder = true;
            this.MainTabStrip.ContextMenuStrip = this.cmsTab;
            this.MainTabStrip.Items.AddRange(new ToolStripItem[] { this.tsbCloseTab });
            manager.ApplyResources(this.MainTabStrip, "MainTabStrip");
            this.MainTabStrip.Name = "MainTabStrip";
            this.MainTabStrip.SelectedTab = null;
            this.MainTabStrip.ShowItemToolTips = true;
            this.MainTabStrip.AfterPaint += new PaintEventHandler(this.MainTabStrip_AfterPaint);
            this.MainTabStrip.SelectedTabChanging += new EventHandler<TabStripCancelEventArgs>(this.MainTabStrip_SelectedTabChanging);
            this.MainTabStrip.SelectedTabChanged += new EventHandler(this.MainTabStrip_SelectedTabChanged);
            this.MainTabStrip.SizeChanged += new EventHandler(this.MainTabStrip_SizeChanged);
            this.MainTabStrip.DoubleClick += new EventHandler(this.MainTabStrip_DoubleClick);
            this.tsbCloseTab.Alignment = ToolStripItemAlignment.Right;
            manager.ApplyResources(this.tsbCloseTab, "tsbCloseTab");
            this.tsbCloseTab.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbCloseTab.Image = Resources.CloseTab;
            this.tsbCloseTab.Name = "tsbCloseTab";
            this.categoryManager.SetCategory(this.actViewItem, this.catFile);
            this.actViewItem.Name = "actViewItem";
            this.actViewItem.Shortcuts = new Keys[] { Keys.F3, Keys.Shift | Keys.F3 };
            manager.ApplyResources(this.actViewItem, "actViewItem");
            this.actViewItem.OnExecute += new EventHandler<ActionEventArgs>(this.actViewItem_OnExecute);
            this.actViewItem.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actViewItem_OnUpdate);
            this.categoryManager.SetCategory(this.actEditItem, this.catFile);
            this.actEditItem.Name = "actEditItem";
            this.actEditItem.Shortcuts = new Keys[] { Keys.F4, Keys.Shift | Keys.F4 };
            manager.ApplyResources(this.actEditItem, "actEditItem");
            this.actEditItem.OnExecute += new EventHandler<ActionEventArgs>(this.actEditItem_OnExecute);
            this.actEditItem.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actEditItem_OnUpdate);
            this.categoryManager.SetCategory(this.actFind, this.catFile);
            this.actFind.Name = "actFind";
            this.actFind.ShortcutKeys = Keys.Alt | Keys.F7;
            manager.ApplyResources(this.actFind, "actFind");
            this.actFind.OnExecute += new EventHandler<ActionEventArgs>(this.actFindDialog_OnExecute);
            this.categoryManager.SetCategory(this.actCopy, this.catFile);
            this.actCopy.Name = "actCopy";
            this.actCopy.ShortcutKeys = Keys.F5;
            manager.ApplyResources(this.actCopy, "actCopy");
            this.actCopy.OnExecute += new EventHandler<ActionEventArgs>(this.actCopy_OnExecute);
            this.actCopy.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actRenameMove, this.catFile);
            this.actRenameMove.Name = "actRenameMove";
            this.actRenameMove.ShortcutKeys = Keys.F6;
            this.actRenameMove.Tag = "true";
            manager.ApplyResources(this.actRenameMove, "actRenameMove");
            this.actRenameMove.OnExecute += new EventHandler<ActionEventArgs>(this.actCopy_OnExecute);
            this.actRenameMove.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actRenameSingleItem, this.catFile);
            this.actRenameSingleItem.Name = "actRenameSingleItem";
            this.actRenameSingleItem.Shortcuts = new Keys[] { Keys.F2, Keys.Shift | Keys.F6 };
            manager.ApplyResources(this.actRenameSingleItem, "actRenameSingleItem");
            this.actRenameSingleItem.OnExecute += new EventHandler<ActionEventArgs>(this.actRenameSingleItem_OnExecute);
            this.actRenameSingleItem.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRenameSingleItem_OnUpdate);
            this.categoryManager.SetCategory(this.actMakeFolder, this.catFile);
            this.actMakeFolder.Name = "actMakeFolder";
            this.actMakeFolder.Shortcuts = new Keys[] { Keys.F7, Keys.Control | Keys.Shift | Keys.N };
            manager.ApplyResources(this.actMakeFolder, "actMakeFolder");
            this.actMakeFolder.OnExecute += new EventHandler<ActionEventArgs>(this.actMakeFolder_OnExecute);
            this.actMakeFolder.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMakeFolder_OnUpdate);
            this.categoryManager.SetCategory(this.actDelete, this.catFile);
            this.actDelete.Name = "actDelete";
            this.actDelete.Shortcuts = new Keys[] { Keys.F8, Keys.Shift | Keys.F8, Keys.Delete, Keys.Shift | Keys.Delete };
            manager.ApplyResources(this.actDelete, "actDelete");
            this.actDelete.OnExecute += new EventHandler<ActionEventArgs>(this.actDelete_OnExecute);
            this.actDelete.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actDeleteSingleItem, this.catFile);
            this.actDeleteSingleItem.Name = "actDeleteSingleItem";
            this.actDeleteSingleItem.ShortcutKeys = Keys.Control | Keys.Delete;
            manager.ApplyResources(this.actDeleteSingleItem, "actDeleteSingleItem");
            this.actDeleteSingleItem.OnExecute += new EventHandler<ActionEventArgs>(this.actDeleteSingleItem_OnExecute);
            this.actDeleteSingleItem.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRenameSingleItem_OnUpdate);
            this.categoryManager.SetCategory(this.actRunAs, this.catFile);
            this.actRunAs.Name = "actRunAs";
            manager.ApplyResources(this.actRunAs, "actRunAs");
            this.actRunAs.OnExecute += new EventHandler<ActionEventArgs>(this.actRunAs_OnExecute);
            this.actRunAs.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRunAs_OnUpdate);
            this.categoryManager.SetCategory(this.actSetAttributes, this.catFile);
            this.actSetAttributes.Name = "actSetAttributes";
            this.actSetAttributes.ShortcutKeys = Keys.Control | Keys.E;
            manager.ApplyResources(this.actSetAttributes, "actSetAttributes");
            this.actSetAttributes.OnExecute += new EventHandler<ActionEventArgs>(this.actSetAttributes_OnExecute);
            this.actSetAttributes.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actShowProperties, this.catFile);
            this.actShowProperties.Name = "actShowProperties";
            manager.ApplyResources(this.actShowProperties, "actShowProperties");
            this.actShowProperties.OnExecute += new EventHandler<ActionEventArgs>(this.actShowProperties_OnExecute);
            this.actShowProperties.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actCutToClipboard, this.catEdit);
            this.actCutToClipboard.Name = "actCutToClipboard";
            this.actCutToClipboard.ShortcutKeys = Keys.Control | Keys.X;
            this.actCutToClipboard.Tag = "true";
            manager.ApplyResources(this.actCutToClipboard, "actCutToClipboard");
            this.actCutToClipboard.OnExecute += new EventHandler<ActionEventArgs>(this.actCopyToClipboard_OnExecute);
            this.actCutToClipboard.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actCopyToClipboard, this.catEdit);
            this.actCopyToClipboard.Name = "actCopyToClipboard";
            this.actCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
            manager.ApplyResources(this.actCopyToClipboard, "actCopyToClipboard");
            this.actCopyToClipboard.OnExecute += new EventHandler<ActionEventArgs>(this.actCopyToClipboard_OnExecute);
            this.actCopyToClipboard.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actPasteFromClipboard, this.catEdit);
            this.actPasteFromClipboard.Name = "actPasteFromClipboard";
            this.actPasteFromClipboard.ShortcutKeys = Keys.Control | Keys.V;
            manager.ApplyResources(this.actPasteFromClipboard, "actPasteFromClipboard");
            this.actPasteFromClipboard.OnExecute += new EventHandler<ActionEventArgs>(this.actPasteFromClipboard_OnExecute);
            this.actPasteFromClipboard.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actPasteFromClipboard_OnUpdate);
            this.categoryManager.SetCategory(this.actPasteShortCut, this.catEdit);
            this.actPasteShortCut.Name = "actPasteShortCut";
            this.actPasteShortCut.ShortcutKeys = Keys.Control | Keys.S;
            manager.ApplyResources(this.actPasteShortCut, "actPasteShortCut");
            this.actPasteShortCut.OnExecute += new EventHandler<ActionEventArgs>(this.actPasteShortCut_OnExecute);
            this.actPasteShortCut.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actPasteShortCut_OnUpdate);
            this.categoryManager.SetCategory(this.actCopyNameAsText, this.catEdit);
            this.actCopyNameAsText.Name = "actCopyNameAsText";
            this.actCopyNameAsText.ShortcutKeys = Keys.Alt | Keys.Control | Keys.C;
            manager.ApplyResources(this.actCopyNameAsText, "actCopyNameAsText");
            this.actCopyNameAsText.OnExecute += new EventHandler<ActionEventArgs>(this.actCopyNameAsText_OnExecute);
            this.actCopyNameAsText.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actCopyFullNameAsText, this.catEdit);
            this.actCopyFullNameAsText.Name = "actCopyFullNameAsText";
            this.actCopyFullNameAsText.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            manager.ApplyResources(this.actCopyFullNameAsText, "actCopyFullNameAsText");
            this.actCopyFullNameAsText.OnExecute += new EventHandler<ActionEventArgs>(this.actCopyNameAsText_OnExecute);
            this.actCopyFullNameAsText.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actEmptyClipboard, this.catEdit);
            this.actEmptyClipboard.Name = "actEmptyClipboard";
            manager.ApplyResources(this.actEmptyClipboard, "actEmptyClipboard");
            this.actEmptyClipboard.OnExecute += new EventHandler<ActionEventArgs>(this.actEmptyClipboard_OnExecute);
            this.actEmptyClipboard.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actEmptyClipboard_OnUpdate);
            this.categoryManager.SetCategory(this.actSelect, this.catEdit);
            this.actSelect.Name = "actSelect";
            manager.ApplyResources(this.actSelect, "actSelect");
            this.actSelect.OnExecute += new EventHandler<ActionEventArgs>(this.actSelect_OnExecute);
            this.categoryManager.SetCategory(this.actUnselect, this.catEdit);
            this.actUnselect.Name = "actUnselect";
            manager.ApplyResources(this.actUnselect, "actUnselect");
            this.actUnselect.OnExecute += new EventHandler<ActionEventArgs>(this.actUnselect_OnExecute);
            this.actUnselect.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actUnselect_OnUpdate);
            this.categoryManager.SetCategory(this.actInvertSelection, this.catEdit);
            this.actInvertSelection.Name = "actInvertSelection";
            manager.ApplyResources(this.actInvertSelection, "actInvertSelection");
            this.actInvertSelection.OnExecute += new EventHandler<ActionEventArgs>(this.actInvertSelection_OnExecute);
            this.categoryManager.SetCategory(this.actRestoreSelection, this.catEdit);
            this.actRestoreSelection.Name = "actRestoreSelection";
            manager.ApplyResources(this.actRestoreSelection, "actRestoreSelection");
            this.actRestoreSelection.OnExecute += new EventHandler<ActionEventArgs>(this.actRestoreSelection_OnExecute);
            this.actRestoreSelection.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRestoreSelection_OnUpdate);
            this.categoryManager.SetCategory(this.actSelectAll, this.catEdit);
            this.actSelectAll.Name = "actSelectAll";
            this.actSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            manager.ApplyResources(this.actSelectAll, "actSelectAll");
            this.actSelectAll.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectAll_OnExecute);
            this.categoryManager.SetCategory(this.actOnePanel, this.catView);
            this.actOnePanel.Name = "actOnePanel";
            manager.ApplyResources(this.actOnePanel, "actOnePanel");
            this.actOnePanel.OnExecute += new EventHandler<ActionEventArgs>(this.actOnePanel_OnExecute);
            this.actOnePanel.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actOnePanel_OnUpdate);
            this.categoryManager.SetCategory(this.actTwoHorizontalPanel, this.catView);
            this.actTwoHorizontalPanel.Name = "actTwoHorizontalPanel";
            manager.ApplyResources(this.actTwoHorizontalPanel, "actTwoHorizontalPanel");
            this.actTwoHorizontalPanel.OnExecute += new EventHandler<ActionEventArgs>(this.actTwoPanel_OnExecute);
            this.actTwoHorizontalPanel.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actTwoPanel_OnUpdate);
            this.categoryManager.SetCategory(this.actTwoVerticalPanel, this.catView);
            this.actTwoVerticalPanel.Name = "actTwoVerticalPanel";
            manager.ApplyResources(this.actTwoVerticalPanel, "actTwoVerticalPanel");
            this.actTwoVerticalPanel.OnExecute += new EventHandler<ActionEventArgs>(this.actTwoPanel_OnExecute);
            this.actTwoVerticalPanel.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actTwoPanel_OnUpdate);
            this.categoryManager.SetCategory(this.actViewAsSmallIcon, this.catView);
            this.actViewAsSmallIcon.Name = "actViewAsSmallIcon";
            manager.ApplyResources(this.actViewAsSmallIcon, "actViewAsSmallIcon");
            this.actViewAsSmallIcon.OnExecute += new EventHandler<ActionEventArgs>(this.actViewAs_OnExecute);
            this.actViewAsSmallIcon.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actViewAs_OnUpdate);
            this.categoryManager.SetCategory(this.actViewAsList, this.catView);
            this.actViewAsList.Name = "actViewAsList";
            this.actViewAsList.ShortcutKeys = Keys.Control | Keys.F1;
            manager.ApplyResources(this.actViewAsList, "actViewAsList");
            this.actViewAsList.OnExecute += new EventHandler<ActionEventArgs>(this.actViewAs_OnExecute);
            this.actViewAsList.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actViewAs_OnUpdate);
            this.categoryManager.SetCategory(this.actViewAsDetails, this.catView);
            this.actViewAsDetails.Name = "actViewAsDetails";
            this.actViewAsDetails.ShortcutKeys = Keys.Control | Keys.F2;
            manager.ApplyResources(this.actViewAsDetails, "actViewAsDetails");
            this.actViewAsDetails.OnExecute += new EventHandler<ActionEventArgs>(this.actViewAs_OnExecute);
            this.actViewAsDetails.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actViewAs_OnUpdate);
            this.categoryManager.SetCategory(this.actAdvancedFilter, this.catView);
            this.actAdvancedFilter.Name = "actAdvancedFilter";
            this.actAdvancedFilter.ShortcutKeys = Keys.Alt | Keys.Delete;
            manager.ApplyResources(this.actAdvancedFilter, "actAdvancedFilter");
            this.actAdvancedFilter.OnExecute += new EventHandler<ActionEventArgs>(this.actAdvancedFilter_OnExecute);
            this.categoryManager.SetCategory(this.actClearFilter, this.catView);
            this.actClearFilter.Name = "actClearFilter";
            manager.ApplyResources(this.actClearFilter, "actClearFilter");
            this.actClearFilter.OnExecute += new EventHandler<ActionEventArgs>(this.actClearFilter_OnExecute);
            this.actClearFilter.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actClearFilter_OnUpdate);
            this.categoryManager.SetCategory(this.actSelectSort, this.catView);
            this.actSelectSort.Name = "actSelectSort";
            this.actSelectSort.ShortcutKeys = Keys.Control | Keys.B;
            manager.ApplyResources(this.actSelectSort, "actSelectSort");
            this.actSelectSort.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectSort_OnExecute);
            this.categoryManager.SetCategory(this.actSortByName, this.catView);
            this.actSortByName.Name = "actSortByName";
            this.actSortByName.ShortcutKeys = Keys.Control | Keys.F3;
            manager.ApplyResources(this.actSortByName, "actSortByName");
            this.actSortByName.OnExecute += new EventHandler<ActionEventArgs>(this.actSortBy_OnExecute);
            this.actSortByName.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSortBy_OnUpdate);
            this.categoryManager.SetCategory(this.actSortByExtension, this.catView);
            this.actSortByExtension.Name = "actSortByExtension";
            this.actSortByExtension.ShortcutKeys = Keys.Control | Keys.F4;
            manager.ApplyResources(this.actSortByExtension, "actSortByExtension");
            this.actSortByExtension.OnExecute += new EventHandler<ActionEventArgs>(this.actSortBy_OnExecute);
            this.actSortByExtension.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSortBy_OnUpdate);
            this.categoryManager.SetCategory(this.actSortByLastWriteTime, this.catView);
            this.actSortByLastWriteTime.Name = "actSortByLastWriteTime";
            this.actSortByLastWriteTime.ShortcutKeys = Keys.Control | Keys.F5;
            manager.ApplyResources(this.actSortByLastWriteTime, "actSortByLastWriteTime");
            this.actSortByLastWriteTime.OnExecute += new EventHandler<ActionEventArgs>(this.actSortBy_OnExecute);
            this.actSortByLastWriteTime.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSortBy_OnUpdate);
            this.categoryManager.SetCategory(this.actSortBySize, this.catView);
            this.actSortBySize.Name = "actSortBySize";
            this.actSortBySize.ShortcutKeys = Keys.Control | Keys.F6;
            manager.ApplyResources(this.actSortBySize, "actSortBySize");
            this.actSortBySize.OnExecute += new EventHandler<ActionEventArgs>(this.actSortBy_OnExecute);
            this.actSortBySize.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSortBy_OnUpdate);
            this.categoryManager.SetCategory(this.actSortDescending, this.catView);
            this.actSortDescending.Name = "actSortDescending";
            manager.ApplyResources(this.actSortDescending, "actSortDescending");
            this.actSortDescending.OnExecute += new EventHandler<ActionEventArgs>(this.actSortDescending_OnExecute);
            this.actSortDescending.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSortDescending_OnUpdate);
            this.categoryManager.SetCategory(this.actCompareFolders, this.catPanel);
            this.actCompareFolders.Name = "actCompareFolders";
            this.actCompareFolders.ShortcutKeys = Keys.Control | Keys.O;
            manager.ApplyResources(this.actCompareFolders, "actCompareFolders");
            this.actCompareFolders.OnExecute += new EventHandler<ActionEventArgs>(this.actCompareFolders_OnExecute);
            this.actCompareFolders.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSwapPanels_OnUpdate);
            this.categoryManager.SetCategory(this.actFolderBranch, this.catPanel);
            this.actFolderBranch.Name = "actFolderBranch";
            this.actFolderBranch.ShortcutKeys = Keys.Control | Keys.H;
            manager.ApplyResources(this.actFolderBranch, "actFolderBranch");
            this.actFolderBranch.OnExecute += new EventHandler<ActionEventArgs>(this.actFolderBranch_OnExecute);
            this.actFolderBranch.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actFolderBranch_OnUpdate);
            this.categoryManager.SetCategory(this.actSwapPanels, this.catPanel);
            this.actSwapPanels.Name = "actSwapPanels";
            this.actSwapPanels.ShortcutKeys = Keys.Control | Keys.U;
            manager.ApplyResources(this.actSwapPanels, "actSwapPanels");
            this.actSwapPanels.OnExecute += new EventHandler<ActionEventArgs>(this.actSwapPanels_OnExecute);
            this.actSwapPanels.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSwapPanels_OnUpdate);
            this.categoryManager.SetCategory(this.actEqualizePanels, this.catPanel);
            this.actEqualizePanels.Name = "actEqualizePanels";
            this.actEqualizePanels.Tag = "0";
            manager.ApplyResources(this.actEqualizePanels, "actEqualizePanels");
            this.actEqualizePanels.OnExecute += new EventHandler<ActionEventArgs>(this.actEqualizePanels_OnExecute);
            this.actEqualizePanels.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSwapPanels_OnUpdate);
            this.categoryManager.SetCategory(this.actBack, this.catPanel);
            this.actBack.Name = "actBack";
            this.actBack.ShortcutKeys = Keys.Alt | Keys.Left;
            manager.ApplyResources(this.actBack, "actBack");
            this.actBack.OnExecute += new EventHandler<ActionEventArgs>(this.actBack_OnExecute);
            this.actBack.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actBack_OnUpdate);
            this.categoryManager.SetCategory(this.actForward, this.catPanel);
            this.actForward.Name = "actForward";
            this.actForward.ShortcutKeys = Keys.Alt | Keys.Right;
            manager.ApplyResources(this.actForward, "actForward");
            this.actForward.OnExecute += new EventHandler<ActionEventArgs>(this.actForward_OnExecute);
            this.actForward.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actForward_OnUpdate);
            this.categoryManager.SetCategory(this.actChangeDrive, this.catPanel);
            this.actChangeDrive.Name = "actChangeDrive";
            this.actChangeDrive.ShortcutKeys = Keys.Alt | Keys.C;
            manager.ApplyResources(this.actChangeDrive, "actChangeDrive");
            this.actChangeDrive.OnExecute += new EventHandler<ActionEventArgs>(this.actChangeDrive_OnExecute);
            this.actChangeDrive.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actChangeDrive_OnUpdate);
            this.categoryManager.SetCategory(this.actChangeFolder, this.catPanel);
            this.actChangeFolder.Name = "actChangeFolder";
            this.actChangeFolder.ShortcutKeys = Keys.Control | Keys.G;
            manager.ApplyResources(this.actChangeFolder, "actChangeFolder");
            this.actChangeFolder.OnExecute += new EventHandler<ActionEventArgs>(this.actChangeFolder_OnExecute);
            this.actChangeFolder.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actChangeFolder_OnUpdate);
            this.categoryManager.SetCategory(this.actFtpConnect, this.catPanel);
            this.actFtpConnect.Name = "actFtpConnect";
            manager.ApplyResources(this.actFtpConnect, "actFtpConnect");
            this.actFtpConnect.OnExecute += new EventHandler<ActionEventArgs>(this.actFtpConnect_OnExecute);
            this.actFtpConnect.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actChangeFolder_OnUpdate);
            this.categoryManager.SetCategory(this.actOpenContainingFolder, this.catPanel);
            this.actOpenContainingFolder.Name = "actOpenContainingFolder";
            manager.ApplyResources(this.actOpenContainingFolder, "actOpenContainingFolder");
            this.actOpenContainingFolder.OnExecute += new EventHandler<ActionEventArgs>(this.actOpenContainingFolder_OnExecute);
            this.actOpenContainingFolder.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actOpenContainingFolder_OnUpdate);
            this.categoryManager.SetCategory(this.actRefresh, this.catPanel);
            this.actRefresh.Name = "actRefresh";
            this.actRefresh.Shortcuts = new Keys[] { Keys.Control | Keys.R, Keys.Alt | Keys.R };
            manager.ApplyResources(this.actRefresh, "actRefresh");
            this.actRefresh.OnExecute += new EventHandler<ActionEventArgs>(this.actRefresh_OnExecute);
            this.actRefresh.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRefresh_OnUpdate);
            this.categoryManager.SetCategory(this.actBookmarkCurrentFolder, this.catBookmarks);
            this.actBookmarkCurrentFolder.Name = "actBookmarkCurrentFolder";
            this.actBookmarkCurrentFolder.Shortcuts = new Keys[] { Keys.Control | Keys.D, Keys.Control | Keys.Shift | Keys.D };
            manager.ApplyResources(this.actBookmarkCurrentFolder, "actBookmarkCurrentFolder");
            this.actBookmarkCurrentFolder.OnExecute += new EventHandler<ActionEventArgs>(this.actBookmarkCurrentFolder_OnExecute);
            this.actBookmarkCurrentFolder.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actBookmarkCurrentFolder_OnUpdate);
            this.categoryManager.SetCategory(this.actOrganizeBookmarks, this.catBookmarks);
            this.actOrganizeBookmarks.Name = "actOrganizeBookmarks";
            manager.ApplyResources(this.actOrganizeBookmarks, "actOrganizeBookmarks");
            this.actOrganizeBookmarks.OnExecute += new EventHandler<ActionEventArgs>(this.actOrganizeBookmarks_OnExecute);
            this.categoryManager.SetCategory(this.actOptions, this.catMisc);
            this.actOptions.Name = "actOptions";
            manager.ApplyResources(this.actOptions, "actOptions");
            this.actOptions.OnExecute += new EventHandler<ActionEventArgs>(this.actOptions_OnExecute);
            this.categoryManager.SetCategory(this.actAbout, this.catMisc);
            this.actAbout.Name = "actAbout";
            manager.ApplyResources(this.actAbout, "actAbout");
            this.actAbout.OnExecute += new EventHandler<ActionEventArgs>(this.actAbout_OnExecute);
            this.categoryManager.SetCategory(this.actMakeLink, this.catFile);
            this.actMakeLink.Name = "actMakeLink";
            manager.ApplyResources(this.actMakeLink, "actMakeLink");
            this.actMakeLink.OnExecute += new EventHandler<ActionEventArgs>(this.actMakeLink_OnExecute);
            this.actMakeLink.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMakeLink_OnUpdate);
            this.catFile.Name = "catFile";
            manager.ApplyResources(this.catFile, "catFile");
            this.catEdit.Name = "catEdit";
            manager.ApplyResources(this.catEdit, "catEdit");
            this.catView.Name = "catView";
            manager.ApplyResources(this.catView, "catView");
            this.catPanel.Name = "catPanel";
            manager.ApplyResources(this.catPanel, "catPanel");
            this.catBookmarks.Name = "catBookmarks";
            manager.ApplyResources(this.catBookmarks, "catBookmarks");
            this.catMisc.Name = "catMisc";
            manager.ApplyResources(this.catMisc, "catMisc");
            this.categoryManager.SetCategory(this.actManageColumns, this.catView);
            this.actManageColumns.Name = "actManageColumns";
            manager.ApplyResources(this.actManageColumns, "actManageColumns");
            this.actManageColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actManageColumns_OnExecute);
            this.categoryManager.SetCategory(this.actSelectByExtension, this.catEdit);
            this.actSelectByExtension.Name = "actSelectByExtension";
            this.actSelectByExtension.Tag = "select";
            manager.ApplyResources(this.actSelectByExtension, "actSelectByExtension");
            this.actSelectByExtension.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectByExtension_OnExecute);
            this.actSelectByExtension.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSelectByExtension_OnUpdate);
            this.categoryManager.SetCategory(this.actUnselectByExtension, this.catEdit);
            this.actUnselectByExtension.Name = "actUnselectByExtension";
            this.actUnselectByExtension.Tag = "unselect";
            manager.ApplyResources(this.actUnselectByExtension, "actUnselectByExtension");
            this.actUnselectByExtension.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectByExtension_OnExecute);
            this.actUnselectByExtension.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSelectByExtension_OnUpdate);
            this.categoryManager.SetCategory(this.actGoToParent, this.catPanel);
            this.actGoToParent.Name = "actGoToParent";
            manager.ApplyResources(this.actGoToParent, "actGoToParent");
            this.actGoToParent.OnExecute += new EventHandler<ActionEventArgs>(this.actGoToParent_OnExecute);
            this.actGoToParent.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actGoToParent_OnUpdate);
            this.categoryManager.SetCategory(this.actGoToRoot, this.catPanel);
            this.actGoToRoot.Name = "actGoToRoot";
            manager.ApplyResources(this.actGoToRoot, "actGoToRoot");
            this.actGoToRoot.OnExecute += new EventHandler<ActionEventArgs>(this.actGoToRoot_OnExecute);
            this.actGoToRoot.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actGoToRoot_OnUpdate);
            this.categoryManager.SetCategory(this.actEditDescription, this.catFile);
            this.actEditDescription.Name = "actEditDescription";
            this.actEditDescription.ShortcutKeys = Keys.Control | Keys.Z;
            manager.ApplyResources(this.actEditDescription, "actEditDescription");
            this.actEditDescription.OnExecute += new EventHandler<ActionEventArgs>(this.actEditDescription_OnExecute);
            this.actEditDescription.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actEditDescription_OnUpdate);
            this.categoryManager.SetCategory(this.actViewAsLargeIcon, this.catView);
            this.actViewAsLargeIcon.Name = "actViewAsLargeIcon";
            manager.ApplyResources(this.actViewAsLargeIcon, "actViewAsLargeIcon");
            this.actViewAsLargeIcon.OnExecute += new EventHandler<ActionEventArgs>(this.actViewAs_OnExecute);
            this.actViewAsLargeIcon.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actViewAs_OnUpdate);
            this.categoryManager.SetCategory(this.actCustomizeFolder, this.catView);
            this.actCustomizeFolder.Name = "actCustomizeFolder";
            manager.ApplyResources(this.actCustomizeFolder, "actCustomizeFolder");
            this.actCustomizeFolder.OnExecute += new EventHandler<ActionEventArgs>(this.actCustomizeFolder_OnExecute);
            this.actCustomizeFolder.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCustomizeFolder_OnUpdate);
            this.categoryManager.SetCategory(this.actInvertEntireSelection, this.catEdit);
            this.actInvertEntireSelection.Name = "actInvertEntireSelection";
            manager.ApplyResources(this.actInvertEntireSelection, "actInvertEntireSelection");
            this.actInvertEntireSelection.OnExecute += new EventHandler<ActionEventArgs>(this.actInvertEntireSelection_OnExecute);
            this.categoryManager.SetCategory(this.actViewAsThumbnail, this.catView);
            this.actViewAsThumbnail.Name = "actViewAsThumbnail";
            manager.ApplyResources(this.actViewAsThumbnail, "actViewAsThumbnail");
            this.actViewAsThumbnail.OnExecute += new EventHandler<ActionEventArgs>(this.actViewAs_OnExecute);
            this.actViewAsThumbnail.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actViewAs_OnUpdate);
            this.categoryManager.SetCategory(this.actCheckForUpdates, this.catMisc);
            this.actCheckForUpdates.Name = "actCheckForUpdates";
            manager.ApplyResources(this.actCheckForUpdates, "actCheckForUpdates");
            this.actCheckForUpdates.OnExecute += new EventHandler<ActionEventArgs>(this.actCheckForUpdates_OnExecute);
            this.categoryManager.SetCategory(this.actUnselectAll, this.catEdit);
            this.actUnselectAll.Name = "actUnselectAll";
            this.actUnselectAll.ShortcutKeys = Keys.Control | Keys.Q;
            manager.ApplyResources(this.actUnselectAll, "actUnselectAll");
            this.actUnselectAll.OnExecute += new EventHandler<ActionEventArgs>(this.actUnselectAll_OnExecute);
            this.actUnselectAll.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actUnselect_OnUpdate);
            this.categoryManager.SetCategory(this.actChangeView, this.catView);
            this.actChangeView.Name = "actChangeView";
            manager.ApplyResources(this.actChangeView, "actChangeView");
            this.actChangeView.OnExecute += new EventHandler<ActionEventArgs>(this.actChangeView_OnExecute);
            this.actChangeView.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actChangeView_OnUpdate);
            this.categoryManager.SetCategory(this.actToggleQuickFind, this.catView);
            this.actToggleQuickFind.Name = "actToggleQuickFind";
            this.actToggleQuickFind.ShortcutKeys = Keys.Control | Keys.F;
            manager.ApplyResources(this.actToggleQuickFind, "actToggleQuickFind");
            this.actToggleQuickFind.OnExecute += new EventHandler<ActionEventArgs>(this.actToggleQuickFind_OnExecute);
            this.categoryManager.SetCategory(this.actChangeDriveLeft, this.catPanel);
            this.actChangeDriveLeft.Name = "actChangeDriveLeft";
            this.actChangeDriveLeft.ShortcutKeys = Keys.Alt | Keys.F1;
            manager.ApplyResources(this.actChangeDriveLeft, "actChangeDriveLeft");
            this.actChangeDriveLeft.OnExecute += new EventHandler<ActionEventArgs>(this.actChangeDrive_OnExecute);
            this.actChangeDriveLeft.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actChangeDrive_OnUpdate);
            this.categoryManager.SetCategory(this.actChangeDriveRight, this.catPanel);
            this.actChangeDriveRight.Name = "actChangeDriveRight";
            this.actChangeDriveRight.ShortcutKeys = Keys.Alt | Keys.F2;
            manager.ApplyResources(this.actChangeDriveRight, "actChangeDriveRight");
            this.actChangeDriveRight.OnExecute += new EventHandler<ActionEventArgs>(this.actChangeDrive_OnExecute);
            this.actChangeDriveRight.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actChangeDrive_OnUpdate);
            this.categoryManager.SetCategory(this.actShowBookmarks, this.catBookmarks);
            this.actShowBookmarks.Name = "actShowBookmarks";
            this.actShowBookmarks.ShortcutKeys = Keys.Alt | Keys.B;
            manager.ApplyResources(this.actShowBookmarks, "actShowBookmarks");
            this.actShowBookmarks.OnExecute += new EventHandler<ActionEventArgs>(this.actShowBookmarks_OnExecute);
            this.categoryManager.SetCategory(this.actResetVisualCache, this.catMisc);
            this.actResetVisualCache.Name = "actResetVisualCache";
            this.actResetVisualCache.ShortcutKeys = Keys.Alt | Keys.Control | Keys.Shift | Keys.R;
            manager.ApplyResources(this.actResetVisualCache, "actResetVisualCache");
            this.actResetVisualCache.OnExecute += new EventHandler<ActionEventArgs>(this.actResetVisualCache_OnExecute);
            this.categoryManager.SetCategory(this.actPack, this.catFile);
            this.actPack.Name = "actPack";
            this.actPack.ShortcutKeys = Keys.Alt | Keys.F5;
            manager.ApplyResources(this.actPack, "actPack");
            this.actPack.OnExecute += new EventHandler<ActionEventArgs>(this.actPack_OnExecute);
            this.actPack.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopy_OnUpdate);
            this.categoryManager.SetCategory(this.actCopyDetailsAsCSV, this.catEdit);
            this.actCopyDetailsAsCSV.Name = "actCopyDetailsAsCSV";
            manager.ApplyResources(this.actCopyDetailsAsCSV, "actCopyDetailsAsCSV");
            this.actCopyDetailsAsCSV.OnExecute += new EventHandler<ActionEventArgs>(this.actCopyDetailsAsCSV_OnExecute);
            this.actCopyDetailsAsCSV.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCopyDetailsAsCSV_OnUpdate);
            this.categoryManager.SetCategory(this.actSaveCurrentLayout, this.catView);
            this.actSaveCurrentLayout.Name = "actSaveCurrentLayout";
            manager.ApplyResources(this.actSaveCurrentLayout, "actSaveCurrentLayout");
            this.actSaveCurrentLayout.OnExecute += new EventHandler<ActionEventArgs>(this.actSaveCurrentLayout_OnExecute);
            this.categoryManager.SetCategory(this.actManageLayouts, this.catView);
            this.actManageLayouts.Name = "actManageLayouts";
            manager.ApplyResources(this.actManageLayouts, "actManageLayouts");
            this.actManageLayouts.OnExecute += new EventHandler<ActionEventArgs>(this.actManageLayouts_OnExecute);
            this.actManageLayouts.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actManageLayouts_OnUpdate);
            this.categoryManager.SetCategory(this.actNewFile, this.catFile);
            this.actNewFile.Name = "actNewFile";
            this.actNewFile.ShortcutKeys = Keys.Control | Keys.N;
            manager.ApplyResources(this.actNewFile, "actNewFile");
            this.actNewFile.OnExecute += new EventHandler<ActionEventArgs>(this.actNewFile_OnExecute);
            this.actNewFile.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actNewFile_OnUpdate);
            this.categoryManager.SetCategory(this.actCalculateOnDemandProperties, this.catPanel);
            this.actCalculateOnDemandProperties.Name = "actCalculateOnDemandProperties";
            manager.ApplyResources(this.actCalculateOnDemandProperties, "actCalculateOnDemandProperties");
            this.actCalculateOnDemandProperties.OnExecute += new EventHandler<ActionEventArgs>(this.actCalculateOnDemandProperties_OnExecute);
            this.actCalculateOnDemandProperties.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRefresh_OnUpdate);
            this.categoryManager.SetCategory(this.actDuplicateTab, this.catTab);
            this.actDuplicateTab.Name = "actDuplicateTab";
            this.actDuplicateTab.ShortcutKeys = Keys.Control | Keys.T;
            manager.ApplyResources(this.actDuplicateTab, "actDuplicateTab");
            this.actDuplicateTab.OnExecute += new EventHandler<ActionEventArgs>(this.actDuplicateTab_OnExecute);
            this.categoryManager.SetCategory(this.actCloseTab, this.catTab);
            this.actCloseTab.Name = "actCloseTab";
            this.actCloseTab.ShortcutKeys = Keys.Control | Keys.W;
            manager.ApplyResources(this.actCloseTab, "actCloseTab");
            this.actCloseTab.OnExecute += new EventHandler<ActionEventArgs>(this.actCloseTab_OnExecute);
            this.actCloseTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCloseTab_OnUpdate);
            this.catTab.Name = "catTab";
            manager.ApplyResources(this.catTab, "catTab");
            this.categoryManager.SetCategory(this.actCloseOtherTabs, this.catTab);
            this.actCloseOtherTabs.Name = "actCloseOtherTabs";
            this.actCloseOtherTabs.ShortcutKeys = Keys.Alt | Keys.Control | Keys.F4;
            manager.ApplyResources(this.actCloseOtherTabs, "actCloseOtherTabs");
            this.actCloseOtherTabs.OnExecute += new EventHandler<ActionEventArgs>(this.actCloseOtherTabs_OnExecute);
            this.actCloseOtherTabs.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCloseTab_OnUpdate);
            this.categoryManager.SetCategory(this.actRenameTab, this.catTab);
            this.actRenameTab.Name = "actRenameTab";
            manager.ApplyResources(this.actRenameTab, "actRenameTab");
            this.actRenameTab.OnExecute += new EventHandler<ActionEventArgs>(this.actRenameTab_OnExecute);
            this.actRenameTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRenameTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToNextTab, this.catTab);
            this.actMoveToNextTab.Name = "actMoveToNextTab";
            this.actMoveToNextTab.ShortcutKeys = Keys.Control | Keys.Tab;
            manager.ApplyResources(this.actMoveToNextTab, "actMoveToNextTab");
            this.actMoveToNextTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNextTab_OnExecute);
            this.actMoveToNextTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNextTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToPreviousTab, this.catTab);
            this.actMoveToPreviousTab.Name = "actMoveToPreviousTab";
            this.actMoveToPreviousTab.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Tab;
            manager.ApplyResources(this.actMoveToPreviousTab, "actMoveToPreviousTab");
            this.actMoveToPreviousTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNextTab_OnExecute);
            this.actMoveToPreviousTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNextTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToFirstTab, this.catTab);
            this.actMoveToFirstTab.Name = "actMoveToFirstTab";
            this.actMoveToFirstTab.ShortcutKeys = Keys.Control | Keys.D1;
            manager.ApplyResources(this.actMoveToFirstTab, "actMoveToFirstTab");
            this.actMoveToFirstTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToFirstTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToSecondTab, this.catTab);
            this.actMoveToSecondTab.Name = "actMoveToSecondTab";
            this.actMoveToSecondTab.ShortcutKeys = Keys.Control | Keys.D2;
            manager.ApplyResources(this.actMoveToSecondTab, "actMoveToSecondTab");
            this.actMoveToSecondTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToSecondTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToThirdTab, this.catTab);
            this.actMoveToThirdTab.Name = "actMoveToThirdTab";
            this.actMoveToThirdTab.ShortcutKeys = Keys.Control | Keys.D3;
            manager.ApplyResources(this.actMoveToThirdTab, "actMoveToThirdTab");
            this.actMoveToThirdTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToThirdTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToFourthTab, this.catTab);
            this.actMoveToFourthTab.Name = "actMoveToFourthTab";
            this.actMoveToFourthTab.ShortcutKeys = Keys.Control | Keys.D4;
            manager.ApplyResources(this.actMoveToFourthTab, "actMoveToFourthTab");
            this.actMoveToFourthTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToFourthTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToFifthTab, this.catTab);
            this.actMoveToFifthTab.Name = "actMoveToFifthTab";
            this.actMoveToFifthTab.ShortcutKeys = Keys.Control | Keys.D5;
            manager.ApplyResources(this.actMoveToFifthTab, "actMoveToFifthTab");
            this.actMoveToFifthTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToFifthTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToSixthTab, this.catTab);
            this.actMoveToSixthTab.Name = "actMoveToSixthTab";
            this.actMoveToSixthTab.ShortcutKeys = Keys.Control | Keys.D6;
            manager.ApplyResources(this.actMoveToSixthTab, "actMoveToSixthTab");
            this.actMoveToSixthTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToSixthTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToSeventhTab, this.catTab);
            this.actMoveToSeventhTab.Name = "actMoveToSeventhTab";
            this.actMoveToSeventhTab.ShortcutKeys = Keys.Control | Keys.D7;
            manager.ApplyResources(this.actMoveToSeventhTab, "actMoveToSeventhTab");
            this.actMoveToSeventhTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToSeventhTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToEighthTab, this.catTab);
            this.actMoveToEighthTab.Name = "actMoveToEighthTab";
            this.actMoveToEighthTab.ShortcutKeys = Keys.Control | Keys.D8;
            manager.ApplyResources(this.actMoveToEighthTab, "actMoveToEighthTab");
            this.actMoveToEighthTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToNthTab_OnExecute);
            this.actMoveToEighthTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToNthTab_OnUpdate);
            this.categoryManager.SetCategory(this.actMoveToLastTab, this.catTab);
            this.actMoveToLastTab.Name = "actMoveToLastTab";
            this.actMoveToLastTab.ShortcutKeys = Keys.Control | Keys.D9;
            manager.ApplyResources(this.actMoveToLastTab, "actMoveToLastTab");
            this.actMoveToLastTab.OnExecute += new EventHandler<ActionEventArgs>(this.actMoveToLastTab_OnExecute);
            this.actMoveToLastTab.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actMoveToLastTab_OnUpdate);
            this.categoryManager.SetCategory(this.actNavigationLink, this.catPanel);
            this.actNavigationLink.Name = "actNavigationLink";
            manager.ApplyResources(this.actNavigationLink, "actNavigationLink");
            this.actNavigationLink.OnExecute += new EventHandler<ActionEventArgs>(this.actNavigationLink_OnExecute);
            this.actNavigationLink.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actNavigationLink_OnUpdate);
            this.categoryManager.SetCategory(this.actLockFolderChange, this.catPanel);
            this.actLockFolderChange.Name = "actLockFolderChange";
            manager.ApplyResources(this.actLockFolderChange, "actLockFolderChange");
            this.actLockFolderChange.OnExecute += new EventHandler<ActionEventArgs>(this.actLockFolderChange_OnExecute);
            this.actLockFolderChange.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actLockFolderChange_OnUpdate);
            this.categoryManager.SetCategory(this.actCloseTabsToRight, this.catTab);
            this.actCloseTabsToRight.Name = "actCloseTabsToRight";
            manager.ApplyResources(this.actCloseTabsToRight, "actCloseTabsToRight");
            this.actCloseTabsToRight.OnExecute += new EventHandler<ActionEventArgs>(this.actCloseTabsToRight_OnExecute);
            this.actCloseTabsToRight.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actCloseTabsToRight_OnUpdate);
            this.categoryManager.SetCategory(this.actSelectByName, this.catEdit);
            this.actSelectByName.Name = "actSelectByName";
            manager.ApplyResources(this.actSelectByName, "actSelectByName");
            this.actSelectByName.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectByName_OnExecute);
            this.actSelectByName.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSelectByExtension_OnUpdate);
            this.categoryManager.SetCategory(this.actToggleOnePanelMode, this.catView);
            this.actToggleOnePanelMode.Name = "actToggleOnePanelMode";
            manager.ApplyResources(this.actToggleOnePanelMode, "actToggleOnePanelMode");
            this.actToggleOnePanelMode.OnExecute += new EventHandler<ActionEventArgs>(this.actToggleOnePanelMode_OnExecute);
            this.actToggleOnePanelMode.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actToggleOnePanelMode_OnUpdate);
            this.categoryManager.SetCategory(this.actToggleFolderBar, this.catView);
            this.actToggleFolderBar.Name = "actToggleFolderBar";
            this.actToggleFolderBar.ShortcutKeys = Keys.Control | Keys.Shift | Keys.T;
            manager.ApplyResources(this.actToggleFolderBar, "actToggleFolderBar");
            this.actToggleFolderBar.OnExecute += new EventHandler<ActionEventArgs>(this.actToggleFolderBar_OnExecute);
            this.actToggleFolderBar.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actToggleFolderBar_OnUpdate);
            this.categoryManager.SetCategory(this.actCustomizeToolbars, this.catMisc);
            this.actCustomizeToolbars.Name = "actCustomizeToolbars";
            this.actCustomizeToolbars.Tag = "Nomad.Controls.Option.ToolbarOptionControl";
            manager.ApplyResources(this.actCustomizeToolbars, "actCustomizeToolbars");
            this.actCustomizeToolbars.OnExecute += new EventHandler<ActionEventArgs>(this.actOptions_OnExecute);
            this.categoryManager.SetCategory(this.actExit, this.catFile);
            this.actExit.Name = "actExit";
            this.actExit.Shortcuts = new Keys[] { Keys.Alt | Keys.X, Keys.Alt | Keys.F4 };
            manager.ApplyResources(this.actExit, "actExit");
            this.actExit.OnExecute += new EventHandler<ActionEventArgs>(this.actExit_Click);
            this.categoryManager.SetCategory(this.actCustomizeTools, this.catMisc);
            this.actCustomizeTools.Name = "actCustomizeTools";
            this.actCustomizeTools.Tag = "Nomad.Controls.Option.ExternalToolsOptionControl";
            manager.ApplyResources(this.actCustomizeTools, "actCustomizeTools");
            this.actCustomizeTools.OnExecute += new EventHandler<ActionEventArgs>(this.actOptions_OnExecute);
            this.categoryManager.SetCategory(this.actSaveSettings, this.catMisc);
            this.actSaveSettings.Name = "actSaveSettings";
            this.actSaveSettings.ShortcutKeys = Keys.Shift | Keys.F9;
            manager.ApplyResources(this.actSaveSettings, "actSaveSettings");
            this.actSaveSettings.OnExecute += new EventHandler<ActionEventArgs>(this.actSaveSettings_OnExecute);
            this.categoryManager.SetCategory(this.actBringToFront, this.catMisc);
            this.actBringToFront.Name = "actBringToFront";
            manager.ApplyResources(this.actBringToFront, "actBringToFront");
            this.actBringToFront.OnExecute += new EventHandler<ActionEventArgs>(this.actBringToFront_OnExecute);
            this.categoryManager.SetCategory(this.actVolumeLabel, this.catFile);
            this.actVolumeLabel.Name = "actVolumeLabel";
            manager.ApplyResources(this.actVolumeLabel, "actVolumeLabel");
            this.actVolumeLabel.OnExecute += new EventHandler<ActionEventArgs>(this.actVolumeLabel_OnExecute);
            this.actVolumeLabel.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actVolumeLabel_OnUpdate);
            this.categoryManager.SetCategory(this.actOpenOutside, this.catFile);
            this.actOpenOutside.Name = "actOpenOutside";
            manager.ApplyResources(this.actOpenOutside, "actOpenOutside");
            this.actOpenOutside.OnExecute += new EventHandler<ActionEventArgs>(this.actOpenOutside_OnExecute);
            this.actOpenOutside.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actOpenOutside_OnUpdate);
            this.categoryManager.SetCategory(this.actRefreshToolbars, this.catMisc);
            this.actRefreshToolbars.Name = "actRefreshToolbars";
            manager.ApplyResources(this.actRefreshToolbars, "actRefreshToolbars");
            this.actRefreshToolbars.OnExecute += new EventHandler<ActionEventArgs>(this.actRefreshToolbars_OnExecute);
            this.actionManager.Actions.AddRange(new Action[] { 
                this.actNewFile, this.actViewItem, this.actEditItem, this.actFind, this.actCopy, this.actRenameMove, this.actPack, this.actRenameSingleItem, this.actMakeLink, this.actMakeFolder, this.actDelete, this.actDeleteSingleItem, this.actRunAs, this.actRunAsAdmin, this.actOpen, this.actOpenOutside, 
                this.actSetAttributes, this.actEditDescription, this.actShowProperties, this.actVolumeLabel, this.actMapNetworkDrive, this.actDisconnectNetworkDrive, this.actExit, this.actCutToClipboard, this.actCopyToClipboard, this.actPasteFromClipboard, this.actPasteShortCut, this.actCopyNameAsText, this.actCopyFullNameAsText, this.actCopyDetailsAsCSV, this.actCopyCurrentFolderAsText, this.actEmptyClipboard, 
                this.actSelectSingleItem, this.actSelectSingleItemAndCalculate, this.actSelect, this.actUnselect, this.actSelectByExtension, this.actUnselectByExtension, this.actSelectByName, this.actInvertSelection, this.actInvertEntireSelection, this.actRestoreSelection, this.actSelectAll, this.actUnselectAll, this.actOnePanel, this.actTwoHorizontalPanel, this.actTwoVerticalPanel, this.actToggleOnePanelMode, 
                this.actToggleFolderBar, this.actSaveCurrentLayout, this.actManageLayouts, this.actViewAsThumbnail, this.actViewAsLargeIcon, this.actViewAsSmallIcon, this.actViewAsList, this.actViewAsDetails, this.actChangeView, this.actSetOneListColumn, this.actSetTwoListColumns, this.actSetThreeListColumns, this.actSetFourListColumns, this.actSetFiveListColumns, this.actSetSixListColumns, this.actSetSevenListColumns, 
                this.actSetEightListColumns, this.actSetNineListColumns, this.actManageColumns, this.actAdvancedFilter, this.actClearFilter, this.actToggleQuickFind, this.actSelectSort, this.actSortByName, this.actSortByExtension, this.actSortByLastWriteTime, this.actSortBySize, this.actSortDescending, this.actCustomizeFolder, this.actOpenAsArchive, this.actCompareFolders, this.actFolderBranch, 
                this.actCalculateOnDemandProperties, this.actSwapPanels, this.actEqualizePanels, this.actLeftPanelToRight, this.actRightPanelToLeft, this.actOpenInFarPanel, this.actNavigationLink, this.actLockFolderChange, this.actBack, this.actForward, this.actChangeDrive, this.actChangeDriveLeft, this.actChangeDriveRight, this.actChangeFolder, this.actQuickChangeFolder, this.actOpenRecentFolders, 
                this.actAddFolderToRecent, this.actGoToParent, this.actGoToRoot, this.actFtpConnect, this.actOpenContainingFolder, this.actRefresh, this.actDuplicateTab, this.actCloseTab, this.actCloseOtherTabs, this.actCloseTabsToRight, this.actRenameTab, this.actMoveToNextTab, this.actMoveToPreviousTab, this.actMoveToFirstTab, this.actMoveToSecondTab, this.actMoveToThirdTab, 
                this.actMoveToFourthTab, this.actMoveToFifthTab, this.actMoveToSixthTab, this.actMoveToSeventhTab, this.actMoveToEighthTab, this.actMoveToLastTab, this.actBookmarkCurrentFolder, this.actBookmarkCurrentTab, this.actOrganizeBookmarks, this.actShowBookmarks, this.actOptions, this.actCustomizeToolbars, this.actCustomizeTools, this.actSaveSettings, this.actResetVisualCache, this.actRefreshToolbars, 
                this.actGCCollect, this.actBringToFront, this.actMinimizeToTray, this.actHelpContents, this.actCheckForUpdates, this.actShowCmdLineHelp, this.actAbout
             });
            this.actionManager.PreviewExecuteAction += new EventHandler<ActionEventArgs>(this.actionManager_PreviewExecuteAction);
            this.categoryManager.SetCategory(this.actSetOneListColumn, this.catView);
            this.actSetOneListColumn.Name = "actSetOneListColumn";
            manager.ApplyResources(this.actSetOneListColumn, "actSetOneListColumn");
            this.actSetOneListColumn.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetOneListColumn.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetTwoListColumns, this.catView);
            this.actSetTwoListColumns.Name = "actSetTwoListColumns";
            manager.ApplyResources(this.actSetTwoListColumns, "actSetTwoListColumns");
            this.actSetTwoListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetTwoListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetThreeListColumns, this.catView);
            this.actSetThreeListColumns.Name = "actSetThreeListColumns";
            manager.ApplyResources(this.actSetThreeListColumns, "actSetThreeListColumns");
            this.actSetThreeListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetThreeListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetFourListColumns, this.catView);
            this.actSetFourListColumns.Name = "actSetFourListColumns";
            manager.ApplyResources(this.actSetFourListColumns, "actSetFourListColumns");
            this.actSetFourListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetFourListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetFiveListColumns, this.catView);
            this.actSetFiveListColumns.Name = "actSetFiveListColumns";
            manager.ApplyResources(this.actSetFiveListColumns, "actSetFiveListColumns");
            this.actSetFiveListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetFiveListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetSixListColumns, this.catView);
            this.actSetSixListColumns.Name = "actSetSixListColumns";
            manager.ApplyResources(this.actSetSixListColumns, "actSetSixListColumns");
            this.actSetSixListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetSixListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetSevenListColumns, this.catView);
            this.actSetSevenListColumns.Name = "actSetSevenListColumns";
            manager.ApplyResources(this.actSetSevenListColumns, "actSetSevenListColumns");
            this.actSetSevenListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetSevenListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetEightListColumns, this.catView);
            this.actSetEightListColumns.Name = "actSetEightListColumns";
            manager.ApplyResources(this.actSetEightListColumns, "actSetEightListColumns");
            this.actSetEightListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetEightListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actSetNineListColumns, this.catView);
            this.actSetNineListColumns.Name = "actSetNineListColumns";
            manager.ApplyResources(this.actSetNineListColumns, "actSetNineListColumns");
            this.actSetNineListColumns.OnExecute += new EventHandler<ActionEventArgs>(this.actSetNListColumn_OnExecute);
            this.actSetNineListColumns.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSetNListColumn_OnUpdate);
            this.categoryManager.SetCategory(this.actOpenInFarPanel, this.catPanel);
            this.actOpenInFarPanel.Name = "actOpenInFarPanel";
            manager.ApplyResources(this.actOpenInFarPanel, "actOpenInFarPanel");
            this.actOpenInFarPanel.OnExecute += new EventHandler<ActionEventArgs>(this.actOpenInFarPanel_OnExecute);
            this.actOpenInFarPanel.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actOpenInFarPanel_OnUpdate);
            this.categoryManager.SetCategory(this.actBookmarkCurrentTab, this.catBookmarks);
            this.actBookmarkCurrentTab.Name = "actBookmarkCurrentTab";
            this.actBookmarkCurrentTab.ShortcutKeys = Keys.Alt | Keys.Control | Keys.D;
            manager.ApplyResources(this.actBookmarkCurrentTab, "actBookmarkCurrentTab");
            this.actBookmarkCurrentTab.OnExecute += new EventHandler<ActionEventArgs>(this.actBookmarkCurrentTab_OnExecute);
            this.categoryManager.SetCategory(this.actHelpContents, this.catMisc);
            this.actHelpContents.Name = "actHelpContents";
            manager.ApplyResources(this.actHelpContents, "actHelpContents");
            this.actHelpContents.OnExecute += new EventHandler<ActionEventArgs>(this.actHelpContents_OnExecute);
            this.categoryManager.SetCategory(this.actShowCmdLineHelp, this.catMisc);
            this.actShowCmdLineHelp.Name = "actShowCmdLineHelp";
            this.actShowCmdLineHelp.Tag = "http://www.nomad-net.info";
            manager.ApplyResources(this.actShowCmdLineHelp, "actShowCmdLineHelp");
            this.actShowCmdLineHelp.OnExecute += new EventHandler<ActionEventArgs>(this.actShowCmdLineHelp_OnExecute);
            this.categoryManager.SetCategory(this.actRunAsAdmin, this.catFile);
            this.actRunAsAdmin.Name = "actRunAsAdmin";
            manager.ApplyResources(this.actRunAsAdmin, "actRunAsAdmin");
            this.actRunAsAdmin.OnExecute += new EventHandler<ActionEventArgs>(this.actRunAsAdmin_OnExecute);
            this.actRunAsAdmin.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRunAsAdmin_OnUpdate);
            this.categoryManager.SetCategory(this.actOpen, this.catFile);
            this.actOpen.Name = "actOpen";
            manager.ApplyResources(this.actOpen, "actOpen");
            this.actOpen.OnExecute += new EventHandler<ActionEventArgs>(this.actOpen_OnExecute);
            this.actOpen.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actOpen_OnUpdate);
            this.categoryManager.SetCategory(this.actMapNetworkDrive, this.catFile);
            this.actMapNetworkDrive.Name = "actMapNetworkDrive";
            manager.ApplyResources(this.actMapNetworkDrive, "actMapNetworkDrive");
            this.actMapNetworkDrive.OnExecute += new EventHandler<ActionEventArgs>(this.actMapNetworkDrive_OnExecute);
            this.categoryManager.SetCategory(this.actDisconnectNetworkDrive, this.catFile);
            this.actDisconnectNetworkDrive.Name = "actDisconnectNetworkDrive";
            manager.ApplyResources(this.actDisconnectNetworkDrive, "actDisconnectNetworkDrive");
            this.actDisconnectNetworkDrive.OnExecute += new EventHandler<ActionEventArgs>(this.actDisconnectNetworkDrive_OnExecute);
            this.categoryManager.SetCategory(this.actCopyCurrentFolderAsText, this.catEdit);
            this.actCopyCurrentFolderAsText.Name = "actCopyCurrentFolderAsText";
            manager.ApplyResources(this.actCopyCurrentFolderAsText, "actCopyCurrentFolderAsText");
            this.actCopyCurrentFolderAsText.OnExecute += new EventHandler<ActionEventArgs>(this.actCopyCurrentFolderAsText_OnExecute);
            this.actCopyCurrentFolderAsText.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actRefresh_OnUpdate);
            this.categoryManager.SetCategory(this.actSelectSingleItem, this.catEdit);
            this.actSelectSingleItem.Name = "actSelectSingleItem";
            this.actSelectSingleItem.ShortcutKeys = Keys.Insert;
            manager.ApplyResources(this.actSelectSingleItem, "actSelectSingleItem");
            this.actSelectSingleItem.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectSingleItem_OnExecute);
            this.actSelectSingleItem.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSelectSingleItem_OnUpdate);
            this.categoryManager.SetCategory(this.actSelectSingleItemAndCalculate, this.catEdit);
            this.actSelectSingleItemAndCalculate.Name = "actSelectSingleItemAndCalculate";
            this.actSelectSingleItemAndCalculate.ShortcutKeys = Keys.Space;
            manager.ApplyResources(this.actSelectSingleItemAndCalculate, "actSelectSingleItemAndCalculate");
            this.actSelectSingleItemAndCalculate.OnExecute += new EventHandler<ActionEventArgs>(this.actSelectSingleItem_OnExecute);
            this.actSelectSingleItemAndCalculate.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSelectSingleItem_OnUpdate);
            this.categoryManager.SetCategory(this.actOpenAsArchive, this.catPanel);
            this.actOpenAsArchive.Name = "actOpenAsArchive";
            manager.ApplyResources(this.actOpenAsArchive, "actOpenAsArchive");
            this.actOpenAsArchive.OnExecute += new EventHandler<ActionEventArgs>(this.actOpenAsArchive_OnExecute);
            this.actOpenAsArchive.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actOpenAsArchive_OnUpdate);
            this.categoryManager.SetCategory(this.actLeftPanelToRight, this.catPanel);
            this.actLeftPanelToRight.Name = "actLeftPanelToRight";
            this.actLeftPanelToRight.Tag = "1";
            manager.ApplyResources(this.actLeftPanelToRight, "actLeftPanelToRight");
            this.actLeftPanelToRight.OnExecute += new EventHandler<ActionEventArgs>(this.actEqualizePanels_OnExecute);
            this.actLeftPanelToRight.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSwapPanels_OnUpdate);
            this.categoryManager.SetCategory(this.actRightPanelToLeft, this.catPanel);
            this.actRightPanelToLeft.Name = "actRightPanelToLeft";
            this.actRightPanelToLeft.Tag = "2";
            manager.ApplyResources(this.actRightPanelToLeft, "actRightPanelToLeft");
            this.actRightPanelToLeft.OnExecute += new EventHandler<ActionEventArgs>(this.actEqualizePanels_OnExecute);
            this.actRightPanelToLeft.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actSwapPanels_OnUpdate);
            this.categoryManager.SetCategory(this.actQuickChangeFolder, this.catPanel);
            this.actQuickChangeFolder.Name = "actQuickChangeFolder";
            this.actQuickChangeFolder.ShortcutKeys = Keys.Control | Keys.Space;
            manager.ApplyResources(this.actQuickChangeFolder, "actQuickChangeFolder");
            this.actQuickChangeFolder.OnExecute += new EventHandler<ActionEventArgs>(this.actQuickChangeFolder_OnExecute);
            this.categoryManager.SetCategory(this.actOpenRecentFolders, this.catPanel);
            this.actOpenRecentFolders.Name = "actOpenRecentFolders";
            this.actOpenRecentFolders.ShortcutKeys = Keys.Alt | Keys.Down;
            manager.ApplyResources(this.actOpenRecentFolders, "actOpenRecentFolders");
            this.actOpenRecentFolders.OnExecute += new EventHandler<ActionEventArgs>(this.actOpenRecentFolders_OnExecute);
            this.categoryManager.SetCategory(this.actAddFolderToRecent, this.catPanel);
            this.actAddFolderToRecent.Name = "actAddFolderToRecent";
            manager.ApplyResources(this.actAddFolderToRecent, "actAddFolderToRecent");
            this.actAddFolderToRecent.OnExecute += new EventHandler<ActionEventArgs>(this.actAddFolderToRecent_OnExecute);
            this.actAddFolderToRecent.OnUpdate += new EventHandler<UpdateActionEventArgs>(this.actAddFolderToRecent_OnUpdate);
            this.categoryManager.SetCategory(this.actGCCollect, this.catMisc);
            this.actGCCollect.Name = "actGCCollect";
            manager.ApplyResources(this.actGCCollect, "actGCCollect");
            this.actGCCollect.OnExecute += new EventHandler<ActionEventArgs>(this.actGCCollect_OnExecute);
            this.categoryManager.SetCategory(this.actMinimizeToTray, this.catMisc);
            this.actMinimizeToTray.Name = "actMinimizeToTray";
            manager.ApplyResources(this.actMinimizeToTray, "actMinimizeToTray");
            this.actMinimizeToTray.OnExecute += new EventHandler<ActionEventArgs>(this.actMinimizeToTray_OnExecute);
            this.tsmiAbout2.Name = "tsmiAbout2";
            manager.ApplyResources(this.tsmiAbout2, "tsmiAbout2");
            this.tsmiBringToFront.Name = "tsmiBringToFront";
            manager.ApplyResources(this.tsmiBringToFront, "tsmiBringToFront");
            this.tsmiExit2.Name = "tsmiExit2";
            manager.ApplyResources(this.tsmiExit2, "tsmiExit2");
            this.categoryManager.CategoryList.AddRange(new Category[] { this.catFile, this.catEdit, this.catView, this.catPanel, this.catTab, this.catBookmarks, this.catMisc });
            this.cmsMenuNew.Items.AddRange(new ToolStripItem[] { this.tsmiNewFile, this.tssNewFile });
            this.cmsMenuNew.Name = "cmsNew";
            manager.ApplyResources(this.cmsMenuNew, "cmsMenuNew");
            this.cmsMenuNew.Opening += new CancelEventHandler(this.NewDropDown_Opening);
            this.cmsMenuFind.Items.AddRange(new ToolStripItem[] { this.tsmiFindDialog, this.tssFind, this.tsmiNoStoredSearches });
            this.cmsMenuFind.Name = "cmsFind";
            manager.ApplyResources(this.cmsMenuFind, "cmsMenuFind");
            this.cmsMenuFind.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
            this.cmsMenuFind.Opening += new CancelEventHandler(this.FindDropDown_Opening);
            this.cmsMenuWindowLayout.Items.AddRange(new ToolStripItem[] { this.tsmiSaveCurrentLayout, this.tsmiManageLayouts, this.tssLayout1, this.tsmiNoStoredLayouts });
            this.cmsMenuWindowLayout.Name = "cmsWindowLayout";
            manager.ApplyResources(this.cmsMenuWindowLayout, "cmsMenuWindowLayout");
            this.cmsMenuWindowLayout.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
            this.cmsMenuWindowLayout.Opening += new CancelEventHandler(this.LayoutDropDown_Opening);
            this.cmsMenuFilter.Items.AddRange(new ToolStripItem[] { this.tsmiFilterDialog, this.tssAdvancedFilter1, this.tsmiClearFilter, this.tssAdvancedFilter2, this.tsmiNoStoredFilters });
            this.cmsMenuFilter.Name = "cmsFilter";
            manager.ApplyResources(this.cmsMenuFilter, "cmsMenuFilter");
            this.cmsMenuFilter.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
            this.cmsMenuFilter.Opening += new CancelEventHandler(this.FilterDropDown_Opening);
            this.cmsMenuTab.Items.AddRange(new ToolStripItem[] { this.tsmiDuplicateTab, this.tsmiRenameTab, this.tssTab1, this.tsmiCloseTab, this.tsmiCloseOtherTabs, this.tssTab2 });
            this.cmsMenuTab.Name = "cmsMenuTab";
            manager.ApplyResources(this.cmsMenuTab, "cmsMenuTab");
            this.cmsMenuTab.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
            this.cmsMenuTab.Opening += new CancelEventHandler(this.TabDropDown_Opening);
            this.cmsMenuSort.Items.AddRange(new ToolStripItem[] { this.tsmiSelectSort, this.tssSort1, this.tsmiSortByName, this.tsmiSortByExtension, this.tsmiSortByLastWriteTime, this.tsmiSortBySize, this.tssSort2, this.tsmiSortDescending });
            this.cmsMenuSort.Name = "cmsMenuSort";
            manager.ApplyResources(this.cmsMenuSort, "cmsMenuSort");
            this.TrayIcon.ContextMenuStrip = this.cmsTray;
            manager.ApplyResources(this.TrayIcon, "TrayIcon");
            this.TrayIcon.MouseClick += new MouseEventHandler(this.TrayIcon_MouseClick);
            this.cmsTray.Items.AddRange(new ToolStripItem[] { this.tsmiAbout2, this.tssTray1, this.tsmiBringToFront, this.tsmiExit2 });
            this.cmsTray.Name = "cmsTray";
            manager.ApplyResources(this.cmsTray, "cmsTray");
            this.tssTray1.Name = "tssTray1";
            manager.ApplyResources(this.tssTray1, "tssTray1");
            manager.ApplyResources(this, "$this");
            base.Controls.Add(this.MainPageSwitcher);
            base.Controls.Add(this.MainTabStrip);
            base.Controls.Add(this.MainMenu);
            base.KeyPreview = true;
            base.MainMenuStrip = this.MainMenu;
            base.Name = "MainForm";
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
            base.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.cmsToolbar.ResumeLayout(false);
            this.cmsMenuViewAs.ResumeLayout(false);
            this.cmsTab.ResumeLayout(false);
            this.MainTabStrip.ResumeLayout(false);
            this.MainTabStrip.PerformLayout();
            this.actionManager.EndInit();
            this.cmsMenuNew.ResumeLayout(false);
            this.cmsMenuFind.ResumeLayout(false);
            this.cmsMenuWindowLayout.ResumeLayout(false);
            this.cmsMenuFilter.ResumeLayout(false);
            this.cmsMenuTab.ResumeLayout(false);
            this.cmsMenuSort.ResumeLayout(false);
            this.cmsTray.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeDropDownMenuItems()
        {
            this.tsmiNew.DropDown = this.cmsMenuNew;
            this.tsmiFind.DropDown = this.cmsMenuFind;
            this.tsmiWindowLayout.DropDown = this.cmsMenuWindowLayout;
            this.tsmiToolbars.DropDown = this.cmsToolbar;
            this.tsmiFilter.DropDown = this.cmsMenuFilter;
            this.tsmiViewAs.DropDown = this.cmsMenuViewAs;
            this.tsmiSort.DropDown = this.cmsMenuSort;
            this.tsmiTab.DropDown = this.cmsMenuTab;
            this.tsmiBookmarks.DropDown.Opening += new CancelEventHandler(this.BookmarksDropDown_Opening);
            ToolStripMenuItem[] itemArray = new ToolStripMenuItem[] { this.tsmiFolderBar, this.tsmiSpecialFolders, this.tsmiBookmarks };
            foreach (ToolStripMenuItem NextOwnerItem in itemArray)
            {
                NextOwnerItem.DropDown.Closed += delegate (object sender, ToolStripDropDownClosedEventArgs e) {
                    ((ToolStripDropDown) sender).OwnerItem = NextOwnerItem;
                };
            }
            this.cmsToolbar.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
            if (SettingsManager.GetArgument<bool>(ArgumentKey.Debug))
            {
                BasicForm.CheckForDuplicateMnemonics(this.MainMenu.Items);
                Stack<ToolStripItemCollection> stack = new Stack<ToolStripItemCollection>();
                stack.Push(this.MainMenu.Items);
                while (stack.Count > 0)
                {
                    foreach (ToolStripItem item in stack.Pop())
                    {
                        ToolStripMenuItem item2 = item as ToolStripMenuItem;
                        if ((item2 != null) && (item2.DropDownItems.Count > 0))
                        {
                            stack.Push(item2.DropDownItems);
                            item2.DropDown.Opened += delegate (object sender, EventArgs e) {
                                BasicForm.CheckForDuplicateMnemonics(((ToolStripDropDown) sender).Items);
                            };
                        }
                    }
                }
            }
        }

        private void InitializeImages()
        {
            if (IconSet.Available)
            {
                Stack<ToolStripItemCollection> stack = new Stack<ToolStripItemCollection>();
                stack.Push(base.MainMenuStrip.Items);
                stack.Push(this.cmsToolbar.Items);
                while (stack.Count > 0)
                {
                    ToolStripItemCollection items = stack.Pop();
                    foreach (ToolStripItem item in items)
                    {
                        ToolStripDropDownItem item2 = item as ToolStripDropDownItem;
                        if ((item2 != null) && (item2.DropDownItems.Count > 0))
                        {
                            stack.Push(item2.DropDownItems);
                        }
                        item.Image = IconSet.GetImage(item.Name);
                    }
                }
                foreach (Action action in this.actionManager.Actions)
                {
                    action.Image = IconSet.GetImage(action.Name);
                }
            }
        }

        private void InitializeKeyboardMap(string keyMap)
        {
            TypeConverter converter = new KeysConverter();
            using (TextReader reader = new StringReader(keyMap))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    if (str != string.Empty)
                    {
                        Action action;
                        string[] strArray = str.Split(new char[] { ',' });
                        List<Keys> list = new List<Keys>();
                        for (int i = 1; i < strArray.Length; i++)
                        {
                            list.Add((Keys) converter.ConvertFromInvariantString(strArray[i]));
                        }
                        if (strArray[0].StartsWith("act", StringComparison.OrdinalIgnoreCase) && this.ActionMap.TryGetValue(strArray[0], out action))
                        {
                            action.Shortcuts = (list.Count > 0) ? list.ToArray() : null;
                        }
                        if (strArray[0].StartsWith("tsmi", StringComparison.OrdinalIgnoreCase))
                        {
                            ToolStripItem[] itemArray = this.MainMenu.Items.Find(strArray[0], true);
                            ToolStripMenuItem item = (itemArray.Length > 0) ? (itemArray[0] as ToolStripMenuItem) : null;
                            if (item != null)
                            {
                                item.ShortcutKeys = (list.Count > 0) ? list[0] : Keys.None;
                            }
                        }
                    }
                }
            }
            this.InitializeSpecialKeyMap();
        }

        private static void InitializePropertyList()
        {
            Settings.Default.HideProperties();
        }

        private void InitializeSpecialFolders()
        {
            this.tsmiFolderDesktop.Tag = Environment.SpecialFolder.Desktop;
            this.tsmiFolderFavorites.Tag = Environment.SpecialFolder.Favorites;
            this.tsmiFolderMyDocuments.Tag = Environment.SpecialFolder.Personal;
            this.tsmiFolderMyPictures.Tag = Environment.SpecialFolder.MyPictures;
            this.tsmiFolderMyMusic.Tag = Environment.SpecialFolder.MyMusic;
            this.tsmiFolderTemp.Tag = OS.TempDirectory;
            this.tsmiFolderWindows.Tag = OS.WindowDirectory;
            this.tsmiFolderSystem.Tag = Environment.SystemDirectory;
            NameValueCollection section = ConfigurationManager.GetSection("specialDirs") as NameValueCollection;
            if (section != null)
            {
                this.tsmiSpecialFolders.DropDown.SuspendLayout();
                for (int i = 0; i < section.Count; i++)
                {
                    string str = section[i];
                    object obj2 = null;
                    string str2 = null;
                    try
                    {
                        obj2 = Enum.Parse(typeof(Environment.SpecialFolder), str, true);
                    }
                    catch (ArgumentException)
                    {
                        try
                        {
                            obj2 = Enum.Parse(typeof(CSIDL), str, true);
                        }
                        catch (ArgumentException)
                        {
                            try
                            {
                                if (OS.IsWinVista)
                                {
                                    obj2 = new Guid(str);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    if ((obj2 == null) && (str.StartsWith("shell:", StringComparison.OrdinalIgnoreCase) || System.IO.Directory.Exists(str)))
                    {
                        str2 = str;
                        str = '_' + i.ToString();
                    }
                    if (!((obj2 == null) && string.IsNullOrEmpty(str2)))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem {
                            Name = "tsmiFolder" + str,
                            Text = section.Keys[i],
                            ToolTipText = str2,
                            Tag = obj2
                        };
                        item.Paint += new PaintEventHandler(this.tsmiSpecialFolder_Paint);
                        item.Click += new EventHandler(this.tsmiSpecialFolder_Click);
                        this.tsmiSpecialFolders.DropDownItems.Add(item);
                        this.categoryManager.SetCategory(item, this.catPanel);
                    }
                }
                this.tsmiSpecialFolders.DropDown.ResumeLayout();
            }
        }

        private void InitializeSpecialKeyMap()
        {
            this.SpecialKeyMap.Clear();
            foreach (IComponent component in this.Commands)
            {
                Action action = component as Action;
                if (action != null)
                {
                    Keys[] shortcuts = action.Shortcuts;
                    if (shortcuts != null)
                    {
                        foreach (Keys keys in shortcuts)
                        {
                            if (IsSpecialKey(keys))
                            {
                                this.SpecialKeyMap.Add(keys, action);
                            }
                        }
                    }
                }
                ToolStripMenuItem item = component as ToolStripMenuItem;
                if ((item != null) && IsSpecialKey(item.ShortcutKeys))
                {
                    this.SpecialKeyMap.Add(item.ShortcutKeys, item);
                }
            }
            if (this.AppCommandActionMap == null)
            {
                this.AppCommandActionMap = new SortedList<APPCOMMAND, Action>(11);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_BROWSER_BACKWARD, this.actBack);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_BROWSER_FORWARD, this.actForward);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_BROWSER_REFRESH, this.actRefresh);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_FIND, this.actFind);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_NEW, this.actNewFile);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_OPEN, this.actDuplicateTab);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_CLOSE, this.actCloseTab);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_COPY, this.actCopyToClipboard);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_CUT, this.actCutToClipboard);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_PASTE, this.actPasteFromClipboard);
                this.AppCommandActionMap.Add(APPCOMMAND.APPCOMMAND_DELETE, this.actDelete);
            }
        }

        private void InitializeTabs()
        {
            this.InitializeTabsFromCmdLineTab();
            if (!SettingsManager.CheckSafeMode(SafeMode.SkipTabs))
            {
                string argument = SettingsManager.GetArgument<string>(ArgumentKey.RecoveryFolder);
                if (!(string.IsNullOrEmpty(argument) || !System.IO.Directory.Exists(argument)))
                {
                    this.RestoreAllTabs(argument, null);
                    CleanupManager.AddDirectory(argument);
                }
                if ((this.MainPageSwitcher.Controls.Count == 0) && Settings.Default.RestoreTabsOnStart)
                {
                    this.RestoreAllTabs(SettingsManager.SpecialFolders.Tabs, Nomad.Trace.Current.Mutex);
                }
            }
            if (this.MainPageSwitcher.Controls.Count == 0)
            {
                TwoPanelContainer newTabContent = TwoPanelContainer.Create();
                newTabContent.BeginLayout();
                try
                {
                    newTabContent.FixMouseWheel = Settings.Default.FixMouseWheel;
                    if (!SettingsManager.CheckSafeMode(SafeMode.SkipTabs))
                    {
                        newTabContent.LoadComponentSettings();
                    }
                    else
                    {
                        newTabContent.ResetComponentSettings();
                    }
                    this.AddNewTab(newTabContent);
                }
                finally
                {
                    newTabContent.EndLayout(true);
                }
            }
            this.MainTabStrip.FirstTab.PerformClick();
            this.InitializeTabsFromCmdLineFolders(this.CurrentTabContent);
        }

        private bool InitializeTabsFromCmdLineFolders(TwoPanelContainer tabContent)
        {
            string str;
            bool flag = false;
            if (!string.IsNullOrEmpty(str = SettingsManager.GetArgument<string>(ArgumentKey.LeftFolder)))
            {
                tabContent.LeftPanel.SetCurrentFolder(str, VirtualItemType.Unknown);
                flag = true;
            }
            if (!string.IsNullOrEmpty(str = SettingsManager.GetArgument<string>(ArgumentKey.RightFolder)))
            {
                tabContent.RightPanel.SetCurrentFolder(str, VirtualItemType.Unknown);
                flag = true;
            }
            if (!string.IsNullOrEmpty(str = SettingsManager.GetArgument<string>(ArgumentKey.CurrentFolder)))
            {
                tabContent.CurrentPanel.SetCurrentFolder(str, VirtualItemType.Unknown);
                flag = true;
            }
            if (!string.IsNullOrEmpty(str = SettingsManager.GetArgument<string>(ArgumentKey.FarFolder)))
            {
                tabContent.FarPanel.SetCurrentFolder(str, VirtualItemType.Unknown);
                flag = true;
            }
            return flag;
        }

        private bool InitializeTabsFromCmdLineTab()
        {
            string argument = SettingsManager.GetArgument<string>(ArgumentKey.Tab);
            if ((argument != null) && ".tab".Equals(Path.GetExtension(argument), StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (Stream stream = System.IO.File.OpenRead(argument))
                    {
                        return (this.AddNewTab(stream, true) != null);
                    }
                }
                catch (Exception exception)
                {
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                }
            }
            return false;
        }

        private void InitializeToolbar(ToolStrip toolbar, string commands)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            IContainer container = null;
            if (toolbar.Items.Count > 0)
            {
                container = new Container();
                foreach (ToolStripItem item in toolbar.Items)
                {
                    ToolStripDropDownItem item2 = item as ToolStripDropDownItem;
                    if (item2 != null)
                    {
                        item2.DropDown = null;
                    }
                    container.Add(item);
                }
            }
            toolbar.SuspendLayout();
            toolbar.Items.Clear();
            if (container != null)
            {
                container.Dispose();
            }
            using (TextReader reader = new StringReader(commands))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    string str2;
                    IconLocation location;
                    switch (str)
                    {
                        case "":
                        {
                            continue;
                        }
                        case "-":
                        {
                            ToolStripSeparator separator = new ToolStripSeparator();
                            separator.Name = "Separator_" + this.UniqueIndex++.ToString();
                            separator.Paint += new PaintEventHandler(this.ToolbarButton_Paint);
                            toolbar.Items.Add(separator);
                            continue;
                        }
                    }
                    ToolStripItemDisplayStyle image = ToolStripItemDisplayStyle.Image;
                    if (ToolbarSettings.ParseToolbarButtonLine(str, out str2, ref image, out location))
                    {
                        if (str2.Equals("bookmarks", StringComparison.OrdinalIgnoreCase))
                        {
                            flag2 = System.IO.Directory.Exists(SettingsManager.SpecialFolders.Bookmarks);
                            if (flag2)
                            {
                                foreach (ToolStripItem item3 in this.CreateBookmarkList(SettingsManager.SpecialFolders.Bookmarks, typeof(ToolStripButton), image))
                                {
                                    item3.Name = "Bookmark_All_" + item3.Name.Substring("Bookmark_".Length);
                                    GeneralTab tag = item3.Tag as GeneralTab;
                                    if (tag != null)
                                    {
                                        item3.ToolTipText = CreateToolTipText(item3.Text, tag.Hotkey);
                                    }
                                    item3.Paint += new PaintEventHandler(this.ToolbarButton_Paint);
                                    toolbar.Items.Add(item3);
                                    IVirtualItem item4 = item3.Tag as IVirtualItem;
                                    if (item4 != null)
                                    {
                                        VirtualItemToolStripEvents.UpdateItemImage(item3, item4);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Action action;
                            if (str2.Equals("tools", StringComparison.OrdinalIgnoreCase))
                            {
                                flag = System.IO.Directory.Exists(SettingsManager.SpecialFolders.Tools);
                                if (flag)
                                {
                                    foreach (ToolStripItem item5 in this.CreateToolList(SettingsManager.SpecialFolders.Tools, typeof(ToolStripButton), image))
                                    {
                                        item5.Name = "Tool_All_" + item5.Name.Substring("Tool_".Length);
                                        item5.Paint += new PaintEventHandler(this.ToolbarButton_Paint);
                                        toolbar.Items.Add(item5);
                                    }
                                }
                                continue;
                            }
                            if (str2.Equals("drives", StringComparison.OrdinalIgnoreCase))
                            {
                                flag3 = true;
                                this.SetUpdateDriveButtonsNeeded();
                                foreach (IVirtualFolder folder in VirtualItem.GetRootFolders())
                                {
                                    if (!Settings.Default.HideNotReadyDrives || VirtualFilePanel.IsRootFolderVisible(folder))
                                    {
                                        ToolStripItem item6 = new ToolStripButton {
                                            Name = string.Format("{0}{1}_{2}", "Drive_All_", this.UniqueIndex++, folder.ShortName),
                                            Text = folder.ShortName,
                                            AutoToolTip = false,
                                            Tag = folder
                                        };
                                        item6.Click += new EventHandler(this.DriveButton_Click);
                                        item6.MouseUp += new MouseEventHandler(VirtualItemToolStripEvents.MouseUp);
                                        item6.MouseHover += new EventHandler(VirtualItemToolStripEvents.MouseHover);
                                        item6.MouseLeave += new EventHandler(VirtualItemToolStripEvents.MouseLeave);
                                        item6.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintImage);
                                        System.Drawing.Color foreColor = VirtualItemHelper.GetForeColor(folder, System.Drawing.Color.Empty);
                                        if (!foreColor.IsEmpty)
                                        {
                                            item6.ForeColor = foreColor;
                                            item6.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintForeColor);
                                        }
                                        toolbar.Items.Add(item6);
                                        VirtualItemToolStripEvents.UpdateItemImage(item6, folder);
                                    }
                                }
                                continue;
                            }
                            ToolStripItem component = null;
                            if (str2.StartsWith("act", StringComparison.OrdinalIgnoreCase) && this.ActionMap.TryGetValue(str2, out action))
                            {
                                ToolStripDropDownItem item8 = null;
                                if ((action == this.actBack) || (action == this.actForward))
                                {
                                    item8 = new ToolStripSplitButton();
                                    item8.DropDownOpening += new EventHandler(this.HistoryButton_DropDownOpening);
                                    item8.DropDown.Closed += new ToolStripDropDownClosedEventHandler(this.CleanupDropDown_Closed);
                                }
                                else
                                {
                                    ToolStripDropDown down;
                                    if (this.ActionDropDownMap.TryGetValue(action, out down))
                                    {
                                        if (action == this.actShowBookmarks)
                                        {
                                            item8 = new ToolStripDropDownButton();
                                        }
                                        else
                                        {
                                            item8 = new ToolStripSplitButton();
                                        }
                                        item8.DropDown = down;
                                    }
                                }
                                if (item8 != null)
                                {
                                    component = item8;
                                }
                                else
                                {
                                    component = new ToolStripButton();
                                }
                                component.Name = string.Format("{0}{1}_{2}", "Action_", this.UniqueIndex++, str2);
                                component.TextChanged += new EventHandler(this.ToolbarButton_TextChanged);
                                component.MouseEnter += new EventHandler(this.ToolbarButton_MouseEnter);
                                this.actionManager.SetAction(component, action);
                            }
                            if ((component == null) && str2.StartsWith("tsmi", StringComparison.OrdinalIgnoreCase))
                            {
                                ToolStripItem[] itemArray = this.MainMenu.Items.Find(str2, true);
                                ToolStripMenuItem item9 = (itemArray.Length > 0) ? (itemArray[0] as ToolStripMenuItem) : null;
                                if ((item9 != null) && (item9.DropDownItems.Count > 0))
                                {
                                    ToolStripDropDownItem item10 = new ToolStripDropDownButton {
                                        Name = string.Format("{0}{1}_{2}", "DropDown_", this.UniqueIndex++, str2)
                                    };
                                    item10.TextChanged += new EventHandler(this.ToolbarButton_TextChanged);
                                    item10.Text = item9.Text;
                                    item10.Image = item9.Image;
                                    item10.DropDown = item9.DropDown;
                                    component = item10;
                                }
                            }
                            if ((component == null) && str2.StartsWith(@"bookmarks\", StringComparison.OrdinalIgnoreCase))
                            {
                                string path = Path.Combine(SettingsManager.SpecialFolders.Bookmarks, str2.Substring(10));
                                if (System.IO.File.Exists(path) && VirtualItem.IsLink(path))
                                {
                                    IVirtualLink virtualLink = new FileSystemShellLink(path);
                                    if (!string.IsNullOrEmpty((string) virtualLink[10]))
                                    {
                                        component = new ToolStripButton();
                                        this.InitializeBookmarkItem(component, virtualLink);
                                        flag2 = true;
                                    }
                                }
                            }
                            if ((component == null) && str2.StartsWith(@"tools\", StringComparison.OrdinalIgnoreCase))
                            {
                                string str4 = Path.Combine(SettingsManager.SpecialFolders.Tools, str2.Substring(6));
                                if (System.IO.File.Exists(str4))
                                {
                                    try
                                    {
                                        using (ShellLink link2 = new ShellLink(str4))
                                        {
                                            component = new ToolStripButton();
                                            this.InitializeToolItem(component, str4, link2);
                                            component.DisplayStyle = image;
                                            flag = true;
                                        }
                                    }
                                    catch (SystemException exception)
                                    {
                                        ApplicationException e = new ApplicationException(string.Format("Error loading tool link '{0}'", str4), exception);
                                        Nomad.Trace.Error.TraceException(TraceEventType.Warning, e);
                                    }
                                }
                            }
                            if (component != null)
                            {
                                if (location != null)
                                {
                                    component.SetTag(2, location);
                                    this.SetToolbarButtonImage(component, location);
                                }
                                component.Paint += new PaintEventHandler(this.ToolbarButton_Paint);
                                component.DisplayStyle = image;
                                toolbar.Items.Add(component);
                            }
                        }
                    }
                }
            }
            CleanupToolbar(toolbar);
            this.JustifyToolbar(toolbar);
            if (toolbar.Items.Count == 0)
            {
                toolbar.Visible = false;
            }
            else
            {
                if (flag)
                {
                    this.InitializeToolsWatcher();
                }
                if (flag2)
                {
                    this.InitializeBookmarksWatcher();
                }
                if (flag3 && (this.VolumeChangedHandler == null))
                {
                    this.VolumeChangedHandler = new EventHandler(this.Volume_Changed);
                    VolumeEvents.TopLevelForm = this;
                    VolumeEvents.Changed += this.VolumeChangedHandler;
                }
            }
            toolbar.ResumeLayout();
        }

        private void InitializeToolbars()
        {
            base.SuspendLayout();
            for (int i = ToolbarSettings.Toolbars.Count - 1; i >= 0; i--)
            {
                ToolbarSettings dataSource = ToolbarSettings.Toolbars[i];
                ToolStrip strip = new ToolStrip {
                    Name = "Toolbar_" + dataSource.SettingsKey,
                    AllowMerge = false,
                    Tag = dataSource,
                    AllowDrop = true,
                    GripStyle = ToolStripGripStyle.Hidden,
                    ContextMenuStrip = this.cmsToolbar
                };
                strip.Paint += new PaintEventHandler(this.Toolbar_Paint);
                strip.VisibleChanged += new EventHandler(this.Toolbar_VisibleChanged);
                strip.TextChanged += new EventHandler(this.Toolbar_TextChanged);
                strip.DragDrop += new DragEventHandler(this.Toolbar_DragDrop);
                strip.DragEnter += new DragEventHandler(this.Toolbar_DragEnter);
                strip.DragLeave += new EventHandler(this.Toolbar_DragLeave);
                strip.DragOver += new DragEventHandler(this.Toolbar_DragOver);
                strip.DataBindings.Add(new Binding("Text", dataSource, "Commands", true, DataSourceUpdateMode.OnPropertyChanged));
                strip.Text = dataSource.Commands;
                strip.DataBindings.Add(new Binding("Dock", dataSource, "Dock", true, DataSourceUpdateMode.OnPropertyChanged));
                strip.DataBindings.Add(new Binding("Visible", dataSource, "Visible", true, DataSourceUpdateMode.OnPropertyChanged));
                SettingsManager.RegisterSettings(dataSource);
                base.Controls.Add(strip);
            }
            this.MainMenu.SendToBack();
            base.ResumeLayout();
        }

        private void InitializeToolItem(ToolStripItem toolItem, string toolPath, ShellLink toolLink)
        {
            Keys hotkey;
            toolItem.Name = string.Format("{0}{1}_{2}", "Tool_", this.UniqueIndex++, Path.GetFileNameWithoutExtension(toolPath));
            toolItem.Text = GetMainMenuFileName(toolPath, true);
            toolItem.SetTag(1, toolPath);
            toolItem.Paint += new PaintEventHandler(this.tsmiExternalTool_Paint);
            toolItem.Click += new EventHandler(this.tsmiExternalTool_Click);
            try
            {
                hotkey = toolLink.Hotkey;
                ToolStripMenuItem item = toolItem as ToolStripMenuItem;
                if (item != null)
                {
                    item.ShortcutKeys = hotkey;
                }
            }
            catch (InvalidEnumArgumentException)
            {
                hotkey = Keys.None;
            }
            toolItem.ToolTipText = toolLink.Description;
            if (string.IsNullOrEmpty(toolItem.ToolTipText))
            {
                toolItem.ToolTipText = toolItem.Text;
            }
            if (toolItem is ToolStripButton)
            {
                toolItem.ToolTipText = CreateToolTipText(toolItem.ToolTipText, hotkey);
            }
        }

        private void InitializeToolStrip(bool reinitialize)
        {
            if (reinitialize)
            {
                Theme.SetTheme(Settings.Default.Theme);
                this.MainTabStrip.SuspendLayout();
                this.MainTabStrip.Renderer = Theme.Current.TabStripRenderer;
                this.MainTabStrip.UseBoldFont = Theme.Current.SelectedTabBoldFont;
                this.MainTabStrip.ResetPadding();
                this.MainTabStrip.ResetTabsHeight();
                foreach (ToolStrip strip in base.FindAllChildren<ToolStrip>())
                {
                    FontFamily family = (strip == this.MainTabStrip) ? Theme.Current.TabStripFontFamily : Theme.Current.ToolStripFontFamily;
                    if (!((family == null) || strip.Font.FontFamily.Equals(family)))
                    {
                        strip.Font = new Font(family, strip.Font.Size);
                    }
                    else
                    {
                        strip.ResetFont();
                    }
                }
                this.MainTabStrip.ResumeLayout();
            }
            this.MainPageSwitcher.BackColor = Theme.Current.ThemeColors.WindowBackground;
        }

        private void InitializeToolsWatcher()
        {
            if (((this.ToolsWatcher == null) && System.IO.Directory.Exists(SettingsManager.SpecialFolders.Tools)) && OS.IsWinNT)
            {
                this.ToolsWatcher = new FileSystemWatcher(SettingsManager.SpecialFolders.Tools, "*.lnk");
                this.ToolsWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                this.ToolsWatcher.Created += new FileSystemEventHandler(this.ToolsFolderChanged);
                this.ToolsWatcher.Deleted += new FileSystemEventHandler(this.ToolsFolderChanged);
                this.ToolsWatcher.Changed += new FileSystemEventHandler(this.ToolsFolderChanged);
                this.ToolsWatcher.Renamed += new RenamedEventHandler(this.ToolsFolderChanged);
                this.ToolsWatcher.SynchronizingObject = this;
                this.ToolsWatcher.EnableRaisingEvents = true;
            }
        }

        private string InstallTotalCmdPlugin(Ini.IniSection installSection, ArchiveFolder source, string sourcePluginName, string destPluginsFolder, IDisposable oldPlugin)
        {
            if (source.FromName(sourcePluginName) != null)
            {
                string[] strArray;
                if (System.IO.Directory.Exists(destPluginsFolder))
                {
                    strArray = System.IO.Directory.GetFiles(destPluginsFolder, sourcePluginName, SearchOption.AllDirectories);
                }
                else
                {
                    strArray = new string[0];
                }
                string path = Path.Combine(destPluginsFolder, Path.GetFileNameWithoutExtension(sourcePluginName));
                string str2 = installSection["description" + Thread.CurrentThread.CurrentUICulture.ThreeLetterISOLanguageName];
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = installSection["description"];
                }
                str2 = string.IsNullOrEmpty(str2) ? string.Empty : (" (" + str2 + ")");
                string format = ((oldPlugin != null) || (strArray.Length > 0)) ? Resources.sAskReinstallPlugin : Resources.sAskInstallPlugin;
                format = string.Format(format, sourcePluginName, str2);
                if (MessageDialog.Show(this, format, Resources.sConfirmInstallPlugin, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) != MessageDialogResult.Yes)
                {
                    return null;
                }
                string str4 = path + ".tmp";
                string destName = sourcePluginName + ".tmp";
                System.IO.Directory.CreateDirectory(str4);
                IVirtualFolder dest = (IVirtualFolder) VirtualItem.FromFullName(str4, VirtualItemType.Folder);
                IVirtualItemFilter filter = new AggregatedVirtualItemFilter(new VirtualItemNameFilter(NamePatternCondition.NotEqual, "pluginst.inf"), new VirtualItemAttributeFilter(0, FileAttributes.Directory));
                CopyWorker woker = new CopyWorker(source.GetContent(), dest, null, 0, filter, new DictionaryRenameFilter(sourcePluginName, destName));
                using (CopyWorkerDialog dialog = new CopyWorkerDialog())
                {
                    dialog.DefaultOverwriteRules = new List<IOverwriteRule>(1);
                    dialog.DefaultOverwriteRules.Add(new OverwriteAllRule(OverwriteDialogResult.Overwrite));
                    if (!dialog.Run(this, woker))
                    {
                        return null;
                    }
                }
                if (System.IO.File.Exists(Path.Combine(str4, destName)))
                {
                    if (oldPlugin != null)
                    {
                        oldPlugin.Dispose();
                    }
                    foreach (string str7 in strArray)
                    {
                        System.IO.File.Delete(str7);
                    }
                    if (System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                    System.IO.Directory.Move(str4, path);
                    string sourceFileName = Path.Combine(path, destName);
                    string destFileName = Path.Combine(path, sourcePluginName);
                    System.IO.File.Move(sourceFileName, destFileName);
                    return destFileName;
                }
            }
            return null;
        }

        private void InstallWcxPlugin(Ini.IniSection installSection, ArchiveFolder source)
        {
            string str = installSection["file"];
            if (!string.IsNullOrEmpty(str))
            {
                WcxFormatInfo format = WcxFormatInfo.GetFormat(str);
                string str2 = this.InstallTotalCmdPlugin(installSection, source, str, WcxFormatInfo.WcxPluginsPath, format);
                if (!string.IsNullOrEmpty(str2))
                {
                    format = WcxFormatInfo.RegisterFormat(str2);
                    string str3 = installSection["defaultextension"];
                    if (!string.IsNullOrEmpty(str3))
                    {
                        format.Extension = str3.Split(new char[] { ',' });
                        format.SaveComponentSettings();
                    }
                }
            }
        }

        private void InstallWdxPlugin(Ini.IniSection installSection, ArchiveFolder source)
        {
            string PluginName = installSection["file"];
            if (!string.IsNullOrEmpty(PluginName))
            {
                Func<PropertyProviderInfo, bool> predicate = delegate (PropertyProviderInfo x) {
                    return x.Key.EndsWith(Path.DirectorySeparatorChar + PluginName, StringComparison.OrdinalIgnoreCase);
                };
                IDisposableContainer oldPlugin = new DisposableContainer(PropertyProviderManager.GetAllProviders().Where<PropertyProviderInfo>(predicate).Cast<IDisposable>());
                this.InstallTotalCmdPlugin(installSection, source, PluginName, WdxPropertyProviderFactory.WdxPluginsPath, oldPlugin);
            }
        }

        private bool IsDroppableAction(Action action)
        {
            if (this.FDroppableActions == null)
            {
                this.FDroppableActions = new HashSet<Action>(0x18);
                this.FDroppableActions.Add(this.actCopy);
                this.FDroppableActions.Add(this.actRenameMove);
                this.FDroppableActions.Add(this.actDelete);
                this.FDroppableActions.Add(this.actSetAttributes);
                this.FDroppableActions.Add(this.actShowProperties);
                this.FDroppableActions.Add(this.actCutToClipboard);
                this.FDroppableActions.Add(this.actCopyToClipboard);
                this.FDroppableActions.Add(this.actCopyNameAsText);
                this.FDroppableActions.Add(this.actCopyFullNameAsText);
                this.FDroppableActions.Add(this.actPack);
                this.FDroppableActions.Add(this.actBookmarkCurrentFolder);
                this.FDroppableActions.Add(this.actRunAs);
                this.FDroppableActions.Add(this.actRunAsAdmin);
                this.FDroppableActions.Add(this.actEditItem);
                this.FDroppableActions.Add(this.actMakeLink);
                this.FDroppableActions.Add(this.actEditDescription);
                this.FDroppableActions.Add(this.actViewItem);
                this.FDroppableActions.Add(this.actCopyDetailsAsCSV);
                this.FDroppableActions.Add(this.actOpen);
                this.FDroppableActions.Add(this.actOpenAsArchive);
                this.FDroppableActions.Add(this.actOpenOutside);
                this.FDroppableActions.Add(this.actOpenContainingFolder);
                this.FDroppableActions.Add(this.actOpenInFarPanel);
                this.FDroppableActions.Add(this.actAddFolderToRecent);
            }
            return this.FDroppableActions.Contains(action);
        }

        private static bool IsSpecialKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Multiply:
                case Keys.Add:
                case Keys.Subtract:
                case Keys.Divide:
                case Keys.Delete:
                case Keys.Space:
                case Keys.Back:
                case Keys.Return:
                case (Keys.Shift | Keys.Insert):
                case (Keys.Shift | Keys.Delete):
                case (Keys.Control | Keys.Insert):
                case (Keys.Control | Keys.A):
                case (Keys.Control | Keys.C):
                case (Keys.Control | Keys.V):
                case (Keys.Control | Keys.X):
                case (Keys.Control | Keys.Z):
                case (Keys.Alt | Keys.Back):
                    return true;
            }
            return false;
        }

        private void JustifyToolbar(ToolStrip toolbar)
        {
            this.JustifyToolbar(toolbar, ((ToolbarSettings) toolbar.Tag).JustifyLabels);
        }

        private void JustifyToolbar(ToolStrip toolbar, bool justify)
        {
            if (justify)
            {
                ToolStripItem current;
                int num = 0;
                using (IEnumerator enumerator = toolbar.Items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = (ToolStripItem) enumerator.Current;
                        switch (current.DisplayStyle)
                        {
                            case ToolStripItemDisplayStyle.Text:
                            case ToolStripItemDisplayStyle.ImageAndText:
                                if (!(current is ToolStripSeparator))
                                {
                                    num++;
                                }
                                break;
                        }
                    }
                }
                if (num > 0)
                {
                    float width = 100f / ((float) num);
                    toolbar.SuspendLayout();
                    toolbar.LayoutStyle = ToolStripLayoutStyle.Table;
                    TableLayoutSettings layoutSettings = (TableLayoutSettings) toolbar.LayoutSettings;
                    layoutSettings.RowCount = 1;
                    layoutSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, (float) toolbar.Height));
                    layoutSettings.ColumnCount = toolbar.Items.Count;
                    layoutSettings.ColumnStyles.Clear();
                    for (int i = 0; i < toolbar.Items.Count; i++)
                    {
                        current = toolbar.Items[i];
                        bool flag = false;
                        switch (current.DisplayStyle)
                        {
                            case ToolStripItemDisplayStyle.Text:
                            case ToolStripItemDisplayStyle.ImageAndText:
                                flag = !(current is ToolStripSeparator);
                                break;
                        }
                        if (flag)
                        {
                            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));
                            current.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            layoutSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                            current.Dock = DockStyle.Left;
                        }
                        layoutSettings.SetCellPosition(current, new TableLayoutPanelCellPosition(i, 0));
                    }
                    toolbar.ResumeLayout();
                }
            }
            else
            {
                toolbar.SuspendLayout();
                toolbar.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                foreach (ToolStripItem item in toolbar.Items)
                {
                    item.Dock = DockStyle.None;
                }
                toolbar.ResumeLayout();
            }
        }

        private void LayoutDropDown_Opening(object sender, CancelEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            down.SuspendLayout();
            try
            {
                down.Items.Clear();
                down.Items.AddRange(new ToolStripItem[] { this.tsmiSaveCurrentLayout, this.tsmiManageLayouts, this.tssLayout1 });
                TwoPanelLayout[] layouts = Settings.Default.Layouts;
                if ((layouts != null) && (layouts.Length > 0))
                {
                    IContainer container = new Container();
                    foreach (TwoPanelLayout layout in layouts)
                    {
                        ToolStripMenuItem component = new ToolStripMenuItem(layout.Name) {
                            Tag = layout
                        };
                        component.Click += new EventHandler(this.LayoutItem_Click);
                        container.Add(component);
                        down.Items.Add(component);
                    }
                    down.Tag = container;
                }
                else
                {
                    down.Items.Add(this.tsmiNoStoredLayouts);
                }
            }
            finally
            {
                down.ResumeLayout();
            }
        }

        private void LayoutItem_Click(object sender, EventArgs e)
        {
            this.CurrentTabContent.WindowLayout = (TwoPanelLayout) ((ToolStripItem) sender).Tag;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (Clipboard.ContainsData("Virtual Item Operation"))
                {
                    Clipboard.Clear();
                }
            }
            catch (ExternalException exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
            this.CurrentTabContent.SaveComponentSettings();
            if (!base.InvokeRequired)
            {
                if (OS.IsDwmCompositionEnabled)
                {
                    Windows.SendMessage(base.Handle, 11, IntPtr.Zero, IntPtr.Zero);
                }
                else
                {
                    LockWindowUpdate.Lock(this);
                }
                this.MainPageSwitcher.Dispose();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool flag = false;
            if (this.MainPageSwitcher.Controls.Count > 1)
            {
                CloseReason closeReason = e.CloseReason;
                if (((closeReason == CloseReason.None) || (closeReason == CloseReason.UserClosing)) || (closeReason == CloseReason.ApplicationExitCall))
                {
                    MessageDialogResult saveTabs = ConfirmationSettings.Default.SaveTabs;
                    if (saveTabs == MessageDialogResult.None)
                    {
                        bool checkBoxChecked = false;
                        Dictionary<MessageDialogResult, string> buttonTextMap = new Dictionary<MessageDialogResult, string>(2);
                        buttonTextMap.Add(MessageDialogResult.Yes, Resources.sMessageButtonSaveAndQuit);
                        buttonTextMap.Add(MessageDialogResult.No, Resources.sMessageButtonQuit);
                        saveTabs = MessageDialog.Show(this, string.Format(Resources.sAskSaveTabs, this.MainPageSwitcher.Controls.Count), Resources.sConfirmCloseTabs, Resources.sRememberQuestionAnswer, ref checkBoxChecked, MessageDialog.ButtonsYesNoCancel, buttonTextMap, MessageBoxIcon.Question, MessageDialogResult.Cancel);
                        if (checkBoxChecked)
                        {
                            switch (saveTabs)
                            {
                                case MessageDialogResult.Yes:
                                case MessageDialogResult.No:
                                    ConfirmationSettings.Default.SaveTabs = saveTabs;
                                    break;
                            }
                        }
                    }
                    switch (saveTabs)
                    {
                        case MessageDialogResult.Yes:
                            flag = this.RememberAllTabs(SettingsManager.SpecialFolders.Tabs, Nomad.Trace.Current.Mutex);
                            break;

                        case MessageDialogResult.No:
                            break;

                        default:
                            e.Cancel = true;
                            return;
                    }
                }
            }
            Settings.Default.RestoreTabsOnStart = flag;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.DiscardSpecialKey(e.KeyData))
            {
                e.Handled = true;
            }
        }

        private void MainMenu_MenuActivate(object sender, EventArgs e)
        {
            if (!this.MainMenu.Visible)
            {
                using (new LockWindowRedraw(this, true))
                {
                    this.MainMenu.Visible = true;
                }
            }
        }

        private void MainMenu_MenuDeactivate(object sender, EventArgs e)
        {
            if (!Settings.Default.MainMenuVisible && this.MainMenu.Visible)
            {
                using (new LockWindowRedraw(this, true))
                {
                    this.MainMenu.Visible = false;
                }
            }
        }

        private void MainMenu_Paint(object sender, PaintEventArgs e)
        {
            if ((((this.ControlModifierKeys != Keys.None) && (this.ControlModifierKeys == (this.actShowBookmarks.ShortcutKeys & ~Keys.KeyCode))) && !this.tsmiBookmarks.Selected) && !this.tsmiBookmarks.Pressed)
            {
                Dictionary<ToolStripItem, Keys> dictionary = new Dictionary<ToolStripItem, Keys>();
                dictionary.Add(this.tsmiBookmarks, this.actShowBookmarks.ShortcutKeys & Keys.KeyCode);
                base.BeginInvoke(new Action<ToolStrip, Dictionary<ToolStripItem, Keys>>(this.DrawKeyboardCues), new object[] { this.MainMenu, dictionary });
            }
        }

        private void MainPageSwitcher_ControlAdded(object sender, ControlEventArgs e)
        {
            this.MainTabStrip.Visible = Settings.Default.AlwaysShowTabStrip || (this.MainPageSwitcher.Controls.Count > 1);
        }

        private void MainTabStrip_AfterPaint(object sender, PaintEventArgs e)
        {
            if (this.ControlModifierKeys != Keys.None)
            {
                Tab tab;
                Dictionary<ToolStripItem, Keys> buttonKeyMap = new Dictionary<ToolStripItem, Keys>();
                Dictionary<int, Keys> dictionary2 = new Dictionary<int, Keys>();
                foreach (Action action in this.catTab.Components)
                {
                    if (action.Tag is int)
                    {
                        dictionary2.Add((int) action.Tag, action.ShortcutKeys);
                    }
                }
                int num = 0;
                for (int i = 0; i < this.MainTabStrip.Items.Count; i++)
                {
                    tab = this.MainTabStrip.Items[i] as Tab;
                    if (tab != null)
                    {
                        Keys keys;
                        if (dictionary2.TryGetValue(++num, out keys) && ((keys & ~Keys.KeyCode) == this.ControlModifierKeys))
                        {
                            buttonKeyMap.Add(tab, keys & Keys.KeyCode);
                        }
                        else if (tab.IsLastTab && ((this.actMoveToLastTab.ShortcutKeys & ~Keys.KeyCode) == this.ControlModifierKeys))
                        {
                            buttonKeyMap.Add(tab, this.actMoveToLastTab.ShortcutKeys & Keys.KeyCode);
                        }
                    }
                }
                if ((this.actMoveToPreviousTab.ShortcutKeys & ~Keys.KeyCode) == this.ControlModifierKeys)
                {
                    tab = this.MainTabStrip.GetNextTab(this.MainTabStrip.SelectedTab, false, true);
                    if (tab != null)
                    {
                        buttonKeyMap[tab] = this.actMoveToPreviousTab.ShortcutKeys & Keys.KeyCode;
                    }
                }
                if ((this.actMoveToNextTab.ShortcutKeys & ~Keys.KeyCode) == this.ControlModifierKeys)
                {
                    tab = this.MainTabStrip.GetNextTab(this.MainTabStrip.SelectedTab, true, true);
                    if (tab != null)
                    {
                        buttonKeyMap[tab] = this.actMoveToNextTab.ShortcutKeys & Keys.KeyCode;
                    }
                }
                if ((this.actCloseTab.ShortcutKeys & ~Keys.KeyCode) == this.ControlModifierKeys)
                {
                    buttonKeyMap.Add(this.tsbCloseTab, this.actCloseTab.ShortcutKeys & Keys.KeyCode);
                }
                if (buttonKeyMap.Count > 0)
                {
                    this.DrawKeyboardCues(this.MainTabStrip, e.Graphics, buttonKeyMap);
                }
            }
        }

        private void MainTabStrip_DoubleClick(object sender, EventArgs e)
        {
            if (this.MainTabStrip.GetItemAt(this.MainTabStrip.PointToClient(Cursor.Position)) == null)
            {
                this.actDuplicateTab.Execute();
            }
        }

        private void MainTabStrip_SelectedTabChanged(object sender, EventArgs e)
        {
            this.FPreviousTab = null;
            this.SetUpdateDriveButtonsNeeded();
        }

        private void MainTabStrip_SelectedTabChanging(object sender, TabStripCancelEventArgs e)
        {
            TwoPanelContainer container = (TwoPanelContainer) e.Tab.TabStripPage.Controls[0];
            if (!(container.IsContentInitialized || !base.IsHandleCreated))
            {
                WaitCursor.ShowUntilIdle();
            }
        }

        private void MainTabStrip_SizeChanged(object sender, EventArgs e)
        {
            this.tsbCloseTab.Height = this.MainTabStrip.DisplayRectangle.Height - 2;
        }

        private void MakeNewFile(IVirtualFolder folder, ShellNew command)
        {
            ICreateVirtualFile file = folder as ICreateVirtualFile;
            if (file != null)
            {
                string newName = null;
                using (NewFileDialog dialog = new NewFileDialog())
                {
                    base.AddOwnedForm(dialog);
                    if (command != null)
                    {
                        dialog.Command = command;
                    }
                    if (dialog.Execute(this.CurrentPanel.CurrentFolder))
                    {
                        command = dialog.Command;
                        newName = dialog.NewName;
                    }
                    else
                    {
                        command = null;
                    }
                }
                if (command != null)
                {
                    try
                    {
                        IChangeVirtualFile Dest = file.CreateFile(newName + command.Extension);
                        IPersistVirtualItem item = Dest as IPersistVirtualItem;
                        if ((item == null) || !item.Exists)
                        {
                            using (new WaitCursor(this))
                            {
                                Stream DestStream = null;
                                if (!(this.ExecuteElevated(Dest, delegate {
                                    DestStream = Dest.Open(FileMode.CreateNew, FileAccess.Write, FileShare.Read, FileOptions.SequentialScan, 0L);
                                }) && (DestStream != null)))
                                {
                                    return;
                                }
                                using (DestStream)
                                {
                                    switch (command.CommandType)
                                    {
                                        case ShellNewCommand.Data:
                                        {
                                            byte[] data = command.Data;
                                            if (data != null)
                                            {
                                                DestStream.Write(data, 0, data.Length);
                                            }
                                            break;
                                        }
                                        case ShellNewCommand.FileName:
                                            command.ResolveFileName();
                                            if (System.IO.File.Exists(command.Command))
                                            {
                                                using (Stream stream = System.IO.File.OpenRead(command.Command))
                                                {
                                                    byte[] buffer = new byte[0x2000];
                                                    int count = 0;
                                                    do
                                                    {
                                                        count = stream.Read(buffer, 0, buffer.Length);
                                                        DestStream.Write(buffer, 0, count);
                                                    }
                                                    while (count > 0);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        this.CurrentPanel.SetFocusedItem(Dest, true, true);
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
        }

        private void NewDropDown_Opening(object sender, CancelEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            if (!Convert.ToBoolean(down.Tag))
            {
                WaitCursor.ShowUntilIdle();
                down.SuspendLayout();
                foreach (ShellNew new2 in ShellNew.All)
                {
                    if (new2.CommandType != ShellNewCommand.Command)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem {
                            Text = new2.Name,
                            Tag = new2
                        };
                        item.Click += new EventHandler(this.tsmiNewFileItem_Click);
                        item.Paint += new PaintEventHandler(this.tsmiNewFileItem_Paint);
                        down.Items.Add(item);
                    }
                }
                down.ResumeLayout();
                down.Tag = true;
            }
        }

        private void OnApplyFilter(object sender, ApplyFilterEventArgs e)
        {
            VirtualFilePanel tag = (VirtualFilePanel) ((Control) sender).Tag;
            tag.Filter = e.Filter;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (OS.IsWinVista && Windows.AddClipboardFormatListener(base.Handle))
            {
                this.SetUIState(UIState.CheckClipboardUsingMessages, true);
            }
            else
            {
                this.SetUIState(UIState.CheckClipboardUsingTicks, true);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Application.Idle += new EventHandler(this.Event_ApplicationIdle);
        }

        private void OnRecovery(object sender, RecoveryEventArgs e)
        {
            this.SaveSessionSettings(RestartAndRecoveryManager.CanRestart, e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.SetUIState(UIState.ShowCrushLogNeeded, System.IO.File.Exists(Nomad.Trace.CrushLogPath));
            this.SetUIState(UIState.StartController, true);
        }

        private void PasteObject(IVirtualFolder folder, string baseName, IDataObject obj)
        {
            try
            {
                foreach (string str in PasteNewFileDialog.SupportedFormats)
                {
                    if (obj.GetDataPresent(str))
                    {
                        if ((str == DataFormats.Bitmap) || (str == DataFormats.Dib))
                        {
                            ImageCodecInfo defaultImageCodec = PasteNewFileDialog.DefaultImageCodec;
                            if (defaultImageCodec != null)
                            {
                                baseName = PasteNewFileDialog.ChangeImageExtension(baseName, defaultImageCodec);
                                this.PasteObject(folder, baseName, true, obj, DataFormats.Bitmap, null, defaultImageCodec);
                            }
                        }
                        else if (((str == DataFormats.UnicodeText) || (str == DataFormats.Text)) || (str == DataFormats.OemText))
                        {
                            baseName = Path.ChangeExtension(baseName, ".txt");
                            this.PasteObject(folder, baseName, true, obj, DataFormats.UnicodeText, Encoding.UTF8, null);
                        }
                        else
                        {
                            string defaultFormatExtension = PasteNewFileDialog.GetDefaultFormatExtension(str);
                            if (!string.IsNullOrEmpty(defaultFormatExtension))
                            {
                                baseName = Path.ChangeExtension(baseName, defaultFormatExtension);
                            }
                            this.PasteObject(folder, baseName, true, obj, str, null, null);
                        }
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                if (!VirtualItem.IsWarningIOException(exception) && !(exception is ExternalException))
                {
                    throw;
                }
                MessageDialog.ShowException(this, exception);
            }
        }

        private void PasteObject(IVirtualFolder folder, string newName, bool silent, IDataObject obj, string format, Encoding encoding, ImageCodecInfo codec)
        {
            ICreateVirtualFile file = folder as ICreateVirtualFile;
            if (file == null)
            {
                throw new WarningException(string.Format(Resources.sCannotCopyToBasicFolder, folder.FullName));
            }
            FileMode CreateMode = FileMode.Create;
            string path = newName;
            int num = 2;
            IChangeVirtualFile NewFile = file.CreateFile(newName);
            IPersistVirtualItem item = NewFile as IPersistVirtualItem;
            if ((item != null) && item.Exists)
            {
                if (silent)
                {
                    CreateMode = FileMode.CreateNew;
                    do
                    {
                        newName = string.Format(Settings.Default.AnotherLinkPattern, Path.GetFileNameWithoutExtension(path), Path.GetExtension(path), num++);
                        item = file.CreateFile(newName) as IPersistVirtualItem;
                    }
                    while ((item != null) && item.Exists);
                }
                else if (MessageDialog.Show(this, string.Format(Resources.sFileAlreadyExists, NewFile.FullName), Resources.sConfirmOverwriteFile, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) != MessageDialogResult.Yes)
                {
                    return;
                }
            }
            NewFile = item as IChangeVirtualFile;
            if (NewFile == null)
            {
                NewFile = file.CreateFile(newName);
            }
            Stream FileStream = null;
            if (this.ExecuteElevated(NewFile, delegate {
                WaitCursor.ShowUntilIdle();
                FileStream = NewFile.Open(CreateMode, FileAccess.Write, FileShare.Read, FileOptions.SequentialScan, 0L);
            }) && (FileStream != null))
            {
                using (FileStream)
                {
                    if ((format == DataFormats.Bitmap) || (format == DataFormats.Dib))
                    {
                        (obj.GetData(format, true) as Image).Save(FileStream, codec, null);
                    }
                    else
                    {
                        string data = obj.GetData(format, true) as string;
                        if (data != null)
                        {
                            if (format == DataFormats.OemText)
                            {
                                byte[] bytes = Encoding.Default.GetBytes(data);
                                data = Encoding.GetEncoding(Windows.GetOEMCP()).GetString(bytes);
                            }
                            else if (((format == DataFormats.Rtf) || (format == DataFormats.Html)) || (format == DataFormats.CommaSeparatedValue))
                            {
                                encoding = Encoding.Default;
                            }
                            using (TextWriter writer = new StreamWriter(FileStream, encoding))
                            {
                                writer.Write(data);
                            }
                        }
                    }
                }
            }
        }

        private void PreviewHotKey(object sender, PreviewHotKeyEventArgs e)
        {
            if (this.KeyMap.ContainsKey(e.HotKey))
            {
                e.Cancel = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.DiscardSpecialKey(keyData))
            {
                return false;
            }
            bool flag = base.ProcessCmdKey(ref msg, keyData) || this.actionManager.ProcessCmdKey(ref msg, keyData);
            if ((keyData & ~Keys.KeyCode) != Keys.None)
            {
                switch ((keyData & Keys.KeyCode))
                {
                    case Keys.None:
                    case Keys.LButton:
                    case Keys.RButton:
                    case Keys.MButton:
                    case Keys.XButton1:
                    case Keys.XButton2:
                        return flag;

                    case Keys.ShiftKey:
                    case Keys.ControlKey:
                    case Keys.Menu:
                        return flag;

                    case Keys.LWin:
                    case Keys.RWin:
                    case Keys.Apps:
                        return flag;

                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                    case Keys.LMenu:
                    case Keys.RMenu:
                        return flag;
                }
                if (!(flag || this.CheckUIState(UIState.HasBookmarks)))
                {
                    this.ReloadBookmarks(this.tsmiBookmarks.DropDown);
                    this.SetUIState(UIState.HasBookmarks, true);
                    flag = base.ProcessCmdKey(ref msg, keyData);
                }
                if (!(flag || this.CheckUIState(UIState.HasTools)))
                {
                    this.ReloadTools(this.tsmiTools.DropDown);
                    this.SetUIState(UIState.HasTools, true);
                    flag = base.ProcessCmdKey(ref msg, keyData);
                }
            }
            return flag;
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (!this.MainMenu.Visible)
            {
                foreach (ToolStripItem item in this.MainMenu.Items)
                {
                    if (item.Text.IndexOf("&" + charCode, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        using (new LockWindowRedraw(this, true))
                        {
                            this.MainMenu.Visible = true;
                        }
                    }
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        public void ReinitializeTabs()
        {
            if (!this.InitializeTabsFromCmdLineTab())
            {
                TwoPanelContainer tabContent = (TwoPanelContainer) this.CurrentTabContent.Clone();
                if (this.InitializeTabsFromCmdLineFolders(tabContent))
                {
                    Tab selectedTab = this.MainTabStrip.SelectedTab;
                    using (!this.MainTabStrip.Visible ? new LockWindowRedraw(this, true) : null)
                    {
                        this.MainTabStrip.SuspendLayout();
                        tabContent.BeginLayout();
                        Tab tab2 = this.AddNewTab(tabContent);
                        tabContent.EndLayout(true);
                        tab2.PerformClick();
                        this.MainTabStrip.ResumeLayout();
                    }
                    this.FPreviousTab = selectedTab;
                }
                else
                {
                    tabContent.Dispose();
                }
            }
        }

        private bool ReloadBookmarks(ToolStripDropDown bookmarksDropDown)
        {
            bool flag = System.IO.Directory.Exists(SettingsManager.SpecialFolders.Bookmarks);
            bookmarksDropDown.SuspendLayout();
            try
            {
                bookmarksDropDown.Items.Clear();
                bookmarksDropDown.Items.Add(this.tsmiBookmarkCurrentFolder);
                bookmarksDropDown.Items.Add(this.tsmiBookmarkCurrentTab);
                bookmarksDropDown.Items.Add(this.tsmiOrganizeBookmarks);
                if (flag)
                {
                    foreach (ToolStripItem item in this.FillBookmarkMenuList(SettingsManager.SpecialFolders.Bookmarks))
                    {
                        bookmarksDropDown.Items.Add(item);
                        IVirtualItem tag = item.Tag as IVirtualItem;
                        if (tag != null)
                        {
                            VirtualItemToolStripEvents.UpdateItemImage(item, tag);
                        }
                    }
                    if (this.tsmiBookmarks.DropDownItems.Count > 3)
                    {
                        bookmarksDropDown.Items.Insert(3, this.tssBookmarks1);
                    }
                }
            }
            finally
            {
                bookmarksDropDown.ResumeLayout();
            }
            if (flag)
            {
                this.InitializeBookmarksWatcher();
            }
            return flag;
        }

        private bool ReloadTools(ToolStripDropDown toolsDropDown)
        {
            bool flag = System.IO.Directory.Exists(SettingsManager.SpecialFolders.Tools);
            toolsDropDown.SuspendLayout();
            try
            {
                toolsDropDown.Items.Clear();
                toolsDropDown.Items.Add(this.tsmiOptions);
                if (flag)
                {
                    foreach (ToolStripItem item in this.CreateToolList(SettingsManager.SpecialFolders.Tools, typeof(ToolStripMenuItem), ToolStripItemDisplayStyle.ImageAndText))
                    {
                        toolsDropDown.Items.Add(item);
                    }
                }
                if (toolsDropDown.Items.Count > 1)
                {
                    toolsDropDown.Items.Insert(1, this.tssTools1);
                }
            }
            finally
            {
                toolsDropDown.ResumeLayout();
            }
            if (flag)
            {
                this.InitializeToolsWatcher();
            }
            return flag;
        }

        private bool RememberAllTabs(string tabsDir, Mutex syncMutex)
        {
            IResourceGuard guard = ResourceGuard.Create(syncMutex);
            try
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                if (System.IO.Directory.Exists(tabsDir))
                {
                    foreach (string str in System.IO.Directory.GetFiles(tabsDir, '*' + ".tab"))
                    {
                        dictionary.Add(str, 0);
                    }
                }
                System.IO.Directory.CreateDirectory(tabsDir);
                for (Tab tab = this.MainTabStrip.FirstTab; tab != null; tab = this.MainTabStrip.GetNextTab(tab, true, false))
                {
                    TwoPanelContainer container = (TwoPanelContainer) tab.TabStripPage.Controls[0];
                    str = Path.Combine(tabsDir, tab.TabIndex.ToString() + ".tab");
                    using (XmlWriter writer = XmlWriter.Create(str))
                    {
                        TwoPanelContainer.SerializeBookmark(writer, container.TabBookmark);
                    }
                    dictionary.Remove(str);
                }
                foreach (string str2 in dictionary.Keys)
                {
                    System.IO.File.Delete(str2);
                }
                return true;
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
            finally
            {
                if (guard != null)
                {
                    guard.Dispose();
                }
            }
            return false;
        }

        private void ResetBookmarks()
        {
            this.SetUIState(UIState.HasBookmarks, false);
            this.SetUpdateToolbarsNeeded("Bookmark_");
        }

        private void ResetKeyboardMap(IDictionary<Keys, IComponent> keyMap)
        {
            List<Keys> list;
            Dictionary<IComponent, List<Keys>> dictionary = new Dictionary<IComponent, List<Keys>>();
            foreach (KeyValuePair<Keys, IComponent> pair in keyMap)
            {
                if (!dictionary.TryGetValue(pair.Value, out list))
                {
                    list = new List<Keys>();
                    dictionary.Add(pair.Value, list);
                }
                list.Add(pair.Key);
            }
            foreach (IComponent component in this.Commands)
            {
                Action key = component as Action;
                if (key != null)
                {
                    key.Shortcuts = dictionary.TryGetValue(key, out list) ? list.ToArray() : null;
                }
                ToolStripMenuItem item = component as ToolStripMenuItem;
                if (item != null)
                {
                    item.ShortcutKeys = dictionary.TryGetValue(item, out list) ? list[0] : Keys.None;
                }
            }
        }

        private void ResetTools()
        {
            this.SetUIState(UIState.HasTools, false);
            this.SetUpdateToolbarsNeeded("Tool_");
        }

        private void RestoreAllTabs(string tabsDir, Mutex syncMutex)
        {
            if (System.IO.Directory.Exists(tabsDir))
            {
                bool flag = false;
                try
                {
                    List<string> list = new List<string>(System.IO.Directory.GetFiles(tabsDir, '*' + ".tab"));
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        int num2;
                        if (!int.TryParse(Path.GetFileNameWithoutExtension(list[i]), out num2))
                        {
                            list.RemoveAt(i);
                        }
                    }
                    list.Sort(delegate (string x, string y) {
                        return int.Parse(Path.GetFileNameWithoutExtension(x)) - int.Parse(Path.GetFileNameWithoutExtension(y));
                    });
                    if (list.Count != 0)
                    {
                        this.MainTabStrip.SuspendLayout();
                        this.MainPageSwitcher.SuspendLayout();
                        flag = true;
                        using (ResourceGuard.Create(syncMutex))
                        {
                            foreach (string str in list)
                            {
                                using (Stream stream = System.IO.File.OpenRead(str))
                                {
                                    this.AddNewTab(stream, false);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                }
                finally
                {
                    if (flag)
                    {
                        this.MainPageSwitcher.ResumeLayout(true);
                        this.MainTabStrip.ResumeLayout();
                    }
                }
            }
        }

        private Process RunAs(IWin32Window owner, IVirtualFileExecute file, string arguments, ExecuteAsUser runAs, string userName, SecureString password, CheckState runInThread)
        {
            bool flag = false;
            switch (runInThread)
            {
                case CheckState.Checked:
                    flag = true;
                    break;

                case CheckState.Indeterminate:
                    flag = ((PathHelper.GetPathType(file.FullName) & PathType.NetworkShare) > PathType.Unknown) && (Convert.ToInt64(file[3]) > 0x500000L);
                    break;
            }
            if (flag)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.RunAsCallback), new object[] { new FakeWin32Window(this), file, arguments, runAs, userName, password });
            }
            else
            {
                try
                {
                    if (!(file.CanExecuteEx && (!string.IsNullOrEmpty(arguments) || (runAs != ExecuteAsUser.CurrentUser))))
                    {
                        return file.Execute(owner);
                    }
                    return file.ExecuteEx(owner, arguments, runAs, userName, password);
                }
                catch (Exception exception)
                {
                    MessageDialog.ShowException(this, exception, Resources.sCaptionRunError, VirtualItem.IsWarningIOException(exception));
                }
            }
            return null;
        }

        private void RunAsCallback(object state)
        {
            object[] objArray = (object[]) state;
            IWin32Window owner = (IWin32Window) objArray[0];
            IVirtualFileExecute execute = (IVirtualFileExecute) objArray[1];
            string str = (string) objArray[2];
            ExecuteAsUser runAs = (ExecuteAsUser) objArray[3];
            string userName = (string) objArray[4];
            SecureString password = (SecureString) objArray[5];
            try
            {
                if (!(execute.CanExecuteEx && (!string.IsNullOrEmpty(str) || (runAs != ExecuteAsUser.CurrentUser))))
                {
                    execute.Execute(owner);
                }
                else
                {
                    execute.ExecuteEx(owner, str, runAs, userName, password);
                }
            }
            catch (Exception exception)
            {
                base.Invoke(new Action<IWin32Window, Exception, string, bool>(MessageDialog.ShowException), new object[] { this, exception, Resources.sCaptionRunError, VirtualItem.IsWarningIOException(exception) });
            }
        }

        private void SaveSessionSettings(bool restart, RecoveryEventArgs e)
        {
            bool flag = false;
            if (e != null)
            {
                e.RecoveryInProgress();
            }
            if (!((e != null) && e.UserCancelled))
            {
                this.PlacementSettings.SavePlacement();
                this.CurrentTabContent.SaveComponentSettings();
                Settings.Default.RestoreTabsOnStart = (this.MainPageSwitcher.Controls.Count > 1) && this.RememberAllTabs(SettingsManager.SpecialFolders.Tabs, Nomad.Trace.Current.Mutex);
                Program.SafeSaveSettings();
                flag = true;
            }
            if (e != null)
            {
                e.RecoveryInProgress();
            }
            if ((((e == null) || !e.UserCancelled) && restart) && RestartAndRecoveryManager.IsSupported)
            {
                string path = Path.Combine(OS.TempDirectory, StringHelper.GuidToCompactString(Guid.NewGuid()));
                System.IO.Directory.CreateDirectory(path);
                if (this.RememberAllTabs(path, null))
                {
                    RestartAndRecoveryManager.RestartCommandLine = string.Format("-new -recovery \"{0}\"", path);
                }
            }
            if (e != null)
            {
                e.RecoveryInProgress();
                e.Finished = flag;
            }
        }

        private void SearchFolderError(object sender, SearchErrorEventArgs e)
        {
            VirtualSearchFolder folder = (VirtualSearchFolder) sender;
            if (!(!Convert.ToBoolean(folder.Tag) && ConfirmationSettings.Default.SearchError))
            {
                e.Continue = true;
            }
            else if (base.InvokeRequired)
            {
                base.Invoke(new EventHandler<SearchErrorEventArgs>(this.SearchFolderError), new object[] { sender, e });
            }
            else
            {
                bool checkBoxChecked = false;
                MessageDialogResult skip = MessageDialog.Show(this, string.Format(Resources.sSearchError, e.Item.FullName, e.Error.Message), Resources.sError, Resources.sDoNotAskAgain, ref checkBoxChecked, new MessageDialogResult[] { MessageDialogResult.Skip, MessageDialogResult.SkipAll, MessageDialogResult.Cancel }, MessageBoxIcon.Hand);
                if (checkBoxChecked)
                {
                    ConfirmationSettings.Default.SearchError = false;
                }
                if (skip == MessageDialogResult.SkipAll)
                {
                    folder.Tag = true;
                    skip = MessageDialogResult.Skip;
                }
                e.Continue = skip == MessageDialogResult.Skip;
            }
        }

        private void SearchItem_Click(object sender, EventArgs e)
        {
            IVirtualFolder currentFolder = this.CurrentPanel.CurrentFolder;
            if (currentFolder != null)
            {
                VirtualSearchFolder folder2 = new VirtualSearchFolder(currentFolder, (IVirtualItemFilter) ((ToolStripItem) sender).Tag, SearchFolderOptions.ProcessSubfolders) {
                    Parent = currentFolder
                };
                this.CurrentPanel.CurrentFolder = folder2;
            }
        }

        private void SelectTabItem_Click(object sender, EventArgs e)
        {
            this.MainTabStrip.SelectedTab = ((ToolStripItem) sender).Tag as Tab;
        }

        private bool SetClipboardDataObject(object data)
        {
            try
            {
                Clipboard.SetDataObject(data, true, 2, 250);
                return true;
            }
            catch (ExternalException exception)
            {
                MessageDialog.ShowException(this, new ApplicationException(string.Format(Resources.sErrorPutToClipboard, exception.Message), exception), true);
                return false;
            }
        }

        private static void SetEnvironmentFolder(IVirtualFolder folder, string nameVar)
        {
            string path = null;
            if (folder is CustomFileSystemFolder)
            {
                path = folder.FullName;
                if (!PathHelper.IsRootPath(path))
                {
                    path = PathHelper.ExcludeTrailingDirectorySeparator(path);
                }
            }
            Environment.SetEnvironmentVariable(nameVar, path);
        }

        private static void SetEnvironmentSelection(IEnumerable<IVirtualItem> selection, string nameVar, string pathVar)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if (selection != null)
            {
                foreach (IVirtualItem item in selection)
                {
                    if (item is FileSystemItem)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append(' ');
                        }
                        builder.Append(EnquoteString(item.Name));
                        if (builder2.Length > 0)
                        {
                            builder2.Append(' ');
                        }
                        builder2.Append(EnquoteString(item.FullName));
                    }
                }
            }
            Environment.SetEnvironmentVariable(nameVar, (builder.Length > 0) ? builder.ToString() : null);
            Environment.SetEnvironmentVariable(pathVar, (builder2.Length > 0) ? builder2.ToString() : null);
        }

        private void SettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "VisualStyleState":
                    if (Application.VisualStyleState != Settings.Default.VisualStyleState)
                    {
                        Application.VisualStyleState = Settings.Default.VisualStyleState;
                    }
                    break;

                case "Theme":
                    this.InitializeToolStrip(true);
                    break;

                case "UICulture":
                {
                    CultureInfo uICulture = Settings.Default.UICulture;
                    if ((uICulture != null) && (Thread.CurrentThread.CurrentUICulture.Name != uICulture.Name))
                    {
                        Thread.CurrentThread.CurrentUICulture = uICulture;
                        PluralInfo.Current = new PluralInfo(Resources.PluralForms);
                        foreach (ToolStrip strip in this.Toolbars)
                        {
                            this.SetUpdateToolbarsNeeded(strip);
                        }
                        FormStringLocalizer.LocalizeForm(this);
                        ChangeVector.Increment(ChangeVector.Localization);
                    }
                    break;
                }
                case "IconOptions":
                {
                    bool flag = VirtualIcon.IconOptions != Settings.Default.IconOptions;
                    bool flag2 = ((VirtualIcon.IconOptions ^ Settings.Default.IconOptions) & IconOptions.ShowOverlayIcons) > 0;
                    VirtualIcon.IconOptions = Settings.Default.IconOptions;
                    if (flag2)
                    {
                        this.ResetBookmarks();
                    }
                    if (flag)
                    {
                        ChangeVector.Increment(ChangeVector.Icon);
                        this.CurrentTabContent.Invalidate(true);
                    }
                    break;
                }
                case "DelayedExtractMode":
                    VirtualIcon.DelayedExtractMode = Settings.Default.DelayedExtractMode;
                    break;

                case "ProcessFolderShortcuts":
                    FileSystemFolder.ProcessFolderShortcuts = Settings.Default.ProcessFolderShortcuts;
                    break;

                case "HideNotReadyDrives":
                    if (this.VolumeChangedHandler != null)
                    {
                        this.VolumeChangedHandler(null, EventArgs.Empty);
                    }
                    break;

                case "SlowVolumeAutoRefresh":
                    CustomFileSystemFolder.SlowVolumeAutoRefresh = Settings.Default.SlowVolumeAutoRefresh;
                    break;

                case "KeyboardMap":
                    this.ResetKeyboardMap(Settings.Default.DefaultKeyMap);
                    this.InitializeKeyboardMap(Settings.Default.KeyboardMap);
                    break;

                case "HiddenProperties":
                    InitializePropertyList();
                    break;

                case "ImageProvider":
                    ImageProvider.ResetDefaultImageProvider();
                    this.actResetVisualCache.Tag = true;
                    break;

                case "Highlighters":
                    ChangeVector.Increment(ChangeVector.Highlighters);
                    break;

                case "AlwaysShowTabStrip":
                    if (this.MainPageSwitcher.Controls.Count == 1)
                    {
                        this.MainTabStrip.Visible = Settings.Default.AlwaysShowTabStrip;
                    }
                    break;

                case "TabWidth":
                {
                    int tabWidth = Settings.Default.TabWidth;
                    this.MainTabStrip.SuspendLayout();
                    foreach (ToolStripItem item in this.MainTabStrip.Items)
                    {
                        Tab tab = item as Tab;
                        if (tab != null)
                        {
                            if (tabWidth < 10)
                            {
                                tab.AutoSize = true;
                            }
                            else
                            {
                                tab.FixedWidth = tabWidth;
                            }
                        }
                    }
                    this.MainTabStrip.ResumeLayout();
                    break;
                }
            }
        }

        private void SetToolbarButtonImage(ToolStripItem Item, IconLocation ImageLocation)
        {
            if (Path.IsPathRooted(ImageLocation.IconFileName))
            {
                bool flag = false;
                Size defaultSmallIconSize = ImageHelper.DefaultSmallIconSize;
                string str = Path.GetExtension(ImageLocation.IconFileName).ToLower();
                if ((str != null) && (((str == ".dll") || (str == ".exe")) || (str == ".ico")))
                {
                    flag = true;
                }
                else
                {
                    flag = ImageLocation.IconIndex > 0;
                }
                if (!flag)
                {
                    try
                    {
                        Image original = Image.FromFile(ImageLocation.IconFileName);
                        if (original.Size != defaultSmallIconSize)
                        {
                            original = new Bitmap(original, defaultSmallIconSize);
                        }
                        Item.Image = original;
                    }
                    catch
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    Item.Image = CustomImageProvider.LoadIconFromLocation(ImageLocation, defaultSmallIconSize);
                }
            }
            else
            {
                Item.Image = IconSet.GetImage(ImageLocation.IconFileName);
            }
            if (Item.Image == null)
            {
                Item.DisplayStyle = ToolStripItemDisplayStyle.Text;
            }
        }

        private void SetUIState(UIState mask, bool value)
        {
            if (value)
            {
                this.FUpdateUIMask |= mask;
            }
            else
            {
                this.FUpdateUIMask &= ~mask;
            }
        }

        private void SetUpdateDriveButtonsNeeded()
        {
            this.SetUIState(UIState.IsDriveButtonsDirty | UIState.IsToolbarsDirty, true);
        }

        private void SetUpdateToolbarsNeeded(string namePrefix)
        {
            foreach (ToolStrip strip in this.Toolbars)
            {
                foreach (ToolStripItem item in strip.Items)
                {
                    if (item.Name.StartsWith(namePrefix, StringComparison.Ordinal))
                    {
                        this.SetUpdateToolbarsNeeded(strip);
                        break;
                    }
                }
            }
        }

        private void SetUpdateToolbarsNeeded(ToolStrip toolBar)
        {
            toolBar.AllowMerge = true;
            this.SetUIState(UIState.IsToolbarsDirty, true);
        }

        private void ShowCrushLog()
        {
            if (this.CheckUIState(UIState.ShowCrushLogNeeded))
            {
                DateTime lastWriteTime;
                this.SetUIState(UIState.ShowCrushLogNeeded, false);
                string input = null;
                try
                {
                    using (ResourceGuard.Create(Nomad.Trace.Current.Mutex))
                    {
                        lastWriteTime = System.IO.File.GetLastWriteTime(Nomad.Trace.CrushLogPath);
                        using (TextReader reader = System.IO.File.OpenText(Nomad.Trace.CrushLogPath))
                        {
                            input = reader.ReadToEnd();
                        }
                        System.IO.File.Delete(Nomad.Trace.CrushLogPath);
                    }
                }
                catch (Exception exception)
                {
                    Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                    return;
                }
                Match match = new Regex(@"^.+?: (?<message>.+?)\s?(---> .+)?$", RegexOptions.Multiline).Match(input);
                using (ExceptionDialog dialog = new ExceptionDialog())
                {
                    base.AddOwnedForm(dialog);
                    dialog.ShowError(match.Success ? match.Groups["message"].Value : null, input, string.Format(Resources.sCrushNotification, lastWriteTime));
                }
            }
        }

        private bool ShowFileCopyDialog(IVirtualFolder destFolder, IEnumerable<IVirtualItem> items, bool move)
        {
            IVirtualFolder folder;
            CopyWorkerOptions copyOptions;
            IVirtualItemFilter filter;
            IRenameFilter renameFilter;
            IOverwriteRule[] defaultOverwriteRules;
            WaitCursor.ShowUntilIdle();
            using (FileSystemCopyDialog dialog = new FileSystemCopyDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.CurrentFolder = this.CurrentPanel.CurrentFolder;
                if (!((destFolder == null) || (destFolder is ArchiveFolder)))
                {
                    dialog.DestFolder = destFolder;
                }
                CopyWorkerOptions options2 = ~CopyWorkerOptions.DeleteSource;
                dialog.CopyOptions = (dialog.CopyOptions & ~options2) | (CopySettings.Default.DefaultCopyOptions & options2);
                dialog.RenameOrMove = move;
                if (!dialog.Execute(this, items))
                {
                    return false;
                }
                folder = dialog.DestFolder;
                copyOptions = dialog.CopyOptions;
                filter = dialog.Filter;
                renameFilter = dialog.RenameFilter;
                defaultOverwriteRules = dialog.DefaultOverwriteRules;
            }
            WaitCursor.ShowUntilIdle();
            try
            {
                if (!VirtualItemHelper.CanCreateInFolder(folder))
                {
                    throw new WarningException(string.Format(Resources.sCannotCopyToBasicFolder, folder.FullName));
                }
                IPersistVirtualItem item = folder as IPersistVirtualItem;
                if (!((item != null) && item.Exists))
                {
                    ICreateVirtualFolder folderRoot = VirtualItemHelper.GetFolderRoot(folder) as ICreateVirtualFolder;
                    if (!((folderRoot == null) || folder.Equals(folderRoot)))
                    {
                        folder = folderRoot.CreateFolder(folder.FullName);
                    }
                }
                DoStartCopy(folder, items, copyOptions, filter, renameFilter, defaultOverwriteRules);
                return true;
            }
            catch (Exception exception)
            {
                if (!VirtualItem.IsWarningIOException(exception))
                {
                    throw;
                }
                MessageDialog.ShowException(this, exception, true);
                return false;
            }
        }

        private bool ShowPackDialog(IVirtualItem destItem, IEnumerable<IVirtualItem> items, ArchiveFormatInfo format)
        {
            WaitCursor.ShowUntilIdle();
            using (FilePackDialog dialog = new FilePackDialog())
            {
                bool flag;
                PackWorker worker;
                base.AddOwnedForm(dialog);
                dialog.CurrentFolder = this.CurrentPanel.CurrentFolder;
                IChangeVirtualFile destFile = destItem as IChangeVirtualFile;
                if (destFile != null)
                {
                    flag = dialog.Execute(this, items, destFile, format);
                }
                else
                {
                    flag = dialog.Execute(this, items, destItem as IVirtualFolder);
                }
                if (!flag)
                {
                    return false;
                }
                IChangeVirtualFile destArchiveFile = dialog.DestArchiveFile as IChangeVirtualFile;
                if (destArchiveFile == null)
                {
                    throw new WarningException(string.Format("Cannot create archive. File '{0}' does not support modification.", dialog.DestArchiveFile.FullName));
                }
                SevenZipFormatInfo info = dialog.Format as SevenZipFormatInfo;
                if (info != null)
                {
                    worker = new PackWorker(info, destArchiveFile, dialog.DestSubFolder, items, dialog.Filter, dialog.UpdateMode, dialog.SevenZipProperties, dialog.Password);
                }
                else
                {
                    WcxFormatInfo info2 = dialog.Format as WcxFormatInfo;
                    if (info2 == null)
                    {
                        throw new InvalidOperationException();
                    }
                    worker = new PackWorker(info2, destArchiveFile, dialog.DestSubFolder, items, dialog.Filter, dialog.UpdateMode);
                }
                PackWorkerDialog dialog2 = PackWorkerDialog.ShowAsync(worker);
                worker.RunAsync(ThreadPriority.Normal);
                return true;
            }
        }

        private void ShowPasteDialog(IVirtualFolder destFolder, IDataObject dataObject, string baseName)
        {
            try
            {
                string dataFormat = null;
                string newName = null;
                Encoding textEncoding = null;
                ImageCodecInfo codec = null;
                using (PasteNewFileDialog dialog = new PasteNewFileDialog())
                {
                    base.AddOwnedForm(dialog);
                    dialog.NewName = baseName;
                    if (dialog.Execute(destFolder, dataObject.GetFormats()))
                    {
                        dataFormat = dialog.DataFormat;
                        newName = dialog.NewName;
                        textEncoding = dialog.TextEncoding;
                        codec = dialog.ImageCodec;
                    }
                }
                if (!string.IsNullOrEmpty(dataFormat))
                {
                    this.PasteObject(destFolder, newName, false, dataObject, dataFormat, textEncoding, codec);
                }
            }
            catch (Exception exception)
            {
                if (!VirtualItem.IsWarningIOException(exception) && !(exception is ExternalException))
                {
                    throw;
                }
                MessageDialog.ShowException(this, exception);
            }
        }

        private void ShowRunAsDialog(IVirtualFileExecute file, string arguments)
        {
            CheckState runInThread;
            ExecuteAsUser runAs;
            string userName;
            SecureString password;
            using (RunAsDialog dialog = new RunAsDialog())
            {
                base.AddOwnedForm(dialog);
                dialog.Arguments = (arguments != null) ? arguments : string.Empty;
                dialog.RunInThread = Settings.Default.RunInThread;
                if (!dialog.Execute(this, file))
                {
                    return;
                }
                runInThread = dialog.RunInThread;
                arguments = dialog.Arguments;
                runAs = dialog.RunAs;
                userName = dialog.UserName;
                password = dialog.Password;
            }
            WaitCursor.ShowUntilIdle();
            this.RunAs(this, file, arguments, runAs, userName, password, runInThread);
        }

        private IVirtualFolder SpecialFolderNeeded(ToolStripMenuItem item)
        {
            if (item.Tag != string.Empty)
            {
                IVirtualFolder tag = item.Tag as IVirtualFolder;
                if (tag != null)
                {
                    return tag;
                }
                try
                {
                    if (item.Tag is Environment.SpecialFolder)
                    {
                        tag = (IVirtualFolder) VirtualItem.FromFullName(Environment.GetFolderPath((Environment.SpecialFolder) item.Tag), VirtualItemType.Folder);
                    }
                    else if (item.Tag is CSIDL)
                    {
                        tag = VirtualItem.FromKnownShellFolder((CSIDL) item.Tag);
                    }
                    else if (item.Tag is Guid)
                    {
                        tag = VirtualItem.FromKnownShellFolder((Guid) item.Tag);
                    }
                    else
                    {
                        string str = item.Tag as string;
                        if (string.IsNullOrEmpty(str))
                        {
                            throw new InvalidOperationException();
                        }
                        tag = (IVirtualFolder) VirtualItem.FromFullName(str, VirtualItemType.Folder);
                    }
                    item.Tag = tag;
                    item.ToolTipText = tag.FullName;
                    return tag;
                }
                catch (Exception exception)
                {
                    item.Tag = string.Empty;
                    item.ToolTipText = exception.Message;
                }
            }
            return null;
        }

        private void StartExternalTool(string toolPath, string arguments)
        {
            Exception innerException = null;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    UseShellExecute = false
                };
                using (ShellLink link = new ShellLink(toolPath))
                {
                    startInfo.FileName = Environment.ExpandEnvironmentVariables(link.Path);
                    if (arguments == null)
                    {
                        startInfo.Arguments = Environment.ExpandEnvironmentVariables(link.Arguments);
                    }
                    else
                    {
                        startInfo.Arguments = arguments;
                    }
                    startInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(link.WorkingDirectory);
                    switch (link.WindowState)
                    {
                        case FormWindowState.Minimized:
                            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            goto Label_00B5;

                        case FormWindowState.Maximized:
                            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                            goto Label_00B5;
                    }
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                }
            Label_00B5:
                Process.Start(startInfo);
            }
            catch (Win32Exception exception2)
            {
                if (exception2.NativeErrorCode != 0x4c7)
                {
                    innerException = exception2;
                }
            }
            catch (SystemException exception3)
            {
                innerException = exception3;
            }
            if (innerException != null)
            {
                MessageDialog.ShowException(this, new WarningException(string.Format(Resources.sErrorStartingExternalTool, Path.GetFileNameWithoutExtension(toolPath), innerException.Message), innerException));
            }
        }

        private static Process StartItem(IChangeVirtualFile item, string programPath)
        {
            string fullName = item.FullName;
            if (fullName.IndexOf(' ') >= 0)
            {
                fullName = '"' + fullName.Replace("\"", "\"\"") + '"';
            }
            return Process.Start(programPath, fullName);
        }

        private void StartProcessWatch(Process watchProcess, IVirtualFile sourceItem, IChangeVirtualFile tempItem)
        {
            if (this.WatchProcessMap == null)
            {
                this.WatchProcessMap = new Dictionary<Process, WatchProcessInfo>();
            }
            lock (this.WatchProcessMap)
            {
                this.WatchProcessMap.Add(watchProcess, new WatchProcessInfo(sourceItem, tempItem.FullName));
            }
            watchProcess.EnableRaisingEvents = true;
            watchProcess.Exited += new EventHandler(this.WatchProcessExited);
        }

        private void TabBookmark_Click(object sender, EventArgs e)
        {
            GeneralTab tag = (GeneralTab) ((ToolStripItem) sender).Tag;
            TwoPanelContainer newTabContent = TwoPanelContainer.Create();
            newTabContent.FixMouseWheel = Settings.Default.FixMouseWheel;
            newTabContent.TabBookmark = tag;
            Tab selectedTab = this.MainTabStrip.SelectedTab;
            using (!this.MainTabStrip.Visible ? new LockWindowRedraw(this, true) : null)
            {
                this.MainTabStrip.SuspendLayout();
                newTabContent.BeginLayout();
                Tab tab3 = this.AddNewTab(newTabContent);
                newTabContent.EndLayout(true);
                tab3.PerformClick();
                this.MainTabStrip.ResumeLayout();
            }
            this.FPreviousTab = selectedTab;
        }

        private void TabButton_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Middle) && (this.MainPageSwitcher.Controls.Count > 1))
            {
                ((Tab) sender).TabStripPage.Dispose();
            }
        }

        private void TabButton_MouseEnter(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            Tab tag = sender as Tab;
            if (tag == null)
            {
                tag = item.Tag as Tab;
            }
            if (tag != null)
            {
                TwoPanelContainer container = (TwoPanelContainer) tag.TabStripPage.Controls[0];
                StringBuilder builder = new StringBuilder();
                if (container.OnePanelMode != TwoPanelContainer.SinglePanel.None)
                {
                    this.AppendPanelFolder(builder, container.CurrentPanel, true);
                }
                else
                {
                    this.AppendPanelFolder(builder, container.LeftPanel, container.LeftPanel == container.CurrentPanel);
                    builder.Append(" # ");
                    this.AppendPanelFolder(builder, container.RightPanel, container.RightPanel == container.CurrentPanel);
                }
                item.ToolTipText = builder.ToString();
            }
        }

        private void TabDropDown_Opening(object sender, CancelEventArgs e)
        {
            ToolStripDropDown down = (ToolStripDropDown) sender;
            Dictionary<int, Keys> dictionary = new Dictionary<int, Keys>();
            foreach (Action action in this.catTab.Components)
            {
                if (action.Tag is int)
                {
                    dictionary.Add((int) action.Tag, action.ShortcutKeys);
                }
            }
            down.SuspendLayout();
            try
            {
                IDisposableContainer container = new DisposableContainer();
                int num = 0;
                foreach (ToolStripItem item in this.MainTabStrip.Items)
                {
                    Tab tab = item as Tab;
                    if (tab != null)
                    {
                        Keys keys;
                        ToolStripMenuItem item2 = new ToolStripMenuItem {
                            Text = item.Text,
                            Tag = tab
                        };
                        item2.Click += new EventHandler(this.SelectTabItem_Click);
                        item2.MouseEnter += new EventHandler(this.TabButton_MouseEnter);
                        if (dictionary.TryGetValue(++num, out keys))
                        {
                            item2.ShortcutKeys = keys;
                        }
                        else if (tab.IsLastTab)
                        {
                            item2.ShortcutKeys = this.actMoveToLastTab.ShortcutKeys;
                        }
                        if (tab.Checked)
                        {
                            Font font = new Font(item2.Font, FontStyle.Bold);
                            container.Add(font);
                            item2.Font = font;
                        }
                        TwoPanelContainer container2 = tab.TabStripPage.Controls[0] as TwoPanelContainer;
                        if (container2.OnePanelMode != TwoPanelContainer.SinglePanel.None)
                        {
                            item2.Image = IconSet.GetImage(this.actOnePanel.Name);
                        }
                        else if (container2.Orientation == Orientation.Vertical)
                        {
                            item2.Image = IconSet.GetImage(this.actTwoVerticalPanel.Name);
                        }
                        else
                        {
                            item2.Image = IconSet.GetImage(this.actTwoHorizontalPanel.Name);
                        }
                        down.Items.Add(item2);
                        container.Add(item2);
                    }
                }
                down.Tag = container;
            }
            finally
            {
                down.ResumeLayout();
            }
        }

        private void ThumbnailSizeMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle tag = (Rectangle) ((ToolStripItem) sender).Tag;
            this.CurrentPanel.ThumbnailSize = new Size(tag.Left, tag.Top);
            this.CurrentPanel.ThumbnailSpacing = tag.Size;
        }

        private void Toolbar_DragDrop(object sender, DragEventArgs e)
        {
            ToolStrip toolbar = (ToolStrip) sender;
            this.Toolbar_DragLeave(sender, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data))
            {
                ToolStripItem itemAt = toolbar.GetItemAt(toolbar.PointToClient(new Point(e.X, e.Y)));
                if (itemAt != null)
                {
                    if (itemAt.Name.StartsWith("Drive_", StringComparison.Ordinal))
                    {
                        this.CurrentPanel_DragDropOnItem(sender, new VirtualItemDragEventArg((IVirtualFolder) itemAt.Tag, e));
                    }
                    else
                    {
                        IEnumerable<IVirtualItem> dataObjectItems = VirtualClipboardItem.GetDataObjectItems(e.Data);
                        Action action = this.actionManager.GetAction(itemAt);
                        if (action != null)
                        {
                            action.Execute(itemAt, dataObjectItems);
                        }
                        else if (itemAt.Name.StartsWith("Tool_", StringComparison.Ordinal))
                        {
                            StringBuilder builder = new StringBuilder();
                            foreach (IVirtualItem item2 in dataObjectItems)
                            {
                                if (item2 is FileSystemItem)
                                {
                                    builder.Append(EnquoteString(item2.FullName));
                                    builder.Append(' ');
                                }
                            }
                            if (builder.Length > 1)
                            {
                                builder.Length--;
                                SetEnvironmentFolder(this.CurrentPanel.CurrentFolder, "curdir");
                                SetEnvironmentFolder(this.FarPanel.CurrentFolder, "fardir");
                                this.StartExternalTool((string) itemAt.GetTag(1), builder.ToString());
                            }
                        }
                    }
                }
                else
                {
                    FileSystemFile onlyOneItem = GetOnlyOneItem<FileSystemFile>(VirtualClipboardItem.GetDataObjectItems(e.Data), null);
                    if ((onlyOneItem != null) && onlyOneItem.CanExecuteEx)
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(onlyOneItem.Name);
                        string str2 = fileNameWithoutExtension + ".lnk";
                        string path = Path.Combine(SettingsManager.SpecialFolders.Tools, str2);
                        bool flag = false;
                        for (int i = toolbar.Items.Count - 1; (i >= 0) && !flag; i--)
                        {
                            string name = toolbar.Items[i].Name;
                            flag = name.StartsWith("Tool_", StringComparison.Ordinal) && name.EndsWith("_" + fileNameWithoutExtension, StringComparison.OrdinalIgnoreCase);
                            if (name.StartsWith("Tool_All_", StringComparison.Ordinal))
                            {
                                flag = true;
                            }
                        }
                        WatcherChangeTypes created = WatcherChangeTypes.Created;
                        if (System.IO.File.Exists(path))
                        {
                            created = (MessageDialog.Show(this, string.Format(Resources.sToolAlreadyExists, fileNameWithoutExtension), Resources.sConfirmOverwriteTool, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question) == MessageDialogResult.Yes) ? WatcherChangeTypes.Changed : ((WatcherChangeTypes) 0);
                        }
                        if (created != 0)
                        {
                            using (ShellLink link = new ShellLink())
                            {
                                link.Path = onlyOneItem.FullName;
                                try
                                {
                                    link.Save(path);
                                    LocalFileSystemCreator.RaiseFileChangedEvent(created, path);
                                }
                                catch (SystemException exception)
                                {
                                    MessageDialog.ShowException(this, exception, true);
                                    created = 0;
                                }
                            }
                            if (!(flag || (created == 0)))
                            {
                                toolbar.Text = this.GetToolbarCommands(toolbar) + Environment.NewLine + @"tools\" + str2;
                            }
                        }
                    }
                }
            }
        }

        private void Toolbar_DragEnter(object sender, DragEventArgs e)
        {
            ToolStrip owner = (ToolStrip) sender;
            DragImage.DragEnter(owner, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data))
            {
                bool dataPresent = e.Data.GetDataPresent(DataFormats.FileDrop);
                this.CommandDropMap = new Dictionary<ToolStripItem, DragDropEffects>();
                IEnumerable<IVirtualItem> dataObjectItems = VirtualClipboardItem.GetDataObjectItems(e.Data);
                foreach (ToolStripItem item in owner.Items)
                {
                    Action action = this.actionManager.GetAction(item);
                    if ((action != null) && this.IsDroppableAction(action))
                    {
                        bool flag2 = (action.Update(item, dataObjectItems) & ActionState.Enabled) > ActionState.None;
                        this.CommandDropMap.Add(item, flag2 ? DragDropEffects.Copy : DragDropEffects.None);
                    }
                    else if (item.Name.StartsWith("Tool_", StringComparison.Ordinal))
                    {
                        this.CommandDropMap.Add(item, dataPresent ? DragDropEffects.Copy : DragDropEffects.None);
                    }
                    else if (item.Name.StartsWith("Drive_", StringComparison.Ordinal))
                    {
                        IVirtualFolder tag = (IVirtualFolder) item.Tag;
                        if (!VirtualItemHelper.CanCreateInFolder(tag))
                        {
                            this.CommandDropMap.Add(item, DragDropEffects.None);
                        }
                        else if (VirtualItemHelper.CanMoveTo(dataObjectItems, tag) == CanMoveResult.All)
                        {
                            this.CommandDropMap.Add(item, DragDropEffects.Move);
                        }
                        else
                        {
                            this.CommandDropMap.Add(item, DragDropEffects.Copy);
                        }
                    }
                    else
                    {
                        this.CommandDropMap.Add(item, DragDropEffects.None);
                    }
                }
                FileSystemFile onlyOneItem = GetOnlyOneItem<FileSystemFile>(dataObjectItems, null);
                if ((onlyOneItem != null) && onlyOneItem.CanExecuteEx)
                {
                    this.CommandDropMap.Add(this.tsmiTools, DragDropEffects.Link);
                }
            }
        }

        private void Toolbar_DragLeave(object sender, EventArgs e)
        {
            ToolStrip owner = (ToolStrip) sender;
            DragImage.DragLeave(owner);
            owner.UnselectAll();
            this.CommandDropMap = null;
        }

        private void Toolbar_DragOver(object sender, DragEventArgs e)
        {
            ToolStrip owner = (ToolStrip) sender;
            DragImage.DragOver(owner, e);
            if (VirtualClipboardItem.DataObjectContainItems(e.Data))
            {
                DragDropEffects none;
                ToolStripItem itemAt = owner.GetItemAt(owner.PointToClient(new Point(e.X, e.Y)));
                if (itemAt == null)
                {
                    itemAt = this.tsmiTools;
                }
                if (!((this.CommandDropMap != null) && this.CommandDropMap.TryGetValue(itemAt, out none)))
                {
                    none = DragDropEffects.None;
                }
                e.Effect = none;
                if ((e.Effect != DragDropEffects.None) && itemAt.Name.StartsWith("Drive_", StringComparison.Ordinal))
                {
                    switch ((e.KeyState & 0x2c))
                    {
                        case 4:
                            e.Effect = DragDropEffects.Move;
                            break;

                        case 8:
                            e.Effect = DragDropEffects.Copy;
                            break;

                        case 0x20:
                            e.Effect = DragDropEffects.Link;
                            break;
                    }
                }
                bool selected = false;
                switch (e.Effect)
                {
                    case DragDropEffects.Copy:
                    case DragDropEffects.Move:
                        selected = !itemAt.Selected;
                        break;

                    case DragDropEffects.Link:
                        selected = (itemAt != this.tsmiTools) && !itemAt.Selected;
                        break;

                    default:
                        for (int i = owner.Items.Count - 1; (i >= 0) && !selected; i--)
                        {
                            selected = owner.Items[i].Selected;
                        }
                        break;
                }
                if (selected)
                {
                    DragImage.Hide();
                    switch (e.Effect)
                    {
                        case DragDropEffects.Copy:
                        case DragDropEffects.Move:
                        case DragDropEffects.Link:
                            itemAt.Select();
                            itemAt.Invalidate();
                            break;

                        default:
                            foreach (ToolStripItem item2 in owner.Items)
                            {
                                if (item2.Selected)
                                {
                                    item2.Invalidate();
                                }
                            }
                            owner.UnselectAll();
                            break;
                    }
                    owner.Update();
                    DragImage.Show();
                }
            }
        }

        private void Toolbar_Paint(object sender, PaintEventArgs e)
        {
            if (this.ControlModifierKeys != Keys.None)
            {
                ToolStrip strip = (ToolStrip) sender;
                Dictionary<ToolStripItem, Keys> dictionary = new Dictionary<ToolStripItem, Keys>();
                foreach (ToolStripItem item in strip.Items)
                {
                    if (item.Visible && !item.IsOnOverflow)
                    {
                        Action action = this.actionManager.GetAction(item);
                        Keys keys = (action == null) ? Keys.None : (action.ShortcutKeys & ~Keys.KeyCode);
                        if ((keys != Keys.None) && (keys == this.ControlModifierKeys))
                        {
                            dictionary.Add(item, action.ShortcutKeys & Keys.KeyCode);
                        }
                    }
                }
                if (dictionary.Count > 0)
                {
                    base.BeginInvoke(new Action<ToolStrip, Dictionary<ToolStripItem, Keys>>(this.DrawKeyboardCues), new object[] { strip, dictionary });
                }
            }
        }

        private void Toolbar_TextChanged(object sender, EventArgs e)
        {
            ToolStrip toolBar = (ToolStrip) sender;
            if (toolBar.Visible && base.IsHandleCreated)
            {
                this.SetUpdateToolbarsNeeded(toolBar);
            }
            else
            {
                toolBar.Items.Clear();
            }
        }

        private void Toolbar_VisibleChanged(object sender, EventArgs e)
        {
            ToolStrip toolbar = (ToolStrip) sender;
            if (toolbar.Visible && (toolbar.Items.Count == 0))
            {
                this.InitializeToolbar(toolbar, toolbar.Text);
            }
        }

        private void ToolbarButton_MouseEnter(object sender, EventArgs e)
        {
            ToolStripItem component = (ToolStripItem) sender;
            Action action = this.actionManager.GetAction(component);
            if (action != null)
            {
                component.ToolTipText = CreateToolTipText(component.Text, action.ShortcutKeys);
            }
        }

        private void ToolbarButton_Paint(object sender, PaintEventArgs e)
        {
            if (this.tsmiRemoveToolbarButton.Visible && (this.tsmiRemoveToolbarButton.Tag == sender))
            {
                VirtualItemToolStripEvents.PaintBorder(sender, e);
            }
        }

        private void ToolbarButton_TextChanged(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            if ((item.Text.IndexOf('&') >= 0) || item.Text.EndsWith("...", StringComparison.Ordinal))
            {
                StringBuilder builder = new StringBuilder(item.Text);
                builder.Replace("&", "");
                builder.Replace("...", "", builder.Length - 3, 3);
                item.Text = builder.ToString();
            }
        }

        private void ToolbarItem_Click(object sender, EventArgs e)
        {
            ToolStrip tag = (ToolStrip) ((ToolStripItem) sender).Tag;
            using (new LockWindowRedraw(this, true))
            {
                tag.Visible = !tag.Visible;
            }
        }

        private void ToolbarItem_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            ToolStrip tag = (ToolStrip) item.Tag;
            item.Enabled = (tag.Items.Count > 0) || !string.IsNullOrEmpty(tag.Text);
            item.Checked = tag.Visible;
        }

        private void ToolsFolderChanged(object sender, FileSystemEventArgs e)
        {
            this.ResetTools();
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.actBringToFront.Execute();
            }
        }

        private void tsmiBookmarkFolder_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            item.DropDown.SuspendLayout();
            try
            {
                item.DropDownItems.Clear();
                foreach (ToolStripItem item2 in this.FillBookmarkMenuList(item.ShortcutKeyDisplayString))
                {
                    item.DropDownItems.Add(item2);
                }
                if (item.DropDownItems.Count == 0)
                {
                    item.DropDownItems.Add(this.tsmiEmpty);
                }
                else
                {
                    Container container = new Container();
                    item.DropDown.Tag = container;
                    foreach (ToolStripMenuItem item3 in item.DropDownItems)
                    {
                        item3.ShowShortcutKeys = false;
                        container.Add(item3);
                    }
                }
            }
            finally
            {
                item.DropDown.ResumeLayout();
            }
        }

        private void tsmiChangeButtonImage_Click(object sender, EventArgs e)
        {
            ToolStripItem tag = (ToolStripItem) this.tsmiRemoveToolbarButton.Tag;
            Match match = this.ToolbarCommandRegex.Match(tag.Name);
            if (match.Success)
            {
                string name = match.Groups["CommandName"].Value;
                IconLocation imageLocation = (IconLocation) tag.GetTag(2);
                bool flag = false;
                WaitCursor.ShowUntilIdle();
                using (SelectButtonImage image = new SelectButtonImage())
                {
                    base.AddOwnedForm(image);
                    image.DefaultImageName = IconSet.ResolveName(name);
                    if (imageLocation != null)
                    {
                        image.ImageLocation = imageLocation;
                    }
                    if (image.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    imageLocation = image.ImageLocation;
                    if (((imageLocation != null) && (imageLocation.IconIndex < 0)) && (imageLocation.IconFileName == image.DefaultImageName))
                    {
                        flag = true;
                        imageLocation = null;
                    }
                }
                tag.SetTag(2, imageLocation);
                if (imageLocation != null)
                {
                    this.SetToolbarButtonImage(tag, imageLocation);
                    if (tag.DisplayStyle == ToolStripItemDisplayStyle.Text)
                    {
                        tag.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    }
                }
                else
                {
                    tag.Image = IconSet.GetImage(name);
                    if (!flag)
                    {
                        tag.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    }
                }
                foreach (ToolStripItem item2 in this.GetRelatedToolbarItems(tag, "Tool_All_"))
                {
                    item2.Name = "Tool_" + item2.Name.Substring("Tool_All_".Length);
                }
                ToolStrip owner = tag.Owner;
                owner.TextChanged -= new EventHandler(this.Toolbar_TextChanged);
                owner.Text = this.GetToolbarCommands(owner);
                owner.TextChanged += new EventHandler(this.Toolbar_TextChanged);
            }
        }

        private void tsmiExternalTool_Click(object sender, EventArgs e)
        {
            bool flag;
            ToolStripItem item = (ToolStripItem) sender;
            string tag = (string) item.GetTag(1);
            string str2 = null;
            try
            {
                using (ShellLink link = new ShellLink(tag))
                {
                    flag = link.Arguments.IndexOf(SettingsManager.EnvironmentVariables.GetVarName("user"), StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            catch
            {
                flag = false;
            }
            if (!flag || InputDialog.Input(this, string.Format(Resources.sAskForCommandUserValue, Path.GetFileNameWithoutExtension(tag)), Resources.sCaptionCommandUserValue, ref str2))
            {
                SetEnvironmentFolder(this.CurrentPanel.CurrentFolder, "curdir");
                FileSystemItem focusedItem = this.CurrentPanel.FocusedItem as FileSystemItem;
                Environment.SetEnvironmentVariable("curitempath", (focusedItem != null) ? EnquoteString(focusedItem.FullName) : null);
                Environment.SetEnvironmentVariable("curitemname", (focusedItem != null) ? EnquoteString(focusedItem.Name) : null);
                SetEnvironmentSelection(this.CurrentPanel.SelectionOrFocused, "curselname", "curselpath");
                SetEnvironmentFolder(this.FarPanel.CurrentFolder, "fardir");
                focusedItem = this.FarPanel.FocusedItem as FileSystemItem;
                Environment.SetEnvironmentVariable("faritempath", (focusedItem != null) ? EnquoteString(focusedItem.FullName) : null);
                Environment.SetEnvironmentVariable("faritemname", (focusedItem != null) ? EnquoteString(focusedItem.Name) : null);
                SetEnvironmentSelection(this.FarPanel.SelectionOrFocused, "farselname", "farselpath");
                Environment.SetEnvironmentVariable("user", str2);
                bool flag2 = this.CheckUIState(UIState.DirectToolStart) || Settings.Default.DirectToolStart;
                if (!flag2)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(tag);
                    if (OS.IsWinVista)
                    {
                        ToolStripMenuItem item3 = item as ToolStripMenuItem;
                        if ((Control.ModifierKeys == (Keys.Alt | Keys.Control | Keys.Shift)) && ((item3 == null) || ((item3.ShortcutKeys & ~Keys.KeyCode) != (Keys.Alt | Keys.Control | Keys.Shift))))
                        {
                            startInfo.Verb = "runas";
                        }
                    }
                    try
                    {
                        Process.Start(startInfo);
                    }
                    catch (Win32Exception exception)
                    {
                        int nativeErrorCode = exception.NativeErrorCode;
                        if (nativeErrorCode == 0x483)
                        {
                            this.SetUIState(UIState.DirectToolStart, true);
                        }
                        else if (nativeErrorCode != 0x4c7)
                        {
                            MessageDialog.ShowException(this, new WarningException(string.Format(Resources.sErrorStartingExternalTool, Path.GetFileNameWithoutExtension(tag), exception.Message), exception));
                        }
                    }
                }
                if (flag2)
                {
                    this.StartExternalTool(tag, null);
                }
            }
        }

        private void tsmiExternalTool_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            if (Settings.Default.IsShowIcons)
            {
                if (item.Image == null)
                {
                    VirtualItemToolStripEvents.ChangeImage(item, ImageProvider.Default.GetFileIcon((string) item.GetTag(1), ImageHelper.DefaultSmallIconSize));
                }
            }
            else
            {
                item.Image = null;
            }
        }

        private void tsmiFolderBarHidden_Paint(object sender, PaintEventArgs e)
        {
            ((ToolStripMenuItem) sender).Checked = !this.CurrentPanel.FolderBarVisible;
        }

        private void tsmiFolderBarOrientation_Click(object sender, EventArgs e)
        {
            this.CurrentPanel.FolderBarOrientation = (Orientation) ((ToolStripMenuItem) sender).Tag;
            this.CurrentPanel.FolderBarVisible = true;
        }

        private void tsmiFolderBarOrientation_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            item.Checked = this.CurrentPanel.FolderBarVisible && (this.CurrentPanel.FolderBarOrientation == ((Orientation) item.Tag));
        }

        private void tsmiHistory_Click(object sender, EventArgs e)
        {
            int mergeIndex = ((ToolStripItem) sender).MergeIndex;
            IVirtualFolder folder = this.CurrentPanel.History.Move(mergeIndex);
            if (!this.CurrentPanel.SetCurrentFolder(folder, false))
            {
                this.CurrentPanel.History.Move(-mergeIndex);
            }
        }

        private void tsmiJustifyToolbar_Click(object sender, EventArgs e)
        {
            ToolStrip sourceControl = this.cmsToolbar.SourceControl as ToolStrip;
            ToolbarSettings tag = (ToolbarSettings) sourceControl.Tag;
            tag.JustifyLabels = !tag.JustifyLabels;
            this.JustifyToolbar(sourceControl, tag.JustifyLabels);
        }

        private void tsmiJustifyToolbar_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            ToolStrip sourceControl = this.cmsToolbar.SourceControl as ToolStrip;
            item.Checked = ((ToolbarSettings) sourceControl.Tag).JustifyLabels;
        }

        private void tsmiMainMenuVisible_Click(object sender, EventArgs e)
        {
            using (new LockWindowRedraw(this, true))
            {
                Settings.Default.MainMenuVisible = !Settings.Default.MainMenuVisible;
            }
        }

        private void tsmiMainMenuVisible_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            item.Checked = Settings.Default.MainMenuVisible;
        }

        private void tsmiNewFileItem_Click(object sender, EventArgs e)
        {
            ShellNew tag = (ShellNew) ((ToolStripItem) sender).Tag;
            this.MakeNewFile(this.CurrentPanel.CurrentFolder, tag);
        }

        private void tsmiNewFileItem_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            if (Settings.Default.IsShowIcons)
            {
                if (item.Image == null)
                {
                    VirtualItemToolStripEvents.ChangeImage(item, ImageProvider.Default.GetDefaultFileIcon("test" + ((ShellNew) item.Tag).Extension, ImageHelper.DefaultSmallIconSize));
                }
            }
            else
            {
                item.Image = null;
            }
            item.Enabled = this.CurrentPanel.CurrentFolder is ICreateVirtualFile;
        }

        private void tsmiRemoveToolbarButton_Click(object sender, EventArgs e)
        {
            ToolStripItem tag = (ToolStripItem) this.tsmiRemoveToolbarButton.Tag;
            ToolStrip owner = tag.Owner;
            IContainer container = new Container();
            container.Add(tag);
            foreach (ToolStripItem item2 in this.GetRelatedToolbarItems(tag, "Tool_All_"))
            {
                item2.Name = "Tool_" + item2.Name.Substring("Tool_All_".Length);
            }
            foreach (ToolStripItem item2 in this.GetRelatedToolbarItems(tag, "Bookmark_All_"))
            {
                container.Add(item2);
            }
            foreach (ToolStripItem item2 in this.GetRelatedToolbarItems(tag, "Drive_All_"))
            {
                container.Add(item2);
            }
            owner.SuspendLayout();
            container.Dispose();
            CleanupToolbar(owner);
            if (owner.Items.Count == 0)
            {
                owner.Visible = false;
            }
            else if (owner.LayoutStyle == ToolStripLayoutStyle.Table)
            {
                this.JustifyToolbar(owner);
            }
            owner.ResumeLayout();
            owner.TextChanged -= new EventHandler(this.Toolbar_TextChanged);
            owner.Text = this.GetToolbarCommands(owner);
            owner.TextChanged += new EventHandler(this.Toolbar_TextChanged);
        }

        private void tsmiSort_DropDownOpening(object sender, EventArgs e)
        {
            VirtualPropertySet availableProperties = this.CurrentPanel.AvailableProperties;
            foreach (ToolStripItem item in ((ToolStripMenuItem) sender).DropDownItems)
            {
                Action action = this.actionManager.GetAction(item);
                if (((item is ToolStripMenuItem) && (action != null)) && (action.Tag != null))
                {
                    int tag = (int) action.Tag;
                    if (!availableProperties[tag])
                    {
                        item.ForeColor = SystemColors.GrayText;
                    }
                    else
                    {
                        item.ResetForeColor();
                    }
                }
            }
        }

        private void tsmiSpecialFolder_Click(object sender, EventArgs e)
        {
            IVirtualFolder folder = this.SpecialFolderNeeded((ToolStripMenuItem) sender);
            if (folder != null)
            {
                VirtualFilePanel currentPanel = this.CurrentPanel;
                if ((Control.ModifierKeys == Keys.Shift) && (this.CurrentTabContent.OnePanelMode == TwoPanelContainer.SinglePanel.None))
                {
                    currentPanel = this.FarPanel;
                }
                currentPanel.CurrentFolder = folder;
            }
        }

        private void tsmiSpecialFolder_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            IVirtualFolder folder = this.SpecialFolderNeeded(item);
            if (Settings.Default.IsShowIcons)
            {
                if (item.Image == null)
                {
                    Size defaultSmallIconSize = ImageHelper.DefaultSmallIconSize;
                    Image itemImage = null;
                    if (sender == this.tsmiFolderDesktop)
                    {
                        itemImage = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Desktop, defaultSmallIconSize);
                    }
                    else if (sender == this.tsmiFolderFavorites)
                    {
                        itemImage = ImageProvider.Default.GetDefaultIcon(DefaultIcon.Favorites, defaultSmallIconSize);
                    }
                    else if (sender == this.tsmiFolderMyDocuments)
                    {
                        itemImage = ImageProvider.Default.GetDefaultIcon(DefaultIcon.MyDocuments, defaultSmallIconSize);
                    }
                    else if (sender == this.tsmiFolderMyPictures)
                    {
                        itemImage = ImageProvider.Default.GetDefaultIcon(DefaultIcon.MyPictures, defaultSmallIconSize);
                    }
                    else if (sender == this.tsmiFolderMyMusic)
                    {
                        itemImage = ImageProvider.Default.GetDefaultIcon(DefaultIcon.MyMusic, defaultSmallIconSize);
                    }
                    if (itemImage == null)
                    {
                        if (folder != null)
                        {
                            itemImage = VirtualIcon.GetIcon(folder, defaultSmallIconSize);
                        }
                        else
                        {
                            itemImage = ImageProvider.Default.GetDefaultIcon(DefaultIcon.UnknownFile, defaultSmallIconSize);
                        }
                    }
                    VirtualItemToolStripEvents.ChangeImage(item, itemImage);
                }
            }
            else
            {
                item.Image = null;
            }
            item.Enabled = (folder != null) && !this.CurrentPanel.IsFolderLocked;
        }

        private void tsmiToolbarButtonDisplayStyle_Click(object sender, EventArgs e)
        {
            ToolStripItem tag = (ToolStripItem) this.tsmiRemoveToolbarButton.Tag;
            ToolStripItemDisplayStyle style = (ToolStripItemDisplayStyle) ((ToolStripItem) sender).Tag;
            if (tag.DisplayStyle != style)
            {
                tag.DisplayStyle = style;
                foreach (ToolStripItem item2 in this.GetRelatedToolbarItems(tag, "Tool_All_"))
                {
                    item2.Name = "Tool_" + item2.Name.Substring("Tool_All_".Length);
                }
                ToolStrip owner = tag.Owner;
                if (owner.LayoutStyle == ToolStripLayoutStyle.Table)
                {
                    this.JustifyToolbar(owner);
                }
                owner.TextChanged -= new EventHandler(this.Toolbar_TextChanged);
                owner.Text = this.GetToolbarCommands(owner);
                owner.TextChanged += new EventHandler(this.Toolbar_TextChanged);
            }
        }

        private void tsmiToolbarButtonDisplayStyle_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            ToolStripItem tag = (ToolStripItem) this.tsmiRemoveToolbarButton.Tag;
            item.Checked = tag.DisplayStyle == ((ToolStripItemDisplayStyle) item.Tag);
        }

        private void tsmiToolbarMoveToTop_Click(object sender, EventArgs e)
        {
            ToolStrip sourceControl = this.cmsToolbar.SourceControl as ToolStrip;
            sourceControl.Dock = (DockStyle) ((ToolStripItem) sender).Tag;
        }

        private void tsmiTools_DropDownOpening(object sender, EventArgs e)
        {
            if (!this.CheckUIState(UIState.HasTools))
            {
                this.SetUIState(UIState.HasTools, this.ReloadTools(this.tsmiTools.DropDown));
            }
        }

        private void tsmiView_DropDownClosed(object sender, EventArgs e)
        {
            this.tsmiColumns.DropDown = (ToolStripDropDown) this.tsmiView.Tag;
        }

        private void tsmiView_DropDownOpening(object sender, EventArgs e)
        {
            this.tsmiViewAsThumbnail.DropDown = (this.CurrentPanel.View == PanelView.Thumbnail) ? this.cmsMenuViewAs : null;
            this.tsmiViewAsList.DropDown = (this.CurrentPanel.View == PanelView.List) ? this.cmsMenuViewAs : null;
            this.tsmiColumns.DropDown = this.CurrentPanel.cmsColumns;
        }

        public void UpdateCulture()
        {
            if (OS.IsElevated)
            {
                this.Text = string.Format("{0} ({1})", "Nomad.NET", Resources.sRoleAdministrator);
            }
        }

        private void UpdateToolbars()
        {
            foreach (ToolStrip strip in this.Toolbars)
            {
                if (strip.AllowMerge)
                {
                    if (strip.Visible)
                    {
                        this.InitializeToolbar(strip, strip.Text);
                    }
                    else
                    {
                        strip.Items.Clear();
                    }
                    strip.AllowMerge = false;
                }
                IVirtualFolder currentFolder = this.CurrentPanel.CurrentFolder;
                if ((strip.Visible && this.CheckUIState(UIState.IsDriveButtonsDirty)) && (currentFolder != null))
                {
                    bool performLayout = false;
                    strip.SuspendLayout();
                    try
                    {
                        foreach (ToolStripItem item in strip.Items)
                        {
                            if (item.Name.StartsWith("Drive_", StringComparison.Ordinal))
                            {
                                ToolStripButton button = (ToolStripButton) item;
                                IVirtualFolder tag = (IVirtualFolder) button.Tag;
                                bool flag2 = (this.CurrentPanel != null) && tag.IsChild(currentFolder);
                                if (button.Checked != flag2)
                                {
                                    button.Checked = flag2;
                                    performLayout = true;
                                }
                            }
                        }
                    }
                    finally
                    {
                        strip.ResumeLayout(performLayout);
                    }
                }
            }
            this.SetUIState(UIState.UpdatingToolbars | UIState.IsDriveButtonsDirty | UIState.IsToolbarsDirty, false);
        }

        private void UpdateWatchedFile(WatchProcessInfo info)
        {
            IVirtualFolder parent = info.SourceItem.Parent;
            if (parent != null)
            {
                MessageDialogResult uploadChangedFileBack = ConfirmationSettings.Default.UploadChangedFileBack;
                switch (uploadChangedFileBack)
                {
                    case MessageDialogResult.None:
                    {
                        bool checkBoxChecked = false;
                        uploadChangedFileBack = MessageDialog.Show(this, string.Format(Resources.sAskUploadFileBack, info.SourceItem.Name, parent.FullName), Resources.sConfirmFileUpload, Resources.sRememberQuestionAnswer, ref checkBoxChecked, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question);
                        if (checkBoxChecked)
                        {
                            ConfirmationSettings.Default.UploadChangedFileBack = uploadChangedFileBack;
                        }
                        break;
                    }
                    case MessageDialogResult.Yes:
                        DoStartCopy(info.SourceItem.Parent, new IVirtualItem[] { VirtualItem.FromFullName(info.TempItemPath, VirtualItemType.File) }, CopySettings.Default.DefaultCopyOptions, null, new SimpleRenameFilter(info.SourceItem.Name), new IOverwriteRule[] { new OverwriteAllRule(OverwriteDialogResult.Overwrite) });
                        return;
                }
            }
        }

        private void Volume_Changed(object sender, EventArgs e)
        {
            this.SetUpdateToolbarsNeeded("Drive_");
        }

        private void WatchProcessExited(object sender, EventArgs e)
        {
            WatchProcessInfo info;
            Process key = (Process) sender;
            lock (this.WatchProcessMap)
            {
                info = this.WatchProcessMap[key];
                this.WatchProcessMap.Remove(key);
            }
            key.Dispose();
            if (info.TempItemLastWriteTime != System.IO.File.GetLastWriteTime(info.TempItemPath))
            {
                if (base.InvokeRequired)
                {
                    base.BeginInvoke(new Action<WatchProcessInfo>(this.UpdateWatchedFile), new object[] { info });
                }
                else
                {
                    this.UpdateWatchedFile(info);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x15:
                case 0x31a:
                    this.InitializeToolStrip(false);
                    break;

                case 0x16:
                    this.SaveSessionSettings(m.LParam.SafeEquals<ENDSESSION>(ENDSESSION.ENDSESSION_CLOSEAPP) && Convert.ToBoolean((int) m.WParam), null);
                    break;

                case 0x319:
                    Action action;
                    if (!this.AppCommandActionMap.TryGetValue(WindowsWrapper.GET_APPCOMMAND_LPARAM(m.LParam), out action))
                    {
                        break;
                    }
                    action.Execute();
                    m.Result = (IntPtr) 1;
                    return;

                case 0x31d:
                    this.ClipbrdMask = 0;
                    this.SetUIState(UIState.IsCuttedItemsDirty, true);
                    break;
            }
            base.WndProc(ref m);
            if ((m.Msg == 5) && ((m.WParam.SafeEquals<SizeMode>(SizeMode.SIZE_MINIMIZED) && !this.CheckUIState(UIState.ProcessingTray)) && Settings.Default.MinimizeToTray))
            {
                this.actMinimizeToTray.Execute();
            }
        }

        private IDictionary<Action, ToolStripDropDown> ActionDropDownMap
        {
            get
            {
                if (this.FActionDropDownMap == null)
                {
                    this.FActionDropDownMap = new Dictionary<Action, ToolStripDropDown>();
                    this.FActionDropDownMap.Add(this.actFind, this.cmsMenuFind);
                    this.FActionDropDownMap.Add(this.actSaveCurrentLayout, this.cmsMenuWindowLayout);
                    this.FActionDropDownMap.Add(this.actChangeView, this.cmsMenuViewAs);
                    this.FActionDropDownMap.Add(this.actAdvancedFilter, this.cmsMenuFilter);
                    this.FActionDropDownMap.Add(this.actSelectSort, this.cmsMenuSort);
                    this.FActionDropDownMap.Add(this.actBookmarkCurrentFolder, this.tsmiBookmarks.DropDown);
                    this.FActionDropDownMap.Add(this.actNewFile, this.cmsMenuNew);
                    this.FActionDropDownMap.Add(this.actShowBookmarks, this.tsmiBookmarks.DropDown);
                }
                return this.FActionDropDownMap;
            }
        }

        private IDictionary<string, Action> ActionMap
        {
            get
            {
                if (this.FActionMap == null)
                {
                    this.FActionMap = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase);
                    foreach (Action action in this.actionManager.Actions)
                    {
                        this.FActionMap.Add(action.Name, action);
                    }
                }
                return this.FActionMap;
            }
        }

        private IVirtualFolder BookmarksFolder
        {
            get
            {
                if (!System.IO.Directory.Exists(SettingsManager.SpecialFolders.Bookmarks))
                {
                    System.IO.Directory.CreateDirectory(SettingsManager.SpecialFolders.Bookmarks);
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, SettingsManager.SpecialFolders.Bookmarks);
                    this.InitializeBookmarksWatcher();
                }
                return new BookmarksRootFolder();
            }
        }

        public IEnumerable<IComponent> Commands
        {
            get
            {
                return new <get_Commands>d__39(-2) { <>4__this = this };
            }
        }

        private VirtualFilePanel CurrentPanel
        {
            get
            {
                return this.CurrentTabContent.CurrentPanel;
            }
        }

        private TwoPanelContainer CurrentTabContent
        {
            get
            {
                TabStripPage selectedTabStripPage = this.MainPageSwitcher.SelectedTabStripPage;
                System.Diagnostics.Debug.Assert(selectedTabStripPage != null);
                return (TwoPanelContainer) selectedTabStripPage.Controls[0];
            }
        }

        private VirtualFilePanel FarPanel
        {
            get
            {
                return this.CurrentTabContent.FarPanel;
            }
        }

        public static MainForm Instance
        {
            get
            {
                return _Instance;
            }
            set
            {
                System.Diagnostics.Debug.Assert(_Instance == null);
                _Instance = value;
            }
        }

        public IDictionary<Keys, IComponent> KeyMap
        {
            get
            {
                Dictionary<Keys, IComponent> dictionary = new Dictionary<Keys, IComponent>();
                foreach (IComponent component in this.Commands)
                {
                    Action action = component as Action;
                    if ((action != null) && (action.Shortcuts != null))
                    {
                        foreach (Keys keys in action.Shortcuts)
                        {
                            dictionary.Add(keys, action);
                        }
                    }
                    ToolStripMenuItem item = component as ToolStripMenuItem;
                    if ((item != null) && (item.ShortcutKeys > Keys.None))
                    {
                        dictionary.Add(item.ShortcutKeys, item);
                    }
                }
                return dictionary;
            }
        }

        private Regex ToolbarCommandRegex
        {
            get
            {
                if (this.FToolbarCommandRegex == null)
                {
                    string str = string.Concat(new object[] { "Action_", '|', "DropDown_", '|', "Tool_", '|', "Tool_All_", '|', "Bookmark_", '|', "Bookmark_All_", '|', "Drive_All_", '|', "Separator_" });
                    this.FToolbarCommandRegex = new Regex("^(?<CommandType>" + str + @")\d{1,5}(_(?<CommandName>.+))?$", RegexOptions.Singleline | RegexOptions.Compiled);
                }
                return this.FToolbarCommandRegex;
            }
        }

        private IEnumerable<ToolStrip> Toolbars
        {
            get
            {
                return new <get_Toolbars>d__5(-2) { <>4__this = this };
            }
        }

        [CompilerGenerated]
        private sealed class <CreateBookmarkList>d__26 : IEnumerable<ToolStripItem>, IEnumerable, IEnumerator<ToolStripItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStripItem <>2__current;
            public string <>3__bookmarkFolder;
            public ToolStripItemDisplayStyle <>3__displayStyle;
            public System.Type <>3__itemType;
            public MainForm <>4__this;
            public string[] <>7__wrap2a;
            public int <>7__wrap2b;
            private int <>l__initialThreadId;
            public ToolStripItem <BookmarkItem>5__28;
            public string <NextBookmarkName>5__27;
            public string bookmarkFolder;
            public ToolStripItemDisplayStyle displayStyle;
            public System.Type itemType;

            [DebuggerHidden]
            public <CreateBookmarkList>d__26(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally29()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    int num = this.<>1__state;
                    if (num != 0)
                    {
                        if (num != 5)
                        {
                            goto Label_0333;
                        }
                        goto Label_02FC;
                    }
                    this.<>1__state = -1;
                    if (System.IO.Directory.Exists(this.bookmarkFolder))
                    {
                        this.<>1__state = 1;
                        this.<>7__wrap2a = System.IO.Directory.GetFiles(this.bookmarkFolder);
                        this.<>7__wrap2b = 0;
                        while (this.<>7__wrap2b < this.<>7__wrap2a.Length)
                        {
                            this.<NextBookmarkName>5__27 = this.<>7__wrap2a[this.<>7__wrap2b];
                            this.<BookmarkItem>5__28 = null;
                            try
                            {
                                ToolStripMenuItem item;
                                if (".tab".Equals(Path.GetExtension(this.<NextBookmarkName>5__27), StringComparison.OrdinalIgnoreCase))
                                {
                                    GeneralTab tab = null;
                                    using (XmlReader reader = XmlReader.Create(System.IO.File.OpenRead(this.<NextBookmarkName>5__27)))
                                    {
                                        tab = TwoPanelContainer.ParseBookmark(reader);
                                    }
                                    if (tab != null)
                                    {
                                        this.<BookmarkItem>5__28 = (ToolStripItem) Activator.CreateInstance(this.itemType);
                                        this.<BookmarkItem>5__28.Name = string.Format("{0}{1}_{2}", "Bookmark_", this.<>4__this.UniqueIndex++, Path.GetFileName(this.<NextBookmarkName>5__27));
                                        this.<BookmarkItem>5__28.Text = MainForm.GetMainMenuFileName(this.<NextBookmarkName>5__27, true);
                                        this.<BookmarkItem>5__28.DisplayStyle = this.displayStyle;
                                        this.<BookmarkItem>5__28.Tag = tab;
                                        this.<BookmarkItem>5__28.Image = IconSet.GetImage("tab");
                                        this.<BookmarkItem>5__28.Click += new EventHandler(this.<>4__this.TabBookmark_Click);
                                        item = this.<BookmarkItem>5__28 as ToolStripMenuItem;
                                        if (item != null)
                                        {
                                            item.ShortcutKeys = tab.Hotkey;
                                        }
                                    }
                                }
                                else if (VirtualItem.IsLink(this.<NextBookmarkName>5__27))
                                {
                                    IVirtualLink virtualLink = null;
                                    string str = null;
                                    virtualLink = new FileSystemShellLink(this.<NextBookmarkName>5__27);
                                    str = (string) virtualLink[10];
                                    if ((virtualLink != null) && !string.IsNullOrEmpty(str))
                                    {
                                        this.<BookmarkItem>5__28 = (ToolStripItem) Activator.CreateInstance(this.itemType);
                                        this.<>4__this.InitializeBookmarkItem(this.<BookmarkItem>5__28, virtualLink);
                                        this.<BookmarkItem>5__28.DisplayStyle = this.displayStyle;
                                        item = this.<BookmarkItem>5__28 as ToolStripMenuItem;
                                        if (item != null)
                                        {
                                            object obj2 = virtualLink[0x16];
                                            if (obj2 != null)
                                            {
                                                try
                                                {
                                                    item.ShortcutKeys = (Keys) obj2;
                                                }
                                                catch (InvalidEnumArgumentException)
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                this.<BookmarkItem>5__28 = null;
                                ApplicationException e = new ApplicationException(string.Format("Error loading bookmark '{0}'", this.<NextBookmarkName>5__27), exception);
                                Nomad.Trace.Error.TraceException(TraceEventType.Warning, e);
                            }
                            if (this.<BookmarkItem>5__28 == null)
                            {
                                goto Label_0303;
                            }
                            this.<>2__current = this.<BookmarkItem>5__28;
                            this.<>1__state = 5;
                            return true;
                        Label_02FC:
                            this.<>1__state = 1;
                        Label_0303:
                            this.<>7__wrap2b++;
                        }
                        this.<>m__Finally29();
                    }
                Label_0333:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
            {
                MainForm.<CreateBookmarkList>d__26 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new MainForm.<CreateBookmarkList>d__26(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.bookmarkFolder = this.<>3__bookmarkFolder;
                d__.itemType = this.<>3__itemType;
                d__.displayStyle = this.<>3__displayStyle;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripItem>.GetEnumerator();
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
                    case 5:
                        this.<>m__Finally29();
                        break;
                }
            }

            ToolStripItem IEnumerator<ToolStripItem>.Current
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
        private sealed class <CreateToolbarItemList>d__a : IEnumerable<ToolStripItem>, IEnumerable, IEnumerator<ToolStripItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStripItem <>2__current;
            public MainForm <>4__this;
            public IEnumerator<ToolStrip> <>7__wrapf;
            private int <>l__initialThreadId;
            public ToolbarSettings <BarSettings>5__c;
            public string <Caption>5__e;
            public ToolStrip <NextToolbar>5__b;
            public ToolStripMenuItem <ToolbarItem>5__d;

            [DebuggerHidden]
            public <CreateToolbarItemList>d__a(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally10()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapf != null)
                {
                    this.<>7__wrapf.Dispose();
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
                            this.<>7__wrapf = this.<>4__this.Toolbars.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrapf.MoveNext())
                            {
                                this.<NextToolbar>5__b = this.<>7__wrapf.Current;
                                this.<BarSettings>5__c = (ToolbarSettings) this.<NextToolbar>5__b.Tag;
                                this.<ToolbarItem>5__d = new ToolStripMenuItem();
                                this.<ToolbarItem>5__d.Tag = this.<NextToolbar>5__b;
                                this.<ToolbarItem>5__d.Click += new EventHandler(this.<>4__this.ToolbarItem_Click);
                                this.<ToolbarItem>5__d.Paint += new PaintEventHandler(this.<>4__this.ToolbarItem_Paint);
                                this.<Caption>5__e = Resources.ResourceManager.GetString(this.<BarSettings>5__c.Caption);
                                if (string.IsNullOrEmpty(this.<Caption>5__e))
                                {
                                    this.<Caption>5__e = this.<BarSettings>5__c.Caption;
                                }
                                this.<ToolbarItem>5__d.Text = this.<Caption>5__e;
                                this.<>2__current = this.<ToolbarItem>5__d;
                                this.<>1__state = 2;
                                return true;
                            Label_0138:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally10();
                            break;

                        case 2:
                            goto Label_0138;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new MainForm.<CreateToolbarItemList>d__a(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripItem>.GetEnumerator();
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
                            this.<>m__Finally10();
                        }
                        break;
                }
            }

            ToolStripItem IEnumerator<ToolStripItem>.Current
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
        private sealed class <CreateToolList>d__1c : IEnumerable<ToolStripItem>, IEnumerable, IEnumerator<ToolStripItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStripItem <>2__current;
            public ToolStripItemDisplayStyle <>3__displayStyle;
            public System.Type <>3__itemType;
            public string <>3__toolFolder;
            public MainForm <>4__this;
            public List<NamedShellLink>.Enumerator <>7__wrap20;
            public NamedShellLink <>7__wrap22;
            private int <>l__initialThreadId;
            public NamedShellLink <NextToolLink>5__1e;
            public ToolStripItem <ToolItem>5__1f;
            public List<NamedShellLink> <ToolList>5__1d;
            public ToolStripItemDisplayStyle displayStyle;
            public System.Type itemType;
            public string toolFolder;

            [DebuggerHidden]
            public <CreateToolList>d__1c(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally21()
            {
                this.<>1__state = -1;
                this.<>7__wrap20.Dispose();
            }

            private void <>m__Finally23()
            {
                this.<>1__state = 3;
                if (this.<>7__wrap22 != null)
                {
                    this.<>7__wrap22.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    int num = this.<>1__state;
                    if (num != 0)
                    {
                        if (num != 5)
                        {
                            goto Label_01B3;
                        }
                        goto Label_0187;
                    }
                    this.<>1__state = -1;
                    if (System.IO.Directory.Exists(this.toolFolder))
                    {
                        this.<ToolList>5__1d = new List<NamedShellLink>();
                        foreach (string str in System.IO.Directory.GetFiles(this.toolFolder, "*.lnk"))
                        {
                            try
                            {
                                this.<ToolList>5__1d.Add(new NamedShellLink(str));
                            }
                            catch (SystemException exception)
                            {
                                ApplicationException e = new ApplicationException(string.Format("Error loading tool link '{0}'", str), exception);
                                Nomad.Trace.Error.TraceException(TraceEventType.Warning, e);
                            }
                        }
                        if (MainForm.CS$<>9__CachedAnonymousMethodDelegate1b == null)
                        {
                            MainForm.CS$<>9__CachedAnonymousMethodDelegate1b = new Comparison<NamedShellLink>(MainForm.<CreateToolList>b__1a);
                        }
                        this.<ToolList>5__1d.Sort(MainForm.CS$<>9__CachedAnonymousMethodDelegate1b);
                        this.<>7__wrap20 = this.<ToolList>5__1d.GetEnumerator();
                        this.<>1__state = 3;
                        while (this.<>7__wrap20.MoveNext())
                        {
                            this.<NextToolLink>5__1e = this.<>7__wrap20.Current;
                            this.<>7__wrap22 = this.<NextToolLink>5__1e;
                            this.<>1__state = 4;
                            this.<ToolItem>5__1f = (ToolStripItem) Activator.CreateInstance(this.itemType);
                            this.<>4__this.InitializeToolItem(this.<ToolItem>5__1f, this.<NextToolLink>5__1e.OriginalName, this.<NextToolLink>5__1e);
                            this.<ToolItem>5__1f.DisplayStyle = this.displayStyle;
                            this.<>2__current = this.<ToolItem>5__1f;
                            this.<>1__state = 5;
                            return true;
                        Label_0187:
                            this.<>1__state = 4;
                            this.<>m__Finally23();
                        }
                        this.<>m__Finally21();
                    }
                Label_01B3:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
            {
                MainForm.<CreateToolList>d__1c d__c;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__c = this;
                }
                else
                {
                    d__c = new MainForm.<CreateToolList>d__1c(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__c.toolFolder = this.<>3__toolFolder;
                d__c.itemType = this.<>3__itemType;
                d__c.displayStyle = this.<>3__displayStyle;
                return d__c;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripItem>.GetEnumerator();
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
                    case 3:
                    case 4:
                    case 5:
                        try
                        {
                            switch (this.<>1__state)
                            {
                                case 4:
                                case 5:
                                    break;

                                default:
                                    break;
                            }
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finally23();
                            }
                        }
                        finally
                        {
                            this.<>m__Finally21();
                        }
                        break;
                }
            }

            ToolStripItem IEnumerator<ToolStripItem>.Current
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
        private sealed class <FillBookmarkMenuList>d__2e : IEnumerable<ToolStripItem>, IEnumerable, IEnumerator<ToolStripItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStripItem <>2__current;
            public string <>3__bookmarkFolder;
            public MainForm <>4__this;
            public string[] <>7__wrap33;
            public int <>7__wrap34;
            public IEnumerator<ToolStripItem> <>7__wrap35;
            private int <>l__initialThreadId;
            public ToolStripMenuItem <FolderItem>5__30;
            public ToolStripItem <NextBookmarkItem>5__31;
            public string <NextFolderName>5__2f;
            public string bookmarkFolder;

            [DebuggerHidden]
            public <FillBookmarkMenuList>d__2e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally32()
            {
                this.<>1__state = -1;
            }

            private void <>m__Finally36()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap35 != null)
                {
                    this.<>7__wrap35.Dispose();
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
                            if (System.IO.Directory.Exists(this.bookmarkFolder))
                            {
                                this.<>1__state = 1;
                                this.<>7__wrap33 = System.IO.Directory.GetDirectories(this.bookmarkFolder);
                                this.<>7__wrap34 = 0;
                                while (this.<>7__wrap34 < this.<>7__wrap33.Length)
                                {
                                    this.<NextFolderName>5__2f = this.<>7__wrap33[this.<>7__wrap34];
                                    this.<FolderItem>5__30 = new ToolStripMenuItem();
                                    this.<FolderItem>5__30.Text = MainForm.GetMainMenuFileName(this.<NextFolderName>5__2f, false);
                                    this.<FolderItem>5__30.ShortcutKeyDisplayString = this.<NextFolderName>5__2f;
                                    this.<FolderItem>5__30.ShowShortcutKeys = false;
                                    this.<FolderItem>5__30.Image = IconSet.GetImage("folder");
                                    this.<FolderItem>5__30.DropDownOpening += new EventHandler(this.<>4__this.tsmiBookmarkFolder_DropDownOpening);
                                    this.<FolderItem>5__30.DropDown.Closed += new ToolStripDropDownClosedEventHandler(this.<>4__this.CleanupDropDown_Closed);
                                    this.<FolderItem>5__30.DropDownItems.Add(new ToolStripMenuItem());
                                    this.<>2__current = this.<FolderItem>5__30;
                                    this.<>1__state = 2;
                                    return true;
                                Label_0150:
                                    this.<>1__state = 1;
                                    this.<>7__wrap34++;
                                }
                                this.<>m__Finally32();
                                this.<>7__wrap35 = this.<>4__this.CreateBookmarkList(this.bookmarkFolder, typeof(ToolStripMenuItem), ToolStripItemDisplayStyle.ImageAndText).GetEnumerator();
                                this.<>1__state = 3;
                                while (this.<>7__wrap35.MoveNext())
                                {
                                    this.<NextBookmarkItem>5__31 = this.<>7__wrap35.Current;
                                    this.<>2__current = this.<NextBookmarkItem>5__31;
                                    this.<>1__state = 4;
                                    return true;
                                Label_01DD:
                                    this.<>1__state = 3;
                                }
                                this.<>m__Finally36();
                            }
                            break;

                        case 2:
                            goto Label_0150;

                        case 4:
                            goto Label_01DD;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
            {
                MainForm.<FillBookmarkMenuList>d__2e d__e;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__e = this;
                }
                else
                {
                    d__e = new MainForm.<FillBookmarkMenuList>d__2e(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__e.bookmarkFolder = this.<>3__bookmarkFolder;
                return d__e;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripItem>.GetEnumerator();
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
                        this.<>m__Finally32();
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally36();
                        }
                        break;
                }
            }

            ToolStripItem IEnumerator<ToolStripItem>.Current
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
        private sealed class <get_Commands>d__39 : IEnumerable<IComponent>, IEnumerable, IEnumerator<IComponent>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IComponent <>2__current;
            public MainForm <>4__this;
            public List<Action>.Enumerator <>7__wrap3e;
            public IEnumerator <>7__wrap40;
            public IDisposable <>7__wrap41;
            private int <>l__initialThreadId;
            public Stack<ToolStripItemCollection> <Collections>5__3b;
            public Action <NextAction>5__3a;
            public ToolStripItem <NextItem>5__3c;
            public ToolStripMenuItem <NextMenuItem>5__3d;

            [DebuggerHidden]
            public <get_Commands>d__39(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally3f()
            {
                this.<>1__state = -1;
                this.<>7__wrap3e.Dispose();
            }

            private void <>m__Finally42()
            {
                this.<>1__state = -1;
                this.<>7__wrap41 = this.<>7__wrap40 as IDisposable;
                if (this.<>7__wrap41 != null)
                {
                    this.<>7__wrap41.Dispose();
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
                            this.<>7__wrap3e = this.<>4__this.actionManager.Actions.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap3e.MoveNext())
                            {
                                this.<NextAction>5__3a = this.<>7__wrap3e.Current;
                                this.<>2__current = this.<NextAction>5__3a;
                                this.<>1__state = 2;
                                return true;
                            Label_0089:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally3f();
                            this.<Collections>5__3b = new Stack<ToolStripItemCollection>();
                            this.<Collections>5__3b.Push(this.<>4__this.MainMenu.Items);
                            while (this.<Collections>5__3b.Count > 0)
                            {
                                this.<>7__wrap40 = this.<Collections>5__3b.Pop().GetEnumerator();
                                this.<>1__state = 3;
                                while (this.<>7__wrap40.MoveNext())
                                {
                                    this.<NextItem>5__3c = (ToolStripItem) this.<>7__wrap40.Current;
                                    this.<NextMenuItem>5__3d = this.<NextItem>5__3c as ToolStripMenuItem;
                                    if (((this.<NextMenuItem>5__3d == null) || (this.<NextMenuItem>5__3d == this.<>4__this.tsmiBookmarks)) || (this.<NextMenuItem>5__3d == this.<>4__this.tsmiTools))
                                    {
                                        goto Label_01C3;
                                    }
                                    if (this.<NextMenuItem>5__3d.DropDownItems.Count > 0)
                                    {
                                        this.<Collections>5__3b.Push(this.<NextMenuItem>5__3d.DropDownItems);
                                        goto Label_01C3;
                                    }
                                    if (this.<>4__this.actionManager.GetAction(this.<NextMenuItem>5__3d) != null)
                                    {
                                        goto Label_01C3;
                                    }
                                    this.<>2__current = this.<NextMenuItem>5__3d;
                                    this.<>1__state = 4;
                                    return true;
                                Label_01BC:
                                    this.<>1__state = 3;
                                Label_01C3:;
                                }
                                this.<>m__Finally42();
                            }
                            break;

                        case 2:
                            goto Label_0089;

                        case 4:
                            goto Label_01BC;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new MainForm.<get_Commands>d__39(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.ComponentModel.IComponent>.GetEnumerator();
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
                            this.<>m__Finally3f();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally42();
                        }
                        break;
                }
            }

            IComponent IEnumerator<IComponent>.Current
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
        private sealed class <get_Toolbars>d__5 : IEnumerable<ToolStrip>, IEnumerable, IEnumerator<ToolStrip>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStrip <>2__current;
            public MainForm <>4__this;
            private int <>l__initialThreadId;
            public int <I>5__6;
            public ToolStrip <NextToolbar>5__7;

            [DebuggerHidden]
            public <get_Toolbars>d__5(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<I>5__6 = this.<>4__this.Controls.Count - 1;
                        while (this.<I>5__6 >= 0)
                        {
                            this.<NextToolbar>5__7 = this.<>4__this.Controls[this.<I>5__6] as ToolStrip;
                            if ((this.<NextToolbar>5__7 == null) || !this.<NextToolbar>5__7.Name.StartsWith("Toolbar_", StringComparison.Ordinal))
                            {
                                goto Label_00AD;
                            }
                            this.<>2__current = this.<NextToolbar>5__7;
                            this.<>1__state = 1;
                            return true;
                        Label_00A6:
                            this.<>1__state = -1;
                        Label_00AD:
                            this.<I>5__6--;
                        }
                        break;

                    case 1:
                        goto Label_00A6;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<ToolStrip> IEnumerable<ToolStrip>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new MainForm.<get_Toolbars>d__5(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStrip>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            ToolStrip IEnumerator<ToolStrip>.Current
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
        private sealed class <GetRelatedToolbarItems>d__13 : IEnumerable<ToolStripItem>, IEnumerable, IEnumerator<ToolStripItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ToolStripItem <>2__current;
            public ToolStripItem <>3__item;
            public string <>3__prefix;
            public MainForm <>4__this;
            private int <>l__initialThreadId;
            public int <I>5__16;
            public int <I>5__17;
            public int <ItemIndex>5__15;
            public ToolStrip <Toolbar>5__14;
            public ToolStripItem item;
            public string prefix;

            [DebuggerHidden]
            public <GetRelatedToolbarItems>d__13(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        if (this.item.Name.StartsWith(this.prefix, StringComparison.Ordinal))
                        {
                            this.<Toolbar>5__14 = this.item.Owner;
                            this.<ItemIndex>5__15 = this.<Toolbar>5__14.Items.IndexOf(this.item);
                            this.<I>5__16 = this.<ItemIndex>5__15 - 1;
                            while ((this.<I>5__16 >= 0) && this.<Toolbar>5__14.Items[this.<I>5__16].Name.StartsWith(this.prefix, StringComparison.Ordinal))
                            {
                                this.<>2__current = this.<Toolbar>5__14.Items[this.<I>5__16];
                                this.<>1__state = 1;
                                return true;
                            Label_00BD:
                                this.<>1__state = -1;
                                this.<I>5__16--;
                            }
                            this.<I>5__17 = this.<ItemIndex>5__15;
                            while ((this.<I>5__17 < this.<Toolbar>5__14.Items.Count) && this.<Toolbar>5__14.Items[this.<I>5__17].Name.StartsWith(this.prefix, StringComparison.Ordinal))
                            {
                                this.<>2__current = this.<Toolbar>5__14.Items[this.<I>5__17];
                                this.<>1__state = 2;
                                return true;
                            Label_013E:
                                this.<>1__state = -1;
                                this.<I>5__17++;
                            }
                        }
                        break;

                    case 1:
                        goto Label_00BD;

                    case 2:
                        goto Label_013E;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
            {
                MainForm.<GetRelatedToolbarItems>d__13 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new MainForm.<GetRelatedToolbarItems>d__13(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.item = this.<>3__item;
                d__.prefix = this.<>3__prefix;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripItem>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            ToolStripItem IEnumerator<ToolStripItem>.Current
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

        [Flags]
        private enum ClipboardState
        {
            ContainsAnyData = 1,
            ContainsImage = 0x10,
            ContainsItems = 2,
            ContainsSelection = 4,
            ContainsText = 8
        }

        private class FakeWin32Window : IWin32Window
        {
            private IntPtr FHandle;

            public FakeWin32Window(IWin32Window owner)
            {
                this.FHandle = owner.Handle;
            }

            public IntPtr Handle
            {
                get
                {
                    return this.FHandle;
                }
            }
        }

        [Flags]
        private enum UIState
        {
            CheckClipboardUsingMessages = 0x80,
            CheckClipboardUsingTicks = 0x100,
            DirectToolStart = 0x1000,
            GCCollectNeeded = 0x400,
            HasBookmarks = 8,
            HasTools = 0x10,
            IsCuttedItemsDirty = 0x200,
            IsDriveButtonsDirty = 2,
            IsToolbarsDirty = 1,
            ProcessingTray = 0x800,
            ShowCrushLogNeeded = 0x20,
            StartController = 0x40,
            UpdatingToolbars = 4
        }

        private class WatchProcessInfo
        {
            public readonly IVirtualFile SourceItem;
            public readonly DateTime TempItemLastWriteTime;
            public readonly string TempItemPath;

            public WatchProcessInfo(IVirtualFile source, string tempPath)
            {
                this.SourceItem = source;
                this.TempItemPath = tempPath;
                this.TempItemLastWriteTime = System.IO.File.GetLastWriteTime(this.TempItemPath);
            }
        }
    }
}

