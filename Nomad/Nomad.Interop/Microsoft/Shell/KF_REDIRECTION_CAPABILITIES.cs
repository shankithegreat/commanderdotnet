﻿namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum KF_REDIRECTION_CAPABILITIES
    {
        KF_REDIRECTION_CAPABILITIES_ALLOW_ALL = 0xff,
        KF_REDIRECTION_CAPABILITIES_DENY_ALL = 0xfff00,
        KF_REDIRECTION_CAPABILITIES_DENY_PERMISSIONS = 0x400,
        KF_REDIRECTION_CAPABILITIES_DENY_POLICY = 0x200,
        KF_REDIRECTION_CAPABILITIES_DENY_POLICY_REDIRECTED = 0x100,
        KF_REDIRECTION_CAPABILITIES_REDIRECTABLE = 1
    }
}

