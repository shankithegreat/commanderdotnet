namespace Nomad.Properties
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Configuration;
    using Nomad.Controls.Filter;
    using Nomad.Dialogs;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), SettingsProvider(typeof(ConfigurableSettingsProvider))]
    internal sealed class Settings : ApplicationSettingsBase
    {
        public const string CaptionNomadNet = "Nomad.NET";
        public const ReleaseType CurrentRelease = ReleaseType.RC;
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));
        public IDictionary<Keys, IComponent> DefaultKeyMap;
        private static ListViewHighlighter[] FDefaultHighlighters;
        public NameFilter FExtractOnRunFilter;
        private bool FShowIcons;
        public static readonly Color TextBoxError = Color.FromArgb(0xff, 0x66, 0x66);

        public void HideProperties()
        {
            VirtualProperty.Visible = ~((VirtualPropertySet) TypeDescriptor.GetConverter(typeof(VirtualPropertySet)).ConvertFromInvariantString(this.HiddenProperties));
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);
            string propertyName = e.PropertyName;
            if (propertyName != null)
            {
                if (!(propertyName == "ShowIcons"))
                {
                    if (propertyName == "ShowItemToolTips")
                    {
                        this.IsShowItemToolTips = this.ShowItemToolTips;
                    }
                    else if (propertyName == "ShowKeyboardCues")
                    {
                        this.IsShowKeyboardCues = this.ShowKeyboardCues;
                    }
                }
                else
                {
                    this.FShowIcons = this.ShowIcons;
                }
            }
        }

        protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
        {
            base.OnSettingsLoaded(sender, e);
            this.CompactPropertyValues();
            this.FShowIcons = this.ShowIcons;
            this.IsShowItemToolTips = this.ShowItemToolTips;
            this.IsShowKeyboardCues = this.ShowKeyboardCues;
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool AlwaysShowTabStrip
        {
            get
            {
                return (bool) this["AlwaysShowTabStrip"];
            }
            set
            {
                this["AlwaysShowTabStrip"] = value;
            }
        }

        [DefaultSettingValue("{0} ({2:d}){1}"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string AnotherLinkPattern
        {
            get
            {
                return (string) this["AnotherLinkPattern"];
            }
        }

        [DefaultSettingValue("Suggest"), UserScopedSetting, DebuggerNonUserCode]
        public System.Windows.Forms.AutoCompleteMode AutoCompleteMode
        {
            get
            {
                return (System.Windows.Forms.AutoCompleteMode) this["AutoCompleteMode"];
            }
            set
            {
                this["AutoCompleteMode"] = value;
            }
        }

        [DefaultSettingValue("http://www.nomad-net.info/home/version.txt"), DebuggerNonUserCode, ApplicationScopedSetting]
        public string CheckForUpdatesUrl
        {
            get
            {
                return (string) this["CheckForUpdatesUrl"];
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool ClearSelectionBeforeWork
        {
            get
            {
                return (bool) this["ClearSelectionBeforeWork"];
            }
            set
            {
                this["ClearSelectionBeforeWork"] = value;
            }
        }

        [UserScopedSetting]
        public KeyValueList<string, ListViewColumnInfo[]> ColumnTemplates
        {
            get
            {
                return (KeyValueList<string, ListViewColumnInfo[]>) this["ColumnTemplates"];
            }
            set
            {
                this["ColumnTemplates"] = value;
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DefaultSettingValue("Yes"), DebuggerNonUserCode]
        public MessageDialogResult DefaultDeleteDialogButton
        {
            get
            {
                return (MessageDialogResult) this["DefaultDeleteDialogButton"];
            }
            set
            {
                this["DefaultDeleteDialogButton"] = value;
            }
        }

        public static ListViewHighlighter[] DefaultHighlighters
        {
            get
            {
                if (FDefaultHighlighters == null)
                {
                    ListViewHighlighter highlighter = new ListViewHighlighter("Hidden", new AggregatedVirtualItemFilter(AggregatedFilterCondition.Any, new VirtualItemAttributeFilter(FileAttributes.System), new VirtualItemAttributeFilter(FileAttributes.Hidden))) {
                        ForeColor = SystemColors.GrayText,
                        AlphaBlend = true
                    };
                    ListViewHighlighter highlighter2 = new ListViewHighlighter("Executable", new AggregatedVirtualItemFilter(new VirtualItemAttributeFilter(0, FileAttributes.Directory), new VirtualItemNameFilter("*.exe;*.com;*.bat"))) {
                        ForeColor = Color.Purple
                    };
                    FDefaultHighlighters = new ListViewHighlighter[] { highlighter, highlighter2 };
                }
                return FDefaultHighlighters;
            }
        }

        public static TwoPanelLayout[] DefaultLayouts
        {
            get
            {
                TwoPanelLayout layout = new TwoPanelLayout {
                    Name = Resources.sLayoutClassic,
                    StoreEntry = TwoPanelLayoutEntry.RightLayout | TwoPanelLayoutEntry.LeftLayout | TwoPanelLayoutEntry.PanelsOrientation | TwoPanelLayoutEntry.OnePanel,
                    LeftLayout = { StoreEntry = PanelLayoutEntry.ListColumnCount | PanelLayoutEntry.ToolbarsVisible | PanelLayoutEntry.View | PanelLayoutEntry.FolderBarVisible },
                    RightLayout = { StoreEntry = PanelLayoutEntry.ListColumnCount | PanelLayoutEntry.ToolbarsVisible | PanelLayoutEntry.View | PanelLayoutEntry.FolderBarVisible }
                };
                TwoPanelLayout layout2 = new TwoPanelLayout {
                    Name = Resources.sLayoutExplorer,
                    OnePanel = true,
                    LeftLayout = { FolderBarVisible = true, FolderBarOrientation = Orientation.Vertical, SplitterPercent = 0x14d, StoreEntry = PanelLayoutEntry.ListColumnCount | PanelLayoutEntry.ToolbarsVisible | PanelLayoutEntry.View | PanelLayoutEntry.FolderBarOrientation | PanelLayoutEntry.FolderBarVisible },
                    StoreEntry = TwoPanelLayoutEntry.LeftLayout | TwoPanelLayoutEntry.OnePanel
                };
                TwoPanelLayout layout3 = new TwoPanelLayout {
                    Name = Resources.sLayoutPictures,
                    OnePanel = true,
                    LeftLayout = { View = PanelView.Thumbnail, StoreEntry = PanelLayoutEntry.ToolbarsVisible | PanelLayoutEntry.View | PanelLayoutEntry.FolderBarVisible },
                    StoreEntry = TwoPanelLayoutEntry.LeftLayout | TwoPanelLayoutEntry.OnePanel
                };
                return new TwoPanelLayout[] { layout, layout2, layout3 };
            }
        }

        [DefaultSettingValue("Always"), UserScopedSetting, DebuggerNonUserCode]
        public Nomad.FileSystem.Virtual.DelayedExtractMode DelayedExtractMode
        {
            get
            {
                return (Nomad.FileSystem.Virtual.DelayedExtractMode) this["DelayedExtractMode"];
            }
            set
            {
                this["DelayedExtractMode"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool DeleteToBin
        {
            get
            {
                return (bool) this["DeleteToBin"];
            }
            set
            {
                this["DeleteToBin"] = value;
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool DirectToolStart
        {
            get
            {
                return (bool) this["DirectToolStart"];
            }
        }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string DisabledPropertyProviders
        {
            get
            {
                return (string) this["DisabledPropertyProviders"];
            }
            set
            {
                this["DisabledPropertyProviders"] = value;
            }
        }

        [DefaultSettingValue(""), DebuggerNonUserCode, UserScopedSetting]
        public string EditorPath
        {
            get
            {
                return (string) this["EditorPath"];
            }
            set
            {
                this["EditorPath"] = value;
            }
        }

        [DefaultSettingValue("True"), DebuggerNonUserCode, UserScopedSetting]
        public bool EnterOpensArchive
        {
            get
            {
                return (bool) this["EnterOpensArchive"];
            }
            set
            {
                this["EnterOpensArchive"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool ExplorerTheme
        {
            get
            {
                return (bool) this["ExplorerTheme"];
            }
            set
            {
                this["ExplorerTheme"] = value;
            }
        }

        public NameFilter ExtractOnRunFilter
        {
            get
            {
                return (this.FExtractOnRunFilter ?? (this.FExtractOnRunFilter = new NameFilter(this.ExtractOnRunMask)));
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("*.htm;*.html")]
        public string ExtractOnRunMask
        {
            get
            {
                return (string) this["ExtractOnRunMask"];
            }
        }

        [DefaultSettingValue("Full"), DebuggerNonUserCode, UserScopedSetting]
        public ComplexFilterView FilterDialogView
        {
            get
            {
                return (ComplexFilterView) this["FilterDialogView"];
            }
            set
            {
                this["FilterDialogView"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting]
        public NamedFilter[] Filters
        {
            get
            {
                return (NamedFilter[]) this["Filters"];
            }
            set
            {
                this["Filters"] = value;
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool FixMouseWheel
        {
            get
            {
                return (bool) this["FixMouseWheel"];
            }
        }

        [DefaultSettingValue("tsmiViewAs\r\n-\r\ntsmiSort\r\nactRefresh\r\n-\r\nactPasteFromClipboard\r\nactPasteShortcut\r\n-\r\n?\r\n-\r\ntsmiNew\r\n-\r\nactShowProperties"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string FolderContextMenuCommands
        {
            get
            {
                return (string) this["FolderContextMenuCommands"];
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool ForceDesktopIniCache
        {
            get
            {
                return (bool) this["ForceDesktopIniCache"];
            }
        }

        [DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
        public string HiddenProperties
        {
            get
            {
                return (string) this["HiddenProperties"];
            }
            set
            {
                this["HiddenProperties"] = value;
            }
        }

        [DefaultSettingValue("True"), DebuggerNonUserCode, UserScopedSetting]
        public bool HideNotReadyDrives
        {
            get
            {
                return (bool) this["HideNotReadyDrives"];
            }
            set
            {
                this["HideNotReadyDrives"] = value;
            }
        }

        [UserScopedSetting]
        public ListViewHighlighter[] Highlighters
        {
            get
            {
                ListViewHighlighter[] defaultHighlighters = (ListViewHighlighter[]) this["Highlighters"];
                if (defaultHighlighters == null)
                {
                    defaultHighlighters = DefaultHighlighters;
                }
                return defaultHighlighters;
            }
            set
            {
                this["Highlighters"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("ExtractIcons, ShowOverlayIcons")]
        public Nomad.FileSystem.Virtual.IconOptions IconOptions
        {
            get
            {
                return (Nomad.FileSystem.Virtual.IconOptions) this["IconOptions"];
            }
            set
            {
                this["IconOptions"] = value;
            }
        }

        [DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
        public string ImageProvider
        {
            get
            {
                return (string) this["ImageProvider"];
            }
            set
            {
                this["ImageProvider"] = value;
            }
        }

        [DefaultSettingValue("True"), ApplicationScopedSetting, DebuggerNonUserCode]
        public bool ImprovedUnhandledExceptionProcessing
        {
            get
            {
                return (bool) this["ImprovedUnhandledExceptionProcessing"];
            }
        }

        public bool IsShowIcons
        {
            get
            {
                return (this.FShowIcons && !SettingsManager.CheckSafeMode(SafeMode.DisableIcons));
            }
        }

        public bool IsShowItemToolTips { get; private set; }

        public bool IsShowKeyboardCues { get; private set; }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string KeyboardMap
        {
            get
            {
                return (string) this["KeyboardMap"];
            }
            set
            {
                this["KeyboardMap"] = value;
            }
        }

        [UserScopedSetting]
        public TwoPanelLayout[] Layouts
        {
            get
            {
                TwoPanelLayout[] layoutArray = (TwoPanelLayout[]) this["Layouts"];
                if (layoutArray == null)
                {
                    return DefaultLayouts;
                }
                return layoutArray;
            }
            set
            {
                this["Layouts"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool MainMenuVisible
        {
            get
            {
                return (bool) this["MainMenuVisible"];
            }
            set
            {
                this["MainMenuVisible"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool MinimizeToTray
        {
            get
            {
                return (bool) this["MinimizeToTray"];
            }
            set
            {
                this["MinimizeToTray"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string NewFileLastExt
        {
            get
            {
                return (string) this["NewFileLastExt"];
            }
            set
            {
                this["NewFileLastExt"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool PerformInitialInitialize
        {
            get
            {
                return (bool) this["PerformInitialInitialize"];
            }
            set
            {
                this["PerformInitialInitialize"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool PersistentIconCache
        {
            get
            {
                return (bool) this["PersistentIconCache"];
            }
            set
            {
                this["PersistentIconCache"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("False"), ApplicationScopedSetting]
        public bool PreserveMainMenuFileNameAmpersand
        {
            get
            {
                return (bool) this["PreserveMainMenuFileNameAmpersand"];
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool ProcessFolderShortcuts
        {
            get
            {
                return (bool) this["ProcessFolderShortcuts"];
            }
            set
            {
                this["ProcessFolderShortcuts"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool RestoreTabsOnStart
        {
            get
            {
                return (bool) this["RestoreTabsOnStart"];
            }
            set
            {
                this["RestoreTabsOnStart"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool RightClickSelect
        {
            get
            {
                return (bool) this["RightClickSelect"];
            }
            set
            {
                this["RightClickSelect"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool RunAsShowPassword
        {
            get
            {
                return (bool) this["RunAsShowPassword"];
            }
            set
            {
                this["RunAsShowPassword"] = value;
            }
        }

        [DefaultSettingValue("Indeterminate"), UserScopedSetting, DebuggerNonUserCode]
        public CheckState RunInThread
        {
            get
            {
                return (CheckState) this["RunInThread"];
            }
            set
            {
                this["RunInThread"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("UseIFilter, DetectEncoding"), DebuggerNonUserCode]
        public ContentFilterOptions SearchContentOptions
        {
            get
            {
                return (ContentFilterOptions) this["SearchContentOptions"];
            }
            set
            {
                this["SearchContentOptions"] = value;
            }
        }

        [DefaultSettingValue("Full"), UserScopedSetting, DebuggerNonUserCode]
        public ComplexFilterView SearchDialogView
        {
            get
            {
                return (ComplexFilterView) this["SearchDialogView"];
            }
            set
            {
                this["SearchDialogView"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting]
        public NamedFilter[] Searches
        {
            get
            {
                return (NamedFilter[]) this["Searches"];
            }
            set
            {
                this["Searches"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool SelectDialogSelectFolders
        {
            get
            {
                return (bool) this["SelectDialogSelectFolders"];
            }
            set
            {
                this["SelectDialogSelectFolders"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool SelectNameWithoutExt
        {
            get
            {
                return (bool) this["SelectNameWithoutExt"];
            }
            set
            {
                this["SelectNameWithoutExt"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("True"), UserScopedSetting]
        public bool ShowIconInTooltip
        {
            get
            {
                return (bool) this["ShowIconInTooltip"];
            }
            set
            {
                this["ShowIconInTooltip"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool ShowIcons
        {
            get
            {
                return (bool) this["ShowIcons"];
            }
            set
            {
                this["ShowIcons"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool ShowItemToolTips
        {
            get
            {
                return (bool) this["ShowItemToolTips"];
            }
            set
            {
                this["ShowItemToolTips"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool ShowItemTooltipsKbd
        {
            get
            {
                return (bool) this["ShowItemTooltipsKbd"];
            }
            set
            {
                this["ShowItemTooltipsKbd"] = value;
            }
        }

        [DefaultSettingValue("True"), DebuggerNonUserCode, UserScopedSetting]
        public bool ShowKeyboardCues
        {
            get
            {
                return (bool) this["ShowKeyboardCues"];
            }
            set
            {
                this["ShowKeyboardCues"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool ShowThumbnailInTooltip
        {
            get
            {
                return (bool) this["ShowThumbnailInTooltip"];
            }
            set
            {
                this["ShowThumbnailInTooltip"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("Simplified"), UserScopedSetting]
        public AutoRefreshMode SlowVolumeAutoRefresh
        {
            get
            {
                return (AutoRefreshMode) this["SlowVolumeAutoRefresh"];
            }
            set
            {
                this["SlowVolumeAutoRefresh"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("0"), DebuggerNonUserCode]
        public int TabWidth
        {
            get
            {
                return (int) this["TabWidth"];
            }
            set
            {
                this["TabWidth"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string Theme
        {
            get
            {
                return (string) this["Theme"];
            }
            set
            {
                this["Theme"] = value;
            }
        }

        [DefaultSettingValue("48, 48"), ApplicationScopedSetting, DebuggerNonUserCode]
        public Size ToolTipIconSize
        {
            get
            {
                return (Size) this["ToolTipIconSize"];
            }
        }

        [DefaultSettingValue("120, 120"), ApplicationScopedSetting, DebuggerNonUserCode]
        public Size TooltipMaxThumbnailSize
        {
            get
            {
                return (Size) this["TooltipMaxThumbnailSize"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("2000"), DebuggerNonUserCode]
        public int TooltipOpacityDelay
        {
            get
            {
                return (int) this["TooltipOpacityDelay"];
            }
            set
            {
                this["TooltipOpacityDelay"] = value;
            }
        }

        [DefaultSettingValue("0.65"), ApplicationScopedSetting, DebuggerNonUserCode]
        public double TooltipOpacityValue
        {
            get
            {
                return (double) this["TooltipOpacityValue"];
            }
        }

        [DefaultSettingValue("1000"), ApplicationScopedSetting, DebuggerNonUserCode]
        public int TooltipThumbnailTimeout
        {
            get
            {
                return (int) this["TooltipThumbnailTimeout"];
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("1000"), DebuggerNonUserCode]
        public int TooltipTimeout
        {
            get
            {
                return (int) this["TooltipTimeout"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("(Default)"), DebuggerNonUserCode]
        public CultureInfo UICulture
        {
            get
            {
                return (CultureInfo) this["UICulture"];
            }
            set
            {
                this["UICulture"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool UseACSEnvironmentVariables
        {
            get
            {
                return (bool) this["UseACSEnvironmentVariables"];
            }
            set
            {
                this["UseACSEnvironmentVariables"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool UseACSFileSystem
        {
            get
            {
                return (bool) this["UseACSFileSystem"];
            }
            set
            {
                this["UseACSFileSystem"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool UseACSKnownShellFolders
        {
            get
            {
                return (bool) this["UseACSKnownShellFolders"];
            }
            set
            {
                this["UseACSKnownShellFolders"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool UseACSRecentItems
        {
            get
            {
                return (bool) this["UseACSRecentItems"];
            }
            set
            {
                this["UseACSRecentItems"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("")]
        public string ViewerPath
        {
            get
            {
                return (string) this["ViewerPath"];
            }
            set
            {
                this["ViewerPath"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("ClientAndNonClientAreasEnabled"), DebuggerNonUserCode]
        public System.Windows.Forms.VisualStyles.VisualStyleState VisualStyleState
        {
            get
            {
                return (System.Windows.Forms.VisualStyles.VisualStyleState) this["VisualStyleState"];
            }
            set
            {
                this["VisualStyleState"] = value;
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool WatchFolderTree
        {
            get
            {
                return (bool) this["WatchFolderTree"];
            }
        }
    }
}

