namespace Nomad
{
    using Microsoft.COM;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;

    internal class VirtualClipboardFolder : VirtualClipboardItem, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable
    {
        public readonly IList<IVirtualItem> Content;

        public VirtualClipboardFolder(FILEDESCRIPTORA descriptor) : base(descriptor)
        {
            this.Content = new List<IVirtualItem>();
        }

        public VirtualClipboardFolder(FILEDESCRIPTORW descriptor) : base(descriptor)
        {
            this.Content = new List<IVirtualItem>();
        }

        public void Dispose()
        {
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
            return ((Item is VirtualClipboardItem) && Item.FullName.StartsWith(PathHelper.IncludeTrailingDirectorySeparator(base.FullName), StringComparison.OrdinalIgnoreCase));
        }
    }
}

