namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum GCS : uint
    {
        HELPTEXTA = 1,
        HELPTEXTW = 5,
        VALIDATEA = 2,
        VALIDATEW = 6,
        VERBA = 0,
        VERBW = 4
    }
}

