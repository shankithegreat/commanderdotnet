namespace Nomad.FileSystem.Properties
{
    using Nomad.FileSystem.Property;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated]
    public sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        [DefaultSettingValue("g"), DebuggerNonUserCode, UserScopedSetting]
        public string DateTimeFormat
        {
            get
            {
                return (string) this["DateTimeFormat"];
            }
            set
            {
                this["DateTimeFormat"] = value;
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DefaultSettingValue("Dynamic"), DebuggerNonUserCode]
        public Nomad.FileSystem.Property.SizeFormat SizeFormat
        {
            get
            {
                return (Nomad.FileSystem.Property.SizeFormat) this["SizeFormat"];
            }
            set
            {
                this["SizeFormat"] = value;
            }
        }
    }
}

