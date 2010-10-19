namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("23170F69-40C1-278A-0000-000500100000")]
    public interface ICryptoGetTextPassword
    {
        [PreserveSig]
        int CryptoGetTextPassword([MarshalAs(UnmanagedType.BStr)] out string password);
    }
}

