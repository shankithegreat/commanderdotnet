namespace Nomad
{
    using Nomad.Configuration;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [CompilerGenerated, SettingsProvider(typeof(ConfigurableSettingsProvider)), GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class TwoPanelContainerSettings : ApplicationSettingsBase
    {
        private static TwoPanelContainerSettings defaultInstance = ((TwoPanelContainerSettings) SettingsBase.Synchronized(new TwoPanelContainerSettings()));

        public static TwoPanelContainerSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("None")]
        public TwoPanelContainer.SinglePanel OnePanelMode
        {
            get
            {
                return (TwoPanelContainer.SinglePanel) this["OnePanelMode"];
            }
            set
            {
                this["OnePanelMode"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("Vertical"), DebuggerNonUserCode]
        public Orientation PanelsOrientation
        {
            get
            {
                return (Orientation) this["PanelsOrientation"];
            }
            set
            {
                this["PanelsOrientation"] = value;
            }
        }

        [DefaultSettingValue("500"), UserScopedSetting, DebuggerNonUserCode]
        public int SplitterPercent
        {
            get
            {
                return (int) this["SplitterPercent"];
            }
            set
            {
                this["SplitterPercent"] = value;
            }
        }
    }
}

