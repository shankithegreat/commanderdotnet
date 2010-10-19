namespace Nomad.Themes
{
    using System;
    using System.Configuration;

    public class ToolStripConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("colorTable")]
        public ColorTableConfigurationElement ColorTable
        {
            get
            {
                return (ColorTableConfigurationElement) base["colorTable"];
            }
        }

        [ConfigurationProperty("fontFamily", DefaultValue=null)]
        public string FontFamily
        {
            get
            {
                return (string) base["fontFamily"];
            }
        }

        [ConfigurationProperty("rendererType")]
        public string RendererType
        {
            get
            {
                return (string) base["rendererType"];
            }
        }

        [ConfigurationProperty("roundedEdges", DefaultValue=false)]
        public bool RoundedEdges
        {
            get
            {
                return (bool) base["roundedEdges"];
            }
        }

        [ConfigurationProperty("useSystemColors", DefaultValue=false)]
        public bool UseSystemColors
        {
            get
            {
                return (bool) base["useSystemColors"];
            }
        }
    }
}

