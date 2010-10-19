namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class Rstrtmgr
    {
        public const int CCH_RM_MAX_APP_NAME = 0xff;
        public const int CCH_RM_MAX_SVC_NAME = 0x3f;
        public const string RstrtmgrDll = "rstrtmgr.dll";

        [DllImport("rstrtmgr.dll")]
        public static extern int RmGetList(SafeRestartSessionHandle dwSessionHandle, out int pnProcInfoNeeded, ref int pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref RM_REBOOT_REASON lpdwRebootReasons);
        [DllImport("rstrtmgr.dll", CharSet=CharSet.Unicode)]
        public static extern int RmRegisterResources(SafeRestartSessionHandle pSessionHandle, int nFiles, string[] rgsFilenames, int nApplications, RM_UNIQUE_PROCESS[] rgApplications, int nServices, string[] rgsServiceNames);
        [DllImport("rstrtmgr.dll", CharSet=CharSet.Unicode)]
        public static extern int RmStartSession(out SafeRestartSessionHandle pSessionHandle, int dwSessionFlags, string strSessionKey);
    }
}

