namespace Nomad.Controls.Filter
{
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IFilterControl
    {
        event EventHandler FilterChanged;

        IVirtualItemFilter Filter { get; }

        bool IsEmpty { get; }

        ToolStrip TopToolStrip { get; }
    }
}

