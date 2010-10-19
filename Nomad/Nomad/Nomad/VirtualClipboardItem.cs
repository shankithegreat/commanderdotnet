namespace Nomad
{
    using Microsoft.COM;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices.ComTypes;
    using System.Windows.Forms;

    internal class VirtualClipboardItem : CustomPropertyProvider, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        private FileAttributes FAttributes;
        private DateTime? FCreationTime;
        private string FFullName;
        private DateTime? FLastAccessTime;
        private DateTime? FLastWriteTime;
        private string FName;
        private IVirtualFolder FParent;

        public VirtualClipboardItem(FILEDESCRIPTORA descriptor)
        {
            this.FFullName = descriptor.cFileName;
            if ((descriptor.dwFlags & FD.FD_ATTRIBUTES) > ((FD) 0))
            {
                this.FAttributes = descriptor.dwFileAttributes;
            }
            if ((descriptor.dwFlags & FD.FD_CREATETIME) > ((FD) 0))
            {
                this.FCreationTime = new DateTime?(FileTimeToDateTime(descriptor.ftCreationTime));
            }
            if ((descriptor.dwFlags & FD.FD_ACCESSTIME) > ((FD) 0))
            {
                this.FLastAccessTime = new DateTime?(FileTimeToDateTime(descriptor.ftLastAccessTime));
            }
            if ((descriptor.dwFlags & FD.FD_WRITESTIME) > ((FD) 0))
            {
                this.FLastWriteTime = new DateTime?(FileTimeToDateTime(descriptor.ftLastWriteTime));
            }
        }

        public VirtualClipboardItem(FILEDESCRIPTORW descriptor)
        {
            this.FFullName = descriptor.cFileName;
            if ((descriptor.dwFlags & FD.FD_ATTRIBUTES) > ((FD) 0))
            {
                this.FAttributes = descriptor.dwFileAttributes;
            }
            if ((descriptor.dwFlags & FD.FD_CREATETIME) > ((FD) 0))
            {
                this.FCreationTime = new DateTime?(FileTimeToDateTime(descriptor.ftCreationTime));
            }
            if ((descriptor.dwFlags & FD.FD_ACCESSTIME) > ((FD) 0))
            {
                this.FLastAccessTime = new DateTime?(FileTimeToDateTime(descriptor.ftLastAccessTime));
            }
            if ((descriptor.dwFlags & FD.FD_WRITESTIME) > ((FD) 0))
            {
                this.FLastWriteTime = new DateTime?(FileTimeToDateTime(descriptor.ftLastWriteTime));
            }
        }

        public static bool ClipboardContainItems()
        {
            return DataObjectContainItems(Clipboard.GetDataObject());
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            int[] properties = new int[2];
            properties[1] = 6;
            VirtualPropertySet set = new VirtualPropertySet(properties);
            set[7] = this.FCreationTime.HasValue;
            set[9] = this.FLastAccessTime.HasValue;
            set[8] = this.FLastWriteTime.HasValue;
            return set;
        }

        public static bool DataObjectContainItems(System.Windows.Forms.IDataObject dataObject)
        {
            return (((dataObject.GetDataPresent("Virtual Items") || dataObject.GetDataPresent(DataFormats.FileDrop)) || dataObject.GetDataPresent("FileGroupDescriptorW")) || dataObject.GetDataPresent("FileGroupDescriptor"));
        }

        public bool Equals(IVirtualItem other)
        {
            return ((other is VirtualClipboardItem) && string.Equals(other.Name, this.FFullName, StringComparison.OrdinalIgnoreCase));
        }

        private static DateTime FileTimeToDateTime(System.Runtime.InteropServices.ComTypes.FILETIME time)
        {
            return DateTime.FromFileTime((time.dwHighDateTime << 0x20) | ((long) ((ulong) time.dwLowDateTime)));
        }

        public static IEnumerable<IVirtualItem> GetClipboardItems()
        {
            if (Clipboard.ContainsData("Virtual Items"))
            {
                IEnumerable<IVirtualItem> data = Clipboard.GetData("Virtual Items") as IEnumerable<IVirtualItem>;
                if (data == null)
                {
                    Clipboard.Clear();
                }
                return data;
            }
            List<IVirtualItem> list = new List<IVirtualItem>();
            if (Clipboard.ContainsFileDropList())
            {
                foreach (string str in Clipboard.GetFileDropList())
                {
                    list.Add(LocalFileSystemCreator.FromFullName(str, VirtualItemType.Unknown, null));
                }
            }
            IEnumerable<IVirtualItem> dataObjectContent = GetDataObjectContent(Clipboard.GetDataObject());
            if (dataObjectContent != null)
            {
                list.AddRange(dataObjectContent);
            }
            return ((list.Count > 0) ? list : null);
        }

        private static IEnumerable<IVirtualItem> GetDataObjectContent(System.Windows.Forms.IDataObject dataObject)
        {
            if (dataObject == null)
            {
                return null;
            }
            List<IVirtualItem> list = new List<IVirtualItem>();
            bool dataPresent = dataObject.GetDataPresent("FileGroupDescriptorW");
            bool flag2 = dataObject.GetDataPresent("FileGroupDescriptor");
            if (dataPresent || flag2)
            {
                System.Runtime.InteropServices.ComTypes.IDataObject obj2 = dataObject as System.Runtime.InteropServices.ComTypes.IDataObject;
                if (obj2 != null)
                {
                    object data = null;
                    if (dataPresent)
                    {
                        data = dataObject.GetData("FileGroupDescriptorW");
                    }
                    else if (flag2)
                    {
                        data = dataObject.GetData("FileGroupDescriptor");
                    }
                    Stream input = data as Stream;
                    if (input != null)
                    {
                        Dictionary<string, VirtualClipboardFolder> dictionary = new Dictionary<string, VirtualClipboardFolder>(StringComparer.OrdinalIgnoreCase);
                        int num = new BinaryReader(input).ReadInt32();
                        for (int i = 0; i < num; i++)
                        {
                            VirtualClipboardItem item;
                            if (dataPresent)
                            {
                                FILEDESCRIPTORW descriptor = ByteArrayHelper.ReadStructureFromStream<FILEDESCRIPTORW>(input);
                                if (((descriptor.dwFlags & FD.FD_ATTRIBUTES) > ((FD) 0)) && ((descriptor.dwFileAttributes & FileAttributes.Directory) > 0))
                                {
                                    item = new VirtualClipboardFolder(descriptor);
                                }
                                else
                                {
                                    item = new VirtualClipboardFile(descriptor, obj2, i);
                                }
                            }
                            else
                            {
                                FILEDESCRIPTORA filedescriptora = ByteArrayHelper.ReadStructureFromStream<FILEDESCRIPTORA>(input);
                                if (((filedescriptora.dwFlags & FD.FD_ATTRIBUTES) > ((FD) 0)) && ((filedescriptora.dwFileAttributes & FileAttributes.Directory) > 0))
                                {
                                    item = new VirtualClipboardFolder(filedescriptora);
                                }
                                else
                                {
                                    item = new VirtualClipboardFile(filedescriptora, obj2, i);
                                }
                            }
                            string directoryName = Path.GetDirectoryName(item.FullName);
                            if (string.IsNullOrEmpty(directoryName))
                            {
                                list.Add(item);
                            }
                            else
                            {
                                VirtualClipboardFolder folder = dictionary[directoryName];
                                item.Parent = folder;
                                folder.Content.Add(item);
                            }
                            VirtualClipboardFolder folder2 = item as VirtualClipboardFolder;
                            if (folder2 != null)
                            {
                                dictionary.Add(PathHelper.ExcludeTrailingDirectorySeparator(item.FullName), folder2);
                            }
                        }
                    }
                }
            }
            return ((list.Count > 0) ? list : null);
        }

        public static IEnumerable<IVirtualItem> GetDataObjectItems(System.Windows.Forms.IDataObject dataObject)
        {
            if (dataObject.GetDataPresent("Virtual Items"))
            {
                return (dataObject.GetData("Virtual Items") as IEnumerable<IVirtualItem>);
            }
            List<IVirtualItem> list = new List<IVirtualItem>();
            if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                System.Collections.IEnumerable data = dataObject.GetData(DataFormats.FileDrop) as System.Collections.IEnumerable;
                foreach (string str in data)
                {
                    list.Add(LocalFileSystemCreator.FromFullName(str, VirtualItemType.Unknown, null));
                }
            }
            IEnumerable<IVirtualItem> dataObjectContent = GetDataObjectContent(dataObject);
            if (dataObjectContent != null)
            {
                list.AddRange(dataObjectContent);
            }
            return ((list.Count > 0) ? list : null);
        }

        public FileAttributes Attributes
        {
            get
            {
                return this.FAttributes;
            }
        }

        public string FullName
        {
            get
            {
                return this.FFullName;
            }
        }

        public virtual object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 6:
                        return this.FAttributes;

                    case 7:
                        return this.FCreationTime;

                    case 8:
                        return this.FLastWriteTime;

                    case 9:
                        return this.FLastAccessTime;

                    case 0:
                        return this.FFullName;
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (this.FName == null)
                {
                    this.FName = Path.GetFileName(this.FFullName);
                }
                return this.FName;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return this.FParent;
            }
            private set
            {
                this.FParent = value;
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

