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
    using System.ComponentModel;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializationWriter1 : XmlSerializationWriter
    {
        protected override void InitCallbacks()
        {
        }

        private void Write1_Object(string n, string ns, object o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(object))
                    {
                        if (type == typeof(CopyProgressSnapshot))
                        {
                            this.Write204_CopyProgressSnapshot(n, ns, (CopyProgressSnapshot) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomBackgroundWorker))
                        {
                            this.Write188_CustomBackgroundWorker(n, ns, (CustomBackgroundWorker) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(EventBackgroundWorker))
                        {
                            this.Write189_EventBackgroundWorker(n, ns, (EventBackgroundWorker) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomAsyncFolder))
                        {
                            this.Write194_CustomAsyncFolder(n, ns, (CustomAsyncFolder) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PackProgressSnapshot))
                        {
                            this.Write187_PackProgressSnapshot(n, ns, (PackProgressSnapshot) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TimeSpan))
                        {
                            this.Write186_TimeSpan(n, ns, (TimeSpan) o, true);
                            return;
                        }
                        if (type == typeof(ProcessedSize))
                        {
                            this.Write185_ProcessedSize(n, ns, (ProcessedSize) o, true);
                            return;
                        }
                        if (type == typeof(GeneralTab))
                        {
                            this.Write181_GeneralTab(n, ns, (GeneralTab) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TwoPanelTab))
                        {
                            this.Write182_TwoPanelTab(n, ns, (TwoPanelTab) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(Trace))
                        {
                            this.Write179_Trace(n, ns, (Trace) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(MarshalByRefObject))
                        {
                            this.Write174_MarshalByRefObject(n, ns, (MarshalByRefObject) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(Controller))
                        {
                            this.Write175_Controller(n, ns, (Controller) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(XPProgressBarRenderer))
                        {
                            this.Write157_XPProgressBarRenderer(n, ns, (XPProgressBarRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VistaProgressBarRenderer))
                        {
                            this.Write155_VistaProgressBarRenderer(n, ns, (VistaProgressBarRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SimpleEncrypt))
                        {
                            this.Write152_SimpleEncrypt(n, ns, (SimpleEncrypt) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PropertyValue))
                        {
                            this.Write151_PropertyValue(n, ns, (PropertyValue) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualIcon))
                        {
                            this.Write150_VirtualIcon(n, ns, (VirtualIcon) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomVirtualFolder))
                        {
                            this.Write144_CustomVirtualFolder(n, ns, (CustomVirtualFolder) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(NullFileSystemCreator))
                        {
                            this.Write143_NullFileSystemCreator(n, ns, (NullFileSystemCreator) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(FtpFileSystemCreator))
                        {
                            this.Write142_FtpFileSystemCreator(n, ns, (FtpFileSystemCreator) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ThrobberRenderer))
                        {
                            this.Write140_ThrobberRenderer(n, ns, (ThrobberRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(Color))
                        {
                            this.Write139_Color(n, ns, (Color) o, true);
                            return;
                        }
                        if (type == typeof(VirtualToolTip))
                        {
                            this.Write137_VirtualToolTip(n, ns, (VirtualToolTip) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TextPropertyProvider))
                        {
                            this.Write136_TextPropertyProvider(n, ns, (TextPropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TagLibPropertyProvider))
                        {
                            this.Write135_TagLibPropertyProvider(n, ns, (TagLibPropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PsdPropertyProvider))
                        {
                            this.Write134_PsdPropertyProvider(n, ns, (PsdPropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ImagePropertyProvider))
                        {
                            this.Write133_ImagePropertyProvider(n, ns, (ImagePropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(HtmlPropertyProvider))
                        {
                            this.Write122_HtmlPropertyProvider(n, ns, (HtmlPropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DummyClientSite))
                        {
                            this.Write121_DummyClientSite(n, ns, (DummyClientSite) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DescriptionPropertyProvider))
                        {
                            this.Write120_DescriptionPropertyProvider(n, ns, (DescriptionPropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VistaThumbnailProvider))
                        {
                            this.Write117_VistaThumbnailProvider(n, ns, (VistaThumbnailProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(HashPropertyProvider))
                        {
                            this.Write116_HashPropertyProvider(n, ns, (HashPropertyProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(FilterHelper))
                        {
                            this.Write111_FilterHelper(n, ns, (FilterHelper) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(FilterContainer))
                        {
                            this.Write109_FilterContainer(n, ns, (FilterContainer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PanelContentContainer))
                        {
                            this.Write172_PanelContentContainer(n, ns, (PanelContentContainer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(NamedFilter))
                        {
                            this.Write110_NamedFilter(n, ns, (NamedFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualHighligher))
                        {
                            this.Write114_VirtualHighligher(n, ns, (VirtualHighligher) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ListViewHighlighter))
                        {
                            this.Write115_ListViewHighlighter(n, ns, (ListViewHighlighter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(Version))
                        {
                            this.Write74_Version(n, ns, (Version) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(BasicFilter))
                        {
                            this.Write65_BasicFilter(n, ns, (BasicFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemNameListFilter))
                        {
                            this.Write107_VirtualItemNameListFilter(n, ns, (VirtualItemNameListFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(NameFilter))
                        {
                            this.Write102_NameFilter(n, ns, (NameFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemFullNameFilter))
                        {
                            this.Write112_VirtualItemFullNameFilter(n, ns, (VirtualItemFullNameFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemNameFilter))
                        {
                            this.Write103_VirtualItemNameFilter(n, ns, (VirtualItemNameFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(AttributeFilter))
                        {
                            this.Write93_AttributeFilter(n, ns, (AttributeFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemAttributeFilter))
                        {
                            this.Write94_VirtualItemAttributeFilter(n, ns, (VirtualItemAttributeFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomContentFilter))
                        {
                            this.Write85_CustomContentFilter(n, ns, (CustomContentFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(HexContentFilter))
                        {
                            this.Write104_HexContentFilter(n, ns, (HexContentFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemHexContentFilter))
                        {
                            this.Write105_VirtualItemHexContentFilter(n, ns, (VirtualItemHexContentFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ContentFilter))
                        {
                            this.Write86_ContentFilter(n, ns, (ContentFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemContentFilter))
                        {
                            this.Write87_VirtualItemContentFilter(n, ns, (VirtualItemContentFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ValueFilter))
                        {
                            this.Write66_ValueFilter(n, ns, (ValueFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TimeFilter))
                        {
                            this.Write89_TimeFilter(n, ns, (TimeFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemTimeFilter))
                        {
                            this.Write91_VirtualItemTimeFilter(n, ns, (VirtualItemTimeFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SimpleFilter<byte>))
                        {
                            this.Write82_SimpleFilterOfByte(n, ns, (SimpleFilter<byte>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(IntegralFilter<byte>))
                        {
                            this.Write83_IntegralFilterOfByte(n, ns, (IntegralFilter<byte>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SimpleFilter<int>))
                        {
                            this.Write80_SimpleFilterOfInt32(n, ns, (SimpleFilter<int>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(IntegralFilter<int>))
                        {
                            this.Write81_IntegralFilterOfInt32(n, ns, (IntegralFilter<int>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SimpleFilter<uint>))
                        {
                            this.Write78_SimpleFilterOfUInt32(n, ns, (SimpleFilter<uint>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(IntegralFilter<uint>))
                        {
                            this.Write79_IntegralFilterOfUInt32(n, ns, (IntegralFilter<uint>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SimpleFilter<long>))
                        {
                            this.Write76_SimpleFilterOfInt64(n, ns, (SimpleFilter<long>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(IntegralFilter<long>))
                        {
                            this.Write77_IntegralFilterOfInt64(n, ns, (IntegralFilter<long>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SizeFilter))
                        {
                            this.Write97_SizeFilter(n, ns, (SizeFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemSizeFilter))
                        {
                            this.Write98_VirtualItemSizeFilter(n, ns, (VirtualItemSizeFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SimpleFilter<Version>))
                        {
                            this.Write75_SimpleFilterOfVersion(n, ns, (SimpleFilter<Version>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(StringFilter))
                        {
                            this.Write72_StringFilter(n, ns, (StringFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DateFilter))
                        {
                            this.Write69_DateFilter(n, ns, (DateFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualItemDateFilter))
                        {
                            this.Write99_VirtualItemDateFilter(n, ns, (VirtualItemDateFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualPropertyFilter))
                        {
                            this.Write84_VirtualPropertyFilter(n, ns, (VirtualPropertyFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(AggregatedVirtualItemFilter))
                        {
                            this.Write108_AggregatedVirtualItemFilter(n, ns, (AggregatedVirtualItemFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(WdxFieldInfo))
                        {
                            this.Write63_WdxFieldInfo(n, ns, (WdxFieldInfo) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ttimeformat))
                        {
                            this.Write62_ttimeformat(n, ns, (ttimeformat) o, true);
                            return;
                        }
                        if (type == typeof(tdateformat))
                        {
                            this.Write61_tdateformat(n, ns, (tdateformat) o, true);
                            return;
                        }
                        if (type == typeof(ContentDefaultParamStruct))
                        {
                            this.Write60_ContentDefaultParamStruct(n, ns, (ContentDefaultParamStruct) o, true);
                            return;
                        }
                        if (type == typeof(ShellFileSystemCreator))
                        {
                            this.Write58_ShellFileSystemCreator(n, ns, (ShellFileSystemCreator) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(NetworkFileSystemCreator))
                        {
                            this.Write57_NetworkFileSystemCreator(n, ns, (NetworkFileSystemCreator) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(LocalFileSystemCreator))
                        {
                            this.Write56_LocalFileSystemCreator(n, ns, (LocalFileSystemCreator) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(WcxErrors))
                        {
                            this.Write52_WcxErrors(n, ns, (WcxErrors) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PackDefaultParamStruct))
                        {
                            this.Write51_PackDefaultParamStruct(n, ns, (PackDefaultParamStruct) o, true);
                            return;
                        }
                        if (type == typeof(EventArgs))
                        {
                            this.Write41_EventArgs(n, ns, (EventArgs) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CancelEventArgs))
                        {
                            this.Write42_CancelEventArgs(n, ns, (CancelEventArgs) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ProcessItemEventArgs))
                        {
                            this.Write43_ProcessItemEventArgs(n, ns, (ProcessItemEventArgs) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ArjHeader))
                        {
                            this.Write40_ArjHeader(n, ns, (ArjHeader) o, true);
                            return;
                        }
                        if (type == typeof(ArchiveFormatInfo))
                        {
                            this.Write37_ArchiveFormatInfo(n, ns, (ArchiveFormatInfo) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PersistArchiveFormatInfo))
                        {
                            this.Write38_PersistArchiveFormatInfo(n, ns, (PersistArchiveFormatInfo) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ElevatedProcess))
                        {
                            this.Write34_ElevatedProcess(n, ns, (ElevatedProcess) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomActionLink))
                        {
                            this.Write26_CustomActionLink(n, ns, (CustomActionLink) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomBindActionLink))
                        {
                            this.Write27_CustomBindActionLink(n, ns, (CustomBindActionLink) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TwoPanelLayout))
                        {
                            this.Write25_TwoPanelLayout(n, ns, (TwoPanelLayout) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PanelLayout))
                        {
                            this.Write22_PanelLayout(n, ns, (PanelLayout) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ListViewColumnInfo))
                        {
                            this.Write17_ListViewColumnInfo(n, ns, (ListViewColumnInfo) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ProviderBase))
                        {
                            this.Write12_ProviderBase(n, ns, (ProviderBase) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SettingsProvider))
                        {
                            this.Write13_SettingsProvider(n, ns, (SettingsProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ConfigurableSettingsProvider))
                        {
                            this.Write14_ConfigurableSettingsProvider(n, ns, (ConfigurableSettingsProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ToolStripRenderer))
                        {
                            this.Write10_ToolStripRenderer(n, ns, (ToolStripRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ToolStripWrapperRenderer))
                        {
                            this.Write31_ToolStripWrapperRenderer(n, ns, (ToolStripWrapperRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(BreadcrumbToolStripRenderer))
                        {
                            this.Write32_BreadcrumbToolStripRenderer(n, ns, (BreadcrumbToolStripRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ToolStripButtonRenderer))
                        {
                            this.Write11_ToolStripButtonRenderer(n, ns, (ToolStripButtonRenderer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(TypeConverter))
                        {
                            this.Write4_TypeConverter(n, ns, (TypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(EncodingConveter))
                        {
                            this.Write132_EncodingConveter(n, ns, (EncodingConveter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(AudioChannelsTypeConverter))
                        {
                            this.Write125_AudioChannelsTypeConverter(n, ns, (AudioChannelsTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PropertyTypeConverter))
                        {
                            this.Write123_PropertyTypeConverter(n, ns, (PropertyTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(RatingTypeConverter))
                        {
                            this.Write131_RatingTypeConverter(n, ns, (RatingTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ISOSpeedTypeConverter))
                        {
                            this.Write130_ISOSpeedTypeConverter(n, ns, (ISOSpeedTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DPITypeConverter))
                        {
                            this.Write129_DPITypeConverter(n, ns, (DPITypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ImageSizeTypeConverter))
                        {
                            this.Write128_ImageSizeTypeConverter(n, ns, (ImageSizeTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DurationTypeConverter))
                        {
                            this.Write127_DurationTypeConverter(n, ns, (DurationTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(AudioSampleRateTypeConverter))
                        {
                            this.Write126_AudioSampleRateTypeConverter(n, ns, (AudioSampleRateTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(BitrateTypeConverter))
                        {
                            this.Write124_BitrateTypeConverter(n, ns, (BitrateTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ArchiveFormatConverter))
                        {
                            this.Write35_ArchiveFormatConverter(n, ns, (ArchiveFormatConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(KeysConverter))
                        {
                            this.Write5_KeysConverter(n, ns, (KeysConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(KeysConverter2))
                        {
                            this.Write6_KeysConverter2(n, ns, (KeysConverter2) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ImageProvider))
                        {
                            this.Write2_ImageProvider(n, ns, (ImageProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ShellImageProvider))
                        {
                            this.Write54_ShellImageProvider(n, ns, (ShellImageProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(CustomImageProvider))
                        {
                            this.Write3_CustomImageProvider(n, ns, (CustomImageProvider) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PropertyTagType))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PropertyTagType", "");
                            base.Writer.WriteString(this.Write7_PropertyTagType((PropertyTagType) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PropertyTag))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PropertyTag", "");
                            base.Writer.WriteString(this.Write8_PropertyTag((PropertyTag) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(LightSource))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("LightSource", "");
                            base.Writer.WriteString(this.Write9_LightSource((LightSource) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ReleaseType))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ReleaseType", "");
                            base.Writer.WriteString(this.Write15_ReleaseType((ReleaseType) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(HorizontalAlignment))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("HorizontalAlignment", "");
                            base.Writer.WriteString(this.Write16_HorizontalAlignment((HorizontalAlignment) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ListViewColumnCollection))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArrayOfListViewColumnInfo", "");
                            ListViewColumnCollection columns = (ListViewColumnCollection) o;
                            if (columns != null)
                            {
                                for (int i = 0; i < columns.Count; i++)
                                {
                                    this.Write17_ListViewColumnInfo("ListViewColumnInfo", "", columns[i], true, false);
                                }
                            }
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PanelLayoutEntry))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PanelLayoutEntry", "");
                            base.Writer.WriteString(this.Write18_PanelLayoutEntry((PanelLayoutEntry) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PanelToolbar))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PanelToolbar", "");
                            base.Writer.WriteString(this.Write19_PanelToolbar((PanelToolbar) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(Orientation))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("Orientation", "");
                            base.Writer.WriteString(this.Write20_Orientation((Orientation) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PanelView))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PanelView", "");
                            base.Writer.WriteString(this.Write21_PanelView((PanelView) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ActivePanel))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ActivePanel", "");
                            base.Writer.WriteString(this.Write23_ActivePanel((ActivePanel) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(TwoPanelLayoutEntry))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("TwoPanelLayoutEntry", "");
                            base.Writer.WriteString(this.Write24_TwoPanelLayoutEntry((TwoPanelLayoutEntry) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ActionState))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ActionState", "");
                            base.Writer.WriteString(this.Write28_ActionState((ActionState) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(BindActionProperty))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("BindActionProperty", "");
                            base.Writer.WriteString(this.Write29_BindActionProperty((BindActionProperty) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(BreadcrumbView))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("BreadcrumbView", "");
                            base.Writer.WriteString(this.Write30_BreadcrumbView((BreadcrumbView) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(InputDialogOption))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("InputDialogOption", "");
                            base.Writer.WriteString(this.Write33_InputDialogOption((InputDialogOption) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ArchiveFormatCapabilities))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArchiveFormatCapabilities", "");
                            base.Writer.WriteString(this.Write36_ArchiveFormatCapabilities((ArchiveFormatCapabilities) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(string[]))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArrayOfString", "");
                            string[] strArray = (string[]) o;
                            if (strArray != null)
                            {
                                for (int j = 0; j < strArray.Length; j++)
                                {
                                    base.WriteNullableStringLiteral("string", "", strArray[j]);
                                }
                            }
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(FindFormatSource))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("FindFormatSource", "");
                            base.Writer.WriteString(this.Write39_FindFormatSource((FindFormatSource) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ProcessorState))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ProcessorState", "");
                            base.Writer.WriteString(this.Write44_ProcessorState((ProcessorState) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SequenseProcessorType))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SequenseProcessorType", "");
                            base.Writer.WriteString(this.Write45_SequenseProcessorType((SequenseProcessorType) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PK_OM))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PK_OM", "");
                            base.Writer.WriteString(this.Write46_PK_OM((PK_OM) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PK_OPERATION))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PK_OPERATION", "");
                            base.Writer.WriteString(this.Write47_PK_OPERATION((PK_OPERATION) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PK_CAPS))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PK_CAPS", "");
                            base.Writer.WriteString(this.Write48_PK_CAPS((PK_CAPS) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PK_VOL))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PK_VOL", "");
                            base.Writer.WriteString(this.Write49_PK_VOL((PK_VOL) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PK_PACK))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PK_PACK", "");
                            base.Writer.WriteString(this.Write50_PK_PACK((PK_PACK) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(DefaultIcon))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("DefaultIcon", "");
                            base.Writer.WriteString(this.Write53_DefaultIcon((DefaultIcon) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(FileSystemItem.ItemCapability))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ItemCapability", "");
                            base.Writer.WriteString(this.Write55_ItemCapability((FileSystemItem.ItemCapability) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ContentFlag))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ContentFlag", "");
                            base.Writer.WriteString(this.Write59_ContentFlag((ContentFlag) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(AggregatedFilterCondition))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("AggregatedFilterCondition", "");
                            base.Writer.WriteString(this.Write64_AggregatedFilterCondition((AggregatedFilterCondition) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(DateComparision))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("DateComparision", "");
                            base.Writer.WriteString(this.Write67_DateComparision((DateComparision) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(DateUnit))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("DateUnit", "");
                            base.Writer.WriteString(this.Write68_DateUnit((DateUnit) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ContentFilterOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ContentFilterOptions", "");
                            base.Writer.WriteString(this.Write70_ContentFilterOptions((ContentFilterOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ContentComparision))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ContentComparision", "");
                            base.Writer.WriteString(this.Write71_ContentComparision((ContentComparision) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SimpleComparision))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SimpleComparision", "");
                            base.Writer.WriteString(this.Write73_SimpleComparision((SimpleComparision) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(TimeComparision))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("TimeComparision", "");
                            base.Writer.WriteString(this.Write88_TimeComparision((TimeComparision) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ItemDateTimePart))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ItemDateTimePart", "");
                            base.Writer.WriteString(this.Write90_ItemDateTimePart((ItemDateTimePart) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(FileAttributes))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("FileAttributes", "");
                            base.Writer.WriteString(this.Write92_FileAttributes((FileAttributes) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SizeUnit))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SizeUnit", "");
                            base.Writer.WriteString(this.Write95_SizeUnit((SizeUnit) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SizeComparision))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SizeComparision", "");
                            base.Writer.WriteString(this.Write96_SizeComparision((SizeComparision) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(NamePatternCondition))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("NamePatternCondition", "");
                            base.Writer.WriteString(this.Write100_NamePatternCondition((NamePatternCondition) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(NamePatternComparision))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("NamePatternComparision", "");
                            base.Writer.WriteString(this.Write101_NamePatternComparision((NamePatternComparision) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(NameListCondition))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("NameListCondition", "");
                            base.Writer.WriteString(this.Write106_NameListCondition((NameListCondition) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(BasicFilter[]))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArrayOfChoice1", "");
                            BasicFilter[] filterArray = (BasicFilter[]) o;
                            if (filterArray != null)
                            {
                                for (int k = 0; k < filterArray.Length; k++)
                                {
                                    BasicFilter filter = filterArray[k];
                                    if (filter != null)
                                    {
                                        if (filter is VirtualItemSizeFilter)
                                        {
                                            this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemDateFilter)
                                        {
                                            this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemContentFilter)
                                        {
                                            this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemTimeFilter)
                                        {
                                            this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemHexContentFilter)
                                        {
                                            this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemNameFilter)
                                        {
                                            this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemAttributeFilter)
                                        {
                                            this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualItemNameListFilter)
                                        {
                                            this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) filter, true, false);
                                        }
                                        else if (filter is AggregatedVirtualItemFilter)
                                        {
                                            this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) filter, true, false);
                                        }
                                        else if (filter is VirtualPropertyFilter)
                                        {
                                            this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) filter, true, false);
                                        }
                                        else if (filter != null)
                                        {
                                            throw base.CreateUnknownTypeException(filter);
                                        }
                                    }
                                }
                            }
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(HighlighterIconType))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("HighlighterIconType", "");
                            base.Writer.WriteString(this.Write113_HighlighterIconType((HighlighterIconType) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CustomizeFolderParts))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CustomizeFolderParts", "");
                            base.Writer.WriteString(this.Write118_CustomizeFolderParts((CustomizeFolderParts) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ColorSpace))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ColorSpace", "");
                            base.Writer.WriteString(this.Write119_ColorSpace((ColorSpace) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ThrobberStyle))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ThrobberStyle", "");
                            base.Writer.WriteString(this.Write138_ThrobberStyle((ThrobberStyle) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(AutoRefreshMode))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("AutoRefreshMode", "");
                            base.Writer.WriteString(this.Write141_AutoRefreshMode((AutoRefreshMode) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CanMoveResult))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CanMoveResult", "");
                            base.Writer.WriteString(this.Write145_CanMoveResult((CanMoveResult) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(IconOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("IconOptions", "");
                            base.Writer.WriteString(this.Write146_IconOptions((IconOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(DelayedExtractMode))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("DelayedExtractMode", "");
                            base.Writer.WriteString(this.Write147_DelayedExtractMode((DelayedExtractMode) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PathView))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PathView", "");
                            base.Writer.WriteString(this.Write148_PathView((PathView) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ContextMenuOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ContextMenuOptions", "");
                            base.Writer.WriteString(this.Write149_ContextMenuOptions((ContextMenuOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PropertyValueList))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArrayOfPropertyValue", "");
                            PropertyValueList list = (PropertyValueList) o;
                            if (list != null)
                            {
                                for (int m = 0; m < list.Count; m++)
                                {
                                    this.Write151_PropertyValue("PropertyValue", "", list[m], true, false);
                                }
                            }
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ProgressRenderMode))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ProgressRenderMode", "");
                            base.Writer.WriteString(this.Write153_ProgressRenderMode((ProgressRenderMode) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ProgressState))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ProgressState", "");
                            base.Writer.WriteString(this.Write154_ProgressState((ProgressState) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(MarqueeStyle))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("MarqueeStyle", "");
                            base.Writer.WriteString(this.Write156_MarqueeStyle((MarqueeStyle) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(AskMode))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("AskMode", "");
                            base.Writer.WriteString(this.Write158_AskMode((AskMode) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(OperationResult))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("OperationResult", "");
                            base.Writer.WriteString(this.Write159_OperationResult((OperationResult) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ItemPropId))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ItemPropId", "");
                            base.Writer.WriteString(this.Write160_ItemPropId((ItemPropId) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(FileTimeType))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("FileTimeType", "");
                            base.Writer.WriteString(this.Write161_FileTimeType((FileTimeType) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ArchivePropId))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArchivePropId", "");
                            base.Writer.WriteString(this.Write162_ArchivePropId((ArchivePropId) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(KnownSevenZipFormat))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("KnownSevenZipFormat", "");
                            base.Writer.WriteString(this.Write163_KnownSevenZipFormat((KnownSevenZipFormat) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SevenZipFormatCapabilities))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SevenZipFormatCapabilities", "");
                            base.Writer.WriteString(this.Write164_SevenZipFormatCapabilities((SevenZipFormatCapabilities) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CompressionLevel))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CompressionLevel", "");
                            base.Writer.WriteString(this.Write165_CompressionLevel((CompressionLevel) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CompressionMethod))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CompressionMethod", "");
                            base.Writer.WriteString(this.Write166_CompressionMethod((CompressionMethod) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(EncryptionMethod))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("EncryptionMethod", "");
                            base.Writer.WriteString(this.Write167_EncryptionMethod((EncryptionMethod) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SevenZipPropertiesBuilder.SolidSizeUnit))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SolidSizeUnit", "");
                            base.Writer.WriteString(this.Write168_SolidSizeUnit((SevenZipPropertiesBuilder.SolidSizeUnit) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ComplexFilterView))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ComplexFilterView", "");
                            base.Writer.WriteString(this.Write169_ComplexFilterView((ComplexFilterView) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ViewFilters))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ViewFilters", "");
                            base.Writer.WriteString(this.Write170_ViewFilters((ViewFilters) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(QuickFindOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("QuickFindOptions", "");
                            base.Writer.WriteString(this.Write171_QuickFindOptions((QuickFindOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ControllerType))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ControllerType", "");
                            base.Writer.WriteString(this.Write173_ControllerType((ControllerType) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(FormPlacement))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("FormPlacement", "");
                            base.Writer.WriteString(this.Write176_FormPlacement((FormPlacement) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ArgumentKey))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArgumentKey", "");
                            base.Writer.WriteString(this.Write177_ArgumentKey((ArgumentKey) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CanMoveListViewItem))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CanMoveListViewItem", "");
                            base.Writer.WriteString(this.Write178_CanMoveListViewItem((CanMoveListViewItem) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(TwoPanelContainer.SinglePanel))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SinglePanel", "");
                            base.Writer.WriteString(this.Write180_SinglePanel((TwoPanelContainer.SinglePanel) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ArchiveUpdateMode))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ArchiveUpdateMode", "");
                            base.Writer.WriteString(this.Write183_ArchiveUpdateMode((ArchiveUpdateMode) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(PackStage))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("PackStage", "");
                            base.Writer.WriteString(this.Write184_PackStage((PackStage) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CopyDestinationItem))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CopyDestinationItem", "");
                            base.Writer.WriteString(this.Write190_CopyDestinationItem((CopyDestinationItem) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(MessageDialogResult))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("MessageDialogResult", "");
                            base.Writer.WriteString(this.Write191_MessageDialogResult((MessageDialogResult) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(DoubleClickAction))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("DoubleClickAction", "");
                            base.Writer.WriteString(this.Write192_DoubleClickAction((DoubleClickAction) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(VirtualFilePanel.ListViewSort))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ListViewSort", "");
                            base.Writer.WriteString(this.Write193_ListViewSort((VirtualFilePanel.ListViewSort) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(SearchFolderOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("SearchFolderOptions", "");
                            base.Writer.WriteString(this.Write195_SearchFolderOptions((SearchFolderOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(FindDuplicateOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("FindDuplicateOptions", "");
                            base.Writer.WriteString(this.Write196_FindDuplicateOptions((FindDuplicateOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(Compare))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("Compare", "");
                            base.Writer.WriteString(this.Write197_Compare((Compare) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(ChangeItemAction))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("ChangeItemAction", "");
                            base.Writer.WriteString(this.Write198_ChangeItemAction((ChangeItemAction) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(AvailableItemActions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("AvailableItemActions", "");
                            base.Writer.WriteString(this.Write199_AvailableItemActions((AvailableItemActions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CompareFoldersOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CompareFoldersOptions", "");
                            base.Writer.WriteString(this.Write200_CompareFoldersOptions((CompareFoldersOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(OverwriteDialogResult))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("OverwriteDialogResult", "");
                            base.Writer.WriteString(this.Write201_OverwriteDialogResult((OverwriteDialogResult) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CopyWorkerOptions))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CopyWorkerOptions", "");
                            base.Writer.WriteString(this.Write202_CopyWorkerOptions((CopyWorkerOptions) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(CopyMode))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("CopyMode", "");
                            base.Writer.WriteString(this.Write203_CopyMode((CopyMode) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        if (type == typeof(IconStyle))
                        {
                            base.Writer.WriteStartElement(n, ns);
                            base.WriteXsiType("IconStyle", "");
                            base.Writer.WriteString(this.Write205_IconStyle((IconStyle) o));
                            base.Writer.WriteEndElement();
                            return;
                        }
                        base.WriteTypedPrimitive(n, ns, o, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                base.WriteEndElement(o);
            }
        }

        private void Write10_ToolStripRenderer(string n, string ns, ToolStripRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(ToolStripRenderer))
                {
                    if (type == typeof(ToolStripWrapperRenderer))
                    {
                        this.Write31_ToolStripWrapperRenderer(n, ns, (ToolStripWrapperRenderer) o, isNullable, true);
                    }
                    else if (type == typeof(BreadcrumbToolStripRenderer))
                    {
                        this.Write32_BreadcrumbToolStripRenderer(n, ns, (BreadcrumbToolStripRenderer) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(ToolStripButtonRenderer))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write11_ToolStripButtonRenderer(n, ns, (ToolStripButtonRenderer) o, isNullable, true);
                    }
                }
            }
        }

        private string Write100_NamePatternCondition(NamePatternCondition v)
        {
            switch (v)
            {
                case NamePatternCondition.Equal:
                    return "Equal";

                case NamePatternCondition.NotEqual:
                    return "NotEqual";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.NamePatternCondition");
        }

        private string Write101_NamePatternComparision(NamePatternComparision v)
        {
            switch (v)
            {
                case NamePatternComparision.Ignore:
                    return "Ignore";

                case NamePatternComparision.StartsWith:
                    return "StartsWith";

                case NamePatternComparision.EndsWith:
                    return "EndsWith";

                case NamePatternComparision.Equals:
                    return "Equals";

                case NamePatternComparision.Wildcards:
                    return "Wildcards";

                case NamePatternComparision.RegEx:
                    return "RegEx";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.NamePatternComparision");
        }

        private void Write102_NameFilter(string n, string ns, NameFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(NameFilter))
                    {
                        if (type == typeof(VirtualItemFullNameFilter))
                        {
                            this.Write112_VirtualItemFullNameFilter(n, ns, (VirtualItemFullNameFilter) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(VirtualItemNameFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write103_VirtualItemNameFilter(n, ns, (VirtualItemNameFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("NameFilter", "");
                }
                if (o.NameCondition != NamePatternCondition.Equal)
                {
                    base.WriteElementString("NameCondition", "", this.Write100_NamePatternCondition(o.NameCondition));
                }
                base.WriteElementString("NameComparision", "", this.Write101_NamePatternComparision(o.NameComparision));
                base.WriteElementString("NamePattern", "", o.NamePattern);
                base.WriteEndElement(o);
            }
        }

        private void Write103_VirtualItemNameFilter(string n, string ns, VirtualItemNameFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemNameFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemNameFilter", "");
                }
                if (o.NameCondition != NamePatternCondition.Equal)
                {
                    base.WriteElementString("NameCondition", "", this.Write100_NamePatternCondition(o.NameCondition));
                }
                base.WriteElementString("NameComparision", "", this.Write101_NamePatternComparision(o.NameComparision));
                base.WriteElementString("NamePattern", "", o.NamePattern);
                base.WriteEndElement(o);
            }
        }

        private void Write104_HexContentFilter(string n, string ns, HexContentFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(HexContentFilter))
                    {
                        if (type != typeof(VirtualItemHexContentFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write105_VirtualItemHexContentFilter(n, ns, (VirtualItemHexContentFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("HexContentFilter", "");
                }
                base.WriteElementString("Comparision", "", this.Write71_ContentComparision(o.Comparision));
                base.WriteElementString("Sequence", "", o.SequenceAsString);
                base.WriteEndElement(o);
            }
        }

        private void Write105_VirtualItemHexContentFilter(string n, string ns, VirtualItemHexContentFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemHexContentFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemHexContentFilter", "");
                }
                base.WriteElementString("Comparision", "", this.Write71_ContentComparision(o.Comparision));
                base.WriteElementString("Sequence", "", o.SequenceAsString);
                base.WriteEndElement(o);
            }
        }

        private string Write106_NameListCondition(NameListCondition v)
        {
            switch (v)
            {
                case NameListCondition.InList:
                    return "InList";

                case NameListCondition.NotInList:
                    return "NotInList";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.Filter.NameListCondition");
        }

        private void Write107_VirtualItemNameListFilter(string n, string ns, VirtualItemNameListFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemNameListFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemNameListFilter", "");
                }
                base.WriteElementString("Condition", "", this.Write106_NameListCondition(o.Condition));
                string[] names = o.Names;
                if (names != null)
                {
                    base.WriteStartElement("Names", "", null, false);
                    for (int i = 0; i < names.Length; i++)
                    {
                        base.WriteNullableStringLiteral("string", "", names[i]);
                    }
                    base.WriteEndElement();
                }
                base.WriteEndElement(o);
            }
        }

        private void Write108_AggregatedVirtualItemFilter(string n, string ns, AggregatedVirtualItemFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(AggregatedVirtualItemFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("AggregatedVirtualItemFilter", "");
                }
                base.WriteElementString("Condition", "", this.Write64_AggregatedFilterCondition(o.Condition));
                BasicFilter[] serializableFilters = o.SerializableFilters;
                if (serializableFilters != null)
                {
                    base.WriteStartElement("Filters", "", null, false);
                    for (int i = 0; i < serializableFilters.Length; i++)
                    {
                        BasicFilter filter = serializableFilters[i];
                        if (filter != null)
                        {
                            if (filter is VirtualItemSizeFilter)
                            {
                                this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemDateFilter)
                            {
                                this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemContentFilter)
                            {
                                this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemTimeFilter)
                            {
                                this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemHexContentFilter)
                            {
                                this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemNameFilter)
                            {
                                this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemAttributeFilter)
                            {
                                this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) filter, true, false);
                            }
                            else if (filter is VirtualItemNameListFilter)
                            {
                                this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) filter, true, false);
                            }
                            else if (filter is AggregatedVirtualItemFilter)
                            {
                                this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) filter, true, false);
                            }
                            else if (filter is VirtualPropertyFilter)
                            {
                                this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) filter, true, false);
                            }
                            else if (filter != null)
                            {
                                throw base.CreateUnknownTypeException(filter);
                            }
                        }
                    }
                    base.WriteEndElement();
                }
                base.WriteEndElement(o);
            }
        }

        private void Write109_FilterContainer(string n, string ns, FilterContainer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(FilterContainer))
                    {
                        if (type == typeof(PanelContentContainer))
                        {
                            this.Write172_PanelContentContainer(n, ns, (PanelContentContainer) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(NamedFilter))
                        {
                            this.Write110_NamedFilter(n, ns, (NamedFilter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(VirtualHighligher))
                        {
                            this.Write114_VirtualHighligher(n, ns, (VirtualHighligher) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(ListViewHighlighter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write115_ListViewHighlighter(n, ns, (ListViewHighlighter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("FilterContainer", "");
                }
                if (o.SerializableFilter != null)
                {
                    if (o.SerializableFilter is VirtualItemSizeFilter)
                    {
                        this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemDateFilter)
                    {
                        this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemHexContentFilter)
                    {
                        this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemTimeFilter)
                    {
                        this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemContentFilter)
                    {
                        this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemAttributeFilter)
                    {
                        this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameFilter)
                    {
                        this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is AggregatedVirtualItemFilter)
                    {
                        this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualPropertyFilter)
                    {
                        this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameListFilter)
                    {
                        this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter != null)
                    {
                        throw base.CreateUnknownTypeException(o.SerializableFilter);
                    }
                }
                base.WriteEndElement(o);
            }
        }

        private void Write11_ToolStripButtonRenderer(string n, string ns, ToolStripButtonRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ToolStripButtonRenderer)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ToolStripButtonRenderer", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write110_NamedFilter(string n, string ns, NamedFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(NamedFilter))
                    {
                        if (type == typeof(VirtualHighligher))
                        {
                            this.Write114_VirtualHighligher(n, ns, (VirtualHighligher) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(ListViewHighlighter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write115_ListViewHighlighter(n, ns, (ListViewHighlighter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("NamedFilter", "");
                }
                if (o.SerializableFilter != null)
                {
                    if (o.SerializableFilter is VirtualItemSizeFilter)
                    {
                        this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemDateFilter)
                    {
                        this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemHexContentFilter)
                    {
                        this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemTimeFilter)
                    {
                        this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemContentFilter)
                    {
                        this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemAttributeFilter)
                    {
                        this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameFilter)
                    {
                        this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is AggregatedVirtualItemFilter)
                    {
                        this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualPropertyFilter)
                    {
                        this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameListFilter)
                    {
                        this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter != null)
                    {
                        throw base.CreateUnknownTypeException(o.SerializableFilter);
                    }
                }
                base.WriteElementString("Name", "", o.Name);
                base.WriteEndElement(o);
            }
        }

        private void Write111_FilterHelper(string n, string ns, FilterHelper o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(FilterHelper)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("FilterHelper", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write112_VirtualItemFullNameFilter(string n, string ns, VirtualItemFullNameFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemFullNameFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemFullNameFilter", "");
                }
                if (o.NameCondition != NamePatternCondition.Equal)
                {
                    base.WriteElementString("NameCondition", "", this.Write100_NamePatternCondition(o.NameCondition));
                }
                base.WriteElementString("NameComparision", "", this.Write101_NamePatternComparision(o.NameComparision));
                base.WriteElementString("NamePattern", "", o.NamePattern);
                base.WriteEndElement(o);
            }
        }

        private string Write113_HighlighterIconType(HighlighterIconType v)
        {
            switch (v)
            {
                case HighlighterIconType.HighlighterIcon:
                    return "HighlighterIcon";

                case HighlighterIconType.TypeIcon:
                    return "TypeIcon";

                case HighlighterIconType.ExtractedIcon:
                    return "ExtractedIcon";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.Filter.HighlighterIconType");
        }

        private void Write114_VirtualHighligher(string n, string ns, VirtualHighligher o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(VirtualHighligher))
                    {
                        if (type != typeof(ListViewHighlighter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write115_ListViewHighlighter(n, ns, (ListViewHighlighter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualHighligher", "");
                }
                if (o.SerializableFilter != null)
                {
                    if (o.SerializableFilter is VirtualItemSizeFilter)
                    {
                        this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemDateFilter)
                    {
                        this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemHexContentFilter)
                    {
                        this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemTimeFilter)
                    {
                        this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemContentFilter)
                    {
                        this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemAttributeFilter)
                    {
                        this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameFilter)
                    {
                        this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is AggregatedVirtualItemFilter)
                    {
                        this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualPropertyFilter)
                    {
                        this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameListFilter)
                    {
                        this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter != null)
                    {
                        throw base.CreateUnknownTypeException(o.SerializableFilter);
                    }
                }
                base.WriteElementString("Name", "", o.Name);
                if (o.IconType != HighlighterIconType.ExtractedIcon)
                {
                    base.WriteElementString("IconType", "", this.Write113_HighlighterIconType(o.IconType));
                }
                if (o.AlphaBlend)
                {
                    base.WriteElementStringRaw("AlphaBlend", "", XmlConvert.ToString(o.AlphaBlend));
                }
                if (o.BlendLevel != 0.5f)
                {
                    base.WriteElementStringRaw("BlendLevel", "", XmlConvert.ToString(o.BlendLevel));
                }
                base.WriteElementString("Icon", "", o.IconLocation);
                if (o.SerializableBlendColor != "White")
                {
                    base.WriteElementString("BlendColor", "", o.SerializableBlendColor);
                }
                base.WriteEndElement(o);
            }
        }

        private void Write115_ListViewHighlighter(string n, string ns, ListViewHighlighter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ListViewHighlighter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ListViewHighlighter", "");
                }
                if (o.SerializableFilter != null)
                {
                    if (o.SerializableFilter is VirtualItemSizeFilter)
                    {
                        this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemDateFilter)
                    {
                        this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemHexContentFilter)
                    {
                        this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemTimeFilter)
                    {
                        this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemContentFilter)
                    {
                        this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemAttributeFilter)
                    {
                        this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameFilter)
                    {
                        this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is AggregatedVirtualItemFilter)
                    {
                        this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualPropertyFilter)
                    {
                        this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameListFilter)
                    {
                        this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter != null)
                    {
                        throw base.CreateUnknownTypeException(o.SerializableFilter);
                    }
                }
                base.WriteElementString("Name", "", o.Name);
                if (o.IconType != HighlighterIconType.ExtractedIcon)
                {
                    base.WriteElementString("IconType", "", this.Write113_HighlighterIconType(o.IconType));
                }
                if (o.AlphaBlend)
                {
                    base.WriteElementStringRaw("AlphaBlend", "", XmlConvert.ToString(o.AlphaBlend));
                }
                if (o.BlendLevel != 0.5f)
                {
                    base.WriteElementStringRaw("BlendLevel", "", XmlConvert.ToString(o.BlendLevel));
                }
                base.WriteElementString("Icon", "", o.IconLocation);
                if (o.SerializableBlendColor != "White")
                {
                    base.WriteElementString("BlendColor", "", o.SerializableBlendColor);
                }
                base.WriteElementString("ForeColor", "", o.SerializableForeColor);
                base.WriteEndElement(o);
            }
        }

        private void Write116_HashPropertyProvider(string n, string ns, HashPropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(HashPropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("HashPropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write117_VistaThumbnailProvider(string n, string ns, VistaThumbnailProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VistaThumbnailProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VistaThumbnailProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write118_CustomizeFolderParts(CustomizeFolderParts v)
        {
            switch (v)
            {
                case CustomizeFolderParts.Columns:
                    return "Columns";

                case CustomizeFolderParts.Icon:
                    return "Icon";

                case CustomizeFolderParts.Filter:
                    return "Filter";

                case CustomizeFolderParts.Sort:
                    return "Sort";

                case CustomizeFolderParts.View:
                    return "View";

                case CustomizeFolderParts.ThumbnailSize:
                    return "ThumbnailSize";

                case CustomizeFolderParts.Colors:
                    return "Colors";

                case CustomizeFolderParts.ListColumnCount:
                    return "ListColumnCount";

                case CustomizeFolderParts.All:
                    return "All";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "All", "Columns", "Icon", "Filter", "Sort", "View", "ThumbnailSize", "Colors", "ListColumnCount" }, new long[] { 0xffL, 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x80L }, "Nomad.FileSystem.Virtual.CustomizeFolderParts");
        }

        private string Write119_ColorSpace(ColorSpace v)
        {
            switch (v)
            {
                case ColorSpace.Unknown:
                    return "Unknown";

                case ColorSpace.RGB:
                    return "RGB";

                case ColorSpace.CMYK:
                    return "CMYK";

                case ColorSpace.Grayscale:
                    return "Grayscale";

                case ColorSpace.YCBCR:
                    return "YCBCR";

                case ColorSpace.YCCK:
                    return "YCCK";

                case ColorSpace.Indexed:
                    return "Indexed";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Property.ColorSpace");
        }

        private void Write12_ProviderBase(string n, string ns, ProviderBase o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(ProviderBase))
                {
                    if (type == typeof(SettingsProvider))
                    {
                        this.Write13_SettingsProvider(n, ns, (SettingsProvider) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(ConfigurableSettingsProvider))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write14_ConfigurableSettingsProvider(n, ns, (ConfigurableSettingsProvider) o, isNullable, true);
                    }
                }
            }
        }

        private void Write120_DescriptionPropertyProvider(string n, string ns, DescriptionPropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(DescriptionPropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("DescriptionPropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write121_DummyClientSite(string n, string ns, DummyClientSite o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(DummyClientSite)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("DummyClientSite", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write122_HtmlPropertyProvider(string n, string ns, HtmlPropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(HtmlPropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("HtmlPropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write123_PropertyTypeConverter(string n, string ns, PropertyTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(PropertyTypeConverter))
                {
                    if (type == typeof(RatingTypeConverter))
                    {
                        this.Write131_RatingTypeConverter(n, ns, (RatingTypeConverter) o, isNullable, true);
                    }
                    else if (type == typeof(ISOSpeedTypeConverter))
                    {
                        this.Write130_ISOSpeedTypeConverter(n, ns, (ISOSpeedTypeConverter) o, isNullable, true);
                    }
                    else if (type == typeof(DPITypeConverter))
                    {
                        this.Write129_DPITypeConverter(n, ns, (DPITypeConverter) o, isNullable, true);
                    }
                    else if (type == typeof(ImageSizeTypeConverter))
                    {
                        this.Write128_ImageSizeTypeConverter(n, ns, (ImageSizeTypeConverter) o, isNullable, true);
                    }
                    else if (type == typeof(DurationTypeConverter))
                    {
                        this.Write127_DurationTypeConverter(n, ns, (DurationTypeConverter) o, isNullable, true);
                    }
                    else if (type == typeof(AudioSampleRateTypeConverter))
                    {
                        this.Write126_AudioSampleRateTypeConverter(n, ns, (AudioSampleRateTypeConverter) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(BitrateTypeConverter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write124_BitrateTypeConverter(n, ns, (BitrateTypeConverter) o, isNullable, true);
                    }
                }
            }
        }

        private void Write124_BitrateTypeConverter(string n, string ns, BitrateTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(BitrateTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("BitrateTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write125_AudioChannelsTypeConverter(string n, string ns, AudioChannelsTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(AudioChannelsTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("AudioChannelsTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write126_AudioSampleRateTypeConverter(string n, string ns, AudioSampleRateTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(AudioSampleRateTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("AudioSampleRateTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write127_DurationTypeConverter(string n, string ns, DurationTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(DurationTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("DurationTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write128_ImageSizeTypeConverter(string n, string ns, ImageSizeTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ImageSizeTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ImageSizeTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write129_DPITypeConverter(string n, string ns, DPITypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(DPITypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("DPITypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write13_SettingsProvider(string n, string ns, SettingsProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(SettingsProvider))
                {
                    if (type != typeof(ConfigurableSettingsProvider))
                    {
                        throw base.CreateUnknownTypeException(o);
                    }
                    this.Write14_ConfigurableSettingsProvider(n, ns, (ConfigurableSettingsProvider) o, isNullable, true);
                }
            }
        }

        private void Write130_ISOSpeedTypeConverter(string n, string ns, ISOSpeedTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ISOSpeedTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ISOSpeedTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write131_RatingTypeConverter(string n, string ns, RatingTypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(RatingTypeConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("RatingTypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write132_EncodingConveter(string n, string ns, EncodingConveter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(EncodingConveter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("EncodingConveter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write133_ImagePropertyProvider(string n, string ns, ImagePropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ImagePropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ImagePropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write134_PsdPropertyProvider(string n, string ns, PsdPropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(PsdPropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("PsdPropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write135_TagLibPropertyProvider(string n, string ns, TagLibPropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(TagLibPropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("TagLibPropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write136_TextPropertyProvider(string n, string ns, TextPropertyProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(TextPropertyProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("TextPropertyProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write137_VirtualToolTip(string n, string ns, VirtualToolTip o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualToolTip)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualToolTip", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write138_ThrobberStyle(ThrobberStyle v)
        {
            switch (v)
            {
                case ThrobberStyle.Custom:
                    return "Custom";

                case ThrobberStyle.MacOSX:
                    return "MacOSX";

                case ThrobberStyle.Firefox:
                    return "Firefox";

                case ThrobberStyle.IE7:
                    return "IE7";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Controls.ThrobberStyle");
        }

        private void Write139_Color(string n, string ns, Color o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(Color)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("Color", "");
            }
            base.WriteEndElement(o);
        }

        private void Write14_ConfigurableSettingsProvider(string n, string ns, ConfigurableSettingsProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ConfigurableSettingsProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ConfigurableSettingsProvider", "");
                }
                base.WriteElementString("ApplicationName", "", o.ApplicationName);
                base.WriteEndElement(o);
            }
        }

        private void Write140_ThrobberRenderer(string n, string ns, ThrobberRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ThrobberRenderer)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ThrobberRenderer", "");
                }
                this.Write139_Color("Color", "", o.Color, false);
                base.WriteElementStringRaw("InnerCircleRadius", "", XmlConvert.ToString(o.InnerCircleRadius));
                base.WriteElementStringRaw("OuterCircleRadius", "", XmlConvert.ToString(o.OuterCircleRadius));
                base.WriteElementStringRaw("NumberOfSpoke", "", XmlConvert.ToString(o.NumberOfSpoke));
                base.WriteElementStringRaw("SpokeThickness", "", XmlConvert.ToString(o.SpokeThickness));
                base.WriteElementString("Style", "", this.Write138_ThrobberStyle(o.Style));
                base.WriteEndElement(o);
            }
        }

        private string Write141_AutoRefreshMode(AutoRefreshMode v)
        {
            switch (v)
            {
                case AutoRefreshMode.None:
                    return "None";

                case AutoRefreshMode.Simplified:
                    return "Simplified";

                case AutoRefreshMode.Full:
                    return "Full";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.LocalFile.AutoRefreshMode");
        }

        private void Write142_FtpFileSystemCreator(string n, string ns, FtpFileSystemCreator o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(FtpFileSystemCreator)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("FtpFileSystemCreator", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write143_NullFileSystemCreator(string n, string ns, NullFileSystemCreator o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(NullFileSystemCreator)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("NullFileSystemCreator", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write144_CustomVirtualFolder(string n, string ns, CustomVirtualFolder o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType && (o.GetType() != typeof(CustomVirtualFolder)))
            {
                throw base.CreateUnknownTypeException(o);
            }
        }

        private string Write145_CanMoveResult(CanMoveResult v)
        {
            switch (v)
            {
                case CanMoveResult.None:
                    return "None";

                case CanMoveResult.Several:
                    return "Several";

                case CanMoveResult.All:
                    return "All";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.CanMoveResult");
        }

        private string Write146_IconOptions(IconOptions v)
        {
            switch (v)
            {
                case IconOptions.ExtractIcons:
                    return "ExtractIcons";

                case IconOptions.DisableExtractSlowIcons:
                    return "DisableExtractSlowIcons";

                case IconOptions.ShowOverlayIcons:
                    return "ShowOverlayIcons";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "ExtractIcons", "DisableExtractSlowIcons", "ShowOverlayIcons" }, new long[] { 1L, 2L, 4L }, "Nomad.FileSystem.Virtual.IconOptions");
        }

        private string Write147_DelayedExtractMode(DelayedExtractMode v)
        {
            switch (v)
            {
                case DelayedExtractMode.Never:
                    return "Never";

                case DelayedExtractMode.Always:
                    return "Always";

                case DelayedExtractMode.OnSlowDrivesOnly:
                    return "OnSlowDrivesOnly";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.DelayedExtractMode");
        }

        private string Write148_PathView(PathView v)
        {
            switch (v)
            {
                case PathView.ShowNormalRootName:
                    return "ShowNormalRootName";

                case PathView.ShowShortRootName:
                    return "ShowShortRootName";

                case PathView.ShowIconForEveryFolder:
                    return "ShowIconForEveryFolder";

                case PathView.ShowActiveState:
                    return "ShowActiveState";

                case PathView.ShowDriveMenuOnHover:
                    return "ShowDriveMenuOnHover";

                case PathView.VistaLikeBreadcrumb:
                    return "VistaLikeBreadcrumb";

                case PathView.ShowFolderIcon:
                    return "ShowFolderIcon";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "ShowNormalRootName", "ShowShortRootName", "ShowIconForEveryFolder", "ShowActiveState", "ShowDriveMenuOnHover", "VistaLikeBreadcrumb", "ShowFolderIcon" }, new long[] { 0L, 1L, 2L, 4L, 8L, 0x10L, 0x20L }, "Nomad.FileSystem.Virtual.PathView");
        }

        private string Write149_ContextMenuOptions(ContextMenuOptions v)
        {
            switch (v)
            {
                case ContextMenuOptions.Explore:
                    return "Explore";

                case ContextMenuOptions.CanRename:
                    return "CanRename";

                case ContextMenuOptions.VerbsOnly:
                    return "VerbsOnly";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "Explore", "CanRename", "VerbsOnly" }, new long[] { 1L, 2L, 4L }, "Nomad.FileSystem.Virtual.ContextMenuOptions");
        }

        private string Write15_ReleaseType(ReleaseType v)
        {
            switch (v)
            {
                case ReleaseType.Final:
                    return "Final";

                case ReleaseType.Alpha:
                    return "Alpha";

                case ReleaseType.Beta:
                    return "Beta";

                case ReleaseType.RC:
                    return "RC";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Configuration.ReleaseType");
        }

        private void Write150_VirtualIcon(string n, string ns, VirtualIcon o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualIcon)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualIcon", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write151_PropertyValue(string n, string ns, PropertyValue o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(PropertyValue)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("PropertyValue", "");
                }
                this.Write1_Object("DataObject", "", o.DataObject, false, false);
                base.WriteElementString("PropertyName", "", o.PropertyName);
                base.WriteEndElement(o);
            }
        }

        private void Write152_SimpleEncrypt(string n, string ns, SimpleEncrypt o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(SimpleEncrypt)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SimpleEncrypt", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write153_ProgressRenderMode(ProgressRenderMode v)
        {
            switch (v)
            {
                case ProgressRenderMode.System:
                    return "System";

                case ProgressRenderMode.Vista:
                    return "Vista";

                case ProgressRenderMode.Custom:
                    return "Custom";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Controls.ProgressRenderMode");
        }

        private string Write154_ProgressState(ProgressState v)
        {
            switch (v)
            {
                case ProgressState.Normal:
                    return "Normal";

                case ProgressState.Pause:
                    return "Pause";

                case ProgressState.Error:
                    return "Error";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Controls.ProgressState");
        }

        private void Write155_VistaProgressBarRenderer(string n, string ns, VistaProgressBarRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VistaProgressBarRenderer)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VistaProgressBarRenderer", "");
                }
                this.Write139_Color("BackgroundColor", "", o.BackgroundColor, false);
                this.Write139_Color("HighlightColor", "", o.HighlightColor, false);
                this.Write139_Color("StartColor", "", o.StartColor, false);
                this.Write139_Color("EndColor", "", o.EndColor, false);
                base.WriteEndElement(o);
            }
        }

        private string Write156_MarqueeStyle(MarqueeStyle v)
        {
            switch (v)
            {
                case MarqueeStyle.Continuous:
                    return "Continuous";

                case MarqueeStyle.LeftRight:
                    return "LeftRight";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Controls.MarqueeStyle");
        }

        private void Write157_XPProgressBarRenderer(string n, string ns, XPProgressBarRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(XPProgressBarRenderer)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("XPProgressBarRenderer", "");
                }
                this.Write139_Color("BarColor", "", o.BarColor, false);
                this.Write139_Color("BarBackgroundColor", "", o.BarBackgroundColor, false);
                base.WriteElementStringRaw("ChunkWidth", "", XmlConvert.ToString(o.ChunkWidth));
                base.WriteElementStringRaw("MarqueeChunks", "", XmlConvert.ToString(o.MarqueeChunks));
                base.WriteElementString("MarqueeStyle", "", this.Write156_MarqueeStyle(o.MarqueeStyle));
                base.WriteEndElement(o);
            }
        }

        private string Write158_AskMode(AskMode v)
        {
            switch (v)
            {
                case AskMode.kExtract:
                    return "kExtract";

                case AskMode.kTest:
                    return "kTest";

                case AskMode.kSkip:
                    return "kSkip";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.AskMode");
        }

        private string Write159_OperationResult(OperationResult v)
        {
            switch (v)
            {
                case OperationResult.kOK:
                    return "kOK";

                case OperationResult.kUnSupportedMethod:
                    return "kUnSupportedMethod";

                case OperationResult.kDataError:
                    return "kDataError";

                case OperationResult.kCRCError:
                    return "kCRCError";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.OperationResult");
        }

        private string Write16_HorizontalAlignment(HorizontalAlignment v)
        {
            switch (v)
            {
                case HorizontalAlignment.Left:
                    return "Left";

                case HorizontalAlignment.Right:
                    return "Right";

                case HorizontalAlignment.Center:
                    return "Center";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "System.Windows.Forms.HorizontalAlignment");
        }

        private string Write160_ItemPropId(ItemPropId v)
        {
            long num;
            ItemPropId id = v;
            if (id <= ItemPropId.kpidVolumeName)
            {
                switch (id)
                {
                    case ItemPropId.kpidNoProperty:
                        return "kpidNoProperty";

                    case ((ItemPropId) 1):
                        goto Label_03AE;

                    case ItemPropId.kpidHandlerItemIndex:
                        return "kpidHandlerItemIndex";

                    case ItemPropId.kpidPath:
                        return "kpidPath";

                    case ItemPropId.kpidName:
                        return "kpidName";

                    case ItemPropId.kpidExtension:
                        return "kpidExtension";

                    case ItemPropId.kpidIsFolder:
                        return "kpidIsFolder";

                    case ItemPropId.kpidSize:
                        return "kpidSize";

                    case ItemPropId.kpidPackedSize:
                        return "kpidPackedSize";

                    case ItemPropId.kpidAttributes:
                        return "kpidAttributes";

                    case ItemPropId.kpidCreationTime:
                        return "kpidCreationTime";

                    case ItemPropId.kpidLastAccessTime:
                        return "kpidLastAccessTime";

                    case ItemPropId.kpidLastWriteTime:
                        return "kpidLastWriteTime";

                    case ItemPropId.kpidSolid:
                        return "kpidSolid";

                    case ItemPropId.kpidCommented:
                        return "kpidCommented";

                    case ItemPropId.kpidEncrypted:
                        return "kpidEncrypted";

                    case ItemPropId.kpidSplitBefore:
                        return "kpidSplitBefore";

                    case ItemPropId.kpidSplitAfter:
                        return "kpidSplitAfter";

                    case ItemPropId.kpidDictionarySize:
                        return "kpidDictionarySize";

                    case ItemPropId.kpidCRC:
                        return "kpidCRC";

                    case ItemPropId.kpidType:
                        return "kpidType";

                    case ItemPropId.kpidIsAnti:
                        return "kpidIsAnti";

                    case ItemPropId.kpidMethod:
                        return "kpidMethod";

                    case ItemPropId.kpidHostOS:
                        return "kpidHostOS";

                    case ItemPropId.kpidFileSystem:
                        return "kpidFileSystem";

                    case ItemPropId.kpidUser:
                        return "kpidUser";

                    case ItemPropId.kpidGroup:
                        return "kpidGroup";

                    case ItemPropId.kpidBlock:
                        return "kpidBlock";

                    case ItemPropId.kpidComment:
                        return "kpidComment";

                    case ItemPropId.kpidPosition:
                        return "kpidPosition";

                    case ItemPropId.kpidPrefix:
                        return "kpidPrefix";

                    case ItemPropId.kpidNumSubFolders:
                        return "kpidNumSubFolders";

                    case ItemPropId.kpidNumSubFiles:
                        return "kpidNumSubFiles";

                    case ItemPropId.kpidUnpackVer:
                        return "kpidUnpackVer";

                    case ItemPropId.kpidVolume:
                        return "kpidVolume";

                    case ItemPropId.kpidIsVolume:
                        return "kpidIsVolume";

                    case ItemPropId.kpidOffset:
                        return "kpidOffset";

                    case ItemPropId.kpidLinks:
                        return "kpidLinks";

                    case ItemPropId.kpidNumBlocks:
                        return "kpidNumBlocks";

                    case ItemPropId.kpidNumVolumes:
                        return "kpidNumVolumes";

                    case ItemPropId.kpidTimeType:
                        return "kpidTimeType";

                    case ItemPropId.kpidBit64:
                        return "kpidBit64";

                    case ItemPropId.kpidBigEndian:
                        return "kpidBigEndian";

                    case ItemPropId.kpidCpu:
                        return "kpidCpu";

                    case ItemPropId.kpidPhySize:
                        return "kpidPhySize";

                    case ItemPropId.kpidHeadersSize:
                        return "kpidHeadersSize";

                    case ItemPropId.kpidChecksum:
                        return "kpidChecksum";

                    case ItemPropId.kpidCharacts:
                        return "kpidCharacts";

                    case ItemPropId.kpidVa:
                        return "kpidVa";

                    case ItemPropId.kpidId:
                        return "kpidId";

                    case ItemPropId.kpidShortName:
                        return "kpidShortName";

                    case ItemPropId.kpidCreatorApp:
                        return "kpidCreatorApp";

                    case ItemPropId.kpidSectorSize:
                        return "kpidSectorSize";

                    case ItemPropId.kpidPosixAttrib:
                        return "kpidPosixAttrib";

                    case ItemPropId.kpidLink:
                        return "kpidLink";

                    case ItemPropId.kpidTotalSize:
                        return "kpidTotalSize";

                    case ItemPropId.kpidFreeSpace:
                        return "kpidFreeSpace";

                    case ItemPropId.kpidClusterSize:
                        return "kpidClusterSize";

                    case ItemPropId.kpidVolumeName:
                        return "kpidVolumeName";
                }
            }
            else
            {
                switch (id)
                {
                    case ItemPropId.kpidLocalName:
                        return "kpidLocalName";

                    case ItemPropId.kpidProvider:
                        return "kpidProvider";

                    case ItemPropId.kpidUserDefined:
                        return "kpidUserDefined";
                }
            }
        Label_03AE:
            num = (long) ((ulong) v);
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.ItemPropId");
        }

        private string Write161_FileTimeType(FileTimeType v)
        {
            switch (v)
            {
                case FileTimeType.kWindows:
                    return "kWindows";

                case FileTimeType.kUnix:
                    return "kUnix";

                case FileTimeType.kDOS:
                    return "kDOS";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.FileTimeType");
        }

        private string Write162_ArchivePropId(ArchivePropId v)
        {
            switch (v)
            {
                case ArchivePropId.kName:
                    return "kName";

                case ArchivePropId.kClassID:
                    return "kClassID";

                case ArchivePropId.kExtension:
                    return "kExtension";

                case ArchivePropId.kAddExtension:
                    return "kAddExtension";

                case ArchivePropId.kUpdate:
                    return "kUpdate";

                case ArchivePropId.kKeepName:
                    return "kKeepName";

                case ArchivePropId.kStartSignature:
                    return "kStartSignature";

                case ArchivePropId.kFinishSignature:
                    return "kFinishSignature";

                case ArchivePropId.kAssociate:
                    return "kAssociate";
            }
            long num = (long) ((ulong) v);
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.ArchivePropId");
        }

        private string Write163_KnownSevenZipFormat(KnownSevenZipFormat v)
        {
            switch (v)
            {
                case KnownSevenZipFormat.Unknown:
                    return "Unknown";

                case KnownSevenZipFormat.SevenZip:
                    return "SevenZip";

                case KnownSevenZipFormat.Arj:
                    return "Arj";

                case KnownSevenZipFormat.BZip2:
                    return "BZip2";

                case KnownSevenZipFormat.Cab:
                    return "Cab";

                case KnownSevenZipFormat.Chm:
                    return "Chm";

                case KnownSevenZipFormat.Compound:
                    return "Compound";

                case KnownSevenZipFormat.Cpio:
                    return "Cpio";

                case KnownSevenZipFormat.Deb:
                    return "Deb";

                case KnownSevenZipFormat.Dmg:
                    return "Dmg";

                case KnownSevenZipFormat.ELF:
                    return "ELF";

                case KnownSevenZipFormat.FAT:
                    return "FAT";

                case KnownSevenZipFormat.FLV:
                    return "FLV";

                case KnownSevenZipFormat.GZip:
                    return "GZip";

                case KnownSevenZipFormat.HFS:
                    return "HFS";

                case KnownSevenZipFormat.Iso:
                    return "Iso";

                case KnownSevenZipFormat.Lzh:
                    return "Lzh";

                case KnownSevenZipFormat.Lzma:
                    return "Lzma";

                case KnownSevenZipFormat.Lzma86:
                    return "Lzma86";

                case KnownSevenZipFormat.MachO:
                    return "MachO";

                case KnownSevenZipFormat.MBR:
                    return "MBR";

                case KnownSevenZipFormat.MsLz:
                    return "MsLz";

                case KnownSevenZipFormat.Mub:
                    return "Mub";

                case KnownSevenZipFormat.Nsis:
                    return "Nsis";

                case KnownSevenZipFormat.NTFS:
                    return "NTFS";

                case KnownSevenZipFormat.PE:
                    return "PE";

                case KnownSevenZipFormat.Rar:
                    return "Rar";

                case KnownSevenZipFormat.Rpm:
                    return "Rpm";

                case KnownSevenZipFormat.Split:
                    return "Split";

                case KnownSevenZipFormat.SWF:
                    return "SWF";

                case KnownSevenZipFormat.SWFc:
                    return "SWFc";

                case KnownSevenZipFormat.Tar:
                    return "Tar";

                case KnownSevenZipFormat.Udf:
                    return "Udf";

                case KnownSevenZipFormat.VHD:
                    return "VHD";

                case KnownSevenZipFormat.Wim:
                    return "Wim";

                case KnownSevenZipFormat.Xar:
                    return "Xar";

                case KnownSevenZipFormat.Xz:
                    return "Xz";

                case KnownSevenZipFormat.Z:
                    return "Z";

                case KnownSevenZipFormat.Zip:
                    return "Zip";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.KnownSevenZipFormat");
        }

        private string Write164_SevenZipFormatCapabilities(SevenZipFormatCapabilities v)
        {
            switch (v)
            {
                case SevenZipFormatCapabilities.Internal:
                    return "Internal";

                case SevenZipFormatCapabilities.AppendExt:
                    return "AppendExt";

                case SevenZipFormatCapabilities.Solid:
                    return "Solid";

                case SevenZipFormatCapabilities.MultiThread:
                    return "MultiThread";

                case SevenZipFormatCapabilities.SFX:
                    return "SFX";

                case SevenZipFormatCapabilities.EncryptFileNames:
                    return "EncryptFileNames";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "Solid", "MultiThread", "SFX", "EncryptFileNames", "Internal", "AppendExt" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L }, "Nomad.FileSystem.Archive.SevenZip.SevenZipFormatCapabilities");
        }

        private string Write165_CompressionLevel(CompressionLevel v)
        {
            switch (v)
            {
                case CompressionLevel.Store:
                    return "Store";

                case CompressionLevel.Fastest:
                    return "Fastest";

                case CompressionLevel.Fast:
                    return "Fast";

                case CompressionLevel.Normal:
                    return "Normal";

                case CompressionLevel.Maximum:
                    return "Maximum";

                case CompressionLevel.Ultra:
                    return "Ultra";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.CompressionLevel");
        }

        private string Write166_CompressionMethod(CompressionMethod v)
        {
            switch (v)
            {
                case CompressionMethod.Copy:
                    return "Copy";

                case CompressionMethod.LZMA:
                    return "LZMA";

                case CompressionMethod.LZMA2:
                    return "LZMA2";

                case CompressionMethod.PPMd:
                    return "PPMd";

                case CompressionMethod.BZip2:
                    return "BZip2";

                case CompressionMethod.Deflate:
                    return "Deflate";

                case CompressionMethod.Deflate64:
                    return "Deflate64";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.CompressionMethod");
        }

        private string Write167_EncryptionMethod(EncryptionMethod v)
        {
            switch (v)
            {
                case EncryptionMethod.AES256:
                    return "AES256";

                case EncryptionMethod.ZipCrypto:
                    return "ZipCrypto";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.EncryptionMethod");
        }

        private string Write168_SolidSizeUnit(SevenZipPropertiesBuilder.SolidSizeUnit v)
        {
            switch (v)
            {
                case SevenZipPropertiesBuilder.SolidSizeUnit.B:
                    return "B";

                case SevenZipPropertiesBuilder.SolidSizeUnit.Kb:
                    return "Kb";

                case SevenZipPropertiesBuilder.SolidSizeUnit.Mb:
                    return "Mb";

                case SevenZipPropertiesBuilder.SolidSizeUnit.Gb:
                    return "Gb";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.SevenZip.SevenZipPropertiesBuilder.SolidSizeUnit");
        }

        private string Write169_ComplexFilterView(ComplexFilterView v)
        {
            switch (v)
            {
                case ComplexFilterView.Basic:
                    return "Basic";

                case ComplexFilterView.Advanced:
                    return "Advanced";

                case ComplexFilterView.Full:
                    return "Full";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Controls.Filter.ComplexFilterView");
        }

        private void Write17_ListViewColumnInfo(string n, string ns, ListViewColumnInfo o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ListViewColumnInfo)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ListViewColumnInfo", "");
                }
                if (o.DefaultWidth != -1)
                {
                    base.WriteAttribute("DefaultWidth", "", XmlConvert.ToString(o.DefaultWidth));
                }
                if (o.DisplayIndex != -1)
                {
                    base.WriteAttribute("DisplayIndex", "", XmlConvert.ToString(o.DisplayIndex));
                }
                if (o.TextAlign != HorizontalAlignment.Left)
                {
                    base.WriteAttribute("TextAlign", "", this.Write16_HorizontalAlignment(o.TextAlign));
                }
                if (o.Width != 0x48)
                {
                    base.WriteAttribute("Width", "", XmlConvert.ToString(o.Width));
                }
                if (o.Visible)
                {
                    base.WriteAttribute("Visible", "", XmlConvert.ToString(o.Visible));
                }
                base.WriteAttribute("Property", "", o.PropertyStr);
                base.WriteEndElement(o);
            }
        }

        private string Write170_ViewFilters(ViewFilters v)
        {
            switch (v)
            {
                case ViewFilters.Advanced:
                    return "Advanced";

                case ViewFilters.Folder:
                    return "Folder";

                case ViewFilters.IncludeMask:
                    return "IncludeMask";

                case ViewFilters.ExcludeMask:
                    return "ExcludeMask";

                case ViewFilters.Content:
                    return "Content";

                case ViewFilters.Attributes:
                    return "Attributes";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "IncludeMask", "ExcludeMask", "Content", "Attributes", "Advanced", "Folder" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L }, "Nomad.Controls.Filter.ViewFilters");
        }

        private string Write171_QuickFindOptions(QuickFindOptions v)
        {
            switch (v)
            {
                case QuickFindOptions.PrefixSearch:
                    return "PrefixSearch";

                case QuickFindOptions.QuickFilter:
                    return "QuickFilter";

                case QuickFindOptions.AlwaysShowFolders:
                    return "AlwaysShowFolders";

                case QuickFindOptions.ExecuteOnEnter:
                    return "ExecuteOnEnter";

                case QuickFindOptions.AutoHide:
                    return "AutoHide";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "PrefixSearch", "QuickFilter", "AlwaysShowFolders", "ExecuteOnEnter", "AutoHide" }, new long[] { 1L, 2L, 4L, 8L, 0x10L }, "Nomad.QuickFindOptions");
        }

        private void Write172_PanelContentContainer(string n, string ns, PanelContentContainer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(PanelContentContainer)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("PanelContentContainer", "");
                }
                if (o.SerializableFilter != null)
                {
                    if (o.SerializableFilter is VirtualItemSizeFilter)
                    {
                        this.Write98_VirtualItemSizeFilter("SizeFilter", "", (VirtualItemSizeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemDateFilter)
                    {
                        this.Write99_VirtualItemDateFilter("DateFilter", "", (VirtualItemDateFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemHexContentFilter)
                    {
                        this.Write105_VirtualItemHexContentFilter("HexContentFilter", "", (VirtualItemHexContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemTimeFilter)
                    {
                        this.Write91_VirtualItemTimeFilter("TimeFilter", "", (VirtualItemTimeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemContentFilter)
                    {
                        this.Write87_VirtualItemContentFilter("ContentFilter", "", (VirtualItemContentFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemAttributeFilter)
                    {
                        this.Write94_VirtualItemAttributeFilter("AttributeFilter", "", (VirtualItemAttributeFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameFilter)
                    {
                        this.Write103_VirtualItemNameFilter("NameFilter", "", (VirtualItemNameFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is AggregatedVirtualItemFilter)
                    {
                        this.Write108_AggregatedVirtualItemFilter("AggregatedFilter", "", (AggregatedVirtualItemFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualPropertyFilter)
                    {
                        this.Write84_VirtualPropertyFilter("PropertyFilter", "", (VirtualPropertyFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter is VirtualItemNameListFilter)
                    {
                        this.Write107_VirtualItemNameListFilter("NameListFilter", "", (VirtualItemNameListFilter) o.SerializableFilter, false, false);
                    }
                    else if (o.SerializableFilter != null)
                    {
                        throw base.CreateUnknownTypeException(o.SerializableFilter);
                    }
                }
                if (o.Locked)
                {
                    base.WriteElementStringRaw("Locked", "", XmlConvert.ToString(o.Locked));
                }
                if (o.QuickFindOptions != (QuickFindOptions.ExecuteOnEnter | QuickFindOptions.PrefixSearch))
                {
                    base.WriteElementString("QuickFindOptions", "", this.Write171_QuickFindOptions(o.QuickFindOptions));
                }
                base.WriteElementStringRaw("FolderStream", "", XmlSerializationWriter.FromByteArrayBase64(o.SerializableFolderStream));
                base.WriteElementString("FolderPath", "", o.SerializableFolderPath);
                base.WriteElementString("Sort", "", o.SerializableSort);
                base.WriteEndElement(o);
            }
        }

        private string Write173_ControllerType(ControllerType v)
        {
            switch (v)
            {
                case ControllerType.Unknown:
                    return "Unknown";

                case ControllerType.Server:
                    return "Server";

                case ControllerType.Client:
                    return "Client";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.ControllerType");
        }

        private void Write174_MarshalByRefObject(string n, string ns, MarshalByRefObject o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(MarshalByRefObject))
                {
                    if (type != typeof(Controller))
                    {
                        throw base.CreateUnknownTypeException(o);
                    }
                    this.Write175_Controller(n, ns, (Controller) o, isNullable, true);
                }
            }
        }

        private void Write175_Controller(string n, string ns, Controller o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(Controller)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("Controller", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write176_FormPlacement(FormPlacement v)
        {
            switch (v)
            {
                case ~FormPlacement.None:
                    return "All";

                case FormPlacement.None:
                    return "None";

                case FormPlacement.Location:
                    return "Location";

                case FormPlacement.Size:
                    return "Size";

                case FormPlacement.WindowState:
                    return "WindowState";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "None", "Location", "Size", "WindowState", "All" }, new long[] { 0L, 1L, 2L, 4L, -1L }, "Nomad.Configuration.FormPlacement");
        }

        private string Write177_ArgumentKey(ArgumentKey v)
        {
            switch (v)
            {
                case ArgumentKey.None:
                    return "None";

                case ArgumentKey.Help:
                    return "Help";

                case ArgumentKey.Init:
                    return "Init";

                case ArgumentKey.Safe:
                    return "Safe";

                case ArgumentKey.NewInstance:
                    return "NewInstance";

                case ArgumentKey.OldInstance:
                    return "OldInstance";

                case ArgumentKey.Tab:
                    return "Tab";

                case ArgumentKey.LeftFolder:
                    return "LeftFolder";

                case ArgumentKey.RightFolder:
                    return "RightFolder";

                case ArgumentKey.CurrentFolder:
                    return "CurrentFolder";

                case ArgumentKey.FarFolder:
                    return "FarFolder";

                case ArgumentKey.Culture:
                    return "Culture";

                case ArgumentKey.FormLocalizer:
                    return "FormLocalizer";

                case ArgumentKey.RecoveryFolder:
                    return "RecoveryFolder";

                case ArgumentKey.Debug:
                    return "Debug";

                case ArgumentKey.Dump:
                    return "Dump";

                case ArgumentKey.Restart:
                    return "Restart";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.ArgumentKey");
        }

        private string Write178_CanMoveListViewItem(CanMoveListViewItem v)
        {
            switch (v)
            {
                case CanMoveListViewItem.Up:
                    return "Up";

                case CanMoveListViewItem.Down:
                    return "Down";

                case CanMoveListViewItem.UpInGroup:
                    return "UpInGroup";

                case CanMoveListViewItem.DownInGroup:
                    return "DownInGroup";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "Up", "Down", "UpInGroup", "DownInGroup" }, new long[] { 1L, 2L, 4L, 8L }, "System.Windows.Forms.CanMoveListViewItem");
        }

        private void Write179_Trace(string n, string ns, Trace o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(Trace)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("Trace", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write18_PanelLayoutEntry(PanelLayoutEntry v)
        {
            switch (v)
            {
                case PanelLayoutEntry.ThumbnailSize:
                    return "ThumbnailSize";

                case PanelLayoutEntry.ListColumnCount:
                    return "ListColumnCount";

                case PanelLayoutEntry.All:
                    return "All";

                case PanelLayoutEntry.None:
                    return "None";

                case PanelLayoutEntry.FolderBarVisible:
                    return "FolderBarVisible";

                case PanelLayoutEntry.FolderBarOrientation:
                    return "FolderBarOrientation";

                case PanelLayoutEntry.View:
                    return "View";

                case PanelLayoutEntry.Columns:
                    return "Columns";

                case PanelLayoutEntry.ToolbarsVisible:
                    return "ToolbarsVisible";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "None", "FolderBarVisible", "FolderBarOrientation", "View", "Columns", "ToolbarsVisible", "ThumbnailSize", "ListColumnCount", "All" }, new long[] { 0L, 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x7fL }, "Nomad.Configuration.PanelLayoutEntry");
        }

        private string Write180_SinglePanel(TwoPanelContainer.SinglePanel v)
        {
            switch (v)
            {
                case TwoPanelContainer.SinglePanel.None:
                    return "None";

                case TwoPanelContainer.SinglePanel.Left:
                    return "Left";

                case TwoPanelContainer.SinglePanel.Right:
                    return "Right";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.TwoPanelContainer.SinglePanel");
        }

        private void Write181_GeneralTab(string n, string ns, GeneralTab o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(GeneralTab))
                    {
                        if (type != typeof(TwoPanelTab))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write182_TwoPanelTab(n, ns, (TwoPanelTab) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("GeneralTab", "");
                }
                base.WriteElementString("Caption", "", o.Caption);
                base.WriteElementString("Hotkey", "", o.SerializableHotkey);
                base.WriteEndElement(o);
            }
        }

        private void Write182_TwoPanelTab(string n, string ns, TwoPanelTab o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(TwoPanelTab)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("TwoPanelTab", "");
                }
                base.WriteElementString("Caption", "", o.Caption);
                base.WriteElementString("Hotkey", "", o.SerializableHotkey);
                this.Write25_TwoPanelLayout("Layout", "", o.Layout, false, false);
                this.Write172_PanelContentContainer("Left", "", o.Left, false, false);
                this.Write172_PanelContentContainer("Right", "", o.Right, false, false);
                base.WriteEndElement(o);
            }
        }

        private string Write183_ArchiveUpdateMode(ArchiveUpdateMode v)
        {
            switch (v)
            {
                case ArchiveUpdateMode.CreateNew:
                    return "CreateNew";

                case ArchiveUpdateMode.OverwriteAll:
                    return "OverwriteAll";

                case ArchiveUpdateMode.SkipAll:
                    return "SkipAll";

                case ArchiveUpdateMode.RefreshOld:
                    return "RefreshOld";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Workers.ArchiveUpdateMode");
        }

        private string Write184_PackStage(PackStage v)
        {
            switch (v)
            {
                case PackStage.NotStarted:
                    return "NotStarted";

                case PackStage.CalculatingSize:
                    return "CalculatingSize";

                case PackStage.ReadingExistingArchive:
                    return "ReadingExistingArchive";

                case PackStage.MovingExistingItems:
                    return "MovingExistingItems";

                case PackStage.PackingNewItems:
                    return "PackingNewItems";

                case PackStage.Relocating:
                    return "Relocating";

                case PackStage.Finished:
                    return "Finished";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Workers.PackStage");
        }

        private void Write185_ProcessedSize(string n, string ns, ProcessedSize o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(ProcessedSize)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("ProcessedSize", "");
            }
            base.WriteEndElement(o);
        }

        private void Write186_TimeSpan(string n, string ns, TimeSpan o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(TimeSpan)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("TimeSpan", "");
            }
            base.WriteEndElement(o);
        }

        private void Write187_PackProgressSnapshot(string n, string ns, PackProgressSnapshot o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(PackProgressSnapshot)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("PackProgressSnapshot", "");
                }
                this.Write185_ProcessedSize("Processed", "", o.Processed, false);
                base.WriteElementStringRaw("TotalCount", "", XmlConvert.ToString(o.TotalCount));
                base.WriteElementStringRaw("ProcessedCount", "", XmlConvert.ToString(o.ProcessedCount));
                this.Write186_TimeSpan("Duration", "", o.Duration, false);
                base.WriteElementStringRaw("CompressionRatio", "", XmlConvert.ToString(o.CompressionRatio));
                base.WriteElementString("Stage", "", this.Write184_PackStage(o.Stage));
                base.WriteEndElement(o);
            }
        }

        private void Write188_CustomBackgroundWorker(string n, string ns, CustomBackgroundWorker o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(CustomBackgroundWorker))
                    {
                        if (type == typeof(EventBackgroundWorker))
                        {
                            this.Write189_EventBackgroundWorker(n, ns, (EventBackgroundWorker) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(CustomAsyncFolder))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write194_CustomAsyncFolder(n, ns, (CustomAsyncFolder) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("CustomBackgroundWorker", "");
                }
                base.WriteElementStringRaw("CancellationPending", "", XmlConvert.ToString(o.CancellationPending));
                base.WriteEndElement(o);
            }
        }

        private void Write189_EventBackgroundWorker(string n, string ns, EventBackgroundWorker o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(EventBackgroundWorker))
                {
                    if (type != typeof(CustomAsyncFolder))
                    {
                        throw base.CreateUnknownTypeException(o);
                    }
                    this.Write194_CustomAsyncFolder(n, ns, (CustomAsyncFolder) o, isNullable, true);
                }
            }
        }

        private string Write19_PanelToolbar(PanelToolbar v)
        {
            switch (v)
            {
                case PanelToolbar.None:
                    return "None";

                case PanelToolbar.Folder:
                    return "Folder";

                case PanelToolbar.Item:
                    return "Item";

                case PanelToolbar.Find:
                    return "Find";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "None", "Folder", "Item", "Find" }, new long[] { 0L, 1L, 2L, 4L }, "Nomad.Configuration.PanelToolbar");
        }

        private string Write190_CopyDestinationItem(CopyDestinationItem v)
        {
            switch (v)
            {
                case CopyDestinationItem.Ask:
                    return "Ask";

                case CopyDestinationItem.File:
                    return "File";

                case CopyDestinationItem.Folder:
                    return "Folder";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Dialogs.CopyDestinationItem");
        }

        private string Write191_MessageDialogResult(MessageDialogResult v)
        {
            switch (v)
            {
                case MessageDialogResult.None:
                    return "None";

                case MessageDialogResult.OK:
                    return "OK";

                case MessageDialogResult.Cancel:
                    return "Cancel";

                case MessageDialogResult.Yes:
                    return "Yes";

                case MessageDialogResult.No:
                    return "No";

                case MessageDialogResult.YesToAll:
                    return "YesToAll";

                case MessageDialogResult.NoToAll:
                    return "NoToAll";

                case MessageDialogResult.Skip:
                    return "Skip";

                case MessageDialogResult.SkipAll:
                    return "SkipAll";

                case MessageDialogResult.Abort:
                    return "Abort";

                case MessageDialogResult.Retry:
                    return "Retry";

                case MessageDialogResult.Ignore:
                    return "Ignore";

                case MessageDialogResult.Shield:
                    return "Shield";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Dialogs.MessageDialogResult");
        }

        private string Write192_DoubleClickAction(DoubleClickAction v)
        {
            switch (v)
            {
                case DoubleClickAction.None:
                    return "None";

                case DoubleClickAction.GoToParent:
                    return "GoToParent";

                case DoubleClickAction.SelectAll:
                    return "SelectAll";

                case DoubleClickAction.UnselectAll:
                    return "UnselectAll";

                case DoubleClickAction.ToggleSelection:
                    return "ToggleSelection";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.DoubleClickAction");
        }

        private string Write193_ListViewSort(VirtualFilePanel.ListViewSort v)
        {
            switch (v)
            {
                case VirtualFilePanel.ListViewSort.None:
                    return "None";

                case VirtualFilePanel.ListViewSort.Fast:
                    return "Fast";

                case VirtualFilePanel.ListViewSort.Full:
                    return "Full";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.VirtualFilePanel.ListViewSort");
        }

        private void Write194_CustomAsyncFolder(string n, string ns, CustomAsyncFolder o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType && (o.GetType() != typeof(CustomAsyncFolder)))
            {
                throw base.CreateUnknownTypeException(o);
            }
        }

        private string Write195_SearchFolderOptions(SearchFolderOptions v)
        {
            switch (v)
            {
                case SearchFolderOptions.AsyncSearch:
                    return "AsyncSearch";

                case SearchFolderOptions.AutoAsyncSearch:
                    return "AutoAsyncSearch";

                case SearchFolderOptions.ExpandAggregatedRoot:
                    return "ExpandAggregatedRoot";

                case SearchFolderOptions.ProcessSubfolders:
                    return "ProcessSubfolders";

                case SearchFolderOptions.ProcessArchives:
                    return "ProcessArchives";

                case SearchFolderOptions.SkipUnmatchedSubfolders:
                    return "SkipUnmatchedSubfolders";

                case SearchFolderOptions.SkipReparsePoints:
                    return "SkipReparsePoints";

                case SearchFolderOptions.DetectChanges:
                    return "DetectChanges";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "ProcessSubfolders", "ProcessArchives", "SkipUnmatchedSubfolders", "SkipReparsePoints", "DetectChanges", "AsyncSearch", "AutoAsyncSearch", "ExpandAggregatedRoot" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x80L }, "Nomad.FileSystem.Virtual.SearchFolderOptions");
        }

        private string Write196_FindDuplicateOptions(FindDuplicateOptions v)
        {
            switch (v)
            {
                case FindDuplicateOptions.SameName:
                    return "SameName";

                case FindDuplicateOptions.SameSize:
                    return "SameSize";

                case FindDuplicateOptions.SameContent:
                    return "SameContent";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "SameName", "SameSize", "SameContent" }, new long[] { 1L, 2L, 4L }, "Nomad.FileSystem.Virtual.FindDuplicateOptions");
        }

        private string Write197_Compare(Compare v)
        {
            switch (v)
            {
                case Compare.Always:
                    return "Always";

                case Compare.Equal:
                    return "Equal";

                case Compare.Greater:
                    return "Greater";

                case Compare.GreaterEqual:
                    return "GreaterEqual";

                case Compare.Less:
                    return "Less";

                case Compare.LessEqual:
                    return "LessEqual";

                case Compare.Never:
                    return "Never";

                case Compare.NotEqual:
                    return "NotEqual";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Workers.Compare");
        }

        private string Write198_ChangeItemAction(ChangeItemAction v)
        {
            switch (v)
            {
                case ChangeItemAction.None:
                    return "None";

                case ChangeItemAction.Retry:
                    return "Retry";

                case ChangeItemAction.Ignore:
                    return "Ignore";

                case ChangeItemAction.Skip:
                    return "Skip";

                case ChangeItemAction.Cancel:
                    return "Cancel";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.ChangeItemAction");
        }

        private string Write199_AvailableItemActions(AvailableItemActions v)
        {
            switch (v)
            {
                case AvailableItemActions.None:
                    return "None";

                case AvailableItemActions.CanRetry:
                    return "CanRetry";

                case AvailableItemActions.CanIgnore:
                    return "CanIgnore";

                case AvailableItemActions.CanRetryOrIgnore:
                    return "CanRetryOrIgnore";

                case AvailableItemActions.CanElevate:
                    return "CanElevate";

                case AvailableItemActions.CanRetryOrElevate:
                    return "CanRetryOrElevate";

                case AvailableItemActions.CanUndoDestination:
                    return "CanUndoDestination";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "None", "CanRetry", "CanIgnore", "CanElevate", "CanUndoDestination", "CanRetryOrIgnore", "CanRetryOrElevate" }, new long[] { 0L, 1L, 2L, 4L, 8L, 3L, 5L }, "Nomad.AvailableItemActions");
        }

        private void Write2_ImageProvider(string n, string ns, ImageProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(ImageProvider))
                {
                    if (type == typeof(ShellImageProvider))
                    {
                        this.Write54_ShellImageProvider(n, ns, (ShellImageProvider) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(CustomImageProvider))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write3_CustomImageProvider(n, ns, (CustomImageProvider) o, isNullable, true);
                    }
                }
            }
        }

        private string Write20_Orientation(Orientation v)
        {
            switch (v)
            {
                case Orientation.Horizontal:
                    return "Horizontal";

                case Orientation.Vertical:
                    return "Vertical";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "System.Windows.Forms.Orientation");
        }

        private string Write200_CompareFoldersOptions(CompareFoldersOptions v)
        {
            switch (v)
            {
                case CompareFoldersOptions.CompareContentAsync:
                    return "CompareContentAsync";

                case CompareFoldersOptions.AutoCompareContentAsync:
                    return "AutoCompareContentAsync";

                case CompareFoldersOptions.CompareAttributes:
                    return "CompareAttributes";

                case CompareFoldersOptions.CompareLastWriteTime:
                    return "CompareLastWriteTime";

                case CompareFoldersOptions.CompareSize:
                    return "CompareSize";

                case CompareFoldersOptions.CompareContent:
                    return "CompareContent";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "CompareAttributes", "CompareLastWriteTime", "CompareSize", "CompareContent", "CompareContentAsync", "AutoCompareContentAsync" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L }, "Nomad.Workers.CompareFoldersOptions");
        }

        private string Write201_OverwriteDialogResult(OverwriteDialogResult v)
        {
            switch (v)
            {
                case OverwriteDialogResult.None:
                    return "None";

                case OverwriteDialogResult.Overwrite:
                    return "Overwrite";

                case OverwriteDialogResult.Append:
                    return "Append";

                case OverwriteDialogResult.Resume:
                    return "Resume";

                case OverwriteDialogResult.Rename:
                    return "Rename";

                case OverwriteDialogResult.Skip:
                    return "Skip";

                case OverwriteDialogResult.Abort:
                    return "Abort";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Workers.OverwriteDialogResult");
        }

        private string Write202_CopyWorkerOptions(CopyWorkerOptions v)
        {
            switch (v)
            {
                case CopyWorkerOptions.CheckFreeSpace:
                    return "CheckFreeSpace";

                case CopyWorkerOptions.ClearROFromCD:
                    return "ClearROFromCD";

                case CopyWorkerOptions.DeleteSource:
                    return "DeleteSource";

                case CopyWorkerOptions.SkipEmptyFolders:
                    return "SkipEmptyFolders";

                case CopyWorkerOptions.AsyncCopy:
                    return "AsyncCopy";

                case CopyWorkerOptions.AutoAsyncCopy:
                    return "AutoAsyncCopy";

                case CopyWorkerOptions.CopyACL:
                    return "CopyACL";

                case CopyWorkerOptions.CopyItemTime:
                    return "CopyItemTime";

                case CopyWorkerOptions.CopyFolderTime:
                    return "CopyFolderTime";

                case CopyWorkerOptions.UseSystemCopy:
                    return "UseSystemCopy";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "DeleteSource", "SkipEmptyFolders", "AsyncCopy", "AutoAsyncCopy", "CheckFreeSpace", "ClearROFromCD", "CopyACL", "CopyItemTime", "CopyFolderTime", "UseSystemCopy" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x80L, 0x100L, 0x200L }, "Nomad.Workers.CopyWorkerOptions");
        }

        private string Write203_CopyMode(CopyMode v)
        {
            switch (v)
            {
                case CopyMode.Sync:
                    return "Sync";

                case CopyMode.Async:
                    return "Async";

                case CopyMode.System:
                    return "System";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Workers.CopyMode");
        }

        private void Write204_CopyProgressSnapshot(string n, string ns, CopyProgressSnapshot o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(CopyProgressSnapshot)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("CopyProgressSnapshot", "");
                }
                this.Write185_ProcessedSize("Processed", "", o.Processed, false);
                base.WriteElementStringRaw("TotalCount", "", XmlConvert.ToString(o.TotalCount));
                base.WriteElementStringRaw("ProcessedCount", "", XmlConvert.ToString(o.ProcessedCount));
                base.WriteElementStringRaw("SkippedCount", "", XmlConvert.ToString(o.SkippedCount));
                this.Write186_TimeSpan("Duration", "", o.Duration, false);
                base.WriteElementString("CopyMode", "", this.Write203_CopyMode(o.CopyMode));
                base.WriteEndElement(o);
            }
        }

        private string Write205_IconStyle(IconStyle v)
        {
            switch (v)
            {
                case IconStyle.DefaultIcon:
                    return "DefaultIcon";

                case IconStyle.CanUseDelayedExtract:
                    return "CanUseDelayedExtract";

                case IconStyle.CanUseAlphaBlending:
                    return "CanUseAlphaBlending";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "DefaultIcon", "CanUseDelayedExtract", "CanUseAlphaBlending" }, new long[] { 1L, 2L, 4L }, "Nomad.FileSystem.Virtual.IconStyle");
        }

        public void Write206_ImageProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ImageProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write2_ImageProvider("ImageProvider", "", (ImageProvider) o, true, false);
            }
        }

        public void Write207_CustomImageProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CustomImageProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write3_CustomImageProvider("CustomImageProvider", "", (CustomImageProvider) o, true, false);
            }
        }

        public void Write208_KeysConverter2(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("KeysConverter2", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write6_KeysConverter2("KeysConverter2", "", (KeysConverter2) o, true, false);
            }
        }

        public void Write209_PropertyTagType(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PropertyTagType", "");
            }
            else
            {
                base.WriteElementString("PropertyTagType", "", this.Write7_PropertyTagType((PropertyTagType) o));
            }
        }

        private string Write21_PanelView(PanelView v)
        {
            switch (v)
            {
                case PanelView.LargeIcon:
                    return "LargeIcon";

                case PanelView.Details:
                    return "Details";

                case PanelView.SmallIcon:
                    return "SmallIcon";

                case PanelView.List:
                    return "List";

                case PanelView.Tile:
                    return "Tile";

                case PanelView.Thumbnail:
                    return "Thumbnail";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.PanelView");
        }

        public void Write210_PropertyTag(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PropertyTag", "");
            }
            else
            {
                base.WriteElementString("PropertyTag", "", this.Write8_PropertyTag((PropertyTag) o));
            }
        }

        public void Write211_LightSource(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("LightSource", "");
            }
            else
            {
                base.WriteElementString("LightSource", "", this.Write9_LightSource((LightSource) o));
            }
        }

        public void Write212_ToolStripButtonRenderer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ToolStripButtonRenderer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write11_ToolStripButtonRenderer("ToolStripButtonRenderer", "", (ToolStripButtonRenderer) o, true, false);
            }
        }

        public void Write213_ConfigurableSettingsProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ConfigurableSettingsProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write14_ConfigurableSettingsProvider("ConfigurableSettingsProvider", "", (ConfigurableSettingsProvider) o, true, false);
            }
        }

        public void Write214_ReleaseType(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ReleaseType", "");
            }
            else
            {
                base.WriteElementString("ReleaseType", "", this.Write15_ReleaseType((ReleaseType) o));
            }
        }

        public void Write215_ListViewColumnInfo(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ListViewColumnInfo", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write17_ListViewColumnInfo("ListViewColumnInfo", "", (ListViewColumnInfo) o, true, false);
            }
        }

        public void Write216_ArrayOfListViewColumnInfo(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ArrayOfListViewColumnInfo", "");
            }
            else
            {
                base.TopLevelElement();
                ListViewColumnCollection columns = (ListViewColumnCollection) o;
                if (columns == null)
                {
                    base.WriteNullTagLiteral("ArrayOfListViewColumnInfo", "");
                }
                else
                {
                    base.WriteStartElement("ArrayOfListViewColumnInfo", "", null, false);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        this.Write17_ListViewColumnInfo("ListViewColumnInfo", "", columns[i], true, false);
                    }
                    base.WriteEndElement();
                }
            }
        }

        public void Write217_PanelLayoutEntry(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PanelLayoutEntry", "");
            }
            else
            {
                base.WriteElementString("PanelLayoutEntry", "", this.Write18_PanelLayoutEntry((PanelLayoutEntry) o));
            }
        }

        public void Write218_PanelToolbar(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PanelToolbar", "");
            }
            else
            {
                base.WriteElementString("PanelToolbar", "", this.Write19_PanelToolbar((PanelToolbar) o));
            }
        }

        public void Write219_PanelLayout(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("PanelLayout", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write22_PanelLayout("PanelLayout", "", (PanelLayout) o, true, false);
            }
        }

        private void Write22_PanelLayout(string n, string ns, PanelLayout o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(PanelLayout)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("PanelLayout", "");
                }
                if (o.FolderBarVisible)
                {
                    base.WriteElementStringRaw("FolderBarVisible", "", XmlConvert.ToString(o.FolderBarVisible));
                }
                if (o.FolderBarOrientation != Orientation.Vertical)
                {
                    base.WriteElementString("FolderBarOrientation", "", this.Write20_Orientation(o.FolderBarOrientation));
                }
                if (o.SplitterPercent != 500)
                {
                    base.WriteElementStringRaw("SplitterPercent", "", XmlConvert.ToString(o.SplitterPercent));
                }
                if (o.View != PanelView.List)
                {
                    base.WriteElementString("View", "", this.Write21_PanelView(o.View));
                }
                if (!o.AutoSizeColumns)
                {
                    base.WriteElementStringRaw("AutoSizeColumns", "", XmlConvert.ToString(o.AutoSizeColumns));
                }
                ListViewColumnInfo[] columns = o.Columns;
                if (columns != null)
                {
                    base.WriteStartElement("Columns", "", null, false);
                    for (int i = 0; i < columns.Length; i++)
                    {
                        this.Write17_ListViewColumnInfo("ListViewColumnInfo", "", columns[i], true, false);
                    }
                    base.WriteEndElement();
                }
                if (o.ListColumnCount != 3)
                {
                    base.WriteElementStringRaw("ListColumnCount", "", XmlConvert.ToString(o.ListColumnCount));
                }
                if (o.ToolbarsVisible != (PanelToolbar.Item | PanelToolbar.Folder))
                {
                    base.WriteElementString("ToolbarsVisible", "", this.Write19_PanelToolbar(o.ToolbarsVisible));
                }
                if (o.StoreEntry != PanelLayoutEntry.None)
                {
                    base.WriteElementString("StoreEntry", "", this.Write18_PanelLayoutEntry(o.StoreEntry));
                }
                base.WriteElementString("ThumbnailSize", "", o.SerializableThumbnailSize);
                base.WriteElementString("ThumbnailSpacing", "", o.SerializableThumbnailSpacing);
                base.WriteEndElement(o);
            }
        }

        public void Write220_ActivePanel(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ActivePanel", "");
            }
            else
            {
                base.WriteElementString("ActivePanel", "", this.Write23_ActivePanel((ActivePanel) o));
            }
        }

        public void Write221_TwoPanelLayoutEntry(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("TwoPanelLayoutEntry", "");
            }
            else
            {
                base.WriteElementString("TwoPanelLayoutEntry", "", this.Write24_TwoPanelLayoutEntry((TwoPanelLayoutEntry) o));
            }
        }

        public void Write222_TwoPanelLayout(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("TwoPanelLayout", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write25_TwoPanelLayout("TwoPanelLayout", "", (TwoPanelLayout) o, true, false);
            }
        }

        public void Write223_CustomActionLink(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CustomActionLink", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write26_CustomActionLink("CustomActionLink", "", (CustomActionLink) o, true, false);
            }
        }

        public void Write224_CustomBindActionLink(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CustomBindActionLink", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write27_CustomBindActionLink("CustomBindActionLink", "", (CustomBindActionLink) o, true, false);
            }
        }

        public void Write225_ActionState(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ActionState", "");
            }
            else
            {
                base.WriteElementString("ActionState", "", this.Write28_ActionState((ActionState) o));
            }
        }

        public void Write226_BindActionProperty(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("BindActionProperty", "");
            }
            else
            {
                base.WriteElementString("BindActionProperty", "", this.Write29_BindActionProperty((BindActionProperty) o));
            }
        }

        public void Write227_BreadcrumbView(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("BreadcrumbView", "");
            }
            else
            {
                base.WriteElementString("BreadcrumbView", "", this.Write30_BreadcrumbView((BreadcrumbView) o));
            }
        }

        public void Write228_BreadcrumbToolStripRenderer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("BreadcrumbToolStripRenderer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write32_BreadcrumbToolStripRenderer("BreadcrumbToolStripRenderer", "", (BreadcrumbToolStripRenderer) o, true, false);
            }
        }

        public void Write229_InputDialogOption(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("InputDialogOption", "");
            }
            else
            {
                base.WriteElementString("InputDialogOption", "", this.Write33_InputDialogOption((InputDialogOption) o));
            }
        }

        private string Write23_ActivePanel(ActivePanel v)
        {
            switch (v)
            {
                case ActivePanel.Unchanged:
                    return "Unchanged";

                case ActivePanel.Left:
                    return "Left";

                case ActivePanel.Right:
                    return "Right";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Configuration.ActivePanel");
        }

        public void Write230_ElevatedProcess(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ElevatedProcess", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write34_ElevatedProcess("ElevatedProcess", "", (ElevatedProcess) o, true, false);
            }
        }

        public void Write231_ArchiveFormatConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ArchiveFormatConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write35_ArchiveFormatConverter("ArchiveFormatConverter", "", (ArchiveFormatConverter) o, true, false);
            }
        }

        public void Write232_ArchiveFormatCapabilities(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ArchiveFormatCapabilities", "");
            }
            else
            {
                base.WriteElementString("ArchiveFormatCapabilities", "", this.Write36_ArchiveFormatCapabilities((ArchiveFormatCapabilities) o));
            }
        }

        public void Write233_ArchiveFormatInfo(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ArchiveFormatInfo", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write37_ArchiveFormatInfo("ArchiveFormatInfo", "", (ArchiveFormatInfo) o, true, false);
            }
        }

        public void Write234_PersistArchiveFormatInfo(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("PersistArchiveFormatInfo", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write38_PersistArchiveFormatInfo("PersistArchiveFormatInfo", "", (PersistArchiveFormatInfo) o, true, false);
            }
        }

        public void Write235_FindFormatSource(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("FindFormatSource", "");
            }
            else
            {
                base.WriteElementString("FindFormatSource", "", this.Write39_FindFormatSource((FindFormatSource) o));
            }
        }

        public void Write236_ArjHeader(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ArjHeader", "");
            }
            else
            {
                this.Write40_ArjHeader("ArjHeader", "", (ArjHeader) o, false);
            }
        }

        public void Write237_ProcessItemEventArgs(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ProcessItemEventArgs", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write43_ProcessItemEventArgs("ProcessItemEventArgs", "", (ProcessItemEventArgs) o, true, false);
            }
        }

        public void Write238_ProcessorState(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ProcessorState", "");
            }
            else
            {
                base.WriteElementString("ProcessorState", "", this.Write44_ProcessorState((ProcessorState) o));
            }
        }

        public void Write239_SequenseProcessorType(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("SequenseProcessorType", "");
            }
            else
            {
                base.WriteElementString("SequenseProcessorType", "", this.Write45_SequenseProcessorType((SequenseProcessorType) o));
            }
        }

        private string Write24_TwoPanelLayoutEntry(TwoPanelLayoutEntry v)
        {
            switch (v)
            {
                case TwoPanelLayoutEntry.ActivePanel:
                    return "ActivePanel";

                case TwoPanelLayoutEntry.All:
                    return "All";

                case TwoPanelLayoutEntry.OnePanel:
                    return "OnePanel";

                case TwoPanelLayoutEntry.PanelsOrientation:
                    return "PanelsOrientation";

                case TwoPanelLayoutEntry.LeftLayout:
                    return "LeftLayout";

                case TwoPanelLayoutEntry.RightLayout:
                    return "RightLayout";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "OnePanel", "PanelsOrientation", "LeftLayout", "RightLayout", "ActivePanel", "All" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x1fL }, "Nomad.Configuration.TwoPanelLayoutEntry");
        }

        public void Write240_PK_OM(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PK_OM", "");
            }
            else
            {
                base.WriteElementString("PK_OM", "", this.Write46_PK_OM((PK_OM) o));
            }
        }

        public void Write241_PK_OPERATION(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PK_OPERATION", "");
            }
            else
            {
                base.WriteElementString("PK_OPERATION", "", this.Write47_PK_OPERATION((PK_OPERATION) o));
            }
        }

        public void Write242_PK_CAPS(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PK_CAPS", "");
            }
            else
            {
                base.WriteElementString("PK_CAPS", "", this.Write48_PK_CAPS((PK_CAPS) o));
            }
        }

        public void Write243_PK_VOL(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PK_VOL", "");
            }
            else
            {
                base.WriteElementString("PK_VOL", "", this.Write49_PK_VOL((PK_VOL) o));
            }
        }

        public void Write244_PK_PACK(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PK_PACK", "");
            }
            else
            {
                base.WriteElementString("PK_PACK", "", this.Write50_PK_PACK((PK_PACK) o));
            }
        }

        public void Write245_PackDefaultParamStruct(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PackDefaultParamStruct", "");
            }
            else
            {
                this.Write51_PackDefaultParamStruct("PackDefaultParamStruct", "", (PackDefaultParamStruct) o, false);
            }
        }

        public void Write246_WcxErrors(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("WcxErrors", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write52_WcxErrors("WcxErrors", "", (WcxErrors) o, true, false);
            }
        }

        public void Write247_DefaultIcon(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("DefaultIcon", "");
            }
            else
            {
                base.WriteElementString("DefaultIcon", "", this.Write53_DefaultIcon((DefaultIcon) o));
            }
        }

        public void Write248_ShellImageProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ShellImageProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write54_ShellImageProvider("ShellImageProvider", "", (ShellImageProvider) o, true, false);
            }
        }

        public void Write249_ItemCapability(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ItemCapability", "");
            }
            else
            {
                base.WriteElementString("ItemCapability", "", this.Write55_ItemCapability((FileSystemItem.ItemCapability) o));
            }
        }

        private void Write25_TwoPanelLayout(string n, string ns, TwoPanelLayout o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(TwoPanelLayout)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("TwoPanelLayout", "");
                }
                base.WriteAttribute("name", "", o.Name);
                if (o.OnePanel)
                {
                    base.WriteElementStringRaw("OnePanel", "", XmlConvert.ToString(o.OnePanel));
                }
                if (o.PanelsOrientation != Orientation.Vertical)
                {
                    base.WriteElementString("PanelsOrientation", "", this.Write20_Orientation(o.PanelsOrientation));
                }
                if (o.SplitterPercent != 500)
                {
                    base.WriteElementStringRaw("SplitterPercent", "", XmlConvert.ToString(o.SplitterPercent));
                }
                if (o.ActivePanel != ActivePanel.Unchanged)
                {
                    base.WriteElementString("ActivePanel", "", this.Write23_ActivePanel(o.ActivePanel));
                }
                base.WriteElementString("StoreEntry", "", this.Write24_TwoPanelLayoutEntry(o.StoreEntry));
                this.Write22_PanelLayout("LeftLayout", "", o.LeftLayout, false, false);
                this.Write22_PanelLayout("RightLayout", "", o.RightLayout, false, false);
                base.WriteEndElement(o);
            }
        }

        public void Write250_LocalFileSystemCreator(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("LocalFileSystemCreator", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write56_LocalFileSystemCreator("LocalFileSystemCreator", "", (LocalFileSystemCreator) o, true, false);
            }
        }

        public void Write251_NetworkFileSystemCreator(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("NetworkFileSystemCreator", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write57_NetworkFileSystemCreator("NetworkFileSystemCreator", "", (NetworkFileSystemCreator) o, true, false);
            }
        }

        public void Write252_ShellFileSystemCreator(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ShellFileSystemCreator", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write58_ShellFileSystemCreator("ShellFileSystemCreator", "", (ShellFileSystemCreator) o, true, false);
            }
        }

        public void Write253_ContentFlag(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ContentFlag", "");
            }
            else
            {
                base.WriteElementString("ContentFlag", "", this.Write59_ContentFlag((ContentFlag) o));
            }
        }

        public void Write254_ContentDefaultParamStruct(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ContentDefaultParamStruct", "");
            }
            else
            {
                this.Write60_ContentDefaultParamStruct("ContentDefaultParamStruct", "", (ContentDefaultParamStruct) o, false);
            }
        }

        public void Write255_tdateformat(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("tdateformat", "");
            }
            else
            {
                this.Write61_tdateformat("tdateformat", "", (tdateformat) o, false);
            }
        }

        public void Write256_ttimeformat(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ttimeformat", "");
            }
            else
            {
                this.Write62_ttimeformat("ttimeformat", "", (ttimeformat) o, false);
            }
        }

        public void Write257_WdxFieldInfo(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("WdxFieldInfo", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write63_WdxFieldInfo("WdxFieldInfo", "", (WdxFieldInfo) o, true, false);
            }
        }

        public void Write258_AggregatedFilterCondition(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("AggregatedFilterCondition", "");
            }
            else
            {
                base.WriteElementString("AggregatedFilterCondition", "", this.Write64_AggregatedFilterCondition((AggregatedFilterCondition) o));
            }
        }

        public void Write259_AggregatedVirtualItemFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("AggregatedVirtualItemFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write108_AggregatedVirtualItemFilter("AggregatedVirtualItemFilter", "", (AggregatedVirtualItemFilter) o, true, false);
            }
        }

        private void Write26_CustomActionLink(string n, string ns, CustomActionLink o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(CustomActionLink))
                {
                    if (type != typeof(CustomBindActionLink))
                    {
                        throw base.CreateUnknownTypeException(o);
                    }
                    this.Write27_CustomBindActionLink(n, ns, (CustomBindActionLink) o, isNullable, true);
                }
            }
        }

        public void Write260_FilterContainer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("FilterContainer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write109_FilterContainer("FilterContainer", "", (FilterContainer) o, true, false);
            }
        }

        public void Write261_NamedFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("NamedFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write110_NamedFilter("NamedFilter", "", (NamedFilter) o, true, false);
            }
        }

        public void Write262_FilterHelper(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("FilterHelper", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write111_FilterHelper("FilterHelper", "", (FilterHelper) o, true, false);
            }
        }

        public void Write263_VirtualItemNameFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemNameFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write103_VirtualItemNameFilter("VirtualItemNameFilter", "", (VirtualItemNameFilter) o, true, false);
            }
        }

        public void Write264_VirtualItemFullNameFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemFullNameFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write112_VirtualItemFullNameFilter("VirtualItemFullNameFilter", "", (VirtualItemFullNameFilter) o, true, false);
            }
        }

        public void Write265_VirtualItemAttributeFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemAttributeFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write94_VirtualItemAttributeFilter("VirtualItemAttributeFilter", "", (VirtualItemAttributeFilter) o, true, false);
            }
        }

        public void Write266_VirtualItemSizeFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemSizeFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write98_VirtualItemSizeFilter("VirtualItemSizeFilter", "", (VirtualItemSizeFilter) o, true, false);
            }
        }

        public void Write267_ItemDateTimePart(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ItemDateTimePart", "");
            }
            else
            {
                base.WriteElementString("ItemDateTimePart", "", this.Write90_ItemDateTimePart((ItemDateTimePart) o));
            }
        }

        public void Write268_VirtualItemDateFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemDateFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write99_VirtualItemDateFilter("VirtualItemDateFilter", "", (VirtualItemDateFilter) o, true, false);
            }
        }

        public void Write269_VirtualItemTimeFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemTimeFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write91_VirtualItemTimeFilter("VirtualItemTimeFilter", "", (VirtualItemTimeFilter) o, true, false);
            }
        }

        private void Write27_CustomBindActionLink(string n, string ns, CustomBindActionLink o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType && (o.GetType() != typeof(CustomBindActionLink)))
            {
                throw base.CreateUnknownTypeException(o);
            }
        }

        public void Write270_VirtualItemContentFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemContentFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write87_VirtualItemContentFilter("VirtualItemContentFilter", "", (VirtualItemContentFilter) o, true, false);
            }
        }

        public void Write271_VirtualItemHexContentFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemHexContentFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write105_VirtualItemHexContentFilter("VirtualItemHexContentFilter", "", (VirtualItemHexContentFilter) o, true, false);
            }
        }

        public void Write272_NameListCondition(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("NameListCondition", "");
            }
            else
            {
                base.WriteElementString("NameListCondition", "", this.Write106_NameListCondition((NameListCondition) o));
            }
        }

        public void Write273_VirtualItemNameListFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualItemNameListFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write107_VirtualItemNameListFilter("VirtualItemNameListFilter", "", (VirtualItemNameListFilter) o, true, false);
            }
        }

        public void Write274_VirtualPropertyFilter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualPropertyFilter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write84_VirtualPropertyFilter("VirtualPropertyFilter", "", (VirtualPropertyFilter) o, true, false);
            }
        }

        public void Write275_VirtualHighligher(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualHighligher", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write114_VirtualHighligher("VirtualHighligher", "", (VirtualHighligher) o, true, false);
            }
        }

        public void Write276_ListViewHighlighter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ListViewHighlighter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write115_ListViewHighlighter("ListViewHighlighter", "", (ListViewHighlighter) o, true, false);
            }
        }

        public void Write277_HighlighterIconType(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("HighlighterIconType", "");
            }
            else
            {
                base.WriteElementString("HighlighterIconType", "", this.Write113_HighlighterIconType((HighlighterIconType) o));
            }
        }

        public void Write278_HashPropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("HashPropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write116_HashPropertyProvider("HashPropertyProvider", "", (HashPropertyProvider) o, true, false);
            }
        }

        public void Write279_VistaThumbnailProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VistaThumbnailProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write117_VistaThumbnailProvider("VistaThumbnailProvider", "", (VistaThumbnailProvider) o, true, false);
            }
        }

        private string Write28_ActionState(ActionState v)
        {
            switch (v)
            {
                case ActionState.None:
                    return "None";

                case ActionState.Enabled:
                    return "Enabled";

                case ActionState.Visible:
                    return "Visible";

                case ActionState.Checked:
                    return "Checked";

                case ActionState.Indeterminate:
                    return "Indeterminate";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "None", "Enabled", "Visible", "Checked", "Indeterminate" }, new long[] { 0L, 1L, 2L, 4L, 8L }, "Nomad.Controls.Actions.ActionState");
        }

        public void Write280_CustomizeFolderParts(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CustomizeFolderParts", "");
            }
            else
            {
                base.WriteElementString("CustomizeFolderParts", "", this.Write118_CustomizeFolderParts((CustomizeFolderParts) o));
            }
        }

        public void Write281_ColorSpace(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ColorSpace", "");
            }
            else
            {
                base.WriteElementString("ColorSpace", "", this.Write119_ColorSpace((ColorSpace) o));
            }
        }

        public void Write282_DescriptionPropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("DescriptionPropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write120_DescriptionPropertyProvider("DescriptionPropertyProvider", "", (DescriptionPropertyProvider) o, true, false);
            }
        }

        public void Write283_DummyClientSite(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("DummyClientSite", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write121_DummyClientSite("DummyClientSite", "", (DummyClientSite) o, true, false);
            }
        }

        public void Write284_HtmlPropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("HtmlPropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write122_HtmlPropertyProvider("HtmlPropertyProvider", "", (HtmlPropertyProvider) o, true, false);
            }
        }

        public void Write285_BitrateTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("BitrateTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write124_BitrateTypeConverter("BitrateTypeConverter", "", (BitrateTypeConverter) o, true, false);
            }
        }

        public void Write286_AudioChannelsTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("AudioChannelsTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write125_AudioChannelsTypeConverter("AudioChannelsTypeConverter", "", (AudioChannelsTypeConverter) o, true, false);
            }
        }

        public void Write287_AudioSampleRateTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("AudioSampleRateTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write126_AudioSampleRateTypeConverter("AudioSampleRateTypeConverter", "", (AudioSampleRateTypeConverter) o, true, false);
            }
        }

        public void Write288_DurationTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("DurationTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write127_DurationTypeConverter("DurationTypeConverter", "", (DurationTypeConverter) o, true, false);
            }
        }

        public void Write289_ImageSizeTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ImageSizeTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write128_ImageSizeTypeConverter("ImageSizeTypeConverter", "", (ImageSizeTypeConverter) o, true, false);
            }
        }

        private string Write29_BindActionProperty(BindActionProperty v)
        {
            switch (v)
            {
                case BindActionProperty.CanClick:
                    return "CanClick";

                case BindActionProperty.CanUpdate:
                    return "CanUpdate";

                case BindActionProperty.All:
                    return "All";

                case BindActionProperty.None:
                    return "None";

                case BindActionProperty.Enabled:
                    return "Enabled";

                case BindActionProperty.Text:
                    return "Text";

                case BindActionProperty.Visible:
                    return "Visible";

                case BindActionProperty.Checked:
                    return "Checked";

                case BindActionProperty.Image:
                    return "Image";

                case BindActionProperty.Shortcuts:
                    return "Shortcuts";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "None", "Enabled", "Text", "Visible", "Checked", "Image", "Shortcuts", "CanClick", "CanUpdate", "All" }, new long[] { 0L, 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x80L, 0xffL }, "Nomad.Controls.Actions.BindActionProperty");
        }

        public void Write290_DPITypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("DPITypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write129_DPITypeConverter("DPITypeConverter", "", (DPITypeConverter) o, true, false);
            }
        }

        public void Write291_ISOSpeedTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ISOSpeedTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write130_ISOSpeedTypeConverter("ISOSpeedTypeConverter", "", (ISOSpeedTypeConverter) o, true, false);
            }
        }

        public void Write292_RatingTypeConverter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("RatingTypeConverter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write131_RatingTypeConverter("RatingTypeConverter", "", (RatingTypeConverter) o, true, false);
            }
        }

        public void Write293_EncodingConveter(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("EncodingConveter", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write132_EncodingConveter("EncodingConveter", "", (EncodingConveter) o, true, false);
            }
        }

        public void Write294_ImagePropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ImagePropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write133_ImagePropertyProvider("ImagePropertyProvider", "", (ImagePropertyProvider) o, true, false);
            }
        }

        public void Write295_PsdPropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("PsdPropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write134_PsdPropertyProvider("PsdPropertyProvider", "", (PsdPropertyProvider) o, true, false);
            }
        }

        public void Write296_TagLibPropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("TagLibPropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write135_TagLibPropertyProvider("TagLibPropertyProvider", "", (TagLibPropertyProvider) o, true, false);
            }
        }

        public void Write297_TextPropertyProvider(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("TextPropertyProvider", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write136_TextPropertyProvider("TextPropertyProvider", "", (TextPropertyProvider) o, true, false);
            }
        }

        public void Write298_VirtualToolTip(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualToolTip", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write137_VirtualToolTip("VirtualToolTip", "", (VirtualToolTip) o, true, false);
            }
        }

        public void Write299_ThrobberStyle(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ThrobberStyle", "");
            }
            else
            {
                base.WriteElementString("ThrobberStyle", "", this.Write138_ThrobberStyle((ThrobberStyle) o));
            }
        }

        private void Write3_CustomImageProvider(string n, string ns, CustomImageProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(CustomImageProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("CustomImageProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write30_BreadcrumbView(BreadcrumbView v)
        {
            switch (v)
            {
                case BreadcrumbView.Breadcrumb:
                    return "Breadcrumb";

                case BreadcrumbView.Drives:
                    return "Drives";

                case BreadcrumbView.SimpleText:
                    return "SimpleText";

                case BreadcrumbView.EnterPath:
                    return "EnterPath";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Controls.Specialized.BreadcrumbView");
        }

        public void Write300_ThrobberRenderer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ThrobberRenderer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write140_ThrobberRenderer("ThrobberRenderer", "", (ThrobberRenderer) o, true, false);
            }
        }

        public void Write301_AutoRefreshMode(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("AutoRefreshMode", "");
            }
            else
            {
                base.WriteElementString("AutoRefreshMode", "", this.Write141_AutoRefreshMode((AutoRefreshMode) o));
            }
        }

        public void Write302_FtpFileSystemCreator(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("FtpFileSystemCreator", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write142_FtpFileSystemCreator("FtpFileSystemCreator", "", (FtpFileSystemCreator) o, true, false);
            }
        }

        public void Write303_NullFileSystemCreator(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("NullFileSystemCreator", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write143_NullFileSystemCreator("NullFileSystemCreator", "", (NullFileSystemCreator) o, true, false);
            }
        }

        public void Write304_CustomVirtualFolder(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CustomVirtualFolder", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write144_CustomVirtualFolder("CustomVirtualFolder", "", (CustomVirtualFolder) o, true, false);
            }
        }

        public void Write305_CanMoveResult(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CanMoveResult", "");
            }
            else
            {
                base.WriteElementString("CanMoveResult", "", this.Write145_CanMoveResult((CanMoveResult) o));
            }
        }

        public void Write306_IconOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("IconOptions", "");
            }
            else
            {
                base.WriteElementString("IconOptions", "", this.Write146_IconOptions((IconOptions) o));
            }
        }

        public void Write307_DelayedExtractMode(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("DelayedExtractMode", "");
            }
            else
            {
                base.WriteElementString("DelayedExtractMode", "", this.Write147_DelayedExtractMode((DelayedExtractMode) o));
            }
        }

        public void Write308_PathView(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PathView", "");
            }
            else
            {
                base.WriteElementString("PathView", "", this.Write148_PathView((PathView) o));
            }
        }

        public void Write309_PanelView(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PanelView", "");
            }
            else
            {
                base.WriteElementString("PanelView", "", this.Write21_PanelView((PanelView) o));
            }
        }

        private void Write31_ToolStripWrapperRenderer(string n, string ns, ToolStripWrapperRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(ToolStripWrapperRenderer))
                    {
                        if (type != typeof(BreadcrumbToolStripRenderer))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write32_BreadcrumbToolStripRenderer(n, ns, (BreadcrumbToolStripRenderer) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ToolStripWrapperRenderer", "");
                }
                base.WriteEndElement(o);
            }
        }

        public void Write310_ContextMenuOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ContextMenuOptions", "");
            }
            else
            {
                base.WriteElementString("ContextMenuOptions", "", this.Write149_ContextMenuOptions((ContextMenuOptions) o));
            }
        }

        public void Write311_VirtualIcon(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VirtualIcon", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write150_VirtualIcon("VirtualIcon", "", (VirtualIcon) o, true, false);
            }
        }

        public void Write312_ArrayOfPropertyValue(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("ArrayOfPropertyValue", "");
            }
            else
            {
                base.TopLevelElement();
                PropertyValueList list = (PropertyValueList) o;
                if (list == null)
                {
                    base.WriteNullTagLiteral("ArrayOfPropertyValue", "");
                }
                else
                {
                    base.WriteStartElement("ArrayOfPropertyValue", "", null, false);
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.Write151_PropertyValue("PropertyValue", "", list[i], true, false);
                    }
                    base.WriteEndElement();
                }
            }
        }

        public void Write313_PropertyValue(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("PropertyValue", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write151_PropertyValue("PropertyValue", "", (PropertyValue) o, true, false);
            }
        }

        public void Write314_SimpleEncrypt(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("SimpleEncrypt", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write152_SimpleEncrypt("SimpleEncrypt", "", (SimpleEncrypt) o, true, false);
            }
        }

        public void Write315_ProgressRenderMode(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ProgressRenderMode", "");
            }
            else
            {
                base.WriteElementString("ProgressRenderMode", "", this.Write153_ProgressRenderMode((ProgressRenderMode) o));
            }
        }

        public void Write316_ProgressState(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ProgressState", "");
            }
            else
            {
                base.WriteElementString("ProgressState", "", this.Write154_ProgressState((ProgressState) o));
            }
        }

        public void Write317_VistaProgressBarRenderer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("VistaProgressBarRenderer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write155_VistaProgressBarRenderer("VistaProgressBarRenderer", "", (VistaProgressBarRenderer) o, true, false);
            }
        }

        public void Write318_MarqueeStyle(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("MarqueeStyle", "");
            }
            else
            {
                base.WriteElementString("MarqueeStyle", "", this.Write156_MarqueeStyle((MarqueeStyle) o));
            }
        }

        public void Write319_XPProgressBarRenderer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("XPProgressBarRenderer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write157_XPProgressBarRenderer("XPProgressBarRenderer", "", (XPProgressBarRenderer) o, true, false);
            }
        }

        private void Write32_BreadcrumbToolStripRenderer(string n, string ns, BreadcrumbToolStripRenderer o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(BreadcrumbToolStripRenderer)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("BreadcrumbToolStripRenderer", "");
                }
                base.WriteEndElement(o);
            }
        }

        public void Write320_AskMode(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("AskMode", "");
            }
            else
            {
                base.WriteElementString("AskMode", "", this.Write158_AskMode((AskMode) o));
            }
        }

        public void Write321_OperationResult(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("OperationResult", "");
            }
            else
            {
                base.WriteElementString("OperationResult", "", this.Write159_OperationResult((OperationResult) o));
            }
        }

        public void Write322_ItemPropId(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ItemPropId", "");
            }
            else
            {
                base.WriteElementString("ItemPropId", "", this.Write160_ItemPropId((ItemPropId) o));
            }
        }

        public void Write323_FileTimeType(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("FileTimeType", "");
            }
            else
            {
                base.WriteElementString("FileTimeType", "", this.Write161_FileTimeType((FileTimeType) o));
            }
        }

        public void Write324_ArchivePropId(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ArchivePropId", "");
            }
            else
            {
                base.WriteElementString("ArchivePropId", "", this.Write162_ArchivePropId((ArchivePropId) o));
            }
        }

        public void Write325_KnownSevenZipFormat(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("KnownSevenZipFormat", "");
            }
            else
            {
                base.WriteElementString("KnownSevenZipFormat", "", this.Write163_KnownSevenZipFormat((KnownSevenZipFormat) o));
            }
        }

        public void Write326_SevenZipFormatCapabilities(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("SevenZipFormatCapabilities", "");
            }
            else
            {
                base.WriteElementString("SevenZipFormatCapabilities", "", this.Write164_SevenZipFormatCapabilities((SevenZipFormatCapabilities) o));
            }
        }

        public void Write327_CompressionLevel(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CompressionLevel", "");
            }
            else
            {
                base.WriteElementString("CompressionLevel", "", this.Write165_CompressionLevel((CompressionLevel) o));
            }
        }

        public void Write328_CompressionMethod(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CompressionMethod", "");
            }
            else
            {
                base.WriteElementString("CompressionMethod", "", this.Write166_CompressionMethod((CompressionMethod) o));
            }
        }

        public void Write329_EncryptionMethod(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("EncryptionMethod", "");
            }
            else
            {
                base.WriteElementString("EncryptionMethod", "", this.Write167_EncryptionMethod((EncryptionMethod) o));
            }
        }

        private string Write33_InputDialogOption(InputDialogOption v)
        {
            switch (v)
            {
                case InputDialogOption.AllowEmptyValue:
                    return "AllowEmptyValue";

                case InputDialogOption.AllowSameValue:
                    return "AllowSameValue";

                case InputDialogOption.ReadOnly:
                    return "ReadOnly";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "AllowEmptyValue", "AllowSameValue", "ReadOnly" }, new long[] { 1L, 2L, 4L }, "Nomad.Dialogs.InputDialogOption");
        }

        public void Write330_SolidSizeUnit(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("SolidSizeUnit", "");
            }
            else
            {
                base.WriteElementString("SolidSizeUnit", "", this.Write168_SolidSizeUnit((SevenZipPropertiesBuilder.SolidSizeUnit) o));
            }
        }

        public void Write331_ComplexFilterView(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ComplexFilterView", "");
            }
            else
            {
                base.WriteElementString("ComplexFilterView", "", this.Write169_ComplexFilterView((ComplexFilterView) o));
            }
        }

        public void Write332_ViewFilters(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ViewFilters", "");
            }
            else
            {
                base.WriteElementString("ViewFilters", "", this.Write170_ViewFilters((ViewFilters) o));
            }
        }

        public void Write333_PanelContentContainer(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("PanelContentContainer", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write172_PanelContentContainer("PanelContentContainer", "", (PanelContentContainer) o, true, false);
            }
        }

        public void Write334_ControllerType(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ControllerType", "");
            }
            else
            {
                base.WriteElementString("ControllerType", "", this.Write173_ControllerType((ControllerType) o));
            }
        }

        public void Write335_Controller(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("Controller", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write175_Controller("Controller", "", (Controller) o, true, false);
            }
        }

        public void Write336_FormPlacement(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("FormPlacement", "");
            }
            else
            {
                base.WriteElementString("FormPlacement", "", this.Write176_FormPlacement((FormPlacement) o));
            }
        }

        public void Write337_ArgumentKey(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ArgumentKey", "");
            }
            else
            {
                base.WriteElementString("ArgumentKey", "", this.Write177_ArgumentKey((ArgumentKey) o));
            }
        }

        public void Write338_CanMoveListViewItem(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CanMoveListViewItem", "");
            }
            else
            {
                base.WriteElementString("CanMoveListViewItem", "", this.Write178_CanMoveListViewItem((CanMoveListViewItem) o));
            }
        }

        public void Write339_Trace(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("Trace", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write179_Trace("Trace", "", (Trace) o, true, false);
            }
        }

        private void Write34_ElevatedProcess(string n, string ns, ElevatedProcess o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ElevatedProcess)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ElevatedProcess", "");
                }
                base.WriteElementStringRaw("KeepAlive", "", XmlConvert.ToString(o.KeepAlive));
                base.WriteEndElement(o);
            }
        }

        public void Write340_SinglePanel(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("SinglePanel", "");
            }
            else
            {
                base.WriteElementString("SinglePanel", "", this.Write180_SinglePanel((TwoPanelContainer.SinglePanel) o));
            }
        }

        public void Write341_GeneralTab(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("GeneralTab", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write181_GeneralTab("GeneralTab", "", (GeneralTab) o, true, false);
            }
        }

        public void Write342_TwoPanelTab(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("TwoPanelTab", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write182_TwoPanelTab("TwoPanelTab", "", (TwoPanelTab) o, true, false);
            }
        }

        public void Write343_ArchiveUpdateMode(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ArchiveUpdateMode", "");
            }
            else
            {
                base.WriteElementString("ArchiveUpdateMode", "", this.Write183_ArchiveUpdateMode((ArchiveUpdateMode) o));
            }
        }

        public void Write344_PackStage(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("PackStage", "");
            }
            else
            {
                base.WriteElementString("PackStage", "", this.Write184_PackStage((PackStage) o));
            }
        }

        public void Write345_PackProgressSnapshot(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("PackProgressSnapshot", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write187_PackProgressSnapshot("PackProgressSnapshot", "", (PackProgressSnapshot) o, true, false);
            }
        }

        public void Write346_CustomBackgroundWorker(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CustomBackgroundWorker", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write188_CustomBackgroundWorker("CustomBackgroundWorker", "", (CustomBackgroundWorker) o, true, false);
            }
        }

        public void Write347_EventBackgroundWorker(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("EventBackgroundWorker", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write189_EventBackgroundWorker("EventBackgroundWorker", "", (EventBackgroundWorker) o, true, false);
            }
        }

        public void Write348_CopyDestinationItem(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CopyDestinationItem", "");
            }
            else
            {
                base.WriteElementString("CopyDestinationItem", "", this.Write190_CopyDestinationItem((CopyDestinationItem) o));
            }
        }

        public void Write349_MessageDialogResult(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("MessageDialogResult", "");
            }
            else
            {
                base.WriteElementString("MessageDialogResult", "", this.Write191_MessageDialogResult((MessageDialogResult) o));
            }
        }

        private void Write35_ArchiveFormatConverter(string n, string ns, ArchiveFormatConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ArchiveFormatConverter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ArchiveFormatConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        public void Write350_DoubleClickAction(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("DoubleClickAction", "");
            }
            else
            {
                base.WriteElementString("DoubleClickAction", "", this.Write192_DoubleClickAction((DoubleClickAction) o));
            }
        }

        public void Write351_QuickFindOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("QuickFindOptions", "");
            }
            else
            {
                base.WriteElementString("QuickFindOptions", "", this.Write171_QuickFindOptions((QuickFindOptions) o));
            }
        }

        public void Write352_ListViewSort(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ListViewSort", "");
            }
            else
            {
                base.WriteElementString("ListViewSort", "", this.Write193_ListViewSort((VirtualFilePanel.ListViewSort) o));
            }
        }

        public void Write353_CustomAsyncFolder(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CustomAsyncFolder", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write194_CustomAsyncFolder("CustomAsyncFolder", "", (CustomAsyncFolder) o, true, false);
            }
        }

        public void Write354_SearchFolderOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("SearchFolderOptions", "");
            }
            else
            {
                base.WriteElementString("SearchFolderOptions", "", this.Write195_SearchFolderOptions((SearchFolderOptions) o));
            }
        }

        public void Write355_FindDuplicateOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("FindDuplicateOptions", "");
            }
            else
            {
                base.WriteElementString("FindDuplicateOptions", "", this.Write196_FindDuplicateOptions((FindDuplicateOptions) o));
            }
        }

        public void Write356_Compare(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("Compare", "");
            }
            else
            {
                base.WriteElementString("Compare", "", this.Write197_Compare((Compare) o));
            }
        }

        public void Write357_ChangeItemAction(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ChangeItemAction", "");
            }
            else
            {
                base.WriteElementString("ChangeItemAction", "", this.Write198_ChangeItemAction((ChangeItemAction) o));
            }
        }

        public void Write358_AvailableItemActions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("AvailableItemActions", "");
            }
            else
            {
                base.WriteElementString("AvailableItemActions", "", this.Write199_AvailableItemActions((AvailableItemActions) o));
            }
        }

        public void Write359_CompareFoldersOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CompareFoldersOptions", "");
            }
            else
            {
                base.WriteElementString("CompareFoldersOptions", "", this.Write200_CompareFoldersOptions((CompareFoldersOptions) o));
            }
        }

        private string Write36_ArchiveFormatCapabilities(ArchiveFormatCapabilities v)
        {
            switch (v)
            {
                case ArchiveFormatCapabilities.DetectFormatByContent:
                    return "DetectFormatByContent";

                case ArchiveFormatCapabilities.CreateArchive:
                    return "CreateArchive";

                case ArchiveFormatCapabilities.UpdateArchive:
                    return "UpdateArchive";

                case ArchiveFormatCapabilities.MultiFileArchive:
                    return "MultiFileArchive";

                case ArchiveFormatCapabilities.EncryptContent:
                    return "EncryptContent";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "DetectFormatByContent", "CreateArchive", "UpdateArchive", "MultiFileArchive", "EncryptContent" }, new long[] { 1L, 2L, 4L, 8L, 0x10L }, "Nomad.FileSystem.Archive.Common.ArchiveFormatCapabilities");
        }

        public void Write360_OverwriteDialogResult(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("OverwriteDialogResult", "");
            }
            else
            {
                base.WriteElementString("OverwriteDialogResult", "", this.Write201_OverwriteDialogResult((OverwriteDialogResult) o));
            }
        }

        public void Write361_CopyWorkerOptions(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CopyWorkerOptions", "");
            }
            else
            {
                base.WriteElementString("CopyWorkerOptions", "", this.Write202_CopyWorkerOptions((CopyWorkerOptions) o));
            }
        }

        public void Write362_CopyMode(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("CopyMode", "");
            }
            else
            {
                base.WriteElementString("CopyMode", "", this.Write203_CopyMode((CopyMode) o));
            }
        }

        public void Write363_ProcessedSize(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("ProcessedSize", "");
            }
            else
            {
                this.Write185_ProcessedSize("ProcessedSize", "", (ProcessedSize) o, false);
            }
        }

        public void Write364_CopyProgressSnapshot(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteNullTagLiteral("CopyProgressSnapshot", "");
            }
            else
            {
                base.TopLevelElement();
                this.Write204_CopyProgressSnapshot("CopyProgressSnapshot", "", (CopyProgressSnapshot) o, true, false);
            }
        }

        public void Write365_IconStyle(object o)
        {
            base.WriteStartDocument();
            if (o == null)
            {
                base.WriteEmptyTag("IconStyle", "");
            }
            else
            {
                base.WriteElementString("IconStyle", "", this.Write205_IconStyle((IconStyle) o));
            }
        }

        private void Write37_ArchiveFormatInfo(string n, string ns, ArchiveFormatInfo o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(ArchiveFormatInfo))
                {
                    if (type != typeof(PersistArchiveFormatInfo))
                    {
                        throw base.CreateUnknownTypeException(o);
                    }
                    this.Write38_PersistArchiveFormatInfo(n, ns, (PersistArchiveFormatInfo) o, isNullable, true);
                }
            }
        }

        private void Write38_PersistArchiveFormatInfo(string n, string ns, PersistArchiveFormatInfo o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType && (o.GetType() != typeof(PersistArchiveFormatInfo)))
            {
                throw base.CreateUnknownTypeException(o);
            }
        }

        private string Write39_FindFormatSource(FindFormatSource v)
        {
            switch (v)
            {
                case FindFormatSource.Content:
                    return "Content";

                case FindFormatSource.Extension:
                    return "Extension";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "Content", "Extension" }, new long[] { 1L, 2L }, "Nomad.FileSystem.Archive.Common.FindFormatSource");
        }

        private void Write4_TypeConverter(string n, string ns, TypeConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(TypeConverter))
                    {
                        if (type == typeof(EncodingConveter))
                        {
                            this.Write132_EncodingConveter(n, ns, (EncodingConveter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(AudioChannelsTypeConverter))
                        {
                            this.Write125_AudioChannelsTypeConverter(n, ns, (AudioChannelsTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(PropertyTypeConverter))
                        {
                            this.Write123_PropertyTypeConverter(n, ns, (PropertyTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(RatingTypeConverter))
                        {
                            this.Write131_RatingTypeConverter(n, ns, (RatingTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ISOSpeedTypeConverter))
                        {
                            this.Write130_ISOSpeedTypeConverter(n, ns, (ISOSpeedTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DPITypeConverter))
                        {
                            this.Write129_DPITypeConverter(n, ns, (DPITypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ImageSizeTypeConverter))
                        {
                            this.Write128_ImageSizeTypeConverter(n, ns, (ImageSizeTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(DurationTypeConverter))
                        {
                            this.Write127_DurationTypeConverter(n, ns, (DurationTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(AudioSampleRateTypeConverter))
                        {
                            this.Write126_AudioSampleRateTypeConverter(n, ns, (AudioSampleRateTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(BitrateTypeConverter))
                        {
                            this.Write124_BitrateTypeConverter(n, ns, (BitrateTypeConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(ArchiveFormatConverter))
                        {
                            this.Write35_ArchiveFormatConverter(n, ns, (ArchiveFormatConverter) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(KeysConverter))
                        {
                            this.Write5_KeysConverter(n, ns, (KeysConverter) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(KeysConverter2))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write6_KeysConverter2(n, ns, (KeysConverter2) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("TypeConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write40_ArjHeader(string n, string ns, ArjHeader o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(ArjHeader)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("ArjHeader", "");
            }
            base.WriteElementStringRaw("Mark", "", XmlConvert.ToString(o.Mark));
            base.WriteElementStringRaw("HeadSize", "", XmlConvert.ToString(o.HeadSize));
            base.WriteElementStringRaw("FirstHeadSize", "", XmlConvert.ToString(o.FirstHeadSize));
            base.WriteElementStringRaw("ArjVer", "", XmlConvert.ToString(o.ArjVer));
            base.WriteElementStringRaw("ArjExtrVer", "", XmlConvert.ToString(o.ArjExtrVer));
            base.WriteElementStringRaw("HostOS", "", XmlConvert.ToString(o.HostOS));
            base.WriteElementStringRaw("Flags", "", XmlConvert.ToString(o.Flags));
            base.WriteElementStringRaw("Method", "", XmlConvert.ToString(o.Method));
            base.WriteElementStringRaw("FileType", "", XmlConvert.ToString(o.FileType));
            base.WriteElementStringRaw("Reserved", "", XmlConvert.ToString(o.Reserved));
            base.WriteElementStringRaw("ftime", "", XmlConvert.ToString(o.ftime));
            base.WriteElementStringRaw("PackSize", "", XmlConvert.ToString(o.PackSize));
            base.WriteElementStringRaw("UnpSize", "", XmlConvert.ToString(o.UnpSize));
            base.WriteElementStringRaw("CRC", "", XmlConvert.ToString(o.CRC));
            base.WriteElementStringRaw("FileSpec", "", XmlConvert.ToString(o.FileSpec));
            base.WriteElementStringRaw("AccessMode", "", XmlConvert.ToString(o.AccessMode));
            base.WriteElementStringRaw("HostData", "", XmlConvert.ToString(o.HostData));
            base.WriteEndElement(o);
        }

        private void Write41_EventArgs(string n, string ns, EventArgs o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(EventArgs))
                    {
                        if (type == typeof(CancelEventArgs))
                        {
                            this.Write42_CancelEventArgs(n, ns, (CancelEventArgs) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(ProcessItemEventArgs))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write43_ProcessItemEventArgs(n, ns, (ProcessItemEventArgs) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("EventArgs", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write42_CancelEventArgs(string n, string ns, CancelEventArgs o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(CancelEventArgs))
                    {
                        if (type != typeof(ProcessItemEventArgs))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write43_ProcessItemEventArgs(n, ns, (ProcessItemEventArgs) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("CancelEventArgs", "");
                }
                base.WriteElementStringRaw("Cancel", "", XmlConvert.ToString(o.Cancel));
                base.WriteEndElement(o);
            }
        }

        private void Write43_ProcessItemEventArgs(string n, string ns, ProcessItemEventArgs o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType && (o.GetType() != typeof(ProcessItemEventArgs)))
            {
                throw base.CreateUnknownTypeException(o);
            }
        }

        private string Write44_ProcessorState(ProcessorState v)
        {
            switch (v)
            {
                case ProcessorState.Initializing:
                    return "Initializing";

                case ProcessorState.InProcess:
                    return "InProcess";

                case ProcessorState.Finished:
                    return "Finished";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.Common.ProcessorState");
        }

        private string Write45_SequenseProcessorType(SequenseProcessorType v)
        {
            switch (v)
            {
                case SequenseProcessorType.Extract:
                    return "Extract";

                case SequenseProcessorType.Delete:
                    return "Delete";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.Common.SequenseProcessorType");
        }

        private string Write46_PK_OM(PK_OM v)
        {
            switch (v)
            {
                case PK_OM.PK_OM_LIST:
                    return "PK_OM_LIST";

                case PK_OM.PK_OM_EXTRACT:
                    return "PK_OM_EXTRACT";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.Wcx.PK_OM");
        }

        private string Write47_PK_OPERATION(PK_OPERATION v)
        {
            switch (v)
            {
                case PK_OPERATION.PK_SKIP:
                    return "PK_SKIP";

                case PK_OPERATION.PK_TEST:
                    return "PK_TEST";

                case PK_OPERATION.PK_EXTRACT:
                    return "PK_EXTRACT";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.Wcx.PK_OPERATION");
        }

        private string Write48_PK_CAPS(PK_CAPS v)
        {
            switch (v)
            {
                case PK_CAPS.PK_CAPS_OPTIONS:
                    return "PK_CAPS_OPTIONS";

                case PK_CAPS.PK_CAPS_MEMPACK:
                    return "PK_CAPS_MEMPACK";

                case PK_CAPS.PK_CAPS_NEW:
                    return "PK_CAPS_NEW";

                case PK_CAPS.PK_CAPS_MODIFY:
                    return "PK_CAPS_MODIFY";

                case PK_CAPS.PK_CAPS_MULTIPLE:
                    return "PK_CAPS_MULTIPLE";

                case PK_CAPS.PK_CAPS_DELETE:
                    return "PK_CAPS_DELETE";

                case PK_CAPS.PK_CAPS_BY_CONTENT:
                    return "PK_CAPS_BY_CONTENT";

                case PK_CAPS.PK_CAPS_SEARCHTEXT:
                    return "PK_CAPS_SEARCHTEXT";

                case PK_CAPS.PK_CAPS_HIDE:
                    return "PK_CAPS_HIDE";

                case PK_CAPS.PK_CAPS_ENCRYPT:
                    return "PK_CAPS_ENCRYPT";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "PK_CAPS_NEW", "PK_CAPS_MODIFY", "PK_CAPS_MULTIPLE", "PK_CAPS_DELETE", "PK_CAPS_OPTIONS", "PK_CAPS_MEMPACK", "PK_CAPS_BY_CONTENT", "PK_CAPS_SEARCHTEXT", "PK_CAPS_HIDE", "PK_CAPS_ENCRYPT" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x80L, 0x100L, 0x200L }, "Nomad.FileSystem.Archive.Wcx.PK_CAPS");
        }

        private string Write49_PK_VOL(PK_VOL v)
        {
            switch (v)
            {
                case PK_VOL.PK_VOL_ASK:
                    return "PK_VOL_ASK";

                case PK_VOL.PK_VOL_NOTIFY:
                    return "PK_VOL_NOTIFY";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Archive.Wcx.PK_VOL");
        }

        private void Write5_KeysConverter(string n, string ns, KeysConverter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(KeysConverter))
                    {
                        if (type != typeof(KeysConverter2))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write6_KeysConverter2(n, ns, (KeysConverter2) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("KeysConverter", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write50_PK_PACK(PK_PACK v)
        {
            switch (v)
            {
                case PK_PACK.PK_PACK_MOVE_FILES:
                    return "PK_PACK_MOVE_FILES";

                case PK_PACK.PK_PACK_SAVE_PATHS:
                    return "PK_PACK_SAVE_PATHS";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "PK_PACK_MOVE_FILES", "PK_PACK_SAVE_PATHS" }, new long[] { 1L, 2L }, "Nomad.FileSystem.Archive.Wcx.PK_PACK");
        }

        private void Write51_PackDefaultParamStruct(string n, string ns, PackDefaultParamStruct o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(PackDefaultParamStruct)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("PackDefaultParamStruct", "");
            }
            base.WriteElementStringRaw("size", "", XmlConvert.ToString(o.size));
            base.WriteElementStringRaw("PluginInterfaceVersionLow", "", XmlConvert.ToString(o.PluginInterfaceVersionLow));
            base.WriteElementStringRaw("PluginInterfaceVersionHi", "", XmlConvert.ToString(o.PluginInterfaceVersionHi));
            base.WriteElementString("DefaultIniName", "", o.DefaultIniName);
            base.WriteEndElement(o);
        }

        private void Write52_WcxErrors(string n, string ns, WcxErrors o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(WcxErrors)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("WcxErrors", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write53_DefaultIcon(DefaultIcon v)
        {
            switch (v)
            {
                case DefaultIcon.UnknownFile:
                    return "UnknownFile";

                case DefaultIcon.DefaultDocument:
                    return "DefaultDocument";

                case DefaultIcon.DefaultApplication:
                    return "DefaultApplication";

                case DefaultIcon.Desktop:
                    return "Desktop";

                case DefaultIcon.Drive:
                    return "Drive";

                case DefaultIcon.Favorites:
                    return "Favorites";

                case DefaultIcon.Folder:
                    return "Folder";

                case DefaultIcon.MyComputer:
                    return "MyComputer";

                case DefaultIcon.MyDocuments:
                    return "MyDocuments";

                case DefaultIcon.MyPictures:
                    return "MyPictures";

                case DefaultIcon.MyMusic:
                    return "MyMusic";

                case DefaultIcon.MyVideos:
                    return "MyVideos";

                case DefaultIcon.SearchFolder:
                    return "SearchFolder";

                case DefaultIcon.NetworkNeighborhood:
                    return "NetworkNeighborhood";

                case DefaultIcon.EntireNetwork:
                    return "EntireNetwork";

                case DefaultIcon.NetworkWorkgroup:
                    return "NetworkWorkgroup";

                case DefaultIcon.NetworkProvider:
                    return "NetworkProvider";

                case DefaultIcon.NetworkServer:
                    return "NetworkServer";

                case DefaultIcon.NetworkFolder:
                    return "NetworkFolder";

                case DefaultIcon.OverlayLink:
                    return "OverlayLink";

                case DefaultIcon.OverlayShare:
                    return "OverlayShare";

                case DefaultIcon.OverlayUnreadable:
                    return "OverlayUnreadable";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.DefaultIcon");
        }

        private void Write54_ShellImageProvider(string n, string ns, ShellImageProvider o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ShellImageProvider)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ShellImageProvider", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write55_ItemCapability(FileSystemItem.ItemCapability v)
        {
            switch (v)
            {
                case FileSystemItem.ItemCapability.IsShellFolderShortcut:
                    return "IsShellFolderShortcut";

                case FileSystemItem.ItemCapability.HasShellFolderShortcut:
                    return "HasShellFolderShortcut";

                case FileSystemItem.ItemCapability.Deleted:
                    return "Deleted";

                case FileSystemItem.ItemCapability.None:
                    return "None";

                case FileSystemItem.ItemCapability.HasParent:
                    return "HasParent";

                case FileSystemItem.ItemCapability.HasExtender:
                    return "HasExtender";

                case FileSystemItem.ItemCapability.HasTarget:
                    return "HasTarget";

                case FileSystemItem.ItemCapability.HasSize:
                    return "HasSize";

                case FileSystemItem.ItemCapability.HasThumbnail:
                    return "HasThumbnail";

                case FileSystemItem.ItemCapability.IsParentReal:
                    return "IsParentReal";

                case FileSystemItem.ItemCapability.UseTargetIcon:
                    return "UseTargetIcon";

                case FileSystemItem.ItemCapability.GlobalFileChangedAssigned:
                    return "GlobalFileChangedAssigned";

                case FileSystemItem.ItemCapability.IsShellLink:
                    return "IsShellLink";

                case FileSystemItem.ItemCapability.IsUrlLink:
                    return "IsUrlLink";

                case FileSystemItem.ItemCapability.HasCreationTime:
                    return "HasCreationTime";

                case FileSystemItem.ItemCapability.HasLastWriteTime:
                    return "HasLastWriteTime";

                case FileSystemItem.ItemCapability.HasVolume:
                    return "HasVolume";

                case FileSystemItem.ItemCapability.QueryRemoveAssigned:
                    return "QueryRemoveAssigned";

                case FileSystemItem.ItemCapability.VolumeEventsAssigned:
                    return "VolumeEventsAssigned";

                case FileSystemItem.ItemCapability.HasLastAccessTime:
                    return "HasLastAccessTime";

                case FileSystemItem.ItemCapability.Unreadable:
                    return "Unreadable";

                case FileSystemItem.ItemCapability.GlobalFolderChangedAssigned:
                    return "GlobalFolderChangedAssigned";

                case FileSystemItem.ItemCapability.CheckNetworkShare:
                    return "CheckNetworkShare";

                case FileSystemItem.ItemCapability.DisableContentMap:
                    return "DisableContentMap";

                case FileSystemItem.ItemCapability.ItemRefreshNeeded:
                    return "ItemRefreshNeeded";

                case FileSystemItem.ItemCapability.HasShellItem:
                    return "HasShellItem";

                case FileSystemItem.ItemCapability.HasExtension:
                    return "HasExtension";

                case FileSystemItem.ItemCapability.IsElevated:
                    return "IsElevated";

                case FileSystemItem.ItemCapability.HasContentFolder:
                    return "HasContentFolder";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { 
                "None", "HasParent", "HasExtender", "HasTarget", "HasSize", "HasThumbnail", "IsParentReal", "IsShellFolderShortcut", "HasShellFolderShortcut", "Deleted", "UseTargetIcon", "GlobalFileChangedAssigned", "IsShellLink", "IsUrlLink", "HasCreationTime", "HasLastWriteTime", 
                "HasLastAccessTime", "Unreadable", "GlobalFolderChangedAssigned", "HasVolume", "QueryRemoveAssigned", "VolumeEventsAssigned", "CheckNetworkShare", "DisableContentMap", "ItemRefreshNeeded", "IsElevated", "HasContentFolder", "HasShellItem", "HasExtension"
             }, new long[] { 
                0L, 1L, 2L, 4L, 8L, 0x10L, 0x20L, 0x40L, 0x80L, 0x100L, 0x200L, 0x400L, 0x800L, 0x1000L, 0x2000L, 0x4000L, 
                0x8000L, 0x10000L, 0x20000L, 0x40000L, 0x80000L, 0x100000L, 0x200000L, 0x400000L, 0x800000L, 0x1000000L, 0x2000000L, 0x4000000L, 0x8000000L
             }, "Nomad.FileSystem.LocalFile.FileSystemItem.ItemCapability");
        }

        private void Write56_LocalFileSystemCreator(string n, string ns, LocalFileSystemCreator o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(LocalFileSystemCreator)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("LocalFileSystemCreator", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write57_NetworkFileSystemCreator(string n, string ns, NetworkFileSystemCreator o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(NetworkFileSystemCreator)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("NetworkFileSystemCreator", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write58_ShellFileSystemCreator(string n, string ns, ShellFileSystemCreator o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ShellFileSystemCreator)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ShellFileSystemCreator", "");
                }
                base.WriteEndElement(o);
            }
        }

        private string Write59_ContentFlag(ContentFlag v)
        {
            switch (v)
            {
                case ContentFlag.contflags_edit:
                    return "contflags_edit";

                case ContentFlag.contflags_substsize:
                    return "contflags_substsize";

                case ContentFlag.contflags_substdatetime:
                    return "contflags_substdatetime";

                case ContentFlag.contflags_substdate:
                    return "contflags_substdate";

                case ContentFlag.contflags_substtime:
                    return "contflags_substtime";

                case ContentFlag.contflags_substattributes:
                    return "contflags_substattributes";

                case ContentFlag.contflags_substattributestr:
                    return "contflags_substattributestr";

                case ContentFlag.contflags_passthrough_size_float:
                    return "contflags_passthrough_size_float";

                case ContentFlag.contflags_fieldedit:
                    return "contflags_fieldedit";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "contflags_edit", "contflags_substsize", "contflags_substdatetime", "contflags_substdate", "contflags_substtime", "contflags_substattributes", "contflags_substattributestr", "contflags_passthrough_size_float", "contflags_substmask", "contflags_fieldedit" }, new long[] { 1L, 2L, 4L, 6L, 8L, 10L, 12L, 14L, 14L, 0x10L }, "Nomad.FileSystem.Property.Providers.Wdx.ContentFlag");
        }

        private void Write6_KeysConverter2(string n, string ns, KeysConverter2 o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(KeysConverter2)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("KeysConverter2", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write60_ContentDefaultParamStruct(string n, string ns, ContentDefaultParamStruct o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(ContentDefaultParamStruct)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("ContentDefaultParamStruct", "");
            }
            base.WriteElementStringRaw("size", "", XmlConvert.ToString(o.size));
            base.WriteElementStringRaw("PluginInterfaceVersionLow", "", XmlConvert.ToString(o.PluginInterfaceVersionLow));
            base.WriteElementStringRaw("PluginInterfaceVersionHi", "", XmlConvert.ToString(o.PluginInterfaceVersionHi));
            base.WriteElementString("DefaultIniName", "", o.DefaultIniName);
            base.WriteEndElement(o);
        }

        private void Write61_tdateformat(string n, string ns, tdateformat o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(tdateformat)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("tdateformat", "");
            }
            base.WriteElementStringRaw("wYear", "", XmlConvert.ToString(o.wYear));
            base.WriteElementStringRaw("wMonth", "", XmlConvert.ToString(o.wMonth));
            base.WriteElementStringRaw("wDay", "", XmlConvert.ToString(o.wDay));
            base.WriteEndElement(o);
        }

        private void Write62_ttimeformat(string n, string ns, ttimeformat o, bool needType)
        {
            if (!needType && (o.GetType() != typeof(ttimeformat)))
            {
                throw base.CreateUnknownTypeException(o);
            }
            base.WriteStartElement(n, ns, o, false, null);
            if (needType)
            {
                base.WriteXsiType("ttimeformat", "");
            }
            base.WriteElementStringRaw("wHour", "", XmlConvert.ToString(o.wHour));
            base.WriteElementStringRaw("wMinute", "", XmlConvert.ToString(o.wMinute));
            base.WriteElementStringRaw("wSecond", "", XmlConvert.ToString(o.wSecond));
            base.WriteEndElement(o);
        }

        private void Write63_WdxFieldInfo(string n, string ns, WdxFieldInfo o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(WdxFieldInfo)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("WdxFieldInfo", "");
                }
                base.WriteElementString("FieldName", "", o.FieldName);
                base.WriteElementStringRaw("FieldType", "", XmlConvert.ToString(o.FieldType));
                string[] units = o.Units;
                if (units != null)
                {
                    base.WriteStartElement("Units", "", null, false);
                    for (int i = 0; i < units.Length; i++)
                    {
                        base.WriteNullableStringLiteral("string", "", units[i]);
                    }
                    base.WriteEndElement();
                }
                base.WriteElementString("Flags", "", this.Write59_ContentFlag(o.Flags));
                base.WriteEndElement(o);
            }
        }

        private string Write64_AggregatedFilterCondition(AggregatedFilterCondition v)
        {
            switch (v)
            {
                case AggregatedFilterCondition.All:
                    return "All";

                case AggregatedFilterCondition.Any:
                    return "Any";

                case AggregatedFilterCondition.None:
                    return "None";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.Filter.AggregatedFilterCondition");
        }

        private void Write65_BasicFilter(string n, string ns, BasicFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(BasicFilter))
                {
                    if (type == typeof(VirtualItemNameListFilter))
                    {
                        this.Write107_VirtualItemNameListFilter(n, ns, (VirtualItemNameListFilter) o, isNullable, true);
                    }
                    else if (type == typeof(NameFilter))
                    {
                        this.Write102_NameFilter(n, ns, (NameFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemFullNameFilter))
                    {
                        this.Write112_VirtualItemFullNameFilter(n, ns, (VirtualItemFullNameFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemNameFilter))
                    {
                        this.Write103_VirtualItemNameFilter(n, ns, (VirtualItemNameFilter) o, isNullable, true);
                    }
                    else if (type == typeof(AttributeFilter))
                    {
                        this.Write93_AttributeFilter(n, ns, (AttributeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemAttributeFilter))
                    {
                        this.Write94_VirtualItemAttributeFilter(n, ns, (VirtualItemAttributeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(CustomContentFilter))
                    {
                        this.Write85_CustomContentFilter(n, ns, (CustomContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(HexContentFilter))
                    {
                        this.Write104_HexContentFilter(n, ns, (HexContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemHexContentFilter))
                    {
                        this.Write105_VirtualItemHexContentFilter(n, ns, (VirtualItemHexContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(ContentFilter))
                    {
                        this.Write86_ContentFilter(n, ns, (ContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemContentFilter))
                    {
                        this.Write87_VirtualItemContentFilter(n, ns, (VirtualItemContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(ValueFilter))
                    {
                        this.Write66_ValueFilter(n, ns, (ValueFilter) o, isNullable, true);
                    }
                    else if (type == typeof(TimeFilter))
                    {
                        this.Write89_TimeFilter(n, ns, (TimeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemTimeFilter))
                    {
                        this.Write91_VirtualItemTimeFilter(n, ns, (VirtualItemTimeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<byte>))
                    {
                        this.Write82_SimpleFilterOfByte(n, ns, (SimpleFilter<byte>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<byte>))
                    {
                        this.Write83_IntegralFilterOfByte(n, ns, (IntegralFilter<byte>) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<int>))
                    {
                        this.Write80_SimpleFilterOfInt32(n, ns, (SimpleFilter<int>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<int>))
                    {
                        this.Write81_IntegralFilterOfInt32(n, ns, (IntegralFilter<int>) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<uint>))
                    {
                        this.Write78_SimpleFilterOfUInt32(n, ns, (SimpleFilter<uint>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<uint>))
                    {
                        this.Write79_IntegralFilterOfUInt32(n, ns, (IntegralFilter<uint>) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<long>))
                    {
                        this.Write76_SimpleFilterOfInt64(n, ns, (SimpleFilter<long>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<long>))
                    {
                        this.Write77_IntegralFilterOfInt64(n, ns, (IntegralFilter<long>) o, isNullable, true);
                    }
                    else if (type == typeof(SizeFilter))
                    {
                        this.Write97_SizeFilter(n, ns, (SizeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemSizeFilter))
                    {
                        this.Write98_VirtualItemSizeFilter(n, ns, (VirtualItemSizeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<Version>))
                    {
                        this.Write75_SimpleFilterOfVersion(n, ns, (SimpleFilter<Version>) o, isNullable, true);
                    }
                    else if (type == typeof(StringFilter))
                    {
                        this.Write72_StringFilter(n, ns, (StringFilter) o, isNullable, true);
                    }
                    else if (type == typeof(DateFilter))
                    {
                        this.Write69_DateFilter(n, ns, (DateFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemDateFilter))
                    {
                        this.Write99_VirtualItemDateFilter(n, ns, (VirtualItemDateFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualPropertyFilter))
                    {
                        this.Write84_VirtualPropertyFilter(n, ns, (VirtualPropertyFilter) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(AggregatedVirtualItemFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write108_AggregatedVirtualItemFilter(n, ns, (AggregatedVirtualItemFilter) o, isNullable, true);
                    }
                }
            }
        }

        private void Write66_ValueFilter(string n, string ns, ValueFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(ValueFilter))
                {
                    if (type == typeof(TimeFilter))
                    {
                        this.Write89_TimeFilter(n, ns, (TimeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemTimeFilter))
                    {
                        this.Write91_VirtualItemTimeFilter(n, ns, (VirtualItemTimeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<byte>))
                    {
                        this.Write82_SimpleFilterOfByte(n, ns, (SimpleFilter<byte>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<byte>))
                    {
                        this.Write83_IntegralFilterOfByte(n, ns, (IntegralFilter<byte>) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<int>))
                    {
                        this.Write80_SimpleFilterOfInt32(n, ns, (SimpleFilter<int>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<int>))
                    {
                        this.Write81_IntegralFilterOfInt32(n, ns, (IntegralFilter<int>) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<uint>))
                    {
                        this.Write78_SimpleFilterOfUInt32(n, ns, (SimpleFilter<uint>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<uint>))
                    {
                        this.Write79_IntegralFilterOfUInt32(n, ns, (IntegralFilter<uint>) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<long>))
                    {
                        this.Write76_SimpleFilterOfInt64(n, ns, (SimpleFilter<long>) o, isNullable, true);
                    }
                    else if (type == typeof(IntegralFilter<long>))
                    {
                        this.Write77_IntegralFilterOfInt64(n, ns, (IntegralFilter<long>) o, isNullable, true);
                    }
                    else if (type == typeof(SizeFilter))
                    {
                        this.Write97_SizeFilter(n, ns, (SizeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemSizeFilter))
                    {
                        this.Write98_VirtualItemSizeFilter(n, ns, (VirtualItemSizeFilter) o, isNullable, true);
                    }
                    else if (type == typeof(SimpleFilter<Version>))
                    {
                        this.Write75_SimpleFilterOfVersion(n, ns, (SimpleFilter<Version>) o, isNullable, true);
                    }
                    else if (type == typeof(StringFilter))
                    {
                        this.Write72_StringFilter(n, ns, (StringFilter) o, isNullable, true);
                    }
                    else if (type == typeof(DateFilter))
                    {
                        this.Write69_DateFilter(n, ns, (DateFilter) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(VirtualItemDateFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write99_VirtualItemDateFilter(n, ns, (VirtualItemDateFilter) o, isNullable, true);
                    }
                }
            }
        }

        private string Write67_DateComparision(DateComparision v)
        {
            switch (v)
            {
                case DateComparision.Ignore:
                    return "Ignore";

                case DateComparision.On:
                    return "On";

                case DateComparision.Before:
                    return "Before";

                case DateComparision.After:
                    return "After";

                case DateComparision.Between:
                    return "Between";

                case DateComparision.NotBetween:
                    return "NotBetween";

                case DateComparision.NotOlderThan:
                    return "NotOlderThan";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.DateComparision");
        }

        private string Write68_DateUnit(DateUnit v)
        {
            switch (v)
            {
                case DateUnit.Day:
                    return "Day";

                case DateUnit.Week:
                    return "Week";

                case DateUnit.Month:
                    return "Month";

                case DateUnit.Year:
                    return "Year";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.DateUnit");
        }

        private void Write69_DateFilter(string n, string ns, DateFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(DateFilter))
                    {
                        if (type != typeof(VirtualItemDateFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write99_VirtualItemDateFilter(n, ns, (VirtualItemDateFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("DateFilter", "");
                }
                base.WriteElementString("DateComparision", "", this.Write67_DateComparision(o.DateComparision));
                if (o.NotOlderThan != 1)
                {
                    base.WriteElementStringRaw("NotOlderThan", "", XmlConvert.ToString(o.NotOlderThan));
                }
                if (o.DateMeasure != DateUnit.Day)
                {
                    base.WriteElementString("DateMeasure", "", this.Write68_DateUnit(o.DateMeasure));
                }
                base.WriteElementStringRaw("FromDate", "", XmlSerializationWriter.FromDate(o.FromDate));
                base.WriteElementStringRaw("ToDate", "", XmlSerializationWriter.FromDate(o.ToDate));
                base.WriteEndElement(o);
            }
        }

        private string Write7_PropertyTagType(PropertyTagType v)
        {
            switch (v)
            {
                case PropertyTagType.PixelFormat4bppIndexed:
                    return "PixelFormat4bppIndexed";

                case PropertyTagType.Byte:
                    return "Byte";

                case PropertyTagType.ASCII:
                    return "ASCII";

                case PropertyTagType.Short:
                    return "Short";

                case PropertyTagType.Long:
                    return "Long";

                case PropertyTagType.Rational:
                    return "Rational";

                case PropertyTagType.Undefined:
                    return "Undefined";

                case PropertyTagType.SLONG:
                    return "SLONG";

                case PropertyTagType.SRational:
                    return "SRational";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.PropertyTagType");
        }

        private string Write70_ContentFilterOptions(ContentFilterOptions v)
        {
            switch (v)
            {
                case ContentFilterOptions.UseIFilter:
                    return "UseIFilter";

                case ContentFilterOptions.DetectEncoding:
                    return "DetectEncoding";

                case ContentFilterOptions.Regex:
                    return "Regex";

                case ContentFilterOptions.CaseSensitive:
                    return "CaseSensitive";

                case ContentFilterOptions.WholeWords:
                    return "WholeWords";

                case ContentFilterOptions.SpaceCompress:
                    return "SpaceCompress";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "Regex", "CaseSensitive", "WholeWords", "SpaceCompress", "UseIFilter", "DetectEncoding" }, new long[] { 1L, 2L, 4L, 8L, 0x10L, 0x20L }, "Nomad.Commons.ContentFilterOptions");
        }

        private string Write71_ContentComparision(ContentComparision v)
        {
            switch (v)
            {
                case ContentComparision.Ignore:
                    return "Ignore";

                case ContentComparision.Contains:
                    return "Contains";

                case ContentComparision.NotContains:
                    return "NotContains";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.ContentComparision");
        }

        private void Write72_StringFilter(string n, string ns, StringFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(StringFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("StringFilter", "");
                }
                base.WriteElementString("Options", "", this.Write70_ContentFilterOptions(o.Options));
                base.WriteElementString("Comparision", "", this.Write71_ContentComparision(o.Comparision));
                base.WriteElementString("Text", "", o.Text);
                base.WriteEndElement(o);
            }
        }

        private string Write73_SimpleComparision(SimpleComparision v)
        {
            switch (v)
            {
                case SimpleComparision.Ignore:
                    return "Ignore";

                case SimpleComparision.Equals:
                    return "Equals";

                case SimpleComparision.Smaller:
                    return "Smaller";

                case SimpleComparision.Larger:
                    return "Larger";

                case SimpleComparision.Between:
                    return "Between";

                case SimpleComparision.NotBetween:
                    return "NotBetween";

                case SimpleComparision.NotEquals:
                    return "NotEquals";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.SimpleComparision");
        }

        private void Write74_Version(string n, string ns, Version o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(Version)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("Version", "");
                }
                base.WriteEndElement(o);
            }
        }

        private void Write75_SimpleFilterOfVersion(string n, string ns, SimpleFilter<Version> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(SimpleFilter<Version>)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SimpleFilterOfVersion", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                this.Write74_Version("FromValue", "", o.FromValue, false, false);
                this.Write74_Version("ToValue", "", o.ToValue, false, false);
                base.WriteEndElement(o);
            }
        }

        private void Write76_SimpleFilterOfInt64(string n, string ns, SimpleFilter<long> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(SimpleFilter<long>))
                    {
                        if (type == typeof(IntegralFilter<long>))
                        {
                            this.Write77_IntegralFilterOfInt64(n, ns, (IntegralFilter<long>) o, isNullable, true);
                            return;
                        }
                        if (type == typeof(SizeFilter))
                        {
                            this.Write97_SizeFilter(n, ns, (SizeFilter) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(VirtualItemSizeFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write98_VirtualItemSizeFilter(n, ns, (VirtualItemSizeFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SimpleFilterOfInt64", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write77_IntegralFilterOfInt64(string n, string ns, IntegralFilter<long> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(IntegralFilter<long>))
                    {
                        if (type == typeof(SizeFilter))
                        {
                            this.Write97_SizeFilter(n, ns, (SizeFilter) o, isNullable, true);
                            return;
                        }
                        if (type != typeof(VirtualItemSizeFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write98_VirtualItemSizeFilter(n, ns, (VirtualItemSizeFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("IntegralFilterOfInt64", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write78_SimpleFilterOfUInt32(string n, string ns, SimpleFilter<uint> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(SimpleFilter<uint>))
                    {
                        if (type != typeof(IntegralFilter<uint>))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write79_IntegralFilterOfUInt32(n, ns, (IntegralFilter<uint>) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SimpleFilterOfUInt32", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write79_IntegralFilterOfUInt32(string n, string ns, IntegralFilter<uint> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(IntegralFilter<uint>)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("IntegralFilterOfUInt32", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private string Write8_PropertyTag(PropertyTag v)
        {
            switch (v)
            {
                case PropertyTag.GpsVer:
                    return "GpsVer";

                case PropertyTag.GpsLatitudeRef:
                    return "GpsLatitudeRef";

                case PropertyTag.GpsLatitude:
                    return "GpsLatitude";

                case PropertyTag.GpsLongitudeRef:
                    return "GpsLongitudeRef";

                case PropertyTag.GpsLongitude:
                    return "GpsLongitude";

                case PropertyTag.GpsAltitudeRef:
                    return "GpsAltitudeRef";

                case PropertyTag.GpsAltitude:
                    return "GpsAltitude";

                case PropertyTag.GpsGpsTime:
                    return "GpsGpsTime";

                case PropertyTag.GpsGpsSatellites:
                    return "GpsGpsSatellites";

                case PropertyTag.GpsGpsStatus:
                    return "GpsGpsStatus";

                case PropertyTag.GpsGpsMeasureMode:
                    return "GpsGpsMeasureMode";

                case PropertyTag.GpsGpsDop:
                    return "GpsGpsDop";

                case PropertyTag.GpsSpeedRef:
                    return "GpsSpeedRef";

                case PropertyTag.GpsSpeed:
                    return "GpsSpeed";

                case PropertyTag.GpsTrackRef:
                    return "GpsTrackRef";

                case PropertyTag.GpsTrack:
                    return "GpsTrack";

                case PropertyTag.GpsImgDirRef:
                    return "GpsImgDirRef";

                case PropertyTag.GpsImgDir:
                    return "GpsImgDir";

                case PropertyTag.GpsMapDatum:
                    return "GpsMapDatum";

                case PropertyTag.GpsDestLatRef:
                    return "GpsDestLatRef";

                case PropertyTag.GpsDestLat:
                    return "GpsDestLat";

                case PropertyTag.GpsDestLongRef:
                    return "GpsDestLongRef";

                case PropertyTag.GpsDestLong:
                    return "GpsDestLong";

                case PropertyTag.GpsDestBearRef:
                    return "GpsDestBearRef";

                case PropertyTag.GpsDestBear:
                    return "GpsDestBear";

                case PropertyTag.GpsDestDistRef:
                    return "GpsDestDistRef";

                case PropertyTag.GpsDestDist:
                    return "GpsDestDist";

                case PropertyTag.NewSubfileType:
                    return "NewSubfileType";

                case PropertyTag.SubfileType:
                    return "SubfileType";

                case PropertyTag.ImageWidth:
                    return "ImageWidth";

                case PropertyTag.ImageHeight:
                    return "ImageHeight";

                case PropertyTag.BitsPerSample:
                    return "BitsPerSample";

                case PropertyTag.Compression:
                    return "Compression";

                case PropertyTag.PhotometricInterp:
                    return "PhotometricInterp";

                case PropertyTag.ThreshHolding:
                    return "ThreshHolding";

                case PropertyTag.CellWidth:
                    return "CellWidth";

                case PropertyTag.CellHeight:
                    return "CellHeight";

                case PropertyTag.FillOrder:
                    return "FillOrder";

                case PropertyTag.DocumentName:
                    return "DocumentName";

                case PropertyTag.ImageDescription:
                    return "ImageDescription";

                case PropertyTag.EquipMake:
                    return "EquipMake";

                case PropertyTag.EquipModel:
                    return "EquipModel";

                case PropertyTag.StripOffsets:
                    return "StripOffsets";

                case PropertyTag.Orientation:
                    return "Orientation";

                case PropertyTag.SamplesPerPixel:
                    return "SamplesPerPixel";

                case PropertyTag.RowsPerStrip:
                    return "RowsPerStrip";

                case PropertyTag.StripBytesCount:
                    return "StripBytesCount";

                case PropertyTag.MinSampleValue:
                    return "MinSampleValue";

                case PropertyTag.MaxSampleValue:
                    return "MaxSampleValue";

                case PropertyTag.XResolution:
                    return "XResolution";

                case PropertyTag.YResolution:
                    return "YResolution";

                case PropertyTag.PlanarConfig:
                    return "PlanarConfig";

                case PropertyTag.PageName:
                    return "PageName";

                case PropertyTag.XPosition:
                    return "XPosition";

                case PropertyTag.YPosition:
                    return "YPosition";

                case PropertyTag.FreeOffset:
                    return "FreeOffset";

                case PropertyTag.FreeByteCounts:
                    return "FreeByteCounts";

                case PropertyTag.GrayResponseUnit:
                    return "GrayResponseUnit";

                case PropertyTag.GrayResponseCurve:
                    return "GrayResponseCurve";

                case PropertyTag.T4Option:
                    return "T4Option";

                case PropertyTag.T6Option:
                    return "T6Option";

                case PropertyTag.ResolutionUnit:
                    return "ResolutionUnit";

                case PropertyTag.PageNumber:
                    return "PageNumber";

                case PropertyTag.TransferFunction:
                    return "TransferFunction";

                case PropertyTag.SoftwareUsed:
                    return "SoftwareUsed";

                case PropertyTag.DateTime:
                    return "DateTime";

                case PropertyTag.Artist:
                    return "Artist";

                case PropertyTag.HostComputer:
                    return "HostComputer";

                case PropertyTag.Predictor:
                    return "Predictor";

                case PropertyTag.WhitePoint:
                    return "WhitePoint";

                case PropertyTag.PrimaryChromaticities:
                    return "PrimaryChromaticities";

                case PropertyTag.ColorMap:
                    return "ColorMap";

                case PropertyTag.HalftoneHints:
                    return "HalftoneHints";

                case PropertyTag.TileWidth:
                    return "TileWidth";

                case PropertyTag.TileLength:
                    return "TileLength";

                case PropertyTag.TileOffset:
                    return "TileOffset";

                case PropertyTag.TileByteCounts:
                    return "TileByteCounts";

                case PropertyTag.InkSet:
                    return "InkSet";

                case PropertyTag.InkNames:
                    return "InkNames";

                case PropertyTag.NumberOfInks:
                    return "NumberOfInks";

                case PropertyTag.DotRange:
                    return "DotRange";

                case PropertyTag.TargetPrinter:
                    return "TargetPrinter";

                case PropertyTag.ExtraSamples:
                    return "ExtraSamples";

                case PropertyTag.SampleFormat:
                    return "SampleFormat";

                case PropertyTag.SMinSampleValue:
                    return "SMinSampleValue";

                case PropertyTag.SMaxSampleValue:
                    return "SMaxSampleValue";

                case PropertyTag.TransferRange:
                    return "TransferRange";

                case PropertyTag.JPEGProc:
                    return "JPEGProc";

                case PropertyTag.JPEGInterFormat:
                    return "JPEGInterFormat";

                case PropertyTag.JPEGInterLength:
                    return "JPEGInterLength";

                case PropertyTag.JPEGRestartInterval:
                    return "JPEGRestartInterval";

                case PropertyTag.JPEGLosslessPredictors:
                    return "JPEGLosslessPredictors";

                case PropertyTag.JPEGPointTransforms:
                    return "JPEGPointTransforms";

                case PropertyTag.JPEGQTables:
                    return "JPEGQTables";

                case PropertyTag.JPEGDCTables:
                    return "JPEGDCTables";

                case PropertyTag.JPEGACTables:
                    return "JPEGACTables";

                case PropertyTag.YCbCrCoefficients:
                    return "YCbCrCoefficients";

                case PropertyTag.YCbCrSubsampling:
                    return "YCbCrSubsampling";

                case PropertyTag.YCbCrPositioning:
                    return "YCbCrPositioning";

                case PropertyTag.REFBlackWhite:
                    return "REFBlackWhite";

                case PropertyTag.Gamma:
                    return "Gamma";

                case PropertyTag.ICCProfileDescriptor:
                    return "ICCProfileDescriptor";

                case PropertyTag.SRGBRenderingIntent:
                    return "SRGBRenderingIntent";

                case PropertyTag.ImageTitle:
                    return "ImageTitle";

                case PropertyTag.ResolutionXUnit:
                    return "ResolutionXUnit";

                case PropertyTag.ResolutionYUnit:
                    return "ResolutionYUnit";

                case PropertyTag.ResolutionXLengthUnit:
                    return "ResolutionXLengthUnit";

                case PropertyTag.ResolutionYLengthUnit:
                    return "ResolutionYLengthUnit";

                case PropertyTag.PrintFlags:
                    return "PrintFlags";

                case PropertyTag.PrintFlagsVersion:
                    return "PrintFlagsVersion";

                case PropertyTag.PrintFlagsCrop:
                    return "PrintFlagsCrop";

                case PropertyTag.PrintFlagsBleedWidth:
                    return "PrintFlagsBleedWidth";

                case PropertyTag.PrintFlagsBleedWidthScale:
                    return "PrintFlagsBleedWidthScale";

                case PropertyTag.HalftoneLPI:
                    return "HalftoneLPI";

                case PropertyTag.HalftoneLPIUnit:
                    return "HalftoneLPIUnit";

                case PropertyTag.HalftoneDegree:
                    return "HalftoneDegree";

                case PropertyTag.HalftoneShape:
                    return "HalftoneShape";

                case PropertyTag.HalftoneMisc:
                    return "HalftoneMisc";

                case PropertyTag.HalftoneScreen:
                    return "HalftoneScreen";

                case PropertyTag.JPEGQuality:
                    return "JPEGQuality";

                case PropertyTag.GridSize:
                    return "GridSize";

                case PropertyTag.ThumbnailFormat:
                    return "ThumbnailFormat";

                case PropertyTag.ThumbnailWidth:
                    return "ThumbnailWidth";

                case PropertyTag.ThumbnailHeight:
                    return "ThumbnailHeight";

                case PropertyTag.ThumbnailColorDepth:
                    return "ThumbnailColorDepth";

                case PropertyTag.ThumbnailPlanes:
                    return "ThumbnailPlanes";

                case PropertyTag.ThumbnailRawBytes:
                    return "ThumbnailRawBytes";

                case PropertyTag.ThumbnailSize:
                    return "ThumbnailSize";

                case PropertyTag.ThumbnailCompressedSize:
                    return "ThumbnailCompressedSize";

                case PropertyTag.ColorTransferFunction:
                    return "ColorTransferFunction";

                case PropertyTag.ThumbnailData:
                    return "ThumbnailData";

                case PropertyTag.ThumbnailImageWidth:
                    return "ThumbnailImageWidth";

                case PropertyTag.ThumbnailImageHeight:
                    return "ThumbnailImageHeight";

                case PropertyTag.ThumbnailBitsPerSample:
                    return "ThumbnailBitsPerSample";

                case PropertyTag.ThumbnailCompression:
                    return "ThumbnailCompression";

                case PropertyTag.ThumbnailPhotometricInterp:
                    return "ThumbnailPhotometricInterp";

                case PropertyTag.ThumbnailImageDescription:
                    return "ThumbnailImageDescription";

                case PropertyTag.ThumbnailEquipMake:
                    return "ThumbnailEquipMake";

                case PropertyTag.ThumbnailEquipModel:
                    return "ThumbnailEquipModel";

                case PropertyTag.ThumbnailStripOffsets:
                    return "ThumbnailStripOffsets";

                case PropertyTag.ThumbnailOrientation:
                    return "ThumbnailOrientation";

                case PropertyTag.ThumbnailSamplesPerPixel:
                    return "ThumbnailSamplesPerPixel";

                case PropertyTag.ThumbnailRowsPerStrip:
                    return "ThumbnailRowsPerStrip";

                case PropertyTag.ThumbnailStripBytesCount:
                    return "ThumbnailStripBytesCount";

                case PropertyTag.ThumbnailResolutionX:
                    return "ThumbnailResolutionX";

                case PropertyTag.ThumbnailResolutionY:
                    return "ThumbnailResolutionY";

                case PropertyTag.ThumbnailPlanarConfig:
                    return "ThumbnailPlanarConfig";

                case PropertyTag.ThumbnailResolutionUnit:
                    return "ThumbnailResolutionUnit";

                case PropertyTag.ThumbnailTransferFunction:
                    return "ThumbnailTransferFunction";

                case PropertyTag.ThumbnailSoftwareUsed:
                    return "ThumbnailSoftwareUsed";

                case PropertyTag.ThumbnailDateTime:
                    return "ThumbnailDateTime";

                case PropertyTag.ThumbnailArtist:
                    return "ThumbnailArtist";

                case PropertyTag.ThumbnailWhitePoint:
                    return "ThumbnailWhitePoint";

                case PropertyTag.ThumbnailPrimaryChromaticities:
                    return "ThumbnailPrimaryChromaticities";

                case PropertyTag.ThumbnailYCbCrCoefficients:
                    return "ThumbnailYCbCrCoefficients";

                case PropertyTag.ThumbnailYCbCrSubsampling:
                    return "ThumbnailYCbCrSubsampling";

                case PropertyTag.ThumbnailYCbCrPositioning:
                    return "ThumbnailYCbCrPositioning";

                case PropertyTag.ThumbnailRefBlackWhite:
                    return "ThumbnailRefBlackWhite";

                case PropertyTag.ThumbnailCopyRight:
                    return "ThumbnailCopyRight";

                case PropertyTag.LuminanceTable:
                    return "LuminanceTable";

                case PropertyTag.ChrominanceTable:
                    return "ChrominanceTable";

                case PropertyTag.FrameDelay:
                    return "FrameDelay";

                case PropertyTag.LoopCount:
                    return "LoopCount";

                case PropertyTag.GlobalPalette:
                    return "GlobalPalette";

                case PropertyTag.IndexBackground:
                    return "IndexBackground";

                case PropertyTag.IndexTransparent:
                    return "IndexTransparent";

                case PropertyTag.PixelUnit:
                    return "PixelUnit";

                case PropertyTag.PixelPerUnitX:
                    return "PixelPerUnitX";

                case PropertyTag.PixelPerUnitY:
                    return "PixelPerUnitY";

                case PropertyTag.PaletteHistogram:
                    return "PaletteHistogram";

                case PropertyTag.Copyright:
                    return "Copyright";

                case PropertyTag.ExifExposureTime:
                    return "ExifExposureTime";

                case PropertyTag.ExifFNumber:
                    return "ExifFNumber";

                case PropertyTag.ExifExposureProg:
                    return "ExifExposureProg";

                case PropertyTag.ExifSpectralSense:
                    return "ExifSpectralSense";

                case PropertyTag.GpsIFD:
                    return "GpsIFD";

                case PropertyTag.ExifISOSpeed:
                    return "ExifISOSpeed";

                case PropertyTag.ExifOECF:
                    return "ExifOECF";

                case PropertyTag.ICCProfile:
                    return "ICCProfile";

                case PropertyTag.ExifIFD:
                    return "ExifIFD";

                case PropertyTag.ExifVer:
                    return "ExifVer";

                case PropertyTag.ExifDTOrig:
                    return "ExifDTOrig";

                case PropertyTag.ExifDTDigitized:
                    return "ExifDTDigitized";

                case PropertyTag.ExifCompConfig:
                    return "ExifCompConfig";

                case PropertyTag.ExifCompBPP:
                    return "ExifCompBPP";

                case PropertyTag.ExifShutterSpeed:
                    return "ExifShutterSpeed";

                case PropertyTag.ExifAperture:
                    return "ExifAperture";

                case PropertyTag.ExifBrightness:
                    return "ExifBrightness";

                case PropertyTag.ExifExposureBias:
                    return "ExifExposureBias";

                case PropertyTag.ExifMaxAperture:
                    return "ExifMaxAperture";

                case PropertyTag.ExifSubjectDist:
                    return "ExifSubjectDist";

                case PropertyTag.ExifMeteringMode:
                    return "ExifMeteringMode";

                case PropertyTag.ExifLightSource:
                    return "ExifLightSource";

                case PropertyTag.ExifFlash:
                    return "ExifFlash";

                case PropertyTag.ExifFocalLength:
                    return "ExifFocalLength";

                case PropertyTag.ExifDTSubsec:
                    return "ExifDTSubsec";

                case PropertyTag.ExifDTOrigSS:
                    return "ExifDTOrigSS";

                case PropertyTag.ExifDTDigSS:
                    return "ExifDTDigSS";

                case PropertyTag.ExifUserComment:
                    return "ExifUserComment";

                case PropertyTag.ExifMakerNote:
                    return "ExifMakerNote";

                case PropertyTag.ExifFPXVer:
                    return "ExifFPXVer";

                case PropertyTag.ExifColorSpace:
                    return "ExifColorSpace";

                case PropertyTag.ExifPixXDim:
                    return "ExifPixXDim";

                case PropertyTag.ExifPixYDim:
                    return "ExifPixYDim";

                case PropertyTag.ExifRelatedWav:
                    return "ExifRelatedWav";

                case PropertyTag.ExifInterop:
                    return "ExifInterop";

                case PropertyTag.ExifFlashEnergy:
                    return "ExifFlashEnergy";

                case PropertyTag.ExifSpatialFR:
                    return "ExifSpatialFR";

                case PropertyTag.ExifFocalXRes:
                    return "ExifFocalXRes";

                case PropertyTag.ExifFocalYRes:
                    return "ExifFocalYRes";

                case PropertyTag.ExifFocalResUnit:
                    return "ExifFocalResUnit";

                case PropertyTag.ExifSubjectLoc:
                    return "ExifSubjectLoc";

                case PropertyTag.ExifExposureIndex:
                    return "ExifExposureIndex";

                case PropertyTag.ExifSensingMethod:
                    return "ExifSensingMethod";

                case PropertyTag.ExifFileSource:
                    return "ExifFileSource";

                case PropertyTag.ExifSceneType:
                    return "ExifSceneType";

                case PropertyTag.ExifCfaPattern:
                    return "ExifCfaPattern";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.PropertyTag");
        }

        private void Write80_SimpleFilterOfInt32(string n, string ns, SimpleFilter<int> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(SimpleFilter<int>))
                    {
                        if (type != typeof(IntegralFilter<int>))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write81_IntegralFilterOfInt32(n, ns, (IntegralFilter<int>) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SimpleFilterOfInt32", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write81_IntegralFilterOfInt32(string n, string ns, IntegralFilter<int> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(IntegralFilter<int>)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("IntegralFilterOfInt32", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write82_SimpleFilterOfByte(string n, string ns, SimpleFilter<byte> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(SimpleFilter<byte>))
                    {
                        if (type != typeof(IntegralFilter<byte>))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write83_IntegralFilterOfByte(n, ns, (IntegralFilter<byte>) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SimpleFilterOfByte", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write83_IntegralFilterOfByte(string n, string ns, IntegralFilter<byte> o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(IntegralFilter<byte>)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("IntegralFilterOfByte", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                base.WriteEndElement(o);
            }
        }

        private void Write84_VirtualPropertyFilter(string n, string ns, VirtualPropertyFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualPropertyFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualPropertyFilter", "");
                }
                base.WriteElementStringRaw("PropertyId", "", XmlConvert.ToString(o.PropertyId));
                if (o.Filter != null)
                {
                    if (o.Filter is IntegralFilter<byte>)
                    {
                        this.Write83_IntegralFilterOfByte("ByteFilter", "", (IntegralFilter<byte>) o.Filter, false, false);
                    }
                    else if (o.Filter is IntegralFilter<uint>)
                    {
                        this.Write79_IntegralFilterOfUInt32("UInt32Filter", "", (IntegralFilter<uint>) o.Filter, false, false);
                    }
                    else if (o.Filter is IntegralFilter<int>)
                    {
                        this.Write81_IntegralFilterOfInt32("Int32Filter", "", (IntegralFilter<int>) o.Filter, false, false);
                    }
                    else if (o.Filter is IntegralFilter<long>)
                    {
                        this.Write77_IntegralFilterOfInt64("Int64Filter", "", (IntegralFilter<long>) o.Filter, false, false);
                    }
                    else if (o.Filter is DateFilter)
                    {
                        this.Write69_DateFilter("DateFilter", "", (DateFilter) o.Filter, false, false);
                    }
                    else if (o.Filter is SimpleFilter<Version>)
                    {
                        this.Write75_SimpleFilterOfVersion("VersionFilter", "", (SimpleFilter<Version>) o.Filter, false, false);
                    }
                    else if (o.Filter is StringFilter)
                    {
                        this.Write72_StringFilter("StringFilter", "", (StringFilter) o.Filter, false, false);
                    }
                    else if (o.Filter != null)
                    {
                        throw base.CreateUnknownTypeException(o.Filter);
                    }
                }
                base.WriteEndElement(o);
            }
        }

        private void Write85_CustomContentFilter(string n, string ns, CustomContentFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else if (!needType)
            {
                System.Type type = o.GetType();
                if (type != typeof(CustomContentFilter))
                {
                    if (type == typeof(HexContentFilter))
                    {
                        this.Write104_HexContentFilter(n, ns, (HexContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(VirtualItemHexContentFilter))
                    {
                        this.Write105_VirtualItemHexContentFilter(n, ns, (VirtualItemHexContentFilter) o, isNullable, true);
                    }
                    else if (type == typeof(ContentFilter))
                    {
                        this.Write86_ContentFilter(n, ns, (ContentFilter) o, isNullable, true);
                    }
                    else
                    {
                        if (type != typeof(VirtualItemContentFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write87_VirtualItemContentFilter(n, ns, (VirtualItemContentFilter) o, isNullable, true);
                    }
                }
            }
        }

        private void Write86_ContentFilter(string n, string ns, ContentFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(ContentFilter))
                    {
                        if (type != typeof(VirtualItemContentFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write87_VirtualItemContentFilter(n, ns, (VirtualItemContentFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ContentFilter", "");
                }
                base.WriteElementString("Options", "", this.Write70_ContentFilterOptions(o.Options));
                base.WriteElementString("Comparision", "", this.Write71_ContentComparision(o.Comparision));
                base.WriteElementString("Text", "", o.Text);
                if (o.EncodingAsString != "UTF8")
                {
                    base.WriteElementString("Encoding", "", o.EncodingAsString);
                }
                base.WriteEndElement(o);
            }
        }

        private void Write87_VirtualItemContentFilter(string n, string ns, VirtualItemContentFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemContentFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemContentFilter", "");
                }
                base.WriteElementString("Options", "", this.Write70_ContentFilterOptions(o.Options));
                base.WriteElementString("Comparision", "", this.Write71_ContentComparision(o.Comparision));
                base.WriteElementString("Text", "", o.Text);
                if (o.EncodingAsString != "UTF8")
                {
                    base.WriteElementString("Encoding", "", o.EncodingAsString);
                }
                base.WriteEndElement(o);
            }
        }

        private string Write88_TimeComparision(TimeComparision v)
        {
            switch (v)
            {
                case TimeComparision.Ignore:
                    return "Ignore";

                case TimeComparision.At:
                    return "At";

                case TimeComparision.Before:
                    return "Before";

                case TimeComparision.After:
                    return "After";

                case TimeComparision.Between:
                    return "Between";

                case TimeComparision.NotBetween:
                    return "NotBetween";

                case TimeComparision.HoursOf1:
                    return "HoursOf1";

                case TimeComparision.HoursOf6:
                    return "HoursOf6";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.TimeComparision");
        }

        private void Write89_TimeFilter(string n, string ns, TimeFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(TimeFilter))
                    {
                        if (type != typeof(VirtualItemTimeFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write91_VirtualItemTimeFilter(n, ns, (VirtualItemTimeFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("TimeFilter", "");
                }
                base.WriteElementString("TimeComparision", "", this.Write88_TimeComparision(o.TimeComparision));
                base.WriteElementStringRaw("FromTime", "", XmlSerializationWriter.FromTime(o.SerializableFromTime));
                base.WriteElementStringRaw("ToTime", "", XmlSerializationWriter.FromTime(o.SerializableToTime));
                base.WriteEndElement(o);
            }
        }

        private string Write9_LightSource(LightSource v)
        {
            switch (v)
            {
                case LightSource.Unknown:
                    return "Unknown";

                case LightSource.Daylight:
                    return "Daylight";

                case LightSource.Fluorescent:
                    return "Fluorescent";

                case LightSource.Tungsten:
                    return "Tungsten";

                case LightSource.StandardLightA:
                    return "StandardLightA";

                case LightSource.StandardLightB:
                    return "StandardLightB";

                case LightSource.StandardLightC:
                    return "StandardLightC";

                case LightSource.D55:
                    return "D55";

                case LightSource.D65:
                    return "D65";

                case LightSource.D75:
                    return "D75";

                case LightSource.Reserved:
                    return "Reserved";

                case LightSource.Other:
                    return "Other";
            }
            long num = (long) ((ulong) v);
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.LightSource");
        }

        private string Write90_ItemDateTimePart(ItemDateTimePart v)
        {
            switch (v)
            {
                case ItemDateTimePart.LastAccessTime:
                    return "LastAccessTime";

                case ItemDateTimePart.CreationTime:
                    return "CreationTime";

                case ItemDateTimePart.LastWriteTime:
                    return "LastWriteTime";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.FileSystem.Virtual.Filter.ItemDateTimePart");
        }

        private void Write91_VirtualItemTimeFilter(string n, string ns, VirtualItemTimeFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemTimeFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemTimeFilter", "");
                }
                base.WriteElementString("TimeComparision", "", this.Write88_TimeComparision(o.TimeComparision));
                base.WriteElementStringRaw("FromTime", "", XmlSerializationWriter.FromTime(o.SerializableFromTime));
                base.WriteElementStringRaw("ToTime", "", XmlSerializationWriter.FromTime(o.SerializableToTime));
                if (o.TimePart != ItemDateTimePart.LastWriteTime)
                {
                    base.WriteElementString("TimePart", "", this.Write90_ItemDateTimePart(o.TimePart));
                }
                base.WriteEndElement(o);
            }
        }

        private string Write92_FileAttributes(FileAttributes v)
        {
            switch (v)
            {
                case FileAttributes.Device:
                    return "Device";

                case FileAttributes.Normal:
                    return "Normal";

                case FileAttributes.Temporary:
                    return "Temporary";

                case FileAttributes.ReadOnly:
                    return "ReadOnly";

                case FileAttributes.Hidden:
                    return "Hidden";

                case FileAttributes.System:
                    return "System";

                case FileAttributes.Directory:
                    return "Directory";

                case FileAttributes.Archive:
                    return "Archive";

                case FileAttributes.SparseFile:
                    return "SparseFile";

                case FileAttributes.ReparsePoint:
                    return "ReparsePoint";

                case FileAttributes.Compressed:
                    return "Compressed";

                case FileAttributes.Offline:
                    return "Offline";

                case FileAttributes.NotContentIndexed:
                    return "NotContentIndexed";

                case FileAttributes.Encrypted:
                    return "Encrypted";
            }
            return XmlSerializationWriter.FromEnum((long) v, new string[] { "ReadOnly", "Hidden", "System", "Directory", "Archive", "Device", "Normal", "Temporary", "SparseFile", "ReparsePoint", "Compressed", "Offline", "NotContentIndexed", "Encrypted" }, new long[] { 1L, 2L, 4L, 0x10L, 0x20L, 0x40L, 0x80L, 0x100L, 0x200L, 0x400L, 0x800L, 0x1000L, 0x2000L, 0x4000L }, "System.IO.FileAttributes");
        }

        private void Write93_AttributeFilter(string n, string ns, AttributeFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(AttributeFilter))
                    {
                        if (type != typeof(VirtualItemAttributeFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write94_VirtualItemAttributeFilter(n, ns, (VirtualItemAttributeFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("AttributeFilter", "");
                }
                base.WriteElementString("IncludeAttributes", "", this.Write92_FileAttributes(o.IncludeAttributes));
                base.WriteElementString("ExcludeAttributes", "", this.Write92_FileAttributes(o.ExcludeAttributes));
                base.WriteEndElement(o);
            }
        }

        private void Write94_VirtualItemAttributeFilter(string n, string ns, VirtualItemAttributeFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemAttributeFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemAttributeFilter", "");
                }
                base.WriteElementString("IncludeAttributes", "", this.Write92_FileAttributes(o.IncludeAttributes));
                base.WriteElementString("ExcludeAttributes", "", this.Write92_FileAttributes(o.ExcludeAttributes));
                base.WriteEndElement(o);
            }
        }

        private string Write95_SizeUnit(SizeUnit v)
        {
            switch (v)
            {
                case SizeUnit.Byte:
                    return "Byte";

                case SizeUnit.KiloByte:
                    return "KiloByte";

                case SizeUnit.MegaByte:
                    return "MegaByte";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.SizeUnit");
        }

        private string Write96_SizeComparision(SizeComparision v)
        {
            switch (v)
            {
                case SizeComparision.Ignore:
                    return "Ignore";

                case SizeComparision.Equals:
                    return "Equals";

                case SizeComparision.Smaller:
                    return "Smaller";

                case SizeComparision.Larger:
                    return "Larger";

                case SizeComparision.Between:
                    return "Between";

                case SizeComparision.NotBetween:
                    return "NotBetween";

                case SizeComparision.PercentOf25:
                    return "PercentOf25";

                case SizeComparision.PercentOf50:
                    return "PercentOf50";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "Nomad.Commons.SizeComparision");
        }

        private void Write97_SizeFilter(string n, string ns, SizeFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType)
                {
                    System.Type type = o.GetType();
                    if (type != typeof(SizeFilter))
                    {
                        if (type != typeof(VirtualItemSizeFilter))
                        {
                            throw base.CreateUnknownTypeException(o);
                        }
                        this.Write98_VirtualItemSizeFilter(n, ns, (VirtualItemSizeFilter) o, isNullable, true);
                        return;
                    }
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("SizeFilter", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                if (o.SizeUnit != SizeUnit.Byte)
                {
                    base.WriteElementString("SizeUnit", "", this.Write95_SizeUnit(o.SizeUnit));
                }
                base.WriteElementString("SizeComparision", "", this.Write96_SizeComparision(o.SizeComparision));
                base.WriteEndElement(o);
            }
        }

        private void Write98_VirtualItemSizeFilter(string n, string ns, VirtualItemSizeFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemSizeFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemSizeFilter", "");
                }
                base.WriteElementString("ValueComparision", "", this.Write73_SimpleComparision(o.ValueComparision));
                base.WriteElementStringRaw("FromValue", "", XmlConvert.ToString(o.FromValue));
                base.WriteElementStringRaw("ToValue", "", XmlConvert.ToString(o.ToValue));
                if (o.SizeUnit != SizeUnit.Byte)
                {
                    base.WriteElementString("SizeUnit", "", this.Write95_SizeUnit(o.SizeUnit));
                }
                base.WriteElementString("SizeComparision", "", this.Write96_SizeComparision(o.SizeComparision));
                base.WriteEndElement(o);
            }
        }

        private void Write99_VirtualItemDateFilter(string n, string ns, VirtualItemDateFilter o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(VirtualItemDateFilter)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("VirtualItemDateFilter", "");
                }
                base.WriteElementString("DateComparision", "", this.Write67_DateComparision(o.DateComparision));
                if (o.NotOlderThan != 1)
                {
                    base.WriteElementStringRaw("NotOlderThan", "", XmlConvert.ToString(o.NotOlderThan));
                }
                if (o.DateMeasure != DateUnit.Day)
                {
                    base.WriteElementString("DateMeasure", "", this.Write68_DateUnit(o.DateMeasure));
                }
                base.WriteElementStringRaw("FromDate", "", XmlSerializationWriter.FromDate(o.FromDate));
                base.WriteElementStringRaw("ToDate", "", XmlSerializationWriter.FromDate(o.ToDate));
                if (o.DatePart != ItemDateTimePart.LastWriteTime)
                {
                    base.WriteElementString("DatePart", "", this.Write90_ItemDateTimePart(o.DatePart));
                }
                base.WriteEndElement(o);
            }
        }
    }
}

