namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("AE5F1430-388B-11D2-8380-00C04F8F5DA1")]
    public interface IEnumScript
    {
        void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumScript ppEnum);
        void Next(uint celt, out SCRIPTINFO rgelt, out uint pceltFetched);
        void Reset();
        void Skip(uint celt);
    }
}

