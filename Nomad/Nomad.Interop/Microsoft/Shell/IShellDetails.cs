namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("000214EC-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellDetails
    {
        void GetDetailsOf(IntPtr pidl, uint iColumn, out SHELLDETAILS pDetails);
        void ColumnClick(uint iColumn);
    }
}

