namespace Microsoft.Shell
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct KNOWNFOLDER_DEFINITION
    {
        public KF_CATEGORY category;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszDescription;
        public Guid fidParent;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszRelativePath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszParsingName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszTooltip;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszLocalizedName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszIcon;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszSecurity;
        public FileAttributes dwAttributes;
        public KF_DEFINITION_FLAGS kfdFlags;
        public Guid ftidType;
    }
}

