namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("275C23E3-3747-11D0-9FEA-00AA003F8646"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumCodePage
    {
        void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumCodePage ppEnum);
        void Next(uint celt, out MIMECPINFO rgelt, out uint pceltFetched);
        void Reset();
        void Skip(uint celt);
    }
}

