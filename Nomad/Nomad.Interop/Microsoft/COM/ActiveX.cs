namespace Microsoft.COM
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public sealed class ActiveX
    {
        public static Guid IID_IClassFactory = new Guid("00000001-0000-0000-C000-000000000046");
        private const string Ole32Dll = "ole32.dll";

        [DllImport("ole32.dll")]
        public static extern int CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, CLSCTX dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv);
        [DllImport("ole32.dll")]
        public static extern int CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, CLSCTX dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [DllImport("ole32.dll")]
        public static extern int CoGetInterfaceAndReleaseStream(System.Runtime.InteropServices.ComTypes.IStream pStm, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [DllImport("ole32.dll")]
        public static extern int CoMarshalInterThreadInterfaceInStream([In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] object pUnk, [MarshalAs(UnmanagedType.Interface)] out System.Runtime.InteropServices.ComTypes.IStream ppStm);
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);
        [DllImport("ole32.dll")]
        public static extern int CreateFileMoniker([MarshalAs(UnmanagedType.LPWStr)] string lpszPathName, out IMoniker ppmk);
        [DllImport("ole32.dll")]
        public static extern int OleDraw([MarshalAs(UnmanagedType.IUnknown)] object pUnk, DVASPECT2 dwAspect, IntPtr hdcDraw, [In] ref Rectangle lprcBounds);
        [DllImport("ole32.dll")]
        public static extern int PropVariantClear(IntPtr pvar);
        [DllImport("ole32.dll")]
        public static extern void ReleaseStgMedium([In] ref STGMEDIUM pmedium);
    }
}

