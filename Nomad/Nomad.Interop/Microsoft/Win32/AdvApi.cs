namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public class AdvApi
    {
        public const string AdvApiDll = "AdvApi32.dll";

        [DllImport("AdvApi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int OpenEncryptedFileRaw([MarshalAs(UnmanagedType.LPTStr)] string lpFileName, OEFR ulFlags, out SafeEncryptedFileHandle pvContext);
        [DllImport("AdvApi32.dll", SetLastError=true)]
        public static extern int ReadEncryptedFileRaw([MarshalAs(UnmanagedType.FunctionPtr)] ExportCallback pfExportCallback, IntPtr pvCallbackContext, SafeEncryptedFileHandle pvContext);
        [DllImport("AdvApi32.dll", SetLastError=true)]
        public static extern int WriteEncryptedFileRaw([MarshalAs(UnmanagedType.FunctionPtr)] ImportCallback pfImportCallback, IntPtr pvCallbackContext, SafeEncryptedFileHandle pvContext);
    }
}

