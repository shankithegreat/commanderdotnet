namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("3DC39D1D-C030-11D0-B81B-00C04FC9B31F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumRfc1766
    {
        void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumRfc1766 ppEnum);
        void Next(uint celt, out RFC1766INFO rgelt, out uint pceltFetched);
        void Reset();
        void Skip(uint celt);
    }
}

