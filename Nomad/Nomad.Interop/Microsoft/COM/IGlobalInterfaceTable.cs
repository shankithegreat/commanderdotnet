namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("00000146-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IGlobalInterfaceTable
    {
        int RegisterInterfaceInGlobal([MarshalAs(UnmanagedType.IUnknown)] object pUnk, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);
        void RevokeInterfaceFromGlobal(int dwCookie);
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetInterfaceFromGlobal(int dwCookie, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);
    }
}

