namespace Microsoft.COM
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("4657278B-411B-11d2-839A-00C04FD918D0")]
    public interface IDropTargetHelper
    {
        [PreserveSig]
        int DragEnter(IntPtr hwndTarget, [MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject, [In] ref Point pt, uint pdwEffect);
        [PreserveSig]
        int DragLeave();
        [PreserveSig]
        int DragOver([In] ref Point ppt, uint dwEffect);
        [PreserveSig]
        int Drop([MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject, [In] ref Point ppt, uint dwEffect);
        void Show(bool fShow);
    }
}

