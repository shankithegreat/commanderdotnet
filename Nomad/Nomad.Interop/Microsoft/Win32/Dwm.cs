namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class Dwm
    {
        public const string DwmApi = "dwmapi.dll";

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled([MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);
    }
}

