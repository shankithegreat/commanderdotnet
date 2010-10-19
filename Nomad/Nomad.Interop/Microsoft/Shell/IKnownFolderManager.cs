namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("8BE2D872-86AA-4d47-B776-32CCA40C7018"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKnownFolderManager
    {
        void FolderIdFromCsidl(CSIDL nCsidl, out Guid pfid);
        void FolderIdToCsidl([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, out CSIDL pnCsidl);
        void GetFolderIds(out IntPtr ppKFId, ref uint pCount);
        void GetFolder([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, out IKnownFolder ppkf);
        void GetFolderByName([MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, out IKnownFolder ppkf);
        void RegisterFolder([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, [In] ref KNOWNFOLDER_DEFINITION pKFD);
        void UnregisterFolder([MarshalAs(UnmanagedType.LPStruct)] Guid rfid);
        void FindFolderFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, FFFP_MODE mode, out IKnownFolder ppkf);
        void FindFolderFromIDList(IntPtr pidl, out IKnownFolder ppkf);
        void Redirect([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, IntPtr hwnd, KF_REDIRECT_FLAGS Flags, [MarshalAs(UnmanagedType.LPWStr)] string pszTargetPath, uint cFolders, Guid[] pExclusion, [MarshalAs(UnmanagedType.LPWStr)] out string ppszError);
    }
}

