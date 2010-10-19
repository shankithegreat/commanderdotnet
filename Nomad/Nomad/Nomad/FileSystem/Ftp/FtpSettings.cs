namespace Nomad.FileSystem.Ftp
{
    using Nomad.Configuration;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [CompilerGenerated, SettingsProvider(typeof(ConfigurableSettingsProvider)), GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class FtpSettings : ApplicationSettingsBase
    {
        private static FtpSettings defaultInstance = ((FtpSettings) SettingsBase.Synchronized(new FtpSettings()));

        [DebuggerNonUserCode, DefaultSettingValue("windows-1251,cp866,koi8-r"), ApplicationScopedSetting]
        public string AdditionalEncodings
        {
            get
            {
                return (string) this["AdditionalEncodings"];
            }
        }

        public static FtpSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("")]
        public string Encoding
        {
            get
            {
                return (string) this["Encoding"];
            }
            set
            {
                this["Encoding"] = value;
            }
        }

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue(@"^(?<month>\d{2})-(?<day>\d{2})-(?<year>\d{2})\s{2}(?<hour>\d{2}):(?<min>\d{2})(?<ampm>AM|PM)\s+(<(?<type>D)IR>\s+|(?<size>\d+)\s)(?<name>.+)$")]
        public string ListPatternDos
        {
            get
            {
                return (string) this["ListPatternDos"];
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue(@"^(?<type>d|l|-)(?:.+\s(?<size>\d+)|.+)\s(?<date>(?<monthname>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)(?:\s(?<day>\d{2})|\s{2}(?<day>\d{1}))(?:\s{2}(?<year>\d{4})|(?:\s(?<hour>\d{2})|\s{2}(?<hour>\d{1})):(?<min>\d{2})))\s(?:(?<name>.+)\s->\s(?<linktarget>.+)|(?<name>.+))$"), ApplicationScopedSetting]
        public string ListPatternUnix
        {
            get
            {
                return (string) this["ListPatternUnix"];
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool StoreCredential
        {
            get
            {
                return (bool) this["StoreCredential"];
            }
            set
            {
                this["StoreCredential"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("Normal")]
        public CharacterCasing UploadFileNameCasing
        {
            get
            {
                return (CharacterCasing) this["UploadFileNameCasing"];
            }
            set
            {
                this["UploadFileNameCasing"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool UseCache
        {
            get
            {
                return (bool) this["UseCache"];
            }
            set
            {
                this["UseCache"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool UsePassive
        {
            get
            {
                return (bool) this["UsePassive"];
            }
            set
            {
                this["UsePassive"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("False")]
        public bool UsePrefetch
        {
            get
            {
                return (bool) this["UsePrefetch"];
            }
            set
            {
                this["UsePrefetch"] = value;
            }
        }
    }
}

