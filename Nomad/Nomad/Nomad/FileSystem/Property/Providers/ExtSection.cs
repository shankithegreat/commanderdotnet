namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Configuration;

    public class ExtSection : ConfigurationSection
    {
        [ConfigurationProperty("ext", IsRequired=true)]
        public string Extension
        {
            get
            {
                return (string) base["ext"];
            }
        }
    }
}

