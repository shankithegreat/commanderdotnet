namespace Microsoft.Win32.Security
{
    using System;

    public enum TOKEN : uint
    {
        TOKEN_ADJUST_DEFAULT = 0x80,
        TOKEN_ADJUST_GROUPS = 0x40,
        TOKEN_ADJUST_PRIVILEGES = 0x20,
        TOKEN_ADJUST_SESSIONID = 0x100,
        TOKEN_ASSIGN_PRIMARY = 1,
        TOKEN_DUPLICATE = 2,
        TOKEN_IMPERSONATE = 4,
        TOKEN_QUERY = 8,
        TOKEN_QUERY_SOURCE = 0x10
    }
}

