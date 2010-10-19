namespace Microsoft.Win32.Security
{
    using System;

    public enum TOKEN_INFORMATION_CLASS
    {
        MaxTokenInfoClass = 0x1d,
        TokenAccessInformation = 0x16,
        TokenAuditPolicy = 0x10,
        TokenDefaultDacl = 6,
        TokenElevation = 20,
        TokenElevationType = 0x12,
        TokenGroups = 2,
        TokenGroupsAndPrivileges = 13,
        TokenHasRestrictions = 0x15,
        TokenImpersonationLevel = 9,
        TokenIntegrityLevel = 0x19,
        TokenLinkedToken = 0x13,
        TokenLogonSid = 0x1c,
        TokenMandatoryPolicy = 0x1b,
        TokenOrigin = 0x11,
        TokenOwner = 4,
        TokenPrimaryGroup = 5,
        TokenPrivileges = 3,
        TokenRestrictedSids = 11,
        TokenSandBoxInert = 15,
        TokenSessionId = 12,
        TokenSessionReference = 14,
        TokenSource = 7,
        TokenStatistics = 10,
        TokenType = 8,
        TokenUIAccess = 0x1a,
        TokenUser = 1,
        TokenVirtualizationAllowed = 0x17,
        TokenVirtualizationEnabled = 0x18
    }
}

