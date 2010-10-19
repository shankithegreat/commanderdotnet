namespace Nomad.Workers.Configuration
{
    using Nomad.Configuration;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated, SettingsProvider(typeof(ConfigurableSettingsProvider))]
    internal sealed class WorkerDialogSettings : ApplicationSettingsBase
    {
        private static WorkerDialogSettings defaultInstance = ((WorkerDialogSettings) SettingsBase.Synchronized(new WorkerDialogSettings()));

        public static WorkerDialogSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DefaultSettingValue("True"), DebuggerNonUserCode]
        public bool DetailsVisible
        {
            get
            {
                return (bool) this["DetailsVisible"];
            }
            set
            {
                this["DetailsVisible"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("False")]
        public bool TopMost
        {
            get
            {
                return (bool) this["TopMost"];
            }
            set
            {
                this["TopMost"] = value;
            }
        }
    }
}

