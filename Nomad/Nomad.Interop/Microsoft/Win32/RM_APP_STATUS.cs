namespace Microsoft.Win32
{
    using System;

    public enum RM_APP_STATUS : uint
    {
        RmStatusErrorOnRestart = 0x20,
        RmStatusErrorOnStop = 0x10,
        RmStatusRestarted = 8,
        RmStatusRestartMasked = 0x80,
        RmStatusRunning = 1,
        RmStatusShutdownMasked = 0x40,
        RmStatusStopped = 2,
        RmStatusStoppedOther = 4,
        RmStatusUnknown = 0
    }
}

