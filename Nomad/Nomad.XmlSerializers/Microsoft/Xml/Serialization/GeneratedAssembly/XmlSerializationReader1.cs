namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Controls.Actions;
    using Nomad.Controls.Filter;
    using Nomad.Controls.Specialized;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Archive.Wcx;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Network;
    using Nomad.FileSystem.Null;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Property.Providers;
    using Nomad.FileSystem.Property.Providers.Wdx;
    using Nomad.FileSystem.Shell;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Workers;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializationReader1 : XmlSerializationReader
    {
        private Hashtable _ActionStateValues;
        private Hashtable _ArchiveFormatCapabilitiesValues;
        private Hashtable _AvailableItemActionsValues;
        private Hashtable _BindActionPropertyValues;
        private Hashtable _CanMoveListViewItemValues;
        private Hashtable _CompareFoldersOptionsValues;
        private Hashtable _ContentFilterOptionsValues;
        private Hashtable _ContentFlagValues;
        private Hashtable _ContextMenuOptionsValues;
        private Hashtable _CopyWorkerOptionsValues;
        private Hashtable _CustomizeFolderPartsValues;
        private Hashtable _FileAttributesValues;
        private Hashtable _FindDuplicateOptionsValues;
        private Hashtable _FindFormatSourceValues;
        private Hashtable _FormPlacementValues;
        private Hashtable _IconOptionsValues;
        private Hashtable _IconStyleValues;
        private Hashtable _InputDialogOptionValues;
        private Hashtable _ItemCapabilityValues;
        private Hashtable _PanelLayoutEntryValues;
        private Hashtable _PanelToolbarValues;
        private Hashtable _PathViewValues;
        private Hashtable _PK_CAPSValues;
        private Hashtable _PK_PACKValues;
        private Hashtable _QuickFindOptionsValues;
        private Hashtable _SearchFolderOptionsValues;
        private Hashtable _SevenZipFormatCapabilitiesValues;
        private Hashtable _TwoPanelLayoutEntryValues;
        private Hashtable _ViewFiltersValues;
        private string id1_ImageProvider;
        private string id10_ReleaseType;
        private string id100_CustomVirtualFolder;
        private string id101_CanMoveResult;
        private string id102_IconOptions;
        private string id103_DelayedExtractMode;
        private string id104_PathView;
        private string id105_PanelView;
        private string id106_ContextMenuOptions;
        private string id107_VirtualIcon;
        private string id108_ArrayOfPropertyValue;
        private string id109_PropertyValue;
        private string id11_ListViewColumnInfo;
        private string id110_SimpleEncrypt;
        private string id111_ProgressRenderMode;
        private string id112_ProgressState;
        private string id113_VistaProgressBarRenderer;
        private string id114_MarqueeStyle;
        private string id115_XPProgressBarRenderer;
        private string id116_AskMode;
        private string id117_OperationResult;
        private string id118_ItemPropId;
        private string id119_FileTimeType;
        private string id12_ArrayOfListViewColumnInfo;
        private string id120_ArchivePropId;
        private string id121_KnownSevenZipFormat;
        private string id122_SevenZipFormatCapabilities;
        private string id123_CompressionLevel;
        private string id124_CompressionMethod;
        private string id125_EncryptionMethod;
        private string id126_SolidSizeUnit;
        private string id127_ComplexFilterView;
        private string id128_ViewFilters;
        private string id129_PanelContentContainer;
        private string id13_PanelLayoutEntry;
        private string id130_ControllerType;
        private string id131_Controller;
        private string id132_FormPlacement;
        private string id133_ArgumentKey;
        private string id134_CanMoveListViewItem;
        private string id135_Trace;
        private string id136_SinglePanel;
        private string id137_GeneralTab;
        private string id138_TwoPanelTab;
        private string id139_ArchiveUpdateMode;
        private string id14_PanelToolbar;
        private string id140_PackStage;
        private string id141_PackProgressSnapshot;
        private string id142_CustomBackgroundWorker;
        private string id143_EventBackgroundWorker;
        private string id144_CopyDestinationItem;
        private string id145_MessageDialogResult;
        private string id146_DoubleClickAction;
        private string id147_QuickFindOptions;
        private string id148_ListViewSort;
        private string id149_CustomAsyncFolder;
        private string id15_PanelLayout;
        private string id150_SearchFolderOptions;
        private string id151_FindDuplicateOptions;
        private string id152_Compare;
        private string id153_ChangeItemAction;
        private string id154_AvailableItemActions;
        private string id155_CompareFoldersOptions;
        private string id156_OverwriteDialogResult;
        private string id157_CopyWorkerOptions;
        private string id158_CopyMode;
        private string id159_ProcessedSize;
        private string id16_ActivePanel;
        private string id160_CopyProgressSnapshot;
        private string id161_IconStyle;
        private string id162_Processed;
        private string id163_TotalCount;
        private string id164_ProcessedCount;
        private string id165_SkippedCount;
        private string id166_Duration;
        private string id167_TimeSpan;
        private string id168_CancellationPending;
        private string id169_CompressionRatio;
        private string id17_TwoPanelLayoutEntry;
        private string id170_Stage;
        private string id171_Caption;
        private string id172_Hotkey;
        private string id173_Layout;
        private string id174_Left;
        private string id175_Right;
        private string id176_AggregatedFilter;
        private string id177_NameFilter;
        private string id178_PropertyFilter;
        private string id179_ContentFilter;
        private string id18_TwoPanelLayout;
        private string id180_NameListFilter;
        private string id181_HexContentFilter;
        private string id182_AttributeFilter;
        private string id183_SizeFilter;
        private string id184_DateFilter;
        private string id185_TimeFilter;
        private string id186_Locked;
        private string id187_FolderStream;
        private string id188_FolderPath;
        private string id189_Sort;
        private string id19_CustomActionLink;
        private string id190_TimeComparision;
        private string id191_FromTime;
        private string id192_ToTime;
        private string id193_TimePart;
        private string id194_DateComparision;
        private string id195_NotOlderThan;
        private string id196_DateMeasure;
        private string id197_FromDate;
        private string id198_ToDate;
        private string id199_DatePart;
        private string id2_Item;
        private string id20_CustomBindActionLink;
        private string id200_ValueComparision;
        private string id201_FromValue;
        private string id202_ToValue;
        private string id203_SizeUnit;
        private string id204_SizeComparision;
        private string id205_IncludeAttributes;
        private string id206_ExcludeAttributes;
        private string id207_Comparision;
        private string id208_Sequence;
        private string id209_Condition;
        private string id21_ActionState;
        private string id210_Names;
        private string id211_string;
        private string id212_Options;
        private string id213_Text;
        private string id214_Encoding;
        private string id215_PropertyId;
        private string id216_StringFilter;
        private string id217_VersionFilter;
        private string id218_Int64Filter;
        private string id219_UInt32Filter;
        private string id22_BindActionProperty;
        private string id220_Int32Filter;
        private string id221_ByteFilter;
        private string id222_IntegralFilterOfByte;
        private string id223_IntegralFilterOfInt32;
        private string id224_IntegralFilterOfUInt32;
        private string id225_IntegralFilterOfInt64;
        private string id226_SimpleFilterOfVersion;
        private string id227_Version;
        private string id228_NameCondition;
        private string id229_NameComparision;
        private string id23_BreadcrumbView;
        private string id230_NamePattern;
        private string id231_Filters;
        private string id232_name;
        private string id233_OnePanel;
        private string id234_PanelsOrientation;
        private string id235_SplitterPercent;
        private string id236_StoreEntry;
        private string id237_LeftLayout;
        private string id238_RightLayout;
        private string id239_FolderBarVisible;
        private string id24_BreadcrumbToolStripRenderer;
        private string id240_FolderBarOrientation;
        private string id241_View;
        private string id242_AutoSizeColumns;
        private string id243_Columns;
        private string id244_ListColumnCount;
        private string id245_ToolbarsVisible;
        private string id246_ThumbnailSize;
        private string id247_ThumbnailSpacing;
        private string id248_DefaultWidth;
        private string id249_DisplayIndex;
        private string id25_InputDialogOption;
        private string id250_TextAlign;
        private string id251_Width;
        private string id252_Visible;
        private string id253_Property;
        private string id254_BarColor;
        private string id255_BarBackgroundColor;
        private string id256_ChunkWidth;
        private string id257_MarqueeChunks;
        private string id258_Color;
        private string id259_BackgroundColor;
        private string id26_ElevatedProcess;
        private string id260_HighlightColor;
        private string id261_StartColor;
        private string id262_EndColor;
        private string id263_DataObject;
        private string id264_PropertyName;
        private string id265_MarshalByRefObject;
        private string id266_BasicFilter;
        private string id267_CustomContentFilter;
        private string id268_ValueFilter;
        private string id269_SimpleFilterOfByte;
        private string id27_ArchiveFormatConverter;
        private string id270_SimpleFilterOfInt32;
        private string id271_SimpleFilterOfUInt32;
        private string id272_SimpleFilterOfInt64;
        private string id273_EventArgs;
        private string id274_CancelEventArgs;
        private string id275_ProviderBase;
        private string id276_SettingsProvider;
        private string id277_ToolStripRenderer;
        private string id278_ToolStripWrapperRenderer;
        private string id279_TypeConverter;
        private string id28_ArchiveFormatCapabilities;
        private string id280_PropertyTypeConverter;
        private string id281_KeysConverter;
        private string id282_HorizontalAlignment;
        private string id283_Orientation;
        private string id284_ArrayOfString;
        private string id285_DateUnit;
        private string id286_ContentFilterOptions;
        private string id287_ContentComparision;
        private string id288_SimpleComparision;
        private string id289_FileAttributes;
        private string id29_ArchiveFormatInfo;
        private string id290_NamePatternCondition;
        private string id291_NamePatternComparision;
        private string id292_ArrayOfChoice1;
        private string id293_ApplicationName;
        private string id294_KeepAlive;
        private string id295_Mark;
        private string id296_HeadSize;
        private string id297_FirstHeadSize;
        private string id298_ArjVer;
        private string id299_ArjExtrVer;
        private string id3_CustomImageProvider;
        private string id30_PersistArchiveFormatInfo;
        private string id300_HostOS;
        private string id301_Flags;
        private string id302_Method;
        private string id303_FileType;
        private string id304_Reserved;
        private string id305_ftime;
        private string id306_PackSize;
        private string id307_UnpSize;
        private string id308_CRC;
        private string id309_FileSpec;
        private string id31_FindFormatSource;
        private string id310_AccessMode;
        private string id311_HostData;
        private string id312_Cancel;
        private string id313_size;
        private string id314_PluginInterfaceVersionLow;
        private string id315_PluginInterfaceVersionHi;
        private string id316_DefaultIniName;
        private string id317_wYear;
        private string id318_wMonth;
        private string id319_wDay;
        private string id32_ArjHeader;
        private string id320_wHour;
        private string id321_wMinute;
        private string id322_wSecond;
        private string id323_FieldName;
        private string id324_FieldType;
        private string id325_Units;
        private string id326_Name;
        private string id327_IconType;
        private string id328_AlphaBlend;
        private string id329_BlendLevel;
        private string id33_ProcessItemEventArgs;
        private string id330_Icon;
        private string id331_BlendColor;
        private string id332_ForeColor;
        private string id333_InnerCircleRadius;
        private string id334_OuterCircleRadius;
        private string id335_NumberOfSpoke;
        private string id336_SpokeThickness;
        private string id337_Style;
        private string id34_ProcessorState;
        private string id35_SequenseProcessorType;
        private string id36_PK_OM;
        private string id37_PK_OPERATION;
        private string id38_PK_CAPS;
        private string id39_PK_VOL;
        private string id4_KeysConverter2;
        private string id40_PK_PACK;
        private string id41_PackDefaultParamStruct;
        private string id42_WcxErrors;
        private string id43_DefaultIcon;
        private string id44_ShellImageProvider;
        private string id45_ItemCapability;
        private string id46_LocalFileSystemCreator;
        private string id47_NetworkFileSystemCreator;
        private string id48_ShellFileSystemCreator;
        private string id49_ContentFlag;
        private string id5_PropertyTagType;
        private string id50_ContentDefaultParamStruct;
        private string id51_tdateformat;
        private string id52_ttimeformat;
        private string id53_WdxFieldInfo;
        private string id54_AggregatedFilterCondition;
        private string id55_AggregatedVirtualItemFilter;
        private string id56_FilterContainer;
        private string id57_NamedFilter;
        private string id58_FilterHelper;
        private string id59_VirtualItemNameFilter;
        private string id6_PropertyTag;
        private string id60_VirtualItemFullNameFilter;
        private string id61_VirtualItemAttributeFilter;
        private string id62_VirtualItemSizeFilter;
        private string id63_ItemDateTimePart;
        private string id64_VirtualItemDateFilter;
        private string id65_VirtualItemTimeFilter;
        private string id66_VirtualItemContentFilter;
        private string id67_VirtualItemHexContentFilter;
        private string id68_NameListCondition;
        private string id69_VirtualItemNameListFilter;
        private string id7_LightSource;
        private string id70_VirtualPropertyFilter;
        private string id71_VirtualHighligher;
        private string id72_ListViewHighlighter;
        private string id73_HighlighterIconType;
        private string id74_HashPropertyProvider;
        private string id75_VistaThumbnailProvider;
        private string id76_CustomizeFolderParts;
        private string id77_ColorSpace;
        private string id78_DescriptionPropertyProvider;
        private string id79_DummyClientSite;
        private string id8_ToolStripButtonRenderer;
        private string id80_HtmlPropertyProvider;
        private string id81_BitrateTypeConverter;
        private string id82_AudioChannelsTypeConverter;
        private string id83_AudioSampleRateTypeConverter;
        private string id84_DurationTypeConverter;
        private string id85_ImageSizeTypeConverter;
        private string id86_DPITypeConverter;
        private string id87_ISOSpeedTypeConverter;
        private string id88_RatingTypeConverter;
        private string id89_EncodingConveter;
        private string id9_ConfigurableSettingsProvider;
        private string id90_ImagePropertyProvider;
        private string id91_PsdPropertyProvider;
        private string id92_TagLibPropertyProvider;
        private string id93_TextPropertyProvider;
        private string id94_VirtualToolTip;
        private string id95_ThrobberStyle;
        private string id96_ThrobberRenderer;
        private string id97_AutoRefreshMode;
        private string id98_FtpFileSystemCreator;
        private string id99_NullFileSystemCreator;

        protected override void InitCallbacks()
        {
        }

        protected override void InitIDs()
        {
            this.id19_CustomActionLink = base.Reader.NameTable.Add("CustomActionLink");
            this.id310_AccessMode = base.Reader.NameTable.Add("AccessMode");
            this.id200_ValueComparision = base.Reader.NameTable.Add("ValueComparision");
            this.id131_Controller = base.Reader.NameTable.Add("Controller");
            this.id146_DoubleClickAction = base.Reader.NameTable.Add("DoubleClickAction");
            this.id241_View = base.Reader.NameTable.Add("View");
            this.id7_LightSource = base.Reader.NameTable.Add("LightSource");
            this.id193_TimePart = base.Reader.NameTable.Add("TimePart");
            this.id12_ArrayOfListViewColumnInfo = base.Reader.NameTable.Add("ArrayOfListViewColumnInfo");
            this.id185_TimeFilter = base.Reader.NameTable.Add("TimeFilter");
            this.id53_WdxFieldInfo = base.Reader.NameTable.Add("WdxFieldInfo");
            this.id23_BreadcrumbView = base.Reader.NameTable.Add("BreadcrumbView");
            this.id323_FieldName = base.Reader.NameTable.Add("FieldName");
            this.id177_NameFilter = base.Reader.NameTable.Add("NameFilter");
            this.id81_BitrateTypeConverter = base.Reader.NameTable.Add("BitrateTypeConverter");
            this.id252_Visible = base.Reader.NameTable.Add("Visible");
            this.id149_CustomAsyncFolder = base.Reader.NameTable.Add("CustomAsyncFolder");
            this.id253_Property = base.Reader.NameTable.Add("Property");
            this.id8_ToolStripButtonRenderer = base.Reader.NameTable.Add("ToolStripButtonRenderer");
            this.id75_VistaThumbnailProvider = base.Reader.NameTable.Add("VistaThumbnailProvider");
            this.id213_Text = base.Reader.NameTable.Add("Text");
            this.id232_name = base.Reader.NameTable.Add("name");
            this.id58_FilterHelper = base.Reader.NameTable.Add("FilterHelper");
            this.id322_wSecond = base.Reader.NameTable.Add("wSecond");
            this.id308_CRC = base.Reader.NameTable.Add("CRC");
            this.id161_IconStyle = base.Reader.NameTable.Add("IconStyle");
            this.id205_IncludeAttributes = base.Reader.NameTable.Add("IncludeAttributes");
            this.id320_wHour = base.Reader.NameTable.Add("wHour");
            this.id330_Icon = base.Reader.NameTable.Add("Icon");
            this.id191_FromTime = base.Reader.NameTable.Add("FromTime");
            this.id28_ArchiveFormatCapabilities = base.Reader.NameTable.Add("ArchiveFormatCapabilities");
            this.id116_AskMode = base.Reader.NameTable.Add("AskMode");
            this.id139_ArchiveUpdateMode = base.Reader.NameTable.Add("ArchiveUpdateMode");
            this.id248_DefaultWidth = base.Reader.NameTable.Add("DefaultWidth");
            this.id122_SevenZipFormatCapabilities = base.Reader.NameTable.Add("SevenZipFormatCapabilities");
            this.id276_SettingsProvider = base.Reader.NameTable.Add("SettingsProvider");
            this.id82_AudioChannelsTypeConverter = base.Reader.NameTable.Add("AudioChannelsTypeConverter");
            this.id246_ThumbnailSize = base.Reader.NameTable.Add("ThumbnailSize");
            this.id160_CopyProgressSnapshot = base.Reader.NameTable.Add("CopyProgressSnapshot");
            this.id152_Compare = base.Reader.NameTable.Add("Compare");
            this.id255_BarBackgroundColor = base.Reader.NameTable.Add("BarBackgroundColor");
            this.id150_SearchFolderOptions = base.Reader.NameTable.Add("SearchFolderOptions");
            this.id21_ActionState = base.Reader.NameTable.Add("ActionState");
            this.id315_PluginInterfaceVersionHi = base.Reader.NameTable.Add("PluginInterfaceVersionHi");
            this.id121_KnownSevenZipFormat = base.Reader.NameTable.Add("KnownSevenZipFormat");
            this.id332_ForeColor = base.Reader.NameTable.Add("ForeColor");
            this.id237_LeftLayout = base.Reader.NameTable.Add("LeftLayout");
            this.id260_HighlightColor = base.Reader.NameTable.Add("HighlightColor");
            this.id127_ComplexFilterView = base.Reader.NameTable.Add("ComplexFilterView");
            this.id50_ContentDefaultParamStruct = base.Reader.NameTable.Add("ContentDefaultParamStruct");
            this.id236_StoreEntry = base.Reader.NameTable.Add("StoreEntry");
            this.id175_Right = base.Reader.NameTable.Add("Right");
            this.id55_AggregatedVirtualItemFilter = base.Reader.NameTable.Add("AggregatedVirtualItemFilter");
            this.id268_ValueFilter = base.Reader.NameTable.Add("ValueFilter");
            this.id108_ArrayOfPropertyValue = base.Reader.NameTable.Add("ArrayOfPropertyValue");
            this.id32_ArjHeader = base.Reader.NameTable.Add("ArjHeader");
            this.id46_LocalFileSystemCreator = base.Reader.NameTable.Add("LocalFileSystemCreator");
            this.id6_PropertyTag = base.Reader.NameTable.Add("PropertyTag");
            this.id273_EventArgs = base.Reader.NameTable.Add("EventArgs");
            this.id279_TypeConverter = base.Reader.NameTable.Add("TypeConverter");
            this.id286_ContentFilterOptions = base.Reader.NameTable.Add("ContentFilterOptions");
            this.id335_NumberOfSpoke = base.Reader.NameTable.Add("NumberOfSpoke");
            this.id27_ArchiveFormatConverter = base.Reader.NameTable.Add("ArchiveFormatConverter");
            this.id86_DPITypeConverter = base.Reader.NameTable.Add("DPITypeConverter");
            this.id159_ProcessedSize = base.Reader.NameTable.Add("ProcessedSize");
            this.id69_VirtualItemNameListFilter = base.Reader.NameTable.Add("VirtualItemNameListFilter");
            this.id203_SizeUnit = base.Reader.NameTable.Add("SizeUnit");
            this.id133_ArgumentKey = base.Reader.NameTable.Add("ArgumentKey");
            this.id259_BackgroundColor = base.Reader.NameTable.Add("BackgroundColor");
            this.id151_FindDuplicateOptions = base.Reader.NameTable.Add("FindDuplicateOptions");
            this.id307_UnpSize = base.Reader.NameTable.Add("UnpSize");
            this.id76_CustomizeFolderParts = base.Reader.NameTable.Add("CustomizeFolderParts");
            this.id250_TextAlign = base.Reader.NameTable.Add("TextAlign");
            this.id304_Reserved = base.Reader.NameTable.Add("Reserved");
            this.id283_Orientation = base.Reader.NameTable.Add("Orientation");
            this.id194_DateComparision = base.Reader.NameTable.Add("DateComparision");
            this.id226_SimpleFilterOfVersion = base.Reader.NameTable.Add("SimpleFilterOfVersion");
            this.id269_SimpleFilterOfByte = base.Reader.NameTable.Add("SimpleFilterOfByte");
            this.id109_PropertyValue = base.Reader.NameTable.Add("PropertyValue");
            this.id277_ToolStripRenderer = base.Reader.NameTable.Add("ToolStripRenderer");
            this.id210_Names = base.Reader.NameTable.Add("Names");
            this.id196_DateMeasure = base.Reader.NameTable.Add("DateMeasure");
            this.id172_Hotkey = base.Reader.NameTable.Add("Hotkey");
            this.id73_HighlighterIconType = base.Reader.NameTable.Add("HighlighterIconType");
            this.id78_DescriptionPropertyProvider = base.Reader.NameTable.Add("DescriptionPropertyProvider");
            this.id5_PropertyTagType = base.Reader.NameTable.Add("PropertyTagType");
            this.id30_PersistArchiveFormatInfo = base.Reader.NameTable.Add("PersistArchiveFormatInfo");
            this.id101_CanMoveResult = base.Reader.NameTable.Add("CanMoveResult");
            this.id14_PanelToolbar = base.Reader.NameTable.Add("PanelToolbar");
            this.id126_SolidSizeUnit = base.Reader.NameTable.Add("SolidSizeUnit");
            this.id168_CancellationPending = base.Reader.NameTable.Add("CancellationPending");
            this.id143_EventBackgroundWorker = base.Reader.NameTable.Add("EventBackgroundWorker");
            this.id186_Locked = base.Reader.NameTable.Add("Locked");
            this.id245_ToolbarsVisible = base.Reader.NameTable.Add("ToolbarsVisible");
            this.id240_FolderBarOrientation = base.Reader.NameTable.Add("FolderBarOrientation");
            this.id294_KeepAlive = base.Reader.NameTable.Add("KeepAlive");
            this.id43_DefaultIcon = base.Reader.NameTable.Add("DefaultIcon");
            this.id97_AutoRefreshMode = base.Reader.NameTable.Add("AutoRefreshMode");
            this.id136_SinglePanel = base.Reader.NameTable.Add("SinglePanel");
            this.id238_RightLayout = base.Reader.NameTable.Add("RightLayout");
            this.id208_Sequence = base.Reader.NameTable.Add("Sequence");
            this.id180_NameListFilter = base.Reader.NameTable.Add("NameListFilter");
            this.id169_CompressionRatio = base.Reader.NameTable.Add("CompressionRatio");
            this.id235_SplitterPercent = base.Reader.NameTable.Add("SplitterPercent");
            this.id100_CustomVirtualFolder = base.Reader.NameTable.Add("CustomVirtualFolder");
            this.id243_Columns = base.Reader.NameTable.Add("Columns");
            this.id275_ProviderBase = base.Reader.NameTable.Add("ProviderBase");
            this.id265_MarshalByRefObject = base.Reader.NameTable.Add("MarshalByRefObject");
            this.id137_GeneralTab = base.Reader.NameTable.Add("GeneralTab");
            this.id309_FileSpec = base.Reader.NameTable.Add("FileSpec");
            this.id63_ItemDateTimePart = base.Reader.NameTable.Add("ItemDateTimePart");
            this.id221_ByteFilter = base.Reader.NameTable.Add("ByteFilter");
            this.id103_DelayedExtractMode = base.Reader.NameTable.Add("DelayedExtractMode");
            this.id257_MarqueeChunks = base.Reader.NameTable.Add("MarqueeChunks");
            this.id202_ToValue = base.Reader.NameTable.Add("ToValue");
            this.id111_ProgressRenderMode = base.Reader.NameTable.Add("ProgressRenderMode");
            this.id228_NameCondition = base.Reader.NameTable.Add("NameCondition");
            this.id132_FormPlacement = base.Reader.NameTable.Add("FormPlacement");
            this.id327_IconType = base.Reader.NameTable.Add("IconType");
            this.id211_string = base.Reader.NameTable.Add("string");
            this.id231_Filters = base.Reader.NameTable.Add("Filters");
            this.id199_DatePart = base.Reader.NameTable.Add("DatePart");
            this.id118_ItemPropId = base.Reader.NameTable.Add("ItemPropId");
            this.id301_Flags = base.Reader.NameTable.Add("Flags");
            this.id115_XPProgressBarRenderer = base.Reader.NameTable.Add("XPProgressBarRenderer");
            this.id13_PanelLayoutEntry = base.Reader.NameTable.Add("PanelLayoutEntry");
            this.id70_VirtualPropertyFilter = base.Reader.NameTable.Add("VirtualPropertyFilter");
            this.id11_ListViewColumnInfo = base.Reader.NameTable.Add("ListViewColumnInfo");
            this.id85_ImageSizeTypeConverter = base.Reader.NameTable.Add("ImageSizeTypeConverter");
            this.id17_TwoPanelLayoutEntry = base.Reader.NameTable.Add("TwoPanelLayoutEntry");
            this.id321_wMinute = base.Reader.NameTable.Add("wMinute");
            this.id263_DataObject = base.Reader.NameTable.Add("DataObject");
            this.id113_VistaProgressBarRenderer = base.Reader.NameTable.Add("VistaProgressBarRenderer");
            this.id333_InnerCircleRadius = base.Reader.NameTable.Add("InnerCircleRadius");
            this.id295_Mark = base.Reader.NameTable.Add("Mark");
            this.id80_HtmlPropertyProvider = base.Reader.NameTable.Add("HtmlPropertyProvider");
            this.id318_wMonth = base.Reader.NameTable.Add("wMonth");
            this.id36_PK_OM = base.Reader.NameTable.Add("PK_OM");
            this.id2_Item = base.Reader.NameTable.Add("");
            this.id270_SimpleFilterOfInt32 = base.Reader.NameTable.Add("SimpleFilterOfInt32");
            this.id222_IntegralFilterOfByte = base.Reader.NameTable.Add("IntegralFilterOfByte");
            this.id190_TimeComparision = base.Reader.NameTable.Add("TimeComparision");
            this.id22_BindActionProperty = base.Reader.NameTable.Add("BindActionProperty");
            this.id331_BlendColor = base.Reader.NameTable.Add("BlendColor");
            this.id165_SkippedCount = base.Reader.NameTable.Add("SkippedCount");
            this.id272_SimpleFilterOfInt64 = base.Reader.NameTable.Add("SimpleFilterOfInt64");
            this.id264_PropertyName = base.Reader.NameTable.Add("PropertyName");
            this.id223_IntegralFilterOfInt32 = base.Reader.NameTable.Add("IntegralFilterOfInt32");
            this.id292_ArrayOfChoice1 = base.Reader.NameTable.Add("ArrayOfChoice1");
            this.id280_PropertyTypeConverter = base.Reader.NameTable.Add("PropertyTypeConverter");
            this.id49_ContentFlag = base.Reader.NameTable.Add("ContentFlag");
            this.id138_TwoPanelTab = base.Reader.NameTable.Add("TwoPanelTab");
            this.id326_Name = base.Reader.NameTable.Add("Name");
            this.id297_FirstHeadSize = base.Reader.NameTable.Add("FirstHeadSize");
            this.id206_ExcludeAttributes = base.Reader.NameTable.Add("ExcludeAttributes");
            this.id64_VirtualItemDateFilter = base.Reader.NameTable.Add("VirtualItemDateFilter");
            this.id187_FolderStream = base.Reader.NameTable.Add("FolderStream");
            this.id254_BarColor = base.Reader.NameTable.Add("BarColor");
            this.id145_MessageDialogResult = base.Reader.NameTable.Add("MessageDialogResult");
            this.id317_wYear = base.Reader.NameTable.Add("wYear");
            this.id45_ItemCapability = base.Reader.NameTable.Add("ItemCapability");
            this.id303_FileType = base.Reader.NameTable.Add("FileType");
            this.id102_IconOptions = base.Reader.NameTable.Add("IconOptions");
            this.id197_FromDate = base.Reader.NameTable.Add("FromDate");
            this.id130_ControllerType = base.Reader.NameTable.Add("ControllerType");
            this.id214_Encoding = base.Reader.NameTable.Add("Encoding");
            this.id125_EncryptionMethod = base.Reader.NameTable.Add("EncryptionMethod");
            this.id42_WcxErrors = base.Reader.NameTable.Add("WcxErrors");
            this.id325_Units = base.Reader.NameTable.Add("Units");
            this.id313_size = base.Reader.NameTable.Add("size");
            this.id216_StringFilter = base.Reader.NameTable.Add("StringFilter");
            this.id288_SimpleComparision = base.Reader.NameTable.Add("SimpleComparision");
            this.id77_ColorSpace = base.Reader.NameTable.Add("ColorSpace");
            this.id114_MarqueeStyle = base.Reader.NameTable.Add("MarqueeStyle");
            this.id142_CustomBackgroundWorker = base.Reader.NameTable.Add("CustomBackgroundWorker");
            this.id181_HexContentFilter = base.Reader.NameTable.Add("HexContentFilter");
            this.id290_NamePatternCondition = base.Reader.NameTable.Add("NamePatternCondition");
            this.id319_wDay = base.Reader.NameTable.Add("wDay");
            this.id229_NameComparision = base.Reader.NameTable.Add("NameComparision");
            this.id110_SimpleEncrypt = base.Reader.NameTable.Add("SimpleEncrypt");
            this.id244_ListColumnCount = base.Reader.NameTable.Add("ListColumnCount");
            this.id249_DisplayIndex = base.Reader.NameTable.Add("DisplayIndex");
            this.id61_VirtualItemAttributeFilter = base.Reader.NameTable.Add("VirtualItemAttributeFilter");
            this.id93_TextPropertyProvider = base.Reader.NameTable.Add("TextPropertyProvider");
            this.id99_NullFileSystemCreator = base.Reader.NameTable.Add("NullFileSystemCreator");
            this.id84_DurationTypeConverter = base.Reader.NameTable.Add("DurationTypeConverter");
            this.id154_AvailableItemActions = base.Reader.NameTable.Add("AvailableItemActions");
            this.id176_AggregatedFilter = base.Reader.NameTable.Add("AggregatedFilter");
            this.id234_PanelsOrientation = base.Reader.NameTable.Add("PanelsOrientation");
            this.id306_PackSize = base.Reader.NameTable.Add("PackSize");
            this.id56_FilterContainer = base.Reader.NameTable.Add("FilterContainer");
            this.id298_ArjVer = base.Reader.NameTable.Add("ArjVer");
            this.id278_ToolStripWrapperRenderer = base.Reader.NameTable.Add("ToolStripWrapperRenderer");
            this.id31_FindFormatSource = base.Reader.NameTable.Add("FindFormatSource");
            this.id314_PluginInterfaceVersionLow = base.Reader.NameTable.Add("PluginInterfaceVersionLow");
            this.id1_ImageProvider = base.Reader.NameTable.Add("ImageProvider");
            this.id285_DateUnit = base.Reader.NameTable.Add("DateUnit");
            this.id148_ListViewSort = base.Reader.NameTable.Add("ListViewSort");
            this.id89_EncodingConveter = base.Reader.NameTable.Add("EncodingConveter");
            this.id20_CustomBindActionLink = base.Reader.NameTable.Add("CustomBindActionLink");
            this.id157_CopyWorkerOptions = base.Reader.NameTable.Add("CopyWorkerOptions");
            this.id256_ChunkWidth = base.Reader.NameTable.Add("ChunkWidth");
            this.id287_ContentComparision = base.Reader.NameTable.Add("ContentComparision");
            this.id291_NamePatternComparision = base.Reader.NameTable.Add("NamePatternComparision");
            this.id54_AggregatedFilterCondition = base.Reader.NameTable.Add("AggregatedFilterCondition");
            this.id233_OnePanel = base.Reader.NameTable.Add("OnePanel");
            this.id305_ftime = base.Reader.NameTable.Add("ftime");
            this.id71_VirtualHighligher = base.Reader.NameTable.Add("VirtualHighligher");
            this.id184_DateFilter = base.Reader.NameTable.Add("DateFilter");
            this.id24_BreadcrumbToolStripRenderer = base.Reader.NameTable.Add("BreadcrumbToolStripRenderer");
            this.id289_FileAttributes = base.Reader.NameTable.Add("FileAttributes");
            this.id224_IntegralFilterOfUInt32 = base.Reader.NameTable.Add("IntegralFilterOfUInt32");
            this.id170_Stage = base.Reader.NameTable.Add("Stage");
            this.id104_PathView = base.Reader.NameTable.Add("PathView");
            this.id140_PackStage = base.Reader.NameTable.Add("PackStage");
            this.id57_NamedFilter = base.Reader.NameTable.Add("NamedFilter");
            this.id218_Int64Filter = base.Reader.NameTable.Add("Int64Filter");
            this.id72_ListViewHighlighter = base.Reader.NameTable.Add("ListViewHighlighter");
            this.id261_StartColor = base.Reader.NameTable.Add("StartColor");
            this.id274_CancelEventArgs = base.Reader.NameTable.Add("CancelEventArgs");
            this.id173_Layout = base.Reader.NameTable.Add("Layout");
            this.id67_VirtualItemHexContentFilter = base.Reader.NameTable.Add("VirtualItemHexContentFilter");
            this.id60_VirtualItemFullNameFilter = base.Reader.NameTable.Add("VirtualItemFullNameFilter");
            this.id59_VirtualItemNameFilter = base.Reader.NameTable.Add("VirtualItemNameFilter");
            this.id47_NetworkFileSystemCreator = base.Reader.NameTable.Add("NetworkFileSystemCreator");
            this.id311_HostData = base.Reader.NameTable.Add("HostData");
            this.id204_SizeComparision = base.Reader.NameTable.Add("SizeComparision");
            this.id217_VersionFilter = base.Reader.NameTable.Add("VersionFilter");
            this.id95_ThrobberStyle = base.Reader.NameTable.Add("ThrobberStyle");
            this.id242_AutoSizeColumns = base.Reader.NameTable.Add("AutoSizeColumns");
            this.id4_KeysConverter2 = base.Reader.NameTable.Add("KeysConverter2");
            this.id239_FolderBarVisible = base.Reader.NameTable.Add("FolderBarVisible");
            this.id18_TwoPanelLayout = base.Reader.NameTable.Add("TwoPanelLayout");
            this.id155_CompareFoldersOptions = base.Reader.NameTable.Add("CompareFoldersOptions");
            this.id37_PK_OPERATION = base.Reader.NameTable.Add("PK_OPERATION");
            this.id166_Duration = base.Reader.NameTable.Add("Duration");
            this.id16_ActivePanel = base.Reader.NameTable.Add("ActivePanel");
            this.id90_ImagePropertyProvider = base.Reader.NameTable.Add("ImagePropertyProvider");
            this.id189_Sort = base.Reader.NameTable.Add("Sort");
            this.id300_HostOS = base.Reader.NameTable.Add("HostOS");
            this.id212_Options = base.Reader.NameTable.Add("Options");
            this.id119_FileTimeType = base.Reader.NameTable.Add("FileTimeType");
            this.id68_NameListCondition = base.Reader.NameTable.Add("NameListCondition");
            this.id9_ConfigurableSettingsProvider = base.Reader.NameTable.Add("ConfigurableSettingsProvider");
            this.id26_ElevatedProcess = base.Reader.NameTable.Add("ElevatedProcess");
            this.id123_CompressionLevel = base.Reader.NameTable.Add("CompressionLevel");
            this.id230_NamePattern = base.Reader.NameTable.Add("NamePattern");
            this.id198_ToDate = base.Reader.NameTable.Add("ToDate");
            this.id25_InputDialogOption = base.Reader.NameTable.Add("InputDialogOption");
            this.id188_FolderPath = base.Reader.NameTable.Add("FolderPath");
            this.id329_BlendLevel = base.Reader.NameTable.Add("BlendLevel");
            this.id178_PropertyFilter = base.Reader.NameTable.Add("PropertyFilter");
            this.id40_PK_PACK = base.Reader.NameTable.Add("PK_PACK");
            this.id183_SizeFilter = base.Reader.NameTable.Add("SizeFilter");
            this.id167_TimeSpan = base.Reader.NameTable.Add("TimeSpan");
            this.id94_VirtualToolTip = base.Reader.NameTable.Add("VirtualToolTip");
            this.id38_PK_CAPS = base.Reader.NameTable.Add("PK_CAPS");
            this.id128_ViewFilters = base.Reader.NameTable.Add("ViewFilters");
            this.id158_CopyMode = base.Reader.NameTable.Add("CopyMode");
            this.id220_Int32Filter = base.Reader.NameTable.Add("Int32Filter");
            this.id96_ThrobberRenderer = base.Reader.NameTable.Add("ThrobberRenderer");
            this.id328_AlphaBlend = base.Reader.NameTable.Add("AlphaBlend");
            this.id174_Left = base.Reader.NameTable.Add("Left");
            this.id91_PsdPropertyProvider = base.Reader.NameTable.Add("PsdPropertyProvider");
            this.id179_ContentFilter = base.Reader.NameTable.Add("ContentFilter");
            this.id195_NotOlderThan = base.Reader.NameTable.Add("NotOlderThan");
            this.id15_PanelLayout = base.Reader.NameTable.Add("PanelLayout");
            this.id10_ReleaseType = base.Reader.NameTable.Add("ReleaseType");
            this.id201_FromValue = base.Reader.NameTable.Add("FromValue");
            this.id293_ApplicationName = base.Reader.NameTable.Add("ApplicationName");
            this.id266_BasicFilter = base.Reader.NameTable.Add("BasicFilter");
            this.id182_AttributeFilter = base.Reader.NameTable.Add("AttributeFilter");
            this.id171_Caption = base.Reader.NameTable.Add("Caption");
            this.id262_EndColor = base.Reader.NameTable.Add("EndColor");
            this.id153_ChangeItemAction = base.Reader.NameTable.Add("ChangeItemAction");
            this.id284_ArrayOfString = base.Reader.NameTable.Add("ArrayOfString");
            this.id312_Cancel = base.Reader.NameTable.Add("Cancel");
            this.id62_VirtualItemSizeFilter = base.Reader.NameTable.Add("VirtualItemSizeFilter");
            this.id134_CanMoveListViewItem = base.Reader.NameTable.Add("CanMoveListViewItem");
            this.id296_HeadSize = base.Reader.NameTable.Add("HeadSize");
            this.id107_VirtualIcon = base.Reader.NameTable.Add("VirtualIcon");
            this.id29_ArchiveFormatInfo = base.Reader.NameTable.Add("ArchiveFormatInfo");
            this.id83_AudioSampleRateTypeConverter = base.Reader.NameTable.Add("AudioSampleRateTypeConverter");
            this.id251_Width = base.Reader.NameTable.Add("Width");
            this.id88_RatingTypeConverter = base.Reader.NameTable.Add("RatingTypeConverter");
            this.id33_ProcessItemEventArgs = base.Reader.NameTable.Add("ProcessItemEventArgs");
            this.id247_ThumbnailSpacing = base.Reader.NameTable.Add("ThumbnailSpacing");
            this.id129_PanelContentContainer = base.Reader.NameTable.Add("PanelContentContainer");
            this.id35_SequenseProcessorType = base.Reader.NameTable.Add("SequenseProcessorType");
            this.id163_TotalCount = base.Reader.NameTable.Add("TotalCount");
            this.id282_HorizontalAlignment = base.Reader.NameTable.Add("HorizontalAlignment");
            this.id227_Version = base.Reader.NameTable.Add("Version");
            this.id66_VirtualItemContentFilter = base.Reader.NameTable.Add("VirtualItemContentFilter");
            this.id316_DefaultIniName = base.Reader.NameTable.Add("DefaultIniName");
            this.id281_KeysConverter = base.Reader.NameTable.Add("KeysConverter");
            this.id124_CompressionMethod = base.Reader.NameTable.Add("CompressionMethod");
            this.id219_UInt32Filter = base.Reader.NameTable.Add("UInt32Filter");
            this.id44_ShellImageProvider = base.Reader.NameTable.Add("ShellImageProvider");
            this.id162_Processed = base.Reader.NameTable.Add("Processed");
            this.id92_TagLibPropertyProvider = base.Reader.NameTable.Add("TagLibPropertyProvider");
            this.id156_OverwriteDialogResult = base.Reader.NameTable.Add("OverwriteDialogResult");
            this.id105_PanelView = base.Reader.NameTable.Add("PanelView");
            this.id41_PackDefaultParamStruct = base.Reader.NameTable.Add("PackDefaultParamStruct");
            this.id112_ProgressState = base.Reader.NameTable.Add("ProgressState");
            this.id39_PK_VOL = base.Reader.NameTable.Add("PK_VOL");
            this.id337_Style = base.Reader.NameTable.Add("Style");
            this.id192_ToTime = base.Reader.NameTable.Add("ToTime");
            this.id48_ShellFileSystemCreator = base.Reader.NameTable.Add("ShellFileSystemCreator");
            this.id164_ProcessedCount = base.Reader.NameTable.Add("ProcessedCount");
            this.id74_HashPropertyProvider = base.Reader.NameTable.Add("HashPropertyProvider");
            this.id215_PropertyId = base.Reader.NameTable.Add("PropertyId");
            this.id3_CustomImageProvider = base.Reader.NameTable.Add("CustomImageProvider");
            this.id141_PackProgressSnapshot = base.Reader.NameTable.Add("PackProgressSnapshot");
            this.id258_Color = base.Reader.NameTable.Add("Color");
            this.id106_ContextMenuOptions = base.Reader.NameTable.Add("ContextMenuOptions");
            this.id267_CustomContentFilter = base.Reader.NameTable.Add("CustomContentFilter");
            this.id98_FtpFileSystemCreator = base.Reader.NameTable.Add("FtpFileSystemCreator");
            this.id120_ArchivePropId = base.Reader.NameTable.Add("ArchivePropId");
            this.id336_SpokeThickness = base.Reader.NameTable.Add("SpokeThickness");
            this.id52_ttimeformat = base.Reader.NameTable.Add("ttimeformat");
            this.id299_ArjExtrVer = base.Reader.NameTable.Add("ArjExtrVer");
            this.id79_DummyClientSite = base.Reader.NameTable.Add("DummyClientSite");
            this.id302_Method = base.Reader.NameTable.Add("Method");
            this.id34_ProcessorState = base.Reader.NameTable.Add("ProcessorState");
            this.id65_VirtualItemTimeFilter = base.Reader.NameTable.Add("VirtualItemTimeFilter");
            this.id324_FieldType = base.Reader.NameTable.Add("FieldType");
            this.id117_OperationResult = base.Reader.NameTable.Add("OperationResult");
            this.id144_CopyDestinationItem = base.Reader.NameTable.Add("CopyDestinationItem");
            this.id87_ISOSpeedTypeConverter = base.Reader.NameTable.Add("ISOSpeedTypeConverter");
            this.id51_tdateformat = base.Reader.NameTable.Add("tdateformat");
            this.id334_OuterCircleRadius = base.Reader.NameTable.Add("OuterCircleRadius");
            this.id147_QuickFindOptions = base.Reader.NameTable.Add("QuickFindOptions");
            this.id207_Comparision = base.Reader.NameTable.Add("Comparision");
            this.id271_SimpleFilterOfUInt32 = base.Reader.NameTable.Add("SimpleFilterOfUInt32");
            this.id209_Condition = base.Reader.NameTable.Add("Condition");
            this.id225_IntegralFilterOfInt64 = base.Reader.NameTable.Add("IntegralFilterOfInt64");
            this.id135_Trace = base.Reader.NameTable.Add("Trace");
        }

        private object Read1_Object(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if (checkType)
            {
                if (flag)
                {
                    if (type != null)
                    {
                        return base.ReadTypedNull(type);
                    }
                    return null;
                }
                if (type == null)
                {
                    return base.ReadTypedPrimitive(new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema"));
                }
                if ((type.Name == this.id160_CopyProgressSnapshot) && (type.Namespace == this.id2_Item))
                {
                    return this.Read204_CopyProgressSnapshot(isNullable, false);
                }
                if ((type.Name == this.id142_CustomBackgroundWorker) && (type.Namespace == this.id2_Item))
                {
                    return this.Read188_CustomBackgroundWorker(isNullable, false);
                }
                if ((type.Name == this.id143_EventBackgroundWorker) && (type.Namespace == this.id2_Item))
                {
                    return this.Read189_EventBackgroundWorker(isNullable, false);
                }
                if ((type.Name == this.id149_CustomAsyncFolder) && (type.Namespace == this.id2_Item))
                {
                    return this.Read194_CustomAsyncFolder(isNullable, false);
                }
                if ((type.Name == this.id141_PackProgressSnapshot) && (type.Namespace == this.id2_Item))
                {
                    return this.Read187_PackProgressSnapshot(isNullable, false);
                }
                if ((type.Name == this.id167_TimeSpan) && (type.Namespace == this.id2_Item))
                {
                    return this.Read186_TimeSpan(false);
                }
                if ((type.Name == this.id159_ProcessedSize) && (type.Namespace == this.id2_Item))
                {
                    return this.Read185_ProcessedSize(false);
                }
                if ((type.Name == this.id137_GeneralTab) && (type.Namespace == this.id2_Item))
                {
                    return this.Read181_GeneralTab(isNullable, false);
                }
                if ((type.Name == this.id138_TwoPanelTab) && (type.Namespace == this.id2_Item))
                {
                    return this.Read182_TwoPanelTab(isNullable, false);
                }
                if ((type.Name == this.id135_Trace) && (type.Namespace == this.id2_Item))
                {
                    return this.Read179_Trace(isNullable, false);
                }
                if ((type.Name == this.id265_MarshalByRefObject) && (type.Namespace == this.id2_Item))
                {
                    return this.Read174_MarshalByRefObject(isNullable, false);
                }
                if ((type.Name == this.id131_Controller) && (type.Namespace == this.id2_Item))
                {
                    return this.Read175_Controller(isNullable, false);
                }
                if ((type.Name == this.id115_XPProgressBarRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read157_XPProgressBarRenderer(isNullable, false);
                }
                if ((type.Name == this.id113_VistaProgressBarRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read155_VistaProgressBarRenderer(isNullable, false);
                }
                if ((type.Name == this.id110_SimpleEncrypt) && (type.Namespace == this.id2_Item))
                {
                    return this.Read152_SimpleEncrypt(isNullable, false);
                }
                if ((type.Name == this.id109_PropertyValue) && (type.Namespace == this.id2_Item))
                {
                    return this.Read151_PropertyValue(isNullable, false);
                }
                if ((type.Name == this.id107_VirtualIcon) && (type.Namespace == this.id2_Item))
                {
                    return this.Read150_VirtualIcon(isNullable, false);
                }
                if ((type.Name == this.id100_CustomVirtualFolder) && (type.Namespace == this.id2_Item))
                {
                    return this.Read144_CustomVirtualFolder(isNullable, false);
                }
                if ((type.Name == this.id99_NullFileSystemCreator) && (type.Namespace == this.id2_Item))
                {
                    return this.Read143_NullFileSystemCreator(isNullable, false);
                }
                if ((type.Name == this.id98_FtpFileSystemCreator) && (type.Namespace == this.id2_Item))
                {
                    return this.Read142_FtpFileSystemCreator(isNullable, false);
                }
                if ((type.Name == this.id96_ThrobberRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read140_ThrobberRenderer(isNullable, false);
                }
                if ((type.Name == this.id258_Color) && (type.Namespace == this.id2_Item))
                {
                    return this.Read139_Color(false);
                }
                if ((type.Name == this.id94_VirtualToolTip) && (type.Namespace == this.id2_Item))
                {
                    return this.Read137_VirtualToolTip(isNullable, false);
                }
                if ((type.Name == this.id93_TextPropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read136_TextPropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id92_TagLibPropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read135_TagLibPropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id91_PsdPropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read134_PsdPropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id90_ImagePropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read133_ImagePropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id80_HtmlPropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read122_HtmlPropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id79_DummyClientSite) && (type.Namespace == this.id2_Item))
                {
                    return this.Read121_DummyClientSite(isNullable, false);
                }
                if ((type.Name == this.id78_DescriptionPropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read120_DescriptionPropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id75_VistaThumbnailProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read117_VistaThumbnailProvider(isNullable, false);
                }
                if ((type.Name == this.id74_HashPropertyProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read116_HashPropertyProvider(isNullable, false);
                }
                if ((type.Name == this.id58_FilterHelper) && (type.Namespace == this.id2_Item))
                {
                    return this.Read111_FilterHelper(isNullable, false);
                }
                if ((type.Name == this.id56_FilterContainer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read109_FilterContainer(isNullable, false);
                }
                if ((type.Name == this.id129_PanelContentContainer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read172_PanelContentContainer(isNullable, false);
                }
                if ((type.Name == this.id57_NamedFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read110_NamedFilter(isNullable, false);
                }
                if ((type.Name == this.id71_VirtualHighligher) && (type.Namespace == this.id2_Item))
                {
                    return this.Read114_VirtualHighligher(isNullable, false);
                }
                if ((type.Name == this.id72_ListViewHighlighter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read115_ListViewHighlighter(isNullable, false);
                }
                if ((type.Name == this.id227_Version) && (type.Namespace == this.id2_Item))
                {
                    return this.Read74_Version(isNullable, false);
                }
                if ((type.Name == this.id266_BasicFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read65_BasicFilter(isNullable, false);
                }
                if ((type.Name == this.id69_VirtualItemNameListFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read107_VirtualItemNameListFilter(isNullable, false);
                }
                if ((type.Name == this.id177_NameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read102_NameFilter(isNullable, false);
                }
                if ((type.Name == this.id60_VirtualItemFullNameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read112_VirtualItemFullNameFilter(isNullable, false);
                }
                if ((type.Name == this.id59_VirtualItemNameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read103_VirtualItemNameFilter(isNullable, false);
                }
                if ((type.Name == this.id182_AttributeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read93_AttributeFilter(isNullable, false);
                }
                if ((type.Name == this.id61_VirtualItemAttributeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read94_VirtualItemAttributeFilter(isNullable, false);
                }
                if ((type.Name == this.id267_CustomContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read85_CustomContentFilter(isNullable, false);
                }
                if ((type.Name == this.id181_HexContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read104_HexContentFilter(isNullable, false);
                }
                if ((type.Name == this.id67_VirtualItemHexContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read105_VirtualItemHexContentFilter(isNullable, false);
                }
                if ((type.Name == this.id179_ContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read86_ContentFilter(isNullable, false);
                }
                if ((type.Name == this.id66_VirtualItemContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read87_VirtualItemContentFilter(isNullable, false);
                }
                if ((type.Name == this.id268_ValueFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read66_ValueFilter(isNullable, false);
                }
                if ((type.Name == this.id185_TimeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read89_TimeFilter(isNullable, false);
                }
                if ((type.Name == this.id65_VirtualItemTimeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read91_VirtualItemTimeFilter(isNullable, false);
                }
                if ((type.Name == this.id269_SimpleFilterOfByte) && (type.Namespace == this.id2_Item))
                {
                    return this.Read82_SimpleFilterOfByte(isNullable, false);
                }
                if ((type.Name == this.id222_IntegralFilterOfByte) && (type.Namespace == this.id2_Item))
                {
                    return this.Read83_IntegralFilterOfByte(isNullable, false);
                }
                if ((type.Name == this.id270_SimpleFilterOfInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read80_SimpleFilterOfInt32(isNullable, false);
                }
                if ((type.Name == this.id223_IntegralFilterOfInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read81_IntegralFilterOfInt32(isNullable, false);
                }
                if ((type.Name == this.id271_SimpleFilterOfUInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read78_SimpleFilterOfUInt32(isNullable, false);
                }
                if ((type.Name == this.id224_IntegralFilterOfUInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read79_IntegralFilterOfUInt32(isNullable, false);
                }
                if ((type.Name == this.id272_SimpleFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read76_SimpleFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id225_IntegralFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read77_IntegralFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id183_SizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read97_SizeFilter(isNullable, false);
                }
                if ((type.Name == this.id62_VirtualItemSizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read98_VirtualItemSizeFilter(isNullable, false);
                }
                if ((type.Name == this.id226_SimpleFilterOfVersion) && (type.Namespace == this.id2_Item))
                {
                    return this.Read75_SimpleFilterOfVersion(isNullable, false);
                }
                if ((type.Name == this.id216_StringFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read72_StringFilter(isNullable, false);
                }
                if ((type.Name == this.id184_DateFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read69_DateFilter(isNullable, false);
                }
                if ((type.Name == this.id64_VirtualItemDateFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read99_VirtualItemDateFilter(isNullable, false);
                }
                if ((type.Name == this.id70_VirtualPropertyFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read84_VirtualPropertyFilter(isNullable, false);
                }
                if ((type.Name == this.id55_AggregatedVirtualItemFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read108_AggregatedVirtualItemFilter(isNullable, false);
                }
                if ((type.Name == this.id53_WdxFieldInfo) && (type.Namespace == this.id2_Item))
                {
                    return this.Read63_WdxFieldInfo(isNullable, false);
                }
                if ((type.Name == this.id52_ttimeformat) && (type.Namespace == this.id2_Item))
                {
                    return this.Read62_ttimeformat(false);
                }
                if ((type.Name == this.id51_tdateformat) && (type.Namespace == this.id2_Item))
                {
                    return this.Read61_tdateformat(false);
                }
                if ((type.Name == this.id50_ContentDefaultParamStruct) && (type.Namespace == this.id2_Item))
                {
                    return this.Read60_ContentDefaultParamStruct(false);
                }
                if ((type.Name == this.id48_ShellFileSystemCreator) && (type.Namespace == this.id2_Item))
                {
                    return this.Read58_ShellFileSystemCreator(isNullable, false);
                }
                if ((type.Name == this.id47_NetworkFileSystemCreator) && (type.Namespace == this.id2_Item))
                {
                    return this.Read57_NetworkFileSystemCreator(isNullable, false);
                }
                if ((type.Name == this.id46_LocalFileSystemCreator) && (type.Namespace == this.id2_Item))
                {
                    return this.Read56_LocalFileSystemCreator(isNullable, false);
                }
                if ((type.Name == this.id42_WcxErrors) && (type.Namespace == this.id2_Item))
                {
                    return this.Read52_WcxErrors(isNullable, false);
                }
                if ((type.Name == this.id41_PackDefaultParamStruct) && (type.Namespace == this.id2_Item))
                {
                    return this.Read51_PackDefaultParamStruct(false);
                }
                if ((type.Name == this.id273_EventArgs) && (type.Namespace == this.id2_Item))
                {
                    return this.Read41_EventArgs(isNullable, false);
                }
                if ((type.Name == this.id274_CancelEventArgs) && (type.Namespace == this.id2_Item))
                {
                    return this.Read42_CancelEventArgs(isNullable, false);
                }
                if ((type.Name == this.id33_ProcessItemEventArgs) && (type.Namespace == this.id2_Item))
                {
                    return this.Read43_ProcessItemEventArgs(isNullable, false);
                }
                if ((type.Name == this.id32_ArjHeader) && (type.Namespace == this.id2_Item))
                {
                    return this.Read40_ArjHeader(false);
                }
                if ((type.Name == this.id29_ArchiveFormatInfo) && (type.Namespace == this.id2_Item))
                {
                    return this.Read37_ArchiveFormatInfo(isNullable, false);
                }
                if ((type.Name == this.id30_PersistArchiveFormatInfo) && (type.Namespace == this.id2_Item))
                {
                    return this.Read38_PersistArchiveFormatInfo(isNullable, false);
                }
                if ((type.Name == this.id26_ElevatedProcess) && (type.Namespace == this.id2_Item))
                {
                    return this.Read34_ElevatedProcess(isNullable, false);
                }
                if ((type.Name == this.id19_CustomActionLink) && (type.Namespace == this.id2_Item))
                {
                    return this.Read26_CustomActionLink(isNullable, false);
                }
                if ((type.Name == this.id20_CustomBindActionLink) && (type.Namespace == this.id2_Item))
                {
                    return this.Read27_CustomBindActionLink(isNullable, false);
                }
                if ((type.Name == this.id18_TwoPanelLayout) && (type.Namespace == this.id2_Item))
                {
                    return this.Read25_TwoPanelLayout(isNullable, false);
                }
                if ((type.Name == this.id15_PanelLayout) && (type.Namespace == this.id2_Item))
                {
                    return this.Read22_PanelLayout(isNullable, false);
                }
                if ((type.Name == this.id11_ListViewColumnInfo) && (type.Namespace == this.id2_Item))
                {
                    return this.Read17_ListViewColumnInfo(isNullable, false);
                }
                if ((type.Name == this.id275_ProviderBase) && (type.Namespace == this.id2_Item))
                {
                    return this.Read12_ProviderBase(isNullable, false);
                }
                if ((type.Name == this.id276_SettingsProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read13_SettingsProvider(isNullable, false);
                }
                if ((type.Name == this.id9_ConfigurableSettingsProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read14_ConfigurableSettingsProvider(isNullable, false);
                }
                if ((type.Name == this.id277_ToolStripRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read10_ToolStripRenderer(isNullable, false);
                }
                if ((type.Name == this.id278_ToolStripWrapperRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read31_ToolStripWrapperRenderer(isNullable, false);
                }
                if ((type.Name == this.id24_BreadcrumbToolStripRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read32_BreadcrumbToolStripRenderer(isNullable, false);
                }
                if ((type.Name == this.id8_ToolStripButtonRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read11_ToolStripButtonRenderer(isNullable, false);
                }
                if ((type.Name == this.id279_TypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read4_TypeConverter(isNullable, false);
                }
                if ((type.Name == this.id89_EncodingConveter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read132_EncodingConveter(isNullable, false);
                }
                if ((type.Name == this.id82_AudioChannelsTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read125_AudioChannelsTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id280_PropertyTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read123_PropertyTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id88_RatingTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read131_RatingTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id87_ISOSpeedTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read130_ISOSpeedTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id86_DPITypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read129_DPITypeConverter(isNullable, false);
                }
                if ((type.Name == this.id85_ImageSizeTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read128_ImageSizeTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id84_DurationTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read127_DurationTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id83_AudioSampleRateTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read126_AudioSampleRateTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id81_BitrateTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read124_BitrateTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id27_ArchiveFormatConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read35_ArchiveFormatConverter(isNullable, false);
                }
                if ((type.Name == this.id281_KeysConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read5_KeysConverter(isNullable, false);
                }
                if ((type.Name == this.id4_KeysConverter2) && (type.Namespace == this.id2_Item))
                {
                    return this.Read6_KeysConverter2(isNullable, false);
                }
                if ((type.Name == this.id1_ImageProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read2_ImageProvider(isNullable, false);
                }
                if ((type.Name == this.id44_ShellImageProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read54_ShellImageProvider(isNullable, false);
                }
                if ((type.Name == this.id3_CustomImageProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read3_CustomImageProvider(isNullable, false);
                }
                if ((type.Name == this.id5_PropertyTagType) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj2 = this.Read7_PropertyTagType(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj2;
                }
                if ((type.Name == this.id6_PropertyTag) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj3 = this.Read8_PropertyTag(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj3;
                }
                if ((type.Name == this.id7_LightSource) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj4 = this.Read9_LightSource(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj4;
                }
                if ((type.Name == this.id10_ReleaseType) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj5 = this.Read15_ReleaseType(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj5;
                }
                if ((type.Name == this.id282_HorizontalAlignment) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj6 = this.Read16_HorizontalAlignment(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj6;
                }
                if ((type.Name == this.id12_ArrayOfListViewColumnInfo) && (type.Namespace == this.id2_Item))
                {
                    ListViewColumnCollection columns = null;
                    if (!base.ReadNull())
                    {
                        if (columns == null)
                        {
                            columns = new ListViewColumnCollection();
                        }
                        ListViewColumnCollection columns2 = columns;
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                            return columns;
                        }
                        base.Reader.ReadStartElement();
                        base.Reader.MoveToContent();
                        int num = 0;
                        int num2 = base.ReaderCount;
                        while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                        {
                            if (base.Reader.NodeType == XmlNodeType.Element)
                            {
                                if ((base.Reader.LocalName == this.id11_ListViewColumnInfo) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    if (columns2 == null)
                                    {
                                        base.Reader.Skip();
                                    }
                                    else
                                    {
                                        columns2.Add(this.Read17_ListViewColumnInfo(true, true));
                                    }
                                }
                                else
                                {
                                    base.UnknownNode(null, ":ListViewColumnInfo");
                                }
                            }
                            else
                            {
                                base.UnknownNode(null, ":ListViewColumnInfo");
                            }
                            base.Reader.MoveToContent();
                            base.CheckReaderCount(ref num, ref num2);
                        }
                        base.ReadEndElement();
                    }
                    return columns;
                }
                if ((type.Name == this.id13_PanelLayoutEntry) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj7 = this.Read18_PanelLayoutEntry(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj7;
                }
                if ((type.Name == this.id14_PanelToolbar) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj8 = this.Read19_PanelToolbar(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj8;
                }
                if ((type.Name == this.id283_Orientation) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj9 = this.Read20_Orientation(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj9;
                }
                if ((type.Name == this.id105_PanelView) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj10 = this.Read21_PanelView(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj10;
                }
                if ((type.Name == this.id16_ActivePanel) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj11 = this.Read23_ActivePanel(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj11;
                }
                if ((type.Name == this.id17_TwoPanelLayoutEntry) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj12 = this.Read24_TwoPanelLayoutEntry(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj12;
                }
                if ((type.Name == this.id21_ActionState) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj13 = this.Read28_ActionState(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj13;
                }
                if ((type.Name == this.id22_BindActionProperty) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj14 = this.Read29_BindActionProperty(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj14;
                }
                if ((type.Name == this.id23_BreadcrumbView) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj15 = this.Read30_BreadcrumbView(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj15;
                }
                if ((type.Name == this.id25_InputDialogOption) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj16 = this.Read33_InputDialogOption(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj16;
                }
                if ((type.Name == this.id28_ArchiveFormatCapabilities) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj17 = this.Read36_ArchiveFormatCapabilities(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj17;
                }
                if ((type.Name == this.id284_ArrayOfString) && (type.Namespace == this.id2_Item))
                {
                    string[] strArray = null;
                    if (base.ReadNull())
                    {
                        return strArray;
                    }
                    string[] a = null;
                    int index = 0;
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                    }
                    else
                    {
                        base.Reader.ReadStartElement();
                        base.Reader.MoveToContent();
                        int num4 = 0;
                        int num5 = base.ReaderCount;
                        while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                        {
                            if (base.Reader.NodeType == XmlNodeType.Element)
                            {
                                if ((base.Reader.LocalName == this.id211_string) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    if (base.ReadNull())
                                    {
                                        a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                        a[index++] = null;
                                    }
                                    else
                                    {
                                        a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                        a[index++] = base.Reader.ReadElementString();
                                    }
                                }
                                else
                                {
                                    base.UnknownNode(null, ":string");
                                }
                            }
                            else
                            {
                                base.UnknownNode(null, ":string");
                            }
                            base.Reader.MoveToContent();
                            base.CheckReaderCount(ref num4, ref num5);
                        }
                        base.ReadEndElement();
                    }
                    return (string[]) base.ShrinkArray(a, index, typeof(string), false);
                }
                if ((type.Name == this.id31_FindFormatSource) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj18 = this.Read39_FindFormatSource(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj18;
                }
                if ((type.Name == this.id34_ProcessorState) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj19 = this.Read44_ProcessorState(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj19;
                }
                if ((type.Name == this.id35_SequenseProcessorType) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj20 = this.Read45_SequenseProcessorType(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj20;
                }
                if ((type.Name == this.id36_PK_OM) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj21 = this.Read46_PK_OM(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj21;
                }
                if ((type.Name == this.id37_PK_OPERATION) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj22 = this.Read47_PK_OPERATION(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj22;
                }
                if ((type.Name == this.id38_PK_CAPS) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj23 = this.Read48_PK_CAPS(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj23;
                }
                if ((type.Name == this.id39_PK_VOL) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj24 = this.Read49_PK_VOL(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj24;
                }
                if ((type.Name == this.id40_PK_PACK) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj25 = this.Read50_PK_PACK(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj25;
                }
                if ((type.Name == this.id43_DefaultIcon) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj26 = this.Read53_DefaultIcon(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj26;
                }
                if ((type.Name == this.id45_ItemCapability) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj27 = this.Read55_ItemCapability(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj27;
                }
                if ((type.Name == this.id49_ContentFlag) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj28 = this.Read59_ContentFlag(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj28;
                }
                if ((type.Name == this.id54_AggregatedFilterCondition) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj29 = this.Read64_AggregatedFilterCondition(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj29;
                }
                if ((type.Name == this.id194_DateComparision) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj30 = this.Read67_DateComparision(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj30;
                }
                if ((type.Name == this.id285_DateUnit) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj31 = this.Read68_DateUnit(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj31;
                }
                if ((type.Name == this.id286_ContentFilterOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj32 = this.Read70_ContentFilterOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj32;
                }
                if ((type.Name == this.id287_ContentComparision) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj33 = this.Read71_ContentComparision(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj33;
                }
                if ((type.Name == this.id288_SimpleComparision) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj34 = this.Read73_SimpleComparision(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj34;
                }
                if ((type.Name == this.id190_TimeComparision) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj35 = this.Read88_TimeComparision(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj35;
                }
                if ((type.Name == this.id63_ItemDateTimePart) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj36 = this.Read90_ItemDateTimePart(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj36;
                }
                if ((type.Name == this.id289_FileAttributes) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj37 = this.Read92_FileAttributes(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj37;
                }
                if ((type.Name == this.id203_SizeUnit) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj38 = this.Read95_SizeUnit(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj38;
                }
                if ((type.Name == this.id204_SizeComparision) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj39 = this.Read96_SizeComparision(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj39;
                }
                if ((type.Name == this.id290_NamePatternCondition) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj40 = this.Read100_NamePatternCondition(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj40;
                }
                if ((type.Name == this.id291_NamePatternComparision) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj41 = this.Read101_NamePatternComparision(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj41;
                }
                if ((type.Name == this.id68_NameListCondition) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj42 = this.Read106_NameListCondition(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj42;
                }
                if ((type.Name == this.id292_ArrayOfChoice1) && (type.Namespace == this.id2_Item))
                {
                    BasicFilter[] filterArray = null;
                    if (base.ReadNull())
                    {
                        return filterArray;
                    }
                    BasicFilter[] filterArray2 = null;
                    int num6 = 0;
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                    }
                    else
                    {
                        base.Reader.ReadStartElement();
                        base.Reader.MoveToContent();
                        int num7 = 0;
                        int num8 = base.ReaderCount;
                        while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                        {
                            if (base.Reader.NodeType == XmlNodeType.Element)
                            {
                                if ((base.Reader.LocalName == this.id180_NameListFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read107_VirtualItemNameListFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id185_TimeFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read91_VirtualItemTimeFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id181_HexContentFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read105_VirtualItemHexContentFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id182_AttributeFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read94_VirtualItemAttributeFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id184_DateFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read99_VirtualItemDateFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id179_ContentFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read87_VirtualItemContentFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id177_NameFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read103_VirtualItemNameFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id178_PropertyFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read84_VirtualPropertyFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id176_AggregatedFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read108_AggregatedVirtualItemFilter(true, true);
                                }
                                else if ((base.Reader.LocalName == this.id183_SizeFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    filterArray2 = (BasicFilter[]) base.EnsureArrayIndex(filterArray2, num6, typeof(BasicFilter));
                                    filterArray2[num6++] = this.Read98_VirtualItemSizeFilter(true, true);
                                }
                                else
                                {
                                    base.UnknownNode(null, ":NameListFilter, :TimeFilter, :HexContentFilter, :AttributeFilter, :DateFilter, :ContentFilter, :NameFilter, :PropertyFilter, :AggregatedFilter, :SizeFilter");
                                }
                            }
                            else
                            {
                                base.UnknownNode(null, ":NameListFilter, :TimeFilter, :HexContentFilter, :AttributeFilter, :DateFilter, :ContentFilter, :NameFilter, :PropertyFilter, :AggregatedFilter, :SizeFilter");
                            }
                            base.Reader.MoveToContent();
                            base.CheckReaderCount(ref num7, ref num8);
                        }
                        base.ReadEndElement();
                    }
                    return (BasicFilter[]) base.ShrinkArray(filterArray2, num6, typeof(BasicFilter), false);
                }
                if ((type.Name == this.id73_HighlighterIconType) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj43 = this.Read113_HighlighterIconType(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj43;
                }
                if ((type.Name == this.id76_CustomizeFolderParts) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj44 = this.Read118_CustomizeFolderParts(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj44;
                }
                if ((type.Name == this.id77_ColorSpace) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj45 = this.Read119_ColorSpace(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj45;
                }
                if ((type.Name == this.id95_ThrobberStyle) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj46 = this.Read138_ThrobberStyle(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj46;
                }
                if ((type.Name == this.id97_AutoRefreshMode) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj47 = this.Read141_AutoRefreshMode(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj47;
                }
                if ((type.Name == this.id101_CanMoveResult) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj48 = this.Read145_CanMoveResult(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj48;
                }
                if ((type.Name == this.id102_IconOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj49 = this.Read146_IconOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj49;
                }
                if ((type.Name == this.id103_DelayedExtractMode) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj50 = this.Read147_DelayedExtractMode(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj50;
                }
                if ((type.Name == this.id104_PathView) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj51 = this.Read148_PathView(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj51;
                }
                if ((type.Name == this.id106_ContextMenuOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj52 = this.Read149_ContextMenuOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj52;
                }
                if ((type.Name == this.id108_ArrayOfPropertyValue) && (type.Namespace == this.id2_Item))
                {
                    PropertyValueList list = null;
                    if (!base.ReadNull())
                    {
                        if (list == null)
                        {
                            list = new PropertyValueList();
                        }
                        PropertyValueList list2 = list;
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                            return list;
                        }
                        base.Reader.ReadStartElement();
                        base.Reader.MoveToContent();
                        int num9 = 0;
                        int num10 = base.ReaderCount;
                        while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                        {
                            if (base.Reader.NodeType == XmlNodeType.Element)
                            {
                                if ((base.Reader.LocalName == this.id109_PropertyValue) && (base.Reader.NamespaceURI == this.id2_Item))
                                {
                                    if (list2 == null)
                                    {
                                        base.Reader.Skip();
                                    }
                                    else
                                    {
                                        list2.Add(this.Read151_PropertyValue(true, true));
                                    }
                                }
                                else
                                {
                                    base.UnknownNode(null, ":PropertyValue");
                                }
                            }
                            else
                            {
                                base.UnknownNode(null, ":PropertyValue");
                            }
                            base.Reader.MoveToContent();
                            base.CheckReaderCount(ref num9, ref num10);
                        }
                        base.ReadEndElement();
                    }
                    return list;
                }
                if ((type.Name == this.id111_ProgressRenderMode) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj53 = this.Read153_ProgressRenderMode(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj53;
                }
                if ((type.Name == this.id112_ProgressState) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj54 = this.Read154_ProgressState(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj54;
                }
                if ((type.Name == this.id114_MarqueeStyle) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj55 = this.Read156_MarqueeStyle(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj55;
                }
                if ((type.Name == this.id116_AskMode) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj56 = this.Read158_AskMode(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj56;
                }
                if ((type.Name == this.id117_OperationResult) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj57 = this.Read159_OperationResult(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj57;
                }
                if ((type.Name == this.id118_ItemPropId) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj58 = this.Read160_ItemPropId(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj58;
                }
                if ((type.Name == this.id119_FileTimeType) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj59 = this.Read161_FileTimeType(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj59;
                }
                if ((type.Name == this.id120_ArchivePropId) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj60 = this.Read162_ArchivePropId(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj60;
                }
                if ((type.Name == this.id121_KnownSevenZipFormat) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj61 = this.Read163_KnownSevenZipFormat(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj61;
                }
                if ((type.Name == this.id122_SevenZipFormatCapabilities) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj62 = this.Read164_SevenZipFormatCapabilities(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj62;
                }
                if ((type.Name == this.id123_CompressionLevel) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj63 = this.Read165_CompressionLevel(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj63;
                }
                if ((type.Name == this.id124_CompressionMethod) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj64 = this.Read166_CompressionMethod(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj64;
                }
                if ((type.Name == this.id125_EncryptionMethod) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj65 = this.Read167_EncryptionMethod(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj65;
                }
                if ((type.Name == this.id126_SolidSizeUnit) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj66 = this.Read168_SolidSizeUnit(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj66;
                }
                if ((type.Name == this.id127_ComplexFilterView) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj67 = this.Read169_ComplexFilterView(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj67;
                }
                if ((type.Name == this.id128_ViewFilters) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj68 = this.Read170_ViewFilters(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj68;
                }
                if ((type.Name == this.id147_QuickFindOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj69 = this.Read171_QuickFindOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj69;
                }
                if ((type.Name == this.id130_ControllerType) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj70 = this.Read173_ControllerType(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj70;
                }
                if ((type.Name == this.id132_FormPlacement) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj71 = this.Read176_FormPlacement(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj71;
                }
                if ((type.Name == this.id133_ArgumentKey) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj72 = this.Read177_ArgumentKey(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj72;
                }
                if ((type.Name == this.id134_CanMoveListViewItem) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj73 = this.Read178_CanMoveListViewItem(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj73;
                }
                if ((type.Name == this.id136_SinglePanel) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj74 = this.Read180_SinglePanel(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj74;
                }
                if ((type.Name == this.id139_ArchiveUpdateMode) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj75 = this.Read183_ArchiveUpdateMode(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj75;
                }
                if ((type.Name == this.id140_PackStage) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj76 = this.Read184_PackStage(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj76;
                }
                if ((type.Name == this.id144_CopyDestinationItem) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj77 = this.Read190_CopyDestinationItem(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj77;
                }
                if ((type.Name == this.id145_MessageDialogResult) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj78 = this.Read191_MessageDialogResult(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj78;
                }
                if ((type.Name == this.id146_DoubleClickAction) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj79 = this.Read192_DoubleClickAction(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj79;
                }
                if ((type.Name == this.id148_ListViewSort) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj80 = this.Read193_ListViewSort(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj80;
                }
                if ((type.Name == this.id150_SearchFolderOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj81 = this.Read195_SearchFolderOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj81;
                }
                if ((type.Name == this.id151_FindDuplicateOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj82 = this.Read196_FindDuplicateOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj82;
                }
                if ((type.Name == this.id152_Compare) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj83 = this.Read197_Compare(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj83;
                }
                if ((type.Name == this.id153_ChangeItemAction) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj84 = this.Read198_ChangeItemAction(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj84;
                }
                if ((type.Name == this.id154_AvailableItemActions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj85 = this.Read199_AvailableItemActions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj85;
                }
                if ((type.Name == this.id155_CompareFoldersOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj86 = this.Read200_CompareFoldersOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj86;
                }
                if ((type.Name == this.id156_OverwriteDialogResult) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj87 = this.Read201_OverwriteDialogResult(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj87;
                }
                if ((type.Name == this.id157_CopyWorkerOptions) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj88 = this.Read202_CopyWorkerOptions(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj88;
                }
                if ((type.Name == this.id158_CopyMode) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj89 = this.Read203_CopyMode(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj89;
                }
                if ((type.Name == this.id161_IconStyle) && (type.Namespace == this.id2_Item))
                {
                    base.Reader.ReadStartElement();
                    object obj90 = this.Read205_IconStyle(base.CollapseWhitespace(base.Reader.ReadString()));
                    base.ReadEndElement();
                    return obj90;
                }
                return base.ReadTypedPrimitive(type);
            }
            if (flag)
            {
                return null;
            }
            object o = new object();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ToolStripRenderer Read10_ToolStripRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id277_ToolStripRenderer) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id278_ToolStripWrapperRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read31_ToolStripWrapperRenderer(isNullable, false);
                }
                if ((type.Name == this.id24_BreadcrumbToolStripRenderer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read32_BreadcrumbToolStripRenderer(isNullable, false);
                }
                if ((type.Name != this.id8_ToolStripButtonRenderer) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read11_ToolStripButtonRenderer(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("ToolStripRenderer", "");
            }
            return null;
        }

        private NamePatternCondition Read100_NamePatternCondition(string s)
        {
            switch (s)
            {
                case "Equal":
                    return NamePatternCondition.Equal;

                case "NotEqual":
                    return NamePatternCondition.NotEqual;
            }
            throw base.CreateUnknownConstantException(s, typeof(NamePatternCondition));
        }

        private NamePatternComparision Read101_NamePatternComparision(string s)
        {
            switch (s)
            {
                case "Ignore":
                    return NamePatternComparision.Ignore;

                case "StartsWith":
                    return NamePatternComparision.StartsWith;

                case "EndsWith":
                    return NamePatternComparision.EndsWith;

                case "Equals":
                    return NamePatternComparision.Equals;

                case "Wildcards":
                    return NamePatternComparision.Wildcards;

                case "RegEx":
                    return NamePatternComparision.RegEx;
            }
            throw base.CreateUnknownConstantException(s, typeof(NamePatternComparision));
        }

        private NameFilter Read102_NameFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id177_NameFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id60_VirtualItemFullNameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read112_VirtualItemFullNameFilter(isNullable, false);
                }
                if ((type.Name != this.id59_VirtualItemNameFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read103_VirtualItemNameFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            NameFilter o = new NameFilter();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id228_NameCondition)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.NameCondition = this.Read100_NamePatternCondition(base.Reader.ReadElementString());
                        }
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id229_NameComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NameComparision = this.Read101_NamePatternComparision(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id230_NamePattern)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NamePattern = base.Reader.ReadElementString();
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":NameCondition, :NameComparision, :NamePattern");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":NameCondition, :NameComparision, :NamePattern");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemNameFilter Read103_VirtualItemNameFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id59_VirtualItemNameFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemNameFilter o = new VirtualItemNameFilter();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id228_NameCondition)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.NameCondition = this.Read100_NamePatternCondition(base.Reader.ReadElementString());
                        }
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id229_NameComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NameComparision = this.Read101_NamePatternComparision(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id230_NamePattern)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NamePattern = base.Reader.ReadElementString();
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":NameCondition, :NameComparision, :NamePattern");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":NameCondition, :NameComparision, :NamePattern");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private HexContentFilter Read104_HexContentFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id181_HexContentFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id67_VirtualItemHexContentFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read105_VirtualItemHexContentFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            HexContentFilter o = new HexContentFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id207_Comparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Comparision = this.Read71_ContentComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id208_Sequence)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SequenceAsString = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Comparision, :Sequence");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Comparision, :Sequence");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemHexContentFilter Read105_VirtualItemHexContentFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id67_VirtualItemHexContentFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemHexContentFilter o = new VirtualItemHexContentFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id207_Comparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Comparision = this.Read71_ContentComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id208_Sequence)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SequenceAsString = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Comparision, :Sequence");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Comparision, :Sequence");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private NameListCondition Read106_NameListCondition(string s)
        {
            switch (s)
            {
                case "InList":
                    return NameListCondition.InList;

                case "NotInList":
                    return NameListCondition.NotInList;
            }
            throw base.CreateUnknownConstantException(s, typeof(NameListCondition));
        }

        private VirtualItemNameListFilter Read107_VirtualItemNameListFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id69_VirtualItemNameListFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemNameListFilter o = new VirtualItemNameListFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id209_Condition)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Condition = this.Read106_NameListCondition(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((base.Reader.LocalName == this.id210_Names) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (!base.ReadNull())
                        {
                            string[] a = null;
                            int index = 0;
                            if (base.Reader.IsEmptyElement)
                            {
                                base.Reader.Skip();
                            }
                            else
                            {
                                base.Reader.ReadStartElement();
                                base.Reader.MoveToContent();
                                int num4 = 0;
                                int num5 = base.ReaderCount;
                                while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                                {
                                    if (base.Reader.NodeType == XmlNodeType.Element)
                                    {
                                        if ((base.Reader.LocalName == this.id211_string) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            if (base.ReadNull())
                                            {
                                                a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                a[index++] = null;
                                            }
                                            else
                                            {
                                                a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                a[index++] = base.Reader.ReadElementString();
                                            }
                                        }
                                        else
                                        {
                                            base.UnknownNode(null, ":string");
                                        }
                                    }
                                    else
                                    {
                                        base.UnknownNode(null, ":string");
                                    }
                                    base.Reader.MoveToContent();
                                    base.CheckReaderCount(ref num4, ref num5);
                                }
                                base.ReadEndElement();
                            }
                            o.Names = (string[]) base.ShrinkArray(a, index, typeof(string), false);
                        }
                    }
                    else
                    {
                        base.UnknownNode(o, ":Condition, :Names");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Condition, :Names");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private AggregatedVirtualItemFilter Read108_AggregatedVirtualItemFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id55_AggregatedVirtualItemFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            AggregatedVirtualItemFilter o = new AggregatedVirtualItemFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id209_Condition)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Condition = this.Read64_AggregatedFilterCondition(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((base.Reader.LocalName == this.id231_Filters) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (!base.ReadNull())
                        {
                            BasicFilter[] a = null;
                            int index = 0;
                            if (base.Reader.IsEmptyElement)
                            {
                                base.Reader.Skip();
                            }
                            else
                            {
                                base.Reader.ReadStartElement();
                                base.Reader.MoveToContent();
                                int num4 = 0;
                                int num5 = base.ReaderCount;
                                while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                                {
                                    if (base.Reader.NodeType == XmlNodeType.Element)
                                    {
                                        if ((base.Reader.LocalName == this.id180_NameListFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read107_VirtualItemNameListFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id185_TimeFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read91_VirtualItemTimeFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id181_HexContentFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read105_VirtualItemHexContentFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id182_AttributeFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read94_VirtualItemAttributeFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id184_DateFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read99_VirtualItemDateFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id179_ContentFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read87_VirtualItemContentFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id177_NameFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read103_VirtualItemNameFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id178_PropertyFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read84_VirtualPropertyFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id176_AggregatedFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read108_AggregatedVirtualItemFilter(true, true);
                                        }
                                        else if ((base.Reader.LocalName == this.id183_SizeFilter) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (BasicFilter[]) base.EnsureArrayIndex(a, index, typeof(BasicFilter));
                                            a[index++] = this.Read98_VirtualItemSizeFilter(true, true);
                                        }
                                        else
                                        {
                                            base.UnknownNode(null, ":NameListFilter, :TimeFilter, :HexContentFilter, :AttributeFilter, :DateFilter, :ContentFilter, :NameFilter, :PropertyFilter, :AggregatedFilter, :SizeFilter");
                                        }
                                    }
                                    else
                                    {
                                        base.UnknownNode(null, ":NameListFilter, :TimeFilter, :HexContentFilter, :AttributeFilter, :DateFilter, :ContentFilter, :NameFilter, :PropertyFilter, :AggregatedFilter, :SizeFilter");
                                    }
                                    base.Reader.MoveToContent();
                                    base.CheckReaderCount(ref num4, ref num5);
                                }
                                base.ReadEndElement();
                            }
                            o.SerializableFilters = (BasicFilter[]) base.ShrinkArray(a, index, typeof(BasicFilter), false);
                        }
                    }
                    else
                    {
                        base.UnknownNode(o, ":Condition, :Filters");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Condition, :Filters");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private FilterContainer Read109_FilterContainer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id56_FilterContainer) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id129_PanelContentContainer) && (type.Namespace == this.id2_Item))
                {
                    return this.Read172_PanelContentContainer(isNullable, false);
                }
                if ((type.Name == this.id57_NamedFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read110_NamedFilter(isNullable, false);
                }
                if ((type.Name == this.id71_VirtualHighligher) && (type.Namespace == this.id2_Item))
                {
                    return this.Read114_VirtualHighligher(isNullable, false);
                }
                if ((type.Name != this.id72_ListViewHighlighter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read115_ListViewHighlighter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            FilterContainer o = new FilterContainer();
            bool[] flagArray = new bool[1];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id176_AggregatedFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read108_AggregatedVirtualItemFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id177_NameFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read103_VirtualItemNameFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id178_PropertyFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read84_VirtualPropertyFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id179_ContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read87_VirtualItemContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id180_NameListFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read107_VirtualItemNameListFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id181_HexContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read105_VirtualItemHexContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id182_AttributeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read94_VirtualItemAttributeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id183_SizeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read98_VirtualItemSizeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id184_DateFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read99_VirtualItemDateFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id185_TimeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read91_VirtualItemTimeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ToolStripButtonRenderer Read11_ToolStripButtonRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id8_ToolStripButtonRenderer) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ToolStripButtonRenderer o = new ToolStripButtonRenderer();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private NamedFilter Read110_NamedFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id57_NamedFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id71_VirtualHighligher) && (type.Namespace == this.id2_Item))
                {
                    return this.Read114_VirtualHighligher(isNullable, false);
                }
                if ((type.Name != this.id72_ListViewHighlighter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read115_ListViewHighlighter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            NamedFilter o = new NamedFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id176_AggregatedFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read108_AggregatedVirtualItemFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id177_NameFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read103_VirtualItemNameFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id178_PropertyFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read84_VirtualPropertyFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id179_ContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read87_VirtualItemContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id180_NameListFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read107_VirtualItemNameListFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id181_HexContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read105_VirtualItemHexContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id182_AttributeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read94_VirtualItemAttributeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id183_SizeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read98_VirtualItemSizeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id184_DateFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read99_VirtualItemDateFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id185_TimeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read91_VirtualItemTimeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id326_Name)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Name = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Name");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Name");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private FilterHelper Read111_FilterHelper(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id58_FilterHelper) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            FilterHelper o = new FilterHelper();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemFullNameFilter Read112_VirtualItemFullNameFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id60_VirtualItemFullNameFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemFullNameFilter o = new VirtualItemFullNameFilter();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id228_NameCondition)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.NameCondition = this.Read100_NamePatternCondition(base.Reader.ReadElementString());
                        }
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id229_NameComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NameComparision = this.Read101_NamePatternComparision(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id230_NamePattern)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NamePattern = base.Reader.ReadElementString();
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":NameCondition, :NameComparision, :NamePattern");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":NameCondition, :NameComparision, :NamePattern");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private HighlighterIconType Read113_HighlighterIconType(string s)
        {
            switch (s)
            {
                case "HighlighterIcon":
                    return HighlighterIconType.HighlighterIcon;

                case "TypeIcon":
                    return HighlighterIconType.TypeIcon;

                case "ExtractedIcon":
                    return HighlighterIconType.ExtractedIcon;
            }
            throw base.CreateUnknownConstantException(s, typeof(HighlighterIconType));
        }

        private VirtualHighligher Read114_VirtualHighligher(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id71_VirtualHighligher) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id72_ListViewHighlighter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read115_ListViewHighlighter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            VirtualHighligher o = new VirtualHighligher();
            bool[] flagArray = new bool[7];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id176_AggregatedFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read108_AggregatedVirtualItemFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id177_NameFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read103_VirtualItemNameFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id178_PropertyFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read84_VirtualPropertyFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id179_ContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read87_VirtualItemContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id180_NameListFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read107_VirtualItemNameListFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id181_HexContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read105_VirtualItemHexContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id182_AttributeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read94_VirtualItemAttributeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id183_SizeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read98_VirtualItemSizeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id184_DateFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read99_VirtualItemDateFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id185_TimeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read91_VirtualItemTimeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id326_Name)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Name = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id327_IconType)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.IconType = this.Read113_HighlighterIconType(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id328_AlphaBlend)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.AlphaBlend = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id329_BlendLevel)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.BlendLevel = XmlConvert.ToSingle(base.Reader.ReadElementString());
                        }
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id330_Icon)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.IconLocation = base.Reader.ReadElementString();
                        flagArray[5] = true;
                    }
                    else if ((!flagArray[6] && (base.Reader.LocalName == this.id331_BlendColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableBlendColor = base.Reader.ReadElementString();
                        flagArray[6] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Name, :IconType, :AlphaBlend, :BlendLevel, :Icon, :BlendColor");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Name, :IconType, :AlphaBlend, :BlendLevel, :Icon, :BlendColor");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ListViewHighlighter Read115_ListViewHighlighter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id72_ListViewHighlighter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ListViewHighlighter o = new ListViewHighlighter();
            bool[] flagArray = new bool[8];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id176_AggregatedFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read108_AggregatedVirtualItemFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id177_NameFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read103_VirtualItemNameFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id178_PropertyFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read84_VirtualPropertyFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id179_ContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read87_VirtualItemContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id180_NameListFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read107_VirtualItemNameListFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id181_HexContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read105_VirtualItemHexContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id182_AttributeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read94_VirtualItemAttributeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id183_SizeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read98_VirtualItemSizeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id184_DateFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read99_VirtualItemDateFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id185_TimeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read91_VirtualItemTimeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id326_Name)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Name = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id327_IconType)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.IconType = this.Read113_HighlighterIconType(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id328_AlphaBlend)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.AlphaBlend = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id329_BlendLevel)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.BlendLevel = XmlConvert.ToSingle(base.Reader.ReadElementString());
                        }
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id330_Icon)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.IconLocation = base.Reader.ReadElementString();
                        flagArray[5] = true;
                    }
                    else if ((!flagArray[6] && (base.Reader.LocalName == this.id331_BlendColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableBlendColor = base.Reader.ReadElementString();
                        flagArray[6] = true;
                    }
                    else if ((!flagArray[7] && (base.Reader.LocalName == this.id332_ForeColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableForeColor = base.Reader.ReadElementString();
                        flagArray[7] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Name, :IconType, :AlphaBlend, :BlendLevel, :Icon, :BlendColor, :ForeColor");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Name, :IconType, :AlphaBlend, :BlendLevel, :Icon, :BlendColor, :ForeColor");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private HashPropertyProvider Read116_HashPropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id74_HashPropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            HashPropertyProvider o = new HashPropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VistaThumbnailProvider Read117_VistaThumbnailProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id75_VistaThumbnailProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VistaThumbnailProvider o = new VistaThumbnailProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private CustomizeFolderParts Read118_CustomizeFolderParts(string s)
        {
            return (CustomizeFolderParts) ((int) XmlSerializationReader.ToEnum(s, this.CustomizeFolderPartsValues, "global::Nomad.FileSystem.Virtual.CustomizeFolderParts"));
        }

        private ColorSpace Read119_ColorSpace(string s)
        {
            switch (s)
            {
                case "Unknown":
                    return ColorSpace.Unknown;

                case "RGB":
                    return ColorSpace.RGB;

                case "CMYK":
                    return ColorSpace.CMYK;

                case "Grayscale":
                    return ColorSpace.Grayscale;

                case "YCBCR":
                    return ColorSpace.YCBCR;

                case "YCCK":
                    return ColorSpace.YCCK;

                case "Indexed":
                    return ColorSpace.Indexed;
            }
            throw base.CreateUnknownConstantException(s, typeof(ColorSpace));
        }

        private ProviderBase Read12_ProviderBase(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id275_ProviderBase) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id276_SettingsProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read13_SettingsProvider(isNullable, false);
                }
                if ((type.Name != this.id9_ConfigurableSettingsProvider) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read14_ConfigurableSettingsProvider(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("ProviderBase", "");
            }
            return null;
        }

        private DescriptionPropertyProvider Read120_DescriptionPropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id78_DescriptionPropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            DescriptionPropertyProvider o = new DescriptionPropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private DummyClientSite Read121_DummyClientSite(bool isNullable, bool checkType)
        {
            DummyClientSite site;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id79_DummyClientSite) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            try
            {
                site = (DummyClientSite) Activator.CreateInstance(typeof(DummyClientSite), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
                goto Label_00A6;
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.FileSystem.Property.Providers.DummyClientSite");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.FileSystem.Property.Providers.DummyClientSite");
            }
        Label_008C:
            if (!base.IsXmlnsAttribute(base.Reader.Name))
            {
                base.UnknownNode(site);
            }
        Label_00A6:
            if (base.Reader.MoveToNextAttribute())
            {
                goto Label_008C;
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return site;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(site, "");
                }
                else
                {
                    base.UnknownNode(site, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return site;
        }

        private HtmlPropertyProvider Read122_HtmlPropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id80_HtmlPropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            HtmlPropertyProvider o = new HtmlPropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PropertyTypeConverter Read123_PropertyTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id280_PropertyTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id88_RatingTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read131_RatingTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id87_ISOSpeedTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read130_ISOSpeedTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id86_DPITypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read129_DPITypeConverter(isNullable, false);
                }
                if ((type.Name == this.id85_ImageSizeTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read128_ImageSizeTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id84_DurationTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read127_DurationTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id83_AudioSampleRateTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read126_AudioSampleRateTypeConverter(isNullable, false);
                }
                if ((type.Name != this.id81_BitrateTypeConverter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read124_BitrateTypeConverter(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("PropertyTypeConverter", "");
            }
            return null;
        }

        private BitrateTypeConverter Read124_BitrateTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id81_BitrateTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            BitrateTypeConverter o = new BitrateTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private AudioChannelsTypeConverter Read125_AudioChannelsTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id82_AudioChannelsTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            AudioChannelsTypeConverter o = new AudioChannelsTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private AudioSampleRateTypeConverter Read126_AudioSampleRateTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id83_AudioSampleRateTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            AudioSampleRateTypeConverter o = new AudioSampleRateTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private DurationTypeConverter Read127_DurationTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id84_DurationTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            DurationTypeConverter o = new DurationTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ImageSizeTypeConverter Read128_ImageSizeTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id85_ImageSizeTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ImageSizeTypeConverter o = new ImageSizeTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private DPITypeConverter Read129_DPITypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id86_DPITypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            DPITypeConverter o = new DPITypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SettingsProvider Read13_SettingsProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id276_SettingsProvider) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id9_ConfigurableSettingsProvider) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read14_ConfigurableSettingsProvider(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("SettingsProvider", "");
            }
            return null;
        }

        private ISOSpeedTypeConverter Read130_ISOSpeedTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id87_ISOSpeedTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ISOSpeedTypeConverter o = new ISOSpeedTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private RatingTypeConverter Read131_RatingTypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id88_RatingTypeConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            RatingTypeConverter o = new RatingTypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private EncodingConveter Read132_EncodingConveter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id89_EncodingConveter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            EncodingConveter o = new EncodingConveter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ImagePropertyProvider Read133_ImagePropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id90_ImagePropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ImagePropertyProvider o = new ImagePropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PsdPropertyProvider Read134_PsdPropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id91_PsdPropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            PsdPropertyProvider o = new PsdPropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private TagLibPropertyProvider Read135_TagLibPropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id92_TagLibPropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            TagLibPropertyProvider o = new TagLibPropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private TextPropertyProvider Read136_TextPropertyProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id93_TextPropertyProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            TextPropertyProvider o = new TextPropertyProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualToolTip Read137_VirtualToolTip(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id94_VirtualToolTip) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualToolTip o = new VirtualToolTip();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ThrobberStyle Read138_ThrobberStyle(string s)
        {
            switch (s)
            {
                case "Custom":
                    return ThrobberStyle.Custom;

                case "MacOSX":
                    return ThrobberStyle.MacOSX;

                case "Firefox":
                    return ThrobberStyle.Firefox;

                case "IE7":
                    return ThrobberStyle.IE7;
            }
            throw base.CreateUnknownConstantException(s, typeof(ThrobberStyle));
        }

        private Color Read139_Color(bool checkType)
        {
            Color color;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id258_Color) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                color = (Color) Activator.CreateInstance(typeof(Color), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
                goto Label_009A;
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::System.Drawing.Color");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::System.Drawing.Color");
            }
        Label_007B:
            if (!base.IsXmlnsAttribute(base.Reader.Name))
            {
                base.UnknownNode(color);
            }
        Label_009A:
            if (base.Reader.MoveToNextAttribute())
            {
                goto Label_007B;
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return color;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(color, "");
                }
                else
                {
                    base.UnknownNode(color, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return color;
        }

        private ConfigurableSettingsProvider Read14_ConfigurableSettingsProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id9_ConfigurableSettingsProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ConfigurableSettingsProvider o = new ConfigurableSettingsProvider();
            bool[] flagArray = new bool[1];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id293_ApplicationName)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ApplicationName = base.Reader.ReadElementString();
                        flagArray[0] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ApplicationName");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ApplicationName");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ThrobberRenderer Read140_ThrobberRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id96_ThrobberRenderer) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ThrobberRenderer o = new ThrobberRenderer();
            bool[] flagArray = new bool[6];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id258_Color)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Color = this.Read139_Color(true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id333_InnerCircleRadius)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.InnerCircleRadius = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id334_OuterCircleRadius)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.OuterCircleRadius = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id335_NumberOfSpoke)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.NumberOfSpoke = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id336_SpokeThickness)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SpokeThickness = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id337_Style)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Style = this.Read138_ThrobberStyle(base.Reader.ReadElementString());
                        flagArray[5] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Color, :InnerCircleRadius, :OuterCircleRadius, :NumberOfSpoke, :SpokeThickness, :Style");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Color, :InnerCircleRadius, :OuterCircleRadius, :NumberOfSpoke, :SpokeThickness, :Style");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private AutoRefreshMode Read141_AutoRefreshMode(string s)
        {
            switch (s)
            {
                case "None":
                    return AutoRefreshMode.None;

                case "Simplified":
                    return AutoRefreshMode.Simplified;

                case "Full":
                    return AutoRefreshMode.Full;
            }
            throw base.CreateUnknownConstantException(s, typeof(AutoRefreshMode));
        }

        private FtpFileSystemCreator Read142_FtpFileSystemCreator(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id98_FtpFileSystemCreator) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            FtpFileSystemCreator o = new FtpFileSystemCreator();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private NullFileSystemCreator Read143_NullFileSystemCreator(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id99_NullFileSystemCreator) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            NullFileSystemCreator o = new NullFileSystemCreator();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private CustomVirtualFolder Read144_CustomVirtualFolder(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id100_CustomVirtualFolder) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("CustomVirtualFolder", "");
            }
            return null;
        }

        private CanMoveResult Read145_CanMoveResult(string s)
        {
            switch (s)
            {
                case "None":
                    return CanMoveResult.None;

                case "Several":
                    return CanMoveResult.Several;

                case "All":
                    return CanMoveResult.All;
            }
            throw base.CreateUnknownConstantException(s, typeof(CanMoveResult));
        }

        private IconOptions Read146_IconOptions(string s)
        {
            return (IconOptions) ((int) XmlSerializationReader.ToEnum(s, this.IconOptionsValues, "global::Nomad.FileSystem.Virtual.IconOptions"));
        }

        private DelayedExtractMode Read147_DelayedExtractMode(string s)
        {
            switch (s)
            {
                case "Never":
                    return DelayedExtractMode.Never;

                case "Always":
                    return DelayedExtractMode.Always;

                case "OnSlowDrivesOnly":
                    return DelayedExtractMode.OnSlowDrivesOnly;
            }
            throw base.CreateUnknownConstantException(s, typeof(DelayedExtractMode));
        }

        private PathView Read148_PathView(string s)
        {
            return (PathView) ((int) XmlSerializationReader.ToEnum(s, this.PathViewValues, "global::Nomad.FileSystem.Virtual.PathView"));
        }

        private ContextMenuOptions Read149_ContextMenuOptions(string s)
        {
            return (ContextMenuOptions) ((int) XmlSerializationReader.ToEnum(s, this.ContextMenuOptionsValues, "global::Nomad.FileSystem.Virtual.ContextMenuOptions"));
        }

        private ReleaseType Read15_ReleaseType(string s)
        {
            switch (s)
            {
                case "Final":
                    return ReleaseType.Final;

                case "Alpha":
                    return ReleaseType.Alpha;

                case "Beta":
                    return ReleaseType.Beta;

                case "RC":
                    return ReleaseType.RC;
            }
            throw base.CreateUnknownConstantException(s, typeof(ReleaseType));
        }

        private VirtualIcon Read150_VirtualIcon(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id107_VirtualIcon) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualIcon o = new VirtualIcon();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PropertyValue Read151_PropertyValue(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id109_PropertyValue) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            PropertyValue o = new PropertyValue();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id263_DataObject)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.DataObject = this.Read1_Object(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id264_PropertyName)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.PropertyName = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":DataObject, :PropertyName");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":DataObject, :PropertyName");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SimpleEncrypt Read152_SimpleEncrypt(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id110_SimpleEncrypt) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            SimpleEncrypt o = new SimpleEncrypt();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ProgressRenderMode Read153_ProgressRenderMode(string s)
        {
            switch (s)
            {
                case "System":
                    return ProgressRenderMode.System;

                case "Vista":
                    return ProgressRenderMode.Vista;

                case "Custom":
                    return ProgressRenderMode.Custom;
            }
            throw base.CreateUnknownConstantException(s, typeof(ProgressRenderMode));
        }

        private ProgressState Read154_ProgressState(string s)
        {
            switch (s)
            {
                case "Normal":
                    return ProgressState.Normal;

                case "Pause":
                    return ProgressState.Pause;

                case "Error":
                    return ProgressState.Error;
            }
            throw base.CreateUnknownConstantException(s, typeof(ProgressState));
        }

        private VistaProgressBarRenderer Read155_VistaProgressBarRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id113_VistaProgressBarRenderer) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VistaProgressBarRenderer o = new VistaProgressBarRenderer();
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id259_BackgroundColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.BackgroundColor = this.Read139_Color(true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id260_HighlightColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.HighlightColor = this.Read139_Color(true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id261_StartColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.StartColor = this.Read139_Color(true);
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id262_EndColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.EndColor = this.Read139_Color(true);
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":BackgroundColor, :HighlightColor, :StartColor, :EndColor");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":BackgroundColor, :HighlightColor, :StartColor, :EndColor");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private MarqueeStyle Read156_MarqueeStyle(string s)
        {
            switch (s)
            {
                case "Continuous":
                    return MarqueeStyle.Continuous;

                case "LeftRight":
                    return MarqueeStyle.LeftRight;
            }
            throw base.CreateUnknownConstantException(s, typeof(MarqueeStyle));
        }

        private XPProgressBarRenderer Read157_XPProgressBarRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id115_XPProgressBarRenderer) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            XPProgressBarRenderer o = new XPProgressBarRenderer();
            bool[] flagArray = new bool[5];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id254_BarColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.BarColor = this.Read139_Color(true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id255_BarBackgroundColor)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.BarBackgroundColor = this.Read139_Color(true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id256_ChunkWidth)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ChunkWidth = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id257_MarqueeChunks)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.MarqueeChunks = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id114_MarqueeStyle)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.MarqueeStyle = this.Read156_MarqueeStyle(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":BarColor, :BarBackgroundColor, :ChunkWidth, :MarqueeChunks, :MarqueeStyle");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":BarColor, :BarBackgroundColor, :ChunkWidth, :MarqueeChunks, :MarqueeStyle");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private AskMode Read158_AskMode(string s)
        {
            switch (s)
            {
                case "kExtract":
                    return AskMode.kExtract;

                case "kTest":
                    return AskMode.kTest;

                case "kSkip":
                    return AskMode.kSkip;
            }
            throw base.CreateUnknownConstantException(s, typeof(AskMode));
        }

        private OperationResult Read159_OperationResult(string s)
        {
            switch (s)
            {
                case "kOK":
                    return OperationResult.kOK;

                case "kUnSupportedMethod":
                    return OperationResult.kUnSupportedMethod;

                case "kDataError":
                    return OperationResult.kDataError;

                case "kCRCError":
                    return OperationResult.kCRCError;
            }
            throw base.CreateUnknownConstantException(s, typeof(OperationResult));
        }

        private HorizontalAlignment Read16_HorizontalAlignment(string s)
        {
            switch (s)
            {
                case "Left":
                    return HorizontalAlignment.Left;

                case "Right":
                    return HorizontalAlignment.Right;

                case "Center":
                    return HorizontalAlignment.Center;
            }
            throw base.CreateUnknownConstantException(s, typeof(HorizontalAlignment));
        }

        private ItemPropId Read160_ItemPropId(string s)
        {
            switch (s)
            {
                case "kpidNoProperty":
                    return ItemPropId.kpidNoProperty;

                case "kpidHandlerItemIndex":
                    return ItemPropId.kpidHandlerItemIndex;

                case "kpidPath":
                    return ItemPropId.kpidPath;

                case "kpidName":
                    return ItemPropId.kpidName;

                case "kpidExtension":
                    return ItemPropId.kpidExtension;

                case "kpidIsFolder":
                    return ItemPropId.kpidIsFolder;

                case "kpidSize":
                    return ItemPropId.kpidSize;

                case "kpidPackedSize":
                    return ItemPropId.kpidPackedSize;

                case "kpidAttributes":
                    return ItemPropId.kpidAttributes;

                case "kpidCreationTime":
                    return ItemPropId.kpidCreationTime;

                case "kpidLastAccessTime":
                    return ItemPropId.kpidLastAccessTime;

                case "kpidLastWriteTime":
                    return ItemPropId.kpidLastWriteTime;

                case "kpidSolid":
                    return ItemPropId.kpidSolid;

                case "kpidCommented":
                    return ItemPropId.kpidCommented;

                case "kpidEncrypted":
                    return ItemPropId.kpidEncrypted;

                case "kpidSplitBefore":
                    return ItemPropId.kpidSplitBefore;

                case "kpidSplitAfter":
                    return ItemPropId.kpidSplitAfter;

                case "kpidDictionarySize":
                    return ItemPropId.kpidDictionarySize;

                case "kpidCRC":
                    return ItemPropId.kpidCRC;

                case "kpidType":
                    return ItemPropId.kpidType;

                case "kpidIsAnti":
                    return ItemPropId.kpidIsAnti;

                case "kpidMethod":
                    return ItemPropId.kpidMethod;

                case "kpidHostOS":
                    return ItemPropId.kpidHostOS;

                case "kpidFileSystem":
                    return ItemPropId.kpidFileSystem;

                case "kpidUser":
                    return ItemPropId.kpidUser;

                case "kpidGroup":
                    return ItemPropId.kpidGroup;

                case "kpidBlock":
                    return ItemPropId.kpidBlock;

                case "kpidComment":
                    return ItemPropId.kpidComment;

                case "kpidPosition":
                    return ItemPropId.kpidPosition;

                case "kpidPrefix":
                    return ItemPropId.kpidPrefix;

                case "kpidNumSubFolders":
                    return ItemPropId.kpidNumSubFolders;

                case "kpidNumSubFiles":
                    return ItemPropId.kpidNumSubFiles;

                case "kpidUnpackVer":
                    return ItemPropId.kpidUnpackVer;

                case "kpidVolume":
                    return ItemPropId.kpidVolume;

                case "kpidIsVolume":
                    return ItemPropId.kpidIsVolume;

                case "kpidOffset":
                    return ItemPropId.kpidOffset;

                case "kpidLinks":
                    return ItemPropId.kpidLinks;

                case "kpidNumBlocks":
                    return ItemPropId.kpidNumBlocks;

                case "kpidNumVolumes":
                    return ItemPropId.kpidNumVolumes;

                case "kpidTimeType":
                    return ItemPropId.kpidTimeType;

                case "kpidBit64":
                    return ItemPropId.kpidBit64;

                case "kpidBigEndian":
                    return ItemPropId.kpidBigEndian;

                case "kpidCpu":
                    return ItemPropId.kpidCpu;

                case "kpidPhySize":
                    return ItemPropId.kpidPhySize;

                case "kpidHeadersSize":
                    return ItemPropId.kpidHeadersSize;

                case "kpidChecksum":
                    return ItemPropId.kpidChecksum;

                case "kpidCharacts":
                    return ItemPropId.kpidCharacts;

                case "kpidVa":
                    return ItemPropId.kpidVa;

                case "kpidId":
                    return ItemPropId.kpidId;

                case "kpidShortName":
                    return ItemPropId.kpidShortName;

                case "kpidCreatorApp":
                    return ItemPropId.kpidCreatorApp;

                case "kpidSectorSize":
                    return ItemPropId.kpidSectorSize;

                case "kpidPosixAttrib":
                    return ItemPropId.kpidPosixAttrib;

                case "kpidLink":
                    return ItemPropId.kpidLink;

                case "kpidTotalSize":
                    return ItemPropId.kpidTotalSize;

                case "kpidFreeSpace":
                    return ItemPropId.kpidFreeSpace;

                case "kpidClusterSize":
                    return ItemPropId.kpidClusterSize;

                case "kpidVolumeName":
                    return ItemPropId.kpidVolumeName;

                case "kpidLocalName":
                    return ItemPropId.kpidLocalName;

                case "kpidProvider":
                    return ItemPropId.kpidProvider;

                case "kpidUserDefined":
                    return ItemPropId.kpidUserDefined;
            }
            throw base.CreateUnknownConstantException(s, typeof(ItemPropId));
        }

        private FileTimeType Read161_FileTimeType(string s)
        {
            switch (s)
            {
                case "kWindows":
                    return FileTimeType.kWindows;

                case "kUnix":
                    return FileTimeType.kUnix;

                case "kDOS":
                    return FileTimeType.kDOS;
            }
            throw base.CreateUnknownConstantException(s, typeof(FileTimeType));
        }

        private ArchivePropId Read162_ArchivePropId(string s)
        {
            switch (s)
            {
                case "kName":
                    return ArchivePropId.kName;

                case "kClassID":
                    return ArchivePropId.kClassID;

                case "kExtension":
                    return ArchivePropId.kExtension;

                case "kAddExtension":
                    return ArchivePropId.kAddExtension;

                case "kUpdate":
                    return ArchivePropId.kUpdate;

                case "kKeepName":
                    return ArchivePropId.kKeepName;

                case "kStartSignature":
                    return ArchivePropId.kStartSignature;

                case "kFinishSignature":
                    return ArchivePropId.kFinishSignature;

                case "kAssociate":
                    return ArchivePropId.kAssociate;
            }
            throw base.CreateUnknownConstantException(s, typeof(ArchivePropId));
        }

        private KnownSevenZipFormat Read163_KnownSevenZipFormat(string s)
        {
            switch (s)
            {
                case "Unknown":
                    return KnownSevenZipFormat.Unknown;

                case "SevenZip":
                    return KnownSevenZipFormat.SevenZip;

                case "Arj":
                    return KnownSevenZipFormat.Arj;

                case "BZip2":
                    return KnownSevenZipFormat.BZip2;

                case "Cab":
                    return KnownSevenZipFormat.Cab;

                case "Chm":
                    return KnownSevenZipFormat.Chm;

                case "Compound":
                    return KnownSevenZipFormat.Compound;

                case "Cpio":
                    return KnownSevenZipFormat.Cpio;

                case "Deb":
                    return KnownSevenZipFormat.Deb;

                case "Dmg":
                    return KnownSevenZipFormat.Dmg;

                case "ELF":
                    return KnownSevenZipFormat.ELF;

                case "FAT":
                    return KnownSevenZipFormat.FAT;

                case "FLV":
                    return KnownSevenZipFormat.FLV;

                case "GZip":
                    return KnownSevenZipFormat.GZip;

                case "HFS":
                    return KnownSevenZipFormat.HFS;

                case "Iso":
                    return KnownSevenZipFormat.Iso;

                case "Lzh":
                    return KnownSevenZipFormat.Lzh;

                case "Lzma":
                    return KnownSevenZipFormat.Lzma;

                case "Lzma86":
                    return KnownSevenZipFormat.Lzma86;

                case "MachO":
                    return KnownSevenZipFormat.MachO;

                case "MBR":
                    return KnownSevenZipFormat.MBR;

                case "MsLz":
                    return KnownSevenZipFormat.MsLz;

                case "Mub":
                    return KnownSevenZipFormat.Mub;

                case "Nsis":
                    return KnownSevenZipFormat.Nsis;

                case "NTFS":
                    return KnownSevenZipFormat.NTFS;

                case "PE":
                    return KnownSevenZipFormat.PE;

                case "Rar":
                    return KnownSevenZipFormat.Rar;

                case "Rpm":
                    return KnownSevenZipFormat.Rpm;

                case "Split":
                    return KnownSevenZipFormat.Split;

                case "SWF":
                    return KnownSevenZipFormat.SWF;

                case "SWFc":
                    return KnownSevenZipFormat.SWFc;

                case "Tar":
                    return KnownSevenZipFormat.Tar;

                case "Udf":
                    return KnownSevenZipFormat.Udf;

                case "VHD":
                    return KnownSevenZipFormat.VHD;

                case "Wim":
                    return KnownSevenZipFormat.Wim;

                case "Xar":
                    return KnownSevenZipFormat.Xar;

                case "Xz":
                    return KnownSevenZipFormat.Xz;

                case "Z":
                    return KnownSevenZipFormat.Z;

                case "Zip":
                    return KnownSevenZipFormat.Zip;
            }
            throw base.CreateUnknownConstantException(s, typeof(KnownSevenZipFormat));
        }

        private SevenZipFormatCapabilities Read164_SevenZipFormatCapabilities(string s)
        {
            return (SevenZipFormatCapabilities) ((int) XmlSerializationReader.ToEnum(s, this.SevenZipFormatCapabilitiesValues, "global::Nomad.FileSystem.Archive.SevenZip.SevenZipFormatCapabilities"));
        }

        private CompressionLevel Read165_CompressionLevel(string s)
        {
            switch (s)
            {
                case "Store":
                    return CompressionLevel.Store;

                case "Fastest":
                    return CompressionLevel.Fastest;

                case "Fast":
                    return CompressionLevel.Fast;

                case "Normal":
                    return CompressionLevel.Normal;

                case "Maximum":
                    return CompressionLevel.Maximum;

                case "Ultra":
                    return CompressionLevel.Ultra;
            }
            throw base.CreateUnknownConstantException(s, typeof(CompressionLevel));
        }

        private CompressionMethod Read166_CompressionMethod(string s)
        {
            switch (s)
            {
                case "Copy":
                    return CompressionMethod.Copy;

                case "LZMA":
                    return CompressionMethod.LZMA;

                case "LZMA2":
                    return CompressionMethod.LZMA2;

                case "PPMd":
                    return CompressionMethod.PPMd;

                case "BZip2":
                    return CompressionMethod.BZip2;

                case "Deflate":
                    return CompressionMethod.Deflate;

                case "Deflate64":
                    return CompressionMethod.Deflate64;
            }
            throw base.CreateUnknownConstantException(s, typeof(CompressionMethod));
        }

        private EncryptionMethod Read167_EncryptionMethod(string s)
        {
            switch (s)
            {
                case "AES256":
                    return EncryptionMethod.AES256;

                case "ZipCrypto":
                    return EncryptionMethod.ZipCrypto;
            }
            throw base.CreateUnknownConstantException(s, typeof(EncryptionMethod));
        }

        private SevenZipPropertiesBuilder.SolidSizeUnit Read168_SolidSizeUnit(string s)
        {
            switch (s)
            {
                case "B":
                    return SevenZipPropertiesBuilder.SolidSizeUnit.B;

                case "Kb":
                    return SevenZipPropertiesBuilder.SolidSizeUnit.Kb;

                case "Mb":
                    return SevenZipPropertiesBuilder.SolidSizeUnit.Mb;

                case "Gb":
                    return SevenZipPropertiesBuilder.SolidSizeUnit.Gb;
            }
            throw base.CreateUnknownConstantException(s, typeof(SevenZipPropertiesBuilder.SolidSizeUnit));
        }

        private ComplexFilterView Read169_ComplexFilterView(string s)
        {
            switch (s)
            {
                case "Basic":
                    return ComplexFilterView.Basic;

                case "Advanced":
                    return ComplexFilterView.Advanced;

                case "Full":
                    return ComplexFilterView.Full;
            }
            throw base.CreateUnknownConstantException(s, typeof(ComplexFilterView));
        }

        private ListViewColumnInfo Read17_ListViewColumnInfo(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id11_ListViewColumnInfo) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ListViewColumnInfo o = new ListViewColumnInfo();
            bool[] flagArray = new bool[6];
            while (base.Reader.MoveToNextAttribute())
            {
                if ((!flagArray[0] && (base.Reader.LocalName == this.id248_DefaultWidth)) && (base.Reader.NamespaceURI == this.id2_Item))
                {
                    o.DefaultWidth = XmlConvert.ToInt32(base.Reader.Value);
                    flagArray[0] = true;
                }
                else
                {
                    if ((!flagArray[1] && (base.Reader.LocalName == this.id249_DisplayIndex)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.DisplayIndex = XmlConvert.ToInt32(base.Reader.Value);
                        flagArray[1] = true;
                        continue;
                    }
                    if ((!flagArray[2] && (base.Reader.LocalName == this.id250_TextAlign)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.TextAlign = this.Read16_HorizontalAlignment(base.Reader.Value);
                        flagArray[2] = true;
                        continue;
                    }
                    if ((!flagArray[3] && (base.Reader.LocalName == this.id251_Width)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Width = XmlConvert.ToInt32(base.Reader.Value);
                        flagArray[3] = true;
                        continue;
                    }
                    if ((!flagArray[4] && (base.Reader.LocalName == this.id252_Visible)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Visible = XmlConvert.ToBoolean(base.Reader.Value);
                        flagArray[4] = true;
                        continue;
                    }
                    if ((!flagArray[5] && (base.Reader.LocalName == this.id253_Property)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.PropertyStr = base.Reader.Value;
                        flagArray[5] = true;
                        continue;
                    }
                    if (!base.IsXmlnsAttribute(base.Reader.Name))
                    {
                        base.UnknownNode(o, ":DefaultWidth, :DisplayIndex, :TextAlign, :Width, :Visible, :Property");
                    }
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ViewFilters Read170_ViewFilters(string s)
        {
            return (ViewFilters) ((int) XmlSerializationReader.ToEnum(s, this.ViewFiltersValues, "global::Nomad.Controls.Filter.ViewFilters"));
        }

        private QuickFindOptions Read171_QuickFindOptions(string s)
        {
            return (QuickFindOptions) ((int) XmlSerializationReader.ToEnum(s, this.QuickFindOptionsValues, "global::Nomad.QuickFindOptions"));
        }

        private PanelContentContainer Read172_PanelContentContainer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id129_PanelContentContainer) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            PanelContentContainer o = new PanelContentContainer();
            bool[] flagArray = new bool[6];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id176_AggregatedFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read108_AggregatedVirtualItemFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id177_NameFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read103_VirtualItemNameFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id178_PropertyFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read84_VirtualPropertyFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id179_ContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read87_VirtualItemContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id180_NameListFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read107_VirtualItemNameListFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id181_HexContentFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read105_VirtualItemHexContentFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id182_AttributeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read94_VirtualItemAttributeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id183_SizeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read98_VirtualItemSizeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id184_DateFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read99_VirtualItemDateFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[0] && (base.Reader.LocalName == this.id185_TimeFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFilter = this.Read91_VirtualItemTimeFilter(false, true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id186_Locked)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.Locked = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        }
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id147_QuickFindOptions)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.QuickFindOptions = this.Read171_QuickFindOptions(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id187_FolderStream)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFolderStream = base.ToByteArrayBase64(false);
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id188_FolderPath)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFolderPath = base.Reader.ReadElementString();
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id189_Sort)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableSort = base.Reader.ReadElementString();
                        flagArray[5] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Locked, :QuickFindOptions, :FolderStream, :FolderPath, :Sort");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":AggregatedFilter, :NameFilter, :PropertyFilter, :ContentFilter, :NameListFilter, :HexContentFilter, :AttributeFilter, :SizeFilter, :DateFilter, :TimeFilter, :Locked, :QuickFindOptions, :FolderStream, :FolderPath, :Sort");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ControllerType Read173_ControllerType(string s)
        {
            switch (s)
            {
                case "Unknown":
                    return ControllerType.Unknown;

                case "Server":
                    return ControllerType.Server;

                case "Client":
                    return ControllerType.Client;
            }
            throw base.CreateUnknownConstantException(s, typeof(ControllerType));
        }

        private MarshalByRefObject Read174_MarshalByRefObject(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id265_MarshalByRefObject) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id131_Controller) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read175_Controller(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("MarshalByRefObject", "");
            }
            return null;
        }

        private Controller Read175_Controller(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id131_Controller) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            Controller o = new Controller();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private FormPlacement Read176_FormPlacement(string s)
        {
            return (FormPlacement) ((int) XmlSerializationReader.ToEnum(s, this.FormPlacementValues, "global::Nomad.Configuration.FormPlacement"));
        }

        private ArgumentKey Read177_ArgumentKey(string s)
        {
            switch (s)
            {
                case "None":
                    return ArgumentKey.None;

                case "Help":
                    return ArgumentKey.Help;

                case "Init":
                    return ArgumentKey.Init;

                case "Safe":
                    return ArgumentKey.Safe;

                case "NewInstance":
                    return ArgumentKey.NewInstance;

                case "OldInstance":
                    return ArgumentKey.OldInstance;

                case "Tab":
                    return ArgumentKey.Tab;

                case "LeftFolder":
                    return ArgumentKey.LeftFolder;

                case "RightFolder":
                    return ArgumentKey.RightFolder;

                case "CurrentFolder":
                    return ArgumentKey.CurrentFolder;

                case "FarFolder":
                    return ArgumentKey.FarFolder;

                case "Culture":
                    return ArgumentKey.Culture;

                case "FormLocalizer":
                    return ArgumentKey.FormLocalizer;

                case "RecoveryFolder":
                    return ArgumentKey.RecoveryFolder;

                case "Debug":
                    return ArgumentKey.Debug;

                case "Dump":
                    return ArgumentKey.Dump;

                case "Restart":
                    return ArgumentKey.Restart;
            }
            throw base.CreateUnknownConstantException(s, typeof(ArgumentKey));
        }

        private CanMoveListViewItem Read178_CanMoveListViewItem(string s)
        {
            return (CanMoveListViewItem) ((int) XmlSerializationReader.ToEnum(s, this.CanMoveListViewItemValues, "global::System.Windows.Forms.CanMoveListViewItem"));
        }

        private Trace Read179_Trace(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id135_Trace) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            Trace o = new Trace();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PanelLayoutEntry Read18_PanelLayoutEntry(string s)
        {
            return (PanelLayoutEntry) ((int) XmlSerializationReader.ToEnum(s, this.PanelLayoutEntryValues, "global::Nomad.Configuration.PanelLayoutEntry"));
        }

        private TwoPanelContainer.SinglePanel Read180_SinglePanel(string s)
        {
            switch (s)
            {
                case "None":
                    return TwoPanelContainer.SinglePanel.None;

                case "Left":
                    return TwoPanelContainer.SinglePanel.Left;

                case "Right":
                    return TwoPanelContainer.SinglePanel.Right;
            }
            throw base.CreateUnknownConstantException(s, typeof(TwoPanelContainer.SinglePanel));
        }

        private GeneralTab Read181_GeneralTab(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id137_GeneralTab) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id138_TwoPanelTab) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read182_TwoPanelTab(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            GeneralTab o = new GeneralTab();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id171_Caption)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Caption = base.Reader.ReadElementString();
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id172_Hotkey)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableHotkey = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Caption, :Hotkey");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Caption, :Hotkey");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private TwoPanelTab Read182_TwoPanelTab(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id138_TwoPanelTab) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            TwoPanelTab o = new TwoPanelTab();
            bool[] flagArray = new bool[5];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id171_Caption)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Caption = base.Reader.ReadElementString();
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id172_Hotkey)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableHotkey = base.Reader.ReadElementString();
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id173_Layout)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Layout = this.Read25_TwoPanelLayout(false, true);
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id174_Left)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Left = this.Read172_PanelContentContainer(false, true);
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id175_Right)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Right = this.Read172_PanelContentContainer(false, true);
                        flagArray[4] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Caption, :Hotkey, :Layout, :Left, :Right");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Caption, :Hotkey, :Layout, :Left, :Right");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ArchiveUpdateMode Read183_ArchiveUpdateMode(string s)
        {
            switch (s)
            {
                case "CreateNew":
                    return ArchiveUpdateMode.CreateNew;

                case "OverwriteAll":
                    return ArchiveUpdateMode.OverwriteAll;

                case "SkipAll":
                    return ArchiveUpdateMode.SkipAll;

                case "RefreshOld":
                    return ArchiveUpdateMode.RefreshOld;
            }
            throw base.CreateUnknownConstantException(s, typeof(ArchiveUpdateMode));
        }

        private PackStage Read184_PackStage(string s)
        {
            switch (s)
            {
                case "NotStarted":
                    return PackStage.NotStarted;

                case "CalculatingSize":
                    return PackStage.CalculatingSize;

                case "ReadingExistingArchive":
                    return PackStage.ReadingExistingArchive;

                case "MovingExistingItems":
                    return PackStage.MovingExistingItems;

                case "PackingNewItems":
                    return PackStage.PackingNewItems;

                case "Relocating":
                    return PackStage.Relocating;

                case "Finished":
                    return PackStage.Finished;
            }
            throw base.CreateUnknownConstantException(s, typeof(PackStage));
        }

        private ProcessedSize Read185_ProcessedSize(bool checkType)
        {
            ProcessedSize size;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id159_ProcessedSize) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                size = (ProcessedSize) Activator.CreateInstance(typeof(ProcessedSize), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
                goto Label_009A;
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.Workers.ProcessedSize");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.Workers.ProcessedSize");
            }
        Label_007B:
            if (!base.IsXmlnsAttribute(base.Reader.Name))
            {
                base.UnknownNode(size);
            }
        Label_009A:
            if (base.Reader.MoveToNextAttribute())
            {
                goto Label_007B;
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return size;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(size, "");
                }
                else
                {
                    base.UnknownNode(size, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return size;
        }

        private TimeSpan Read186_TimeSpan(bool checkType)
        {
            TimeSpan span;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id167_TimeSpan) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                span = (TimeSpan) Activator.CreateInstance(typeof(TimeSpan), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
                goto Label_009A;
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::System.TimeSpan");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::System.TimeSpan");
            }
        Label_007B:
            if (!base.IsXmlnsAttribute(base.Reader.Name))
            {
                base.UnknownNode(span);
            }
        Label_009A:
            if (base.Reader.MoveToNextAttribute())
            {
                goto Label_007B;
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return span;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(span, "");
                }
                else
                {
                    base.UnknownNode(span, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return span;
        }

        private PackProgressSnapshot Read187_PackProgressSnapshot(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id141_PackProgressSnapshot) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            PackProgressSnapshot o = new PackProgressSnapshot();
            bool[] flagArray = new bool[6];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id162_Processed)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Processed = this.Read185_ProcessedSize(true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id163_TotalCount)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.TotalCount = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id164_ProcessedCount)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ProcessedCount = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id166_Duration)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Duration = this.Read186_TimeSpan(true);
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id169_CompressionRatio)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.CompressionRatio = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id170_Stage)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Stage = this.Read184_PackStage(base.Reader.ReadElementString());
                        flagArray[5] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Processed, :TotalCount, :ProcessedCount, :Duration, :CompressionRatio, :Stage");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Processed, :TotalCount, :ProcessedCount, :Duration, :CompressionRatio, :Stage");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private CustomBackgroundWorker Read188_CustomBackgroundWorker(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id142_CustomBackgroundWorker) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id143_EventBackgroundWorker) && (type.Namespace == this.id2_Item))
                {
                    return this.Read189_EventBackgroundWorker(isNullable, false);
                }
                if ((type.Name != this.id149_CustomAsyncFolder) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read194_CustomAsyncFolder(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            CustomBackgroundWorker o = new CustomBackgroundWorker();
            bool[] flagArray = new bool[1];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id168_CancellationPending)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.CancellationPending = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":CancellationPending");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":CancellationPending");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private EventBackgroundWorker Read189_EventBackgroundWorker(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id143_EventBackgroundWorker) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id149_CustomAsyncFolder) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read194_CustomAsyncFolder(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("EventBackgroundWorker", "");
            }
            return null;
        }

        private PanelToolbar Read19_PanelToolbar(string s)
        {
            return (PanelToolbar) ((int) XmlSerializationReader.ToEnum(s, this.PanelToolbarValues, "global::Nomad.Configuration.PanelToolbar"));
        }

        private CopyDestinationItem Read190_CopyDestinationItem(string s)
        {
            switch (s)
            {
                case "Ask":
                    return CopyDestinationItem.Ask;

                case "File":
                    return CopyDestinationItem.File;

                case "Folder":
                    return CopyDestinationItem.Folder;
            }
            throw base.CreateUnknownConstantException(s, typeof(CopyDestinationItem));
        }

        private MessageDialogResult Read191_MessageDialogResult(string s)
        {
            switch (s)
            {
                case "None":
                    return MessageDialogResult.None;

                case "OK":
                    return MessageDialogResult.OK;

                case "Cancel":
                    return MessageDialogResult.Cancel;

                case "Yes":
                    return MessageDialogResult.Yes;

                case "No":
                    return MessageDialogResult.No;

                case "YesToAll":
                    return MessageDialogResult.YesToAll;

                case "NoToAll":
                    return MessageDialogResult.NoToAll;

                case "Skip":
                    return MessageDialogResult.Skip;

                case "SkipAll":
                    return MessageDialogResult.SkipAll;

                case "Abort":
                    return MessageDialogResult.Abort;

                case "Retry":
                    return MessageDialogResult.Retry;

                case "Ignore":
                    return MessageDialogResult.Ignore;

                case "Shield":
                    return MessageDialogResult.Shield;
            }
            throw base.CreateUnknownConstantException(s, typeof(MessageDialogResult));
        }

        private DoubleClickAction Read192_DoubleClickAction(string s)
        {
            switch (s)
            {
                case "None":
                    return DoubleClickAction.None;

                case "GoToParent":
                    return DoubleClickAction.GoToParent;

                case "SelectAll":
                    return DoubleClickAction.SelectAll;

                case "UnselectAll":
                    return DoubleClickAction.UnselectAll;

                case "ToggleSelection":
                    return DoubleClickAction.ToggleSelection;
            }
            throw base.CreateUnknownConstantException(s, typeof(DoubleClickAction));
        }

        private VirtualFilePanel.ListViewSort Read193_ListViewSort(string s)
        {
            switch (s)
            {
                case "None":
                    return VirtualFilePanel.ListViewSort.None;

                case "Fast":
                    return VirtualFilePanel.ListViewSort.Fast;

                case "Full":
                    return VirtualFilePanel.ListViewSort.Full;
            }
            throw base.CreateUnknownConstantException(s, typeof(VirtualFilePanel.ListViewSort));
        }

        private CustomAsyncFolder Read194_CustomAsyncFolder(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id149_CustomAsyncFolder) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("CustomAsyncFolder", "");
            }
            return null;
        }

        private SearchFolderOptions Read195_SearchFolderOptions(string s)
        {
            return (SearchFolderOptions) ((int) XmlSerializationReader.ToEnum(s, this.SearchFolderOptionsValues, "global::Nomad.FileSystem.Virtual.SearchFolderOptions"));
        }

        private FindDuplicateOptions Read196_FindDuplicateOptions(string s)
        {
            return (FindDuplicateOptions) ((int) XmlSerializationReader.ToEnum(s, this.FindDuplicateOptionsValues, "global::Nomad.FileSystem.Virtual.FindDuplicateOptions"));
        }

        private Compare Read197_Compare(string s)
        {
            switch (s)
            {
                case "Always":
                    return Compare.Always;

                case "Equal":
                    return Compare.Equal;

                case "Greater":
                    return Compare.Greater;

                case "GreaterEqual":
                    return Compare.GreaterEqual;

                case "Less":
                    return Compare.Less;

                case "LessEqual":
                    return Compare.LessEqual;

                case "Never":
                    return Compare.Never;

                case "NotEqual":
                    return Compare.NotEqual;
            }
            throw base.CreateUnknownConstantException(s, typeof(Compare));
        }

        private ChangeItemAction Read198_ChangeItemAction(string s)
        {
            switch (s)
            {
                case "None":
                    return ChangeItemAction.None;

                case "Retry":
                    return ChangeItemAction.Retry;

                case "Ignore":
                    return ChangeItemAction.Ignore;

                case "Skip":
                    return ChangeItemAction.Skip;

                case "Cancel":
                    return ChangeItemAction.Cancel;
            }
            throw base.CreateUnknownConstantException(s, typeof(ChangeItemAction));
        }

        private AvailableItemActions Read199_AvailableItemActions(string s)
        {
            return (AvailableItemActions) ((int) XmlSerializationReader.ToEnum(s, this.AvailableItemActionsValues, "global::Nomad.AvailableItemActions"));
        }

        private ImageProvider Read2_ImageProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id1_ImageProvider) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id44_ShellImageProvider) && (type.Namespace == this.id2_Item))
                {
                    return this.Read54_ShellImageProvider(isNullable, false);
                }
                if ((type.Name != this.id3_CustomImageProvider) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read3_CustomImageProvider(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("ImageProvider", "");
            }
            return null;
        }

        private Orientation Read20_Orientation(string s)
        {
            switch (s)
            {
                case "Horizontal":
                    return Orientation.Horizontal;

                case "Vertical":
                    return Orientation.Vertical;
            }
            throw base.CreateUnknownConstantException(s, typeof(Orientation));
        }

        private CompareFoldersOptions Read200_CompareFoldersOptions(string s)
        {
            return (CompareFoldersOptions) ((int) XmlSerializationReader.ToEnum(s, this.CompareFoldersOptionsValues, "global::Nomad.Workers.CompareFoldersOptions"));
        }

        private OverwriteDialogResult Read201_OverwriteDialogResult(string s)
        {
            switch (s)
            {
                case "None":
                    return OverwriteDialogResult.None;

                case "Overwrite":
                    return OverwriteDialogResult.Overwrite;

                case "Append":
                    return OverwriteDialogResult.Append;

                case "Resume":
                    return OverwriteDialogResult.Resume;

                case "Rename":
                    return OverwriteDialogResult.Rename;

                case "Skip":
                    return OverwriteDialogResult.Skip;

                case "Abort":
                    return OverwriteDialogResult.Abort;
            }
            throw base.CreateUnknownConstantException(s, typeof(OverwriteDialogResult));
        }

        private CopyWorkerOptions Read202_CopyWorkerOptions(string s)
        {
            return (CopyWorkerOptions) ((int) XmlSerializationReader.ToEnum(s, this.CopyWorkerOptionsValues, "global::Nomad.Workers.CopyWorkerOptions"));
        }

        private CopyMode Read203_CopyMode(string s)
        {
            switch (s)
            {
                case "Sync":
                    return CopyMode.Sync;

                case "Async":
                    return CopyMode.Async;

                case "System":
                    return CopyMode.System;
            }
            throw base.CreateUnknownConstantException(s, typeof(CopyMode));
        }

        private CopyProgressSnapshot Read204_CopyProgressSnapshot(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id160_CopyProgressSnapshot) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            CopyProgressSnapshot o = new CopyProgressSnapshot();
            bool[] flagArray = new bool[6];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id162_Processed)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Processed = this.Read185_ProcessedSize(true);
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id163_TotalCount)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.TotalCount = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id164_ProcessedCount)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ProcessedCount = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id165_SkippedCount)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SkippedCount = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id166_Duration)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Duration = this.Read186_TimeSpan(true);
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id158_CopyMode)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.CopyMode = this.Read203_CopyMode(base.Reader.ReadElementString());
                        flagArray[5] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Processed, :TotalCount, :ProcessedCount, :SkippedCount, :Duration, :CopyMode");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Processed, :TotalCount, :ProcessedCount, :SkippedCount, :Duration, :CopyMode");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private IconStyle Read205_IconStyle(string s)
        {
            return (IconStyle) ((int) XmlSerializationReader.ToEnum(s, this.IconStyleValues, "global::Nomad.FileSystem.Virtual.IconStyle"));
        }

        public object Read206_ImageProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id1_ImageProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read2_ImageProvider(true, true);
            }
            base.UnknownNode(null, ":ImageProvider");
            return null;
        }

        public object Read207_CustomImageProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id3_CustomImageProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read3_CustomImageProvider(true, true);
            }
            base.UnknownNode(null, ":CustomImageProvider");
            return null;
        }

        public object Read208_KeysConverter2()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id4_KeysConverter2) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read6_KeysConverter2(true, true);
            }
            base.UnknownNode(null, ":KeysConverter2");
            return null;
        }

        public object Read209_PropertyTagType()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id5_PropertyTagType) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read7_PropertyTagType(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PropertyTagType");
            return null;
        }

        private PanelView Read21_PanelView(string s)
        {
            switch (s)
            {
                case "LargeIcon":
                    return PanelView.LargeIcon;

                case "Details":
                    return PanelView.Details;

                case "SmallIcon":
                    return PanelView.SmallIcon;

                case "List":
                    return PanelView.List;

                case "Tile":
                    return PanelView.Tile;

                case "Thumbnail":
                    return PanelView.Thumbnail;
            }
            throw base.CreateUnknownConstantException(s, typeof(PanelView));
        }

        public object Read210_PropertyTag()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id6_PropertyTag) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read8_PropertyTag(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PropertyTag");
            return null;
        }

        public object Read211_LightSource()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id7_LightSource) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read9_LightSource(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":LightSource");
            return null;
        }

        public object Read212_ToolStripButtonRenderer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id8_ToolStripButtonRenderer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read11_ToolStripButtonRenderer(true, true);
            }
            base.UnknownNode(null, ":ToolStripButtonRenderer");
            return null;
        }

        public object Read213_ConfigurableSettingsProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id9_ConfigurableSettingsProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read14_ConfigurableSettingsProvider(true, true);
            }
            base.UnknownNode(null, ":ConfigurableSettingsProvider");
            return null;
        }

        public object Read214_ReleaseType()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id10_ReleaseType) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read15_ReleaseType(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ReleaseType");
            return null;
        }

        public object Read215_ListViewColumnInfo()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id11_ListViewColumnInfo) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read17_ListViewColumnInfo(true, true);
            }
            base.UnknownNode(null, ":ListViewColumnInfo");
            return null;
        }

        public object Read216_ArrayOfListViewColumnInfo()
        {
            object obj2 = null;
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id12_ArrayOfListViewColumnInfo) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                if (!base.ReadNull())
                {
                    if (obj2 == null)
                    {
                        obj2 = new ListViewColumnCollection();
                    }
                    ListViewColumnCollection columns = (ListViewColumnCollection) obj2;
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        return obj2;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int whileIterations = 0;
                    int readerCount = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            if ((base.Reader.LocalName == this.id11_ListViewColumnInfo) && (base.Reader.NamespaceURI == this.id2_Item))
                            {
                                if (columns == null)
                                {
                                    base.Reader.Skip();
                                }
                                else
                                {
                                    columns.Add(this.Read17_ListViewColumnInfo(true, true));
                                }
                            }
                            else
                            {
                                base.UnknownNode(null, ":ListViewColumnInfo");
                            }
                        }
                        else
                        {
                            base.UnknownNode(null, ":ListViewColumnInfo");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref whileIterations, ref readerCount);
                    }
                    base.ReadEndElement();
                    return obj2;
                }
                if (obj2 == null)
                {
                    obj2 = new ListViewColumnCollection();
                }
                ListViewColumnCollection collection1 = (ListViewColumnCollection) obj2;
                return obj2;
            }
            base.UnknownNode(null, ":ArrayOfListViewColumnInfo");
            return obj2;
        }

        public object Read217_PanelLayoutEntry()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id13_PanelLayoutEntry) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read18_PanelLayoutEntry(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PanelLayoutEntry");
            return null;
        }

        public object Read218_PanelToolbar()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id14_PanelToolbar) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read19_PanelToolbar(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PanelToolbar");
            return null;
        }

        public object Read219_PanelLayout()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id15_PanelLayout) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read22_PanelLayout(true, true);
            }
            base.UnknownNode(null, ":PanelLayout");
            return null;
        }

        private PanelLayout Read22_PanelLayout(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id15_PanelLayout) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            PanelLayout o = new PanelLayout();
            bool[] flagArray = new bool[11];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id239_FolderBarVisible)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.FolderBarVisible = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        }
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id240_FolderBarOrientation)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.FolderBarOrientation = this.Read20_Orientation(base.Reader.ReadElementString());
                        }
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id235_SplitterPercent)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.SplitterPercent = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id241_View)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.View = this.Read21_PanelView(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id242_AutoSizeColumns)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.AutoSizeColumns = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        }
                        flagArray[4] = true;
                    }
                    else if ((base.Reader.LocalName == this.id243_Columns) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (!base.ReadNull())
                        {
                            ListViewColumnInfo[] a = null;
                            int index = 0;
                            if (base.Reader.IsEmptyElement)
                            {
                                base.Reader.Skip();
                            }
                            else
                            {
                                base.Reader.ReadStartElement();
                                base.Reader.MoveToContent();
                                int num4 = 0;
                                int num5 = base.ReaderCount;
                                while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                                {
                                    if (base.Reader.NodeType == XmlNodeType.Element)
                                    {
                                        if ((base.Reader.LocalName == this.id11_ListViewColumnInfo) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            a = (ListViewColumnInfo[]) base.EnsureArrayIndex(a, index, typeof(ListViewColumnInfo));
                                            a[index++] = this.Read17_ListViewColumnInfo(true, true);
                                        }
                                        else
                                        {
                                            base.UnknownNode(null, ":ListViewColumnInfo");
                                        }
                                    }
                                    else
                                    {
                                        base.UnknownNode(null, ":ListViewColumnInfo");
                                    }
                                    base.Reader.MoveToContent();
                                    base.CheckReaderCount(ref num4, ref num5);
                                }
                                base.ReadEndElement();
                            }
                            o.Columns = (ListViewColumnInfo[]) base.ShrinkArray(a, index, typeof(ListViewColumnInfo), false);
                        }
                    }
                    else if ((!flagArray[6] && (base.Reader.LocalName == this.id244_ListColumnCount)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.ListColumnCount = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        }
                        flagArray[6] = true;
                    }
                    else if ((!flagArray[7] && (base.Reader.LocalName == this.id245_ToolbarsVisible)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.ToolbarsVisible = this.Read19_PanelToolbar(base.Reader.ReadElementString());
                        }
                        flagArray[7] = true;
                    }
                    else if ((!flagArray[8] && (base.Reader.LocalName == this.id236_StoreEntry)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.StoreEntry = this.Read18_PanelLayoutEntry(base.Reader.ReadElementString());
                        }
                        flagArray[8] = true;
                    }
                    else if ((!flagArray[9] && (base.Reader.LocalName == this.id246_ThumbnailSize)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableThumbnailSize = base.Reader.ReadElementString();
                        flagArray[9] = true;
                    }
                    else if ((!flagArray[10] && (base.Reader.LocalName == this.id247_ThumbnailSpacing)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableThumbnailSpacing = base.Reader.ReadElementString();
                        flagArray[10] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":FolderBarVisible, :FolderBarOrientation, :SplitterPercent, :View, :AutoSizeColumns, :Columns, :ListColumnCount, :ToolbarsVisible, :StoreEntry, :ThumbnailSize, :ThumbnailSpacing");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":FolderBarVisible, :FolderBarOrientation, :SplitterPercent, :View, :AutoSizeColumns, :Columns, :ListColumnCount, :ToolbarsVisible, :StoreEntry, :ThumbnailSize, :ThumbnailSpacing");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        public object Read220_ActivePanel()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id16_ActivePanel) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read23_ActivePanel(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ActivePanel");
            return null;
        }

        public object Read221_TwoPanelLayoutEntry()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id17_TwoPanelLayoutEntry) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read24_TwoPanelLayoutEntry(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":TwoPanelLayoutEntry");
            return null;
        }

        public object Read222_TwoPanelLayout()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id18_TwoPanelLayout) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read25_TwoPanelLayout(true, true);
            }
            base.UnknownNode(null, ":TwoPanelLayout");
            return null;
        }

        public object Read223_CustomActionLink()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id19_CustomActionLink) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read26_CustomActionLink(true, true);
            }
            base.UnknownNode(null, ":CustomActionLink");
            return null;
        }

        public object Read224_CustomBindActionLink()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id20_CustomBindActionLink) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read27_CustomBindActionLink(true, true);
            }
            base.UnknownNode(null, ":CustomBindActionLink");
            return null;
        }

        public object Read225_ActionState()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id21_ActionState) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read28_ActionState(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ActionState");
            return null;
        }

        public object Read226_BindActionProperty()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id22_BindActionProperty) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read29_BindActionProperty(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":BindActionProperty");
            return null;
        }

        public object Read227_BreadcrumbView()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id23_BreadcrumbView) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read30_BreadcrumbView(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":BreadcrumbView");
            return null;
        }

        public object Read228_BreadcrumbToolStripRenderer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id24_BreadcrumbToolStripRenderer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read32_BreadcrumbToolStripRenderer(true, true);
            }
            base.UnknownNode(null, ":BreadcrumbToolStripRenderer");
            return null;
        }

        public object Read229_InputDialogOption()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id25_InputDialogOption) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read33_InputDialogOption(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":InputDialogOption");
            return null;
        }

        private ActivePanel Read23_ActivePanel(string s)
        {
            switch (s)
            {
                case "Unchanged":
                    return ActivePanel.Unchanged;

                case "Left":
                    return ActivePanel.Left;

                case "Right":
                    return ActivePanel.Right;
            }
            throw base.CreateUnknownConstantException(s, typeof(ActivePanel));
        }

        public object Read230_ElevatedProcess()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id26_ElevatedProcess) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read34_ElevatedProcess(true, true);
            }
            base.UnknownNode(null, ":ElevatedProcess");
            return null;
        }

        public object Read231_ArchiveFormatConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id27_ArchiveFormatConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read35_ArchiveFormatConverter(true, true);
            }
            base.UnknownNode(null, ":ArchiveFormatConverter");
            return null;
        }

        public object Read232_ArchiveFormatCapabilities()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id28_ArchiveFormatCapabilities) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read36_ArchiveFormatCapabilities(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ArchiveFormatCapabilities");
            return null;
        }

        public object Read233_ArchiveFormatInfo()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id29_ArchiveFormatInfo) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read37_ArchiveFormatInfo(true, true);
            }
            base.UnknownNode(null, ":ArchiveFormatInfo");
            return null;
        }

        public object Read234_PersistArchiveFormatInfo()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id30_PersistArchiveFormatInfo) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read38_PersistArchiveFormatInfo(true, true);
            }
            base.UnknownNode(null, ":PersistArchiveFormatInfo");
            return null;
        }

        public object Read235_FindFormatSource()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id31_FindFormatSource) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read39_FindFormatSource(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":FindFormatSource");
            return null;
        }

        public object Read236_ArjHeader()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id32_ArjHeader) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read40_ArjHeader(true);
            }
            base.UnknownNode(null, ":ArjHeader");
            return null;
        }

        public object Read237_ProcessItemEventArgs()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id33_ProcessItemEventArgs) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read43_ProcessItemEventArgs(true, true);
            }
            base.UnknownNode(null, ":ProcessItemEventArgs");
            return null;
        }

        public object Read238_ProcessorState()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id34_ProcessorState) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read44_ProcessorState(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ProcessorState");
            return null;
        }

        public object Read239_SequenseProcessorType()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id35_SequenseProcessorType) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read45_SequenseProcessorType(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":SequenseProcessorType");
            return null;
        }

        private TwoPanelLayoutEntry Read24_TwoPanelLayoutEntry(string s)
        {
            return (TwoPanelLayoutEntry) ((int) XmlSerializationReader.ToEnum(s, this.TwoPanelLayoutEntryValues, "global::Nomad.Configuration.TwoPanelLayoutEntry"));
        }

        public object Read240_PK_OM()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id36_PK_OM) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read46_PK_OM(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PK_OM");
            return null;
        }

        public object Read241_PK_OPERATION()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id37_PK_OPERATION) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read47_PK_OPERATION(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PK_OPERATION");
            return null;
        }

        public object Read242_PK_CAPS()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id38_PK_CAPS) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read48_PK_CAPS(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PK_CAPS");
            return null;
        }

        public object Read243_PK_VOL()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id39_PK_VOL) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read49_PK_VOL(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PK_VOL");
            return null;
        }

        public object Read244_PK_PACK()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id40_PK_PACK) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read50_PK_PACK(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PK_PACK");
            return null;
        }

        public object Read245_PackDefaultParamStruct()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id41_PackDefaultParamStruct) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read51_PackDefaultParamStruct(true);
            }
            base.UnknownNode(null, ":PackDefaultParamStruct");
            return null;
        }

        public object Read246_WcxErrors()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id42_WcxErrors) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read52_WcxErrors(true, true);
            }
            base.UnknownNode(null, ":WcxErrors");
            return null;
        }

        public object Read247_DefaultIcon()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id43_DefaultIcon) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read53_DefaultIcon(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":DefaultIcon");
            return null;
        }

        public object Read248_ShellImageProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id44_ShellImageProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read54_ShellImageProvider(true, true);
            }
            base.UnknownNode(null, ":ShellImageProvider");
            return null;
        }

        public object Read249_ItemCapability()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id45_ItemCapability) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read55_ItemCapability(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ItemCapability");
            return null;
        }

        private TwoPanelLayout Read25_TwoPanelLayout(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id18_TwoPanelLayout) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            TwoPanelLayout o = new TwoPanelLayout();
            bool[] flagArray = new bool[8];
            while (base.Reader.MoveToNextAttribute())
            {
                if ((!flagArray[0] && (base.Reader.LocalName == this.id232_name)) && (base.Reader.NamespaceURI == this.id2_Item))
                {
                    o.Name = base.Reader.Value;
                    flagArray[0] = true;
                }
                else if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o, ":name");
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[1] && (base.Reader.LocalName == this.id233_OnePanel)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.OnePanel = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        }
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id234_PanelsOrientation)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.PanelsOrientation = this.Read20_Orientation(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id235_SplitterPercent)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.SplitterPercent = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id16_ActivePanel)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.ActivePanel = this.Read23_ActivePanel(base.Reader.ReadElementString());
                        }
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id236_StoreEntry)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.StoreEntry = this.Read24_TwoPanelLayoutEntry(base.Reader.ReadElementString());
                        flagArray[5] = true;
                    }
                    else if ((!flagArray[6] && (base.Reader.LocalName == this.id237_LeftLayout)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.LeftLayout = this.Read22_PanelLayout(false, true);
                        flagArray[6] = true;
                    }
                    else if ((!flagArray[7] && (base.Reader.LocalName == this.id238_RightLayout)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.RightLayout = this.Read22_PanelLayout(false, true);
                        flagArray[7] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":OnePanel, :PanelsOrientation, :SplitterPercent, :ActivePanel, :StoreEntry, :LeftLayout, :RightLayout");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":OnePanel, :PanelsOrientation, :SplitterPercent, :ActivePanel, :StoreEntry, :LeftLayout, :RightLayout");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        public object Read250_LocalFileSystemCreator()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id46_LocalFileSystemCreator) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read56_LocalFileSystemCreator(true, true);
            }
            base.UnknownNode(null, ":LocalFileSystemCreator");
            return null;
        }

        public object Read251_NetworkFileSystemCreator()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id47_NetworkFileSystemCreator) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read57_NetworkFileSystemCreator(true, true);
            }
            base.UnknownNode(null, ":NetworkFileSystemCreator");
            return null;
        }

        public object Read252_ShellFileSystemCreator()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id48_ShellFileSystemCreator) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read58_ShellFileSystemCreator(true, true);
            }
            base.UnknownNode(null, ":ShellFileSystemCreator");
            return null;
        }

        public object Read253_ContentFlag()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id49_ContentFlag) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read59_ContentFlag(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ContentFlag");
            return null;
        }

        public object Read254_ContentDefaultParamStruct()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id50_ContentDefaultParamStruct) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read60_ContentDefaultParamStruct(true);
            }
            base.UnknownNode(null, ":ContentDefaultParamStruct");
            return null;
        }

        public object Read255_tdateformat()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id51_tdateformat) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read61_tdateformat(true);
            }
            base.UnknownNode(null, ":tdateformat");
            return null;
        }

        public object Read256_ttimeformat()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id52_ttimeformat) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read62_ttimeformat(true);
            }
            base.UnknownNode(null, ":ttimeformat");
            return null;
        }

        public object Read257_WdxFieldInfo()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id53_WdxFieldInfo) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read63_WdxFieldInfo(true, true);
            }
            base.UnknownNode(null, ":WdxFieldInfo");
            return null;
        }

        public object Read258_AggregatedFilterCondition()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id54_AggregatedFilterCondition) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read64_AggregatedFilterCondition(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":AggregatedFilterCondition");
            return null;
        }

        public object Read259_AggregatedVirtualItemFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id55_AggregatedVirtualItemFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read108_AggregatedVirtualItemFilter(true, true);
            }
            base.UnknownNode(null, ":AggregatedVirtualItemFilter");
            return null;
        }

        private CustomActionLink Read26_CustomActionLink(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id19_CustomActionLink) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id20_CustomBindActionLink) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read27_CustomBindActionLink(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("CustomActionLink", "");
            }
            return null;
        }

        public object Read260_FilterContainer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id56_FilterContainer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read109_FilterContainer(true, true);
            }
            base.UnknownNode(null, ":FilterContainer");
            return null;
        }

        public object Read261_NamedFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id57_NamedFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read110_NamedFilter(true, true);
            }
            base.UnknownNode(null, ":NamedFilter");
            return null;
        }

        public object Read262_FilterHelper()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id58_FilterHelper) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read111_FilterHelper(true, true);
            }
            base.UnknownNode(null, ":FilterHelper");
            return null;
        }

        public object Read263_VirtualItemNameFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id59_VirtualItemNameFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read103_VirtualItemNameFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemNameFilter");
            return null;
        }

        public object Read264_VirtualItemFullNameFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id60_VirtualItemFullNameFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read112_VirtualItemFullNameFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemFullNameFilter");
            return null;
        }

        public object Read265_VirtualItemAttributeFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id61_VirtualItemAttributeFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read94_VirtualItemAttributeFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemAttributeFilter");
            return null;
        }

        public object Read266_VirtualItemSizeFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id62_VirtualItemSizeFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read98_VirtualItemSizeFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemSizeFilter");
            return null;
        }

        public object Read267_ItemDateTimePart()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id63_ItemDateTimePart) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read90_ItemDateTimePart(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ItemDateTimePart");
            return null;
        }

        public object Read268_VirtualItemDateFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id64_VirtualItemDateFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read99_VirtualItemDateFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemDateFilter");
            return null;
        }

        public object Read269_VirtualItemTimeFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id65_VirtualItemTimeFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read91_VirtualItemTimeFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemTimeFilter");
            return null;
        }

        private CustomBindActionLink Read27_CustomBindActionLink(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id20_CustomBindActionLink) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("CustomBindActionLink", "");
            }
            return null;
        }

        public object Read270_VirtualItemContentFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id66_VirtualItemContentFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read87_VirtualItemContentFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemContentFilter");
            return null;
        }

        public object Read271_VirtualItemHexContentFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id67_VirtualItemHexContentFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read105_VirtualItemHexContentFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemHexContentFilter");
            return null;
        }

        public object Read272_NameListCondition()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id68_NameListCondition) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read106_NameListCondition(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":NameListCondition");
            return null;
        }

        public object Read273_VirtualItemNameListFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id69_VirtualItemNameListFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read107_VirtualItemNameListFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualItemNameListFilter");
            return null;
        }

        public object Read274_VirtualPropertyFilter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id70_VirtualPropertyFilter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read84_VirtualPropertyFilter(true, true);
            }
            base.UnknownNode(null, ":VirtualPropertyFilter");
            return null;
        }

        public object Read275_VirtualHighligher()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id71_VirtualHighligher) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read114_VirtualHighligher(true, true);
            }
            base.UnknownNode(null, ":VirtualHighligher");
            return null;
        }

        public object Read276_ListViewHighlighter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id72_ListViewHighlighter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read115_ListViewHighlighter(true, true);
            }
            base.UnknownNode(null, ":ListViewHighlighter");
            return null;
        }

        public object Read277_HighlighterIconType()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id73_HighlighterIconType) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read113_HighlighterIconType(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":HighlighterIconType");
            return null;
        }

        public object Read278_HashPropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id74_HashPropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read116_HashPropertyProvider(true, true);
            }
            base.UnknownNode(null, ":HashPropertyProvider");
            return null;
        }

        public object Read279_VistaThumbnailProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id75_VistaThumbnailProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read117_VistaThumbnailProvider(true, true);
            }
            base.UnknownNode(null, ":VistaThumbnailProvider");
            return null;
        }

        private ActionState Read28_ActionState(string s)
        {
            return (ActionState) ((int) XmlSerializationReader.ToEnum(s, this.ActionStateValues, "global::Nomad.Controls.Actions.ActionState"));
        }

        public object Read280_CustomizeFolderParts()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id76_CustomizeFolderParts) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read118_CustomizeFolderParts(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CustomizeFolderParts");
            return null;
        }

        public object Read281_ColorSpace()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id77_ColorSpace) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read119_ColorSpace(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ColorSpace");
            return null;
        }

        public object Read282_DescriptionPropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id78_DescriptionPropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read120_DescriptionPropertyProvider(true, true);
            }
            base.UnknownNode(null, ":DescriptionPropertyProvider");
            return null;
        }

        public object Read283_DummyClientSite()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id79_DummyClientSite) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read121_DummyClientSite(true, true);
            }
            base.UnknownNode(null, ":DummyClientSite");
            return null;
        }

        public object Read284_HtmlPropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id80_HtmlPropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read122_HtmlPropertyProvider(true, true);
            }
            base.UnknownNode(null, ":HtmlPropertyProvider");
            return null;
        }

        public object Read285_BitrateTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id81_BitrateTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read124_BitrateTypeConverter(true, true);
            }
            base.UnknownNode(null, ":BitrateTypeConverter");
            return null;
        }

        public object Read286_AudioChannelsTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id82_AudioChannelsTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read125_AudioChannelsTypeConverter(true, true);
            }
            base.UnknownNode(null, ":AudioChannelsTypeConverter");
            return null;
        }

        public object Read287_AudioSampleRateTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id83_AudioSampleRateTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read126_AudioSampleRateTypeConverter(true, true);
            }
            base.UnknownNode(null, ":AudioSampleRateTypeConverter");
            return null;
        }

        public object Read288_DurationTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id84_DurationTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read127_DurationTypeConverter(true, true);
            }
            base.UnknownNode(null, ":DurationTypeConverter");
            return null;
        }

        public object Read289_ImageSizeTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id85_ImageSizeTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read128_ImageSizeTypeConverter(true, true);
            }
            base.UnknownNode(null, ":ImageSizeTypeConverter");
            return null;
        }

        private BindActionProperty Read29_BindActionProperty(string s)
        {
            return (BindActionProperty) ((int) XmlSerializationReader.ToEnum(s, this.BindActionPropertyValues, "global::Nomad.Controls.Actions.BindActionProperty"));
        }

        public object Read290_DPITypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id86_DPITypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read129_DPITypeConverter(true, true);
            }
            base.UnknownNode(null, ":DPITypeConverter");
            return null;
        }

        public object Read291_ISOSpeedTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id87_ISOSpeedTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read130_ISOSpeedTypeConverter(true, true);
            }
            base.UnknownNode(null, ":ISOSpeedTypeConverter");
            return null;
        }

        public object Read292_RatingTypeConverter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id88_RatingTypeConverter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read131_RatingTypeConverter(true, true);
            }
            base.UnknownNode(null, ":RatingTypeConverter");
            return null;
        }

        public object Read293_EncodingConveter()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id89_EncodingConveter) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read132_EncodingConveter(true, true);
            }
            base.UnknownNode(null, ":EncodingConveter");
            return null;
        }

        public object Read294_ImagePropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id90_ImagePropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read133_ImagePropertyProvider(true, true);
            }
            base.UnknownNode(null, ":ImagePropertyProvider");
            return null;
        }

        public object Read295_PsdPropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id91_PsdPropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read134_PsdPropertyProvider(true, true);
            }
            base.UnknownNode(null, ":PsdPropertyProvider");
            return null;
        }

        public object Read296_TagLibPropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id92_TagLibPropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read135_TagLibPropertyProvider(true, true);
            }
            base.UnknownNode(null, ":TagLibPropertyProvider");
            return null;
        }

        public object Read297_TextPropertyProvider()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id93_TextPropertyProvider) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read136_TextPropertyProvider(true, true);
            }
            base.UnknownNode(null, ":TextPropertyProvider");
            return null;
        }

        public object Read298_VirtualToolTip()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id94_VirtualToolTip) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read137_VirtualToolTip(true, true);
            }
            base.UnknownNode(null, ":VirtualToolTip");
            return null;
        }

        public object Read299_ThrobberStyle()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id95_ThrobberStyle) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read138_ThrobberStyle(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ThrobberStyle");
            return null;
        }

        private CustomImageProvider Read3_CustomImageProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id3_CustomImageProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            CustomImageProvider o = new CustomImageProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private BreadcrumbView Read30_BreadcrumbView(string s)
        {
            switch (s)
            {
                case "Breadcrumb":
                    return BreadcrumbView.Breadcrumb;

                case "Drives":
                    return BreadcrumbView.Drives;

                case "SimpleText":
                    return BreadcrumbView.SimpleText;

                case "EnterPath":
                    return BreadcrumbView.EnterPath;
            }
            throw base.CreateUnknownConstantException(s, typeof(BreadcrumbView));
        }

        public object Read300_ThrobberRenderer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id96_ThrobberRenderer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read140_ThrobberRenderer(true, true);
            }
            base.UnknownNode(null, ":ThrobberRenderer");
            return null;
        }

        public object Read301_AutoRefreshMode()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id97_AutoRefreshMode) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read141_AutoRefreshMode(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":AutoRefreshMode");
            return null;
        }

        public object Read302_FtpFileSystemCreator()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id98_FtpFileSystemCreator) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read142_FtpFileSystemCreator(true, true);
            }
            base.UnknownNode(null, ":FtpFileSystemCreator");
            return null;
        }

        public object Read303_NullFileSystemCreator()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id99_NullFileSystemCreator) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read143_NullFileSystemCreator(true, true);
            }
            base.UnknownNode(null, ":NullFileSystemCreator");
            return null;
        }

        public object Read304_CustomVirtualFolder()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id100_CustomVirtualFolder) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read144_CustomVirtualFolder(true, true);
            }
            base.UnknownNode(null, ":CustomVirtualFolder");
            return null;
        }

        public object Read305_CanMoveResult()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id101_CanMoveResult) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read145_CanMoveResult(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CanMoveResult");
            return null;
        }

        public object Read306_IconOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id102_IconOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read146_IconOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":IconOptions");
            return null;
        }

        public object Read307_DelayedExtractMode()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id103_DelayedExtractMode) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read147_DelayedExtractMode(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":DelayedExtractMode");
            return null;
        }

        public object Read308_PathView()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id104_PathView) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read148_PathView(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PathView");
            return null;
        }

        public object Read309_PanelView()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id105_PanelView) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read21_PanelView(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PanelView");
            return null;
        }

        private ToolStripWrapperRenderer Read31_ToolStripWrapperRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id278_ToolStripWrapperRenderer) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id24_BreadcrumbToolStripRenderer) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read32_BreadcrumbToolStripRenderer(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            ToolStripWrapperRenderer o = new ToolStripWrapperRenderer();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        public object Read310_ContextMenuOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id106_ContextMenuOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read149_ContextMenuOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ContextMenuOptions");
            return null;
        }

        public object Read311_VirtualIcon()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id107_VirtualIcon) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read150_VirtualIcon(true, true);
            }
            base.UnknownNode(null, ":VirtualIcon");
            return null;
        }

        public object Read312_ArrayOfPropertyValue()
        {
            object obj2 = null;
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id108_ArrayOfPropertyValue) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                if (!base.ReadNull())
                {
                    if (obj2 == null)
                    {
                        obj2 = new PropertyValueList();
                    }
                    PropertyValueList list = (PropertyValueList) obj2;
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        return obj2;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int whileIterations = 0;
                    int readerCount = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            if ((base.Reader.LocalName == this.id109_PropertyValue) && (base.Reader.NamespaceURI == this.id2_Item))
                            {
                                if (list == null)
                                {
                                    base.Reader.Skip();
                                }
                                else
                                {
                                    list.Add(this.Read151_PropertyValue(true, true));
                                }
                            }
                            else
                            {
                                base.UnknownNode(null, ":PropertyValue");
                            }
                        }
                        else
                        {
                            base.UnknownNode(null, ":PropertyValue");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref whileIterations, ref readerCount);
                    }
                    base.ReadEndElement();
                    return obj2;
                }
                if (obj2 == null)
                {
                    obj2 = new PropertyValueList();
                }
                PropertyValueList list1 = (PropertyValueList) obj2;
                return obj2;
            }
            base.UnknownNode(null, ":ArrayOfPropertyValue");
            return obj2;
        }

        public object Read313_PropertyValue()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id109_PropertyValue) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read151_PropertyValue(true, true);
            }
            base.UnknownNode(null, ":PropertyValue");
            return null;
        }

        public object Read314_SimpleEncrypt()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id110_SimpleEncrypt) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read152_SimpleEncrypt(true, true);
            }
            base.UnknownNode(null, ":SimpleEncrypt");
            return null;
        }

        public object Read315_ProgressRenderMode()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id111_ProgressRenderMode) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read153_ProgressRenderMode(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ProgressRenderMode");
            return null;
        }

        public object Read316_ProgressState()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id112_ProgressState) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read154_ProgressState(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ProgressState");
            return null;
        }

        public object Read317_VistaProgressBarRenderer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id113_VistaProgressBarRenderer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read155_VistaProgressBarRenderer(true, true);
            }
            base.UnknownNode(null, ":VistaProgressBarRenderer");
            return null;
        }

        public object Read318_MarqueeStyle()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id114_MarqueeStyle) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read156_MarqueeStyle(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":MarqueeStyle");
            return null;
        }

        public object Read319_XPProgressBarRenderer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id115_XPProgressBarRenderer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read157_XPProgressBarRenderer(true, true);
            }
            base.UnknownNode(null, ":XPProgressBarRenderer");
            return null;
        }

        private BreadcrumbToolStripRenderer Read32_BreadcrumbToolStripRenderer(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id24_BreadcrumbToolStripRenderer) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            BreadcrumbToolStripRenderer o = new BreadcrumbToolStripRenderer();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        public object Read320_AskMode()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id116_AskMode) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read158_AskMode(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":AskMode");
            return null;
        }

        public object Read321_OperationResult()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id117_OperationResult) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read159_OperationResult(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":OperationResult");
            return null;
        }

        public object Read322_ItemPropId()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id118_ItemPropId) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read160_ItemPropId(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ItemPropId");
            return null;
        }

        public object Read323_FileTimeType()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id119_FileTimeType) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read161_FileTimeType(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":FileTimeType");
            return null;
        }

        public object Read324_ArchivePropId()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id120_ArchivePropId) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read162_ArchivePropId(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ArchivePropId");
            return null;
        }

        public object Read325_KnownSevenZipFormat()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id121_KnownSevenZipFormat) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read163_KnownSevenZipFormat(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":KnownSevenZipFormat");
            return null;
        }

        public object Read326_SevenZipFormatCapabilities()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id122_SevenZipFormatCapabilities) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read164_SevenZipFormatCapabilities(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":SevenZipFormatCapabilities");
            return null;
        }

        public object Read327_CompressionLevel()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id123_CompressionLevel) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read165_CompressionLevel(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CompressionLevel");
            return null;
        }

        public object Read328_CompressionMethod()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id124_CompressionMethod) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read166_CompressionMethod(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CompressionMethod");
            return null;
        }

        public object Read329_EncryptionMethod()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id125_EncryptionMethod) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read167_EncryptionMethod(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":EncryptionMethod");
            return null;
        }

        private InputDialogOption Read33_InputDialogOption(string s)
        {
            return (InputDialogOption) ((int) XmlSerializationReader.ToEnum(s, this.InputDialogOptionValues, "global::Nomad.Dialogs.InputDialogOption"));
        }

        public object Read330_SolidSizeUnit()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id126_SolidSizeUnit) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read168_SolidSizeUnit(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":SolidSizeUnit");
            return null;
        }

        public object Read331_ComplexFilterView()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id127_ComplexFilterView) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read169_ComplexFilterView(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ComplexFilterView");
            return null;
        }

        public object Read332_ViewFilters()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id128_ViewFilters) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read170_ViewFilters(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ViewFilters");
            return null;
        }

        public object Read333_PanelContentContainer()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id129_PanelContentContainer) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read172_PanelContentContainer(true, true);
            }
            base.UnknownNode(null, ":PanelContentContainer");
            return null;
        }

        public object Read334_ControllerType()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id130_ControllerType) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read173_ControllerType(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ControllerType");
            return null;
        }

        public object Read335_Controller()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id131_Controller) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read175_Controller(true, true);
            }
            base.UnknownNode(null, ":Controller");
            return null;
        }

        public object Read336_FormPlacement()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id132_FormPlacement) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read176_FormPlacement(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":FormPlacement");
            return null;
        }

        public object Read337_ArgumentKey()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id133_ArgumentKey) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read177_ArgumentKey(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ArgumentKey");
            return null;
        }

        public object Read338_CanMoveListViewItem()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id134_CanMoveListViewItem) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read178_CanMoveListViewItem(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CanMoveListViewItem");
            return null;
        }

        public object Read339_Trace()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id135_Trace) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read179_Trace(true, true);
            }
            base.UnknownNode(null, ":Trace");
            return null;
        }

        private ElevatedProcess Read34_ElevatedProcess(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id26_ElevatedProcess) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ElevatedProcess o = new ElevatedProcess();
            bool[] flagArray = new bool[1];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id294_KeepAlive)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.KeepAlive = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":KeepAlive");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":KeepAlive");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        public object Read340_SinglePanel()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id136_SinglePanel) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read180_SinglePanel(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":SinglePanel");
            return null;
        }

        public object Read341_GeneralTab()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id137_GeneralTab) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read181_GeneralTab(true, true);
            }
            base.UnknownNode(null, ":GeneralTab");
            return null;
        }

        public object Read342_TwoPanelTab()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id138_TwoPanelTab) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read182_TwoPanelTab(true, true);
            }
            base.UnknownNode(null, ":TwoPanelTab");
            return null;
        }

        public object Read343_ArchiveUpdateMode()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id139_ArchiveUpdateMode) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read183_ArchiveUpdateMode(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ArchiveUpdateMode");
            return null;
        }

        public object Read344_PackStage()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id140_PackStage) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read184_PackStage(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":PackStage");
            return null;
        }

        public object Read345_PackProgressSnapshot()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id141_PackProgressSnapshot) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read187_PackProgressSnapshot(true, true);
            }
            base.UnknownNode(null, ":PackProgressSnapshot");
            return null;
        }

        public object Read346_CustomBackgroundWorker()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id142_CustomBackgroundWorker) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read188_CustomBackgroundWorker(true, true);
            }
            base.UnknownNode(null, ":CustomBackgroundWorker");
            return null;
        }

        public object Read347_EventBackgroundWorker()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id143_EventBackgroundWorker) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read189_EventBackgroundWorker(true, true);
            }
            base.UnknownNode(null, ":EventBackgroundWorker");
            return null;
        }

        public object Read348_CopyDestinationItem()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id144_CopyDestinationItem) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read190_CopyDestinationItem(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CopyDestinationItem");
            return null;
        }

        public object Read349_MessageDialogResult()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id145_MessageDialogResult) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read191_MessageDialogResult(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":MessageDialogResult");
            return null;
        }

        private ArchiveFormatConverter Read35_ArchiveFormatConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id27_ArchiveFormatConverter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ArchiveFormatConverter o = new ArchiveFormatConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        public object Read350_DoubleClickAction()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id146_DoubleClickAction) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read192_DoubleClickAction(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":DoubleClickAction");
            return null;
        }

        public object Read351_QuickFindOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id147_QuickFindOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read171_QuickFindOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":QuickFindOptions");
            return null;
        }

        public object Read352_ListViewSort()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id148_ListViewSort) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read193_ListViewSort(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ListViewSort");
            return null;
        }

        public object Read353_CustomAsyncFolder()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id149_CustomAsyncFolder) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read194_CustomAsyncFolder(true, true);
            }
            base.UnknownNode(null, ":CustomAsyncFolder");
            return null;
        }

        public object Read354_SearchFolderOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id150_SearchFolderOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read195_SearchFolderOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":SearchFolderOptions");
            return null;
        }

        public object Read355_FindDuplicateOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id151_FindDuplicateOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read196_FindDuplicateOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":FindDuplicateOptions");
            return null;
        }

        public object Read356_Compare()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id152_Compare) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read197_Compare(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":Compare");
            return null;
        }

        public object Read357_ChangeItemAction()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id153_ChangeItemAction) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read198_ChangeItemAction(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":ChangeItemAction");
            return null;
        }

        public object Read358_AvailableItemActions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id154_AvailableItemActions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read199_AvailableItemActions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":AvailableItemActions");
            return null;
        }

        public object Read359_CompareFoldersOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id155_CompareFoldersOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read200_CompareFoldersOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CompareFoldersOptions");
            return null;
        }

        private ArchiveFormatCapabilities Read36_ArchiveFormatCapabilities(string s)
        {
            return (ArchiveFormatCapabilities) ((int) XmlSerializationReader.ToEnum(s, this.ArchiveFormatCapabilitiesValues, "global::Nomad.FileSystem.Archive.Common.ArchiveFormatCapabilities"));
        }

        public object Read360_OverwriteDialogResult()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id156_OverwriteDialogResult) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read201_OverwriteDialogResult(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":OverwriteDialogResult");
            return null;
        }

        public object Read361_CopyWorkerOptions()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id157_CopyWorkerOptions) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read202_CopyWorkerOptions(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CopyWorkerOptions");
            return null;
        }

        public object Read362_CopyMode()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id158_CopyMode) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read203_CopyMode(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":CopyMode");
            return null;
        }

        public object Read363_ProcessedSize()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id159_ProcessedSize) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read185_ProcessedSize(true);
            }
            base.UnknownNode(null, ":ProcessedSize");
            return null;
        }

        public object Read364_CopyProgressSnapshot()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id160_CopyProgressSnapshot) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read204_CopyProgressSnapshot(true, true);
            }
            base.UnknownNode(null, ":CopyProgressSnapshot");
            return null;
        }

        public object Read365_IconStyle()
        {
            base.Reader.MoveToContent();
            if (base.Reader.NodeType == XmlNodeType.Element)
            {
                if ((base.Reader.LocalName != this.id161_IconStyle) || (base.Reader.NamespaceURI != this.id2_Item))
                {
                    throw base.CreateUnknownNodeException();
                }
                return this.Read205_IconStyle(base.Reader.ReadElementString());
            }
            base.UnknownNode(null, ":IconStyle");
            return null;
        }

        private ArchiveFormatInfo Read37_ArchiveFormatInfo(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id29_ArchiveFormatInfo) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id30_PersistArchiveFormatInfo) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read38_PersistArchiveFormatInfo(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("ArchiveFormatInfo", "");
            }
            return null;
        }

        private PersistArchiveFormatInfo Read38_PersistArchiveFormatInfo(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id30_PersistArchiveFormatInfo) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("PersistArchiveFormatInfo", "");
            }
            return null;
        }

        private FindFormatSource Read39_FindFormatSource(string s)
        {
            return (FindFormatSource) ((int) XmlSerializationReader.ToEnum(s, this.FindFormatSourceValues, "global::Nomad.FileSystem.Archive.Common.FindFormatSource"));
        }

        private TypeConverter Read4_TypeConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id279_TypeConverter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id89_EncodingConveter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read132_EncodingConveter(isNullable, false);
                }
                if ((type.Name == this.id82_AudioChannelsTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read125_AudioChannelsTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id280_PropertyTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read123_PropertyTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id88_RatingTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read131_RatingTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id87_ISOSpeedTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read130_ISOSpeedTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id86_DPITypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read129_DPITypeConverter(isNullable, false);
                }
                if ((type.Name == this.id85_ImageSizeTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read128_ImageSizeTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id84_DurationTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read127_DurationTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id83_AudioSampleRateTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read126_AudioSampleRateTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id81_BitrateTypeConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read124_BitrateTypeConverter(isNullable, false);
                }
                if ((type.Name == this.id27_ArchiveFormatConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read35_ArchiveFormatConverter(isNullable, false);
                }
                if ((type.Name == this.id281_KeysConverter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read5_KeysConverter(isNullable, false);
                }
                if ((type.Name != this.id4_KeysConverter2) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read6_KeysConverter2(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            TypeConverter o = new TypeConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ArjHeader Read40_ArjHeader(bool checkType)
        {
            ArjHeader header;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id32_ArjHeader) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                header = (ArjHeader) Activator.CreateInstance(typeof(ArjHeader), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.FileSystem.Archive.Common.ArjHeader");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.FileSystem.Archive.Common.ArjHeader");
            }
            bool[] flagArray = new bool[0x11];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(header);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return header;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id295_Mark)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.Mark = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id296_HeadSize)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.HeadSize = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id297_FirstHeadSize)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.FirstHeadSize = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id298_ArjVer)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.ArjVer = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id299_ArjExtrVer)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.ArjExtrVer = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id300_HostOS)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.HostOS = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[5] = true;
                    }
                    else if ((!flagArray[6] && (base.Reader.LocalName == this.id301_Flags)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.Flags = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[6] = true;
                    }
                    else if ((!flagArray[7] && (base.Reader.LocalName == this.id302_Method)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.Method = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[7] = true;
                    }
                    else if ((!flagArray[8] && (base.Reader.LocalName == this.id303_FileType)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.FileType = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[8] = true;
                    }
                    else if ((!flagArray[9] && (base.Reader.LocalName == this.id304_Reserved)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.Reserved = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[9] = true;
                    }
                    else if ((!flagArray[10] && (base.Reader.LocalName == this.id305_ftime)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.ftime = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[10] = true;
                    }
                    else if ((!flagArray[11] && (base.Reader.LocalName == this.id306_PackSize)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.PackSize = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[11] = true;
                    }
                    else if ((!flagArray[12] && (base.Reader.LocalName == this.id307_UnpSize)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.UnpSize = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[12] = true;
                    }
                    else if ((!flagArray[13] && (base.Reader.LocalName == this.id308_CRC)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.CRC = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[13] = true;
                    }
                    else if ((!flagArray[14] && (base.Reader.LocalName == this.id309_FileSpec)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.FileSpec = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[14] = true;
                    }
                    else if ((!flagArray[15] && (base.Reader.LocalName == this.id310_AccessMode)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.AccessMode = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[15] = true;
                    }
                    else if ((!flagArray[0x10] && (base.Reader.LocalName == this.id311_HostData)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        header.HostData = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[0x10] = true;
                    }
                    else
                    {
                        base.UnknownNode(header, ":Mark, :HeadSize, :FirstHeadSize, :ArjVer, :ArjExtrVer, :HostOS, :Flags, :Method, :FileType, :Reserved, :ftime, :PackSize, :UnpSize, :CRC, :FileSpec, :AccessMode, :HostData");
                    }
                }
                else
                {
                    base.UnknownNode(header, ":Mark, :HeadSize, :FirstHeadSize, :ArjVer, :ArjExtrVer, :HostOS, :Flags, :Method, :FileType, :Reserved, :ftime, :PackSize, :UnpSize, :CRC, :FileSpec, :AccessMode, :HostData");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return header;
        }

        private EventArgs Read41_EventArgs(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id273_EventArgs) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id274_CancelEventArgs) && (type.Namespace == this.id2_Item))
                {
                    return this.Read42_CancelEventArgs(isNullable, false);
                }
                if ((type.Name != this.id33_ProcessItemEventArgs) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read43_ProcessItemEventArgs(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            EventArgs o = new EventArgs();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private CancelEventArgs Read42_CancelEventArgs(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id274_CancelEventArgs) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id33_ProcessItemEventArgs) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read43_ProcessItemEventArgs(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            CancelEventArgs o = new CancelEventArgs();
            bool[] flagArray = new bool[1];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id312_Cancel)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Cancel = XmlConvert.ToBoolean(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Cancel");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Cancel");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ProcessItemEventArgs Read43_ProcessItemEventArgs(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id33_ProcessItemEventArgs) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("ProcessItemEventArgs", "");
            }
            return null;
        }

        private ProcessorState Read44_ProcessorState(string s)
        {
            switch (s)
            {
                case "Initializing":
                    return ProcessorState.Initializing;

                case "InProcess":
                    return ProcessorState.InProcess;

                case "Finished":
                    return ProcessorState.Finished;
            }
            throw base.CreateUnknownConstantException(s, typeof(ProcessorState));
        }

        private SequenseProcessorType Read45_SequenseProcessorType(string s)
        {
            switch (s)
            {
                case "Extract":
                    return SequenseProcessorType.Extract;

                case "Delete":
                    return SequenseProcessorType.Delete;
            }
            throw base.CreateUnknownConstantException(s, typeof(SequenseProcessorType));
        }

        private PK_OM Read46_PK_OM(string s)
        {
            switch (s)
            {
                case "PK_OM_LIST":
                    return PK_OM.PK_OM_LIST;

                case "PK_OM_EXTRACT":
                    return PK_OM.PK_OM_EXTRACT;
            }
            throw base.CreateUnknownConstantException(s, typeof(PK_OM));
        }

        private PK_OPERATION Read47_PK_OPERATION(string s)
        {
            switch (s)
            {
                case "PK_SKIP":
                    return PK_OPERATION.PK_SKIP;

                case "PK_TEST":
                    return PK_OPERATION.PK_TEST;

                case "PK_EXTRACT":
                    return PK_OPERATION.PK_EXTRACT;
            }
            throw base.CreateUnknownConstantException(s, typeof(PK_OPERATION));
        }

        private PK_CAPS Read48_PK_CAPS(string s)
        {
            return (PK_CAPS) ((int) XmlSerializationReader.ToEnum(s, this.PK_CAPSValues, "global::Nomad.FileSystem.Archive.Wcx.PK_CAPS"));
        }

        private PK_VOL Read49_PK_VOL(string s)
        {
            switch (s)
            {
                case "PK_VOL_ASK":
                    return PK_VOL.PK_VOL_ASK;

                case "PK_VOL_NOTIFY":
                    return PK_VOL.PK_VOL_NOTIFY;
            }
            throw base.CreateUnknownConstantException(s, typeof(PK_VOL));
        }

        private KeysConverter Read5_KeysConverter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id281_KeysConverter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id4_KeysConverter2) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read6_KeysConverter2(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            KeysConverter o = new KeysConverter();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PK_PACK Read50_PK_PACK(string s)
        {
            return (PK_PACK) ((int) XmlSerializationReader.ToEnum(s, this.PK_PACKValues, "global::Nomad.FileSystem.Archive.Wcx.PK_PACK"));
        }

        private PackDefaultParamStruct Read51_PackDefaultParamStruct(bool checkType)
        {
            PackDefaultParamStruct struct2;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id41_PackDefaultParamStruct) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                struct2 = (PackDefaultParamStruct) Activator.CreateInstance(typeof(PackDefaultParamStruct), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.FileSystem.Archive.Wcx.PackDefaultParamStruct");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.FileSystem.Archive.Wcx.PackDefaultParamStruct");
            }
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(struct2);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return struct2;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id313_size)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.size = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id314_PluginInterfaceVersionLow)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.PluginInterfaceVersionLow = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id315_PluginInterfaceVersionHi)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.PluginInterfaceVersionHi = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id316_DefaultIniName)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.DefaultIniName = base.Reader.ReadElementString();
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(struct2, ":size, :PluginInterfaceVersionLow, :PluginInterfaceVersionHi, :DefaultIniName");
                    }
                }
                else
                {
                    base.UnknownNode(struct2, ":size, :PluginInterfaceVersionLow, :PluginInterfaceVersionHi, :DefaultIniName");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return struct2;
        }

        private WcxErrors Read52_WcxErrors(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id42_WcxErrors) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            WcxErrors o = new WcxErrors();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private DefaultIcon Read53_DefaultIcon(string s)
        {
            switch (s)
            {
                case "UnknownFile":
                    return DefaultIcon.UnknownFile;

                case "DefaultDocument":
                    return DefaultIcon.DefaultDocument;

                case "DefaultApplication":
                    return DefaultIcon.DefaultApplication;

                case "Desktop":
                    return DefaultIcon.Desktop;

                case "Drive":
                    return DefaultIcon.Drive;

                case "Favorites":
                    return DefaultIcon.Favorites;

                case "Folder":
                    return DefaultIcon.Folder;

                case "MyComputer":
                    return DefaultIcon.MyComputer;

                case "MyDocuments":
                    return DefaultIcon.MyDocuments;

                case "MyPictures":
                    return DefaultIcon.MyPictures;

                case "MyMusic":
                    return DefaultIcon.MyMusic;

                case "MyVideos":
                    return DefaultIcon.MyVideos;

                case "SearchFolder":
                    return DefaultIcon.SearchFolder;

                case "NetworkNeighborhood":
                    return DefaultIcon.NetworkNeighborhood;

                case "EntireNetwork":
                    return DefaultIcon.EntireNetwork;

                case "NetworkWorkgroup":
                    return DefaultIcon.NetworkWorkgroup;

                case "NetworkProvider":
                    return DefaultIcon.NetworkProvider;

                case "NetworkServer":
                    return DefaultIcon.NetworkServer;

                case "NetworkFolder":
                    return DefaultIcon.NetworkFolder;

                case "OverlayLink":
                    return DefaultIcon.OverlayLink;

                case "OverlayShare":
                    return DefaultIcon.OverlayShare;

                case "OverlayUnreadable":
                    return DefaultIcon.OverlayUnreadable;
            }
            throw base.CreateUnknownConstantException(s, typeof(DefaultIcon));
        }

        private ShellImageProvider Read54_ShellImageProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id44_ShellImageProvider) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ShellImageProvider o = new ShellImageProvider();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private FileSystemItem.ItemCapability Read55_ItemCapability(string s)
        {
            return (FileSystemItem.ItemCapability) ((int) XmlSerializationReader.ToEnum(s, this.ItemCapabilityValues, "global::Nomad.FileSystem.LocalFile.FileSystemItem.ItemCapability"));
        }

        private LocalFileSystemCreator Read56_LocalFileSystemCreator(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id46_LocalFileSystemCreator) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            LocalFileSystemCreator o = new LocalFileSystemCreator();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private NetworkFileSystemCreator Read57_NetworkFileSystemCreator(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id47_NetworkFileSystemCreator) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            NetworkFileSystemCreator o = new NetworkFileSystemCreator();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ShellFileSystemCreator Read58_ShellFileSystemCreator(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id48_ShellFileSystemCreator) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            ShellFileSystemCreator o = new ShellFileSystemCreator();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ContentFlag Read59_ContentFlag(string s)
        {
            return (ContentFlag) ((int) XmlSerializationReader.ToEnum(s, this.ContentFlagValues, "global::Nomad.FileSystem.Property.Providers.Wdx.ContentFlag"));
        }

        private KeysConverter2 Read6_KeysConverter2(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id4_KeysConverter2) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            KeysConverter2 o = new KeysConverter2();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private ContentDefaultParamStruct Read60_ContentDefaultParamStruct(bool checkType)
        {
            ContentDefaultParamStruct struct2;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id50_ContentDefaultParamStruct) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                struct2 = (ContentDefaultParamStruct) Activator.CreateInstance(typeof(ContentDefaultParamStruct), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.FileSystem.Property.Providers.Wdx.ContentDefaultParamStruct");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.FileSystem.Property.Providers.Wdx.ContentDefaultParamStruct");
            }
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(struct2);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return struct2;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id313_size)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.size = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id314_PluginInterfaceVersionLow)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.PluginInterfaceVersionLow = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id315_PluginInterfaceVersionHi)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.PluginInterfaceVersionHi = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id316_DefaultIniName)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        struct2.DefaultIniName = base.Reader.ReadElementString();
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(struct2, ":size, :PluginInterfaceVersionLow, :PluginInterfaceVersionHi, :DefaultIniName");
                    }
                }
                else
                {
                    base.UnknownNode(struct2, ":size, :PluginInterfaceVersionLow, :PluginInterfaceVersionHi, :DefaultIniName");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return struct2;
        }

        private tdateformat Read61_tdateformat(bool checkType)
        {
            tdateformat tdateformat;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id51_tdateformat) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                tdateformat = (tdateformat) Activator.CreateInstance(typeof(tdateformat), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.FileSystem.Property.Providers.Wdx.tdateformat");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.FileSystem.Property.Providers.Wdx.tdateformat");
            }
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(tdateformat);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return tdateformat;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id317_wYear)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        tdateformat.wYear = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id318_wMonth)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        tdateformat.wMonth = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id319_wDay)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        tdateformat.wDay = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(tdateformat, ":wYear, :wMonth, :wDay");
                    }
                }
                else
                {
                    base.UnknownNode(tdateformat, ":wYear, :wMonth, :wDay");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return tdateformat;
        }

        private ttimeformat Read62_ttimeformat(bool checkType)
        {
            ttimeformat ttimeformat;
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            if ((checkType && (type != null)) && ((type.Name != this.id52_ttimeformat) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            try
            {
                ttimeformat = (ttimeformat) Activator.CreateInstance(typeof(ttimeformat), BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null);
            }
            catch (MissingMethodException)
            {
                throw base.CreateInaccessibleConstructorException("global::Nomad.FileSystem.Property.Providers.Wdx.ttimeformat");
            }
            catch (SecurityException)
            {
                throw base.CreateCtorHasSecurityException("global::Nomad.FileSystem.Property.Providers.Wdx.ttimeformat");
            }
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(ttimeformat);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return ttimeformat;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id320_wHour)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        ttimeformat.wHour = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id321_wMinute)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        ttimeformat.wMinute = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id322_wSecond)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        ttimeformat.wSecond = XmlConvert.ToUInt16(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(ttimeformat, ":wHour, :wMinute, :wSecond");
                    }
                }
                else
                {
                    base.UnknownNode(ttimeformat, ":wHour, :wMinute, :wSecond");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return ttimeformat;
        }

        private WdxFieldInfo Read63_WdxFieldInfo(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id53_WdxFieldInfo) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            WdxFieldInfo o = new WdxFieldInfo();
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id323_FieldName)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FieldName = base.Reader.ReadElementString();
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id324_FieldType)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FieldType = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((base.Reader.LocalName == this.id325_Units) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (!base.ReadNull())
                        {
                            string[] a = null;
                            int index = 0;
                            if (base.Reader.IsEmptyElement)
                            {
                                base.Reader.Skip();
                            }
                            else
                            {
                                base.Reader.ReadStartElement();
                                base.Reader.MoveToContent();
                                int num4 = 0;
                                int num5 = base.ReaderCount;
                                while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                                {
                                    if (base.Reader.NodeType == XmlNodeType.Element)
                                    {
                                        if ((base.Reader.LocalName == this.id211_string) && (base.Reader.NamespaceURI == this.id2_Item))
                                        {
                                            if (base.ReadNull())
                                            {
                                                a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                a[index++] = null;
                                            }
                                            else
                                            {
                                                a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                a[index++] = base.Reader.ReadElementString();
                                            }
                                        }
                                        else
                                        {
                                            base.UnknownNode(null, ":string");
                                        }
                                    }
                                    else
                                    {
                                        base.UnknownNode(null, ":string");
                                    }
                                    base.Reader.MoveToContent();
                                    base.CheckReaderCount(ref num4, ref num5);
                                }
                                base.ReadEndElement();
                            }
                            o.Units = (string[]) base.ShrinkArray(a, index, typeof(string), false);
                        }
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id301_Flags)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Flags = this.Read59_ContentFlag(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":FieldName, :FieldType, :Units, :Flags");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":FieldName, :FieldType, :Units, :Flags");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private AggregatedFilterCondition Read64_AggregatedFilterCondition(string s)
        {
            switch (s)
            {
                case "All":
                    return AggregatedFilterCondition.All;

                case "Any":
                    return AggregatedFilterCondition.Any;

                case "None":
                    return AggregatedFilterCondition.None;
            }
            throw base.CreateUnknownConstantException(s, typeof(AggregatedFilterCondition));
        }

        private BasicFilter Read65_BasicFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id266_BasicFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id69_VirtualItemNameListFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read107_VirtualItemNameListFilter(isNullable, false);
                }
                if ((type.Name == this.id177_NameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read102_NameFilter(isNullable, false);
                }
                if ((type.Name == this.id60_VirtualItemFullNameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read112_VirtualItemFullNameFilter(isNullable, false);
                }
                if ((type.Name == this.id59_VirtualItemNameFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read103_VirtualItemNameFilter(isNullable, false);
                }
                if ((type.Name == this.id182_AttributeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read93_AttributeFilter(isNullable, false);
                }
                if ((type.Name == this.id61_VirtualItemAttributeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read94_VirtualItemAttributeFilter(isNullable, false);
                }
                if ((type.Name == this.id267_CustomContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read85_CustomContentFilter(isNullable, false);
                }
                if ((type.Name == this.id181_HexContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read104_HexContentFilter(isNullable, false);
                }
                if ((type.Name == this.id67_VirtualItemHexContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read105_VirtualItemHexContentFilter(isNullable, false);
                }
                if ((type.Name == this.id179_ContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read86_ContentFilter(isNullable, false);
                }
                if ((type.Name == this.id66_VirtualItemContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read87_VirtualItemContentFilter(isNullable, false);
                }
                if ((type.Name == this.id268_ValueFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read66_ValueFilter(isNullable, false);
                }
                if ((type.Name == this.id185_TimeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read89_TimeFilter(isNullable, false);
                }
                if ((type.Name == this.id65_VirtualItemTimeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read91_VirtualItemTimeFilter(isNullable, false);
                }
                if ((type.Name == this.id269_SimpleFilterOfByte) && (type.Namespace == this.id2_Item))
                {
                    return this.Read82_SimpleFilterOfByte(isNullable, false);
                }
                if ((type.Name == this.id222_IntegralFilterOfByte) && (type.Namespace == this.id2_Item))
                {
                    return this.Read83_IntegralFilterOfByte(isNullable, false);
                }
                if ((type.Name == this.id270_SimpleFilterOfInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read80_SimpleFilterOfInt32(isNullable, false);
                }
                if ((type.Name == this.id223_IntegralFilterOfInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read81_IntegralFilterOfInt32(isNullable, false);
                }
                if ((type.Name == this.id271_SimpleFilterOfUInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read78_SimpleFilterOfUInt32(isNullable, false);
                }
                if ((type.Name == this.id224_IntegralFilterOfUInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read79_IntegralFilterOfUInt32(isNullable, false);
                }
                if ((type.Name == this.id272_SimpleFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read76_SimpleFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id225_IntegralFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read77_IntegralFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id183_SizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read97_SizeFilter(isNullable, false);
                }
                if ((type.Name == this.id62_VirtualItemSizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read98_VirtualItemSizeFilter(isNullable, false);
                }
                if ((type.Name == this.id226_SimpleFilterOfVersion) && (type.Namespace == this.id2_Item))
                {
                    return this.Read75_SimpleFilterOfVersion(isNullable, false);
                }
                if ((type.Name == this.id216_StringFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read72_StringFilter(isNullable, false);
                }
                if ((type.Name == this.id184_DateFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read69_DateFilter(isNullable, false);
                }
                if ((type.Name == this.id64_VirtualItemDateFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read99_VirtualItemDateFilter(isNullable, false);
                }
                if ((type.Name == this.id70_VirtualPropertyFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read84_VirtualPropertyFilter(isNullable, false);
                }
                if ((type.Name != this.id55_AggregatedVirtualItemFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read108_AggregatedVirtualItemFilter(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("BasicFilter", "");
            }
            return null;
        }

        private ValueFilter Read66_ValueFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id268_ValueFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id185_TimeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read89_TimeFilter(isNullable, false);
                }
                if ((type.Name == this.id65_VirtualItemTimeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read91_VirtualItemTimeFilter(isNullable, false);
                }
                if ((type.Name == this.id269_SimpleFilterOfByte) && (type.Namespace == this.id2_Item))
                {
                    return this.Read82_SimpleFilterOfByte(isNullable, false);
                }
                if ((type.Name == this.id222_IntegralFilterOfByte) && (type.Namespace == this.id2_Item))
                {
                    return this.Read83_IntegralFilterOfByte(isNullable, false);
                }
                if ((type.Name == this.id270_SimpleFilterOfInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read80_SimpleFilterOfInt32(isNullable, false);
                }
                if ((type.Name == this.id223_IntegralFilterOfInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read81_IntegralFilterOfInt32(isNullable, false);
                }
                if ((type.Name == this.id271_SimpleFilterOfUInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read78_SimpleFilterOfUInt32(isNullable, false);
                }
                if ((type.Name == this.id224_IntegralFilterOfUInt32) && (type.Namespace == this.id2_Item))
                {
                    return this.Read79_IntegralFilterOfUInt32(isNullable, false);
                }
                if ((type.Name == this.id272_SimpleFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read76_SimpleFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id225_IntegralFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read77_IntegralFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id183_SizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read97_SizeFilter(isNullable, false);
                }
                if ((type.Name == this.id62_VirtualItemSizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read98_VirtualItemSizeFilter(isNullable, false);
                }
                if ((type.Name == this.id226_SimpleFilterOfVersion) && (type.Namespace == this.id2_Item))
                {
                    return this.Read75_SimpleFilterOfVersion(isNullable, false);
                }
                if ((type.Name == this.id216_StringFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read72_StringFilter(isNullable, false);
                }
                if ((type.Name == this.id184_DateFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read69_DateFilter(isNullable, false);
                }
                if ((type.Name != this.id64_VirtualItemDateFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read99_VirtualItemDateFilter(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("ValueFilter", "");
            }
            return null;
        }

        private DateComparision Read67_DateComparision(string s)
        {
            switch (s)
            {
                case "Ignore":
                    return DateComparision.Ignore;

                case "On":
                    return DateComparision.On;

                case "Before":
                    return DateComparision.Before;

                case "After":
                    return DateComparision.After;

                case "Between":
                    return DateComparision.Between;

                case "NotBetween":
                    return DateComparision.NotBetween;

                case "NotOlderThan":
                    return DateComparision.NotOlderThan;
            }
            throw base.CreateUnknownConstantException(s, typeof(DateComparision));
        }

        private DateUnit Read68_DateUnit(string s)
        {
            switch (s)
            {
                case "Day":
                    return DateUnit.Day;

                case "Week":
                    return DateUnit.Week;

                case "Month":
                    return DateUnit.Month;

                case "Year":
                    return DateUnit.Year;
            }
            throw base.CreateUnknownConstantException(s, typeof(DateUnit));
        }

        private DateFilter Read69_DateFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id184_DateFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id64_VirtualItemDateFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read99_VirtualItemDateFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            DateFilter o = new DateFilter();
            bool[] flagArray = new bool[5];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id194_DateComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.DateComparision = this.Read67_DateComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id195_NotOlderThan)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.NotOlderThan = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        }
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id196_DateMeasure)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.DateMeasure = this.Read68_DateUnit(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id197_FromDate)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromDate = XmlSerializationReader.ToDate(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id198_ToDate)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToDate = XmlSerializationReader.ToDate(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":DateComparision, :NotOlderThan, :DateMeasure, :FromDate, :ToDate");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":DateComparision, :NotOlderThan, :DateMeasure, :FromDate, :ToDate");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PropertyTagType Read7_PropertyTagType(string s)
        {
            switch (s)
            {
                case "PixelFormat4bppIndexed":
                    return PropertyTagType.PixelFormat4bppIndexed;

                case "Byte":
                    return PropertyTagType.Byte;

                case "ASCII":
                    return PropertyTagType.ASCII;

                case "Short":
                    return PropertyTagType.Short;

                case "Long":
                    return PropertyTagType.Long;

                case "Rational":
                    return PropertyTagType.Rational;

                case "Undefined":
                    return PropertyTagType.Undefined;

                case "SLONG":
                    return PropertyTagType.SLONG;

                case "SRational":
                    return PropertyTagType.SRational;
            }
            throw base.CreateUnknownConstantException(s, typeof(PropertyTagType));
        }

        private ContentFilterOptions Read70_ContentFilterOptions(string s)
        {
            return (ContentFilterOptions) ((int) XmlSerializationReader.ToEnum(s, this.ContentFilterOptionsValues, "global::Nomad.Commons.ContentFilterOptions"));
        }

        private ContentComparision Read71_ContentComparision(string s)
        {
            switch (s)
            {
                case "Ignore":
                    return ContentComparision.Ignore;

                case "Contains":
                    return ContentComparision.Contains;

                case "NotContains":
                    return ContentComparision.NotContains;
            }
            throw base.CreateUnknownConstantException(s, typeof(ContentComparision));
        }

        private StringFilter Read72_StringFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id216_StringFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            StringFilter o = new StringFilter();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id212_Options)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Options = this.Read70_ContentFilterOptions(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id207_Comparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Comparision = this.Read71_ContentComparision(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id213_Text)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Text = base.Reader.ReadElementString();
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Options, :Comparision, :Text");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Options, :Comparision, :Text");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SimpleComparision Read73_SimpleComparision(string s)
        {
            switch (s)
            {
                case "Ignore":
                    return SimpleComparision.Ignore;

                case "Equals":
                    return SimpleComparision.Equals;

                case "Smaller":
                    return SimpleComparision.Smaller;

                case "Larger":
                    return SimpleComparision.Larger;

                case "Between":
                    return SimpleComparision.Between;

                case "NotBetween":
                    return SimpleComparision.NotBetween;

                case "NotEquals":
                    return SimpleComparision.NotEquals;
            }
            throw base.CreateUnknownConstantException(s, typeof(SimpleComparision));
        }

        private Version Read74_Version(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id227_Version) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            Version o = new Version();
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SimpleFilter<Version> Read75_SimpleFilterOfVersion(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id226_SimpleFilterOfVersion) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            SimpleFilter<Version> o = new SimpleFilter<Version>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = this.Read74_Version(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = this.Read74_Version(false, true);
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SimpleFilter<long> Read76_SimpleFilterOfInt64(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id272_SimpleFilterOfInt64) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id225_IntegralFilterOfInt64) && (type.Namespace == this.id2_Item))
                {
                    return this.Read77_IntegralFilterOfInt64(isNullable, false);
                }
                if ((type.Name == this.id183_SizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read97_SizeFilter(isNullable, false);
                }
                if ((type.Name != this.id62_VirtualItemSizeFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read98_VirtualItemSizeFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            SimpleFilter<long> o = new SimpleFilter<long>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private IntegralFilter<long> Read77_IntegralFilterOfInt64(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id225_IntegralFilterOfInt64) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id183_SizeFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read97_SizeFilter(isNullable, false);
                }
                if ((type.Name != this.id62_VirtualItemSizeFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read98_VirtualItemSizeFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            IntegralFilter<long> o = new IntegralFilter<long>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SimpleFilter<uint> Read78_SimpleFilterOfUInt32(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id271_SimpleFilterOfUInt32) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id224_IntegralFilterOfUInt32) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read79_IntegralFilterOfUInt32(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            SimpleFilter<uint> o = new SimpleFilter<uint>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private IntegralFilter<uint> Read79_IntegralFilterOfUInt32(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id224_IntegralFilterOfUInt32) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            IntegralFilter<uint> o = new IntegralFilter<uint>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToUInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private PropertyTag Read8_PropertyTag(string s)
        {
            switch (s)
            {
                case "GpsVer":
                    return PropertyTag.GpsVer;

                case "GpsLatitudeRef":
                    return PropertyTag.GpsLatitudeRef;

                case "GpsLatitude":
                    return PropertyTag.GpsLatitude;

                case "GpsLongitudeRef":
                    return PropertyTag.GpsLongitudeRef;

                case "GpsLongitude":
                    return PropertyTag.GpsLongitude;

                case "GpsAltitudeRef":
                    return PropertyTag.GpsAltitudeRef;

                case "GpsAltitude":
                    return PropertyTag.GpsAltitude;

                case "GpsGpsTime":
                    return PropertyTag.GpsGpsTime;

                case "GpsGpsSatellites":
                    return PropertyTag.GpsGpsSatellites;

                case "GpsGpsStatus":
                    return PropertyTag.GpsGpsStatus;

                case "GpsGpsMeasureMode":
                    return PropertyTag.GpsGpsMeasureMode;

                case "GpsGpsDop":
                    return PropertyTag.GpsGpsDop;

                case "GpsSpeedRef":
                    return PropertyTag.GpsSpeedRef;

                case "GpsSpeed":
                    return PropertyTag.GpsSpeed;

                case "GpsTrackRef":
                    return PropertyTag.GpsTrackRef;

                case "GpsTrack":
                    return PropertyTag.GpsTrack;

                case "GpsImgDirRef":
                    return PropertyTag.GpsImgDirRef;

                case "GpsImgDir":
                    return PropertyTag.GpsImgDir;

                case "GpsMapDatum":
                    return PropertyTag.GpsMapDatum;

                case "GpsDestLatRef":
                    return PropertyTag.GpsDestLatRef;

                case "GpsDestLat":
                    return PropertyTag.GpsDestLat;

                case "GpsDestLongRef":
                    return PropertyTag.GpsDestLongRef;

                case "GpsDestLong":
                    return PropertyTag.GpsDestLong;

                case "GpsDestBearRef":
                    return PropertyTag.GpsDestBearRef;

                case "GpsDestBear":
                    return PropertyTag.GpsDestBear;

                case "GpsDestDistRef":
                    return PropertyTag.GpsDestDistRef;

                case "GpsDestDist":
                    return PropertyTag.GpsDestDist;

                case "NewSubfileType":
                    return PropertyTag.NewSubfileType;

                case "SubfileType":
                    return PropertyTag.SubfileType;

                case "ImageWidth":
                    return PropertyTag.ImageWidth;

                case "ImageHeight":
                    return PropertyTag.ImageHeight;

                case "BitsPerSample":
                    return PropertyTag.BitsPerSample;

                case "Compression":
                    return PropertyTag.Compression;

                case "PhotometricInterp":
                    return PropertyTag.PhotometricInterp;

                case "ThreshHolding":
                    return PropertyTag.ThreshHolding;

                case "CellWidth":
                    return PropertyTag.CellWidth;

                case "CellHeight":
                    return PropertyTag.CellHeight;

                case "FillOrder":
                    return PropertyTag.FillOrder;

                case "DocumentName":
                    return PropertyTag.DocumentName;

                case "ImageDescription":
                    return PropertyTag.ImageDescription;

                case "EquipMake":
                    return PropertyTag.EquipMake;

                case "EquipModel":
                    return PropertyTag.EquipModel;

                case "StripOffsets":
                    return PropertyTag.StripOffsets;

                case "Orientation":
                    return PropertyTag.Orientation;

                case "SamplesPerPixel":
                    return PropertyTag.SamplesPerPixel;

                case "RowsPerStrip":
                    return PropertyTag.RowsPerStrip;

                case "StripBytesCount":
                    return PropertyTag.StripBytesCount;

                case "MinSampleValue":
                    return PropertyTag.MinSampleValue;

                case "MaxSampleValue":
                    return PropertyTag.MaxSampleValue;

                case "XResolution":
                    return PropertyTag.XResolution;

                case "YResolution":
                    return PropertyTag.YResolution;

                case "PlanarConfig":
                    return PropertyTag.PlanarConfig;

                case "PageName":
                    return PropertyTag.PageName;

                case "XPosition":
                    return PropertyTag.XPosition;

                case "YPosition":
                    return PropertyTag.YPosition;

                case "FreeOffset":
                    return PropertyTag.FreeOffset;

                case "FreeByteCounts":
                    return PropertyTag.FreeByteCounts;

                case "GrayResponseUnit":
                    return PropertyTag.GrayResponseUnit;

                case "GrayResponseCurve":
                    return PropertyTag.GrayResponseCurve;

                case "T4Option":
                    return PropertyTag.T4Option;

                case "T6Option":
                    return PropertyTag.T6Option;

                case "ResolutionUnit":
                    return PropertyTag.ResolutionUnit;

                case "PageNumber":
                    return PropertyTag.PageNumber;

                case "TransferFunction":
                    return PropertyTag.TransferFunction;

                case "SoftwareUsed":
                    return PropertyTag.SoftwareUsed;

                case "DateTime":
                    return PropertyTag.DateTime;

                case "Artist":
                    return PropertyTag.Artist;

                case "HostComputer":
                    return PropertyTag.HostComputer;

                case "Predictor":
                    return PropertyTag.Predictor;

                case "WhitePoint":
                    return PropertyTag.WhitePoint;

                case "PrimaryChromaticities":
                    return PropertyTag.PrimaryChromaticities;

                case "ColorMap":
                    return PropertyTag.ColorMap;

                case "HalftoneHints":
                    return PropertyTag.HalftoneHints;

                case "TileWidth":
                    return PropertyTag.TileWidth;

                case "TileLength":
                    return PropertyTag.TileLength;

                case "TileOffset":
                    return PropertyTag.TileOffset;

                case "TileByteCounts":
                    return PropertyTag.TileByteCounts;

                case "InkSet":
                    return PropertyTag.InkSet;

                case "InkNames":
                    return PropertyTag.InkNames;

                case "NumberOfInks":
                    return PropertyTag.NumberOfInks;

                case "DotRange":
                    return PropertyTag.DotRange;

                case "TargetPrinter":
                    return PropertyTag.TargetPrinter;

                case "ExtraSamples":
                    return PropertyTag.ExtraSamples;

                case "SampleFormat":
                    return PropertyTag.SampleFormat;

                case "SMinSampleValue":
                    return PropertyTag.SMinSampleValue;

                case "SMaxSampleValue":
                    return PropertyTag.SMaxSampleValue;

                case "TransferRange":
                    return PropertyTag.TransferRange;

                case "JPEGProc":
                    return PropertyTag.JPEGProc;

                case "JPEGInterFormat":
                    return PropertyTag.JPEGInterFormat;

                case "JPEGInterLength":
                    return PropertyTag.JPEGInterLength;

                case "JPEGRestartInterval":
                    return PropertyTag.JPEGRestartInterval;

                case "JPEGLosslessPredictors":
                    return PropertyTag.JPEGLosslessPredictors;

                case "JPEGPointTransforms":
                    return PropertyTag.JPEGPointTransforms;

                case "JPEGQTables":
                    return PropertyTag.JPEGQTables;

                case "JPEGDCTables":
                    return PropertyTag.JPEGDCTables;

                case "JPEGACTables":
                    return PropertyTag.JPEGACTables;

                case "YCbCrCoefficients":
                    return PropertyTag.YCbCrCoefficients;

                case "YCbCrSubsampling":
                    return PropertyTag.YCbCrSubsampling;

                case "YCbCrPositioning":
                    return PropertyTag.YCbCrPositioning;

                case "REFBlackWhite":
                    return PropertyTag.REFBlackWhite;

                case "Gamma":
                    return PropertyTag.Gamma;

                case "ICCProfileDescriptor":
                    return PropertyTag.ICCProfileDescriptor;

                case "SRGBRenderingIntent":
                    return PropertyTag.SRGBRenderingIntent;

                case "ImageTitle":
                    return PropertyTag.ImageTitle;

                case "ResolutionXUnit":
                    return PropertyTag.ResolutionXUnit;

                case "ResolutionYUnit":
                    return PropertyTag.ResolutionYUnit;

                case "ResolutionXLengthUnit":
                    return PropertyTag.ResolutionXLengthUnit;

                case "ResolutionYLengthUnit":
                    return PropertyTag.ResolutionYLengthUnit;

                case "PrintFlags":
                    return PropertyTag.PrintFlags;

                case "PrintFlagsVersion":
                    return PropertyTag.PrintFlagsVersion;

                case "PrintFlagsCrop":
                    return PropertyTag.PrintFlagsCrop;

                case "PrintFlagsBleedWidth":
                    return PropertyTag.PrintFlagsBleedWidth;

                case "PrintFlagsBleedWidthScale":
                    return PropertyTag.PrintFlagsBleedWidthScale;

                case "HalftoneLPI":
                    return PropertyTag.HalftoneLPI;

                case "HalftoneLPIUnit":
                    return PropertyTag.HalftoneLPIUnit;

                case "HalftoneDegree":
                    return PropertyTag.HalftoneDegree;

                case "HalftoneShape":
                    return PropertyTag.HalftoneShape;

                case "HalftoneMisc":
                    return PropertyTag.HalftoneMisc;

                case "HalftoneScreen":
                    return PropertyTag.HalftoneScreen;

                case "JPEGQuality":
                    return PropertyTag.JPEGQuality;

                case "GridSize":
                    return PropertyTag.GridSize;

                case "ThumbnailFormat":
                    return PropertyTag.ThumbnailFormat;

                case "ThumbnailWidth":
                    return PropertyTag.ThumbnailWidth;

                case "ThumbnailHeight":
                    return PropertyTag.ThumbnailHeight;

                case "ThumbnailColorDepth":
                    return PropertyTag.ThumbnailColorDepth;

                case "ThumbnailPlanes":
                    return PropertyTag.ThumbnailPlanes;

                case "ThumbnailRawBytes":
                    return PropertyTag.ThumbnailRawBytes;

                case "ThumbnailSize":
                    return PropertyTag.ThumbnailSize;

                case "ThumbnailCompressedSize":
                    return PropertyTag.ThumbnailCompressedSize;

                case "ColorTransferFunction":
                    return PropertyTag.ColorTransferFunction;

                case "ThumbnailData":
                    return PropertyTag.ThumbnailData;

                case "ThumbnailImageWidth":
                    return PropertyTag.ThumbnailImageWidth;

                case "ThumbnailImageHeight":
                    return PropertyTag.ThumbnailImageHeight;

                case "ThumbnailBitsPerSample":
                    return PropertyTag.ThumbnailBitsPerSample;

                case "ThumbnailCompression":
                    return PropertyTag.ThumbnailCompression;

                case "ThumbnailPhotometricInterp":
                    return PropertyTag.ThumbnailPhotometricInterp;

                case "ThumbnailImageDescription":
                    return PropertyTag.ThumbnailImageDescription;

                case "ThumbnailEquipMake":
                    return PropertyTag.ThumbnailEquipMake;

                case "ThumbnailEquipModel":
                    return PropertyTag.ThumbnailEquipModel;

                case "ThumbnailStripOffsets":
                    return PropertyTag.ThumbnailStripOffsets;

                case "ThumbnailOrientation":
                    return PropertyTag.ThumbnailOrientation;

                case "ThumbnailSamplesPerPixel":
                    return PropertyTag.ThumbnailSamplesPerPixel;

                case "ThumbnailRowsPerStrip":
                    return PropertyTag.ThumbnailRowsPerStrip;

                case "ThumbnailStripBytesCount":
                    return PropertyTag.ThumbnailStripBytesCount;

                case "ThumbnailResolutionX":
                    return PropertyTag.ThumbnailResolutionX;

                case "ThumbnailResolutionY":
                    return PropertyTag.ThumbnailResolutionY;

                case "ThumbnailPlanarConfig":
                    return PropertyTag.ThumbnailPlanarConfig;

                case "ThumbnailResolutionUnit":
                    return PropertyTag.ThumbnailResolutionUnit;

                case "ThumbnailTransferFunction":
                    return PropertyTag.ThumbnailTransferFunction;

                case "ThumbnailSoftwareUsed":
                    return PropertyTag.ThumbnailSoftwareUsed;

                case "ThumbnailDateTime":
                    return PropertyTag.ThumbnailDateTime;

                case "ThumbnailArtist":
                    return PropertyTag.ThumbnailArtist;

                case "ThumbnailWhitePoint":
                    return PropertyTag.ThumbnailWhitePoint;

                case "ThumbnailPrimaryChromaticities":
                    return PropertyTag.ThumbnailPrimaryChromaticities;

                case "ThumbnailYCbCrCoefficients":
                    return PropertyTag.ThumbnailYCbCrCoefficients;

                case "ThumbnailYCbCrSubsampling":
                    return PropertyTag.ThumbnailYCbCrSubsampling;

                case "ThumbnailYCbCrPositioning":
                    return PropertyTag.ThumbnailYCbCrPositioning;

                case "ThumbnailRefBlackWhite":
                    return PropertyTag.ThumbnailRefBlackWhite;

                case "ThumbnailCopyRight":
                    return PropertyTag.ThumbnailCopyRight;

                case "LuminanceTable":
                    return PropertyTag.LuminanceTable;

                case "ChrominanceTable":
                    return PropertyTag.ChrominanceTable;

                case "FrameDelay":
                    return PropertyTag.FrameDelay;

                case "LoopCount":
                    return PropertyTag.LoopCount;

                case "GlobalPalette":
                    return PropertyTag.GlobalPalette;

                case "IndexBackground":
                    return PropertyTag.IndexBackground;

                case "IndexTransparent":
                    return PropertyTag.IndexTransparent;

                case "PixelUnit":
                    return PropertyTag.PixelUnit;

                case "PixelPerUnitX":
                    return PropertyTag.PixelPerUnitX;

                case "PixelPerUnitY":
                    return PropertyTag.PixelPerUnitY;

                case "PaletteHistogram":
                    return PropertyTag.PaletteHistogram;

                case "Copyright":
                    return PropertyTag.Copyright;

                case "ExifExposureTime":
                    return PropertyTag.ExifExposureTime;

                case "ExifFNumber":
                    return PropertyTag.ExifFNumber;

                case "ExifIFD":
                    return PropertyTag.ExifIFD;

                case "ICCProfile":
                    return PropertyTag.ICCProfile;

                case "ExifExposureProg":
                    return PropertyTag.ExifExposureProg;

                case "ExifSpectralSense":
                    return PropertyTag.ExifSpectralSense;

                case "GpsIFD":
                    return PropertyTag.GpsIFD;

                case "ExifISOSpeed":
                    return PropertyTag.ExifISOSpeed;

                case "ExifOECF":
                    return PropertyTag.ExifOECF;

                case "ExifVer":
                    return PropertyTag.ExifVer;

                case "ExifDTOrig":
                    return PropertyTag.ExifDTOrig;

                case "ExifDTDigitized":
                    return PropertyTag.ExifDTDigitized;

                case "ExifCompConfig":
                    return PropertyTag.ExifCompConfig;

                case "ExifCompBPP":
                    return PropertyTag.ExifCompBPP;

                case "ExifShutterSpeed":
                    return PropertyTag.ExifShutterSpeed;

                case "ExifAperture":
                    return PropertyTag.ExifAperture;

                case "ExifBrightness":
                    return PropertyTag.ExifBrightness;

                case "ExifExposureBias":
                    return PropertyTag.ExifExposureBias;

                case "ExifMaxAperture":
                    return PropertyTag.ExifMaxAperture;

                case "ExifSubjectDist":
                    return PropertyTag.ExifSubjectDist;

                case "ExifMeteringMode":
                    return PropertyTag.ExifMeteringMode;

                case "ExifLightSource":
                    return PropertyTag.ExifLightSource;

                case "ExifFlash":
                    return PropertyTag.ExifFlash;

                case "ExifFocalLength":
                    return PropertyTag.ExifFocalLength;

                case "ExifMakerNote":
                    return PropertyTag.ExifMakerNote;

                case "ExifUserComment":
                    return PropertyTag.ExifUserComment;

                case "ExifDTSubsec":
                    return PropertyTag.ExifDTSubsec;

                case "ExifDTOrigSS":
                    return PropertyTag.ExifDTOrigSS;

                case "ExifDTDigSS":
                    return PropertyTag.ExifDTDigSS;

                case "ExifFPXVer":
                    return PropertyTag.ExifFPXVer;

                case "ExifColorSpace":
                    return PropertyTag.ExifColorSpace;

                case "ExifPixXDim":
                    return PropertyTag.ExifPixXDim;

                case "ExifPixYDim":
                    return PropertyTag.ExifPixYDim;

                case "ExifRelatedWav":
                    return PropertyTag.ExifRelatedWav;

                case "ExifInterop":
                    return PropertyTag.ExifInterop;

                case "ExifFlashEnergy":
                    return PropertyTag.ExifFlashEnergy;

                case "ExifSpatialFR":
                    return PropertyTag.ExifSpatialFR;

                case "ExifFocalXRes":
                    return PropertyTag.ExifFocalXRes;

                case "ExifFocalYRes":
                    return PropertyTag.ExifFocalYRes;

                case "ExifFocalResUnit":
                    return PropertyTag.ExifFocalResUnit;

                case "ExifSubjectLoc":
                    return PropertyTag.ExifSubjectLoc;

                case "ExifExposureIndex":
                    return PropertyTag.ExifExposureIndex;

                case "ExifSensingMethod":
                    return PropertyTag.ExifSensingMethod;

                case "ExifFileSource":
                    return PropertyTag.ExifFileSource;

                case "ExifSceneType":
                    return PropertyTag.ExifSceneType;

                case "ExifCfaPattern":
                    return PropertyTag.ExifCfaPattern;
            }
            throw base.CreateUnknownConstantException(s, typeof(PropertyTag));
        }

        private SimpleFilter<int> Read80_SimpleFilterOfInt32(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id270_SimpleFilterOfInt32) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id223_IntegralFilterOfInt32) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read81_IntegralFilterOfInt32(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            SimpleFilter<int> o = new SimpleFilter<int>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private IntegralFilter<int> Read81_IntegralFilterOfInt32(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id223_IntegralFilterOfInt32) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            IntegralFilter<int> o = new IntegralFilter<int>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SimpleFilter<byte> Read82_SimpleFilterOfByte(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id269_SimpleFilterOfByte) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id222_IntegralFilterOfByte) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read83_IntegralFilterOfByte(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            SimpleFilter<byte> o = new SimpleFilter<byte>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private IntegralFilter<byte> Read83_IntegralFilterOfByte(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id222_IntegralFilterOfByte) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            IntegralFilter<byte> o = new IntegralFilter<byte>();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToByte(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualPropertyFilter Read84_VirtualPropertyFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id70_VirtualPropertyFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualPropertyFilter o = new VirtualPropertyFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id215_PropertyId)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.PropertyId = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id184_DateFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read69_DateFilter(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id216_StringFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read72_StringFilter(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id217_VersionFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read75_SimpleFilterOfVersion(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id218_Int64Filter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read77_IntegralFilterOfInt64(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id219_UInt32Filter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read79_IntegralFilterOfUInt32(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id220_Int32Filter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read81_IntegralFilterOfInt32(false, true);
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id221_ByteFilter)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Filter = this.Read83_IntegralFilterOfByte(false, true);
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":PropertyId, :DateFilter, :StringFilter, :VersionFilter, :Int64Filter, :UInt32Filter, :Int32Filter, :ByteFilter");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":PropertyId, :DateFilter, :StringFilter, :VersionFilter, :Int64Filter, :UInt32Filter, :Int32Filter, :ByteFilter");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private CustomContentFilter Read85_CustomContentFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id267_CustomContentFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name == this.id181_HexContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read104_HexContentFilter(isNullable, false);
                }
                if ((type.Name == this.id67_VirtualItemHexContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read105_VirtualItemHexContentFilter(isNullable, false);
                }
                if ((type.Name == this.id179_ContentFilter) && (type.Namespace == this.id2_Item))
                {
                    return this.Read86_ContentFilter(isNullable, false);
                }
                if ((type.Name != this.id66_VirtualItemContentFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read87_VirtualItemContentFilter(isNullable, false);
            }
            if (!flag)
            {
                throw base.CreateAbstractTypeException("CustomContentFilter", "");
            }
            return null;
        }

        private ContentFilter Read86_ContentFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id179_ContentFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id66_VirtualItemContentFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read87_VirtualItemContentFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            ContentFilter o = new ContentFilter();
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id212_Options)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Options = this.Read70_ContentFilterOptions(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id207_Comparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Comparision = this.Read71_ContentComparision(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id213_Text)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Text = base.Reader.ReadElementString();
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id214_Encoding)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.EncodingAsString = base.Reader.ReadElementString();
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Options, :Comparision, :Text, :Encoding");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Options, :Comparision, :Text, :Encoding");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemContentFilter Read87_VirtualItemContentFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id66_VirtualItemContentFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemContentFilter o = new VirtualItemContentFilter();
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id212_Options)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Options = this.Read70_ContentFilterOptions(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id207_Comparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Comparision = this.Read71_ContentComparision(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id213_Text)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.Text = base.Reader.ReadElementString();
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id214_Encoding)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.EncodingAsString = base.Reader.ReadElementString();
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":Options, :Comparision, :Text, :Encoding");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":Options, :Comparision, :Text, :Encoding");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private TimeComparision Read88_TimeComparision(string s)
        {
            switch (s)
            {
                case "Ignore":
                    return TimeComparision.Ignore;

                case "At":
                    return TimeComparision.At;

                case "Before":
                    return TimeComparision.Before;

                case "After":
                    return TimeComparision.After;

                case "Between":
                    return TimeComparision.Between;

                case "NotBetween":
                    return TimeComparision.NotBetween;

                case "HoursOf1":
                    return TimeComparision.HoursOf1;

                case "HoursOf6":
                    return TimeComparision.HoursOf6;
            }
            throw base.CreateUnknownConstantException(s, typeof(TimeComparision));
        }

        private TimeFilter Read89_TimeFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id185_TimeFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id65_VirtualItemTimeFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read91_VirtualItemTimeFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            TimeFilter o = new TimeFilter();
            bool[] flagArray = new bool[3];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id190_TimeComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.TimeComparision = this.Read88_TimeComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id191_FromTime)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFromTime = XmlSerializationReader.ToTime(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id192_ToTime)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableToTime = XmlSerializationReader.ToTime(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":TimeComparision, :FromTime, :ToTime");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":TimeComparision, :FromTime, :ToTime");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private LightSource Read9_LightSource(string s)
        {
            switch (s)
            {
                case "Unknown":
                    return LightSource.Unknown;

                case "Daylight":
                    return LightSource.Daylight;

                case "Fluorescent":
                    return LightSource.Fluorescent;

                case "Tungsten":
                    return LightSource.Tungsten;

                case "StandardLightA":
                    return LightSource.StandardLightA;

                case "StandardLightB":
                    return LightSource.StandardLightB;

                case "StandardLightC":
                    return LightSource.StandardLightC;

                case "D55":
                    return LightSource.D55;

                case "D65":
                    return LightSource.D65;

                case "D75":
                    return LightSource.D75;

                case "Reserved":
                    return LightSource.Reserved;

                case "Other":
                    return LightSource.Other;
            }
            throw base.CreateUnknownConstantException(s, typeof(LightSource));
        }

        private ItemDateTimePart Read90_ItemDateTimePart(string s)
        {
            switch (s)
            {
                case "LastAccessTime":
                    return ItemDateTimePart.LastAccessTime;

                case "CreationTime":
                    return ItemDateTimePart.CreationTime;

                case "LastWriteTime":
                    return ItemDateTimePart.LastWriteTime;
            }
            throw base.CreateUnknownConstantException(s, typeof(ItemDateTimePart));
        }

        private VirtualItemTimeFilter Read91_VirtualItemTimeFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id65_VirtualItemTimeFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemTimeFilter o = new VirtualItemTimeFilter();
            bool[] flagArray = new bool[4];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id190_TimeComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.TimeComparision = this.Read88_TimeComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id191_FromTime)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableFromTime = XmlSerializationReader.ToTime(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id192_ToTime)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SerializableToTime = XmlSerializationReader.ToTime(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id193_TimePart)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.TimePart = this.Read90_ItemDateTimePart(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":TimeComparision, :FromTime, :ToTime, :TimePart");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":TimeComparision, :FromTime, :ToTime, :TimePart");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private FileAttributes Read92_FileAttributes(string s)
        {
            return (FileAttributes) ((int) XmlSerializationReader.ToEnum(s, this.FileAttributesValues, "global::System.IO.FileAttributes"));
        }

        private AttributeFilter Read93_AttributeFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id182_AttributeFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id61_VirtualItemAttributeFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read94_VirtualItemAttributeFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            AttributeFilter o = new AttributeFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id205_IncludeAttributes)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.IncludeAttributes = this.Read92_FileAttributes(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id206_ExcludeAttributes)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ExcludeAttributes = this.Read92_FileAttributes(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":IncludeAttributes, :ExcludeAttributes");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":IncludeAttributes, :ExcludeAttributes");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemAttributeFilter Read94_VirtualItemAttributeFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id61_VirtualItemAttributeFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemAttributeFilter o = new VirtualItemAttributeFilter();
            bool[] flagArray = new bool[2];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id205_IncludeAttributes)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.IncludeAttributes = this.Read92_FileAttributes(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id206_ExcludeAttributes)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ExcludeAttributes = this.Read92_FileAttributes(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":IncludeAttributes, :ExcludeAttributes");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":IncludeAttributes, :ExcludeAttributes");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private SizeUnit Read95_SizeUnit(string s)
        {
            switch (s)
            {
                case "Byte":
                    return SizeUnit.Byte;

                case "KiloByte":
                    return SizeUnit.KiloByte;

                case "MegaByte":
                    return SizeUnit.MegaByte;
            }
            throw base.CreateUnknownConstantException(s, typeof(SizeUnit));
        }

        private SizeComparision Read96_SizeComparision(string s)
        {
            switch (s)
            {
                case "Ignore":
                    return SizeComparision.Ignore;

                case "Equals":
                    return SizeComparision.Equals;

                case "Smaller":
                    return SizeComparision.Smaller;

                case "Larger":
                    return SizeComparision.Larger;

                case "Between":
                    return SizeComparision.Between;

                case "NotBetween":
                    return SizeComparision.NotBetween;

                case "PercentOf25":
                    return SizeComparision.PercentOf25;

                case "PercentOf50":
                    return SizeComparision.PercentOf50;
            }
            throw base.CreateUnknownConstantException(s, typeof(SizeComparision));
        }

        private SizeFilter Read97_SizeFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id183_SizeFilter) || (type.Namespace != this.id2_Item)))
            {
                if ((type.Name != this.id62_VirtualItemSizeFilter) || (type.Namespace != this.id2_Item))
                {
                    throw base.CreateUnknownTypeException(type);
                }
                return this.Read98_VirtualItemSizeFilter(isNullable, false);
            }
            if (flag)
            {
                return null;
            }
            SizeFilter o = new SizeFilter();
            bool[] flagArray = new bool[5];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id203_SizeUnit)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.SizeUnit = this.Read95_SizeUnit(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id204_SizeComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SizeComparision = this.Read96_SizeComparision(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue, :SizeUnit, :SizeComparision");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue, :SizeUnit, :SizeComparision");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemSizeFilter Read98_VirtualItemSizeFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id62_VirtualItemSizeFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemSizeFilter o = new VirtualItemSizeFilter();
            bool[] flagArray = new bool[5];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id200_ValueComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ValueComparision = this.Read73_SimpleComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id201_FromValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id202_ToValue)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToValue = XmlConvert.ToInt64(base.Reader.ReadElementString());
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id203_SizeUnit)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.SizeUnit = this.Read95_SizeUnit(base.Reader.ReadElementString());
                        }
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id204_SizeComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.SizeComparision = this.Read96_SizeComparision(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue, :SizeUnit, :SizeComparision");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":ValueComparision, :FromValue, :ToValue, :SizeUnit, :SizeComparision");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        private VirtualItemDateFilter Read99_VirtualItemDateFilter(bool isNullable, bool checkType)
        {
            XmlQualifiedName type = checkType ? base.GetXsiType() : null;
            bool flag = false;
            if (isNullable)
            {
                flag = base.ReadNull();
            }
            if ((checkType && (type != null)) && ((type.Name != this.id64_VirtualItemDateFilter) || (type.Namespace != this.id2_Item)))
            {
                throw base.CreateUnknownTypeException(type);
            }
            if (flag)
            {
                return null;
            }
            VirtualItemDateFilter o = new VirtualItemDateFilter();
            bool[] flagArray = new bool[6];
            while (base.Reader.MoveToNextAttribute())
            {
                if (!base.IsXmlnsAttribute(base.Reader.Name))
                {
                    base.UnknownNode(o);
                }
            }
            base.Reader.MoveToElement();
            if (base.Reader.IsEmptyElement)
            {
                base.Reader.Skip();
                return o;
            }
            base.Reader.ReadStartElement();
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    if ((!flagArray[0] && (base.Reader.LocalName == this.id194_DateComparision)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.DateComparision = this.Read67_DateComparision(base.Reader.ReadElementString());
                        flagArray[0] = true;
                    }
                    else if ((!flagArray[1] && (base.Reader.LocalName == this.id195_NotOlderThan)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.NotOlderThan = XmlConvert.ToInt32(base.Reader.ReadElementString());
                        }
                        flagArray[1] = true;
                    }
                    else if ((!flagArray[2] && (base.Reader.LocalName == this.id196_DateMeasure)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.DateMeasure = this.Read68_DateUnit(base.Reader.ReadElementString());
                        }
                        flagArray[2] = true;
                    }
                    else if ((!flagArray[3] && (base.Reader.LocalName == this.id197_FromDate)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.FromDate = XmlSerializationReader.ToDate(base.Reader.ReadElementString());
                        flagArray[3] = true;
                    }
                    else if ((!flagArray[4] && (base.Reader.LocalName == this.id198_ToDate)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        o.ToDate = XmlSerializationReader.ToDate(base.Reader.ReadElementString());
                        flagArray[4] = true;
                    }
                    else if ((!flagArray[5] && (base.Reader.LocalName == this.id199_DatePart)) && (base.Reader.NamespaceURI == this.id2_Item))
                    {
                        if (base.Reader.IsEmptyElement)
                        {
                            base.Reader.Skip();
                        }
                        else
                        {
                            o.DatePart = this.Read90_ItemDateTimePart(base.Reader.ReadElementString());
                        }
                        flagArray[5] = true;
                    }
                    else
                    {
                        base.UnknownNode(o, ":DateComparision, :NotOlderThan, :DateMeasure, :FromDate, :ToDate, :DatePart");
                    }
                }
                else
                {
                    base.UnknownNode(o, ":DateComparision, :NotOlderThan, :DateMeasure, :FromDate, :ToDate, :DatePart");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            base.ReadEndElement();
            return o;
        }

        internal Hashtable ActionStateValues
        {
            get
            {
                if (this._ActionStateValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("Enabled", 1L);
                    hashtable.Add("Visible", 2L);
                    hashtable.Add("Checked", 4L);
                    hashtable.Add("Indeterminate", 8L);
                    this._ActionStateValues = hashtable;
                }
                return this._ActionStateValues;
            }
        }

        internal Hashtable ArchiveFormatCapabilitiesValues
        {
            get
            {
                if (this._ArchiveFormatCapabilitiesValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("DetectFormatByContent", 1L);
                    hashtable.Add("CreateArchive", 2L);
                    hashtable.Add("UpdateArchive", 4L);
                    hashtable.Add("MultiFileArchive", 8L);
                    hashtable.Add("EncryptContent", 0x10L);
                    this._ArchiveFormatCapabilitiesValues = hashtable;
                }
                return this._ArchiveFormatCapabilitiesValues;
            }
        }

        internal Hashtable AvailableItemActionsValues
        {
            get
            {
                if (this._AvailableItemActionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("CanRetry", 1L);
                    hashtable.Add("CanIgnore", 2L);
                    hashtable.Add("CanElevate", 4L);
                    hashtable.Add("CanUndoDestination", 8L);
                    hashtable.Add("CanRetryOrIgnore", 3L);
                    hashtable.Add("CanRetryOrElevate", 5L);
                    this._AvailableItemActionsValues = hashtable;
                }
                return this._AvailableItemActionsValues;
            }
        }

        internal Hashtable BindActionPropertyValues
        {
            get
            {
                if (this._BindActionPropertyValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("Enabled", 1L);
                    hashtable.Add("Text", 2L);
                    hashtable.Add("Visible", 4L);
                    hashtable.Add("Checked", 8L);
                    hashtable.Add("Image", 0x10L);
                    hashtable.Add("Shortcuts", 0x20L);
                    hashtable.Add("CanClick", 0x40L);
                    hashtable.Add("CanUpdate", 0x80L);
                    hashtable.Add("All", 0xffL);
                    this._BindActionPropertyValues = hashtable;
                }
                return this._BindActionPropertyValues;
            }
        }

        internal Hashtable CanMoveListViewItemValues
        {
            get
            {
                if (this._CanMoveListViewItemValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("Up", 1L);
                    hashtable.Add("Down", 2L);
                    hashtable.Add("UpInGroup", 4L);
                    hashtable.Add("DownInGroup", 8L);
                    this._CanMoveListViewItemValues = hashtable;
                }
                return this._CanMoveListViewItemValues;
            }
        }

        internal Hashtable CompareFoldersOptionsValues
        {
            get
            {
                if (this._CompareFoldersOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("CompareAttributes", 1L);
                    hashtable.Add("CompareLastWriteTime", 2L);
                    hashtable.Add("CompareSize", 4L);
                    hashtable.Add("CompareContent", 8L);
                    hashtable.Add("CompareContentAsync", 0x10L);
                    hashtable.Add("AutoCompareContentAsync", 0x20L);
                    this._CompareFoldersOptionsValues = hashtable;
                }
                return this._CompareFoldersOptionsValues;
            }
        }

        internal Hashtable ContentFilterOptionsValues
        {
            get
            {
                if (this._ContentFilterOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("Regex", 1L);
                    hashtable.Add("CaseSensitive", 2L);
                    hashtable.Add("WholeWords", 4L);
                    hashtable.Add("SpaceCompress", 8L);
                    hashtable.Add("UseIFilter", 0x10L);
                    hashtable.Add("DetectEncoding", 0x20L);
                    this._ContentFilterOptionsValues = hashtable;
                }
                return this._ContentFilterOptionsValues;
            }
        }

        internal Hashtable ContentFlagValues
        {
            get
            {
                if (this._ContentFlagValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("contflags_edit", 1L);
                    hashtable.Add("contflags_substsize", 2L);
                    hashtable.Add("contflags_substdatetime", 4L);
                    hashtable.Add("contflags_substdate", 6L);
                    hashtable.Add("contflags_substtime", 8L);
                    hashtable.Add("contflags_substattributes", 10L);
                    hashtable.Add("contflags_substattributestr", 12L);
                    hashtable.Add("contflags_passthrough_size_float", 14L);
                    hashtable.Add("contflags_substmask", 14L);
                    hashtable.Add("contflags_fieldedit", 0x10L);
                    this._ContentFlagValues = hashtable;
                }
                return this._ContentFlagValues;
            }
        }

        internal Hashtable ContextMenuOptionsValues
        {
            get
            {
                if (this._ContextMenuOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("Explore", 1L);
                    hashtable.Add("CanRename", 2L);
                    hashtable.Add("VerbsOnly", 4L);
                    this._ContextMenuOptionsValues = hashtable;
                }
                return this._ContextMenuOptionsValues;
            }
        }

        internal Hashtable CopyWorkerOptionsValues
        {
            get
            {
                if (this._CopyWorkerOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("DeleteSource", 1L);
                    hashtable.Add("SkipEmptyFolders", 2L);
                    hashtable.Add("AsyncCopy", 4L);
                    hashtable.Add("AutoAsyncCopy", 8L);
                    hashtable.Add("CheckFreeSpace", 0x10L);
                    hashtable.Add("ClearROFromCD", 0x20L);
                    hashtable.Add("CopyACL", 0x40L);
                    hashtable.Add("CopyItemTime", 0x80L);
                    hashtable.Add("CopyFolderTime", 0x100L);
                    hashtable.Add("UseSystemCopy", 0x200L);
                    this._CopyWorkerOptionsValues = hashtable;
                }
                return this._CopyWorkerOptionsValues;
            }
        }

        internal Hashtable CustomizeFolderPartsValues
        {
            get
            {
                if (this._CustomizeFolderPartsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("All", 0xffL);
                    hashtable.Add("Columns", 1L);
                    hashtable.Add("Icon", 2L);
                    hashtable.Add("Filter", 4L);
                    hashtable.Add("Sort", 8L);
                    hashtable.Add("View", 0x10L);
                    hashtable.Add("ThumbnailSize", 0x20L);
                    hashtable.Add("Colors", 0x40L);
                    hashtable.Add("ListColumnCount", 0x80L);
                    this._CustomizeFolderPartsValues = hashtable;
                }
                return this._CustomizeFolderPartsValues;
            }
        }

        internal Hashtable FileAttributesValues
        {
            get
            {
                if (this._FileAttributesValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("ReadOnly", 1L);
                    hashtable.Add("Hidden", 2L);
                    hashtable.Add("System", 4L);
                    hashtable.Add("Directory", 0x10L);
                    hashtable.Add("Archive", 0x20L);
                    hashtable.Add("Device", 0x40L);
                    hashtable.Add("Normal", 0x80L);
                    hashtable.Add("Temporary", 0x100L);
                    hashtable.Add("SparseFile", 0x200L);
                    hashtable.Add("ReparsePoint", 0x400L);
                    hashtable.Add("Compressed", 0x800L);
                    hashtable.Add("Offline", 0x1000L);
                    hashtable.Add("NotContentIndexed", 0x2000L);
                    hashtable.Add("Encrypted", 0x4000L);
                    this._FileAttributesValues = hashtable;
                }
                return this._FileAttributesValues;
            }
        }

        internal Hashtable FindDuplicateOptionsValues
        {
            get
            {
                if (this._FindDuplicateOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("SameName", 1L);
                    hashtable.Add("SameSize", 2L);
                    hashtable.Add("SameContent", 4L);
                    this._FindDuplicateOptionsValues = hashtable;
                }
                return this._FindDuplicateOptionsValues;
            }
        }

        internal Hashtable FindFormatSourceValues
        {
            get
            {
                if (this._FindFormatSourceValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("Content", 1L);
                    hashtable.Add("Extension", 2L);
                    this._FindFormatSourceValues = hashtable;
                }
                return this._FindFormatSourceValues;
            }
        }

        internal Hashtable FormPlacementValues
        {
            get
            {
                if (this._FormPlacementValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("Location", 1L);
                    hashtable.Add("Size", 2L);
                    hashtable.Add("WindowState", 4L);
                    hashtable.Add("All", -1L);
                    this._FormPlacementValues = hashtable;
                }
                return this._FormPlacementValues;
            }
        }

        internal Hashtable IconOptionsValues
        {
            get
            {
                if (this._IconOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("ExtractIcons", 1L);
                    hashtable.Add("DisableExtractSlowIcons", 2L);
                    hashtable.Add("ShowOverlayIcons", 4L);
                    this._IconOptionsValues = hashtable;
                }
                return this._IconOptionsValues;
            }
        }

        internal Hashtable IconStyleValues
        {
            get
            {
                if (this._IconStyleValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("DefaultIcon", 1L);
                    hashtable.Add("CanUseDelayedExtract", 2L);
                    hashtable.Add("CanUseAlphaBlending", 4L);
                    this._IconStyleValues = hashtable;
                }
                return this._IconStyleValues;
            }
        }

        internal Hashtable InputDialogOptionValues
        {
            get
            {
                if (this._InputDialogOptionValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("AllowEmptyValue", 1L);
                    hashtable.Add("AllowSameValue", 2L);
                    hashtable.Add("ReadOnly", 4L);
                    this._InputDialogOptionValues = hashtable;
                }
                return this._InputDialogOptionValues;
            }
        }

        internal Hashtable ItemCapabilityValues
        {
            get
            {
                if (this._ItemCapabilityValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("HasParent", 1L);
                    hashtable.Add("HasExtender", 2L);
                    hashtable.Add("HasTarget", 4L);
                    hashtable.Add("HasSize", 8L);
                    hashtable.Add("HasThumbnail", 0x10L);
                    hashtable.Add("IsParentReal", 0x20L);
                    hashtable.Add("IsShellFolderShortcut", 0x40L);
                    hashtable.Add("HasShellFolderShortcut", 0x80L);
                    hashtable.Add("Deleted", 0x100L);
                    hashtable.Add("UseTargetIcon", 0x200L);
                    hashtable.Add("GlobalFileChangedAssigned", 0x400L);
                    hashtable.Add("IsShellLink", 0x800L);
                    hashtable.Add("IsUrlLink", 0x1000L);
                    hashtable.Add("HasCreationTime", 0x2000L);
                    hashtable.Add("HasLastWriteTime", 0x4000L);
                    hashtable.Add("HasLastAccessTime", 0x8000L);
                    hashtable.Add("Unreadable", 0x10000L);
                    hashtable.Add("GlobalFolderChangedAssigned", 0x20000L);
                    hashtable.Add("HasVolume", 0x40000L);
                    hashtable.Add("QueryRemoveAssigned", 0x80000L);
                    hashtable.Add("VolumeEventsAssigned", 0x100000L);
                    hashtable.Add("CheckNetworkShare", 0x200000L);
                    hashtable.Add("DisableContentMap", 0x400000L);
                    hashtable.Add("ItemRefreshNeeded", 0x800000L);
                    hashtable.Add("IsElevated", 0x1000000L);
                    hashtable.Add("HasContentFolder", 0x2000000L);
                    hashtable.Add("HasShellItem", 0x4000000L);
                    hashtable.Add("HasExtension", 0x8000000L);
                    this._ItemCapabilityValues = hashtable;
                }
                return this._ItemCapabilityValues;
            }
        }

        internal Hashtable PanelLayoutEntryValues
        {
            get
            {
                if (this._PanelLayoutEntryValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("FolderBarVisible", 1L);
                    hashtable.Add("FolderBarOrientation", 2L);
                    hashtable.Add("View", 4L);
                    hashtable.Add("Columns", 8L);
                    hashtable.Add("ToolbarsVisible", 0x10L);
                    hashtable.Add("ThumbnailSize", 0x20L);
                    hashtable.Add("ListColumnCount", 0x40L);
                    hashtable.Add("All", 0x7fL);
                    this._PanelLayoutEntryValues = hashtable;
                }
                return this._PanelLayoutEntryValues;
            }
        }

        internal Hashtable PanelToolbarValues
        {
            get
            {
                if (this._PanelToolbarValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("None", 0L);
                    hashtable.Add("Folder", 1L);
                    hashtable.Add("Item", 2L);
                    hashtable.Add("Find", 4L);
                    this._PanelToolbarValues = hashtable;
                }
                return this._PanelToolbarValues;
            }
        }

        internal Hashtable PathViewValues
        {
            get
            {
                if (this._PathViewValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("ShowNormalRootName", 0L);
                    hashtable.Add("ShowShortRootName", 1L);
                    hashtable.Add("ShowIconForEveryFolder", 2L);
                    hashtable.Add("ShowActiveState", 4L);
                    hashtable.Add("ShowDriveMenuOnHover", 8L);
                    hashtable.Add("VistaLikeBreadcrumb", 0x10L);
                    hashtable.Add("ShowFolderIcon", 0x20L);
                    this._PathViewValues = hashtable;
                }
                return this._PathViewValues;
            }
        }

        internal Hashtable PK_CAPSValues
        {
            get
            {
                if (this._PK_CAPSValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("PK_CAPS_NEW", 1L);
                    hashtable.Add("PK_CAPS_MODIFY", 2L);
                    hashtable.Add("PK_CAPS_MULTIPLE", 4L);
                    hashtable.Add("PK_CAPS_DELETE", 8L);
                    hashtable.Add("PK_CAPS_OPTIONS", 0x10L);
                    hashtable.Add("PK_CAPS_MEMPACK", 0x20L);
                    hashtable.Add("PK_CAPS_BY_CONTENT", 0x40L);
                    hashtable.Add("PK_CAPS_SEARCHTEXT", 0x80L);
                    hashtable.Add("PK_CAPS_HIDE", 0x100L);
                    hashtable.Add("PK_CAPS_ENCRYPT", 0x200L);
                    this._PK_CAPSValues = hashtable;
                }
                return this._PK_CAPSValues;
            }
        }

        internal Hashtable PK_PACKValues
        {
            get
            {
                if (this._PK_PACKValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("PK_PACK_MOVE_FILES", 1L);
                    hashtable.Add("PK_PACK_SAVE_PATHS", 2L);
                    this._PK_PACKValues = hashtable;
                }
                return this._PK_PACKValues;
            }
        }

        internal Hashtable QuickFindOptionsValues
        {
            get
            {
                if (this._QuickFindOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("PrefixSearch", 1L);
                    hashtable.Add("QuickFilter", 2L);
                    hashtable.Add("AlwaysShowFolders", 4L);
                    hashtable.Add("ExecuteOnEnter", 8L);
                    hashtable.Add("AutoHide", 0x10L);
                    this._QuickFindOptionsValues = hashtable;
                }
                return this._QuickFindOptionsValues;
            }
        }

        internal Hashtable SearchFolderOptionsValues
        {
            get
            {
                if (this._SearchFolderOptionsValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("ProcessSubfolders", 1L);
                    hashtable.Add("ProcessArchives", 2L);
                    hashtable.Add("SkipUnmatchedSubfolders", 4L);
                    hashtable.Add("SkipReparsePoints", 8L);
                    hashtable.Add("DetectChanges", 0x10L);
                    hashtable.Add("AsyncSearch", 0x20L);
                    hashtable.Add("AutoAsyncSearch", 0x40L);
                    hashtable.Add("ExpandAggregatedRoot", 0x80L);
                    this._SearchFolderOptionsValues = hashtable;
                }
                return this._SearchFolderOptionsValues;
            }
        }

        internal Hashtable SevenZipFormatCapabilitiesValues
        {
            get
            {
                if (this._SevenZipFormatCapabilitiesValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("Solid", 1L);
                    hashtable.Add("MultiThread", 2L);
                    hashtable.Add("SFX", 4L);
                    hashtable.Add("EncryptFileNames", 8L);
                    hashtable.Add("Internal", 0x10L);
                    hashtable.Add("AppendExt", 0x20L);
                    this._SevenZipFormatCapabilitiesValues = hashtable;
                }
                return this._SevenZipFormatCapabilitiesValues;
            }
        }

        internal Hashtable TwoPanelLayoutEntryValues
        {
            get
            {
                if (this._TwoPanelLayoutEntryValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("OnePanel", 1L);
                    hashtable.Add("PanelsOrientation", 2L);
                    hashtable.Add("LeftLayout", 4L);
                    hashtable.Add("RightLayout", 8L);
                    hashtable.Add("ActivePanel", 0x10L);
                    hashtable.Add("All", 0x1fL);
                    this._TwoPanelLayoutEntryValues = hashtable;
                }
                return this._TwoPanelLayoutEntryValues;
            }
        }

        internal Hashtable ViewFiltersValues
        {
            get
            {
                if (this._ViewFiltersValues == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("IncludeMask", 1L);
                    hashtable.Add("ExcludeMask", 2L);
                    hashtable.Add("Content", 4L);
                    hashtable.Add("Attributes", 8L);
                    hashtable.Add("Advanced", 0x10L);
                    hashtable.Add("Folder", 0x20L);
                    this._ViewFiltersValues = hashtable;
                }
                return this._ViewFiltersValues;
            }
        }
    }
}

