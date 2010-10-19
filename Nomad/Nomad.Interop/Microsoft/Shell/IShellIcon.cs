namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E5-0000-0000-C000-000000000046")]
    public interface IShellIcon
    {
        int GetIconOf(IntPtr pidl, GIL_IN flags, out int IconIndex);
    }
}

