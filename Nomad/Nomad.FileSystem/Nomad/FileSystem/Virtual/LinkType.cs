namespace Nomad.FileSystem.Virtual
{
    using System;

    [Flags]
    public enum LinkType
    {
        Default = 1,
        HardLink = 4,
        JuntionPoint = 8,
        MountPoint = 0x10,
        None = 0,
        ShellFolderLink = 2,
        SymbolicLink = 0x20
    }
}

