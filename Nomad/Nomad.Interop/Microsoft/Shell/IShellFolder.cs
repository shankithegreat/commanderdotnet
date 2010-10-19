namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("000214E6-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellFolder
    {
        void ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved, [In, MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName, out uint pchEaten, out IntPtr ppidl, [MarshalAs(UnmanagedType.U4)] ref SFGAO pdwAttributes);
        void EnumObjects(IntPtr hwndOwner, [MarshalAs(UnmanagedType.U4)] SHCONTF grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);
        void BindToObject(IntPtr pidl, IntPtr pbcReserved, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvOut);
        void BindToStorage(IntPtr pidl, IntPtr pbcReserved, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);
        [PreserveSig]
        int CompareIDs(int lParam, IntPtr pidl1, IntPtr pidl2);
        void CreateViewObject(IntPtr hwndOwner, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvOut);
        void GetAttributesOf(int cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr[] apidl, [MarshalAs(UnmanagedType.U4)] ref SFGAO rgfInOut);
        void GetUIObjectOf(IntPtr hwndOwner, int cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] IntPtr[] apidl, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, IntPtr rgfReserved, [MarshalAs(UnmanagedType.Interface)] out object ppvOut);
        void GetDisplayNameOf(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] SHGNO uFlags, out STRRET lpName);
        void SetNameOf(IntPtr hwndOwner, IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string lpszName, [MarshalAs(UnmanagedType.U4)] SHGNO uFlags, out IntPtr ppidlOut);
    }
}

