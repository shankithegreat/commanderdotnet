namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0C6C4200-C589-11D0-999A-00C04FD655E1")]
    public interface IShellIconOverlayIdentifier
    {
        [PreserveSig]
        int IsMemberOf([MarshalAs(UnmanagedType.LPWStr)] string pwszPath, SFGAO dwAttrib);
        void GetOverlayInfo([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszIconFile, int cchMax, out int pIndex, out ISIOI pdwFlags);
        int GetPriority();
    }
}

