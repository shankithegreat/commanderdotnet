namespace Nomad.FileSystem.Ftp
{
    using System;

    [Flags]
    internal enum FtpItemCapability
    {
        HasParent = 1,
        IsPathEncoded = 2
    }
}

