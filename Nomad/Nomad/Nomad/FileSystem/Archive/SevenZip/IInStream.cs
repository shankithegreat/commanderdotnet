namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("23170F69-40C1-278A-0000-000300030000")]
    public interface IInStream
    {
        uint Read(IntPtr data, uint size);
        void Seek(long offset, uint seekOrigin, IntPtr newPosition);
    }
}

