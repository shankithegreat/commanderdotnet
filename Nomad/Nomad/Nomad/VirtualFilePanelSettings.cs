namespace Nomad
{
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
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
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [SettingsProvider(typeof(ConfigurableSettingsProvider)), CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class VirtualFilePanelSettings : ApplicationSettingsBase
    {
        private static VirtualFilePanelSettings defaultInstance = ((VirtualFilePanelSettings) SettingsBase.Synchronized(new VirtualFilePanelSettings()));
        private IVirtualItemFilter FHiddenItemsFilter;
        private bool HasHidddenItemsFilter;
        private const string PropertyListFont = "ListFont";

        public VirtualFilePanelSettings()
        {
            this.CompactProperties();
        }

        public static int DefaultColumnWidth(int propertyId, Font font)
        {
            VirtualProperty property = VirtualProperty.Get(propertyId);
            if (property == null)
            {
                return 0;
            }
            int defaultWidth = 60;
            string text = null;
            if (propertyId == 0)
            {
                defaultWidth = 120;
            }
            else if (property.PropertyType == typeof(Version))
            {
                text = "99.99.99.99";
            }
            else
            {
                if ((property.PropertyConverter != null) && property.PropertyConverter.CanConvertTo(typeof(PropertyMeasure)))
                {
                    PropertyMeasure measure = property.PropertyConverter.ConvertTo(null, typeof(PropertyMeasure)) as PropertyMeasure;
                    if (measure != null)
                    {
                        text = measure.MeasureString;
                        defaultWidth = measure.DefaultWidth;
                        goto Label_01BC;
                    }
                }
                if (property.PropertyLength <= 0)
                {
                    switch (System.Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.SByte:
                            text = "-129";
                            goto Label_01BC;

                        case TypeCode.Byte:
                            text = "259";
                            goto Label_01BC;

                        case TypeCode.Int16:
                            text = "-39999";
                            goto Label_01BC;

                        case TypeCode.UInt16:
                            text = "69999";
                            goto Label_01BC;
                    }
                }
                else
                {
                    switch (System.Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Double:
                            text = new string('9', property.PropertyLength);
                            goto Label_01BC;

                        case TypeCode.String:
                            text = new string('M', property.PropertyLength);
                            goto Label_01BC;
                    }
                    if (property.PropertyType == typeof(byte[]))
                    {
                        text = new string('F', property.PropertyLength);
                    }
                }
            }
        Label_01BC:
            if (text != null)
            {
                defaultWidth = TextRenderer.MeasureText(text, font).Width;
            }
            return (defaultWidth + 12);
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);
            string propertyName = e.PropertyName;
            if ((propertyName != null) && (propertyName == "ShowUpFolderItem"))
            {
                this.IsShowUpFolderItem = this.ShowUpFolderItem;
            }
        }

        protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
        {
            base.OnSettingsLoaded(sender, e);
            this.CompactPropertyValues();
        }

        [DefaultSettingValue("ShowShortRootName, ShowActiveState, ShowDriveMenuOnHover"), UserScopedSetting, DebuggerNonUserCode]
        public PathView BreadcrumbOptions
        {
            get
            {
                return (PathView) this["BreadcrumbOptions"];
            }
            set
            {
                this["BreadcrumbOptions"] = value;
            }
        }

        [UserScopedSetting]
        public PanelContentContainer Content
        {
            get
            {
                return (PanelContentContainer) this["Content"];
            }
            set
            {
                this["Content"] = value;
            }
        }

        public static VirtualFilePanelSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        public static ListViewColumnInfo[] DefaultColumns
        {
            get
            {
                return new ListViewColumnInfo[] { new ListViewColumnInfo(0, 120, true), new ListViewColumnInfo(3, HorizontalAlignment.Right, 60, true), new ListViewColumnInfo(4, HorizontalAlignment.Right, 60, false), new ListViewColumnInfo(5, HorizontalAlignment.Right, 60, false), new ListViewColumnInfo(8, 60, true) };
            }
        }

        [DefaultSettingValue(@"C:\"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string DefaultPath
        {
            get
            {
                return (string) this["DefaultPath"];
            }
        }

        [DefaultSettingValue("FreeSize"), UserScopedSetting, DebuggerNonUserCode]
        public string DriveMenuProperty
        {
            get
            {
                return (string) this["DriveMenuProperty"];
            }
            set
            {
                this["DriveMenuProperty"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("None")]
        public DoubleClickAction EmptySpaceDoubleClickAction
        {
            get
            {
                return (DoubleClickAction) this["EmptySpaceDoubleClickAction"];
            }
            set
            {
                this["EmptySpaceDoubleClickAction"] = value;
            }
        }

        [DefaultSettingValue("Normal"), UserScopedSetting, DebuggerNonUserCode]
        public CharacterCasing FileNameCasing
        {
            get
            {
                return (CharacterCasing) this["FileNameCasing"];
            }
            set
            {
                this["FileNameCasing"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("Normal")]
        public CharacterCasing FolderNameCasing
        {
            get
            {
                return (CharacterCasing) this["FolderNameCasing"];
            }
            set
            {
                this["FolderNameCasing"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("{0}")]
        public string FolderNameTemplate
        {
            get
            {
                return (string) this["FolderNameTemplate"];
            }
            set
            {
                this["FolderNameTemplate"] = value;
            }
        }

        public IVirtualItemFilter HiddenItemsFilter
        {
            get
            {
                if (!this.HasHidddenItemsFilter)
                {
                    List<IVirtualItemFilter> filters = new List<IVirtualItemFilter>();
                    if (this.HideSysHidItems)
                    {
                        filters.Add(new VirtualItemAttributeFilter(FileAttributes.System));
                        filters.Add(new VirtualItemAttributeFilter(FileAttributes.Hidden));
                    }
                    if (this.UseHiddenItemsList)
                    {
                        using (TextReader reader = new StringReader(this.HiddenItemsList))
                        {
                            string str;
                            while ((str = reader.ReadLine()) != null)
                            {
                                str = str.Trim();
                                if (!string.IsNullOrEmpty(str))
                                {
                                    if (str.StartsWith('^') && str.EndsWith('$'))
                                    {
                                        filters.Add(new VirtualItemFullNameFilter(str));
                                    }
                                    else
                                    {
                                        IVirtualItemFilter filter;
                                        string str2 = PathHelper.ExcludeTrailingDirectorySeparator(str);
                                        if (str2.ContainsAny(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar }))
                                        {
                                            filter = new VirtualItemFullNameFilter(str);
                                        }
                                        else
                                        {
                                            filter = new VirtualItemNameFilter(str2);
                                        }
                                        if (PathHelper.HasTrailingDirectorySeparator(str))
                                        {
                                            filters.Add(new AggregatedVirtualItemFilter(new VirtualItemAttributeFilter(FileAttributes.Directory), filter));
                                        }
                                        else
                                        {
                                            filters.Add(filter);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.FHiddenItemsFilter = null;
                    if (filters.Count == 1)
                    {
                        this.FHiddenItemsFilter = filters[0];
                    }
                    else if (filters.Count > 1)
                    {
                        this.FHiddenItemsFilter = new AggregatedVirtualItemFilter(AggregatedFilterCondition.Any, filters);
                    }
                    this.HasHidddenItemsFilter = true;
                }
                return this.FHiddenItemsFilter;
            }
        }

        [DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
        public string HiddenItemsList
        {
            get
            {
                return (string) this["HiddenItemsList"];
            }
            set
            {
                this["HiddenItemsList"] = value;
            }
        }

        [DefaultSettingValue("AB"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string HideDrives
        {
            get
            {
                return (string) this["HideDrives"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool HideNameExtension
        {
            get
            {
                return (bool) this["HideNameExtension"];
            }
            set
            {
                this["HideNameExtension"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool HideSysHidItems
        {
            get
            {
                return (bool) this["HideSysHidItems"];
            }
            set
            {
                this["HideSysHidItems"] = value;
            }
        }

        public bool IsShowUpFolderItem { get; private set; }

        public override object this[string propertyName]
        {
            get
            {
                return base[propertyName];
            }
            set
            {
                string str = propertyName;
                if (str != null)
                {
                    if (!(str == "HideSysHidItems") && !(str == "UseHiddenItemsList"))
                    {
                        if ((str == "HiddenItemsList") && this.UseHiddenItemsList)
                        {
                            this.FHiddenItemsFilter = null;
                            this.HasHidddenItemsFilter = false;
                        }
                    }
                    else
                    {
                        this.FHiddenItemsFilter = null;
                        this.HasHidddenItemsFilter = false;
                    }
                }
                base[propertyName] = value;
            }
        }

        [UserScopedSetting]
        public PanelLayout Layout
        {
            get
            {
                return (PanelLayout) this["Layout"];
            }
            set
            {
                this["Layout"] = value;
            }
        }

        [UserScopedSetting]
        public SerializableDictionary<KnownListViewColor, Color> ListColorMap
        {
            get
            {
                return (SerializableDictionary<KnownListViewColor, Color>) this["ListColorMap"];
            }
            set
            {
                this["ListColorMap"] = value;
            }
        }

        [UserScopedSetting]
        public Font ListFont
        {
            get
            {
                Font iconTitleFont = (Font) this["ListFont"];
                if (iconTitleFont == null)
                {
                    iconTitleFont = SystemFonts.IconTitleFont;
                }
                return iconTitleFont;
            }
            set
            {
                if ((value != null) || (this["ListFont"] != null))
                {
                    this["ListFont"] = value;
                }
            }
        }

        public bool ListFontEnabled
        {
            get
            {
                return (this["ListFont"] != null);
            }
        }

        [DefaultSettingValue("96"), ApplicationScopedSetting, DebuggerNonUserCode]
        public short MaxBackDepth
        {
            get
            {
                return (short) this["MaxBackDepth"];
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("32"), DebuggerNonUserCode]
        public short MaxForwardDepth
        {
            get
            {
                return (short) this["MaxForwardDepth"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("100"), DebuggerNonUserCode]
        public int MinListColumnWidth
        {
            get
            {
                return (int) this["MinListColumnWidth"];
            }
            set
            {
                this["MinListColumnWidth"] = value;
            }
        }

        [DefaultSettingValue("True"), ApplicationScopedSetting, DebuggerNonUserCode]
        public bool OptimizedColumnCount
        {
            get
            {
                return (bool) this["OptimizedColumnCount"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("PrefixSearch, ExecuteOnEnter"), DebuggerNonUserCode]
        public Nomad.QuickFindOptions QuickFindOptions
        {
            get
            {
                return (Nomad.QuickFindOptions) this["QuickFindOptions"];
            }
            set
            {
                this["QuickFindOptions"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool ShowColumnLines
        {
            get
            {
                return (bool) this["ShowColumnLines"];
            }
            set
            {
                this["ShowColumnLines"] = value;
            }
        }

        [DefaultSettingValue("True"), DebuggerNonUserCode, UserScopedSetting]
        public bool ShowUpFolderItem
        {
            get
            {
                return (bool) this["ShowUpFolderItem"];
            }
            set
            {
                this["ShowUpFolderItem"] = value;
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("96, 96"), DebuggerNonUserCode]
        public Size ThumbnailSize
        {
            get
            {
                return (Size) this["ThumbnailSize"];
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("120, -1"), DebuggerNonUserCode]
        public Size ThumbnailSpacingSize
        {
            get
            {
                return (Size) this["ThumbnailSpacingSize"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool UseHiddenItemsList
        {
            get
            {
                return (bool) this["UseHiddenItemsList"];
            }
            set
            {
                this["UseHiddenItemsList"] = value;
            }
        }
    }
}

