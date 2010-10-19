namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport, Guid("000214EB-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IExtractIconA
    {
        [PreserveSig]
        int GetIconLocation(GIL_IN uFlags, [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder szIconFile, uint cchMax, out int piIndex, out GIL_OUT pwFlags);
        [PreserveSig]
        int Extract([In, MarshalAs(UnmanagedType.LPStr)] string pszFile, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize);
    }
}

