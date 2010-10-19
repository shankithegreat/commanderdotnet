namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0E700BE1-9DB6-11d1-A1CE-00C04FD75D13")]
    public interface IEnumExtraSearch
    {
        [PreserveSig]
        int Next(int celt, ref EXTRASEARCH rgelt, out int pceltFetched);
        void Skip(int celt);
        void Reset();
        void Clone(out IEnumExtraSearch ppenum);
    }
}

