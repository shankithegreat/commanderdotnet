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
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class XmlSerializerContract : XmlSerializerImplementation
    {
        private Hashtable readMethods;
        private Hashtable typedSerializers;
        private Hashtable writeMethods;

        public override bool CanSerialize(System.Type type)
        {
            return ((type == typeof(ImageProvider)) || ((type == typeof(CustomImageProvider)) || ((type == typeof(KeysConverter2)) || ((type == typeof(PropertyTagType)) || ((type == typeof(PropertyTag)) || ((type == typeof(LightSource)) || ((type == typeof(ToolStripButtonRenderer)) || ((type == typeof(ConfigurableSettingsProvider)) || ((type == typeof(ReleaseType)) || ((type == typeof(ListViewColumnInfo)) || ((type == typeof(ListViewColumnCollection)) || ((type == typeof(PanelLayoutEntry)) || ((type == typeof(PanelToolbar)) || ((type == typeof(PanelLayout)) || ((type == typeof(ActivePanel)) || ((type == typeof(TwoPanelLayoutEntry)) || ((type == typeof(TwoPanelLayout)) || ((type == typeof(CustomActionLink)) || ((type == typeof(CustomBindActionLink)) || ((type == typeof(ActionState)) || ((type == typeof(BindActionProperty)) || ((type == typeof(BreadcrumbView)) || ((type == typeof(BreadcrumbToolStripRenderer)) || ((type == typeof(InputDialogOption)) || ((type == typeof(ElevatedProcess)) || ((type == typeof(ArchiveFormatConverter)) || ((type == typeof(ArchiveFormatCapabilities)) || ((type == typeof(ArchiveFormatInfo)) || ((type == typeof(PersistArchiveFormatInfo)) || ((type == typeof(FindFormatSource)) || ((type == typeof(ArjHeader)) || ((type == typeof(ProcessItemEventArgs)) || ((type == typeof(ProcessorState)) || ((type == typeof(SequenseProcessorType)) || ((type == typeof(PK_OM)) || ((type == typeof(PK_OPERATION)) || ((type == typeof(PK_CAPS)) || ((type == typeof(PK_VOL)) || ((type == typeof(PK_PACK)) || ((type == typeof(PackDefaultParamStruct)) || ((type == typeof(WcxErrors)) || ((type == typeof(DefaultIcon)) || ((type == typeof(ShellImageProvider)) || ((type == typeof(FileSystemItem.ItemCapability)) || ((type == typeof(LocalFileSystemCreator)) || ((type == typeof(NetworkFileSystemCreator)) || ((type == typeof(ShellFileSystemCreator)) || ((type == typeof(ContentFlag)) || ((type == typeof(ContentDefaultParamStruct)) || ((type == typeof(tdateformat)) || ((type == typeof(ttimeformat)) || ((type == typeof(WdxFieldInfo)) || ((type == typeof(AggregatedFilterCondition)) || ((type == typeof(AggregatedVirtualItemFilter)) || ((type == typeof(FilterContainer)) || ((type == typeof(NamedFilter)) || ((type == typeof(FilterHelper)) || ((type == typeof(VirtualItemNameFilter)) || ((type == typeof(VirtualItemFullNameFilter)) || ((type == typeof(VirtualItemAttributeFilter)) || ((type == typeof(VirtualItemSizeFilter)) || ((type == typeof(ItemDateTimePart)) || ((type == typeof(VirtualItemDateFilter)) || ((type == typeof(VirtualItemTimeFilter)) || ((type == typeof(VirtualItemContentFilter)) || ((type == typeof(VirtualItemHexContentFilter)) || ((type == typeof(NameListCondition)) || ((type == typeof(VirtualItemNameListFilter)) || ((type == typeof(VirtualPropertyFilter)) || ((type == typeof(VirtualHighligher)) || ((type == typeof(ListViewHighlighter)) || ((type == typeof(HighlighterIconType)) || ((type == typeof(HashPropertyProvider)) || ((type == typeof(VistaThumbnailProvider)) || ((type == typeof(CustomizeFolderParts)) || ((type == typeof(ColorSpace)) || ((type == typeof(DescriptionPropertyProvider)) || ((type == typeof(DummyClientSite)) || ((type == typeof(HtmlPropertyProvider)) || ((type == typeof(BitrateTypeConverter)) || ((type == typeof(AudioChannelsTypeConverter)) || ((type == typeof(AudioSampleRateTypeConverter)) || ((type == typeof(DurationTypeConverter)) || ((type == typeof(ImageSizeTypeConverter)) || ((type == typeof(DPITypeConverter)) || ((type == typeof(ISOSpeedTypeConverter)) || ((type == typeof(RatingTypeConverter)) || ((type == typeof(EncodingConveter)) || ((type == typeof(ImagePropertyProvider)) || ((type == typeof(PsdPropertyProvider)) || ((type == typeof(TagLibPropertyProvider)) || ((type == typeof(TextPropertyProvider)) || ((type == typeof(VirtualToolTip)) || ((type == typeof(ThrobberStyle)) || ((type == typeof(ThrobberRenderer)) || ((type == typeof(AutoRefreshMode)) || ((type == typeof(FtpFileSystemCreator)) || ((type == typeof(NullFileSystemCreator)) || ((type == typeof(CustomVirtualFolder)) || ((type == typeof(CanMoveResult)) || ((type == typeof(IconOptions)) || ((type == typeof(DelayedExtractMode)) || ((type == typeof(PathView)) || ((type == typeof(PanelView)) || ((type == typeof(ContextMenuOptions)) || ((type == typeof(VirtualIcon)) || ((type == typeof(PropertyValueList)) || ((type == typeof(PropertyValue)) || ((type == typeof(SimpleEncrypt)) || ((type == typeof(ProgressRenderMode)) || ((type == typeof(ProgressState)) || ((type == typeof(VistaProgressBarRenderer)) || ((type == typeof(MarqueeStyle)) || ((type == typeof(XPProgressBarRenderer)) || ((type == typeof(AskMode)) || ((type == typeof(OperationResult)) || ((type == typeof(ItemPropId)) || ((type == typeof(FileTimeType)) || ((type == typeof(ArchivePropId)) || ((type == typeof(KnownSevenZipFormat)) || ((type == typeof(SevenZipFormatCapabilities)) || ((type == typeof(CompressionLevel)) || ((type == typeof(CompressionMethod)) || ((type == typeof(EncryptionMethod)) || ((type == typeof(SevenZipPropertiesBuilder.SolidSizeUnit)) || ((type == typeof(ComplexFilterView)) || ((type == typeof(ViewFilters)) || ((type == typeof(PanelContentContainer)) || ((type == typeof(ControllerType)) || ((type == typeof(Controller)) || ((type == typeof(FormPlacement)) || ((type == typeof(ArgumentKey)) || ((type == typeof(CanMoveListViewItem)) || ((type == typeof(Trace)) || ((type == typeof(TwoPanelContainer.SinglePanel)) || ((type == typeof(GeneralTab)) || ((type == typeof(TwoPanelTab)) || ((type == typeof(ArchiveUpdateMode)) || ((type == typeof(PackStage)) || ((type == typeof(PackProgressSnapshot)) || ((type == typeof(CustomBackgroundWorker)) || ((type == typeof(EventBackgroundWorker)) || ((type == typeof(CopyDestinationItem)) || ((type == typeof(MessageDialogResult)) || ((type == typeof(DoubleClickAction)) || ((type == typeof(QuickFindOptions)) || ((type == typeof(VirtualFilePanel.ListViewSort)) || ((type == typeof(CustomAsyncFolder)) || ((type == typeof(SearchFolderOptions)) || ((type == typeof(FindDuplicateOptions)) || ((type == typeof(Compare)) || ((type == typeof(ChangeItemAction)) || ((type == typeof(AvailableItemActions)) || ((type == typeof(CompareFoldersOptions)) || ((type == typeof(OverwriteDialogResult)) || ((type == typeof(CopyWorkerOptions)) || ((type == typeof(CopyMode)) || ((type == typeof(ProcessedSize)) || ((type == typeof(CopyProgressSnapshot)) || (type == typeof(IconStyle)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))));
        }

        public override XmlSerializer GetSerializer(System.Type type)
        {
            if (type == typeof(ImageProvider))
            {
                return new ImageProviderSerializer();
            }
            if (type == typeof(CustomImageProvider))
            {
                return new CustomImageProviderSerializer();
            }
            if (type == typeof(KeysConverter2))
            {
                return new KeysConverter2Serializer();
            }
            if (type == typeof(PropertyTagType))
            {
                return new PropertyTagTypeSerializer();
            }
            if (type == typeof(PropertyTag))
            {
                return new PropertyTagSerializer();
            }
            if (type == typeof(LightSource))
            {
                return new LightSourceSerializer();
            }
            if (type == typeof(ToolStripButtonRenderer))
            {
                return new ToolStripButtonRendererSerializer();
            }
            if (type == typeof(ConfigurableSettingsProvider))
            {
                return new ConfigurableSettingsProviderSerializer();
            }
            if (type == typeof(ReleaseType))
            {
                return new ReleaseTypeSerializer();
            }
            if (type == typeof(ListViewColumnInfo))
            {
                return new ListViewColumnInfoSerializer();
            }
            if (type == typeof(ListViewColumnCollection))
            {
                return new ListViewColumnCollectionSerializer();
            }
            if (type == typeof(PanelLayoutEntry))
            {
                return new PanelLayoutEntrySerializer();
            }
            if (type == typeof(PanelToolbar))
            {
                return new PanelToolbarSerializer();
            }
            if (type == typeof(PanelLayout))
            {
                return new PanelLayoutSerializer();
            }
            if (type == typeof(ActivePanel))
            {
                return new ActivePanelSerializer();
            }
            if (type == typeof(TwoPanelLayoutEntry))
            {
                return new TwoPanelLayoutEntrySerializer();
            }
            if (type == typeof(TwoPanelLayout))
            {
                return new TwoPanelLayoutSerializer();
            }
            if (type == typeof(CustomActionLink))
            {
                return new CustomActionLinkSerializer();
            }
            if (type == typeof(CustomBindActionLink))
            {
                return new CustomBindActionLinkSerializer();
            }
            if (type == typeof(ActionState))
            {
                return new ActionStateSerializer();
            }
            if (type == typeof(BindActionProperty))
            {
                return new BindActionPropertySerializer();
            }
            if (type == typeof(BreadcrumbView))
            {
                return new BreadcrumbViewSerializer();
            }
            if (type == typeof(BreadcrumbToolStripRenderer))
            {
                return new BreadcrumbToolStripRendererSerializer();
            }
            if (type == typeof(InputDialogOption))
            {
                return new InputDialogOptionSerializer();
            }
            if (type == typeof(ElevatedProcess))
            {
                return new ElevatedProcessSerializer();
            }
            if (type == typeof(ArchiveFormatConverter))
            {
                return new ArchiveFormatConverterSerializer();
            }
            if (type == typeof(ArchiveFormatCapabilities))
            {
                return new ArchiveFormatCapabilitiesSerializer();
            }
            if (type == typeof(ArchiveFormatInfo))
            {
                return new ArchiveFormatInfoSerializer();
            }
            if (type == typeof(PersistArchiveFormatInfo))
            {
                return new PersistArchiveFormatInfoSerializer();
            }
            if (type == typeof(FindFormatSource))
            {
                return new FindFormatSourceSerializer();
            }
            if (type == typeof(ArjHeader))
            {
                return new ArjHeaderSerializer();
            }
            if (type == typeof(ProcessItemEventArgs))
            {
                return new ProcessItemEventArgsSerializer();
            }
            if (type == typeof(ProcessorState))
            {
                return new ProcessorStateSerializer();
            }
            if (type == typeof(SequenseProcessorType))
            {
                return new SequenseProcessorTypeSerializer();
            }
            if (type == typeof(PK_OM))
            {
                return new PK_OMSerializer();
            }
            if (type == typeof(PK_OPERATION))
            {
                return new PK_OPERATIONSerializer();
            }
            if (type == typeof(PK_CAPS))
            {
                return new PK_CAPSSerializer();
            }
            if (type == typeof(PK_VOL))
            {
                return new PK_VOLSerializer();
            }
            if (type == typeof(PK_PACK))
            {
                return new PK_PACKSerializer();
            }
            if (type == typeof(PackDefaultParamStruct))
            {
                return new PackDefaultParamStructSerializer();
            }
            if (type == typeof(WcxErrors))
            {
                return new WcxErrorsSerializer();
            }
            if (type == typeof(DefaultIcon))
            {
                return new DefaultIconSerializer();
            }
            if (type == typeof(ShellImageProvider))
            {
                return new ShellImageProviderSerializer();
            }
            if (type == typeof(FileSystemItem.ItemCapability))
            {
                return new ItemCapabilitySerializer();
            }
            if (type == typeof(LocalFileSystemCreator))
            {
                return new LocalFileSystemCreatorSerializer();
            }
            if (type == typeof(NetworkFileSystemCreator))
            {
                return new NetworkFileSystemCreatorSerializer();
            }
            if (type == typeof(ShellFileSystemCreator))
            {
                return new ShellFileSystemCreatorSerializer();
            }
            if (type == typeof(ContentFlag))
            {
                return new ContentFlagSerializer();
            }
            if (type == typeof(ContentDefaultParamStruct))
            {
                return new ContentDefaultParamStructSerializer();
            }
            if (type == typeof(tdateformat))
            {
                return new tdateformatSerializer();
            }
            if (type == typeof(ttimeformat))
            {
                return new ttimeformatSerializer();
            }
            if (type == typeof(WdxFieldInfo))
            {
                return new WdxFieldInfoSerializer();
            }
            if (type == typeof(AggregatedFilterCondition))
            {
                return new AggregatedFilterConditionSerializer();
            }
            if (type == typeof(AggregatedVirtualItemFilter))
            {
                return new AggregatedVirtualItemFilterSerializer();
            }
            if (type == typeof(FilterContainer))
            {
                return new FilterContainerSerializer();
            }
            if (type == typeof(NamedFilter))
            {
                return new NamedFilterSerializer();
            }
            if (type == typeof(FilterHelper))
            {
                return new FilterHelperSerializer();
            }
            if (type == typeof(VirtualItemNameFilter))
            {
                return new VirtualItemNameFilterSerializer();
            }
            if (type == typeof(VirtualItemFullNameFilter))
            {
                return new VirtualItemFullNameFilterSerializer();
            }
            if (type == typeof(VirtualItemAttributeFilter))
            {
                return new VirtualItemAttributeFilterSerializer();
            }
            if (type == typeof(VirtualItemSizeFilter))
            {
                return new VirtualItemSizeFilterSerializer();
            }
            if (type == typeof(ItemDateTimePart))
            {
                return new ItemDateTimePartSerializer();
            }
            if (type == typeof(VirtualItemDateFilter))
            {
                return new VirtualItemDateFilterSerializer();
            }
            if (type == typeof(VirtualItemTimeFilter))
            {
                return new VirtualItemTimeFilterSerializer();
            }
            if (type == typeof(VirtualItemContentFilter))
            {
                return new VirtualItemContentFilterSerializer();
            }
            if (type == typeof(VirtualItemHexContentFilter))
            {
                return new VirtualItemHexContentFilterSerializer();
            }
            if (type == typeof(NameListCondition))
            {
                return new NameListConditionSerializer();
            }
            if (type == typeof(VirtualItemNameListFilter))
            {
                return new VirtualItemNameListFilterSerializer();
            }
            if (type == typeof(VirtualPropertyFilter))
            {
                return new VirtualPropertyFilterSerializer();
            }
            if (type == typeof(VirtualHighligher))
            {
                return new VirtualHighligherSerializer();
            }
            if (type == typeof(ListViewHighlighter))
            {
                return new ListViewHighlighterSerializer();
            }
            if (type == typeof(HighlighterIconType))
            {
                return new HighlighterIconTypeSerializer();
            }
            if (type == typeof(HashPropertyProvider))
            {
                return new HashPropertyProviderSerializer();
            }
            if (type == typeof(VistaThumbnailProvider))
            {
                return new VistaThumbnailProviderSerializer();
            }
            if (type == typeof(CustomizeFolderParts))
            {
                return new CustomizeFolderPartsSerializer();
            }
            if (type == typeof(ColorSpace))
            {
                return new ColorSpaceSerializer();
            }
            if (type == typeof(DescriptionPropertyProvider))
            {
                return new DescriptionPropertyProviderSerializer();
            }
            if (type == typeof(DummyClientSite))
            {
                return new DummyClientSiteSerializer();
            }
            if (type == typeof(HtmlPropertyProvider))
            {
                return new HtmlPropertyProviderSerializer();
            }
            if (type == typeof(BitrateTypeConverter))
            {
                return new BitrateTypeConverterSerializer();
            }
            if (type == typeof(AudioChannelsTypeConverter))
            {
                return new AudioChannelsTypeConverterSerializer();
            }
            if (type == typeof(AudioSampleRateTypeConverter))
            {
                return new AudioSampleRateTypeConverterSerializer();
            }
            if (type == typeof(DurationTypeConverter))
            {
                return new DurationTypeConverterSerializer();
            }
            if (type == typeof(ImageSizeTypeConverter))
            {
                return new ImageSizeTypeConverterSerializer();
            }
            if (type == typeof(DPITypeConverter))
            {
                return new DPITypeConverterSerializer();
            }
            if (type == typeof(ISOSpeedTypeConverter))
            {
                return new ISOSpeedTypeConverterSerializer();
            }
            if (type == typeof(RatingTypeConverter))
            {
                return new RatingTypeConverterSerializer();
            }
            if (type == typeof(EncodingConveter))
            {
                return new EncodingConveterSerializer();
            }
            if (type == typeof(ImagePropertyProvider))
            {
                return new ImagePropertyProviderSerializer();
            }
            if (type == typeof(PsdPropertyProvider))
            {
                return new PsdPropertyProviderSerializer();
            }
            if (type == typeof(TagLibPropertyProvider))
            {
                return new TagLibPropertyProviderSerializer();
            }
            if (type == typeof(TextPropertyProvider))
            {
                return new TextPropertyProviderSerializer();
            }
            if (type == typeof(VirtualToolTip))
            {
                return new VirtualToolTipSerializer();
            }
            if (type == typeof(ThrobberStyle))
            {
                return new ThrobberStyleSerializer();
            }
            if (type == typeof(ThrobberRenderer))
            {
                return new ThrobberRendererSerializer();
            }
            if (type == typeof(AutoRefreshMode))
            {
                return new AutoRefreshModeSerializer();
            }
            if (type == typeof(FtpFileSystemCreator))
            {
                return new FtpFileSystemCreatorSerializer();
            }
            if (type == typeof(NullFileSystemCreator))
            {
                return new NullFileSystemCreatorSerializer();
            }
            if (type == typeof(CustomVirtualFolder))
            {
                return new CustomVirtualFolderSerializer();
            }
            if (type == typeof(CanMoveResult))
            {
                return new CanMoveResultSerializer();
            }
            if (type == typeof(IconOptions))
            {
                return new IconOptionsSerializer();
            }
            if (type == typeof(DelayedExtractMode))
            {
                return new DelayedExtractModeSerializer();
            }
            if (type == typeof(PathView))
            {
                return new PathViewSerializer();
            }
            if (type == typeof(PanelView))
            {
                return new PanelViewSerializer();
            }
            if (type == typeof(ContextMenuOptions))
            {
                return new ContextMenuOptionsSerializer();
            }
            if (type == typeof(VirtualIcon))
            {
                return new VirtualIconSerializer();
            }
            if (type == typeof(PropertyValueList))
            {
                return new PropertyValueListSerializer();
            }
            if (type == typeof(PropertyValue))
            {
                return new PropertyValueSerializer();
            }
            if (type == typeof(SimpleEncrypt))
            {
                return new SimpleEncryptSerializer();
            }
            if (type == typeof(ProgressRenderMode))
            {
                return new ProgressRenderModeSerializer();
            }
            if (type == typeof(ProgressState))
            {
                return new ProgressStateSerializer();
            }
            if (type == typeof(VistaProgressBarRenderer))
            {
                return new VistaProgressBarRendererSerializer();
            }
            if (type == typeof(MarqueeStyle))
            {
                return new MarqueeStyleSerializer();
            }
            if (type == typeof(XPProgressBarRenderer))
            {
                return new XPProgressBarRendererSerializer();
            }
            if (type == typeof(AskMode))
            {
                return new AskModeSerializer();
            }
            if (type == typeof(OperationResult))
            {
                return new OperationResultSerializer();
            }
            if (type == typeof(ItemPropId))
            {
                return new ItemPropIdSerializer();
            }
            if (type == typeof(FileTimeType))
            {
                return new FileTimeTypeSerializer();
            }
            if (type == typeof(ArchivePropId))
            {
                return new ArchivePropIdSerializer();
            }
            if (type == typeof(KnownSevenZipFormat))
            {
                return new KnownSevenZipFormatSerializer();
            }
            if (type == typeof(SevenZipFormatCapabilities))
            {
                return new SevenZipFormatCapabilitiesSerializer();
            }
            if (type == typeof(CompressionLevel))
            {
                return new CompressionLevelSerializer();
            }
            if (type == typeof(CompressionMethod))
            {
                return new CompressionMethodSerializer();
            }
            if (type == typeof(EncryptionMethod))
            {
                return new EncryptionMethodSerializer();
            }
            if (type == typeof(SevenZipPropertiesBuilder.SolidSizeUnit))
            {
                return new SolidSizeUnitSerializer();
            }
            if (type == typeof(ComplexFilterView))
            {
                return new ComplexFilterViewSerializer();
            }
            if (type == typeof(ViewFilters))
            {
                return new ViewFiltersSerializer();
            }
            if (type == typeof(PanelContentContainer))
            {
                return new PanelContentContainerSerializer();
            }
            if (type == typeof(ControllerType))
            {
                return new ControllerTypeSerializer();
            }
            if (type == typeof(Controller))
            {
                return new ControllerSerializer();
            }
            if (type == typeof(FormPlacement))
            {
                return new FormPlacementSerializer();
            }
            if (type == typeof(ArgumentKey))
            {
                return new ArgumentKeySerializer();
            }
            if (type == typeof(CanMoveListViewItem))
            {
                return new CanMoveListViewItemSerializer();
            }
            if (type == typeof(Trace))
            {
                return new TraceSerializer();
            }
            if (type == typeof(TwoPanelContainer.SinglePanel))
            {
                return new SinglePanelSerializer();
            }
            if (type == typeof(GeneralTab))
            {
                return new GeneralTabSerializer();
            }
            if (type == typeof(TwoPanelTab))
            {
                return new TwoPanelTabSerializer();
            }
            if (type == typeof(ArchiveUpdateMode))
            {
                return new ArchiveUpdateModeSerializer();
            }
            if (type == typeof(PackStage))
            {
                return new PackStageSerializer();
            }
            if (type == typeof(PackProgressSnapshot))
            {
                return new PackProgressSnapshotSerializer();
            }
            if (type == typeof(CustomBackgroundWorker))
            {
                return new CustomBackgroundWorkerSerializer();
            }
            if (type == typeof(EventBackgroundWorker))
            {
                return new EventBackgroundWorkerSerializer();
            }
            if (type == typeof(CopyDestinationItem))
            {
                return new CopyDestinationItemSerializer();
            }
            if (type == typeof(MessageDialogResult))
            {
                return new MessageDialogResultSerializer();
            }
            if (type == typeof(DoubleClickAction))
            {
                return new DoubleClickActionSerializer();
            }
            if (type == typeof(QuickFindOptions))
            {
                return new QuickFindOptionsSerializer();
            }
            if (type == typeof(VirtualFilePanel.ListViewSort))
            {
                return new ListViewSortSerializer();
            }
            if (type == typeof(CustomAsyncFolder))
            {
                return new CustomAsyncFolderSerializer();
            }
            if (type == typeof(SearchFolderOptions))
            {
                return new SearchFolderOptionsSerializer();
            }
            if (type == typeof(FindDuplicateOptions))
            {
                return new FindDuplicateOptionsSerializer();
            }
            if (type == typeof(Compare))
            {
                return new CompareSerializer();
            }
            if (type == typeof(ChangeItemAction))
            {
                return new ChangeItemActionSerializer();
            }
            if (type == typeof(AvailableItemActions))
            {
                return new AvailableItemActionsSerializer();
            }
            if (type == typeof(CompareFoldersOptions))
            {
                return new CompareFoldersOptionsSerializer();
            }
            if (type == typeof(OverwriteDialogResult))
            {
                return new OverwriteDialogResultSerializer();
            }
            if (type == typeof(CopyWorkerOptions))
            {
                return new CopyWorkerOptionsSerializer();
            }
            if (type == typeof(CopyMode))
            {
                return new CopyModeSerializer();
            }
            if (type == typeof(ProcessedSize))
            {
                return new ProcessedSizeSerializer();
            }
            if (type == typeof(CopyProgressSnapshot))
            {
                return new CopyProgressSnapshotSerializer();
            }
            if (type == typeof(IconStyle))
            {
                return new IconStyleSerializer();
            }
            return null;
        }

        public override XmlSerializationReader Reader
        {
            get
            {
                return new XmlSerializationReader1();
            }
        }

        public override Hashtable ReadMethods
        {
            get
            {
                if (this.readMethods == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable["Nomad.ImageProvider::"] = "Read206_ImageProvider";
                    hashtable["Nomad.CustomImageProvider::"] = "Read207_CustomImageProvider";
                    hashtable["Nomad.Commons.KeysConverter2::"] = "Read208_KeysConverter2";
                    hashtable["Nomad.Commons.PropertyTagType::"] = "Read209_PropertyTagType";
                    hashtable["Nomad.Commons.PropertyTag::"] = "Read210_PropertyTag";
                    hashtable["Nomad.Commons.LightSource::"] = "Read211_LightSource";
                    hashtable["Nomad.Controls.ToolStripButtonRenderer::"] = "Read212_ToolStripButtonRenderer";
                    hashtable["Nomad.Configuration.ConfigurableSettingsProvider::"] = "Read213_ConfigurableSettingsProvider";
                    hashtable["Nomad.Configuration.ReleaseType::"] = "Read214_ReleaseType";
                    hashtable["Nomad.Configuration.ListViewColumnInfo::"] = "Read215_ListViewColumnInfo";
                    hashtable["Nomad.Configuration.ListViewColumnCollection::"] = "Read216_ArrayOfListViewColumnInfo";
                    hashtable["Nomad.Configuration.PanelLayoutEntry::"] = "Read217_PanelLayoutEntry";
                    hashtable["Nomad.Configuration.PanelToolbar::"] = "Read218_PanelToolbar";
                    hashtable["Nomad.Configuration.PanelLayout::"] = "Read219_PanelLayout";
                    hashtable["Nomad.Configuration.ActivePanel::"] = "Read220_ActivePanel";
                    hashtable["Nomad.Configuration.TwoPanelLayoutEntry::"] = "Read221_TwoPanelLayoutEntry";
                    hashtable["Nomad.Configuration.TwoPanelLayout::"] = "Read222_TwoPanelLayout";
                    hashtable["Nomad.Controls.Actions.CustomActionLink::"] = "Read223_CustomActionLink";
                    hashtable["Nomad.Controls.Actions.CustomBindActionLink::"] = "Read224_CustomBindActionLink";
                    hashtable["Nomad.Controls.Actions.ActionState::"] = "Read225_ActionState";
                    hashtable["Nomad.Controls.Actions.BindActionProperty::"] = "Read226_BindActionProperty";
                    hashtable["Nomad.Controls.Specialized.BreadcrumbView::"] = "Read227_BreadcrumbView";
                    hashtable["Nomad.Commons.Controls.BreadcrumbToolStripRenderer::"] = "Read228_BreadcrumbToolStripRenderer";
                    hashtable["Nomad.Dialogs.InputDialogOption::"] = "Read229_InputDialogOption";
                    hashtable["Nomad.ElevatedProcess::"] = "Read230_ElevatedProcess";
                    hashtable["Nomad.FileSystem.Archive.ArchiveFormatConverter::"] = "Read231_ArchiveFormatConverter";
                    hashtable["Nomad.FileSystem.Archive.Common.ArchiveFormatCapabilities::"] = "Read232_ArchiveFormatCapabilities";
                    hashtable["Nomad.FileSystem.Archive.Common.ArchiveFormatInfo::"] = "Read233_ArchiveFormatInfo";
                    hashtable["Nomad.FileSystem.Archive.Common.PersistArchiveFormatInfo::"] = "Read234_PersistArchiveFormatInfo";
                    hashtable["Nomad.FileSystem.Archive.Common.FindFormatSource::"] = "Read235_FindFormatSource";
                    hashtable["Nomad.FileSystem.Archive.Common.ArjHeader::"] = "Read236_ArjHeader";
                    hashtable["Nomad.FileSystem.Archive.Common.ProcessItemEventArgs::"] = "Read237_ProcessItemEventArgs";
                    hashtable["Nomad.FileSystem.Archive.Common.ProcessorState::"] = "Read238_ProcessorState";
                    hashtable["Nomad.FileSystem.Archive.Common.SequenseProcessorType::"] = "Read239_SequenseProcessorType";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_OM::"] = "Read240_PK_OM";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_OPERATION::"] = "Read241_PK_OPERATION";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_CAPS::"] = "Read242_PK_CAPS";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_VOL::"] = "Read243_PK_VOL";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_PACK::"] = "Read244_PK_PACK";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PackDefaultParamStruct::"] = "Read245_PackDefaultParamStruct";
                    hashtable["Nomad.FileSystem.Archive.Wcx.WcxErrors::"] = "Read246_WcxErrors";
                    hashtable["Nomad.DefaultIcon::"] = "Read247_DefaultIcon";
                    hashtable["Nomad.ShellImageProvider::"] = "Read248_ShellImageProvider";
                    hashtable["Nomad.FileSystem.LocalFile.FileSystemItem+ItemCapability::"] = "Read249_ItemCapability";
                    hashtable["Nomad.FileSystem.LocalFile.LocalFileSystemCreator::"] = "Read250_LocalFileSystemCreator";
                    hashtable["Nomad.FileSystem.Network.NetworkFileSystemCreator::"] = "Read251_NetworkFileSystemCreator";
                    hashtable["Nomad.FileSystem.Shell.ShellFileSystemCreator::"] = "Read252_ShellFileSystemCreator";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.ContentFlag::"] = "Read253_ContentFlag";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.ContentDefaultParamStruct::"] = "Read254_ContentDefaultParamStruct";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.tdateformat::"] = "Read255_tdateformat";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.ttimeformat::"] = "Read256_ttimeformat";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.WdxFieldInfo::"] = "Read257_WdxFieldInfo";
                    hashtable["Nomad.FileSystem.Virtual.Filter.AggregatedFilterCondition::"] = "Read258_AggregatedFilterCondition";
                    hashtable["Nomad.FileSystem.Virtual.Filter.AggregatedVirtualItemFilter::"] = "Read259_AggregatedVirtualItemFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.FilterContainer::"] = "Read260_FilterContainer";
                    hashtable["Nomad.FileSystem.Virtual.Filter.NamedFilter::"] = "Read261_NamedFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.FilterHelper::"] = "Read262_FilterHelper";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemNameFilter::"] = "Read263_VirtualItemNameFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemFullNameFilter::"] = "Read264_VirtualItemFullNameFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemAttributeFilter::"] = "Read265_VirtualItemAttributeFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemSizeFilter::"] = "Read266_VirtualItemSizeFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.ItemDateTimePart::"] = "Read267_ItemDateTimePart";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemDateFilter::"] = "Read268_VirtualItemDateFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemTimeFilter::"] = "Read269_VirtualItemTimeFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemContentFilter::"] = "Read270_VirtualItemContentFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemHexContentFilter::"] = "Read271_VirtualItemHexContentFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.NameListCondition::"] = "Read272_NameListCondition";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemNameListFilter::"] = "Read273_VirtualItemNameListFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualPropertyFilter::"] = "Read274_VirtualPropertyFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualHighligher::"] = "Read275_VirtualHighligher";
                    hashtable["Nomad.FileSystem.Virtual.Filter.ListViewHighlighter::"] = "Read276_ListViewHighlighter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.HighlighterIconType::"] = "Read277_HighlighterIconType";
                    hashtable["Nomad.FileSystem.Property.Providers.HashPropertyProvider::"] = "Read278_HashPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.VistaThumbnailProvider::"] = "Read279_VistaThumbnailProvider";
                    hashtable["Nomad.FileSystem.Virtual.CustomizeFolderParts::"] = "Read280_CustomizeFolderParts";
                    hashtable["Nomad.FileSystem.Property.ColorSpace::"] = "Read281_ColorSpace";
                    hashtable["Nomad.FileSystem.Property.Providers.DescriptionPropertyProvider::"] = "Read282_DescriptionPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.DummyClientSite::"] = "Read283_DummyClientSite";
                    hashtable["Nomad.FileSystem.Property.Providers.HtmlPropertyProvider::"] = "Read284_HtmlPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.BitrateTypeConverter::"] = "Read285_BitrateTypeConverter";
                    hashtable["Nomad.FileSystem.Property.AudioChannelsTypeConverter::"] = "Read286_AudioChannelsTypeConverter";
                    hashtable["Nomad.FileSystem.Property.AudioSampleRateTypeConverter::"] = "Read287_AudioSampleRateTypeConverter";
                    hashtable["Nomad.FileSystem.Property.DurationTypeConverter::"] = "Read288_DurationTypeConverter";
                    hashtable["Nomad.FileSystem.Property.ImageSizeTypeConverter::"] = "Read289_ImageSizeTypeConverter";
                    hashtable["Nomad.FileSystem.Property.DPITypeConverter::"] = "Read290_DPITypeConverter";
                    hashtable["Nomad.FileSystem.Property.ISOSpeedTypeConverter::"] = "Read291_ISOSpeedTypeConverter";
                    hashtable["Nomad.FileSystem.Property.RatingTypeConverter::"] = "Read292_RatingTypeConverter";
                    hashtable["Nomad.FileSystem.Property.EncodingConveter::"] = "Read293_EncodingConveter";
                    hashtable["Nomad.FileSystem.Property.Providers.ImagePropertyProvider::"] = "Read294_ImagePropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.PsdPropertyProvider::"] = "Read295_PsdPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.TagLibPropertyProvider::"] = "Read296_TagLibPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.TextPropertyProvider::"] = "Read297_TextPropertyProvider";
                    hashtable["Nomad.FileSystem.Virtual.VirtualToolTip::"] = "Read298_VirtualToolTip";
                    hashtable["Nomad.Controls.ThrobberStyle::"] = "Read299_ThrobberStyle";
                    hashtable["Nomad.Controls.ThrobberRenderer::"] = "Read300_ThrobberRenderer";
                    hashtable["Nomad.FileSystem.LocalFile.AutoRefreshMode::"] = "Read301_AutoRefreshMode";
                    hashtable["Nomad.FileSystem.Ftp.FtpFileSystemCreator::"] = "Read302_FtpFileSystemCreator";
                    hashtable["Nomad.FileSystem.Null.NullFileSystemCreator::"] = "Read303_NullFileSystemCreator";
                    hashtable["Nomad.FileSystem.Virtual.CustomVirtualFolder::"] = "Read304_CustomVirtualFolder";
                    hashtable["Nomad.FileSystem.Virtual.CanMoveResult::"] = "Read305_CanMoveResult";
                    hashtable["Nomad.FileSystem.Virtual.IconOptions::"] = "Read306_IconOptions";
                    hashtable["Nomad.FileSystem.Virtual.DelayedExtractMode::"] = "Read307_DelayedExtractMode";
                    hashtable["Nomad.FileSystem.Virtual.PathView::"] = "Read308_PathView";
                    hashtable["Nomad.FileSystem.Virtual.PanelView::"] = "Read309_PanelView";
                    hashtable["Nomad.FileSystem.Virtual.ContextMenuOptions::"] = "Read310_ContextMenuOptions";
                    hashtable["Nomad.FileSystem.Virtual.VirtualIcon::"] = "Read311_VirtualIcon";
                    hashtable["Nomad.Controls.PropertyValueList::"] = "Read312_ArrayOfPropertyValue";
                    hashtable["Nomad.Controls.PropertyValue::"] = "Read313_PropertyValue";
                    hashtable["Nomad.Commons.SimpleEncrypt::"] = "Read314_SimpleEncrypt";
                    hashtable["Nomad.Controls.ProgressRenderMode::"] = "Read315_ProgressRenderMode";
                    hashtable["Nomad.Controls.ProgressState::"] = "Read316_ProgressState";
                    hashtable["Nomad.Controls.VistaProgressBarRenderer::"] = "Read317_VistaProgressBarRenderer";
                    hashtable["Nomad.Controls.MarqueeStyle::"] = "Read318_MarqueeStyle";
                    hashtable["Nomad.Controls.XPProgressBarRenderer::"] = "Read319_XPProgressBarRenderer";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.AskMode::"] = "Read320_AskMode";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.OperationResult::"] = "Read321_OperationResult";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.ItemPropId::"] = "Read322_ItemPropId";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.FileTimeType::"] = "Read323_FileTimeType";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.ArchivePropId::"] = "Read324_ArchivePropId";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.KnownSevenZipFormat::"] = "Read325_KnownSevenZipFormat";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.SevenZipFormatCapabilities::"] = "Read326_SevenZipFormatCapabilities";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.CompressionLevel::"] = "Read327_CompressionLevel";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.CompressionMethod::"] = "Read328_CompressionMethod";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.EncryptionMethod::"] = "Read329_EncryptionMethod";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.SevenZipPropertiesBuilder+SolidSizeUnit::"] = "Read330_SolidSizeUnit";
                    hashtable["Nomad.Controls.Filter.ComplexFilterView::"] = "Read331_ComplexFilterView";
                    hashtable["Nomad.Controls.Filter.ViewFilters::"] = "Read332_ViewFilters";
                    hashtable["Nomad.Configuration.PanelContentContainer::"] = "Read333_PanelContentContainer";
                    hashtable["Nomad.ControllerType::"] = "Read334_ControllerType";
                    hashtable["Nomad.Controller::"] = "Read335_Controller";
                    hashtable["Nomad.Configuration.FormPlacement::"] = "Read336_FormPlacement";
                    hashtable["Nomad.ArgumentKey::"] = "Read337_ArgumentKey";
                    hashtable["System.Windows.Forms.CanMoveListViewItem::"] = "Read338_CanMoveListViewItem";
                    hashtable["Nomad.Trace::"] = "Read339_Trace";
                    hashtable["Nomad.TwoPanelContainer+SinglePanel::"] = "Read340_SinglePanel";
                    hashtable["Nomad.GeneralTab::"] = "Read341_GeneralTab";
                    hashtable["Nomad.TwoPanelTab::"] = "Read342_TwoPanelTab";
                    hashtable["Nomad.Workers.ArchiveUpdateMode::"] = "Read343_ArchiveUpdateMode";
                    hashtable["Nomad.Workers.PackStage::"] = "Read344_PackStage";
                    hashtable["Nomad.Workers.PackProgressSnapshot::"] = "Read345_PackProgressSnapshot";
                    hashtable["Nomad.CustomBackgroundWorker::"] = "Read346_CustomBackgroundWorker";
                    hashtable["Nomad.EventBackgroundWorker::"] = "Read347_EventBackgroundWorker";
                    hashtable["Nomad.Dialogs.CopyDestinationItem::"] = "Read348_CopyDestinationItem";
                    hashtable["Nomad.Dialogs.MessageDialogResult::"] = "Read349_MessageDialogResult";
                    hashtable["Nomad.DoubleClickAction::"] = "Read350_DoubleClickAction";
                    hashtable["Nomad.QuickFindOptions::"] = "Read351_QuickFindOptions";
                    hashtable["Nomad.VirtualFilePanel+ListViewSort::"] = "Read352_ListViewSort";
                    hashtable["Nomad.FileSystem.Virtual.CustomAsyncFolder::"] = "Read353_CustomAsyncFolder";
                    hashtable["Nomad.FileSystem.Virtual.SearchFolderOptions::"] = "Read354_SearchFolderOptions";
                    hashtable["Nomad.FileSystem.Virtual.FindDuplicateOptions::"] = "Read355_FindDuplicateOptions";
                    hashtable["Nomad.Workers.Compare::"] = "Read356_Compare";
                    hashtable["Nomad.ChangeItemAction::"] = "Read357_ChangeItemAction";
                    hashtable["Nomad.AvailableItemActions::"] = "Read358_AvailableItemActions";
                    hashtable["Nomad.Workers.CompareFoldersOptions::"] = "Read359_CompareFoldersOptions";
                    hashtable["Nomad.Workers.OverwriteDialogResult::"] = "Read360_OverwriteDialogResult";
                    hashtable["Nomad.Workers.CopyWorkerOptions::"] = "Read361_CopyWorkerOptions";
                    hashtable["Nomad.Workers.CopyMode::"] = "Read362_CopyMode";
                    hashtable["Nomad.Workers.ProcessedSize::"] = "Read363_ProcessedSize";
                    hashtable["Nomad.Workers.CopyProgressSnapshot::"] = "Read364_CopyProgressSnapshot";
                    hashtable["Nomad.FileSystem.Virtual.IconStyle::"] = "Read365_IconStyle";
                    if (this.readMethods == null)
                    {
                        this.readMethods = hashtable;
                    }
                }
                return this.readMethods;
            }
        }

        public override Hashtable TypedSerializers
        {
            get
            {
                if (this.typedSerializers == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("Nomad.Commons.Controls.BreadcrumbToolStripRenderer::", new BreadcrumbToolStripRendererSerializer());
                    hashtable.Add("Nomad.ShellImageProvider::", new ShellImageProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.KnownSevenZipFormat::", new KnownSevenZipFormatSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.ImageSizeTypeConverter::", new ImageSizeTypeConverterSerializer());
                    hashtable.Add("Nomad.Commons.PropertyTagType::", new PropertyTagTypeSerializer());
                    hashtable.Add("Nomad.AvailableItemActions::", new AvailableItemActionsSerializer());
                    hashtable.Add("Nomad.Workers.ArchiveUpdateMode::", new ArchiveUpdateModeSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.Wdx.ContentFlag::", new ContentFlagSerializer());
                    hashtable.Add("Nomad.Trace::", new TraceSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.PK_VOL::", new PK_VOLSerializer());
                    hashtable.Add("Nomad.VirtualFilePanel+ListViewSort::", new ListViewSortSerializer());
                    hashtable.Add("Nomad.Controls.ProgressRenderMode::", new ProgressRenderModeSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.ArchiveFormatCapabilities::", new ArchiveFormatCapabilitiesSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.HighlighterIconType::", new HighlighterIconTypeSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemContentFilter::", new VirtualItemContentFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.PanelView::", new PanelViewSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemAttributeFilter::", new VirtualItemAttributeFilterSerializer());
                    hashtable.Add("Nomad.Configuration.PanelLayout::", new PanelLayoutSerializer());
                    hashtable.Add("Nomad.Commons.LightSource::", new LightSourceSerializer());
                    hashtable.Add("Nomad.Controls.ThrobberRenderer::", new ThrobberRendererSerializer());
                    hashtable.Add("Nomad.Workers.CompareFoldersOptions::", new CompareFoldersOptionsSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.CanMoveResult::", new CanMoveResultSerializer());
                    hashtable.Add("Nomad.Configuration.PanelContentContainer::", new PanelContentContainerSerializer());
                    hashtable.Add("Nomad.Dialogs.CopyDestinationItem::", new CopyDestinationItemSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.Wdx.ttimeformat::", new ttimeformatSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.DescriptionPropertyProvider::", new DescriptionPropertyProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.CustomizeFolderParts::", new CustomizeFolderPartsSerializer());
                    hashtable.Add("Nomad.GeneralTab::", new GeneralTabSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemNameFilter::", new VirtualItemNameFilterSerializer());
                    hashtable.Add("Nomad.Commons.SimpleEncrypt::", new SimpleEncryptSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.AudioSampleRateTypeConverter::", new AudioSampleRateTypeConverterSerializer());
                    hashtable.Add("Nomad.Configuration.TwoPanelLayout::", new TwoPanelLayoutSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemFullNameFilter::", new VirtualItemFullNameFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.SevenZipFormatCapabilities::", new SevenZipFormatCapabilitiesSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.AggregatedVirtualItemFilter::", new AggregatedVirtualItemFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.DelayedExtractMode::", new DelayedExtractModeSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.DPITypeConverter::", new DPITypeConverterSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.IconOptions::", new IconOptionsSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.DurationTypeConverter::", new DurationTypeConverterSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.PK_OPERATION::", new PK_OPERATIONSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.PackDefaultParamStruct::", new PackDefaultParamStructSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.Wdx.WdxFieldInfo::", new WdxFieldInfoSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.PsdPropertyProvider::", new PsdPropertyProviderSerializer());
                    hashtable.Add("Nomad.Workers.CopyWorkerOptions::", new CopyWorkerOptionsSerializer());
                    hashtable.Add("Nomad.Controls.ToolStripButtonRenderer::", new ToolStripButtonRendererSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.OperationResult::", new OperationResultSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.HtmlPropertyProvider::", new HtmlPropertyProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.FindFormatSource::", new FindFormatSourceSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.TagLibPropertyProvider::", new TagLibPropertyProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.Wdx.tdateformat::", new tdateformatSerializer());
                    hashtable.Add("Nomad.Controller::", new ControllerSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.ColorSpace::", new ColorSpaceSerializer());
                    hashtable.Add("Nomad.TwoPanelContainer+SinglePanel::", new SinglePanelSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.ArchiveFormatInfo::", new ArchiveFormatInfoSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.SevenZipPropertiesBuilder+SolidSizeUnit::", new SolidSizeUnitSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.ArchiveFormatConverter::", new ArchiveFormatConverterSerializer());
                    hashtable.Add("Nomad.Configuration.PanelToolbar::", new PanelToolbarSerializer());
                    hashtable.Add("Nomad.FileSystem.Ftp.FtpFileSystemCreator::", new FtpFileSystemCreatorSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemHexContentFilter::", new VirtualItemHexContentFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.DummyClientSite::", new DummyClientSiteSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.AudioChannelsTypeConverter::", new AudioChannelsTypeConverterSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.ItemPropId::", new ItemPropIdSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.PK_OM::", new PK_OMSerializer());
                    hashtable.Add("Nomad.Commons.KeysConverter2::", new KeysConverter2Serializer());
                    hashtable.Add("Nomad.Workers.Compare::", new CompareSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.SearchFolderOptions::", new SearchFolderOptionsSerializer());
                    hashtable.Add("Nomad.Controls.Actions.ActionState::", new ActionStateSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.HashPropertyProvider::", new HashPropertyProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemNameListFilter::", new VirtualItemNameListFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.LocalFile.LocalFileSystemCreator::", new LocalFileSystemCreatorSerializer());
                    hashtable.Add("Nomad.Workers.PackStage::", new PackStageSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.NamedFilter::", new NamedFilterSerializer());
                    hashtable.Add("Nomad.Configuration.PanelLayoutEntry::", new PanelLayoutEntrySerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.SequenseProcessorType::", new SequenseProcessorTypeSerializer());
                    hashtable.Add("Nomad.ChangeItemAction::", new ChangeItemActionSerializer());
                    hashtable.Add("Nomad.FileSystem.Shell.ShellFileSystemCreator::", new ShellFileSystemCreatorSerializer());
                    hashtable.Add("Nomad.Controls.Filter.ComplexFilterView::", new ComplexFilterViewSerializer());
                    hashtable.Add("Nomad.Dialogs.InputDialogOption::", new InputDialogOptionSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.ContextMenuOptions::", new ContextMenuOptionsSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.ItemDateTimePart::", new ItemDateTimePartSerializer());
                    hashtable.Add("Nomad.Configuration.ReleaseType::", new ReleaseTypeSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.PK_CAPS::", new PK_CAPSSerializer());
                    hashtable.Add("Nomad.Workers.PackProgressSnapshot::", new PackProgressSnapshotSerializer());
                    hashtable.Add("Nomad.Controls.MarqueeStyle::", new MarqueeStyleSerializer());
                    hashtable.Add("Nomad.ControllerType::", new ControllerTypeSerializer());
                    hashtable.Add("Nomad.FileSystem.LocalFile.AutoRefreshMode::", new AutoRefreshModeSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualHighligher::", new VirtualHighligherSerializer());
                    hashtable.Add("Nomad.CustomImageProvider::", new CustomImageProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.WcxErrors::", new WcxErrorsSerializer());
                    hashtable.Add("Nomad.Controls.PropertyValue::", new PropertyValueSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.CompressionMethod::", new CompressionMethodSerializer());
                    hashtable.Add("Nomad.DoubleClickAction::", new DoubleClickActionSerializer());
                    hashtable.Add("Nomad.ImageProvider::", new ImageProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.CompressionLevel::", new CompressionLevelSerializer());
                    hashtable.Add("Nomad.Controls.VistaProgressBarRenderer::", new VistaProgressBarRendererSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.TextPropertyProvider::", new TextPropertyProviderSerializer());
                    hashtable.Add("Nomad.Controls.Specialized.BreadcrumbView::", new BreadcrumbViewSerializer());
                    hashtable.Add("Nomad.Controls.Actions.CustomActionLink::", new CustomActionLinkSerializer());
                    hashtable.Add("Nomad.Commons.PropertyTag::", new PropertyTagSerializer());
                    hashtable.Add("Nomad.ArgumentKey::", new ArgumentKeySerializer());
                    hashtable.Add("Nomad.ElevatedProcess::", new ElevatedProcessSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.PathView::", new PathViewSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.FileTimeType::", new FileTimeTypeSerializer());
                    hashtable.Add("Nomad.Controls.ProgressState::", new ProgressStateSerializer());
                    hashtable.Add("Nomad.Controls.ThrobberStyle::", new ThrobberStyleSerializer());
                    hashtable.Add("Nomad.FileSystem.Network.NetworkFileSystemCreator::", new NetworkFileSystemCreatorSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.NameListCondition::", new NameListConditionSerializer());
                    hashtable.Add("Nomad.Controls.Filter.ViewFilters::", new ViewFiltersSerializer());
                    hashtable.Add("Nomad.Controls.Actions.BindActionProperty::", new BindActionPropertySerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.FilterContainer::", new FilterContainerSerializer());
                    hashtable.Add("Nomad.Controls.Actions.CustomBindActionLink::", new CustomBindActionLinkSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.ArchivePropId::", new ArchivePropIdSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.AskMode::", new AskModeSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.ListViewHighlighter::", new ListViewHighlighterSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.ISOSpeedTypeConverter::", new ISOSpeedTypeConverterSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.CustomVirtualFolder::", new CustomVirtualFolderSerializer());
                    hashtable.Add("Nomad.Configuration.ConfigurableSettingsProvider::", new ConfigurableSettingsProviderSerializer());
                    hashtable.Add("Nomad.Configuration.TwoPanelLayoutEntry::", new TwoPanelLayoutEntrySerializer());
                    hashtable.Add("Nomad.Workers.CopyProgressSnapshot::", new CopyProgressSnapshotSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.ArjHeader::", new ArjHeaderSerializer());
                    hashtable.Add("Nomad.Configuration.FormPlacement::", new FormPlacementSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.EncodingConveter::", new EncodingConveterSerializer());
                    hashtable.Add("Nomad.Workers.ProcessedSize::", new ProcessedSizeSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.VirtualToolTip::", new VirtualToolTipSerializer());
                    hashtable.Add("Nomad.Dialogs.MessageDialogResult::", new MessageDialogResultSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.FindDuplicateOptions::", new FindDuplicateOptionsSerializer());
                    hashtable.Add("Nomad.FileSystem.Null.NullFileSystemCreator::", new NullFileSystemCreatorSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemDateFilter::", new VirtualItemDateFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.ProcessorState::", new ProcessorStateSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.PersistArchiveFormatInfo::", new PersistArchiveFormatInfoSerializer());
                    hashtable.Add("System.Windows.Forms.CanMoveListViewItem::", new CanMoveListViewItemSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Wcx.PK_PACK::", new PK_PACKSerializer());
                    hashtable.Add("Nomad.Workers.CopyMode::", new CopyModeSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.AggregatedFilterCondition::", new AggregatedFilterConditionSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.FilterHelper::", new FilterHelperSerializer());
                    hashtable.Add("Nomad.Configuration.ListViewColumnInfo::", new ListViewColumnInfoSerializer());
                    hashtable.Add("Nomad.Configuration.ListViewColumnCollection::", new ListViewColumnCollectionSerializer());
                    hashtable.Add("Nomad.FileSystem.LocalFile.FileSystemItem+ItemCapability::", new ItemCapabilitySerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemSizeFilter::", new VirtualItemSizeFilterSerializer());
                    hashtable.Add("Nomad.Configuration.ActivePanel::", new ActivePanelSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualPropertyFilter::", new VirtualPropertyFilterSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.Common.ProcessItemEventArgs::", new ProcessItemEventArgsSerializer());
                    hashtable.Add("Nomad.Workers.OverwriteDialogResult::", new OverwriteDialogResultSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.Filter.VirtualItemTimeFilter::", new VirtualItemTimeFilterSerializer());
                    hashtable.Add("Nomad.TwoPanelTab::", new TwoPanelTabSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.Wdx.ContentDefaultParamStruct::", new ContentDefaultParamStructSerializer());
                    hashtable.Add("Nomad.CustomBackgroundWorker::", new CustomBackgroundWorkerSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.VistaThumbnailProvider::", new VistaThumbnailProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.CustomAsyncFolder::", new CustomAsyncFolderSerializer());
                    hashtable.Add("Nomad.DefaultIcon::", new DefaultIconSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.IconStyle::", new IconStyleSerializer());
                    hashtable.Add("Nomad.FileSystem.Virtual.VirtualIcon::", new VirtualIconSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.Providers.ImagePropertyProvider::", new ImagePropertyProviderSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.RatingTypeConverter::", new RatingTypeConverterSerializer());
                    hashtable.Add("Nomad.FileSystem.Archive.SevenZip.EncryptionMethod::", new EncryptionMethodSerializer());
                    hashtable.Add("Nomad.EventBackgroundWorker::", new EventBackgroundWorkerSerializer());
                    hashtable.Add("Nomad.Controls.PropertyValueList::", new PropertyValueListSerializer());
                    hashtable.Add("Nomad.FileSystem.Property.BitrateTypeConverter::", new BitrateTypeConverterSerializer());
                    hashtable.Add("Nomad.QuickFindOptions::", new QuickFindOptionsSerializer());
                    hashtable.Add("Nomad.Controls.XPProgressBarRenderer::", new XPProgressBarRendererSerializer());
                    if (this.typedSerializers == null)
                    {
                        this.typedSerializers = hashtable;
                    }
                }
                return this.typedSerializers;
            }
        }

        public override Hashtable WriteMethods
        {
            get
            {
                if (this.writeMethods == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable["Nomad.ImageProvider::"] = "Write206_ImageProvider";
                    hashtable["Nomad.CustomImageProvider::"] = "Write207_CustomImageProvider";
                    hashtable["Nomad.Commons.KeysConverter2::"] = "Write208_KeysConverter2";
                    hashtable["Nomad.Commons.PropertyTagType::"] = "Write209_PropertyTagType";
                    hashtable["Nomad.Commons.PropertyTag::"] = "Write210_PropertyTag";
                    hashtable["Nomad.Commons.LightSource::"] = "Write211_LightSource";
                    hashtable["Nomad.Controls.ToolStripButtonRenderer::"] = "Write212_ToolStripButtonRenderer";
                    hashtable["Nomad.Configuration.ConfigurableSettingsProvider::"] = "Write213_ConfigurableSettingsProvider";
                    hashtable["Nomad.Configuration.ReleaseType::"] = "Write214_ReleaseType";
                    hashtable["Nomad.Configuration.ListViewColumnInfo::"] = "Write215_ListViewColumnInfo";
                    hashtable["Nomad.Configuration.ListViewColumnCollection::"] = "Write216_ArrayOfListViewColumnInfo";
                    hashtable["Nomad.Configuration.PanelLayoutEntry::"] = "Write217_PanelLayoutEntry";
                    hashtable["Nomad.Configuration.PanelToolbar::"] = "Write218_PanelToolbar";
                    hashtable["Nomad.Configuration.PanelLayout::"] = "Write219_PanelLayout";
                    hashtable["Nomad.Configuration.ActivePanel::"] = "Write220_ActivePanel";
                    hashtable["Nomad.Configuration.TwoPanelLayoutEntry::"] = "Write221_TwoPanelLayoutEntry";
                    hashtable["Nomad.Configuration.TwoPanelLayout::"] = "Write222_TwoPanelLayout";
                    hashtable["Nomad.Controls.Actions.CustomActionLink::"] = "Write223_CustomActionLink";
                    hashtable["Nomad.Controls.Actions.CustomBindActionLink::"] = "Write224_CustomBindActionLink";
                    hashtable["Nomad.Controls.Actions.ActionState::"] = "Write225_ActionState";
                    hashtable["Nomad.Controls.Actions.BindActionProperty::"] = "Write226_BindActionProperty";
                    hashtable["Nomad.Controls.Specialized.BreadcrumbView::"] = "Write227_BreadcrumbView";
                    hashtable["Nomad.Commons.Controls.BreadcrumbToolStripRenderer::"] = "Write228_BreadcrumbToolStripRenderer";
                    hashtable["Nomad.Dialogs.InputDialogOption::"] = "Write229_InputDialogOption";
                    hashtable["Nomad.ElevatedProcess::"] = "Write230_ElevatedProcess";
                    hashtable["Nomad.FileSystem.Archive.ArchiveFormatConverter::"] = "Write231_ArchiveFormatConverter";
                    hashtable["Nomad.FileSystem.Archive.Common.ArchiveFormatCapabilities::"] = "Write232_ArchiveFormatCapabilities";
                    hashtable["Nomad.FileSystem.Archive.Common.ArchiveFormatInfo::"] = "Write233_ArchiveFormatInfo";
                    hashtable["Nomad.FileSystem.Archive.Common.PersistArchiveFormatInfo::"] = "Write234_PersistArchiveFormatInfo";
                    hashtable["Nomad.FileSystem.Archive.Common.FindFormatSource::"] = "Write235_FindFormatSource";
                    hashtable["Nomad.FileSystem.Archive.Common.ArjHeader::"] = "Write236_ArjHeader";
                    hashtable["Nomad.FileSystem.Archive.Common.ProcessItemEventArgs::"] = "Write237_ProcessItemEventArgs";
                    hashtable["Nomad.FileSystem.Archive.Common.ProcessorState::"] = "Write238_ProcessorState";
                    hashtable["Nomad.FileSystem.Archive.Common.SequenseProcessorType::"] = "Write239_SequenseProcessorType";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_OM::"] = "Write240_PK_OM";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_OPERATION::"] = "Write241_PK_OPERATION";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_CAPS::"] = "Write242_PK_CAPS";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_VOL::"] = "Write243_PK_VOL";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PK_PACK::"] = "Write244_PK_PACK";
                    hashtable["Nomad.FileSystem.Archive.Wcx.PackDefaultParamStruct::"] = "Write245_PackDefaultParamStruct";
                    hashtable["Nomad.FileSystem.Archive.Wcx.WcxErrors::"] = "Write246_WcxErrors";
                    hashtable["Nomad.DefaultIcon::"] = "Write247_DefaultIcon";
                    hashtable["Nomad.ShellImageProvider::"] = "Write248_ShellImageProvider";
                    hashtable["Nomad.FileSystem.LocalFile.FileSystemItem+ItemCapability::"] = "Write249_ItemCapability";
                    hashtable["Nomad.FileSystem.LocalFile.LocalFileSystemCreator::"] = "Write250_LocalFileSystemCreator";
                    hashtable["Nomad.FileSystem.Network.NetworkFileSystemCreator::"] = "Write251_NetworkFileSystemCreator";
                    hashtable["Nomad.FileSystem.Shell.ShellFileSystemCreator::"] = "Write252_ShellFileSystemCreator";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.ContentFlag::"] = "Write253_ContentFlag";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.ContentDefaultParamStruct::"] = "Write254_ContentDefaultParamStruct";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.tdateformat::"] = "Write255_tdateformat";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.ttimeformat::"] = "Write256_ttimeformat";
                    hashtable["Nomad.FileSystem.Property.Providers.Wdx.WdxFieldInfo::"] = "Write257_WdxFieldInfo";
                    hashtable["Nomad.FileSystem.Virtual.Filter.AggregatedFilterCondition::"] = "Write258_AggregatedFilterCondition";
                    hashtable["Nomad.FileSystem.Virtual.Filter.AggregatedVirtualItemFilter::"] = "Write259_AggregatedVirtualItemFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.FilterContainer::"] = "Write260_FilterContainer";
                    hashtable["Nomad.FileSystem.Virtual.Filter.NamedFilter::"] = "Write261_NamedFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.FilterHelper::"] = "Write262_FilterHelper";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemNameFilter::"] = "Write263_VirtualItemNameFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemFullNameFilter::"] = "Write264_VirtualItemFullNameFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemAttributeFilter::"] = "Write265_VirtualItemAttributeFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemSizeFilter::"] = "Write266_VirtualItemSizeFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.ItemDateTimePart::"] = "Write267_ItemDateTimePart";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemDateFilter::"] = "Write268_VirtualItemDateFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemTimeFilter::"] = "Write269_VirtualItemTimeFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemContentFilter::"] = "Write270_VirtualItemContentFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemHexContentFilter::"] = "Write271_VirtualItemHexContentFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.NameListCondition::"] = "Write272_NameListCondition";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualItemNameListFilter::"] = "Write273_VirtualItemNameListFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualPropertyFilter::"] = "Write274_VirtualPropertyFilter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.VirtualHighligher::"] = "Write275_VirtualHighligher";
                    hashtable["Nomad.FileSystem.Virtual.Filter.ListViewHighlighter::"] = "Write276_ListViewHighlighter";
                    hashtable["Nomad.FileSystem.Virtual.Filter.HighlighterIconType::"] = "Write277_HighlighterIconType";
                    hashtable["Nomad.FileSystem.Property.Providers.HashPropertyProvider::"] = "Write278_HashPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.VistaThumbnailProvider::"] = "Write279_VistaThumbnailProvider";
                    hashtable["Nomad.FileSystem.Virtual.CustomizeFolderParts::"] = "Write280_CustomizeFolderParts";
                    hashtable["Nomad.FileSystem.Property.ColorSpace::"] = "Write281_ColorSpace";
                    hashtable["Nomad.FileSystem.Property.Providers.DescriptionPropertyProvider::"] = "Write282_DescriptionPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.DummyClientSite::"] = "Write283_DummyClientSite";
                    hashtable["Nomad.FileSystem.Property.Providers.HtmlPropertyProvider::"] = "Write284_HtmlPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.BitrateTypeConverter::"] = "Write285_BitrateTypeConverter";
                    hashtable["Nomad.FileSystem.Property.AudioChannelsTypeConverter::"] = "Write286_AudioChannelsTypeConverter";
                    hashtable["Nomad.FileSystem.Property.AudioSampleRateTypeConverter::"] = "Write287_AudioSampleRateTypeConverter";
                    hashtable["Nomad.FileSystem.Property.DurationTypeConverter::"] = "Write288_DurationTypeConverter";
                    hashtable["Nomad.FileSystem.Property.ImageSizeTypeConverter::"] = "Write289_ImageSizeTypeConverter";
                    hashtable["Nomad.FileSystem.Property.DPITypeConverter::"] = "Write290_DPITypeConverter";
                    hashtable["Nomad.FileSystem.Property.ISOSpeedTypeConverter::"] = "Write291_ISOSpeedTypeConverter";
                    hashtable["Nomad.FileSystem.Property.RatingTypeConverter::"] = "Write292_RatingTypeConverter";
                    hashtable["Nomad.FileSystem.Property.EncodingConveter::"] = "Write293_EncodingConveter";
                    hashtable["Nomad.FileSystem.Property.Providers.ImagePropertyProvider::"] = "Write294_ImagePropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.PsdPropertyProvider::"] = "Write295_PsdPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.TagLibPropertyProvider::"] = "Write296_TagLibPropertyProvider";
                    hashtable["Nomad.FileSystem.Property.Providers.TextPropertyProvider::"] = "Write297_TextPropertyProvider";
                    hashtable["Nomad.FileSystem.Virtual.VirtualToolTip::"] = "Write298_VirtualToolTip";
                    hashtable["Nomad.Controls.ThrobberStyle::"] = "Write299_ThrobberStyle";
                    hashtable["Nomad.Controls.ThrobberRenderer::"] = "Write300_ThrobberRenderer";
                    hashtable["Nomad.FileSystem.LocalFile.AutoRefreshMode::"] = "Write301_AutoRefreshMode";
                    hashtable["Nomad.FileSystem.Ftp.FtpFileSystemCreator::"] = "Write302_FtpFileSystemCreator";
                    hashtable["Nomad.FileSystem.Null.NullFileSystemCreator::"] = "Write303_NullFileSystemCreator";
                    hashtable["Nomad.FileSystem.Virtual.CustomVirtualFolder::"] = "Write304_CustomVirtualFolder";
                    hashtable["Nomad.FileSystem.Virtual.CanMoveResult::"] = "Write305_CanMoveResult";
                    hashtable["Nomad.FileSystem.Virtual.IconOptions::"] = "Write306_IconOptions";
                    hashtable["Nomad.FileSystem.Virtual.DelayedExtractMode::"] = "Write307_DelayedExtractMode";
                    hashtable["Nomad.FileSystem.Virtual.PathView::"] = "Write308_PathView";
                    hashtable["Nomad.FileSystem.Virtual.PanelView::"] = "Write309_PanelView";
                    hashtable["Nomad.FileSystem.Virtual.ContextMenuOptions::"] = "Write310_ContextMenuOptions";
                    hashtable["Nomad.FileSystem.Virtual.VirtualIcon::"] = "Write311_VirtualIcon";
                    hashtable["Nomad.Controls.PropertyValueList::"] = "Write312_ArrayOfPropertyValue";
                    hashtable["Nomad.Controls.PropertyValue::"] = "Write313_PropertyValue";
                    hashtable["Nomad.Commons.SimpleEncrypt::"] = "Write314_SimpleEncrypt";
                    hashtable["Nomad.Controls.ProgressRenderMode::"] = "Write315_ProgressRenderMode";
                    hashtable["Nomad.Controls.ProgressState::"] = "Write316_ProgressState";
                    hashtable["Nomad.Controls.VistaProgressBarRenderer::"] = "Write317_VistaProgressBarRenderer";
                    hashtable["Nomad.Controls.MarqueeStyle::"] = "Write318_MarqueeStyle";
                    hashtable["Nomad.Controls.XPProgressBarRenderer::"] = "Write319_XPProgressBarRenderer";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.AskMode::"] = "Write320_AskMode";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.OperationResult::"] = "Write321_OperationResult";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.ItemPropId::"] = "Write322_ItemPropId";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.FileTimeType::"] = "Write323_FileTimeType";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.ArchivePropId::"] = "Write324_ArchivePropId";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.KnownSevenZipFormat::"] = "Write325_KnownSevenZipFormat";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.SevenZipFormatCapabilities::"] = "Write326_SevenZipFormatCapabilities";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.CompressionLevel::"] = "Write327_CompressionLevel";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.CompressionMethod::"] = "Write328_CompressionMethod";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.EncryptionMethod::"] = "Write329_EncryptionMethod";
                    hashtable["Nomad.FileSystem.Archive.SevenZip.SevenZipPropertiesBuilder+SolidSizeUnit::"] = "Write330_SolidSizeUnit";
                    hashtable["Nomad.Controls.Filter.ComplexFilterView::"] = "Write331_ComplexFilterView";
                    hashtable["Nomad.Controls.Filter.ViewFilters::"] = "Write332_ViewFilters";
                    hashtable["Nomad.Configuration.PanelContentContainer::"] = "Write333_PanelContentContainer";
                    hashtable["Nomad.ControllerType::"] = "Write334_ControllerType";
                    hashtable["Nomad.Controller::"] = "Write335_Controller";
                    hashtable["Nomad.Configuration.FormPlacement::"] = "Write336_FormPlacement";
                    hashtable["Nomad.ArgumentKey::"] = "Write337_ArgumentKey";
                    hashtable["System.Windows.Forms.CanMoveListViewItem::"] = "Write338_CanMoveListViewItem";
                    hashtable["Nomad.Trace::"] = "Write339_Trace";
                    hashtable["Nomad.TwoPanelContainer+SinglePanel::"] = "Write340_SinglePanel";
                    hashtable["Nomad.GeneralTab::"] = "Write341_GeneralTab";
                    hashtable["Nomad.TwoPanelTab::"] = "Write342_TwoPanelTab";
                    hashtable["Nomad.Workers.ArchiveUpdateMode::"] = "Write343_ArchiveUpdateMode";
                    hashtable["Nomad.Workers.PackStage::"] = "Write344_PackStage";
                    hashtable["Nomad.Workers.PackProgressSnapshot::"] = "Write345_PackProgressSnapshot";
                    hashtable["Nomad.CustomBackgroundWorker::"] = "Write346_CustomBackgroundWorker";
                    hashtable["Nomad.EventBackgroundWorker::"] = "Write347_EventBackgroundWorker";
                    hashtable["Nomad.Dialogs.CopyDestinationItem::"] = "Write348_CopyDestinationItem";
                    hashtable["Nomad.Dialogs.MessageDialogResult::"] = "Write349_MessageDialogResult";
                    hashtable["Nomad.DoubleClickAction::"] = "Write350_DoubleClickAction";
                    hashtable["Nomad.QuickFindOptions::"] = "Write351_QuickFindOptions";
                    hashtable["Nomad.VirtualFilePanel+ListViewSort::"] = "Write352_ListViewSort";
                    hashtable["Nomad.FileSystem.Virtual.CustomAsyncFolder::"] = "Write353_CustomAsyncFolder";
                    hashtable["Nomad.FileSystem.Virtual.SearchFolderOptions::"] = "Write354_SearchFolderOptions";
                    hashtable["Nomad.FileSystem.Virtual.FindDuplicateOptions::"] = "Write355_FindDuplicateOptions";
                    hashtable["Nomad.Workers.Compare::"] = "Write356_Compare";
                    hashtable["Nomad.ChangeItemAction::"] = "Write357_ChangeItemAction";
                    hashtable["Nomad.AvailableItemActions::"] = "Write358_AvailableItemActions";
                    hashtable["Nomad.Workers.CompareFoldersOptions::"] = "Write359_CompareFoldersOptions";
                    hashtable["Nomad.Workers.OverwriteDialogResult::"] = "Write360_OverwriteDialogResult";
                    hashtable["Nomad.Workers.CopyWorkerOptions::"] = "Write361_CopyWorkerOptions";
                    hashtable["Nomad.Workers.CopyMode::"] = "Write362_CopyMode";
                    hashtable["Nomad.Workers.ProcessedSize::"] = "Write363_ProcessedSize";
                    hashtable["Nomad.Workers.CopyProgressSnapshot::"] = "Write364_CopyProgressSnapshot";
                    hashtable["Nomad.FileSystem.Virtual.IconStyle::"] = "Write365_IconStyle";
                    if (this.writeMethods == null)
                    {
                        this.writeMethods = hashtable;
                    }
                }
                return this.writeMethods;
            }
        }

        public override XmlSerializationWriter Writer
        {
            get
            {
                return new XmlSerializationWriter1();
            }
        }
    }
}

