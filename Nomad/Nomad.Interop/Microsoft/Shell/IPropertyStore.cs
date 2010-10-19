namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore
    {
        void GetCount(out uint cProps);
        void GetAt(uint iProp, out PropertyKey pkey);
        void GetValue([In] ref PropertyKey pkey, out PropVariant pv);
        void SetValue([In] ref PropertyKey pkey, [In] ref PropVariant pv);
        void Commit();
    }
}

