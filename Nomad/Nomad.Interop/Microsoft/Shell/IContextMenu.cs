namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("000214e4-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IContextMenu
    {
        void QueryContextMenu(IntPtr hmenu, uint iMenu, uint idCmdFirst, uint idCmdLast, CMF uFlags);
        [PreserveSig]
        int InvokeCommand([In] ref CMINVOKECOMMANDINFOEX info);
        void GetCommandString(uint idCmd, GCS uFlags, uint pwReserved, IntPtr pszName, int cchMax);
    }
}

