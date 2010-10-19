namespace Microsoft.Win32
{
    using System;

    public enum RM_APP_TYPE : uint
    {
        RmConsole = 5,
        RmCritical = 0x3e8,
        RmExplorer = 4,
        RmMainWindow = 1,
        RmOtherWindow = 2,
        RmService = 3,
        RmUnknownApp = 0
    }
}

