namespace Nomad.Configuration
{
    using System;
    using System.Configuration;
    using System.Windows.Forms;

    public class SectionBlockElement : ConfigurationElement
    {
        [ConfigurationProperty("text")]
        public string BlockCaption
        {
            get
            {
                return (string) base["text"];
            }
        }

        [ConfigurationProperty("name", IsKey=true, IsRequired=true)]
        public string BlockName
        {
            get
            {
                return (string) base["name"];
            }
        }

        [ConfigurationProperty("type", IsRequired=true)]
        public string BlockType
        {
            get
            {
                return (string) base["type"];
            }
        }

        [ConfigurationProperty("dock", DefaultValue=1)]
        public DockStyle Dock
        {
            get
            {
                return (DockStyle) base["dock"];
            }
        }
    }
}

