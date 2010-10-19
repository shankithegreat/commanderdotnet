namespace Nomad
{
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Windows.Forms;

    public class PreviewContextMenuEventArgs : CancelVirtualItemEventArgs
    {
        public ContextMenuStrip ContextMenu;
        public ContextMenuOptions Options;

        public PreviewContextMenuEventArgs(IVirtualItem item, ContextMenuStrip contextMenu, ContextMenuOptions options) : base(item)
        {
            this.ContextMenu = contextMenu;
            this.Options = options;
        }
    }
}

