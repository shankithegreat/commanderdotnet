namespace Nomad.Workers.Configuration
{
    using Nomad.Configuration;
    using Nomad.Workers;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [SettingsProvider(typeof(ConfigurableSettingsProvider)), CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    public sealed class CopySettings : ApplicationSettingsBase
    {
        private static CopySettings defaultInstance = ((CopySettings) SettingsBase.Synchronized(new CopySettings()));

        [DebuggerNonUserCode, DefaultSettingValue("262144"), UserScopedSetting]
        public int CopyBufferSize
        {
            get
            {
                return (int) this["CopyBufferSize"];
            }
            set
            {
                this["CopyBufferSize"] = value;
            }
        }

        public static CopySettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DefaultSettingValue("AutoAsyncCopy, CheckFreeSpace, ClearROFromCD, CopyItemTime, UseSystemCopy"), DebuggerNonUserCode, UserScopedSetting]
        public CopyWorkerOptions DefaultCopyOptions
        {
            get
            {
                return (CopyWorkerOptions) this["DefaultCopyOptions"];
            }
            set
            {
                this["DefaultCopyOptions"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool ShowThumbnailInOverwriteDialog
        {
            get
            {
                return (bool) this["ShowThumbnailInOverwriteDialog"];
            }
            set
            {
                this["ShowThumbnailInOverwriteDialog"] = value;
            }
        }

        [DefaultSettingValue("96, 96"), DebuggerNonUserCode, ApplicationScopedSetting]
        public Size ThumbnailSize
        {
            get
            {
                return (Size) this["ThumbnailSize"];
            }
        }
    }
}

