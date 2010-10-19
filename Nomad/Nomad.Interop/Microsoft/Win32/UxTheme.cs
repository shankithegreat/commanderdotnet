namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class UxTheme
    {
        private const string UxThemeDll = "uxtheme.dll";

        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pszSubAppName, [MarshalAs(UnmanagedType.LPWStr)] string pszSubIdList);
        [DllImport("uxtheme.dll")]
        public static extern int SetWindowThemeAttribute(IntPtr hWnd, WindowThemeAttributeType wtype, ref WTA_OPTIONS attributes, uint size);
    }
}

