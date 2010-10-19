namespace Nomad.Configuration
{
    using System;
    using System.Configuration;
    using System.Reflection;

    public class SectionBlockCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SectionBlockElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SectionBlockElement) element).BlockName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public SectionBlockElement this[int index]
        {
            get
            {
                return (SectionBlockElement) base.BaseGet(index);
            }
        }
    }
}

