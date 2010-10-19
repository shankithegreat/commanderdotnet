namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214FA-0000-0000-C000-000000000046")]
    public interface IExtractIconW
    {
        [PreserveSig]
        int GetIconLocation(GIL_IN uFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szIconFile, uint cchMax, out int piIndex, out GIL_OUT pwFlags);
        [PreserveSig]
        int Extract([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize);
    }
}

