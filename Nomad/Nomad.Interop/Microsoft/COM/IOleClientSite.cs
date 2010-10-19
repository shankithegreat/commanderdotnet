namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000118-0000-0000-C000-000000000046")]
    public interface IOleClientSite
    {
        [PreserveSig]
        int SaveObject();
        [PreserveSig]
        int GetMoniker(OLEGETMONIKER dwAssign, OLEWHICHMK dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);
        [PreserveSig]
        void GetContainer(out IOleContainer container);
        void ShowObject();
        void OnShowWindow([MarshalAs(UnmanagedType.Bool)] bool fShow);
        void RequestNewObjectLayout();
    }
}

