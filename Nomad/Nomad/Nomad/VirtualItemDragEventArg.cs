namespace Nomad
{
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Windows.Forms;

    public class VirtualItemDragEventArg : DragEventArgs
    {
        public readonly IVirtualItem Item;

        public VirtualItemDragEventArg(IVirtualItem item, DragEventArgs e) : base(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect)
        {
            this.Item = item;
        }
    }
}

