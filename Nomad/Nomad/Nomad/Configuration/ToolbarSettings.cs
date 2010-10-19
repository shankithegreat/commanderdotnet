namespace Nomad.Configuration
{
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), SettingsProvider(typeof(ConfigurableSettingsProvider)), CompilerGenerated]
    internal sealed class ToolbarSettings : ApplicationSettingsBase
    {
        private static ToolbarSettings defaultInstance = ((ToolbarSettings) SettingsBase.Synchronized(new ToolbarSettings()));
        private static List<ToolbarSettings> ToolbarList;

        public ToolbarSettings()
        {
            this.CompactProperties();
        }

        public static string CreateToolbarButtonLine(string command, ToolStripItemDisplayStyle displayStyle, IconLocation imageLocation)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
            if (command == string.Empty)
            {
                throw new ArgumentException();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(command);
            if (displayStyle != ToolStripItemDisplayStyle.Image)
            {
                builder.Append(',');
                builder.Append(displayStyle);
            }
            if (imageLocation != null)
            {
                if (displayStyle == ToolStripItemDisplayStyle.Image)
                {
                    builder.Append(',');
                }
                builder.Append(',');
                builder.Append(imageLocation.IconFileName);
                if (imageLocation.IconIndex >= 0)
                {
                    builder.Append(',');
                    builder.Append(imageLocation.IconIndex);
                }
            }
            return builder.ToString();
        }

        protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
        {
            base.OnSettingsLoaded(sender, e);
            this.CompactPropertyValues();
        }

        public static bool ParseToolbarButtonLine(string source, out string command, ref ToolStripItemDisplayStyle displayStyle, out IconLocation imageLocation)
        {
            command = null;
            imageLocation = null;
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }
            int startIndex = 0;
            command = StringHelper.ReadValue(source, ref startIndex, new char[] { ',' }).Trim();
            if (startIndex > 0)
            {
                string str = StringHelper.ReadValue(source, ref startIndex, new char[] { ',' }).Trim();
                try
                {
                    displayStyle = (ToolStripItemDisplayStyle) Enum.Parse(typeof(ToolStripItemDisplayStyle), str);
                }
                catch (ArgumentException)
                {
                }
            }
            if (startIndex > 0)
            {
                string iconLocation = source.Substring(startIndex).Trim();
                imageLocation = IconLocation.TryParse(iconLocation);
                if (!(((imageLocation == null) || (imageLocation.IconIndex != 0)) || iconLocation.EndsWith(",0", StringComparison.Ordinal)))
                {
                    imageLocation = new IconLocation(imageLocation.IconFileName, -1);
                }
            }
            return true;
        }

        public string Caption { get; private set; }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string Commands
        {
            get
            {
                return (string) this["Commands"];
            }
            set
            {
                this["Commands"] = value;
            }
        }

        public static ToolbarSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("actBack,ImageAndText\r\nactForward\r\nactRefresh\r\n-\r\nactBookmarkCurrentFolder\r\nactChangeFolder\r\ntsmiSpecialFolders\r\n-\r\nactFind,ImageAndText\r\n-\r\nactCutToClipboard\r\nactCopyToClipboard\r\nactPasteFromClipboard\r\n-\r\nactNewFile\r\nactMakeFolder\r\nactCopy\r\nactDelete\r\nactRunAs\r\nactSetAttributes\r\n-\r\nTools")]
        public string DefaultCommands
        {
            get
            {
                return (string) this["DefaultCommands"];
            }
        }

        [DefaultSettingValue("Top"), DebuggerNonUserCode, UserScopedSetting]
        public DockStyle Dock
        {
            get
            {
                return (DockStyle) this["Dock"];
            }
            set
            {
                this["Dock"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool JustifyLabels
        {
            get
            {
                return (bool) this["JustifyLabels"];
            }
            set
            {
                this["JustifyLabels"] = value;
            }
        }

        public static IList<ToolbarSettings> Toolbars
        {
            get
            {
                if (ToolbarList == null)
                {
                    NameValueCollection section = ConfigurationManager.GetSection("toolbars") as NameValueCollection;
                    if ((section != null) && (section.Count > 0))
                    {
                        ToolbarList = new List<ToolbarSettings>(section.Count);
                        for (int i = 0; i < section.Count; i++)
                        {
                            ToolbarSettings item = new ToolbarSettings {
                                SettingsKey = section.Keys[i],
                                Caption = section[i]
                            };
                            if (string.IsNullOrEmpty(item.Commands) && (i == 0))
                            {
                                item.Commands = item.DefaultCommands;
                            }
                            ToolbarList.Add(item);
                        }
                    }
                    else
                    {
                        ToolbarList = new List<ToolbarSettings>();
                    }
                }
                return ToolbarList;
            }
        }

        [DefaultSettingValue("True"), DebuggerNonUserCode, UserScopedSetting]
        public bool Visible
        {
            get
            {
                return (bool) this["Visible"];
            }
            set
            {
                this["Visible"] = value;
            }
        }
    }
}

