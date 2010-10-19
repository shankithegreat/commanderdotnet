namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface IVirtualItemUI : IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb);
        bool ExecuteVerb(IWin32Window owner, string verb);
        Image GetIcon(Size size, IconStyle style);
        void ShowProperties(IWin32Window owner);

        VirtualHighligher Highlighter { get; }

        string ToolTip { get; }
    }
}

