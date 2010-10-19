namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public interface IVirtualFolderUI : IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable
    {
        ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, IEnumerable<IVirtualItem> items, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb);
        void ShowProperties(IWin32Window owner, IEnumerable<IVirtualItem> items);
    }
}

