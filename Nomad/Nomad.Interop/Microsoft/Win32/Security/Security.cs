namespace Microsoft.Win32.Security
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public static class Security
    {
        private const string AdvApi32Dll = "Advapi32.dll";

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", SetLastError=true)]
        public static extern bool AdjustTokenPrivileges(SafeTokenHandle TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, [In] ref TOKEN_PRIVILEGES NewState, uint BufferLength, out TOKEN_PRIVILEGES PreviousState, out uint ReturnLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", SetLastError=true)]
        public static extern bool AdjustTokenPrivileges(SafeTokenHandle TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, [In] ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);
        public static bool ChangeCurrentProcessPrivilege(string privilege, bool enable)
        {
            return ChangeProcessPrivilege(Process.GetCurrentProcess(), privilege, enable);
        }

        public static bool ChangeProcessPrivilege(Process process, string privilege, bool enable)
        {
            SafeTokenHandle handle;
            if (!OpenProcessToken(process.Handle, TOKEN.TOKEN_ADJUST_PRIVILEGES | TOKEN.TOKEN_QUERY, out handle))
            {
                return false;
            }
            try
            {
                LUID lpLuid = new LUID();
                if (!LookupPrivilegeValue(null, privilege, out lpLuid))
                {
                    return false;
                }
                TOKEN_PRIVILEGES newState = new TOKEN_PRIVILEGES {
                    PrivilegeCount = 1,
                    Privileges = new LUID_AND_ATTRIBUTES[1]
                };
                newState.Privileges[0].Luid = lpLuid;
                newState.Privileges[0].Attributes = enable ? 2 : 0;
                if (!AdjustTokenPrivileges(handle, false, ref newState, (uint) Marshal.SizeOf(newState), IntPtr.Zero, IntPtr.Zero))
                {
                    return false;
                }
            }
            finally
            {
                handle.Close();
            }
            return true;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", SetLastError=true)]
        public static extern bool GetTokenInformation(SafeTokenHandle TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, ref TOKEN_ELEVATION TokenInformation, int TokenInformationLength, out uint ReturnLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", SetLastError=true)]
        public static extern bool GetTokenInformation(SafeTokenHandle TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, ref TOKEN_ELEVATION_TYPE TokenInformation, int TokenInformationLength, out uint ReturnLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", SetLastError=true)]
        public static extern bool GetTokenInformation(SafeTokenHandle TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, int TokenInformationLength, out uint ReturnLength);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool LookupPrivilegeValue([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpName, out LUID lpLuid);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Advapi32.dll", SetLastError=true)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, TOKEN DesiredAccess, out SafeTokenHandle TokenHandle);
    }
}

