namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public interface IVirtualCachedFolder : IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable
    {
        event EventHandler CachedContentChanged;

        event EventHandler<VirtualItemChangedEventArgs> OnChanged;

        void ClearContentCache();
        IEnumerable<IVirtualItem> GetCachedContent();
        void RaiseChanged(WatcherChangeTypes changeType, IVirtualItem item);

        Nomad.FileSystem.Virtual.CacheState CacheState { get; }
    }
}

