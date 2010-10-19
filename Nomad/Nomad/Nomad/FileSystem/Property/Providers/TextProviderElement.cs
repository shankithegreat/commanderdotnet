namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Text;

    public class TextProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("backColor", DefaultValue="White")]
        public Color BackColor
        {
            get
            {
                return (Color) base["backColor"];
            }
        }

        [ConfigurationProperty("detectEncoding", DefaultValue="true")]
        public bool DetectEncoding
        {
            get
            {
                return (Convert.ToBoolean(base["detectEncoding"]) || (base["encoding"] == null));
            }
        }

        [ConfigurationProperty("encoding")]
        public System.Text.Encoding Encoding
        {
            get
            {
                if (base["encoding"] == null)
                {
                    return System.Text.Encoding.Default;
                }
                return (System.Text.Encoding) base["encoding"];
            }
        }

        [ConfigurationProperty("ext", IsKey=true, IsRequired=true)]
        public string Extension
        {
            get
            {
                return ((string) base["ext"]).ToLower();
            }
        }

        [ConfigurationProperty("font", DefaultValue="Lucida Console, 7")]
        public System.Drawing.Font Font
        {
            get
            {
                return (System.Drawing.Font) base["font"];
            }
        }

        [ConfigurationProperty("foreColor", DefaultValue="Black")]
        public Color ForeColor
        {
            get
            {
                return (Color) base["foreColor"];
            }
        }

        [ConfigurationProperty("removeBlankLines", DefaultValue="false")]
        public bool RemoveBlankLines
        {
            get
            {
                return Convert.ToBoolean(base["removeBlankLines"]);
            }
        }

        [ConfigurationProperty("wrapLines", DefaultValue="false")]
        public bool WrapLines
        {
            get
            {
                return Convert.ToBoolean(base["wrapLines"]);
            }
        }
    }
}

