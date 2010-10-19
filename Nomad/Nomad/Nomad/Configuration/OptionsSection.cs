namespace Nomad.Configuration
{
    using System;
    using System.Configuration;

    public class OptionsSection : ConfigurationSection
    {
        [ConfigurationProperty("order"), IntegerValidator(MinValue=0)]
        public int Order
        {
            get
            {
                return (int) base["order"];
            }
        }

        [ConfigurationProperty("sectionBlocks", IsDefaultCollection=false), ConfigurationCollection(typeof(SectionBlockCollection))]
        public SectionBlockCollection SectionBlocks
        {
            get
            {
                return (SectionBlockCollection) base["sectionBlocks"];
            }
        }

        [ConfigurationProperty("text", IsRequired=true)]
        public string SectionCaption
        {
            get
            {
                return (string) base["text"];
            }
        }

        [ConfigurationProperty("description")]
        public string SectionDescription
        {
            get
            {
                return (string) base["description"];
            }
        }

        [ConfigurationProperty("image")]
        public string SectionImage
        {
            get
            {
                return (string) base["image"];
            }
        }
    }
}

