namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, Guid("0000011B-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleContainer
    {
        void ParseDisplayName([MarshalAs(UnmanagedType.Interface)] object pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);
        void EnumObjects(OLECONTF grfFlags, out IEnumUnknown ppenum);
        void LockContainer([MarshalAs(UnmanagedType.Bool)] bool fLock);
    }
}

