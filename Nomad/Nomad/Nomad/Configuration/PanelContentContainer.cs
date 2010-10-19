namespace Nomad.Configuration
{
    using Nomad;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class PanelContentContainer : FilterContainer
    {
        private VirtualItemContainer<IVirtualFolder> FFolder = new VirtualItemContainer<IVirtualFolder>();
        private IComparer<IVirtualItem> FSort;

        [XmlIgnore]
        public VirtualItemContainer<IVirtualFolder> Folder
        {
            get
            {
                return this.FFolder;
            }
        }

        [DefaultValue(false)]
        public bool Locked { get; set; }

        [DefaultValue(9)]
        public Nomad.QuickFindOptions QuickFindOptions { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("FolderPath")]
        public string SerializableFolderPath
        {
            get
            {
                return this.Folder.SerializableItemPath;
            }
            set
            {
                this.Folder.SerializableItemPath = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("FolderStream", DataType="base64Binary")]
        public byte[] SerializableFolderStream
        {
            get
            {
                return this.Folder.SerializableItemStream;
            }
            set
            {
                this.Folder.SerializableItemStream = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), XmlElement("Sort")]
        public string SerializableSort
        {
            get
            {
                return XmlSerializable.ObjectToString<VirtualItemComparer>(this.FSort as VirtualItemComparer);
            }
            set
            {
                this.FSort = XmlSerializable.StringToObject<VirtualItemComparer>(value);
            }
        }

        [XmlIgnore]
        public IComparer<IVirtualItem> Sort
        {
            get
            {
                return this.FSort;
            }
            set
            {
                this.FSort = value;
            }
        }
    }
}

