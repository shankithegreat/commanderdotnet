namespace Nomad.FileSystem.Special
{
    using Microsoft;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    internal class VirtualTempFolder : CustomPropertyProvider, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, ICreateVirtualFile, ICreateVirtualFolder, IGetVirtualRoot
    {
        private List<IVirtualItem> Content;
        private static VirtualTempFolder FDefault;
        private IVirtualItem FLastCreatedItem;
        private string FTempFolder;

        public VirtualTempFolder()
        {
            this.Content = new List<IVirtualItem>();
            this.FTempFolder = OS.TempDirectory;
        }

        public VirtualTempFolder(string tempFolder)
        {
            this.Content = new List<IVirtualItem>();
            this.FTempFolder = tempFolder;
            if (!Directory.Exists(this.FTempFolder))
            {
                throw new DirectoryNotFoundException();
            }
        }

        public IChangeVirtualFile CreateFile(string name)
        {
            string str = Path.ChangeExtension(StringHelper.GuidToCompactString(Guid.NewGuid()), Path.GetExtension(name));
            string fullName = Path.Combine(this.FullName, str);
            IChangeVirtualFile item = new TempFileSystemFile(fullName, name, this);
            lock (this.Content)
            {
                this.Content.Add(item);
            }
            CleanupManager.AddFile(fullName);
            this.FLastCreatedItem = item;
            return item;
        }

        public IVirtualFolder CreateFolder(string name)
        {
            string str = StringHelper.GuidToCompactString(Guid.NewGuid());
            DirectoryInfo info = new DirectoryInfo(Path.Combine(this.FullName, str));
            info.Create();
            IVirtualFolder item = new TempFileSystemFolder(info, name, this);
            lock (this.Content)
            {
                this.Content.Add(item);
            }
            CleanupManager.AddDirectory(info.FullName);
            this.FLastCreatedItem = item;
            return item;
        }

        public void Dispose()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Dispose), true);
        }

        protected void Dispose(object disposing)
        {
            List<IVirtualItem> content;
            List<IVirtualItem> list2;
            if ((bool) disposing)
            {
                lock ((list2 = this.Content))
                {
                    content = new List<IVirtualItem>(this.Content);
                }
            }
            else
            {
                content = this.Content;
            }
            foreach (IChangeVirtualItem item in content)
            {
                try
                {
                    item.Delete(false);
                }
                catch
                {
                }
                CleanupManager.Remove(item.FullName);
            }
            if ((bool) disposing)
            {
                lock ((list2 = this.Content))
                {
                    foreach (IVirtualItem item2 in content)
                    {
                        this.Content.Remove(item2);
                    }
                }
            }
        }

        public bool Equals(IVirtualItem other)
        {
            return false;
        }

        ~VirtualTempFolder()
        {
            this.Dispose(false);
        }

        public IEnumerable<IVirtualItem> GetContent()
        {
            return this.Content;
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return this.GetContent().OfType<IVirtualFolder>();
        }

        public bool IsChild(IVirtualItem Item)
        {
            return this.Content.Contains(Item);
        }

        public FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Directory;
            }
        }

        public override VirtualPropertySet AvailableProperties
        {
            get
            {
                return DefaultProperty.NameAttrPropertySet;
            }
        }

        public static VirtualTempFolder Default
        {
            get
            {
                if (FDefault == null)
                {
                    FDefault = new VirtualTempFolder();
                }
                return FDefault;
            }
        }

        public string FullName
        {
            get
            {
                return this.FTempFolder;
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

        public IVirtualItem LastCreatedItem
        {
            get
            {
                return this.FLastCreatedItem;
            }
        }

        public string Name
        {
            get
            {
                return "Temp";
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return null;
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                return this;
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

