namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("000214F2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumIDList
    {
        [PreserveSig]
        int Next(int celt, ref IntPtr rgelt, out int pceltFetched);
        void Skip(int celt);
        void Reset();
        void Clone(out IEnumIDList ppenum);
    }
}

