namespace Microsoft.Shell
{
    using Microsoft.COM;
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("6332DEBF-87B5-4670-90C0-5E57B408A49E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICustomDestinationList
    {
        void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        void BeginList(out uint cMaxSlots, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        void AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, [MarshalAs(UnmanagedType.Interface)] IObjectArray poa);
        void AppendKnownCategory(KNOWNDESTCATEGORY category);
        void AddUserTasks([MarshalAs(UnmanagedType.Interface)] IObjectArray poa);
        void CommitList();
        void GetRemovedDestinations([In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
        void AbortList();
    }
}

