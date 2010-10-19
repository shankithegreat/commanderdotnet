namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3AA7AF7E-9B36-420c-A8E3-F77D4674A488")]
    public interface IKnownFolder
    {
        Guid GetId();
        KF_CATEGORY GetCategory();
        void GetShellItem(KF_FLAG dwFlags, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);
        void GetPath(KF_FLAG dwFlags, out IntPtr ppszPath);
        void SetPath(KF_FLAG dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszPath);
        void GetIDList(KF_FLAG dwFlags, out IntPtr ppidl);
        void GetFolderType(out Guid pftid);
        KF_REDIRECTION_CAPABILITIES GetRedirectionCapabilities();
        void GetFolderDefinition(out KNOWNFOLDER_DEFINITION pKFD);
    }
}

