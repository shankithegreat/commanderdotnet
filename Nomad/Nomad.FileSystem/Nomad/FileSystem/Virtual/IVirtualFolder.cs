namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections.Generic;

    public interface IVirtualFolder : IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable
    {
        IEnumerable<IVirtualItem> GetContent();
        IEnumerable<IVirtualFolder> GetFolders();
        bool IsChild(IVirtualItem Item);
    }
}

