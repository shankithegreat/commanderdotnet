namespace Nomad.Themes
{
    using System;
    using System.Configuration;
    using System.Reflection;

    public class ColorElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ColorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ColorElement) element).Color;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ColorElement this[string Name]
        {
            get
            {
                return (ColorElement) base.BaseGet(Name);
            }
        }

        public ColorElement this[int index]
        {
            get
            {
                return (ColorElement) base.BaseGet(index);
            }
        }
    }
}

