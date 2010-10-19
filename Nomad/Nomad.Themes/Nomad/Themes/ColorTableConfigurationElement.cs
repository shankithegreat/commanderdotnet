namespace Nomad.Themes
{
    using System;
    using System.Configuration;

    public class ColorTableConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection=true), ConfigurationCollection(typeof(ColorElementCollection))]
        public ColorElementCollection ColorTable
        {
            get
            {
                return (ColorElementCollection) base[""];
            }
        }

        [ConfigurationProperty("colorTableType")]
        public string ColorTableType
        {
            get
            {
                return (string) base["colorTableType"];
            }
        }
    }
}

