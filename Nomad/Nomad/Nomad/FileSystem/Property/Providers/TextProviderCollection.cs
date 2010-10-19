namespace Nomad.FileSystem.Property.Providers
{
    using System;
    using System.Configuration;
    using System.Reflection;

    public class TextProviderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TextProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TextProviderElement) element).Extension;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public TextProviderElement this[string Name]
        {
            get
            {
                return (TextProviderElement) base.BaseGet(Name.ToLower());
            }
        }

        public TextProviderElement this[int index]
        {
            get
            {
                return (TextProviderElement) base.BaseGet(index);
            }
        }
    }
}

