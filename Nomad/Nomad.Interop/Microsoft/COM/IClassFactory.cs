namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000001-0000-0000-C000-000000000046")]
    public interface IClassFactory
    {
        void CreateInstance([MarshalAs(UnmanagedType.Interface)] object pUnkOuter, [In, MarshalAs(UnmanagedType.LPStruct)] Guid refiid, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
        void LockServer(bool fLock);
    }
}

