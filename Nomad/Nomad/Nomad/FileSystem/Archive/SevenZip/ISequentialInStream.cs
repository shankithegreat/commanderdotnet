﻿namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("23170F69-40C1-278A-0000-000300010000")]
    public interface ISequentialInStream
    {
        uint Read(IntPtr data, uint size);
    }
}

