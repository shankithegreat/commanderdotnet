namespace Microsoft.Win32
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate bool EnumResNameDelegate(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);
}

