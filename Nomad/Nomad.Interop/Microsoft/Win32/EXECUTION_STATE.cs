namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum EXECUTION_STATE : uint
    {
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 2,
        ES_SYSTEM_REQUIRED = 1
    }
}

