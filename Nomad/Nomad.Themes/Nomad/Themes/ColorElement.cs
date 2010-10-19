namespace Nomad.Themes
{
    using System;
    using System.Configuration;
    using System.Drawing;

    public class ColorElement : ConfigurationElement
    {
        [ConfigurationProperty("alpha", DefaultValue=0xff)]
        public byte Alpha
        {
            get
            {
                return (byte) base["alpha"];
            }
        }

        [ConfigurationProperty("color", IsRequired=true, IsKey=true)]
        public string Color
        {
            get
            {
                return (string) base["color"];
            }
        }

        [ConfigurationProperty("value")]
        public System.Drawing.Color Value
        {
            get
            {
                return (System.Drawing.Color) base["value"];
            }
        }
    }
}

