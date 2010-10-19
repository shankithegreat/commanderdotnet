namespace Nomad.FileSystem.Null
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Reflection;

    public class NullItem : CustomPropertyProvider, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        private string FName;

        protected NullItem(string name)
        {
            this.FName = name;
        }

        public bool Equals(IVirtualItem other)
        {
            return ((other != null) && (other.GetType() == base.GetType()));
        }

        public virtual FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Normal;
            }
        }

        public override VirtualPropertySet AvailableProperties
        {
            get
            {
                return DefaultProperty.NameAttrPropertySet;
            }
        }

        public string FullName
        {
            get
            {
                return (NullFileSystemCreator.UriScheme + Uri.SchemeDelimiter + "./" + this.FName);
            }
        }

        public object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 0:
                        return this.Name;

                    case 1:
                        return Path.GetExtension(this.FName);

                    case 6:
                        return this.Attributes;
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                return this.FName;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return null;
            }
        }

        public string ShortName
        {
            get
            {
                return this.FName;
            }
        }
    }
}

