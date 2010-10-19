namespace Nomad.Themes
{
    using System;
    using System.Configuration;

    public class TabStripConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("colorTable")]
        public ColorTableConfigurationElement ColorTable
        {
            get
            {
                return (ColorTableConfigurationElement) base["colorTable"];
            }
        }

        [ConfigurationProperty("drawFocusRect", DefaultValue=true)]
        public bool DrawFocusRect
        {
            get
            {
                return (bool) base["drawFocusRect"];
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

        [ConfigurationProperty("useBoldFont", DefaultValue=true)]
        public bool UseBoldFont
        {
            get
            {
                return (bool) base["useBoldFont"];
            }
        }

        [ConfigurationProperty("useVisualStyles", DefaultValue=true)]
        public bool UseVisualStyles
        {
            get
            {
                return (bool) base["useVisualStyles"];
            }
        }
    }
}

