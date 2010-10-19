namespace Microsoft
{
    using Microsoft.Win32;
    using Microsoft.Win32.Security;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public static class OS
    {
        private static readonly OSVersion Version;

        static OS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32Windows:
                    if (Environment.OSVersion.Version.Major == 4)
                    {
                        if (Environment.OSVersion.Version.Minor < 90)
                        {
                            if (Environment.OSVersion.Version.Minor >= 10)
                            {
                                Version = OSVersion.Win98;
                            }
                            break;
                        }
                        Version = OSVersion.WinME;
                    }
                    break;

                case PlatformID.Win32NT:
                    if (Environment.OSVersion.Version.Major < 6)
                    {
                        if (Environment.OSVersion.Version.Major >= 5)
                        {
                            Version = (Environment.OSVersion.Version.Minor > 0) ? OSVersion.WinXP : OSVersion.Win2k;
                        }
                        else if (Environment.OSVersion.Version.Major >= 4)
                        {
                            Version = OSVersion.WinNT;
                        }
                        break;
                    }
                    Version = (Environment.OSVersion.Version.Minor > 0) ? OSVersion.Win7 : OSVersion.WinVista;
                    break;
            }
        }

        public static Microsoft.ElevationType ElevationType
        {
            get
            {
                SafeTokenHandle handle;
                if (IsWinVista && Microsoft.Win32.Security.Security.OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN.TOKEN_QUERY, out handle))
                {
                    try
                    {
                        uint num;
                        TOKEN_ELEVATION_TYPE tokenInformation = (TOKEN_ELEVATION_TYPE) 0;
                        if (Microsoft.Win32.Security.Security.GetTokenInformation(handle, TOKEN_INFORMATION_CLASS.TokenElevationType, ref tokenInformation, 4, out num))
                        {
                            return (Microsoft.ElevationType) tokenInformation;
                        }
                    }
                    finally
                    {
                        handle.Close();
                    }
                }
                return Microsoft.ElevationType.None;
            }
        }

        public static bool IsDwmCompositionEnabled
        {
            get
            {
                bool flag;
                return ((IsWinVista && HRESULT.SUCCEEDED(Dwm.DwmIsCompositionEnabled(out flag))) && flag);
            }
        }

        public static bool IsElevated
        {
            get
            {
                SafeTokenHandle handle;
                if (IsWinVista && Microsoft.Win32.Security.Security.OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN.TOKEN_QUERY, out handle))
                {
                    try
                    {
                        uint num;
                        TOKEN_ELEVATION tokenInformation = new TOKEN_ELEVATION();
                        if (Microsoft.Win32.Security.Security.GetTokenInformation(handle, TOKEN_INFORMATION_CLASS.TokenElevation, ref tokenInformation, Marshal.SizeOf(tokenInformation), out num))
                        {
                            return tokenInformation.TokenIsElevated;
                        }
                    }
                    finally
                    {
                        handle.Close();
                    }
                }
                return false;
            }
        }

        public static bool IsWin2k
        {
            get
            {
                return (Version >= OSVersion.Win2k);
            }
        }

        public static bool IsWin7
        {
            get
            {
                return (Version >= OSVersion.Win7);
            }
        }

        public static bool IsWinME
        {
            get
            {
                return (Version == OSVersion.WinME);
            }
        }

        public static bool IsWinNT
        {
            get
            {
                return (Version >= OSVersion.WinNT);
            }
        }

        public static bool IsWinVista
        {
            get
            {
                return (Version >= OSVersion.WinVista);
            }
        }

        public static bool IsWinXP
        {
            get
            {
                return (Version >= OSVersion.WinXP);
            }
        }

        public static Size MouseHoverSize
        {
            get
            {
                Size mouseHoverSize = SystemInformation.MouseHoverSize;
                return (mouseHoverSize.IsEmpty ? new Size(4, 4) : mouseHoverSize);
            }
        }

        public static int MouseHoverTime
        {
            get
            {
                int mouseHoverTime = SystemInformation.MouseHoverTime;
                return ((mouseHoverTime > 0) ? mouseHoverTime : 400);
            }
        }

        public static string TempDirectory
        {
            get
            {
                StringBuilder lpBuffer = new StringBuilder(260);
                if (Windows.GetTempPath(lpBuffer.Capacity, lpBuffer) == 0)
                {
                    throw new Win32Exception();
                }
                return lpBuffer.ToString();
            }
        }

        public static string WindowDirectory
        {
            get
            {
                StringBuilder lpBuffer = new StringBuilder(260);
                if (Windows.GetWindowsDirectory(lpBuffer, lpBuffer.Capacity) == 0)
                {
                    throw new Win32Exception();
                }
                return lpBuffer.ToString();
            }
        }

        private enum OSVersion
        {
            Unknown,
            Win98,
            WinME,
            WinNT,
            Win2k,
            WinXP,
            WinVista,
            Win7
        }
    }
}

