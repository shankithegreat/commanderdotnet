namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public interface IAsyncVirtualFolder : IVirtualCachedFolder, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable
    {
        event AsyncCompletedEventHandler Completed;

        event ProgressChangedEventHandler ProgressChanged;

        IAsyncResult BeginGetContent();
        void CancelAsync();
        IEnumerable<IVirtualItem> EndGetContent(IAsyncResult result);
    }
}

