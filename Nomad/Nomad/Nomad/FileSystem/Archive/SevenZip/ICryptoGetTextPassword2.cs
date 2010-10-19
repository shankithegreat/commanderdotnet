namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("23170F69-40C1-278A-0000-000500110000")]
    public interface ICryptoGetTextPassword2
    {
        void CryptoGetTextPassword2([MarshalAs(UnmanagedType.Bool)] out bool passwordIsDefined, [MarshalAs(UnmanagedType.BStr)] out string password);
    }
}

