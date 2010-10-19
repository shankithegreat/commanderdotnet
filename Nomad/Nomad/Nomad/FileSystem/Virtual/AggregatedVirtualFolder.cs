namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    [Serializable]
    public class AggregatedVirtualFolder : CustomVirtualFolder, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable
    {
        private List<IVirtualItem> FContent = null;

        public AggregatedVirtualFolder(IEnumerable<IVirtualItem> content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (content is ICollection)
            {
                this.FContent = new List<IVirtualItem>(((ICollection) content).Count);
            }
            else
            {
                this.FContent = new List<IVirtualItem>();
            }
            foreach (IVirtualItem item in content)
            {
                ICloneable cloneable = item as ICloneable;
                if (cloneable != null)
                {
                    IVirtualItem item2 = (IVirtualItem) cloneable.Clone();
                    this.FContent.Add(item2);
                }
                else
                {
                    this.FContent.Add(item);
                }
            }
        }

        public void Dispose()
        {
        }

        public bool Equals(IVirtualItem other)
        {
            return false;
        }

        public override IEnumerable<IVirtualItem> GetContent()
        {
            return this.FContent;
        }

        public PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            return (this.AvailableProperties[propertyId] ? PropertyAvailability.Normal : PropertyAvailability.None);
        }

        public bool HasChildren()
        {
            return ((this.FContent != null) && (this.FContent.Count > 0));
        }

        public bool IsChild(IVirtualItem Item)
        {
            return false;
        }

        public FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Directory;
            }
        }

        public VirtualPropertySet AvailableProperties
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
                return this.Name;
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
                return "Aggregated Folder";
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
                return this.Name;
            }
        }
    }
}

