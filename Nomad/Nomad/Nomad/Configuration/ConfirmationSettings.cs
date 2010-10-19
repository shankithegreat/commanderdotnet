namespace Nomad.Configuration
{
    using Nomad.Dialogs;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, SettingsProvider(typeof(ConfigurableSettingsProvider)), GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class ConfirmationSettings : ApplicationSettingsBase
    {
        private static ConfirmationSettings defaultInstance = ((ConfirmationSettings) SettingsBase.Synchronized(new ConfirmationSettings()));

        public ConfirmationSettings()
        {
            this.CompactProperties();
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("None")]
        public MessageDialogResult AnotherInstance
        {
            get
            {
                return (MessageDialogResult) this["AnotherInstance"];
            }
            set
            {
                this["AnotherInstance"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool BookmarkFolder
        {
            get
            {
                return (bool) this["BookmarkFolder"];
            }
            set
            {
                this["BookmarkFolder"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool CloseTabs
        {
            get
            {
                return (bool) this["CloseTabs"];
            }
            set
            {
                this["CloseTabs"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool CopyAlternateDataStreams
        {
            get
            {
                return (bool) this["CopyAlternateDataStreams"];
            }
            set
            {
                this["CopyAlternateDataStreams"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("Ask")]
        public Nomad.Dialogs.CopyDestinationItem CopyDestinationItem
        {
            get
            {
                return (Nomad.Dialogs.CopyDestinationItem) this["CopyDestinationItem"];
            }
            set
            {
                this["CopyDestinationItem"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool CreateAnotherLink
        {
            get
            {
                return (bool) this["CreateAnotherLink"];
            }
            set
            {
                this["CreateAnotherLink"] = value;
            }
        }

        public static ConfirmationSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool DeleteNonEmptyFolder
        {
            get
            {
                return (bool) this["DeleteNonEmptyFolder"];
            }
            set
            {
                this["DeleteNonEmptyFolder"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool DeleteReadOnlyFile
        {
            get
            {
                return (bool) this["DeleteReadOnlyFile"];
            }
            set
            {
                this["DeleteReadOnlyFile"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool DragDrop
        {
            get
            {
                return (bool) this["DragDrop"];
            }
            set
            {
                this["DragDrop"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("None")]
        public MessageDialogResult ExtractOnRun
        {
            get
            {
                return (MessageDialogResult) this["ExtractOnRun"];
            }
            set
            {
                this["ExtractOnRun"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("True"), UserScopedSetting]
        public bool NavigateError
        {
            get
            {
                return (bool) this["NavigateError"];
            }
            set
            {
                this["NavigateError"] = value;
            }
        }

        [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
        public bool Paste
        {
            get
            {
                return (bool) this["Paste"];
            }
            set
            {
                this["Paste"] = value;
            }
        }

        [DefaultSettingValue("None"), UserScopedSetting, DebuggerNonUserCode]
        public MessageDialogResult SaveTabs
        {
            get
            {
                return (MessageDialogResult) this["SaveTabs"];
            }
            set
            {
                this["SaveTabs"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool SearchError
        {
            get
            {
                return (bool) this["SearchError"];
            }
            set
            {
                this["SearchError"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("None")]
        public MessageDialogResult UploadChangedFileBack
        {
            get
            {
                return (MessageDialogResult) this["UploadChangedFileBack"];
            }
            set
            {
                this["UploadChangedFileBack"] = value;
            }
        }
    }
}

