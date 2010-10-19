namespace Nomad.FileSystem.Property.Providers
{
    using System.Configuration;
    using System.Drawing;

    public class TextProviderSection : ConfigurationSection
    {
        [ConfigurationProperty("extensions", IsDefaultCollection=false), ConfigurationCollection(typeof(TextProviderCollection))]
        public TextProviderCollection TextProviders
        {
            get
            {
                return (TextProviderCollection) base["extensions"];
            }
        }

        [ConfigurationProperty("thumbnailSize", DefaultValue="120, 120")]
        public Size ThumbnailSize
        {
            get
            {
                return (Size) base["thumbnailSize"];
            }
        }
    }
}

