namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("5632B1A4-E38A-400A-928A-D4CD63230295")]
    public interface IObjectCollection
    {
        void GetCount(out uint cObjects);
        void GetAt(uint iIndex, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        void AddObject([MarshalAs(UnmanagedType.Interface)] object pvObject);
        void AddFromArray([MarshalAs(UnmanagedType.Interface)] IObjectArray poaSource);
        void RemoveObject(uint uiIndex);
        void Clear();
    }
}

