namespace Microsoft.COM
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("DE5BF786-477A-11d2-839D-00C04FD918D0")]
    public interface IDragSourceHelper
    {
        void InitializeFromBitmap([In] ref SHDRAGIMAGE pshdi, [MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject);
        void InitializeFromWindow(IntPtr hwnd, [In] ref Point ppt, [MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject);
    }
}

