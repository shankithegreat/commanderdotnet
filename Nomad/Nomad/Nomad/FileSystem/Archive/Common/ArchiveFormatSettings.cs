namespace Nomad.FileSystem.Archive.Common
{
    using Nomad.Commons;
    using Nomad.Configuration;
    using System;
    using System.Configuration;
    using System.Text;

    [SettingsProvider(typeof(ConfigurableSettingsProvider))]
    public class ArchiveFormatSettings : ApplicationSettingsBase
    {
        private static ArchiveFormatSettings defaultInstance = ((ArchiveFormatSettings) SettingsBase.Synchronized(new ArchiveFormatSettings()));
        private NameFilter FHideMasksFilter;

        public static ArchiveFormatSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False")]
        public bool Disabled
        {
            get
            {
                return (bool) this["Disabled"];
            }
            set
            {
                this["Disabled"] = value;
            }
        }

        [UserScopedSetting]
        public string Extension
        {
            get
            {
                return (string) this["Extension"];
            }
            set
            {
                this["Extension"] = value;
            }
        }

        public string[] ExtensionList
        {
            get
            {
                return ((this.Extension == null) ? null : this.Extension.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }
            set
            {
                StringBuilder builder = new StringBuilder();
                if (value != null)
                {
                    foreach (string str in value)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append(',');
                        }
                        builder.Append(str);
                    }
                }
                this.Extension = builder.ToString();
            }
        }

        [UserScopedSetting, DefaultSettingValue("False")]
        public bool HideFormat
        {
            get
            {
                return (bool) this["HideFormat"];
            }
            set
            {
                this["HideFormat"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("*.chm;*.doc;*.docx;*.exe;*.msi;*.ppt;*.pptx;*.odp;*.ods;*.odt;*.xls;*.xlsx")]
        public string HideMask
        {
            get
            {
                return (string) this["HideMask"];
            }
            set
            {
                this["HideMask"] = value;
                this.FHideMasksFilter = null;
            }
        }

        public NameFilter HideMaskFilter
        {
            get
            {
                if (!((this.FHideMasksFilter != null) || string.IsNullOrEmpty(this.HideMask)))
                {
                    this.FHideMasksFilter = new NameFilter(this.HideMask);
                }
                return this.FHideMasksFilter;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting]
        public bool Initialized
        {
            get
            {
                return (bool) this["Initialized"];
            }
            set
            {
                this["Initialized"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False")]
        public bool UsePipes
        {
            get
            {
                return (bool) this["UsePipes"];
            }
            set
            {
                this["UsePipes"] = value;
            }
        }
    }
}

