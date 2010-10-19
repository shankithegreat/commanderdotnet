namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.Windows.Forms;

    public interface IResolveLink
    {
        Keys Hotkey { get; }

        IVirtualItem Target { get; }

        string TargetPath { get; }
    }
}

