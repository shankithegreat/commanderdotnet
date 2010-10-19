namespace Microsoft.IE
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3050F3F0-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface ICustomDoc
    {
        void SetUIHandler([MarshalAs(UnmanagedType.Interface)] IDocHostUIHandler pUIHandler);
    }
}

