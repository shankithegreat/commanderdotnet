namespace Microsoft.Win32
{
    using System;

    public enum RM_REBOOT_REASON : uint
    {
        RmRebootReasonCriticalProcess = 4,
        RmRebootReasonCriticalService = 8,
        RmRebootReasonDetectedSelf = 0x10,
        RmRebootReasonNone = 0,
        RmRebootReasonPermissionDenied = 1,
        RmRebootReasonSessionMismatch = 2
    }
}

