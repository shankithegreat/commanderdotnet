namespace Nomad.Themes
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Xml;

    public class ThemeConfigurationSection : ConfigurationSection
    {
        private string _ThemeKey;

        public static ThemeConfigurationSection Load(string fileName)
        {
            ThemeConfigurationSection section2;
            try
            {
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    reader.MoveToContent();
                    ThemeConfigurationSection section = new ThemeConfigurationSection();
                    section.DeserializeElement(reader, false);
                    section._ThemeKey = Path.GetFileName(fileName);
                    section2 = section;
                }
            }
            catch
            {
                section2 = null;
            }
            return section2;
        }

        [ConfigurationProperty("caption", IsRequired=true)]
        public string Caption
        {
            get
            {
                return (string) base["caption"];
            }
        }

        [ConfigurationProperty("isDefault", DefaultValue=false)]
        public bool IsDefault
        {
            get
            {
                return (bool) base["isDefault"];
            }
        }

        [ConfigurationProperty("listViewColors")]
        public ColorTableConfigurationElement ListViewColors
        {
            get
            {
                return (ColorTableConfigurationElement) base["listViewColors"];
            }
        }

        [ConfigurationProperty("tabStripRenderer")]
        public TabStripConfigurationElement TabStripRenderer
        {
            get
            {
                return (TabStripConfigurationElement) base["tabStripRenderer"];
            }
        }

        public string ThemeColorName
        {
            get
            {
                string caption = this.Caption;
                int index = caption.IndexOf('/');
                if (index > 0)
                {
                    return caption.Substring(index + 1);
                }
                return null;
            }
        }

        [ConfigurationProperty("themeColors")]
        public ColorTableConfigurationElement ThemeColors
        {
            get
            {
                return (ColorTableConfigurationElement) base["themeColors"];
            }
        }

        public string ThemeFamilyName
        {
            get
            {
                string caption = this.Caption;
                int index = caption.IndexOf('/');
                if (index > 0)
                {
                    return caption.Substring(0, index);
                }
                return caption;
            }
        }

        public string ThemeKey
        {
            get
            {
                return (this._ThemeKey ?? base.SectionInformation.SectionName);
            }
        }

        [ConfigurationProperty("type", IsRequired=true)]
        public string ThemeType
        {
            get
            {
                return (string) base["type"];
            }
        }

        [ConfigurationProperty("toolStripRenderer")]
        public ToolStripConfigurationElement ToolStripRenderer
        {
            get
            {
                return (ToolStripConfigurationElement) base["toolStripRenderer"];
            }
        }
    }
}

