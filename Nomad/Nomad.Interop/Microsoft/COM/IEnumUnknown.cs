namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000100-0000-0000-C000-000000000046")]
    public interface IEnumUnknown
    {
        [PreserveSig]
        int Next(int celt, [MarshalAs(UnmanagedType.IUnknown)] out object rgelt, out int pceltFetched);
        [PreserveSig]
        int Skip(int celt);
        void Reset();
        void Clone(out IEnumUnknown ppenum);
    }
}

