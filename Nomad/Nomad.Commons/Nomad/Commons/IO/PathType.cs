namespace Nomad.Commons.IO
{
    using System;

    [Flags]
    public enum PathType
    {
        File = 4,
        Folder = 2,
        Invalid = -1,
        NetworkServer = 8,
        NetworkShare = 0x10,
        Relative = 0x40,
        Unknown = 0,
        Uri = 0x20,
        Volume = 1
    }
}

