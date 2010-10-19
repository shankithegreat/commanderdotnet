namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("23170F69-40C1-278A-0000-000300070000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOutStreamFlush
    {
        void Flush();
    }
}

