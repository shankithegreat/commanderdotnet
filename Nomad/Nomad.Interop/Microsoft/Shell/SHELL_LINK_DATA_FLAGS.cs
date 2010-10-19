namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum SHELL_LINK_DATA_FLAGS : uint
    {
        SLDF_DISABLE_KNOWNFOLDER_RELATIVE_TRACKING = 0x200000,
        SLDF_ENABLE_TARGET_METADATA = 0x80000,
        SLDF_FORCE_NO_LINKINFO = 0x100,
        SLDF_FORCE_NO_LINKTRACK = 0x40000,
        SLDF_FORCE_UNCNAME = 0x10000,
        SLDF_HAS_ARGS = 0x20,
        SLDF_HAS_DARWINID = 0x1000,
        SLDF_HAS_EXP_ICON_SZ = 0x4000,
        SLDF_HAS_EXP_SZ = 0x200,
        SLDF_HAS_ICONLOCATION = 0x40,
        SLDF_HAS_ID_LIST = 1,
        SLDF_HAS_LINK_INFO = 2,
        SLDF_HAS_LOGO3ID = 0x800,
        SLDF_HAS_NAME = 4,
        SLDF_HAS_RELPATH = 8,
        SLDF_HAS_WORKINGDIR = 0x10,
        SLDF_NO_PIDL_ALIAS = 0x8000,
        SLDF_RESERVED = 0x80000000,
        SLDF_RUN_IN_SEPARATE = 0x400,
        SLDF_RUN_WITH_SHIMLAYER = 0x20000,
        SLDF_RUNAS_USER = 0x2000,
        SLDF_UNICODE = 0x80,
        SLDF_VALID = 0x3ff7ff
    }
}

