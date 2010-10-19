namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum KF_FLAG : uint
    {
        KF_FLAG_CREATE = 0x8000,
        KF_FLAG_DEFAULT_PATH = 0x400,
        KF_FLAG_DONT_UNEXPAND = 0x2000,
        KF_FLAG_DONT_VERIFY = 0x4000,
        KF_FLAG_INIT = 0x800,
        KF_FLAG_NO_ALIAS = 0x1000,
        KF_FLAG_NOT_PARENT_RELATIVE = 0x200,
        KF_FLAG_SIMPLE_IDLIST = 0x100
    }
}

